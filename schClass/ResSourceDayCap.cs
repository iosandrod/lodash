using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Algorithm
{
    /// <summary>
    /// 资源日时间段，按日排产,按工作日历定义的每天时间段
    /// </summary>
    ///  
    [Serializable]
    public class ResSourceDayCap : IComparable, ICloneable, ISerializable
    {
        public SchData schData = null;        //所有排程数据

        private string cResourceNo = "";    //对应资源ＩＤ号,要设置     
        private string cIsInfinityAbility = "0";    // 0 产能有限，1 产能无限     
        public Resource resource = null;                   // 时段对应的资源  有值        
        private DateTime dBegTime;    //时间段开始时间
        private DateTime dEndTime;   //时间段结束时间
        private int holdingTime = 0;     //时段总长, dDEndTimeTime - dBegTime,单位为秒
        private int allottedTime = 0;    //已分配时间,包括维修、故障时间
        private int maintainTime = 0;    //维修、故障时间
        private int availableTime = 0;   //时段内可用时间，计算出来
        public int WorkTimeAct = 0;        //学习曲线折扣,有效加工时间
        private int notWorkTime = 0;     //时段内空闲时间，计算出来,用于检查
        public int iPeriodID = 1;        //时段ID，排程完成写回数据库时，重新生成，唯一
        public DateTime dPeriodDay;      //时段所属日期
        public string FShiftType;       //时段所属班次 白班、夜班、中班等
        public int iTaskCount = 0;      //当日已排任务总数
        public int iTaskMaxCount = 0;      //当日可排任务总数                                

        //日可排程时间段
        public List<ResTimeRange> ResTimeRangeList = new List<ResTimeRange>(10);

        //当前时间段对应 加工任务时间段列表，同一时间段有安排多个任务的情况 
        //只存入当前时间段对应时间段，空闲时间段
        public List<TaskTimeRange> taskTimeRangeList = new List<TaskTimeRange>(10);

        //存入已排工单任务
        public List<TaskTimeRange> WorkTimeRangeList = new List<TaskTimeRange>(10);

              

        public ResTimeRange ResTimeRangePre;          //前资源时间段
        public ResTimeRange ResTimeRangePost;         //后资源时间段

        public int iSchSdID = -1;                     //记录更新、新增时间段的任务ID
        public int iProcessProductID = -1;
        public int iResProcessID = -1;
        public int iSchSNMax = -1;

        //初始化结构函数
        public ResSourceDayCap(string as_ResourceNo)
        {
            this.cResourceNo = as_ResourceNo;
        }
        public ResSourceDayCap(string as_ResourceNo, DateTime adBegTime, DateTime adEndTime)
        {
            this.cResourceNo = as_ResourceNo;
            this.dBegTime = adBegTime;
            this.dEndTime = adEndTime;
            this.AllottedTime = 0;
            //this.NotWorkTime = this.HoldingTime; 
        }

        public ResSourceDayCap()
        {

        }


        ////初始化时，给时段分配已排任务，生成任务时段占用列表
        //public int TimeSchTaskFreezeInit(SchProductRouteRes as_SchProductRouteRes, ref DateTime adCanBegDate, ref DateTime adCanEndDate)
        //{

        //    Boolean bSchdule = true;   //正式排产

        //    //模拟排产时bSchdule = false，取大于等于当前时间的所有任务时间段，包含已排任务;正式排产，只取空闲时间段
        //    List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanBegDate, false, bSchdule, Convert.ToInt32(as_SchProductRouteRes.iResRationHour + as_SchProductRouteRes.iResPreTime), false);

        //    try
        //    {

        //        //给每个空闲时间段，分配任务,注意：模拟排产时，这里包含已排任务的时段，得跳过重来
        //        for (int i = 0; i < NoTimeRangeList.Count; i++)
        //        {

        //            //if (ai_workTime == 0) break;


        //            //空闲时间段
        //            TaskTimeRange NoTimeRange = NoTimeRangeList[i];

        //            if (NoTimeRange.DBegTime >= adCanEndDate) break;  //时间段大于待排结束时间段，退出



        //            ////记录排产工时的原始值
        //            //ai_workTimeOld = ai_workTime;
        //            //排产时间段小于5分钟，而且没有排完，则该空闲时段因为太小，不排产
        //            if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime)
        //            {
        //                continue;
        //            }



        //            //待排产的任务时段
        //            TaskTimeRange taskTimeRange1 = new TaskTimeRange();

        //            taskTimeRange1.cTaskType = 1;  //工作
        //            taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
        //            taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
        //            taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
        //            taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
        //            taskTimeRange1.cResourceNo = this.cResourceNo;

        //            taskTimeRange1.resource = this.resource;                     //资源对象
        //            taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
        //            taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
        //            taskTimeRange1.resTimeRange = this;

        //            //可用时间段小于时段开始时间,整个时间段都可用
        //            if (NoTimeRange.dBegTime >= adCanBegDate)
        //            {
        //                //整个空闲时段都占用
        //                if (adCanEndDate > NoTimeRange.DEndTime)
        //                {

        //                    taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
        //                }
        //                else        ////部分占用
        //                {
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = adCanEndDate;
        //                    taskTimeRange1.AllottedTime = Convert.ToInt32((taskTimeRange1.DEndTime - taskTimeRange1.DBegTime).TotalSeconds);
        //                }
        //            }
        //            else if ((NoTimeRange.dBegTime < adCanBegDate && NoTimeRange.dEndTime > adCanBegDate))    // 可用时间段大于时段开始时间,前面部分时间段不可用
        //            {
        //                if (NoTimeRange.DEndTime >= adCanEndDate)
        //                {
        //                    taskTimeRange1.DBegTime = adCanBegDate;
        //                    taskTimeRange1.DEndTime = adCanEndDate;
        //                    taskTimeRange1.AllottedTime = Convert.ToInt32((taskTimeRange1.DEndTime - taskTimeRange1.DBegTime).TotalSeconds);

        //                }
        //                else if (NoTimeRange.DEndTime < adCanEndDate)
        //                {

        //                    taskTimeRange1.DBegTime = adCanBegDate;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
        //                    taskTimeRange1.AllottedTime = Convert.ToInt32((taskTimeRange1.DEndTime - taskTimeRange1.DBegTime).TotalSeconds);

        //                }
        //                else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
        //                {
        //                    continue;
        //                }
        //            }
        //            else
        //            {
        //                continue;
        //            }

        //            if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
        //            {
        //                //MessageBox.Show("数据异常，生成新任务开始时间不对!", "提示");
        //                throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
        //            }

        //            //正式排程,冻结任务排产
        //            if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
        //            {
        //                int m = 1;
        //                throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
        //                return -1;
        //            }

        //            //冻结任务排产,在原时间段中，插入新增加的工作任务时段,this.resource.TaskTimeRangeList同步增加任务
        //            TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);

        //            //资源任务对象中，增加任务已排时间段 不确定能否同时增加
        //            as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);


        //            //下个可开始时间为当前任务的结束时间
        //            adCanBegDate = taskTimeRange1.DEndTime;
        //            //bFirtTime = false;      //是否第一个排产时间段,不是第一个


        //            //正排程完，检查时段数据是否正确
        //            if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //            {
        //                throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //                return -1;
        //            }


        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }

        //    return 0; //剩下未排时间

        //}


        ////初始化时，已排任务正排 给时段分配任务,加工时间不重算
        //public int TimeSchTaskSortInit(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref Boolean bFirtTime)
        //{
        //    int taskallottedTime = 0;            //任务在本时间段内 总安排时间
        //    int ai_workTimeOld = ai_workTime;    //用于记录ai_workTime值
        //    //Boolean bFirtTime = true;            //是否第一个排产时间段
        //    //int ai_CycTimeTol = 0;             //换刀时间

        //    if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
        //    {
        //        int i = 1;
        //    }

        //    //模拟排产时bSchdule = false，取大于等于当前时间的所有任务时间段，包含已排任务;正式排产，只取空闲时间段
        //    List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanBegDate, false, bSchdule, ai_workTime + ai_ResPreTime, false);

        //    try
        //    {

        //        //给每个空闲时间段，分配任务,注意：模拟排产时，这里包含已排任务的时段，得跳过重来
        //        for (int i = 0; i < NoTimeRangeList.Count; i++)
        //        {

        //            if (ai_workTime <= 0) break;

        //            //空闲时间段
        //            TaskTimeRange NoTimeRange = NoTimeRangeList[i];

        //            if (bFirtTime)   //是第一个排产时间段,计算换产时间
        //            {
        //                if (NoTimeRange.AvailableTime == 0) continue;    //是排第一个时段，期该时段没有可用时间，则继续                       

        //                ai_CycTimeTol = 0;   //设为0

        //                //计算换产时间 和 换刀时间                        
        //                ai_ResPreTime = this.resource.GetChangeTime(as_SchProductRouteRes, ai_workTime, NoTimeRange.DBegTime, ref ai_CycTimeTol, bSchdule);


        //                if (ai_ResPreTime > 0)
        //                {
        //                    int K = 0;
        //                }
        //                //计算换产时间包含换刀时间
        //                ai_ResPreTime += ai_CycTimeTol;

        //                //总加工时间 包含 换产时间包含换刀时间,再进行排产
        //                ai_workTime = Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime;
        //            }

        //            //模拟排产时，未排完，遇到有其他已排任务，则整个任务从头重排，换产时间也要重新计算，因为其前一任务可能已改变
        //            if (bSchdule == false)
        //            {
        //                if (NoTimeRange.cTaskType != 0 && ai_workTime > 0)  //只要不是空闲时间段
        //                {
        //                    //任务ai_ResPreTime
        //                    //ai_ResPreTime = 
        //                    //ai_ResPreTime = this.resource.GetChangeTime(as_SchProductRouteRes, ai_workTime, adStartDate, ref ai_CycTimeTol);
        //                    bFirtTime = true;   //是否第一个排产时间段

        //                    ai_workTime = ai_workTimeTask;            //返回原值
        //                    adCanBegDate = NoTimeRange.DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
        //                    adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
        //                    continue;
        //                }
        //            }

        //            ////记录排产工时的原始值
        //            //ai_workTimeOld = ai_workTime;
        //            //排产时间段小于5分钟，而且没有排完，则该空闲时段因为太小，不排产
        //            if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime && ai_workTime > 0)
        //            {
        //                continue;
        //            }



        //            //待排产的任务时段
        //            TaskTimeRange taskTimeRange1 = new TaskTimeRange();

        //            taskTimeRange1.cTaskType = 1;  //工作
        //            taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
        //            taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
        //            taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
        //            taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
        //            taskTimeRange1.cResourceNo = this.cResourceNo;

        //            taskTimeRange1.resource = this.resource;                     //资源对象
        //            taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
        //            taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
        //            taskTimeRange1.resTimeRange = this;

        //            //有排产，在本涵数里面修改，是否首次排产，以免多次重算前准备时间 2019-09-11
        //            if (bFirtTime)
        //                bFirtTime = false;


        //            //可用时间段小于时段开始时间,整个时间段都可用
        //            if (NoTimeRange.dBegTime >= adCanBegDate)
        //            {

        //                //整个空闲时段都占用
        //                if (ai_workTime > NoTimeRange.AvailableTime)
        //                {

        //                    taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime;


        //                    ai_workTime -= NoTimeRange.AvailableTime;
        //                }
        //                else        ////部分占用
        //                {
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DBegTime.AddSeconds(ai_workTime);
        //                    taskTimeRange1.AllottedTime = ai_workTime;

        //                    ai_workTime = 0;        //剩余待分配工作时间为0 
        //                }
        //            }
        //            else                // 可用时间段大于时段开始时间,前面部分时间段不可用
        //            {
        //                if (NoTimeRange.DEndTime > adCanBegDate)
        //                {
        //                    TimeSpan lTimeSpan = (NoTimeRange.DEndTime - adCanBegDate);
        //                    int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);

        //                    //整个空闲时段都占用，未排完
        //                    if (ai_workTime > iAvailableTime)
        //                    {
        //                        taskTimeRange1.DBegTime = adCanBegDate;
        //                        taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
        //                        taskTimeRange1.AllottedTime = iAvailableTime;

        //                        ai_workTime -= iAvailableTime;
        //                    }
        //                    else        ////部分占用,排完
        //                    {
        //                        taskTimeRange1.DBegTime = adCanBegDate;
        //                        taskTimeRange1.DEndTime = adCanBegDate.AddSeconds(ai_workTime);
        //                        taskTimeRange1.AllottedTime = ai_workTime;

        //                        ai_workTime = 0;        //剩余待分配工作时间为0 
        //                    }
        //                }
        //                else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
        //                {
        //                    continue;
        //                }
        //            }

        //            if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
        //            {
        //                //MessageBox.Show("数据异常，生成新任务开始时间不对!", "提示");
        //                throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
        //            }

        //            if (bSchdule)  //正式排程
        //            {
        //                //正式排程
        //                if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
        //                {
        //                    int m = 1;
        //                    throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
        //                    return -1;
        //                }

        //                //冻结任务排产 在原时间段中，插入新增加的工作任务时段,this.resource.TaskTimeRangeList同步增加任务
        //                TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);

        //                ////资源所有任务时段列表中增加 ,不用再增加
        //                //this.resource.TaskTimeRangeList.Add(taskTimeRange1);

        //                //资源任务对象中，增加任务已排时间段 不确定能否同时增加
        //                as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
        //            }

        //            //下个可开始时间为当前任务的结束时间
        //            adCanBegDate = taskTimeRange1.DEndTime;
        //            bFirtTime = false;      //是否第一个排产时间段,不是第一个

        //            if (bSchdule)  //正式排程
        //            {
        //                //正排程完，检查时段数据是否正确
        //                if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //                {
        //                    throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //                    return -1;
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }

        //    return ai_workTime; //剩下未排时间

        //}



        ////正排 给时段分配任务，生成任务时段占用列表，更新时段状态.ai_workTime 可以返回值（减去本时段的可用时间剩余值）//bSchdule = true正式排程，false模拟排程
        //public int TimeSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref Boolean bFirtTime, ref int ai_DisWorkTime, Boolean bReCalWorkTime = true, ResTimeRange resTimeRangeNext = null, SchProductRouteRes as_SchProductRouteResPre = null)
        //{
        //    int taskallottedTime = 0;            //任务在本时间段内 总安排时间
        //    int ai_workTimeOld = ai_workTime;    //用于记录ai_workTime值
        //    double iDayDis = 1;                  //考虑学习曲线每天折扣
        //    DateTime dtiDayDis = adCanBegDate.AddDays(-1);           //学习曲线日期
        //    DateTime dtTaskBegDate = adCanBegDate;                   //任务开始排产日期
        //    int iTaskAllottedTime = 0;                    //学习曲线 时段分配工时            
        //    int ai_workTimeDisTol = ai_workTime;             //用于记录学习曲线打则后的
        //    int ai_workTimeAct = 0;                         //累计已排有效时间

        //    //Boolean bFirtTime = true;            //是否第一个排产时间段
        //    //int ai_CycTimeTol = 0;             //换刀时间
        //    string message;
        //    DateTime ldtBeginDate = DateTime.Now;




        //    if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
        //    {
        //        int i = 1;
        //    }

        //    //模拟排产时bSchdule = false，取大于等于当前时间的所有任务时间段，包含已排任务;正式排产，只取空闲时间段
        //    List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanBegDate, false, bSchdule, ai_workTime + ai_ResPreTime, true);

        //    if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
        //    {
        //        message = string.Format(@"3.4.2、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
        //                                                as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
        //        SchParam.Debug(message, "资源运算");
        //        ldtBeginDate = DateTime.Now;
        //    }

        //    ////如果没有合适时间段,继续
        //    //if (NoTimeRangeList.Count < 1)
        //    //{
        //    //    //模拟排产  2019-08-30 Jonas Cheng 
        //    //    if (bSchdule == false && this.WorkTimeRangeList.Count > 0 )
        //    //    {
        //    //        if ( ai_workTime > 0)  //只要不是空闲时间段
        //    //        {                      
        //    //            bFirtTime = true;   //是否第一个排产时间段
        //    //            ai_workTime = ai_workTimeTask;            //返回原值      
        //    //            adCanBegDate = this.DEndTime;             //重置可开工时间
        //    //        }                    
        //    //    }

        //    //    return ai_workTimeTask;               
        //    //}

        //    ////有其他排产任务,而且排不下时 2019-09-09 Jonas Cheng 
        //    //if (this.WorkTimeRangeList.Count > 0 && this.NotWorkTime < ai_workTime )
        //    //{
        //    //    bFirtTime = true;   //是否第一个排产时间段
        //    //    ai_workTime = ai_workTimeTask;            //返回原值      
        //    //    adCanBegDate = this.WorkTimeRangeList[this.WorkTimeRangeList.Count - 1].DEndTime; //this.DEndTime;             //重置可开工时间
        //    //}


        //    //正排程完，检查时段数据是否正确
        //    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //    {
        //        throw new Exception("出错位置：排程时段检查出错TimeSchTask.TimeSchTask！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //        return -1;
        //    }

        //    try
        //    {



        //        //给每个空闲时间段，分配任务,注意：模拟排产时，这里包含已排任务的时段，得跳过重来
        //        for (int i = 0; i < NoTimeRangeList.Count; i++)
        //        {

        //            //当前任务已排产完成
        //            if (ai_workTime <= 0)
        //            {
        //                ai_workTime = 0;
        //                break;
        //            }

        //            //空闲时间段
        //            TaskTimeRange NoTimeRange = NoTimeRangeList[i];

        //            if (bFirtTime)   //是第一个排产时间段,计算换产时间
        //            {
        //                //NoTimeRange.cTaskType == 1 ||
        //                if (NoTimeRange.AvailableTime == 0)
        //                    continue;    //是排第一个时段，期该时段没有可用时间，则继续      



        //                ////如果中间有空隙可用时间时，前面的空隙时间不可用。不是最后一个时间段,而且后一个时间段还是已排的任务
        //                //if (i + 1 < NoTimeRangeList.Count && NoTimeRangeList[i + 1].cTaskType != 0 && NoTimeRange.AvailableTime < ai_workTime)  //NoTimeRange.AvailableTime < ai_workTime
        //                //{
        //                //    if (bSchdule == false)
        //                //    {
        //                //        adCanBegDateTask = NoTimeRangeList[0].DEndTime;
        //                //        adCanBegDate = NoTimeRangeList[0].DEndTime;
        //                //    }
        //                //    continue;
        //                //}

        //                if (bReCalWorkTime)   //重新计算前准备时间，已下达生产任务单，不用重新计算 bReCalWorkTime = false
        //                {
        //                    ai_CycTimeTol = 0;   //设为0

        //                    //计算换产时间 和 换刀时间,已经开工的任务，前准备时间不变 //前准备时间为0 2017-12-13                            
        //                    if (this.resource.iSchBatch <= 1 && as_SchProductRouteRes.iActResReqQty > 0)
        //                        //ai_ResPreTime = (int)(as_SchProductRouteRes.iResPreTime);
        //                        ai_ResPreTime = 0;
        //                    else
        //                    {
        //                        ai_ResPreTime = this.resource.GetChangeTime(as_SchProductRouteRes, ai_workTime, NoTimeRange.DBegTime, ref ai_CycTimeTol, bSchdule, as_SchProductRouteResPre);
        //                        ////如果已下的任务,重取前准备时间为0，还是维持原来准备时间。 2017-06-18
        //                        //if (as_SchProductRouteRes.cWoNo != "" && ai_ResPreTime < as_SchProductRouteRes.iResPreTime )
        //                        //    ai_ResPreTime = (int)(as_SchProductRouteRes.iResPreTime);
        //                    }


        //                    if (ai_ResPreTime > 0)
        //                    {
        //                        int K = 0;
        //                    }
        //                    //计算换产时间包含换刀时间
        //                    ai_ResPreTime += ai_CycTimeTol;

        //                    //总加工时间 包含 换产时间包含换刀时间,再进行排产
        //                    ai_workTime = Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime;  //Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime
        //                    ai_workTimeDisTol = ai_workTime;
        //                }

        //                //dtTaskBegDate = NoTimeRange.DEndTime;     //设置任务排产开始日期
        //                as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime;
        //            }


        //            //排到本时间段时，检查如果没有排完，但遇到到了已排任务，则重新选任务结束时间，重新开始排产
        //            //后者遇到本时间段已全部排满任务，也是重头再排
        //            if (bSchdule == false)
        //            {
        //                ////有其他排产任务,而且排不下时 2019-09-09 Jonas Cheng  NoTimeRange.AvailableTime < ai_workTime 当前时间段排不下。找下一个时间段 2020-01-06
        //                //if (this.WorkTimeRangeList.Count > 0 && this.WorkTimeRangeList[0].DBegTime <= NoTimeRange.DBegTime && NoTimeRange.AvailableTime < ai_workTime)

        //                if (as_SchProductRouteRes.cLearnCurvesNo != "")
        //                {
        //                    iDayDis = as_SchProductRouteRes.TechLearnCurves.GetDayDisValue(as_SchProductRouteRes.dResLeanBegDate, NoTimeRange.DEndTime);
        //                }

        //                //空闲时间段后面有任务，而且当前任务没有排完，当前时间段排不下，重新再排
        //                if (NoTimeRange.taskTimeRangePost != null && ai_workTime / iDayDis > NoTimeRange.AvailableTime)
        //                {
        //                    bFirtTime = true;   //是否第一个排产时间段
        //                    ai_workTime = ai_workTimeTask;            //返回原值      
        //                    //adCanBegDate = this.WorkTimeRangeList[this.WorkTimeRangeList.Count - 1].DEndTime; //this.DEndTime;             //重置可开工时间
        //                    adCanBegDate = NoTimeRange.DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
        //                    adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
        //                }
        //                else
        //                {
        //                    adCanBegDate = NoTimeRange.DBegTime;
        //                }

        //                //模拟排产时，未排完，遇到有其他已排任务，则整个任务从头重排，换产时间也要重新计算，因为其前一任务可能已改变
        //                if (NoTimeRange.cTaskType != 0 && ai_workTime > 0)  //只要不是空闲时间段
        //                {
        //                    //任务ai_ResPreTime                            
        //                    bFirtTime = true;   //是否第一个排产时间段

        //                    ai_workTime = ai_workTimeTask;            //返回原值
        //                    adCanBegDate = NoTimeRange.DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
        //                    adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回

        //                    continue;
        //                }


        //            }


        //            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
        //            {
        //                message = string.Format(@"3.4.3、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
        //                                                        as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
        //                SchParam.Debug(message, "资源运算");
        //                ldtBeginDate = DateTime.Now;
        //            }


        //            ////记录排产工时的原始值
        //            //ai_workTimeOld = ai_workTime;
        //            //排产时间段小于5分钟，而且没有排完，则该空闲时段因为太小，不排产, 如果任务比较小，可以排下时，正常排产 2020-01-06
        //            if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime && ai_workTime > 0 && ai_workTime > NoTimeRange.AvailableTime)
        //            {
        //                continue;
        //            }


        //            //DateTime ldtEndDate = DateTime.Now;
        //            //TimeSpan interval = ldtEndDate - ldtBeginDate;
        //            //SchParam.iWaitTime = interval.TotalMilliseconds;//计算间隔时间


        //            //待排产的任务时段
        //            TaskTimeRange taskTimeRange1 = new TaskTimeRange();

        //            taskTimeRange1.cTaskType = 1;  //工作
        //            taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
        //            taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
        //            taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
        //            taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
        //            taskTimeRange1.cResourceNo = this.cResourceNo;

        //            taskTimeRange1.resource = this.resource;                     //资源对象
        //            taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
        //            taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
        //            taskTimeRange1.resTimeRange = this;

        //            //学习曲线 ai_workTime
        //            //iDayDis = 0;                  //考虑学习曲线每天折扣
        //            //DateTime dtiDayDis
        //            //当前日期学习曲线折扣
        //            if (as_SchProductRouteRes.cLearnCurvesNo != "")
        //            {
        //                ////找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于adCanBgDate,4小时内的任务，才认为是相邻任务
        //                ////List<TaskTimeRange> ListReturn = this.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.DEndTime <= adStartDate && p.cTaskType == 1; }); // p.AvailableTime > 0 
        //                //List<TaskTimeRange> ListReturn = this.resource.GetTaskTimeRangeList(false).FindAll(delegate(TaskTimeRange p) { return p.DEndTime <= as_SchProductRouteRes.dResLeanBegDate && p.DEndTime.AddDays(10) > as_SchProductRouteRes.dResLeanBegDate && p.cTaskType == 1; }); // p.AvailableTime > 0 
        //                //TaskTimeRange TaskTimeRangePre = null;   //前一个生产任务,也可能没有任务，也要计算换产时间

        //                //if (ListReturn.Count > 0)
        //                //{
        //                //    ////必须要排序,取最近一个任务
        //                //    //ListReturn.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare( p2.DBegTime,p1.DBegTime); });

        //                //    TaskTimeRangePre = ListReturn[0];

        //                //    //int iDays = SchData.GetDateDiff(this.schProductRouteRes.resource, "d", dtBegDate, dtCurDate);

        //                //}

        //                iDayDis = as_SchProductRouteRes.TechLearnCurves.GetDayDisValue(as_SchProductRouteRes.dResLeanBegDate, NoTimeRange.DEndTime);

        //                if (iDayDis > 0)
        //                {
        //                    //iTaskAllottedTime = 0;                    //学习曲线 时段分配工时            
        //                    ai_workTime = Convert.ToInt32(ai_workTime / iDayDis);             //用于记录学习曲线打则后的
        //                }
        //                else
        //                {
        //                    iDayDis = 1;
        //                }
        //            }
        //            else
        //            {
        //                iDayDis = 1;
        //            }

        //            //ldtEndDate = DateTime.Now;
        //            //interval = ldtEndDate - ldtBeginDate;
        //            //SchParam.iWaitTime2 = interval.TotalMilliseconds;//计算间隔时间

        //            //有排产，在本涵数里面修改，是否首次排产，以免多次重算前准备时间 2019-09-11
        //            if (bFirtTime)
        //                bFirtTime = false;

        //            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
        //            {
        //                int m = 1;
        //            }

        //            //可用时间段小于时段开始时间,整个时间段都可用
        //            if (NoTimeRange.dBegTime >= adCanBegDate)
        //            {

        //                //整个空闲时段都占用
        //                if (ai_workTime > NoTimeRange.AvailableTime)
        //                {
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
        //                    taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;


        //                    iTaskAllottedTime = NoTimeRange.AvailableTime;
        //                    ai_workTime -= NoTimeRange.AvailableTime;

        //                    //NoTimeRange.allottedTime = iTaskAllottedTime;

        //                }
        //                else        ////部分占用
        //                {
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DBegTime.AddSeconds(ai_workTime);
        //                    taskTimeRange1.AllottedTime = ai_workTime;

        //                    iTaskAllottedTime = ai_workTime;
        //                    ai_workTime = 0;        //剩余待分配工作时间为0 

        //                    //NoTimeRange.allottedTime = iTaskAllottedTime;

        //                }
        //            }
        //            else                // 可用时间段大于时段开始时间,前面部分时间段不可用
        //            {
        //                if (NoTimeRange.DEndTime > adCanBegDate)
        //                {
        //                    TimeSpan lTimeSpan = (NoTimeRange.DEndTime - adCanBegDate);
        //                    int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);

        //                    //整个空闲时段都占用，未排完
        //                    if (ai_workTime > iAvailableTime)
        //                    {
        //                        taskTimeRange1.DBegTime = adCanBegDate;
        //                        taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
        //                        taskTimeRange1.AllottedTime = iAvailableTime;


        //                        iTaskAllottedTime = iAvailableTime;
        //                        ai_workTime -= iAvailableTime;

        //                        //NoTimeRange.allottedTime = iTaskAllottedTime;

        //                    }
        //                    else        ////部分占用,排完
        //                    {
        //                        taskTimeRange1.DBegTime = adCanBegDate;
        //                        taskTimeRange1.DEndTime = adCanBegDate.AddSeconds(ai_workTime);
        //                        taskTimeRange1.AllottedTime = ai_workTime;

        //                        iTaskAllottedTime = ai_workTime;
        //                        ai_workTime = 0;        //剩余待分配工作时间为0 

        //                        //NoTimeRange.allottedTime = iTaskAllottedTime;
        //                    }
        //                }
        //                else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
        //                {
        //                    continue;
        //                }
        //            }



        //            //当前分配的时间,考虑学习曲线折扣
        //            ai_workTimeAct += Convert.ToInt32(iTaskAllottedTime * iDayDis);
        //            taskTimeRange1.WorkTimeAct = Convert.ToInt32(iTaskAllottedTime * iDayDis);

        //            //if (taskTimeRange1.DBegTime < SchParam.ldtBeginDate.AddDays(-5) )
        //            //{
        //            //    //MessageBox.Show("数据异常，生成新任务开始时间不对!", "提示");
        //            //    throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
        //            //    int j = 1;
        //            //}

        //            if (taskTimeRange1.DBegTime >= taskTimeRange1.DEndTime)
        //            {
        //                //MessageBox.Show("数据异常，生成新任务开始时间,完成时间不对!", "提示");
        //                throw new Exception(string.Format("数据异常，生成新任务开始时间不对!开始大于结束时间"));
        //                return -1;
        //            }

        //            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
        //            {
        //                message = string.Format(@"3.4.4、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
        //                                                        as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
        //                SchParam.Debug(message, "资源运算");
        //                ldtBeginDate = DateTime.Now;
        //            }


        //            //ldtEndDate = DateTime.Now;
        //            //interval = ldtEndDate - ldtBeginDate;
        //            //SchParam.iWaitTime3 = interval.TotalMilliseconds;//计算间隔时间

        //            if (bSchdule)  //正式排程
        //            {
        //                if (this.cResourceNo == "3.04.24" && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
        //                {
        //                    int ii = 1;
        //                }
        //                if (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
        //                {
        //                    int ii = 1;
        //                }

        //                //正式排程,正排
        //                if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
        //                {
        //                    int m = 1;
        //                    //throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
        //                    //return -1;
        //                }


        //                //正排 在原时间段中，插入新增加的工作任务时段,this.resource.TaskTimeRangeList同步增加任务
        //                TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);


        //                //正式排程，正排
        //                if (this.cIsInfinityAbility != "1" && (this.AllottedTime + this.AvailableTime > this.HoldingTime))
        //                {
        //                    int m = 1;
        //                    //throw new Exception(string.Format("出错位置2：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
        //                    //return -1;
        //                }

        //                ////资源所有任务时段列表中增加 ,不用再增加
        //                //this.resource.TaskTimeRangeList.Add(taskTimeRange1);

        //                //资源任务对象中，增加任务已排时间段 不确定能否同时增加
        //                as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
        //            }

        //            //下个可开始时间为当前任务的结束时间
        //            adCanBegDate = taskTimeRange1.DEndTime;
        //            //bFirtTime = false;      //是否第一个排产时间段,不是第一个

        //            //int allottedTime = this.resource.ResTimeRangeList[47].AllottedTime;




        //            if (bSchdule)  //正式排程
        //            {
        //                //正排程完，检查时段数据是否正确
        //                if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //                {
        //                    string Errormessage = string.Format(@"检查时段数据出错CheckResTimeRange,排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],时段开始时间[{4}],时段结束时间[{5}],时段总工时[{6}],分配工时[{7}],空闲工时[{8}],差异工时[{9}]",
        //                                               as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, this.dBegTime, this.dEndTime, this.HoldingTime, this.AllottedTime, this.NotWorkTime, this.AvailableTime - this.NotWorkTime);
        //                    SchParam.Error(Errormessage, "资源运算出错");

        //                    throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！" + taskTimeRange1.DEndTime.ToString());
        //                    return -1;
        //                }
        //            }


        //            //ldtEndDate = DateTime.Now;
        //            //interval = ldtEndDate - ldtBeginDate;
        //            //SchParam.iWaitTime4 = interval.TotalMilliseconds;//计算间隔时间
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }

        //    //重新计算有效加工时间,考虑学习曲线折扣
        //    ai_workTime = ai_workTimeDisTol - ai_workTimeAct;
        //    if (ai_workTime < 0) ai_workTime = 0;

        //    if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
        //    {
        //        message = string.Format(@"3.4.5、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
        //                                                as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
        //        SchParam.Debug(message, "资源运算");
        //        ldtBeginDate = DateTime.Now;
        //    }



        //    //DateTime ldtEndDate2 = DateTime.Now;
        //    //TimeSpan interval2 = ldtEndDate2 - ldtBeginDate;
        //    //SchParam.iWaitTime5 = interval2.TotalMilliseconds;//计算间隔时间

        //    return ai_workTime; //剩下未排时间

        //}

        ////有限产能倒排 给时段分配任务，生成任务时段占用列表，更新时段状态.ai_workTime 可以返回值（减去本时段的可用时间剩余值） //bSchdule = true正式排程，false模拟排程
        //public int TimeSchTaskRev(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanEndDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref Boolean bFirtTime)
        //{
        //    int taskallottedTime = 0;   //任务在本时间段内 总安排时间
        //    int ai_workTimeOld = 0;     //用于记录ai_workTime值，当空闲时间段太下时，如10分钟，则该时间段不排，ai_workTime值还原。
        //    //Boolean bFirtTime = true;   //是否第一个排产时间段

        //    //已排序，由大到小
        //    List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanEndDate, true, bSchdule, ai_workTime, true);

        //    if (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
        //    {
        //        int ii = 1;
        //    }

        //    //SchParam.Debug("3.111111111111 TimeSchTaskRev", "资源运算");


        //    //cSchType '1' 有限产能倒排程完，检查时段数据是否正确；'2' 新增加无限产能倒排，不检查 2020-04-04  as_SchProductRouteRes.schProductRoute.schProduct.cSchType == "1" &&          
        //    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //    {
        //        throw new Exception("出错位置：倒排排程时段检查出错TimeSchTask.TimeSchTaskRev！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //        return -1;
        //    }

        //    try
        //    {
        //        //给每个空闲时间段，分配任务。注意：模拟排产时，这里包含已排任务的时段，得跳过重来
        //        for (int i = 0; i < NoTimeRangeList.Count; i++)
        //        {
        //            ////记录排产工时的原始值
        //            //ai_workTimeOld = ai_workTime;

        //            if (ai_workTime <= 0)
        //            {
        //                ai_workTime = 0;
        //                break;
        //            }

        //            TaskTimeRange NoTimeRange = NoTimeRangeList[i];


        //            if (bFirtTime)   //是第一个排产时间段,计算换产时间
        //            {
        //                if (NoTimeRange.AvailableTime == 0) continue;    //是排第一个时段，期该时段没有可用时间，则继续 

        //                ////如果中间有空隙可用时间时，前面的空隙时间不可用。不是最后一个时间段,而且后一个时间段还是已排的任务
        //                ////if (i + 1 < NoTimeRangeList.Count && NoTimeRangeList[i + 1].cTaskType != 0 && NoTimeRange.AvailableTime < ai_workTime)
        //                //if (i + 1 < NoTimeRangeList.Count && NoTimeRangeList[i + 1].cTaskType != 0 && NoTimeRange.AvailableTime < ai_workTime)
        //                //{
        //                //    if (bSchdule == false)  //模拟排产
        //                //    {
        //                //        adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;  
        //                //        adCanBegDateTask = NoTimeRange.DBegTime;
        //                //    }
        //                //    continue;

        //                //}

        //                ///倒排正常应该要重算前准备时间的
        //                //if (bReCalWorkTime)   //重新计算前准备时间，已下达生产任务单，不用重新计算 bReCalWorkTime = false
        //                //{
        //                //    ai_CycTimeTol = 0;   //设为0

        //                //    计算换产时间 和 换刀时间,已经开工的任务，前准备时间不变 //前准备时间为0 2017-12-13                            
        //                //    if (this.resource.iSchBatch <= 1 && as_SchProductRouteRes.iActResReqQty > 0)
        //                //        ai_ResPreTime = (int)(as_SchProductRouteRes.iResPreTime);
        //                //    ai_ResPreTime = 0;
        //                //    else
        //                //    {
        //                //        ai_ResPreTime = this.resource.GetChangeTime(as_SchProductRouteRes, ai_workTime, NoTimeRange.DBegTime, ref ai_CycTimeTol, bSchdule);
        //                //        //如果已下的任务,重取前准备时间为0，还是维持原来准备时间。 2017-06-18
        //                //        if (as_SchProductRouteRes.cWoNo != "" && ai_ResPreTime < as_SchProductRouteRes.iResPreTime)
        //                //            ai_ResPreTime = (int)(as_SchProductRouteRes.iResPreTime);
        //                //    }


        //                //    if (ai_ResPreTime > 0)
        //                //    {
        //                //        int K = 0;
        //                //    }
        //                //    计算换产时间包含换刀时间
        //                //    ai_ResPreTime += ai_CycTimeTol;

        //                //    总加工时间 包含 换产时间包含换刀时间,再进行排产
        //                //    ai_workTime = Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime;  //Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime
        //                //    ai_workTimeDisTol = ai_workTime;
        //                //}

        //                //dtTaskBegDate = NoTimeRange.DEndTime;     //设置任务排产开始日期
        //                as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime;



        //            }


        //            //模拟排产时，没有排完，重新开始 //&& NoTimeRange.NotWorkTime < ai_workTime
        //            if (bSchdule == false) //没有排完 //&& NoTimeRange.NotWorkTime < ai_workTime
        //            {
        //                ////排到本时间段时，检查如果没有排完，但遇到到了已排任务，则重新选任务结束时间，重新开始排产
        //                //if (bSchdule == false) //没有排完 //&& NoTimeRange.NotWorkTime < ai_workTime
        //                //{
        //                //    ////有其他排产任务,而且排不下时 2019-09-09 Jonas Cheng 
        //                //    if (this.WorkTimeRangeList.Count > 0 && this.WorkTimeRangeList[0].DEndTime >= NoTimeRange.DEndTime)
        //                //    {
        //                //        bFirtTime = true;   //是否第一个排产时间段
        //                //        ai_workTime = ai_workTimeTask;            //返回原值      
        //                //        adCanEndDate = this.WorkTimeRangeList[0].DBegTime; //this.DEndTime;             //重置可开工时间
        //                //    }

        //                //}

        //                //空闲时间段后面有任务，而且当前任务没有排完，当前时间段排不下，重新再排
        //                if (NoTimeRange.taskTimeRangePre != null && ai_workTime > NoTimeRange.AvailableTime)
        //                {
        //                    bFirtTime = true;   //是否第一个排产时间段
        //                    ai_workTime = ai_workTimeTask;            //返回原值      
        //                                                              //adCanBegDate = this.WorkTimeRangeList[this.WorkTimeRangeList.Count - 1].DEndTime; //this.DEndTime;             //重置可开工时间
        //                    adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;  
        //                    adCanBegDateTask = NoTimeRange.DBegTime;
        //                }
        //                else
        //                {
        //                    //adCanEndDate = NoTimeRange.DBegTime;
        //                    //adCanEndDate = NoTimeRange.DEndTime;
        //                }

        //                //模拟排产时，未排完，遇到有其他已排任务，则整个任务重排
        //                if (NoTimeRange.cTaskType == 1 && ai_workTime > 0)
        //                {
        //                    bFirtTime = true;          //是否第一个排产时间段
        //                    ai_workTime = ai_workTimeTask;            //返回原值
        //                    adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;        //重排可开始时间，重当前时段点开始,后面会累加更新
        //                    adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
        //                    continue;
        //                }
        //            }


        //            //排产时间段小于5分钟( 5* 60 )，而且没有排完，则该空闲时段因为太小，不排产
        //            if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime && ai_workTime > 0 && NoTimeRange.AvailableTime < ai_workTime)
        //            {
        //                continue;
        //            }


        //            TaskTimeRange taskTimeRange1 = new TaskTimeRange();
        //            taskTimeRange1.cTaskType = 1;  //工作
        //            taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
        //            taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
        //            taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
        //            taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
        //            taskTimeRange1.cResourceNo = this.cResourceNo;
        //            taskTimeRange1.resource = this.resource;                     //资源对象
        //            taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
        //            taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
        //            taskTimeRange1.resTimeRange = this;


        //            //SchParam.Debug("3.22222222222 TimeSchTaskRev", "资源运算");

        //            //有排产，在本涵数里面修改，是否首次排产，以免多次重算前准备时间 2019-09-11
        //            if (bFirtTime)
        //                bFirtTime = false;

        //            //可用时间段小于时段结束时间,整个时间段都可用
        //            if (NoTimeRange.DEndTime <= adCanEndDate)
        //            {

        //                //整个空闲时段都占用
        //                if (ai_workTime > NoTimeRange.AvailableTime)
        //                {
        //                    taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime;

        //                    ai_workTime -= NoTimeRange.AvailableTime;
        //                }
        //                else        ////部分占用,倒排,从结束往前排
        //                {
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime; //NoTimeRange.DEndTime;  2020-03-28
        //                    taskTimeRange1.DBegTime = NoTimeRange.DEndTime.AddSeconds(-ai_workTime);
        //                    taskTimeRange1.AllottedTime = ai_workTime;

        //                    ai_workTime = 0;        //剩余待分配工作时间为0 
        //                }
        //            }
        //            else                // 可用时间小于时段结束时间,后面部分时间段不可用
        //            {
        //                if (NoTimeRange.DBegTime < adCanEndDate)
        //                {
        //                    TimeSpan lTimeSpan = (adCanEndDate - NoTimeRange.DBegTime);
        //                    int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);

        //                    //整个空闲时段都占用，未排完
        //                    if (ai_workTime > iAvailableTime)
        //                    {
        //                        taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                        taskTimeRange1.DEndTime = adCanEndDate;
        //                        taskTimeRange1.AllottedTime = iAvailableTime;

        //                        ai_workTime -= iAvailableTime;
        //                    }
        //                    else        ////部分占用,排完
        //                    {
        //                        taskTimeRange1.DBegTime = adCanEndDate.AddSeconds(-ai_workTime);
        //                        taskTimeRange1.DEndTime = adCanEndDate;
        //                        taskTimeRange1.AllottedTime = ai_workTime;

        //                        ai_workTime = 0;        //剩余待分配工作时间为0 
        //                    }
        //                }
        //                else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
        //                {
        //                    continue;
        //                }
        //            }



        //            if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
        //            {
        //                //MessageBox.Show("数据异常，生成新任务开始时间不对!", "提示")
        //                throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
        //                return -1;
        //            }

        //            if (taskTimeRange1.DBegTime < NoTimeRange.DBegTime && taskTimeRange1.DBegTime > NoTimeRange.DEndTime)
        //            {
        //                //MessageBox.Show("数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!", "提示");
        //                throw new Exception(string.Format("数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!"));
        //                return -1;
        //            }

        //            if (bSchdule)  //正式排程
        //            {
        //                if (taskTimeRange1.AllottedTime == 0)
        //                {
        //                    int K = 0;
        //                    continue;
        //                }

        //                //正式排程,排程任务倒排
        //                if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
        //                {
        //                    int m = 1;
        //                    throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
        //                    return -1;
        //                }

        //                //倒排 在原时间段中，插入新增加的工作任务时段,this.resource.TaskTimeRangeList同步增加任务
        //                TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);

        //                //正式排程,排程任务倒
        //                if (this.cIsInfinityAbility != "1" && (this.AllottedTime + this.AvailableTime > this.HoldingTime))
        //                {
        //                    int m = 1;
        //                    throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
        //                    return -1;
        //                }

        //                ////资源所有任务时段列表中增加,不用再增加
        //                //this.resource.TaskTimeRangeList.Add(taskTimeRange1);

        //                //资源任务对象中，增加任务已排时间段 不确定能否同时增加
        //                as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
        //            }


        //            //倒排 下个结束时间为当前任务的开始时间
        //            adCanEndDate = taskTimeRange1.DBegTime;
        //            bFirtTime = false;      //是否第一个排产时间段,不是第一个

        //            if (bSchdule)  //正式排程
        //            {
        //                //排程完，检查时段数据是否正确 ,产能无限的资源不用检查
        //                if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //                {
        //                    throw new Exception("出错位置：TimeSchTaskRev, 订单行号：" + as_SchProductRouteRes.iSchSdID + "产品编号[" + as_SchProductRouteRes.schProductRoute.cInvCode + "]加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],时段日期[" + this.DBegTime.ToShortDateString() + " " + this.DBegTime.ToLongTimeString() + "]检查异常！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //                    return -1;
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }

        //    return ai_workTime; //剩下未排时间

        //}

        ////无限产能倒排 给时段分配任务，生成任务时段占用列表，更新时段状态.ai_workTime 可以返回值（减去本时段的可用时间剩余值） //bSchdule = true正式排程，false模拟排程
        //public int TimeSchTaskRevInfinite(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanEndDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref Boolean bFirtTime)
        //{
        //    int taskallottedTime = 0;   //任务在本时间段内 总安排时间
        //    int ai_workTimeOld = 0;     //用于记录ai_workTime值，当空闲时间段太下时，如10分钟，则该时间段不排，ai_workTime值还原。
        //    //Boolean bFirtTime = true;   //是否第一个排产时间段

        //    //已排序，由大到小
        //    List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanEndDate, true, bSchdule, ai_workTime, false);

        //    if (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
        //    {
        //        int ii = 1;
        //    }

        //    //cSchType '1' 有限产能倒排程完，检查时段数据是否正确；'2' 新增加无限产能倒排，不检查 2020-04-04            
        //    //if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType == "1" && !CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //    //{
        //    //    throw new Exception("出错位置：倒排排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //    //    return -1;
        //    //}

        //    try
        //    {
        //        //给每个空闲时间段，分配任务。注意：模拟排产时，这里包含已排任务的时段，得跳过重来
        //        for (int i = 0; i < NoTimeRangeList.Count; i++)
        //        {
        //            ////记录排产工时的原始值
        //            //ai_workTimeOld = ai_workTime;

        //            if (ai_workTime <= 0)
        //            {
        //                ai_workTime = 0;
        //                break;
        //            }

        //            TaskTimeRange NoTimeRange = NoTimeRangeList[i];


        //            if (bFirtTime)   //是第一个排产时间段,计算换产时间
        //            {
        //                //if (NoTimeRange.AvailableTime == 0) continue;    //是排第一个时段，期该时段没有可用时间，则继续 

        //                ////如果中间有空隙可用时间时，前面的空隙时间不可用。不是最后一个时间段,而且后一个时间段还是已排的任务
        //                ////if (i + 1 < NoTimeRangeList.Count && NoTimeRangeList[i + 1].cTaskType != 0 && NoTimeRange.AvailableTime < ai_workTime)
        //                //if (i + 1 < NoTimeRangeList.Count && NoTimeRangeList[i + 1].cTaskType != 0 && NoTimeRange.AvailableTime < ai_workTime)
        //                //{
        //                //    if (bSchdule == false)  //模拟排产
        //                //    {
        //                //        adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;  
        //                //        adCanBegDateTask = NoTimeRange.DBegTime;
        //                //    }
        //                //    continue;

        //                //}

        //                ///倒排正常应该要重算前准备时间的
        //                //if (bReCalWorkTime)   //重新计算前准备时间，已下达生产任务单，不用重新计算 bReCalWorkTime = false
        //                //{
        //                //    ai_CycTimeTol = 0;   //设为0

        //                //    计算换产时间 和 换刀时间,已经开工的任务，前准备时间不变 //前准备时间为0 2017-12-13                            
        //                //    if (this.resource.iSchBatch <= 1 && as_SchProductRouteRes.iActResReqQty > 0)
        //                //        ai_ResPreTime = (int)(as_SchProductRouteRes.iResPreTime);
        //                //    ai_ResPreTime = 0;
        //                //    else
        //                //    {
        //                //        ai_ResPreTime = this.resource.GetChangeTime(as_SchProductRouteRes, ai_workTime, NoTimeRange.DBegTime, ref ai_CycTimeTol, bSchdule);
        //                //        //如果已下的任务,重取前准备时间为0，还是维持原来准备时间。 2017-06-18
        //                //        if (as_SchProductRouteRes.cWoNo != "" && ai_ResPreTime < as_SchProductRouteRes.iResPreTime)
        //                //            ai_ResPreTime = (int)(as_SchProductRouteRes.iResPreTime);
        //                //    }


        //                //    if (ai_ResPreTime > 0)
        //                //    {
        //                //        int K = 0;
        //                //    }
        //                //    计算换产时间包含换刀时间
        //                //    ai_ResPreTime += ai_CycTimeTol;

        //                //    总加工时间 包含 换产时间包含换刀时间,再进行排产
        //                //    ai_workTime = Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime;  //Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime
        //                //    ai_workTimeDisTol = ai_workTime;
        //                //}

        //                //dtTaskBegDate = NoTimeRange.DEndTime;     //设置任务排产开始日期
        //                as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime;



        //            }


        //            //模拟排产时，没有排完，重新开始 //&& NoTimeRange.NotWorkTime < ai_workTime
        //            if (bSchdule == false) //没有排完 //&& NoTimeRange.NotWorkTime < ai_workTime
        //            {
        //                ////排到本时间段时，检查如果没有排完，但遇到到了已排任务，则重新选任务结束时间，重新开始排产
        //                //if (bSchdule == false) //没有排完 //&& NoTimeRange.NotWorkTime < ai_workTime
        //                //{
        //                //    ////有其他排产任务,而且排不下时 2019-09-09 Jonas Cheng 
        //                //    if (this.WorkTimeRangeList.Count > 0 && this.WorkTimeRangeList[0].DEndTime >= NoTimeRange.DEndTime)
        //                //    {
        //                //        bFirtTime = true;   //是否第一个排产时间段
        //                //        ai_workTime = ai_workTimeTask;            //返回原值      
        //                //        adCanEndDate = this.WorkTimeRangeList[0].DBegTime; //this.DEndTime;             //重置可开工时间
        //                //    }

        //                //}

        //                ////空闲时间段后面有任务，而且当前任务没有排完，当前时间段排不下，重新再排
        //                //if (NoTimeRange.taskTimeRangePre != null && ai_workTime > NoTimeRange.AvailableTime)
        //                //{
        //                //    bFirtTime = true;   //是否第一个排产时间段
        //                //    ai_workTime = ai_workTimeTask;            //返回原值      
        //                //                                              //adCanBegDate = this.WorkTimeRangeList[this.WorkTimeRangeList.Count - 1].DEndTime; //this.DEndTime;             //重置可开工时间
        //                //    adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;  
        //                //    adCanBegDateTask = NoTimeRange.DBegTime;
        //                //}
        //                //else
        //                //{
        //                //    //adCanEndDate = NoTimeRange.DBegTime;
        //                //    //adCanEndDate = NoTimeRange.DEndTime;
        //                //}

        //                ////模拟排产时，未排完，遇到有其他已排任务，则整个任务重排
        //                //if (NoTimeRange.cTaskType == 1 && ai_workTime > 0)
        //                //{
        //                //    bFirtTime = true;          //是否第一个排产时间段
        //                //    ai_workTime = ai_workTimeTask;            //返回原值
        //                //    adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;        //重排可开始时间，重当前时段点开始,后面会累加更新
        //                //    adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
        //                //    continue;
        //                //}
        //            }


        //            ////排产时间段小于5分钟( 5* 60 )，而且没有排完，则该空闲时段因为太小，不排产
        //            //if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime && ai_workTime > 0 && NoTimeRange.AvailableTime < ai_workTime)
        //            //{
        //            //    continue;
        //            //}


        //            TaskTimeRange taskTimeRange1 = new TaskTimeRange();
        //            taskTimeRange1.cTaskType = 1;  //工作
        //            taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
        //            taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
        //            taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
        //            taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
        //            taskTimeRange1.cResourceNo = this.cResourceNo;
        //            taskTimeRange1.resource = this.resource;                     //资源对象
        //            taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
        //            taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
        //            taskTimeRange1.resTimeRange = this;


        //            //有排产，在本涵数里面修改，是否首次排产，以免多次重算前准备时间 2019-09-11
        //            if (bFirtTime)
        //                bFirtTime = false;

        //            //可用时间段小于时段结束时间,整个时间段都可用
        //            if (NoTimeRange.DEndTime <= adCanEndDate)
        //            {

        //                //整个空闲时段都占用
        //                if (ai_workTime > NoTimeRange.HoldingTime)
        //                {
        //                    taskTimeRange1.AllottedTime = NoTimeRange.HoldingTime;
        //                    taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime;

        //                    ai_workTime -= NoTimeRange.HoldingTime;
        //                }
        //                else        ////部分占用,倒排,从结束往前排
        //                {
        //                    taskTimeRange1.DEndTime = NoTimeRange.DEndTime; //NoTimeRange.DEndTime;  2020-03-28
        //                    taskTimeRange1.DBegTime = NoTimeRange.DEndTime.AddSeconds(-ai_workTime);
        //                    taskTimeRange1.AllottedTime = ai_workTime;

        //                    ai_workTime = 0;        //剩余待分配工作时间为0 
        //                }
        //            }
        //            else                // 可用时间小于时段结束时间,后面部分时间段不可用
        //            {
        //                if (NoTimeRange.DBegTime < adCanEndDate)
        //                {
        //                    TimeSpan lTimeSpan = (adCanEndDate - NoTimeRange.DBegTime);
        //                    int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);

        //                    //整个空闲时段都占用，未排完
        //                    if (ai_workTime > iAvailableTime)
        //                    {
        //                        taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
        //                        taskTimeRange1.DEndTime = adCanEndDate;
        //                        taskTimeRange1.AllottedTime = iAvailableTime;

        //                        ai_workTime -= iAvailableTime;
        //                    }
        //                    else        ////部分占用,排完
        //                    {
        //                        taskTimeRange1.DBegTime = adCanEndDate.AddSeconds(-ai_workTime);
        //                        taskTimeRange1.DEndTime = adCanEndDate;
        //                        taskTimeRange1.AllottedTime = ai_workTime;

        //                        ai_workTime = 0;        //剩余待分配工作时间为0 
        //                    }
        //                }
        //                else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
        //                {
        //                    continue;
        //                }
        //            }



        //            if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
        //            {
        //                //MessageBox.Show("数据异常，生成新任务开始时间不对!", "提示");
        //                throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));

        //                return -1;
        //            }

        //            if (taskTimeRange1.DBegTime < NoTimeRange.DBegTime && taskTimeRange1.DBegTime > NoTimeRange.DEndTime)
        //            {
        //                //MessageBox.Show("数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!", "提示")
        //                throw new Exception(string.Format("数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!"));
        //                return -1;
        //            }

        //            if (bSchdule)  //正式排程
        //            {
        //                if (taskTimeRange1.AllottedTime == 0)
        //                {
        //                    int K = 0;
        //                    continue;
        //                }
        //                //无限产能排产，结果不改变资源工作任务时段。
        //                //在原时间段中，插入新增加的工作任务时段,this.resource.TaskTimeRangeList同步增加任务
        //                //TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);

        //                ////资源所有任务时段列表中增加,不用再增加
        //                //this.resource.TaskTimeRangeList.Add(taskTimeRange1);

        //                //资源任务对象中，增加任务已排时间段 不确定能否同时增加
        //                as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
        //            }


        //            //倒排 下个结束时间为当前任务的开始时间
        //            adCanEndDate = taskTimeRange1.DBegTime;
        //            bFirtTime = false;      //是否第一个排产时间段,不是第一个

        //            //无限产能排产，不用检查任务时间段。
        //            //if (bSchdule)  //正式排程
        //            //{
        //            //    //排程完，检查时段数据是否正确 ,产能无限的资源不用检查
        //            //    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //            //    {
        //            //        throw new Exception("出错位置：TimeSchTaskRev, 订单行号：" + as_SchProductRouteRes.iSchSdID + "产品编号[" + as_SchProductRouteRes.schProductRoute.cInvCode + "]加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],时段日期[" + this.DBegTime.ToShortDateString() + " " + this.DBegTime.ToLongTimeString() + "]检查异常！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //            //        return -1;
        //            //    }
        //            //}

        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }

        //    return ai_workTime; //剩下未排时间

        //}


        ////检查资源时段内任务时间是否正确,空闲时间 = 可用时间
        //public Boolean CheckResTimeRange()
        //{

        //    //无限产能不用检查
        //    if (this.cIsInfinityAbility == "1") return true;

        //    //检查可用时间 和 空闲时间是否相等
        //    if (System.Math.Abs(this.AvailableTime - this.NotWorkTime) > 5)  //不能直接相等，可能有计算误差 2022-06-22
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        return ThowErrText(string.Format("检查分配时间{0} > 空闲时间{1}", this.AllottedTime, this.NotWorkTime));    //小数点，有误差    
        //    }

        //    int liNotWorkTime = 0;

        //    //检查空闲时间和明细是否一致
        //    foreach (TaskTimeRange taskTimeRange in this.TaskTimeRangeList)
        //    {
        //        liNotWorkTime += taskTimeRange.NotWorkTime;
        //    }

        //    if (System.Math.Abs(liNotWorkTime - this.NotWorkTime) > 5)
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        return ThowErrText(string.Format("检查空闲时间{0}与明细汇总{1}不一致", this.NotWorkTime, liNotWorkTime));    //小数点，有误差   
        //    }


        //    //检查分配时间 + 空闲时间 是否等于时间段总长
        //    if (System.Math.Abs(this.AllottedTime + this.NotWorkTime - this.HoldingTime) > 5)
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        return ThowErrText(string.Format("检查分配时间{0} + 空闲时间{1} 大于 时间段总长{2}", this.AllottedTime, this.NotWorkTime, this.HoldingTime));    //小数点，有误差   
        //    }

        //    //检查排程时间段，是否与其他已排任务由重叠，资源甘特图中有重复2019-08-30

        //    return true;

        //}

        ////显示出错信息 2019-12-22 JonasCheng
        //public Boolean ThowErrText(string ErrText)
        //{
        //    //Clipboard.SetText(this.ToString());
        //    throw new Exception("出错位置：排程时段检查出错TimeSchTask.ThowErrText！" + ErrText); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //    return false;    //小数点，有误差      

        //}

        ////传入一个时间点,返回时段内的可用时间段,区分正排,往前找一个空闲时间段；倒排,往后找一个空闲时间段
        //public TaskTimeRange GetAvailableTimeRange(DateTime adCanBegDate, Boolean bSchRev = false)
        //{
        //    ////之前没有已排任务记录,先生成一个无任务的时间段,保证每个时间段都有任务
        //    //if (TaskTimeRangeList.Count == 0)
        //    //{
        //    //    //NoTaskTime任务对应的时段
        //    //    TaskTimeRange NoTaskTime = GetNoWorkTaskTimeRange(this.DBegTime, this.DEndTime);
        //    //    TaskTimeRangeList.Add(NoTaskTime);
        //    //    //this.resource.TaskTimeRangeList.Add(NoTaskTime);
        //    //    this.NotWorkTime += NoTaskTime.NotWorkTime;

        //    //    return NoTaskTime;
        //    //}

        //    //TaskTimeRange lTaskTimeRange = new TaskTimeRange();
        //    TaskTimeRange lTaskTimeRangeFind = null;
        //    List<TaskTimeRange> TaskTimeRangeTemp = new List<TaskTimeRange>(10);

        //    //获取可用时间段，检查时段数据是否正确，2021-11-17 
        //    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //        return null;
        //    }

        //    if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用
        //    {
        //        TaskTimeRangeTemp = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.cTaskType == 0; });
        //        //取最后一条空闲记录
        //        if (TaskTimeRangeTemp.Count > 0)
        //            lTaskTimeRangeFind = TaskTimeRangeTemp[TaskTimeRangeTemp.Count - 1];
        //    }
        //    else                                 //产能有限
        //    {
        //        if (bSchRev)  //倒排
        //        {
        //            //查找任务时段大于adCanBegDate，而且是空闲的时段
        //            TaskTimeRangeTemp = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return (p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate || p2.DEndTime < adCanBegDate) && p2.cTaskType == 0; });
        //            //取最后一条空闲记录
        //            if (TaskTimeRangeTemp.Count > 0)
        //                lTaskTimeRangeFind = TaskTimeRangeTemp[TaskTimeRangeTemp.Count - 1];
        //        }
        //        else          //正排
        //        {
        //            //查找任务时段大于adCanBegDate，而且是空闲的时段
        //            TaskTimeRangeTemp = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return (p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate || p2.DBegTime > adCanBegDate) && p2.cTaskType == 0; });
        //            //取第一条空闲记录
        //            if (TaskTimeRangeTemp.Count > 0)
        //                lTaskTimeRangeFind = TaskTimeRangeTemp[0];
        //        }
        //    }



        //    //找到了当期时间点对应的任务，可能是空闲，也有可能是占用的          
        //    return lTaskTimeRangeFind;

        //}

        ////传入一个时间点,返回可用时间段列表 bSchRev false 正排; true 倒排 ,加工时间  bIncludeWorkTime 是否包含已排任务时间段 
        //public List<TaskTimeRange> GetAvailableTimeRangeList(DateTime adCanBegDate, Boolean bSchRev, Boolean bSchdule = true, int ai_workTime = 0, Boolean bIncludeWorkTime = true)
        //{
        //    //之前没有已排任务记录,先生成一个无任务的时间段,保证每个时间段都有任务
        //    //if (TaskTimeRangeList.Count == 0)
        //    //{
        //    //    //NoTaskTime任务对应的时段
        //    //    TaskTimeRange NoTaskTime = GetNoWorkTaskTimeRange(this.DBegTime, this.DEndTime);
        //    //    TaskTimeRangeList.Add(NoTaskTime);
        //    //    return TaskTimeRangeList;

        //    //}
        //    //正式排产时，为了提高效率，不包含已排任务 2020-06-24
        //    if (bSchdule && bIncludeWorkTime) bIncludeWorkTime = false;

        //    List<TaskTimeRange> lTaskTimeRangeList = new List<TaskTimeRange>();

        //    if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用
        //    {
        //        lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.cTaskType == 0; });
        //        //排序，由小到大
        //        lTaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
        //    }
        //    else                                 //产能有限
        //    {
        //        //查找任务时段大于adCanBegDate，而且是空闲的时段
        //        if (bSchRev)  //倒排
        //        {
        //            if (bSchdule)  //正式排产
        //            {
        //                //lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.cTaskType == 0 && (p2.DBegTime == this.DBegTime || p2.AvailableTime >= ai_workTime); });
        //                lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.cTaskType == 0; });

        //                ////模拟排产时，要确保任务能被排下，包含已排任务
        //                //if (bIncludeWorkTime)
        //                //    lTaskTimeRangeList.AddRange(this.WorkTimeRangeList);
        //            }
        //            else           //模拟排产，取所有字段
        //            {
        //                //lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.cTaskType == 0 && (p2.DBegTime == this.DBegTime || p2.AvailableTime >= ai_workTime); });
        //                lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.cTaskType == 0; });

        //                //模拟排产时，要确保任务能被排下，包含已排任务
        //                if (bIncludeWorkTime)
        //                    lTaskTimeRangeList.AddRange(this.WorkTimeRangeList);

        //                //TaskTimeRange taskTimeRangeWork;
        //                ////同时返回大于当前空白时间段的后一个已排任务的明细，用于模拟排产 2019-07-07
        //                //int iCount = lTaskTimeRangeList.Count;
        //                //for (int i = 0; i < iCount; i++)
        //                //{
        //                //    //每个空闲时间段后面，找一个已排任务，防止任务跨越。2020-01-07 
        //                //    if (i < iCount - 1)
        //                //        taskTimeRangeWork = WorkTimeRangeList.Find(delegate (TaskTimeRange p2) { return p2.DEndTime <= lTaskTimeRangeList[i].DEndTime && p2.DEndTime > lTaskTimeRangeList[i + 1].DEndTime && p2.cTaskType != 0; });
        //                //    else
        //                //        taskTimeRangeWork = WorkTimeRangeList.Find(delegate(TaskTimeRange p2) { return p2.DEndTime <= lTaskTimeRangeList[i].DEndTime && p2.cTaskType != 0; });

        //                //    //当前任务后面有已排产的任务，必须要求空闲时段可用时间AvailableTime大于排产时间ai_workTime
        //                //    if (taskTimeRangeWork != null)
        //                //        if (lTaskTimeRangeList[i].AvailableTime >= ai_workTime)
        //                //            lTaskTimeRangeList.Add(taskTimeRangeWork);
        //                //        else    //空闲时间段太小,不考虑
        //                //        {
        //                //            lTaskTimeRangeList.Remove(lTaskTimeRangeList[i]);
        //                //            i--;
        //                //        }
        //                //}

        //            }

        //            //排序，由大到小
        //            lTaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
        //        }
        //        else          //正排
        //        {
        //            if (bSchdule)  //正式排产
        //            {
        //                //lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return p2.DEndTime > adCanBegDate && p2.cTaskType == 0 && (p2.DEndTime == this.DEndTime || p2.AvailableTime >= ai_workTime); });
        //                lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DEndTime > adCanBegDate && p2.cTaskType == 0; });

        //                ////模拟排产时，要确保任务能被排下，包含已排任务,冻结排产时
        //                //if (bIncludeWorkTime)
        //                //    lTaskTimeRangeList.AddRange(this.WorkTimeRangeList);
        //            }
        //            else           //模拟排产，取所有字段
        //            {
        //                //lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return p2.DEndTime > adCanBegDate && p2.cTaskType == 0 && (p2.DEndTime == this.DEndTime || p2.AvailableTime >= ai_workTime); });
        //                lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DEndTime > adCanBegDate && p2.cTaskType == 0; });


        //                //同时返回大于当前空白时间段的后一个已排任务的明细，用于模拟排产 2019-07-07

        //                int iCount = lTaskTimeRangeList.Count;

        //                if (iCount < 1) //没有空闲时间段，说明已经排满了任务,正排返回第1条已排任务  2019-09-10
        //                {
        //                    if (WorkTimeRangeList.Count > 0)
        //                        lTaskTimeRangeList.Add(WorkTimeRangeList[0]);
        //                }
        //                else
        //                {
        //                    //模拟排产时，要确保任务能被排下，包含已排任务
        //                    if (bIncludeWorkTime)
        //                        lTaskTimeRangeList.AddRange(this.WorkTimeRangeList);

        //                    //TaskTimeRange taskTimeRangeWork;

        //                    //for (int i = 0; i < iCount; i++)
        //                    //{
        //                    //    ////所有已排任务都放入待排任务清单
        //                    //    //if (WorkTimeRangeList.Count > 0)
        //                    //    //    lTaskTimeRangeList.AddRange(WorkTimeRangeList);

        //                    //    //每个空闲时间段后面，找一个已排任务，防止任务跨越。2020-01-07 
        //                    //    if (i < iCount - 1  )
        //                    //        taskTimeRangeWork = WorkTimeRangeList.Find(delegate (TaskTimeRange p2) { return p2.DBegTime >= lTaskTimeRangeList[i].DBegTime && p2.DBegTime < lTaskTimeRangeList[i + 1].DBegTime && p2.cTaskType != 0; });
        //                    //    else
        //                    //        taskTimeRangeWork = WorkTimeRangeList.Find(delegate(TaskTimeRange p2) { return p2.DBegTime >= lTaskTimeRangeList[i].DBegTime && p2.cTaskType != 0; });

        //                    //    //当前任务后面有已排产的任务，必须要求空闲时段可用时间AvailableTime大于排产时间ai_workTime
        //                    //    if (taskTimeRangeWork != null)
        //                    //        lTaskTimeRangeList.Add(taskTimeRangeWork);   //不管大小都应该加上已排任务

        //                    //    //if (lTaskTimeRangeList[i].AvailableTime >= ai_workTime)
        //                    //    //    lTaskTimeRangeList.Add(taskTimeRangeWork);
        //                    //    //else    //空闲时间段太小,不考虑
        //                    //    //{
        //                    //    //    lTaskTimeRangeList.Remove(lTaskTimeRangeList[i]);
        //                    //    //    i--;
        //                    //    //}
        //                    //}
        //                }

        //            }
        //            //排序，由小到大
        //            if (lTaskTimeRangeList.Count > 1)
        //                lTaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
        //        }
        //    }

        //    return lTaskTimeRangeList;
        //}

        ////传入一个时间点,返回最早可用时间
        //public DateTime GetAvailableTime(DateTime adCanBegDate, Boolean bSchRev = false)
        //{

        //    TaskTimeRange lTaskTimeRange = GetAvailableTimeRange(adCanBegDate, bSchRev);

        //    if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用
        //    {
        //        return adCanBegDate;
        //    }
        //    else                                 //产能有限
        //    {
        //        if (bSchRev) //倒排，返回最大的可用时间
        //        {
        //            //找到了当期时间点对应的任务，可能是空闲，也有可能是占用的
        //            if (lTaskTimeRange != null)
        //            {
        //                return lTaskTimeRange.DEndTime;
        //            }
        //            return adCanBegDate;
        //        }
        //        else
        //        {
        //            //找到了当期时间点对应的任务，可能是空闲，也有可能是占用的
        //            if (lTaskTimeRange != null)
        //            {
        //                return lTaskTimeRange.DBegTime;
        //            }
        //            return DateTime.Today;
        //        }
        //    }
        //}

        ////传入一个时间点,返回对应的时段ResTimeRange
        //public ResTimeRange GetResTimeRange(DateTime adCanBegDate, string as_Type = "1")  //"1" 当前时段，"2" 上一时段 ,"3" 下一时段
        //{
        //    List<ResTimeRange> lResTimeRangeList = this.resource.ResTimeRangeList.FindAll(delegate (ResTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate; });

        //    if (lResTimeRangeList.Count > 0)
        //        return lResTimeRangeList[0];
        //    else
        //        return null;
        //}

        ////分割任务对象，传入一个总的任务对象，一个插入的任务对象，结果写入TaskTimeRangeList
        //public int TaskTimeRangeSplit(TaskTimeRange aToltalTaskRange, TaskTimeRange aNewTaskRange)
        //{

        //    TaskTimeRange NoTaskTime1, NoTaskTime2;


        //    if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用 ,aToltalTaskRange不变
        //    {
        //        //增加新任务占用时间段，aToltalTaskRange不变
        //        WorkTimeRangeList.Add(aNewTaskRange);
        //        //this.resource.TaskTimeRangeList.Add(aNewTaskRange);
        //        //this.AllottedTime += aNewTaskRange.AllottedTime;   //已分配时间增加
        //    }
        //    else                                 //产能有限
        //    {
        //        //if (dBegDate >= dEndDate)
        //        //{
        //        //    throw new Exception("生成空闲任务时间段必须大于0！");
        //        //    return NoTaskTime;
        //        //}
        //        try
        //        {
        //            if (aNewTaskRange.iProcessProductID == SchParam.iProcessProductID && aNewTaskRange.iSchSdID == SchParam.iSchSdID)
        //            {
        //                int i = 1;
        //            }


        //            //正排程完，检查时段数据是否正确
        //            if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //            {
        //                //Clipboard.SetText(this.ToString());
        //                throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //                return -1;
        //            }

        //            //if (this.AllottedTime + aNewTaskRange.HoldingTime > this.HoldingTime)
        //            //{
        //            //    throw new Exception(string.Format("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit,累计分配时间段{0}大于总时间段{1}", (this.AllottedTime + aNewTaskRange.AllottedTime).ToString(), this.HoldingTime.ToString())); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //            //    return -1;
        //            //}
        //            if (this.AllottedTime + aNewTaskRange.AllottedTime > this.HoldingTime)  //AllottedTime
        //            {
        //                //Clipboard.SetText(this.ToString());
        //                throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", aToltalTaskRange.dBegTime.ToString(), aToltalTaskRange.dEndTime.ToString(), aNewTaskRange.dBegTime.ToString(), aNewTaskRange.dEndTime.ToString()));
        //                return -1;
        //            }

        //            //要插入的任务全部占用,两任务时间段相同
        //            if (aToltalTaskRange.DBegTime == aNewTaskRange.DBegTime && aToltalTaskRange.DEndTime == aNewTaskRange.DEndTime)
        //            {
        //                //TaskTimeRangeList.Remove(aToltalTaskRange);   //空闲任务中删除该分段
        //                //WorkTimeRangeList.Add(aNewTaskRange);      //已排任务中增加该分段

        //                this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, null);

        //            }
        //            else if (aToltalTaskRange.DBegTime == aNewTaskRange.DBegTime)    //时间段头相同
        //            {

        //                ////增加新任务占用时间段
        //                //WorkTimeRangeList.Add(aNewTaskRange);

        //                NoTaskTime1 = GetNoWorkTaskTimeRange(aNewTaskRange.DEndTime, aToltalTaskRange.DEndTime);
        //                ////更新原来时间段
        //                //CopyTaskTimeRange(aToltalTaskRange, NoTaskTime1);

        //                this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1);

        //                aToltalTaskRange.taskTimeRangePre = aNewTaskRange;  //当前空闲时间段的前任务是aNewTaskRange 

        //            }
        //            else if (aToltalTaskRange.DEndTime == aNewTaskRange.DEndTime)   //时间段尾相同
        //            {

        //                //增加新任务占用时间段
        //                //WorkTimeRangeList.Add(aNewTaskRange);

        //                NoTaskTime1 = GetNoWorkTaskTimeRange(aToltalTaskRange.DBegTime, aNewTaskRange.DBegTime);
        //                //更新原来时间段
        //                //CopyTaskTimeRange(aToltalTaskRange, NoTaskTime1);

        //                this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1);

        //                aToltalTaskRange.taskTimeRangePost = aNewTaskRange;  //当前空闲时间段的后任务是aNewTaskRange 

        //            }
        //            else    ////时间段头、尾都不相同，分成三个时间段
        //            {

        //                //增加新任务占用时间段
        //                //WorkTimeRangeList.Add(aNewTaskRange);                        

        //                NoTaskTime1 = GetNoWorkTaskTimeRange(aToltalTaskRange.DBegTime, aNewTaskRange.DBegTime);
        //                NoTaskTime2 = GetNoWorkTaskTimeRange(aNewTaskRange.DEndTime, aToltalTaskRange.DEndTime);
        //                //更新原来时间段
        //                //CopyTaskTimeRange(aToltalTaskRange, NoTaskTime1);

        //                this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1, NoTaskTime2);

        //                aToltalTaskRange.taskTimeRangePost = aNewTaskRange;  //当前空闲时间段的后任务是aNewTaskRange 
        //                NoTaskTime2.taskTimeRangePre = aNewTaskRange;   //当前空闲时间段的前任务是aNewTaskRange 

        //                //int allottedTimeTemp = this.AllottedTime;
        //                //this.AllottedTime = allottedTimeTemp + aNewTaskRange.AllottedTime;    //已分配时间增加
        //                //this.NotWorkTime = this.NotWorkTime - aNewTaskRange.AllottedTime;     //空闲时间减少


        //                //增加空闲时间段
        //                //TaskTimeRangeList.Add(NoTaskTime2);
        //                //this.resource.TaskTimeRangeList.Add(NoTaskTime2);


        //            }


        //            //正排程完，检查时段数据是否正确
        //            if (this.cIsInfinityAbility != "1" && !CheckResTimeRange())
        //            {
        //                throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！时段开始时间:" + aToltalTaskRange.dBegTime.ToString() + "时段结束时间:" + aToltalTaskRange.dEndTime.ToString() + "任务开始时间:" + aNewTaskRange.dBegTime.ToString() + "任务结束时间:" + aNewTaskRange.dEndTime.ToString()); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //                return -1;
        //            }
        //        }
        //        catch (Exception exp)
        //        {
        //            throw exp;
        //        }
        //    }

        //    return 1;
        //}

        ////生成一个空闲的任务时间段
        //public TaskTimeRange GetNoWorkTaskTimeRange(DateTime dBegDate, DateTime dEndDate, Boolean bCreate = false)
        //{
        //    TaskTimeRange NoTaskTime = new TaskTimeRange();
        //    if (dBegDate >= dEndDate)
        //    {
        //        throw new Exception("生成空闲任务时间段必须大于0！");
        //        return NoTaskTime;
        //    }


        //    NoTaskTime.cVersionNo = "";
        //    NoTaskTime.cTaskType = 0;  //空闲
        //    NoTaskTime.iSchSdID = -1;
        //    NoTaskTime.iProcessProductID = -1;
        //    NoTaskTime.iResProcessID = -1;
        //    NoTaskTime.cResourceNo = this.cResourceNo;
        //    NoTaskTime.CIsInfinityAbility = this.CIsInfinityAbility;

        //    NoTaskTime.DBegTime = dBegDate;//dBegDate.AddSeconds(1);   //生成的空闲时间段增加1秒
        //    NoTaskTime.DEndTime = dEndDate;
        //    NoTaskTime.AllottedTime = 0;
        //    NoTaskTime.HoldingTime = (int)((TimeSpan)(NoTaskTime.DEndTime - NoTaskTime.DBegTime)).TotalSeconds;

        //    //NoTaskTime.NotWorkTime = NoTaskTime.HoldingTime;

        //    NoTaskTime.resource = this.resource;
        //    NoTaskTime.resTimeRange = this;
        //    NoTaskTime.schProductRouteRes = null;
        //    NoTaskTime.schData = this.schData;


        //    if (bCreate) //创建一个空的任务时间段,初始化时调用
        //    {
        //        NoTaskTime.AddTaskTimeRange(this);

        //        //this.TaskTimeRangeList.Add(NoTaskTime);
        //        //this.NotWorkTime += NoTaskTime.NotWorkTime;
        //    }

        //    //if (!CheckResTimeRange())
        //    //{
        //    //    return null ;
        //    //}

        //    return NoTaskTime;
        //}

        ////复制任务时间段，返回aOldTaskRange
        //public TaskTimeRange CopyTaskTimeRange(TaskTimeRange aOldTaskRange, TaskTimeRange aNewTaskRange)
        //{
        //    aOldTaskRange.cVersionNo = aNewTaskRange.cVersionNo;

        //    aOldTaskRange.iSchSdID = aNewTaskRange.iSchSdID;
        //    aOldTaskRange.iProcessProductID = aNewTaskRange.iProcessProductID;
        //    aOldTaskRange.iResProcessID = aNewTaskRange.iResProcessID;
        //    aOldTaskRange.cResourceNo = this.cResourceNo;

        //    aOldTaskRange.DBegTime = aNewTaskRange.DBegTime;
        //    aOldTaskRange.DEndTime = aNewTaskRange.DEndTime;
        //    aOldTaskRange.HoldingTime = aNewTaskRange.HoldingTime;
        //    aOldTaskRange.AllottedTime = aNewTaskRange.AllottedTime;
        //    aOldTaskRange.NotWorkTime = 0;


        //    aOldTaskRange.resource = aNewTaskRange.resource;
        //    aOldTaskRange.resTimeRange = aNewTaskRange.resTimeRange;
        //    aOldTaskRange.schProductRouteRes = aNewTaskRange.schProductRouteRes;
        //    aOldTaskRange.schData = aNewTaskRange.schData;

        //    ////aOldTaskRange空闲时间段分配了任务aNewTaskRange  不需要
        //    //if ((aOldTaskRange.cTaskType != aNewTaskRange.cTaskType) && aOldTaskRange.cTaskType == 0)
        //    //{
        //    //     //重新计算已分配时间、空闲可用时间
        //    //    this.AllottedTime += aNewTaskRange.AllottedTime;    //已分配时间增加
        //    //    this.NotWorkTime -= aNewTaskRange.AllottedTime;     //空闲时间减少
        //    //}
        //    aOldTaskRange.cTaskType = aNewTaskRange.cTaskType;  //空闲

        //    //记录更新时间段的ID
        //    aOldTaskRange.iSchSdID = aNewTaskRange.iSchSdID;
        //    aOldTaskRange.iProcessProductID = aNewTaskRange.iProcessProductID;
        //    aOldTaskRange.iResProcessID = aNewTaskRange.iResProcessID;
        //    aOldTaskRange.iSchSNMax = SchParam.iSchSNMax;


        //    return aOldTaskRange;
        //}

        ////合并空闲时间段，任务删除后，会空出空闲时间段
        //public int MegTaskTimeRangeAll()
        //{
        //    if (this.TaskTimeRangeList.Count <= 1) return 1;

        //    TaskTimeRange TaskTimeRangePre = null;//第一个不合并，从第二个时段开始
        //    TaskTimeRange TaskTimeRangeNew = null;

        //    //开始日期由小到大排序
        //    this.TaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

        //    int iCount = this.TaskTimeRangeList.Count;


        //    int allottedTime = 0;

        //    for (int i = iCount - 1; i >= 0; i--)
        //    {
        //        TaskTimeRangeNew = this.TaskTimeRangeList[i];
        //        //连续两时段都空闲,当前时间段的结束时间 等于 下个时间段的开始时间 才能合并
        //        if (TaskTimeRangePre != null && TaskTimeRangePre.cTaskType == 0 && TaskTimeRangeNew.cTaskType == 0 && TaskTimeRangeNew.DEndTime == TaskTimeRangePre.DBegTime)
        //        {
        //            //合并时间段后，以最新的时间段，继续和后面的时间段合并 2021-11-18 
        //            TaskTimeRangeNew = MegTaskTimeRange(TaskTimeRangePre, TaskTimeRangeNew);

        //            if (TaskTimeRangeNew == null)
        //            {
        //                throw new Exception("检验任务合并时间段出错,位置ReTimeRange.MegTaskTimeRangeAll");
        //                return -1;
        //            }
        //        }

        //        TaskTimeRangePre = TaskTimeRangeNew;
        //    }

        //    ////重新设置可用时间allottedTime
        //    //this.AllottedTime = allottedTime;

        //    return 1;
        //}

        ////合并两个空闲时间段
        //public TaskTimeRange MegTaskTimeRange(TaskTimeRange TaskTimeRangeLast, TaskTimeRange TaskTimeRangeNew)
        //{
        //    if (TaskTimeRangeLast.cTaskType != 0) return null;    //时间大的时段
        //    if (TaskTimeRangeNew.cTaskType != 0) return null;

        //    TaskTimeRange NoWorkTaskTime = GetNoWorkTaskTimeRange(TaskTimeRangeNew.DBegTime, TaskTimeRangeLast.DEndTime, false);
        //    NoWorkTaskTime.taskTimeRangePre = TaskTimeRangeLast.taskTimeRangePre;  //前时间段的任务，写到后时间段任务
        //    NoWorkTaskTime.taskTimeRangePost = TaskTimeRangeNew.taskTimeRangePost;  //前时间段的任务，写到后时间段任务

        //    ////保留TaskTimeRangeNew，删除前面一个时段
        //    //TaskTimeRangeNew.DBegTime = TaskTimeRangeNew.DBegTime;
        //    //TaskTimeRangeNew.DEndTime = TaskTimeRangeLast.DEndTime;

        //    //2022 - 05 - 20 JonasCheng
        //    TaskTimeRangeNew.taskTimeRangePre = TaskTimeRangeLast.taskTimeRangePre;  //前时间段的任务，写到后时间段任务
        //    if (Math.Abs(TaskTimeRangeLast.HoldingTime + TaskTimeRangeNew.HoldingTime - NoWorkTaskTime.HoldingTime) > 5)  //不能直接相等，可能有计算误差 2022-06-22
        //    {
        //        string message = string.Format(@"2、原时间段长[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],新时间段长[{4}],未分配时间段长[{5}]",
        //                                                       TaskTimeRangeLast.HoldingTime.ToString(), TaskTimeRangeLast.iSchSdID, TaskTimeRangeLast.iProcessProductID, TaskTimeRangeLast.cResourceNo, TaskTimeRangeNew.HoldingTime.ToString(), NoWorkTaskTime.HoldingTime.ToString());
        //        SchParam.Debug(message, "资源运算");

        //        throw new Exception("检验任务拆分后时间段出错,位置ReTimeRange.MegTaskTimeRange!" + message);
        //        return null;
        //    }

        //    //检查删除后的时间段是否正确 2021-11-15 JonasCheng
        //    if (!this.CheckResTimeRange())
        //    {
        //        string message = string.Format(@"2、原时间段长[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],新时间段长[{4}],未分配时间段长[{5}]",
        //                                                      TaskTimeRangeLast.HoldingTime.ToString(), TaskTimeRangeLast.iSchSdID, TaskTimeRangeLast.iProcessProductID, TaskTimeRangeLast.cResourceNo, TaskTimeRangeNew.HoldingTime.ToString(), NoWorkTaskTime.HoldingTime.ToString());
        //        SchParam.Debug(message, "资源运算");
        //        //Clipboard.SetText(this.ToString());
        //        throw new Exception("清除任务时合并空闲时间段出错,位置ReTimeRange.MegTaskTimeRange！" + message);
        //        return null;
        //    }

        //    //this.TaskTimeRangeList.Remove(TaskTimeRangeLast);
        //    //this.TaskTimeRangeList.Remove(TaskTimeRangeNew);
        //    //this.TaskTimeRangeList.Add(NoWorkTaskTime);


        //    TaskTimeRangeLast.RemoveTaskTimeRange(this);
        //    TaskTimeRangeNew.RemoveTaskTimeRange(this);

        //    NoWorkTaskTime.AddTaskTimeRange(this);

        //    ////检查删除后的时间段是否正确 2021-11-15 JonasCheng  2022-6-02 先关闭
        //    //if (!this.CheckResTimeRange())
        //    //{
        //    //    //Clipboard.SetText(this.ToString());
        //    //    throw new Exception("清除任务时出错,倒排删除已排任务时间段,位置ReTimeRange.TaskTimeRangeClear！" );
        //    //    return null;
        //    //}

        //    //TaskTimeRangeNew.resTimeRange.TaskTimeRangeList.Remove(TaskTimeRangeLast);

        //    return NoWorkTaskTime;
        //}

        ////没用到
        //public int CheckTaskOverlap(SchProductRouteRes as_SchProductRouteRes, DateTime dt_ResDate, Boolean bSchRev = false)
        //{
        //    ///检查当前时段当前已排任务是否可能重叠
        //    if (CheckCurTimeTaskOverlap(as_SchProductRouteRes, dt_ResDate, dt_ResDate, bSchRev) < 0) return -1;

        //    ///检查下一时段当前已排任务是否可能重叠 ,返回可排日期dt_ResDate 
        //    if (bSchRev == false)
        //    {
        //        if (CheckNextTimeTaskOverlap(as_SchProductRouteRes, this.DEndTime, dt_ResDate, bSchRev) < 0) return -1;
        //    }
        //    else
        //    {
        //        if (CheckNextTimeTaskOverlap(as_SchProductRouteRes, this.DBegTime, dt_ResDate, bSchRev) < 0) return -1;
        //    }

        //    return 1;
        //}

        ////检查当前已排任务是否可能重叠
        //public int CheckCurTimeTaskOverlap(SchProductRouteRes as_SchProductRouteRes, DateTime dt_ResDate, DateTime adCanBegDate, Boolean bSchRev = false)
        //{
        //    if (this.TaskTimeRangeList.Count < 1) return 1;  //没有排任务，都空闲

        //    if (this.CIsInfinityAbility == "1") return 1;    //资源产能无限时，不管是否重叠，不用检查


        //    try
        //    {
        //        //当前时间段是否有排该任务,找时段开始日期(倒排) 或 结束日期(正排)等于任务结束时间.
        //        TaskTimeRange TaskTimeRange2 = this.TaskTimeRangeList.Find(delegate (TaskTimeRange p1) { return (p1.iSchSdID == as_SchProductRouteRes.iSchSdID && p1.iProcessProductID == as_SchProductRouteRes.iProcessProductID && p1.iResProcessID == as_SchProductRouteRes.iResProcessID && (p1.DEndTime == dt_ResDate || p1.DBegTime == dt_ResDate)); });

        //        if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
        //        {
        //            int i = 1;

        //        }


        //        if (TaskTimeRange2 != null)   //有排任务
        //        {
        //            DateTime ldtResEndDate;

        //            if (bSchRev == false)     //正排
        //            {
        //                ldtResEndDate = TaskTimeRange2.DEndTime;
        //                //本时段内找，是否有其他任务，已排                    
        //                //List<TaskTimeRange> TaskTimeRangeList1 = this.resource.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return (p1.iSchSdID != as_SchProductRouteRes.iSchSdID || p1.iProcessProductID != as_SchProductRouteRes.iProcessProductID || p1.iResProcessID != as_SchProductRouteRes.iResProcessID) && p1.DBegTime > ldtResEndDate && p1.CResourceNo == this.cResourceNo; });
        //                List<TaskTimeRange> TaskTimeRangeList1 = this.resource.GetTaskTimeRangeList().FindAll(delegate (TaskTimeRange p1) { return p1.DBegTime >= ldtResEndDate && p1.CResourceNo == this.cResourceNo; });

        //                if (TaskTimeRangeList1.Count < 1) return -1;
        //                //TaskTimeRangeList1.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

        //                TaskTimeRange TaskTimeRange3 = TaskTimeRangeList1[0];

        //                if (TaskTimeRange3 != null && TaskTimeRange3.cTaskType == 1 && TaskTimeRange3.iSchSdID != TaskTimeRange2.iSchSdID && TaskTimeRange3.iProcessProductID != TaskTimeRange2.iProcessProductID && TaskTimeRange3.iResProcessID != TaskTimeRange2.iResProcessID)
        //                {
        //                    adCanBegDate = TaskTimeRange3.DEndTime;
        //                    return -1;         //有重叠
        //                }
        //            }
        //            else                      //倒排
        //            {
        //                ldtResEndDate = TaskTimeRange2.DBegTime;
        //                //本时段内找，是否有其他任务，已排

        //                //List<TaskTimeRange> TaskTimeRangeList1 = this.resource.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return (p1.iSchSdID != as_SchProductRouteRes.iSchSdID || p1.iProcessProductID != as_SchProductRouteRes.iProcessProductID || p1.iResProcessID != as_SchProductRouteRes.iResProcessID) && p1.DEndTime < ldtResEndDate && p1.CResourceNo == this.cResourceNo; });
        //                List<TaskTimeRange> TaskTimeRangeList1 = this.resource.GetTaskTimeRangeList(false).FindAll(delegate (TaskTimeRange p1) { return p1.DEndTime <= ldtResEndDate && p1.CResourceNo == this.cResourceNo; });

        //                if (TaskTimeRangeList1.Count < 1) return -1;
        //                //TaskTimeRangeList1.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime,p1.DBegTime); });

        //                TaskTimeRange TaskTimeRange3 = TaskTimeRangeList1[0];

        //                if (TaskTimeRange3 != null && TaskTimeRange3.cTaskType == 1 && TaskTimeRange3.iSchSdID != TaskTimeRange2.iSchSdID && TaskTimeRange3.iProcessProductID != TaskTimeRange2.iProcessProductID && TaskTimeRange3.iResProcessID != TaskTimeRange2.iResProcessID)
        //                {
        //                    adCanBegDate = TaskTimeRange3.DBegTime;
        //                    return -1;         //有重叠
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        throw new Exception("检验任务是否重叠出错,位置ReTimeRange.CheckCurTimeTaskOverlap！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
        //        return -1;
        //    }

        //    return 1;       //无重叠
        //}

        ////检查下一时段当前已排任务是否可能重叠
        //public int CheckNextTimeTaskOverlap(SchProductRouteRes as_SchProductRouteRes, DateTime dt_ResDate, DateTime adCanBegDate, Boolean bSchRev = false)
        //{
        //    ResTimeRange ResTimeRange1 = null;

        //    if (this.CIsInfinityAbility == "1") return 1;    //资源产能无限时，不管是否重叠，不用检查

        //    try
        //    {
        //        if (bSchRev == false)     //正排
        //        {
        //            //找出下一资源时段
        //            List<ResTimeRange> ResTimeRangeList = this.resource.ResTimeRangeList.FindAll(delegate (ResTimeRange p1) { return p1.DBegTime >= dt_ResDate && p1.CResourceNo == this.cResourceNo; });
        //            ResTimeRangeList.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
        //            if (ResTimeRangeList.Count > 0)
        //            {
        //                ResTimeRange1 = ResTimeRangeList[0];
        //                //调用当前时段CheckCurTimeTaskOverlap
        //                return ResTimeRange1.CheckCurTimeTaskOverlap(as_SchProductRouteRes, ResTimeRange1.DBegTime, adCanBegDate, bSchRev);
        //            }
        //            else
        //            {
        //                return -1;
        //            }
        //        }
        //        else                      //倒排
        //        {
        //            //找出下一资源时段
        //            List<ResTimeRange> ResTimeRangeList = this.resource.ResTimeRangeList.FindAll(delegate (ResTimeRange p1) { return p1.DEndTime <= dt_ResDate && p1.CResourceNo == this.cResourceNo; });
        //            ResTimeRangeList.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
        //            if (ResTimeRangeList.Count > 0)
        //            {
        //                ResTimeRange1 = ResTimeRangeList[ResTimeRangeList.Count - 1];
        //                //
        //                return ResTimeRange1.CheckCurTimeTaskOverlap(as_SchProductRouteRes, ResTimeRange1.DEndTime, adCanBegDate, bSchRev);
        //            }
        //            else
        //            {
        //                return -1;
        //            }
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        throw new Exception("检验任务是否重叠出错,位置ReTimeRange.CheckNextTimeTaskOverlap！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
        //        return -1;
        //    }

        //    return 1;       //无重叠
        //}

        ////因为拆分时间段导致时间段超出，变成同一事务处理 2021-11-16 JonasCheng
        ////包含新增加，修改，删除时间段
        //public int ModifyResTimeRange(TaskTimeRange aNewTaskRange, TaskTimeRange oldTaskTimeRange, TaskTimeRange NoTaskTime1, TaskTimeRange NoTaskTime2 = null)
        //{
        //    //0、检查本时间段是否异常           
        //    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //        return -1;
        //    }

        //    //1、增加新任务占用时间段
        //    //WorkTimeRangeList.Add(aNewTaskRange);           
        //    aNewTaskRange.AddWorkimeRange(this);

        //    //2、删除原来空闲时间段oldTaskTimeRange,全部用完了
        //    if (NoTaskTime1 == null)
        //    {
        //        //this.TaskTimeRangeList.Remove(oldTaskTimeRange);
        //        oldTaskTimeRange.RemoveTaskTimeRange(this);
        //    }
        //    else //替换原来时间段
        //    {
        //        //oldTaskTimeRange.dBegTime = NoTaskTime1.dBegTime;
        //        //oldTaskTimeRange.dEndTime = NoTaskTime1.dEndTime;

        //        oldTaskTimeRange.AddTaskTimeRange(this, NoTaskTime1);
        //    }

        //    //3、插入新的时间段
        //    if (NoTaskTime2 != null)
        //    {
        //        //this.TaskTimeRangeList.Add(NoTaskTime2);

        //        NoTaskTime2.AddTaskTimeRange(this);
        //    }

        //    //4、检查本时间段是否异常           
        //    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
        //    {
        //        //Clipboard.SetText(this.ToString());
        //        throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
        //        return -1;
        //    }

        //    return 1;
        //}


        #region //属性函数封装
        public string CResourceNo
        {
            get { return cResourceNo; }
            set { cResourceNo = value; }
        }

        public string CIsInfinityAbility
        {
            get { return cIsInfinityAbility; }
            set { cIsInfinityAbility = value; }
        }

        public DateTime DBegTime
        {
            get { return dBegTime; }
            set
            {
                if (value < new DateTime(2000, 1, 1))
                {
                    throw new Exception(string.Format("资源编号{0}时间段开始日期{1}不能小于2000-01-01日,开始时间{3},结束时间{4}", this.cResourceNo, value.ToString(), this.dBegTime.ToShortDateString(), this.dEndTime.ToShortDateString()));
                }

                dBegTime = value;
                this.holdingTime = this.HoldingTime;
            }
        }


        public DateTime DEndTime
        {
            get { return dEndTime; }
            set
            {
                if (value < new DateTime(2000, 1, 1))
                {
                    throw new Exception(string.Format("资源编号{0}时间段结束日期{1}不能小于2000-01-01日,开始时间{3},结束时间{4}", this.cResourceNo, value.ToString(), this.dBegTime.ToShortDateString(), this.dEndTime.ToShortDateString()));
                }

                if (value <= dBegTime)
                {
                    throw new Exception(string.Format("资源编号{0}时间段结束日期{1}不能小于时段开始时间,开始时间{3},结束时间{4}", this.cResourceNo, value.ToString(), this.dBegTime.ToShortDateString(), this.dEndTime.ToShortDateString()));
                }

                dEndTime = value;


                this.holdingTime = this.HoldingTime;
            }
        }

        /// <summary>
        /// 片段时长，单位：秒
        /// </summary>
        public int HoldingTime
        {
            get
            {
                //直接设置holdingTime值，以免重算 2019-07-07
                //if (this.holdingTime > 0)
                //    return this.holdingTime;
                //else
                //{
                if (dEndTime != null && dBegTime != null && dEndTime > dBegTime)
                {
                    TimeSpan its = dEndTime - dBegTime;


                    if (its.TotalSeconds > 0)
                    {
                        return (int)its.TotalSeconds; //如果没有设置，则时长缺省为结束时间与开始时间之差
                    }
                    else
                    {
                        throw new Exception("the timerange no set");
                    }
                }
                else
                {
                    return 0;
                }
                //}
            }
            set
            {
                //holdingTime = this.HoldingTime;

                ////重新计算空闲时间段
                //if (this.notWorkTime == 0 )
                //    this.notWorkTime = holdingTime - this.AllottedTime;
                //if (value > 0)
                //{
                //    holdingTime = value;
                //}
                //else
                //{
                //    throw new Exception("时间段占用时间必须大于0");
                //}
            }
        }

        public int AllottedTime   //特殊处理，分ResTimeRange和TaskTimeRange
        {
            get
            {
                if (this.GetType().ToString() == "Algorithm.TaskTimeRange")
                {
                    return allottedTime;
                }
                else   //"Algorithm.ResTimeRange" 资源时段的占用时间，取所有已分配任务的已占用时间
                {
                    int allottedTimeTemp = 0;

                    if (this.CIsInfinityAbility != "1" && this.WorkTimeRangeList.Count > 0)   //有限产能，统计时段内已分配任务时间。
                    {
                        //List<TaskTimeRange> TaskTimeRangeTempList = this.TaskTimeRangeList.FindAll(delegate (TaskTimeRange p1) { return (p1.allottedTime > 0); });

                        foreach (TaskTimeRange taskTimeRange in this.WorkTimeRangeList)
                        {
                            allottedTimeTemp += taskTimeRange.allottedTime;
                        }

                        if (allottedTime < allottedTimeTemp)
                            allottedTime = allottedTime;


                        ////如果当前时间段小于5分钟，则认为全部排满,暂不处理
                        //if (this.HoldingTime - allottedTime < 5 * 60)  allottedTime = this.HoldingTime;
                    }
                    //if (this.CIsInfinityAbility != "1") //有限产能，统计时段内已分配任务时间。
                    //{
                    //    return allottedTime;
                    //}
                    //else
                    //    return 0;

                    //无限产能，时段占用时间为零
                    return allottedTimeTemp;
                }

            }

            set
            {   //只有TaskTimeRange才可以设置AllottedTime  
                if (value >= 0)
                {

                    //if (this.CIsInfinityAbility != "1" && (value) > this.holdingTime)
                    //{
                    //    //int j = 1;
                    //     throw new Exception(string.Format("资源编号{0}时间段已分配时间{1}必须大于本时间段可用时间{2},开始时间{3},结束时间{4}", this.cResourceNo, (allottedTime + value).ToString(), this.holdingTime.ToString(),this.dBegTime.ToShortDateString(), this.dEndTime.ToShortDateString()));
                    //}

                    allottedTime = value;

                    ////自动计算空闲时间
                    //notWorkTime = this.holdingTime - allottedTime;
                    //if (this.CIsInfinityAbility != "1" && notWorkTime < 0 )
                    //{
                    //    throw new Exception(string.Format("资源编号{0}空闲时间段不能小于0,已分配时间{1}必须大于本时间段可用时间{2},开始时间{3},结束时间{4}", this.cResourceNo, allottedTime.ToString(), this.holdingTime.ToString(), this.dBegTime.ToShortDateString(), this.dEndTime.ToShortDateString()));
                    //}

                }
                else
                {
                    throw new Exception("时间段已分配时间必须大于0");
                }
            }
        }

        public int AvailableTime      //取时段内可用时间
        {
            get
            {
                if (this.CIsInfinityAbility == "1")   ////无限产能。
                {
                    return holdingTime;
                }
                else     //有限产能，统计时段内已分配任务时间
                {
                    //如果是任务类型，只有一个时间段
                    if (this.GetType().ToString() == "Algorithm.TaskTimeRange")
                    {
                        //if（(this as TaskTimeRange).cTaskType == "0"）   //0空闲时间段
                        //    (this.DEndTime - this.DBegTime)
                        TimeSpan its = dEndTime - dBegTime;
                        if (its.TotalSeconds > 0)
                        {
                            return (int)its.TotalSeconds; //如果没有设置，则时长缺省为结束时间与开始时间之差
                        }
                        return holdingTime;
                    }
                    else if (this.holdingTime - this.AllottedTime >= 0)
                    {
                        return this.holdingTime - this.AllottedTime;
                    }
                    else if (this.holdingTime - this.AllottedTime <= 30)  //计算有误差，小于1秒 2020-06-02 Jonas Cheng 
                        return 0;
                    else
                    {

                        throw new Exception("出错位置：排程时取时段内可用时间出错TimeSchTask.AvailableTime！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                        return 0;
                    }
                }

            }

        }

        public int NotWorkTime  //空闲时间
        {
            get
            {
                if (this.GetType().ToString() == "Algorithm.TaskTimeRange")
                {
                    return AvailableTime;   //可用时间
                }
                else   //"Algorithm.ResTimeRange" 资源时段的占用时间，取所有已分配任务的已占用时间
                {
                    return AvailableTime;   //可用时间


                    //foreach (TaskTimeRange taskTimeRange in this.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return (p1.cTaskType == 0); }))
                    //{
                    //    notWorkTime += taskTimeRange.HoldingTime;
                    //}

                    //无限产能，时段占用时间为零
                    //if (notWorkTime == 0 )
                    //    return this.HoldingTime;
                    //else
                    //return notWorkTime;
                }

            }
            set
            {   //只有TaskTimeRange才可以设置AllottedTime
                //空闲时间段不用设置
                //if( value >= 0 )
                //{
                //    notWorkTime = value;
                //}
                //else   //"Algorithm.ResTimeRange" 资源时段的占用时间，取所有已分配任务的已占用时间
                //{
                //    throw new Exception(string.Format("资源编号{0}时间段空闲时间必须大于等于0,开始时间{1},结束时间{2}",this.cResourceNo,this.dBegTime.ToShortDateString(),this.dEndTime.ToShortDateString()));
                //}
            }


        }

        public int MaintainTime  //维修、故障时间
        {
            get
            {
                //if (this.GetType().ToString() == "Algorithm.TaskTimeRange")
                //{
                //    if (((TaskTimeRange)this).cTaskType == 2) //任务时间类型： 0 空闲， 1 加工时间 2 维修时间  ---3 前准备时间 4 后准备时间 ，暂时没用
                //        return this.holdingTime;   //可用时间
                //    else
                //        return 0;
                //}
                //else   //"Algorithm.ResTimeRange" 资源时段的占用时间，取所有已分配任务的已占用时间
                //{
                    maintainTime = 0;

                    foreach (TaskTimeRange taskTimeRange in this.TaskTimeRangeList.FindAll(delegate (TaskTimeRange p1) { return (p1.cTaskType == 2); }))
                    {
                        maintainTime += taskTimeRange.HoldingTime;
                    }

                    //无限产能，时段占用时间为零
                    return maintainTime;
                //}

            }
        }

        /// <summary>
        /// 时间片段的用途，主要有“休息”、“维护”、“工作”
        /// </summary>
        private TimeRangeAttribute attribute;

        public TimeRangeAttribute Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }

        public List<TaskTimeRange> TaskTimeRangeList
        {
            get { return taskTimeRangeList; }
            set
            {
                if (this.cResourceNo == "gys20097")
                {
                    int i = 0;
                }
                taskTimeRangeList = value;
            }
        }

        /// <summary>
        /// 时间片断状态，0=计划；1=实际
        /// </summary>
        public int state;

        /// <summary>
        /// 资源占用率
        /// </summary>
        public double loadRate;

        /// <summary>
        /// 实现IComparable接口，便于TimeRange类集合排序
        /// </summary>
        /// <param name="obj">要比较的TimeRange类对象</param>
        /// <returns>开始时间之差</returns>
        public int CompareTo(object obj)
        {
            if (obj is ResTimeRange)
            {
                ResTimeRange newTimeRange = (ResTimeRange)obj;

                if (this.dBegTime == newTimeRange.dBegTime && this.dEndTime == newTimeRange.dEndTime)
                {
                    return 1;
                }
                else
                {
                    return -1;

                }
            }
            else
            {
                throw new ArgumentException("对象非TimeRange类型");
            }
        }

        /// <summary>
        /// 对象拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    
}
