using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Algorithm
{
    /// <summary>
    /// 任务类，代表与资源关联的某工序
    /// </summary>
    public class ScheduleTask : IComparable
    {
        public String productOrderID;


        /// <summary>
        /// 任务批量
        /// </summary>
        public float batch;

        /// <summary>
        /// 排产方式，0=附加，1=插入
        /// </summary>
        public int scheduleStyle = 0;

        /// <summary>
        /// 任务关联编号,唯一
        /// </summary>
        public string RelateID
        {
            get
            {
                return relateID;
            }
        }

        /// <summary>
        /// 属性RelateID值
        /// </summary>
        private string relateID;

        /// <summary>
        /// 任务原始编号
        /// </summary>
        public int ID
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// 属性ID值
        /// </summary>
        private int id;

        /// <summary>
        /// 任务类型，分为：设计、普通、外协等
        /// </summary>
        public TaskType type;

        /// <summary>
        /// 任务名称
        /// </summary>
        public string name;

        /// <summary>
        /// 任务允许最早开始时间，为距离原点时间秒数
        /// </summary>
        public float earliestStartTime;

        /// <summary>
        /// 任务允许最晚结束时间，为距离原点时间秒数
        /// </summary>
        public float latestEndTime;

        /// <summary>
        /// 任务计划开始时间，为距离原点时间秒数
        /// </summary>
        public float planStartTime = 0;

        /// <summary>
        /// 任务计划结束时间，为距离原点时间秒数
        /// </summary>
        public float planEndTime;

        /// <summary>
        /// 任务实际开始时间，为距离原点时间秒数
        /// </summary>
        public float actualStartTime;

        /// <summary>
        /// 任务实际结束时间，为距离原点时间秒数
        /// </summary>
        public float actualEndTime;

        /// <summary>
        /// 任务前处理时间，单位秒,由加工资源决定
        /// </summary>
        public float preProcessingTime = 0;

        /// <summary>
        /// 任务后处理时间，单位秒，由加工资源决定
        /// </summary>
        public float postProcessingTime = 0;

        /// <summary>
        /// 单件加工时间，单位秒，由加工资源决定
        /// </summary>
        public float onepeiceProcessTime;

        /// <summary>
        /// 后工序延迟时间，单位秒
        /// </summary>
        public float postTaskDelayTime;

        /// <summary>
        /// 后工序延迟批量
        /// </summary>
        public float postTaskDelayBatch;

        /// <summary>
        /// 任务加工时间，单位秒
        /// </summary>
        public float ProcessingTime
        {
            get
            {
                return batch * onepeiceProcessTime;// +preProcessingTime + postProcessingTime;
            }
        }

        /// <summary>
        /// 任务计划加工时间段列表，为TimeRange对象集合
        /// </summary>
        public ArrayList planProcessingTimeList = new ArrayList();

        /// <summary>
        /// 任务实际加工时间段列表，为TimeRange对象集合
        /// </summary>
        public ArrayList actualProcessingTimeList = new ArrayList();

        /// <summary>
        /// 任务实际加工时间，单位为秒
        /// </summary>
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

        /// <summary>
        /// 任务所选加工主资源
        /// </summary>
        public ScheduleResource selectedMainResoure;

        /// <summary>
        /// 任务计划允许的最早加工时间，为距离原点时间秒数,排程后得出
        /// </summary>
        public float planEarliestStartTime;

        /// <summary>
        /// 任务计划允许的最晚结束时间，为距离原点时间秒数，排程后得出
        /// </summary>
        public float planLatestEndTime;

        /// <summary>
        /// 最否固定任务
        /// </summary>
        public bool isFixedTask = false;

        /// <summary>
        /// 是否关键任务
        /// </summary>
        public bool isKeyTask = false;

        /// <summary>
        /// 与后任务允许间隔最小值
        /// </summary>
        public float postInternalTimeMin = 0;

        /// <summary>
        /// 最小运送批量
        /// </summary>
        //public float transportBatch = 1;

        /// <summary>
        /// 每次运送到下一任务加工场所所需时间，单位为秒
        /// </summary>
        public float transportTime = 0;

        /// <summary>
        /// 与后任务允许间隔最大值
        /// </summary>
        public float postInternalTimeMax = Scheduling.maxTimeValue;

        /// <summary>
        /// 移动方式，0=整批移动；1=按时移动，2=按量移动
        /// </summary>
        public int moveType;

        /// <summary>
        /// 按时移动时，移动时间间隔
        /// </summary>
        public float moveTime;

        /// <summary>
        /// 按量移动时，移动批量
        /// </summary>
        public float moveBatch;

        /// <summary>
        /// 工序序数
        /// </summary>
        public float processQuanitys = 1;

        /// <summary>
        /// 所属产品
        /// </summary>
        public string affiliationProduct;

        /// <summary>
        /// 所属订单
        /// </summary>
        public string affiliationOrder;

        /// <summary>
        /// 前任务列表，为任务集合
        /// </summary>
        public ArrayList preTaskList = new ArrayList();

        /// <summary>
        /// 后任务列表，为任务集合
        /// </summary>
        public ArrayList postTaskList = new ArrayList();

        /// <summary>
        /// 加工状态
        /// </summary>
        /// <remarks>
        /// 0=不能开始加工;
        /// 1=可以开始加工;
        /// 2=正在加工;
        /// 3=加工完成
        /// </remarks>
        public int proState;

        /// <summary>
        /// 加工合格率，影响下一工序批量
        /// </summary>
        public double passRate;

        /// <summary>
        /// 可选择资源列表，为ScheduleResource集合
        /// </summary>
        public ArrayList maySelecetResource = new ArrayList(3);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">任务原始编号</param>
        /// <param name="relateID" >任务关联编号</param>
        public ScheduleTask(int id, string relateID)
        {

            this.id = id;

            this.relateID = relateID;

            //iniData();

        }

        /// <summary>
        /// 空构造函数
        /// </summary>
        public ScheduleTask()
        {

        }

        /// <summary>
        /// 初始化任务属性
        /// </summary>
        public void iniData()
        {
            //取内存数据库数据赋给各属性
            /*this.affiliationOrder;
            this.affiliationProduct;
            this.name;
            this.passRate;
            this.proState;
            this.type;
            */

        }

        /// <summary>
        /// 实现IComparable接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
                //return (int)(this.planStartTime - ((ScheduleTask)obj).planStartTime);
            }
            else
            {
                throw new ArgumentException("对象非ScheduleTask");
            }
        }

        /// <summary>
        /// 清除计算过程数据，使对象恢复初始状态
        /// </summary>
        public void clear()
        {
            //this.earliestStartTime = 0;
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
        /// <summary>
        /// 拷贝实体副本
        /// </summary>
        /// <remarks>将任务实体中的每一个数据拷贝到一个新的任务实体中.</remarks>
        /// <returns>实体的拷贝</returns>
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
            //st.transportBatch = transportBatch;
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

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType
    {
        Design,        //设计
        Normal,        //普通
        Outsource      //外协
    }

    /*
    /// <summary>
    /// 任务在具体资源上的单件加工时间信息
    /// </summary>
    class unitTime
    {
        /// <summary>
        /// 资源编号
        /// </summary>
        string resourceID;
        
        /// <summary>
        /// 单件加工时间
        /// </summary>
        int singleTime;
    }
    */
}
