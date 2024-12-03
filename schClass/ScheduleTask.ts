//@ts-nocheck
class ScheduleTask {
  productOrderID: string
  batch: number
  scheduleStyle: number = 0
  relateID: string
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
  planProcessingTimeList: ResTimeRange[] = []
  actualProcessingTimeList: ResTimeRange[] = []
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
  preTaskList: ScheduleTask[] = []
  postTaskList: ScheduleTask[] = []
  proState: number
  passRate: number
  maySelecetResource: ScheduleResource[] = []

  constructor(id: number, relateID: string) {
    this.id = id
    this.relateID = relateID
  }

  compareTo(obj: ScheduleTask): number {
    if (this.planStartTime - obj.planStartTime > 0.001) {
      return 1
    } else if (obj.planStartTime - this.planStartTime > 0.001) {
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
    this.planProcessingTimeList = []
    this.planStartTime = 0
  }

  deepClone(): ScheduleTask {
    const st = new ScheduleTask(this.id, this.relateID)
    st.actualEndTime = this.actualEndTime
    st.actualStartTime = this.actualStartTime
    st.affiliationOrder = this.affiliationOrder
    st.affiliationProduct = this.affiliationProduct
    st.batch = this.batch
    st.earliestStartTime = this.earliestStartTime
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

    this.planProcessingTimeList.forEach((item) => {
      st.planProcessingTimeList.push(item.clone())
    })
    this.actualProcessingTimeList.forEach((item) => {
      st.actualProcessingTimeList.push(item.clone())
    })

    this.preTaskList.forEach((task) => {
      st.preTaskList.push(task)
    })

    this.postTaskList.forEach((task) => {
      st.postTaskList.push(task)
    })

    return st
  }

  get ProcessingTime(): number {
    return this.batch * this.onepeiceProcessTime
  }

  get actualProcessingTime(): number {
    return this.actualProcessingTimeList.reduce(
      (aTime, t) => aTime + t.HoldingTime,
      0,
    )
  }
}

enum TaskType {
  Design = 'Design', //设计
  Normal = 'Normal', //普通
  Outsource = 'Outsource', //外协
}

class ResTimeRange {
  HoldingTime: number
  clone(): ResTimeRange {
    return new ResTimeRange()
  }
}

class ScheduleResource {}

class Scheduling {
  static maxTimeValue: number = Number.MAX_VALUE
}
