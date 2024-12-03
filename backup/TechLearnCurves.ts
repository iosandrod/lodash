import { DataRow, SchData, SchProductRouteRes } from './type'; // Replace with actual data library import

class TechLearnCurves {
    public schData: SchData | null = null; // 所有排程数据
    private schProductRouteRes: SchProductRouteRes; // 工艺特征对应的资源任务
    public cResourceNo: string = ""; // 对应资源编号,要设置

    public iInterID: number; // 学习曲线类型ID
    public cLearnCurvesNo: string; // 学习曲线类型
    public cLearnCurvesName: string; // 学习曲线名称
    public cTechNo: string; // 工艺编号
    public iDayDis1: number; // 学习曲线第1天标准产能折扣
    public iDayDis2: number; // 学习曲线第1天标准产能折扣
    public iDayDis3: number; // 学习曲线第1天标准产能折扣
    public iDayDis4: number; // 学习曲线第1天标准产能折扣
    public iDayDis5: number; // 学习曲线第1天标准产能折扣
    public iDayDis6: number; // 学习曲线第1天标准产能折扣
    public iDayDis7: number; // 学习曲线第1天标准产能折扣
    public iDayDis8: number; // 学习曲线第1天标准产能折扣
    public iDayDis9: number; // 学习曲线第1天标准产能折扣
    public iDayDis10: number; // 学习曲线第1天标准产能折扣
    public iDayDis11: number; // 学习曲线第1天标准产能折扣
    public iDayDis12: number; // 学习曲线第1天标准产能折扣
    public iDayDis13: number; // 学习曲线第1天标准产能折扣
    public iDayDis14: number; // 学习曲线第1天标准产能折扣
    public iDayDis15: number; // 学习曲线第1天标准产能折扣
    public iDayDis16: number; // 学习曲线第1天标准产能折扣
    public iDayDis17: number; // 学习曲线第1天标准产能折扣
    public iDayDis18: number; // 学习曲线第1天标准产能折扣
    public iDayDis19: number; // 学习曲线第1天标准产能折扣
    public iDayDis20: number; // 学习曲线第1天标准产能折扣
    public iDayDis21: number; // 学习曲线第1天标准产能折扣
    public iDayDis22: number; // 学习曲线第1天标准产能折扣
    public iDayDis23: number; // 学习曲线第1天标准产能折扣
    public iDayDis24: number; // 学习曲线第1天标准产能折扣
    public iDayDis25: number; // 学习曲线第1天标准产能折扣
    public iDayDis26: number; // 学习曲线第1天标准产能折扣
    public iDayDis27: number; // 学习曲线第1天标准产能折扣
    public iDayDis28: number; // 学习曲线第1天标准产能折扣
    public iDayDis29: number; // 学习曲线第1天标准产能折扣
    public iDayDis30: number; // 学习曲线第1天标准产能折扣
    public iDayDis31: number; // 学习曲线第1天标准产能折扣
    public iDiffCoe: number; // 加工难度系数
    public iCapacity: number; // 单件工时（暂不用）
    public iResPreTime: number; // 资源前准备时间（暂不用）
    public iResCapacity: number; // 资源的单件工时
    public dtBegDate: Date; // 学习曲线开始日期
    public dtCurDate: Date; // 学习曲线排产日期
    public iDays: number; // 学习曲线第几天
    public iDayCapacity: number; // 学习曲线第几天
    public iDayHourDis: number; // 学习曲线当前天数折扣
    public cDefine1: string;
    public cDefine2: string;
    public cDefine3: string;
    public cDefine4: string;
    public cDefine5: string;

    constructor(cLearnCurvesNo: string, schProductRouteRes: SchProductRouteRes) {
        this.schProductRouteRes = schProductRouteRes;
        this.cResourceNo = schProductRouteRes.cResourceNo;
        this.schData = schProductRouteRes.schData;
        if (cLearnCurvesNo === "") return;
        this.getTechLearnCurves(cLearnCurvesNo);
    }

    public getTechLearnCurves(cLearnCurvesNo: string): void {
        const dr: DataRow[] = this.schData.dtTechLearnCurves.select(`cLearnCurvesNo = '${cLearnCurvesNo}'`);
        if (dr.length < 1) return;
        this.getTechLearnCurvesFromRow(dr[0]);
    }

    public getTechLearnCurvesFromRow(drTechLearnCurves: DataRow): void {
        try {
            this.iInterID = drTechLearnCurves["iInterID"] as number; // 工艺特征内码
            this.cLearnCurvesNo = drTechLearnCurves["cLearnCurvesNo"].toString(); // 工艺特征编号
            this.cLearnCurvesName = drTechLearnCurves["cLearnCurvesName"].toString(); // 工艺特征编号
            this.cTechNo = drTechLearnCurves["cTechNo"].toString(); // 工艺编号
            this.iDayDis1 = parseFloat(drTechLearnCurves["iDayDis1"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis2 = parseFloat(drTechLearnCurves["iDayDis2"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis3 = parseFloat(drTechLearnCurves["iDayDis3"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis4 = parseFloat(drTechLearnCurves["iDayDis4"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis5 = parseFloat(drTechLearnCurves["iDayDis5"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis6 = parseFloat(drTechLearnCurves["iDayDis6"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis7 = parseFloat(drTechLearnCurves["iDayDis7"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis8 = parseFloat(drTechLearnCurves["iDayDis8"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis9 = parseFloat(drTechLearnCurves["iDayDis9"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis10 = parseFloat(drTechLearnCurves["iDayDis10"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis11 = parseFloat(drTechLearnCurves["iDayDis11"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis12 = parseFloat(drTechLearnCurves["iDayDis12"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis13 = parseFloat(drTechLearnCurves["iDayDis13"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis14 = parseFloat(drTechLearnCurves["iDayDis14"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis15 = parseFloat(drTechLearnCurves["iDayDis15"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis16 = parseFloat(drTechLearnCurves["iDayDis16"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis17 = parseFloat(drTechLearnCurves["iDayDis17"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis18 = parseFloat(drTechLearnCurves["iDayDis18"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis19 = parseFloat(drTechLearnCurves["iDayDis19"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis20 = parseFloat(drTechLearnCurves["iDayDis20"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis21 = parseFloat(drTechLearnCurves["iDayDis21"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis22 = parseFloat(drTechLearnCurves["iDayDis22"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis23 = parseFloat(drTechLearnCurves["iDayDis23"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis24 = parseFloat(drTechLearnCurves["iDayDis24"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis25 = parseFloat(drTechLearnCurves["iDayDis25"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis26 = parseFloat(drTechLearnCurves["iDayDis26"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis27 = parseFloat(drTechLearnCurves["iDayDis27"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis28 = parseFloat(drTechLearnCurves["iDayDis28"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis29 = parseFloat(drTechLearnCurves["iDayDis29"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis30 = parseFloat(drTechLearnCurves["iDayDis30"].toString()); // 学习曲线第1天标准产能折扣
            this.iDayDis31 = parseFloat(drTechLearnCurves["iDayDis31"].toString()); // 学习曲线第1天标准产能折扣
            this.cDefine1 = drTechLearnCurves["cDefine22"].toString();
            this.cDefine2 = drTechLearnCurves["cDefine23"].toString();
            this.cDefine3 = drTechLearnCurves["cDefine24"].toString();
            this.cDefine4 = drTechLearnCurves["cDefine25"].toString();
            this.cDefine5 = drTechLearnCurves["cDefine26"].toString();
        } catch (exp) {
            throw exp;
        }
    }

    public getDayDisValue(dtBegDate: Date, dtCurDate: Date): number {
        let iDayDis = 1;
        if (this.schProductRouteRes.dActResBegDate != null && this.schProductRouteRes.dActResBegDate > new Date("1900-01-01")) {
            dtBegDate = this.schProductRouteRes.dActResBegDate;
        } else if (this.schProductRouteRes.schProductRoute.dActBegDate != null && this.schProductRouteRes.schProductRoute.dActBegDate > new Date("1900-01-01")) {
            dtBegDate = this.schProductRouteRes.dActResBegDate;
        }
        const iDays = SchData.getDateDiff(this.schProductRouteRes.resource, "d", dtBegDate, dtCurDate);
        if (iDays <= 1) {
            iDayDis = this.iDayDis1;
        } else if (iDays === 2) {
            iDayDis = this.iDayDis2;
        } else if (iDays === 3) {
            iDayDis = this.iDayDis3;
        } else if (iDays === 4) {
            iDayDis = this.iDayDis4;
        } else if (iDays === 5) {
            iDayDis = this.iDayDis5;
        } else if (iDays === 6) {
            iDayDis = this.iDayDis6;
        } else if (iDays === 7) {
            iDayDis = this.iDayDis7;
        } else if (iDays === 8) {
            iDayDis = this.iDayDis8;
        } else if (iDays === 9) {
            iDayDis = this.iDayDis9;
        } else if (iDays === 10) {
            iDayDis = this.iDayDis10;
        } else if (iDays === 11) {
            iDayDis = this.iDayDis11;
        } else if (iDays === 12) {
            iDayDis = this.iDayDis12;
        } else if (iDays === 13) {
            iDayDis = this.iDayDis13;
        } else if (iDays === 14) {
            iDayDis = this.iDayDis14;
        } else if (iDays === 15) {
            iDayDis = this.iDayDis15;
        } else if (iDays === 16) {
            iDayDis = this.iDayDis16;
        } else if (iDays === 17) {
            iDayDis = this.iDayDis17;
        } else if (iDays === 18) {
            iDayDis = this.iDayDis18;
        } else if (iDays === 19) {
            iDayDis = this.iDayDis19;
        } else if (iDays === 20) {
            iDayDis = this.iDayDis20;
        } else if (iDays === 21) {
            iDayDis = this.iDayDis21;
        } else if (iDays === 22) {
            iDayDis = this.iDayDis22;
        } else if (iDays === 23) {
            iDayDis = this.iDayDis23;
        } else if (iDays === 24) {
            iDayDis = this.iDayDis24;
        } else if (iDays === 25) {
            iDayDis = this.iDayDis25;
        } else if (iDays === 26) {
            iDayDis = this.iDayDis26;
        } else if (iDays === 27) {
            iDayDis = this.iDayDis27;
        } else if (iDays === 28) {
            iDayDis = this.iDayDis28;
        } else if (iDays === 29) {
            iDayDis = this.iDayDis29;
        } else if (iDays === 30) {
            iDayDis = this.iDayDis30;
        } else {
            iDayDis = this.iDayDis31;
        }
        return iDayDis;
    }

    public getWorkItemDays(schProductRouteRes: SchProductRouteRes, dtBegDate: Date): number {
        let liReturn = 0;
        return liReturn;
    }
}

