using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace Algorithm
{
    [Serializable]
    public class TaskTimeRange : ResTimeRange
    {
        public SchProductRouteRes schProductRouteRes = null;   //资源任务对象        
        public int iSchSdID = -1;            //排程产品号ID
        public string cVersionNo;            //排程版本号
        public int iProcessProductID = -1;  //排程工序ID
        public int iResProcessID = -1;      //排程资源工序ID
        public string cWoNo = "";             //工单号
        public int cTaskType = 1;           //任务时间类型： 0 空闲， 1 加工时间 2 维修时间  ---3 前准备时间 4 后准备时间 ，暂时没用
        public double iResReqQty = 0;       //时段内计划加工任务数量
        public int iResRationHour = 0;   //时段内计划加工时间，秒
        public ResTimeRange resTimeRange = null;   //对应的资源时间段
        public TaskTimeRange taskTimeRangePre = null;          //前资源任务时间段,用于判断前面是否有任务，倒排 2020-01-07 
        public TaskTimeRange taskTimeRangePost = null;         //后资源任务时间段,用于判断后面是否有任务，正排
        public void TaskTimeRangeClear(SchProductRouteRes as_SchProductRouteRes)
        {
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID )
            {
                int i = 1;
            }
            try
            {
                if (!this.resTimeRange.CheckResTimeRange())
                {
                    throw new Exception("出错位置：倒排删除已排任务时间段时，排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                    return;
                }
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)
                {
                    int i = 1;
                }
                if (this.resTimeRange.CIsInfinityAbility != "1")
                {
                    TaskTimeRange TaskTimeRange2 = this.resTimeRange.WorkTimeRangeList.Find(delegate (TaskTimeRange p) { return p.iSchSdID == this.iSchSdID && p.iProcessProductID == this.iProcessProductID && p.iResProcessID == this.iResProcessID && p.DBegTime == this.DBegTime; });
                    if (TaskTimeRange2 == null)
                    {
                        return;
                    }
                    try
                    {
                        TaskTimeRange NoWorkTaskTimeRange = base.GetNoWorkTaskTimeRange(TaskTimeRange2.DBegTime, TaskTimeRange2.DEndTime,false);
                        NoWorkTaskTimeRange.AddTaskTimeRange(this.resTimeRange);
                        TaskTimeRange2.RemoveWorkTimeRange(this.resTimeRange);
                        if (!this.resTimeRange.CheckResTimeRange())
                        {
                            throw new Exception("清除任务时出错,倒排删除已排任务时间段,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iProcessProductID);
                            return;
                        }
                    }
                    catch (Exception error)
                    {
                        throw new Exception("清除任务时出错,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iSchSdID + ";" + as_SchProductRouteRes.iProcessProductID + ";任务号" + as_SchProductRouteRes.iResProcessID + "开工时间:" + this.DBegTime.ToString() + "\n\r " + error.Message.ToString());
                        return;
                    }
                    if (this.resTimeRange.TaskTimeRangeList.Count > 1)
                        this.resTimeRange.MegTaskTimeRangeAll();
                    if (!this.resTimeRange.CheckResTimeRange())
                    {
                        throw new Exception("清除任务时出错,倒排删除已排任务时间段,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iProcessProductID );
                        return ;
                    }
                }
                TaskTimeRange TaskTimeRange1 = as_SchProductRouteRes.TaskTimeRangeList.Find(delegate (TaskTimeRange p) { return p.iSchSdID == this.iSchSdID && p.iProcessProductID == this.iProcessProductID && p.iResProcessID == this.iResProcessID && p.DBegTime == this.DBegTime; });
                as_SchProductRouteRes.TaskTimeRangeList.Remove(TaskTimeRange1);
            }
            catch (Exception error)
            {
                throw new Exception("清除任务时出错,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iSchSdID + ";" + as_SchProductRouteRes.iProcessProductID + ";任务号" + as_SchProductRouteRes.iResProcessID + "开工时间:"+ this.DBegTime.ToString()+ "\n\r " + error.Message.ToString());
                return;
            }
        }
        public int AddTaskTimeRange(ResTimeRange as_ResTimeRange, TaskTimeRange as_NewTaskTimeRange = null)
        {
            if (as_ResTimeRange.iSchSdID == SchParam.iSchSdID && as_ResTimeRange.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            if (this.CResourceNo == "gys20097" && this.resTimeRange.iPeriodID == 629217169)
            {
                int i = 0;
            }
            if (as_NewTaskTimeRange != null)
            {
                this.DBegTime = as_NewTaskTimeRange.DBegTime;
                this.DEndTime = as_NewTaskTimeRange.DEndTime;
            }
            else  //新增空闲时间段
            {
                this.resTimeRange = as_ResTimeRange;
                as_ResTimeRange.TaskTimeRangeList.Add(this);
            }
            return 1;
        }
        public int RemoveTaskTimeRange(ResTimeRange as_ResTimeRange)
        {
            if (as_ResTimeRange.iSchSdID == SchParam.iSchSdID && as_ResTimeRange.iProcessProductID == SchParam.iProcessProductID )  //调试断点1 SchProduct
            {
                int i = 1;
            }
            if (this.CResourceNo == "gys20097" && this.resTimeRange.iPeriodID == 629217169)
            {
                int i = 0;
            }
            as_ResTimeRange.TaskTimeRangeList.Remove(this);
            return 1;
        }
        public int AddWorkimeRange(ResTimeRange as_ResTimeRange)
        {
            if (as_ResTimeRange.iSchSdID == SchParam.iSchSdID && as_ResTimeRange.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            if (this.CResourceNo == "gys20097" && this.resTimeRange.iPeriodID == 629217169)
            {
                int i = 0;
            }
            this.resTimeRange = as_ResTimeRange;
            as_ResTimeRange.WorkTimeRangeList.Add(this);
            return 1;
        }
        public int RemoveWorkTimeRange(ResTimeRange as_ResTimeRange)
        {
            if (as_ResTimeRange.iSchSdID == SchParam.iSchSdID && as_ResTimeRange.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            if (this.CResourceNo == "gys20097" && this.resTimeRange.iPeriodID == 629217169)
            {
                int i = 0;
            }
            as_ResTimeRange.WorkTimeRangeList.Remove(this);
            return 1;
        }
    }
}