"use strict";
//@ts-nocheck
Object.defineProperty(exports, "__esModule", { value: true });
exports.SchInterface = void 0;
class SchInterface {
    constructor() {
        //@ts-ignore
        this.schData = new SchData();
        //@ts-ignore
        this.sqlPro = new APSRun.SqlPro();
        this.RationHourUnit = '1';
        this.ConnectString = '';
        this.ServerUri = 'ws://192.168.1.15:9000';
        this.client = null;
        this.iProgress = 0;
        this.iBatchRowCount = 50000;
        this.name = 'GetSchOperationProgress';
        this.User = 'admin';
        this.Company = '001';
        this.socketId = '123';
        this.dlShowProcess = this.ShowProcess.bind(this);
    }
    async SetInit(input) {
        const ConnectString = input.ConnectString;
        this.ServerUri = input.ServerUri;
        APSRun.SqlPro.ConnectionStrings = ConnectString;
        this.ConnectString = ConnectString;
        return `${ConnectString}__123456 _${this.ServerUri}`;
    }
    async SetInitB(ConnectString, ServerUri = 'ws://192.168.1.15:9000', Company = '', cUser = 'admin') {
        APSRun.SqlPro.ConnectionStrings = ConnectString;
        this.ConnectString = ConnectString;
        this.ServerUri = ServerUri;
        this.dlShowProcess = this.ShowProcess.bind(this);
        return '123456';
    }
    async Start(input) {
        const cVersionNo = input.cVersionNo;
        const adte_Start = new Date(input.adte_Start);
        const adte_End = new Date(input.adte_End);
        const bShowTips = true;
        const cSchType = input.cSchType;
        const cTechSchType = input.cTechSchType;
        const iCurSchRunTimes = input.iCurSchRunTimes;
        this.User = input.User;
        this.ConnectString = input.ConnectString;
        this.ServerUri = input.ServerUri;
        this.Company = input.Company;
        this.socketId = input.socketID;
        this.ldtBegDate = adte_Start;
        APSRun.SqlPro.ConnectionStrings = this.ConnectString;
        this.ServerUri = this.ServerUri;
        console.log('ConnectString ' + this.ConnectString);
        console.log('this.ServerUri ' + this.ServerUri);
        this.message = '开始智能排程';
        this.schPrecent = 0;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        this.message = '取排程参数';
        this.schPrecent = 1;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        console.log('进入排程函数');
        if (cSchType === '2') {
            cVersionNo = 'SureVersion';
        }
        this.schData.cVersionNo = cVersionNo;
        this.schData.dtStart = adte_Start;
        this.schData.dtEnd = adte_End;
        this.schData.dtToday = new Date(); // Assuming this is the current date
        this.schData.cCalculateNo = `${this.User};${this.schData.dtToday}`;
        console.log(this.schData.dtToday);
        SchParam.cVersionNo = cVersionNo;
        SchParam.dtStart = adte_Start;
        SchParam.dtEnd = adte_End;
        SchParam.dtToday = this.schData.dtToday;
        SchParam.bReSchedule = '0';
        SchParam.cSchCapType = '1';
        SchParam.cSchType = cSchType;
        SchParam.cTechSchType = cTechSchType;
        SchParam.iCurSchRunTimes = iCurSchRunTimes;
        Algorithm.SchParam.dtParamValue = await SqlPro.GetDataTable('select * from t_ParamValue where 1 = 1 ', '');
        Algorithm.SchParam.GetSchParams();
        console.log('初始化参数完成');
        this.message = '初始化参数完成';
        this.schPrecent = 2;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        if (SchParam.bReSchedule !== '1') {
            console.log('bReSchedule != 1 0第一次排');
            this.message = '第一次排';
            this.schPrecent = 2;
            this.dlShowProcess(this.schPrecent.toString(), this.message);
            try {
                const lsSql2 = `EXECUTE P_SchedulePrePre '${this.schData.cVersionNo}','${this.schData.dtStart.toString()}','${this.schData.dtEnd.toString()}','${SchParam.cSchType}','${SchParam.cTechSchType}','${this.schData.cCalculateNo}','${SchParam.iCurSchRunTimes}'`;
                await SqlPro.ExecuteNonQuery(lsSql2, null);
                const lsSql = `EXECUTE P_SchedulePre '${this.schData.cVersionNo}','${this.schData.dtStart.toString()}','${this.schData.dtEnd.toString()}','${SchParam.cSchType}','${SchParam.cTechSchType}','${this.schData.cCalculateNo}','${SchParam.iCurSchRunTimes}'`;
                this.message = '1、排程前准备工作已完成';
                this.schPrecent = 3;
                this.dlShowProcess(this.schPrecent.toString(), this.message);
                await SqlPro.ExecuteNonQuery(lsSql, null);
                console.log('执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中');
            }
            catch (excp) {
                this.message =
                    '排产计算出错！位置1、排产前处理出错,出错内容：' + excp.toString();
                this.schPrecent = 100;
                this.dlShowProcess(this.schPrecent.toString(), this.message);
                console.log('排产计算出错！位置1、排产前处理出错,出错内容：' + excp.toString());
                throw excp;
            }
            this.schData.iProgress = 5;
            console.log('完成5%');
            this.message = '1、排程前准备工作已完成:';
            this.schPrecent = this.schData.iProgress;
            this.dlShowProcess(this.schPrecent.toString(), this.message);
            console.log(bShowTips);
        }
        try {
            if ((await this.GetResourceList()) < 0)
                return -1;
        }
        catch (excp) {
            this.schPrecent = 100;
            this.dlShowProcess(this.schPrecent.toString(), this.message);
            console.log('排产计算出错！位置2、生成资源工作列表及资源工作日历,出错内容：' +
                excp.toString());
            throw excp;
        }
        this.schData.iProgress = 15;
        console.log('过程GetResourceList  15%');
        this.message = '2、资源工作日历已完成';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        if ((await this.GetSchData()) < 0)
            return -1;
        this.schData.iProgress = 20;
        console.log('过程GetSchData  20%');
        this.message = '3、生成订单产品工艺模型列表';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        console.log('开个线程统计排程明细进度');
        if ((await this.ResSchTaskInit()) < 0)
            return -1;
        this.schData.iProgress = 30;
        console.log('过程GetSchData  30%');
        this.message = '4、已开工任务排程...';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        try {
            const scheduling = new Scheduling(this.schData);
            const threadRows = new Promise((resolve) => {
                this.ShowSchProgress();
                resolve();
            });
            await threadRows;
            if ((await scheduling.SchMainRun(cSchType)) < 0)
                return -1;
        }
        catch (excp) {
            this.message =
                '排产计算出错！位置1、排产前处理出错,出错内容：' + excp.toString();
            this.schPrecent = 100;
            this.dlShowProcess(this.schPrecent.toString(), this.message);
            console.log('排产计算出错！位置1、排产前处理出错,出错内容：' + excp.toString());
            throw excp;
        }
        this.schData.iProgress = 80;
        console.log('过程GetSchData  80%');
        this.message = '5、产品订单按优先级进行排产';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        if ((await this.SaveSchData()) < 0) {
            return -1;
        }
        this.schData.iProgress = 90;
        this.message = '6、排程结果写回数据库';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        try {
            const lsSql = `EXECUTE P_SchedulePost '${this.schData.cVersionNo}','${this.schData.dtStart.toString()}','${this.schData.dtEnd.toString()}','${this.schData.dtToday.toString()}','${SchParam.cSchType}','${SchParam.cTechSchType}','${this.schData.cCalculateNo}','${SchParam.iCurSchRunTimes}'`;
            await SqlPro.ExecuteNonQuery(lsSql, null);
        }
        catch (excp) {
            console.log('排产计算出错！位置GetResourceList(),出错内容：' + excp.toString());
            throw excp;
        }
        this.schData.iProgress = 95;
        this.message = '7、排程后处理完成';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        console.log('过程GetSchData  95%');
        try {
            const lsSql = `EXECUTE P_SchKPI '${this.schData.cVersionNo}','${this.ldtBegDate}','${new Date()}','${this.schData.cCalculateNo}','','${SchParam.cSchType}','${SchParam.cTechSchType}'`;
            await SqlPro.ExecuteNonQuery(lsSql, null);
        }
        catch (excp) {
            console.log('排产后记录KPI' + excp.message.toString());
            throw excp;
        }
        this.schData.iProgress = 98;
        this.message = '8、排产后记录KPI完成';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        console.log('排程完毕  100%');
        this.schData.iProgress = 100;
        this.message = '排程完毕';
        this.schPrecent = this.schData.iProgress;
        this.dlShowProcess(this.schPrecent.toString(), this.message);
        return 1;
    }
    static IsNullable(t) {
        return (!t.isValueType ||
            (t.isGenericType && t.getGenericTypeDefinition() === Nullable));
    }
    static GetCoreType(t) {
        if (t != null && this.IsNullable(t)) {
            if (!t.isValueType) {
                return t;
            }
            else {
                return Nullable.GetUnderlyingType(t);
            }
        }
        else {
            return t;
        }
    }
    async GetResourceList() {
        // Implementation here
        return 0;
    }
    async GetSchData() {
        // Implementation here
        return 0;
    }
    async ResSchTaskInit() {
        // Implementation here
        return 0;
    }
    async SaveSchData() {
        // Implementation here
        return 0;
    }
    ShowProcess(schPrecent, message) {
        try {
            this.SendAsync('schPrecent:' + schPrecent);
            this.SendAsync('message:' + message);
        }
        catch (exp) {
            throw exp;
        }
    }
    SendAsync(message) {
        // Implementation for sending messages
    }
    ShowSchProgress() {
        // Implementation for showing schedule progress
    }
    GetResourceList() {
        console.log('取生成了资源工作日历的资源列表');
        let lsResAddWhere = " and cStatus <> '3' and (cResourceNo in (select distinct cResourceNo ";
        let lstg_Sql = `EXECUTE P_GetSchDataResource '${this.schData.cVersionNo}', '${this.schData.dtStart.addDays(-20)}', '${this.schData.dtEnd}', '${SchParam.cSchType}', '${SchParam.cTechSchType}', '${this.schData.cCalculateNo}'`;
        this.schData.dtResource = SqlPro.GetDataTable(lstg_Sql, null);
        let lsSql = "SELECT FProChaTypeID,FResChaTypeName,cParentClsNo,bEnd,cBarCode,cNote,cMacNo FROM t_ProChaType where isnull(bEnd,1) = '1'";
        this.schData.dtProChaType = SqlPro.GetDataTable(lsSql, null);
        console.log('取工艺特征类型');
        lsSql = `SELECT FResChaValueID, FResChaValueNo, FResChaValueName, FProChaTypeID,isnull(FResChaCycleValue,0) as FResChaCycleValue, isnull(FResChaRePlaceTime,0) as FResChaRePlaceTime, FResChaMemo,isnull(FUseFixedPlaceTime,'1') as FUseFixedPlaceTime, 
                    FSchSN, isnull(FUseChaCycleValue,'0') as FUseChaCycleValue, cDefine1, cDefine2, cDefine3, cDefine4, cDefine5, cDefine6, cDefine7, cDefine8, cDefine9, cDefine10, isnull(cDefine11,0) as cDefine11, isnull(cDefine12,0) as cDefine12, isnull(cDefine13,0) as cDefine13, 
                    isnull(cDefine14,0) as cDefine14,isnull(cDefine15,'1900-01-01') cDefine15,isnull(cDefine16,'1900-01-01')  cDefine16
                FROM dbo.t_ResChaValue with (nolock) where 1 = 1 `;
        this.schData.dtResChaValue = SqlPro.GetDataTable(lsSql, null);
        console.log('取工艺特征值');
        lsSql =
            'SELECT FProChaTypeID,FResChaValue1ID,FResChaValue2ID,FResChaExcTime FROM dbo.t_ResChaCrossTime with (nolock) where 1 = 1';
        this.schData.dtResChaCrossTime = SqlPro.GetDataTable(lsSql, null);
        console.log('取工艺特征转换时间');
        console.log('取排程参数');
        lstg_Sql = '';
        lstg_Sql = `Update t_SchProduct set dEarLiestSchDate = '${this.schData.dtStart}' where dEarLiestSchDate < '${this.schData.dtStart}' and isnull(cWoNo,'') = ''`;
        SqlPro.ExecuteNonQuery(lstg_Sql, null);
        console.log('更新未排订单排程可开始时间');
        try {
            console.log('schData.dtStart - schData.dtEnd' +
                this.schData.dtStart.addDays(-20) +
                this.schData.dtEnd);
            lstg_Sql = `EXECUTE P_GetResWorkTime '${this.schData.cVersionNo}','${this.schData.dtStart.addDays(-20)}','${this.schData.dtEnd}','${SchParam.cSchType}','${SchParam.cTechSchType}','${this.schData.cCalculateNo}'`;
            let dt_ResourceTime = SqlPro.GetDataTable(lstg_Sql, null);
            this.schData.dt_ResourceTime = dt_ResourceTime;
            this.schData.dt_ResourceTime.DefaultView.Sort =
                'iPeriodTimeID asc,dPeriodDay asc';
            this.schData.iProgress = 13;
            let message = '2.1、资源工作日历生成完成';
            let schPrecent = this.schData.iProgress;
            this.dlShowProcess(schPrecent.toString(), message);
            console.log('调用过程P_GetResWorkTime生成所有资源的工作日历');
            lstg_Sql = `SELECT * FROM t_SchResWorkTime with (nolock) WHERE cType = '1' and cSourceType = '2' AND cVersionNo = '${this.schData.cVersionNo}'`;
            let dt_ResourceSpecTime = SqlPro.GetDataTable(lstg_Sql, null);
            this.schData.dt_ResourceSpecTime = dt_ResourceSpecTime;
            this.schData.dt_ResourceSpecTime.DefaultView.Sort = 'dPeriodDay asc';
            let cWorkCenter;
            let lobj_WorkCenter;
            console.log('3、生成工作中心');
            for (let drWorkCenter of this.schData.dtWorkCenter.Rows) {
                cWorkCenter = drWorkCenter['cWcNo'].toString();
                lobj_WorkCenter = new Algorithm.WorkCenter(cWorkCenter, this.schData);
                this.schData.WorkCenterList.push(lobj_WorkCenter);
            }
            let cResourceNoOld = '';
            let cResourceNo;
            let lobj_Resource;
            let lResTimeRange;
            let ResTimeRangeType;
            console.log('开始按资源循环，生成资源对象，同时生成资源时间段');
            for (let drResource of this.schData.dtResource.Rows) {
                cResourceNo = drResource['cResourceNo'].toString();
                if (cResourceNo === 'gys20039') {
                    let m = 1;
                }
                lobj_Resource = new Resource(cResourceNo, this.schData);
                this.schData.ResourceList.push(lobj_Resource);
                if (lobj_Resource.bTeamResource !== '1') {
                    let drResTime = dt_ResourceTime.Select(`cResourceNo = '${cResourceNo}'`);
                    for (let drResTimeRange of drResTime) {
                        lResTimeRange = new ResTimeRange();
                        lResTimeRange.CResourceNo = cResourceNo;
                        lResTimeRange.resource = lobj_Resource;
                        lResTimeRange.CIsInfinityAbility = drResource['cIsInfinityAbility'].toString();
                        lResTimeRange.iPeriodID = drResTimeRange['iPeriodTimeID'];
                        lResTimeRange.DBegTime = drResTimeRange['dPeriodBegTime'];
                        lResTimeRange.DEndTime = drResTimeRange['dPeriodEndTime'];
                        lResTimeRange.dPeriodDay = drResTimeRange['dPeriodDay'];
                        lResTimeRange.FShiftType = drResTimeRange['FShiftType'].toString();
                        if (lResTimeRange.FShiftType === '') {
                            lResTimeRange.FShiftType = 'A班'; //班次 A班 夜班 中班等
                        }
                        lResTimeRange.Attribute = (parseInt(drResTimeRange['cTimeRangeType'].toString()));
                        lResTimeRange.GetNoWorkTaskTimeRange(lResTimeRange.DBegTime, lResTimeRange.DEndTime, true);
                        lobj_Resource.ResTimeRangeList.push(lResTimeRange);
                    }
                    let drResSpecTime = dt_ResourceSpecTime.Select(`cResourceNo = '${cResourceNo}'`);
                    for (let drResTimeRange of drResSpecTime) {
                        lResTimeRange = new ResTimeRange();
                        lResTimeRange.CResourceNo = cResourceNo;
                        lResTimeRange.resource = lobj_Resource;
                        lResTimeRange.iPeriodID = parseInt(drResTimeRange['iPeriodTimeID'].toString());
                        lResTimeRange.CIsInfinityAbility = drResource['cIsInfinityAbility'].toString();
                        lResTimeRange.DBegTime = drResTimeRange['dPeriodBegTime'];
                        lResTimeRange.DEndTime = drResTimeRange['dPeriodEndTime'];
                        lResTimeRange.Attribute = (parseInt(drResTimeRange['cTimeRangeType'].toString()));
                        lResTimeRange.HoldingTime =
                            parseInt(parseFloat(drResTimeRange['iHoldingTime'].toString()).toString()) * 60;
                        lobj_Resource.ResSpecTimeRangeList.push(lResTimeRange);
                    }
                    if (drResSpecTime.length > 0) {
                        lobj_Resource.MergeTimeRange();
                    }
                    lobj_Resource.getResSourceDayCapList();
                }
            }
            this.schData.KeyResourceList = this.schData.ResourceList.filter((p1) => p1.cIsKey === '1' && p1.iKeySchSN > 0);
            this.schData.KeyResourceList.sort((p1, p2) => p1.iKeySchSN - p2.iKeySchSN);
            this.schData.TeamResourceList = this.schData.ResourceList.filter((p1) => p1.bTeamResource === '1');
            if (this.schData.TeamResourceList.length > 0) {
                for (let TeamResource of this.schData.TeamResourceList) {
                    let ResourceSubList = this.schData.ResourceList.filter((p1) => p1.cTeamResourceNo === TeamResource.cResourceNo);
                    TeamResource.TeamResourceList = ResourceSubList;
                    for (let TeamResourceSub of ResourceSubList) {
                        TeamResourceSub.TeamResource = TeamResource;
                    }
                }
            }
        }
        catch (ex1) {
            console.log('排产计算出错！位置GetResourceList(),出错内容：' + ex1.toString());
            throw ex1;
            return -1;
        }
        return 1;
    }
    async getSchData() {
        console.log('开始第二步骤');
        // 2.0 SQL准备
        let cSchWo = this.getParamValue('cSchWo');
        console.log('cSchWo取值完毕');
        let lsSchProductRouteItem = `
            SELECT ISNULL(iSchBatch, 1) as iSchBatch, cVersionNo, iSchSdID, iProcessProductID, iInterID, iEntryID, cWoNo, cInvCode, cInvCodeFull,
            ISNULL(iBomLevel, 0) as iBomLevel, cLevelInfo, cLevelPath, cSubInvCode, cSubInvCodeFull, iSeqID, cUtterType, cSubRelate,
            ISNULL(iQtyPer, 0) as iQtyPer, ISNULL(iParentQty, 0) as iParentQty, ISNULL(iSubQty, 0) as iSubQty, ISNULL(iScrapt, 0) as iScrapt,
            iNormalQty, iRetPercent, iReqQty, dReqDate, iProQty, iScrapQty, ISNULL(iNormalScrapQty, 0) as iNormalScrapQty,
            ISNULL(iKeepQty, 0) as iKeepQty, ISNULL(iPlanQty, 0) as iPlanQty, cWhNo, cPacNo, iRetOffsetLt, cNote,
            ISNULL(cGetItemType, '0') as cGetItemType, ISNULL(bself, '0') as bself, ISNULL(dForeInDate, GETDATE()) as dForeInDate
            FROM dbo.t_SchProductRouteItem WITH (NOLOCK)
            WHERE 1 = 1 AND iSchSdID IN (SELECT iSchSdID FROM t_SchProduct WHERE cVersionNo = '${schData.cVersionNo}' AND ISNULL(cSelected, '0') = '1' AND ISNULL(iSchQty, 0) > 0 AND ISNULL(cWoNo, '') = '')
            AND cVersionNo = '${schData.cVersionNo}'
        `;
        if (cSchWo == '1') {
            lsSchProductRouteItem += `
                UNION
                SELECT ISNULL(iSchBatch, 6) as iSchBatch, cVersionNo, iSchSdID, iProcessProductID, iInterID, iEntryID, cWoNo, cInvCode, cInvCodeFull,
                ISNULL(iBomLevel, 0) as iBomLevel, cLevelInfo, cLevelPath, cSubInvCode, cSubInvCodeFull, iSeqID, cUtterType, cSubRelate,
                ISNULL(iQtyPer, 0) as iQtyPer, ISNULL(iParentQty, 0) as iParentQty, ISNULL(iSubQty, 0) as iSubQty, ISNULL(iScrapt, 0) as iScrapt,
                iNormalQty, iRetPercent, iReqQty, dReqDate, iProQty, iScrapQty, ISNULL(iNormalScrapQty, 0) as iNormalScrapQty,
                ISNULL(iKeepQty, 0) as iKeepQty, ISNULL(iPlanQty, 0) as iPlanQty, cWhNo, cPacNo, iRetOffsetLt, cNote,
                ISNULL(cGetItemType, '0') as cGetItemType, ISNULL(bself, '0') as bself, ISNULL(dForeInDate, GETDATE()) as dForeInDate
                FROM dbo.t_SchProductRouteItem WITH (NOLOCK)
                WHERE 1 = 1 AND iSchSdID IN (SELECT t_SchProduct.iSchSdID FROM t_SchProduct INNER JOIN (SELECT cWoNo FROM t_WorkOrder WHERE cStatus IN ('I', 'A', 'G')) b ON (t_SchProduct.cWoNo = b.cWoNo) WHERE cVersionNo = 'SureVersion')
                AND cVersionNo = 'SureVersion'
                AND iSchSdID IN (SELECT iSchSdID FROM t_SchProduct WHERE cVersionNo = 'SureVersion' AND ISNULL(cSelected, '0') = '1')
                ORDER BY cVersionNo, iSchSdID, iProcessProductID, cInvCodeFull, cSubInvCodeFull
            `;
        }
        SchParam.cSelfEndDate = this.getParamValue('cSelfEndDate');
        let lsSchProductRouteResTime = `
            SELECT ISNULL(iSchBatch, 6) as iSchBatch, cVersionNo, iSchSdID, iProcessProductID, ISNULL(iInterID, 0) as iInterID, ISNULL(iWoProcessID, 0) iWoProcessID,
            ISNULL(iResProcessID, 0) as iResProcessID, ISNULL(cWoNo, '') cWoNo, ISNULL(iResourceID, 0) as iResourceID, cResourceNo, cResourceName, iTimeID,
            dResBegDate, dResEndDate, iResReqQty, ISNULL(iResRationHour, 0) as iResRationHour, ISNULL(cSimulateVer, '') as cSimulateVer,
            ISNULL(cNote, '') as cNote, ISNULL(cTaskType, '1') as cTaskType
            FROM t_SchProductRouteResTime WITH (NOLOCK)
            WHERE 1 = 1 AND iSchSdID IN (SELECT iSchSdID FROM t_SchProduct WHERE cVersionNo = '${schData.cVersionNo}' AND ISNULL(cSelected, '0') = '1' AND ISNULL(iSchQty, 0) > 0 AND ISNULL(cWoNo, '') = '')
            AND cVersionNo = '${schData.cVersionNo}'
            ORDER BY cVersionNo, iSchSdID, iProcessProductID, cResourceNo, iTimeID
        `;
        let lsItem = `
            SELECT a.iItemID, a.cInvCode, a.cInvName, a.cInvStd, a.cItemClsNo, ISNULL(a.cVenCode, '') as cVenCode, ISNULL(a.bSale, '0') as bSale,
            ISNULL(a.bPurchase, '0') as bPurchase, ISNULL(a.bSelf, '1') as bSelf, ISNULL(a.bProxyForeign, '0') as bProxyForeign,
            ISNULL(a.cComUnitCode, '') as cComUnitCode, ISNULL(a.cWcNo, '') as cWcNo, ISNULL(a.iProSec, '') as iProSec, ISNULL(a.iPriority, '') as iPriority,
            ISNULL(a.iSafeStock, '') as iSafeStock, ISNULL(a.iTopLot, '') as iTopLot, ISNULL(a.iLowLot, '') as iLowLot, ISNULL(a.iIncLot, '') as iIncLot,
            ISNULL(a.iAvgLot, '') as iAvgLot, ISNULL(a.cLeadTimeType, '') as cLeadTimeType, ISNULL(a.iAvgLeadTime, '') as iAvgLeadTime,
            ISNULL(a.iAdvanceDate, '') as iAdvanceDate, ISNULL(a.cRouteCode, '') as cRouteCode, ISNULL(a.cPlanMode, '') as cPlanMode,
            ISNULL(a.cWorkRouteType, '') as cWorkRouteType, ISNULL(a.cTechNo, '') as cTechNo, ISNULL(a.cKeyResourceNo, '') as cKeyResourceNo,
            ISNULL(a.cInjectItemType, '') as cInjectItemType, ISNULL(a.cMoldNo, '') as cMoldNo, ISNULL(a.cSubMoldNo, '') as cSubMoldNo,
            ISNULL(a.cMoldPosition, '') as cMoldPosition, ISNULL(a.iMoldSubQty, 0) as iMoldSubQty, ISNULL(a.iMoldCount, 0) as iMoldCount,
            ISNULL(a.cMaterial, '') as cMaterial, ISNULL(a.cColor, '') as cColor, ISNULL(a.fVolume, 0) as fVolume, ISNULL(a.flength, 0) as flength,
            ISNULL(a.fWidth, 0) as fWidth, ISNULL(a.fHeight, 0) as fHeight, ISNULL(a.fNetWeight, 0) as fNetWeight, ISNULL(a.iItemDifficulty, 1) as iItemDifficulty,
            ISNULL(a.cSize1, '') as cSize1, ISNULL(a.cSize2, '') as cSize2, ISNULL(a.cSize3, '') as cSize3, ISNULL(a.cSize4, '') as cSize4,
            ISNULL(a.cSize5, '') as cSize5, ISNULL(a.cSize6, '') as cSize6, ISNULL(a.cSize7, '') as cSize7, ISNULL(a.cSize8, '') as cSize8,
            ISNULL(a.cSize9, '') as cSize9, ISNULL(a.cSize10, '') as cSize10, ISNULL(a.cSize11, 0) as cSize11, ISNULL(a.cSize12, 0) as cSize12,
            ISNULL(a.cSize13, 0) as cSize13, ISNULL(a.cSize14, 0) as cSize14, ISNULL(a.cSize15, '') as cSize15, ISNULL(a.cSize16, '') as cSize16
            FROM t_Item a WITH (NOLOCK)
            WHERE a.cInvCode IN (SELECT DISTINCT cWorkItemNo FROM t_SchProductRoute)
        `;
        let lsWorkCenter = 'SELECT * FROM t_WorkCenter';
        let lsDepartment = 'SELECT * FROM t_Department';
        let lsPerson = `
            SELECT a.cPsn_Num, a.cPsn_Name, a.cDepCode, a.iRecordID, a.rPersonType, a.rSex, a.dBirthDate, a.rNativePlace, a.rNational, a.rhealthStatus,
            a.rMarriStatus, a.vIDNo, a.MPicture, a.rPerResidence, a.vAliaName, a.dJoinworkDate, a.dEnterDate, a.dRegularDate, a.vSSNo, a.rworkAttend,
            a.vCardNo, a.rtbmRule, a.rCheckInFlag, a.dLastDate, a.hrts, a.vstatus1, a.nstatus2, a.bPsnPerson, a.cPsnMobilePhone, a.cPsnFPhone,
            a.cPsnOPhone, a.cPsnInPhone, a.cPsnEmail, a.cPsnPostAddr, a.cPsnPostCode, a.cPsnFAddr, a.cPsnQQCode, a.cPsnURL, a.CpsnOSeat, a.dEnterUnitDate,
            a.cPsnProperty, a.cPsnBankCode, a.cPsnAccount, a.pk_hr_hi_person, a.bProbation, a.cDutyclass, a.bTakeTM, a.MPictureqpb, a.rIDType, a.rCountry,
            a.dLeaveDate, a.rFigure, a.rWorkStatus, a.EmploymentForm, a.rPersonParameters, a.bDutyLock, a.bpsnshop, a.cPosition, a.cEnglishName,
            a.cEducation, a.cReservefundNo, a.fCreditQuantity, a.iCreDate, a.cCreGrade, a.iLowRate, a.cOfferGrade, a.iOfferRate, a.dPValidDate,
            a.dPInValidDate, a.cPsnDefine1, a.cPsnDefine2, a.cPsnDefine3, a.cPsnDefine4, a.cPsnDefine5, a.cPsnDefine6, a.cPsnDefine7, a.cPsnDefine8,
            a.cPsnDefine9, a.cPsnDefine10, a.cPsnDefine11, a.cPsnDefine12, a.cPsnDefine13, a.cPsnDefine14, a.cPsnDefine15, a.cPsnDefine16, a.cMemberShip,
            a.cClasses, a.blacklist, a.blacklistNote, a.cBusDepCode
            FROM t_Person a WITH (NOLOCK)
            INNER JOIN t_team b WITH (NOLOCK) ON (a.cDutyclass = b.cTeamNo)
        `;
        let lsteam = 'SELECT * FROM t_team';
        let lsTechInfo = `
            SELECT iInterID, cTechNo, cTechName, ISNULL(cResClsNo, '') as cResClsNo, ISNULL(cWcNo, '') as cWcNo, ISNULL(cDeptNo, '') as cDeptNo,
            ISNULL(cResourceNo, '') as cResourceNo, ISNULL(cTechReq, '') as cTechReq, ISNULL(cNote, '') as cNote, ISNULL(cTechDefine1, '') as cTechDefine1,
            ISNULL(cTechDefine2, '') as cTechDefine2, ISNULL(cTechDefine3, '') as cTechDefine3, ISNULL(cTechDefine4, '') as cTechDefine4,
            ISNULL(cTechDefine5, '') as cTechDefine5, ISNULL(cTechDefine6, '') as cTechDefine6, ISNULL(cTechDefine7, '') as cTechDefine7,
            ISNULL(cTechDefine8, '') as cTechDefine8, ISNULL(cTechDefine9, '') as cTechDefine9, ISNULL(cTechDefine10, '') as cTechDefine10,
            ISNULL(cTechDefine11, 0) as cTechDefine11, ISNULL(cTechDefine12, 0) as cTechDefine12, ISNULL(cTechDefine13, 0) as cTechDefine13,
            ISNULL(cTechDefine14, 0) as cTechDefine14, ISNULL(cTechDefine15, '') as cTechDefine15, ISNULL(cTechDefine16, '') as cTechDefine16,
            ISNULL(cFormula, '') as cFormula, ISNULL(cFormula2, '') as cFormula2, ISNULL(iSeqPretime, 0) as iSeqPretime, ISNULL(iSeqPostTime, 0) as iSeqPostTime,
            ISNULL(cAttributeValue1, '') as cAttributeValue1, ISNULL(cAttributeValue2, '') as cAttributeValue2, ISNULL(cAttributeValue3, '') as cAttributeValue3,
            ISNULL(cAttributeValue4, '') as cAttributeValue4, ISNULL(cAttributeValue5, '') as cAttributeValue5, ISNULL(cAttributeValue6, '') as cAttributeValue6,
            ISNULL(cAttributeValue7, '') as cAttributeValue7, ISNULL(cAttributeValue8, '') as cAttributeValue8, ISNULL(cAttributeValue9, '') as cAttributeValue9,
            ISNULL(cAttributeValue10, '') as cAttributeValue10, ISNULL(iTechValue, 0) as iTechValue, ISNULL(iOrder, 0) as iOrder,
            ISNULL(iTechDifficulty, 1) as iTechDifficulty, ISNULL(iSeqPretime, 0) as iSeqPretime, ISNULL(iSeqPostTime, 0) as iSeqPostTime
            FROM dbo.t_TechInfo WITH (NOLOCK)
        `;
        let lsTechLearnCurves = `
            SELECT iInterID, cLearnCurvesNo, cLearnCurvesName, cTechNo, ISNULL(iDayDis1, 0) as iDayDis1, ISNULL(iDayDis2, 0) as iDayDis2,
            ISNULL(iDayDis3, 0) as iDayDis3, ISNULL(iDayDis4, 0) as iDayDis4, ISNULL(iDayDis5, 0) as iDayDis5, ISNULL(iDayDis6, 0) as iDayDis6,
            ISNULL(iDayDis7, 0) as iDayDis7, ISNULL(iDayDis8, 0) as iDayDis8, ISNULL(iDayDis9, 0) as iDayDis9, ISNULL(iDayDis10, 0) as iDayDis10,
            ISNULL(iDayDis11, 0) as iDayDis11, ISNULL(iDayDis12, 0) as iDayDis12, ISNULL(iDayDis13, 0) as iDayDis13, ISNULL(iDayDis14, 0) as iDayDis14,
            ISNULL(iDayDis15, 0) as iDayDis15, ISNULL(iDayDis16, 0) as iDayDis16, ISNULL(iDayDis17, 0) as iDayDis17, ISNULL(iDayDis18, 0) as iDayDis18,
            ISNULL(iDayDis19, 0) as iDayDis19, ISNULL(iDayDis20, 0) as iDayDis20, ISNULL(iDayDis21, 0) as iDayDis21, ISNULL(iDayDis22, 0) as iDayDis22,
            ISNULL(iDayDis23, 0) as iDayDis23, ISNULL(iDayDis24, 0) as iDayDis24, ISNULL(iDayDis25, 0) as iDayDis25, ISNULL(iDayDis26, 0) as iDayDis26,
            ISNULL(iDayDis27, 0) as iDayDis27, ISNULL(iDayDis28, 0) as iDayDis28, ISNULL(iDayDis29, 0) as iDayDis29, ISNULL(iDayDis30, 0) as iDayDis30,
            ISNULL(iDayDis31, 0) as iDayDis31, ISNULL(iDiffCoe, 0) as iDiffCoe, ISNULL(iCapacity, 0) as iCapacity, ISNULL(iResPreTime, 0) as iResPreTime,
            cNote, cDefine22, cDefine23, cDefine24, cDefine25, cDefine26
            FROM dbo.t_TechLearnCurves WITH (NOLOCK)
        `;
        let lsResTechScheduSN = 'SELECT * FROM t_ResTechScheduSN';
        console.log('sql准备完毕');
        try {
            console.log('2.10 t_TechInfo');
            schData.dtTechInfo = await this.getSqlDataTable(lsTechInfo);
            let cTechNo;
            let lobj_TechInfo;
            schData.dtTechInfo.forEach((drTechInfo) => {
                cTechNo = drTechInfo.cTechNo;
                lobj_TechInfo = new TechInfo(cTechNo, this.schData);
                this.schData.TechInfoList.push(lobj_TechInfo);
            });
            console.log('2.0 t_Item 已确认的生产任务单取SureVersion版本数据');
            schData.dtItem = await this.getSqlDataTable(lsItem);
            let cInvCode;
            let lobj_Item;
            schData.dtItem.forEach((drItem) => {
                cInvCode = drItem.cInvCode;
                lobj_Item = new Item(cInvCode, this.schData);
                this.schData.ItemList.push(lobj_Item);
            });
            let lstg_Sql = '';
            console.log('2.1 填充SchProductList');
            lstg_Sql = `EXECUTE P_GetSchProductWorkItem '${schData.cVersionNo}', '${schData.dtStart.addDays(-20)}', '${schData.dtEnd}', '${SchParam.cSchType}', '${SchParam.cTechSchType}', '${this.schData.cCalculateNo}'`;
            schData.dtSchProductWorkItem = await this.getSqlDataTable(lstg_Sql);
            schData.dtSchProductWorkItem.forEach((dr) => {
                let lSchProductWorkItem = new SchProductWorkItem();
                lSchProductWorkItem.iSchSdID = dr.iSchSdID;
                lSchProductWorkItem.iBomAutoID = dr.iBomAutoID;
                lSchProductWorkItem.cVersionNo = dr.cVersionNo;
                lSchProductWorkItem.iInterID = dr.iInterID;
                lSchProductWorkItem.iSdLineID = dr.iSdLineID;
                lSchProductWorkItem.iSeqID = dr.iSeqID;
                lSchProductWorkItem.cCustNo = dr.cCustNo;
                lSchProductWorkItem.cPriorityType = dr.cPriorityType;
                lSchProductWorkItem.cStatus = dr.cStatus;
                lSchProductWorkItem.iItemID = -1;
                lSchProductWorkItem.cInvCode = dr.cInvCode.trim();
                lSchProductWorkItem.cInvName = dr.cInvName;
                lSchProductWorkItem.cInvStd = dr.cInvStd;
                lSchProductWorkItem.iReqQty = dr.iReqQty;
                lSchProductWorkItem.dBegDate = dr.dBegDate;
                lSchProductWorkItem.dEndDate = dr.dEndDate;
                lSchProductWorkItem.dCanBegDate = dr.dCanBegDate ?? new Date();
                lSchProductWorkItem.dCanEndDate = dr.dCanEndDate ?? dr.dEndDate;
                lSchProductWorkItem.cMiNo = dr.cMiNo;
                lSchProductWorkItem.iPriority = dr.iPriority;
                lSchProductWorkItem.cWoNo = dr.cWoNo;
                lSchProductWorkItem.cColor = dr.cColor;
                lSchProductWorkItem.cNote = dr.cNote;
                lSchProductWorkItem.cType = dr.cType;
                lSchProductWorkItem.cSchType = dr.cSchType;
                lSchProductWorkItem.iSchPriority = dr.iPriority;
                lSchProductWorkItem.iSchBatch = dr.iSchBatch;
                lSchProductWorkItem.iWorkQtyPd = dr.iWorkQtyPd;
                lSchProductWorkItem.cBatchNo = dr.cBatchNo;
                lSchProductWorkItem.iSchSN = dr.iSchSN;
                lSchProductWorkItem.cWorkRouteType = dr.cWorkRouteType;
                lSchProductWorkItem.cSchSNType = dr.cSchSNType;
                lSchProductWorkItem.cAttributes1 = dr.cAttributes1;
                lSchProductWorkItem.cAttributes2 = dr.cAttributes2;
                lSchProductWorkItem.cAttributes3 = dr.cAttributes3;
                lSchProductWorkItem.cAttributes4 = dr.cAttributes4;
                lSchProductWorkItem.cAttributes5 = dr.cAttributes5;
                lSchProductWorkItem.cAttributes6 = dr.cAttributes6;
                lSchProductWorkItem.cAttributes7 = dr.cAttributes7;
                lSchProductWorkItem.cAttributes8 = dr.cAttributes8;
                lSchProductWorkItem.cAttributes9 = dr.cAttributes9;
                lSchProductWorkItem.cAttributes10 = dr.cAttributes10;
                lSchProductWorkItem.cAttributes11 = dr.cAttributes11;
                lSchProductWorkItem.cAttributes12 = dr.cAttributes12;
                lSchProductWorkItem.cAttributes13 = dr.cAttributes13;
                lSchProductWorkItem.cAttributes14 = dr.cAttributes14;
                lSchProductWorkItem.cAttributes15 = dr.cAttributes15;
                lSchProductWorkItem.cAttributes16 = dr.cAttributes16;
                lSchProductWorkItem.schData = this.schData;
                schData.SchProductWorkItemList.push(lSchProductWorkItem);
            });
            lstg_Sql = `EXECUTE P_GetSchDataProduct '${schData.cVersionNo}', '${schData.dtStart.addDays(-20)}', '${schData.dtEnd}', '${SchParam.cSchType}', '${SchParam.cTechSchType}', '${this.schData.cCalculateNo}'`;
            schData.dtSchProduct = await this.getSqlDataTable(lstg_Sql);
            schData.dtSchProduct.forEach((dr) => {
                let lSchProduct = new SchProduct();
                lSchProduct.iSchSdID = dr.iSchSdID;
                lSchProduct.cVersionNo = dr.cVersionNo;
                lSchProduct.iInterID = dr.iInterID;
                lSchProduct.iSdLineID = dr.iSdLineID;
                lSchProduct.iSeqID = dr.iSeqID;
                lSchProduct.iModelID = dr.iModelID;
                lSchProduct.cCustNo = dr.cCustNo;
                lSchProduct.cCustName = dr.cCustName;
                lSchProduct.cSTCode = dr.cSTCode;
                lSchProduct.cBusType = dr.cBusType;
                lSchProduct.cPriorityType = dr.cPriorityType;
                lSchProduct.cStatus = dr.cStatus;
                lSchProduct.cRequireType = dr.cRequireType;
                lSchProduct.iItemID = -1;
                lSchProduct.cInvCode = dr.cInvCode.trim();
                lSchProduct.cInvName = dr.cInvName;
                lSchProduct.cInvStd = dr.cInvStd;
                lSchProduct.cUnitCode = dr.cUnitCode;
                lSchProduct.iReqQty = dr.iReqQty;
                lSchProduct.dRequireDate = dr.dRequireDate;
                lSchProduct.dDeliveryDate = dr.dDeliveryDate;
                lSchProduct.dEarliestSchDate = dr.dEarliestSchDate ?? new Date();
                lSchProduct.cSchStatus = dr.cSchStatus;
                lSchProduct.cMiNo = dr.cMiNo;
                lSchProduct.iPriority = dr.iPriority;
                lSchProduct.cSelected = dr.cSelected;
                lSchProduct.cWoNo = dr.cWoNo;
                lSchProduct.iPlanQty = dr.iPlanQty;
                lSchProduct.cNeedSet = dr.cNeedSet;
                lSchProduct.iFHQuantity = dr.iFHQuantity;
                lSchProduct.iKPQuantity = dr.iKPQuantity;
                lSchProduct.iSourceLineID = dr.iSourceLineID;
                lSchProduct.cColor = dr.cColor;
                lSchProduct.cNote = dr.cNote;
                lSchProduct.bSet = dr.bSet;
                lSchProduct.cType = dr.cType;
                lSchProduct.cSchType = dr.cSchType;
                lSchProduct.iSchPriority = dr.iPriority;
                lSchProduct.iSchBatch = dr.iSchBatch;
                lSchProduct.iDeliveryDays = dr.iDeliveryDays;
                lSchProduct.cScheduled = dr.cScheduled;
                lSchProduct.iWorkQtyPd = dr.iWorkQtyPd;
                lSchProduct.cBatchNo = dr.cBatchNo;
                lSchProduct.iSchSN = dr.iSchSN;
                lSchProduct.cGroupSN = dr.cGroupSN;
                lSchProduct.cGroupQty = dr.cGroupQty;
                lSchProduct.cCustomize = dr.cCustomize;
                lSchProduct.cWorkRouteType = dr.cWorkRouteType;
                lSchProduct.cSchSNType = dr.cSchSNType;
                lSchProduct.cAttributes1 = dr.cAttributes1;
                lSchProduct.cAttributes2 = dr.cAttributes2;
                lSchProduct.cAttributes3 = dr.cAttributes3;
                lSchProduct.cAttributes4 = dr.cAttributes4;
                lSchProduct.cAttributes5 = dr.cAttributes5;
                lSchProduct.cAttributes6 = dr.cAttributes6;
                lSchProduct.cAttributes7 = dr.cAttributes7;
                lSchProduct.cAttributes8 = dr.cAttributes8;
                lSchProduct.cAttributes9 = dr.cAttributes9;
                lSchProduct.cAttributes10 = dr.cAttributes10;
                lSchProduct.cAttributes11 = dr.cAttributes11;
                lSchProduct.cAttributes12 = dr.cAttributes12;
                lSchProduct.cAttributes13 = dr.cAttributes13;
                lSchProduct.cAttributes14 = dr.cAttributes14;
                lSchProduct.cAttributes15 = dr.cAttributes15;
                lSchProduct.cAttributes16 = dr.cAttributes16;
                lSchProduct.schData = this.schData;
                schData.SchProductList.push(lSchProduct);
            });
            console.log('2.2 填充SchProductRouteList 改用存储过程取数，提供速度');
            lstg_Sql = `EXECUTE P_GetSchDataProductRoute '${schData.cVersionNo}', '${schData.dtStart.addDays(-20)}', '${schData.dtEnd}', '${SchParam.cSchType}', '${SchParam.cTechSchType}', '${this.schData.cCalculateNo}'`;
            schData.dtSchProductRoute = await this.getSqlDataTable(lstg_Sql);
            schData.dtSchProductRoute.forEach((dr) => {
                let lSchProductRoute = new SchProductRoute();
                lSchProductRoute.iSchSdID = dr.iSchSdID;
                lSchProductRoute.cVersionNo = dr.cVersionNo;
                lSchProductRoute.iModelID = dr.iModelID;
                lSchProductRoute.iProcessProductID = dr.iProcessProductID;
                lSchProductRoute.cWoNo = dr.cWoNo;
                lSchProductRoute.iInterID = dr.iInterID;
                lSchProductRoute.iWoProcessID = dr.iWoProcessID;
                lSchProductRoute.iItemID = -1;
                lSchProductRoute.cInvCode = dr.cInvCode.trim();
                lSchProductRoute.iWorkItemID = -1;
                lSchProductRoute.cWorkItemNo = dr.cWorkItemNo;
                lSchProductRoute.item = schData.ItemList.find((p) => p.cInvCode == lSchProductRoute.cWorkItemNo);
                lSchProductRoute.iProcessID = dr.iProcessID;
                lSchProductRoute.iWoSeqID = dr.iWoSeqID;
                lSchProductRoute.cTechNo = dr.cTechNo;
                lSchProductRoute.techInfo = schData.TechInfoList.find((p) => p.cTechNo == lSchProductRoute.cTechNo);
                lSchProductRoute.cSeqNote = dr.cSeqNote;
                lSchProductRoute.cWcNo = dr.cWcNo;
                lSchProductRoute.iNextSeqID = dr.iNextSeqID;
                lSchProductRoute.cPreProcessID = dr.cPreProcessID;
                lSchProductRoute.cPostProcessID = dr.cPostProcessID;
                lSchProductRoute.cPreProcessItem = dr.cPreProcessItem;
                lSchProductRoute.cPostProcessItem = dr.cPostProcessItem;
                lSchProductRoute.iAutoID = dr.iAutoID;
                lSchProductRoute.cLevelInfo = dr.cLevelInfo;
                lSchProductRoute.iLevel = dr.iLevel;
                lSchProductRoute.iParentItemID = dr.iParentItemID;
                lSchProductRoute.cParentItemNo = dr.cParentItemNo;
                lSchProductRoute.cCompSeq = dr.cCompSeq;
                lSchProductRoute.cMoveType = dr.cMoveType;
                lSchProductRoute.iMoveInterTime = dr.iMoveInterTime;
                lSchProductRoute.iMoveInterQty = dr.iMoveInterQty;
                lSchProductRoute.iSeqPreTime = dr.iSeqPreTime;
                lSchProductRoute.iSeqPostTime = dr.iSeqPostTime;
                lSchProductRoute.iLaborTime = dr.iLaborTime;
                lSchProductRoute.iLeadTime = dr.iLeadTime;
                lSchProductRoute.cStatus = dr.cStatus;
                lSchProductRoute.iPriority = dr.iPriority;
                lSchProductRoute.iReqQty = dr.iReqQty;
                lSchProductRoute.iReqQtyOld = dr.iReqQtyOld;
                lSchProductRoute.iActQty = dr.iActQty;
                lSchProductRoute.iRealHour = dr.iRealHour;
                lSchProductRoute.dBegDate = dr.dBegDate;
                lSchProductRoute.dEndDate = dr.dEndDate;
                lSchProductRoute.dActBegDate = dr.dActBegDate;
                lSchProductRoute.dActEndDate = dr.dActEndDate;
                lSchProductRoute.cNote = dr.cNote;
                lSchProductRoute.iDevCountPd = parseInt(dr.iDevCountPd.toString());
                lSchProductRoute.cDevCountPdExp = dr.cDevCountPdExp;
                lSchProductRoute.cParellelType = dr.cParellelType;
                lSchProductRoute.cParallelNo = dr.cParallelNo;
                lSchProductRoute.cKeyBrantch = dr.cKeyBrantch;
                lSchProductRoute.iCapacity = parseFloat(dr.iCapacity.toString());
                lSchProductRoute.cCapacityExp = dr.cCapacityExp;
                lSchProductRoute.iAdvanceDate = parseFloat(dr.iAdvanceDate.toString());
                lSchProductRoute.iSchBatch = dr.iSchBatch;
                lSchProductRoute.schData = this.schData;
                schData.SchProductRouteList.push(lSchProductRoute);
            });
            console.log('2.4 填充SchProductRouteResList');
            lstg_Sql = `EXECUTE P_GetSchDataProductRouteRes '${schData.cVersionNo}', '${schData.dtStart.addDays(-20)}', '${schData.dtEnd}', '${SchParam.cSchType}', '${SchParam.cTechSchType}', '${this.schData.cCalculateNo}'`;
            schData.dtSchProductRouteRes = await this.getSqlDataTable(lstg_Sql);
            schData.dtSchProductRouteRes.forEach((dr) => {
                let lSchProductRouteRes = new SchProductRouteRes();
                lSchProductRouteRes.schData = this.schData;
                lSchProductRouteRes.iSchSdID = dr.iSchSdID;
                lSchProductRouteRes.cVersionNo = dr.cVersionNo;
                lSchProductRouteRes.iProcessProductID = dr.iProcessProductID;
                lSchProductRouteRes.iProcessID = dr.iProcessID;
                lSchProductRouteRes.iResProcessID = dr.iResProcessID;
                lSchProductRouteRes.iResourceAbilityID = dr.iResourceAbilityID;
                lSchProductRouteRes.cWoNo = dr.cWoNo;
                lSchProductRouteRes.iItemID = -1;
                lSchProductRouteRes.cInvCode = dr.cInvCode.trim();
                lSchProductRouteRes.iWoSeqID = dr.iWoSeqID;
                lSchProductRouteRes.cTechNo = dr.cTechNo;
                lSchProductRouteRes.cSeqNote = dr.cSeqNote;
                lSchProductRouteRes.iResGroupNo = dr.iResGroupNo;
                lSchProductRouteRes.iResGroupPriority = dr.iResGroupPriority;
                lSchProductRouteRes.cSelected = dr.cSelected;
                lSchProductRouteRes.iResourceID = dr.iResourceID;
                lSchProductRouteRes.cResourceNo = dr.cResourceNo;
                lSchProductRouteRes.cResourceName = dr.cResourceName;
                lSchProductRouteRes.cTeamResourceNo = dr.cTeamResourceNo;
                lSchProductRouteRes.iResReqQty = dr.iResReqQty;
                lSchProductRouteRes.iResReqQtyOld = dr.iResReqQtyOld;
                lSchProductRouteRes.dResBegDate = dr.dResBegDate;
                lSchProductRouteRes.dResEndDate = dr.dResEndDate;
                lSchProductRouteRes.iResRationHour =
                    this.getParamValue('HourMinSecond') == '1'
                        ? dr.iResRationHour * 60
                        : dr.iResRationHour;
                lSchProductRouteRes.iViceResource1ID =
                    dr.iViceResource1ID == null ? -1 : dr.iViceResource1ID;
                lSchProductRouteRes.cViceResource1No = dr.cViceResource1No;
                lSchProductRouteRes.iViceResource2ID =
                    dr.iViceResource2ID == null ? -1 : dr.iViceResource2ID;
                lSchProductRouteRes.cViceResource2No = dr.cViceResource2No;
                lSchProductRouteRes.iViceResource3ID =
                    dr.iViceResource3ID == null ? -1 : dr.iViceResource3ID;
                lSchProductRouteRes.cViceResource3No = dr.cViceResource3No;
                lSchProductRouteRes.cWorkType = dr.cWorkType;
                lSchProductRouteRes.iBatchQty = dr.iBatchQty;
                lSchProductRouteRes.iBatchQtyBase = dr.iBatchQtyBase;
                lSchProductRouteRes.iBatchWorkTime = dr.iBatchWorkTime;
                lSchProductRouteRes.iBatchInterTime = dr.iBatchInterTime;
                lSchProductRouteRes.iResPreTime = dr.iResPreTime;
                lSchProductRouteRes.iResPreTimeOld = dr.iResPreTimeOld;
                lSchProductRouteRes.cResPreTimeExp = dr.cResPreTimeExp;
                lSchProductRouteRes.iPriorityRes = dr.iPriorityRes;
                lSchProductRouteRes.iPriorityResLast = dr.iPriorityResLast;
                let iCapacity = 0;
                if (this.getParamValue('MinOrHour') == '1') {
                    iCapacity = dr.iCapacity * 60;
                }
                else if (this.getParamValue('MinOrHour') == '2') {
                    iCapacity = dr.iCapacity * 3600;
                }
                else {
                    iCapacity = dr.iCapacity;
                }
                lSchProductRouteRes.iCapacity = iCapacity;
                lSchProductRouteRes.cCapacityExp = dr.cCapacityExp;
                lSchProductRouteRes.cIsInfinityAbility = dr.cIsInfinityAbility;
                lSchProductRouteRes.iResPostTime = dr.iResPostTime;
                lSchProductRouteRes.iCycTime = dr.iCycTime;
                lSchProductRouteRes.iProcessPassRate = dr.iProcessPassRate;
                lSchProductRouteRes.iEfficiency = dr.iEfficiency;
                lSchProductRouteRes.iHoursPd = dr.iHoursPd;
                lSchProductRouteRes.iWorkQtyPd = dr.iWorkQtyPd;
                lSchProductRouteRes.iWorkersPd = dr.iWorkersPd;
                lSchProductRouteRes.iDevCountPd = dr.iDevCountPd;
                lSchProductRouteRes.cLearnCurvesNo = dr.cLearnCurvesNo;
                lSchProductRouteRes.iLaborTime =
                    this.getParamValue('HourMinSecond') == '0'
                        ? dr.iLaborTime
                        : dr.iLaborTime * 60;
                lSchProductRouteRes.iLeadTime = dr.iLeadTime;
                lSchProductRouteRes.iActResReqQty = dr.iActResReqQty;
                lSchProductRouteRes.iActResRationHour = dr.iActResRationHour;
                lSchProductRouteRes.dActResBegDate = dr.dActResBegDate;
                lSchProductRouteRes.dActResEndDate = dr.dActResEndDate;
                let cBatch = dr.cDefine24 == '' ? '-1' : dr.cDefine24;
                lSchProductRouteRes.iBatch = parseInt(cBatch);
                lSchProductRouteRes.FResChaValue1ID = dr.FResChaValue1ID;
                lSchProductRouteRes.FResChaValue2ID = dr.FResChaValue2ID;
                lSchProductRouteRes.FResChaValue3ID = dr.FResChaValue3ID;
                lSchProductRouteRes.FResChaValue4ID = dr.FResChaValue4ID;
                lSchProductRouteRes.FResChaValue5ID = dr.FResChaValue5ID;
                lSchProductRouteRes.FResChaValue6ID = dr.FResChaValue6ID;
                lSchProductRouteRes.resource = schData.ResourceList.find((p) => p.cResourceNo == lSchProductRouteRes.cResourceNo);
                if (lSchProductRouteRes.resource == null) {
                    throw new Error(`排程ID[${lSchProductRouteRes.iSchSdID}, ${lSchProductRouteRes.iProcessProductID}] 资源编号[${lSchProductRouteRes.cResourceNo}]不存在,注意区分大小写和空格,或者资源没有定义工作日历!`);
                }
                if (lSchProductRouteRes.resource.FProChaType1ID != '-1' &&
                    lSchProductRouteRes.resource.FProChaType1ID != '')
                    lSchProductRouteRes.resChaValue1 = new ResChaValue(lSchProductRouteRes.FResChaValue1ID, lSchProductRouteRes, 1);
                if (lSchProductRouteRes.resource.FProChaType2ID != '-1' &&
                    lSchProductRouteRes.resource.FProChaType2ID != '')
                    lSchProductRouteRes.resChaValue2 = new ResChaValue(lSchProductRouteRes.FResChaValue2ID, lSchProductRouteRes, 2);
                if (lSchProductRouteRes.resource.FProChaType3ID != '-1' &&
                    lSchProductRouteRes.resource.FProChaType3ID != '')
                    lSchProductRouteRes.resChaValue3 = new ResChaValue(lSchProductRouteRes.FResChaValue3ID, lSchProductRouteRes, 3);
                if (lSchProductRouteRes.resource.FProChaType4ID != '-1' &&
                    lSchProductRouteRes.resource.FProChaType4ID != '')
                    lSchProductRouteRes.resChaValue4 = new ResChaValue(lSchProductRouteRes.FResChaValue4ID, lSchProductRouteRes, 4);
                if (lSchProductRouteRes.resource.FProChaType5ID != '-1' &&
                    lSchProductRouteRes.resource.FProChaType5ID != '')
                    lSchProductRouteRes.resChaValue5 = new ResChaValue(lSchProductRouteRes.FResChaValue5ID, lSchProductRouteRes, 5);
                if (lSchProductRouteRes.resource.FProChaType6ID != '-1' &&
                    lSchProductRouteRes.resource.FProChaType6ID != '')
                    lSchProductRouteRes.resChaValue6 = new ResChaValue(lSchProductRouteRes.FResChaValue6ID, lSchProductRouteRes, 6);
                lSchProductRouteRes.FResChaValue1Cyc = dr.FResChaValue1Cyc;
                lSchProductRouteRes.FResChaValue2Cyc = dr.FResChaValue2Cyc;
                lSchProductRouteRes.FResChaValue3Cyc = dr.FResChaValue3Cyc;
                lSchProductRouteRes.FResChaValue4Cyc = dr.FResChaValue4Cyc;
                lSchProductRouteRes.FResChaValue5Cyc = dr.FResChaValue5Cyc;
                lSchProductRouteRes.FResChaValue6Cyc = dr.FResChaValue6Cyc;
                lSchProductRouteRes.cDefine22 = dr.cDefine22;
                lSchProductRouteRes.cDefine23 = dr.cDefine23;
                lSchProductRouteRes.cDefine34 = dr.cDefine34;
                lSchProductRouteRes.cDefine35 = 0;
                lSchProductRouteRes.iSchBatch = dr.iSchBatch;
                lSchProductRouteRes.TaskTimeRangeList = [];
                schData.SchProductRouteResList.push(lSchProductRouteRes);
            });
            console.log('2.5 填充SchProductRouteItemList');
            if (SchParam.cSelfEndDate == '1') {
                schData.dtSchProductRouteItem = await this.getSqlDataTable(lsSchProductRouteItem);
                schData.dtSchProductRouteItem.forEach((dr) => {
                    let lSchProductRouteItem = new SchProductRouteItem();
                    lSchProductRouteItem.schData = this.schData;
                    lSchProductRouteItem.iSchSdID = dr.iSchSdID;
                    lSchProductRouteItem.cVersionNo = dr.cVersionNo;
                    lSchProductRouteItem.iProcessProductID = dr.iProcessProductID;
                    if (lSchProductRouteItem.iProcessProductID < 0)
                        return;
                    lSchProductRouteItem.iEntryID = dr.iEntryID;
                    lSchProductRouteItem.cWoNo = dr.cWoNo;
                    lSchProductRouteItem.cInvCode = dr.cInvCode.trim();
                    lSchProductRouteItem.iWoSeqID = dr.iSeqID;
                    lSchProductRouteItem.cInvCodeFull = dr.cInvCodeFull;
                    lSchProductRouteItem.cSubInvCode = dr.cSubInvCode;
                    lSchProductRouteItem.cSubInvCodeFull = dr.cSubInvCodeFull;
                    lSchProductRouteItem.bSelf = dr.bSelf;
                    lSchProductRouteItem.cUtterType = dr.cUtterType;
                    lSchProductRouteItem.cSubRelate = dr.cSubRelate;
                    lSchProductRouteItem.iQtyPer = dr.iQtyPer;
                    lSchProductRouteItem.iScrapt = dr.iScrapt;
                    lSchProductRouteItem.iReqQty = dr.iReqQty;
                    lSchProductRouteItem.iNormalQty = dr.iNormalQty;
                    lSchProductRouteItem.iScrapQty = dr.iScrapQty;
                    lSchProductRouteItem.iProQty = dr.iProQty;
                    lSchProductRouteItem.iKeepQty = dr.iKeepQty;
                    lSchProductRouteItem.iPlanQty = dr.iPlanQty;
                    lSchProductRouteItem.dReqDate = dr.dReqDate;
                    lSchProductRouteItem.dForeInDate = dr.dForeInDate;
                    lSchProductRouteItem.iSchBatch = dr.iSchBatch;
                    lSchProductRouteItem.schProductRoute = schData.SchProductRouteList.find((p) => p.iProcessProductID == lSchProductRouteItem.iProcessProductID);
                    if (lSchProductRouteItem.schProductRoute == null) {
                        return;
                    }
                    schData.SchProductRouteItemList.push(lSchProductRouteItem);
                });
            }
            console.log('2.6 建立对象之间的关系');
            let k = 0;
            schData.SchProductRouteList.forEach((lSchProductRoute) => {
                lSchProductRoute.SchProductRouteResList = schData.SchProductRouteResList.filter((p) => p.iSchSdID == lSchProductRoute.iSchSdID &&
                    p.iProcessProductID == lSchProductRoute.iProcessProductID);
                if (lSchProductRoute.SchProductRouteResList.length > 0) {
                    lSchProductRoute.SchProductRouteResList.forEach((lSchProductRouteRes) => {
                        lSchProductRouteRes.schProductRoute = lSchProductRoute;
                    });
                }
                else {
                    lSchProductRoute.dBegDate = this.schData.dtStart;
                    lSchProductRoute.dEndDate = this.schData.dtStart.addMinutes(1);
                    lSchProductRoute.BScheduled = 1;
                }
                if (SchParam.cSelfEndDate == '1') {
                    lSchProductRoute.SchProductRouteItemList = schData.SchProductRouteItemList.filter((p) => p.iSchSdID == lSchProductRoute.iSchSdID &&
                        p.iProcessProductID == lSchProductRoute.iProcessProductID);
                }
                if (lSchProductRoute.cStatus != '2' &&
                    lSchProductRoute.cStatus != '4' &&
                    lSchProductRoute.cPreProcessItem != '') {
                    this.getSchProductRouteList(lSchProductRoute, true);
                }
                else if (SchParam.ExecTaskSchType != '1' &&
                    lSchProductRoute.cStatus != '4' &&
                    lSchProductRoute.cPreProcessItem != '') {
                    this.getSchProductRouteList(lSchProductRoute, true);
                }
                if (lSchProductRoute.cVersionNo != 'SureVersion') {
                    let ListRouteRes = lSchProductRoute.SchProductRouteResList.filter((p) => p.cSelected == '1');
                    let iRandom;
                    let iCount = ListRouteRes.length, iRowCount = ListRouteRes.length;
                    let iResCount = lSchProductRoute.iDevCountPd;
                    if (iResCount == 0)
                        iResCount = 2;
                    if (ListRouteRes.length > 2 && iResCount < iCount) {
                        for (let i = iRowCount - 1; i >= 0; i--) {
                            if (ListRouteRes[i].resource.iDistributionRate >= 100)
                                continue;
                            let rd = new Random();
                            iRandom = rd.next(1, 100);
                            if (iRandom > ListRouteRes[i].resource.iDistributionRate) {
                                ListRouteRes[i].cSelected = '0';
                                ListRouteRes[i].cCanScheduled = '0';
                                ListRouteRes[i].iResReqQty = 0;
                                ListRouteRes[i].iResRationHour = 0;
                                iCount--;
                            }
                            if (iCount <= iResCount)
                                break;
                        }
                    }
                }
                k++;
            });
            schData.SchProductList.forEach((lSchProduct) => {
                lSchProduct.SchProductRouteList = schData.SchProductRouteList.filter((p) => p.iSchSdID == lSchProduct.iSchSdID);
                lSchProduct.SchProductRouteList.forEach((lSchProductRoute) => {
                    lSchProductRoute.schProduct = lSchProduct;
                });
            });
            schData.SchProductWorkItemList.forEach((lSchProductWorkItem) => {
                lSchProductWorkItem.SchProductRouteList = schData.SchProductRouteList.filter((p) => p.iSchSdID == lSchProductWorkItem.iSchSdID &&
                    p.cWoNo == lSchProductWorkItem.cWoNo);
                lSchProductWorkItem.SchProductRouteList.forEach((lSchProductRoute) => {
                    lSchProductRoute.schProductWorkItem = lSchProductWorkItem;
                });
            });
            console.log('2.7 其他基础资料表');
            schData.dtWorkCenter = await this.getSqlDataTable(lsWorkCenter);
            schData.dtDepartment = await this.getSqlDataTable(lsDepartment);
            schData.dtPerson = await this.getSqlDataTable(lsPerson);
            schData.dtTeam = await this.getSqlDataTable(lsteam);
            schData.dtTechLearnCurves = await this.getSqlDataTable(lsTechLearnCurves);
            schData.dtResTechScheduSN = await this.getSqlDataTable(lsResTechScheduSN);
            let SchProductRouteList = schData.SchProductRouteList.filter((p1) => p1.cDevCountPdExp != '' && p1.cWoNo == '');
            console.log('第二步骤结束');
            return 1;
        }
        catch (ex1) {
            console.log(`排产计算出错！位置第二步骤,出错内容：${ex1.toString()}`);
            throw ex1;
            return -1;
        }
    }
    async getSqlDataTable(query) {
        let pool = await sql.connect(this.config);
        let result = await pool.request().query(query);
        return result.recordset;
    }
    getParamValue(paramName) {
        // Implement this method to retrieve the parameter value
        return '';
    }
    getResourceList() {
        console.log('取生成了资源工作日历的资源列表');
        let lsResAddWhere = " and cStatus <> '3' and (cResourceNo in (select distinct cResourceNo ";
        let lstg_Sql = `EXECUTE P_GetSchDataResource '${this.schData.cVersionNo}', '${this.schData.dtStart.addDays(-20)}', '${this.schData.dtEnd}', '${SchParam.cSchType}', '${SchParam.cTechSchType}', '${this.schData.cCalculateNo}'`;
        this.schData.dtResource = SqlPro.getDataTable(lstg_Sql, null);
        let lsSql = "SELECT FProChaTypeID,FResChaTypeName,cParentClsNo,bEnd,cBarCode,cNote,cMacNo FROM t_ProChaType where isnull(bEnd,1) = '1'";
        this.schData.dtProChaType = SqlPro.getDataTable(lsSql, null);
        console.log('取工艺特征类型');
        lsSql = `SELECT FResChaValueID, FResChaValueNo, FResChaValueName, FProChaTypeID,isnull(FResChaCycleValue,0) as FResChaCycleValue, isnull(FResChaRePlaceTime,0) as FResChaRePlaceTime, FResChaMemo,isnull(FUseFixedPlaceTime,'1') as FUseFixedPlaceTime, 
                    FSchSN, isnull(FUseChaCycleValue,'0') as FUseChaCycleValue, cDefine1, cDefine2, cDefine3, cDefine4, cDefine5, cDefine6, cDefine7, cDefine8, cDefine9, cDefine10, isnull(cDefine11,0) as cDefine11, isnull(cDefine12,0) as cDefine12, isnull(cDefine13,0) as cDefine13, 
                    isnull(cDefine14,0) as cDefine14,isnull(cDefine15,'1900-01-01') cDefine15,isnull(cDefine16,'1900-01-01')  cDefine16
                FROM dbo.t_ResChaValue with (nolock) where 1 = 1 `;
        this.schData.dtResChaValue = SqlPro.getDataTable(lsSql, null);
        console.log('取工艺特征值');
        lsSql =
            'SELECT FProChaTypeID,FResChaValue1ID,FResChaValue2ID,FResChaExcTime FROM dbo.t_ResChaCrossTime with (nolock) where 1 = 1';
        this.schData.dtResChaCrossTime = SqlPro.getDataTable(lsSql, null);
        console.log('取工艺特征转换时间');
        console.log('取排程参数');
        lstg_Sql = '';
        lstg_Sql = `Update t_SchProduct set dEarLiestSchDate = '${this.schData.dtStart}' where dEarLiestSchDate < '${this.schData.dtStart}' and isnull(cWoNo,'') = ''`;
        SqlPro.executeNonQuery(lstg_Sql, null);
        console.log('更新未排订单排程可开始时间');
        try {
            console.log('schData.dtStart - schData.dtEnd' +
                this.schData.dtStart.addDays(-20) +
                this.schData.dtEnd);
            lstg_Sql = `EXECUTE P_GetResWorkTime '${this.schData.cVersionNo}','${this.schData.dtStart.addDays(-20)}','${this.schData.dtEnd}','${SchParam.cSchType}','${SchParam.cTechSchType}','${this.schData.cCalculateNo}'`;
            let dt_ResourceTime = SqlPro.getDataTable(lstg_Sql, null);
            this.schData.dt_ResourceTime = dt_ResourceTime;
            this.schData.dt_ResourceTime.defaultView.sort =
                'iPeriodTimeID asc,dPeriodDay asc';
            this.schData.iProgress = 13;
            let message = '2.1、资源工作日历生成完成';
            let schPrecent = this.schData.iProgress;
            this.dlShowProcess(schPrecent.toString(), message);
            console.log('调用过程P_GetResWorkTime生成所有资源的工作日历');
            lstg_Sql = `SELECT * FROM t_SchResWorkTime with (nolock) WHERE cType = '1' and cSourceType = '2' AND cVersionNo = '${this.schData.cVersionNo}'`;
            let dt_ResourceSpecTime = SqlPro.getDataTable(lstg_Sql, null);
            this.schData.dt_ResourceSpecTime = dt_ResourceSpecTime;
            this.schData.dt_ResourceSpecTime.defaultView.sort = 'dPeriodDay asc';
            let cWorkCenter;
            let lobj_WorkCenter;
            console.log('3、生成工作中心');
            for (let drWorkCenter of this.schData.dtWorkCenter.rows) {
                cWorkCenter = drWorkCenter['cWcNo'].toString();
                lobj_WorkCenter = new Algorithm.WorkCenter(cWorkCenter, this.schData);
                this.schData.WorkCenterList.push(lobj_WorkCenter);
            }
            let cResourceNoOld = '';
            let cResourceNo;
            let lobj_Resource;
            let lResTimeRange;
            let ResTimeRangeType;
            console.log('开始按资源循环，生成资源对象，同时生成资源时间段');
            for (let drResource of this.schData.dtResource.rows) {
                cResourceNo = drResource['cResourceNo'].toString();
                if (cResourceNo === 'gys20039') {
                    let m = 1;
                }
                lobj_Resource = new Resource(cResourceNo, this.schData);
                this.schData.ResourceList.push(lobj_Resource);
                if (lobj_Resource.bTeamResource !== '1') {
                    let drResTime = dt_ResourceTime.select(`cResourceNo = '${cResourceNo}'`);
                    for (let drResTimeRange of drResTime) {
                        lResTimeRange = new ResTimeRange();
                        lResTimeRange.CResourceNo = cResourceNo;
                        lResTimeRange.resource = lobj_Resource;
                        lResTimeRange.CIsInfinityAbility = drResource['cIsInfinityAbility'].toString();
                        lResTimeRange.iPeriodID = parseInt(drResTimeRange['iPeriodTimeID']);
                        lResTimeRange.DBegTime = new Date(drResTimeRange['dPeriodBegTime']);
                        lResTimeRange.DEndTime = new Date(drResTimeRange['dPeriodEndTime']);
                        lResTimeRange.dPeriodDay = new Date(drResTimeRange['dPeriodDay']);
                        lResTimeRange.FShiftType = drResTimeRange['FShiftType'].toString();
                        if (lResTimeRange.FShiftType === '') {
                            lResTimeRange.FShiftType = 'A班'; //班次 A班 夜班 中班等
                        }
                        lResTimeRange.Attribute = (parseInt(drResTimeRange['cTimeRangeType'].toString()));
                        lResTimeRange.getNoWorkTaskTimeRange(lResTimeRange.DBegTime, lResTimeRange.DEndTime, true);
                        lobj_Resource.ResTimeRangeList.push(lResTimeRange);
                    }
                    let drResSpecTime = dt_ResourceSpecTime.select(`cResourceNo = '${cResourceNo}'`);
                    for (let drResTimeRange of drResSpecTime) {
                        lResTimeRange = new ResTimeRange();
                        lResTimeRange.CResourceNo = cResourceNo;
                        lResTimeRange.resource = lobj_Resource;
                        lResTimeRange.iPeriodID = parseInt(drResTimeRange['iPeriodTimeID'].toString());
                        lResTimeRange.CIsInfinityAbility = drResource['cIsInfinityAbility'].toString();
                        lResTimeRange.DBegTime = new Date(drResTimeRange['dPeriodBegTime']);
                        lResTimeRange.DEndTime = new Date(drResTimeRange['dPeriodEndTime']);
                        lResTimeRange.Attribute = (parseInt(drResTimeRange['cTimeRangeType'].toString()));
                        lResTimeRange.HoldingTime =
                            parseInt(parseFloat(drResTimeRange['iHoldingTime'].toString())) *
                                60;
                        lobj_Resource.ResSpecTimeRangeList.push(lResTimeRange);
                    }
                    if (drResSpecTime.length > 0) {
                        lobj_Resource.mergeTimeRange();
                    }
                    lobj_Resource.getResSourceDayCapList();
                }
            }
            this.schData.KeyResourceList = this.schData.ResourceList.filter((p1) => p1.cIsKey === '1' && p1.iKeySchSN > 0);
            this.schData.KeyResourceList.sort((p1, p2) => p1.iKeySchSN - p2.iKeySchSN);
            this.schData.TeamResourceList = this.schData.ResourceList.filter((p1) => p1.bTeamResource === '1');
            if (this.schData.TeamResourceList.length > 0) {
                for (let TeamResource of this.schData.TeamResourceList) {
                    let ResourceSubList = this.schData.ResourceList.filter((p1) => p1.cTeamResourceNo === TeamResource.cResourceNo);
                    TeamResource.TeamResourceList = ResourceSubList;
                    for (let TeamResourceSub of ResourceSubList) {
                        TeamResourceSub.TeamResource = TeamResource;
                    }
                }
            }
        }
        catch (ex1) {
            console.log('排产计算出错！位置GetResourceList(),出错内容：' + ex1.toString());
            throw ex1;
            return -1;
        }
        return 1;
    }
}
exports.SchInterface = SchInterface;
