"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SchProductRoute = void 0;
const Base_1 = require("./Base");
const type_1 = require("./type");
class SchProductRoute extends Base_1.Base {
    constructor() {
        super(...arguments);
        this.schData = null;
        this.bScheduled = 0;
        this.iDevCountPd = 1;
        this.iSchBatch = 6;
        this.cBatchNoFlag = 0;
        this.SchProductRouteItemList = new Array(10);
        this.SchProductRouteResList = new Array(10);
        this.SchProductRoutePreList = new Array(10);
        this.SchProductRouteNextList = new Array(10);
        //   GetObjectData(info: any, context: any) {
        //     throw new Error('Not implemented')
        //   }
    }
    get iSeqPreTime() {
        if (this.ISeqPretime == null)
            this.ISeqPretime = 0;
        return this.ISeqPretime;
    }
    set iSeqPreTime(value) {
        this.ISeqPretime = value;
    }
    get iSeqPostTime() {
        if (this.ISeqPostTime == null)
            this.ISeqPostTime = 0;
        return this.ISeqPostTime;
    }
    set iSeqPostTime(value) {
        this.ISeqPostTime = value;
    }
    get BScheduled() {
        return this.bScheduled;
    }
    set BScheduled(value) {
        let SchParam = this.SchParam;
        if (value == 1 && this.bScheduled == 0) {
            if (this.schData.iCurRows < this.schData.iTotalRows) {
                const iCount = this.SchProductRouteResList.filter((item) => item.iResReqQty == 0).length;
                this.schData.iCurRows += iCount;
            }
        }
        else if (value == 0 && this.bScheduled == 1) {
            const iCount = this.SchProductRouteResList.filter((item) => item.iResReqQty == 0).length;
            if (this.schData.iCurRows > iCount) {
                this.schData.iCurRows -= iCount;
            }
            else {
                this.schData.iCurRows = 0;
            }
        }
        this.bScheduled = value;
        if (this.iSchSdID == SchParam.iSchSdID) {
            const dt = this.dBegDate;
            const dt2 = this.dEndDate;
        }
        if (this.iSchSdID == SchParam.iSchSdID &&
            this.iProcessProductID == SchParam.iProcessProductID) {
            const dt = this.dBegDate;
            const dt2 = this.dEndDate;
        }
    }
    ProcessSchTask(bFreeze = false) {
        let Comparer = this.Comparer;
        let SchParam = this.SchParam;
        if (this.bScheduled == 1)
            return 1;
        if (this.iSchSdID == SchParam.iSchSdID &&
            this.iProcessProductID == SchParam.iProcessProductID) {
            let j;
            j = 1;
        }
        if (this.cParallelNo != '' &&
            this.cKeyBrantch != '1' &&
            this.SchProductRouteNextList.length > 0 &&
            this.SchProductRouteNextList[0].bScheduled != 1) {
            return 1;
        }
        let dDateTemp = this.schData.dtStart;
        let dCanBegDate = this.schData.dtStart;
        let dCanBegDateProcess = this.schData.dtStart;
        try {
            if (dCanBegDate < this.dEarlySubItemDate)
                dCanBegDate = this.dEarlySubItemDate;
            if (this.schProduct != null) {
                if (dCanBegDate < this.schProduct.dEarliestSchDate)
                    dCanBegDate = this.schProduct.dEarliestSchDate;
            }
            if (this.schProductWorkItem != null) {
                if (dCanBegDate < this.schProductWorkItem.dCanBegDate)
                    dCanBegDate = this.schProductWorkItem.dCanBegDate;
            }
            if (this.cVersionNo.toLowerCase() == 'sureversion' &&
                this.dFirstBegDate > this.schData.dtStart &&
                //@ts-ignore
                dCanBegDate <
                    this.dFirstBegDate.setDate(this.dFirstBegDate.getDate() - SchParam.dLastBegDateBeforeDays)) {
                //@ts-ignore
                dCanBegDate = this.dFirstBegDate.setDate(this.dFirstBegDate.getDate() - SchParam.dLastBegDateBeforeDays);
            }
            this.GetRouteEarlyBegDate();
            if (dCanBegDate < this.dEarlyBegDate)
                dCanBegDate = this.dEarlyBegDate;
        }
        catch (error) { }
        if (this.cVersionNo.toLowerCase() == 'sureversion') {
            if (SchParam.UseAPS == '3' &&
                this.schProductWorkItem != null &&
                this.schProductWorkItem.cStatus == 'I' &&
                this.SchProductRouteResList.length > 1) {
                try {
                    this.ResourceSelect(dCanBegDate, bFreeze);
                }
                catch (error) {
                    if (SchParam.iSchSdID < 1)
                        throw new Error('多资源选择出错，订单行号：' +
                            this.iSchSdID +
                            ',位置SchProductRoute.ResourceSelect！工序ID号：' +
                            this.iProcessProductID +
                            '\n\r ' +
                            error.message.toString());
                    else
                        throw new Error('多资源选择出错，订单行号：' +
                            this.iSchSdID +
                            ',位置SchProductRoute.ResourceSelect！工序ID号：' +
                            this.iProcessProductID +
                            '\n\r ' +
                            error.message.toString() +
                            '明细信息:' +
                            error.stack);
                    return -1;
                }
            }
            const ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.iResReqQtyOld > 0);
            if (this.item != null &&
                this.item.cMoldPosition != '' &&
                this.item.cMoldPosition != '1') {
                const ListRouteRes2 = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.iResReqQty > 0);
                for (let i = 0; i < ListRouteRes2.length; i++) {
                    ListRouteRes2[i].dResBegDate = dCanBegDate;
                    ListRouteRes2[i].dResEndDate = new type_1.DateTime(dCanBegDate.getTime() + 60000);
                    ListRouteRes2[i].TaskTimeRangeList.length = 0;
                    ListRouteRes2[i].BScheduled = 1;
                }
            }
            for (let i = 0; i < ListRouteRes.length; i++) {
                if (ListRouteRes[i].iResReqQty > 0)
                    ListRouteRes[i].TaskSchTask(ListRouteRes[i].iResReqQty, dCanBegDate);
                else {
                    ListRouteRes[i].dResBegDate = dCanBegDate;
                    ListRouteRes[i].dResEndDate = new type_1.DateTime(dCanBegDate.getTime() + 60000);
                    ListRouteRes[i].TaskTimeRangeList.length = 0;
                    ListRouteRes[i].BScheduled = 1;
                }
            }
        }
        else {
            try {
                if (this.iReqQty > 0) {
                    try {
                        this.ResourceSelect(dCanBegDate, bFreeze);
                    }
                    catch (error) {
                        if (SchParam.iSchSdID < 1)
                            throw new Error('多资源选择出错，订单行号：' +
                                this.iSchSdID +
                                ',位置SchProductRoute.ResourceSelect！工序ID号：' +
                                this.iProcessProductID +
                                '\n\r ' +
                                error.message.toString());
                        else
                            throw new Error('多资源选择出错，订单行号：' +
                                this.iSchSdID +
                                ',位置SchProductRoute.ResourceSelect！工序ID号：' +
                                this.iProcessProductID +
                                '\n\r ' +
                                error.message.toString() +
                                '明细信息:' +
                                error.stack);
                        return -1;
                    }
                    if (this.item != null &&
                        this.item.cMoldPosition != null &&
                        this.item.cMoldPosition != '' &&
                        this.item.cMoldPosition != '0' &&
                        this.item.cMoldPosition != '1') {
                        const ListRouteRes2 = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.iResReqQty > 0);
                        for (let i = 0; i < ListRouteRes2.length; i++) {
                            ListRouteRes2[i].dResBegDate = dCanBegDate;
                            ListRouteRes2[i].dResEndDate = new type_1.DateTime(dCanBegDate.getTime() + 60000);
                            ListRouteRes2[i].TaskTimeRangeList.length = 0;
                            ListRouteRes2[i].BScheduled = 1;
                        }
                    }
                }
                else {
                    const ListRouteRes2 = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.iResReqQty > 0);
                    for (let i = 0; i < ListRouteRes2.length; i++) {
                        if (ListRouteRes2[i].iResReqQty > 0)
                            ListRouteRes2[i].TaskSchTask(ListRouteRes2[i].iResReqQty, dCanBegDate);
                        else {
                            ListRouteRes2[i].dResBegDate = dCanBegDate;
                            ListRouteRes2[i].dResEndDate = new type_1.DateTime(dCanBegDate.getTime() + 60000);
                            ListRouteRes2[i].TaskTimeRangeList.length = 0;
                            ListRouteRes2[i].BScheduled = 1;
                        }
                    }
                }
            }
            catch (error) {
                if (SchParam.iSchSdID < 1)
                    throw new Error('订单行号：' +
                        this.iSchSdID +
                        '资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：' +
                        this.iProcessProductID +
                        '\n\r ' +
                        error.message.toString());
                else
                    throw new Error('订单行号：' +
                        this.iSchSdID +
                        '资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：' +
                        this.iProcessProductID +
                        '\n\r ' +
                        error.message.toString() +
                        '明细信息:' +
                        error.stack);
                return -1;
            }
            const ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.cCanScheduled == '1');
            let iResCount = ListRouteRes.length;
            if (iResCount < 1) {
                const ListRouteResCan = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.cCanScheduled == '0');
                if (ListRouteResCan.length < 1) {
                    throw new Error('订单行号：' +
                        this.iSchSdID +
                        '资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：' +
                        this.iProcessProductID +
                        '\n\r ' +
                        "没有可排产资源编号.或排产范围太小,开始日期：'" +
                        dCanBegDate.toLocaleDateString() +
                        "'");
                    return -1;
                }
                else {
                    ListRouteResCan.forEach((item) => {
                        item.BScheduled = 1;
                    });
                    this.BScheduled = 1;
                    return 1;
                }
            }
            dCanBegDateProcess = dCanBegDate;
            for (let i = 0; i < ListRouteRes.length; i++) {
                let iResReqQty = ListRouteRes[i].iResReqQty;
                try {
                    const ldtBeginDate = new type_1.DateTime();
                    ListRouteRes[i].TaskSchTask(iResReqQty, dCanBegDate);
                    const ldtEndedDate = new type_1.DateTime();
                    const iWaitTime2 = new type_1.DateTime().getTime() - ldtBeginDate.getTime();
                    const interval = ldtEndedDate.getTime() - ldtBeginDate.getTime();
                }
                catch (error) {
                    if (SchParam.iSchSdID < 1)
                        throw new Error('资源任务正排出错,订单行号：' +
                            this.iSchSdID +
                            '资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：' +
                            this.iProcessProductID +
                            '\n\r ' +
                            error.message.toString());
                    else
                        throw new Error('资源任务正排出错,订单行号：' +
                            this.iSchSdID +
                            '资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：' +
                            this.iProcessProductID +
                            '\n\r ' +
                            error.message.toString() +
                            '明细信息:' +
                            error.stack);
                    return -1;
                }
                if (this.SchProductRoutePreList != null &&
                    this.SchProductRoutePreList.length > 0) {
                    let SchData = this.schData;
                    this.SchProductRoutePreList.forEach((schProductRoutePre) => {
                        if (schProductRoutePre.bScheduled != 1)
                            return;
                        if (SchParam.iPreTaskRev > 0 &&
                            ListRouteRes[i].dResEndDate.getTime() -
                                schProductRoutePre.dEndDate.getTime() >
                                SchParam.iPreTaskRev * 3600000) {
                            const iDiffMinites = SchData.GetDateDiff('m', schProductRoutePre.dEndDate, ListRouteRes[i].dResEndDate) -
                                schProductRoutePre.iSeqPostTime -
                                ListRouteRes[i].schProductRoute.iSeqPreTime;
                            if (iDiffMinites > SchParam.iPreTaskRev * 60) {
                                const iMoveTime = this.GetProcessMoveQty(schProductRoutePre);
                                dCanBegDate = new type_1.DateTime(ListRouteRes[i].dResEndDate.getTime() -
                                    iMoveTime * 60000 -
                                    schProductRoutePre.iSeqPostTime * 60000 -
                                    ListRouteRes[i].schProductRoute.iSeqPreTime * 60000);
                                schProductRoutePre.ProcessClearTask();
                                try {
                                    schProductRoutePre.ProcessSchTaskRev('0');
                                }
                                catch (error) {
                                    if (SchParam.iSchSdID < 1)
                                        throw new Error('订单行号：' +
                                            this.iSchSdID +
                                            '资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：' +
                                            this.iProcessProductID +
                                            '\n\r ' +
                                            error.message.toString());
                                    else
                                        throw new Error('订单行号：' +
                                            this.iSchSdID +
                                            '资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：' +
                                            this.iProcessProductID +
                                            '\n\r ' +
                                            error.message.toString() +
                                            '明细信息:' +
                                            error.stack);
                                    return -1;
                                }
                            }
                        }
                    });
                }
            }
        }
        const list1 = this.SchProductRouteResList.filter((p1) => p1.iSchSdID == this.iSchSdID &&
            p1.iProcessProductID == this.iProcessProductID &&
            p1.iResReqQty > 0);
        if (list1.length > 0) {
            list1.sort((p1, p2) => Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate));
            this.dBegDate = list1[0].dResBegDate;
            this.dEndDate = list1[list1.length - 1].dResEndDate;
        }
        else {
            const list2 = this.SchProductRouteResList.filter((p1) => p1.iSchSdID == this.iSchSdID &&
                p1.iProcessProductID == this.iProcessProductID &&
                p1.iResReqQty > 0);
            if (list2.length > 0) {
                list2.sort((p1, p2) => Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate));
                this.dBegDate = list2[0].dResBegDate;
                this.dEndDate = list2[list2.length - 1].dResEndDate;
            }
        }
        let iLaborTime = 0;
        this.SchProductRouteResList.forEach((lSchProductRouteRes) => {
            iLaborTime += lSchProductRouteRes.iResRationHour;
        });
        this.iLaborTime = iLaborTime;
        this.BScheduled = 1;
        if (this.schProduct.cBatchNo != '' && this.cBatchNoFlag == 0)
            this.cBatchSchRoute();
    }
    ResourceSelect(dCanBegDate, bFreeze = false) {
        let Comparer = this.Comparer;
        let SchParam = this.SchParam;
        let ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected == '1');
        if (this.iSchSdID == SchParam.iSchSdID &&
            this.iProcessProductID == SchParam.iProcessProductID) {
            let j;
        }
        const iRouteResCountFirst = ListRouteRes.length;
        if (iRouteResCountFirst < 1) {
            throw new Error(`多资源选择正排出错,订单行号：${this.iSchSdID}，没有找到已选择的可排产资源，资源产能明细总资源数量为${this.SchProductRouteResList.length}，位置SchProductRoute.ProcessSchTask！工序ID号：${this.iProcessProductID}，物料编号${this.cInvCode}`);
            return -1;
        }
        if (bFreeze || this.iActQty > 0) {
            if (this.iReqQty > 0) {
                for (let i = 0; i < ListRouteRes.length; i++) {
                    if (ListRouteRes[i].iResReqQty == 0) {
                        ListRouteRes[i].BScheduled = 1;
                        ListRouteRes[i].cCanScheduled = '0';
                    }
                }
            }
            return 1;
        }
        else {
            if (ListRouteRes.length > 1) {
                if (this.SchProductRoutePreList.length == 1 &&
                    this.SchProductRoutePreList[0].cInvCode == this.cInvCode &&
                    this.SchProductRoutePreList[0].SchProductRouteResList[0].resource
                        .cDayPlanShowType != '') {
                    const schProductRouteRes = this.SchProductRoutePreList[0].SchProductRouteResList.find((item) => item.BScheduled == 1 &&
                        item.iResReqQty > 0 &&
                        item.resource.cDayPlanShowType != '');
                    if (schProductRouteRes != null) {
                        this.SchProductRouteResList.forEach((item) => {
                            if (item.resource.cDayPlanShowType != '' &&
                                !item.resource.cDayPlanShowType.includes(schProductRouteRes.resource.cDayPlanShowType) &&
                                item.cSelected == '1') {
                                item.cSelected = '0';
                                item.iResReqQty = 0;
                                item.cDefine25 =
                                    '关联分组号不同取消:' +
                                        schProductRouteRes.resource.cDayPlanShowType;
                            }
                        });
                    }
                    const ListRouteRes2 = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.iResReqQty > 0);
                    if (ListRouteRes2.length < 1) {
                        ListRouteRes[0].cSelected = '1';
                        ListRouteRes[0].iResReqQty = this.iReqQty;
                    }
                }
            }
        }
        ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected == '1');
        let iResCount = ListRouteRes.length;
        if (iResCount <= 1)
            return 1;
        if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1)
            iResCount = this.iDevCountPd;
        ListRouteRes.sort((p1, p2) => Comparer.Default.Compare(p1.iCapacity / p1.iBatchQty, p2.iCapacity / p1.iBatchQty));
        let iCapacity = ListRouteRes[0].iCapacity;
        let iBatchQty = ListRouteRes[0].iBatchQty;
        let iMinResCount = 1;
        if (ListRouteRes.length > 1) {
            if (this.schProduct.iWorkQtyPd > 0 &&
                this.schProduct.iWorkQtyPd < this.iReqQty) {
                let iDevCount = (this.schProduct.iWorkQtyPd * iCapacity) /
                    3600 /
                    ListRouteRes[0].resource.iResHoursPd /
                    ListRouteRes[0].resource.iResourceNumber /
                    ListRouteRes[0].iBatchQty;
                if (this.item != null &&
                    this.item.iItemDifficulty > 0 &&
                    this.item.iItemDifficulty != 1)
                    iDevCount = iDevCount * this.item.iItemDifficulty;
                if (ListRouteRes[0].resource.iResDifficulty > 0 &&
                    ListRouteRes[0].resource.iResDifficulty != 1)
                    iDevCount = iDevCount * ListRouteRes[0].resource.iResDifficulty;
                if (this.techInfo.iTechDifficulty > 0 &&
                    this.techInfo.iTechDifficulty != 1)
                    iDevCount = iDevCount * this.techInfo.iTechDifficulty;
                iMinResCount = Math.floor(iDevCount);
                iResCount = iMinResCount;
            }
            else {
                if (ListRouteRes[0].resource.iMinWorkTime > 0)
                    iMinResCount = Math.floor((this.iReqQty * iCapacity) /
                        iBatchQty /
                        3600 /
                        ListRouteRes[0].resource.iMinWorkTime);
                else
                    iMinResCount = iResCount;
            }
        }
        if (iResCount > iMinResCount && iMinResCount <= ListRouteRes.length)
            iResCount = iMinResCount;
        if (iResCount < 1 && ListRouteRes.length > 0)
            iResCount = 1;
        if (this.item != null &&
            this.item.iMoldCount > 0 &&
            iResCount > this.item.iMoldCount)
            iResCount = this.item.iMoldCount;
        let iResReqQtyPer = this.iReqQty;
        let iLeftReqQty = this.iReqQty;
        let iResReqQty = iResReqQtyPer;
        if (SchParam.cMutResourceType != '4') {
            try {
                for (let i = 0; i < ListRouteRes.length; i++) {
                    if (ListRouteRes[i].cCanScheduled != '1')
                        continue;
                    ListRouteRes[i].TestResSchTask(iResReqQty, dCanBegDate);
                }
            }
            catch (error) {
                if (SchParam.iSchSdID < 1)
                    throw new Error('资源选择正排出错,订单行号：' +
                        this.iSchSdID +
                        '资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：' +
                        this.iProcessProductID +
                        '\n\r ' +
                        error.message.toString());
                else
                    throw new Error('资源选择正排出错,订单行号：' +
                        this.iSchSdID +
                        '资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：' +
                        this.iProcessProductID +
                        '\n\r ' +
                        error.message.toString() +
                        '明细信息:' +
                        error.stack);
                return -1;
            }
        }
        let iSelectReturn = 0;
        if (iSelectReturn <= 0) {
            if (SchParam.cMutResourceType == '4') {
                ListRouteRes.sort((p1, p2) => Comparer.Default.Compare(p1.resource.iPlanDays, p2.resource.iPlanDays));
            }
            else if (SchParam.cMutResourceType == '3') {
                if (ListRouteRes.length > 1) {
                    for (let i = 0; i < ListRouteRes.length; i++) {
                        if (ListRouteRes[i].cCanScheduled != '1')
                            continue;
                        ListRouteRes[i].cDefine38 = new type_1.DateTime(ListRouteRes[i].dResEndDate.getTime() +
                            (ListRouteRes[i].iResGroupPriority + 1 - 1) *
                                SchParam.iMutResourceDiffHour *
                                3600000);
                    }
                    const ListRouteResSort = ListRouteRes.sort((c1, c2) => Comparer.Default.Compare(c1.cDefine38, c2.cDefine38) ||
                        Comparer.Default.Compare(c1.iResGroupPriority, c2.iResGroupPriority));
                    ListRouteRes = ListRouteResSort;
                }
            }
            else if (SchParam.cMutResourceType == '2') {
                ListRouteRes.sort((p1, p2) => Comparer.Default.Compare(p1.dResBegDate, p2.dResBegDate));
            }
            else {
                ListRouteRes.sort((p1, p2) => Comparer.Default.Compare(p1.dResEndDate, p2.dResEndDate));
            }
            const ResSelectList = [];
            const MoldSelectList = [];
            if (ListRouteRes.length > 1) {
                let j = 0;
                for (let i = 0; i < ListRouteRes.length; i++) {
                    if (ListRouteRes[i].cCanScheduled != '1')
                        continue;
                    if (j <= iResCount) {
                        if (ResSelectList.length > 0) {
                            const ResSelect = ResSelectList.find((s) => s.equals(ListRouteRes[i].cResourceNo));
                            if (ResSelectList.length > 0 &&
                                ResSelect != null &&
                                ResSelect != '') {
                                ListRouteRes[i].cCanScheduled = '0';
                                ListRouteRes[i].iResReqQty = 0;
                                ListRouteRes[i].iResRationHour = 0;
                                continue;
                            }
                        }
                        ResSelectList.push(ListRouteRes[i].cResourceNo);
                    }
                    j++;
                    if (j > iResCount) {
                        ListRouteRes[i].cCanScheduled = '0';
                        ListRouteRes[i].iResReqQty = 0;
                        ListRouteRes[i].iResRationHour = 0;
                    }
                }
            }
        }
        ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.cCanScheduled == '1');
        iResCount = ListRouteRes.length;
        if (iResCount < 1) {
            throw new Error(`多资源选择正排出错,订单行号：${this.iSchSdID}，没有找到已选择的可排产资源，资源产能明细总资源数量为${this.SchProductRouteResList.length}，初始选择可排资源数量为${iRouteResCountFirst}，位置SchProductRoute.ProcessSchTask！工序ID号：${this.iProcessProductID}，物料编号${this.cInvCode}`);
            return -1;
        }
        this.ResReqQtyDispatch();
        return 1;
    }
    ResReqQtyDispatch() {
        let ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.cCanScheduled == '1');
        let iResCount = ListRouteRes.length;
        if (iResCount <= 1)
            return 0;
        let iResReqQtyPer = Math.floor((this.iReqQty - this.iActQty) / iResCount);
        let iLeftReqQty = this.iReqQty - this.iActQty;
        let iResReqQty = iResReqQtyPer;
        if (iResReqQtyPer < 1) {
            iResCount = 1;
            iResReqQtyPer = this.iReqQty - this.iActQty;
        }
        for (let i = 0; i < ListRouteRes.length; i++) {
            if (iLeftReqQty <= 0)
                continue;
            if (i == ListRouteRes.length)
                iResReqQty = iLeftReqQty;
            else {
                if (ListRouteRes[i].iBatchQtyBase > 1 && ListRouteRes.length > 1) {
                    iResReqQty =
                        Math.ceil(iResReqQtyPer / ListRouteRes[i].iBatchQtyBase) *
                            ListRouteRes[i].iBatchQtyBase;
                }
                else {
                    iResReqQty = iResReqQtyPer;
                }
            }
            if (iLeftReqQty - iResReqQty > 0) {
                iLeftReqQty = iLeftReqQty - iResReqQty;
            }
            else {
                iResReqQty = iLeftReqQty;
                iLeftReqQty = 0;
            }
            if (this.iSchBatch == 1) {
                iResReqQty = ListRouteRes[i].iResReqQty;
            }
            if (iLeftReqQty > 0 && i == ListRouteRes.length - 1) {
                iResReqQty += iLeftReqQty;
            }
            ListRouteRes[i].iResReqQty = iResReqQty;
        }
        return 1;
    }
    ResourceSelectRev(dCanBegDate, bFreeze = false) {
        let Comparer = this.Comparer;
        let ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected == '1');
        if (bFreeze || this.iActQty > 0) {
            for (let i = 0; i < ListRouteRes.length; i++) {
                if (ListRouteRes[i].iResReqQty == 0) {
                    ListRouteRes[i].BScheduled = 1;
                    ListRouteRes[i].cCanScheduled = '0';
                }
            }
            return 1;
        }
        else {
            if (ListRouteRes.length > 0) {
                if (this.SchProductRouteNextList.length == 1 &&
                    this.SchProductRouteNextList[0].cInvCode == this.cInvCode) {
                    const schProductRouteRes = this.SchProductRouteNextList[0].SchProductRouteResList.find((item) => item.BScheduled == 1 &&
                        item.iResReqQty > 0 &&
                        item.resource.cDayPlanShowType != '');
                    if (schProductRouteRes != null) {
                        this.SchProductRouteResList.forEach((item) => {
                            if (item.resource.cDayPlanShowType != '' &&
                                !(schProductRouteRes.resource.cDayPlanShowType.includes(item.resource.cDayPlanShowType) ||
                                    item.resource.cDayPlanShowType.includes(schProductRouteRes.resource.cDayPlanShowType)) &&
                                item.cSelected == '1') {
                                item.cSelected = '0';
                                item.iResReqQty = 0;
                                item.cDefine25 =
                                    '关联分组号不同取消:' +
                                        schProductRouteRes.resource.cDayPlanShowType;
                            }
                        });
                        const ListRouteRes2 = this.SchProductRouteResList.filter((p) => p.cSelected == '1' && p.iResReqQty > 0);
                        if (ListRouteRes2.length < 1) {
                            ListRouteRes[0].cSelected = '1';
                            ListRouteRes[0].iResReqQty = this.iReqQty;
                        }
                    }
                }
            }
        }
        let SchParam = this.SchParam;
        if (this.iSchSdID == SchParam.iSchSdID &&
            this.iProcessProductID == SchParam.iProcessProductID) {
            let j;
        }
        let iResCount = ListRouteRes.length;
        if (iResCount <= 1)
            return 1;
        if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1)
            iResCount = this.iDevCountPd;
        ListRouteRes.sort((p1, p2) => Comparer.Default.Compare(p1.iCapacity, p2.iCapacity));
        let iCapacity = ListRouteRes[0].iCapacity;
        let iMinResCount = 1;
        if (ListRouteRes.length > 1 && this.cDevCountPdExp.length < 1) {
            if (this.schProduct.iWorkQtyPd > 0 &&
                this.schProduct.iWorkQtyPd < this.iReqQty) {
                let iDevCount = (this.schProduct.iWorkQtyPd * iCapacity) /
                    3600 /
                    ListRouteRes[0].resource.iResHoursPd /
                    ListRouteRes[0].resource.iResourceNumber /
                    ListRouteRes[0].iBatchQty;
                if (this.item != null &&
                    this.item.iItemDifficulty > 0 &&
                    this.item.iItemDifficulty != 1)
                    iDevCount = iDevCount * this.item.iItemDifficulty;
                if (ListRouteRes[0].resource.iResDifficulty > 0 &&
                    ListRouteRes[0].resource.iResDifficulty != 1)
                    iDevCount = iDevCount * ListRouteRes[0].resource.iResDifficulty;
                if (this.techInfo.iTechDifficulty > 0 &&
                    this.techInfo.iTechDifficulty != 1)
                    iDevCount = iDevCount * this.techInfo.iTechDifficulty;
                iMinResCount = Math.floor(iDevCount);
                iResCount = iMinResCount;
            }
            else {
                if (SchParam.iTaskMinWorkTime > 0)
                    iMinResCount = Math.floor((this.iReqQty * iCapacity) / 3600 / SchParam.iTaskMinWorkTime);
                else
                    iMinResCount = iResCount;
            }
        }
        if (iResCount > iMinResCount && iMinResCount <= ListRouteRes.length)
            iResCount = iMinResCount;
        if (iResCount < 1 && ListRouteRes.length > 0)
            iResCount = 1;
        let iResReqQty = this.iReqQty;
        let iSelectReturn = 0;
        if (iSelectReturn <= 0) {
            if (ListRouteRes.length > 1) {
                if (SchParam.cMutResourceType == '4') {
                    ListRouteRes.sort((p1, p2) => Comparer.Default.Compare(p1.resource.iPlanDays, p2.resource.iPlanDays));
                }
                else {
                    for (let i = 0; i < ListRouteRes.length; i++) {
                        if (ListRouteRes[i].cCanScheduled != '1')
                            continue;
                        ListRouteRes[i].cDefine38 = new type_1.DateTime(ListRouteRes[i].dResEndDate.getTime() +
                            (ListRouteRes[i].iResGroupPriority - 1) *
                                SchParam.iMutResourceDiffHour *
                                3600000);
                    }
                    const ListRouteResSort = ListRouteRes.sort((c1, c2) => Comparer.Default.Compare(c1.cDefine38, c2.cDefine38) ||
                        Comparer.Default.Compare(c1.iResGroupPriority, c2.iResGroupPriority));
                    ListRouteRes = ListRouteResSort;
                }
            }
            if (ListRouteRes.length > 1) {
                let j = 0;
                for (let i = 0; i < ListRouteRes.length; i++) {
                    if (ListRouteRes[i].cCanScheduled != '1')
                        continue;
                    j++;
                    if (j > iResCount) {
                        ListRouteRes[i].cCanScheduled = '0';
                        ListRouteRes[i].iResReqQty = 0;
                        ListRouteRes[i].iResRationHour = 0;
                    }
                }
            }
        }
        this.ResReqQtyDispatch();
        return 1;
    }
    cBatchResourceSelect() {
        this.cBatchNoFlag = 1;
        const ListBatchRoute = this.schData.SchProductRouteList.filter((p) => p.cVersionNo == this.cVersionNo &&
            p.schProduct.cBatchNo == this.schProduct.cBatchNo &&
            p.schProduct.cWorkRouteType == this.schProduct.cWorkRouteType &&
            p.iWoSeqID == this.iWoSeqID &&
            p.cBatchNoFlag == 0 &&
            p.iSchBatch == this.iSchBatch);
        if (ListBatchRoute.length < 1)
            return 0;
        ListBatchRoute.forEach((batchRoute) => {
            for (let i = 0; i < batchRoute.SchProductRouteResList.length; i++) {
                if (batchRoute.SchProductRouteResList[i].resource.cIsInfinityAbility ==
                    '0') {
                    const BatchRouteRes = this.SchProductRouteResList.find((p) => p.cVersionNo.trim() == batchRoute.cVersionNo &&
                        p.cResourceNo ==
                            batchRoute.SchProductRouteResList[i].cResourceNo &&
                        p.iWoSeqID == batchRoute.iWoSeqID);
                    if (BatchRouteRes != null && BatchRouteRes.iResReqQty > 0) {
                        batchRoute.SchProductRouteResList[i].cSelected = '1';
                        batchRoute.SchProductRouteResList[i].iResReqQty = batchRoute.iReqQty;
                    }
                    else {
                        batchRoute.SchProductRouteResList[i].cSelected = '0';
                        batchRoute.SchProductRouteResList[i].iResReqQty = 0;
                    }
                }
                batchRoute.cBatchNoFlag = 1;
            }
        });
        return 1;
    }
    cBatchSchRoute() {
        const ListBatchRoute = this.schData.SchProductRouteList.filter((p) => p.cVersionNo == this.cVersionNo &&
            p.schProduct.cBatchNo == this.schProduct.cBatchNo &&
            p.schProduct.cWorkRouteType == this.schProduct.cWorkRouteType &&
            p.iProcessID == this.iProcessID &&
            p.iSchSdID != this.iSchSdID &&
            p.cBatchNoFlag != 2);
        if (ListBatchRoute.length < 1)
            return 0;
        if (this.cWoNo == '' && this.SchProductRouteResList.length > 1)
            this.cBatchResourceSelect();
        ListBatchRoute.forEach((batchRoute) => {
            batchRoute.cBatchNoFlag = 2;
            batchRoute.ProcessSchTask();
        });
        return 1;
    }
    ProcessSchTaskPre(bCurTask = true, bFreeze = false) {
        let DateTime = this.DateTime;
        let SchParam = this.SchParam;
        try {
            for (const schProductRoute of this.SchProductRoutePreList) {
                if (schProductRoute.bScheduled === 0) {
                    schProductRoute.ProcessSchTaskPre(bCurTask, bFreeze);
                }
            }
            if (bCurTask) {
                SchParam.ldtBeginDate = DateTime.now();
                this.ProcessSchTask(bFreeze);
                SchParam.iWaitTime = DateTime.now()
                    .diff(SchParam.ldtBeginDate)
                    .as('milliseconds');
            }
        }
        catch (error) {
            if (SchParam.iSchSdID < 1) {
                throw new Error(`订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message}`);
            }
            else {
                throw new Error(`订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message} 明细信息:${error.stack}`);
            }
            return -1;
        }
        return 1;
    }
    ProcessSchTaskNext(cTag = '1') {
        try {
            if (this.bScheduled === 0) {
                this.ProcessSchTask();
            }
            if (this.SchProductRouteNextList.length < 1)
                return 1;
            const schProductRouteNext = this.SchProductRouteNextList[0];
            if (!schProductRouteNext) {
                throw new Error(`订单行号：${this.iSchSdID}请检查产品[${this.cInvCode}]加工物料[${this.cWorkItemNo}]工序号[${this.iWoSeqID.toString()}]工艺路线是否完整!`);
                return -1;
            }
            if (cTag === '1') {
                if (schProductRouteNext.cInvCode === this.cInvCode) {
                    schProductRouteNext.ProcessSchTaskNext('1');
                }
            }
            else if (cTag === '2') {
                if (schProductRouteNext.cInvCode === this.cInvCode &&
                    schProductRouteNext.iWoSeqID < 50) {
                    schProductRouteNext.ProcessSchTaskNext('2');
                }
            }
            else if (cTag === '4') {
                if (schProductRouteNext.bScheduled === 0) {
                    schProductRouteNext.ProcessSchTask();
                }
                else {
                    schProductRouteNext.ProcessSchTaskNext('4');
                }
            }
        }
        catch (error) {
            const SchParam = this.SchParam;
            if (SchParam.iSchSdID < 1) {
                throw new Error(`订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message}`);
            }
            else {
                throw new Error(`订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message} 明细信息:${error.stack}`);
            }
            return -1;
        }
        return 1;
    }
    ProcessSchTaskRevPre(cTag = '1', bSet = '0') {
        if (cTag === '2') {
            if (this.iWoSeqID < 10 && this.bScheduled === 1)
                return 0;
        }
        if (this.iReqQty <= 0) {
            this.bScheduled = 1;
            return 0;
        }
        this.ProcessSchTaskRev(bSet);
        for (const schProductRoute of this.SchProductRoutePreList) {
            if (cTag === '1') {
                if (schProductRoute.cInvCode !== this.cInvCode)
                    continue;
                schProductRoute.ProcessSchTaskRevPre('1');
            }
            else if (cTag === '2') {
                if (schProductRoute.iWoSeqID < 50)
                    continue;
                schProductRoute.ProcessSchTaskRevPre('2');
            }
            else if (cTag === '3') {
                schProductRoute.ProcessSchTaskRevPre('3');
            }
        }
        return 1;
    }
    ProcessSchTaskRev(bSet = '0') {
        const SchParam = this.SchParam;
        let dDateTemp = this.schData.dtStart;
        let dCanEndDate = this.schData.dtStart;
        if (this.iSchSdID === SchParam.iSchSdID &&
            this.iProcessProductID === SchParam.iProcessProductID) {
            let j;
        }
        try {
            if (this.SchProductRouteNextList.length > 0) {
                for (const schProductRoute of this.SchProductRouteNextList) {
                    dDateTemp = schProductRoute.GetPreProcessCanEndDate(this);
                    if (dCanEndDate < dDateTemp)
                        dCanEndDate = dDateTemp;
                }
                if (bSet === '1') {
                    if (dCanEndDate <= this.dEndDate) {
                        this.ProcessSchTask();
                        return 1;
                    }
                }
                if (dCanEndDate < this.schData.dtStart) {
                    throw new Error(`订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：${this.iSchSdID}工序ID号：${this.iProcessProductID}\n\r 倒排开始时间${dCanEndDate}小于排产开始日期${this.schData.dtStart},无法继续倒排!`);
                    return -1;
                }
            }
            else {
                dCanEndDate = this.schProduct.dDeliveryDate;
                if (dCanEndDate <= this.schData.dtStart) {
                    //@ts-ignore
                    dCanEndDate = this.schData.dtStart.plus({ months: 1 });
                }
            }
            if (this.bScheduled === 1)
                this.ProcessClearTask();
            if (this.iProcessProductID === SchParam.iProcessProductID &&
                this.iSchSdID === SchParam.iSchSdID) {
                let i = 1;
            }
            this.ResourceSelectRev(dCanEndDate);
            const ListRouteRes2 = this.SchProductRouteResList.filter((p) => p.cSelected === '1' && p.cCanScheduled === '1');
            const iResCount = ListRouteRes2.length;
            if (iResCount < 1) {
                const ListRouteResCan = this.SchProductRouteResList.filter((p) => p.cSelected === '1' && p.cCanScheduled === '0');
                if (ListRouteResCan.length < 1) {
                    throw new Error(`订单行号：${this.iSchSdID}资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：${this.iProcessProductID}\n\r 没有可排产资源编号.或排产范围太小,最早可排开始日期：'${dCanEndDate.toLocaleString(this.DateTime.DATE_FULL)}'`);
                    return -1;
                }
                else {
                    ListRouteResCan.forEach((item) => {
                        item.BScheduled = 1;
                    });
                    return 1;
                }
            }
            const ListRouteRes = this.SchProductRouteResList.filter((p) => p.cSelected === '1' && p.iResReqQty > 0);
            let SchData = this.schData;
            for (const LSchProductRouteRes of ListRouteRes) {
                LSchProductRouteRes.TaskSchTaskRev(LSchProductRouteRes.iResReqQty, dCanEndDate);
                if (this.SchProductRouteNextList &&
                    this.SchProductRouteNextList.length > 0) {
                    if (LSchProductRouteRes.dResBegDate
                        //@ts-ignore
                        .diff(this.SchProductRouteNextList[0].dBegDate)
                        .as('hours') > 0) {
                        const iDiffMinites = SchData.GetDateDiff('m', this.SchProductRouteNextList[0].dBegDate, LSchProductRouteRes.dResBegDate);
                        if (iDiffMinites > 0) {
                            //@ts-ignore
                            dCanEndDate = LSchProductRouteRes.dResEndDate.minus({
                                minutes: iDiffMinites,
                            });
                            LSchProductRouteRes.TaskClearTask();
                            try {
                                LSchProductRouteRes.TaskSchTaskRev(LSchProductRouteRes.iResReqQty, dCanEndDate);
                            }
                            catch (error) {
                                if (SchParam.iSchSdID < 1) {
                                    throw new Error(`订单行号：${this.iSchSdID}倒排时，资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：${this.iProcessProductID}\n\r ${error.message}`);
                                }
                                else {
                                    throw new Error(`订单行号：${this.iSchSdID}倒排时，资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：${this.iProcessProductID}\n\r ${error.message} 明细信息:${error.stack}`);
                                }
                                return -1;
                            }
                        }
                    }
                }
            }
            const list1 = this.SchProductRouteResList.filter((p1) => p1.iSchSdID === this.iSchSdID &&
                p1.iProcessProductID === this.iProcessProductID &&
                p1.iResReqQty > 0);
            if (list1.length > 0) {
                list1.sort((p1, p2) => 
                //@ts-ignore
                p1.dResBegDate.diff(p2.dResBegDate).as('milliseconds'));
                this.dBegDate = list1[0].dResBegDate;
                this.dEndDate = list1[list1.length - 1].dResEndDate;
            }
            let iLaborTime = 0;
            for (const lSchProductRouteRes of list1) {
                iLaborTime += lSchProductRouteRes.iResRationHour;
            }
            this.iLaborTime = iLaborTime;
            this.BScheduled = 1;
        }
        catch (error) {
            throw new Error(`订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：${this.iSchSdID}工序ID号：${this.iProcessProductID}\n\r ${error.message}`);
            return -1;
        }
        return 1;
    }
    GetNextProcessCanBegDate(schProductRouteNext) {
        let ldtFirstEndDate = this.dBegDate;
        let cWorkType = '0';
        let liCapacity = 1;
        let iBatchQty = 1;
        let SchParam = this.SchParam;
        try {
            if (this.cStatus === '4' || this.iActQty > 0 || this.iReqQty === 0) {
                if (this.cVersionNo !== 'SureVersion') {
                    //@ts-ignore
                    return SchParam.dtStart.plus({ days: SchParam.dEarliestSchDateDays });
                }
                else {
                    return SchParam.dtStart;
                }
            }
            if (this.SchProductRouteResList.length > 0) {
                liCapacity = this.SchProductRouteResList[0].iCapacity;
                cWorkType = this.SchProductRouteResList[0].cWorkType;
                iBatchQty = this.SchProductRouteResList[0].iBatchQty;
            }
            if (this.cParellelType === 'ES') {
                if (SchParam.NextSeqBegTime > 0) {
                    ldtFirstEndDate = this.dBegDate.plus({
                        minutes: this.iMoveTime + this.iSeqPostTime + SchParam.NextSeqBegTime,
                    });
                    if (ldtFirstEndDate >
                        this.dEndDate.plus({ minutes: this.iMoveTime + this.iSeqPostTime })) {
                        ldtFirstEndDate = this.dEndDate.plus({
                            minutes: this.iMoveTime + this.iSeqPostTime,
                        });
                    }
                }
                else {
                    ldtFirstEndDate = this.dEndDate.plus({
                        minutes: this.iMoveTime + this.iSeqPostTime,
                    });
                }
            }
            else if (this.cParellelType === 'EE') {
                if (schProductRouteNext.BScheduled === 1) {
                    if (this.cMoveType === '1') {
                        ldtFirstEndDate = schProductRouteNext.dEndDate.minus({
                            minutes: this.iMoveInterTime + this.iMoveTime + this.iSeqPostTime,
                        });
                    }
                    else if (this.cMoveType === '2') {
                        if (cWorkType === '1') {
                            ldtFirstEndDate = schProductRouteNext.dEndDate.minus({
                                minutes: (this.iMoveInterQty * liCapacity) / 60 / iBatchQty +
                                    this.iMoveTime +
                                    this.iSeqPostTime,
                            });
                        }
                        else {
                            ldtFirstEndDate = schProductRouteNext.dEndDate.minus({
                                minutes: (this.iMoveInterQty * liCapacity) / 60 +
                                    this.iMoveTime +
                                    this.iSeqPostTime,
                            });
                        }
                    }
                }
                else {
                    if (this.cMoveType === '1') {
                        ldtFirstEndDate = this.dBegDate.plus({
                            minutes: this.iMoveInterTime + this.iMoveTime + this.iSeqPostTime,
                        });
                    }
                    else if (this.cMoveType === '2') {
                        if (cWorkType === '1') {
                            ldtFirstEndDate = this.dBegDate.plus({
                                minutes: (this.iMoveInterQty * liCapacity) / 60 / iBatchQty +
                                    this.iMoveTime +
                                    this.iSeqPostTime,
                            });
                        }
                        else {
                            ldtFirstEndDate = this.dBegDate.plus({
                                minutes: (this.iMoveInterQty * liCapacity) / 60 +
                                    this.iMoveTime +
                                    this.iSeqPostTime,
                            });
                        }
                    }
                }
            }
            else {
                if (this.cMoveType === '1') {
                    ldtFirstEndDate = this.dBegDate.plus({
                        minutes: this.iMoveInterTime + this.iMoveTime + this.iSeqPostTime,
                    });
                }
                else if (this.cMoveType === '2') {
                    if (cWorkType === '1') {
                        ldtFirstEndDate = this.dBegDate.plus({
                            minutes: (this.iMoveInterQty * liCapacity) / 60 / iBatchQty +
                                this.iMoveTime +
                                this.iSeqPostTime,
                        });
                    }
                    else {
                        ldtFirstEndDate = this.dBegDate.plus({
                            minutes: (this.iMoveInterQty * liCapacity) / 60 +
                                this.iMoveTime +
                                this.iSeqPostTime,
                        });
                    }
                }
            }
            if (schProductRouteNext) {
                ldtFirstEndDate = ldtFirstEndDate.plus({
                    minutes: schProductRouteNext.iSeqPreTime,
                });
            }
        }
        catch (error) {
            throw new Error(`订单行号：${this.iSchSdID}资源计算时出错,位置SchProductRoute.GetNextProcessCanBegDate！订单行号：${this.iSchSdID}工序ID号：${this.iProcessProductID}\n\r ${error.message} ${error.stack}`);
        }
        return ldtFirstEndDate;
    }
    GetProcessMoveTime(schProductRoutePre) {
        let iMoveTime = 0;
        let liCapacity = 1;
        if (schProductRoutePre.SchProductRouteResList.length > 0) {
            liCapacity = schProductRoutePre.SchProductRouteResList[0].iCapacity;
        }
        if (schProductRoutePre.cMoveType === '1') {
            iMoveTime =
                schProductRoutePre.iMoveInterTime + schProductRoutePre.iMoveTime;
        }
        else if (schProductRoutePre.cMoveType === '2') {
            iMoveTime =
                (schProductRoutePre.iMoveInterQty * liCapacity) / 60 +
                    schProductRoutePre.iMoveTime;
        }
        else {
            iMoveTime = schProductRoutePre.iMoveTime;
        }
        return iMoveTime;
    }
    GetProcessMoveQty(schProductRoutePre) {
        let iMoveTime = 0;
        let liCapacity = 1;
        let ibatchqty = 1;
        if (schProductRoutePre.SchProductRouteResList.length > 0) {
            liCapacity = schProductRoutePre.SchProductRouteResList[0].iCapacity;
            ibatchqty = schProductRoutePre.SchProductRouteResList[0].iBatchQty;
            if (liCapacity === 0)
                liCapacity = 1;
            if (schProductRoutePre.cMoveType === '1') {
                iMoveTime = schProductRoutePre.iMoveInterTime;
            }
            else if (schProductRoutePre.cMoveType === '2') {
                iMoveTime =
                    (schProductRoutePre.iMoveInterQty * liCapacity) / ibatchqty / 60;
            }
            else {
                iMoveTime = 0;
            }
        }
        return iMoveTime;
    }
    GetPreProcessCanEndDate(schProductRoutePre, cType = '1') {
        let ldtFirstEndDate;
        const iMoveTime = this.GetProcessMoveQty(schProductRoutePre);
        ldtFirstEndDate = this.dEndDate.minus({
            minutes: this.iMoveTime + this.iSeqPreTime + iMoveTime,
        });
        if (schProductRoutePre && schProductRoutePre.iSeqPostTime) {
            ldtFirstEndDate = ldtFirstEndDate.minus({
                minutes: schProductRoutePre.iSeqPostTime,
            });
            schProductRoutePre.cDefine28 = `;本工序后准备时间${schProductRoutePre.iSeqPostTime}`;
            schProductRoutePre.cDefine28 += `;本工序可开工时间${ldtFirstEndDate}`;
        }
        return ldtFirstEndDate;
    }
    ProcessClearTask() {
        for (const schProductRouteRes of this.SchProductRouteResList) {
            schProductRouteRes.TaskClearTask();
        }
        this.BScheduled = 0;
    }
    GetRouteEarlyBegDate() {
        const SchParam = this.SchParam;
        let dDateTemp = this.schData.dtStart;
        try {
            if (!this.schProduct)
                return;
            if (this.iSchSdID === SchParam.iSchSdID) {
                let j = 1;
            }
            if (this.iSchSdID === SchParam.iSchSdID &&
                this.iProcessProductID === SchParam.iProcessProductID) {
                let j = 1;
            }
            this.cDefine27 = `dEarlyBegDate:${this.dEarlyBegDate}`;
            if (this.SchProductRoutePreList &&
                this.SchProductRoutePreList.length > 0) {
                for (const schProductRoutePre of this.SchProductRoutePreList) {
                    if (schProductRoutePre.bScheduled === 0) {
                        schProductRoutePre.ProcessSchTask();
                    }
                    dDateTemp = schProductRoutePre.GetNextProcessCanBegDate(this);
                    if (this.cStatus !== '4' && this.dEarlyBegDate < dDateTemp) {
                        this.dEarlyBegDate = dDateTemp;
                        this.cDefine27 += `;前工序${schProductRoutePre.iProcessProductID}:${dDateTemp}`;
                    }
                }
            }
            else if (this.iSeqPreTime > 0 && this.cStatus !== '2') {
                //@ts-ignore
                dDateTemp = SchParam.dtStart.plus({ minutes: this.iSeqPreTime });
                if (this.dEarlyBegDate < dDateTemp) {
                    this.dEarlyBegDate = dDateTemp;
                    this.cDefine27 += `;本工序前准备时间${this.iSeqPreTime}:${dDateTemp}`;
                }
            }
            if (SchParam.cPurEndDate === '1' && this.cVersionNo !== 'SureVersion') {
                if (this.techInfo &&
                    (this.techInfo.iOrder === 0 || this.techInfo.iOrder === 3)) {
                    let dPurEarliestSchDate = this.schData.dtStart;
                    const SchProductRouteItemListPur = this.SchProductRouteItemList.filter((p) => p.bSelf !== '1' &&
                        p.cInvCode === this.cWorkItemNo &&
                        p.cInvCodeFull === this.cWorkItemNoFull);
                    if (SchProductRouteItemListPur &&
                        SchProductRouteItemListPur.length > 0) {
                        for (const schProductRouteItem of SchProductRouteItemListPur) {
                            if (schProductRouteItem.iReqQty > 0 &&
                                schProductRouteItem.dEarlySubItemDate > dPurEarliestSchDate) {
                                dPurEarliestSchDate = schProductRouteItem.dEarlySubItemDate;
                            }
                        }
                        if (this.dEarlyBegDate < dPurEarliestSchDate) {
                            this.dEarlyBegDate = dPurEarliestSchDate;
                            this.cDefine27 += `;采购件最晚到料日期:${dPurEarliestSchDate}`;
                        }
                    }
                }
            }
            if (this.item && this.item.iAdvanceDate > 0) {
                //@ts-ignore
                dDateTemp = this.schProduct.dEarliestSchDate.plus({
                    days: this.item.iAdvanceDate,
                });
                if (this.dEarlyBegDate < dDateTemp) {
                    this.dEarlyBegDate = dDateTemp;
                    this.cDefine27 += `;产品采购提前期${this.item.iAdvanceDate}:${dDateTemp}`;
                }
            }
        }
        catch (error) {
            throw new Error(`订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：${this.iSchSdID}工序ID号：${this.iProcessProductID}\n\r ${error.message} ${error.stack}`);
        }
    } //
}
exports.SchProductRoute = SchProductRoute;
