import {
  Resource,
  SchData,
  TaskTimeRange,
  ResSourceDayCap,
  SchProductRouteRes,
  SchParam,
  Comparer,
  DateTime,
  ResTimeRange,
  ScheduleResource,
  ArrayList,
  Scheduling
} from './type'
enum TaskType {
  Design,
  Normal,
  Outsource,
}

export class ScheduleTask {
  productOrderID: string
  batch: number
  scheduleStyle: number = 0
  private relateID: string
  private id: number
  type: TaskType
  name: string
  earliestStartTime: number
  latestEndTime: number
  planStartTime: number = 0
  planEndTime: number
  actualStartTime: number
  actualEndTime: number
  preProcessingTime: number = 0
  postProcessingTime: number = 0
  onepeiceProcessTime: number
  postTaskDelayTime: number
  postTaskDelayBatch: number
  planProcessingTimeList: ArrayList<ResTimeRange> = new ArrayList<
    ResTimeRange
  >()
  actualProcessingTimeList: ArrayList<ResTimeRange> = new ArrayList<
    ResTimeRange
  >()
  selectedMainResoure: ScheduleResource
  planEarliestStartTime: number
  planLatestEndTime: number
  isFixedTask: boolean = false
  isKeyTask: boolean = false
  postInternalTimeMin: number = 0
  transportTime: number = 0
  postInternalTimeMax: number = Scheduling.maxTimeValue
  moveType: number
  moveTime: number
  moveBatch: number
  processQuanitys: number = 1
  affiliationProduct: string
  affiliationOrder: string
  preTaskList: ArrayList<ScheduleTask> = new ArrayList<ScheduleTask>()
  postTaskList: ArrayList<ScheduleTask> = new ArrayList<ScheduleTask>()
  proState: number
  passRate: number
  maySelecetResource: ArrayList<any> = new ArrayList<any>(3)

  constructor(id?: number, relateID?: string) {
    if (id !== undefined && relateID !== undefined) {
      this.id = id
      this.relateID = relateID
    }
  }

  get RelateID(): string {
    return this.relateID
  }

  get ID(): number {
    return this.id
  }

  get ProcessingTime(): number {
    return this.batch * this.onepeiceProcessTime
  }

  get actualProcessingTime(): number {
    let aTime = 0
    for (let t of this.actualProcessingTimeList) {
      aTime += t.HoldingTime
    }
    return aTime
  }

  iniData(): void {
    // Implementation omitted
  }

  compareTo(other: ScheduleTask): number {
    if (this.planStartTime - other.planStartTime > 0.001) {
      return 1
    } else if (other.planStartTime - this.planStartTime > 0.001) {
      return -1
    } else {
      return 0
    }
  }

  clear(): void {
    if (this.earliestStartTime < 0) {
      this.earliestStartTime = 0
    }
    this.latestEndTime = Scheduling.maxTimeValue
    this.onepeiceProcessTime = 0
    this.planEndTime = Scheduling.maxTimeValue
    this.planProcessingTimeList.clear()
    this.planStartTime = 0
  }

  deepClone(): ScheduleTask {
    let st = new ScheduleTask()
    st.actualEndTime = this.actualEndTime
    st.actualStartTime = this.actualStartTime
    st.affiliationOrder = this.affiliationOrder
    st.affiliationProduct = this.affiliationProduct
    st.batch = this.batch
    st.earliestStartTime = this.earliestStartTime
    st.id = this.id
    st.isFixedTask = this.isFixedTask
    st.isKeyTask = this.isKeyTask
    st.latestEndTime = this.latestEndTime
    st.name = this.name
    st.onepeiceProcessTime = this.onepeiceProcessTime
    st.planEarliestStartTime = this.planEarliestStartTime
    st.planEndTime = this.planEndTime
    st.planLatestEndTime = this.planLatestEndTime
    st.planStartTime = this.planStartTime
    st.postInternalTimeMax = this.postInternalTimeMax
    st.postInternalTimeMin = this.postInternalTimeMin
    st.postProcessingTime = this.postProcessingTime
    st.preProcessingTime = this.preProcessingTime
    st.proState = this.proState
    st.relateID = this.relateID
    st.selectedMainResoure = this.selectedMainResoure
    st.transportTime = this.transportTime
    st.type = this.type

    for (let item of this.planProcessingTimeList) {
      st.planProcessingTimeList.add(item.Clone())
    }

    for (let item of this.actualProcessingTimeList) {
      st.actualProcessingTimeList.add(item.Clone())
    }

    for (let stask of this.preTaskList) {
      st.preTaskList.add(stask)
    }

    for (let stask of this.postTaskList) {
      st.postTaskList.add(stask)
    }

    return st
  }
}

class UnitTime {
  resourceID: string
  singleTime: number
}
