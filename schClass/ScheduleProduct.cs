using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Algorithm
{
    /// <summary>
    /// 产品类，代表实际生产的成品，半成品，零部件等，与排程层次相关  
    /// </summary>
    public class ScheduleProduct : IComparable
    {
        /// <summary>
        /// 任务列表，为任务类集合
        /// </summary>
        /// <remarks>储存产品各工序加工细节</remarks>
        public ArrayList taskList;

        /// <summary>
        /// 理想加工时间
        /// </summary>
        public float perfectProcessTime;
        /// <summary>
        /// 产品批量
        /// </summary>
        public float batch = 1;

        /// <summary>
        /// 产品加工合格率，用于确定第一个工序的批量
        /// </summary>
        public double passRate = 1;

        /// <summary>
        /// 产品原始编号
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
        /// 产品关联ID
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
        /// 允许最早开始时间，为距离原点时间秒数
        /// </summary>
        public float earliestStartTime = 0;

        /// <summary>
        /// 允许最晚结束时间，为距离原点时间秒数
        /// </summary>
        public float latestEndTime = Scheduling.maxTimeValue;

        /// <summary>
        /// 计划开始加工时间
        /// </summary>
        public float planStartTime;

        /// <summary>
        /// 计划结束加工时间
        /// </summary>
        public float planEndTime;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <remarks>初始化对象数据</remarks>
        /// <returns>无返回值</returns>
        public ScheduleProduct()
        {
            taskList = new ArrayList(10);
        }

        /// <summary>
        /// 有参数构造器
        /// </summary>
        /// <param name="id" >原始编号</param>
        /// <param name="relateID" >关联编号</param>
        public ScheduleProduct(int id, string relateID)
        {
            taskList = new ArrayList(10);
            this.id = id;
            this.relateID = relateID;

            //iniData();

        }

        /// <summary>
        /// 实现IComparable接口，便于产品排序
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>-1,1</returns>
        public int CompareTo(object obj)
        {
            if (obj is ScheduleProduct)
            {
                ScheduleProduct tempSP = (ScheduleProduct)obj;

                if (this.priority != tempSP.priority)
                {
                    return tempSP.priority - this.priority;
                }
                else
                {
                    if (this.deliveryTime == tempSP.deliveryTime)
                    {
                        if (this.clientImportantDegree == tempSP.clientImportantDegree)
                        {
                            if (this.receiveTime == tempSP.receiveTime)
                            {
                                Random rnd = new Random();
                                if (rnd.NextDouble() < 0.5)
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
                                if (this.receiveTime > tempSP.receiveTime)
                                {
                                    return 1;
                                }
                                else
                                {
                                    return -1;
                                }
                            }
                        }
                        else
                        {
                            if (this.clientImportantDegree > tempSP.clientImportantDegree)
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                    }
                    else
                    {
                        if (this.deliveryTime > tempSP.deliveryTime)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException("对象非ScheduleProduct类型");
            }
        }


        /// <summary>
        /// 初始化产品相关属性
        /// </summary>
        public void iniData()
        {
            //从内存数据库中取数据赋给各属性
            //基础属性
            /*this.affiliationOrder;
            this.batch;
            this.clientImportantDegree;
            this.clientImportantWeight;
            this.clientName;
            this.deliveryTime;
            this.earliestStartTime;
            this.latestEndTime;
            this.name;
            this.overdueDegree;
            this.overdueWeight;
            this.passRate;
            this.priority;
            this.productType;
            this.taskList;*/

            //排程属性
            //任务之间的前后关系，间隔时间
            //各任务具体批量
            //各任务转运方式、转运时间、转运批量

        }
        /// <summary>
        /// 深度拷贝，暂不实现
        /// </summary>
        /// <remarks>将实体中的所有数据拷贝到一个新的实体中</remarks>
        /// <returns>实体的一个副本</returns>
        public ScheduleProduct deepClone()
        {
            ScheduleProduct temp = new ScheduleProduct();

            return temp;
        }

        /// <summary>
        /// 计算理想情况下，加工该产品所需时间
        /// </summary>
        /// <returns></returns>
        public float getPerfectProcessTime()
        {
            float perfectProcessTime = 0;

            return perfectProcessTime;
        }

        /// <summary>
        /// 计算计划状态下加工该产品所需的时间
        /// </summary>
        /// <returns></returns>
        public float getPlanProcessTime()
        {
            float planProcessTime = 0;

            return planProcessTime;
        }

        /// <summary>
        /// 计算产品加工过程中的待工时间
        /// </summary>
        public float getWaitTime()
        {
            return getPlanProcessTime() - getPerfectProcessTime();
        }

        /// <summary>
        /// 所属订单，为订单编号
        /// </summary>
        public string affiliationOrder;

        /// <summary>
        /// 客户名称
        /// </summary>
        public string clientName;

        /// <summary>
        /// 属性ClientImportantDegree值
        /// </summary>
        private int clientImportantDegree;

        /// <summary>
        /// 客户重要度,0-5,0最不重要，5最重要，从订单属性继承，亦可排程前指定
        /// </summary>
        public int ClientImportantDegree
        {
            get
            {
                return clientImportantDegree;
            }
            set
            {
                if (value > 5 || value < 0)
                {
                    throw new ArgumentException("客户重要度应设为0-5之间");
                }
                else
                {
                    clientImportantDegree = value;
                }
            }
        }

        /// <summary>
        /// 属性OverdueDegree值
        /// </summary>
        private int overdueDegree;

        /// <summary>
        /// 交货紧急度，0-5，0最不紧急，5最紧急，从订单属性中继承，亦可排程中指定
        /// </summary>
        public int OverdueDegree
        {
            get
            {
                return overdueDegree;
            }
            set
            {
                if (value > 5 || value < 0)
                {
                    throw new ArgumentException("订单紧急度应设为0-5之间");
                }
                else
                {
                    overdueDegree = value;
                }
            }
        }


        /// <summary>
        /// 属性ClientImportantWeight值
        /// </summary>
        private int clientImportantWeight;

        /// <summary>
        /// 客户重要度权重,0-5之间，在系统设置中设置，或排程前指定
        /// </summary>
        public int ClientImportantWeight
        {
            get
            {
                return clientImportantWeight;
            }
            set
            {
                if (value > 5 || value < 0)
                {
                    throw new ArgumentException("客户重要性权重应设为0-5之间");
                }
                else
                {
                    clientImportantWeight = value;
                }
            }

        }

        /// <summary>
        /// 属性OverdueWeight值
        /// </summary>
        private int overdueWeight;

        /// <summary>
        /// 交货紧急度权重，0-10之间,在系统设置中设置，或排程前指定
        /// </summary>
        public int OverdueWeight
        {
            get
            {
                return overdueWeight;

            }
            set
            {
                if (value > 10 || value < 0)
                {
                    throw new ArgumentException("订单紧急度权重应设为0-10之间");
                }
                else
                {
                    overdueWeight = value;
                }
            }
        }

        /// <summary>
        /// 属性Priority值
        /// </summary>
        private int priority = -1;

        /// <summary>
        /// 订单优先级，设定或由客户友好度、重要度、交货紧急度及相应权重计算得出，或排程前指定
        /// </summary>
        public int Priority
        {
            get
            {
                if (priority == -1)
                {
                    priority = clientImportantDegree * clientImportantWeight + overdueDegree * overdueWeight;
                }

                return priority;
            }
            set
            {
                if (value > 100 || value < 0)
                {
                    throw new ArgumentException("任务优先级应设为0-100之间");
                }
                else
                {
                    priority = value;
                }
            }
        }

        /// <summary>
        /// 产品类型，分为部件、半成品、成品
        /// </summary>
        public ProductType productType;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string name;

        /// <summary>
        /// 交货时间,为距离原点时间秒数
        /// </summary>
        public float deliveryTime = Scheduling.maxTimeValue;

        /// <summary>
        /// 接受时间，为距离原点时间秒数
        /// </summary>
        public float receiveTime = 0;
    }

    public enum ProductType
    {
        Assembly,    //部件
        WIP,         //半成品
        FinishedGoods //成品
    }
}
