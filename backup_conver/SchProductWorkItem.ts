import { SchData, SchParam, SchProductRoute } from './type'

export class SchProductWorkItem {
  schData: SchData | null = null // All scheduling data
  bScheduled: number = 0 // Product scheduling status
  iSchSdID: number
  iBomAutoID: number // Primary key ID
  cVersionNo: string
  iInterID: number
  iSdLineID: number
  iSeqID: number
  iModelID: number
  cSdOrderNo: string
  cCustNo: string
  cCustName: string
  cSTCode: string
  cBusType: string
  cPriorityType: number
  cStatus: string
  cSales: string
  cRequireType: string
  iItemID: number
  cInvCode: string
  cInvCodeFull: string
  cInvName: string
  cInvStd: string
  cUnitCode: string
  iReqQty: number
  cEnough: string // Is there enough material
  iBomLevel: number // BOM level
  dRequireDate: Date // Required date
  dCanEndDate: Date // Latest delivery date
  dCanBegDate: Date // Earliest scheduling date
  dBegDate: Date // Scheduling start time
  dEndDate: Date // Scheduling end time
  dProductDate: Date // Product delivery date
  cSchStatus: string
  cMiNo: string
  iPriority: number = -1 // Product priority
  cSelected: string
  cWoNo: string // Work order number
  iPlanQty: number
  cNeedSet: string
  iFHQuantity: number
  iKPQuantity: number
  iSourceLineID: number
  cColor: string
  cNote: string
  bSet: string = 'false' // Is it necessary to match
  iSchPriority: number = -1 // Product scheduling priority
  iSchBatch: number = 6 // Scheduling batch
  cType: string = '' // Planning type
  cSchType: string = '0' // Scheduling type
  cPlanMode: string = '2' // Planning type
  cScheduled: string = '0' // Scheduling confirmation
  iAdvanceDate: number // Cumulative advance days
  cBatchNo: string = '' // Tray number
  iSchSN: number = 0 // Scheduling order
  cSchSNType: string // Scheduling order type
  iWoPriorityRes: number // Scheduling order
  iWoPriorityResLast: number // Last scheduling order
  cGroupSN: number = 0 // Group number
  cGroupQty: number = 0 // Group quantity
  cCustomize: string = '' // Is it customized
  cWorkRouteType: string = '' // Work route type
  cAttributes1: string = '' // Processing attribute 1
  cAttributes2: string = '' // Processing attribute 2
  cAttributes3: string = '' // Processing attribute 3
  cAttributes4: string = '' // Processing attribute 4
  cAttributes5: string = '' // Processing attribute 5
  cAttributes6: string = '' // Processing attribute 6
  cAttributes7: string = '' // Processing attribute 7
  cAttributes8: string = '' // Processing attribute 8
  cAttributes9: number = 0 // Processing attribute 9
  cAttributes10: number = 0 // Processing attribute 10
  cAttributes11: number = 0 // Processing attribute 11
  cAttributes12: number = 0 // Processing attribute 12
  cAttributes13: string = '' // Processing attribute 13
  cAttributes14: string = '' // Processing attribute 14
  cAttributes15: string = '' // Processing attribute 15
  cAttributes16: string = '' // Processing attribute 16
  iDeliveryDays: number // Delivery days
  iWorkQtyPd: number // Daily scheduling quantity

  SchProductRouteList: SchProductRoute[] = new Array<SchProductRoute>(10)

  ProductSchTask(): number {
    try {
      if (this.SchProductRouteList.length < 1) return 1 // Some products have no processes, skip
      const schProductRouteTemp: any = this.SchProductRouteList.filter(
        (p1) =>
          p1.iSchSdID === this.iSchSdID &&
          p1.cWorkItemNo.trim().toLowerCase() ===
            this.cInvCode.trim().toLowerCase(),
      )
      schProductRouteTemp.sort((p1, p2) => p1.iWoSeqID - p2.iWoSeqID)
      if (schProductRouteTemp.length < 1) return 1
      const schProductRouteLast =
        schProductRouteTemp[schProductRouteTemp.length - 1]
      schProductRouteLast.ProcessSchTaskPre()
      const list1: any = this.SchProductRouteList.filter(
        (p1: any) => p1.iSchSdID === this.iSchSdID && p1.iReqQty > 0,
      )
      if (list1.length > 0) {
        list1.sort(
          (p1: any, p2: any) => p1.dEndDate.getTime() - p2.dEndDate.getTime(),
        )
        this.dBegDate = list1[0].dBegDate // Get the maximum completion time from scheduled tasks
        this.dEndDate = list1[list1.length - 1].dEndDate // Get the maximum completion time from scheduled tasks
        this.iWoPriorityResLast = SchParam.iSchWoSNMax++ // Maximum scheduling task count
        this.bScheduled = 1 // Mark as scheduled
      }
    } catch (error) {
      throw new Error(
        `Error calculating product scheduling, location SchProduct.ProductSchTask! Product number [${this.cInvCode}]\n ${error.message}`,
      )
      return -1
    }
    return 1
  }

  ProductSchTaskInv(): number {
    try {
      if (this.SchProductRouteList.length < 1) return 1 // Some products have no processes, skip
      const schProductRouteTemp = this.SchProductRouteList.filter(
        (p1: any) =>
          p1.iSchSdID === this.iSchSdID &&
          p1.cWorkItemNo.trim().toLowerCase() ===
            this.cInvCode.trim().toLowerCase(),
      )
      schProductRouteTemp.sort((p1: any, p2: any) => p1.iWoSeqID - p2.iWoSeqID)
      if (schProductRouteTemp.length < 1) return 1
      const schProductRouteLast: any =
        schProductRouteTemp[schProductRouteTemp.length - 1]
      schProductRouteLast.ProcessSchTaskRevPre('3') // 1 Same processing material for all processes; 2 White blank process; 3 All lower-level material semi-finished processes
      const list1: any = this.SchProductRouteList.filter(
        (p1: any) => p1.iSchSdID === this.iSchSdID && p1.iReqQty > 0,
      )
      if (list1.length > 0) {
        list1.sort(
          (p1: any, p2: any) => p1.dEndDate.getTime() - p2.dEndDate.getTime(),
        )
        this.dBegDate = list1[0].dBegDate // Get the maximum completion time from scheduled tasks
        this.dEndDate = list1[list1.length - 1].dEndDate // Get the maximum completion time from scheduled tasks
        this.iWoPriorityResLast = SchParam.iSchWoSNMax++ // Maximum scheduling task count
        this.bScheduled = 1 // Mark as scheduled
      }
    } catch (error) {
      throw new Error(
        `Error calculating product scheduling, location SchProduct.ProductSchTask! Product number [${this.cInvCode}]\n ${error.message}`,
      )
      return -1
    }
    return 1
  }
}
