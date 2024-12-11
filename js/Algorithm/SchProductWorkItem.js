"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SchProductWorkItem = void 0;
const Base_1 = require("./Base");
class SchProductWorkItem extends Base_1.Base {
    constructor() {
        super(...arguments);
        this.schData = null; // All scheduling data
        this.bScheduled = 0; // Product scheduling status
        this.iPriority = -1; // Product priority
        this.bSet = 'false'; // Is it necessary to match
        this.iSchPriority = -1; // Product scheduling priority
        this.iSchBatch = 6; // Scheduling batch
        this.cType = ''; // Planning type
        this.cSchType = '0'; // Scheduling type
        this.cPlanMode = '2'; // Planning type
        this.cScheduled = '0'; // Scheduling confirmation
        this.cBatchNo = ''; // Tray number
        this.iSchSN = 0; // Scheduling order
        this.cGroupSN = 0; // Group number
        this.cGroupQty = 0; // Group quantity
        this.cCustomize = ''; // Is it customized
        this.cWorkRouteType = ''; // Work route type
        this.cAttributes1 = ''; // Processing attribute 1
        this.cAttributes2 = ''; // Processing attribute 2
        this.cAttributes3 = ''; // Processing attribute 3
        this.cAttributes4 = ''; // Processing attribute 4
        this.cAttributes5 = ''; // Processing attribute 5
        this.cAttributes6 = ''; // Processing attribute 6
        this.cAttributes7 = ''; // Processing attribute 7
        this.cAttributes8 = ''; // Processing attribute 8
        this.cAttributes9 = 0; // Processing attribute 9
        this.cAttributes10 = 0; // Processing attribute 10
        this.cAttributes11 = 0; // Processing attribute 11
        this.cAttributes12 = 0; // Processing attribute 12
        this.cAttributes13 = ''; // Processing attribute 13
        this.cAttributes14 = ''; // Processing attribute 14
        this.cAttributes15 = ''; // Processing attribute 15
        this.cAttributes16 = ''; // Processing attribute 16
        this.SchProductRouteList = new Array(10);
    }
    ProductSchTask() {
        let SchParam = this.SchParam;
        try {
            if (this.SchProductRouteList.length < 1)
                return 1; // Some products have no processes, skip
            const schProductRouteTemp = this.SchProductRouteList.filter((p1) => p1.iSchSdID === this.iSchSdID &&
                p1.cWorkItemNo.trim().toLowerCase() ===
                    this.cInvCode.trim().toLowerCase());
            schProductRouteTemp.sort((p1, p2) => p1.iWoSeqID - p2.iWoSeqID);
            if (schProductRouteTemp.length < 1)
                return 1;
            const schProductRouteLast = schProductRouteTemp[schProductRouteTemp.length - 1];
            schProductRouteLast.ProcessSchTaskPre();
            const list1 = this.SchProductRouteList.filter((p1) => p1.iSchSdID === this.iSchSdID && p1.iReqQty > 0);
            if (list1.length > 0) {
                list1.sort((p1, p2) => p1.dEndDate.getTime() - p2.dEndDate.getTime());
                this.dBegDate = list1[0].dBegDate; // Get the maximum completion time from scheduled tasks
                this.dEndDate = list1[list1.length - 1].dEndDate; // Get the maximum completion time from scheduled tasks
                this.iWoPriorityResLast = SchParam.iSchWoSNMax++; // Maximum scheduling task count
                this.bScheduled = 1; // Mark as scheduled
            }
        }
        catch (error) {
            throw new Error(`Error calculating product scheduling, location SchProduct.ProductSchTask! Product number [${this.cInvCode}]\n ${error.message}`);
            return -1;
        }
        return 1;
    }
    ProductSchTaskInv() {
        let SchParam = this.SchParam;
        try {
            if (this.SchProductRouteList.length < 1)
                return 1; // Some products have no processes, skip
            const schProductRouteTemp = this.SchProductRouteList.filter((p1) => p1.iSchSdID === this.iSchSdID &&
                p1.cWorkItemNo.trim().toLowerCase() ===
                    this.cInvCode.trim().toLowerCase());
            schProductRouteTemp.sort((p1, p2) => p1.iWoSeqID - p2.iWoSeqID);
            if (schProductRouteTemp.length < 1)
                return 1;
            const schProductRouteLast = schProductRouteTemp[schProductRouteTemp.length - 1];
            schProductRouteLast.ProcessSchTaskRevPre('3'); // 1 Same processing material for all processes; 2 White blank process; 3 All lower-level material semi-finished processes
            const list1 = this.SchProductRouteList.filter((p1) => p1.iSchSdID === this.iSchSdID && p1.iReqQty > 0);
            if (list1.length > 0) {
                list1.sort((p1, p2) => p1.dEndDate.getTime() - p2.dEndDate.getTime());
                this.dBegDate = list1[0].dBegDate; // Get the maximum completion time from scheduled tasks
                this.dEndDate = list1[list1.length - 1].dEndDate; // Get the maximum completion time from scheduled tasks
                this.iWoPriorityResLast = SchParam.iSchWoSNMax++; // Maximum scheduling task count
                this.bScheduled = 1; // Mark as scheduled
            }
        }
        catch (error) {
            throw new Error(`Error calculating product scheduling, location SchProduct.ProductSchTask! Product number [${this.cInvCode}]\n ${error.message}`);
            return -1;
        }
        return 1;
    }
}
exports.SchProductWorkItem = SchProductWorkItem;
