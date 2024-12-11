//mport { DataTable } from 'some-datatable-library'; // Replace with actual DataTable library
//import { Logger_No } from 'some-logger-library'; // Replace with actual Logger library
import { DataTable, DateTime } from './type'; //
export class SchParam {
	public UseAPS: string = '1'; // 1 使用; 0 不使用  2 不使用APS系统，直接发放生成生产任务单 3 使用APS， 接ERP系统生产任务单排产
	public SchType: string = '1'; // 排程运算方式
	public ResProcessCharCount: number = 4; // 资源使用工艺特征总数4定义资源档案中，每个资源的最大使用工艺特数
	public TaskSelectRes: number = 1; // 任务选择机台原则
	public PeriodLeftTime: number = 20; // 每个工作时间段剩余多少分种不安排任务
	public TaskSchNotBreak: number = 1; // 排程任务不能中断
	public DiffMaxTime: number = 8; // 需配套生产最大相差时间
	public SetMinDelayTime: number = 24; // 配套最少延期时间
	public AllResiEfficient: number = 100; // 所有资源利用率%
	public iProcessProductID: number = 9401; // 用于调试
	public dtParamValue: DataTable = new DataTable(); // 系统参数表
	public iSchSdID: number = 191; // 用于调试
	public iSchSNMax: number = 1; // 排产顺序号
	public iSchSNMin: number = -1; // 最小已排的任务排产顺序
	public iSchWoSNMax: number = 1; // 最大工单排产任务数
	public dtResLastSchTime: DateTime; // 记录任务上次排产完成时间
	public cDayPlanMove: string = '0'; // 1 排程调度优化计算
	public NextSeqBegTime: number = 120; // 后工序可开工时间为前工序开工后多长时间(分钟)
	public ReSchWo: string = '0'; // 重排已下达任务开工时间
	public cCustomer: string = ''; // 客户编号，用于特殊处理
	public ExecTaskSchType: string = '1'; // 已执行任务排产处理方式
	public ExecTaskSort: string = '1'; // 已执行任务排产方式
	public PreSeqEndDate: string = '1'; // 考虑前工序完工时间约束
	public cSelfEndDate: string = '0'; // 1 排产考虑半成品完工时间; 0 不考虑
	public cPurEndDate: string = '0'; // 1 排产考虑采购件采购提前期; 0 不考虑
	public bReSchedule: string = '0'; // 1 重排; 0 第一次排
	public bSchKeyBySN: string = '0'; // 1 关键资源排产不能穿插
	public NextProductBegTime: number = 0; // 后序产品为前半成品开工后多长时间(分钟)可排产
	public cVersionNo: string = '';
	public dtStart: DateTime; // 开始排程时间
	public dtEnd: DateTime; // 排程截止时间
	public dtToday: DateTime; // 当前时间，取数据库
	public iTaskMinWorkTime: number = 480; // 单资源超过多长时间自动分配到多机台
	public cMutResourceType: string = '1'; // 多资源选择规则
	public iMutResourceDiffHour: number = 48; // 按资源组排产优先级选择资源时
	public SchTaskRevTag: string = '3'; // 倒排方式
	public cSchRunType: string = '1'; // 排程算法策略
	public cSchCapType: string = '0'; // 排程产能方案
	public cSchType: string = '0'; // 排程方式
	public cTechSchType: string = '0'; // 工艺段排产方式
	public cProChaType1Sort: string = '0'; // 排产优化顺序定义
	public iPreTaskRev: number = 24; // 后工序完工时间比前工序大多少小时前工序倒排
	public ResSelectLastTaskDays: number; // 多资源排产优先选择最近多少天内生产过的资源
	public iSchThread: number = 5; // 多线程排程
	public ldtBeginDate: DateTime; // 开始时间
	public iWaitTime: number; // 运行时间
	public iWaitTime2: number; // 运行时间
	public iWaitTime3: number; // 运行时间
	public iWaitTime4: number; // 运行时间
	public iWaitTime5: number; // 运行时间
	public APSDebug: string; // APS调试模式
	public dEarliestSchDateDays: number; // 最早可排日期天数
	public dLastBegDateBeforeDays: number; // 重排时工序第一次计划开工日期前最早可提前多少天
	public iCurSchRunTimes: string; // 当前排程次数

	public GetSchParams(): void {
		let SchParam = this;
		SchParam.UseAPS = SchParam.GetParam('UseAPS');
		SchParam.ResProcessCharCount = parseInt(SchParam.GetParam('ResProcessCharCount'));
		SchParam.TaskSelectRes = parseInt(SchParam.GetParam('TaskSelectRes'));
		SchParam.PeriodLeftTime = parseInt(SchParam.GetParam('PeriodLeftTime')) * 60;
		SchParam.TaskSchNotBreak = parseInt(SchParam.GetParam('TaskSchNotBreak'));
		SchParam.DiffMaxTime = parseInt(SchParam.GetParam('DiffMaxTime'));
		SchParam.SetMinDelayTime = parseInt(SchParam.GetParam('SetMinDelayTime'));
		SchParam.AllResiEfficient = parseFloat(SchParam.GetParam('AllResiEfficient'));
		if (SchParam.AllResiEfficient <= 0.01 || SchParam.AllResiEfficient > 100) SchParam.AllResiEfficient = 100;
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
		if (sNextProductBegTime === '') sNextProductBegTime = '240';
		SchParam.NextProductBegTime = parseInt(sNextProductBegTime);
		SchParam.bSchKeyBySN = SchParam.GetParam('bSchKeyBySN');
		SchParam.cSchRunType = SchParam.GetParam('cSchRunType');
		if (SchParam.cSchRunType === '') SchParam.cSchRunType = '1';
		SchParam.cSchCapType = SchParam.GetParam('cSchCapType');
		if (SchParam.cSchCapType === '') SchParam.cSchCapType = '0';
		let sTaskMinWorkTime = SchParam.GetParam('iTaskMinWorkTime');
		if (sTaskMinWorkTime === '') sTaskMinWorkTime = '480';
		SchParam.iTaskMinWorkTime = parseInt(sTaskMinWorkTime);
		let sMutResourceDiffHour = SchParam.GetParam('iMutResourceDiffHour');
		if (sMutResourceDiffHour === '') sMutResourceDiffHour = '48';
		SchParam.iMutResourceDiffHour = parseInt(sMutResourceDiffHour);
		let sPreTaskRev = SchParam.GetParam('PreTaskRev');
		if (sPreTaskRev === '') sPreTaskRev = '24';
		SchParam.iPreTaskRev = parseFloat(sPreTaskRev);
		SchParam.cMutResourceType = SchParam.GetParam('cMutResourceType');
		if (SchParam.cMutResourceType === '') SchParam.cMutResourceType = '1';
		let cResSelectLastTaskDays = SchParam.GetParam('cResSelectLastTaskDays');
		if (cResSelectLastTaskDays === '') cResSelectLastTaskDays = '7';
		SchParam.ResSelectLastTaskDays = parseInt(cResSelectLastTaskDays);
		SchParam.APSDebug = SchParam.GetParam('APSDebug');
		if (SchParam.APSDebug === '') SchParam.APSDebug = '0';
		SchParam.cProChaType1Sort = SchParam.GetParam('cProChaType1Sort');
		if (SchParam.cProChaType1Sort === '') SchParam.cProChaType1Sort = '0';
		let cdEarliestSchDateDays = SchParam.GetParam('dEarliestSchDateDays');
		if (cdEarliestSchDateDays === '') cdEarliestSchDateDays = '1';
		SchParam.dEarliestSchDateDays = parseInt(cdEarliestSchDateDays);
		let cdLastBegDateBeforeDays = SchParam.GetParam('dLastBegDateBeforeDays');
		if (cdLastBegDateBeforeDays === '') cdLastBegDateBeforeDays = '1';
		SchParam.dLastBegDateBeforeDays = parseInt(cdLastBegDateBeforeDays);
		SchParam.iSchSNMax = 1;
		SchParam.iSchWoSNMax = 1; // 最大工单排产任务数
		try {
			SchParam.Debug('排程运算开始', '参数准备');
		} catch (exp) {
			// Handle exception
		}
	}//
	public  GetParam(cParamNo: string) {
		let SchParam = this;
		let drParam =  SchParam.dtParamValue.Select(`cParamNo = '${cParamNo}'`);
		let cValue: string = ''; //
		if (drParam.length > 0) {
			cValue = drParam[0]['cValue'].toString();
		}
		return cValue;
	}

	public objLogger: any; //Logger_No; // = new Logger(); // 日志处理对象

	public Debug(message: string, form: string, Event: string = ''): void {
		const parameters: any[] = [message, form, Event];
		try {
			// Debug logic
		} catch (exp) {
			const error = exp.message;
		}
	}

	public Error(message: string, form: string, Event: string = ''): void {
		const parameters: any[] = [message, form, Event];
		try {
			// Error logic
		} catch (exp) {
			const error = exp.message;
		}
	}
}
