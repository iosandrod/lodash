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
        //   public  int iTaskID = -1;         //t_SchProductRoute.iProcessProductID 对应的加工任务,用于ScheduleTask,ScheduleResource,TimeRange等对象中。
        public int cTaskType = 1;           //任务时间类型： 0 空闲， 1 加工时间 2 维修时间  ---3 前准备时间 4 后准备时间 ，暂时没用
        public double iResReqQty = 0;       //时段内计划加工任务数量
        public int iResRationHour = 0;   //时段内计划加工时间，秒
        public ResTimeRange resTimeRange = null;   //对应的资源时间段

        public TaskTimeRange taskTimeRangePre = null;          //前资源任务时间段,用于判断前面是否有任务，倒排 2020-01-07 
        public TaskTimeRange taskTimeRangePost = null;         //后资源任务时间段,用于判断后面是否有任务，正排

        //任务时段清除,清除已排工序使用
        public void TaskTimeRangeClear(SchProductRouteRes as_SchProductRouteRes)
        {
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID )
            {
                int i = 1;
            }

            try
            {
                //检查删除后的时间段是否正确 2021-11-15 JonasCheng
                if (!this.resTimeRange.CheckResTimeRange())
                {
                    //Clipboard.SetText(this.ToString());
                    throw new Exception("出错位置：倒排删除已排任务时间段时，排程时段检查出错TimeSchTask.TaskTimeRangeSplit！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                    return;
                }


                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)
                {
                    int i = 1;
                }

                //1、资源时间段任务列表中删除 当前任务时间段，并建个新的空闲时间段
                //TaskTimeRange TaskTimeRange2 = this.resTimeRange.TaskTimeRangeList.Find(delegate(TaskTimeRange p) { return p.iSchSdID == this.iSchSdID && p.iProcessProductID == this.iProcessProductID && p.iResProcessID == this.iResProcessID && p.DBegTime == this.DBegTime; });
                //this.resTimeRange.TaskTimeRangeList.Remove(TaskTimeRange2);

                //TaskTimeRange TaskTimeRange2 = this.resTimeRange.WorkTimeRangeList.Find(delegate (TaskTimeRange p) { return p.iSchSdID == this.iSchSdID && p.iProcessProductID == this.iProcessProductID && p.iResProcessID == this.iResProcessID && p.DBegTime == this.DBegTime; });
                ////this.resTimeRange.TaskTimeRangeList.Remove(TaskTimeRange2);
                //this.resTimeRange.WorkTimeRangeList.Remove(TaskTimeRange2);

                //2、资源所有任务列表中删除 当前任务时间段,，并建个新的空闲时间段               
                //TaskTimeRange TaskTimeRange3 = this.resource.TaskTimeRangeList.Find(delegate(TaskTimeRange p) { return p.iSchSdID == this.iSchSdID && p.iProcessProductID == this.iProcessProductID && p.iResProcessID == this.iResProcessID && p.DBegTime == this.DBegTime; });
                //this.resource.TaskTimeRangeList.Remove(TaskTimeRange3);

                //3、有限产能,需增加空闲时间段，无限产能时不用
                if (this.resTimeRange.CIsInfinityAbility != "1")
                {
                    TaskTimeRange TaskTimeRange2 = this.resTimeRange.WorkTimeRangeList.Find(delegate (TaskTimeRange p) { return p.iSchSdID == this.iSchSdID && p.iProcessProductID == this.iProcessProductID && p.iResProcessID == this.iResProcessID && p.DBegTime == this.DBegTime; });

                    if (TaskTimeRange2 == null)
                    {
                        //Clipboard.SetText(this.ToString());
                        //throw new Exception("清除任务时出错,对应的加工任务时间段没找到,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iProcessProductID);
                        return;
                    }

                    try
                    {
                        
                        //4.1 建一个新的空闲任务时间段
                        TaskTimeRange NoWorkTaskTimeRange = base.GetNoWorkTaskTimeRange(TaskTimeRange2.DBegTime, TaskTimeRange2.DEndTime,false);
                        //this.resTimeRange.TaskTimeRangeList.Add(NoWorkTaskTimeRange);
                        //this.resource.TaskTimeRangeList.Add(NoWorkTaskTimeRange);

                        NoWorkTaskTimeRange.AddTaskTimeRange(this.resTimeRange);

                        //this.resTimeRange.TaskTimeRangeList.Remove(TaskTimeRange2);
                        //this.resTimeRange.WorkTimeRangeList.Remove(TaskTimeRange2);
                        TaskTimeRange2.RemoveWorkTimeRange(this.resTimeRange);

                        //检查删除后的时间段是否正确 2021-11-15 JonasCheng
                        if (!this.resTimeRange.CheckResTimeRange())
                        {
                            //Clipboard.SetText(this.ToString());
                            throw new Exception("清除任务时出错,倒排删除已排任务时间段,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iProcessProductID);
                            return;
                        }
                    }

                    catch (Exception error)
                    {
                        throw new Exception("清除任务时出错,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iSchSdID + ";" + as_SchProductRouteRes.iProcessProductID + ";任务号" + as_SchProductRouteRes.iResProcessID + "开工时间:" + this.DBegTime.ToString() + "\n\r " + error.Message.ToString());
                        return;
                    }

                    //4.2 合并空闲时段,如果有多个空闲时间段，连续的合并
                    if (this.resTimeRange.TaskTimeRangeList.Count > 1)
                        this.resTimeRange.MegTaskTimeRangeAll();

                    //检查删除后的时间段是否正确 2021-11-15 JonasCheng
                    if (!this.resTimeRange.CheckResTimeRange())
                    {
                        //Clipboard.SetText(this.ToString());
                        throw new Exception("清除任务时出错,倒排删除已排任务时间段,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iProcessProductID );
                        return ;
                    }

                }

                //4、任务资源列表中删除 当前任务时间段
                TaskTimeRange TaskTimeRange1 = as_SchProductRouteRes.TaskTimeRangeList.Find(delegate (TaskTimeRange p) { return p.iSchSdID == this.iSchSdID && p.iProcessProductID == this.iProcessProductID && p.iResProcessID == this.iResProcessID && p.DBegTime == this.DBegTime; });
                as_SchProductRouteRes.TaskTimeRangeList.Remove(TaskTimeRange1);


            }
            catch (Exception error)
            {
                throw new Exception("清除任务时出错,位置ReTimeRange.TaskTimeRangeClear！工序ID号：" + as_SchProductRouteRes.iSchSdID + ";" + as_SchProductRouteRes.iProcessProductID + ";任务号" + as_SchProductRouteRes.iResProcessID + "开工时间:"+ this.DBegTime.ToString()+ "\n\r " + error.Message.ToString());
                return;
            }



        }

        //空闲任务添加
        public int AddTaskTimeRange(ResTimeRange as_ResTimeRange, TaskTimeRange as_NewTaskTimeRange = null)
        {
            //if (this.CResourceNo == "gys20097" && (as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19") || as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19") ) )
            //{
            //    int i = 0;
            //}
            if (as_ResTimeRange.iSchSdID == SchParam.iSchSdID && as_ResTimeRange.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }


            if (this.CResourceNo == "gys20097" && this.resTimeRange.iPeriodID == 629217169)
            {
                int i = 0;
            }

            //修改原来时间段
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
        //空闲任务删除
        public int RemoveTaskTimeRange(ResTimeRange as_ResTimeRange)
        {
            //if (this.CResourceNo == "gys20097" && (as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19") || as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19")))
            //{
            //    int i = 0;
            //}

            if (as_ResTimeRange.iSchSdID == SchParam.iSchSdID && as_ResTimeRange.iProcessProductID == SchParam.iProcessProductID )  //调试断点1 SchProduct
            {
                int i = 1;
            }

            if (this.CResourceNo == "gys20097" && this.resTimeRange.iPeriodID == 629217169)
            {
                int i = 0;
            }
            //this.resTimeRange = null;
            as_ResTimeRange.TaskTimeRangeList.Remove(this);
            return 1;
        }

        //工作任务添加
        public int AddWorkimeRange(ResTimeRange as_ResTimeRange)
        {
            //if (this.CResourceNo == "gys20097" && (as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19") || as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19")))
            //{
            //    int i = 0;
            //}
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
        //工作任务删除
        public int RemoveWorkTimeRange(ResTimeRange as_ResTimeRange)
        {
            //if (this.CResourceNo == "gys20097" && (as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19") || as_ResTimeRange.DBegTime.Date == DateTime.Parse("2021-11-19")))
            //{
            //    int i = 0;
            //}
            if (as_ResTimeRange.iSchSdID == SchParam.iSchSdID && as_ResTimeRange.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }

            if (this.CResourceNo == "gys20097" && this.resTimeRange.iPeriodID == 629217169)
            {
                int i = 0;
            }

            //this.resTimeRange = null;
            as_ResTimeRange.WorkTimeRangeList.Remove(this);
            return 1;
        }




    }
}
