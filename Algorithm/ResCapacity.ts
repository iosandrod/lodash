import { ArrayList } from './type'

export class ResCapacity {
  public FResourceID: number // Resource ID
  public FProductID: number // Product ID
  public FProcessID: number // Process number in the product process model
  public FEfficiency: number // Efficiency
  public FCapacity1: number // Capacity 10 seconds/PCS
  public FCapacity2: number
  public FPreSetoutTime: number // Pre-setup time
  public FPostSetoutTime: number // Post-setup time
  public FProcessPassRate: number // Pass rate
  public viceResIDList: ArrayList<number> // Store副资源引用
  public FPriority: number // Main resource priority
  public FTechCharacter1: string = '' // Product current process technology feature 1
  public FTechCharacter2: string = '' // Product current process technology feature 2
  public FTechCharacter3: string = '' // Product current process technology feature 3
  public FTechCharacter4: string = '' // Product current process technology feature 4
  public FTechCharacter5: string = '' // Product current process technology feature 5
  public FTechCharacter6: string = '' // Product current process technology feature 6
  public FWorkType: number = 0 // Production method  0 Single piece production / 1 Batch production
  public FBatchWorkTime: number = 0 // Production time per batch
  public FBatchQty: number = 0 // Production quantity per batch
  public FBatchInterTime: number = 0 // Interval time between batches
  private iPreTaskID: number = 0 // Previous task ID, used to get pre-setup time
  public get IPreTaskID(): number {
    return this.iPreTaskID
  }
  public set IPreTaskID(value: number) {
    this.iPreTaskID = value
  }
  private iPreTaskTime: number = 0 // Interval time with previous task, used to determine whether to consider pre-setup time
  public get IPreTaskTime(): number {
    return this.iPreTaskTime
  }
  public set IPreTaskTime(value: number) {
    this.iPreTaskTime = value
  }
  private iTaskID: number = 0 // Task ID
  public get ITaskID(): number {
    return this.iTaskID
  }
  public set ITaskID(value: number) {
    this.iTaskID = value
  }
  private planQty: number = 0 // Planned production quantity
  public get PlanQty(): number {
    return this.planQty
  }
  public set PlanQty(value: number) {
    this.planQty = value
    if (this.FWorkType === 0) {
      // Single piece production
      this.workTime = this.planQty * this.FCapacity1
    } else if (this.FWorkType === 1) {
      // Batch production
      // Logic for batch production (not implemented in the original code)
    } else {
      this.workTime = 1
    } 
  }
  private workTime: number = 0 // Planned processing time (excluding pre and post setup time)
  public get WorkTime(): number {
    return this.workTime
  }
  public set WorkTime(value: number) {
    this.workTime = value
  }
}
