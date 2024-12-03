using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace Algorithm
{
    public class ScheduleTask : IComparable
    {
        public String productOrderID;
        public float batch;
        public int scheduleStyle = 0;
        public string RelateID
        {
            get
            {
                return relateID;
            }
        }
        private string relateID;
        public int ID
        {
            get
            {
                return id;
            }
        }
        private int id;
        public TaskType type;
        public string name;
        public float earliestStartTime;
        public float latestEndTime;
        public float planStartTime = 0;
        public float planEndTime;
        public float actualStartTime;
        public float actualEndTime;
        public float preProcessingTime = 0;
        public float postProcessingTime = 0;
        public float onepeiceProcessTime;
        public float postTaskDelayTime;
        public float postTaskDelayBatch;
        public float ProcessingTime
        {
            get
            {
                return batch * onepeiceProcessTime;// +preProcessingTime + postProcessingTime;
            }
        }
        public ArrayList planProcessingTimeList = new ArrayList();
        public ArrayList actualProcessingTimeList = new ArrayList();
        public float actualProcessingTime
        {
            get
            {
                float aTime = 0;
                foreach (ResTimeRange t in actualProcessingTimeList)
                {
                    aTime += t.HoldingTime;
                }
                return aTime;
            }
        }
        public ScheduleResource selectedMainResoure;
        public float planEarliestStartTime;
        public float planLatestEndTime;
        public bool isFixedTask = false;
        public bool isKeyTask = false;
        public float postInternalTimeMin = 0;
        public float transportTime = 0;
        public float postInternalTimeMax = Scheduling.maxTimeValue;
        public int moveType;
        public float moveTime;
        public float moveBatch;
        public float processQuanitys = 1;
        public string affiliationProduct;
        public string affiliationOrder;
        public ArrayList preTaskList = new ArrayList();
        public ArrayList postTaskList = new ArrayList();
        public int proState;
        public double passRate;
        public ArrayList maySelecetResource = new ArrayList(3);
        public ScheduleTask(int id, string relateID)
        {
            this.id = id;
            this.relateID = relateID;
        }
        public ScheduleTask()
        {
        }
        public void iniData()
        {
            this.affiliationProduct;
            this.name;
            this.passRate;
            this.proState;
            this.type;
        }
        public int CompareTo(object obj)
        {
            if (obj is ScheduleTask)
            {
                ScheduleTask tempST = (ScheduleTask)obj;
                if (this.planStartTime - tempST.planStartTime > 0.001)
                {
                    return 1;
                }
                else
                {
                    if (tempST.planStartTime - this.planStartTime > 0.001)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                throw new ArgumentException("对象非ScheduleTask");
            }
        }
        public void clear()
        {
            if (this.earliestStartTime < 0)
            {
                this.earliestStartTime = 0;
            }
            this.latestEndTime = Scheduling.maxTimeValue;
            this.onepeiceProcessTime = 0;
            this.planEndTime = Scheduling.maxTimeValue;
            this.planProcessingTimeList.Clear();
            this.planStartTime = 0;
        }
        public ScheduleTask deepClone()
        {
            ScheduleTask st = new ScheduleTask();///产生一个临时任务,将现有任务的参数拷贝到临时任务中,并返回临时任务.
            st.actualEndTime = actualEndTime;
            st.actualStartTime = actualStartTime;
            st.affiliationOrder = affiliationOrder;
            st.affiliationProduct = affiliationProduct;
            st.batch = batch;
            st.earliestStartTime = earliestStartTime;
            st.id = id;
            st.isFixedTask = isFixedTask;
            st.isKeyTask = isKeyTask;
            st.latestEndTime = latestEndTime;
            st.name = name;
            st.onepeiceProcessTime = onepeiceProcessTime;
            st.planEarliestStartTime = planEarliestStartTime;
            st.planEndTime = planEndTime;
            st.planLatestEndTime = planLatestEndTime;
            st.planStartTime = planStartTime;
            st.postInternalTimeMax = postInternalTimeMax;
            st.postInternalTimeMin = postInternalTimeMin;
            st.postProcessingTime = postProcessingTime;
            st.preProcessingTime = preProcessingTime;
            st.proState = proState;
            st.relateID = relateID;
            st.selectedMainResoure = selectedMainResoure;
            st.transportTime = transportTime;
            st.type = type;
            for (int i = 0; i < planProcessingTimeList.Count; i++)
            {
                st.planProcessingTimeList.Add(((ResTimeRange)planProcessingTimeList[i]).Clone());
            }
            for (int i = 0; i < actualProcessingTimeList.Count; i++)
            {
                st.actualProcessingTimeList.Add(((ResTimeRange)actualProcessingTimeList[i]).Clone());
            }
            foreach (ScheduleTask stask in preTaskList)
            {
                st.preTaskList.Add(stask);
            }
            foreach (ScheduleTask stask in postTaskList)
            {
                st.postTaskList.Add(stask);
            }
            return st;
        }
    }
    public enum TaskType
    {
        Design,        //设计
        Normal,        //普通
        Outsource      //外协
    }
    class unitTime
    {
        string resourceID;
        int singleTime;
    }
}