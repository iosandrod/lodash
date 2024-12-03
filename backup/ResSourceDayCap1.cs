using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Algorithm
{
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
        public List<ResTimeRange> ResTimeRangeList = new List<ResTimeRange>(10);
        public List<TaskTimeRange> taskTimeRangeList = new List<TaskTimeRange>(10);
        public List<TaskTimeRange> WorkTimeRangeList = new List<TaskTimeRange>(10);
        public ResTimeRange ResTimeRangePre;          //前资源时间段
        public ResTimeRange ResTimeRangePost;         //后资源时间段
        public int iSchSdID = -1;                     //记录更新、新增时间段的任务ID
        public int iProcessProductID = -1;
        public int iResProcessID = -1;
        public int iSchSNMax = -1;
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
        }
        public ResSourceDayCap()
        {
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
                    maintainTime = 0;
                    foreach (TaskTimeRange taskTimeRange in this.TaskTimeRangeList.FindAll(delegate (TaskTimeRange p1) { return (p1.cTaskType == 2); }))
                    {
                        maintainTime += taskTimeRange.HoldingTime;
                    }
                    return maintainTime;
            }
        }
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
}