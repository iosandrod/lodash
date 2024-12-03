using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace Algorithm
{
    public class ScheduleProduct : IComparable
    {
        public ArrayList taskList;
        public float perfectProcessTime;
        public float batch = 1;
        public double passRate = 1;
        public int ID
        {
            get
            {
                return id;
            }
        }
        private int id;
        public string RelateID
        {
            get
            {
                return relateID;
            }
        }
        private string relateID;
        public float earliestStartTime = 0;
        public float latestEndTime = Scheduling.maxTimeValue;
        public float planStartTime;
        public float planEndTime;
        public ScheduleProduct()
        {
            taskList = new ArrayList(10);
        }
        public ScheduleProduct(int id, string relateID)
        {
            taskList = new ArrayList(10);
            this.id = id;
            this.relateID = relateID;
        }
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
        public void iniData()
        {
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
        }
        public ScheduleProduct deepClone()
        {
            ScheduleProduct temp = new ScheduleProduct();
            return temp;
        }
        public float getPerfectProcessTime()
        {
            float perfectProcessTime = 0;
            return perfectProcessTime;
        }
        public float getPlanProcessTime()
        {
            float planProcessTime = 0;
            return planProcessTime;
        }
        public float getWaitTime()
        {
            return getPlanProcessTime() - getPerfectProcessTime();
        }
        public string affiliationOrder;
        public string clientName;
        private int clientImportantDegree;
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
        private int overdueDegree;
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
        private int clientImportantWeight;
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
        private int overdueWeight;
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
        private int priority = -1;
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
        public ProductType productType;
        public string name;
        public float deliveryTime = Scheduling.maxTimeValue;
        public float receiveTime = 0;
    }
    public enum ProductType
    {
        Assembly,    //部件
        WIP,         //半成品
        FinishedGoods //成品
    }
}