using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Algorithm
{
    [Serializable]
    public class ResTimeRange : IComparable, ICloneable,ISerializable
    {
        public SchData schData = null;        //所有排程数据
        public string cResourceNo = "";    //对应资源ＩＤ号,要设置     
        public string cIsInfinityAbility = "0";    // 0 产能有限，1 产能无限     
        public Resource resource = null;                   // 时段对应的资源  有值        
        public DateTime dBegTime;    //时间段开始时间
        public DateTime dEndTime;   //时间段结束时间
        public int holdingTime = 0;     //时段总长, dDEndTimeTime - dBegTime,单位为秒
        public int allottedTime = 0;    //已分配时间,包括维修、故障时间
        public int maintainTime = 0;    //维修、故障时间
        public int availableTime = 0;   //时段内可用时间，计算出来
        public int WorkTimeAct = 0;        //学习曲线折扣,有效加工时间
        public int notWorkTime = 0;     //时段内空闲时间，计算出来,用于检查
        public int iPeriodID = 1;        //时段ID，排程完成写回数据库时，重新生成，唯一
        public DateTime dPeriodDay;      //时段所属日期
        public string   FShiftType;       //时段所属班次 白班、夜班、中班等
        public List<TaskTimeRange> taskTimeRangeList = new List<TaskTimeRange>(10);
        public List<TaskTimeRange> WorkTimeRangeList = new List<TaskTimeRange>(10);
        public ResTimeRange ResTimeRangePre;          //前资源时间段
        public ResTimeRange ResTimeRangePost;         //后资源时间段
        public int iSchSdID = -1;                     //记录更新、新增时间段的任务ID
        public int iProcessProductID = -1;
        public int iResProcessID = -1;
        public int iSchSNMax = -1;
        public ResSourceDayCap resSourceDayCap =null;  //资源日产能限制,2023-10-05 新增加，每个时间段排程完成时，设置当天日产能已排工时。
        public ResTimeRange(string as_ResourceNo)
        {
            this.cResourceNo = as_ResourceNo;
        }
        public ResTimeRange(string as_ResourceNo, DateTime adBegTime, DateTime adEndTime)
        {
            this.cResourceNo = as_ResourceNo;
            this.dBegTime = adBegTime;
            this.dEndTime = adEndTime;
            this.AllottedTime = 0;
        }
        public ResTimeRange()
        {
        }
        public int TimeSchTaskFreezeInit(SchProductRouteRes as_SchProductRouteRes, ref DateTime adCanBegDate, ref DateTime adCanEndDate)
        {
            Boolean bSchdule = true;   //正式排产
            List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanBegDate, false, bSchdule,Convert.ToInt32(as_SchProductRouteRes.iResRationHour + as_SchProductRouteRes.iResPreTime),false);
            try
            {
                for (int i = 0; i < NoTimeRangeList.Count; i++)
                {
                    TaskTimeRange NoTimeRange = NoTimeRangeList[i];
                    if (NoTimeRange.DBegTime >= adCanEndDate) break;  //时间段大于待排结束时间段，退出
                    if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime  )
                    {
                        continue;
                    }
                    TaskTimeRange taskTimeRange1 = new TaskTimeRange();
                    taskTimeRange1.cTaskType = 1;  //工作
                    taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
                    taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
                    taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
                    taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
                    taskTimeRange1.cResourceNo = this.cResourceNo;
                    taskTimeRange1.resource = this.resource;                     //资源对象
                    taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
                    taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
                    taskTimeRange1.resTimeRange = this;
                    if (NoTimeRange.dBegTime >= adCanBegDate)
                    {
                        if (adCanEndDate > NoTimeRange.DEndTime)
                        {
                            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                        }
                        else        ////部分占用
                        {
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = adCanEndDate;
                            taskTimeRange1.AllottedTime = Convert.ToInt32((taskTimeRange1.DEndTime - taskTimeRange1.DBegTime).TotalSeconds);
                        }
                    }
                    else if ((NoTimeRange.dBegTime < adCanBegDate && NoTimeRange.dEndTime > adCanBegDate))    // 可用时间段大于时段开始时间,前面部分时间段不可用
                    {
                        if (NoTimeRange.DEndTime >= adCanEndDate)
                        {
                            taskTimeRange1.DBegTime = adCanBegDate;
                            taskTimeRange1.DEndTime = adCanEndDate;
                            taskTimeRange1.AllottedTime = Convert.ToInt32((taskTimeRange1.DEndTime - taskTimeRange1.DBegTime).TotalSeconds);
                        }
                        else if (NoTimeRange.DEndTime < adCanEndDate)
                        {
                            taskTimeRange1.DBegTime = adCanBegDate;
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                            taskTimeRange1.AllottedTime = Convert.ToInt32((taskTimeRange1.DEndTime - taskTimeRange1.DBegTime).TotalSeconds);
                        }
                        else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
                    {
                        throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
                    }
                    if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
                    {
                        int m = 1;
                        throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
                        return -1;
                    }
                    TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);
                    as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
                    adCanBegDate = taskTimeRange1.DEndTime;
                    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
                    {
                        throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                        return -1;
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return 0; //剩下未排时间
        }
        public int TimeSchTaskSortInit(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref Boolean bFirtTime)
        {
            int taskallottedTime = 0;            //任务在本时间段内 总安排时间
            int ai_workTimeOld = ai_workTime;    //用于记录ai_workTime值
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanBegDate, false, bSchdule, ai_workTime + ai_ResPreTime, false);
            try
            {
                for (int i = 0; i < NoTimeRangeList.Count; i++)
                {
                    if (ai_workTime <= 0) break;
                    TaskTimeRange NoTimeRange = NoTimeRangeList[i];
                    if (bFirtTime)   //是第一个排产时间段,计算换产时间
                    {
                        if (NoTimeRange.AvailableTime == 0) continue;    //是排第一个时段，期该时段没有可用时间，则继续                       
                        ai_CycTimeTol = 0;   //设为0
                        ai_ResPreTime = this.resource.GetChangeTime(as_SchProductRouteRes, ai_workTime, NoTimeRange.DBegTime, ref ai_CycTimeTol, bSchdule);
                        if (ai_ResPreTime > 0)
                        {
                            int K = 0;
                        }
                        ai_ResPreTime += ai_CycTimeTol;
                        ai_workTime = Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime;
                    }
                    if (bSchdule == false)
                    {
                        if (NoTimeRange.cTaskType != 0 && ai_workTime > 0)  //只要不是空闲时间段
                        {
                            bFirtTime = true;   //是否第一个排产时间段
                            ai_workTime = ai_workTimeTask;            //返回原值
                            adCanBegDate = NoTimeRange.DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
                            adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
                            continue;
                        }
                    }
                    if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime && ai_workTime > 0)
                    {
                        continue;
                    }
                    TaskTimeRange taskTimeRange1 = new TaskTimeRange();
                    taskTimeRange1.cTaskType = 1;  //工作
                    taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
                    taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
                    taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
                    taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
                    taskTimeRange1.cResourceNo = this.cResourceNo;
                    taskTimeRange1.resource = this.resource;                     //资源对象
                    taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
                    taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
                    taskTimeRange1.resTimeRange = this;
                    if (bFirtTime)
                        bFirtTime = false;
                    if (NoTimeRange.dBegTime >= adCanBegDate)
                    {
                        if (ai_workTime > NoTimeRange.AvailableTime)
                        {
                            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                            ai_workTime -= NoTimeRange.AvailableTime;
                        }
                        else        ////部分占用
                        {
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = NoTimeRange.DBegTime.AddSeconds(ai_workTime);
                            taskTimeRange1.AllottedTime = ai_workTime;
                            ai_workTime = 0;        //剩余待分配工作时间为0 
                        }
                    }
                    else                // 可用时间段大于时段开始时间,前面部分时间段不可用
                    {
                        if (NoTimeRange.DEndTime > adCanBegDate)
                        {
                            TimeSpan lTimeSpan = (NoTimeRange.DEndTime - adCanBegDate);
                            int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);
                            if (ai_workTime > iAvailableTime)
                            {
                                taskTimeRange1.DBegTime = adCanBegDate;
                                taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                                taskTimeRange1.AllottedTime = iAvailableTime;
                                ai_workTime -= iAvailableTime;
                            }
                            else        ////部分占用,排完
                            {
                                taskTimeRange1.DBegTime = adCanBegDate;
                                taskTimeRange1.DEndTime = adCanBegDate.AddSeconds(ai_workTime);
                                taskTimeRange1.AllottedTime = ai_workTime;
                                ai_workTime = 0;        //剩余待分配工作时间为0 
                            }
                        }
                        else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
                        {
                            continue;
                        }
                    }
                    if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
                    {
                        throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
                    }
                    if (bSchdule)  //正式排程
                    {
                        if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
                        {
                            int m = 1;
                            throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
                            return -1;
                        }
                        TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);
                        as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
                    }
                    adCanBegDate = taskTimeRange1.DEndTime;
                    bFirtTime = false;      //是否第一个排产时间段,不是第一个
                    if (bSchdule)  //正式排程
                    {
                        if (!CheckResTimeRange() && this.cIsInfinityAbility != "1" )
                        {
                            throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                            return -1;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return ai_workTime; //剩下未排时间
        }
        public int TimeSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref Boolean bFirtTime,ref int ai_DisWorkTime,Boolean bReCalWorkTime = true, ResTimeRange resTimeRangeNext = null, SchProductRouteRes as_SchProductRouteResPre = null)
        {
            int taskallottedTime = 0;            //任务在本时间段内 总安排时间
            int ai_workTimeOld = ai_workTime;    //用于记录ai_workTime值
            double iDayDis = 1;                  //考虑学习曲线每天折扣
            DateTime dtiDayDis = adCanBegDate.AddDays(-1);           //学习曲线日期
            DateTime dtTaskBegDate = adCanBegDate;                   //任务开始排产日期
            int iTaskAllottedTime = 0;                    //学习曲线 时段分配工时            
            int ai_workTimeDisTol = ai_workTime;             //用于记录学习曲线打则后的
            int ai_workTimeAct = 0 ;                         //累计已排有效时间
            string message;
            DateTime ldtBeginDate = DateTime.Now;
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanBegDate, false, bSchdule, ai_workTime + ai_ResPreTime,true);
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
            {
                message = string.Format(@"3.4.2、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                        as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                SchParam.Debug(message, "资源运算");
                ldtBeginDate = DateTime.Now;
            }
            if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
            {
                throw new Exception("出错位置：排程时段检查出错TimeSchTask.TimeSchTask！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                return -1;
            }
            try
            {
                for (int i = 0; i < NoTimeRangeList.Count; i++)
                {
                    if (ai_workTime <= 0)
                    {
                        ai_workTime = 0;
                        break;
                    }
                    TaskTimeRange NoTimeRange = NoTimeRangeList[i];
                    if (bFirtTime)   //是第一个排产时间段,计算换产时间
                    {
                        if ( NoTimeRange.AvailableTime == 0) 
                            continue;    //是排第一个时段，期该时段没有可用时间，则继续      
                        if (bReCalWorkTime)   //重新计算前准备时间，已下达生产任务单，不用重新计算 bReCalWorkTime = false
                        {
                            ai_CycTimeTol = 0;   //设为0
                            if (this.resource.iSchBatch <= 1 && as_SchProductRouteRes.iActResReqQty > 0)
                                ai_ResPreTime = 0;
                            else
                            {
                                ai_ResPreTime = this.resource.GetChangeTime(as_SchProductRouteRes, ai_workTime, NoTimeRange.DBegTime, ref ai_CycTimeTol, bSchdule, as_SchProductRouteResPre);
                            }
                            if (ai_ResPreTime > 0)
                            {
                                int K = 0;
                            }
                            ai_ResPreTime += ai_CycTimeTol;
                            ai_workTime = Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime;  //Convert.ToInt32(as_SchProductRouteRes.iResRationHour) + ai_ResPreTime
                            ai_workTimeDisTol = ai_workTime;
                        }
                        as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime;
                    }
                    if (bSchdule == false) 
                    {
                        if (as_SchProductRouteRes.cLearnCurvesNo != "")
                        {
                            iDayDis = as_SchProductRouteRes.TechLearnCurves.GetDayDisValue(as_SchProductRouteRes.dResLeanBegDate, NoTimeRange.DEndTime);
                        }
                        if (NoTimeRange.taskTimeRangePost != null &&   ai_workTime/ iDayDis > NoTimeRange.AvailableTime )
                        {
                            bFirtTime = true;   //是否第一个排产时间段
                            ai_workTime = ai_workTimeTask;            //返回原值      
                            adCanBegDate = NoTimeRange.DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
                            adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
                        }
                        else
                        {
                            adCanBegDate = NoTimeRange.DBegTime;
                        }
                        if (NoTimeRange.cTaskType != 0 && ai_workTime > 0)  //只要不是空闲时间段
                        {
                            bFirtTime = true;   //是否第一个排产时间段
                            ai_workTime = ai_workTimeTask;            //返回原值
                            adCanBegDate = NoTimeRange.DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
                            adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
                            continue;
                        }
                    }
                    if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                    {
                        message = string.Format(@"3.4.3、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                                as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                        SchParam.Debug(message, "资源运算");
                        ldtBeginDate = DateTime.Now;
                    }
                    if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime && ai_workTime > 0 && ai_workTime > NoTimeRange.AvailableTime )
                    {
                        continue;
                    }
                    TaskTimeRange taskTimeRange1 = new TaskTimeRange();
                    taskTimeRange1.cTaskType = 1;  //工作
                    taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
                    taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
                    taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
                    taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
                    taskTimeRange1.cResourceNo = this.cResourceNo;
                    taskTimeRange1.resource = this.resource;                     //资源对象
                    taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
                    taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
                    taskTimeRange1.resTimeRange = this;
                    if (as_SchProductRouteRes.cLearnCurvesNo != "")
                    {
                        iDayDis = as_SchProductRouteRes.TechLearnCurves.GetDayDisValue(as_SchProductRouteRes.dResLeanBegDate, NoTimeRange.DEndTime);
                        if (iDayDis > 0)
                        {
                            ai_workTime = Convert.ToInt32(ai_workTime / iDayDis);             //用于记录学习曲线打则后的
                        }
                        else
                        {                           
                            iDayDis = 1;
                        }
                    }
                    else
                    {                        
                        iDayDis = 1;
                    }
                    if (bFirtTime)
                        bFirtTime = false;
                    if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
                    {
                        int m = 1;
                    }
                    if (NoTimeRange.dBegTime >= adCanBegDate)
                    {
                        if (ai_workTime > NoTimeRange.AvailableTime)
                        {
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;
                            iTaskAllottedTime = NoTimeRange.AvailableTime;
                            ai_workTime -= NoTimeRange.AvailableTime;
                        }
                        else        ////部分占用
                        {
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = NoTimeRange.DBegTime.AddSeconds(ai_workTime);
                            taskTimeRange1.AllottedTime = ai_workTime;
                            iTaskAllottedTime = ai_workTime;
                            ai_workTime = 0;        //剩余待分配工作时间为0 
                        }
                    }
                    else                // 可用时间段大于时段开始时间,前面部分时间段不可用
                    {
                        if (NoTimeRange.DEndTime > adCanBegDate)
                        {
                            TimeSpan lTimeSpan = (NoTimeRange.DEndTime - adCanBegDate);
                            int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);
                            if (ai_workTime > iAvailableTime)
                            {
                                taskTimeRange1.DBegTime = adCanBegDate;
                                taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                                taskTimeRange1.AllottedTime = iAvailableTime;
                                iTaskAllottedTime = iAvailableTime;
                                ai_workTime -= iAvailableTime;
                            }
                            else        ////部分占用,排完
                            {
                                taskTimeRange1.DBegTime = adCanBegDate;
                                taskTimeRange1.DEndTime = adCanBegDate.AddSeconds(ai_workTime);
                                taskTimeRange1.AllottedTime = ai_workTime;
                                iTaskAllottedTime = ai_workTime;
                                ai_workTime = 0;        //剩余待分配工作时间为0 
                            }
                        }
                        else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
                        {
                            continue;
                        }
                    }
                    ai_workTimeAct += Convert.ToInt32(iTaskAllottedTime * iDayDis);
                    taskTimeRange1.WorkTimeAct = Convert.ToInt32(iTaskAllottedTime * iDayDis);
                    if (taskTimeRange1.DBegTime >= taskTimeRange1.DEndTime )
                    {
                        throw new Exception(string.Format("数据异常，生成新任务开始时间不对!开始大于结束时间"));
                        return -1;
                    }
                    if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                    {
                        message = string.Format(@"3.4.4、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                                as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                        SchParam.Debug(message, "资源运算");
                        ldtBeginDate = DateTime.Now;
                    }
                    if (bSchdule)  //正式排程
                    {
                        if (this.cResourceNo == "3.04.24" && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                        {
                            int ii = 1;
                        }
                        if (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
                        {
                            int ii = 1;
                        }
                        if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
                        {
                            int m = 1;
                        }
                        TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);
                        if (this.cIsInfinityAbility != "1" && (this.AllottedTime + this.AvailableTime > this.HoldingTime))
                        {
                            int m = 1;
                        }
                        as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
                    }
                    adCanBegDate = taskTimeRange1.DEndTime;
                    if (bSchdule)  //正式排程
                    {
                        if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
                        {
                            string Errormessage = string.Format(@"检查时段数据出错CheckResTimeRange,排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],时段开始时间[{4}],时段结束时间[{5}],时段总工时[{6}],分配工时[{7}],空闲工时[{8}],差异工时[{9}]",
                                                       as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, this.dBegTime, this.dEndTime, this.HoldingTime, this.AllottedTime, this.NotWorkTime, this.AvailableTime - this.NotWorkTime );
                            SchParam.Error(Errormessage, "资源运算出错");
                            throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！" + taskTimeRange1.DEndTime.ToString()); 
                            return -1;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            ai_workTime = ai_workTimeDisTol - ai_workTimeAct;
            if (ai_workTime < 0) ai_workTime = 0;
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
            {
                message = string.Format(@"3.4.5、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                        as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                SchParam.Debug(message, "资源运算");
                ldtBeginDate = DateTime.Now;
            }
            return ai_workTime; //剩下未排时间
        }
        public int TimeSchTaskRev(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanEndDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref Boolean bFirtTime)
        {
            int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeOld = 0;     //用于记录ai_workTime值，当空闲时间段太下时，如10分钟，则该时间段不排，ai_workTime值还原。
            List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanEndDate, true, bSchdule, ai_workTime,true );
            if (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int ii = 1;
            }
            if ( !CheckResTimeRange() && this.cIsInfinityAbility != "1")
            {
                throw new Exception("出错位置：倒排排程时段检查出错TimeSchTask.TimeSchTaskRev！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                return -1;
            }
            try
            {
                for (int i = 0; i < NoTimeRangeList.Count; i++)
                {
                    if (ai_workTime <= 0)
                    {
                        ai_workTime = 0;
                        break;
                    }
                    TaskTimeRange NoTimeRange = NoTimeRangeList[i];
                    if (bFirtTime)   //是第一个排产时间段,计算换产时间
                    {
                        if (NoTimeRange.AvailableTime == 0) continue;    //是排第一个时段，期该时段没有可用时间，则继续 
                        as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime;
                    }
                    if (bSchdule == false) //没有排完 //&& NoTimeRange.NotWorkTime < ai_workTime
                    {
                        if (NoTimeRange.taskTimeRangePre != null && ai_workTime > NoTimeRange.AvailableTime)
                        {
                            bFirtTime = true;   //是否第一个排产时间段
                            ai_workTime = ai_workTimeTask;            //返回原值      
                            adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;  
                            adCanBegDateTask = NoTimeRange.DBegTime;
                        }
                        else
                        {
                        }
                        if (NoTimeRange.cTaskType == 1 && ai_workTime > 0)
                        {
                            bFirtTime = true;          //是否第一个排产时间段
                            ai_workTime = ai_workTimeTask;            //返回原值
                            adCanEndDate = NoTimeRange.DBegTime;    //adCanBegDateTask;        //重排可开始时间，重当前时段点开始,后面会累加更新
                            adCanBegDateTask = NoTimeRange.DEndTime;  //重新设置任务可开始时间,并返回
                            continue;
                        }
                    }
                    if (NoTimeRange.AvailableTime < SchParam.PeriodLeftTime && ai_workTime > 0 && NoTimeRange.AvailableTime < ai_workTime)
                    {
                        continue;
                    }
                    TaskTimeRange taskTimeRange1 = new TaskTimeRange();
                    taskTimeRange1.cTaskType = 1;  //工作
                    taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
                    taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
                    taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
                    taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
                    taskTimeRange1.cResourceNo = this.cResourceNo;
                    taskTimeRange1.resource = this.resource;                     //资源对象
                    taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
                    taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
                    taskTimeRange1.resTimeRange = this;
                    if (bFirtTime)
                        bFirtTime = false;
                    if (NoTimeRange.DEndTime <= adCanEndDate)
                    {
                        if (ai_workTime > NoTimeRange.AvailableTime)
                        {
                            taskTimeRange1.AllottedTime = NoTimeRange.AvailableTime;
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                            ai_workTime -= NoTimeRange.AvailableTime;
                        }
                        else        ////部分占用,倒排,从结束往前排
                        {
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime; //NoTimeRange.DEndTime;  2020-03-28
                            taskTimeRange1.DBegTime = NoTimeRange.DEndTime.AddSeconds(-ai_workTime);
                            taskTimeRange1.AllottedTime = ai_workTime;
                            ai_workTime = 0;        //剩余待分配工作时间为0 
                        }
                    }
                    else                // 可用时间小于时段结束时间,后面部分时间段不可用
                    {
                        if (NoTimeRange.DBegTime < adCanEndDate)
                        {
                            TimeSpan lTimeSpan = (adCanEndDate - NoTimeRange.DBegTime);
                            int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);
                            if (ai_workTime > iAvailableTime)
                            {
                                taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                                taskTimeRange1.DEndTime = adCanEndDate;
                                taskTimeRange1.AllottedTime = iAvailableTime;
                                ai_workTime -= iAvailableTime;
                            }
                            else        ////部分占用,排完
                            {
                                taskTimeRange1.DBegTime = adCanEndDate.AddSeconds(-ai_workTime);
                                taskTimeRange1.DEndTime = adCanEndDate;
                                taskTimeRange1.AllottedTime = ai_workTime;
                                ai_workTime = 0;        //剩余待分配工作时间为0 
                            }
                        }
                        else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
                        {
                            continue;
                        }
                    }
                    if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
                    {
                        throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
                        return -1;
                    }
                    if (taskTimeRange1.DBegTime < NoTimeRange.DBegTime && taskTimeRange1.DBegTime > NoTimeRange.DEndTime)
                    {
                        throw new Exception(string.Format("数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!"));
                        return -1;
                    }
                    if (bSchdule)  //正式排程
                    {
                        if (taskTimeRange1.AllottedTime == 0)
                        {
                            int K = 0;
                            continue;
                        }
                        if (this.cIsInfinityAbility != "1" && (this.AllottedTime + taskTimeRange1.AllottedTime > this.HoldingTime))
                        {
                            int m = 1;
                            throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
                            return -1;
                        }
                        TaskTimeRangeSplit(NoTimeRange, taskTimeRange1);
                        if (this.cIsInfinityAbility != "1" && (this.AllottedTime + this.AvailableTime > this.HoldingTime))
                        {
                            int m = 1;
                            throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", NoTimeRange.dBegTime.ToString(), NoTimeRange.dEndTime.ToString(), taskTimeRange1.dBegTime.ToString(), taskTimeRange1.dEndTime.ToString()));
                            return -1;
                        }
                        as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
                    }
                    adCanEndDate = taskTimeRange1.DBegTime;
                    bFirtTime = false;      //是否第一个排产时间段,不是第一个
                    if (bSchdule)  //正式排程
                    {
                        if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
                        {
                            throw new Exception("出错位置：TimeSchTaskRev, 订单行号：" + as_SchProductRouteRes.iSchSdID + "产品编号[" + as_SchProductRouteRes.schProductRoute.cInvCode + "]加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],时段日期[" + this.DBegTime.ToShortDateString() + " " + this.DBegTime.ToLongTimeString() + "]检查异常！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                            return -1;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return ai_workTime; //剩下未排时间
        }
        public int TimeSchTaskRevInfinite(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanEndDate, int ai_workTimeTask, ref DateTime adCanBegDateTask, Boolean bSchdule, ref Boolean bFirtTime)
        {
            int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeOld = 0;     //用于记录ai_workTime值，当空闲时间段太下时，如10分钟，则该时间段不排，ai_workTime值还原。
            List<TaskTimeRange> NoTimeRangeList = GetAvailableTimeRangeList(adCanEndDate, true, bSchdule, ai_workTime,false);
            if (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int ii = 1;
            }
            try
            {
                for (int i = 0; i < NoTimeRangeList.Count; i++)
                {
                    if (ai_workTime <= 0)
                    {
                        ai_workTime = 0;
                        break;
                    }
                    TaskTimeRange NoTimeRange = NoTimeRangeList[i];
                    if (bFirtTime)   //是第一个排产时间段,计算换产时间
                    {
                        as_SchProductRouteRes.dResLeanBegDate = NoTimeRange.DEndTime;
                    }
                    if (bSchdule == false) //没有排完 //&& NoTimeRange.NotWorkTime < ai_workTime
                    {
                    }
                    TaskTimeRange taskTimeRange1 = new TaskTimeRange();
                    taskTimeRange1.cTaskType = 1;  //工作
                    taskTimeRange1.cVersionNo = as_SchProductRouteRes.cVersionNo;
                    taskTimeRange1.iSchSdID = as_SchProductRouteRes.iSchSdID;
                    taskTimeRange1.iProcessProductID = as_SchProductRouteRes.iProcessProductID;
                    taskTimeRange1.iResProcessID = as_SchProductRouteRes.iResProcessID;
                    taskTimeRange1.cResourceNo = this.cResourceNo;
                    taskTimeRange1.resource = this.resource;                     //资源对象
                    taskTimeRange1.schProductRouteRes = as_SchProductRouteRes;   //资源任务对象
                    taskTimeRange1.schData = as_SchProductRouteRes.schData;      //所有排产数据
                    taskTimeRange1.resTimeRange = this;
                    if (bFirtTime)
                        bFirtTime = false;
                    if (NoTimeRange.DEndTime <= adCanEndDate)
                    {
                        if (ai_workTime > NoTimeRange.HoldingTime)
                        {
                            taskTimeRange1.AllottedTime = NoTimeRange.HoldingTime;
                            taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime;
                            ai_workTime -= NoTimeRange.HoldingTime;
                        }
                        else        ////部分占用,倒排,从结束往前排
                        {
                            taskTimeRange1.DEndTime = NoTimeRange.DEndTime; //NoTimeRange.DEndTime;  2020-03-28
                            taskTimeRange1.DBegTime = NoTimeRange.DEndTime.AddSeconds(-ai_workTime);
                            taskTimeRange1.AllottedTime = ai_workTime;
                            ai_workTime = 0;        //剩余待分配工作时间为0 
                        }
                    }
                    else                // 可用时间小于时段结束时间,后面部分时间段不可用
                    {
                        if (NoTimeRange.DBegTime < adCanEndDate)
                        {
                            TimeSpan lTimeSpan = (adCanEndDate - NoTimeRange.DBegTime);
                            int iAvailableTime = Convert.ToInt32(lTimeSpan.TotalSeconds);
                            if (ai_workTime > iAvailableTime)
                            {
                                taskTimeRange1.DBegTime = NoTimeRange.DBegTime;
                                taskTimeRange1.DEndTime = adCanEndDate;
                                taskTimeRange1.AllottedTime = iAvailableTime;
                                ai_workTime -= iAvailableTime;
                            }
                            else        ////部分占用,排完
                            {
                                taskTimeRange1.DBegTime = adCanEndDate.AddSeconds(-ai_workTime);
                                taskTimeRange1.DEndTime = adCanEndDate;
                                taskTimeRange1.AllottedTime = ai_workTime;
                                ai_workTime = 0;        //剩余待分配工作时间为0 
                            }
                        }
                        else            //不在空闲时段可用范围内，不能排,找下一个空闲时间段
                        {
                            continue;
                        }
                    }
                    if (taskTimeRange1.DBegTime < Convert.ToDateTime("2011-01-01"))
                    {
                        throw new Exception(string.Format("数据异常，生成新任务开始时间不对!"));
                        return -1;
                    }
                    if (taskTimeRange1.DBegTime < NoTimeRange.DBegTime && taskTimeRange1.DBegTime > NoTimeRange.DEndTime)
                    {
                        throw new Exception(string.Format("数据异常，生成新任务开始时间小于可用时间段,且结束时间大于可用时间段!"));
                        return -1;
                    }
                    if (bSchdule)  //正式排程
                    {
                        if (taskTimeRange1.AllottedTime == 0)
                        {
                            int K = 0;
                            continue;
                        }
                        as_SchProductRouteRes.TaskTimeRangeList.Add(taskTimeRange1);
                    }
                    adCanEndDate = taskTimeRange1.DBegTime;
                    bFirtTime = false;      //是否第一个排产时间段,不是第一个
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return ai_workTime; //剩下未排时间
        }
        public Boolean CheckResTimeRange()
        {
            if (this.cIsInfinityAbility == "1") return true;
            if (System.Math.Abs(this.AvailableTime - this.NotWorkTime) > 5)  //不能直接相等，可能有计算误差 2022-06-22
            {
                return ThowErrText(string.Format("检查分配时间{0} > 空闲时间{1}", this.AllottedTime, this.NotWorkTime));    //小数点，有误差    
            }
            int liNotWorkTime = 0;
            foreach (TaskTimeRange taskTimeRange in this.TaskTimeRangeList)
            {
                liNotWorkTime += taskTimeRange.NotWorkTime;
            }
            if (System.Math.Abs(liNotWorkTime - this.NotWorkTime) > 5)
            {
                return ThowErrText(string.Format("检查空闲时间{0}与明细汇总{1}不一致", this.NotWorkTime, liNotWorkTime));    //小数点，有误差   
            }
            if (System.Math.Abs(this.AllottedTime + this.NotWorkTime - this.HoldingTime) > 5)
            {
                return ThowErrText(string.Format("检查分配时间{0} + 空闲时间{1} 大于 时间段总长{2}", this.AllottedTime, this.NotWorkTime, this.HoldingTime));    //小数点，有误差   
            }
            return true ; 
        }
        public Boolean ThowErrText(string ErrText )
        {
            throw new Exception("出错位置：排程时段检查出错TimeSchTask.ThowErrText！" + ErrText); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
            return false;    //小数点，有误差      
        }
        public TaskTimeRange GetAvailableTimeRange(DateTime adCanBegDate, Boolean bSchRev = false)
        {
            TaskTimeRange lTaskTimeRangeFind = null;
            List<TaskTimeRange> TaskTimeRangeTemp = new List<TaskTimeRange>(10);
            if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
            {
                throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                return null;
            }
            if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用
            {
                TaskTimeRangeTemp = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return p2.cTaskType == 0; });
                if (TaskTimeRangeTemp.Count > 0)
                    lTaskTimeRangeFind = TaskTimeRangeTemp[TaskTimeRangeTemp.Count - 1];
            }
            else                                 //产能有限
            {
                if (bSchRev)  //倒排
                {
                    TaskTimeRangeTemp = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return (p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate || p2.DEndTime < adCanBegDate) && p2.cTaskType == 0; });
                    if (TaskTimeRangeTemp.Count > 0)
                        lTaskTimeRangeFind = TaskTimeRangeTemp[TaskTimeRangeTemp.Count - 1];
                }
                else          //正排
                {
                    TaskTimeRangeTemp = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return (p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate || p2.DBegTime > adCanBegDate) && p2.cTaskType == 0; });
                    if (TaskTimeRangeTemp.Count > 0)
                        lTaskTimeRangeFind = TaskTimeRangeTemp[0];
                }
            }
            return lTaskTimeRangeFind;
        }
        public List<TaskTimeRange> GetAvailableTimeRangeList(DateTime adCanBegDate, Boolean bSchRev, Boolean bSchdule = true, int ai_workTime = 0,Boolean bIncludeWorkTime = true )
        {
            if (bSchdule && bIncludeWorkTime) bIncludeWorkTime = false;
            List<TaskTimeRange> lTaskTimeRangeList = new List<TaskTimeRange>();
            if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用
            {
                lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return p2.cTaskType == 0; });
                lTaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            }
            else                                 //产能有限
            {
                if (bSchRev)  //倒排
                {
                    if (bSchdule)  //正式排产
                    {
                        lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.cTaskType == 0; });
                    }
                    else           //模拟排产，取所有字段
                    {
                        lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.cTaskType == 0; });
                        if (bIncludeWorkTime)
                            lTaskTimeRangeList.AddRange(this.WorkTimeRangeList);
                    }
                    lTaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
                }
                else          //正排
                {
                    if (bSchdule)  //正式排产
                    {
                        lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DEndTime > adCanBegDate && p2.cTaskType == 0; });
                    }
                    else           //模拟排产，取所有字段
                    {
                        lTaskTimeRangeList = TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.DEndTime > adCanBegDate && p2.cTaskType == 0; });
                        int iCount = lTaskTimeRangeList.Count;
                        if (iCount < 1) //没有空闲时间段，说明已经排满了任务,正排返回第1条已排任务  2019-09-10
                        {
                            if (WorkTimeRangeList.Count > 0)
                                lTaskTimeRangeList.Add(WorkTimeRangeList[0]);
                        }
                        else
                        {
                            if (bIncludeWorkTime)
                                lTaskTimeRangeList.AddRange(this.WorkTimeRangeList);
                        }
                    }
                    if (lTaskTimeRangeList.Count > 1 )
                        lTaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                }
            }
            return lTaskTimeRangeList;
        }
        public DateTime GetAvailableTime(DateTime adCanBegDate, Boolean bSchRev = false)
        {
            TaskTimeRange lTaskTimeRange = GetAvailableTimeRange(adCanBegDate, bSchRev);
            if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用
            {
                return adCanBegDate;
            }
            else                                 //产能有限
            {
                if (bSchRev) //倒排，返回最大的可用时间
                {
                    if (lTaskTimeRange != null)
                    {
                        return lTaskTimeRange.DEndTime;
                    }
                    return adCanBegDate;
                }
                else
                {
                    if (lTaskTimeRange != null)
                    {
                        return lTaskTimeRange.DBegTime;
                    }
                    return DateTime.Today;
                }
            }
        }
        public ResTimeRange GetResTimeRange(DateTime adCanBegDate, string as_Type = "1")  //"1" 当前时段，"2" 上一时段 ,"3" 下一时段
        {
            List<ResTimeRange> lResTimeRangeList = this.resource.ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime <= adCanBegDate && p2.DEndTime > adCanBegDate; });
            if (lResTimeRangeList.Count > 0)
                return lResTimeRangeList[0];
            else
                return null;
        }
        public int TaskTimeRangeSplit(TaskTimeRange aToltalTaskRange, TaskTimeRange aNewTaskRange)
        {
            TaskTimeRange NoTaskTime1, NoTaskTime2;
            if (this.CIsInfinityAbility == "1")   //产能无限，整个时段都可用 ,aToltalTaskRange不变
            {
                WorkTimeRangeList.Add(aNewTaskRange);
            }
            else                                 //产能有限
            {
                try
                {
                    if (aNewTaskRange.iProcessProductID == SchParam.iProcessProductID && aNewTaskRange.iSchSdID == SchParam.iSchSdID)
                    {
                        int i = 1;
                    }
                    if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
                    {
                        throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                        return -1;
                    }
                    if (this.AllottedTime + aNewTaskRange.AllottedTime > this.HoldingTime)  //AllottedTime
                    {
                        throw new Exception(string.Format("出错位置1：排程时段分配时间大于时段总工时!TimeSchTask.TaskTimeRangeSplit！时段开始时间:{0}时段结束时间:{1}任务开始时间:{2}任务结束时间:{3}", aToltalTaskRange.dBegTime.ToString(), aToltalTaskRange.dEndTime.ToString(), aNewTaskRange.dBegTime.ToString(), aNewTaskRange.dEndTime.ToString()));
                        return -1;
                    }
                    if (aToltalTaskRange.DBegTime == aNewTaskRange.DBegTime && aToltalTaskRange.DEndTime == aNewTaskRange.DEndTime)
                    {
                        this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, null);
                    }
                    else if (aToltalTaskRange.DBegTime == aNewTaskRange.DBegTime)    //时间段头相同
                    {
                        NoTaskTime1 = GetNoWorkTaskTimeRange(aNewTaskRange.DEndTime, aToltalTaskRange.DEndTime);
                        this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1);
                        aToltalTaskRange.taskTimeRangePre = aNewTaskRange;  //当前空闲时间段的前任务是aNewTaskRange 
                    }
                    else if (aToltalTaskRange.DEndTime == aNewTaskRange.DEndTime)   //时间段尾相同
                    {
                        NoTaskTime1 = GetNoWorkTaskTimeRange(aToltalTaskRange.DBegTime, aNewTaskRange.DBegTime);
                        this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1);
                        aToltalTaskRange.taskTimeRangePost = aNewTaskRange;  //当前空闲时间段的后任务是aNewTaskRange 
                    }
                    else    ////时间段头、尾都不相同，分成三个时间段
                    {
                        NoTaskTime1 = GetNoWorkTaskTimeRange(aToltalTaskRange.DBegTime, aNewTaskRange.DBegTime);
                        NoTaskTime2 = GetNoWorkTaskTimeRange(aNewTaskRange.DEndTime, aToltalTaskRange.DEndTime);
                        this.ModifyResTimeRange(aNewTaskRange, aToltalTaskRange, NoTaskTime1, NoTaskTime2);
                        aToltalTaskRange.taskTimeRangePost = aNewTaskRange;  //当前空闲时间段的后任务是aNewTaskRange 
                        NoTaskTime2.taskTimeRangePre = aNewTaskRange;   //当前空闲时间段的前任务是aNewTaskRange 
                    }
                    if (this.cIsInfinityAbility != "1" && !CheckResTimeRange() )
                    {
                        throw new Exception("出错位置：排程时段检查出错TimeSchTask.CheckResTimeRange！时段开始时间:" + aToltalTaskRange.dBegTime.ToString() + "时段结束时间:" + aToltalTaskRange.dEndTime.ToString() + "任务开始时间:" + aNewTaskRange.dBegTime.ToString() + "任务结束时间:" + aNewTaskRange.dEndTime.ToString()); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                        return -1;
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
            return 1;
        }
        public TaskTimeRange GetNoWorkTaskTimeRange(DateTime dBegDate, DateTime dEndDate, Boolean bCreate = false)
        {
            TaskTimeRange NoTaskTime = new TaskTimeRange();
            if (dBegDate >= dEndDate)
            {
                throw new Exception("生成空闲任务时间段必须大于0！");
                return NoTaskTime;
            }
            NoTaskTime.cVersionNo = "";
            NoTaskTime.cTaskType = 0;  //空闲
            NoTaskTime.iSchSdID = -1;
            NoTaskTime.iProcessProductID = -1;
            NoTaskTime.iResProcessID = -1;
            NoTaskTime.cResourceNo = this.cResourceNo;
            NoTaskTime.CIsInfinityAbility = this.CIsInfinityAbility;
            NoTaskTime.DBegTime = dBegDate;//dBegDate.AddSeconds(1);   //生成的空闲时间段增加1秒
            NoTaskTime.DEndTime = dEndDate;
            NoTaskTime.AllottedTime = 0;
            NoTaskTime.HoldingTime = (int)((TimeSpan)(NoTaskTime.DEndTime - NoTaskTime.DBegTime)).TotalSeconds;
            NoTaskTime.resource = this.resource;
            NoTaskTime.resTimeRange = this;
            NoTaskTime.schProductRouteRes = null;
            NoTaskTime.schData = this.schData;
            if (bCreate) //创建一个空的任务时间段,初始化时调用
            {
                NoTaskTime.AddTaskTimeRange(this);
            }
            return NoTaskTime;
        }
        public TaskTimeRange CopyTaskTimeRange(TaskTimeRange aOldTaskRange, TaskTimeRange aNewTaskRange)
        {
            aOldTaskRange.cVersionNo = aNewTaskRange.cVersionNo;
            aOldTaskRange.iSchSdID = aNewTaskRange.iSchSdID;
            aOldTaskRange.iProcessProductID = aNewTaskRange.iProcessProductID;
            aOldTaskRange.iResProcessID = aNewTaskRange.iResProcessID;
            aOldTaskRange.cResourceNo = this.cResourceNo;
            aOldTaskRange.DBegTime = aNewTaskRange.DBegTime;
            aOldTaskRange.DEndTime = aNewTaskRange.DEndTime;
            aOldTaskRange.HoldingTime = aNewTaskRange.HoldingTime;
            aOldTaskRange.AllottedTime = aNewTaskRange.AllottedTime;
            aOldTaskRange.NotWorkTime = 0;
            aOldTaskRange.resource = aNewTaskRange.resource;
            aOldTaskRange.resTimeRange = aNewTaskRange.resTimeRange;
            aOldTaskRange.schProductRouteRes = aNewTaskRange.schProductRouteRes;
            aOldTaskRange.schData = aNewTaskRange.schData;
            aOldTaskRange.cTaskType = aNewTaskRange.cTaskType;  //空闲
            aOldTaskRange.iSchSdID = aNewTaskRange.iSchSdID;
            aOldTaskRange.iProcessProductID = aNewTaskRange.iProcessProductID;
            aOldTaskRange.iResProcessID = aNewTaskRange.iResProcessID;
            aOldTaskRange.iSchSNMax = SchParam.iSchSNMax;
            return aOldTaskRange;
        }
        public int MegTaskTimeRangeAll()
        {
            if (this.TaskTimeRangeList.Count <= 1) return 1;
            TaskTimeRange TaskTimeRangePre = null;//第一个不合并，从第二个时段开始
            TaskTimeRange TaskTimeRangeNew = null;
            this.TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            int iCount = this.TaskTimeRangeList.Count;
            int allottedTime = 0;
            for (int i = iCount - 1; i >= 0; i--)
            {
                TaskTimeRangeNew = this.TaskTimeRangeList[i];
                if (TaskTimeRangePre != null && TaskTimeRangePre.cTaskType == 0 && TaskTimeRangeNew.cTaskType == 0  && TaskTimeRangeNew.DEndTime == TaskTimeRangePre.DBegTime)
                {
                    TaskTimeRangeNew = MegTaskTimeRange(TaskTimeRangePre, TaskTimeRangeNew);
                    if (TaskTimeRangeNew == null)
                    {
                        throw new Exception("检验任务合并时间段出错,位置ReTimeRange.MegTaskTimeRangeAll");
                        return -1;
                    }
                }   
                TaskTimeRangePre = TaskTimeRangeNew;
            }
            return 1;
        }
        public TaskTimeRange MegTaskTimeRange(TaskTimeRange TaskTimeRangeLast, TaskTimeRange TaskTimeRangeNew)
        {
            if (TaskTimeRangeLast.cTaskType != 0) return null;    //时间大的时段
            if (TaskTimeRangeNew.cTaskType != 0) return null;
            TaskTimeRange NoWorkTaskTime = GetNoWorkTaskTimeRange(TaskTimeRangeNew.DBegTime, TaskTimeRangeLast.DEndTime, false);
            NoWorkTaskTime.taskTimeRangePre = TaskTimeRangeLast.taskTimeRangePre;  //前时间段的任务，写到后时间段任务
            NoWorkTaskTime.taskTimeRangePost = TaskTimeRangeNew.taskTimeRangePost;  //前时间段的任务，写到后时间段任务
            TaskTimeRangeNew.taskTimeRangePre = TaskTimeRangeLast.taskTimeRangePre;  //前时间段的任务，写到后时间段任务
            if (Math.Abs(TaskTimeRangeLast.HoldingTime + TaskTimeRangeNew.HoldingTime - NoWorkTaskTime.HoldingTime) > 5 )  //不能直接相等，可能有计算误差 2022-06-22
            {
                string message = string.Format(@"2、原时间段长[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],新时间段长[{4}],未分配时间段长[{5}]",
                                                               TaskTimeRangeLast.HoldingTime.ToString(), TaskTimeRangeLast.iSchSdID, TaskTimeRangeLast.iProcessProductID, TaskTimeRangeLast.cResourceNo, TaskTimeRangeNew.HoldingTime.ToString(), NoWorkTaskTime.HoldingTime.ToString());
                SchParam.Debug(message, "资源运算");
                throw new Exception("检验任务拆分后时间段出错,位置ReTimeRange.MegTaskTimeRange!" + message);
                return null;
            }
            if (!this.CheckResTimeRange())
            {
                string message = string.Format(@"2、原时间段长[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],新时间段长[{4}],未分配时间段长[{5}]",
                                                              TaskTimeRangeLast.HoldingTime.ToString(), TaskTimeRangeLast.iSchSdID, TaskTimeRangeLast.iProcessProductID, TaskTimeRangeLast.cResourceNo, TaskTimeRangeNew.HoldingTime.ToString(), NoWorkTaskTime.HoldingTime.ToString());
                SchParam.Debug(message, "资源运算");
                throw new Exception("清除任务时合并空闲时间段出错,位置ReTimeRange.MegTaskTimeRange！" + message);
                return null;
            }
            TaskTimeRangeLast.RemoveTaskTimeRange(this);
            TaskTimeRangeNew.RemoveTaskTimeRange(this);
            NoWorkTaskTime.AddTaskTimeRange(this);
            return NoWorkTaskTime;
        }
        public int CheckTaskOverlap(SchProductRouteRes as_SchProductRouteRes, DateTime dt_ResDate, Boolean bSchRev = false)
        {
            if (CheckCurTimeTaskOverlap(as_SchProductRouteRes, dt_ResDate, dt_ResDate, bSchRev) < 0) return -1;
            if (bSchRev == false)
            {
                if (CheckNextTimeTaskOverlap(as_SchProductRouteRes, this.DEndTime, dt_ResDate, bSchRev) < 0) return -1;
            }
            else
            {
                if (CheckNextTimeTaskOverlap(as_SchProductRouteRes, this.DBegTime, dt_ResDate, bSchRev) < 0) return -1;
            }
            return 1;
        }
        public int CheckCurTimeTaskOverlap(SchProductRouteRes as_SchProductRouteRes, DateTime dt_ResDate, DateTime adCanBegDate, Boolean bSchRev = false)
        {
            if (this.TaskTimeRangeList.Count < 1) return 1;  //没有排任务，都空闲
            if (this.CIsInfinityAbility == "1") return 1;    //资源产能无限时，不管是否重叠，不用检查
            try
            {
                TaskTimeRange TaskTimeRange2 = this.TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return (p1.iSchSdID == as_SchProductRouteRes.iSchSdID && p1.iProcessProductID == as_SchProductRouteRes.iProcessProductID && p1.iResProcessID == as_SchProductRouteRes.iResProcessID && (p1.DEndTime == dt_ResDate || p1.DBegTime == dt_ResDate)); });
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }
                if (TaskTimeRange2 != null)   //有排任务
                {
                    DateTime ldtResEndDate;
                    if (bSchRev == false)     //正排
                    {
                        ldtResEndDate = TaskTimeRange2.DEndTime;
                        List<TaskTimeRange> TaskTimeRangeList1 = this.resource.GetTaskTimeRangeList().FindAll(delegate(TaskTimeRange p1) { return p1.DBegTime >= ldtResEndDate && p1.CResourceNo == this.cResourceNo; });
                        if (TaskTimeRangeList1.Count < 1) return -1;
                        TaskTimeRange TaskTimeRange3 = TaskTimeRangeList1[0];
                        if (TaskTimeRange3 != null && TaskTimeRange3.cTaskType == 1 && TaskTimeRange3.iSchSdID != TaskTimeRange2.iSchSdID && TaskTimeRange3.iProcessProductID != TaskTimeRange2.iProcessProductID && TaskTimeRange3.iResProcessID != TaskTimeRange2.iResProcessID)
                        {
                            adCanBegDate = TaskTimeRange3.DEndTime;
                            return -1;         //有重叠
                        }
                    }
                    else                      //倒排
                    {
                        ldtResEndDate = TaskTimeRange2.DBegTime;
                        List<TaskTimeRange> TaskTimeRangeList1 = this.resource.GetTaskTimeRangeList(false).FindAll(delegate(TaskTimeRange p1) { return p1.DEndTime <= ldtResEndDate && p1.CResourceNo == this.cResourceNo; });
                        if (TaskTimeRangeList1.Count < 1) return -1;
                        TaskTimeRange TaskTimeRange3 = TaskTimeRangeList1[0];
                        if (TaskTimeRange3 != null && TaskTimeRange3.cTaskType == 1 && TaskTimeRange3.iSchSdID != TaskTimeRange2.iSchSdID && TaskTimeRange3.iProcessProductID != TaskTimeRange2.iProcessProductID && TaskTimeRange3.iResProcessID != TaskTimeRange2.iResProcessID)
                        {
                            adCanBegDate = TaskTimeRange3.DBegTime;
                            return -1;         //有重叠
                        }
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception("检验任务是否重叠出错,位置ReTimeRange.CheckCurTimeTaskOverlap！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1;       //无重叠
        }
        public int CheckNextTimeTaskOverlap(SchProductRouteRes as_SchProductRouteRes, DateTime dt_ResDate, DateTime adCanBegDate, Boolean bSchRev = false)
        {
            ResTimeRange ResTimeRange1 = null;
            if (this.CIsInfinityAbility == "1") return 1;    //资源产能无限时，不管是否重叠，不用检查
            try
            {
                if (bSchRev == false)     //正排
                {
                    List<ResTimeRange> ResTimeRangeList = this.resource.ResTimeRangeList.FindAll(delegate(ResTimeRange p1) { return p1.DBegTime >= dt_ResDate && p1.CResourceNo == this.cResourceNo; });
                    ResTimeRangeList.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    if (ResTimeRangeList.Count > 0)
                    {
                        ResTimeRange1 = ResTimeRangeList[0];
                        return ResTimeRange1.CheckCurTimeTaskOverlap(as_SchProductRouteRes, ResTimeRange1.DBegTime, adCanBegDate, bSchRev);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else                      //倒排
                {
                    List<ResTimeRange> ResTimeRangeList = this.resource.ResTimeRangeList.FindAll(delegate(ResTimeRange p1) { return p1.DEndTime <= dt_ResDate && p1.CResourceNo == this.cResourceNo; });
                    ResTimeRangeList.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    if (ResTimeRangeList.Count > 0)
                    {
                        ResTimeRange1 = ResTimeRangeList[ResTimeRangeList.Count - 1];
                        return ResTimeRange1.CheckCurTimeTaskOverlap(as_SchProductRouteRes, ResTimeRange1.DEndTime, adCanBegDate, bSchRev);
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception("检验任务是否重叠出错,位置ReTimeRange.CheckNextTimeTaskOverlap！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1;       //无重叠
        }
        public int ModifyResTimeRange(TaskTimeRange aNewTaskRange,TaskTimeRange oldTaskTimeRange, TaskTimeRange NoTaskTime1, TaskTimeRange NoTaskTime2 = null)
        {
            if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
            {
                throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                return -1;
            }
            aNewTaskRange.AddWorkimeRange(this);
            if (NoTaskTime1 == null)
            {
                oldTaskTimeRange.RemoveTaskTimeRange(this);
            }
            else //替换原来时间段
            {
                oldTaskTimeRange.AddTaskTimeRange(this, NoTaskTime1);
            }
            if (NoTaskTime2 != null)
            {
                NoTaskTime2.AddTaskTimeRange(this);
            }
            if (!CheckResTimeRange() && this.cIsInfinityAbility != "1")
            {
                throw new Exception("出错位置：排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                return -1;
            }
            return 1;
        }
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
                if (value < new DateTime(2000,1,1))
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
        public int HoldingTime
        {
            get
            {
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
            }
            set
            {
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
                        foreach (TaskTimeRange taskTimeRange in this.WorkTimeRangeList)
                        {
                            allottedTimeTemp += taskTimeRange.allottedTime;
                        }
                        if (allottedTime < allottedTimeTemp)
                            allottedTime = allottedTime;
                    }
                    return allottedTimeTemp;
                }
            }
            set
            {   //只有TaskTimeRange才可以设置AllottedTime  
                if (value >= 0)
                {
                    allottedTime = value;
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
                    if (this.GetType().ToString() == "Algorithm.TaskTimeRange")
                    {
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
                }
            }
            set
            {   //只有TaskTimeRange才可以设置AllottedTime
            }
        }
        public int MaintainTime  //维修、故障时间
        {
            get
            {
                if (this.GetType().ToString() == "Algorithm.TaskTimeRange")
                {
                    if (((TaskTimeRange)this).cTaskType == 2) //任务时间类型： 0 空闲， 1 加工时间 2 维修时间  ---3 前准备时间 4 后准备时间 ，暂时没用
                        return this.holdingTime;   //可用时间
                    else
                        return 0;
                }
                else   //"Algorithm.ResTimeRange" 资源时段的占用时间，取所有已分配任务的已占用时间
                {
                    maintainTime = 0;
                    foreach (TaskTimeRange taskTimeRange in this.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return (p1.cTaskType == 2); }))
                    {
                        maintainTime += taskTimeRange.HoldingTime;
                    }
                    return maintainTime;
                }
            }
        }
        private TimeRangeAttribute attribute;
        public TimeRangeAttribute Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }
        public List<TaskTimeRange> TaskTimeRangeList { 
            get { return taskTimeRangeList; }
            set {
                if (this.cResourceNo == "gys20097")
                {
                    int i = 0;
                }
                taskTimeRangeList = value;                                
            }
        }
        public int state;
        public double loadRate;
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
    public enum TimeRangeAttribute
    {
        Work = 0,
        Maintain = 1,
        Snag = 2,
        Overtime = 3,
        MayOvertime = 4
    }
}