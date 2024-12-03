using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Data.Common;
namespace Algorithm
{
    [Serializable]
    public class SchProductRouteRes : ISerializable
    {
        #region //SchProductRouteRes属性定义
        public SchData schData = null;        //所有排程数据
        private int bScheduled = 0;            //是否已排产 0 未排，1 已排
        public int BScheduled
        {
            get { return bScheduled; }
            set { 
                if (value == 1 && bScheduled == 0) //设置为排产完成1
                {
                    if (this.schData.iCurRows < this.schData.iTotalRows)
                    {
                        this.schData.iCurRows++;                        
                    }
                    this.resource.iSchHours += this.iResRationHour;
                    this.resource.iPlanDays = this.resource.iSchHours/3600/ this.resource.iResHoursPd;
                }
                else if (value == 0 && bScheduled == 1) //原来是已排产，设置为未排产0
                {
                    if (this.schData.iCurRows > 1)
                        this.schData.iCurRows--;
                    this.resource.iSchHours -= this.iResRationHour;
                    this.resource.iPlanDays = this.resource.iSchHours / 3600 / this.resource.iResHoursPd;
                }
                bScheduled = value;
                if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
                {
                    DateTime dt = this.dResBegDate;
                    DateTime dt2 = this.dResEndDate;
                }
                   SchParam.dtResLastSchTime = DateTime.Now;
                   this.cDefine37 = SchParam.dtResLastSchTime;                        //排程运算结束时间
                }
        }
        public string cVersionNo;       //排程版本号
        public int iSchSdID;            //产品号
        public int iProcessProductID;   //任务号
        public int iProcessID;
        public int iResProcessID;
        public int iResourceAbilityID;   //明细ID
        public string cWoNo;             //工单号
        public int iItemID;
        public string cInvCode;          //加工物料
        public int iWoSeqID;             //工序号
        public string cTechNo;           //工艺编号
        public string cSeqNote;          //工艺说明
        public string iResGroupNo;       //资源组，同一组资源相互可替换
        public double iResGroupPriority;     //资源组优先级
        public string cSelected;          //是否选择可用
        public string cCanScheduled = "1";     //是否可选择排产,默认为0，资源选择后为1
        public double iResourceID;           //资源物料排产顺序
        public string cResourceNo;        //资源编号
        public string cResourceName;
        public double iResReqQty;          //分配资源生产数量
        public double iResReqQtyOld;          //原计划生产数量              
        public DateTime dResBegDate;     //资源计划开始时间
        public DateTime dResEndDate;      //资源计划结束时间
        public DateTime dResLeanBegDate;     //学习曲线开始时间
        public double iResRationHour;      //资源总加工工时
        public double iActResReqQty;       //实际完工数量，部分完工
        public double iActResRationHour;   //实际完工工时，
        public DateTime dActResBegDate;      //实际开工时间
        public DateTime dActResEndDate;      //实际完工时间
        public int iViceResource1ID;       //模具位置1
        public string cViceResource1No;   //辅助资源编号，也考虑其时间不能冲突,模具号1
        public int iViceResource2ID;      //模具位置2
        public string cViceResource2No;
        public int iViceResource3ID;      //模具位置3
        public string cViceResource3No;
        public string cWorkType;          //加工方式， 1 单件，2 批量加工
        public double iBatchQty;          //每批加工数量
        public double iBatchQtyBase;      //随件单批量 2023-06-02 
        public double iBatchWorkTime;     //每批加工时间（过程,还是用iCapacity）
        public double iBatchInterTime;    //每批间隔时间
        public double iResPreTime { get; set; }          //资源前准备时间
        public double iResPreTimeOld;       //资源前准备时间,工艺路线中设置
        public string cResPreTimeExp { get; set; }
        private double ICapacity;             //产能  秒/每件
        private int   iMoldCount = 1;             //模具套数
        public string cTeamResourceNo ;            //资源组编号
        public string cLearnCurvesNo;              //学习曲线编号
        private TechLearnCurves techLearnCurves;             //学习曲线
        public TechLearnCurves TechLearnCurves
        {
            get
            {              
                if (this.cLearnCurvesNo == "") this.techLearnCurves = null;
                if (this.techLearnCurves == null)  
                    this.techLearnCurves = new TechLearnCurves(this.cLearnCurvesNo,this);
                return this.techLearnCurves;
            }
        }
        public double iCapacity
        {
            get { return this.resource.cResOccupyType == "1" ? ICapacity / this.resource.iResourceNumber : ICapacity; }
            set { ICapacity = value; }
        }
        public string cCapacityExp;         //产能公式  
        public double cIsInfinityAbility;     //0 产能有限 1 产能无限 
        public double iResPostTime;          //资源后准备时间
        public string cResPostTimeExp;
        public double iCycTime;              //周期换刀时间
        public double iProcessPassRate;
        public double iEfficiency;           //效率，正常加工时间/效率，90%时，加工时间较长
        public double iHoursPd;
        public double iWorkQtyPd;
        public double iWorkersPd;
        public double iDevCountPd;
        public double iLaborTime;
        public double iLeadTime;
        public Int32 iSchSN = 1;            //排产顺序，记录每个任务的排产先后顺序
        public DateTime dCanResBegDate;     //可开工时间     2019-06-23
        public double iResWaitTime;            //任务开工等待时间，计划开工时间 - 可开工时间//从上工序可开工时间 + 第1批加工时间 + 批量移转时间
        public string FResChaValue1ID;             //工艺特征1    模具、刀具等
        public string FResChaValue2ID;             //工艺特征2
        public string FResChaValue3ID;
        public string FResChaValue4ID;
        public string FResChaValue5ID;
        public string FResChaValue6ID;
        public int FResChaValue1IDLeft = 0;              //工艺特征1    模具、刀具，一个换刀周期累计已用时间
        public int FResChaValue2IDLeft = 0;             //工艺特征2
        public int FResChaValue3IDLeft = 0;
        public int FResChaValue4IDLeft = 0;
        public int FResChaValue5IDLeft = 0;
        public int FResChaValue6IDLeft = 0;
        public ResChaValue resChaValue1;             //工艺特征1    模具、刀具等
        public ResChaValue resChaValue2;             //工艺特征2
        public ResChaValue resChaValue3;
        public ResChaValue resChaValue4;
        public ResChaValue resChaValue5;
        public ResChaValue resChaValue6;
        public double FResChaValue1Cyc;          //工艺特征1更换周期 
        public double FResChaValue2Cyc;
        public double FResChaValue3Cyc;
        public double FResChaValue4Cyc;
        public double FResChaValue5Cyc;
        public double FResChaValue6Cyc;
        public string cResourceNote;
        public string cDefine22;
        public string cDefine23;
        public string cDefine24;
        public string cDefine25;
        public string cDefine26;
        public string cDefine27;              //排程运算总时间
        public string cDefine28;
        public string cDefine29;
        public string cDefine30;
        public string cDefine31;
        public double cDefine32;
        public double cDefine33;
        public double cDefine34;              //上次排产顺序
        public double cDefine35;              //本次排产顺序，全局的
        public DateTime cDefine36;
        public DateTime cDefine37;            //排程运算结束时间
        public DateTime cDefine38;            //排程优先级对比时间
        public Int32 iPriorityRes;              //资源排产优化级, 可以用过程计算资源任务优先级
        public Int32 iPriorityResLast;              //资源排产优化级, 可以用过程计算资源任务优先级
        public SchProductRouteRes SchProductRouteResPre;      //前一个资源任务明细
        public int iBatch = 1;         //排产批次 油漆
        public int iSchBatch = 6;      //排产批次
        public int iDayMaxQty = 0;     //日最大限产，按类别定义  2023-10-02 
        public int iWeekMaxQty = 0;    //周最大限产，按类别定义  2023-10-02 
        #endregion
        public DateTime dEarliestStartTime;
        public DateTime dLatestEndTime;
        public List<TaskTimeRange> TaskTimeRangeList;
        public Resource resource;                //有值
        public SchProductRoute schProductRoute;  //有值
        public int DispatchSchTask(ref double ai_ResReqQty, DateTime adCanBegDate,SchProductRouteRes as_SchProductRouteResPre )
        {
            double fWorkTime = 0;     //用于过程计算
            int iWorkTime = 0;
            int iBatch = 0;
            DateTime ldtBeginDate = DateTime.Now;
            if (this.bScheduled == 1) return 0; //如果当前工序已排,则不用重排。如上次已确认的生产任务单
            try
            {
                String message;
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"1、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                                this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, 0);
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                if (adCanBegDate < this.dEarliestStartTime)
                {
                    adCanBegDate = this.dEarliestStartTime;
                }
                else
                    this.dEarliestStartTime = adCanBegDate;            //记录当前任务最早可开工时间 2019-12-10
                if (this.cWorkType == "1")   //批量加工 
                {
                    if (ai_ResReqQty / iBatchQty < Convert.ToInt32(ai_ResReqQty / iBatchQty))
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty) + 1;
                    else
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty);
                    if (iBatch < 1) iBatch = 1;
                    fWorkTime = Convert.ToDouble(iBatch * this.iCapacity + (iBatch - 1) * iBatchInterTime);
                }
                else                          // ‘0’单件加工,单价加工除以批量,类似单件加工。有些批量加工类型，可以同时加工不同品种  2022-08-08 JonasCheng
                {
                    fWorkTime = Convert.ToDouble(ai_ResReqQty * this.iCapacity / iBatchQty );//Convert.ToDouble(ai_ResReqQty * this.iCapacity);
                }
                if (this.schProductRoute.item != null && this.schProductRoute.item.iItemDifficulty > 0 && this.schProductRoute.item.iItemDifficulty != 1)
                    fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty;
                if (this.resource != null && this.resource.iResDifficulty > 0 && this.resource.iResDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0 && this.schProductRoute.techInfo.iTechDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);
                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }
                iWorkTime = (int)Convert.ToDecimal((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段
                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"2、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],运算时间[{5}]",
                                                                this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }
                int ai_ResPreTime = 0;   //资源换产时间
                int ai_CycTimeTol = 0;   //换刀时间
                if (adCanBegDate < this.resource.dMaxExeDate)
                    adCanBegDate = this.resource.dMaxExeDate;
                DateTime ldtBeginDateRes = DateTime.Now;
                if (this.cSeqNote == "折弯")
                {
                    ldtBeginDateRes = DateTime.Now;
                }
                try
                {
                    this.resource.ResSchTask(this, ref iWorkTime, adCanBegDate, ref ai_ResPreTime, ref ai_CycTimeTol, true, as_SchProductRouteResPre);
                }
                catch (Exception error)
                {
                    throw new Exception("资源正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                    return -1;
                }
                Double iWaitTime = DateTime.Now.Subtract(ldtBeginDateRes).TotalMilliseconds;
                if (this.cSeqNote == "折弯")
                {
                    iWaitTime = iWaitTime;
                }
                else
                {
                    iWaitTime = iWaitTime;
                }
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                this.iResPreTime = ai_ResPreTime; ///60;   //转换成分钟 前台显示  //this.iResPreTimeOld 
                this.iCycTime = ai_CycTimeTol;///60;
                this.iResRationHour += ai_ResPreTime; //+ ai_CycTimeTol ; /// 3600;    //总工时包含换产时间 + 换刀时间, //转换成小时 前台显示
                List<TaskTimeRange> list1 = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID; });
                if (TaskTimeRangeList.Count > 0)
                {
                    TaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    this.dResBegDate = TaskTimeRangeList[0].DBegTime;                 //取已排资源任务排产时间段的第1段开始时间
                    this.dResEndDate = TaskTimeRangeList[TaskTimeRangeList.Count - 1].DEndTime;   //取已排资源任务排产时间段的最后1段结束时间
                    Int32 iResReqQtyTemp = 0;
                    Double iAllottedTime = 0;
                    Int32 iResReqQtyTemp9 = 0;
                    if (int.TryParse(this.iResReqQty.ToString(), out iResReqQtyTemp9))
                    {
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                            if (iResReqQtyTemp < this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty = (this.cWorkType == "1" ? this.iBatchQty : 1) * Convert.ToInt32(iAllottedTime / this.iCapacity);
                                if (lTaskTimeRange.iResReqQty > this.iResReqQty)
                                    lTaskTimeRange.iResReqQty = this.iResReqQty;
                            }
                            else
                            {
                                lTaskTimeRange.iResReqQty = 0;
                            }
                            iResReqQtyTemp += Convert.ToInt32(lTaskTimeRange.iResReqQty);
                            if (iResReqQtyTemp > this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty -= iResReqQtyTemp - this.iResReqQty;
                                iResReqQtyTemp -= iResReqQtyTemp - int.Parse(this.iResReqQty.ToString());
                            }
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp;
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }
                        }
                    }
                    else    //需求数量带小数的
                    {
                        Double iResReqQtyTemp2 = 0;
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                            if (iResReqQtyTemp2 < this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty = (this.iResReqQty) * Convert.ToInt32((iAllottedTime / this.iCapacity));
                            }
                            else
                            {
                                lTaskTimeRange.iResReqQty = 0;
                            }
                            iResReqQtyTemp2 += Convert.ToDouble(lTaskTimeRange.iResReqQty);
                            if (iResReqQtyTemp2 > this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty -= iResReqQtyTemp2 - this.iResReqQty;
                                iResReqQtyTemp2 -= iResReqQtyTemp2 - Double.Parse(this.iResReqQty.ToString());
                            }
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp2;
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }
                        }
                    }
                }
                this.dCanResBegDate = adCanBegDate;      //记录任务可开工时间；
                this.iResWaitTime = (this.dResBegDate - this.dCanResBegDate).TotalHours;   //记录任务可开工时间；
                if (this.iResWaitTime > 1)
                {
                    int j = 0;
                }
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"9、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                }
            }
            catch (Exception exp)
            {
                SchParam.Debug(exp.Message, "资源运算");
                throw new Exception(" 出错信息:" + exp.Message);
            }
            return 1;
        }
        public int TaskSchTask(ref double ai_ResReqQty, DateTime adCanBegDate)
        {
            double fWorkTime = 0;     //用于过程计算
            int iWorkTime = 0;
            int iBatch = 0;
            DateTime ldtBeginDate = DateTime.Now;
            if (this.bScheduled == 1) return 0; //如果当前工序已排,则不用重排。如上次已确认的生产任务单
            try
            { 
                String message;
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"1、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                                this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, 0);
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                if (adCanBegDate < this.dEarliestStartTime)
                {
                    adCanBegDate = this.dEarliestStartTime;
                }
                else
                    this.dEarliestStartTime = adCanBegDate;            //记录当前任务最早可开工时间 2019-12-10
                if (this.cWorkType == "1")   //批量加工 
                {
                    if (ai_ResReqQty / iBatchQty > Convert.ToInt32(ai_ResReqQty / iBatchQty))
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty) + 1;
                    else if (ai_ResReqQty / iBatchQty < Convert.ToInt32(ai_ResReqQty / iBatchQty))
                    {
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty) ;
                    }
                    else
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty);
                    if (iBatch < 1) iBatch = 1;
                    fWorkTime = Convert.ToDouble(iBatch * this.iCapacity + (iBatch - 1) * iBatchInterTime);
                }
                else                          // ‘0’单件加工 
                {
                    fWorkTime = Convert.ToDouble(ai_ResReqQty * this.iCapacity/ iBatchQty);
                }
                if (this.schProductRoute.item != null &&  this.schProductRoute.item.iItemDifficulty > 0 && this.schProductRoute.item.iItemDifficulty != 1)
                    fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty;
                if (this.resource != null && this.resource.iResDifficulty > 0 && this.resource.iResDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0 && this.schProductRoute.techInfo.iTechDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);
                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }
                if (this.resource.bTeamResource == "1")
                {
                    if (this.resource.TeamResourceList.Count < 1)
                    {
                        throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编组没有具体资源编号！");
                        return -1;
                    }
                    this.resource.TeamResourceList.Sort(delegate(Resource p1, Resource p2) { return Comparer<DateTime>.Default.Compare(p1.GetEarlyStartDate(adCanBegDate, false), p2.GetEarlyStartDate(adCanBegDate, false)); });
                    this.cTeamResourceNo = this.cResourceNo ;
                    this.cResourceNo = this.resource.TeamResourceList[0].cResourceNo;
                    this.resource = this.resource.TeamResourceList[0];
                }
                iWorkTime = (int)Convert.ToDecimal((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段
                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"2、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],运算时间[{5}]",
                                                                this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now,"ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                 }
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }
                int ai_ResPreTime = 0;   //资源换产时间
                int ai_CycTimeTol = 0;   //换刀时间
                if (adCanBegDate < this.resource.dMaxExeDate )
                    adCanBegDate = this.resource.dMaxExeDate;
                DateTime ldtBeginDateRes = DateTime.Now;
                if (this.cSeqNote == "折弯")
                {
                    ldtBeginDateRes = DateTime.Now; 
                }
                try
                {
                    ldtBeginDate = DateTime.Now;
                    this.resource.ResSchTask(this, ref iWorkTime, adCanBegDate, ref ai_ResPreTime, ref ai_CycTimeTol);
                }
                catch (Exception error)
                {
                    throw new Exception("资源正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                    return -1;
                }
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                this.iResPreTime = ai_ResPreTime; ///60;   //转换成分钟 前台显示  //this.iResPreTimeOld 
                this.iCycTime = ai_CycTimeTol;///60;
                this.iResRationHour +=  ai_ResPreTime ; //+ ai_CycTimeTol ; /// 3600;    //总工时包含换产时间 + 换刀时间, //转换成小时 前台显示
                if (TaskTimeRangeList.Count > 0)
                {
                    TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    this.dResBegDate = TaskTimeRangeList[0].DBegTime;                 //取已排资源任务排产时间段的第1段开始时间
                    this.dResEndDate = TaskTimeRangeList[TaskTimeRangeList.Count - 1].DEndTime;   //取已排资源任务排产时间段的最后1段结束时间
                    Int32 iResReqQtyTemp = 0;
                    Double iAllottedTime = 0;
                    Int32 iResReqQtyTemp9 = 0;
                    if (int.TryParse(this.iResReqQty.ToString(),out iResReqQtyTemp9))
                    {
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                            if (iResReqQtyTemp < this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty = (this.cWorkType == "1" ? this.iBatchQty : 1) * Convert.ToInt32(iAllottedTime / this.iCapacity);
                            }
                            else
                            {
                                lTaskTimeRange.iResReqQty = 0;
                            }
                            iResReqQtyTemp += Convert.ToInt32(lTaskTimeRange.iResReqQty);
                            if (iResReqQtyTemp > this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty -= iResReqQtyTemp - this.iResReqQty;
                                iResReqQtyTemp -= iResReqQtyTemp - int.Parse(this.iResReqQty.ToString());
                            }
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp;
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }
                        }
                    }
                    else    //需求数量带小数的
                    {
                        Double iResReqQtyTemp2 = 0;
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                            if (iResReqQtyTemp2 < this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty = (this.iResReqQty) * Convert.ToInt32((iAllottedTime / this.iCapacity));
                            }
                            else
                            {
                                lTaskTimeRange.iResReqQty = 0;
                            }
                            iResReqQtyTemp2 += Convert.ToDouble(lTaskTimeRange.iResReqQty);
                            if (iResReqQtyTemp2 > this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty -= iResReqQtyTemp2 - this.iResReqQty;
                                iResReqQtyTemp2 -= iResReqQtyTemp2 - Double.Parse(this.iResReqQty.ToString());
                            }
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp2;
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }
                        }
                    }
                }
                this.dCanResBegDate = adCanBegDate;      //记录任务可开工时间；
                this.iResWaitTime = (this.dResBegDate - this.dCanResBegDate).TotalHours;   //记录任务可开工时间；
                if (this.iResWaitTime > 1)
                {
                    int j = 0;
                }
             }
            catch (Exception exp)
            {
                throw new Exception(" 出错信息:" + exp.Message);
            }
            return 1;
        }
        public int TaskSchTaskRev(ref double ai_ResReqQty, DateTime adCanEndDate,string cType = "1")
        {
            double fWorkTime = 0;   //用于临时计算
            int iWorkTime = 0;
            int iBatch = 0;
            if (this.bScheduled == 1) return 0; //如果当前工序已排,则不用重排。如上次已确认的生产任务单
            try
            { 
                if (this.cWorkType == "1")   //批量加工 
                {
                    if (ai_ResReqQty / iBatchQty < Convert.ToInt32(ai_ResReqQty / iBatchQty))
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty) + 1;
                    else
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty);
                    if (iBatch < 1) iBatch = 1;
                    fWorkTime = Convert.ToDouble(iBatch * this.iCapacity + (iBatch - 1) * iBatchInterTime);
                }
                else                         // ‘0’单件加工,单价加工除以批量,类似单件加工。有些批量加工类型，可以同时加工不同品种  2022-08-08 JonasCheng
                {
                    fWorkTime = Convert.ToDouble(ai_ResReqQty * this.iCapacity/ iBatchQty);//Convert.ToDouble(ai_ResReqQty * this.iCapacity);
                }
                if (this.schProductRoute.item != null && this.schProductRoute.item.iItemDifficulty > 0 && this.schProductRoute.item.iItemDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.item.iItemDifficulty);
                if (this.resource != null && this.resource.iResDifficulty > 0 && this.resource.iResDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0 && this.schProductRoute.techInfo.iTechDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);
                if (this.resource.bTeamResource == "1")
                {
                    if (this.resource.TeamResourceList.Count < 1)
                    {
                        throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编组没有具体资源编号！");
                        return -1;
                    }
                    this.resource.TeamResourceList.Sort(delegate(Resource p1, Resource p2) { return Comparer<DateTime>.Default.Compare(p1.GetEarlyStartDate(adCanEndDate, true), p2.GetEarlyStartDate(adCanEndDate, true)); });
                    this.cTeamResourceNo = this.cResourceNo;
                    this.cResourceNo = this.resource.TeamResourceList[0].cResourceNo;
                    this.resource = this.resource.TeamResourceList[0];
                }
                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }
                iWorkTime = (int)Convert.ToDecimal((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段
                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;
                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }
                try
                {
                    this.resource.ResSchTaskRev(this, ref iWorkTime, adCanEndDate);
                }
                catch (Exception ex2)
                {                
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.ResSchTaskRev,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]倒排出错! 资源任务号"+ this.iResourceAbilityID + " "+ ex2.Message);
                    return -1;
                }
                try
                {
                    if (TaskTimeRangeList.Count > 0)
                    {
                        TaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                        this.dResBegDate = TaskTimeRangeList[0].DBegTime;                 //取已排资源任务排产时间段的第1段开始时间
                        this.dResEndDate = TaskTimeRangeList[TaskTimeRangeList.Count - 1].DEndTime;   //取已排资源任务排产时间段的最后1段结束时间
                        Int32 iResReqQtyTemp = 0;
                        Double iAllottedTime = 0;
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.AllottedTime * this.resource.iEfficient * SchParam.AllResiEfficient / 10000;   //考虑资源效率和所有资源利用率参数
                            lTaskTimeRange.iResReqQty = Convert.ToInt32(iAllottedTime / this.iCapacity);
                            iResReqQtyTemp += Convert.ToInt32(lTaskTimeRange.iResReqQty);
                            if (i == 0) //TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp;
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }
                        }
                    }
                }                 
                catch (Exception ex2)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.ResSchTaskRev,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]倒排出错! 资源任务号" + this.iResourceAbilityID + " TaskTimeRangeList" + ex2.Message);
                    return -1;
                }
        }
            catch (Exception exp)
            {
                SchParam.Debug(exp.Message, "资源运算");
                throw new Exception(" 出错信息:" + exp.Message);
            }
            return 1;
        }
        public void TaskClearTask()
        {
            this.resource.ResClearTask(this);
        }
        public int TestResSchTask(ref double ai_ResReqQty, DateTime adCanBegDate)
        {
            double fWorkTime = 0;   //
            int iWorkTime = 0;
            int iBatch = 0;
            if (this.bScheduled == 1) return 0; //如果当前工序已排,则不用重排。如上次已确认的生产任务单
            try
            { 
                if (this.cWorkType == "1")   //批量加工 
                {
                    if (ai_ResReqQty / iBatchQty < Convert.ToInt32(ai_ResReqQty / iBatchQty))
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty) + 1;
                    else
                        iBatch = Convert.ToInt32(ai_ResReqQty / iBatchQty);
                    if (iBatch < 1) iBatch = 1;
                    fWorkTime = Convert.ToDouble(iBatch * this.iCapacity + (iBatch - 1) * iBatchInterTime);
                }
                else                          // ‘0’单件加工,单价加工除以批量,类似单件加工。有些批量加工类型，可以同时加工不同品种  2022-08-08 JonasCheng
                {
                    fWorkTime = Convert.ToDouble(ai_ResReqQty * this.iCapacity/ iBatchQty);//Convert.ToDouble(ai_ResReqQty * this.iCapacity);
                }
                if (this.schProductRoute.item != null && this.schProductRoute.item.iItemDifficulty > 0)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.item.iItemDifficulty);
                if (this.resource != null && this.resource.iResDifficulty > 0)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);
                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TestResSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }
                iWorkTime = (int)Convert.ToDecimal(((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient)));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段
                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;
                if (adCanBegDate < this.resource.dMaxExeDate)
                    adCanBegDate = this.resource.dMaxExeDate;
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                int ai_workTimeTest = iWorkTime;
                int ai_CycTimeTol = 0, ai_ResPreTime = 0;
                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;
                try
                {
                    int li_Return = this.resource.TestResSchTask(this, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDate, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate, false);
                    if (li_Return < 0)
                    {
                        string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", this.iSchSdID, this.cInvCode, this.cResourceNo, this.iProcessProductID, this.iCapacity, this.iResReqQty, iWorkTime / 3600, iWorkTime / 3600, adCanBegDateTest);
                        throw new Exception(cError);
                        this.cCanScheduled = "0";
                        return -1;
                    }
                    else   //可排
                    {
                        this.cCanScheduled = "1";
                        this.dResBegDate = dtBegDate;
                        this.dResEndDate = dtEndDate;
                    }
                }
                catch (Exception error)
                {                
                    throw new Exception("资源模拟测试正排出错,订单行号：" + this.iSchSdID + ",位置SchProductRouteRes.TestResSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                    return -1;
                }
            }
            catch (Exception exp)
            {
                SchParam.Debug(exp.Message,"资源运算");
                throw new Exception( " 出错信息:" + exp.Message);
            }
            return 1;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}