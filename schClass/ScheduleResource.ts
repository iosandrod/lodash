class ScheduleResource {
  private id: number
  private type: number
  private number: number
  private occupyStyle: number

  public isKey: number
  public name: string
  public resTimeStyle: number = 0
  public resTimeGroup: number = 4
  public isHasViceRes: boolean = false
  public viceRes: ScheduleResource | null = null

  public capacityList: any[] = []
  public holdingTimeList: any[] = []
  public idleAndWorkTimeList: any[] = []
  public taskList: any[] = []
  public tempHoldingTimeList: any[] = []
  public tempIdleTimeList: any[] = []
  public tempIdleDetailTimeList: any[] = []

  constructor(id: number = 0) {
    this.id = id
  }

  get ID(): number {
    return this.id
  }

  get Type(): number {
    return this.type
  }

  set Type(value: number) {
    this.type = value
  }

  get Number(): number {
    return this.number
  }

  set Number(value: number) {
    this.number = value
  }

  get OccupyStyle(): number {
    return this.occupyStyle
  }

  set OccupyStyle(value: number) {
    this.occupyStyle = value
  }

  // Initialization method
  iniData(): void {
    // Add any initialization logic if needed
  }

  // Deep clone method
  deepClone(): ScheduleResource {
    const temp = new ScheduleResource()
    // Implement deep copy logic if required
    return temp
  }

  // Comparison method (to be implemented)
  CompareIdleAndWorkTimeList(aarr_idleAndWorkTimeList: any[]): any[] {
    return []
  }
}
