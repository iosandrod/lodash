import {
  Resource,
  TaskTimeRange,
  SchProductRouteRes,
  SchParam,
  ResTimeRange,

  Base,
} from './type'

export class ResSourceDayCap extends Base{
  private cResourceNo: string = '' // 对应资源ＩＤ号,要设置
  private cIsInfinityAbility: string = '0' // 0 产能有限，1 产能无限
  resource: Resource | null = null // 时段对应的资源 有值
  private dBegTime: Date // 时间段开始时间
  private dEndTime: Date // 时间段结束时间
  private holdingTime: number = 0 // 时段总长, dDEndTimeTime - dBegTime,单位为秒
  private allottedTime: number = 0 // 已分配时间,包括维修、故障时间
  private maintainTime: number = 0 // 维修、故障时间
  private availableTime: number = 0 // 时段内可用时间，计算出来
  WorkTimeAct: number = 0 // 学习曲线折扣,有效加工时间
  private notWorkTime: number = 0 // 时段内空闲时间，计算出来,用于检查
  iPeriodID: number = 1 // 时段ID，排程完成写回数据库时，重新生成，唯一
  dPeriodDay: Date // 时段所属日期
  FShiftType: string // 时段所属班次 白班、夜班、中班等
  iTaskCount: number = 0 // 当日已排任务总数
  iTaskMaxCount: number = 0 // 当日可排任务总数
  ResTimeRangeList: ResTimeRange[] = new Array<ResTimeRange>(10)
  taskTimeRangeList: TaskTimeRange[] = new Array<TaskTimeRange>(10)
  WorkTimeRangeList: TaskTimeRange[] = new Array<TaskTimeRange>(10)
  ResTimeRangePre: ResTimeRange | null // 前资源时间段
  ResTimeRangePost: ResTimeRange | null // 后资源时间段
  iSchSdID: number = -1 // 记录更新、新增时间段的任务ID
  iProcessProductID: number = -1
  iResProcessID: number = -1
  iSchSNMax: number = -1

  constructor(as_ResourceNo?: string)
  constructor(as_ResourceNo?: string, adBegTime?: Date, adEndTime?: Date)
  constructor(as_ResourceNo?: string, adBegTime?: Date, adEndTime?: Date) {
    super()
    if (as_ResourceNo) {
      this.cResourceNo = as_ResourceNo
    }
    if (adBegTime && adEndTime) {
      this.dBegTime = adBegTime
      this.dEndTime = adEndTime
      this.AllottedTime = 0
    }
  }

  get CResourceNo(): string {
    return this.cResourceNo
  }
  set CResourceNo(value: string) {
    this.cResourceNo = value
  }

  get CIsInfinityAbility(): string {
    return this.cIsInfinityAbility
  }
  set CIsInfinityAbility(value: string) {
    this.cIsInfinityAbility = value
  }

  get DBegTime(): Date {
    return this.dBegTime
  }
  set DBegTime(value: Date) {
    if (value < new Date(2000, 0, 1)) {
      throw new Error(
        `资源编号${
          this.cResourceNo
        }时间段开始日期${value.toString()}不能小于2000-01-01日,开始时间${this.dBegTime.toLocaleDateString()},结束时间${this.dEndTime.toLocaleDateString()}`,
      )
    }
    this.dBegTime = value
    this.holdingTime = this.HoldingTime
  }

  get DEndTime(): Date {
    return this.dEndTime
  }
  set DEndTime(value: Date) {
    if (value < new Date(2000, 0, 1)) {
      throw new Error(
        `资源编号${
          this.cResourceNo
        }时间段结束日期${value.toString()}不能小于2000-01-01日,开始时间${this.dBegTime.toLocaleDateString()},结束时间${this.dEndTime.toLocaleDateString()}`,
      )
    }
    if (value <= this.dBegTime) {
      throw new Error(
        `资源编号${
          this.cResourceNo
        }时间段结束日期${value.toString()}不能小于时段开始时间,开始时间${this.dBegTime.toLocaleDateString()},结束时间${this.dEndTime.toLocaleDateString()}`,
      )
    }
    this.dEndTime = value
    this.holdingTime = this.HoldingTime
  }

  get HoldingTime(): number {
    if (this.dEndTime && this.dBegTime && this.dEndTime > this.dBegTime) {
      const its = this.dEndTime.getTime() - this.dBegTime.getTime()
      if (its > 0) {
        return Math.floor(its / 1000) // 如果没有设置，则时长缺省为结束时间与开始时间之差
      } else {
        throw new Error('the timerange no set')
      }
    } else {
      return 0
    }
  }

  get AllottedTime(): number {
    if (this.constructor.name === 'TaskTimeRange') {
      return this.allottedTime
    } else {
      let allottedTimeTemp = 0
      if (
        this.CIsInfinityAbility !== '1' &&
        this.WorkTimeRangeList.length > 0
      ) {
        // 有限产能，统计时段内已分配任务时间。
        for (const taskTimeRange of this.WorkTimeRangeList) {
          allottedTimeTemp += taskTimeRange.allottedTime
        }
        if (this.allottedTime < allottedTimeTemp) {
          this.allottedTime = this.allottedTime
        }
      }
      return allottedTimeTemp
    }
  }
  set AllottedTime(value: number) {
    if (value >= 0) {
      this.allottedTime = value
    } else {
      throw new Error('时间段已分配时间必须大于0')
    }
  }

  get AvailableTime(): number {
    if (this.CIsInfinityAbility === '1') {
      // 无限产能。
      return this.holdingTime
    } else {
      // 有限产能，统计时段内已分配任务时间
      if (this.constructor.name === 'TaskTimeRange') {
        const its = this.dEndTime.getTime() - this.dBegTime.getTime()
        if (its > 0) {
          return Math.floor(its / 1000) // 如果没有设置，则时长缺省为结束时间与开始时间之差
        }
        return this.holdingTime
      } else if (this.holdingTime - this.AllottedTime >= 0) {
        return this.holdingTime - this.AllottedTime
      } else if (this.holdingTime - this.AllottedTime <= 30) {
        // 计算有误差，小于1秒
        return 0
      } else {
        throw new Error(
          '出错位置：排程时取时段内可用时间出错TimeSchTask.AvailableTime！',
        )
        return 0
      }
    }
  }

  get NotWorkTime(): number {
    if (this.constructor.name === 'TaskTimeRange') {
      return this.AvailableTime // 可用时间
    } else {
      // "Algorithm.ResTimeRange" 资源时段的占用时间，取所有已分配任务的已占用时间
      return this.AvailableTime // 可用时间
    }
  }
  set NotWorkTime(value: number) {
    // 只有TaskTimeRange才可以设置AllottedTime
  }

  get MaintainTime(): number {
    this.maintainTime = 0
    for (const taskTimeRange of this.taskTimeRangeList.filter(
      (p1) => p1.cTaskType === 2,
    )) {
      this.maintainTime += taskTimeRange.HoldingTime
    }
    return this.maintainTime
  }

  private attribute: any
  get Attribute(): any {
    return this.attribute
  }
  set Attribute(value: any) {
    this.attribute = value
  }

  get TaskTimeRangeList(): TaskTimeRange[] {
    return this.taskTimeRangeList
  }
  set TaskTimeRangeList(value: TaskTimeRange[]) {
    if (this.cResourceNo === 'gys20097') {
      let i = 0
    }
    this.taskTimeRangeList = value
  }

  state: number
  loadRate: number

  compareTo(obj: any): number {
    if (obj instanceof ResTimeRange) {
      const newTimeRange = obj as ResTimeRange
      if (
        this.dBegTime === newTimeRange.dBegTime &&
        this.dEndTime === newTimeRange.dEndTime
      ) {
        return 1
      } else {
        return -1
      }
    } else {
      throw new Error('对象非TimeRange类型')
    }
  }


  clone(): any {
    return Object.assign(Object.create(Object.getPrototypeOf(this)), this)
  }

  
}
