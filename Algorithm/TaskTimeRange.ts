import { Base } from './Base'
import { ResTimeRange, SchParam, SchProductRouteRes } from './type'

export class TaskTimeRange extends ResTimeRange {
  public schProductRouteRes: SchProductRouteRes | null = null
  public iSchSdID: number = -1
  public cVersionNo: string
  public iProcessProductID: number = -1
  public iResProcessID: number = -1
  public cWoNo: string = ''
  public cTaskType: number = 1
  public iResReqQty: number = 0
  public iResRationHour: number = 0
  public resTimeRange: ResTimeRange | null = null
  public taskTimeRangePre: TaskTimeRange | null = null
  public taskTimeRangePost: TaskTimeRange | null = null
  public taskTimeRangeClear(as_SchProductRouteRes: SchProductRouteRes): void {
    let SchParam=this.SchParam
    if (
      as_SchProductRouteRes.iProcessProductID === SchParam.iProcessProductID &&
      as_SchProductRouteRes.iSchSdID === SchParam.iSchSdID
    ) {
      let i = 1
    }
    try {
      if (!this.resTimeRange?.CheckResTimeRange()) {
        throw new Error(
          '出错位置：倒排删除已排任务时间段时，排程时段检查出错TimeSchTask.TaskTimeRangeSplit！',
        )
      }
      if (
        as_SchProductRouteRes.iProcessProductID ===
          SchParam.iProcessProductID &&
        as_SchProductRouteRes.iSchSdID === SchParam.iSchSdID
      ) {
        let i = 1
      }
      if (this.resTimeRange?.cIsInfinityAbility !== '1') {
        let taskTimeRange2 = this.resTimeRange?.WorkTimeRangeList.find(
          (p) =>
            p.iSchSdID === this.iSchSdID &&
            p.iProcessProductID === this.iProcessProductID &&
            p.iResProcessID === this.iResProcessID &&
            p.dBegTime === this.dBegTime,
        ) //
        if (taskTimeRange2 == null) {
          return
        }
        try {
          let noWorkTaskTimeRange = this.GetNoWorkTaskTimeRange(
            taskTimeRange2.dBegTime,
            taskTimeRange2.dEndTime,
            false,
          )
          noWorkTaskTimeRange.addTaskTimeRange(this.resTimeRange)
          taskTimeRange2.removeWorkTimeRange(this.resTimeRange)
          if (!this.resTimeRange?.CheckResTimeRange()) {
            throw new Error(
              `清除任务时出错,倒排删除已排任务时间段,位置ReTimeRange.TaskTimeRangeClear！工序ID号：${as_SchProductRouteRes.iProcessProductID}`,
            )
          }
        } catch (error) {
          throw new Error(
            `清除任务时出错,位置ReTimeRange.TaskTimeRangeClear！工序ID号：${
              as_SchProductRouteRes.iSchSdID
            };${as_SchProductRouteRes.iProcessProductID};任务号${
              as_SchProductRouteRes.iResProcessID
            }开工时间:${this.dBegTime.toString()}\n\r ${error.message}`,
          )
        }
        if (this.resTimeRange?.taskTimeRangeList.length > 1) {
          this.resTimeRange.MegTaskTimeRangeAll()
        }
        if (!this.resTimeRange?.CheckResTimeRange()) {
          throw new Error(
            `清除任务时出错,倒排删除已排任务时间段,位置ReTimeRange.TaskTimeRangeClear！工序ID号：${as_SchProductRouteRes.iProcessProductID}`,
          )
        }
      }
      let taskTimeRange1 = as_SchProductRouteRes.TaskTimeRangeList.find(
        (p) =>
          p.iSchSdID === this.iSchSdID &&
          p.iProcessProductID === this.iProcessProductID &&
          p.iResProcessID === this.iResProcessID &&
          p.dBegTime === this.dBegTime,
      )
      as_SchProductRouteRes.TaskTimeRangeList = as_SchProductRouteRes.TaskTimeRangeList.filter(
        (item) => item !== taskTimeRange1,
      )
    } catch (error) {
      throw new Error(
        `清除任务时出错,位置ReTimeRange.TaskTimeRangeClear！工序ID号：${
          as_SchProductRouteRes.iSchSdID
        };${as_SchProductRouteRes.iProcessProductID};任务号${
          as_SchProductRouteRes.iResProcessID
        }开工时间:${this.dBegTime.toString()}\n\r ${error.message}`,
      )
    }
  }

  public addTaskTimeRange(
    as_ResTimeRange: ResTimeRange,
    as_NewTaskTimeRange: TaskTimeRange | null = null,
  ): number {
    let SchParam=this.SchParam
    if (
      as_ResTimeRange.iSchSdID === SchParam.iSchSdID &&
      as_ResTimeRange.iProcessProductID === SchParam.iProcessProductID
    ) {
      let i = 1
    }
    if (
      this.cResourceNo === 'gys20097' &&
      this.resTimeRange?.iPeriodID === 629217169
    ) {
      let i = 0
    }
    if (as_NewTaskTimeRange != null) {
      this.dBegTime = as_NewTaskTimeRange.dBegTime
      this.dEndTime = as_NewTaskTimeRange.dEndTime
    } else {
      this.resTimeRange = as_ResTimeRange
      as_ResTimeRange.taskTimeRangeList.push(this)
    }
    return 1
  }

  public removeTaskTimeRange(as_ResTimeRange: ResTimeRange): number {
    let SchParam=this.SchParam
    if (
      as_ResTimeRange.iSchSdID === SchParam.iSchSdID &&
      as_ResTimeRange.iProcessProductID === SchParam.iProcessProductID
    ) {
      let i = 1
    }
    if (
      this.cResourceNo === 'gys20097' &&
      this.resTimeRange?.iPeriodID === 629217169
    ) {
      let i = 0
    }
    as_ResTimeRange.taskTimeRangeList = as_ResTimeRange.taskTimeRangeList.filter(
      (item) => item !== this,
    )
    return 1
  }

  public addWorkTimeRange(as_ResTimeRange: ResTimeRange): number {
    let SchParam=this.SchParam
    if (
      as_ResTimeRange.iSchSdID === SchParam.iSchSdID &&
      as_ResTimeRange.iProcessProductID === SchParam.iProcessProductID
    ) {
      let i = 1
    }
    if (
      this.cResourceNo === 'gys20097' &&
      this.resTimeRange?.iPeriodID === 629217169
    ) {
      let i = 0
    }
    this.resTimeRange = as_ResTimeRange
    as_ResTimeRange.WorkTimeRangeList.push(this)
    return 1
  }

  public removeWorkTimeRange(as_ResTimeRange: ResTimeRange): number {
    let SchParam=this.SchParam
    if (
      as_ResTimeRange.iSchSdID === SchParam.iSchSdID &&
      as_ResTimeRange.iProcessProductID === SchParam.iProcessProductID
    ) {
      let i = 1
    }
    if (
      this.cResourceNo === 'gys20097' &&
      this.resTimeRange?.iPeriodID === 629217169
    ) {
      let i = 0
    }
    as_ResTimeRange.WorkTimeRangeList = as_ResTimeRange.WorkTimeRangeList.filter(
      (item) => item !== this,
    )
    return 1
  }
}
