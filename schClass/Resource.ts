//@ts-nocheck
import { Base } from './Base'
import { gsDecoration } from './Util'
import { WorkCenter } from './WorkCenter'
type Context = { name: string }
// export class Resource extends Base {
//   iResourceID: number = 0
//   cResourceNo: string = ''
//   cResourceName: string = ''
//   cResClsNo: string = ''
//   cResourceType: string = '' // 0 主资源, 1 辅资源
//   @gsDecoration({
//     get: function () {
//       let _this: Resource = this
//       let SchParam = _this.schParam
//       if (SchParam.cSchCapType === '1') {
//         return Math.max(this.IResourceNumber, this.iOverResourceNumber)
//       } else if (SchParam.cSchCapType === '2') {
//         return Math.max(this.IResourceNumber, this.iLimitResourceNumber)
//       }
//       return _this.IResourceNumber
//     },
//   })
//   IResourceNumber: number = 0
//   cResOccupyType: string = '' // 0 整体, 1 单人单台
//   iPreStocks: number = 0
//   iPostStocks: number = 0
//   iUsage: number = 0
//   IEfficient: number = 0
//   cResouceInformation: string = ''
//   cIsInfinityAbility: string = '' // 0 产能有限, 1 产能无限
//   bScheduled: number = 0
//   iOverResourceNumber: number = 0
//   iLimitResourceNumber: number = 0
//   iOverEfficient: number = 0
//   iLimitEfficient: number = 0
//   iResDifficulty: number = 0
//   iDistributionRate: number = 0
//   cWcNo: string = ''
//   @gsDecoration({})
//   @gsDecoration({
//     get: function (this: Resource) {
//       let _this = this
//       let cWcNo = _this.cWcNo
//       let schData =
//         _this.schData?.WorkCenterList.filter((wc) => {
//           return wc.cWcNo === cWcNo //
//         }) || []
//       let tw = schData[0] || null
//       return tw //
//     }, ////
//   })
//   resWorkCenter: WorkCenter | null = null //
//   getResWorkCenter: () => WorkCenter
//   //   get ResWorkCenter(): WorkCenter | null {
//   //     if (!this.cWcNo || !this.schData) {
//   //       return null
//   //     }

//   //     const workCenters = this.schData.WorkCenterList.filter(
//   //       (wc) => wc.cWcNo === this.cWcNo,
//   //     )

//   //     return workCenters.length > 0 ? workCenters[0] : null
//   //   }
//   //   get iResourceNumber(): number {
//   //     if (SchParam.cSchCapType === '1') {
//   //       return Math.max(this.IResourceNumber, this.iOverResourceNumber)
//   //     } else if (SchParam.cSchCapType === '2') {
//   //       return Math.max(this.IResourceNumber, this.iLimitResourceNumber)
//   //     }
//   //     return this.IResourceNumber
//   //   }

//   //   set iResourceNumber(value: number) {
//   //     this.IResourceNumber = value
//   //   }
//   @gsDecoration({
//     get: function () {
//       //   let _this: Resource = this
//       //   if (this.IEfficient <= 0) {
//       //     this.IEfficient = 100
//       //   }
//       //   if (SchParam.cSchCapType === '1') {
//       //     return Math.max(this.IEfficient, this.iOverEfficient)
//       //   } else if (SchParam.cSchCapType === '2') {
//       //     return Math.max(this.IEfficient, this.iLimitEfficient)
//       //   }
//       //   return this.IEfficient
//     },
//   })
//   iEfficient: number
//   getIEfficient: () => number
//   setIDfficient: (...args) => void //
//   //   get iEfficient(): number {
//   //     if (this.IEfficient <= 0) {
//   //       this.IEfficient = 100
//   //     }
//   //     if (SchParam.cSchCapType === '1') {
//   //       return Math.max(this.IEfficient, this.iOverEfficient)
//   //     } else if (SchParam.cSchCapType === '2') {
//   //       return Math.max(this.IEfficient, this.iLimitEfficient)
//   //     }
//   //     return this.IEfficient
//   //   }

//   //   set iEfficient(value: number) {
//   //     this.IEfficient = value
//   //   }

//   // constructor();
//   // constructor(drResource: DataRow);
//   // constructor(cResourceNo: string, schData: SchData);
//   // constructor(arg1?: any, arg2?: SchData) {
//   //     if (arg1 && typeof arg1 === 'object') {
//   //         this.GetResource(arg1);
//   //     } else if (typeof arg1 === 'string' && arg2) {
//   //         this.schData = arg2;
//   //         const rows = this.schData.dtResource.select(
//   //             `cResourceNo = '${arg1}'`
//   //         );
//   //         if (rows.length > 0) {
//   //             this.GetResource(rows[0]);
//   //         }
//   //     }
//   // }

//   // GetResource(drResource: DataRow): void {
//   //     this.iResourceID = drResource['iResourceID'];
//   //     this.cResourceNo = drResource['cResourceNo'];
//   //     this.cResourceName = drResource['cResourceName'];
//   //     this.cResClsNo = drResource['cResClsNo'];
//   //     this.cResourceType = drResource['cResourceType'];
//   //     this.IResourceNumber = drResource['iResourceNumber'];
//   //     this.cResOccupyType = drResource['cResOccupyType'];
//   //     this.iPreStocks = drResource['iPreStocks'];
//   //     this.iPostStocks = drResource['iPostStocks'];
//   //     this.iUsage = drResource['iUsage'];
//   //     this.cResouceInformation = drResource['cResouceInformation'];
//   //     this.cIsInfinityAbility = drResource['cIsInfinityAbility'];
//   //     this.bScheduled = drResource['bScheduled'];
//   //     this.iOverResourceNumber = drResource['iOverResourceNumber'];
//   //     this.iLimitResourceNumber = drResource['iLimitResourceNumber'];
//   //     this.iOverEfficient = drResource['iOverEfficient'];
//   //     this.iLimitEfficient = drResource['iLimitEfficient'];
//   //     this.iResDifficulty = drResource['iResDifficulty'];
//   //     this.iDistributionRate = drResource['iDistributionRate'];
//   //     this.cWcNo = drResource['cWcNo'];
//   // }
// }
class ResTimeRange {
  // Define the properties and methods of ResTimeRange if needed
  WorkTimeRangeList: TaskTimeRange[]
}

class ResSourceDayCap {
  // Define the properties and methods of ResSourceDayCap if needed
}

class SchProductRouteRes {
  // Define the properties and methods of SchProductRouteRes if needed
}

class TaskTimeRange {
  DBegTime: Date
  // Define other properties and methods of TaskTimeRange if needed
}
// 示例全局对象
export class Resource extends Base {
  // Resource属性定义
  schData: any = null // 所有排程数据

  iResourceID: number // 资源物料排产顺序
  cResourceNo: string // 资源编号
  cResourceName: string
  cResClsNo: string // 资源类别
  cResourceType: string // 0 主资源 1 辅资源
  IResourceNumber: number // 资源数量
  cResOccupyType: string // 0 整体 1 单人单台, 单件工时/资源数量
  iPreStocks: number // 每批换产时间(分)
  iPostStocks: number // 每批维修时间(分)
  iUsage: number // 资源使用率
  iEfficient: number
  IEfficient: number // 资源效率 不能为0
  cResouceInformation: string // 资源简称
  cIsInfinityAbility: string // 0 产能有限 1 产能无限
  bScheduled: number = 0 // 任务是否已排产 0 未排，1 已排
  iOverResourceNumber: number // 加班资源数 对应排程产能方案为加班
  iLimitResourceNumber: number // 极限资源数 对应排程产能方案为极限
  iOverEfficient: number // 加班效率 对应排程产能方案为加班
  iLimitEfficient: number // 极限效率 对应排程产能方案为极限
  iResDifficulty: number // 资源加工难度系数 资源组时, 可以设置产能区分
  iDistributionRate: number // 资源选择比例 资源组时, 可以设置资源选择比例，比如本厂必选，100，委外厂商10%，20%等，对于已下达的任务，不能重新选择
  cWcNo: string // 工作中心
  resWorkCenter: WorkCenter | null = null
  cDeptNo: string // 生产部门
  cDeptName: string
  cStatus: string // 资源状态
  iSchemeID: number // 日历ID
  iCacheTime: number // 资源缓冲时间（分钟）
  iLastBatchPercent: number // 最后一批分批百分比，不超过，则作为一批处理
  cIsKey: string // 关键资源
  iKeySchSN: number // 关键排产顺序
  cNeedChanged: string // 维修单无换产时间
  dMaxExeDate: Date // 最大可排时间, 最当前最大开工工序完工时间
  cProChaType1Sort: string // 关键资源是否 1 按资源物料排产排序，进行优化生产，0 否则按工艺特征排序进行优化
  cDayPlanShowType: string // 关联分组号，分组号相同或包含的，前后工序间关联分组号相同，资源才可以选择参与排产
  iChangeTime: number // 换料时间(秒)
  iResPreTime: number // 前准备时间(秒), 生成产品工艺模型时，如果工艺路线没有定义，取资源档案
  iTurnsType: string // 轮换类型  0 不轮换 1 按加工时间轮换 2 按任务数轮换
  iTurnsTime: number // 轮换时间/任务数(分)

  cTeamNo: string // 班组1
  cTeamNo2: string // 班组2
  cTeamNo3: string // 班组3
  cBatch1Filter: string // 批次1过滤
  cBatch2Filter: string // 批次2过滤
  cBatch3Filter: string // 批次3过滤
  cBatch4Filter: string // 批次4过滤
  cBatch5Filter: string // 批次5过滤
  iBatchWoSeqID: number // 批次工序号
  cBatch1WorkTime: number // 批次1加工时间
  cBatch2WorkTime: number // 批次2加工时间
  cBatch3WorkTime: number // 批次3加工时间
  cBatch4WorkTime: number // 批次4加工时间
  cBatch5WorkTime: number // 批次5加工时间
  cPriorityType: string // 排产优先级
  cResBarCode: string // 资源条码号
  cTeamResourceNo: string // 资源组编码
  bTeamResource: string // 是否资源组 0 设备 1 资源组
  cSuppliyMode: string // 供料方式 0 独自供料 1 资源组集中供料
  cResOperator: string // 资源操作员
  cResManager: string // 资源管理员
  TeamResourceList: Resource[] = [] // 资源组列表
  TeamResource: Resource // 资源组
  bAllocated: string = '0' // "1" 资源组在同一个任务中已分配 , "0" 未分配

  iResWorkersPd: number // 资源日排产工人数
  iResHoursPd: number // 资源日排产工时
  iResOverHoursPd: number // 资源日加班工时

  iLabRate: number // 人工费率
  iPowerRate: number // 电费费率
  iOtherRate: number // 其他费率
  iMinWorkTime: number // 最小加工分拆时间，大于此时间时，拆分多个资源生产

  FProChaType1ID: string // 资源工艺特征类型1
  FProChaType2ID: string // 资源工艺特征类型2
  FProChaType3ID: string
  FProChaType4ID: string
  FProChaType5ID: string
  FProChaType6ID: string
  cDefine1: string
  cDefine2: string
  cDefine3: string
  cDefine4: string
  cDefine5: string
  cDefine6: string
  cDefine7: string
  cDefine8: string
  cDefine9: string
  cDefine10: string
  cDefine11: number
  cDefine12: number
  cDefine13: number
  cDefine14: number
  cDefine15: Date
  cDefine16: Date

  iBatch: number = 2 // 当前批量生产批号, 从2开始, 当前正在加工的为1批
  dBatchBegDate: Date // 批量加工开始时间
  dBatchEndDate: Date // 批量加工完工时间，下批排产时只能从此时间往后排

  cTimeNote: string = '' // 资源换产时间说明
  iSchBatch: number = 6 // 排产批次, 批次换时，重新写值
  cSelected: number = 1 // 是否选择参与排产 1 ,0 不参与优化排产

  iSchHours: number = 0 // 已排任务工时
  iPlanDays: number = 0
  get ResWorkCenter(): WorkCenter | null {
    if (this.cWcNo === '') {
      this.resWorkCenter = null
    } else {
      const ListWorkCenter = this.schData.WorkCenterList.filter(
        (p1: WorkCenter) => p1.cWcNo === this.cWcNo,
      )

      if (ListWorkCenter.length > 0) {
        this.resWorkCenter = ListWorkCenter[0]
      } else {
        this.resWorkCenter = null
      }
    }

    return this.resWorkCenter
  }
  get iResourceNumber(): number {
    const SchParam = this.schParam
    if (SchParam.cSchCapType === '1') {
      // 加班资源数
      return this.IResourceNumber > this.iOverResourceNumber
        ? this.IResourceNumber
        : this.iOverResourceNumber
    } else if (SchParam.cSchCapType === '2') {
      // 极限资源数
      return this.IResourceNumber > this.iLimitResourceNumber
        ? this.IResourceNumber
        : this.iLimitResourceNumber
    } else {
      return this.IResourceNumber
    }
  }

  set iResourceNumber(value: number) {
    this.IResourceNumber = value
  }
  constructor(cResourceNo: string) {
    super()
    // The SQL query and the DataTable fetching logic have been commented out in the original code,
    // so this part is not directly translated.
    const dr = this.schData.dtResource.filter(
      (row: any) => row.cResourceNo === cResourceNo,
    )
    if (dr.length < 1) return

    this.getResource(dr[0])
  }
  getResource(drResource: any) {
    let SchParam = this.schParam
    this.iResourceID = <number>drResource['iResourceID']
    this.cResourceNo = <string>drResource['cResourceNo']
    this.cResourceName = <string>drResource['cResourceName']
    this.cResClsNo = <string>drResource['cResClsNo']
    this.cResourceType = <string>drResource['cResourceType']
    this.iResourceNumber = <number>drResource['iResourceNumber']
    this.cResOccupyType = <string>drResource['cResOccupyType']
    this.iPreStocks = <number>drResource['iPreStocks']
    this.iPostStocks = <number>drResource['iPostStocks']
    this.iUsage = <number>drResource['iUsage']
    this.iEfficient = <number>drResource['iEfficient']
    this.iResDifficulty = <number>drResource['iResDifficulty']
    this.iDistributionRate = <number>drResource['iDistributionRate']

    if (this.iEfficient === 0) this.iEfficient = 100

    this.cIsInfinityAbility = <string>drResource['cIsInfinityAbility']
    if (this.cIsInfinityAbility === '') this.cIsInfinityAbility = '0'

    this.cWcNo = <string>drResource['cWcNo']
    this.cDeptNo = <string>drResource['cDeptNo']
    this.cDeptName = <string>drResource['cDeptName']
    this.cStatus = <string>drResource['cStatus']
    this.iSchemeID = <number>drResource['iResourceNumber']
    this.iCacheTime = <number>drResource['iCacheTime']
    this.iLastBatchPercent = <number>drResource['iLastBatchPercent']
    this.cIsKey = <string>drResource['cIsKey']
    this.iKeySchSN = <number>drResource['iKeySchSN']
    this.cNeedChanged = <string>drResource['cNeedChanged']
    this.iChangeTime = <number>drResource['iChangeTime'] * 60
    this.iResPreTime = <number>drResource['iResPreTime'] * 60
    this.iTurnsType = <string>drResource['iTurnsType']
    this.iTurnsTime = <number>drResource['iTurnsTime'] * 60
    this.iLabRate = <number>drResource['iLastBatchPercent']
    this.cTeamNo = <string>drResource['cTeamNo']
    this.cTeamNo2 = <string>drResource['cTeamNo2']
    this.cTeamNo3 = <string>drResource['cTeamNo3']
    this.cBatch1Filter = <string>drResource['cBatch1Filter']
    this.cBatch2Filter = <string>drResource['cBatch2Filter']
    this.cBatch3Filter = <string>drResource['cBatch3Filter']
    this.cBatch4Filter = <string>drResource['cBatch4Filter']
    this.cBatch5Filter = <string>drResource['cBatch5Filter']
    this.iBatchWoSeqID = <number>drResource['iBatchWoSeqID']
    this.cBatch1WorkTime = <number>drResource['cBatch1WorkTime']
    this.cBatch2WorkTime = <number>drResource['cBatch2WorkTime']
    this.cBatch3WorkTime = <number>drResource['cBatch3WorkTime']
    this.cBatch4WorkTime = <number>drResource['cBatch4WorkTime']
    this.cBatch5WorkTime = <number>drResource['cBatch5WorkTime']
    this.cPriorityType = <string>drResource['cPriorityType']
    this.cResBarCode = <string>drResource['cResBarCode']
    this.cTeamResourceNo = <string>drResource['cTeamResourceNo']
    this.bTeamResource = <string>drResource['bTeamResource']
    this.cSuppliyMode = <string>drResource['cSuppliyMode']
    this.cResOperator = <string>drResource['cResOperator']
    this.cResManager = <string>drResource['cResManager']
    this.iOverResourceNumber = <number>drResource['iOverResourceNumber']
    this.iLimitResourceNumber = <number>drResource['iLimitResourceNumber']
    this.iOverEfficient = <number>drResource['iOverEfficient']
    this.iLimitEfficient = <number>drResource['iLimitEfficient']

    if (this.iOverEfficient === 0) this.iOverEfficient = 100
    if (this.iLimitEfficient === 0) this.iLimitEfficient = 100

    this.iResWorkersPd = <number>drResource['iResWorkersPd']
    this.iResHoursPd = <number>drResource['iResHoursPd']
    this.iResOverHoursPd = <number>drResource['iResOverHoursPd']
    this.iPowerRate = <number>drResource['iPowerRate']
    this.iOtherRate = <number>drResource['iOtherRate']
    this.iMinWorkTime = <number>drResource['iMinWorkTime']

    if (this.iMinWorkTime < 1) this.iMinWorkTime = SchParam.iTaskMinWorkTime

    this.cProChaType1Sort = <string>drResource['cProChaType1Sort']
    this.FProChaType1ID = <string>drResource['FProChaType1ID']
    this.FProChaType2ID = <string>drResource['FProChaType2ID']
    this.FProChaType3ID = <string>drResource['FProChaType3ID']
    this.FProChaType4ID = <string>drResource['FProChaType4ID']
    this.FProChaType5ID = <string>drResource['FProChaType5ID']
    this.FProChaType6ID = <string>drResource['FProChaType6ID']
    this.cDefine1 = <string>drResource['cResDefine1']
    this.cDefine2 = <string>drResource['cResDefine2']
    this.cDefine3 = <string>drResource['cResDefine3']
    this.cDefine4 = <string>drResource['cResDefine4']
    this.cDefine5 = <string>drResource['cResDefine5']
    this.cDefine6 = <string>drResource['cResDefine6']
    this.cDefine7 = <string>drResource['cResDefine7']
    this.cDefine8 = <string>drResource['cResDefine8']
    this.cDefine9 = <string>drResource['cResDefine9']
    this.cDefine10 = <string>drResource['cResDefine10']
    this.cDefine11 = <number>drResource['cResDefine11']
    this.cDefine12 = <number>drResource['cResDefine12']
    this.cDefine13 = <number>drResource['cResDefine13']
    this.cDefine14 = <number>drResource['cResDefine14']
    this.cDefine15 = <Date>drResource['cResDefine15']
    this.cDefine16 = <Date>drResource['cResDefine16']
    this.cDayPlanShowType = <string>drResource['cDayPlanShowType']
    this.dMaxExeDate = <Date>drResource['dMaxExeDate']
  }
  ResTimeRangeList: ResTimeRange[] = new Array(10)
  ResSourceDayCapList: ResSourceDayCap[] = new Array(10)
  ResTimeRangeListBak: ResTimeRange[] = new Array(10)
  ResSpecTimeRangeList: ResTimeRange[] = new Array(10)
  schProductRouteResList: SchProductRouteRes[] = new Array(10)

  // Method to get task time ranges, optionally sorted in ascending or descending order
  GetTaskTimeRangeList(
    dBegDate: Date,
    bSchRev: boolean = false,
    _OrderASC: boolean = true,
  ): TaskTimeRange[] {
    let OrderASC = dBegDate
    if (typeof _OrderASC === 'boolean') {
      OrderASC = dBegDate
    } else {
      OrderASC = _OrderASC //
    }
    if (typeof dBegDate === 'boolean') {
      let ListTaskTimeRangeAll: TaskTimeRange[] = []

      // Collecting work time ranges from each ResTimeRange in the list
      for (let ResTimeRange1 of this.ResTimeRangeList) {
        ListTaskTimeRangeAll.push(...ResTimeRange1.WorkTimeRangeList)
      }

      // Sorting the task time ranges based on the starting time (DBegTime)
      if (OrderASC) {
        ListTaskTimeRangeAll.sort(
          (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
        )
      } else {
        ListTaskTimeRangeAll.sort(
          (p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime(),
        )
      }

      return ListTaskTimeRangeAll
    } else {
      let ListTaskTimeRangeAll: TaskTimeRange[] = []

      if (!bSchRev) {
        //正排,时段结束时间>= dBegDate
        for (let ResTimeRange1 of this.ResTimeRangeList.filter(
          (p1) => p1.DEndTime >= dBegDate,
        )) {
          if (ResTimeRange1.DEndTime >= dBegDate) {
            if (ResTimeRange1.WorkTimeRangeList.length > 0) {
              ListTaskTimeRangeAll.push(...ResTimeRange1.WorkTimeRangeList)
              break
            }
          }
        }
      } else {
        //倒排 时段开始时间<= dBegDate
        for (let ResTimeRange1 of this.ResTimeRangeList.filter(
          (p1) => p1.DBegTime <= dBegDate,
        )) {
          if (ResTimeRange1.DBegTime <= dBegDate) {
            if (ResTimeRange1.WorkTimeRangeList.length > 0) {
              ListTaskTimeRangeAll.push(...ResTimeRange1.WorkTimeRangeList)
              break
            }
          }
        }
      }

      // Sorting
      ListTaskTimeRangeAll.sort((p1, p2) => {
        return OrderASC
          ? p1.DBegTime.getTime() - p2.DBegTime.getTime()
          : p2.DBegTime.getTime() - p1.DBegTime.getTime()
      })

      return ListTaskTimeRangeAll
    }
  }

  // Method to get unassigned tasks, sorted by process features
  GetNotSchTask(): SchProductRouteRes[] {
    // 1. Key resources scheduling: sort production tasks by process features
    this.schProductRouteResList = this.schProductRouteResList.filter(
      (p1) =>
        p1.cResourceNo === this.cResourceNo &&
        p1.BScheduled === 0 &&
        p1.iSchBatch === this.iSchBatch,
    )

    // Sorting by process features or material scheduling order
    this.schProductRouteResList.sort((p1, p2) => this.TaskComparer(p1, p2))

    return this.schProductRouteResList
  }

  // Define the TaskComparer method here, if needed
  TaskComparer(p1: SchProductRouteRes, p2: SchProductRouteRes): number {
    // Define comparison logic based on process features
    return 0 // Example return value, modify as needed
  }
  public GetNotSchTask(): SchProductRouteRes[] {
    // 1. Key resource scheduling: Sort production tasks by process characteristics
    this.schProductRouteResList = this.schData.SchProductRouteResList.filter(
      (p1: SchProductRouteRes) =>
        p1.cResourceNo === this.cResourceNo &&
        p1.BScheduled === 0 &&
        p1.iSchBatch === this.iSchBatch,
    )

    // Sort by process characteristics optimization or material scheduling order
    this.schProductRouteResList.sort(
      (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
        this.TaskComparer(p1, p2),
    )

    return this.schProductRouteResList
  }

  // MergeTimeRange method
  public MergeTimeRange(): void {
    // 1. First process overtime situation
    // Find all overtime time ranges for the current resource
    let ResSpecTimeRangeList1 = this.ResSpecTimeRangeList.filter(
      (p1: ResTimeRange) =>
        p1.Attribute === TimeRangeAttribute.Work ||
        p1.Attribute === TimeRangeAttribute.Overtime ||
        p1.Attribute === TimeRangeAttribute.MayOvertime,
    )

    ResSpecTimeRangeList1.sort(
      (p1: ResTimeRange, p2: ResTimeRange) =>
        p1.DBegTime.getTime() - p2.DBegTime.getTime(),
    )

    let dCanBegDate = this.schData.dtStart
    let dCanEndDate = this.schData.dtEnd

    // Special resource work calendar for overtime
    ResSpecTimeRangeList1.forEach((resTimeRange: ResTimeRange) => {
      let lResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p2: ResTimeRange) => p2.DBegTime <= resTimeRange.DBegTime,
      )
      lResTimeRangeList1.sort(
        (p1: ResTimeRange, p2: ResTimeRange) =>
          p2.DBegTime.getTime() - p1.DBegTime.getTime(),
      )

      if (lResTimeRangeList1.length > 0) {
        dCanBegDate = lResTimeRangeList1[0].DBegTime // Get the largest time period that starts before the special time range
      }

      let lResTimeRangeList = this.ResTimeRangeList.filter(
        (p2: ResTimeRange) => p2.DBegTime >= dCanBegDate,
      )
      lResTimeRangeList.sort(
        (p1: ResTimeRange, p2: ResTimeRange) =>
          p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )

      let resLastTimeRange: ResTimeRange | null = null

      for (let i = 0; i < lResTimeRangeList.length; i++) {
        const resWorkTimeRange = lResTimeRangeList[i]

        if (i > 0) {
          // From the second segment, create an idle time range
          let resAddTimeRange = new ResTimeRange()

          // Determine the start time for the new time range
          resAddTimeRange.DBegTime =
            resTimeRange.DBegTime < resLastTimeRange!.DEndTime
              ? resLastTimeRange!.DEndTime
              : resTimeRange.DBegTime

          // Determine the end time for the new time range
          if (resTimeRange.DEndTime > resWorkTimeRange.DBegTime) {
            resAddTimeRange.DEndTime = resWorkTimeRange.DBegTime
          } else if (resTimeRange.DEndTime > resAddTimeRange.DBegTime) {
            resAddTimeRange.DEndTime = resTimeRange.DEndTime
          } else {
            break
          }

          resAddTimeRange.CResourceNo = this.cResourceNo
          resAddTimeRange.resource = this
          resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility
          resAddTimeRange.Attribute = resTimeRange.Attribute
          resAddTimeRange.GetNoWorkTaskTimeRange(
            resAddTimeRange.DBegTime,
            resAddTimeRange.DEndTime,
            true,
          )

          // Add the newly created idle time range
          this.ResTimeRangeList.push(resAddTimeRange)
        }

        resLastTimeRange = resWorkTimeRange
      }
    })

    // 2. Process maintenance time
    // Find all maintenance time ranges for the current resource
    let ResSpecTimeRangeList2 = this.ResSpecTimeRangeList.filter(
      (p1: ResTimeRange) =>
        p1.Attribute === TimeRangeAttribute.Maintain ||
        p1.Attribute === TimeRangeAttribute.Snag,
    )

    ResSpecTimeRangeList2.sort(
      (p1: ResTimeRange, p2: ResTimeRange) =>
        p1.DBegTime.getTime() - p2.DBegTime.getTime(),
    )

    ResSpecTimeRangeList2.forEach((resTimeRange: ResTimeRange) => {
      let lResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p2: ResTimeRange) => p2.DBegTime <= resTimeRange.DBegTime,
      )
      lResTimeRangeList1.sort(
        (p1: ResTimeRange, p2: ResTimeRange) =>
          p2.DBegTime.getTime() - p1.DBegTime.getTime(),
      )

      if (lResTimeRangeList1.length > 0) {
        dCanBegDate = lResTimeRangeList1[0].DBegTime
      } else {
        dCanBegDate = this.schData.dtStart
      }

      let lResTimeRangeList = this.ResTimeRangeList.filter(
        (p2: ResTimeRange) => p2.DBegTime >= dCanBegDate,
      )
      lResTimeRangeList.sort(
        (p1: ResTimeRange, p2: ResTimeRange) =>
          p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )

      let resLastTimeRange: ResTimeRange | null = null

      for (let i = 0; i < lResTimeRangeList.length; i++) {
        const resWorkTimeRange = lResTimeRangeList[i]

        if (resTimeRange.DEndTime >= resWorkTimeRange.DBegTime) {
          // Exclude the work time range from the current task
          this.DeleteTimeRangeSub(resWorkTimeRange, resTimeRange)
        } else {
          break
        }
      }
    })
  }

  // Additional methods used (TaskComparer, DeleteTimeRangeSub, etc.)
  private TaskComparer(p1: SchProductRouteRes, p2: SchProductRouteRes): number {
    // Implement comparison logic here
    return 0 // Replace with actual comparison logic
  }

  private DeleteTimeRangeSub(
    resWorkTimeRange: ResTimeRange,
    resTimeRange: ResTimeRange,
  ): void {
    // Implement the logic to delete a time range here
  }
  public mergeTimeRangeSub(
    resWorkTimeRange: ResTimeRange,
    resSpecTimeRange: ResTimeRange,
  ): void {
    const resAddTimeRange = new ResTimeRange()
    resAddTimeRange.CResourceNo = this.cResourceNo
    resAddTimeRange.resource = this
    resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility
    resAddTimeRange.Attribute = resSpecTimeRange.Attribute

    if (resWorkTimeRange === null) {
      // Add a new time range
      resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime
      resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
    } else {
      // Overtime overlaps with the beginning of the work time
      if (
        resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime &&
        resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime
      ) {
        resAddTimeRange.DBegTime = resWorkTimeRange.DEndTime
        resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
      }
      // Overtime overlaps with the end of the work time
      else if (
        resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime &&
        resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime
      ) {
        resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime
        resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
      }
      // Overtime does not overlap with work time, outside work time
      else if (
        resWorkTimeRange.DEndTime < resSpecTimeRange.DBegTime ||
        resWorkTimeRange.DBegTime > resSpecTimeRange.DEndTime
      ) {
        resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime
        resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
      }
      // Completely overlapping, no need to add time range
      else {
        resAddTimeRange = null
      }
    }

    // Add the work time range to the list
    if (resAddTimeRange !== null) {
      // Generate an empty task range
      resAddTimeRange.getNoWorkTaskTimeRange(
        resAddTimeRange.DBegTime,
        resAddTimeRange.DEndTime,
        false,
      )
      this.ResTimeRangeList.push(resAddTimeRange)
    }
  }

  // Add a time range after the last time range
  public addTimeRange(
    resLastTimeRange: ResTimeRange,
    resWorkTimeRange: ResTimeRange,
  ): void {
    const resAddTimeRange = new ResTimeRange()
    resAddTimeRange.CResourceNo = this.cResourceNo
    resAddTimeRange.resource = this
    resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility
    resAddTimeRange.DBegTime = resLastTimeRange.DEndTime
    resAddTimeRange.DEndTime = resLastTimeRange.DBegTime

    // Generate an empty task range
    resAddTimeRange.getNoWorkTaskTimeRange(
      resAddTimeRange.DBegTime,
      resAddTimeRange.DEndTime,
      false,
    )
    this.ResTimeRangeList.push(resAddTimeRange)
  }

  // Exclude a work time range and set it as unavailable
  public deleteTimeRangeSub(
    resWorkTimeRange: ResTimeRange,
    resSpecTimeRange: ResTimeRange,
  ): void {
    const resAddTimeRange = new TaskTimeRange()

    if (resSpecTimeRange.DBegTime < resWorkTimeRange.DBegTime) {
      resAddTimeRange.DBegTime = resWorkTimeRange.DBegTime
    } else if (resSpecTimeRange.DBegTime < resWorkTimeRange.DEndTime) {
      resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime // Start time of the new added range
    } else {
      return
    }

    if (resSpecTimeRange.DEndTime > resWorkTimeRange.DEndTime) {
      resAddTimeRange.DEndTime = resWorkTimeRange.DEndTime
    } else if (resSpecTimeRange.DEndTime > resAddTimeRange.DBegTime) {
      resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
    } else {
      return
    }

    if (resAddTimeRange !== null) {
      // Split the current work time range and remove the maintenance time
      if (resWorkTimeRange.TaskTimeRangeList.length > 0) {
        resAddTimeRange = resWorkTimeRange.getNoWorkTaskTimeRange(
          resAddTimeRange.DBegTime,
          resAddTimeRange.DEndTime,
          false,
        )
        resAddTimeRange.AllottedTime = resAddTimeRange.HoldingTime
        resAddTimeRange.Attribute = resSpecTimeRange.Attribute
        resAddTimeRange.cTaskType = 2 // 0 Free, 1 Work, 2 Maintenance

        resWorkTimeRange.taskTimeRangeSplit(
          resWorkTimeRange.TaskTimeRangeList[0],
          resAddTimeRange,
        )
      }
    }
  }
  public getResSourceDayCapList(): void {
    this.ResSourceDayCapList = new Array<ResSourceDayCap>(10)

    let resSourceDayCap: ResSourceDayCap = new ResSourceDayCap()
    let ldt_todayLast: Date = new Date()

    // Group by resource number and day
    const groupedResTimeRange = this.ResTimeRangeList.reduce(
      (groups, resTimeRange) => {
        const key = `${resTimeRange.cResourceNo}_${resTimeRange.dPeriodDay}`
        if (!groups[key]) {
          groups[key] = []
        }
        groups[key].push(resTimeRange)
        return groups
      },
      {},
    )

    // Sort by start time, from earliest to latest
    this.ResTimeRangeList.sort(
      (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
    )

    this.ResTimeRangeList.forEach((ResTimeRange1) => {
      // Only generate one record per resource per day
      if (ldt_todayLast.getTime() !== ResTimeRange1.dPeriodDay.getTime()) {
        resSourceDayCap = new ResSourceDayCap()
        resSourceDayCap.dPeriodDay = ResTimeRange1.dPeriodDay
        resSourceDayCap.DBegTime = ResTimeRange1.DBegTime
        this.ResSourceDayCapList.push(resSourceDayCap)
      }

      resSourceDayCap.ResTimeRangeList.push(ResTimeRange1)
      ResTimeRange1.resSourceDayCap = resSourceDayCap

      ldt_todayLast = ResTimeRange1.dPeriodDay
    })
  }

  public SchTaskFreezeInit(
    as_SchProductRouteRes: SchProductRouteRes,
    adCanBegDate: Date,
    adCanEndDate: Date,
  ): number {
    try {
      if (as_SchProductRouteRes.BScheduled === 1) return 0 // Task already scheduled, no action

      // Find all available time ranges with AvailableTime > 0 and end time >= adCanBegDate
      let ResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime >= adCanBegDate,
      )

      // Sort by start time
      ResTimeRangeList1.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )

      // Loop through all available time ranges and schedule tasks
      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (ResTimeRangeList1[i].DBegTime > adCanEndDate) break
        // Call the resource time range scheduling
        ResTimeRangeList1[i].TimeSchTaskFreezeInit(
          as_SchProductRouteRes,
          adCanBegDate,
          adCanEndDate,
        )
      }

      // If remaining task work time is greater than 0, the task hasn't been scheduled fully
      as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++ // Increment scheduling number
      as_SchProductRouteRes.BScheduled = 1 // Set as scheduled
      as_SchProductRouteRes.schProductRoute.BScheduled = 1 // Set product route as scheduled

      // Log scheduling information
      if (SchParam.APSDebug === '1') {
        const message2 = `3、Schedule sequence [${
          as_SchProductRouteRes.iSchSN
        }], Resource number [${
          as_SchProductRouteRes.cResourceNo
        }], Material number [${
          as_SchProductRouteRes.cInvCode
        }], Sequence number [${
          as_SchProductRouteRes.schProductRoute.schProduct.cSchSNType
        }], Sequence order [${
          as_SchProductRouteRes.schProductRoute.schProduct.iSchSN
        }], Task priority [${
          as_SchProductRouteRes.iPriorityRes
        }], Order priority [${
          as_SchProductRouteRes.schProductRoute.schProduct.iPriority
        }], Process [${
          as_SchProductRouteRes.iWoSeqID + as_SchProductRouteRes.cSeqNote
        }], Batch [${as_SchProductRouteRes.iSchBatch}], Work order number [${
          as_SchProductRouteRes.cWoNo
        }]`

        console.log(message2)
      }
    } catch (error) {
      throw new Error(
        `Error scheduling task for product route, process ID: ${as_SchProductRouteRes.iProcessProductID} - ${error.message}`,
      )
      return -1
    }

    return 1
  }
  public SchTaskSortInit(
    as_SchProductRouteRes: SchProductRouteRes,
    adCanBegDate: Date,
    adCanEndDate: Date,
  ): number {
    // First, process the tasks before
    as_SchProductRouteRes.schProductRoute.ProcessSchTaskPre(false)

    // Recalculate work time, subtract completed portion
    let ai_workTime = 0
    let ai_ResReqQty =
      as_SchProductRouteRes.iResReqQty - as_SchProductRouteRes.iActResReqQty
    let iBatchCount = 0

    if (as_SchProductRouteRes.cWorkType === '1') {
      // Batch processing
      // If the batch is insufficient, add 1 batch
      if (
        ai_ResReqQty / as_SchProductRouteRes.iBatchQty <
        Math.floor(ai_ResReqQty / as_SchProductRouteRes.iBatchQty)
      ) {
        iBatchCount =
          Math.floor(ai_ResReqQty / as_SchProductRouteRes.iBatchQty) + 1
      } else {
        iBatchCount = Math.floor(ai_ResReqQty / as_SchProductRouteRes.iBatchQty)
      }
      if (iBatchCount < 1) iBatchCount = 1
      ai_workTime =
        iBatchCount * as_SchProductRouteRes.iCapacity +
        (iBatchCount - 1) * as_SchProductRouteRes.iBatchInterTime
    } else {
      // "0" single-piece processing
      ai_workTime = ai_ResReqQty * as_SchProductRouteRes.iCapacity
    }

    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate // Task can start date

    if (
      as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID
    ) {
      // Debugging breakpoint 1
      let i = 1
    }

    try {
      // 1.1 Return the available start date for scheduling
      let adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = ai_workTime
      let ai_disWorkTime = ai_workTime
      let ai_ResPreTime = 0 // Resource change time
      let ai_CycTimeTol = 0 // Tool change time
      let dtBegDate = adCanBegDate,
        dtEndDate = adCanBegDate

      // Return adCanBegDateTask, task start date, and adCanBegDateTest, task completion date
      let li_Return = this.TestResSchTask(
        as_SchProductRouteRes,
        ai_workTimeTest,
        adCanBegDateTest,
        adCanBegDateTask,
        false,
        ai_ResPreTime,
        ai_CycTimeTol,
        dtBegDate,
        dtEndDate,
      )
      if (li_Return < 0) {
        let cError = `Order line number: ${
          as_SchProductRouteRes.iSchSdID
        }, processing material [${
          as_SchProductRouteRes.cInvCode
        }] cannot be scheduled in resource [${
          as_SchProductRouteRes.cResourceNo
        }], task number [${
          as_SchProductRouteRes.iProcessProductID
        }], single-piece capacity [${
          as_SchProductRouteRes.iCapacity
        }], processing quantity [${
          as_SchProductRouteRes.iResReqQty
        }], work hours [${ai_workTimeTask / 3600}], unassigned work hours [${
          ai_workTime / 3600
        }], max assignable time [${adCanBegDateTest}], please check the work calendar or capacity/plan too large!`
        throw new Error(cError)
        return -1
      }

      adCanBegDate = adCanBegDateTask

      // 2.1 Find all available time ranges where AvailableTime > 0
      let ResTimeRangeList1 = ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime > adCanBegDate,
      )
      // Sort the list
      ResTimeRangeList1.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )

      let bFirtTime = true // Is it the first scheduling time slot?

      // 2.2 Iterate over available time slots and schedule production tasks
      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (ai_workTime === 0) break

        // Schedule tasks within the time slots
        ResTimeRangeList1[i].TimeSchTask(
          as_SchProductRouteRes,
          ai_workTime,
          adCanBegDate,
          ai_workTimeTask,
          adCanBegDateTask,
          true,
          ai_ResPreTime,
          ai_CycTimeTol,
          bFirtTime,
          ai_disWorkTime,
          false,
        )

        if (bFirtTime) {
          dtBegDate = ResTimeRangeList1[i].DBegTime
          as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime
          bFirtTime = false
        }
      }

      // 2.3 If work time remaining, task scheduling failed
      if (ai_workTime > 0) {
        let cError = `Order line number: ${
          as_SchProductRouteRes.iSchSdID
        }, processing material [${
          as_SchProductRouteRes.cInvCode
        }] cannot be scheduled in resource [${
          as_SchProductRouteRes.cResourceNo
        }], task number [${
          as_SchProductRouteRes.iProcessProductID
        }], single-piece capacity [${
          as_SchProductRouteRes.iCapacity
        }], processing quantity [${
          as_SchProductRouteRes.iResReqQty
        }], work hours [${ai_workTimeTask / 3600}], unassigned work hours [${
          ai_workTime / 3600
        }], max assignable time [${
          ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
        }], please check the work calendar or capacity/plan too large!`
        throw new Error(cError)
        return -1
      } else {
        as_SchProductRouteRes.iSchSN = SchParam.iSchSNMin--
        as_SchProductRouteRes.BScheduled = 1 // Set as scheduled
        as_SchProductRouteRes.schProductRoute.BScheduled = 1 // Set as scheduled
      }
    } catch (error) {
      throw new Error(
        `Order line number: ${as_SchProductRouteRes.iSchSdID} error while calculating resource schedule, position Resource.ResSchTask! Process ID: ${as_SchProductRouteRes.iProcessProductID}\n\r ${error.message}`,
      )
      return -1
    }

    return 1 // Remaining unassigned time
  }
  public ResSchBefore(): number {
    if (this.cResourceNo === 'YQ-17-07') {
      let j = 1
    }

    // If it’s a manual work order -2 or packaging subtask -1, directly schedule without batching
    if (this.iSchBatch < 0) {
      // Key resource optimization scheduling, without considering grouping and rotation
      this.KeyResSchTask()
      return -1 // No further scheduling for this batch
    }

    return 1
  }

  // Resource scheduling after processing
  public ResSchAfter(): number {
    return 1
  }

  // Resource dispatch scheduling optimization 2020-08-20 JonasCheng, formal version iSchBatch -100 full schedule
  public ResDispatchSch(iSchBatch: number): number {
    let SchProductRouteResPre: any = null
    let ListSchProductRouteRes: any[] = []
    let LastCanBegDate: Date = new Date()

    try {
      if (this.cResourceNo === '42001') {
        let j = 1
      }

      // First scheduling call, optimize scheduling by resource, batch optimization iSchBatch -100
      if (iSchBatch === -100) {
        ListSchProductRouteRes = this.schData.SchProductRouteResList.filter(
          (p1: any) => p1.cResourceNo === this.cResourceNo && p1.iResReqQty > 0,
        )

        if (ListSchProductRouteRes.length > 0) {
          // Set current task as unscheduled
          ListSchProductRouteRes.forEach((SchProductRouteResTemp: any) => {
            if (SchProductRouteResTemp.schProductRoute.cStatus === '4') {
              SchProductRouteResTemp.iSchSN = this.SchParam.iSchSNMax++
              SchProductRouteResTemp.BScheduled = 1
              SchProductRouteResTemp.schProductRoute.BScheduled = 1
            } else {
              SchProductRouteResTemp.cDefine25 = ''
              SchProductRouteResTemp.iResPreTime = 0
              SchProductRouteResTemp.SchProductRouteResPre = null
              SchProductRouteResTemp.BScheduled = 0
              SchProductRouteResTemp.schProductRoute.BScheduled = 0
              SchProductRouteResTemp.TaskClearTask()
            }
          })

          // Sort tasks by resource or other defined parameters
          ListSchProductRouteRes.sort((p1: any, p2: any) =>
            this.ResTaskComparer(p1, p2),
          )

          // Schedule tasks in order
          for (let i = 0; i < ListSchProductRouteRes.length; i++) {
            if (ListSchProductRouteRes[i].schProductRoute.BScheduled === 1) {
              SchProductRouteResPre = ListSchProductRouteRes[i]
              continue
            }

            ListSchProductRouteRes[
              i
            ].SchProductRouteResPre = SchProductRouteResPre
            ListSchProductRouteRes[i].DispatchSchTask(
              ListSchProductRouteRes[i].iResReqQty,
              LastCanBegDate,
              SchProductRouteResPre,
            )

            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate
            SchProductRouteResPre = ListSchProductRouteRes[i]
          }
        }
      } else if (iSchBatch === -200) {
        // Re-schedule only confirmed version tasks
        ListSchProductRouteRes = this.schData.SchProductRouteResList.filter(
          (p1: any) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.iResReqQty > 0 &&
            p1.cVersionNo.trim().toLowerCase() === 'sureversion',
        )

        if (ListSchProductRouteRes.length > 0) {
          ListSchProductRouteRes.sort((p1: any, p2: any) =>
            this.ResTaskComparer(p1, p2),
          )

          for (let i = 0; i < ListSchProductRouteRes.length; i++) {
            if (ListSchProductRouteRes[i].schProductRoute.BScheduled === 1) {
              SchProductRouteResPre = ListSchProductRouteRes[i]
              continue
            }

            ListSchProductRouteRes[
              i
            ].SchProductRouteResPre = SchProductRouteResPre

            // Consider the earliest available time for the previous step
            if (this.SchParam.ExecTaskSchType === '2') {
              LastCanBegDate = this.SchParam.dtStart
              LastCanBegDate = this.GetTaskCanBegDate(
                ListSchProductRouteRes[i],
                LastCanBegDate,
              )
            }

            ListSchProductRouteRes[i].DispatchSchTask(
              ListSchProductRouteRes[i].iResReqQty,
              LastCanBegDate,
              SchProductRouteResPre,
            )

            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate
            SchProductRouteResPre = ListSchProductRouteRes[i]
          }
        }
      }
    } catch (error) {
      console.error('Error during scheduling', error)
    }

    return 1
  }

  private ResTaskComparer(p1: any, p2: any): number {
    // Implement comparison logic here
    return 0
  }

  private GetTaskCanBegDate(task: any, lastCanBegDate: Date): Date {
    // Implement logic to calculate the earliest available start date
    return new Date()
  }
  public ResDispatchSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: Date,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    bReCalWorkTime: boolean = true,
  ): number {
    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate // Task can begin date, re-assigned when the task is interrupted
    let ldtBeginDate = new Date()
    let message = ''

    // Check if the schedule IDs match or resource IDs match
    if (
      (as_SchProductRouteRes.iSchSdID === SchParam.iSchSdID &&
        as_SchProductRouteRes.iProcessProductID ===
          SchParam.iProcessProductID) ||
      as_SchProductRouteRes.iResourceAbilityID === SchParam.iProcessProductID
    ) {
      let i = 1 // Debug breakpoint
    }

    if (
      (as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID &&
        as_SchProductRouteRes.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
      (as_SchProductRouteRes.iProcessProductID === 193864 &&
        as_SchProductRouteRes.schProductRoute.iSchSdID === 1070)
    ) {
      message = `3.1、排产顺序[${as_SchProductRouteRes.iSchSN}],计划ID[${
        as_SchProductRouteRes.iSchSdID
      }],任务ID[${as_SchProductRouteRes.iProcessProductID}],资源编号[${
        as_SchProductRouteRes.cResourceNo
      }],开始排产时间[${new Date().toLocaleString()}],完成排产时间[${SchData.GetDateDiffString(
        ldtBeginDate,
        new Date(),
        'ms',
      )}]`
      SchParam.Debug(message, '资源运算')
      ldtBeginDate = new Date()
    }

    try {
      // 1.1 Return available start time for scheduling
      let adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = ai_workTime
      let ai_disWorkTime = ai_workTime
      let dtBegDate = adCanBegDate,
        dtEndDate = adCanBegDate

      SchParam.ldtBeginDate = new Date()

      // Returns adCanBegDateTask as the task's official start time
      let li_Return = this.TestResSchTask(
        as_SchProductRouteRes,
        ai_workTimeTest,
        adCanBegDateTest,
        adCanBegDateTask,
        false,
        ai_ResPreTime,
        ai_CycTimeTol,
        dtBegDate,
        dtEndDate,
        bReCalWorkTime,
      )

      if (li_Return < 0) {
        const cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${adCanBegDateTest}]`
        throw new Error(cError)
      }

      SchParam.iWaitTime =
        new Date().getTime() - SchParam.ldtBeginDate.getTime()

      if (
        (as_SchProductRouteRes.iProcessProductID ===
          SchParam.iProcessProductID &&
          as_SchProductRouteRes.schProductRoute.iSchSdID ===
            SchParam.iSchSdID) ||
        (as_SchProductRouteRes.iProcessProductID === 193864 &&
          as_SchProductRouteRes.schProductRoute.iSchSdID === 1070)
      ) {
        message = `3.2、TestResSchTask 排产顺序[${
          as_SchProductRouteRes.iSchSN
        }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
          as_SchProductRouteRes.iProcessProductID
        }],资源编号[${
          as_SchProductRouteRes.cResourceNo
        }],开始排产时间[${new Date().toLocaleString()}],完成排产时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new Date(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new Date()
      }

      adCanBegDate = adCanBegDateTask

      // 2.1 Find all available time slots where AvailableTime > 0 and DEndTime > adCanBegDate
      let ResTimeRangeList1 = ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime > adCanBegDate,
      )
      ResTimeRangeList1.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )

      let bFirtTime = true

      if (
        (as_SchProductRouteRes.iProcessProductID ===
          SchParam.iProcessProductID &&
          as_SchProductRouteRes.schProductRoute.iSchSdID ===
            SchParam.iSchSdID) ||
        (as_SchProductRouteRes.iProcessProductID === 193864 &&
          as_SchProductRouteRes.schProductRoute.iSchSdID === 1070)
      ) {
        message = `3.3、ResTimeRangeList1 排产顺序[${
          as_SchProductRouteRes.iSchSN
        }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
          as_SchProductRouteRes.iProcessProductID
        }],资源编号[${
          as_SchProductRouteRes.cResourceNo
        }],开始排产时间[${new Date().toLocaleString()}],完成排产时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new Date(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new Date()
      }

      // 2.1 Loop through available time slots and schedule production tasks
      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (
          bFirtTime &&
          ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime &&
          ai_workTime > ResTimeRangeList1[i].AvailableTime
        ) {
          continue
        }

        let ldtBeginDateRessource = new Date()
        if (as_SchProductRouteRes.cSeqNote === '折弯') {
          ldtBeginDateRessource = new Date()
        }

        ResTimeRangeList1[i].TimeSchTask(
          as_SchProductRouteRes,
          ai_workTime,
          adCanBegDate,
          ai_workTimeTask,
          adCanBegDateTask,
          true,
          ai_ResPreTime,
          ai_CycTimeTol,
          bFirtTime,
          ai_disWorkTime,
          bReCalWorkTime,
        )

        let iWaitTime = new Date().getTime() - ldtBeginDateRessource.getTime()

        if (as_SchProductRouteRes.cSeqNote === '折弯') {
          iWaitTime = iWaitTime
        }

        if (bFirtTime) {
          dtBegDate = ResTimeRangeList1[i].DBegTime
          as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime
        }

        if (ai_workTime <= 0) break
      }

      if (
        (as_SchProductRouteRes.iProcessProductID ===
          SchParam.iProcessProductID &&
          as_SchProductRouteRes.schProductRoute.iSchSdID ===
            SchParam.iSchSdID) ||
        (as_SchProductRouteRes.iProcessProductID === 193864 &&
          as_SchProductRouteRes.schProductRoute.iSchSdID === 1070)
      ) {
        message = `3.4、TimeSchTask 排产顺序[${
          as_SchProductRouteRes.iSchSN
        }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
          as_SchProductRouteRes.iProcessProductID
        }],资源编号[${
          as_SchProductRouteRes.cResourceNo
        }],开始排产时间[${new Date().toLocaleString()}],完成排产时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new Date(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new Date()
      }

      // 2.3 Check if task's remaining work time > 0, meaning task is not completed
      if (ai_workTime > 0) {
        const cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${adCanBegDateTest}]`
        throw new Error(cError)
      }
    } catch (e) {
      // Handle error
    }

    return ai_workTimeTask
  }
  TestResSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTimeTest: number,
    adCanBegDateTest: Date,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    bReCalWorkTime: boolean,
    as_SchProductRouteResPre: SchProductRouteRes | null,
  ): number {
    // Simulate test for scheduling task
    return 0
  }

  ResSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: Date,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    bReCalWorkTime: boolean = true,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate

    let ldtBeginDate = new Date()
    let message = ''
    let iWaitTime: number

    if (
      (as_SchProductRouteRes.iSchSdID === SchParam.iSchSdID &&
        as_SchProductRouteRes.iProcessProductID ===
          SchParam.iProcessProductID) ||
      as_SchProductRouteRes.iResourceAbilityID === SchParam.iProcessProductID
    ) {
      let i = 1
    }

    if (
      (as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID &&
        as_SchProductRouteRes.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
      (as_SchProductRouteRes.iProcessProductID === 193864 &&
        as_SchProductRouteRes.schProductRoute.iSchSdID === 1070)
    ) {
      message = `3.1、排产顺序[${as_SchProductRouteRes.iSchSN}],计划ID[${
        as_SchProductRouteRes.iSchSdID
      }],任务ID[${as_SchProductRouteRes.iProcessProductID}],资源编号[${
        as_SchProductRouteRes.cResourceNo
      }],开始排产时间[${new Date()}],完成排产时间[${this.getDateDiff(
        ldtBeginDate,
        new Date(),
        'ms',
      )}]`
      SchParam.Debug(message, '资源运算')
      ldtBeginDate = new Date()
    }

    try {
      let adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = ai_workTime
      let ai_disWorkTime = ai_workTime
      let dtBegDate = adCanBegDate
      let dtEndDate = adCanBegDate

      SchParam.ldtBeginDate = new Date()
      ldtBeginDate = new Date()

      let li_Return = this.TestResSchTask(
        as_SchProductRouteRes,
        ai_workTimeTest,
        adCanBegDateTest,
        ai_ResPreTime,
        ai_CycTimeTol,
        bReCalWorkTime,
        as_SchProductRouteResPre,
      )
      if (li_Return < 0) {
        let cError = `订单行号：${as_SchProductRouteRes.iSchSdID}, 加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${adCanBegDateTest}]`
        throw new Error(cError)
      }

      let ldtEndedDate = new Date()
      let interval = ldtEndedDate.getTime() - ldtBeginDate.getTime()
      SchParam.iWaitTime = interval

      if (
        (as_SchProductRouteRes.iProcessProductID ===
          SchParam.iProcessProductID &&
          as_SchProductRouteRes.schProductRoute.iSchSdID ===
            SchParam.iSchSdID) ||
        (as_SchProductRouteRes.iProcessProductID === 193864 &&
          as_SchProductRouteRes.schProductRoute.iSchSdID === 1070)
      ) {
        message = `3.2、TestResSchTask 排产顺序[${
          as_SchProductRouteRes.iSchSN
        }],计划ID[${as_SchProductRouteRes.iSchSdID}],任务ID[${
          as_SchProductRouteRes.iProcessProductID
        }],资源编号[${
          as_SchProductRouteRes.cResourceNo
        }],开始排产时间[${new Date()}],完成排产时间[${this.getDateDiff(
          ldtBeginDate,
          new Date(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new Date()
      }

      adCanBegDate = adCanBegDateTask

      let ResTimeRangeList1 = this.getResTimeRangeList()
      ResTimeRangeList1 = ResTimeRangeList1.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime > adCanBegDate,
      )
      ResTimeRangeList1.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )

      let bFirtTime = true

      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (
          bFirtTime &&
          ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime &&
          ai_workTime > ResTimeRangeList1[i].AvailableTime
        )
          continue

        let ldtBeginDateRessource = new Date()
        if (as_SchProductRouteRes.cSeqNote === '折弯') {
          ldtBeginDateRessource = new Date()
        }

        try {
          ResTimeRangeList1[i].TimeSchTask(
            as_SchProductRouteRes,
            ai_workTime,
            adCanBegDate,
            ai_workTimeTask,
            adCanBegDateTask,
            bReCalWorkTime,
            ai_ResPreTime,
            ai_CycTimeTol,
            bFirtTime,
            ai_disWorkTime,
          )
        } catch (error) {
          throw new Error(
            `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResTimeRangeList1！工序ID号：${as_SchProductRouteRes.iProcessProductID}\n\r ${error.message}`,
          )
        }

        if (bFirtTime) {
          dtBegDate = ResTimeRangeList1[i].DBegTime
          as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime
        }

        if (ai_workTime <= 0) break
      }

      return 1
    } catch (error) {
      console.error(error)
      return -1
    }
  }

  getDateDiff(startDate: Date, endDate: Date, unit: string): string {
    const diffInMs = endDate.getTime() - startDate.getTime()
    return `${diffInMs} ${unit}`
  }

  getResTimeRangeList(): ResTimeRange[] {
    return []
  }
  public resSchTaskRev(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: Date,
  ): number {
    // let taskallottedTime = 0; // Task allotted time in this period
    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate // Task can start scheduling date, re-set if interrupted
    let dtBegDate: Date = adCanBegDate
    let dtEndDate: Date = adCanBegDate

    if (
      as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID
    ) {
      let i = 1

      const resTimeRangeListTest = ResTimeRangeList.filter(
        (p) => p.DBegTime <= adCanBegDate,
      )
    }

    // Return available start time to schedule, ensuring no breaks in the task

    let adCanBegDateTest = adCanBegDate // Task start date
    let ai_workTimeTest = ai_workTime
    let ai_ResPreTime = 0 // Resource preparation time
    let ai_CycTimeTol = 0 // Tool change time, not used when reversing schedule

    SchParam.ldtBeginDate = new Date()

    try {
      // SchParam.Debug("1.11111111 ResSchTaskRev TestResSchTask", "Resource Calculation");

      // Return adCanBegDateTask as the official scheduling start date
      let li_Return = this.testResSchTask(
        as_SchProductRouteRes,
        ai_workTimeTest,
        adCanBegDateTest,
        adCanBegDateTask,
        true,
        ai_ResPreTime,
        ai_CycTimeTol,
        dtBegDate,
        dtEndDate,
      )

      if (li_Return < 0) {
        let cError = `Order line number: ${
          as_SchProductRouteRes.iSchSdID
        }, processed material [${
          as_SchProductRouteRes.cInvCode
        }] in resource [${
          as_SchProductRouteRes.cResourceNo
        }] cannot be scheduled, task number [${
          as_SchProductRouteRes.iProcessProductID
        }], unit capacity [${
          as_SchProductRouteRes.iCapacity
        }], processing quantity [${
          as_SchProductRouteRes.iResReqQty
        }], work time [${ai_workTimeTask / 3600}], unscheduled time [${
          ai_workTime / 3600
        }], max available time [${adCanBegDateTest}], check work calendar or unit capacity and plan quantity!`
        throw new Error(cError)
      }

      // Update adCanBegDate with the scheduled start time
      adCanBegDate = adCanBegDateTask

      // Find available time segments with time greater than 0, ensuring the segment start is less than adCanBegDate
      let resTimeRangeList1 = ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DBegTime <= adCanBegDate,
      )
      // Sort by start time in descending order
      resTimeRangeList1.sort(
        (p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime(),
      )

      let bFirtTime = true // Whether it's the first scheduling time period

      // Reverse scheduling, start from the longest time period
      for (let i = 0; i < resTimeRangeList1.length; i++) {
        if (ai_workTime === 0) break

        if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType !== '2') {
          // Limited capacity reverse scheduling
          resTimeRangeList1[i].timeSchTaskRev(
            as_SchProductRouteRes,
            ai_workTime,
            adCanBegDate,
            ai_workTimeTask,
            adCanBegDateTask,
            true,
            bFirtTime,
          )
        } else {
          // Unlimited capacity reverse scheduling
          resTimeRangeList1[i].timeSchTaskRevInfinite(
            as_SchProductRouteRes,
            ai_workTime,
            adCanBegDate,
            ai_workTimeTask,
            adCanBegDateTask,
            true,
            bFirtTime,
          )
        }
      }

      // If there is remaining work time, scheduling failed
      if (ai_workTime > 0) {
        let cError = `Order line number: ${
          as_SchProductRouteRes.iSchSdID
        }, processed material [${
          as_SchProductRouteRes.cInvCode
        }] in resource [${
          as_SchProductRouteRes.cResourceNo
        }] cannot be scheduled, task number [${
          as_SchProductRouteRes.iProcessProductID
        }], unit capacity [${
          as_SchProductRouteRes.iCapacity
        }], processing quantity [${
          as_SchProductRouteRes.iResReqQty
        }], work time [${ai_workTimeTask / 3600}], unscheduled time [${
          ai_workTime / 3600
        }], max available time [${
          resTimeRangeList1[resTimeRangeList1.length - 1].DEndTime
        }]`
        throw new Error(cError)
      } else {
        as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++ // Scheduling sequence number
        as_SchProductRouteRes.BScheduled = 1 // Mark as scheduled

        // Log scheduling details
        if (SchParam.APSDebug === '1') {
          let message2 = `3. Scheduling sequence [${
            as_SchProductRouteRes.iSchSN
          }], resource ID [${
            as_SchProductRouteRes.cResourceNo
          }], material ID [${as_SchProductRouteRes.cInvCode}], batch number [${
            as_SchProductRouteRes.schProductRoute.schProduct.cSchSNType
          }], batch sequence [${
            as_SchProductRouteRes.schProductRoute.schProduct.iSchSN
          }], task priority [${
            as_SchProductRouteRes.iPriorityRes
          }], order priority [${
            as_SchProductRouteRes.schProductRoute.schProduct.iPriority
          }], process [${
            as_SchProductRouteRes.iWoSeqID +
            as_SchProductRouteRes.cSeqNote.trim()
          }], batch [${as_SchProductRouteRes.iSchBatch}], work order number [${
            as_SchProductRouteRes.cWoNo
          }]`

          message2 += ` Plan ID [${as_SchProductRouteRes.iSchSdID}], task ID [${as_SchProductRouteRes.iProcessProductID}], plan start time [${as_SchProductRouteRes.dResBegDate}], plan end time [${as_SchProductRouteRes.dResEndDate}]`
          message2 += ` Available start time [${as_SchProductRouteRes.dCanResBegDate}], preparation time [${as_SchProductRouteRes.iResPreTime}], process feature 1 [${as_SchProductRouteRes.FResChaValue1ID}], process feature 2 [${as_SchProductRouteRes.FResChaValue2ID}]`

          SchParam.Debug(
            message2,
            'SchProductRouteRes.ResSchTaskRev Scheduling Complete',
          )
        }
      }
    } catch (error) {
      throw new Error(
        `Order line number: ${as_SchProductRouteRes.iSchSdID} encountered an error during resource reverse calculation, position Resource.ResSchTaskRev! Process ID: ${as_SchProductRouteRes.iProcessProductID}\n\r ${error.message}`,
      )
      return -1
    }

    return 1 // Remaining unscheduled time
  }
  public TestResSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: Date,
    adCanBegDateTask: Date,
    bSchRev: boolean,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    dtBegDate: Date,
    dtEndDate: Date,
    bShowTips: boolean = true,
    bReCalWorkTime: boolean = true,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let ai_workTimeTask = ai_workTime
    let ai_disWorkTime = ai_workTime
    let adCanBegDateTask2 = adCanBegDateTask
    let ai_ResPostTime = 0
    dtEndDate = adCanBegDate

    const ldtBeginDate = new Date()

    if (
      as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID &&
      as_SchProductRouteRes.iSchSdID === SchParam.iSchSdID
    ) {
      let i = 1 // Debug point
    }

    try {
      let ResTimeRangeList1: ResTimeRange[] = []

      if (!bSchRev) {
        // Forward scheduling
        ResTimeRangeList1 = ResTimeRangeList.filter(
          (p) => p.DEndTime > adCanBegDateTask2,
        )

        if (ResTimeRangeList.length < 1) {
          if (bShowTips) {
            let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${as_SchProductRouteRes.cInvCode}]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${as_SchProductRouteRes.iProcessProductID}],请检查是否有工作日历或当前资源是资源组!`
            throw new Error(cError)
          }
          return -1
        }

        ResTimeRangeList1.sort(
          (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
        )

        let bFirtTime = true
        let resTimeRangeNext: ResTimeRange | null = null

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime === 0) break

          if (i < ResTimeRangeList1.length - 1) {
            resTimeRangeNext = ResTimeRangeList1[i + 1]
          } else {
            resTimeRangeNext = null
          }

          if (
            ResTimeRangeList1[i].AvailableTime <= 0 &&
            ResTimeRangeList1[i].AllottedTime > 0
          ) {
            bFirtTime = true
            ai_workTime = ai_workTimeTask
            adCanBegDate = ResTimeRangeList1[i].DEndTime
            adCanBegDateTask = ResTimeRangeList1[i].DEndTime
            continue
          }

          try {
            SchParam.ldtBeginDate = new Date()

            ResTimeRangeList1[i].TimeSchTask(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              ai_ResPreTime,
              ai_CycTimeTol,
              bFirtTime,
              ai_disWorkTime,
              bReCalWorkTime,
              resTimeRangeNext,
              as_SchProductRouteResPre,
            )
          } catch (error) {
            throw new Error(
              `时间段排程出错,订单行号：${as_SchProductRouteRes.iSchSdID} 资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID}\n\r${error.message}`,
            )
          }

          if (bFirtTime) {
            dtBegDate = adCanBegDate
            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate
          }
        }

        dtEndDate = adCanBegDate
      } else {
        // Reverse scheduling
        ResTimeRangeList1 = ResTimeRangeList.filter(
          (p) => p.DBegTime < adCanBegDateTask2,
        )
        ResTimeRangeList1.sort(
          (p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime(),
        )

        let bFirtTime = true

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime === 0) break

          if (
            as_SchProductRouteRes.schProductRoute.schProduct.cSchType !== '2'
          ) {
            ResTimeRangeList1[i].TimeSchTaskRev(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )
          } else {
            ResTimeRangeList1[i].TimeSchTaskRevInfinite(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )
          }

          if (bFirtTime) {
            dtEndDate = adCanBegDate
          }

          if (bFirtTime) {
            bFirtTime = false
            dtBegDate = adCanBegDate
            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate
          }
        }
      }
    } catch (error) {
      throw new Error(`Error in TestResSchTask: ${error.message}`)
    }

    return 0
  }
  public testResSchTaskNew(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: Date,
    adCanBegDateTask: Date,
    bSchRev: boolean,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    dtBegDate: Date,
    dtEndDate: Date,
    bShowTips: boolean = true,
    bReCalWorkTime: boolean = true,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    // Initialize variables
    let ai_workTimeTask = ai_workTime
    let ai_disWorkTime = ai_workTime
    let adCanBegDateTask2 = adCanBegDateTask
    let ai_ResPostTime = 0 // Resource post preparation time
    dtEndDate = adCanBegDate

    if (
      as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID &&
      as_SchProductRouteRes.iSchSdID === SchParam.iSchSdID
    ) {
      let i = 1 // Debug breakpoint
    }

    try {
      // Find available time ranges
      let ResTimeRangeList1: ResTimeRange[] = []

      if (!bSchRev) {
        // Forward scheduling
        ResTimeRangeList1 = ResTimeRangeList.filter(
          (p) => p.DEndTime > adCanBegDateTask2,
        )
        ResTimeRangeList1.sort(
          (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
        )

        let bFirtTime = true // Is it the first time slot?

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime === 0) break

          // Schedule within the available time slot
          ResTimeRangeList1[i].timeSchTask(
            as_SchProductRouteRes,
            ai_workTime,
            adCanBegDate,
            ai_workTimeTask,
            adCanBegDateTask,
            false,
            ai_ResPreTime,
            ai_CycTimeTol,
            bFirtTime,
            ai_disWorkTime,
            bReCalWorkTime,
          )

          if (bFirtTime) {
            dtBegDate = adCanBegDate
            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate
          }
        }

        dtEndDate = adCanBegDate // Task end date
      } else {
        // Reverse scheduling
        ResTimeRangeList1 = ResTimeRangeList.filter(
          (p) => p.DBegTime < adCanBegDateTask2,
        )
        ResTimeRangeList1.sort(
          (p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime(),
        )

        let bFirtTime = true

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime === 0) break

          // Reverse schedule with available time slots
          if (
            as_SchProductRouteRes.schProductRoute.schProduct.cSchType !== '2'
          ) {
            // Limited capacity reverse scheduling
            ResTimeRangeList1[i].timeSchTaskRev(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )
          } else {
            // Unlimited capacity reverse scheduling
            ResTimeRangeList1[i].timeSchTaskRevInfinite(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )
          }

          if (bFirtTime) {
            dtEndDate = adCanBegDate
          }
        }

        dtBegDate = adCanBegDate // Task start date
      }

      // Check if task work time is still remaining
      if (ai_workTime > 0) {
        if (bShowTips) {
          const cError = `Order line number: ${
            as_SchProductRouteRes.iSchSdID
          }, processing material [${
            as_SchProductRouteRes.cInvCode
          }] on resource [${
            as_SchProductRouteRes.cResourceNo
          }] cannot be scheduled. Task number: [${
            as_SchProductRouteRes.iProcessProductID
          }], unit capacity: [${as_SchProductRouteRes.iCapacity}], quantity: [${
            as_SchProductRouteRes.iResReqQty
          }], work time: [${ai_workTimeTask / 3600}], remaining work time: [${
            ai_workTime / 3600
          }], max available time: [${
            ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
          }]. Please check the work calendar or capacity, or adjust the plan quantity.`
          throw new Error(cError)
        }
        return -1
      }
    } catch (error) {
      if (bShowTips) {
        throw new Error(
          `Order line number: ${as_SchProductRouteRes.iSchSdID} scheduling calculation error, position Resource.ResSchTask! Process ID: ${as_SchProductRouteRes.iProcessProductID}\n ${error.message}`,
        )
      }
      return -1
    }

    return 1 // Remaining unscheduled time
  }
  GetEarlyStartDate(adStartDate: Date, bSchRev: boolean): Date {
    let ListReturn: ResTimeRange[] = [] // List of available resource time ranges
    let dtEndDate: Date = adStartDate // Initially, set to adStartDate

    if (!bSchRev) {
      // Forward scheduling
      // Find all time ranges with AvailableTime > 0 and end time greater than adStartDate
      ListReturn = ResTimeRangeList.filter(
        (p) => p.DEndTime > adStartDate && p.AvailableTime > 0,
      )

      // Sort by the start time (ascending)
      ListReturn.sort((p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime())

      if (ListReturn.length > 0) {
        dtEndDate = ListReturn[0].DEndTime
      }
    } else {
      // Reverse scheduling
      // Find all time ranges with AvailableTime > 0 and start time less than adStartDate
      ListReturn = ResTimeRangeList.filter(
        (p) => p.DBegTime < adStartDate && p.AvailableTime > 0,
      )

      // Sort by the start time (descending)
      ListReturn.sort((p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime())

      if (ListReturn.length > 0) {
        dtEndDate = ListReturn[0].DBegTime
      }
    }

    return dtEndDate
  }
  GetSchStartDate(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adStartDate: Date,
    bSchRev: boolean,
    dtEndDate: Date,
  ): Date {
    let ListReturn: ResTimeRange[] = []
    let ai_workTimeOld: number = ai_workTime

    let TaskTimeRangeBeg: TaskTimeRange | null = null
    let TaskTimeRangeEnd: TaskTimeRange | null = null

    // If specific condition is met for debugging, break into a debugger or log
    if (
      as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID
    ) {
      let i = 1 // Placeholder for debugging
    }

    // Simulate the scheduling process (the commented-out code would contain sorting and logic similar to the original)
    // ...

    // Loop to find a suitable time range for scheduling
    for (let i = 0; i < ListReturn.length; i++) {
      if (ai_workTime === ai_workTimeOld) {
        TaskTimeRangeBeg = null
        ResTimeRangeBeg = ListReturn[i]
      }

      if (ListReturn[i].AvailableTime === 0) {
        ai_workTime = ai_workTimeOld
        continue
      }

      if (!bSchRev) {
        // Forward scheduling
        ListReturn[i].TaskTimeRangeList.sort(
          (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
        )

        for (const TaskTimeRange1 of ListReturn[i].TaskTimeRangeList) {
          if (
            ListReturn[i].CIsInfinityAbility === '0' &&
            TaskTimeRange1.cTaskType === 1 &&
            ai_workTime > 0
          ) {
            ai_workTime = ai_workTimeOld
            ResTimeRangeBeg = ListReturn[i]
            TaskTimeRangeBeg = TaskTimeRange1
            continue
          }

          if (TaskTimeRange1.cTaskType === 0) {
            if (!TaskTimeRangeBeg) TaskTimeRangeBeg = TaskTimeRange1
            ai_workTime -= TaskTimeRange1.AvailableTime
            if (ai_workTime < 0) {
              ai_workTime = 0
              TaskTimeRangeEnd = TaskTimeRange1
              dtEndDate = TaskTimeRange1.DBegTime
              break
            }
            continue
          }
        }
      } else {
        // Reverse scheduling
        ListReturn[i].TaskTimeRangeList.sort(
          (p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime(),
        )

        for (const TaskTimeRange1 of ListReturn[i].TaskTimeRangeList) {
          if (
            ListReturn[i].CIsInfinityAbility === '0' &&
            TaskTimeRange1.cTaskType === 1 &&
            ai_workTime > 0
          ) {
            ai_workTime = ai_workTimeOld
            ResTimeRangeBeg = ListReturn[i]
            TaskTimeRangeBeg = TaskTimeRange1
            continue
          }

          if (TaskTimeRange1.cTaskType === 0) {
            if (!TaskTimeRangeBeg) TaskTimeRangeBeg = TaskTimeRange1
            ai_workTime -= TaskTimeRange1.AvailableTime
            if (ai_workTime < 0) {
              ai_workTime = 0
              TaskTimeRangeEnd = TaskTimeRange1
              dtEndDate = TaskTimeRange1.DBegTime
              break
            }
            continue
          }
        }
      }

      if (ai_workTime === 0) {
        ResTimeRangeEnd = ListReturn[i]
        break
      }
    }

    if (ai_workTime > 0) {
      throw new Error(
        `Unable to schedule product route ${as_SchProductRouteRes.iProcessProductID}`,
      )
    }

    return !bSchRev ? TaskTimeRangeBeg.DBegTime : TaskTimeRangeBeg.DEndTime
  }
  public GetChangeTime(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adStartDate: Date,
    iCycTimeTol: number,
    bSchdule: boolean,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let iPreTime = 0 // 前准备时间(换产时间)

    // If the product type is "PSH", skip change time calculation
    if (
      this.cNeedChanged === '1' &&
      as_SchProductRouteRes.cWoNo !== '' &&
      as_SchProductRouteRes.schProductRoute.schProduct.cType === 'PSH'
    ) {
      return 0
    }

    // If no previous product route is provided, find the previous scheduled task
    if (as_SchProductRouteResPre === null) {
      const ListSchProductRouteResAll = this.schData.SchProductRouteResList.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.iResReqQty > 0 &&
          p1.BScheduled === 1 &&
          p1.dResBegDate <= adStartDate,
      )
      ListSchProductRouteResAll.sort(
        (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
          p2.dResBegDate.getTime() - p1.dResBegDate.getTime(),
      )

      if (ListSchProductRouteResAll.length > 0) {
        as_SchProductRouteResPre = ListSchProductRouteResAll[0]
      }
    }

    // Reset the time note
    this.cTimeNote = ''

    // Calculate the change time based on resource process characteristics
    if (SchParam.ResProcessCharCount > 0) {
      iPreTime = this.GetChangeTime(
        as_SchProductRouteRes,
        ai_workTime,
        as_SchProductRouteResPre,
        iCycTimeTol,
        bSchdule,
      )
    }

    // Add resource preparation time if process characteristics are present
    if (iPreTime > 0) {
      iPreTime += parseInt(this.iResPreTime.toString())
      this.cTimeNote += ` 资源前准备时间:[${this.iResPreTime}];`
    }

    // Add material change time if the work item has changed
    if (as_SchProductRouteResPre !== null) {
      if (
        as_SchProductRouteRes.schProductRoute.cWorkItemNo !==
        as_SchProductRouteResPre.schProductRoute.cWorkItemNo
      ) {
        iPreTime += parseInt(this.iChangeTime.toString())
        this.cTimeNote += ` 换料时间:[${this.iChangeTime}];`
      }
    } else {
      // If there is no previous material, add material change time
      iPreTime += parseInt(this.iChangeTime.toString())
      this.cTimeNote += ` 换料时间:[${this.iChangeTime}];`
    }

    // Add process route preparation time if applicable
    if (as_SchProductRouteRes.iResPreTimeOld > 0) {
      iPreTime += as_SchProductRouteRes.iResPreTimeOld
      this.cTimeNote += ` 工艺路线资源前准备时间:[${as_SchProductRouteRes.iResPreTimeOld}];`
    }

    // Store the time note in the resource task's cDefine25 field
    as_SchProductRouteRes.cDefine25 = this.cTimeNote

    return iPreTime // Return the change time
  }
}
