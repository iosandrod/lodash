import {
  Resource,
  SchData,
  TaskTimeRange,
  IComparable,
  ICloneable,
  ResSourceDayCap,
  SchProductRouteRes,
  SchParam,
  Comparer,
  DateTime,
} from './type'

export class ResTimeRange implements IComparable<ResTimeRange>, ICloneable {
  schData: SchData | null = null // 所有排程数据
  cResourceNo: string = '' // 对应资源ID号,要设置
  cIsInfinityAbility: string = '0' // 0 产能有限，1 产能无限
  resource: Resource | null = null // 时段对应的资源 有值
  dBegTime: Date = new Date() // 时间段开始时间
  dEndTime: Date = new Date() // 时间段结束时间
  holdingTime: number = 0 // 时段总长, dEndTime - dBegTime,单位为秒
  allottedTime: number = 0 // 已分配时间,包括维修、故障时间
  maintainTime: number = 0 // 维修、故障时间
  availableTime: number = 0 // 时段内可用时间，计算出来
  WorkTimeAct: number = 0 // 学习曲线折扣,有效加工时间
  notWorkTime: number = 0 // 时段内空闲时间，计算出来,用于检查
  iPeriodID: number = 1 // 时段ID，排程完成写回数据库时，重新生成，唯一
  dPeriodDay: Date = new Date() // 时段所属日期
  FShiftType: string = '' // 时段所属班次 白班、夜班、中班等
  taskTimeRangeList: TaskTimeRange[] = new Array(10)
  WorkTimeRangeList: TaskTimeRange[] = new Array(10)
  ResTimeRangePre: ResTimeRange | null = null // 前资源时间段
  ResTimeRangePost: ResTimeRange | null = null // 后资源时间段
  iSchSdID: number = -1 // 记录更新、新增时间段的任务ID
  iProcessProductID: number = -1
  iResProcessID: number = -1
  iSchSNMax: number = -1
  resSourceDayCap: ResSourceDayCap | null = null // 资源日产能限制,2023-10-05 新增加，每个时间段排程完成时，设置当天日产能已排工时。

  constructor(as_ResourceNo?: string, adBegTime?: Date, adEndTime?: Date) {
    if (as_ResourceNo) {
      this.cResourceNo = as_ResourceNo
      if (adBegTime && adEndTime) {
        this.dBegTime = adBegTime
        this.dEndTime = adEndTime
        this.allottedTime = 0
      }
    }
  }

  TimeSchTaskFreezeInit(
    as_SchProductRouteRes: SchProductRouteRes,
    adCanBegDate: Date,
    adCanEndDate: Date,
  ): number {
    let bSchdule = true // 正式排产
    let NoTimeRangeList = this.GetAvailableTimeRangeList(
      adCanBegDate,
      false,
      bSchdule,
      as_SchProductRouteRes.iResRationHour + as_SchProductRouteRes.iResPreTime,
      false,
    )
    try {
      for (let i = 0; i < NoTimeRangeList.length; i++) {
        let NoTimeRange = NoTimeRangeList[i]
        if (NoTimeRange.DBegTime >= adCanEndDate) break // 时间段大于待排结束时间段，退出
        if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime) {
          continue
        }
        let taskTimeRange1: any = new TaskTimeRange()
        taskTimeRange1.cTaskType = 1 // 工作
        taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo
        taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID
        taskTimeRange1.iProcessProductID =
          as_SchProductRouteRes.iProcessProductID
        taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID
        taskTimeRange1.cResourceNo = this.cResourceNo
        taskTimeRange1.resource = this.resource // 资源对象
        taskTimeRange1.schProductRouteRes = as_SchProductRouteRes // 资源任务对象
        taskTimeRange1.schData = as_SchProductRouteRes.schData // 所有排产数据
        taskTimeRange1.resTimeRange = this
        if (NoTimeRange.dBegTime >= adCanBegDate) {
          if (adCanEndDate > NoTimeRange.DEndTime) {
            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime
          } else {
            // 部分占用
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = adCanEndDate
            taskTimeRange1.AllottedTime = Math.floor(
              (taskTimeRange1.DEndTime.getTime() -
                taskTimeRange1.DBegTime.getTime()) /
                1000,
            )
          }
        } else if (
          NoTimeRange.dBegTime < adCanBegDate &&
          NoTimeRange.dEndTime > adCanBegDate
        ) {
          // 可用时间段大于时段开始时间,前面部分时间段不可用
          if (NoTimeRange.DEndTime >= adCanEndDate) {
            taskTimeRange1.DBegTime = adCanBegDate
            taskTimeRange1.DEndTime = adCanEndDate
            taskTimeRange1.AllottedTime = Math.floor(
              (taskTimeRange1.DEndTime.getTime() -
                taskTimeRange1.DBegTime.getTime()) /
                1000,
            )
          } else if (NoTimeRange.DEndTime < adCanEndDate) {
            taskTimeRange1.DBegTime = adCanBegDate
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime
            taskTimeRange1.AllottedTime = Math.floor(
              (taskTimeRange1.DEndTime.getTime() -
                taskTimeRange1.DBegTime.getTime()) /
                1000,
            )
          } else {
            // 不在空闲时段可用范围内，不能排,找下一个空闲时间段
            continue
          }
        } else {
          continue
        }
        if (taskTimeRange1.DBegTime < new Date('2011-01-01')) {
          throw new Error('数据异常，生成新任务开始时间不对!')
        }
        if (
          this.cIsInfinityAbility != '1' &&
          this.allottedTime + taskTimeRange1.AllottedTime > this.holdingTime
        ) {
          throw new Error(
            `出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:${NoTimeRange.dBegTime.toString()}时段结束时间:${NoTimeRange.dEndTime.toString()}任务开始时间:${taskTimeRange1.dBegTime.toString()}任务结束时间:${taskTimeRange1.dEndTime.toString()}`,
          )
          return -1
        }
        this.TaskTimeRangeSplit(NoTimeRange, taskTimeRange1)
        as_SchProductRouteRes.TaskTimeRangeList.push(taskTimeRange1)
        adCanBegDate = taskTimeRange1.DEndTime
        if (!this.CheckResTimeRange() && this.cIsInfinityAbility != '1') {
          throw new Error(
            '出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！',
          )
          return -1
        }
      }
    } catch (exp) {
      throw exp
    }
    return 0 // 剩下未排时间
  }

  TimeSchTaskSortInit(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: Date,
    ai_workTimeTask: number,
    adCanBegDateTask: Date,
    bSchdule: boolean,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    bFirtTime: boolean,
  ): number {
    let taskallottedTime = 0 // 任务在本时间段内 总安排时间
    let ai_workTimeOld = ai_workTime // 用于记录ai_workTime值
    if (
      as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID &&
      as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID
    ) {
      // 调试断点1 SchProduct
      let i = 1
    }
    let NoTimeRangeList = this.GetAvailableTimeRangeList(
      adCanBegDate,
      false,
      bSchdule,
      ai_workTime + ai_ResPreTime,
      false,
    )
    try {
      for (let i = 0; i < NoTimeRangeList.length; i++) {
        if (ai_workTime <= 0) break
        let NoTimeRange = NoTimeRangeList[i]
        if (bFirtTime) {
          // 是第一个排产时间段,计算换产时间
          if (NoTimeRange.AvailableTime == 0) continue // 是排第一个时段，期该时段没有可用时间，则继续
          ai_CycTimeTol = 0 // 设为0
          ai_ResPreTime = this.resource!.GetChangeTime(
            as_SchProductRouteRes,
            ai_workTime,
            NoTimeRange.DBegTime,
            ai_CycTimeTol,
            bSchdule,
          )
          if (ai_ResPreTime > 0) {
            let K = 0
          }
          ai_ResPreTime += ai_CycTimeTol
          ai_workTime = as_SchProductRouteRes.iResRationHour + ai_ResPreTime
        }
        if (bSchdule == false) {
          if (NoTimeRange.cTaskType != 0 && ai_workTime > 0) {
            // 只要不是空闲时间段
            bFirtTime = true // 是否第一个排产时间段
            ai_workTime = ai_workTimeTask // 返回原值
            adCanBegDate = NoTimeRange.DEndTime // adCanBegDateTask; // 重排可开始时间，重当前时段点开始
            adCanBegDateTask = NoTimeRange.DEndTime // 重新设置任务可开始时间,并返回
            continue
          }
        }
        if (
          NoTimeRange.AvailableTime < SchParam.PeriodLeftTime &&
          ai_workTime > 0
        ) {
          continue
        }
        let taskTimeRange1 = new TaskTimeRange()
        taskTimeRange1.cTaskType = 1 // 工作
        taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo
        taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID
        taskTimeRange1.iProcessProductID =
          as_SchProductRouteRes.iProcessProductID
        taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID
        taskTimeRange1.cResourceNo = this.cResourceNo
        taskTimeRange1.resource = this.resource // 资源对象
        taskTimeRange1.schProductRouteRes = as_SchProductRouteRes // 资源任务对象
        taskTimeRange1.schData = as_SchProductRouteRes.schData // 所有排产数据
        taskTimeRange1.resTimeRange = this
        if (bFirtTime) bFirtTime = false
        if (NoTimeRange.dBegTime >= adCanBegDate) {
          if (ai_workTime > NoTimeRange.AvailableTime) {
            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime
            ai_workTime -= NoTimeRange.AvailableTime
          } else {
            // 部分占用
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = new Date(
              NoTimeRange.DBegTime.getTime() + ai_workTime * 1000,
            )
            taskTimeRange1.AllottedTime = ai_workTime
            ai_workTime = 0 // 剩余待分配工作时间为0
          }
        } else {
          // 可用时间段大于时段开始时间,前面部分时间段不可用
          if (NoTimeRange.DEndTime > adCanBegDate) {
            let lTimeSpan =
              NoTimeRange.DEndTime.getTime() - adCanBegDate.getTime()
            let iAvailableTime = Math.floor(lTimeSpan / 1000)
            if (ai_workTime > iAvailableTime) {
              taskTimeRange1.DBegTime = adCanBegDate
              taskTimeRange1.DEndTime = NoTimeRange.DEndTime
              taskTimeRange1.AllottedTime = iAvailableTime
              ai_workTime -= iAvailableTime
            } else {
              // 部分占用,排完
              taskTimeRange1.DBegTime = adCanBegDate
              taskTimeRange1.DEndTime = new Date(
                adCanBegDate.getTime() + ai_workTime * 1000,
              )
              taskTimeRange1.AllottedTime = ai_workTime
              ai_workTime = 0 // 剩余待分配工作时间为0
            }
          } else {
            // 不在空闲时段可用范围内，不能排,找下一个空闲时间段
            continue
          }
        }
        if (taskTimeRange1.DBegTime < new Date('2011-01-01')) {
          throw new Error('数据异常，生成新任务开始时间不对!')
        }
        if (bSchdule) {
          // 正式排程
          if (
            this.cIsInfinityAbility != '1' &&
            this.allottedTime + taskTimeRange1.AllottedTime > this.holdingTime
          ) {
            let m = 1
            throw new Error(
              `出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:${NoTimeRange.dBegTime.toString()}时段结束时间:${NoTimeRange.dEndTime.toString()}任务开始时间:${taskTimeRange1.dBegTime.toString()}任务结束时间:${taskTimeRange1.dEndTime.toString()}`,
            )
            return -1
          }
          this.TaskTimeRangeSplit(NoTimeRange, taskTimeRange1)
          as_SchProductRouteRes.TaskTimeRangeList.push(taskTimeRange1)
        }
        adCanBegDate = taskTimeRange1.DEndTime
        bFirtTime = false // 是否第一个排产时间段,不是第一个
        if (bSchdule) {
          // 正式排程
          if (!this.CheckResTimeRange() && this.cIsInfinityAbility != '1') {
            throw new Error(
              '出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！',
            )
            return -1
          }
        }
      }
    } catch (exp) {
      throw exp
    }
    return ai_workTime // 剩下未排时间
  }

  TimeSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: Date,
    ai_workTimeTask: number,
    adCanBegDateTask: Date,
    bSchdule: boolean,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    bFirtTime: boolean,
    ai_DisWorkTime: number,
    bReCalWorkTime: boolean = true,
    resTimeRangeNext: ResTimeRange | null = null,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let taskallottedTime = 0 // 任务在本时间段内 总安排时间
    let ai_workTimeOld = ai_workTime // 用于记录ai_workTime值
    let iDayDis = 1 // 考虑学习曲线每天折扣
    let dtiDayDis = new Date(adCanBegDate.getTime() - 86400000) // 学习曲线日期
    let dtTaskBegDate = adCanBegDate // 任务开始排产日期
    let iTaskAllottedTime = 0 // 学习曲线 时段分配工时
    let ai_workTimeDisTol = ai_workTime // 用于记录学习曲线打则后的
    let ai_workTimeAct = 0 // 累计已排有效时间
    let message: string
    let ldtBeginDate = new Date()
    if (
      (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID &&
        as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID) ||
      as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID
    ) {
      // 调试断点1 SchProduct
      let i = 1
    }
    let NoTimeRangeList = this.GetAvailableTimeRangeList(
      adCanBegDate,
      false,
      bSchdule,
      ai_workTime + ai_ResPreTime,
      true,
    )
    if (
      (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID &&
        as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID) ||
      (as_SchProductRouteRes.iProcessProductID == 193864 &&
        as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)
    ) {
      // 调试断点1 SchProduct
      message = `3.4.2、TimeSchTask 排产顺序[${
        as_SchProductRouteRes.iSchSN
      }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
        as_SchProductRouteRes.iProcessProductID
      }],资源编号[${
        as_SchProductRouteRes.cResourceNo
      }],开始排产时间[${new Date()}],完成排产时间[${SchData.GetDateDiffString(
        ldtBeginDate,
        new Date(),
        'ms',
      )}]`
      SchParam.Debug(message, '资源运算')
      ldtBeginDate = new Date()
    }
    if (!this.CheckResTimeRange() && this.cIsInfinityAbility != '1') {
      throw new Error('出错位置：排程时段检查出错TimeSchTask.TimeSchTask！')
      return -1
    }
    try {
      for (let i = 0; i < NoTimeRangeList.length; i++) {
        if (ai_workTime <= 0) {
          ai_workTime = 0
          break
        }
        let NoTimeRange = NoTimeRangeList[i]
        if (bFirtTime) {
          // 是第一个排产时间段,计算换产时间
          if (NoTimeRange.AvailableTime == 0) continue // 是排第一个时段，期该时段没有可用时间，则继续
          if (bReCalWorkTime) {
            // 重新计算前准备时间，已下达生产任务单，不用重新计算 bReCalWorkTime = false
            ai_CycTimeTol = 0 // 设为0
            if (
              this.resource!.iSchBatch <= 1 &&
              as_SchProductRouteRes.iActResReqQty > 0
            )
              ai_ResPreTime = 0
            else {
              ai_ResPreTime = this.resource!.GetChangeTime(
                as_SchProductRouteRes,
                ai_workTime,
                NoTimeRange.DBegTime,
                ai_CycTimeTol,
                bSchdule,
                as_SchProductRouteResPre,
              )
            }
            if (ai_ResPreTime > 0) {
              let K = 0
            }
            ai_ResPreTime += ai_CycTimeTol
            ai_workTime = as_SchProductRouteRes.iResRationHour + ai_ResPreTime // as_SchProductRouteRes.iResRationHour + ai_ResPreTime
            ai_workTimeDisTol = ai_workTime
          }
          as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime
        }
        if (bSchdule == false) {
          if (as_SchProductRouteRes.cLearnCurvesNo != '') {
            iDayDis = as_SchProductRouteRes.TechLearnCurves.GetDayDisValue(
              as_SchProductRouteRes.dResLeanBegDate,
              NoTimeRange.DEndTime,
            )
          }
          if (
            NoTimeRange.taskTimeRangePost != null &&
            ai_workTime / iDayDis > NoTimeRange.AvailableTime
          ) {
            bFirtTime = true // 是否第一个排产时间段
            ai_workTime = ai_workTimeTask // 返回原值
            adCanBegDate = NoTimeRange.DEndTime // adCanBegDateTask; // 重排可开始时间，重当前时段点开始
            adCanBegDateTask = NoTimeRange.DEndTime // 重新设置任务可开始时间,并返回
          } else {
            adCanBegDate = NoTimeRange.DBegTime
          }
          if (NoTimeRange.cTaskType != 0 && ai_workTime > 0) {
            // 只要不是空闲时间段
            bFirtTime = true // 是否第一个排产时间段
            ai_workTime = ai_workTimeTask // 返回原值
            adCanBegDate = NoTimeRange.DEndTime // adCanBegDateTask; // 重排可开始时间，重当前时段点开始
            adCanBegDateTask = NoTimeRange.DEndTime // 重新设置任务可开始时间,并返回
            continue
          }
        }
        if (
          (as_SchProductRouteRes.iProcessProductID ==
            SchParam.iProcessProductID &&
            as_SchProductRouteRes.schProductRoute.iSchSdID ==
              SchParam.iSchSdID) ||
          (as_SchProductRouteRes.iProcessProductID == 193864 &&
            as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)
        ) {
          // 调试断点1 SchProduct
          message = `3.4.3、TimeSchTask 排产顺序[${
            as_SchProductRouteRes.iSchSN
          }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
            as_SchProductRouteRes.iProcessProductID
          }],资源编号[${
            as_SchProductRouteRes.cResourceNo
          }],开始排产时间[${new Date()}],完成排产时间[${SchData.GetDateDiffString(
            ldtBeginDate,
            new Date(),
            'ms',
          )}]`
          SchParam.Debug(message, '资源运算')
          ldtBeginDate = new Date()
        }
        if (
          NoTimeRange.AvailableTime < SchParam.PeriodLeftTime &&
          ai_workTime > 0 &&
          ai_workTime > NoTimeRange.AvailableTime
        ) {
          continue
        }
        let taskTimeRange1 = new TaskTimeRange()
        taskTimeRange1.cTaskType = 1 // 工作
        taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo
        taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID
        taskTimeRange1.iProcessProductID =
          as_SchProductRouteRes.iProcessProductID
        taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID
        taskTimeRange1.cResourceNo = this.cResourceNo
        taskTimeRange1.resource = this.resource // 资源对象
        taskTimeRange1.schProductRouteRes = as_SchProductRouteRes // 资源任务对象
        taskTimeRange1.schData = as_SchProductRouteRes.schData // 所有排产数据
        taskTimeRange1.resTimeRange = this
        if (as_SchProductRouteRes.cLearnCurvesNo != '') {
          iDayDis = as_SchProductRouteRes.TechLearnCurves.GetDayDisValue(
            as_SchProductRouteRes.dResLeanBegDate,
            NoTimeRange.DEndTime,
          )
          if (iDayDis > 0) {
            ai_workTime = Math.floor(ai_workTime / iDayDis) // 用于记录学习曲线打则后的
          } else {
            iDayDis = 1
          }
        } else {
          iDayDis = 1
        }
        if (bFirtTime) bFirtTime = false
        if (
          (as_SchProductRouteRes.iProcessProductID ==
            SchParam.iProcessProductID &&
            as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID) ||
          as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID
        ) {
          // 调试断点1 SchProduct
          let m = 1
        }
        if (NoTimeRange.dBegTime >= adCanBegDate) {
          if (ai_workTime > NoTimeRange.AvailableTime) {
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime
            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime
            iTaskAllottedTime = NoTimeRange.AvailableTime
            ai_workTime -= NoTimeRange.AvailableTime
          } else {
            // 部分占用
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = new Date(
              NoTimeRange.DBegTime.getTime() + ai_workTime * 1000,
            )
            taskTimeRange1.AllottedTime = ai_workTime
            iTaskAllottedTime = ai_workTime
            ai_workTime = 0 // 剩余待分配工作时间为0
          }
        } else {
          // 可用时间段大于时段开始时间,前面部分时间段不可用
          if (NoTimeRange.DEndTime > adCanBegDate) {
            let lTimeSpan =
              NoTimeRange.DEndTime.getTime() - adCanBegDate.getTime()
            let iAvailableTime = Math.floor(lTimeSpan / 1000)
            if (ai_workTime > iAvailableTime) {
              taskTimeRange1.DBegTime = adCanBegDate
              taskTimeRange1.DEndTime = NoTimeRange.DEndTime
              taskTimeRange1.AllottedTime = iAvailableTime
              iTaskAllottedTime = iAvailableTime
              ai_workTime -= iAvailableTime
            } else {
              // 部分占用,排完
              taskTimeRange1.DBegTime = adCanBegDate
              taskTimeRange1.DEndTime = new Date(
                adCanBegDate.getTime() + ai_workTime * 1000,
              )
              taskTimeRange1.AllottedTime = ai_workTime
              iTaskAllottedTime = ai_workTime
              ai_workTime = 0 // 剩余待分配工作时间为0
            }
          } else {
            // 不在空闲时段可用范围内，不能排,找下一个空闲时间段
            continue
          }
        }
        ai_workTimeAct += Math.floor(iTaskAllottedTime * iDayDis)
        taskTimeRange1.WorkTimeAct = Math.floor(iTaskAllottedTime * iDayDis)
        if (taskTimeRange1.DBegTime >= taskTimeRange1.DEndTime) {
          throw new Error('数据异常，生成新任务开始时间不对!开始大于结束时间')
          return -1
        }
        if (
          (as_SchProductRouteRes.iProcessProductID ==
            SchParam.iProcessProductID &&
            as_SchProductRouteRes.schProductRoute.iSchSdID ==
              SchParam.iSchSdID) ||
          (as_SchProductRouteRes.iProcessProductID == 193864 &&
            as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)
        ) {
          // 调试断点1 SchProduct
          message = `3.4.4、TimeSchTask 排产顺序[${
            as_SchProductRouteRes.iSchSN
          }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
            as_SchProductRouteRes.iProcessProductID
          }],资源编号[${
            as_SchProductRouteRes.cResourceNo
          }],开始排产时间[${new Date()}],完成排产时间[${SchData.GetDateDiffString(
            ldtBeginDate,
            new Date(),
            'ms',
          )}]`
          SchParam.Debug(message, '资源运算')
          ldtBeginDate = new Date()
        }
        if (bSchdule) {
          // 正式排程
          if (
            this.cResourceNo == '3.04.24' &&
            as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID
          ) {
            // 调试断点1 SchProduct
            let ii = 1
          }
          if (
            (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID &&
              as_SchProductRouteRes.iProcessProductID ==
                SchParam.iProcessProductID) ||
            as_SchProductRouteRes.iResourceAbilityID ==
              SchParam.iProcessProductID
          ) {
            // 调试断点1 SchProduct
            let ii = 1
          }
          if (
            this.cIsInfinityAbility != '1' &&
            this.allottedTime + taskTimeRange1.AllottedTime > this.holdingTime
          ) {
            let m = 1
          }
          this.TaskTimeRangeSplit(NoTimeRange, taskTimeRange1)
          if (
            this.cIsInfinityAbility != '1' &&
            this.allottedTime + this.AvailableTime > this.holdingTime
          ) {
            let m = 1
          }
          as_SchProductRouteRes.TaskTimeRangeList.push(taskTimeRange1)
        }
        adCanBegDate = taskTimeRange1.DEndTime
        if (bSchdule) {
          // 正式排程
          if (!this.CheckResTimeRange() && this.cIsInfinityAbility != '1') {
            let Errormessage = `检查时段数据出错CheckResTimeRange,排产顺序[${
              as_SchProductRouteRes.iSchSN
            }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
              as_SchProductRouteRes.iProcessProductID
            }],资源编号[${as_SchProductRouteRes.cResourceNo}],时段开始时间[${
              this.dBegTime
            }],时段结束时间[${this.dEndTime}],时段总工时[${
              this.holdingTime
            }],分配工时[${this.allottedTime}],空闲工时[${
              this.notWorkTime
            }],差异工时[${this.AvailableTime - this.notWorkTime}]`
            SchParam.Error(Errormessage, '资源运算出错')
            throw new Error(
              '出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！' +
                taskTimeRange1.DEndTime.toString(),
            )
            return -1
          }
        }
      }
    } catch (exp) {
      throw exp
    }
    ai_workTime = ai_workTimeDisTol - ai_workTimeAct
    if (ai_workTime < 0) ai_workTime = 0
    if (
      (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID &&
        as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID) ||
      (as_SchProductRouteRes.iProcessProductID == 193864 &&
        as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)
    ) {
      // 调试断点1 SchProduct
      message = `3.4.5、TimeSchTask 排产顺序[${
        as_SchProductRouteRes.iSchSN
      }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
        as_SchProductRouteRes.iProcessProductID
      }],资源编号[${
        as_SchProductRouteRes.cResourceNo
      }],开始排产时间[${new Date()}],完成排产时间[${SchData.GetDateDiffString(
        ldtBeginDate,
        new Date(),
        'ms',
      )}]`
      SchParam.Debug(message, '资源运算')
      ldtBeginDate = new Date()
    }
    return ai_workTime // 剩下未排时间
  }

  TimeSchTaskRev(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanEndDate: Date,
    ai_workTimeTask: number,
    adCanBegDateTask: Date,
    bSchdule: boolean,
    bFirtTime: boolean,
  ): number {
    let taskallottedTime = 0 // 任务在本时间段内 总安排时间
    let ai_workTimeOld = 0 // 用于记录ai_workTime值，当空闲时间段太下时，如10分钟，则该时间段不排，ai_workTime值还原。
    let NoTimeRangeList = this.GetAvailableTimeRangeList(
      adCanEndDate,
      true,
      bSchdule,
      ai_workTime,
      true,
    )
    if (
      (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID &&
        as_SchProductRouteRes.iProcessProductID ==
          SchParam.iProcessProductID) ||
      as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID
    ) {
      // 调试断点1 SchProduct
      let ii = 1
    }
    if (!this.CheckResTimeRange() && this.cIsInfinityAbility != '1') {
      throw new Error(
        '出错位置：倒排排程时段检查出错TimeSchTask.TimeSchTaskRev！',
      )
      return -1
    }
    try {
      for (let i = 0; i < NoTimeRangeList.length; i++) {
        if (ai_workTime <= 0) {
          ai_workTime = 0
          break
        }
        let NoTimeRange = NoTimeRangeList[i]
        if (bFirtTime) {
          // 是第一个排产时间段,计算换产时间
          if (NoTimeRange.AvailableTime == 0) continue // 是排第一个时段，期该时段没有可用时间，则继续
          as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime
        }
        if (bSchdule == false) {
          // 没有排完
          if (
            NoTimeRange.taskTimeRangePre != null &&
            ai_workTime > NoTimeRange.AvailableTime
          ) {
            bFirtTime = true // 是否第一个排产时间段
            ai_workTime = ai_workTimeTask // 返回原值
            adCanEndDate = NoTimeRange.DBegTime // adCanBegDateTask;
            adCanBegDateTask = NoTimeRange.DBegTime
          } else {
          }
          if (NoTimeRange.cTaskType == 1 && ai_workTime > 0) {
            bFirtTime = true // 是否第一个排产时间段
            ai_workTime = ai_workTimeTask // 返回原值
            adCanEndDate = NoTimeRange.DBegTime // adCanBegDateTask; // 重排可开始时间，重当前时段点开始,后面会累加更新
            adCanBegDateTask = NoTimeRange.DEndTime // 重新设置任务可开始时间,并返回
            continue
          }
        }
        if (
          NoTimeRange.AvailableTime < SchParam.PeriodLeftTime &&
          ai_workTime > 0 &&
          NoTimeRange.AvailableTime < ai_workTime
        ) {
          continue
        }
        let taskTimeRange1 = new TaskTimeRange()
        taskTimeRange1.cTaskType = 1 // 工作
        taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo
        taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID
        taskTimeRange1.iProcessProductID =
          as_SchProductRouteRes.iProcessProductID
        taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID
        taskTimeRange1.cResourceNo = this.cResourceNo
        taskTimeRange1.resource = this.resource // 资源对象
        taskTimeRange1.schProductRouteRes = as_SchProductRouteRes // 资源任务对象
        taskTimeRange1.schData = as_SchProductRouteRes.schData // 所有排产数据
        taskTimeRange1.resTimeRange = this
        if (bFirtTime) bFirtTime = false
        if (NoTimeRange.DEndTime <= adCanEndDate) {
          if (ai_workTime > NoTimeRange.AvailableTime) {
            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime
            ai_workTime -= NoTimeRange.AvailableTime
          } else {
            // 部分占用,倒排,从结束往前排
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime // NoTimeRange.DEndTime;  2020-03-28
            taskTimeRange1.DBegTime = new Date(
              NoTimeRange.DEndTime.getTime() - ai_workTime * 1000,
            )
            taskTimeRange1.AllottedTime = ai_workTime
            ai_workTime = 0 // 剩余待分配工作时间为0
          }
        } else {
          // 可用时间小于时段结束时间,后面部分时间段不可用
          if (NoTimeRange.DBegTime < adCanEndDate) {
            let lTimeSpan =
              adCanEndDate.getTime() - NoTimeRange.DBegTime.getTime()
            let iAvailableTime = Math.floor(lTimeSpan / 1000)
            if (ai_workTime > iAvailableTime) {
              taskTimeRange1.DBegTime = NoTimeRange.DBegTime
              taskTimeRange1.DEndTime = adCanEndDate
              taskTimeRange1.AllottedTime = iAvailableTime
              ai_workTime -= iAvailableTime
            } else {
              // 部分占用,排完
              taskTimeRange1.DBegTime = new Date(
                adCanEndDate.getTime() - ai_workTime * 1000,
              )
              taskTimeRange1.DEndTime = adCanEndDate
              taskTimeRange1.AllottedTime = ai_workTime
              ai_workTime = 0 // 剩余待分配工作时间为0
            }
          } else {
            // 不在空闲时段可用范围内，不能排,找下一个空闲时间段
            continue
          }
        }
        if (taskTimeRange1.DBegTime < new Date('2011-01-01')) {
          throw new Error('数据异常，生成新任务开始时间不对!')
          return -1
        }
        if (
          taskTimeRange1.DBegTime < NoTimeRange.DBegTime &&
          taskTimeRange1.DBegTime > NoTimeRange.DEndTime
        ) {
          throw new Error(
            '数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!',
          )
          return -1
        }
        if (bSchdule) {
          // 正式排程
          if (taskTimeRange1.AllottedTime == 0) {
            let K = 0
            continue
          }
          if (
            this.cIsInfinityAbility != '1' &&
            this.allottedTime + taskTimeRange1.AllottedTime > this.holdingTime
          ) {
            let m = 1
            throw new Error(
              `出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:${NoTimeRange.dBegTime.toString()}时段结束时间:${NoTimeRange.dEndTime.toString()}任务开始时间:${taskTimeRange1.dBegTime.toString()}任务结束时间:${taskTimeRange1.dEndTime.toString()}`,
            )
            return -1
          }
          this.TaskTimeRangeSplit(NoTimeRange, taskTimeRange1)
          if (
            this.cIsInfinityAbility != '1' &&
            this.allottedTime + this.AvailableTime > this.holdingTime
          ) {
            let m = 1
            throw new Error(
              `出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:${NoTimeRange.dBegTime.toString()}时段结束时间:${NoTimeRange.dEndTime.toString()}任务开始时间:${taskTimeRange1.dBegTime.toString()}任务结束时间:${taskTimeRange1.dEndTime.toString()}`,
            )
            return -1
          }
          as_SchProductRouteRes.TaskTimeRangeList.push(taskTimeRange1)
        }
        adCanEndDate = taskTimeRange1.DBegTime
        bFirtTime = false // 是否第一个排产时间段,不是第一个
        if (bSchdule) {
          // 正式排程
          if (!this.CheckResTimeRange() && this.cIsInfinityAbility != '1') {
            throw new Error(
              `出错位置：TimeSchTaskRev, 订单行号：${
                as_SchProductRouteRes.iSchSdID
              }产品编号[${
                as_SchProductRouteRes.schProductRoute.cInvCode
              }]加工物料[${as_SchProductRouteRes.cInvCode}]在资源[${
                as_SchProductRouteRes.cResourceNo
              }]无法排下,任务号[${
                as_SchProductRouteRes.iProcessProductID
              }],时段日期[${this.DBegTime.toDateString()} ${this.DBegTime.toTimeString()}]检查异常！`,
            )
            return -1
          }
        }
      }
    } catch (exp) {
      throw exp
    }
    return ai_workTime // 剩下未排时间
  }

  TimeSchTaskRevInfinite(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanEndDate: Date,
    ai_workTimeTask: number,
    adCanBegDateTask: Date,
    bSchdule: boolean,
    bFirtTime: boolean,
  ): number {
    let taskallottedTime = 0 // 任务在本时间段内 总安排时间
    let ai_workTimeOld = 0 // 用于记录ai_workTime值，当空闲时间段太下时，如10分钟，则该时间段不排，ai_workTime值还原。
    let NoTimeRangeList = this.GetAvailableTimeRangeList(
      adCanEndDate,
      true,
      bSchdule,
      ai_workTime,
      false,
    )
    if (
      (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID &&
        as_SchProductRouteRes.iProcessProductID ==
          SchParam.iProcessProductID) ||
      as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID
    ) {
      // 调试断点1 SchProduct
      let ii = 1
    }
    try {
      for (let i = 0; i < NoTimeRangeList.length; i++) {
        if (ai_workTime <= 0) {
          ai_workTime = 0
          break
        }
        let NoTimeRange = NoTimeRangeList[i]
        if (bFirtTime) {
          // 是第一个排产时间段,计算换产时间
          as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime
        }
        if (bSchdule == false) {
          // 没有排完
        }
        let taskTimeRange1 = new TaskTimeRange()
        taskTimeRange1.cTaskType = 1 // 工作
        taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo
        taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID
        taskTimeRange1.iProcessProductID =
          as_SchProductRouteRes.iProcessProductID
        taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID
        taskTimeRange1.cResourceNo = this.cResourceNo
        taskTimeRange1.resource = this.resource // 资源对象
        taskTimeRange1.schProductRouteRes = as_SchProductRouteRes // 资源任务对象
        taskTimeRange1.schData = as_SchProductRouteRes.schData // 所有排产数据
        taskTimeRange1.resTimeRange = this
        if (bFirtTime) bFirtTime = false
        if (NoTimeRange.DEndTime <= adCanEndDate) {
          if (ai_workTime > NoTimeRange.HoldingTime) {
            taskTimeRange1.AllottedTime = NoTimeRange.HoldingTime
            taskTimeRange1.DBegTime = NoTimeRange.DBegTime
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime
            ai_workTime -= NoTimeRange.HoldingTime
          } else {
            // 部分占用,倒排,从结束往前排
            taskTimeRange1.DEndTime = NoTimeRange.DEndTime // NoTimeRange.DEndTime;  2020-03-28
            taskTimeRange1.DBegTime = new Date(
              NoTimeRange.DEndTime.getTime() - ai_workTime * 1000,
            )
            taskTimeRange1.AllottedTime = ai_workTime
            ai_workTime = 0 // 剩余待分配工作时间为0
          }
        } else {
          // 可用时间小于时段结束时间,后面部分时间段不可用
          if (NoTimeRange.DBegTime < adCanEndDate) {
            let lTimeSpan =
              adCanEndDate.getTime() - NoTimeRange.DBegTime.getTime()
            let iAvailableTime = Math.floor(lTimeSpan / 1000)
            if (ai_workTime > iAvailableTime) {
              taskTimeRange1.DBegTime = NoTimeRange.DBegTime
              taskTimeRange1.DEndTime = adCanEndDate
              taskTimeRange1.AllottedTime = iAvailableTime
              ai_workTime -= iAvailableTime
            } else {
              // 部分占用,排完
              taskTimeRange1.DBegTime = new Date(
                adCanEndDate.getTime() - ai_workTime * 1000,
              )
              taskTimeRange1.DEndTime = adCanEndDate
              taskTimeRange1.AllottedTime = ai_workTime
              ai_workTime = 0 // 剩余待分配工作时间为0
            }
          } else {
            // 不在空闲时段可用范围内，不能排,找下一个空闲时间段
            continue
          }
        }
        if (taskTimeRange1.DBegTime < new Date('2011-01-01')) {
          throw new Error('数据异常，生成新任务开始时间不对!')
          return -1
        }
        if (
          taskTimeRange1.DBegTime < NoTimeRange.DBegTime &&
          taskTimeRange1.DBegTime > NoTimeRange.DEndTime
        ) {
          throw new Error(
            '数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!',
          )
          return -1
        }
        if (bSchdule) {
          // 正式排程
          if (taskTimeRange1.AllottedTime == 0) {
            let K = 0
            continue
          }
          as_SchProductRouteRes.TaskTimeRangeList.push(taskTimeRange1)
        }
        adCanEndDate = taskTimeRange1.DBegTime
        bFirtTime = false // 是否第一个排产时间段,不是第一个
      }
    } catch (exp) {
      throw exp
    }
    return ai_workTime // 剩下未排时间
  }

  CheckResTimeRange(): boolean {
    if (this.cIsInfinityAbility == '1') return true
    if (Math.abs(this.AvailableTime - this.notWorkTime) > 5) {
      // 不能直接相等，可能有计算误差 2022-06-22
      this.ThowErrText(
        `检查分配时间${this.allottedTime} > 空闲时间${this.notWorkTime}`,
      ) // 小数点，有误差
      return false
    }
    let liNotWorkTime = 0
    for (let taskTimeRange of this.TaskTimeRangeList) {
      liNotWorkTime += taskTimeRange.NotWorkTime
    }
    if (Math.abs(liNotWorkTime - this.notWorkTime) > 5) {
      this.ThowErrText(
        `检查空闲时间${this.notWorkTime}与明细汇总${liNotWorkTime}不一致`,
      ) // 小数点，有误差
      return false
    }
    if (Math.abs(this.allottedTime + this.notWorkTime - this.holdingTime) > 5) {
      this.ThowErrText(
        `检查分配时间${this.allottedTime} + 空闲时间${this.notWorkTime} 大于 时间段总长${this.holdingTime}`,
      ) // 小数点，有误差
      return false
    }
    return true
  }

  ThowErrText(ErrText: string): boolean {
    throw new Error(
      '出错位置：排程时段检查出错TimeSchTask.ThowErrText！' + ErrText,
    )
    return false // 小数点，有误差
  }

  GetAvailableTimeRange(
    adCanBegDate: Date,
    bSchRev: boolean = false,
  ): TaskTimeRange | null {
    let lTaskTimeRangeFind: TaskTimeRange | null = null
    let TaskTimeRangeTemp: TaskTimeRange[] = []
    if (!this.CheckResTimeRange() && this.cIsInfinityAbility != '1') {
      throw new Error(
        '出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！',
      )
      return null
    }
    if (this.CIsInfinityAbility == '1') {
      // 产能无限，整个时段都可用
      TaskTimeRangeTemp = this.TaskTimeRangeList.filter(
        (p2) => p2.cTaskType == 0,
      )
      if (TaskTimeRangeTemp.length > 0) {
        lTaskTimeRangeFind = TaskTimeRangeTemp[TaskTimeRangeTemp.length - 1]
      }
    } else {
      // 产能有限
      if (bSchRev) {
        // 倒排
        TaskTimeRangeTemp = this.TaskTimeRangeList.filter(
          (p2) =>
            ((p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate) ||
              p2.DEndTime < adCanBegDate) &&
            p2.cTaskType == 0,
        )
        if (TaskTimeRangeTemp.length > 0) {
          lTaskTimeRangeFind = TaskTimeRangeTemp[TaskTimeRangeTemp.length - 1]
        }
      } else {
        // 正排
        TaskTimeRangeTemp = this.TaskTimeRangeList.filter(
          (p2) =>
            ((p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate) ||
              p2.DBegTime > adCanBegDate) &&
            p2.cTaskType == 0,
        )
        if (TaskTimeRangeTemp.length > 0) {
          lTaskTimeRangeFind = TaskTimeRangeTemp[0]
        }
      }
    }
    return lTaskTimeRangeFind
  }
  GetAvailableTimeRangeList(
    adCanBegDate: Date,
    bSchRev: boolean,
    bSchdule: boolean = true,
    ai_workTime: number = 0,
    bIncludeWorkTime: boolean = true,
  ): TaskTimeRange[] {
    if (bSchdule && bIncludeWorkTime) bIncludeWorkTime = false
    let lTaskTimeRangeList: TaskTimeRange[] = []
    if (this.CIsInfinityAbility == '1') {
      lTaskTimeRangeList = this.TaskTimeRangeList.filter(
        (p2) => p2.cTaskType == 0,
      )
      lTaskTimeRangeList.sort((p1, p2) =>
        Comparer.default(p1.DBegTime, p2.DBegTime),
      )
    } else {
      if (bSchRev) {
        if (bSchdule) {
          lTaskTimeRangeList = this.TaskTimeRangeList.filter(
            (p2) => p2.DBegTime <= adCanBegDate && p2.cTaskType == 0,
          )
        } else {
          lTaskTimeRangeList = this.TaskTimeRangeList.filter(
            (p2) => p2.DBegTime <= adCanBegDate && p2.cTaskType == 0,
          )
          if (bIncludeWorkTime)
            lTaskTimeRangeList.push(...this.WorkTimeRangeList)
        }
        lTaskTimeRangeList.sort((p1, p2) =>
          Comparer.default(p2.DBegTime, p1.DBegTime),
        )
      } else {
        if (bSchdule) {
          lTaskTimeRangeList = this.TaskTimeRangeList.filter(
            (p2) => p2.DEndTime > adCanBegDate && p2.cTaskType == 0,
          )
        } else {
          lTaskTimeRangeList = this.TaskTimeRangeList.filter(
            (p2) => p2.DEndTime > adCanBegDate && p2.cTaskType == 0,
          )
          const iCount = lTaskTimeRangeList.length
          if (iCount < 1) {
            if (this.WorkTimeRangeList.length > 0)
              lTaskTimeRangeList.push(this.WorkTimeRangeList[0])
          } else {
            if (bIncludeWorkTime)
              lTaskTimeRangeList.push(...this.WorkTimeRangeList)
          }
        }
        if (lTaskTimeRangeList.length > 1)
          lTaskTimeRangeList.sort((p1, p2) =>
            Comparer.default(p1.DBegTime, p2.DBegTime),
          )
      }
    }
    return lTaskTimeRangeList
  }

  GetAvailableTime(adCanBegDate: Date, bSchRev: boolean = false): Date {
    const lTaskTimeRange = this.GetAvailableTimeRange(adCanBegDate, bSchRev)
    if (this.CIsInfinityAbility == '1') {
      return adCanBegDate
    } else {
      if (bSchRev) {
        if (lTaskTimeRange != null) {
          return lTaskTimeRange.DEndTime
        }
        return adCanBegDate
      } else {
        if (lTaskTimeRange != null) {
          return lTaskTimeRange.DBegTime
        }
        return new Date()
      }
    }
  }

  GetResTimeRange(
    adCanBegDate: Date,
    as_Type: string = '1',
  ): ResTimeRange | null {
    const lResTimeRangeList = this.resource.ResTimeRangeList.filter(
      (p2: ResTimeRange) =>
        p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate,
    )
    if (lResTimeRangeList.length > 0) return lResTimeRangeList[0]
    else return null
  }

  TaskTimeRangeSplit(
    aToltalTaskRange: TaskTimeRange,
    aNewTaskRange: TaskTimeRange,
  ): number {
    let NoTaskTime1: TaskTimeRange | null = null
    let NoTaskTime2: TaskTimeRange | null = null
    if (this.CIsInfinityAbility == '1') {
      this.WorkTimeRangeList.push(aNewTaskRange)
    } else {
      try {
        if (
          aNewTaskRange.iProcessProductID == SchParam.iProcessProductID &&
          aNewTaskRange.iSchSdID == SchParam.iSchSdID
        ) {
          let i = 1
        }
        if (!this.CheckResTimeRange() && this.CIsInfinityAbility != '1') {
          throw new Error(
            '出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！',
          )
          return -1
        }
        if (this.AllottedTime + aNewTaskRange.AllottedTime > this.HoldingTime) {
          throw new Error(
            `出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:${aToltalTaskRange.DBegTime.toString()}时段结束时间:${aToltalTaskRange.DEndTime.toString()}任务开始时间:${aNewTaskRange.DBegTime.toString()}任务结束时间:${aNewTaskRange.DEndTime.toString()}`,
          )
          return -1
        }
        if (
          aToltalTaskRange.DBegTime == aNewTaskRange.DBegTime &&
          aToltalTaskRange.DEndTime == aNewTaskRange.DEndTime
        ) {
          this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, null)
        } else if (aToltalTaskRange.DBegTime == aNewTaskRange.DBegTime) {
          NoTaskTime1 = this.GetNoWorkTaskTimeRange(
            aNewTaskRange.DEndTime,
            aToltalTaskRange.DEndTime,
          )
          this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1)
          aToltalTaskRange.taskTimeRangePre = aNewTaskRange
        } else if (aToltalTaskRange.DEndTime == aNewTaskRange.DEndTime) {
          NoTaskTime1 = this.GetNoWorkTaskTimeRange(
            aToltalTaskRange.DBegTime,
            aNewTaskRange.DBegTime,
          )
          this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1)
          aToltalTaskRange.taskTimeRangePost = aNewTaskRange
        } else {
          NoTaskTime1 = this.GetNoWorkTaskTimeRange(
            aToltalTaskRange.DBegTime,
            aNewTaskRange.DBegTime,
          )
          NoTaskTime2 = this.GetNoWorkTaskTimeRange(
            aNewTaskRange.DEndTime,
            aToltalTaskRange.DEndTime,
          )
          this.ModifyResTimeRange(
            aNewTaskRange,
            aToltalTaskRange,
            NoTaskTime1,
            NoTaskTime2,
          )
          aToltalTaskRange.taskTimeRangePost = aNewTaskRange
          if (NoTaskTime2) NoTaskTime2.taskTimeRangePre = aNewTaskRange
        }
        if (this.CIsInfinityAbility != '1' && !this.CheckResTimeRange()) {
          throw new Error(
            `出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！时段开始时间:${aToltalTaskRange.DBegTime.toString()}时段结束时间:${aToltalTaskRange.DEndTime.toString()}任务开始时间:${aNewTaskRange.DBegTime.toString()}任务结束时间:${aNewTaskRange.DEndTime.toString()}`,
          )
          return -1
        }
      } catch (exp) {
        throw exp
      }
    }
    return 1
  }

  GetNoWorkTaskTimeRange(
    dBegDate: Date,
    dEndDate: Date,
    bCreate: boolean = false,
  ): TaskTimeRange {
    const NoTaskTime = new TaskTimeRange()
    if (dBegDate >= dEndDate) {
      throw new Error('生成空闲任务时间段必须大于0！')
      return NoTaskTime
    }
    NoTaskTime.cVersionNo = ''
    NoTaskTime.cTaskType = 0
    NoTaskTime.iSchSdID = -1
    NoTaskTime.iProcessProductID = -1
    NoTaskTime.iResProcessID = -1
    NoTaskTime.cResourceNo = this.cResourceNo
    NoTaskTime.CIsInfinityAbility = this.CIsInfinityAbility
    NoTaskTime.DBegTime = dBegDate
    NoTaskTime.DEndTime = dEndDate
    NoTaskTime.AllottedTime = 0
    NoTaskTime.HoldingTime = Math.floor(
      (NoTaskTime.DEndTime.getTime() - NoTaskTime.DBegTime.getTime()) / 1000,
    )
    NoTaskTime.resource = this.resource
    NoTaskTime.resTimeRange = this
    NoTaskTime.schProductRouteRes = null
    NoTaskTime.schData = this.schData
    if (bCreate) {
      NoTaskTime.AddTaskTimeRange(this)
    }
    return NoTaskTime
  }

  CopyTaskTimeRange(
    aOldTaskRange: TaskTimeRange,
    aNewTaskRange: TaskTimeRange,
  ): TaskTimeRange {
    aOldTaskRange.cVersionNo = aNewTaskRange.cVersionNo
    aOldTaskRange.iSchSdID = aNewTaskRange.iSchSdID
    aOldTaskRange.iProcessProductID = aNewTaskRange.iProcessProductID
    aOldTaskRange.iResProcessID = aNewTaskRange.iResProcessID
    aOldTaskRange.cResourceNo = this.cResourceNo
    aOldTaskRange.DBegTime = aNewTaskRange.DBegTime
    aOldTaskRange.DEndTime = aNewTaskRange.DEndTime
    aOldTaskRange.HoldingTime = aNewTaskRange.HoldingTime
    aOldTaskRange.AllottedTime = aNewTaskRange.AllottedTime
    aOldTaskRange.resource = aNewTaskRange.resource
    aOldTaskRange.resTimeRange = aNewTaskRange.resTimeRange
    aOldTaskRange.schProductRouteRes = aNewTaskRange.schProductRouteRes
    aOldTaskRange.schData = aNewTaskRange.schData
    aOldTaskRange.cTaskType = aNewTaskRange.cTaskType
    aOldTaskRange.iSchSdID = aNewTaskRange.iSchSdID
    aOldTaskRange.iProcessProductID = aNewTaskRange.iProcessProductID
    aOldTaskRange.iResProcessID = aNewTaskRange.iResProcessID
    aOldTaskRange.iSchSNMax = SchParam.iSchSNMax
    return aOldTaskRange
  }

  MegTaskTimeRangeAll(): number {
    if (this.TaskTimeRangeList.length <= 1) return 1
    let TaskTimeRangePre: TaskTimeRange | null = null
    let TaskTimeRangeNew: TaskTimeRange | null = null
    this.TaskTimeRangeList.sort((p1, p2) =>
      Comparer.default(p1.DBegTime, p2.DBegTime),
    )
    const iCount = this.TaskTimeRangeList.length
    let allottedTime = 0
    for (let i = iCount - 1; i >= 0; i--) {
      TaskTimeRangeNew = this.TaskTimeRangeList[i]
      if (
        TaskTimeRangePre != null &&
        TaskTimeRangePre.cTaskType == 0 &&
        TaskTimeRangeNew.cTaskType == 0 &&
        TaskTimeRangeNew.DEndTime == TaskTimeRangePre.DBegTime
      ) {
        TaskTimeRangeNew = this.MegTaskTimeRange(
          TaskTimeRangePre,
          TaskTimeRangeNew,
        )
        if (TaskTimeRangeNew == null) {
          throw new Error(
            '检验任务合并时间段出错,位置ReTimeRange.MegTaskTimeRangeAll',
          )
          return -1
        }
      }
      TaskTimeRangePre = TaskTimeRangeNew
    }
    return 1
  }

  MegTaskTimeRange(
    TaskTimeRangeLast: TaskTimeRange,
    TaskTimeRangeNew: TaskTimeRange,
  ): TaskTimeRange | null {
    if (TaskTimeRangeLast.cTaskType != 0) return null
    if (TaskTimeRangeNew.cTaskType != 0) return null
    const NoWorkTaskTime = this.GetNoWorkTaskTimeRange(
      TaskTimeRangeNew.DBegTime,
      TaskTimeRangeLast.DEndTime,
      false,
    )
    NoWorkTaskTime.taskTimeRangePre = TaskTimeRangeLast.taskTimeRangePre
    NoWorkTaskTime.taskTimeRangePost = TaskTimeRangeNew.taskTimeRangePost
    TaskTimeRangeNew.taskTimeRangePre = TaskTimeRangeLast.taskTimeRangePre
    if (
      Math.abs(
        TaskTimeRangeLast.HoldingTime +
          TaskTimeRangeNew.HoldingTime -
          NoWorkTaskTime.HoldingTime,
      ) > 5
    ) {
      const message = `2、原时间段长[${TaskTimeRangeLast.HoldingTime}],计划ID[${TaskTimeRangeLast.iSchSdID}],任务ID[${TaskTimeRangeLast.iProcessProductID}],资源编号[${TaskTimeRangeLast.cResourceNo}],新时间段长[${TaskTimeRangeNew.HoldingTime}],未分配时间段长[${NoWorkTaskTime.HoldingTime}]`
      SchParam.Debug(message, '资源运算')
      throw new Error(
        '检验任务拆分后时间段出错,位置ReTimeRange.MegTaskTimeRange!' + message,
      )
      return null
    }
    if (!this.CheckResTimeRange()) {
      const message = `2、原时间段长[${TaskTimeRangeLast.HoldingTime}],计划ID[${TaskTimeRangeLast.iSchSdID}],任务ID[${TaskTimeRangeLast.iProcessProductID}],资源编号[${TaskTimeRangeLast.cResourceNo}],新时间段长[${TaskTimeRangeNew.HoldingTime}],未分配时间段长[${NoWorkTaskTime.HoldingTime}]`
      SchParam.Debug(message, '资源运算')
      throw new Error(
        '清除任务时合并空闲时间段出错,位置ReTimeRange.MegTaskTimeRange！' +
          message,
      )
      return null
    }
    TaskTimeRangeLast.RemoveTaskTimeRange(this)
    TaskTimeRangeNew.RemoveTaskTimeRange(this)
    NoWorkTaskTime.AddTaskTimeRange(this)
    return NoWorkTaskTime
  }

  CheckTaskOverlap(
    as_SchProductRouteRes: any,
    dt_ResDate: Date,
    bSchRev: boolean = false,
  ): number {
    if (
      this.CheckCurTimeTaskOverlap(
        as_SchProductRouteRes,
        dt_ResDate,
        dt_ResDate,
        bSchRev,
      ) < 0
    )
      return -1
    if (bSchRev == false) {
      if (
        this.CheckNextTimeTaskOverlap(
          as_SchProductRouteRes,
          this.DEndTime,
          dt_ResDate,
          bSchRev,
        ) < 0
      )
        return -1
    } else {
      if (
        this.CheckNextTimeTaskOverlap(
          as_SchProductRouteRes,
          this.DBegTime,
          dt_ResDate,
          bSchRev,
        ) < 0
      )
        return -1
    }
    return 1
  }

  CheckCurTimeTaskOverlap(
    as_SchProductRouteRes: any,
    dt_ResDate: Date,
    adCanBegDate: Date,
    bSchRev: boolean = false,
  ): number {
    if (this.TaskTimeRangeList.length < 1) return 1
    if (this.CIsInfinityAbility == '1') return 1
    try {
      const TaskTimeRange2 = this.TaskTimeRangeList.find(
        (p1) =>
          p1.iSchSdID == as_SchProductRouteRes.iSchSdID &&
          p1.iProcessProductID == as_SchProductRouteRes.iProcessProductID &&
          p1.iResProcessID == as_SchProductRouteRes.iResProcessID &&
          (p1.DEndTime == dt_ResDate || p1.DBegTime == dt_ResDate),
      )
      if (TaskTimeRange2 != null) {
        let ldtResEndDate: Date
        if (bSchRev == false) {
          ldtResEndDate = TaskTimeRange2.DEndTime
          const TaskTimeRangeList1 = this.resource
            .GetTaskTimeRangeList()
            .filter(
              (p1) =>
                p1.DBegTime >= ldtResEndDate &&
                p1.CResourceNo == this.cResourceNo,
            )
          if (TaskTimeRangeList1.length < 1) return -1
          const TaskTimeRange3 = TaskTimeRangeList1[0]
          if (
            TaskTimeRange3 != null &&
            TaskTimeRange3.cTaskType == 1 &&
            TaskTimeRange3.iSchSdID != TaskTimeRange2.iSchSdID &&
            TaskTimeRange3.iProcessProductID !=
              TaskTimeRange2.iProcessProductID &&
            TaskTimeRange3.iResProcessID != TaskTimeRange2.iResProcessID
          ) {
            adCanBegDate = TaskTimeRange3.DEndTime
            return -1
          }
        } else {
          ldtResEndDate = TaskTimeRange2.DBegTime
          const TaskTimeRangeList1 = this.resource
            .GetTaskTimeRangeList(false)
            .filter(
              (p1) =>
                p1.DEndTime <= ldtResEndDate &&
                p1.CResourceNo == this.cResourceNo,
            )
          if (TaskTimeRangeList1.length < 1) return -1
          const TaskTimeRange3 = TaskTimeRangeList1[0]
          if (
            TaskTimeRange3 != null &&
            TaskTimeRange3.cTaskType == 1 &&
            TaskTimeRange3.iSchSdID != TaskTimeRange2.iSchSdID &&
            TaskTimeRange3.iProcessProductID !=
              TaskTimeRange2.iProcessProductID &&
            TaskTimeRange3.iResProcessID != TaskTimeRange2.iResProcessID
          ) {
            adCanBegDate = TaskTimeRange3.DBegTime
            return -1
          }
        }
      }
    } catch (error) {
      throw new Error(
        '检验任务是否重叠出错,位置ReTimeRange.CheckCurTimeTaskOverlap！工序ID号：' +
          as_SchProductRouteRes.iProcessProductID +
          '\n\r ' +
          error.message,
      )
      return -1
    }
    return 1
  }

  CheckNextTimeTaskOverlap(
    as_SchProductRouteRes: any,
    dt_ResDate: Date,
    adCanBegDate: Date,
    bSchRev: boolean = false,
  ): number {
    let ResTimeRange1: any = null
    if (this.CIsInfinityAbility == '1') return 1
    try {
      if (bSchRev == false) {
        const ResTimeRangeList = this.resource.ResTimeRangeList.filter(
          (p1) =>
            p1.DBegTime >= dt_ResDate && p1.CResourceNo == this.cResourceNo,
        )
        ResTimeRangeList.sort((p1, p2) =>
          Comparer.default(p1.DBegTime, p2.DBegTime),
        )
        if (ResTimeRangeList.length > 0) {
          ResTimeRange1 = ResTimeRangeList[0]
          return ResTimeRange1.CheckCurTimeTaskOverlap(
            as_SchProductRouteRes,
            ResTimeRange1.DBegTime,
            adCanBegDate,
            bSchRev,
          )
        } else {
          return -1
        }
      } else {
        const ResTimeRangeList = this.resource.ResTimeRangeList.filter(
          (p1) =>
            p1.DEndTime <= dt_ResDate && p1.CResourceNo == this.cResourceNo,
        )
        ResTimeRangeList.sort((p1, p2) =>
          Comparer.default(p1.DBegTime, p2.DBegTime),
        )
        if (ResTimeRangeList.length > 0) {
          ResTimeRange1 = ResTimeRangeList[ResTimeRangeList.length - 1]
          return ResTimeRange1.CheckCurTimeTaskOverlap(
            as_SchProductRouteRes,
            ResTimeRange1.DEndTime,
            adCanBegDate,
            bSchRev,
          )
        } else {
          return -1
        }
      }
    } catch (error) {
      throw new Error(
        '检验任务是否重叠出错,位置ReTimeRange.CheckNextTimeTaskOverlap！工序ID号：' +
          as_SchProductRouteRes.iProcessProductID +
          '\n\r ' +
          error.message,
      )
      return -1
    }
    return 1
  }

  ModifyResTimeRange(
    aNewTaskRange: TaskTimeRange,
    oldTaskTimeRange: TaskTimeRange,
    NoTaskTime1: TaskTimeRange | null,
    NoTaskTime2: TaskTimeRange | null = null,
  ): number {
    if (!this.CheckResTimeRange() && this.CIsInfinityAbility != '1') {
      throw new Error(
        '出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！',
      )
      return -1
    }
    aNewTaskRange.AddWorkimeRange(this)
    if (NoTaskTime1 == null) {
      oldTaskTimeRange.RemoveTaskTimeRange(this)
    } else {
      oldTaskTimeRange.AddTaskTimeRange(this, NoTaskTime1)
    }
    if (NoTaskTime2 != null) {
      NoTaskTime2.AddTaskTimeRange(this)
    }
    if (!this.CheckResTimeRange() && this.CIsInfinityAbility != '1') {
      throw new Error(
        '出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！',
      )
      return -1
    }
    return 1
  }

  get CResourceNo(): string {
    return this.cResourceNo
  }

  set CResourceNo(value: string) {
    this.cResourceNo = value
  }

  get CIsInfinityAbility(): string {
    return this.CIsInfinityAbility
  }

  set CIsInfinityAbility(value: string) {
    this.CIsInfinityAbility = value
  }

  get DBegTime(): Date {
    return this.dBegTime
  }

  set DBegTime(value: Date) {
    if (value < new Date(2000, 1, 1)) {
      throw new Error(
        `资源编号${
          this.cResourceNo
        }时间段开始日期${value.toString()}不能小于2000-01-01日,开始时间${this.dBegTime.toDateString()},结束时间${this.dEndTime.toDateString()}`,
      )
    }
    this.dBegTime = value
    this.HoldingTime = this.HoldingTime
  }
  get DEndTime(): DateTime {
    return this.dEndTime
  }

  set DEndTime(value: DateTime) {
    if (value < DateTime.fromObject({ year: 2000, month: 1, day: 1 })) {
      throw new Error(
        `资源编号${
          this.cResourceNo
        }时间段结束日期${value.toISO()}不能小于2000-01-01日,开始时间${this.dBegTime.toLocaleString()},结束时间${this.dEndTime.toLocaleString()}`,
      )
    }
    if (value <= this.dBegTime) {
      throw new Error(
        `资源编号${
          this.cResourceNo
        }时间段结束日期${value.toISO()}不能小于时段开始时间,开始时间${this.dBegTime.toLocaleString()},结束时间${this.dEndTime.toLocaleString()}`,
      )
    }
    this.dEndTime = value
    this.holdingTime = this.HoldingTime
  }

  get HoldingTime(): number {
    if (this.dEndTime && this.dBegTime && this.dEndTime > this.dBegTime) {
      //@ts-ignore
      const diff = this.dEndTime.diff(this.dBegTime)
      if (diff.as('seconds') > 0) {
        return Math.floor(diff.as('seconds'))
      } else {
        throw new Error('the timerange no set')
      }
    } else {
      return 0
    }
  }

  set HoldingTime(value: number) {
    // Empty setter
  }

  get AllottedTime(): number {
    if (this.constructor.name === 'TaskTimeRange') {
      return this.allottedTime
    } else {
      let allottedTimeTemp = 0
      if (
        this.cIsInfinityAbility !== '1' &&
        this.WorkTimeRangeList.length > 0
      ) {
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
    if (this.cIsInfinityAbility === '1') {
      return this.holdingTime
    } else {
      if (this.constructor.name === 'TaskTimeRange') {
        //@ts-ignore
        const diff = this.dEndTime.diff(this.dBegTime)
        if (diff.as('seconds') > 0) {
          return Math.floor(diff.as('seconds'))
        }
        return this.holdingTime
      } else if (this.holdingTime - this.AllottedTime >= 0) {
        return this.holdingTime - this.AllottedTime
      } else if (this.holdingTime - this.AllottedTime <= 30) {
        return 0
      } else {
        throw new Error(
          '出错位置：排程时取时段内可用时间出错TimeSchTask.AvailableTime！',
        )
      }
    }
  }

  get NotWorkTime(): number {
    return this.AvailableTime
  }

  set NotWorkTime(value: number) {
    // Empty setter
  }

  get MaintainTime(): number {
    if (this.constructor.name === 'TaskTimeRange') {
      if (((this as unknown) as TaskTimeRange).cTaskType === 2) {
        return this.holdingTime
      } else {
        return 0
      }
    } else {
      this.maintainTime = 0
      for (const taskTimeRange of this.taskTimeRangeList.filter(
        (p1) => p1.cTaskType === 2,
      )) {
        this.maintainTime += taskTimeRange.HoldingTime
      }
      return this.maintainTime
    }
  }

  private attribute: any

  get Attribute(): any {
    return this.attribute
  }

  set Attribute(value: any) {
    //
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

  public state: number
  public loadRate: number

  public compareTo(obj: any): number {
    if (obj instanceof ResTimeRange) {
      const newTimeRange = obj as ResTimeRange
      if (
        //@ts-ignore
        this.dBegTime.equals(newTimeRange.dBegTime) &&
        //@ts-ignore
        this.dEndTime.equals(newTimeRange.dEndTime)
      ) {
        return 1
      } else {
        return -1
      }
    } else {
      throw new Error('对象非TimeRange类型')
    }
  }

  public clone(): this {
    return Object.create(this)
  }
}
