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
                   

                //任务已排产完成数加1,完工记录数加1
                if (value == 1 && bScheduled == 0) //设置为排产完成1
                {
                    if (this.schData.iCurRows < this.schData.iTotalRows)
                    {
                        this.schData.iCurRows++;                        
                    }

                    //当前资源排产工时增加 2022-11-06 JonasCheng
                    this.resource.iSchHours += this.iResRationHour;
                    this.resource.iPlanDays = this.resource.iSchHours/3600/ this.resource.iResHoursPd;

                }
                else if (value == 0 && bScheduled == 1) //原来是已排产，设置为未排产0
                {
                    if (this.schData.iCurRows > 1)
                        this.schData.iCurRows--;

                    //当前资源排产工时增加
                    this.resource.iSchHours -= this.iResRationHour;
                    this.resource.iPlanDays = this.resource.iSchHours / 3600 / this.resource.iResHoursPd;
                }

                bScheduled = value;

                //Console.WriteLine("任务 iSchSdID:" + this.iSchSdID.ToString() + ",iProcessProductID:" + this.iProcessProductID.ToString() + ",iResProcessID:" + this.iResProcessID.ToString() + ",cResourceNo:" + this.cResourceNo.ToString() + " 时间" + DateTime.Now.ToString());


                //调试
                if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
                {
                    DateTime dt = this.dResBegDate;
                    DateTime dt2 = this.dResEndDate;
                }

                    ////任务已排，记录排产开始、结束时间、排产顺序
                    //if (value == 1 || value == 2)
                    //{
                    //    String message = string.Format(@"排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],资源名称[{4}],开工[{5}],完工[{6}],排产顺序[{7}],总工时[{8}],前准备时间[{9}],加工物料[{10}],工序号[{11}],工单号[{12}],最早开工时间[{13}],油漆分批批次[{14}],加工数量[{15}],单件工时[{16}],工序状态[{17}],开始排产时间[{18}],完成排产时间[{19}]",
                    //                                    this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, this.resource.cResourceName, this.dResBegDate, this.dResEndDate, this.iSchSN, this.iResRationHour, this.iResPreTime, this.cInvCode, this.iWoSeqID, this.cWoNo, this.schProductRoute.dEarlyBegDate, this.iBatch, this.iResReqQty, this.iCapacity, this.schProductRoute.cStatus, SchParam.dtResLastSchTime, DateTime.Now);
                    //    SchParam.Debug(message, "排程运算工时");

                    //}

                    //if (value == 1)
                    //{
                    //    string message = string.Format("工艺特征:" + this.cDefine25);
                    //    SchParam.Debug(message, "排程运算工艺");
                    //}
                    
                   //更新最新排产时间
                   //this.cDefine27 = SchData.GetDateDiffString(SchParam.dtResLastSchTime, DateTime.Now, "s");//  ;             //排程运算开始时间
                   SchParam.dtResLastSchTime = DateTime.Now;
                   this.cDefine37 = SchParam.dtResLastSchTime;                        //排程运算结束时间

                ////每排一个任务，进度条刷新 2021-03-20 JonasCheng
                //if (bScheduled == 1)
                //    this.schData.iCurRows++;
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
        //public string FResChaValue7ID;
        //public string FResChaValue8ID;
        //public string FResChaValue9ID;
        //public string FResChaValue10ID;
        //public string FResChaValue11ID;
        //public string FResChaValue12ID;


        public int FResChaValue1IDLeft = 0;              //工艺特征1    模具、刀具，一个换刀周期累计已用时间
        public int FResChaValue2IDLeft = 0;             //工艺特征2
        public int FResChaValue3IDLeft = 0;
        public int FResChaValue4IDLeft = 0;
        public int FResChaValue5IDLeft = 0;
        public int FResChaValue6IDLeft = 0;
        //public int FResChaValue7IDLeft = 0;
        //public int FResChaValue8IDLeft = 0;
        //public int FResChaValue9IDLeft = 0;
        //public int FResChaValue10IDLeft = 0;
        //public int FResChaValue11IDLeft = 0;
        //public int FResChaValue12IDLeft = 0;


        public ResChaValue resChaValue1;             //工艺特征1    模具、刀具等
        public ResChaValue resChaValue2;             //工艺特征2
        public ResChaValue resChaValue3;
        public ResChaValue resChaValue4;
        public ResChaValue resChaValue5;
        public ResChaValue resChaValue6;
        //public ResChaValue resChaValue7;
        //public ResChaValue resChaValue8;
        //public ResChaValue resChaValue9;
        //public ResChaValue resChaValue10;
        //public ResChaValue resChaValue11;
        //public ResChaValue resChaValue12;

        public double FResChaValue1Cyc;          //工艺特征1更换周期 
        public double FResChaValue2Cyc;
        public double FResChaValue3Cyc;
        public double FResChaValue4Cyc;
        public double FResChaValue5Cyc;
        public double FResChaValue6Cyc;
        //public double FResChaValue7Cyc;
        //public double FResChaValue8Cyc;
        //public double FResChaValue9Cyc;
        //public double FResChaValue10Cyc;
        //public double FResChaValue11Cyc;
        //public double FResChaValue12Cyc;
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

        /// <summary>
        /// 任务计划允许的最早加工时间，排程后得出
        /// 1、考虑前工序的开工时间，有多道工序时，同时考虑，取时间最大者。
        /// 2、考虑材料的到货时间。材料需要分配后，才能确定当前任务对应的材料是库存预留，采购订单明细，还是本次计划等
        /// </summary>
        public DateTime dEarliestStartTime;

        /// <summary>
        /// 任务计划允许的最晚结束时间，排程后得出
        /// </summary>
        public DateTime dLatestEndTime;

        //与Scheduling中的一至，传入
        //public List<Resource> ResourceList; //= new List<Resource>(10);


        //TaskTimeRange
        //iSchSdID,保存资源分配给任务的时间段。
        //private ArrayList TaskTimeRangeList = new ArrayList(10);

        //与Scheduling中的一至，传入,所有的TaskTimeRange
        public List<TaskTimeRange> TaskTimeRangeList;

        //对应的资源对象
        public Resource resource;                //有值

        //对应的工序对象
        public SchProductRoute schProductRoute;  //有值


        //调度优化排产，正排 2020-08-20 JonasCheng 
        public int DispatchSchTask(ref double ai_ResReqQty, DateTime adCanBegDate,SchProductRouteRes as_SchProductRouteResPre )
        {

            //计算加工工作时间，分单件及批量加工两种方式
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

                //如果资源可开始日期大于adCanBegDate,则以this.dEarliestStartTime
                if (adCanBegDate < this.dEarliestStartTime)
                {
                    adCanBegDate = this.dEarliestStartTime;
                }
                else
                    this.dEarliestStartTime = adCanBegDate;            //记录当前任务最早可开工时间 2019-12-10



                if (this.cWorkType == "1")   //批量加工 
                {
                    //不足一批的小数部分，当一批计算
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

                //考虑加工物料的难度系数 2019-03-09
                if (this.schProductRoute.item != null && this.schProductRoute.item.iItemDifficulty > 0 && this.schProductRoute.item.iItemDifficulty != 1)
                    fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty;

                //考虑资源加工难度系数 2019-03-09
                if (this.resource != null && this.resource.iResDifficulty > 0 && this.resource.iResDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);

                //考虑工艺加工难度系数 2019-03-09
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0 && this.schProductRoute.techInfo.iTechDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);

                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }


                //考虑资源利用率,如90% ,iWorkTime会延长,最后再转换成整数(秒)
                iWorkTime = (int)Convert.ToDecimal((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段

                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;

                //GetDateDiffString(System.DateTime Date1, System.DateTime Date2, string Interval)
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

                //------调用资源对象的排任务方法 -------------------------------------
                int ai_ResPreTime = 0;   //资源换产时间
                int ai_CycTimeTol = 0;   //换刀时间

                //考虑资源的最早可排时间,cNeedChanged = '1'时，取开工任务的最大完工日期 2014-04-01
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
                                                      //this.dCanResBegDate = adCanBegDate;      //记录任务可开工时间；
                                                      //this.iResWaitTime =  (this.dResBegDate - this.dCanResBegDate).TotalHours;   //记录任务可开工时间；



                //ai_ResPreTime 时间包含了 ai_CycTimeTol this.iResPreTimeOld 

                //更新当前资源任务的计划开始时间，计划完工时间
                //TaskTimeRangeList.Sort()
                List<TaskTimeRange> list1 = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID; });

                if (TaskTimeRangeList.Count > 0)
                {
                    //倒排，从最后一个时段开始计算时段生产数量，因为第1、2时段可能会有准备时间，换产时间
                    TaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    this.dResBegDate = TaskTimeRangeList[0].DBegTime;                 //取已排资源任务排产时间段的第1段开始时间
                    this.dResEndDate = TaskTimeRangeList[TaskTimeRangeList.Count - 1].DEndTime;   //取已排资源任务排产时间段的最后1段结束时间

                    Int32 iResReqQtyTemp = 0;
                    Double iAllottedTime = 0;

                    //this.iSchSN = SchParam.iSchSNMax++;   //记录当前工序排产顺序

                    Int32 iResReqQtyTemp9 = 0;

                    //如果加工数量带小数点 2018-1-15
                    if (int.TryParse(this.iResReqQty.ToString(), out iResReqQtyTemp9))
                    {

                        //计算各个已排任务时段的加工数量，要做4舍5入,从最后一个时段开始;除去中间换刀时间
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                                                                                                                                                                                                                       //考虑加工批量
                            if (iResReqQtyTemp < this.iResReqQty)
                            {
                                lTaskTimeRange.iResReqQty = (this.cWorkType == "1" ? this.iBatchQty : 1) * Convert.ToInt32(iAllottedTime / this.iCapacity);
                                //特殊处理，如果单件时间非常小，及时数量非常大，异常 2022-10-17 JonasCheng
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

                            //前面时段要做4舍5入，尾差放到最后一个时段
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp;
                                //防止出现负数
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }

                            ////同步更新资源任务列表中，时段任务加工数量
                            //TaskTimeRange ResTaskTimeRange = this.resource.TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID && p1.DBegTime ==lTaskTimeRange.DBegTime ; });
                            //ResTaskTimeRange.iResReqQty = lTaskTimeRange.iResReqQty;
                        }

                    }
                    else    //需求数量带小数的
                    {
                        Double iResReqQtyTemp2 = 0;

                        //计算各个已排任务时段的加工数量，要做4舍5入,从最后一个时段开始;除去中间换刀时间
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                                                                                                                                                                                                                       //考虑加工批量
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

                            //前面时段要做4舍5入，尾差放到最后一个时段
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp2;
                                //防止出现负数
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }

                            ////同步更新资源任务列表中，时段任务加工数量
                            //TaskTimeRange ResTaskTimeRange = this.resource.TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID && p1.DBegTime ==lTaskTimeRange.DBegTime ; });
                            //ResTaskTimeRange.iResReqQty = lTaskTimeRange.iResReqQty;
                        }

                    }
                }

                //记录
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
                //string message = string.Format(@"9、SchProductRouteRes.DispatchSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}] 运算出错",
                //                                            this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                SchParam.Debug(exp.Message, "资源运算");

                throw new Exception(" 出错信息:" + exp.Message);

                //throw exp;
            }


            return 1;

        }


        //正排 给资源分配任务，生成资源任务时段占用列表
        public int TaskSchTask(ref double ai_ResReqQty, DateTime adCanBegDate)
        {


            //计算加工工作时间，分单件及批量加工两种方式
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

                //如果资源可开始日期大于adCanBegDate,则以this.dEarliestStartTime
                if (adCanBegDate < this.dEarliestStartTime)
                {
                    adCanBegDate = this.dEarliestStartTime;
                }
                else
                    this.dEarliestStartTime = adCanBegDate;            //记录当前任务最早可开工时间 2019-12-10
                       


                if (this.cWorkType == "1")   //批量加工 
                {
                    //不足一批的小数部分，当一批计算
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

                //考虑加工物料的难度系数 2019-03-09
                if (this.schProductRoute.item != null &&  this.schProductRoute.item.iItemDifficulty > 0 && this.schProductRoute.item.iItemDifficulty != 1)
                    fWorkTime = fWorkTime * this.schProductRoute.item.iItemDifficulty;

                //考虑资源加工难度系数 2019-03-09
                if (this.resource != null && this.resource.iResDifficulty > 0 && this.resource.iResDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);

                //考虑工艺加工难度系数 2019-03-09
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0 && this.schProductRoute.techInfo.iTechDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);

                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }

                //资源组选择合适资源进行排产2016-05-25

                if (this.resource.bTeamResource == "1")
                {
                    if (this.resource.TeamResourceList.Count < 1)
                    {
                        throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编组没有具体资源编号！");
                        return -1;
                    }

                    //关键资源按排产优先级排序
                    this.resource.TeamResourceList.Sort(delegate(Resource p1, Resource p2) { return Comparer<DateTime>.Default.Compare(p1.GetEarlyStartDate(adCanBegDate, false), p2.GetEarlyStartDate(adCanBegDate, false)); });

                    this.cTeamResourceNo = this.cResourceNo ;
                    this.cResourceNo = this.resource.TeamResourceList[0].cResourceNo;
                    this.resource = this.resource.TeamResourceList[0];

                }

                //考虑资源利用率,如90% ,iWorkTime会延长,最后再转换成整数(秒)
                iWorkTime = (int)Convert.ToDecimal((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段

                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;

                //GetDateDiffString(System.DateTime Date1, System.DateTime Date2, string Interval)
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

                //------调用资源对象的排任务方法 -------------------------------------
                int ai_ResPreTime = 0;   //资源换产时间
                int ai_CycTimeTol = 0;   //换刀时间

                //考虑资源的最早可排时间,cNeedChanged = '1'时，取开工任务的最大完工日期 2014-04-01
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


                //DateTime ldtEndedDate = DateTime.Now;            

                //TimeSpan interval = ldtEndedDate - ldtBeginDate;//计算间隔时间

                //Double iWaitTime = interval.TotalMilliseconds;
                //if (this.cSeqNote == "折弯")
                //{
                //    iWaitTime = iWaitTime;
                //}
                //else
                //{
                //    iWaitTime = iWaitTime;
                //}

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
                //this.dCanResBegDate = adCanBegDate;      //记录任务可开工时间；
                //this.iResWaitTime =  (this.dResBegDate - this.dCanResBegDate).TotalHours;   //记录任务可开工时间；

            

                //ai_ResPreTime 时间包含了 ai_CycTimeTol this.iResPreTimeOld 

                //更新当前资源任务的计划开始时间，计划完工时间
                //TaskTimeRangeList.Sort()
                //List<TaskTimeRange> list1 = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID; });

                if (TaskTimeRangeList.Count > 0)
                {
                    //倒排，从最后一个时段开始计算时段生产数量，因为第1、2时段可能会有准备时间，换产时间
                    TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    this.dResBegDate = TaskTimeRangeList[0].DBegTime;                 //取已排资源任务排产时间段的第1段开始时间
                    this.dResEndDate = TaskTimeRangeList[TaskTimeRangeList.Count - 1].DEndTime;   //取已排资源任务排产时间段的最后1段结束时间

                    Int32 iResReqQtyTemp = 0;
                    Double iAllottedTime = 0;

                

                    //this.iSchSN = SchParam.iSchSNMax++;   //记录当前工序排产顺序

                    Int32 iResReqQtyTemp9 = 0;

                    //如果加工数量带小数点 2018-1-15
                    if (int.TryParse(this.iResReqQty.ToString(),out iResReqQtyTemp9))
                    {

                        //计算各个已排任务时段的加工数量，要做4舍5入,从最后一个时段开始;除去中间换刀时间
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                            //考虑加工批量
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

                            //前面时段要做4舍5入，尾差放到最后一个时段
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp;
                                //防止出现负数
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }

                            ////同步更新资源任务列表中，时段任务加工数量
                            //TaskTimeRange ResTaskTimeRange = this.resource.TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID && p1.DBegTime ==lTaskTimeRange.DBegTime ; });
                            //ResTaskTimeRange.iResReqQty = lTaskTimeRange.iResReqQty;
                        }

                    }
                    else    //需求数量带小数的
                    {
                        Double iResReqQtyTemp2 = 0;

                        //计算各个已排任务时段的加工数量，要做4舍5入,从最后一个时段开始;除去中间换刀时间
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.WorkTimeAct * this.resource.iEfficient * SchParam.AllResiEfficient / 10000 * (this.iResRationHour - this.iResPreTime) / this.iResRationHour;   //考虑资源效率和所有资源利用率参数
                            //考虑加工批量
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

                            //前面时段要做4舍5入，尾差放到最后一个时段
                            if (i == 0)//TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp2;
                                //防止出现负数
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }

                            ////同步更新资源任务列表中，时段任务加工数量
                            //TaskTimeRange ResTaskTimeRange = this.resource.TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID && p1.DBegTime ==lTaskTimeRange.DBegTime ; });
                            //ResTaskTimeRange.iResReqQty = lTaskTimeRange.iResReqQty;
                        }
                    
                    }
                }

                //记录
                this.dCanResBegDate = adCanBegDate;      //记录任务可开工时间；
                this.iResWaitTime = (this.dResBegDate - this.dCanResBegDate).TotalHours;   //记录任务可开工时间；

                if (this.iResWaitTime > 1)
                {
                    int j = 0;
                }

                //if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                //{
                //    message = string.Format(@"9、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                //                                            this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                //    SchParam.Debug(message, "资源运算");
                //}
             }
            catch (Exception exp)
            {
                //string message = string.Format(@"2、SchProductRouteRes.TaskSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],运算时间[{5}]",
                //                                               this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                //SchParam.Debug(exp.Message, "资源运算");

                throw new Exception(" 出错信息:" + exp.Message);
                
                
            }
            

            return 1;

        }

        //倒排 给资源分配任务，生成资源任务时段占用列表, cType = "1" 正常倒排调用, "2" 正排中调用倒排 --------2023-03-06
        public int TaskSchTaskRev(ref double ai_ResReqQty, DateTime adCanEndDate,string cType = "1")
        {
            //计算加工工作时间，分单件及批量加工两种方式
            double fWorkTime = 0;   //用于临时计算
            int iWorkTime = 0;
            int iBatch = 0;

            if (this.bScheduled == 1) return 0; //如果当前工序已排,则不用重排。如上次已确认的生产任务单

            try
            { 
                if (this.cWorkType == "1")   //批量加工 
                {
                    //不足一批的小数部分，当一批计算
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

                //考虑加工物料的难度系数 2019-03-09
                if (this.schProductRoute.item != null && this.schProductRoute.item.iItemDifficulty > 0 && this.schProductRoute.item.iItemDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.item.iItemDifficulty);

                //考虑资源加工难度系数 2019-03-09
                if (this.resource != null && this.resource.iResDifficulty > 0 && this.resource.iResDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);

                //考虑工艺加工难度系数 2019-03-09
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0 && this.schProductRoute.techInfo.iTechDifficulty != 1)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);

                //SchParam.Debug("0000000", "资源运算");

                //资源组选择合适资源进行排产2016-05-25

                if (this.resource.bTeamResource == "1")
                {
                    if (this.resource.TeamResourceList.Count < 1)
                    {
                        throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编组没有具体资源编号！");
                        return -1;
                    }

                    //关键资源按排产优先级排序
                    this.resource.TeamResourceList.Sort(delegate(Resource p1, Resource p2) { return Comparer<DateTime>.Default.Compare(p1.GetEarlyStartDate(adCanEndDate, true), p2.GetEarlyStartDate(adCanEndDate, true)); });

                    this.cTeamResourceNo = this.cResourceNo;
                    this.cResourceNo = this.resource.TeamResourceList[0].cResourceNo;
                    this.resource = this.resource.TeamResourceList[0];
               

                }

                //SchParam.Debug("0000111111111", "资源运算");

                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }



                //考虑资源利用率,如90% ,iWorkTime会延长 
                iWorkTime = (int)Convert.ToDecimal((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段


                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;

                if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }

                //SchParam.Debug("1111111111", "资源运算");


                //---------调用资源对象的排任务方法------------------------------------------------
                try
                {
                    this.resource.ResSchTaskRev(this, ref iWorkTime, adCanEndDate);

                    //SchParam.Debug("1122222222222 ResSchTaskRev ", "资源运算");
                }
                catch (Exception ex2)
                {                
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.ResSchTaskRev,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]倒排出错! 资源任务号"+ this.iResourceAbilityID + " "+ ex2.Message);
                
                    return -1;
                }
                //SchParam.Debug("22222", "资源运算");


                //更新当前资源任务的计划开始时间，计划完工时间
                //TaskTimeRangeList.Sort()
                //List<TaskTimeRange> list1 = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID; });

                try
                {

                    if (TaskTimeRangeList.Count > 0)
                    {
                        TaskTimeRangeList.Sort(delegate (TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                        this.dResBegDate = TaskTimeRangeList[0].DBegTime;                 //取已排资源任务排产时间段的第1段开始时间
                        this.dResEndDate = TaskTimeRangeList[TaskTimeRangeList.Count - 1].DEndTime;   //取已排资源任务排产时间段的最后1段结束时间

                        Int32 iResReqQtyTemp = 0;
                        Double iAllottedTime = 0;

                        //SchParam.Debug("3333333333", "资源运算");

                        //this.iSchSN = SchParam.iSchSNMax++;   //记录当前工序排产顺序

                        //计算各个已排任务时段的加工数量，要做4舍5入
                        for (int i = TaskTimeRangeList.Count - 1; i >= 0; i--)
                        {
                            TaskTimeRange lTaskTimeRange = TaskTimeRangeList[i];
                            iAllottedTime = (int)(Double)lTaskTimeRange.AllottedTime * this.resource.iEfficient * SchParam.AllResiEfficient / 10000;   //考虑资源效率和所有资源利用率参数

                            lTaskTimeRange.iResReqQty = Convert.ToInt32(iAllottedTime / this.iCapacity);

                            iResReqQtyTemp += Convert.ToInt32(lTaskTimeRange.iResReqQty);

                            //前面时段要做4舍5入，尾差放到最后一个时段
                            if (i == 0) //TaskTimeRangeList.Count - 1
                            {
                                lTaskTimeRange.iResReqQty += this.iResReqQty - iResReqQtyTemp;
                                //防止出现负数
                                if (lTaskTimeRange.iResReqQty < 0) lTaskTimeRange.iResReqQty = 0;
                            }

                            ////同步更新资源任务列表中，时段任务加工数量
                            //TaskTimeRange ResTaskTimeRange = this.resource.TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResProcessID == this.iResProcessID && p1.DBegTime == lTaskTimeRange.DBegTime; });
                            //ResTaskTimeRange.iResReqQty = lTaskTimeRange.iResReqQty;
                        }

                        //SchParam.Debug("444444444", "资源运算");
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
                //string message = string.Format(@"2、SchProductRouteRes.TaskSchTaskRev 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}]",
                //                                              this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now);
                SchParam.Debug(exp.Message, "资源运算");

                throw new Exception(" 出错信息:" + exp.Message);
                //throw exp;
            }

            return 1;

        }

        //清除资源已排具体任务占用时间段,注意TaskTimeRangeList包含所有任务已排时间段
        public void TaskClearTask()
        {
            //调用资源的清除任务
            this.resource.ResClearTask(this);

            ////2016-06-06
            //this.iResReqQty = 0 ;
            //this.iResRationHour = 0;

        }

        //正排,测试能否排下，用于选择资源
        public int TestResSchTask(ref double ai_ResReqQty, DateTime adCanBegDate)
        {
            //计算加工工作时间，分单件及批量加工两种方式
            double fWorkTime = 0;   //
            int iWorkTime = 0;
            int iBatch = 0;

            if (this.bScheduled == 1) return 0; //如果当前工序已排,则不用重排。如上次已确认的生产任务单


            try
            { 
                if (this.cWorkType == "1")   //批量加工 
                {
                    //不足一批的小数部分，当一批计算
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

                //考虑加工物料的难度系数 2019-03-09
                if (this.schProductRoute.item != null && this.schProductRoute.item.iItemDifficulty > 0)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.item.iItemDifficulty);

                //考虑资源加工难度系数 2019-03-09
                if (this.resource != null && this.resource.iResDifficulty > 0)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.resource.iResDifficulty);

                //考虑工艺加工难度系数 2019-03-09
                if (this.schProductRoute.techInfo != null && this.schProductRoute.techInfo.iTechDifficulty > 0)
                    fWorkTime = Convert.ToDouble(fWorkTime * this.schProductRoute.techInfo.iTechDifficulty);

                if (this.resource == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TestResSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编号不能为空！");
                    return -1;
                }


                //资源组选择合适资源进行排产2016-05-25

                //if (this.resource.bTeamResource == "1")
                //{
                //    if (this.resource.TeamResourceList.Count < 1)
                //    {
                //        throw new Exception("订单行号：" + this.iSchSdID + "出错位置:SchProductRouteRes.TaskSchTask,加工物料[" + this.cInvCode + "] 工序[" + this.cSeqNote + "]对应资源编组没有具体资源编号！");
                //        return -1;
                //    }

                //    foreach (Resource ResourceTemp in this.resource.TeamResourceList)
                //    {
                //        ResourceTemp.bAllocated = "0";
                //    }

                //    //如果同一任务中有多个资源组时，不能选到同一台设备
                //    foreach( SchProductRouteRes SchProductRouteResTemp in   this.schProductRoute.SchProductRouteResList )
                //    {
                //        if (SchProductRouteResTemp.cResourceNo != this.resource.cResourceNo)
                //        {
                //            Resource ResourceTemp = this.resource.TeamResourceList.Find(delegate(Resource p1) { return p1.cResourceNo == SchProductRouteResTemp.cResourceNo; });
                //            if(ResourceTemp != null)
                //                ResourceTemp.bAllocated = "1";
                //             //this.resource.TeamResourceList.Find(" =" +SchProductRouteResTemp.cResourceNo)
                //        }

                //    }

                //    //关键资源按排产优先级排序
                //    List<Resource> listTeam = this.resource.TeamResourceList.FindAll(delegate(Resource p1) { return p1.bAllocated == "0"; });

                //    listTeam.Sort(delegate(Resource p1, Resource p2) { return Comparer<DateTime>.Default.Compare(p1.GetEarlyStartDate(adCanBegDate, false), p2.GetEarlyStartDate(adCanBegDate, false)); });

                //    this.cTeamResourceNo = this.cResourceNo;
                //    this.cResourceNo = listTeam[0].cResourceNo;
                //    this.resource = listTeam[0];
                

                //}

                //考虑资源利用率,如90% ,iWorkTime会延长 
                iWorkTime = (int)Convert.ToDecimal(((100.00 * 100 * fWorkTime) / (this.resource.iEfficient * SchParam.AllResiEfficient)));
                if (iWorkTime < 1) iWorkTime = 1;     //加工时间小于1秒，没法找到对应时段

                this.iResReqQty = ai_ResReqQty;
                this.iResRationHour = iWorkTime;

                //--------1.1 返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开-------------------------------

                //考虑资源的最早可排时间,cNeedChanged = '1'时，取开工任务的最大完工日期 2014-04-01
                if (adCanBegDate < this.resource.dMaxExeDate)
                    adCanBegDate = this.resource.dMaxExeDate;

                //DateTime dtEndDate = adCanBegDate;
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                //DateTime adCanEndDateTest = adCanBegDate;
                int ai_workTimeTest = iWorkTime;
                //int ai_ResPreTime = 0;
                int ai_CycTimeTol = 0, ai_ResPreTime = 0;

                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;


                //String message;
                //if (this.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID || this.iProcessProductID == 193864 && schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                //{
                //    message = string.Format(@"1、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                //                                                this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, 0);
                //    SchParam.Debug(message, "资源运算");
                //    //ldtBeginDate = DateTime.Now;
                //}


                //返回adCanBegDateTask，为任务可开始排产时间,作为正式排产开始日期;adCanBegDateTest 任务完工时间
                try
                {
                    int li_Return = this.resource.TestResSchTask(this, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDate, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate, false);
                    if (li_Return < 0)
                    {
                        string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", this.iSchSdID, this.cInvCode, this.cResourceNo, this.iProcessProductID, this.iCapacity, this.iResReqQty, iWorkTime / 3600, iWorkTime / 3600, adCanBegDateTest);
                        throw new Exception(cError);
                        //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + adCanBegDateTest.ToLongTimeString() + ",请检查工作日历!");
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
                //string message = string.Format(@"1、SchProductRouteRes.TestResSchTask排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                //                                                this.iSchSN, this.iSchSdID, this.iProcessProductID, this.cResourceNo, DateTime.Now, 0);
                SchParam.Debug(exp.Message,"资源运算");

                throw new Exception( " 出错信息:" + exp.Message);

                //throw exp;
            }


            ////计算用adCanBegDate
            //adCanBegDate = adCanBegDateTask;

            return 1;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
