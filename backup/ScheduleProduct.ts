import { ArrayList } from './type'

export class ScheduleResource {
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
  public capacityList: ArrayList<any> = new ArrayList(5)
  public holdingTimeList: ArrayList<any> = new ArrayList(10)
  public idleAndWorkTimeList: ArrayList<any> = new ArrayList(10)
  public taskList: ArrayList<any> = new ArrayList(10)
  public tempHoldingTimeList: ArrayList<any> = new ArrayList(5)
  public tempIdleTimeList: ArrayList<any> = new ArrayList(5)
  public tempIdleDetailTimeList: ArrayList<any> = new ArrayList(5)

  constructor(id?: number) {
    if (id !== undefined) {
      this.id = id
    }
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

  public iniData(): void {
    // Implementation here
  }

  public deepClone(): ScheduleResource {
    let temp: ScheduleResource = new ScheduleResource()
    // Deep clone implementation here
    return temp
  }

  public CompareIdleAndWorkTimeList(
    aarr_idleAndWorkTimeList: ArrayList<any>,
  ): ArrayList<any> | null {
    return null
  }
}
