import { Base } from './Base'
import {
  ResChaValue,
  Resource,
  SchData,
  SchParam,
  TaskTimeRange,
  TechLearnCurves,
  SchProductRoute,
  DateTime,
} from './type'

export class SchProductRouteRes extends Base{
  schData: SchData | null = null
  private bScheduled: number = 0
  cVersionNo: string
  iSchSdID: number
  iProcessProductID: number
  iProcessID: number
  iResourceAbilityID: number
  cWoNo: string
  iItemID: number
  cInvCode: string
  iWoSeqID: number
  iResProcessID: any
  cTechNo: string
  cSeqNote: string
  iResGroupNo: string
  iResGroupPriority: number
  cSelected: string
  cCanScheduled: string = '1'
  iResourceID: number
  cResourceNo: string
  cResourceName: string
  iResReqQty: number
  iResReqQtyOld: number
  dResBegDate: DateTime
  dResEndDate: DateTime
  dResLeanBegDate: DateTime
  iResRationHour: number
  iActResReqQty: number
  iActResRationHour: number
  dActResBegDate: DateTime
  dActResEndDate: DateTime
  iViceResource1ID: number
  cViceResource1No: string
  iViceResource2ID: number
  cViceResource2No: string
  iViceResource3ID: number
  cViceResource3No: string
  cWorkType: string
  iBatchQty: number
  iBatchQtyBase: number
  iBatchWorkTime: number
  iBatchInterTime: number
  iResPreTime: number
  iResPreTimeOld: number
  cResPreTimeExp: string
  private ICapacity: number
  private iMoldCount: number = 1
  cTeamResourceNo: string
  cLearnCurvesNo: string
  private techLearnCurves: TechLearnCurves | null = null
  cCapacityExp: string
  cIsInfinityAbility: number
  iResPostTime: number
  cResPostTimeExp: string
  iCycTime: number
  iProcessPassRate: number
  iEfficiency: number
  iHoursPd: number
  iWorkQtyPd: number
  iWorkersPd: number
  iDevCountPd: number
  iLaborTime: number
  iLeadTime: number
  iSchSN: number = 1
  dCanResBegDate: DateTime
  iResWaitTime: number
  FResChaValue1ID: string
  FResChaValue2ID: string
  FResChaValue3ID: string
  FResChaValue4ID: string
  FResChaValue5ID: string
  FResChaValue6ID: string
  FResChaValue1IDLeft: number = 0
  FResChaValue2IDLeft: number = 0
  FResChaValue3IDLeft: number = 0
  FResChaValue4IDLeft: number = 0
  FResChaValue5IDLeft: number = 0
  FResChaValue6IDLeft: number = 0
  resChaValue1: ResChaValue
  resChaValue2: ResChaValue
  resChaValue3: ResChaValue
  resChaValue4: ResChaValue
  resChaValue5: ResChaValue
  resChaValue6: ResChaValue
  FResChaValue1Cyc: number
  FResChaValue2Cyc: number
  FResChaValue3Cyc: number
  FResChaValue4Cyc: number
  FResChaValue5Cyc: number
  FResChaValue6Cyc: number
  cResourceNote: string
  cDefine22: string
  cDefine23: string
  cDefine24: string
  cDefine25: string
  cDefine26: string
  cDefine27: string
  cDefine28: string
  cDefine29: string
  cDefine30: string
  cDefine31: string
  cDefine32: number
  cDefine33: number
  cDefine34: number
  cDefine35: number
  cDefine36: DateTime
  cDefine37: DateTime
  cDefine38: DateTime
  iPriorityRes: number
  iPriorityResLast: number
  SchProductRouteResPre: SchProductRouteRes
  iBatch: number = 1
  iSchBatch: number = 6
  iDayMaxQty: number = 0
  iWeekMaxQty: number = 0
  dEarliestStartTime: DateTime
  dLatestEndTime: DateTime
  TaskTimeRangeList: TaskTimeRange[] = []
  resource: Resource
  schProductRoute: SchProductRoute

  get BScheduled(): number {
    return this.bScheduled
  }

  set BScheduled(value: number) {
    let SchParam=this.SchParam
    if (value === 1 && this.bScheduled === 0) {
      if (this.schData.iCurRows < this.schData.iTotalRows) {
        this.schData.iCurRows++
      }
      this.resource.iSchHours += this.iResRationHour
      this.resource.iPlanDays =
        this.resource.iSchHours / 3600 / this.resource.iResHoursPd
    } else if (value === 0 && this.bScheduled === 1) {
      if (this.schData.iCurRows > 1) {
        this.schData.iCurRows--
      }
      this.resource.iSchHours -= this.iResRationHour
      this.resource.iPlanDays =
        this.resource.iSchHours / 3600 / this.resource.iResHoursPd
    }
    this.bScheduled = value
    if (
      this.iSchSdID === SchParam.iSchSdID &&
      this.iProcessProductID === SchParam.iProcessProductID
    ) {
      const dt = this.dResBegDate
      const dt2 = this.dResEndDate
      SchParam.dtResLastSchTime = new DateTime()
      this.cDefine37 = SchParam.dtResLastSchTime
    }
  }

  get TechLearnCurves(): TechLearnCurves | null {
    if (this.cLearnCurvesNo === '') this.techLearnCurves = null
    if (this.techLearnCurves == null)
      //@ts-ignore
      this.techLearnCurves = new TechLearnCurves(this.cLearnCurvesNo, this)
    return this.techLearnCurves
  }

  get iCapacity(): number {
    return this.resource.cResOccupyType === '1'
      ? this.ICapacity / this.resource.iResourceNumber
      : this.ICapacity
  }

  set iCapacity(value: number) {
    this.ICapacity = value
  }

  DispatchSchTask(
    ai_ResReqQty: number,
    adCanBegDate: DateTime,
    as_SchProductRouteResPre: SchProductRouteRes,
  ): number {
    const SchParam = this.SchParam
    let fWorkTime = 0
    let iWorkTime = 0
    let iBatch = 0
    let ldtBeginDate: any = new DateTime()
    if (this.bScheduled === 1) return 0
    try {
      let message: string
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        (this.iProcessProductID === 193864 &&
          this.schProductRoute.iSchSdID === 1070)
      ) {
        message = `1、排产顺序[${this.iSchSN}],计划ID[${
          this.iSchSdID
        }],任务ID[${this.iProcessProductID}],资源编号[${
          this.cResourceNo
        }],开始排产时间[${new DateTime()}],完成排产时间[${0}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new DateTime()
      }
      if (adCanBegDate < this.dEarliestStartTime) {
        adCanBegDate = this.dEarliestStartTime
      } else {
        this.dEarliestStartTime = adCanBegDate
      }
      if (this.cWorkType === '1') {
        if (
          ai_ResReqQty / this.iBatchQty <
          Math.floor(ai_ResReqQty / this.iBatchQty)
        )
          iBatch = Math.floor(ai_ResReqQty / this.iBatchQty) + 1
        else iBatch = Math.floor(ai_ResReqQty / this.iBatchQty)
        if (iBatch < 1) iBatch = 1
        fWorkTime =
          iBatch * this.iCapacity + (iBatch - 1) * this.iBatchInterTime
      } else {
        fWorkTime = (ai_ResReqQty * this.iCapacity) / this.iBatchQty
      }
      if (
        this.schProductRoute.item != null &&
        this.schProductRoute.item.iItemDifficulty > 0 &&
        this.schProductRoute.item.iItemDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty
      if (
        this.resource != null &&
        this.resource.iResDifficulty > 0 &&
        this.resource.iResDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.resource.iResDifficulty
      if (
        this.schProductRoute.techInfo != null &&
        this.schProductRoute.techInfo.iTechDifficulty > 0 &&
        this.schProductRoute.techInfo.iTechDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.schProductRoute.techInfo.iTechDifficulty
      if (this.resource == null) {
        throw new Error(
          `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.TaskSchTask,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]对应资源编号不能为空！`,
        )
        return -1
      }
      iWorkTime = Math.floor(
        (100.0 * 100 * fWorkTime) /
          (this.resource.iEfficient * SchParam.AllResiEfficient),
      )
      if (iWorkTime < 1) iWorkTime = 1
      this.iResReqQty = ai_ResReqQty
      this.iResRationHour = iWorkTime
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        (this.iProcessProductID === 193864 &&
          this.schProductRoute.iSchSdID === 1070)
      ) {
        message = `2、排产顺序[${this.iSchSN}],计划ID[${
          this.iSchSdID
        }],任务ID[${this.iProcessProductID}],资源编号[${
          this.cResourceNo
        }],开始排产时间[${new DateTime()}],运算时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new DateTime(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new DateTime()
      }
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        this.iResourceAbilityID === SchParam.iProcessProductID
      ) {
        let i = 1
      }
      let ai_ResPreTime = 0
      let ai_CycTimeTol = 0
      if (adCanBegDate < this.resource.dMaxExeDate)
        adCanBegDate = this.resource.dMaxExeDate
      let ldtBeginDateRes = new DateTime()
      if (this.cSeqNote === '折弯') {
        ldtBeginDateRes = new DateTime()
      }
      try {
        this.resource.ResSchTask(
          this,
          iWorkTime,
          adCanBegDate,
          ai_ResPreTime,
          ai_CycTimeTol,
          true,
          as_SchProductRouteResPre,
        )
      } catch (error) {
        throw new Error(
          `资源正排出错,订单行号：${this.iSchSdID} 资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：${this.iProcessProductID} \n\r ${error.message}`,
        )
        return -1
      }
      const iWaitTime = new DateTime().getTime() - ldtBeginDateRes.getTime()
      if (this.cSeqNote === '折弯') {
      } else {
      }
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        (this.iProcessProductID === 193864 &&
          this.schProductRoute.iSchSdID === 1070)
      ) {
        message = `3、排产顺序[${this.iSchSN}],计划ID[${
          this.iSchSdID
        }],任务ID[${this.iProcessProductID}],资源编号[${
          this.cResourceNo
        }],开始排产时间[${new DateTime()}],完成排产时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new DateTime(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new DateTime()
      }
      this.iResPreTime = ai_ResPreTime
      this.iCycTime = ai_CycTimeTol
      this.iResRationHour += ai_ResPreTime
      const list1 = this.TaskTimeRangeList.filter(
        (p1) =>
          p1.iSchSdID === this.iSchSdID &&
          p1.iProcessProductID === this.iProcessProductID &&
          p1.iResProcessID === this.iResProcessID,
      )
      if (this.TaskTimeRangeList.length > 0) {
        this.TaskTimeRangeList.sort(
          (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
        )
        this.dResBegDate = this.TaskTimeRangeList[0].DBegTime
        this.dResEndDate = this.TaskTimeRangeList[
          this.TaskTimeRangeList.length - 1
        ].DEndTime
        let iResReqQtyTemp = 0
        let iAllottedTime = 0
        let iResReqQtyTemp9 = 0
        if (parseInt(this.iResReqQty.toString(), 10)) {
          for (let i = this.TaskTimeRangeList.length - 1; i >= 0; i--) {
            const lTaskTimeRange = this.TaskTimeRangeList[i]
            iAllottedTime =
              (((lTaskTimeRange.WorkTimeAct *
                this.resource.iEfficient *
                SchParam.AllResiEfficient) /
                10000) *
                (this.iResRationHour - this.iResPreTime)) /
              this.iResRationHour
            if (iResReqQtyTemp < this.iResReqQty) {
              lTaskTimeRange.iResReqQty =
                (this.cWorkType === '1' ? this.iBatchQty : 1) *
                Math.floor(iAllottedTime / this.iCapacity)
              if (lTaskTimeRange.iResReqQty > this.iResReqQty)
                lTaskTimeRange.iResReqQty = this.iResReqQty
            } else {
              lTaskTimeRange.iResReqQty = 0
            }
            iResReqQtyTemp += lTaskTimeRange.iResReqQty
            if (iResReqQtyTemp > this.iResReqQty) {
              lTaskTimeRange.iResReqQty -= iResReqQtyTemp - this.iResReqQty
              iResReqQtyTemp -=
                iResReqQtyTemp - parseInt(this.iResReqQty.toString(), 10)
            }
            if (i === 0) {
              lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp
              if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0
            }
          }
        } else {
          let iResReqQtyTemp2 = 0
          for (let i = this.TaskTimeRangeList.length - 1; i >= 0; i--) {
            const lTaskTimeRange = this.TaskTimeRangeList[i]
            iAllottedTime =
              (((lTaskTimeRange.WorkTimeAct *
                this.resource.iEfficient *
                SchParam.AllResiEfficient) /
                10000) *
                (this.iResRationHour - this.iResPreTime)) /
              this.iResRationHour
            if (iResReqQtyTemp2 < this.iResReqQty) {
              lTaskTimeRange.iResReqQty =
                this.iResReqQty * Math.floor(iAllottedTime / this.iCapacity)
            } else {
              lTaskTimeRange.iResReqQty = 0
            }
            iResReqQtyTemp2 += lTaskTimeRange.iResReqQty
            if (iResReqQtyTemp2 > this.iResReqQty) {
              lTaskTimeRange.iResReqQty -= iResReqQtyTemp2 - this.iResReqQty
              iResReqQtyTemp2 -=
                iResReqQtyTemp2 - parseFloat(this.iResReqQty.toString())
            }
            if (i === 0) {
              lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp2
              if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0
            }
          }
        }
      }
      this.dCanResBegDate = adCanBegDate
      this.iResWaitTime =
        (this.dResBegDate.getTime() - this.dCanResBegDate.getTime()) /
        (1000 * 60 * 60)
      if (this.iResWaitTime > 1) {
        let j = 0
      }
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        (this.iProcessProductID === 193864 &&
          this.schProductRoute.iSchSdID === 1070)
      ) {
        message = `9、排产顺序[${this.iSchSN}],计划ID[${
          this.iSchSdID
        }],任务ID[${this.iProcessProductID}],资源编号[${
          this.cResourceNo
        }],开始排产时间[${new DateTime()}],完成排产时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new DateTime(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
      }
    } catch (exp) {
      SchParam.Debug(exp.message, '资源运算')
      throw new Error(`出错信息: ${exp.message}`)
    }
    return 1
  }

  TaskSchTask(ai_ResReqQty: number, adCanBegDate: DateTime): number {
    let SchParam=this.SchParam
    let fWorkTime = 0
    let iWorkTime = 0
    let iBatch = 0
    let ldtBeginDate = new DateTime()
    if (this.bScheduled === 1) return 0
    try {
      let message: string
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        (this.iProcessProductID === 193864 &&
          this.schProductRoute.iSchSdID === 1070)
      ) {
        message = `1、排产顺序[${this.iSchSN}],计划ID[${
          this.iSchSdID
        }],任务ID[${this.iProcessProductID}],资源编号[${
          this.cResourceNo
        }],开始排产时间[${new DateTime()}],完成排产时间[${0}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new DateTime()
      }
      if (adCanBegDate < this.dEarliestStartTime) {
        adCanBegDate = this.dEarliestStartTime
      } else {
        this.dEarliestStartTime = adCanBegDate
      }
      if (this.cWorkType === '1') {
        if (
          ai_ResReqQty / this.iBatchQty >
          Math.floor(ai_ResReqQty / this.iBatchQty)
        )
          iBatch = Math.floor(ai_ResReqQty / this.iBatchQty) + 1
        else if (
          ai_ResReqQty / this.iBatchQty <
          Math.floor(ai_ResReqQty / this.iBatchQty)
        )
          iBatch = Math.floor(ai_ResReqQty / this.iBatchQty)
        else iBatch = Math.floor(ai_ResReqQty / this.iBatchQty)
        if (iBatch < 1) iBatch = 1
        fWorkTime =
          iBatch * this.iCapacity + (iBatch - 1) * this.iBatchInterTime
      } else {
        fWorkTime = (ai_ResReqQty * this.iCapacity) / this.iBatchQty
      }
      if (
        this.schProductRoute.item != null &&
        this.schProductRoute.item.iItemDifficulty > 0 &&
        this.schProductRoute.item.iItemDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty
      if (
        this.resource != null &&
        this.resource.iResDifficulty > 0 &&
        this.resource.iResDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.resource.iResDifficulty
      if (
        this.schProductRoute.techInfo != null &&
        this.schProductRoute.techInfo.iTechDifficulty > 0 &&
        this.schProductRoute.techInfo.iTechDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.schProductRoute.techInfo.iTechDifficulty
      if (this.resource == null) {
        throw new Error(
          `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.TaskSchTask,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]对应资源编号不能为空！`,
        )
        return -1
      }
      if (this.resource.bTeamResource === '1') {
        if (this.resource.TeamResourceList.length < 1) {
          throw new Error(
            `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.TaskSchTask,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]对应资源编组没有具体资源编号！`,
          )
          return -1
        }
        this.resource.TeamResourceList.sort(
          (p1, p2) =>
            p1.GetEarlyStartDate(adCanBegDate, false).getTime() -
            p2.GetEarlyStartDate(adCanBegDate, false).getTime(),
        )
        this.cTeamResourceNo = this.cResourceNo
        this.cResourceNo = this.resource.TeamResourceList[0].cResourceNo
        this.resource = this.resource.TeamResourceList[0]
      }
      iWorkTime = Math.floor(
        (100.0 * 100 * fWorkTime) /
          (this.resource.iEfficient * SchParam.AllResiEfficient),
      )
      if (iWorkTime < 1) iWorkTime = 1
      this.iResReqQty = ai_ResReqQty
      this.iResRationHour = iWorkTime
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        (this.iProcessProductID === 193864 &&
          this.schProductRoute.iSchSdID === 1070)
      ) {
        message = `2、排产顺序[${this.iSchSN}],计划ID[${
          this.iSchSdID
        }],任务ID[${this.iProcessProductID}],资源编号[${
          this.cResourceNo
        }],开始排产时间[${new DateTime()}],运算时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new DateTime(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new DateTime()
      }
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        this.iResourceAbilityID === SchParam.iProcessProductID
      ) {
        let i = 1
      }
      let ai_ResPreTime = 0
      let ai_CycTimeTol = 0
      if (adCanBegDate < this.resource.dMaxExeDate)
        adCanBegDate = this.resource.dMaxExeDate
      let ldtBeginDateRes = new DateTime()
      if (this.cSeqNote === '折弯') {
        ldtBeginDateRes = new DateTime()
      }
      try {
        ldtBeginDate = new DateTime()
        this.resource.ResSchTask(
          this,
          iWorkTime,
          adCanBegDate,
          ai_ResPreTime,
          ai_CycTimeTol,
        )
      } catch (error) {
        throw new Error(
          `资源正排出错,订单行号：${this.iSchSdID} 资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：${this.iProcessProductID} \n\r ${error.message}`,
        )
        return -1
      }
      if (
        (this.iProcessProductID === SchParam.iProcessProductID &&
          this.schProductRoute.iSchSdID === SchParam.iSchSdID) ||
        (this.iProcessProductID === 193864 &&
          this.schProductRoute.iSchSdID === 1070)
      ) {
        message = `3、排产顺序[${this.iSchSN}],计划ID[${
          this.iSchSdID
        }],任务ID[${this.iProcessProductID}],资源编号[${
          this.cResourceNo
        }],开始排产时间[${new DateTime()}],完成排产时间[${SchData.GetDateDiffString(
          ldtBeginDate,
          new DateTime(),
          'ms',
        )}]`
        SchParam.Debug(message, '资源运算')
        ldtBeginDate = new DateTime()
      }
      this.iResPreTime = ai_ResPreTime
      this.iCycTime = ai_CycTimeTol
      this.iResRationHour += ai_ResPreTime
      if (this.TaskTimeRangeList.length > 0) {
        this.TaskTimeRangeList.sort(
          (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
        )
        this.dResBegDate = this.TaskTimeRangeList[0].DBegTime
        this.dResEndDate = this.TaskTimeRangeList[
          this.TaskTimeRangeList.length - 1
        ].DEndTime
        let iResReqQtyTemp = 0
        let iAllottedTime = 0
        let iResReqQtyTemp9 = 0
        if (parseInt(this.iResReqQty.toString(), 10)) {
          for (let i = this.TaskTimeRangeList.length - 1; i >= 0; i--) {
            const lTaskTimeRange = this.TaskTimeRangeList[i]
            iAllottedTime =
              (((lTaskTimeRange.WorkTimeAct *
                this.resource.iEfficient *
                SchParam.AllResiEfficient) /
                10000) *
                (this.iResRationHour - this.iResPreTime)) /
              this.iResRationHour
            if (iResReqQtyTemp < this.iResReqQty) {
              lTaskTimeRange.iResReqQty =
                (this.cWorkType === '1' ? this.iBatchQty : 1) *
                Math.floor(iAllottedTime / this.iCapacity)
            } else {
              lTaskTimeRange.iResReqQty = 0
            }
            iResReqQtyTemp += lTaskTimeRange.iResReqQty
            if (iResReqQtyTemp > this.iResReqQty) {
              lTaskTimeRange.iResReqQty -= iResReqQtyTemp - this.iResReqQty
              iResReqQtyTemp -=
                iResReqQtyTemp - parseInt(this.iResReqQty.toString(), 10)
            }
            if (i === 0) {
              lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp
              if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0
            }
          }
        } else {
          let iResReqQtyTemp2 = 0
          for (let i = this.TaskTimeRangeList.length - 1; i >= 0; i--) {
            const lTaskTimeRange = this.TaskTimeRangeList[i]
            iAllottedTime =
              (((lTaskTimeRange.WorkTimeAct *
                this.resource.iEfficient *
                SchParam.AllResiEfficient) /
                10000) *
                (this.iResRationHour - this.iResPreTime)) /
              this.iResRationHour
            if (iResReqQtyTemp2 < this.iResReqQty) {
              lTaskTimeRange.iResReqQty =
                this.iResReqQty * Math.floor(iAllottedTime / this.iCapacity)
            } else {
              lTaskTimeRange.iResReqQty = 0
            }
            iResReqQtyTemp2 += lTaskTimeRange.iResReqQty
            if (iResReqQtyTemp2 > this.iResReqQty) {
              lTaskTimeRange.iResReqQty -= iResReqQtyTemp2 - this.iResReqQty
              iResReqQtyTemp2 -=
                iResReqQtyTemp2 - parseFloat(this.iResReqQty.toString())
            }
            if (i === 0) {
              lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp2
              if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0
            }
          }
        }
      }
      this.dCanResBegDate = adCanBegDate
      this.iResWaitTime =
        (this.dResBegDate.getTime() - this.dCanResBegDate.getTime()) /
        (1000 * 60 * 60)
      if (this.iResWaitTime > 1) {
        let j = 0
      }
    } catch (exp) {
      throw new Error(`出错信息: ${exp.message}`)
    }
    return 1
  }

  TaskSchTaskRev(
    ai_ResReqQty: number,
    adCanEndDate: DateTime,
    cType: string = '1',
  ): number {
    let SchParam=this.SchParam
    let fWorkTime = 0
    let iWorkTime = 0
    let iBatch = 0
    if (this.bScheduled === 1) return 0
    try {
      if (this.cWorkType === '1') {
        if (
          ai_ResReqQty / this.iBatchQty <
          Math.floor(ai_ResReqQty / this.iBatchQty)
        )
          iBatch = Math.floor(ai_ResReqQty / this.iBatchQty) + 1
        else iBatch = Math.floor(ai_ResReqQty / this.iBatchQty)
        if (iBatch < 1) iBatch = 1
        fWorkTime =
          iBatch * this.iCapacity + (iBatch - 1) * this.iBatchInterTime
      } else {
        fWorkTime = (ai_ResReqQty * this.iCapacity) / this.iBatchQty
      }
      if (
        this.schProductRoute.item != null &&
        this.schProductRoute.item.iItemDifficulty > 0 &&
        this.schProductRoute.item.iItemDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty
      if (
        this.resource != null &&
        this.resource.iResDifficulty > 0 &&
        this.resource.iResDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.resource.iResDifficulty
      if (
        this.schProductRoute.techInfo != null &&
        this.schProductRoute.techInfo.iTechDifficulty > 0 &&
        this.schProductRoute.techInfo.iTechDifficulty !== 1
      )
        fWorkTime = fWorkTime * this.schProductRoute.techInfo.iTechDifficulty
      if (this.resource.bTeamResource === '1') {
        if (this.resource.TeamResourceList.length < 1) {
          throw new Error(
            `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.TaskSchTask,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]对应资源编组没有具体资源编号！`,
          )
          return -1
        }
        this.resource.TeamResourceList.sort(
          (p1, p2) =>
            p1.GetEarlyStartDate(adCanEndDate, true).getTime() -
            p2.GetEarlyStartDate(adCanEndDate, true).getTime(),
        )
        this.cTeamResourceNo = this.cResourceNo
        this.cResourceNo = this.resource.TeamResourceList[0].cResourceNo
        this.resource = this.resource.TeamResourceList[0]
      }
      if (this.resource == null) {
        throw new Error(
          `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.TaskSchTask,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]对应资源编号不能为空！`,
        )
        return -1
      }
      iWorkTime = Math.floor(
        (100.0 * 100 * fWorkTime) /
          (this.resource.iEfficient * SchParam.AllResiEfficient),
      )
      if (iWorkTime < 1) iWorkTime = 1
      this.iResReqQty = ai_ResReqQty
      this.iResRationHour = iWorkTime
      if (
        this.iProcessProductID === SchParam.iProcessProductID &&
        this.schProductRoute.iSchSdID === SchParam.iSchSdID
      ) {
        let i = 1
      }
      try {
        this.resource.ResSchTaskRev(this, iWorkTime, adCanEndDate)
      } catch (ex2) {
        throw new Error(
          `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.ResSchTaskRev,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]倒排出错! 资源任务号${this.iResourceAbilityID} ${ex2.message}`,
        )
        return -1
      }
      try {
        if (this.TaskTimeRangeList.length > 0) {
          this.TaskTimeRangeList.sort(
            (p1, p2) => p1.DBegTime.getTime() - p2.DBegTime.getTime(),
          )
          this.dResBegDate = this.TaskTimeRangeList[0].DBegTime
          this.dResEndDate = this.TaskTimeRangeList[
            this.TaskTimeRangeList.length - 1
          ].DEndTime
          let iResReqQtyTemp = 0
          let iAllottedTime = 0
          for (let i = this.TaskTimeRangeList.length - 1; i >= 0; i--) {
            const lTaskTimeRange = this.TaskTimeRangeList[i]
            iAllottedTime =
              (lTaskTimeRange.AllottedTime *
                this.resource.iEfficient *
                SchParam.AllResiEfficient) /
              10000
            lTaskTimeRange.iResReqQty = Math.floor(
              iAllottedTime / this.iCapacity,
            )
            iResReqQtyTemp += lTaskTimeRange.iResReqQty
            if (i === 0) {
              lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp
              if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0
            }
          }
        }
      } catch (ex2) {
        throw new Error(
          `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.ResSchTaskRev,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]倒排出错! 资源任务号${this.iResourceAbilityID} TaskTimeRangeList ${ex2.message}`,
        )
        return -1
      }
    } catch (exp) {
      SchParam.Debug(exp.message, '资源运算')
      throw new Error(`出错信息: ${exp.message}`)
    }
    return 1
  }

  TaskClearTask() {
    this.resource.ResClearTask(this)
  }

  TestResSchTask(ai_ResReqQty: number, adCanBegDate: DateTime): number {
    let fWorkTime = 0
    let iWorkTime = 0
    let iBatch = 0
    if (this.bScheduled === 1) return 0
    try {
      if (this.cWorkType === '1') {
        if (
          ai_ResReqQty / this.iBatchQty <
          Math.floor(ai_ResReqQty / this.iBatchQty)
        )
          iBatch = Math.floor(ai_ResReqQty / this.iBatchQty) + 1
        else iBatch = Math.floor(ai_ResReqQty / this.iBatchQty)
        if (iBatch < 1) iBatch = 1
        fWorkTime =
          iBatch * this.iCapacity + (iBatch - 1) * this.iBatchInterTime
      } else {
        fWorkTime = (ai_ResReqQty * this.iCapacity) / this.iBatchQty
      }
      if (
        this.schProductRoute.item != null &&
        this.schProductRoute.item.iItemDifficulty > 0
      )
        fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty
      if (this.resource != null && this.resource.iResDifficulty > 0)
        fWorkTime = fWorkTime * this.resource.iResDifficulty
      if (
        this.schProductRoute.techInfo != null &&
        this.schProductRoute.techInfo.iTechDifficulty > 0
      )
        fWorkTime = fWorkTime * this.schProductRoute.techInfo.iTechDifficulty
      if (this.resource == null) {
        throw new Error(
          `订单行号：${this.iSchSdID} 出错位置:SchProductRouteRes.TestResSchTask,加工物料[${this.cInvCode}] 工序[${this.cSeqNote}]对应资源编号不能为空！`,
        )
        return -1
      }
      let SchParam = this.SchParam
      iWorkTime = Math.floor(
        (100.0 * 100 * fWorkTime) /
          (this.resource.iEfficient * SchParam.AllResiEfficient),
      )
      if (iWorkTime < 1) iWorkTime = 1
      this.iResReqQty = ai_ResReqQty
      this.iResRationHour = iWorkTime
      if (adCanBegDate < this.resource.dMaxExeDate)
        adCanBegDate = this.resource.dMaxExeDate
      const adCanBegDateTest = adCanBegDate
      let ai_workTimeTest = iWorkTime
      let ai_CycTimeTol = 0,
        ai_ResPreTime = 0
      let dtBegDate = adCanBegDate,
        dtEndDate = adCanBegDate
      try {
        const li_Return = this.resource.TestResSchTask(
          this,
          ai_workTimeTest,
          adCanBegDateTest,
          adCanBegDate,
          false,
          ai_ResPreTime,
          ai_CycTimeTol,
          dtBegDate,
          dtEndDate,
          false,
        )
        if (li_Return < 0) {
          const cError = `订单行号：${this.iSchSdID} ,加工物料[${this.cInvCode}]在资源[${this.cResourceNo}]无法排下,任务号[${this.iProcessProductID}],单件产能[${this.iCapacity}],加工数量[${this.iResReqQty}],加工工时[${iWorkTime}],未排工时[${iWorkTime}],最大可排时间[${adCanBegDateTest}],请检查工作日历或单件产能、计划数量太大!`
          throw new Error(cError)
          this.cCanScheduled = '0'
          return -1
        } else {
          this.cCanScheduled = '1'
          this.dResBegDate = dtBegDate
          this.dResEndDate = dtEndDate
        }
      } catch (error) {
        throw new Error(
          `资源模拟测试正排出错,订单行号：${this.iSchSdID} ,位置SchProductRouteRes.TestResSchTask！工序ID号：${this.iProcessProductID} \n\r ${error.message}`,
        )
        return -1
      }
    } catch (exp) {
      let SchParam = this.SchParam
      SchParam.Debug(exp.message, '资源运算')
      throw new Error(`出错信息: ${exp.message}`)
    }
    return 1
  }

//   GetObjectData(info: any, context: any): void {
//     throw new Error('Method not implemented.')
//   }
}
