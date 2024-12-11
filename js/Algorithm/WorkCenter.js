"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WorkCenter = void 0;
const type_1 = require("./type"); // Assuming SchData is defined in a separate file
class WorkCenter extends type_1.Base {
    constructor(cWcNo, as_SchData) {
        super();
        this.schData = null;
        this.ListResource = new Array(10);
        if (cWcNo && as_SchData) {
            this.schData = as_SchData;
            const dr = this.schData.dtWorkCenter.filter((row) => row.cWcNo === cWcNo);
            if (dr.length < 1)
                return;
            this.GetWorkCenter(dr[0]);
        }
    }
    GetWorkCenter(drWorkCenter) {
        try {
            this.iWcID = drWorkCenter.iWcID;
            this.cWcNo = drWorkCenter.cWcNo;
            this.cWcName = drWorkCenter.cWcName;
            this.cWcClsNo = drWorkCenter.cWcClsNo;
            this.cAbilityMode = drWorkCenter.cAbilityMode;
            this.cDeptNo = drWorkCenter.cDeptNo;
            this.iWorkersPd = Number(drWorkCenter.iWorkersPd);
            this.iShiftPd = Number(drWorkCenter.iShiftPd);
            this.iDevCount = Number(drWorkCenter.iDevCount);
            this.iUsage = Number(drWorkCenter.iUsage);
            this.iEfficient = Number(drWorkCenter.iEfficient);
            this.cEntrustNo = drWorkCenter.cEntrustNo;
            this.bKeyRes = drWorkCenter.bKeyRes;
            this.cStatus = drWorkCenter.cStatus;
            this.iLaborRate = Number(drWorkCenter.iLaborRate);
            this.iFixRate = Number(drWorkCenter.iFixRate);
            this.iFlexRate = Number(drWorkCenter.iFlexRate);
            this.bBackflash = drWorkCenter.bBackflash;
            this.bWasteDirect = drWorkCenter.bWasteDirect;
            this.cDutyor = drWorkCenter.cDutyor;
            this.bFakeRet = drWorkCenter.bFakeRet;
            this.cNote = drWorkCenter.cNote;
            this.cLaborRateType = drWorkCenter.cLaborRateType;
            this.iOverHours = Number(drWorkCenter.iOverHours);
            this.cPrvNo = drWorkCenter.cPrvNo;
            this.cWcDefine1 = drWorkCenter.cWcDefine1;
            this.cWcDefine2 = drWorkCenter.cWcDefine2;
            this.cWcDefine3 = drWorkCenter.cWcDefine3;
            this.cWcDefine4 = drWorkCenter.cWcDefine4;
            this.cWcDefine5 = drWorkCenter.cWcDefine5;
            this.cWcDefine6 = drWorkCenter.cWcDefine6;
            this.cWcDefine7 = drWorkCenter.cWcDefine7;
            this.cWcDefine8 = drWorkCenter.cWcDefine8;
            this.cWcDefine9 = drWorkCenter.cWcDefine9;
            this.cWcDefine10 = drWorkCenter.cWcDefine10;
            this.cWcDefine11 = Number(drWorkCenter.cWcDefine11);
            this.cWcDefine12 = Number(drWorkCenter.cWcDefine12);
            this.cWcDefine13 = Number(drWorkCenter.cWcDefine13);
            this.cWcDefine14 = Number(drWorkCenter.cWcDefine14);
            this.cWcDefine15 = new Date(drWorkCenter.cWcDefine15);
            this.cWcDefine16 = new Date(drWorkCenter.cWcDefine16);
        }
        catch (exp) {
            throw exp;
        }
    }
}
exports.WorkCenter = WorkCenter;
