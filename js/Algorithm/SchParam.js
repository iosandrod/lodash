"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SchParam = void 0;
//mport { DataTable } from 'some-datatable-library'; // Replace with actual DataTable library
//import { Logger_No } from 'some-logger-library'; // Replace with actual Logger library
const type_1 = require("./type"); //
class SchParam {
    constructor() {
        this.UseAPS = '1'; // 1 使用; 0 不使用  2 不使用APS系统，直接发放生成生产任务单 3 使用APS， 接ERP系统生产任务单排产
        this.SchType = '1'; // 排程运算方式
        this.ResProcessCharCount = 4; // 资源使用工艺特征总数4定义资源档案中，每个资源的最大使用工艺特数
        this.TaskSelectRes = 1; // 任务选择机台原则
        this.PeriodLeftTime = 20; // 每个工作时间段剩余多少分种不安排任务
        this.TaskSchNotBreak = 1; // 排程任务不能中断
        this.DiffMaxTime = 8; // 需配套生产最大相差时间
        this.SetMinDelayTime = 24; // 配套最少延期时间
        this.AllResiEfficient = 100; // 所有资源利用率%
        this.iProcessProductID = 9401; // 用于调试
        this.dtParamValue = new type_1.DataTable(); // 系统参数表
        this.iSchSdID = 191; // 用于调试
        this.iSchSNMax = 1; // 排产顺序号
        this.iSchSNMin = -1; // 最小已排的任务排产顺序
        this.iSchWoSNMax = 1; // 最大工单排产任务数
        this.cDayPlanMove = '0'; // 1 排程调度优化计算
        this.NextSeqBegTime = 120; // 后工序可开工时间为前工序开工后多长时间(分钟)
        this.ReSchWo = '0'; // 重排已下达任务开工时间
        this.cCustomer = ''; // 客户编号，用于特殊处理
        this.ExecTaskSchType = '1'; // 已执行任务排产处理方式
        this.ExecTaskSort = '1'; // 已执行任务排产方式
        this.PreSeqEndDate = '1'; // 考虑前工序完工时间约束
        this.cSelfEndDate = '0'; // 1 排产考虑半成品完工时间; 0 不考虑
        this.cPurEndDate = '0'; // 1 排产考虑采购件采购提前期; 0 不考虑
        this.bReSchedule = '0'; // 1 重排; 0 第一次排
        this.bSchKeyBySN = '0'; // 1 关键资源排产不能穿插
        this.NextProductBegTime = 0; // 后序产品为前半成品开工后多长时间(分钟)可排产
        this.cVersionNo = '';
        this.iTaskMinWorkTime = 480; // 单资源超过多长时间自动分配到多机台
        this.cMutResourceType = '1'; // 多资源选择规则
        this.iMutResourceDiffHour = 48; // 按资源组排产优先级选择资源时
        this.SchTaskRevTag = '3'; // 倒排方式
        this.cSchRunType = '1'; // 排程算法策略
        this.cSchCapType = '0'; // 排程产能方案
        this.cSchType = '0'; // 排程方式
        this.cTechSchType = '0'; // 工艺段排产方式
        this.cProChaType1Sort = '0'; // 排产优化顺序定义
        this.iPreTaskRev = 24; // 后工序完工时间比前工序大多少小时前工序倒排
        this.iSchThread = 5; // 多线程排程
    }
    GetSchParams() {
        let SchParam = this;
        SchParam.UseAPS = SchParam.GetParam('UseAPS');
        SchParam.ResProcessCharCount = parseInt(SchParam.GetParam('ResProcessCharCount'));
        SchParam.TaskSelectRes = parseInt(SchParam.GetParam('TaskSelectRes'));
        SchParam.PeriodLeftTime = parseInt(SchParam.GetParam('PeriodLeftTime')) * 60;
        SchParam.TaskSchNotBreak = parseInt(SchParam.GetParam('TaskSchNotBreak'));
        SchParam.DiffMaxTime = parseInt(SchParam.GetParam('DiffMaxTime'));
        SchParam.SetMinDelayTime = parseInt(SchParam.GetParam('SetMinDelayTime'));
        SchParam.AllResiEfficient = parseFloat(SchParam.GetParam('AllResiEfficient'));
        if (SchParam.AllResiEfficient <= 0.01 || SchParam.AllResiEfficient > 100)
            SchParam.AllResiEfficient = 100;
        SchParam.iProcessProductID = parseInt(SchParam.GetParam('iProcessProductID'));
        SchParam.iSchSdID = parseInt(SchParam.GetParam('iSchSdID'));
        SchParam.cDayPlanMove = SchParam.GetParam('cDayPlanMove');
        SchParam.NextSeqBegTime = parseInt(SchParam.GetParam('NextSeqBegTime'));
        SchParam.ReSchWo = SchParam.GetParam('ReSchWo');
        SchParam.cCustomer = SchParam.GetParam('cCustomer');
        SchParam.ExecTaskSchType = SchParam.GetParam('ExecTaskSchType');
        SchParam.ExecTaskSort = SchParam.GetParam('ExecTaskSort');
        SchParam.cSelfEndDate = SchParam.GetParam('cSelfEndDate');
        SchParam.cPurEndDate = SchParam.GetParam('cPurEndDate');
        let sNextProductBegTime = SchParam.GetParam('NextProductBegTime');
        if (sNextProductBegTime === '')
            sNextProductBegTime = '240';
        SchParam.NextProductBegTime = parseInt(sNextProductBegTime);
        SchParam.bSchKeyBySN = SchParam.GetParam('bSchKeyBySN');
        SchParam.cSchRunType = SchParam.GetParam('cSchRunType');
        if (SchParam.cSchRunType === '')
            SchParam.cSchRunType = '1';
        SchParam.cSchCapType = SchParam.GetParam('cSchCapType');
        if (SchParam.cSchCapType === '')
            SchParam.cSchCapType = '0';
        let sTaskMinWorkTime = SchParam.GetParam('iTaskMinWorkTime');
        if (sTaskMinWorkTime === '')
            sTaskMinWorkTime = '480';
        SchParam.iTaskMinWorkTime = parseInt(sTaskMinWorkTime);
        let sMutResourceDiffHour = SchParam.GetParam('iMutResourceDiffHour');
        if (sMutResourceDiffHour === '')
            sMutResourceDiffHour = '48';
        SchParam.iMutResourceDiffHour = parseInt(sMutResourceDiffHour);
        let sPreTaskRev = SchParam.GetParam('PreTaskRev');
        if (sPreTaskRev === '')
            sPreTaskRev = '24';
        SchParam.iPreTaskRev = parseFloat(sPreTaskRev);
        SchParam.cMutResourceType = SchParam.GetParam('cMutResourceType');
        if (SchParam.cMutResourceType === '')
            SchParam.cMutResourceType = '1';
        let cResSelectLastTaskDays = SchParam.GetParam('cResSelectLastTaskDays');
        if (cResSelectLastTaskDays === '')
            cResSelectLastTaskDays = '7';
        SchParam.ResSelectLastTaskDays = parseInt(cResSelectLastTaskDays);
        SchParam.APSDebug = SchParam.GetParam('APSDebug');
        if (SchParam.APSDebug === '')
            SchParam.APSDebug = '0';
        SchParam.cProChaType1Sort = SchParam.GetParam('cProChaType1Sort');
        if (SchParam.cProChaType1Sort === '')
            SchParam.cProChaType1Sort = '0';
        let cdEarliestSchDateDays = SchParam.GetParam('dEarliestSchDateDays');
        if (cdEarliestSchDateDays === '')
            cdEarliestSchDateDays = '1';
        SchParam.dEarliestSchDateDays = parseInt(cdEarliestSchDateDays);
        let cdLastBegDateBeforeDays = SchParam.GetParam('dLastBegDateBeforeDays');
        if (cdLastBegDateBeforeDays === '')
            cdLastBegDateBeforeDays = '1';
        SchParam.dLastBegDateBeforeDays = parseInt(cdLastBegDateBeforeDays);
        SchParam.iSchSNMax = 1;
        SchParam.iSchWoSNMax = 1; // 最大工单排产任务数
        try {
            SchParam.Debug('排程运算开始', '参数准备');
        }
        catch (exp) {
            // Handle exception
        }
    } //
    GetParam(cParamNo) {
        let SchParam = this;
        let drParam = SchParam.dtParamValue.Select(`cParamNo = '${cParamNo}'`);
        let cValue = ''; //
        if (drParam.length > 0) {
            cValue = drParam[0]['cValue'].toString();
        }
        return cValue;
    }
    Debug(message, form, Event = '') {
        const parameters = [message, form, Event];
        try {
            // Debug logic
        }
        catch (exp) {
            const error = exp.message;
        }
    }
    Error(message, form, Event = '') {
        const parameters = [message, form, Event];
        try {
            // Error logic
        }
        catch (exp) {
            const error = exp.message;
        }
    }
}
exports.SchParam = SchParam;
