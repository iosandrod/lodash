import { Base } from './Base'
import {
  SchData,
  SchProductWorkItem,
  SchProductRouteRes,
  DataRow,
  WorkCenter,
  ResSourceDayCap,
  ResTimeRange,
  // TimeRangeAttribute,
  TaskTimeRange,
  // ResTimeRangeList,
  Comparer,
  DateTime,
} from './type'
export class Resource extends Base {
  iResourceID: number
  cResourceNo: string
  cResourceName: string
  cResClsNo: string
  cResourceType: string
  private IResourceNumber: number
  cResOccupyType: string
  iPreStocks: number
  iPostStocks: number
  iUsage: number
  private IEfficient: number
  cResouceInformation: string
  cIsInfinityAbility: string
  bScheduled: number = 0
  iOverResourceNumber: number
  iLimitResourceNumber: number
  iOverEfficient: number
  iLimitEfficient: number
  iResDifficulty: number
  iDistributionRate: number
  cWcNo: string
  private resWorkCenter: WorkCenter | null = null
  cDeptNo: string
  cDeptName: string
  cStatus: string
  iSchemeID: number
  iCacheTime: number
  iLastBatchPercent: number
  cIsKey: string
  iKeySchSN: number
  cNeedChanged: string
  dMaxExeDate: DateTime
  cProChaType1Sort: string
  cDayPlanShowType: string
  iChangeTime: number
  iResPreTime: number
  iTurnsType: string
  iTurnsTime: number
  cTeamNo: string
  cTeamNo2: string
  cTeamNo3: string
  cBatch1Filter: string
  cBatch2Filter: string
  cBatch3Filter: string
  cBatch4Filter: string
  cBatch5Filter: string
  iBatchWoSeqID: number
  cBatch1WorkTime: number
  cBatch2WorkTime: number
  cBatch3WorkTime: number
  cBatch4WorkTime: number
  cBatch5WorkTime: number
  cPriorityType: string
  cResBarCode: string
  cTeamResourceNo: string
  bTeamResource: string
  cSuppliyMode: string
  cResOperator: string
  cResManager: string
  TeamResourceList: Resource[] = new Array(10)
  TeamResource: Resource
  bAllocated: string = '0'
  iResWorkersPd: number
  iResHoursPd: number
  iResOverHoursPd: number
  iLabRate: number
  iPowerRate: number
  iOtherRate: number
  iMinWorkTime: number
  FProChaType1ID: string
  FProChaType2ID: string
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
  cDefine15: DateTime
  cDefine16: DateTime
  iBatch: number = 2
  dBatchBegDate: DateTime
  dBatchEndDate: DateTime
  cTimeNote: string = ''
  iSchBatch: number = 6
  cSelected: number = 1
  iSchHours: number = 0
  iPlanDays: number = 0

  constructor(cResourceNo?: string) {
    super()
    if (cResourceNo) {
      const schData = this.schData
      const dr = schData.dtResource.filter(
        (row) => row.cResourceNo === cResourceNo,
      )
      if (dr.length < 1) return
      this.getResource(dr[0])
    }
  }

  get ResWorkCenter(): WorkCenter | null {
    if (this.cWcNo === '') this.resWorkCenter = null
    const listWorkCenter = this.schData.WorkCenterList.filter(
      (p1) => p1.cWcNo === this.cWcNo,
    )
    if (listWorkCenter.length > 0) {
      this.resWorkCenter = listWorkCenter[0]
    } else {
      this.resWorkCenter = null
    }
    return this.resWorkCenter
  }

  get iResourceNumber(): number {
    const SchParam = this.SchParam
    if (SchParam.cSchCapType === '1') {
      return this.IResourceNumber > this.iOverResourceNumber
        ? this.IResourceNumber
        : this.iOverResourceNumber
    } else if (SchParam.cSchCapType === '2') {
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

  get iEfficient(): number {
    const SchParam = this.SchParam
    if (this.IEfficient <= 0) this.IEfficient = 100
    if (SchParam.cSchCapType === '1') {
      return this.IEfficient > this.iOverEfficient
        ? this.IEfficient
        : this.iOverEfficient
    } else if (SchParam.cSchCapType === '2') {
      return this.IEfficient > this.iLimitEfficient
        ? this.IEfficient
        : this.iLimitEfficient
    } else {
      return this.IEfficient
    }
  }

  set iEfficient(value: number) {
    this.IEfficient = value
  }

  ResTimeRangeList: ResTimeRange[] = new Array(10)
  ResSourceDayCapList: ResSourceDayCap[] = new Array(10)
  ResTimeRangeListBak: ResTimeRange[] = new Array(10)
  ResSpecTimeRangeList: ResTimeRange[] = new Array(10)
  schProductRouteResList: SchProductRouteRes[] = new Array(10)

  getNotSchTask(): SchProductRouteRes[] {
    this.schProductRouteResList = this.schData.SchProductRouteResList.filter(
      (p1) =>
        p1.cResourceNo === this.cResourceNo &&
        p1.BScheduled === 0 &&
        p1.iSchBatch === this.iSchBatch,
    )
    this.schProductRouteResList.sort((p1, p2) => this.TaskComparer(p1, p2))
    return this.schProductRouteResList
  }

  mergeTimeRange(): void {
    let TimeRangeAttribute = this.TimeRangeAttribute
    let resSpecTimeRangeList1 = this.ResSpecTimeRangeList.filter(
      (p1: any) =>
        p1.Attribute === TimeRangeAttribute.Work ||
        p1.Attribute === TimeRangeAttribute.Overtime ||
        p1.Attribute === TimeRangeAttribute.MayOvertime,
    )
    resSpecTimeRangeList1.sort(
      (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
    )
    let dCanBegDate = this.schData.dtStart
    let dCanEndDate = this.schData.dtEnd

    for (const resTimeRange of resSpecTimeRangeList1) {
      dCanBegDate = this.schData.dtStart
      dCanEndDate = this.schData.dtEnd
      let lResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p2) => p2.DBegTime <= resTimeRange.DBegTime,
      )
      lResTimeRangeList1.sort(
        (p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime(),
      )
      if (lResTimeRangeList1.length > 0) {
        dCanBegDate = lResTimeRangeList1[0].DBegTime
      }
      let lResTimeRangeList = this.ResTimeRangeList.filter(
        (p2) => p2.DBegTime >= dCanBegDate,
      )
      lResTimeRangeList.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )
      let resLastTimeRange: ResTimeRange | null = null
      const iCount = lResTimeRangeList.length
      for (let i = 0; i < iCount; i++) {
        const resWorkTimeRange = lResTimeRangeList[i]
        if (i > 0) {
          const resAddTimeRange = new ResTimeRange()
          if (resTimeRange.DBegTime < resLastTimeRange!.DEndTime) {
            resAddTimeRange.DBegTime = resLastTimeRange!.DEndTime
          } else {
            resAddTimeRange.DBegTime = resTimeRange.DBegTime
          }
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
          this.ResTimeRangeList.push(resAddTimeRange)
        }
        resLastTimeRange = resWorkTimeRange
      }
    }
    // let TimeRangeAttribute=this.TimeRangeAttribute
    let resSpecTimeRangeList2 = this.ResSpecTimeRangeList.filter(
      (p1) =>
        p1.Attribute === TimeRangeAttribute.Maintain ||
        p1.Attribute === TimeRangeAttribute.Snag,
    )
    resSpecTimeRangeList2.sort(
      (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
    )
    for (const resTimeRange of resSpecTimeRangeList2) {
      let lResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p2) => p2.DBegTime <= resTimeRange.DBegTime,
      )
      lResTimeRangeList1.sort(
        (p1, p2) => p2.DBegTime.getTime() - p1.DBegTime.getTime(),
      )
      if (lResTimeRangeList1.length > 0) {
        dCanBegDate = lResTimeRangeList1[0].DBegTime
      } else {
        dCanBegDate = this.schData.dtStart
      }
      let lResTimeRangeList = this.ResTimeRangeList.filter(
        (p2) => p2.DBegTime >= dCanBegDate,
      )
      lResTimeRangeList.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )
      let resLastTimeRange: ResTimeRange | null = null
      const iCount = lResTimeRangeList.length
      for (let i = 0; i < iCount; i++) {
        const resWorkTimeRange = lResTimeRangeList[i]
        if (resTimeRange.DEndTime >= resWorkTimeRange.DBegTime) {
          this.deleteTimeRangeSub(resWorkTimeRange, resTimeRange)
        } else {
          break
        }
        resLastTimeRange = resWorkTimeRange
      }
    }
  }

  mergeTimeRangeSub(
    resWorkTimeRange: ResTimeRange,
    resSpecTimeRange: ResTimeRange,
  ): void {
    const resAddTimeRange = new ResTimeRange()
    resAddTimeRange.CResourceNo = this.cResourceNo
    resAddTimeRange.resource = this
    resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility
    resAddTimeRange.Attribute = resSpecTimeRange.Attribute
    if (resWorkTimeRange == null) {
      resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime
      resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
    } else {
      if (
        resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime &&
        resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime
      ) {
        resAddTimeRange.DBegTime = resWorkTimeRange.DEndTime
        resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
      } else if (
        resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime &&
        resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime
      ) {
        resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime
        resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
      } else if (
        resWorkTimeRange.DEndTime < resSpecTimeRange.DBegTime ||
        resWorkTimeRange.DBegTime > resSpecTimeRange.DEndTime
      ) {
        resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime
        resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime
      } else {
        return
      }
    }
    if (resAddTimeRange != null) {
      resAddTimeRange.GetNoWorkTaskTimeRange(
        resAddTimeRange.DBegTime,
        resAddTimeRange.DEndTime,
        false,
      )
      this.ResTimeRangeList.push(resAddTimeRange)
    }
  }

  addTimeRange(
    resLastTimeRange: ResTimeRange,
    resWorkTimeRange: ResTimeRange,
  ): void {
    const resAddTimeRange = new ResTimeRange()
    resAddTimeRange.CResourceNo = this.cResourceNo
    resAddTimeRange.resource = this
    resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility
    resAddTimeRange.DBegTime = resLastTimeRange.DEndTime
    resAddTimeRange.DEndTime = resLastTimeRange.DBegTime
    resAddTimeRange.GetNoWorkTaskTimeRange(
      resAddTimeRange.DBegTime,
      resAddTimeRange.DEndTime,
      false,
    )
    this.ResTimeRangeList.push(resAddTimeRange)
  }

  deleteTimeRangeSub(
    resWorkTimeRange: ResTimeRange,
    resSpecTimeRange: ResTimeRange,
  ): void {
    const resAddTimeRange = new TaskTimeRange()
    if (resSpecTimeRange.DBegTime < resWorkTimeRange.DBegTime) {
      resAddTimeRange.DBegTime = resWorkTimeRange.DBegTime
    } else if (resSpecTimeRange.DBegTime < resWorkTimeRange.DEndTime) {
      resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime
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
    if (resAddTimeRange != null) {
      if (resWorkTimeRange.TaskTimeRangeList.length > 0) {
        const newTimeRange = resWorkTimeRange.GetNoWorkTaskTimeRange(
          resAddTimeRange.DBegTime,
          resAddTimeRange.DEndTime,
          false,
        )
        newTimeRange.AllottedTime = newTimeRange.HoldingTime
        newTimeRange.Attribute = resSpecTimeRange.Attribute
        newTimeRange.cTaskType = 2
        resWorkTimeRange.TaskTimeRangeSplit(
          resWorkTimeRange.TaskTimeRangeList[0],
          newTimeRange,
        )
      }
    }
  }

  getResSourceDayCapList(): void {
    this.ResSourceDayCapList = new Array(10)
    let resSourceDayCap = new ResSourceDayCap()
    let ldt_todayLast = new DateTime()
    const groupedResTimeRange = this.ResTimeRangeList.reduce(
      (acc, resTimeRange) => {
        const key = `${resTimeRange.cResourceNo},${resTimeRange.dPeriodDay}`
        if (!acc[key]) {
          acc[key] = []
        }
        acc[key].push(resTimeRange)
        return acc
      },
      {} as { [key: string]: ResTimeRange[] },
    )

    this.ResTimeRangeList.sort(
      (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
    )
    for (const ResTimeRange1 of this.ResTimeRangeList) {
      if (ldt_todayLast.getTime() !== ResTimeRange1.dPeriodDay.getTime()) {
        resSourceDayCap = new ResSourceDayCap()
        resSourceDayCap.dPeriodDay = ResTimeRange1.dPeriodDay
        resSourceDayCap.DBegTime = ResTimeRange1.DBegTime
        this.ResSourceDayCapList.push(resSourceDayCap)
      }
      resSourceDayCap.ResTimeRangeList.push(ResTimeRange1)
      ResTimeRange1.resSourceDayCap = resSourceDayCap
      ldt_todayLast = ResTimeRange1.dPeriodDay
    }
  }

  schTaskFreezeInit(
    as_SchProductRouteRes: SchProductRouteRes,
    adCanBegDate: DateTime,
    adCanEndDate: DateTime,
  ): number {
    let SchParam = this.SchParam
    try {
      if (as_SchProductRouteRes.BScheduled == 1) return 0
      let ResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime >= adCanBegDate,
      )
      ResTimeRangeList1.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )
      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (ResTimeRangeList1[i].DBegTime > adCanEndDate) break
        ResTimeRangeList1[i].TimeSchTaskFreezeInit(
          as_SchProductRouteRes,
          adCanBegDate,
          adCanEndDate,
        )
      }
      as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++
      as_SchProductRouteRes.BScheduled = 1
      as_SchProductRouteRes.schProductRoute.BScheduled = 1
      if (SchParam.APSDebug == '1') {
        const message2 = `3、排产顺序[${
          as_SchProductRouteRes.iSchSN
        }],资源编号[${as_SchProductRouteRes.cResourceNo}],物料编号[${
          as_SchProductRouteRes.cInvCode
        }], 座次号[${
          as_SchProductRouteRes.schProductRoute.schProduct.cSchSNType
        }],座次顺序[${
          as_SchProductRouteRes.schProductRoute.schProduct.iSchSN
        }],任务优先级[${as_SchProductRouteRes.iPriorityRes}],订单优先级[${
          as_SchProductRouteRes.schProductRoute.schProduct.iPriority
        }],工序[${
          as_SchProductRouteRes.iWoSeqID + as_SchProductRouteRes.cSeqNote.trim()
        }],排程批次[${as_SchProductRouteRes.iSchBatch}],工单号[${
          as_SchProductRouteRes.cWoNo
        }]`
        SchParam.Debug(
          message2,
          'SchProductRouteRes.SchTaskFreezeInit工序排产完成',
        )
      }
    } catch (error) {
      throw new Error(
        `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID}\n\r ${error.message}`,
      )
    }
    return 1
  }

  schTaskSortInit(
    as_SchProductRouteRes: SchProductRouteRes,
    adCanBegDate: DateTime,
    adCanEndDate: DateTime,
  ): number {
    let SchParam = this.SchParam
    as_SchProductRouteRes.schProductRoute.ProcessSchTaskPre(false)
    let ai_workTime = 0
    const ai_ResReqQty =
      as_SchProductRouteRes.iResReqQty - as_SchProductRouteRes.iActResReqQty
    let iBatchCount = 0
    if (as_SchProductRouteRes.cWorkType == '1') {
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
      ai_workTime = ai_ResReqQty * as_SchProductRouteRes.iCapacity
    }
    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate
    try {
      let adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = ai_workTime
      let ai_disWorkTime = ai_workTime
      let ai_ResPreTime = 0
      let ai_CycTimeTol = 0
      let dtBegDate = adCanBegDate,
        dtEndDate = adCanBegDate
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
        const cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${adCanBegDateTest}],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
        return -1
      }
      adCanBegDate = adCanBegDateTask
      let ResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime > adCanBegDate,
      )
      ResTimeRangeList1.sort(
        (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
      )
      let bFirtTime = true
      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (ai_workTime == 0) break
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
        }
      }
      if (ai_workTime > 0) {
        const cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${
          ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
        }],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
        return -1
      } else {
        as_SchProductRouteRes.iSchSN = SchParam.iSchSNMin--
        as_SchProductRouteRes.BScheduled = 1
        as_SchProductRouteRes.schProductRoute.BScheduled = 1
      }
    } catch (error) {
      throw new Error(
        `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID}\n\r ${error.message}`,
      )
      return -1
    }
    return 1
  }

  resSchBefore(): number {
    if (this.iSchBatch < 0) {
      this.KeyResSchTask()
      return -1
    }
    return 1
  }

  resSchAfter(): number {
    return 1
  }

  resDispatchSch(iSchBatch: number): number {
    let SchParam = this.SchParam
    let SchProductRouteResPre: SchProductRouteRes | null = null
    let ListSchProductRouteRes: SchProductRouteRes[] = []
    let LastCanBegDate = new DateTime()
    try {
      if (iSchBatch == -100) {
        ListSchProductRouteRes = this.schData.SchProductRouteResList.filter(
          (p1) => p1.cResourceNo === this.cResourceNo && p1.iResReqQty > 0,
        )
        if (ListSchProductRouteRes.length > 0) {
          ListSchProductRouteRes.forEach((SchProductRouteResTemp) => {
            if (SchProductRouteResTemp.schProductRoute.cStatus === '4') {
              SchProductRouteResTemp.iSchSN = SchParam.iSchSNMax++
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
          ListSchProductRouteRes.sort((p1, p2) => this.ResTaskComparer(p1, p2))
          for (let i = 0; i < ListSchProductRouteRes.length; i++) {
            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1) {
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
      } else if (iSchBatch == -200) {
        ListSchProductRouteRes = this.schData.SchProductRouteResList.filter(
          (p1) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.iResReqQty > 0 &&
            p1.cVersionNo.trim().toLowerCase() === 'sureversion',
        )
        if (ListSchProductRouteRes.length > 0) {
          ListSchProductRouteRes.sort((p1, p2) => this.ResTaskComparer(p1, p2))
          for (let i = 0; i < ListSchProductRouteRes.length; i++) {
            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1) {
              SchProductRouteResPre = ListSchProductRouteRes[i]
              continue
            }
            if (SchParam.ExecTaskSchType === '2') {
              LastCanBegDate = SchParam.dtStart
              LastCanBegDate = this.GetTaskCanBegDate(
                ListSchProductRouteRes[i],
                LastCanBegDate,
              )
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
      } else {
        ListSchProductRouteRes = this.schData.SchProductRouteResList.filter(
          (p1) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.iSchBatch === iSchBatch &&
            p1.iResReqQty > 0,
        )
        if (ListSchProductRouteRes.length > 0) {
          ListSchProductRouteRes.forEach((SchProductRouteResTemp) => {
            SchProductRouteResTemp.iPriorityRes = SchProductRouteResTemp.iSchSN
            SchProductRouteResTemp.cDefine25 = ''
            SchProductRouteResTemp.iResPreTime = 0
            SchProductRouteResTemp.SchProductRouteResPre = null
            SchProductRouteResTemp.BScheduled = 0
            SchProductRouteResTemp.schProductRoute.BScheduled = 0
            SchProductRouteResTemp.TaskClearTask()
          })
          ListSchProductRouteRes.sort((p1, p2) => this.ResTaskComparer(p1, p2))
          for (let i = 0; i < ListSchProductRouteRes.length; i++) {
            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1) {
              SchProductRouteResPre = ListSchProductRouteRes[i]
              continue
            }
            if (SchParam.ExecTaskSchType === '2') {
              LastCanBegDate = this.GetTaskCanBegDate(
                ListSchProductRouteRes[i],
                LastCanBegDate,
              )
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
      }
    } catch (exp) {
      throw exp
    }
    return 1
  }

  private getResource(drResource: any): void {
    let SchParam = this.SchParam
    this.iResourceID = drResource['iResourceID']
    this.cResourceNo = drResource['cResourceNo']
    this.cResourceName = drResource['cResourceName']
    this.cResClsNo = drResource['cResClsNo']
    this.cResourceType = drResource['cResourceType']
    this.IResourceNumber = drResource['iResourceNumber']
    this.cResOccupyType = drResource['cResOccupyType']
    this.iPreStocks = Number(drResource['iPreStocks'])
    this.iPostStocks = Number(drResource['iPostStocks'])
    this.iUsage = Number(drResource['iUsage'])
    this.IEfficient = Number(drResource['iEfficient'])
    this.iResDifficulty = Number(drResource['iResDifficulty'])
    this.iDistributionRate = Number(drResource['iDistributionRate'])
    if (this.IEfficient == 0) this.IEfficient = 100
    this.cIsInfinityAbility = drResource['cIsInfinityAbility']
    if (this.cIsInfinityAbility == '') this.cIsInfinityAbility = '0'
    this.cWcNo = drResource['cWcNo']
    this.cDeptNo = drResource['cDeptNo']
    this.cDeptName = drResource['cDeptName']
    this.cStatus = drResource['cStatus']
    this.iSchemeID = drResource['iResourceNumber']
    this.iCacheTime = Number(drResource['iCacheTime'])
    this.iLastBatchPercent = Number(drResource['iLastBatchPercent'])
    this.cIsKey = drResource['cIsKey']
    this.iKeySchSN = drResource['iKeySchSN']
    this.cNeedChanged = drResource['cNeedChanged']
    this.iChangeTime = Number(drResource['iChangeTime']) * 60
    this.iResPreTime = Number(drResource['iResPreTime']) * 60
    this.iTurnsType = drResource['iTurnsType']
    this.iTurnsTime = Number(drResource['iTurnsTime']) * 60
    this.iLabRate = Number(drResource['iLastBatchPercent'])
    this.cTeamNo = drResource['cTeamNo']
    this.cTeamNo2 = drResource['cTeamNo2']
    this.cTeamNo3 = drResource['cTeamNo3']
    this.cBatch1Filter = drResource['cBatch1Filter']
    this.cBatch2Filter = drResource['cBatch2Filter']
    this.cBatch3Filter = drResource['cBatch3Filter']
    this.cBatch4Filter = drResource['cBatch4Filter']
    this.cBatch5Filter = drResource['cBatch5Filter']
    this.iBatchWoSeqID = Number(drResource['iBatchWoSeqID'])
    this.cBatch1WorkTime = Number(drResource['cBatch1WorkTime'])
    this.cBatch2WorkTime = Number(drResource['cBatch2WorkTime'])
    this.cBatch3WorkTime = Number(drResource['cBatch3WorkTime'])
    this.cBatch4WorkTime = Number(drResource['cBatch4WorkTime'])
    this.cBatch5WorkTime = Number(drResource['cBatch5WorkTime'])
    this.cPriorityType = drResource['cPriorityType']
    this.cResBarCode = drResource['cResBarCode']
    this.cTeamResourceNo = drResource['cTeamResourceNo']
    this.bTeamResource = drResource['bTeamResource']
    this.cSuppliyMode = drResource['cSuppliyMode']
    this.cResOperator = drResource['cResOperator']
    this.cResManager = drResource['cResManager']
    this.iOverResourceNumber = Number(drResource['iOverResourceNumber'])
    this.iLimitResourceNumber = Number(drResource['iLimitResourceNumber'])
    this.iOverEfficient = Number(drResource['iOverEfficient'])
    this.iLimitEfficient = Number(drResource['iLimitEfficient'])
    if (this.iOverEfficient == 0) this.iOverEfficient = 100
    if (this.iLimitEfficient == 0) this.iLimitEfficient = 100
    this.iResWorkersPd = Number(drResource['iResWorkersPd'])
    this.iResHoursPd = Number(drResource['iResHoursPd'])
    this.iResOverHoursPd = Number(drResource['iResOverHoursPd'])
    this.iPowerRate = Number(drResource['iPowerRate'])
    this.iOtherRate = Number(drResource['iOtherRate'])
    this.iMinWorkTime = Number(drResource['iMinWorkTime'])
    if (this.iMinWorkTime < 1) this.iMinWorkTime = SchParam.iTaskMinWorkTime
    this.cProChaType1Sort = drResource['cProChaType1Sort']
    this.FProChaType1ID = drResource['FProChaType1ID']
    this.FProChaType2ID = drResource['FProChaType2ID']
    this.FProChaType3ID = drResource['FProChaType3ID']
    this.FProChaType4ID = drResource['FProChaType4ID']
    this.FProChaType5ID = drResource['FProChaType5ID']
    this.FProChaType6ID = drResource['FProChaType6ID']
    this.cDefine1 = drResource['cResDefine1']
    this.cDefine2 = drResource['cResDefine2']
    this.cDefine3 = drResource['cResDefine3']
    this.cDefine4 = drResource['cResDefine4']
    this.cDefine5 = drResource['cResDefine5']
    this.cDefine6 = drResource['cResDefine6']
    this.cDefine7 = drResource['cResDefine7']
    this.cDefine8 = drResource['cResDefine8']
    this.cDefine9 = drResource['cResDefine9']
    this.cDefine10 = drResource['cResDefine10']
    this.cDefine11 = Number(drResource['cResDefine11'])
    this.cDefine12 = Number(drResource['cResDefine12'])
    this.cDefine13 = Number(drResource['cResDefine13'])
    this.cDefine14 = Number(drResource['cResDefine14'])
    this.cDefine15 = new DateTime(drResource['cResDefine15'])
    this.cDefine16 = new DateTime(drResource['cResDefine16'])
    this.cDayPlanShowType = drResource['cDayPlanShowType']
    this.dMaxExeDate = new DateTime(drResource['dMaxExeDate'])
  }
  GetTaskCanBegDate(
    schProductRouteRes: SchProductRouteRes,
    LastCanBegDate: DateTime,
  ): DateTime {
    let dCanBegDate = LastCanBegDate
    let dCanBegDateTemp = LastCanBegDate

    schProductRouteRes.schProductRoute.SchProductRoutePreList.forEach(
      (schProductRoutePre) => {
        if (schProductRoutePre.BScheduled === 0)
          schProductRoutePre.ProcessSchTask()
        dCanBegDateTemp = schProductRoutePre.GetNextProcessCanBegDate(
          schProductRouteRes.schProductRoute,
        )
        if (dCanBegDateTemp > dCanBegDate) dCanBegDate = dCanBegDateTemp
      },
    )

    return dCanBegDate
  }

  ResDispatchSchWo(iSchBatch: number): number {
    let SchParam = this.SchParam
    let Comparer=this.Comparer
    let SchProductRouteResPre: SchProductRouteRes | null = null
    let ListSchProductRouteRes: SchProductRouteRes[] = []
    let LastCanBegDate = new DateTime()

    ListSchProductRouteRes = this.schData.SchProductRouteResList.filter(
      (p1) => p1.cResourceNo === this.cResourceNo && p1.iResReqQty > 0,
    )

    if (ListSchProductRouteRes.length > 0) {
      ListSchProductRouteRes.forEach((SchProductRouteResTemp) => {
        SchProductRouteResTemp.cDefine25 = ''
        SchProductRouteResTemp.iResPreTime = 0
        SchProductRouteResTemp.SchProductRouteResPre = null
        SchProductRouteResTemp.BScheduled = 0
        SchProductRouteResTemp.schProductRoute.BScheduled = 0
        SchProductRouteResTemp.TaskClearTask()
      })

      if (SchParam.cProChaType1Sort === '9') {
        if (this.cProChaType1Sort === '0') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(p1.iPriorityRes, p2.iPriorityRes),
          )
        } else if (this.cProChaType1Sort === '1') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(
              p1.schProductRoute.schProductWorkItem.iWoPriorityRes,
              p2.schProductRoute.schProductWorkItem.iWoPriorityRes,
            ),
          )
        } else if (this.cProChaType1Sort === '2') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(
              p1.schProductRoute.schProductWorkItem.iPriority,
              p2.schProductRoute.schProductWorkItem.iPriority,
            ),
          )
        } else if (this.cProChaType1Sort === '3') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(p1.iSchSN, p2.iSchSN),
          )
        } else if (this.cProChaType1Sort === '4') {
          ListSchProductRouteRes.sort((p1, p2) => this.TaskComparer(p1, p2))
        } else {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(p1.dResBegDate, p2.dResBegDate),
          )
        }
      } else {
        if (SchParam.cProChaType1Sort === '0') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(p1.iPriorityRes, p2.iPriorityRes),
          )
        } else if (SchParam.cProChaType1Sort === '1') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(
              p1.schProductRoute.schProductWorkItem.dRequireDate,
              p2.schProductRoute.schProductWorkItem.dRequireDate,
            ),
          )
        } else if (SchParam.cProChaType1Sort === '2') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(
              p1.schProductRoute.schProductWorkItem.dRequireDate,
              p2.schProductRoute.schProductWorkItem.dRequireDate,
            ),
          )
        } else if (SchParam.cProChaType1Sort === '3') {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(p1.iSchSN, p2.iSchSN),
          )
        } else if (SchParam.cProChaType1Sort === '4') {
          ListSchProductRouteRes.sort((p1, p2) => this.TaskComparer(p1, p2))
        } else {
          ListSchProductRouteRes.sort((p1, p2) =>
            Comparer.default(p1.dResBegDate, p2.dResBegDate),
          )
        }
      }

      for (let i = 0; i < ListSchProductRouteRes.length; i++) {
        if (ListSchProductRouteRes[i].schProductRoute.BScheduled === 1) {
          SchProductRouteResPre = ListSchProductRouteRes[i]
          continue
        }
        ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre
        ListSchProductRouteRes[i].DispatchSchTask(
          ListSchProductRouteRes[i].iResReqQty,
          LastCanBegDate,
          SchProductRouteResPre,
        )
        LastCanBegDate = ListSchProductRouteRes[i].dResEndDate
        SchProductRouteResPre = ListSchProductRouteRes[i]
      }
    }
    return 1
  }

  ResDispatchSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: DateTime,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    bReCalWorkTime: boolean = true,
  ): number {
    let SchParam = this.SchParam
    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate
    let ldtBeginDate = new DateTime()
    let message = ''
    let ai_disWorkTime = ai_workTime
    let dtBegDate = adCanBegDate
    let dtEndDate = adCanBegDate
    let Comparer=this.Comparer
    try {
      let adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = ai_workTime
      SchParam.ldtBeginDate = new DateTime()

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
        let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${adCanBegDateTest}],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
      }

      adCanBegDate = adCanBegDateTask
      let ResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime > adCanBegDate,
      )
      ResTimeRangeList1.sort((p1, p2) =>
        Comparer.default(p1.DBegTime, p2.DBegTime),
      )
      let bFirtTime = true

      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (
          bFirtTime &&
          ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime &&
          ai_workTime > ResTimeRangeList1[i].AvailableTime
        )
          continue
        let ldtBeginDateRessource = new DateTime()
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
        if (bFirtTime) {
          dtBegDate = ResTimeRangeList1[i].DBegTime
          as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime
        }
        if (ai_workTime <= 0) break
      }

      if (ai_workTime > 0) {
        let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${
          ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
        }],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
      } else {
        as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++
        as_SchProductRouteRes.BScheduled = 1
      }
    } catch (error) {
      throw new Error(
        `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResDispatchSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID} \n\r ${error.message}`,
      )
    }
    return 1
  }

  ResSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: DateTime,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    bReCalWorkTime: boolean = true,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let SchParam = this.SchParam
    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate
    let ldtBeginDate = new DateTime()
    let message = ''
    let ai_disWorkTime = ai_workTime
    let dtBegDate = adCanBegDate
    let dtEndDate = adCanBegDate
    let Comparer=this.Comparer//
    try {
      let adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = ai_workTime
      SchParam.ldtBeginDate = new DateTime()
      ldtBeginDate = new DateTime()

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
        true,
        as_SchProductRouteResPre,
      )
      if (li_Return < 0) {
        let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${adCanBegDateTest}],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
      }

      adCanBegDate = adCanBegDateTask
      let ResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DEndTime > adCanBegDate,
      )
      ResTimeRangeList1.sort((p1, p2) =>
        Comparer.default(p1.DBegTime, p2.DBegTime),
      )
      let bFirtTime = true

      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (
          bFirtTime &&
          ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime &&
          ai_workTime > ResTimeRangeList1[i].AvailableTime
        )
          continue
        let ldtBeginDateRessource = new DateTime()
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
        if (bFirtTime) {
          dtBegDate = ResTimeRangeList1[i].DBegTime
          as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime
        }
        if (ai_workTime <= 0) break
      }

      if (ai_workTime > 0) {
        let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${
          ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
        }],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
      } else {
        as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++
        as_SchProductRouteRes.BScheduled = 1
      }
    } catch (error) {
      throw new Error(
        `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID} \n\r ${error.message}`,
      )
    }
    return 1
  }

  ResSchTaskRev(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: DateTime,
  ): number {
    let Comparer=this.Comparer
    let SchParam = this.SchParam
    let ai_workTimeTask = ai_workTime
    let adCanBegDateTask = adCanBegDate
    let dtBegDate = adCanBegDate
    let dtEndDate = adCanBegDate
    let ai_ResPreTime = 0
    let ai_CycTimeTol = 0
    SchParam.ldtBeginDate = new DateTime()

    try {
      let adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = ai_workTime

      let li_Return = this.TestResSchTask(
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
        let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${adCanBegDateTest}],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
      }

      adCanBegDate = adCanBegDateTask
      let ResTimeRangeList1 = this.ResTimeRangeList.filter(
        (p) => p.AvailableTime > 0 && p.DBegTime <= adCanBegDate,
      )
      ResTimeRangeList1.sort((p1, p2) =>
        Comparer.default(p2.DBegTime, p1.DBegTime),
      )
      let bFirtTime = true

      for (let i = 0; i < ResTimeRangeList1.length; i++) {
        if (ai_workTime == 0) break
        if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType != '2')
          ResTimeRangeList1[i].TimeSchTaskRev(
            as_SchProductRouteRes,
            ai_workTime,
            adCanBegDate,
            ai_workTimeTask,
            adCanBegDateTask,
            true,
            bFirtTime,
          )
        else
          ResTimeRangeList1[i].TimeSchTaskRevInfinite(
            as_SchProductRouteRes,
            ai_workTime,
            adCanBegDate,
            ai_workTimeTask,
            adCanBegDateTask,
            true,
            bFirtTime,
          )
      }

      if (ai_workTime > 0) {
        let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
          as_SchProductRouteRes.cInvCode
        }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
          as_SchProductRouteRes.iProcessProductID
        }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
          as_SchProductRouteRes.iResReqQty
        }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
          ai_workTime / 3600
        }],最大可排时间[${
          ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
        }],请检查工作日历或单件产能、计划数量太大!`
        throw new Error(cError)
      } else {
        as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++
        as_SchProductRouteRes.BScheduled = 1
      }
    } catch (error) {
      throw new Error(
        `订单行号：${as_SchProductRouteRes.iSchSdID}资源倒排计算时出错,位置Resource.ResSchTaskRev！工序ID号：${as_SchProductRouteRes.iProcessProductID} \n\r ${error.message}`,
      )
    }
    return 1
  }

  TestResSchTask(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: DateTime,
    adCanBegDateTask: DateTime,
    bSchRev: boolean,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    dtBegDate: DateTime,
    dtEndDate: DateTime,
    bShowTips: boolean = true,
    bReCalWorkTime: boolean = true,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let SchParam = this.SchParam
    let ai_workTimeTask = ai_workTime
    let ai_disWorkTime = ai_workTime
    let adCanBegDateTask2 = adCanBegDateTask
    let ai_ResPostTime = 0
    dtEndDate = adCanBegDate
    let Comparer=this.Comparer
    try {
      let ResTimeRangeList1: ResTimeRange[] = []
      if (!bSchRev) {
        ResTimeRangeList1 = this.ResTimeRangeList.filter(
          (p) => p.DEndTime > adCanBegDateTask2,
        )
        if (ResTimeRangeList1.length < 1) {
          if (bShowTips) {
            let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${as_SchProductRouteRes.cInvCode}]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${as_SchProductRouteRes.iProcessProductID}],请检查是否有工作日历或当前资源是资源组!`
            throw new Error(cError)
          }
          return -1
        }
        ResTimeRangeList1.sort((p1, p2) =>
          Comparer.default(p1.DBegTime, p2.DBegTime),
        )
        let bFirtTime = true
        let resTimeRangeNext: ResTimeRange | null = null

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime == 0) break
          if (i < ResTimeRangeList1.length - 1)
            resTimeRangeNext = ResTimeRangeList1[i + 1]
          else resTimeRangeNext = null

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
            SchParam.ldtBeginDate = new DateTime()
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
              `时间段排程出错,订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID} \n\r ${error.message}`,
            )
          }

          if (bFirtTime) {
            dtBegDate = adCanBegDate
            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate
          }
        }
        dtEndDate = adCanBegDate
      } else {
        ResTimeRangeList1 = this.ResTimeRangeList.filter(
          (p) => p.DBegTime < adCanBegDateTask2,
        )
        ResTimeRangeList1.sort((p1, p2) =>
          Comparer.default(p2.DBegTime, p1.DBegTime),
        )
        let bFirtTime = true

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime == 0) break
          if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType != '2')
            ResTimeRangeList1[i].TimeSchTaskRev(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )
          else
            ResTimeRangeList1[i].TimeSchTaskRevInfinite(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )

          if (bFirtTime) {
            dtEndDate = adCanBegDate
            bFirtTime = false
            dtBegDate = adCanBegDate
            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate
          } else if (i + 1 < ResTimeRangeList1.length) {
            if (
              ResTimeRangeList1[i + 1].WorkTimeRangeList.length > 0 &&
              ResTimeRangeList1[i + 1].NotWorkTime < ai_workTime
            ) {
              bFirtTime = true
              ai_workTime = ai_workTimeTask
              adCanBegDate =
                ResTimeRangeList1[i + 1].WorkTimeRangeList[
                  ResTimeRangeList1[i + 1].WorkTimeRangeList.length - 1
                ].DEndTime
            }
          }
        }
        dtBegDate = adCanBegDate
      }

      if (ai_workTime > 0) {
        if (bShowTips) {
          let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
            as_SchProductRouteRes.cInvCode
          }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
            as_SchProductRouteRes.iProcessProductID
          }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
            as_SchProductRouteRes.iResReqQty
          }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
            ai_workTime / 3600
          }],最大可排时间[${
            ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
          }],请检查工作日历或单件产能、计划数量太大!`
          throw new Error(cError)
        }
        return -1
      }
    } catch (error) {
      if (bShowTips) {
        throw new Error(
          `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID} \n\r ${error.message}`,
        )
      }
      return -1
    }
    return 1
  }

  TestResSchTaskNew(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adCanBegDate: DateTime,
    adCanBegDateTask: DateTime,
    bSchRev: boolean,
    ai_ResPreTime: number,
    ai_CycTimeTol: number,
    dtBegDate: DateTime,
    dtEndDate: DateTime,
    bShowTips: boolean = true,
    bReCalWorkTime: boolean = true,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let Comparer=this.Comparer
    let ai_workTimeTask = ai_workTime
    let ai_disWorkTime = ai_workTime
    let adCanBegDateTask2 = adCanBegDateTask
    let ai_ResPostTime = 0
    dtEndDate = adCanBegDate

    try {
      let ResTimeRangeList1: ResTimeRange[] = []
      if (!bSchRev) {
        ResTimeRangeList1 = this.ResTimeRangeList.filter(
          (p) => p.DEndTime > adCanBegDateTask2,
        )
        ResTimeRangeList1.sort((p1, p2) =>
          Comparer.default(p1.DBegTime, p2.DBegTime),
        )
        let bFirtTime = true

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime == 0) break
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
          )
          if (bFirtTime) {
            dtBegDate = adCanBegDate
            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate
          }
        }
        dtEndDate = adCanBegDate
      } else {
        ResTimeRangeList1 = this.ResTimeRangeList.filter(
          (p) => p.DBegTime < adCanBegDateTask2,
        )
        ResTimeRangeList1.sort((p1, p2) =>
          Comparer.default(p2.DBegTime, p1.DBegTime),
        )
        let bFirtTime = true

        for (let i = 0; i < ResTimeRangeList1.length; i++) {
          if (ai_workTime == 0) break
          if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType != '2')
            ResTimeRangeList1[i].TimeSchTaskRev(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )
          else
            ResTimeRangeList1[i].TimeSchTaskRevInfinite(
              as_SchProductRouteRes,
              ai_workTime,
              adCanBegDate,
              ai_workTimeTask,
              adCanBegDateTask,
              false,
              bFirtTime,
            )

          if (bFirtTime) dtEndDate = adCanBegDate
        }
        dtBegDate = adCanBegDate
      }

      if (ai_workTime > 0) {
        if (bShowTips) {
          let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
            as_SchProductRouteRes.cInvCode
          }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
            as_SchProductRouteRes.iProcessProductID
          }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
            as_SchProductRouteRes.iResReqQty
          }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
            ai_workTime / 3600
          }],最大可排时间[${
            ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
          }],请检查工作日历或单件产能、计划数量太大!`
          throw new Error(cError)
        }
        return -1
      }
    } catch (error) {
      if (bShowTips) {
        throw new Error(
          `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID} \n\r ${error.message}`,
        )
      }
      return -1
    }
    return 1
  }
  // TestResSchTaskNew(
  //   as_SchProductRouteRes: SchProductRouteRes,
  //   ai_workTime: number,
  //   adCanBegDate: DateTime,
  //   adCanBegDateTask: DateTime,
  //   bSchRev: boolean,
  //   ai_ResPreTime: number,
  //   ai_CycTimeTol: number,
  //   dtBegDate: DateTime,
  //   dtEndDate: DateTime,
  //   bShowTips: boolean = true,
  //   bReCalWorkTime: boolean = true,
  //   as_SchProductRouteResPre: SchProductRouteRes | null = null,
  // ): number {
  //   let ai_workTimeTask = ai_workTime
  //   let ai_disWorkTime = ai_workTime
  //   let adCanBegDateTask2 = adCanBegDateTask
  //   let ai_ResPostTime = 0
  //   dtEndDate = adCanBegDate

  //   if (
  //     as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID &&
  //     as_SchProductRouteRes.iSchSdID === SchParam.iSchSdID
  //   ) {
  //     let i = 1
  //   }

  //   try {
  //     let ResTimeRangeList1: ResTimeRange[] = []
  //     if (!bSchRev) {
  //       ResTimeRangeList1 = ResTimeRangeList.filter(
  //         (p) => p.DEndTime > adCanBegDateTask2,
  //       )
  //       ResTimeRangeList1.sort((p1, p2) =>
  //         Comparer.default(p1.DBegTime, p2.DBegTime),
  //       )
  //       let bFirtTime = true
  //       for (let i = 0; i < ResTimeRangeList1.length; i++) {
  //         if (ai_workTime === 0) break
  //         ResTimeRangeList1[i].TimeSchTask(
  //           as_SchProductRouteRes,
  //           ai_workTime,
  //           adCanBegDate,
  //           ai_workTimeTask,
  //           adCanBegDateTask,
  //           false,
  //           ai_ResPreTime,
  //           ai_CycTimeTol,
  //           bFirtTime,
  //           ai_disWorkTime,
  //           bReCalWorkTime,
  //         )
  //         if (bFirtTime) {
  //           dtBegDate = adCanBegDate
  //           as_SchProductRouteRes.dResLeanBegDate = adCanBegDate
  //         }
  //       }
  //       dtEndDate = adCanBegDate
  //     } else {
  //       ResTimeRangeList1 = ResTimeRangeList.filter(
  //         (p) => p.DBegTime < adCanBegDateTask2,
  //       )
  //       ResTimeRangeList1.sort((p1, p2) =>
  //         Comparer.default(p2.DBegTime, p1.DBegTime),
  //       )
  //       let bFirtTime = true
  //       for (let i = 0; i < ResTimeRangeList1.length; i++) {
  //         if (ai_workTime === 0) break
  //         if (
  //           as_SchProductRouteRes.schProductRoute.schProduct.cSchType !== '2'
  //         ) {
  //           ResTimeRangeList1[i].TimeSchTaskRev(
  //             as_SchProductRouteRes,
  //             ai_workTime,
  //             adCanBegDate,
  //             ai_workTimeTask,
  //             adCanBegDateTask,
  //             false,
  //             bFirtTime,
  //           )
  //         } else {
  //           ResTimeRangeList1[i].TimeSchTaskRevInfinite(
  //             as_SchProductRouteRes,
  //             ai_workTime,
  //             adCanBegDate,
  //             ai_workTimeTask,
  //             adCanBegDateTask,
  //             false,
  //             bFirtTime,
  //           )
  //         }
  //         if (bFirtTime) dtEndDate = adCanBegDate
  //       }
  //       dtBegDate = adCanBegDate
  //     }

  //     if (ai_workTime > 0) {
  //       if (bShowTips) {
  //         let cError = `订单行号：${as_SchProductRouteRes.iSchSdID} ,加工物料[${
  //           as_SchProductRouteRes.cInvCode
  //         }]在资源[${as_SchProductRouteRes.cResourceNo}]无法排下,任务号[${
  //           as_SchProductRouteRes.iProcessProductID
  //         }],单件产能[${as_SchProductRouteRes.iCapacity}],加工数量[${
  //           as_SchProductRouteRes.iResReqQty
  //         }],加工工时[${ai_workTimeTask / 3600}],未排工时[${
  //           ai_workTime / 3600
  //         }],最大可排时间[${
  //           ResTimeRangeList1[ResTimeRangeList1.length - 1].DEndTime
  //         }],请检查工作日历或单件产能、计划数量太大!`
  //         throw new Error(cError)
  //       }
  //       return -1
  //     }
  //   } catch (error) {
  //     if (bShowTips) {
  //       throw new Error(
  //         `订单行号：${as_SchProductRouteRes.iSchSdID}资源正排计算时出错,位置Resource.ResSchTask！工序ID号：${as_SchProductRouteRes.iProcessProductID}\n\r ${error.message}`,
  //       )
  //     }
  //     return -1
  //   }
  //   return 1
  // }

  GetEarlyStartDate(adStartDate: DateTime, bSchRev: boolean): DateTime {
    let ListReturn: ResTimeRange[] = []
    let Comparer=this.Comparer
    let iCanSchTime = 0
    let ResTimeRangeBeg: ResTimeRange | null = null
    let ResTimeRangeEnd: ResTimeRange | null = null
    let dtBegDate: DateTime
    let TaskTimeRangeBeg: TaskTimeRange | null = null
    let TaskTimeRangeEnd: TaskTimeRange | null = null
    let dtEndDate = adStartDate
    if (!bSchRev) {
      ListReturn = this.ResTimeRangeList.filter(
        (p) => p.DEndTime > adStartDate && p.AvailableTime > 0,
      )
      ListReturn.sort((p1, p2) => Comparer.default(p1.DBegTime, p2.DBegTime))
      if (ListReturn.length > 0) {
        dtEndDate = ListReturn[0].DEndTime
      }
    } else {
      ListReturn = this.ResTimeRangeList.filter(
        (p) => p.DBegTime < adStartDate && p.AvailableTime > 0,
      )
      ListReturn.sort((p1, p2) => Comparer.default(p2.DBegTime, p1.DBegTime))
      if (ListReturn.length > 0) {
        dtEndDate = ListReturn[0].DBegTime
      }
    }
    return dtEndDate
  }

  GetSchStartDate(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adStartDate: DateTime,
    bSchRev: boolean,
    dtEndDate: DateTime,
  ): DateTime {
    let SchParam = this.SchParam
    let ListReturn: ResTimeRange[] = new Array(10)
    let ai_workTimeOld = ai_workTime
    let iCanSchTime = 0
    let ResTimeRangeBeg: ResTimeRange | null = null
    let ResTimeRangeEnd: ResTimeRange | null = null
    let dtBegDate: DateTime
    let TaskTimeRangeBeg: TaskTimeRange | null = null
    let TaskTimeRangeEnd: TaskTimeRange | null = null

    if (
      as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID
    ) {
      let i = 1
    }

    if (!bSchRev) {
      return TaskTimeRangeBeg!.DBegTime
    } else {
      return TaskTimeRangeBeg!.DEndTime
    }
  }

  GetChangeTime(
    as_SchProductRouteRes: SchProductRouteRes,
    ai_workTime: number,
    adStartDate: DateTime,
    iCycTimeTol: number,
    bSchdule: boolean,
    as_SchProductRouteResPre: SchProductRouteRes | null = null,
  ): number {
    let Comparer=this.Comparer
    let SchParam = this.SchParam
    let iPreTime = 0
    if (
      this.cNeedChanged === '1' &&
      as_SchProductRouteRes.cWoNo !== '' &&
      as_SchProductRouteRes.schProductRoute.schProduct.cType === 'PSH'
    )
      return 0
    if (as_SchProductRouteResPre === null) {
      let ListSchProductRouteResAll = this.schData.SchProductRouteResList.filter(
        (p1) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.iResReqQty > 0 &&
          p1.BScheduled === 1 &&
          p1.dResBegDate <= adStartDate,
      )
      ListSchProductRouteResAll.sort((p1, p2) =>
        Comparer.default(p2.dResBegDate, p1.dResBegDate),
      )
      if (ListSchProductRouteResAll.length > 0) {
        as_SchProductRouteResPre = ListSchProductRouteResAll[0]
      }
    }
    let cTimeNote = ''
    if (SchParam.ResProcessCharCount > 0) {
      iPreTime = this.GetChangeTime(
        as_SchProductRouteRes,
        ai_workTime,
        //@ts-ignore
        as_SchProductRouteResPre,
        iCycTimeTol,
        bSchdule,
      )
    }
    if (iPreTime > 0) {
      iPreTime += parseInt(this.iResPreTime.toString())
      cTimeNote += ` 资源前准备时间:[${this.iResPreTime.toString()}]`
    }
    if (as_SchProductRouteResPre !== null) {
      if (
        as_SchProductRouteRes.schProductRoute.cWorkItemNo !==
        as_SchProductRouteResPre.schProductRoute.cWorkItemNo
      ) {
        iPreTime += parseInt(this.iChangeTime.toString())
        cTimeNote += ` 换料时间:[${this.iChangeTime.toString()}]`
      }
    } else {
      iPreTime += parseInt(this.iChangeTime.toString())
      cTimeNote += ` 换料时间:[${this.iChangeTime.toString()}]`
    }
    if (as_SchProductRouteRes.iResPreTimeOld > 0) {
      iPreTime += as_SchProductRouteRes.iResPreTimeOld
      cTimeNote += ` 工艺路线资源前准备时间:[${as_SchProductRouteRes.iResPreTimeOld.toString()}]`
    }
    as_SchProductRouteRes.cDefine25 = cTimeNote
    return iPreTime
  }

  // GetChangeTime(
  //   as_SchProductRouteRes: SchProductRouteRes,
  //   ai_workTime: number,
  //   as_SchProductRouteResPre: SchProductRouteRes,
  //   iCycTimeTol: number,
  //   bSchdule: boolean,
  // ): number {
  //   let iPreTime = 0
  //   iCycTimeTol = 0
  //   let iCycTime: number[] = new Array(12).fill(0)
  //   let iChaValue: number[] = new Array(12).fill(0)

  //   try {
  //     if (as_SchProductRouteResPre !== null) {
  //       let cTimeNote = `任务号排产顺序:[${SchParam.iSchSNMax}]前任务号:[${as_SchProductRouteResPre.iSchSdID}][${as_SchProductRouteResPre.iProcessProductID}],排产顺序：[${as_SchProductRouteResPre.iSchSN}]`
  //       if (
  //         this.FProChaType1ID !== '-1' &&
  //         this.FProChaType1ID !== '' &&
  //         as_SchProductRouteRes.resChaValue1 !== null &&
  //         SchParam.ResProcessCharCount > 0
  //       ) {
  //         cTimeNote += ` 工艺特征1:${as_SchProductRouteRes.resChaValue1.FResChaValueNo}`
  //         if (as_SchProductRouteResPre.resChaValue1 !== null)
  //           cTimeNote += ` 前工艺特征1:${as_SchProductRouteResPre.resChaValue1.FResChaValueNo}`
  //         iChaValue[0] = as_SchProductRouteRes.resChaValue1.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           as_SchProductRouteResPre.resChaValue1,
  //           ai_workTime,
  //           iCycTime,
  //           0,
  //           bSchdule,
  //           as_SchProductRouteResPre,
  //         )
  //       }
  //       if (
  //         this.FProChaType2ID !== '-1' &&
  //         this.FProChaType2ID !== '' &&
  //         as_SchProductRouteRes.resChaValue2 !== null &&
  //         SchParam.ResProcessCharCount > 1
  //       ) {
  //         cTimeNote += ` 工艺特征2:${as_SchProductRouteRes.resChaValue2.FResChaValueNo}`
  //         if (as_SchProductRouteResPre.resChaValue2 !== null)
  //           cTimeNote += ` 前工艺特征2:${as_SchProductRouteResPre.resChaValue2.FResChaValueNo}`
  //         iChaValue[1] = as_SchProductRouteRes.resChaValue2.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           as_SchProductRouteResPre.resChaValue2,
  //           ai_workTime,
  //           iCycTime,
  //           1,
  //           bSchdule,
  //           as_SchProductRouteResPre,
  //         )
  //       }
  //       if (
  //         this.FProChaType3ID !== '-1' &&
  //         this.FProChaType3ID !== '' &&
  //         as_SchProductRouteRes.resChaValue3 !== null &&
  //         SchParam.ResProcessCharCount > 2
  //       ) {
  //         cTimeNote += ` 工艺特征3:${as_SchProductRouteRes.resChaValue3.FResChaValueNo}`
  //         if (as_SchProductRouteResPre.resChaValue3 !== null)
  //           cTimeNote += ` 前工艺特征3:${as_SchProductRouteResPre.resChaValue3.FResChaValueNo}`
  //         iChaValue[2] = as_SchProductRouteRes.resChaValue3.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           as_SchProductRouteResPre.resChaValue3,
  //           ai_workTime,
  //           iCycTime,
  //           2,
  //           bSchdule,
  //           as_SchProductRouteResPre,
  //         )
  //       }
  //       if (
  //         this.FProChaType4ID !== '-1' &&
  //         this.FProChaType4ID !== '' &&
  //         as_SchProductRouteRes.resChaValue4 !== null &&
  //         SchParam.ResProcessCharCount > 3
  //       ) {
  //         cTimeNote += ` 工艺特征4:${as_SchProductRouteResPre.resChaValue4.FResChaValueNo}`
  //         if (as_SchProductRouteResPre.resChaValue4 !== null)
  //           cTimeNote += ` 前工艺特征4:${as_SchProductRouteResPre.resChaValue4.FResChaValueNo}`
  //         iChaValue[3] = as_SchProductRouteRes.resChaValue4.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           as_SchProductRouteResPre.resChaValue4,
  //           ai_workTime,
  //           iCycTime,
  //           3,
  //           bSchdule,
  //           as_SchProductRouteResPre,
  //         )
  //       }
  //       if (
  //         this.FProChaType5ID !== '-1' &&
  //         this.FProChaType5ID !== '' &&
  //         as_SchProductRouteRes.resChaValue5 !== null &&
  //         SchParam.ResProcessCharCount > 4
  //       ) {
  //         cTimeNote += ` 工艺特征5:${as_SchProductRouteResPre.resChaValue5.FResChaValueNo}`
  //         if (as_SchProductRouteResPre.resChaValue5 !== null)
  //           cTimeNote += ` 前工艺特征5:${as_SchProductRouteResPre.resChaValue5.FResChaValueNo}`
  //         iChaValue[4] = as_SchProductRouteRes.resChaValue5.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           as_SchProductRouteResPre.resChaValue5,
  //           ai_workTime,
  //           iCycTime,
  //           4,
  //           bSchdule,
  //           as_SchProductRouteResPre,
  //         )
  //       }
  //       if (
  //         this.FProChaType6ID !== '-1' &&
  //         this.FProChaType6ID !== '' &&
  //         as_SchProductRouteRes.resChaValue6 !== null &&
  //         SchParam.ResProcessCharCount > 5
  //       ) {
  //         cTimeNote += ` 工艺特征6:${as_SchProductRouteRes.resChaValue6.FResChaValueNo}`
  //         if (as_SchProductRouteResPre.resChaValue6 !== null)
  //           cTimeNote += ` 前工艺特征6:${as_SchProductRouteResPre.resChaValue6.FResChaValueNo}`
  //         iChaValue[5] = as_SchProductRouteRes.resChaValue6.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           as_SchProductRouteResPre.resChaValue6,
  //           ai_workTime,
  //           iCycTime,
  //           5,
  //           bSchdule,
  //           as_SchProductRouteResPre,
  //         )
  //       }
  //     } else {
  //       if (
  //         this.FProChaType1ID !== '-1' &&
  //         this.FProChaType1ID !== '' &&
  //         as_SchProductRouteRes.resChaValue1 !== null
  //       ) {
  //         iChaValue[0] = as_SchProductRouteRes.resChaValue1.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           null,
  //           ai_workTime,
  //           iCycTime,
  //           0,
  //           bSchdule,
  //           null,
  //         )
  //       }
  //       if (
  //         this.FProChaType2ID !== '-1' &&
  //         this.FProChaType2ID !== '' &&
  //         as_SchProductRouteRes.resChaValue2 !== null
  //       ) {
  //         iChaValue[1] = as_SchProductRouteRes.resChaValue2.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           null,
  //           ai_workTime,
  //           iCycTime,
  //           1,
  //           bSchdule,
  //           null,
  //         )
  //       }
  //       if (
  //         this.FProChaType3ID !== '-1' &&
  //         this.FProChaType3ID !== '' &&
  //         as_SchProductRouteRes.resChaValue3 !== null
  //       ) {
  //         iChaValue[2] = as_SchProductRouteRes.resChaValue3.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           null,
  //           ai_workTime,
  //           iCycTime,
  //           2,
  //           bSchdule,
  //           null,
  //         )
  //       }
  //       if (
  //         this.FProChaType4ID !== '-1' &&
  //         this.FProChaType4ID !== '' &&
  //         as_SchProductRouteRes.resChaValue4 !== null
  //       ) {
  //         iChaValue[3] = as_SchProductRouteRes.resChaValue4.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           null,
  //           ai_workTime,
  //           iCycTime,
  //           3,
  //           bSchdule,
  //           null,
  //         )
  //       }
  //       if (
  //         this.FProChaType5ID !== '-1' &&
  //         this.FProChaType5ID !== '' &&
  //         as_SchProductRouteRes.resChaValue5 !== null
  //       ) {
  //         iChaValue[4] = as_SchProductRouteRes.resChaValue5.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           null,
  //           ai_workTime,
  //           iCycTime,
  //           4,
  //           bSchdule,
  //           null,
  //         )
  //       }
  //       if (
  //         this.FProChaType6ID !== '-1' &&
  //         this.FProChaType6ID !== '' &&
  //         as_SchProductRouteRes.resChaValue6 !== null
  //       ) {
  //         iChaValue[5] = as_SchProductRouteRes.resChaValue6.GetChaValueChangeTime(
  //           as_SchProductRouteRes,
  //           null,
  //           ai_workTime,
  //           iCycTime,
  //           5,
  //           bSchdule,
  //           null,
  //         )
  //       }
  //     }

  //     for (let i = 0; i < 7; i++) {
  //       if (iChaValue[i] > 0) {
  //         cTimeNote += ` 特征${i + 1}: 前准备时间[${iChaValue[i]}]`
  //         iPreTime += iChaValue[i]
  //       }
  //       if (iCycTime[i] > 0) {
  //         cTimeNote += ` 特征${i + 1}: 换刀时间[${iCycTime[i]}]`
  //         iCycTimeTol += iCycTime[i]
  //       }
  //     }
  //   } catch (exp) {
  //     throw new Error(
  //       `订单行号：${as_SchProductRouteRes.iSchSdID}出错位置:Resource.GetChangeTime 工艺特征换产时间计算出错！`,
  //     )
  //     return -1
  //   }
  //   return iPreTime
  // }

  KeyResSchTask(iCount: number = 1): number {
    let SchParam = this.SchParam
    let Comparer=this.Comparer
    this.getNotSchTask()
    if (this.schProductRouteResList.length < 1) return 1
    this.schData.SchProductList.sort((p1, p2) =>
      Comparer.default(p1.iSchPriority, p2.iSchPriority),
    )
    let iSchPriority = this.schData.SchProductList[
      this.schData.SchProductList.length - 1
    ].iSchPriority
    if (iSchPriority < 0) iSchPriority = 0
    let as_SchProductRouteResLast: SchProductRouteRes | null = null

    this.schProductRouteResList.forEach((schProductRouteRes) => {
      if (
        schProductRouteRes.iProcessProductID === SchParam.iProcessProductID &&
        schProductRouteRes.iSchSdID === SchParam.iSchSdID
      ) {
        let i = 1
      }
      schProductRouteRes.schProductRoute.ProcessSchTaskPre(false)
      if (SchParam.bSchKeyBySN === '1' && as_SchProductRouteResLast !== null) {
        schProductRouteRes.schProductRoute.dEarlyBegDate =
          as_SchProductRouteResLast.dResEndDate
        schProductRouteRes.dEarliestStartTime =
          as_SchProductRouteResLast.dResEndDate
      }
      schProductRouteRes.schProductRoute.ProcessSchTaskNext('2')
      if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0) {
        iSchPriority++
        schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority
      }
      as_SchProductRouteResLast = schProductRouteRes
    })

    return 1
  }

  KeyResSchTaskPre(): number {
    let Comparer=this.Comparer
    let schProductRouteResList = this.schData.SchProductRouteResList.filter(
      (p1) =>
        p1.cResourceNo === this.cResourceNo && p1.iSchBatch === this.iSchBatch,
    )
    schProductRouteResList.sort((p1, p2) => this.TaskComparer(p1, p2))
    this.schData.SchProductList.sort((p1, p2) =>
      Comparer.default(p1.iSchPriority, p2.iSchPriority),
    )
    let iSchPriority = this.schData.SchProductList[
      this.schData.SchProductList.length - 1
    ].iSchPriority
    if (iSchPriority < 0) iSchPriority = 0
    return 1
  }

  KeyResSchTaskSingle(
    as_iTurmsTime: number,
    as_SchProductRouteResLast: SchProductRouteRes,
  ): number {
    let Comparer=this.Comparer
    let SchParam = this.SchParam
    this.schData.SchProductList.sort((p1, p2) =>
      Comparer.default(p1.iSchPriority, p2.iSchPriority),
    )
    let iSchPriority = this.schData.SchProductList[
      this.schData.SchProductList.length - 1
    ].iSchPriority
    if (iSchPriority < 0) iSchPriority = 0

    if (this.iTurnsType === '1') {
      if (as_iTurmsTime < 0) as_iTurmsTime = this.iTurnsTime
      if (this.iTurnsTime <= 0 && as_SchProductRouteResLast !== null) {
        as_iTurmsTime = as_SchProductRouteResLast.iResPreTime
        if (
          as_SchProductRouteResLast.resource.cResourceNo !== this.cResourceNo &&
          as_SchProductRouteResLast.resource.bScheduled === 1
        ) {
          as_iTurmsTime = 99999999999
        }
      }
      if (as_iTurmsTime === 0) return 1
      let iTolWorkTime = 0

      while (iTolWorkTime < as_iTurmsTime) {
        let schProductRouteRes: any = this.schProductRouteResList.find(
          (p1) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.schProductRoute.BScheduled === 0,
        )
        if (schProductRouteRes === null) {
          SchParam.Debug(
            `关键资源${
              this.cResourceNo
            }轮换排产,轮换批次[${this.iBatch.toString()}],总工时[${(
              iTolWorkTime / 60
            ).toString()}],所有任务已排完.`,
            '关键资源轮换生产',
          )
          this.iBatch++
          return -1
        }
        schProductRouteRes.schProductRoute.ProcessSchTaskPre(false)
        if (
          SchParam.bSchKeyBySN === '1' &&
          as_SchProductRouteResLast !== null
        ) {
          schProductRouteRes.schProductRoute.dEarlyBegDate =
            as_SchProductRouteResLast.dResEndDate
          schProductRouteRes.dEarliestStartTime =
            as_SchProductRouteResLast.dResEndDate
        }
        schProductRouteRes.schProductRoute.ProcessSchTaskNext('2')
        if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0) {
          iSchPriority++
          schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority
        }
        schProductRouteRes.iBatch = this.iBatch
        iTolWorkTime += schProductRouteRes.iResRationHour
        as_SchProductRouteResLast = schProductRouteRes
      }
      this.iBatch++
      SchParam.Debug(
        `关键资源${
          this.cResourceNo
        }轮换排产,轮换批次[${this.iBatch.toString()}],总工时[${(
          iTolWorkTime / 60
        ).toString()}]`,
        '关键资源轮换生产',
      )
    } else if (this.iTurnsType === '2') {
      if (as_iTurmsTime < 0) as_iTurmsTime = this.iTurnsTime
      let iTolWorks = 0
      as_iTurmsTime = 1

      while (iTolWorks < as_iTurmsTime || this.iTurnsTime === 0) {
        let schProductRouteRes: any = this.schProductRouteResList.find(
          (p1) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.schProductRoute.BScheduled === 0,
        )
        if (schProductRouteRes === null) {
          this.iBatch++
          return -1
        }
        schProductRouteRes.schProductRoute.ProcessSchTaskPre(false)
        if (
          SchParam.bSchKeyBySN === '1' &&
          as_SchProductRouteResLast !== null
        ) {
          schProductRouteRes.schProductRoute.dEarlyBegDate =
            as_SchProductRouteResLast.dResEndDate
          schProductRouteRes.dEarliestStartTime =
            as_SchProductRouteResLast.dResEndDate
        }
        schProductRouteRes.schProductRoute.ProcessSchTaskNext('2')
        if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0) {
          iSchPriority++
          schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority
        }
        iTolWorks += 1
        as_SchProductRouteResLast = schProductRouteRes
        if (this.cBatch2WorkTime < 10) this.cBatch2WorkTime = 10
        if (
          this.iTurnsTime === 0 &&
          as_SchProductRouteResLast.iResPreTime >= this.cBatch2WorkTime * 60
        ) {
          this.iBatch++
          return 1
        }
      }
      this.iBatch++
    }
    return 1
  }
  KeyResBatch(): number {
    let schData = this.schData
    let as_SchProductRouteResLast = new SchProductRouteRes()
    try {
      let schProductRouteResListNoTest = schData.SchProductRouteResList.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.iWoSeqID === this.iBatchWoSeqID &&
          p1.iSchBatch === this.iSchBatch,
      )
      let schProductRouteResListNoSeq = schData.SchProductRouteResList.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.schProductRoute.BScheduled === 0 &&
          p1.iSchBatch === this.iSchBatch,
      )
      let schProductRouteResListNo = schData.SchProductRouteResList.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.schProductRoute.BScheduled === 0 &&
          p1.iWoSeqID === this.iBatchWoSeqID &&
          p1.iSchBatch === this.iSchBatch,
      )

      if (schProductRouteResListNo.length < 1) return -1

      schProductRouteResListNo.forEach(
        (schProductRouteRes: SchProductRouteRes) => {
          schProductRouteRes.schProductRoute.ProcessSchTaskPre(false)
          schProductRouteRes.schProductRoute.GetRouteEarlyBegDate()
          schProductRouteRes.dEarliestStartTime =
            schProductRouteRes.schProductRoute.dEarlyBegDate
        },
      )

      schProductRouteResListNo.sort(
        (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
          this.TaskComparer(p1, p2),
      )

      if (this.iSchBatch === 1) {
        let schProductRouteResListFirst = schData.SchProductRouteResList.filter(
          (p1: SchProductRouteRes) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.iWoSeqID === this.iBatchWoSeqID &&
            p1.iSchBatch === this.iSchBatch &&
            p1.iBatch === 1,
        )
        if (schProductRouteResListFirst.length > 0) {
          this.KeyResBatchSchTaskSubFirst(
            1,
            as_SchProductRouteResLast,
            schProductRouteResListFirst,
          )
        }
      }

      if (
        this.cBatch2Filter === '' &&
        this.cBatch3Filter === '' &&
        this.cBatch4Filter === '' &&
        this.cBatch5Filter === ''
      ) {
        this.KeyResBatchSchTask(
          this.cBatch1Filter,
          this.iBatchWoSeqID,
          true,
          this.iTurnsTime,
          schProductRouteResListNo,
          as_SchProductRouteResLast,
        )
      } else {
        let bExist = true
        let iBatchCount = 1
        while (bExist) {
          if (this.cBatch1Filter !== '')
            this.KeyResBatchSchTask(
              this.cBatch1Filter,
              this.iBatchWoSeqID,
              false,
              this.cBatch1WorkTime * 60,
              schProductRouteResListNo,
              as_SchProductRouteResLast,
            )
          if (this.cBatch2Filter !== '')
            this.KeyResBatchSchTask(
              this.cBatch2Filter,
              this.iBatchWoSeqID,
              false,
              this.cBatch2WorkTime * 60,
              schProductRouteResListNo,
              as_SchProductRouteResLast,
            )
          if (this.cBatch3Filter !== '')
            this.KeyResBatchSchTask(
              this.cBatch3Filter,
              this.iBatchWoSeqID,
              false,
              this.cBatch3WorkTime * 60,
              schProductRouteResListNo,
              as_SchProductRouteResLast,
            )
          if (this.cBatch4Filter !== '')
            this.KeyResBatchSchTask(
              this.cBatch4Filter,
              this.iBatchWoSeqID,
              false,
              this.cBatch4WorkTime * 60,
              schProductRouteResListNo,
              as_SchProductRouteResLast,
            )
          if (this.cBatch5Filter !== '')
            this.KeyResBatchSchTask(
              this.cBatch5Filter,
              this.iBatchWoSeqID,
              false,
              this.cBatch5WorkTime * 60,
              schProductRouteResListNo,
              as_SchProductRouteResLast,
            )

          iBatchCount++
          let schProductRouteResListNo2 = schProductRouteResListNo.filter(
            (p1: SchProductRouteRes) =>
              p1.cResourceNo === this.cResourceNo &&
              p1.schProductRoute.BScheduled === 0,
          )
          if (schProductRouteResListNo2.length < 1) {
            bExist = false
            break
          }
          if (iBatchCount > 20) {
            this.KeyResBatchSchTask(
              '',
              this.iBatchWoSeqID,
              true,
              0,
              schProductRouteResListNo,
              as_SchProductRouteResLast,
            )
            break
          }
        }
        this.KeyResBatchSchTask(
          '',
          this.iBatchWoSeqID,
          true,
          0,
          schProductRouteResListNo,
          as_SchProductRouteResLast,
        )
      }
    } catch (exp2) {
      throw exp2
    }
    return 1
  }

  KeyResBatchSchTask(
    cBatchFilter: string,
    iBatchWoSeqID: number,
    bSchAll: boolean,
    as_iTurmsTime: number,
    as_schProductRouteResListNo: SchProductRouteRes[],
    as_SchProductRouteResLast: SchProductRouteRes,
  ): number {
    let schProductRouteResListNo = new Array<SchProductRouteRes>(10)
    let schProductRouteResListLast = new Array<SchProductRouteRes>(10)
let Comparer=this.Comparer
    if (cBatchFilter !== '') {
      schProductRouteResListNo = as_schProductRouteResListNo.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.schProductRoute.BScheduled === 0 &&
          cBatchFilter.indexOf(p1.FResChaValue1ID) >= 0 &&
          p1.iWoSeqID === iBatchWoSeqID &&
          p1.iSchBatch === this.iSchBatch,
      )
    } else {
      schProductRouteResListNo = as_schProductRouteResListNo.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.schProductRoute.BScheduled === 0 &&
          p1.iWoSeqID === iBatchWoSeqID &&
          p1.iSchBatch === this.iSchBatch,
      )
    }

    if (schProductRouteResListNo.length < 1) return -1

    schProductRouteResListNo.sort(
      (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
        Comparer.Default.Compare(
          p1.schProductRoute.dEarlyBegDate,
          p2.schProductRoute.dEarlyBegDate,
        ),
    )

    let iTolWorkTime = 0
    if (as_iTurmsTime < 1) as_iTurmsTime = this.iTurnsTime
    if (as_iTurmsTime < 1) as_iTurmsTime = 240 * 60

    for (let i = 0; i < schProductRouteResListNo.length; i++) {
      let schProductRouteRes = schProductRouteResListNo[i]
      schProductRouteRes.iResRationHour =
        schProductRouteRes.iResReqQty * schProductRouteRes.iCapacity
      iTolWorkTime += schProductRouteRes.iResRationHour

      if (iTolWorkTime > as_iTurmsTime) {
        as_SchProductRouteResLast = schProductRouteRes
        this.SchParam.Debug(
          `资源编号${this.cResourceNo}，分批${
            this.iBatch
          },分批条件${cBatchFilter}，分批工时${(
            as_iTurmsTime / 60
          ).toString()}，累计工时${(
            iTolWorkTime / 60
          ).toString()}，总任务数${schProductRouteResListNo.length.toString()}`,
          '资源分批',
        )
        this.KeyResBatchSchTaskSub(
          this.iBatch,
          as_SchProductRouteResLast,
          as_schProductRouteResListNo,
          cBatchFilter,
        )
        iTolWorkTime = 0
        this.iBatch++
        if (!bSchAll) return this.iBatch
      }

      if (i === schProductRouteResListNo.length - 1) {
        as_SchProductRouteResLast = schProductRouteRes
        this.SchParam.Debug(
          `资源编号${this.cResourceNo}最后一批，分批${
            this.iBatch
          },分批条件${cBatchFilter}，分批工时${(
            as_iTurmsTime / 60
          ).toString()}，累计工时${(
            iTolWorkTime / 60
          ).toString()}，总任务数${schProductRouteResListNo.length.toString()}`,
          '资源分批',
        )
        this.KeyResBatchSchTaskSub(
          this.iBatch,
          as_SchProductRouteResLast,
          as_schProductRouteResListNo,
          cBatchFilter,
        )
        this.iBatch++
        if (!bSchAll) return this.iBatch
      }
    }
    return 1
  }

  KeyResBatchSchTaskSub(
    as_iBatch: number,
    as_SchProductRouteResLast: SchProductRouteRes,
    as_schProductRouteResListNo: SchProductRouteRes[],
    cBatchFilter: string,
  ): number {
    let Comparer=this.Comparer
    let schProductRouteResListBatch = as_schProductRouteResListNo.filter(
      (p1: SchProductRouteRes) =>
        p1.cResourceNo === this.cResourceNo &&
        p1.schProductRoute.BScheduled === 0 &&
        p1.iWoSeqID === this.iBatchWoSeqID &&
        p1.iBatch === as_iBatch &&
        p1.iSchBatch === this.iSchBatch,
    )

    if (schProductRouteResListBatch.length < 1) return -1

    let dEndDateTemp = this.SchParam.dtToday
    if (this.dBatchEndDate > this.SchParam.dtToday) {
      dEndDateTemp = this.dBatchEndDate
    }

    schProductRouteResListBatch.forEach(
      (schProductRouteRes: SchProductRouteRes) => {
        dEndDateTemp = schProductRouteRes.schProductRoute.dEarlyBegDate
        if (dEndDateTemp > this.dBatchBegDate) {
          this.dBatchBegDate = dEndDateTemp
        }
      },
    )

    this.dBatchBegDate = new DateTime(
      this.dBatchBegDate.getTime() + this.iPreStocks * 60000,
    )
    this.dBatchBegDate = new DateTime(
      this.dBatchBegDate.getTime() + this.iPostStocks * 60000,
    )

    schProductRouteResListBatch.forEach(
      (schProductRouteRes: SchProductRouteRes) => {
        schProductRouteRes.schProductRoute.dEarlyBegDate = this.dBatchBegDate
      },
    )

    schProductRouteResListBatch.forEach(
      (schProductRouteRes: SchProductRouteRes) => {
        schProductRouteRes.schProductRoute.ProcessSchTask()
        let schProductRouteResListWorkItem = this.schData.SchProductRouteResList.filter(
          (p1: SchProductRouteRes) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.schProductRoute.BScheduled === 0 &&
            p1.iWoSeqID > this.iBatchWoSeqID &&
            p1.cInvCode === schProductRouteRes.cInvCode &&
            p1.iSchSdID === schProductRouteRes.iSchSdID,
        )
        schProductRouteResListWorkItem.forEach(
          (schProductRouteResNext: SchProductRouteRes) => {
            schProductRouteResNext.iBatch = as_iBatch
            schProductRouteResNext.iSchSN = schProductRouteRes.iSchSN
          },
        )
        as_SchProductRouteResLast = schProductRouteRes
      },
    )

    let schProductRouteResListNext = new Array<SchProductRouteRes>(10)
    let schProductRouteResListNextAdd = new Array<SchProductRouteRes>(10)

    for (
      let iNextSeqID = this.iBatchWoSeqID + 1;
      iNextSeqID < 90;
      iNextSeqID++
    ) {
      schProductRouteResListNext = this.schData.SchProductRouteResList.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.schProductRoute.BScheduled === 0 &&
          p1.iSchBatch === this.iSchBatch &&
          p1.iBatch === as_iBatch &&
          p1.iWoSeqID === iNextSeqID,
      )

      if (schProductRouteResListNext.length > 0) {
        schProductRouteResListNext.sort(
          (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
            Comparer.Default.Compare(p1.iSchSN, p2.iSchSN),
        )
        schProductRouteResListNext.forEach((ResNext: SchProductRouteRes) => {
          ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate
          ResNext.schProductRoute.ProcessSchTask()
          if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate) {
            this.dBatchBegDate = ResNext.schProductRoute.dEndDate
          }
        })
      }

      if (cBatchFilter !== '') {
        schProductRouteResListNextAdd = this.schData.SchProductRouteResList.filter(
          (p1: SchProductRouteRes) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.schProductRoute.BScheduled === 0 &&
            cBatchFilter.indexOf(p1.FResChaValue1ID) >= 0 &&
            p1.FResChaValue1ID !== '' &&
            p1.iWoSeqID === iNextSeqID &&
            p1.iSchBatch === this.iSchBatch &&
            p1.iBatch !== as_iBatch &&
            p1.schProductRoute.dEarlyBegDate <= this.dBatchBegDate,
        )
      } else {
        schProductRouteResListNextAdd = this.schData.SchProductRouteResList.filter(
          (p1: SchProductRouteRes) =>
            p1.cResourceNo === this.cResourceNo &&
            p1.schProductRoute.BScheduled === 0 &&
            p1.iWoSeqID === iNextSeqID &&
            p1.iSchBatch === this.iSchBatch &&
            p1.FResChaValue1ID !== '' &&
            p1.iBatch !== as_iBatch &&
            p1.schProductRoute.dEarlyBegDate <= this.dBatchBegDate,
        )
      }

      if (schProductRouteResListNextAdd.length > 0) {
        schProductRouteResListNextAdd.sort(
          (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
            Comparer.Default.Compare(p1.iSchSN, p2.iSchSN),
        )
        schProductRouteResListNextAdd.forEach((ResNext: SchProductRouteRes) => {
          ResNext.iBatch = as_iBatch
          ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate
          ResNext.schProductRoute.ProcessSchTask()
          if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate) {
            this.dBatchBegDate = ResNext.schProductRoute.dEndDate
          }
        })
      }
    }

    let schProductRouteResListSched = this.schData.SchProductRouteResList.filter(
      (p1: SchProductRouteRes) =>
        p1.cResourceNo === this.cResourceNo &&
        p1.schProductRoute.BScheduled === 1 &&
        p1.iSchBatch === this.iSchBatch &&
        p1.iBatch === as_iBatch &&
        p1.schProductRoute.cStatus !== '4',
    )

    schProductRouteResListSched.sort(
      (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
        Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate),
    )

    if (schProductRouteResListSched.length > 0) {
      this.dBatchBegDate =
        schProductRouteResListSched[
          schProductRouteResListSched.length - 1
        ].dResEndDate
    }

    return 1
  }

  KeyResBatchSchTaskSubFirst(
    as_iBatch: number,
    as_SchProductRouteResLast: SchProductRouteRes,
    as_schProductRouteResListNo: SchProductRouteRes[],
  ): number {
    let Comparer=this.Comparer
    let dEndDateTemp = this.SchParam.dtToday
    this.dBatchBegDate = dEndDateTemp
    let schProductRouteResListNext = new Array<SchProductRouteRes>(10)

    for (let iNextSeqID = this.iBatchWoSeqID; iNextSeqID < 80; iNextSeqID++) {
      schProductRouteResListNext = this.schData.SchProductRouteResList.filter(
        (p1: SchProductRouteRes) =>
          p1.cResourceNo === this.cResourceNo &&
          p1.schProductRoute.BScheduled === 0 &&
          p1.iSchBatch === this.iSchBatch &&
          p1.iBatch === as_iBatch &&
          p1.iWoSeqID === iNextSeqID,
      )

      if (schProductRouteResListNext.length > 0) {
        schProductRouteResListNext.sort(
          (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
            Comparer.Default.Compare(p1.iSchSN, p2.iSchSN),
        )
        schProductRouteResListNext.forEach((ResNext: SchProductRouteRes) => {
          ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate
          ResNext.schProductRoute.ProcessSchTask()
          if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate) {
            this.dBatchBegDate = ResNext.schProductRoute.dEndDate
          }
        })
      }
    }

    let schProductRouteResListSched = this.schData.SchProductRouteResList.filter(
      (p1: SchProductRouteRes) =>
        p1.cResourceNo === this.cResourceNo &&
        p1.schProductRoute.BScheduled === 1 &&
        p1.iSchBatch === this.iSchBatch &&
        p1.iBatch === as_iBatch &&
        p1.schProductRoute.cStatus !== '4',
    )

    schProductRouteResListSched.sort(
      (p1: SchProductRouteRes, p2: SchProductRouteRes) =>
        Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate),
    )

    if (schProductRouteResListSched.length > 0) {
      this.dBatchBegDate =
        schProductRouteResListSched[
          schProductRouteResListSched.length - 1
        ].dResEndDate
    }

    return 1
  }

  ResClearTask(aSchProductRoute: any): void {
    aSchProductRoute.SchProductRouteResList.forEach(
      (as_SchProductRouteRes: SchProductRouteRes) => {
        this.ResClearTask(as_SchProductRouteRes)
      },
    )
  }

  // ResClearTask(as_SchProductRouteRes: SchProductRouteRes): void {
  //   let taskTimeRangeList1 = as_SchProductRouteRes.TaskTimeRangeList.filter(
  //     (p: any) =>
  //       p.iSchSdID === as_SchProductRouteRes.iSchSdID &&
  //       p.iProcessProductID === as_SchProductRouteRes.iProcessProductID &&
  //       p.iResProcessID === as_SchProductRouteRes.iResProcessID,
  //   )

  //   taskTimeRangeList1.forEach((taskTimeRange: any) => {
  //     taskTimeRange.TaskTimeRangeClear(as_SchProductRouteRes)
  //   })

  //   as_SchProductRouteRes.BScheduled = 0
  // }

  TaskComparer(p1: SchProductRouteRes, p2: SchProductRouteRes): number {
    let Comparer=this.Comparer
    if (this.iSchBatch !== 1) {
      if (this.cIsKey === '1' && this.cProChaType1Sort === '1') {
        if (Comparer.Default.Compare(p1.iResourceID, p2.iResourceID) === 0) {
          if (
            p1.resChaValue1 != null &&
            p2.resChaValue1 != null &&
            Comparer.Default.Compare(
              p1.resChaValue1.FSchSN,
              p2.resChaValue1.FSchSN,
            ) === 0
          ) {
            return 1
          } else if (p1.resChaValue1 == null || p2.resChaValue1 == null) {
            return 1
          } else {
            return Comparer.Default.Compare(
              p1.resChaValue1.FSchSN,
              p2.resChaValue1.FSchSN,
            )
          }
        } else {
          return Comparer.Default.Compare(p1.iResourceID, p2.iResourceID)
        }
      } else {
        if (
          p1.resChaValue1 != null &&
          p2.resChaValue1 != null &&
          Comparer.Default.Compare(
            p1.resChaValue1.FSchSN,
            p2.resChaValue1.FSchSN,
          ) === 0
        ) {
          if (
            p1.resChaValue2 != null &&
            p2.resChaValue2 != null &&
            Comparer.Default.Compare(
              p1.resChaValue2.FSchSN,
              p2.resChaValue2.FSchSN,
            ) === 0
          ) {
            if (
              p1.resChaValue3 != null &&
              p2.resChaValue3 != null &&
              Comparer.Default.Compare(
                p1.resChaValue3.FSchSN,
                p2.resChaValue3.FSchSN,
              ) === 0
            ) {
              if (
                p1.resChaValue4 != null &&
                p2.resChaValue4 != null &&
                Comparer.Default.Compare(
                  p1.resChaValue4.FSchSN,
                  p2.resChaValue4.FSchSN,
                ) === 0
              ) {
                if (
                  p1.resChaValue5 != null &&
                  p2.resChaValue5 != null &&
                  Comparer.Default.Compare(
                    p1.resChaValue5.FSchSN,
                    p2.resChaValue5.FSchSN,
                  ) === 0
                ) {
                  if (
                    p1.resChaValue6 != null &&
                    p2.resChaValue6 != null &&
                    Comparer.Default.Compare(
                      p1.resChaValue6.FSchSN,
                      p2.resChaValue6.FSchSN,
                    ) === 0
                  ) {
                    return 1
                  } else {
                    if (p1.resChaValue6 == null || p2.resChaValue6 == null) {
                      return 1
                    } else {
                      return Comparer.Default.Compare(
                        p1.resChaValue6.FSchSN,
                        p2.resChaValue6.FSchSN,
                      )
                    }
                  }
                } else {
                  if (p1.resChaValue5 == null || p2.resChaValue5 == null) {
                    return 1
                  } else {
                    return Comparer.Default.Compare(
                      p1.resChaValue5.FSchSN,
                      p2.resChaValue5.FSchSN,
                    )
                  }
                }
              } else {
                if (p1.resChaValue4 == null || p2.resChaValue4 == null) {
                  return 1
                } else {
                  return Comparer.Default.Compare(
                    p1.resChaValue4.FSchSN,
                    p2.resChaValue4.FSchSN,
                  )
                }
              }
            } else {
              if (p1.resChaValue3 == null || p2.resChaValue3 == null) {
                return 1
              } else {
                return Comparer.Default.Compare(
                  p1.resChaValue3.FSchSN,
                  p2.resChaValue3.FSchSN,
                )
              }
            }
          } else {
            if (p1.resChaValue2 == null || p2.resChaValue2 == null) {
              return 1
            } else {
              return Comparer.Default.Compare(
                p1.resChaValue2.FSchSN,
                p2.resChaValue2.FSchSN,
              )
            }
          }
        } else {
          if (p1.resChaValue1 == null || p2.resChaValue1 == null) {
            return 1
          } else {
            return Comparer.Default.Compare(
              p1.resChaValue1.FSchSN,
              p2.resChaValue1.FSchSN,
            )
          }
        }
      }
    } else {
      if (
        Comparer.Default.Compare(
          p1.schProductRoute.schProduct.cMiNo,
          p2.schProductRoute.schProduct.cMiNo,
        ) === 0
      ) {
        if (Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate) === 0) {
          return 1
        } else {
          return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
        }
      } else {
        return Comparer.Default.Compare(
          p1.schProductRoute.schProduct.cMiNo,
          p2.schProductRoute.schProduct.cMiNo,
        )
      }
    }
    return 1
  }

  ResTaskComparer(p1: SchProductRouteRes, p2: SchProductRouteRes): number {
    const Comparer = this.Comparer
    const SchData = this.schData
    const SchParam = this.SchParam
    if (this.SchParam.cProChaType1Sort === '5') {
      return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
    } else if (this.SchParam.cProChaType1Sort === '0') {
      if (Comparer.Default.Compare(p1.iPriorityRes, p2.iPriorityRes) === 0) {
        return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
      } else {
        return Comparer.Default.Compare(p1.iPriorityRes, p2.iPriorityRes)
      }
    } else if (this.SchParam.cProChaType1Sort === '1') {
      if (
        p1.schProductRoute.schProductWorkItem == null ||
        p2.schProductRoute.schProductWorkItem == null
      ) {
        if (
          Comparer.Default.Compare(
            p1.schProductRoute.schProduct.iPriority,
            p2.schProductRoute.schProduct.iPriority,
          ) === 0
        ) {
          return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
        } else {
          return Comparer.Default.Compare(
            p1.schProductRoute.schProduct.iPriority,
            p2.schProductRoute.schProduct.iPriority,
          )
        }
      } else {
        return Comparer.Default.Compare(
          p1.schProductRoute.schProductWorkItem.dRequireDate,
          p2.schProductRoute.schProductWorkItem.dRequireDate,
        )
      }
    } else if (this.SchParam.cProChaType1Sort === '2') {
      if (
        p1.schProductRoute.schProductWorkItem == null ||
        p2.schProductRoute.schProductWorkItem == null
      ) {
        if (
          Comparer.Default.Compare(
            p1.schProductRoute.schProduct.iPriority,
            p2.schProductRoute.schProduct.iPriority,
          ) === 0
        ) {
          return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
        } else {
          return Comparer.Default.Compare(
            p1.schProductRoute.schProduct.iPriority,
            p2.schProductRoute.schProduct.iPriority,
          )
        }
      } else {
        if (
          Comparer.Default.Compare(
            p1.schProductRoute.schProductWorkItem.iPriority,
            p2.schProductRoute.schProductWorkItem.iPriority,
          ) === 0
        ) {
          return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
        } else {
          return Comparer.Default.Compare(
            p1.schProductRoute.schProductWorkItem.iPriority,
            p2.schProductRoute.schProductWorkItem.iPriority,
          )
        }
      }
    } else if (Comparer.Default.Compare(p1.iSchBatch, p2.iSchBatch) === 0) {
      if (this.SchParam.cProChaType1Sort === '3') {
        if (Comparer.Default.Compare(p1.iSchSN, p2.iSchSN) === 0) {
          return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
        } else {
          return Comparer.Default.Compare(p1.iSchSN, p2.iSchSN)
        }
      } else if (this.SchParam.cProChaType1Sort === '4') {
        if (this.TaskComparer(p1, p2) === 0) {
          return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
        } else {
          return this.TaskComparer(p1, p2)
        }
      } else {
        return Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate)
      }
    } else {
      return Comparer.Default.Compare(p1.iSchBatch, p2.iSchBatch)
    }
    return 1
  }
  public getTaskTimeRangeList(orderASC: boolean = true): TaskTimeRange[] {
    const listTaskTimeRangeAll: TaskTimeRange[] = []

    for (const resTimeRange of this.ResTimeRangeList) {
      listTaskTimeRangeAll.push(...resTimeRange.WorkTimeRangeList)
    }

    // Sorting
    if (orderASC) {
      // Ascending order
      listTaskTimeRangeAll.sort(
        (p1, p2) => p1.dBegTime.getTime() - p2.dBegTime.getTime(),
      )
    } else {
      // Descending order
      listTaskTimeRangeAll.sort(
        (p1, p2) => p2.dBegTime.getTime() - p1.dBegTime.getTime(),
      )
    }

    return listTaskTimeRangeAll
  }
}
