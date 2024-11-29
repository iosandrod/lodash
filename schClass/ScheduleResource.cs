using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Algorithm
{
    /// <summary>
    /// 需加入多资源时段合并，资源产能细化
    /// </summary>
    public class ScheduleResource
    {
        /// <summary>
        /// 内码，唯一标识
        /// </summary>
        public long ID
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// 属性ID值
        /// </summary>
        private long id;

        /// <summary>
        /// 类型，0=主资源；1=副资源；2=资源集群；3=加工单元
        /// </summary>
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }

        }

        /// <summary>
        /// 资源数量
        /// </summary>
        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

   
        public int isKey;
        
        

        /// <summary>
        /// 属性Number值
        /// </summary>
        private int number;

        /// <summary>
        /// 资源占用方式,0=独占；1=共享；2=无限产能
        /// </summary>
        public int OccupyStyle
        {
            get
            {
                return occupyStyle;
            }
            set
            {
                occupyStyle = value;
            }
        }

        public ArrayList capacityList = new ArrayList(5);

        /// <summary>
        /// 属性OccupyStyle值
        /// </summary>
        private int occupyStyle;

        /// <summary>
        /// 属性Type的值
        /// </summary>
        private int type;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string name;

        /// <summary>
        /// 资源时间组织方式，0=按实际时间组织，1=按时间段组织，如1小时，半个班，1个班，1天等
        /// </summary>
        public int resTimeStyle = 0;

        /// <summary>
        /// 按时间段组织资源时间时，时间段大小，单位为小时
        /// </summary>
        public int resTimeGroup = 4;

        public bool isHasViceRes = false;

        public ScheduleResource viceRes = null;

        /// <summary>
        /// 无参数构造器
        /// </summary>
        public ScheduleResource()
        {
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="id" >资源编号</param>
        /// <remarks>初始化对象数据</remarks>
        /// <returns>无返回值</returns>
        public ScheduleResource(long id)
        {
            this.id = id;
            //iniData();
        }

        /// <summary>
        /// 取内存数据库数据赋给各属性
        /// </summary>
        public void iniData()
        {
            //name
            //holdingTimeList
            //taskList
        }

        /// <summary>
        /// 深度拷贝,暂时未实现
        /// </summary>
        /// <remarks>将实体中的所有数据拷贝到一个新的实体中</remarks>
        /// <returns>实体的一个副本</returns>
        public ScheduleResource deepClone()
        {
            ScheduleResource temp = new ScheduleResource();//临时加工单元实体


            return temp;
        }

        public ArrayList CompareIdleAndWorkTimeList(ArrayList aarr_idleAndWorkTimeList)
        {

            return null;
        }

        /// <summary>
        /// 从排程原始时间开始，资源已安排时间片段列表,元素为TimeRange对象
        /// </summary>
        public ArrayList holdingTimeList = new ArrayList(10);

        /// <summary>
        /// 从排程原始时间开始，资源空闲和工作时间片段列表,元素为TimeRange对象,每段占用时间为resTimeGroup
        /// </summary>
        public ArrayList idleAndWorkTimeList = new ArrayList(10);

        /// <summary>
        /// 资源已安排任务列表，元素为ScheduleTask对象
        /// </summary>
        public ArrayList taskList = new ArrayList(10);

        /// <summary>
        /// 临时占用时间列表，用于计算某时间后的资源安排时间片段，元素为TimeRange对象
        /// </summary>
        public ArrayList tempHoldingTimeList = new ArrayList(5);

        /// <summary>
        /// 临时连通空闲时间列表，为TimeRange集合
        /// </summary>
        public ArrayList tempIdleTimeList = new ArrayList(5);

        /// <summary>
        /// 临时单独空闲时间列表，为TimeRange集合
        /// </summary>
        public ArrayList tempIdleDetailTimeList = new ArrayList(5);

        ///// <summary>
        ///// 计算某时间后，资源的安排时间片段列表，结果存入tempHoldingTimeList中
        ///// </summary>
        ///// <param name="iniTime">初始时间，为距离排程原始时间秒数</param>
        //public void holdingTimeCleanUp(float iniTime)
        //{
        //    //ArrayList tempList = new ArrayList(5);

        //    //tempList.Clear();
        //    tempIdleDetailTimeList.Clear();
        //    tempHoldingTimeList.Clear();    //临时列表清空
        //    tempIdleTimeList.Clear();

        //    bool flag = false;   //判断是否确定初始时间所处的区间

        //    for (int i = 0; i < holdingTimeList.Count; i++)    //遍历已安排时间列表
        //    {
        //        if (!flag)   //如果未确定初始时间位置
        //        {
        //            if (((TimeRange)holdingTimeList[i]).Start <= iniTime && ((TimeRange)holdingTimeList[i]).End > iniTime)
        //            {
        //                TimeRange tr = new TimeRange();  //如果初始时间处于某时段之间，则将初始时间之后的时段压入TempHoldingTimeList中
        //                tr.Start = iniTime;
        //                tr.End = ((TimeRange)holdingTimeList[i]).End;
        //                tr.attribute = ((TimeRange)holdingTimeList[i]).attribute;
        //                tempHoldingTimeList.Add(tr);

        //                flag = true;
        //                continue;
        //            }

        //            if (((TimeRange)holdingTimeList[i]).Start > iniTime)  //如果初始时间处于某时段之前，则将此时段压入tempList中
        //            {
        //                TimeRange tr = (TimeRange)((TimeRange)holdingTimeList[i]).Clone();
        //                tempHoldingTimeList.Add(tr);

        //                flag = true;
        //            }
        //        }
        //        else   //否则，将holdingTimeList中的后续时段压入到tempList中
        //        {
        //            TimeRange tr = (TimeRange)((TimeRange)holdingTimeList[i]).Clone();

        //            tempHoldingTimeList.Add(tr);
        //        }
        //    }

        //    if (isHasViceRes)
        //    {
        //        viceRes.holdingTimeCleanUp(iniTime);
        //        for (int i = 0; i < viceRes.tempHoldingTimeList.Count; i++)
        //        {
        //            tempHoldingTimeList.Add(viceRes.tempHoldingTimeList[i]);//占用时段合并
        //        }

        //        tempHoldingTimeList.Sort();  //排序

        //        for (int i = 0; i < tempHoldingTimeList.Count - 1; )    //已安排时间融合
        //        {
        //            TimeRange tr1 = (TimeRange)tempHoldingTimeList[i];
        //            TimeRange tr2 = (TimeRange)tempHoldingTimeList[i + 1];

        //            if (tr1.End >= tr2.Start)
        //            {
        //                if (tr1.attribute == TimeRangeAttribute.Work)
        //                {
        //                    /*if (ts2.attribute == "占用")
        //                    {
        //                        ts2.start = ts1.start;
        //                        tempHoldingTimeList.RemoveAt(i);
        //                    }
        //                    else
        //                    {
        //                        ts2.start = ts1.end;
        //                        i++;
        //                    }*/
        //                    if (tr1.End < tr2.End)
        //                    {
        //                        tr1.End = tr2.End;
        //                    }
        //                    tempHoldingTimeList.RemoveAt(i + 1);

        //                }
        //                else
        //                {
        //                    if (tr2.attribute == TimeRangeAttribute.Work)
        //                    {
        //                        tr2.Start = tr1.Start;
        //                        if (tr2.End < tr1.End)
        //                        {
        //                            tr2.End = tr1.End;
        //                        }
        //                        tempHoldingTimeList.RemoveAt(i);
        //                    }
        //                    else
        //                    {
        //                        tr2.Start = tr1.Start;
        //                        if (tr2.End < tr1.End)
        //                        {
        //                            tr2.End = tr1.End;
        //                        }
        //                        tempHoldingTimeList.RemoveAt(i);
        //                    }


        //                }
        //            }
        //            else
        //            {
        //                i++;
        //            }
        //        }


        //    }

        //    for (int i = 0; i < tempHoldingTimeList.Count; i++)
        //    {
        //        if (((TimeRange)tempHoldingTimeList[i]).HoldingTime < 0)
        //        {
        //            throw new Exception("holdtimeclean is fault");
        //        }
        //    }


        //}

        //public void check()
        //{
        //    for (int i = 0; i < holdingTimeList.Count; i++)
        //    {
        //        if (((TimeRange)holdingTimeList[i]).HoldingTime < 0)
        //        {
        //            throw new Exception("insertTask is fault");
        //        }
        //    }
        //}

        ///// <summary>
        ///// 将安排好的任务插入到资源任务列表TaskList及已安排时间列表holdingTimeList中
        ///// </summary>
        ///// <param name="ts">待插入的任务</param>
        //public void insertTask(ScheduleTask st)
        //{

        //    taskList.Add(st);
        //    taskList.Sort();

        //    for (int i = 0; i < st.planProcessingTimeList.Count; i++)
        //    {
        //        holdingTimeList.Add(st.planProcessingTimeList[i]);
        //    }

        //    holdingTimeList.Sort();

            
        //    /*int pos = 0;//插入位置
        //    for (int i = 0; i < ts.actualProcessingTimeList.Count; i++)   //遍历任务加工时段
        //    {
        //        bool insertFlag = false;    //确定是否找到插入位置

        //        for (int j = pos; j < holdingTimeList.Count; j++)   //遍历资源已安排时段列表
        //        {
        //            if (((TimeRange)ts.actualProcessingTimeList[i]).End <= ((TimeRange)holdingTimeList[j]).Start)
        //            {
        //                holdingTimeList.Insert(j, ts.actualProcessingTimeList [i]);
                        
        //                insertFlag = true;
        //                pos = j + 1;
                        
        //                break;
        //            }
        //        }

        //        if (!insertFlag)  //插入不了则在后面附加
        //        {
        //            holdingTimeList.Add(ts.actualProcessingTimeList [i]);
        //            pos = holdingTimeList.Count;
        //        }
        //    }

        //    if (taskList.Count < 1)   //如果资源未安排任务，则直接添加
        //    {
        //        taskList.Add(ts);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < taskList.Count; i++)    //遍历任务列表
        //        {
        //            if (ts.actualStartTime < ((ScheduleTask)taskList[i]).actualStartTime)
        //            {
        //                taskList.Insert(i, ts);   //播入后返回
        //                return;
        //            }
        //        }

        //        taskList.Add(ts);    //遍历无法插入则附加
        //    }*/

        //    if (isHasViceRes)
        //    {
        //        viceRes.insertTask(st);
        //    }

        //}

        ///// <summary>
        ///// 为排程任务安排加工时间
        ///// </summary>
        ///// <param name="st">要排程的任务</param>
        ///// <param name="scheduleStyle">排程方式</param>
        ///// <returns >任务完成时间</returns>
        //public float addScheduleTask(ref ScheduleTask st, int scheduleStyle)
        //{
        //    //int endTime = 0;
        //    st.clear();
        //    //if (st.preTaskList.Count > 0)
        //    //{
        //   //     st.earliestStartTime = 0;
        //    //}
        //    st.onepeiceProcessTime = getUnitProcessTimeofTask(st.ID);//得到单件加工时间

        //    isHasViceRes = getViceRes(st);
            
        //    for (int i = 0; i < st.preTaskList.Count; i++)       //计算任务允许最早开工时间
        //    {
        //         ScheduleTask tempST = (ScheduleTask)st.preTaskList[i];

        //         //int planET = 0;
        //         float planST = 0;
        //         planST = tempST.planEndTime;
        //        //planST = tempST.planStartTime + tempST.postTaskDelayTime;
        //            /*if (tempST.onepeiceProcessTime > st.onepeiceProcessTime)
        //            {
        //                int latestBatch = 0;
        //                if (tempST.batch == tempST.transportBatch)
        //                {
        //                    latestBatch = tempST.batch;
        //                }
        //                else
        //                {
        //                    latestBatch = tempST.batch % tempST.transportBatch;
        //                }

        //                if (latestBatch == 0)
        //                {
        //                    planET = tempST.planEndTime + tempST.transportTime + tempST.transportBatch * st.onepeiceProcessTime;
        //                }
        //                else
        //                {
        //                    planET = tempST.planEndTime+tempST .onepeiceProcessTime *(tempST .transportBatch - latestBatch)  + tempST.transportTime + latestBatch * st.onepeiceProcessTime;
        //                }
                    
        //                planST = planET - st.batch * st.onepeiceProcessTime;


        //            }
        //            else
        //            {
        //                planST = tempST.planStartTime + tempST.onepeiceProcessTime * tempST.transportBatch + tempST.transportTime;
                    
        //            }*/

        //            if (planST > st.earliestStartTime)
        //            {
        //                st.earliestStartTime = planST;
        //            }

        //    }
            

        //    if (resTimeStyle == 0)//精确排程
        //    {

        //        if (scheduleStyle == 0)  //附加排程
        //        {
        //            float iniTime = 0;
        //            float resourceEarliestTime = 0;      //资源允许最早时间

        //            //ScheduleTask tempST = (ScheduleTask)taskList[taskList.Count - 1];

        //            if (taskList.Count > 0)
        //            {
        //                //st.preProcessingTime = getPreProcessTimeofTask(((ScheduleTask)taskList[taskList.Count - 1]).ID, st.ID);
        //                resourceEarliestTime = ((ScheduleTask)taskList[taskList.Count - 1]).planEndTime /*+ tempST.postProcessingTime*/ + st.preProcessingTime;
        //            }
        //            else
        //            {
        //                //st.preProcessingTime = getPreProcessTimeofTask(0, st.ID);
        //                //resourceEarliestTime = st.preProcessingTime;

        //                resourceEarliestTime = 0;
        //            }

        //            if (isHasViceRes)
        //            {
        //                if (viceRes.taskList.Count > 0)
        //                {
        //                    if (resourceEarliestTime < ((ScheduleTask)viceRes.taskList[viceRes.taskList.Count - 1]).planEndTime)
        //                    {
        //                        resourceEarliestTime = ((ScheduleTask)viceRes.taskList[viceRes.taskList.Count - 1]).planEndTime;
        //                    }
        //                }
        //            }


        //            if (st.earliestStartTime > resourceEarliestTime)
        //            {
        //                //iniTime = st.earliestStartTime-st.preProcessingTime ;

        //                iniTime = st.earliestStartTime;
        //            }
        //            else
        //            {
        //                //iniTime = resourceEarliestTime-st.preProcessingTime ;
        //                iniTime = resourceEarliestTime;
        //            }

        //            holdingTimeCleanUp(iniTime);       //确定任务初始时间，清理资源占用时段

        //            if (tempHoldingTimeList.Count < 1)
        //            {
        //                TimeRange tr = new TimeRange();
        //                tr.attribute = TimeRangeAttribute.Work;
        //                tr.Start = iniTime;// +st.preProcessingTime;
        //                tr.End = tr.Start + st.ProcessingTime;

        //                st.planProcessingTimeList.Add(tr);

        //                //endTime = tr.End;

        //                return tr.End;
        //            }
        //            else
        //            {
        //                if (tempHoldingTimeList.Count == 1)
        //                {
        //                    TimeRange tempTR = (TimeRange)tempHoldingTimeList[0];

        //                    if (tempTR.Start - iniTime >= /*st.preProcessingTime*/+st.ProcessingTime)
        //                    {
        //                        TimeRange tr = new TimeRange();
        //                        tr.attribute = TimeRangeAttribute.Work;
        //                        tr.Start = iniTime;// +st.preProcessingTime;
        //                        tr.End = tr.Start + st.ProcessingTime;

        //                        st.planProcessingTimeList.Add(tr);

        //                        return tr.End;
        //                        //endTime = tr.End;
        //                    }
        //                    else
        //                    {
        //                        if (tempTR.Start > iniTime)
        //                        {
        //                            TimeRange tr1 = new TimeRange();
        //                            TimeRange tr2 = new TimeRange();

        //                            tr1.attribute = TimeRangeAttribute.Work;
        //                            tr1.Start = iniTime;
        //                            tr1.End = tempTR.Start;
        //                            st.planProcessingTimeList.Add(tr1);

        //                            tr2.attribute = TimeRangeAttribute.Work;
        //                            tr2.Start = tempTR.End;
        //                            tr2.End = tr2.Start + st.ProcessingTime + iniTime - tempTR.Start;

        //                            st.planProcessingTimeList.Add(tr2);

        //                            return tr2.End;
        //                        }
        //                        else
        //                        {
        //                            TimeRange tr = new TimeRange();

        //                            tr.Start = tempTR.End;
        //                            tr.End = tr.Start + st.ProcessingTime;
        //                            tr.attribute = TimeRangeAttribute.Work;

        //                            st.planProcessingTimeList.Add(tr);

        //                            return tr.End;

        //                        }
        //                        //endTime = tr2.End;
        //                        /*if (tempTR.Start - iniTime > st.preProcessingTime)
        //                        {
        //                            TimeRange tr1 = new TimeRange();
        //                            tr1.Start = iniTime + st.preProcessingTime;
        //                            tr1.End = tempTR.Start;
        //                            tr1.attribute = TimeRangeAttribute.Work;
        //                            st.planProcessingTimeList.Add(tr1);

        //                            TimeRange tr2 = new TimeRange();
        //                            tr2.Start = tempTR.End;
        //                            tr2.End = st.ProcessingTime - tr1.HoldingTime;
        //                            tr2.attribute = TimeRangeAttribute.Work;
        //                            st.planProcessingTimeList.Add(tr2);

        //                            endTime = tr2.End;

        //                        }
        //                        else
        //                        {
        //                            if (tempTR.Start - iniTime == st.preProcessingTime)
        //                            {
        //                                TimeRange tr = new TimeRange();
        //                                tr.Start = tempTR.End;
        //                                tr.End = tr.Start + st.ProcessingTime;
        //                                tr.attribute = TimeRangeAttribute.Work;
        //                                st.planProcessingTimeList.Add(tr);

        //                                endTime = tr.End;
        //                            }
        //                            else
        //                            {
        //                                TimeRange tr = new TimeRange();
        //                                tr.Start = tempTR.End + st.preProcessingTime + iniTime - tempTR.Start;
        //                                tr.End = tr.Start + st.ProcessingTime;
        //                                tr.attribute = TimeRangeAttribute.Work;
        //                                st.planProcessingTimeList.Add (tr);

        //                                endTime = tr.End;
        //                            }
        //                        }*/
        //                    }
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < tempHoldingTimeList.Count; i++)
        //                    {
        //                        //int time1 = st.preProcessingTime;
        //                        float time = st.ProcessingTime;

        //                        float idleTime = 0;

        //                        if (i == 0)
        //                        {
        //                            idleTime = ((TimeRange)tempHoldingTimeList[0]).Start - iniTime;
        //                        }
        //                        else
        //                        {
        //                            idleTime = ((TimeRange)tempHoldingTimeList[i]).Start - ((TimeRange)tempHoldingTimeList[i - 1]).End;
        //                        }


        //                        if (idleTime >= time)
        //                        {
        //                            TimeRange tr = new TimeRange();
        //                            tr.attribute = TimeRangeAttribute.Work;

        //                            if (i == 0)
        //                            {
        //                                tr.Start = iniTime;
        //                            }
        //                            else
        //                            {
        //                                tr.Start = ((TimeRange)tempHoldingTimeList[i - 1]).End;
        //                            }

        //                            tr.End = tr.Start + time;

        //                            st.planProcessingTimeList.Add(tr);

        //                            return tr.End;
        //                            //endTime = tr.End;
        //                        }
        //                        else
        //                        {
        //                            if (idleTime > 0)
        //                            {
        //                                TimeRange tr = new TimeRange();
        //                                tr.attribute = TimeRangeAttribute.Work;

        //                                if (i == 0)
        //                                {
        //                                    tr.Start = iniTime;
        //                                }
        //                                else
        //                                {
        //                                    tr.Start = ((TimeRange)tempHoldingTimeList[i - 1]).End;
        //                                }

        //                                tr.End = tr.Start + idleTime;


        //                                st.planProcessingTimeList.Add(tr);

        //                                time -= idleTime;
        //                            }
        //                            else
        //                            {
        //                                continue;
        //                            }

        //                        }
        //                        /*if (idleTime >= time1 + time2)
        //                        {
        //                            TimeRange tr = new TimeRange();
        //                            tr.attribute = TimeRangeAttribute.Work;

        //                            if (i == 0)
        //                            {
        //                                tr.Start = iniTime + st.preProcessingTime;
        //                                tr.End = tr.Start + st.ProcessingTime;
        //                            }
        //                            else
        //                            {
        //                                tr.Start = ((TimeRange)tempHoldingTimeList[i - 1]).End + time1;
        //                                tr.End = tr.Start + time2;
        //                            }
        //                            st.planProcessingTimeList.Add(tr);

        //                            endTime = tr.End;
        //                        }
        //                        else
        //                        {
        //                            if (idleTime > time1)
        //                            {
        //                                TimeRange tr = new TimeRange();
        //                                tr.attribute = TimeRangeAttribute.Work;

        //                                if (i == 0)
        //                                {
        //                                    tr.Start = iniTime + time1;
        //                                    tr.End = iniTime + idleTime;
        //                                }
        //                                else
        //                                {
        //                                    tr.Start = ((TimeRange)tempHoldingTimeList[i - 1]).End + time1;
        //                                    tr.End = ((TimeRange)tempHoldingTimeList[i]).Start;
        //                                }

        //                                time1 = 0;
        //                                time2 -= tr.HoldingTime;

        //                            }
        //                            else
        //                            {
        //                                if (idleTime == time1)
        //                                {
        //                                    time1 = 0;
        //                                }
        //                                else
        //                                {
        //                                    time1 -= idleTime;
        //                                }
        //                            }
        //                        }*/
        //                    }
        //                }
        //            }
        //        }
        //        else           //插入排程
        //        {
        //            float iniTime = st.earliestStartTime;

        //            holdingTimeCleanUp(iniTime);

        //            //计算单独空闲时间
        //            for (int i = 0; i < tempHoldingTimeList.Count; i++)
        //            {
        //                if (i == 0)
        //                {
        //                    TimeRange temptr = (TimeRange)tempHoldingTimeList[0];
        //                    if (iniTime < temptr.Start)
        //                    {
        //                        TimeRange trnew = new TimeRange();
        //                        trnew.Start = iniTime;
        //                        trnew.End = temptr.Start;
        //                        trnew.attribute = TimeRangeAttribute.Idle;
        //                        tempIdleDetailTimeList.Add(trnew);
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        continue;
        //                    }
        //                }

        //                TimeRange temptr1 = (TimeRange)tempHoldingTimeList[i - 1];
        //                TimeRange temptr2 = (TimeRange)tempHoldingTimeList[i];

        //                if (temptr2.Start > temptr1.End)
        //                {
        //                    TimeRange trnew1 = new TimeRange();
        //                    trnew1.Start = temptr1.End;
        //                    trnew1.End = temptr2.Start;
        //                    trnew1.attribute = TimeRangeAttribute.Idle;
        //                    tempIdleDetailTimeList.Add(trnew1);
        //                }

        //            }

        //            if (tempHoldingTimeList.Count > 0)
        //            {
        //                TimeRange tr = new TimeRange();
        //                tr.Start = ((TimeRange)tempHoldingTimeList[tempHoldingTimeList.Count - 1]).End;
        //                tr.End = Scheduling.maxTimeValue;
        //                tr.attribute = TimeRangeAttribute.Idle;
        //                tempIdleDetailTimeList.Add(tr);
        //            }

        //            //计算连通空闲时间
        //            if (tempHoldingTimeList.Count == 0)  //如果当前资源无已安排时段，直接安排任务
        //            {
        //                TimeRange tr = new TimeRange();

        //                /*if (taskList.Count > 0)
        //                {
        //                    st.preProcessingTime = getPreProcessTimeofTask(((ScheduleTask)taskList[taskList.Count - 1]).ID, st.ID);

        //                    if (((ScheduleTask)taskList[taskList.Count - 1]).planEndTime + st.preProcessingTime > iniTime)
        //                    {
        //                        tr.Start = ((ScheduleTask)taskList[taskList.Count - 1]).planEndTime + st.preProcessingTime;
        //                    }
        //                    else
        //                    {
        //                        tr.Start = iniTime;
        //                    }
        //                }
        //                else
        //                {
        //                    st.preProcessingTime = getPreProcessTimeofTask(0, st.ID);
        //                    if (st.preProcessingTime > iniTime)
        //                    {
        //                        tr.Start = st.preProcessingTime;
        //                    }
        //                    else
        //                    {
        //                        tr.Start = iniTime;
        //                    }
        //                }*/

        //                tr.Start = iniTime;
        //                tr.End = tr.Start + st.ProcessingTime;
        //                tr.attribute = TimeRangeAttribute.Work;

        //                st.planProcessingTimeList.Add(tr);
        //                return tr.End;
        //            }
        //            else
        //            {
        //                if (tempHoldingTimeList.Count == 1)
        //                 {
        //                    if (((TimeRange)tempHoldingTimeList[0]).attribute != TimeRangeAttribute.Work)   //如果资源组仅有一个非“占用”已安排时段，直接安排任务
        //                    {
        //                        if (((TimeRange)tempHoldingTimeList[0]).Start - iniTime >= st.ProcessingTime)
        //                        {
        //                            TimeRange tr = new TimeRange();

        //                            tr.Start = iniTime;
        //                            tr.End = tr.Start + st.ProcessingTime;
        //                            tr.attribute = TimeRangeAttribute.Work;

        //                            st.planProcessingTimeList.Add(tr);

        //                            return tr.End;
        //                        }
        //                        else
        //                        {
        //                            if (((TimeRange)tempHoldingTimeList[0]).Start > iniTime)
        //                            {
        //                                TimeRange tr1 = new TimeRange();
        //                                TimeRange tr2 = new TimeRange();

        //                                tr1.Start = iniTime;
        //                                tr1.End = ((TimeRange)tempHoldingTimeList[0]).Start;
        //                                tr1.attribute = TimeRangeAttribute.Work;

        //                                st.planProcessingTimeList.Add(tr1);

        //                                tr2.Start = ((TimeRange)tempHoldingTimeList[0]).End;
        //                                tr2.End = tr2.Start + st.ProcessingTime - tr1.HoldingTime;
        //                                tr2.attribute = TimeRangeAttribute.Work;

        //                                st.planProcessingTimeList.Add(tr2);

        //                                return tr2.End;
        //                            }
        //                            else
        //                            {
        //                                TimeRange tr = new TimeRange();

        //                                tr.Start = ((TimeRange)tempHoldingTimeList[0]).End;
        //                                tr.End = tr.Start + st.ProcessingTime;
        //                                tr.attribute = TimeRangeAttribute.Work;

        //                                st.planProcessingTimeList.Add(tr);

        //                                return tr.End;
        //                            }
        //                        }
        //                        /*if (iniTime + rg.oneProcessTime * tsk.batch + tsk.readyTime > ((Mytimespan)tempHoldingTimeList[0]).start)
                             
        //                            Mytimespan ts1 = new Mytimespan();
        //                            Mytimespan ts2 = new Mytimespan();

        //                            if (iniTime != ((Mytimespan)tempHoldingTimeList[0]).start)
        //                            {
        //                                ts1.start = iniTime;
        //                                ts1.end = ((Mytimespan)tempHoldingTimeList[0]).start;
        //                                ts1.attribute = "占用";
        //                                tempProcessTimeList.Add(ts1);
        //                            }

        //                            ts2.start = ((Mytimespan)tempHoldingTimeList[0]).end;
        //                            ts2.end = iniTime + rg.oneProcessTime * tsk.batch + tsk.readyTime + ((Mytimespan)tempHoldingTimeList[0]).holdingtime;
        //                            ts2.attribute = "占用";


        //                            tempProcessTimeList.Add(ts2);
        //                            return ts2.end;
        //                        }
        //                        else
        //                        {
        //                            Mytimespan ts = new Mytimespan();
        //                            ts.start = iniTime;
        //                            ts.end = iniTime + rg.oneProcessTime * tsk.batch + tsk.readyTime;
        //                            ts.attribute = "占用";
        //                            tempProcessTimeList.Add(ts);
        //                            return ts.end;
        //                        }*/
        //                    }
        //                    else           //否则计算连通时间
        //                    {
        //                        if (iniTime >= ((TimeRange)tempHoldingTimeList[0]).Start)
        //                        {
        //                            TimeRange tr = new TimeRange();
        //                            tr.Start = ((TimeRange)tempHoldingTimeList[0]).End;
        //                            tr.End = tr.Start + st.ProcessingTime;
        //                            tr.attribute = TimeRangeAttribute.Work;

        //                            st.planProcessingTimeList.Add(tr);
        //                            return tr.End;
        //                        }
        //                        else
        //                        {
        //                            if (((TimeRange)tempHoldingTimeList[0]).Start - iniTime >= st.ProcessingTime)
        //                            {
        //                                TimeRange tr = new TimeRange();

        //                                tr.Start = iniTime;
        //                                tr.End = tr.Start + st.ProcessingTime;
        //                                tr.attribute = TimeRangeAttribute.Work;

        //                                st.planProcessingTimeList.Add(tr);

        //                                return tr.End;
        //                            }
        //                            else
        //                            {
        //                                TimeRange tr = new TimeRange();
        //                                tr.Start = ((TimeRange)tempHoldingTimeList[0]).End;
        //                                tr.End = tr.Start + st.ProcessingTime;
        //                                tr.attribute = TimeRangeAttribute.Work;

        //                                st.planProcessingTimeList.Add(tr);
        //                                return tr.End;
        //                            }
        //                        }
        //                    }
        //                }
        //                else     //遍历计算连通时间
        //                {


        //                    ArrayList al = new ArrayList(5);   //占用时段位置列表

        //                    for (int i = 0; i < tempHoldingTimeList.Count; i++)
        //                    {

        //                        if (((TimeRange)tempHoldingTimeList[i]).attribute == TimeRangeAttribute.Work)
        //                        {
        //                            al.Add(i);
        //                        }
        //                    }

        //                    if (al.Count < 1)     //如果没用占用时段，直接按排
        //                    {
        //                        float time = st.ProcessingTime;

        //                        for (int i = 0; i < tempHoldingTimeList.Count; i++)
        //                        {
        //                            //int time1 = st.preProcessingTime;
                                    

        //                            float idleTime = 0;

        //                            if (i == 0)
        //                            {
        //                                idleTime = ((TimeRange)tempHoldingTimeList[0]).Start - iniTime;
        //                            }
        //                            else
        //                            {
        //                                idleTime = ((TimeRange)tempHoldingTimeList[i]).Start - ((TimeRange)tempHoldingTimeList[i - 1]).End;
        //                            }


        //                            if (idleTime >= time)
        //                            {
        //                                TimeRange tr = new TimeRange();
        //                                tr.attribute = TimeRangeAttribute.Work;

        //                                if (i == 0)
        //                                {
        //                                    tr.Start = iniTime;
        //                                }
        //                                else
        //                                {
        //                                    tr.Start = ((TimeRange)tempHoldingTimeList[i - 1]).End;
        //                                }

        //                                tr.End = tr.Start + time;

        //                                st.planProcessingTimeList.Add(tr);

        //                                return tr.End;
        //                                //endTime = tr.End;
        //                            }
        //                            else
        //                            {
        //                                if (idleTime > 0)
        //                                {
        //                                    TimeRange tr = new TimeRange();
        //                                    tr.attribute = TimeRangeAttribute.Work;

        //                                    if (i == 0)
        //                                    {
        //                                        tr.Start = iniTime;
        //                                    }
        //                                    else
        //                                    {
        //                                        tr.Start = ((TimeRange)tempHoldingTimeList[i - 1]).End;
        //                                    }

        //                                    tr.End = tr.Start + idleTime;


        //                                    st.planProcessingTimeList.Add(tr);

        //                                    time -= idleTime;
        //                                }
        //                                else
        //                                {
        //                                    continue;
        //                                }

        //                            }
        //                        }
        //                    }

        //                    else   //计算连通时间
        //                    {
        //                        for (int i = 0; i < al.Count; i++)
        //                        {
        //                            int tempint2 = (int)al[i];   //后一占用时段
        //                            float idle = 0;        //两占用时段之间的空闲时间值

        //                            int tempint1;      //前一占用时段

        //                            if (i == 0)
        //                            {
        //                                idle = ((TimeRange)tempHoldingTimeList[tempint2]).Start - iniTime;

        //                                if (tempint2 > 0)
        //                                {
        //                                    for (int j = 0; j < tempint2; j++)
        //                                    {
        //                                        idle = idle - ((TimeRange)tempHoldingTimeList[j]).HoldingTime;
        //                                    }
        //                                }

        //                                if (idle > 0)
        //                                {
        //                                    TimeRange tr = new TimeRange();
        //                                    tr.Start = iniTime;
        //                                    tr.HoldingTime = idle;
        //                                    tr.End = ((TimeRange)tempHoldingTimeList[tempint2]).Start;
        //                                    tr.attribute = TimeRangeAttribute.Idle;
        //                                    tempIdleTimeList.Add(tr);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                tempint1 = (int)al[i - 1];

        //                                idle = ((TimeRange)tempHoldingTimeList[tempint2]).Start - ((TimeRange)tempHoldingTimeList[tempint1]).End;

        //                                if (tempint2 - tempint1 > 1)
        //                                {
        //                                    for (int j = tempint1 + 1; j < tempint2; j++)
        //                                    {
        //                                        idle = idle - ((TimeRange)tempHoldingTimeList[j]).HoldingTime;
        //                                    }
        //                                }

        //                                if (idle > 0)
        //                                {
        //                                    TimeRange tr = new TimeRange();
        //                                    tr.Start = ((TimeRange)tempHoldingTimeList[tempint1]).End;
        //                                    tr.End = ((TimeRange)tempHoldingTimeList[tempint2]).Start;
        //                                    tr.HoldingTime = idle;
        //                                    tr.attribute = TimeRangeAttribute.Idle;
        //                                    tempIdleTimeList.Add(tr);
        //                                }
        //                            }



        //                        }

        //                        TimeRange trnew = new TimeRange();

        //                        int position = (int)al[al.Count - 1];
                                
        //                        if (position < tempHoldingTimeList.Count-1)
        //                        {
        //                            if (((TimeRange)tempHoldingTimeList[position + 1]).Start > ((TimeRange)tempHoldingTimeList[position]).End)
        //                            {
        //                                trnew.Start = ((TimeRange)tempHoldingTimeList[position]).End;
        //                            }
        //                            else
        //                            {
        //                                trnew.Start = ((TimeRange)tempHoldingTimeList[position + 1]).End;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            trnew.Start = ((TimeRange)tempHoldingTimeList[position]).End;
        //                        }
                                
        //                        trnew.End = Scheduling.maxTimeValue;
        //                        trnew.attribute = TimeRangeAttribute.Idle;
        //                        tempIdleTimeList.Add(trnew);
        //                    }

        //                }

        //                for (int i = 0; i < tempIdleTimeList.Count; i++)   //根据连通空闲时间确定任务的加工时间
        //                {
        //                    TimeRange tr = (TimeRange)tempIdleTimeList[i];   //取连通空闲时段

        //                    if (tr.HoldingTime < st.ProcessingTime)
        //                    {
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        bool flag = false;
        //                        float tempTime = st.ProcessingTime;

        //                        for (int j = 0; j < tempIdleDetailTimeList.Count; j++)
        //                        {
        //                            TimeRange tr1 = (TimeRange)tempIdleDetailTimeList[j];
        //                            if (!flag)
        //                            {
        //                                if (Math.Abs ( tr.Start - tr1.Start) < 0.001)
        //                                {
        //                                    if (((TimeRange)tempIdleDetailTimeList[j]).HoldingTime >= tempTime)
        //                                    {
        //                                        TimeRange trnew = new TimeRange();

        //                                        trnew.Start = ((TimeRange)tempIdleDetailTimeList[j]).Start;
        //                                        trnew.End = trnew.Start + tempTime;
        //                                        trnew.attribute = TimeRangeAttribute.Work;

        //                                        st.planProcessingTimeList.Add(trnew);

        //                                        return trnew.End;
        //                                    }
        //                                    else
        //                                    {
        //                                        st.planProcessingTimeList.Add(tempIdleDetailTimeList[j]);
        //                                        flag = true;
        //                                        tempTime -= ((TimeRange)tempIdleDetailTimeList[j]).HoldingTime;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    continue;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (((TimeRange)tempIdleDetailTimeList[j]).HoldingTime >= tempTime)
        //                                {
        //                                    TimeRange trnew = new TimeRange();

        //                                    trnew.Start = ((TimeRange)tempIdleDetailTimeList[j]).Start;
        //                                    trnew.End = trnew.Start + tempTime;
        //                                    trnew.attribute = TimeRangeAttribute.Work;

        //                                    st.planProcessingTimeList.Add(trnew);

        //                                    return trnew.End;
        //                                }
        //                                else
        //                                {
        //                                    st.planProcessingTimeList.Add(tempIdleDetailTimeList[j]);
        //                                    tempTime -= ((TimeRange)tempIdleDetailTimeList[j]).HoldingTime;
        //                                }

        //                            }
        //                        }
        //                        /*int spaceTime = onoffTime(rg, tsk, ts);   //计算任务前后是否需要换模、换料、换tr
        //                        //int outTime = 0;                          //如果最早开工时间位于两空闲时间段之间，下一空闲段需空出的时间
        //                        if (spaceTime > -1)       //如果可以安排，确定任务的详细加工时间段
        //                        {
        //                            bool flag = false;   //确定是否找到了返回值所处的空闲时段

        //                            int needTime = rg.oneProcessTime * tsk.batch + tsk.readyTime;   //任务所需时间

        //                            for (int j = 0; j < tempIdleTimeDetailList.Count; j++)
        //                            {
        //                                if (!flag)
        //                                {
        //                                    Mytimespan tsnew = (Mytimespan)tempIdleTimeDetailList[j];//取资源组详细空闲时段

        //                                    if (ts.start == tsnew.start)    //判断返回值所处的空闲时段
        //                                    {
        //                                        flag = true;

        //                                        if (spaceTime + needTime <= tsnew.holdingtime)
        //                                        {
        //                                            Mytimespan tenew1 = new Mytimespan();

        //                                            tenew1.start = spaceTime + ts.start;
        //                                            tenew1.end = spaceTime + ts.start + needTime;
        //                                            tenew1.attribute = "占用";

        //                                            tempProcessTimeList.Add(tenew1);

        //                                            return tenew1.end;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (spaceTime < tsnew.holdingtime)
        //                                            {
        //                                                Mytimespan tenew2 = new Mytimespan();

        //                                                tenew2.start = ts.start + spaceTime;
        //                                                tenew2.end = tsnew.end;
        //                                                tenew2.attribute = "占用";

        //                                                tempProcessTimeList.Add(tenew2);

        //                                                needTime = needTime - tenew2.holdingtime;      //安排一个空闲时间后，任务占用时间相应减少

        //                                                spaceTime = 0;
        //                                            }
        //                                            else
        //                                            {
        //                                                spaceTime = spaceTime - tsnew.holdingtime;
        //                                            }
        //                                        }

        //                                    }
        //                                }

        //                                else
        //                                {
        //                                    if (needTime + spaceTime <= ((Mytimespan)tempIdleTimeDetailList[j]).holdingtime)
        //                                    {
        //                                        Mytimespan tenew = new Mytimespan();

        //                                        tenew.start = ((Mytimespan)tempIdleTimeDetailList[j]).start + spaceTime;
        //                                        tenew.end = tenew.start + needTime;
        //                                        tenew.attribute = "占用";

        //                                        tempProcessTimeList.Add(tenew);

        //                                        return tenew.end;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (spaceTime < ((Mytimespan)tempIdleTimeDetailList[j]).holdingtime)
        //                                        {
        //                                            Mytimespan tenew = new Mytimespan();
        //                                            tenew.start = ((Mytimespan)tempIdleTimeDetailList[j]).start + spaceTime;
        //                                            tenew.end = ((Mytimespan)tempIdleTimeDetailList[j]).end;
        //                                            tenew.attribute = "占用";

        //                                            tempProcessTimeList.Add(tenew);

        //                                            needTime = needTime - tenew.holdingtime;

        //                                            spaceTime = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            spaceTime = spaceTime - ((Mytimespan)tempIdleTimeDetailList[j]).holdingtime;
        //                                        }

        //                                    }
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            continue;
        //                        }*/
        //                    }
        //                }


        //            }
        //        }
        //    }

        //    else   //分段排程
        //    {
        //        if (scheduleStyle == 0)//附加式
        //        {
        //            if (taskList.Count > 0)
        //            {
        //                if (st.earliestStartTime <= ((ScheduleTask)taskList[taskList.Count - 1]).planEndTime)
        //                {
        //                    st.earliestStartTime = ((ScheduleTask)taskList[taskList.Count - 1]).planEndTime;
        //                }
        //            }

        //            if (isHasViceRes)
        //            {
        //                if (viceRes.taskList.Count > 0)
        //                {
        //                    if (st.earliestStartTime < ((ScheduleTask)viceRes.taskList[viceRes.taskList.Count - 1]).planEndTime)
        //                    {
        //                        st.earliestStartTime = ((ScheduleTask)viceRes.taskList[viceRes.taskList.Count - 1]).planEndTime;
        //                    }
        //                }
        //            }


        //            ArrayList tempIdleAndWorkTimeList = new ArrayList(5);


        //            if (isHasViceRes)
        //            {

        //            }
        //            else
        //            {
        //                tempIdleAndWorkTimeList = idleAndWorkTimeList;
        //            }


        //            for (int i = 0; i < idleAndWorkTimeList.Count; i++)
        //            {
        //                TimeRange tr = (TimeRange)idleAndWorkTimeList[i];

        //                if (tr.Start >= st.earliestStartTime)
        //                {
        //                    int n = 0;
        //                    if (st.ProcessingTime % resTimeGroup != 0)
        //                    {
        //                        n = (int)(st.ProcessingTime / resTimeGroup) + 1;
        //                    }
        //                    else
        //                    {
        //                        n = (int)st.ProcessingTime / resTimeGroup;
        //                    }

        //                    if (n > 1)
        //                    {
        //                        TimeRange tempTr = tr;

        //                        for (int j = 1; j < n; j++)
        //                        {
        //                            if (tempTr.End == ((TimeRange)idleAndWorkTimeList[i + j]).Start)
        //                            {
        //                                tempTr.HoldingTime += ((TimeRange)idleAndWorkTimeList[i + j]).HoldingTime;
        //                                tempTr.End = ((TimeRange)idleAndWorkTimeList[i + j]).End;
        //                            }
        //                            else
        //                            {
        //                                TimeRange addTr = new TimeRange();
        //                                addTr = tempTr;
        //                                addTr.attribute = TimeRangeAttribute.Work;
        //                                st.planProcessingTimeList.Add(addTr);
        //                                tempTr = (TimeRange)idleAndWorkTimeList[i + j];
        //                            }

        //                        }

        //                        tempTr.attribute = TimeRangeAttribute.Work;
        //                        st.planProcessingTimeList.Add(tempTr);

        //                        return tempTr.End;
        //                    }
        //                    else
        //                    {
        //                        TimeRange tempTr = tr;
        //                        tempTr.attribute = TimeRangeAttribute.Work;
        //                        st.planProcessingTimeList.Add(tempTr);
        //                        return tr.End;
        //                    }
        //                }

        //            }

        //        }
        //        else   //插入式
        //        {
        //            int pos = 0;
        //            for (int i = 0; i < idleAndWorkTimeList.Count; i++)
        //            {
        //                if (st.earliestStartTime <= ((TimeRange)idleAndWorkTimeList[i]).Start)
        //                {
        //                    pos = i;
        //                    break;
        //                }
        //            }

        //            int n = 0;
        //            if (st.ProcessingTime % resTimeGroup != 0)
        //            {
        //                n = (int)(st.ProcessingTime / resTimeGroup) + 1;
        //            }
        //            else
        //            {
        //                n = (int)st.ProcessingTime / resTimeGroup;
        //            }

        //            ArrayList workPos = new ArrayList(5);

        //            for (int i = pos; i < idleAndWorkTimeList.Count; i++)
        //            {
        //                if (((TimeRange)idleAndWorkTimeList[i]).attribute == TimeRangeAttribute.Work)
        //                {
        //                    workPos.Add(i);
        //                }
        //            }

        //            if (workPos.Count > 0)
        //            {
        //                for (int i = 0; i < workPos.Count; i++)
        //                {
        //                    int pos1 = pos;
        //                    int k = (int)workPos[i];

        //                    if (k - pos1 >= n)
        //                    {
        //                        TimeRange tr = (TimeRange)idleAndWorkTimeList[pos1];
        //                        if (n > 1)
        //                        {
        //                            TimeRange tempTr = tr;

        //                            for (int j = 1; j < n; j++)
        //                            {
        //                                if (tempTr.End == ((TimeRange)idleAndWorkTimeList[pos1 + j]).Start)
        //                                {
        //                                    tempTr.HoldingTime += ((TimeRange)idleAndWorkTimeList[pos1 + j]).HoldingTime;

        //                                    tempTr.End = ((TimeRange)idleAndWorkTimeList[pos1 + j]).End;
        //                                }
        //                                else
        //                                {
        //                                    TimeRange addTr = new TimeRange();
        //                                    addTr = tempTr;
        //                                    addTr.attribute = TimeRangeAttribute.Work;
        //                                    st.planProcessingTimeList.Add(addTr);
        //                                    tempTr = (TimeRange)idleAndWorkTimeList[pos1 + j];
        //                                }

        //                            }

        //                            tempTr.attribute = TimeRangeAttribute.Work;
        //                            st.planProcessingTimeList.Add(tempTr);

        //                            return tempTr.End;
        //                        }
        //                        else
        //                        {
        //                            TimeRange tempTr = tr;
        //                            tempTr.attribute = TimeRangeAttribute.Work;
        //                            st.planProcessingTimeList.Add(tempTr);
        //                            return tr.End;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        pos1 = k + 1;
        //                    }
        //                }

        //                pos = (int)workPos[workPos.Count - 1] + 1;

        //                TimeRange newtr = (TimeRange)idleAndWorkTimeList[pos];
        //                if (n > 1)
        //                {
        //                    TimeRange tempTr = newtr;

        //                    for (int j = 1; j < n; j++)
        //                    {
        //                        if (tempTr.End == ((TimeRange)idleAndWorkTimeList[pos + j]).Start)
        //                        {
        //                            tempTr.HoldingTime += ((TimeRange)idleAndWorkTimeList[pos + j]).HoldingTime;
        //                            tempTr.End = ((TimeRange)idleAndWorkTimeList[pos + j]).End;
        //                        }
        //                        else
        //                        {
        //                            TimeRange addTr = new TimeRange();
        //                            addTr = tempTr;
        //                            addTr.attribute = TimeRangeAttribute.Work;
        //                            st.planProcessingTimeList.Add(addTr);
        //                            tempTr = (TimeRange)idleAndWorkTimeList[pos + j];
        //                        }

        //                    }

        //                    tempTr.attribute = TimeRangeAttribute.Work;
        //                    st.planProcessingTimeList.Add(tempTr);

        //                    return tempTr.End;
        //                }
        //                else
        //                {
        //                    TimeRange tempTr = newtr;
        //                    tempTr.attribute = TimeRangeAttribute.Work;
        //                    st.planProcessingTimeList.Add(tempTr);
        //                    return newtr.End;
        //                }

        //            }
        //            else
        //            {
        //                TimeRange tr = (TimeRange)idleAndWorkTimeList[pos];
        //                if (n > 1)
        //                {
        //                    TimeRange tempTr = tr;

        //                    for (int j = 1; j < n; j++)
        //                    {
        //                        if (tempTr.End == ((TimeRange)idleAndWorkTimeList[pos + j]).Start)
        //                        {
        //                            tempTr.HoldingTime += ((TimeRange)idleAndWorkTimeList[pos + j]).HoldingTime;
        //                            tempTr.End = ((TimeRange)idleAndWorkTimeList[pos + j]).End;
        //                        }
        //                        else
        //                        {
        //                            TimeRange addTr = new TimeRange();
        //                            addTr = tempTr;
        //                            addTr.attribute = TimeRangeAttribute.Work;
        //                            st.planProcessingTimeList.Add(addTr);
        //                            tempTr = (TimeRange)idleAndWorkTimeList[pos + j];
        //                        }

        //                    }

        //                    tempTr.attribute = TimeRangeAttribute.Work;
        //                    st.planProcessingTimeList.Add(tempTr);

        //                    return tempTr.End;
        //                }
        //                else
        //                {
        //                    TimeRange tempTr = tr;
        //                    tempTr.attribute = TimeRangeAttribute.Work;
        //                    st.planProcessingTimeList.Add(tempTr);
        //                    return tr.End;
        //                }
        //            }


        //        }
        //    }

        //    return 0;
        //    //return endTime;
        //}

        ///// <summary>
        ///// 得到某任务单件加工时间
        ///// </summary>
        ///// <param name="taskID"></param>
        ///// <returns></returns>
        //public float getUnitProcessTimeofTask(int taskID)
        //{
        //    float unitTime = 0;
            
        //    //查询内存数据库
        //    for (int i = 0; i < this.capacityList.Count; i++)
        //    {
        //        if ((this.capacityList[i] as ResCapacity).FProcessID == taskID)
        //        {
        //            return (this.capacityList[i] as ResCapacity).FCapacity1;
        //        }
        //    }

        //    return unitTime;
        //}

        ///// <summary>
        ///// 得到某任务的加工准备时间，暂时不考虑
        ///// </summary>
        ///// <param name="preTaskID">前任务</param>
        ///// <param name="taskID">现任务</param>
        ///// <returns>加工准备时间</returns>
        //public float getPreProcessTimeofTask(int preTaskID, int taskID)
        //{
        //    float rtnTime = 0;
        //    //查询内存数据库

        //    return rtnTime;
        //}

        ///// <summary>
        ///// 得到任务所占资源负荷
        ///// </summary>
        ///// <param name="taskID"></param>
        ///// <returns></returns>
        //public double getBurthenofTask(int taskID)
        //{

        //    return 0;
        //}

        //public bool getViceRes(ScheduleTask st)
        //{
        //    for (int i = 0; i < this.capacityList.Count; i++)
        //    {
        //        if ((this.capacityList[i] as ResCapacity).FProcessID == st.ID)
        //        {
        //            if ((this.capacityList[i] as ResCapacity).viceResIDList.Count == 0)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                this.viceRes = (this.capacityList[i] as ResCapacity).viceResIDList[0] as ScheduleResource;
        //                return true;
        //            }
        //        }
        //    }
            
        //    return false;
        //}
    }

    //public class ResCapacity
    //{
    //    public int FResourceID;

    //    public int FProcessID;

    //    public float FEfficiency;

    //    public float FCapacity1;

    //    public float FCapacity2;

    //    public float FPreSetoutTime;

    //    public float FPostSetoutTime;

    //    public float FProcessPassRate;

    //    public ArrayList viceResIDList; //存储副资源引用

        //public float processPassRate;

        //public float onceTime;

        //public float onceNumber;

        //public float capOccupyRate;

        //public int maxCapOccupy;

        //public float minWorkingBatch;

        //public float minWorkingTime;

        //public float maxIdleRate;
         
//}

}