using System;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
namespace Algorithm
{    
    public static class SchParam 
    {
        public static string UseAPS = "1";        //1 使用; 0 不使用  2 不使用APS系统，直接发放生成生产任务单 3 使用APS， 接ERP系统生产任务单排产
        public static string SchType = "1";       //排程运算方式 //0 按订单排产  //1 单独调度优化排产//2--按工单优先级调度优化排产（正式版本） //3 --按资源调度优化排产（正式版本）
        public static int ResProcessCharCount = 4; //资源使用工艺特征总数4定义资源档案中，每个资源的最大使用工艺特数
        public static int TaskSelectRes = 1;           //任务选择机台原则11、订单交期优先，选择最早可用机台 (默认)   2、换产时间最小优先 
        public static int PeriodLeftTime = 20;     //每个工作时间段剩余多少分种不安排任务20每个工作时间段剩余多少分钟不排新任务。如下午下班前20分钟，不再安排新任务
        public static int TaskSchNotBreak = 1;      //排程任务不能中断1排程任务不能中断，必须连续排产。资源会出现一些小的时间间隙，但不能完全排下，则该时间段不能利用。  1、排程任务不能中断  0、排程任务可以中断
        public static int DiffMaxTime = 8;          //  需配套生产最大相差时间    需配套生产物料，最大相差时间（小时），如8小时 ,越大计算次数越小，越快
        public static int SetMinDelayTime = 24;         //  配套最少延期时间          需配套生产物料，最后一部件生产完工后，最少延期多长时间，装配开始,越大，缺料情况越少。 
        public static double AllResiEfficient = 100;    //所有资源利用率%          最大为100%，正常不到100%,资源本身定义的效益生效
        public static int iProcessProductID = 9401;  //用于调试， 等于此工序任务ID时，断点会停
        public static DataTable dtParamValue = new DataTable();        //系统参数表
        public static int iSchSdID = 191;  //用于调试， 等于此工序任务ID时，断点会停
        public static int iSchSNMax = 1;                //排产顺序号
        public static int iSchSNMin = -1;               //最小已排的任务排产顺序
        public static int iSchWoSNMax = 1;              //最大工单排产任务数
        public static DateTime dtResLastSchTime ;          //记录任务上次排产完成时间        
        public static string cDayPlanMove = "0";            //1 排程调度优化计算
        public static int NextSeqBegTime = 120;             //后工序可开工时间为前工序开工后多长时间(分钟) 120
        public static string ReSchWo = "0";                 //重排已下达任务开工时间  已下达任务重排开工时间,关键工序生产顺序不变(按原来计划开工时间顺序序), 1 重排 0 不重排
        public static string cCustomer = "";                //客户编号，用于特殊处理
        public static string ExecTaskSchType = "1";             //已执行任务排产处理方式  1 冻结;2 已执行计划重排,考虑前工序完工时间;3 已执行计划重排,不考虑前工序完工时间;
        public static string ExecTaskSort = "1";                //已执行任务排产方式   1 不优化,按开工时间排序; 2 优化，按工艺特征排序优化
        public static string PreSeqEndDate = "1";               //考虑前工序完工时间约束 1 考虑前工序完工时间约束; 0 不考虑,资源任务会排连续
        public static string cSelfEndDate = "0";   // 1 排产考虑半成品完工时间; 0 不考虑;半成品完工后，上层半成品才可以排产
        public static string cPurEndDate = "0";   // 1 排产考虑采购件采购提前期; 0 不考虑; 采购件考虑采购提前期，上层半成品才可以排产
        public static string bReSchedule = "0";   // 1 重排; 0 第一次排
        public static string bSchKeyBySN = "0";      // 1 关键资源排产不能穿插，任务排序后，排在后面的任务开工时间不能大于前面任务。 0 允许穿插，后面任务可以排在前面任务之前。
        public static int NextProductBegTime = 0;   // 后序产品为前半成品开工后多长时间(分钟)可排产
        public static string cVersionNo = "";
        public static DateTime dtStart;              //     开始排程时间
        public static DateTime dtEnd;                //     排程截止时间
        public static DateTime dtToday;              //     当前时间，取数据库 
        public static int      iTaskMinWorkTime = 480;              //单资源超过多长时间自动分配到多机台， 0 不分配   480分钟  每个资源任务最小工作时间 8 小时,超过8小时才考虑多资源一起排
        public static string cMutResourceType = "1";                //多资源选择规则 “1”最早可完工日期资源优先选择，"2" 最早可开工日期资源优先选择  
        public static int iMutResourceDiffHour = 48;             //按资源组排产优先级选择资源时，后资源完工日期比前资源相早多少小时，选用第2资源
        public static string SchTaskRevTag = "3";              // 倒排方式 1 加工物料相同的所有工序; 2 白茬工序 ; 3 所有半成品工序
        public static string cSchRunType = "1";              //排程算法策略 1 按订单优先及排产 ; 2 关键资源优化排产
        public static string cSchCapType = "0";   // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能
        public static string cSchType = "0";      //排程方式  0 ---正常排产, 1--资源调度优化排产  2  按工艺段优化排产  3 按资源优化排产 2020-08-25
        public static string cTechSchType = "0";      //工艺段排产方式  0普通排产 1注塑排产 2装配排产 3表面处理 4委外排产
        public static string cProChaType1Sort = "0";      //排产优化顺序定义  0 按资源优先级 1 按工单需求日期 2 按订单优先级 3 座次优先级 4 工艺特征 5 开工时间 9 按资源档案定义 2020-08-25
        public static double iPreTaskRev = 24;     // 后工序完工时间比前工序大多少小时前工序倒排 24 小时，太小了容易倒排出错
        public static int ResSelectLastTaskDays;   // 多资源排产优先选择最近多少天内生产过的资源, '7' 天,'0 表示不考虑，越大基本固定在之前排产过的机台'
        public static int  iSchThread = 5 ;       // 多线程排程    
        public static DateTime ldtBeginDate ;     //开始时间
        public static double    iWaitTime;        //运行时间
        public static double iWaitTime2;        //运行时间
        public static double iWaitTime3;        //运行时间
        public static double iWaitTime4;        //运行时间
        public static double iWaitTime5;        //运行时间
        public static string APSDebug;             //APS调试模式
        public static int dEarliestSchDateDays;    //最早可排日期天数 产品最早可排时间，如果产品没有定义，则取这个时间做为产品最早可排时间，如7天。也可以在产品物料档案中定义采购周期 
        public static int dLastBegDateBeforeDays;  //重排时工序第一次计划开工日期前最早可提前多少天  2022-05-31 JonasCheng        
        public static string iCurSchRunTimes  ;     //当前排程次数
        public static void GetSchParams()
        {
            UseAPS =GetParam("UseAPS");
            ResProcessCharCount = Convert.ToInt16(GetParam("ResProcessCharCount"));
            TaskSelectRes = Convert.ToInt16(GetParam("TaskSelectRes"));
            PeriodLeftTime = Convert.ToInt16(GetParam("PeriodLeftTime")) * 60;
            TaskSchNotBreak = Convert.ToInt16(GetParam("TaskSchNotBreak"));
            DiffMaxTime = Convert.ToInt16(GetParam("DiffMaxTime"));
            SetMinDelayTime = Convert.ToInt16(GetParam("SetMinDelayTime"));
            AllResiEfficient = Convert.ToDouble(GetParam("AllResiEfficient"));
            if (AllResiEfficient <= 0.01 || AllResiEfficient > 100) AllResiEfficient = 100;
            iProcessProductID = Convert.ToInt32(GetParam("iProcessProductID"));
            iSchSdID = Convert.ToInt32(GetParam("iSchSdID"));
            cDayPlanMove = GetParam("cDayPlanMove");
            NextSeqBegTime = Convert.ToInt32(GetParam("NextSeqBegTime"));
            ReSchWo = GetParam("ReSchWo");
            cCustomer = GetParam("cCustomer");
            ExecTaskSchType = GetParam("ExecTaskSchType");   
            ExecTaskSort = GetParam("ExecTaskSort");
            cSelfEndDate =  GetParam("cSelfEndDate");  // 1 排产考虑半成品完工时间; 0 不考虑;半成品完工后，上层半成品才可以排产
            cPurEndDate = GetParam("cPurEndDate");   // 1 排产考虑采购件采购提前期; 0 不考虑; 采购件考虑采购提前期，上层半成品才可以排产
            string sNextProductBegTime = GetParam("NextProductBegTime");
            if (sNextProductBegTime == "") sNextProductBegTime = "240";
            NextProductBegTime = Convert.ToInt32(sNextProductBegTime);   // 后序产品为前半成品开工后多长时间(分钟)可排产
            bSchKeyBySN = GetParam("bSchKeyBySN");      // 1 关键资源排产不能穿插，任务排序后，排在后面的任务开工时间不能大于前面任务。 0 允许穿插，后面任务可以排在前面任务之前。
            cSchRunType = GetParam("cSchRunType");      ////排程算法策略   1 按订单优先及排产 ; 2 关键资源优化排产
            if (cSchRunType == "") cSchRunType = "1";
            cSchCapType = GetParam("cSchCapType");      // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能
            if (cSchRunType == "") cSchRunType = "0";
            string sTaskMinWorkTime = GetParam("iTaskMinWorkTime");   
            if (sTaskMinWorkTime == "") sTaskMinWorkTime = "480";   
            iTaskMinWorkTime = Convert.ToInt32(sTaskMinWorkTime);   // 0 表示不分配多机台 480分钟  每个资源任务最小工作时间 8 小时,超过8小时才考虑多资源一起排
            string sMutResourceDiffHour = GetParam("iMutResourceDiffHour");
            if (sMutResourceDiffHour == "") sMutResourceDiffHour = "48";
            iMutResourceDiffHour = Convert.ToInt32(sMutResourceDiffHour);   //按资源组排产优先级选择资源时，后资源完工日期比前资源相早多少小时，选用第2资源
            string sPreTaskRev = GetParam("PreTaskRev");
            if (sPreTaskRev == "") sPreTaskRev = "24";
            iPreTaskRev = Convert.ToDouble(sPreTaskRev);    // 后工序完工时间比前工序大多少小时前工序倒排 24 小时，太小了容易倒排出错
            cMutResourceType = GetParam("cMutResourceType");              //多资源选择规则 “1”最早可完工日期资源优先选择，"2" 最早可开工日期资源优先选择  
            if (cMutResourceType == "") cMutResourceType = "1";
            string cResSelectLastTaskDays = GetParam("cResSelectLastTaskDays");
            if (cResSelectLastTaskDays == "") cResSelectLastTaskDays = "7";
            ResSelectLastTaskDays = Convert.ToInt32(cResSelectLastTaskDays);   //多资源排产优先选择最近多少天内生产过的资源,7天 ,0 不考虑
            APSDebug = GetParam("APSDebug");                            //APS排程运算调试，输出到日志  1 调试 
            if (APSDebug == "") APSDebug = "0";
            cProChaType1Sort = GetParam("cProChaType1Sort");            //排产优化顺序定义  0 按资源优先级 1 按工单优先级 2 按订单优先级 3 座次优先级 4 工艺特征 5 开工时间 9 按资源档案定义     
            if (cProChaType1Sort == "") cProChaType1Sort = "0";
            string cdEarliestSchDateDays = GetParam("dEarliestSchDateDays");
            if (cdEarliestSchDateDays == "") cdEarliestSchDateDays = "1";
            dEarliestSchDateDays = Convert.ToInt32(cdEarliestSchDateDays);   //最早可排日期天数 默认1天
            string cdLastBegDateBeforeDays = GetParam("dLastBegDateBeforeDays");
            if (cdLastBegDateBeforeDays == "") cdLastBegDateBeforeDays = "1";
            dLastBegDateBeforeDays = Convert.ToInt32(cdLastBegDateBeforeDays);   // //重排时工序第一次计划开工日期前最早可提前多少天  2022-05-31 JonasCheng
            iSchSNMax = 1;
            iSchWoSNMax = 1;              //最大工单排产任务数
            try
            {
                SchParam.Debug("排程运算开始", "参数准备");
            }
            catch (Exception exp)
            { 
            }
        }
        public static string GetParam(string cParamNo)
        {         
            DataRow[] drParam = dtParamValue.Select("cParamNo = '" + cParamNo + "'");
            string cValue = "";
            if (drParam.Length > 0)
            {
                cValue = drParam[0]["cValue"].ToString();
            }
            return cValue;
        }
        public static Logger_No objLogger; //= new Logger();              //日志处理对象
        public static void Debug(String message,String form ,String Event = "")
        {
            Object[] parameters = new Object[3];
            parameters[0] = message;
            parameters[1] = form;
            parameters[2] = Event;
            try
            {
            }
            catch (Exception exp)
            {
                string error = exp.Message;
            }
        }
        public static void Error(String message, String form, String Event = "")
        {
            Object[] parameters = new Object[3];
            parameters[0] = message;
            parameters[1] = form;
            parameters[2] = Event;
            try
            {
            }
            catch (Exception exp)
            {
                string error = exp.Message;
            }
        }
    }
}