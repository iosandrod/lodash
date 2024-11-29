export class _SchParam {
  UseAPS: string = '1' // 1 使用; 0 不使用  2 不使用APS系统，直接发放生成生产任务单 3 使用APS，接ERP系统生产任务单排产
  SchType: string = '1' // 排程运算方式 0 按订单排产 1 单独调度优化排产
  ResProcessCharCount: number = 4 // 资源使用工艺特征总数
  TaskSelectRes: number = 1 // 任务选择机台原则
  PeriodLeftTime: number = 20 // 每个工作时间段剩余多少分钟不安排任务
  TaskSchNotBreak: number = 1 // 排程任务不能中断
  DiffMaxTime: number = 8 // 配套生产最大相差时间
  SetMinDelayTime: number = 24 // 配套最少延期时间
  AllResiEfficient: number = 100 // 所有资源利用率百分比
  iProcessProductID: number = 9401 // 调试工序任务ID
  dtParamValue: Array<any> = [] // 参数表，模拟DataTable
  iSchSdID: number = 191 // 调试用ID
  iSchSNMax: number = 1 // 排产顺序号
  iSchSNMin: number = -1 // 最小已排任务顺序
  iSchWoSNMax: number = 1 // 最大工单排产任务数
  dtResLastSchTime: Date // 记录任务上次排产完成时间
  cDayPlanMove: string = '0' // 排程调度优化计算
  NextSeqBegTime: number = 120 // 后工序可开工时间
  ReSchWo: string = '0' // 重排已下达任务开工时间
  cCustomer: string = '' // 客户编号
  ExecTaskSchType: string = '1' // 已执行任务排产处理方式
  ExecTaskSort: string = '1' // 已执行任务排产方式
  PreSeqEndDate: string = '1' // 考虑前工序完工时间约束
  cSelfEndDate: string = '0' // 半成品完工时间
  cPurEndDate: string = '0' // 采购件提前期
  bReSchedule: string = '0' // 是否重排
  bSchKeyBySN: string = '0' // 关键资源排产不能穿插
  NextProductBegTime: number = 0 // 后序产品可排产时间
  cVersionNo: string = ''
  dtStart: Date
  dtEnd: Date
  dtToday: Date
  iTaskMinWorkTime: number = 480 // 单资源最小工作时间
  cMutResourceType: string = '1' // 多资源选择规则
  iMutResourceDiffHour: number = 48 // 资源组排产优先级
  SchTaskRevTag: string = '3' // 倒排方式
  cSchRunType: string = '1' // 排程算法策略
  cSchCapType: string = '0' // 排程产能方案
  cSchType: string = '0' // 排程方式
  cTechSchType: string = '0' // 工艺段排产方式
  cProChaType1Sort: string = '0' // 排产优化顺序
  iPreTaskRev: number = 24 // 前工序倒排时间
  ResSelectLastTaskDays: number = 7 // 多资源排产优先选择最近天数
  iSchThread: number = 5 // 多线程排程
  ldtBeginDate: Date
  iWaitTime: number = 0
  iWaitTime2: number = 0
  iWaitTime3: number = 0
  iWaitTime4: number = 0
  iWaitTime5: number = 0
  APSDebug: string = '' // APS调试模式
  dEarliestSchDateDays: number = 7 // 最早可排日期天数
  dLastBegDateBeforeDays: number = 0 // 重排最早提前天数
  iCurSchRunTimes: string = '' // 当前排程次数

  GetSchParams(): void {
    // 模拟获取参数的函数，用于从外部配置读取参数
    function GetParam(paramName: string): any {
      // 假定从外部配置文件或数据库中读取参数
      return 'default_value' // 返回默认值
    }
	let SchParam=this
    SchParam.UseAPS = GetParam('UseAPS')
    SchParam.ResProcessCharCount = parseInt(GetParam('ResProcessCharCount'))
    SchParam.TaskSelectRes = parseInt(GetParam('TaskSelectRes'))
    SchParam.PeriodLeftTime = parseInt(GetParam('PeriodLeftTime')) * 60
    SchParam.TaskSchNotBreak = parseInt(GetParam('TaskSchNotBreak'))
    SchParam.DiffMaxTime = parseInt(GetParam('DiffMaxTime'))
    SchParam.SetMinDelayTime = parseInt(GetParam('SetMinDelayTime'))
    SchParam.AllResiEfficient = parseFloat(GetParam('AllResiEfficient'))
  }
}

export const SchParam = new _SchParam() //
