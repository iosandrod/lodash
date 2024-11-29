using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;

namespace Algorithm
{
    [Serializable]
    public class SchProductRoute : ISerializable
    {
        #region //SchProductRoute属性定义
        public SchData schData = null;        //所有排程数据
        private int bScheduled = 0;            //是否已排产 0 未排，1 已排

        public string cVersionNo{get;set;}      //排程版本
        public int iSchSdID { get; set; }           //排程产品ID
        public int iModelID { get; set; }            //产品模型ID
        public int iProcessProductID { get; set; }   //工艺模型中的任务号
        public string cWoNo { get; set; }           //工单号
        public int iInterID { get; set; }            //工单内码
        public int iWoProcessID { get; set; }
        public int iItemID { get; set; }            //产品ID
        public string cInvCode { get; set; }         //产品编号 
        public int iWorkItemID { get; set; }         //加工物料ID
        public string cWorkItemNo { get; set; }      //加工物料编号 
        public string cWorkItemNoFull { get; set; }      //加工物料编号全路径
        public int iProcessID { get; set; }          //
        public int iWoSeqID { get; set; }           //工序号
        public string cTechNo { get; set; }          //工艺编号 
        public string cSeqNote { get; set; }         //工艺说明
        public string cWcNo { get; set; }            //工作中心
        public int iNextSeqID { get; set; }          //后序工序
        public string cPreProcessID { get; set; }
        public string cPostProcessID { get; set; }
        public string cPreProcessItem { get; set; }   //前工序列表，iSchSdID相同，iProcessProductID号
        public string cPostProcessItem { get; set; }  //后工序列表，iSchSdID相同，iProcessProductID号
        //X	int
        //Y	int
        public int iAutoID { get; set; }
        public string cLevelInfo { get; set; }
        public int iLevel { get; set; }
        public int iParentItemID { get; set; }        //父项ID
        public string cParentItemNo { get; set; }     //父项编号

        public string cParentItemNoFull { get; set; }     //父项编号全路径
        //iProcessSumQty	decimal
        //iQtyPer	decimal
        //iParentQty	decimal
        //cSourceCode	varchar
        //iSourceInterID	int
        //iSourceEntryID	int
        //cEasID	nvarchar
        public string cParellelType { get; set; }      //并行类型 ES 前工序结束后工序开始  SS 前工序开始后工序开始(差一个批次移转时间)  EE 同时结束(差一个批次移转时间)  
        public string cParallelNo { get; set; }        //并行码
        //iSeqRatio	decimal
        public string cKeyBrantch { get; set; }        //关键分支
        public string cCompSeq { get; set; }           //完工工序
        //cIsReport	char
        public string cMoveType { get; set; }           //移转方式 0 整体 1 按时 2 按量 
        public double iMoveInterTime { get; set; }       //移动间隔时间
        public double iMoveInterQty { get; set; }        //移动间隔数量
        public double iMoveTime { get; set; }            //移动时间
        public int iDevCountPd = 1;        //用于加工本工序最大资源数
        public string cDevCountPdExp { get; set; }	    //排产资源数表达式

        private double ISeqPretime;
        public double iSeqPreTime { get {
                if (this.ISeqPretime == null) this.ISeqPretime = 0;

                //if (this.techInfo == null) return this.ISeqPretime;

                //if (this.techInfo.iSeqPretime == null) this.techInfo.iSeqPretime = 0;

                //return (this.ISeqPretime < this.techInfo.iSeqPretime ? this.techInfo.iSeqPretime : this.ISeqPretime);
                return this.ISeqPretime;
                }   
                set { ISeqPretime = value; } 
        }	        //工序前准备时间

        private double ISeqPostTime;
        public double iSeqPostTime
        {
            get
            {
                if (this.ISeqPostTime == null) this.ISeqPostTime = 0;

                //if (this.techInfo == null) return this.ISeqPostTime;

                //if (this.techInfo.iSeqPostTime == null) this.techInfo.iSeqPostTime = 0; 

                //return (this.ISeqPostTime < this.techInfo.iSeqPostTime ? this.techInfo.iSeqPostTime : this.ISeqPostTime);
                return this.ISeqPostTime;
            }
            set { ISeqPostTime = value; }
        }	        //工序后准备时间
        //cWorkType	char
        //iBatchQty	decimal
        //iBatchWorkTime	decimal
        //iBatchInterTime	decimal
        //public decimal iResPreTime { get; set; }         //工序前准备时间
        //public string cResPreTimeExp { get; set; }       //工序前准备时间表达式
        public decimal iCapacity { get; set; }
        public string cCapacityExp { get; set; }	        //单件工时表达式
        //public decimal iResPostTime { get; set; }
        //public string cResPostTimeExp { get; set; }      //工序后准备时间
        public decimal iProcessPassRate { get; set; }
        public decimal iEfficiency { get; set; }
        public decimal iHoursPd { get; set; }
        public decimal iWorkQtyPd { get; set; }
        public decimal iWorkersPd { get; set; }


        public double iLaborTime { get; set; }          //计划生产总工时
        public double iLeadTime { get; set; }               //提前期
        public string cStatus { get; set; }               //工序状态
        public string cSourStatus { get; set; }              //原始工序状态
        public int iPriority { get; set; }                  //工序优先级
        public double iReqQty { get; set; }                  //计划生产数量
        public double iReqQtyOld { get; set; }               //原计划生产数量
        public double iActQty { get; set; }                  //实际生产数量
        public double iRealHour { get; set; }                 //实际加工工时
        public DateTime dBegDate { get; set; }                //计划开工时间
        public DateTime dEndDate { get; set; }                //计划完工时间
        public DateTime dActBegDate { get; set; }             //实际开工时间
        public DateTime dActEndDate { get; set; }             //实际完工时间
        public DateTime dEarlyBegDate { get; set; }           //最早可开始时间 ,排产时更新,正排时有用
        public DateTime dEarlySubItemDate { get; set; }       //最早材料到料时间 ,外部设置,正排时有用
        public decimal iAdvanceDate { get; set; }             //加工件累计提前期，本层物料采购最长提前期，天，影响本物料工序可开工时间 2019-03-07
        public decimal iAvgLeadTime { get; set; }             //加工提前期  //暂时不用
        public decimal iItemDifficulty { get; set; }          //物料加工难度系数,影响工序资源加工工时，难度系数大时加工时间长    

        public string cNote { get; set; }

        public string cDefine22 { get; set; }
        public string cDefine23 { get; set; }
        public string cDefine24 { get; set; }
        public string cDefine25 { get; set; }
        public string cDefine26 { get; set; }
        public string cDefine27 { get; set; } 
        public string cDefine28{ get; set; }
        public string cDefine29 { get; set; }
        public string cDefine30 { get; set; }
        public string cDefine31 { get; set; }
        public double cDefine32 { get; set; } 
        public double cDefine33{ get; set; }
        public double cDefine34 { get; set; }              //上次排产顺序
        public double cDefine35 { get; set; }              //本次排产顺序，全局的
        public DateTime cDefine36 { get; set; }
        public DateTime cDefine37 { get; set; } 

        public int iSchBatch = 6;               //排产批次
        public int cBatchNoFlag = 0;            //0 托盘资源选择未处理 1 托盘资源选择已处理
        public DateTime dCanBegDate { get; set; }      //工序最早可开工日期
        public DateTime dFirstBegDate { get; set; }    //第一次开工日期，允许可提前多少天开工，设置参数,如1天，如果未执行要考虑，已经执行了不用考虑
        public DateTime dFirstEndDate { get; set; }    //第一次完工日期，允许可提前多少天开工，设置参数,如1天
        


        public int BScheduled
        {
            get { return bScheduled; }
            set
            {
                //排程已排数量不准，工序排完后，下面所有未选择工序都设置未已排产。2022-11-06 JonasCheng
                if (value == 1 && bScheduled == 0) //设置为排产完成1
                {
                    if (this.schData.iCurRows < this.schData.iTotalRows)
                    {
                        int iCount = this.SchProductRouteResList.FindAll(item => item.iResReqQty == 0).Count;
                        this.schData.iCurRows += iCount;
                    }

                }
                else if (value == 0 && bScheduled == 1) //原来是已排产，设置为未排产0
                {                   
                    int iCount = this.SchProductRouteResList.FindAll(item => item.iResReqQty == 0).Count;

                    if (this.schData.iCurRows > iCount)
                        this.schData.iCurRows -= iCount;
                    else
                        this.schData.iCurRows = 0;
                }

                bScheduled = value;
                //Console.WriteLine("工序iSchSdID:" + this.iSchSdID.ToString() + ",iProcessProductID:" + this.iProcessProductID.ToString() + " 时间" + DateTime.Now.ToString());

                ////当前工序设置为已排产,而且托盘号不为空,cBatchNoFlag 0 托盘资源选择未处理 1 托盘资源选择已处理 ,而且有多个资源可以选择时调用
                //if (value == 1 && this.schProduct.cBatchNo != "" && this.cWoNo == "" && cBatchNoFlag == 0 && this.SchProductRouteResList.Count > 1)
                //{
                //    //设置当前托盘的批号为
                //    this.cBatchResourceSelect();
                //}

                if (this.iSchSdID == SchParam.iSchSdID)
                {
                    DateTime dt = this.dBegDate;
                    DateTime dt2 = this.dEndDate;
                }

                //调试
                if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
                {
                    DateTime dt = this.dBegDate;
                    DateTime dt2 = this.dEndDate;
                }

                ////每排一个任务，进度条刷新 2021-03-20 JonasCheng
                //if (bScheduled == 1)
                //    this.schData.iCurRows++;
            }
        }
        
        #endregion

        //用于保存一个工序可选资源列表及产能数据
        //private ArrayList SchProductRouteResList = new ArrayList(10);
        public SchProduct schProduct;          //工序对应产品信息  
        public SchProductWorkItem schProductWorkItem;          //工序对应工单信息  
        public Item item;                      //加工物料信息
        public TechInfo techInfo;              //工艺信息

        //加工件子料
        public List<SchProductRouteItem> SchProductRouteItemList = new List<SchProductRouteItem>(10);


        public List<SchProductRouteRes> SchProductRouteResList = new List<SchProductRouteRes>(10);



        //与Scheduling中的一至，传入
        //public List<Resource> ResourceList; //= new List<Resource>(10);

        //与Scheduling中的一至，传入
        //public List<SchProductRoute> SchProductRouteList ;

        //当前工序前工序列表,用于方向查找
        public List<SchProductRoute> SchProductRoutePreList = new List<SchProductRoute>(10);

        //当前工序后工序列表,用于正向查找
        public List<SchProductRoute> SchProductRouteNextList = new List<SchProductRoute>(10);


        //排当前工序, 工序排程,需要选择可排产的资源记录，并分配各资源的加工数量等
        public int ProcessSchTask(Boolean bFreeze = false)
        {

            //    select * from dbo.t_SchProductRoute
            //    where cVersionNo = 'NewVersion'
            //    order by iLevel desc,cParentItemNo,cWorkItemNo,iProcessProductID

            //产品工艺模型中工序有先有后，必须按工艺模型的先后进行排程。
            //按t_SchProductRoute.iLevel Desc 

            ////计算本工序的最早开工时间，考虑移动批量。
            // 整批移转时：取上工序的完工时间 
            // 批量移转时，按时：上工序开工时间 + 批量间隔时间 + 批量移转时间
            // 批量移转时，按量：上工序开工时间 + 批量间隔数量 × 产能 + 批量移转时间
            // 上工序有多道时，取最大的可开工时间

            //如果当前工序已排，则返回
            if (this.bScheduled == 1) return 1;


            //调试
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
                j = 1;
            }

            //如果当前工序是并行工序（非关键分支）,后工序还没有排产，则先跳过排产。一定要等后工序排完，才排
            if (this.cParallelNo != "" && this.cKeyBrantch != "1" && this.SchProductRouteNextList.Count > 0 && this.SchProductRouteNextList[0].bScheduled != 1)
            {
                return 1;
            }

            DateTime dDateTemp = this.schData.dtStart;//DateTime.Now;
            DateTime dCanBegDate = this.schData.dtStart;//DateTime.Now;    //有多道前序工序时，找最大的可开工时间   
            
            DateTime dCanBegDateProcess = this.schData.dtStart;  //工序可开工时间

            try
            {
                ////0 不考虑前工序结束时间   2021-10-28 JonasCheng
                //if (SchParam.PreSeqEndDate != "1")
                //{
                //    dCanBegDate = SchParam.dtStart;
                //}
                //else   //考虑前工序结束时间
                {

                    if (dCanBegDate < this.dEarlySubItemDate)
                        dCanBegDate = this.dEarlySubItemDate;

                    //如果产品中设置了最早可排时间，而且前工序最晚完工时比它小，则取最大值进行排产
                    //与参数配套使用SetMinDelayTime = 24;         //  配套最少延期时间,见SchProduct.ProductSchTaskRev()
                    if (this.schProduct != null)
                    {
                        if (dCanBegDate < this.schProduct.dEarliestSchDate)
                            dCanBegDate = this.schProduct.dEarliestSchDate;
                    }

                    if (this.schProductWorkItem != null)
                    {
                        if (dCanBegDate < this.schProductWorkItem.dCanBegDate)
                            dCanBegDate = this.schProductWorkItem.dCanBegDate;
                    }

                    //如果是正式订单，重排时可能会提前很久就开始生产，材料还没有到。dLastBegDateBeforeDays   2022-05-31 JonasCheng               
                    if (this.cVersionNo.ToLower() == "sureversion" && this.dFirstBegDate > schData.dtStart && dCanBegDate < this.dFirstBegDate.AddDays(-SchParam.dLastBegDateBeforeDays))
                        dCanBegDate = this.dFirstBegDate.AddDays(-SchParam.dLastBegDateBeforeDays);

                    //计算工序最早可排时间
                    this.GetRouteEarlyBegDate();

                    //考虑当前工序最早可排时间,下层半成品的完工时间,原材料的采购提前期
                    if (dCanBegDate < this.dEarlyBegDate)
                        dCanBegDate = this.dEarlyBegDate;
                }


                //如果工序已执行(已汇报或工序状态已下达，或已开工),保持原来的任务分配，资源编号选择不变
                //排程正式版本，不用重新选择资源，保持原来的任务分配，资源编号选择不变 2021-11-06 JonasCheng 
                if (this.cVersionNo.ToLower() == "sureversion"   ) //非待工状态  //|| this.iActQty > 0 || ( this.cStatus != "0" 
                {
                    //引用ERP生产任务单排程， 生产任务单处于新单状态,有多资源明细 2021-12-27 
                    //重新选择资源及分配加工数量
                    if (SchParam.UseAPS == "3" && this.schProductWorkItem != null && this.schProductWorkItem.cStatus == "I" && this.SchProductRouteResList.Count > 1)
                    {
                        try
                        {
                            ResourceSelect(dCanBegDate, bFreeze);
                        }
                        catch (Exception error)
                        {
                            //System.Windows.Forms.Clipboard.SetText(error.StackTrace);
                            if (SchParam.iSchSdID < 1)
                                throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                            else
                                throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);



                            return -1;
                        }
                    }

                    ////找出所有选择可用的机台,可排下的资源
                    List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQtyOld > 0 ; });

                    ////按选择的机台数平均分配生产任务
                    //int iResCount = ListRouteRes.Count;
                    //if (iResCount < 1)  //没有可用资源
                    //{
                    //    throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + "没有可排产资源编号.或排产范围太小,开始日期：'" + dCanBegDate.ToLongDateString() + "'"  );
                    //    return -1;
                    //}

                    //如果模盒号大于1，表示同一套模具加工,类似左中右部件一起排产，只排左，中右都设置为模盒号为2、3，不参与排产 2020-08-04 
                    if (this.item !=null && this.item.cMoldPosition != "" && this.item.cMoldPosition != "1")
                    {
                        List<SchProductRouteRes> ListRouteRes2 = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty > 0; });

                        for (int i = 0; i < ListRouteRes2.Count; i++)
                        {
                            ListRouteRes2[i].dResBegDate = dCanBegDate;
                            ListRouteRes2[i].dResEndDate = dCanBegDate.AddMinutes(1);
                            ListRouteRes2[i].TaskTimeRangeList.Clear();
                            ListRouteRes2[i].BScheduled = 1;

                        }
                    }

                    //正常排产
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        //调用SchProductRouteRes.TaskSchTask,加工时间重新计算                       
                        if (ListRouteRes[i].iResReqQty > 0)
                            ListRouteRes[i].TaskSchTask(ref ListRouteRes[i].iResReqQty, dCanBegDate);
                        else
                        {
                            ListRouteRes[i].dResBegDate = dCanBegDate;
                            ListRouteRes[i].dResEndDate = dCanBegDate.AddMinutes(1);
                            ListRouteRes[i].TaskTimeRangeList.Clear();
                            ListRouteRes[i].BScheduled = 1;


                        }

                    }
                }
                else  //新版本
                {
                    //根据工序最大机台数量,如2台，选择最优的两台资源进行排产,ListRouteRes只留两台cSelected的 2013-10-21
                    //资源选择,cCanScheduled = '1',先找出最早可开工日期的工序
                    //if (this.cWoNo == "")
                    try
                    {
                        if (this.iReqQty > 0)
                        {
                            try
                            {
                                ResourceSelect(dCanBegDate, bFreeze);
                            }
                            catch (Exception error)
                            {
                                //System.Windows.Forms.Clipboard.SetText(error.StackTrace);
                                if (SchParam.iSchSdID < 1)
                                    throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                                else
                                    throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);

                                

                                return -1;
                            }

                            //如果模盒号大于1，表示同一套模具加工,类似左中右部件一起排产，只排左，中右都设置为模盒号为2、3，不参与排产 2020-08-04 
                            if (this.item != null && this.item.cMoldPosition != null &&  this.item.cMoldPosition != "" && this.item.cMoldPosition != "0" && this.item.cMoldPosition != "1")
                            {
                                List<SchProductRouteRes> ListRouteRes2 = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty > 0; });

                                for (int i = 0; i < ListRouteRes2.Count; i++)
                                {
                                    ListRouteRes2[i].dResBegDate = dCanBegDate;
                                    ListRouteRes2[i].dResEndDate = dCanBegDate.AddMinutes(1);
                                    ListRouteRes2[i].TaskTimeRangeList.Clear();
                                    ListRouteRes2[i].BScheduled = 1;

                                }
                            }
                        }
                        else
                        {
                            List<SchProductRouteRes> ListRouteRes2 = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty > 0; });

                            for (int i = 0; i < ListRouteRes2.Count; i++)
                            {
                                //调用SchProductRouteRes.TaskSchTask,加工时间重新计算                       
                                if (ListRouteRes2[i].iResReqQty > 0)
                                    ListRouteRes2[i].TaskSchTask(ref ListRouteRes2[i].iResReqQty, dCanBegDate);
                                else
                                {
                                    ListRouteRes2[i].dResBegDate = dCanBegDate;
                                    ListRouteRes2[i].dResEndDate = dCanBegDate.AddMinutes(1);
                                    ListRouteRes2[i].TaskTimeRangeList.Clear();
                                    ListRouteRes2[i].BScheduled = 1;
                                }

                            }
                        }
                    }
                    catch (Exception error)
                    {
                        //System.Windows.Forms.Clipboard.SetText(error.StackTrace);
                        if ( SchParam.iSchSdID < 1   )
                            throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                        else
                            throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                        

                        return -1;
                    }


                    ////找出所有选择可用的机台,可排下的资源
                    List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });

                    //按选择的机台数平均分配生产任务
                    int iResCount = ListRouteRes.Count;
                    if (iResCount < 1 )  //没有可用资源
                    {
                        //如果存在资源任务已完工，cCanScheduled = '0',则本工序不用排产
                        List<SchProductRouteRes> ListRouteResCan = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "0"; });

                        if (ListRouteResCan.Count < 1)
                        {
                            throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + "没有可排产资源编号.或排产范围太小,开始日期：'" + dCanBegDate.ToLongDateString() + "'");
                            return -1;
                        }
                        else
                        {
                            //更新工序状态为已排
                            //this.BScheduled = 1;    //0 未排 1 已排
                            foreach (var item in ListRouteResCan)
                            {
                                item.BScheduled = 1;
                            }

                            this.BScheduled = 1;    //0 未排 1 已排

                            return 1;
                        }                       
                    }                    

                    //double iResReqQtyPer = (int)(this.iReqQty - this.iActQty) / iResCount;  //小数取整，尾差留到最后一批
                    //double iLeftReqQty = this.iReqQty - this.iActQty;
                    //double iResReqQty = iResReqQtyPer;
                   

                    ////2016-06-06
                    //if (iResReqQtyPer < 1)
                    //{
                    //    //iResCount = (int)iLeftReqQty; //this.iReqQty;
                    //    //iResReqQtyPer = 1;
                    //    iResCount = 1;
                    //    iResReqQtyPer = (this.iReqQty - this.iActQty);
                    //}

                    //记录工序可开工时间
                    dCanBegDateProcess = dCanBegDate;

                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        ////
                        //if (iLeftReqQty <= 0) continue;

                        ////每个资源任务排产时，都用工序可开工时间
                        //dCanBegDate = dCanBegDateProcess;
                       
                        ////调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                        //if (i == ListRouteRes.Count)
                        //    iResReqQty = iLeftReqQty;
                        //else
                        //{                            
                        //    //如果最小加工批量大于1，取整,有多资源排产
                        //    if (ListRouteRes[i].iBatchQtyBase > 1 && ListRouteRes.Count > 1 )
                        //    {
                        //        iResReqQty = Math.Ceiling(iResReqQtyPer / ListRouteRes[i].iBatchQtyBase) * ListRouteRes[i].iBatchQtyBase;
                        //    }
                        //    else
                        //    {
                        //        iResReqQty = iResReqQtyPer;
                        //    }
                        //}

                        ////记录每排产一个机台后，剩余的计划数量
                        //if (iLeftReqQty - iResReqQty > 0)
                        //{
                        //    iLeftReqQty = iLeftReqQty - iResReqQty;
                        //}
                        //else
                        //{ 
                        //    //最后一台机，剩下的数量都排给他
                        //    iResReqQty = iLeftReqQty;
                        //    iLeftReqQty = 0;
                        //}

                        ////调用SchProductRouteRes.TaskSchTask,加工时间重新计算
                        //if (this.iSchBatch == 1) //已执行生产任务单,资源计划数量不变,已减去已完工数量 2014-11-04
                        //{
                        //    iResReqQty = ListRouteRes[i].iResReqQty; //- ListRouteRes[i].iActResReqQty;                       
                        //}
                        

                        ////最后一个资源排产时，剩余尾数数量全部排在这台
                        //if (iLeftReqQty > 0 && i == ListRouteRes.Count - 1)
                        //{
                        //    iResReqQty += iLeftReqQty;
                        //}

                        //这里不改变资源排产数量，多资源分配数量改到ResourceSelect函数中处理
                        double iResReqQty = ListRouteRes[i].iResReqQty;

                        //按资源排产时，提示用户
                        try
                        {
                            DateTime ldtBeginDate = DateTime.Now;
                            ListRouteRes[i].TaskSchTask(ref iResReqQty, dCanBegDate);

                            DateTime ldtEndedDate = DateTime.Now;

                            Double iWaitTime2 = DateTime.Now.Subtract(ldtBeginDate).TotalMilliseconds;

                            TimeSpan interval = ldtEndedDate - ldtBeginDate;//计算间隔时间
                            //Console.WriteLine("{0} - {1} = {2}", ldtBeginDate, ldtEndedDate, interval.ToString());

                        }
                        catch (Exception error)
                        {
                            //System.Windows.Forms.Clipboard.SetText(this.ToString());
                            if (SchParam.iSchSdID < 1)
                                throw new Exception("资源任务正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                            else
                                throw new Exception("资源任务正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);

                            
                            return -1;
                        }
                        

                        //2020 - 01 - 07  JonasCheng 倒排容易出错，先关闭
                        //-------如果本工序计划完工日期，比上工序的完工日期早,而且本工序是非产能无限，则需要重排（产能无限资源则不需要重排） 2017-10-29--------------
                        //if(    ListRouteRes[i].dResEndDate )

                        if (SchProductRoutePreList != null && this.SchProductRoutePreList.Count > 0)
                        {
                            foreach (var schProductRoutePre in SchProductRoutePreList)
                            {
                                if (schProductRoutePre.bScheduled != 1) continue;
                          
                                //SchParam.iPreTaskRev // 后工序完工时间比前工序大多少小时前工序倒排 24 小时，太小了容易倒排出错  2020-01-09
                                //同一加工物料,后工序的计划完工日期小于前工序的计划完工日期，重排该工序时间
                                //if (SchParam.iPreTaskRev > 0 && SchProductRoutePreList[0].cInvCode == this.cInvCode && SchProductRoutePreList[0].dEndDate.Subtract(ListRouteRes[i].dResEndDate).TotalHours > SchParam.iPreTaskRev) //&&  ListRouteRes[i].resource.cIsInfinityAbility != "1"
                                //&& SchProductRoutePreList[0].cInvCode == this.cInvCode 物料编码可以不同
                                if (SchParam.iPreTaskRev > 0  && ListRouteRes[i].dResEndDate.Subtract(schProductRoutePre.dEndDate).TotalHours > SchParam.iPreTaskRev) //&&  ListRouteRes[i].resource.cIsInfinityAbility != "1"
                                {
                                    //区分当前任务资源是否无限产能，如果是无限产能，而且前工序为整批移转时,则直接修改本工序完工时间为前工序的结束时间 2017-11-26
                                    //if (ListRouteRes[i].resource.cIsInfinityAbility == "1" && ListRouteRes[i].schProductRoute.cMoveType == "0")
                                    //{
                                    //    //当前工序的完工时间为前工序的完工时间
                                    //    ListRouteRes[i].dResEndDate = SchProductRoutePreList[0].dEndDate;
                                    //}
                                    //else     //
                                    {

                                        // Int32 liReturn = SchData.GetDateDiff("h", ListRouteRes[i].dResEndDate, ListRouteRes[k].dResEndDate);

                                        //SchProductRoutePreList[i].dEndDate > ListRouteRes[i].dResEndDate

                                        //可排产日期 + 差异时间 
                                        Double iDiffMinites = SchData.GetDateDiff("m", schProductRoutePre.dEndDate, ListRouteRes[i].dResEndDate) - schProductRoutePre.iSeqPostTime - ListRouteRes[i].schProductRoute.iSeqPreTime ;
                                        //dCanBegDate =   ListRouteRes[i].dResBegDate.AddMinutes(iDiffMinites + 5);
                                        if (iDiffMinites > SchParam.iPreTaskRev * 60 )
                                        {

                                            //计算前工序的批量移转时间
                                            Double iMoveTime = this.GetProcessMoveQty(schProductRoutePre);

                                            //if (iMoveTime < iDiffMinites  ) iMoveTime = iDiffMinites ;


                                            //倒排日期，以前工序的完工日期 + 10分钟，进行倒排,不能用dCanBegDate
                                            //dCanBegDate = SchProductRoutePreList[0].dEndDate.AddMinutes(iMoveTime); //iDiffMinites

                                            //倒排日期，用后工序的完工日期 - 移转时间 - 工序前准备时间 分钟，进行倒排,不能用dCanBegDate 2023-10-17 
                                            //dCanBegDate = SchProductRoutePreList[0].dEndDate.AddMinutes(iMoveTime); //iDiffMinites
                                            dCanBegDate = ListRouteRes[i].dResEndDate.AddMinutes(-iMoveTime - schProductRoutePre.iSeqPostTime - ListRouteRes[i].schProductRoute.iSeqPreTime); //iDiffMinites
                                                                                                                                                                                              //清空之前排产
                                                                                                                                                                                            //schProductRoutePre..TaskClearTask();
                                            //清除本工序                                                                                                                                                  //取消当前工序的所排任务
                                            schProductRoutePre.ProcessClearTask() ;

                                            // 可排产时间dCanBegDate,重新排产
                                            //ListRouteRes[i].TaskSchTask(ref iResReqQty, dCanBegDate);

                                            //倒排前道工序 2020-01-10
                                            try
                                            {
                                                //正排时调用倒排 2023-03-06 
                                                //ListRouteRes[i].TaskSchTaskRev(ref iResReqQty, dCanBegDate,"2");
                                                schProductRoutePre.ProcessSchTaskRev("0");
                                            }
                                            catch (Exception error)
                                            {
                                                //System.Windows.Forms.Clipboard.SetText(this.ToString());


                                                if (SchParam.iSchSdID < 1)
                                                    throw new Exception("订单行号：" + this.iSchSdID + "资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                                                else
                                                    throw new Exception("订单行号：" + this.iSchSdID + "资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);


                                                return -1;
                                            }
                                        }
                                    }

                                }
                            }


                        }
                    }


                }

                //更新当前任务的计划开始时间，计划完工时间,计划数量大于0，可能有多资源的情况            
                List<SchProductRouteRes> list1 = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResReqQty > 0; });

                if (list1.Count > 0)
                {
                    list1.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    this.dBegDate = list1[0].dResBegDate;                 //取已排资源任务排产时间的最小开始时间
                    this.dEndDate = list1[list1.Count - 1].dResEndDate;   //取已排资源任务排产时间的最大结束时间
                }
                else     //
                {
                    //更新当前任务的计划开始时间，计划完工时间  ,增加 iResReqQty大于0条件         
                    List<SchProductRouteRes> list2 = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResReqQty > 0; });

                    if (list2.Count > 0)
                    {
                        list2.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        this.dBegDate = list2[0].dResBegDate;                 //取已排资源任务排产时间的最小开始时间
                        this.dEndDate = list2[list2.Count - 1].dResEndDate;   //取已排资源任务排产时间的最大结束时间
                    }
                }

                //取资源总的加工时间；前置时间，也应该取资源的前置准备时间
                int iLaborTime = 0;
                foreach (SchProductRouteRes lSchProductRouteRes in list1)
                {
                    iLaborTime += Convert.ToInt32(lSchProductRouteRes.iResRationHour);
                }

                this.iLaborTime = iLaborTime;

               

                //更新工序状态为已排
                this.BScheduled = 1;    //0 未排 1 已排

                ////记录排产日志
                //if (SchParam.APSDebug == "1")
                //{
                //    string message = string.Format(@"2、物料编号[{0}],计划ID[{1}],任务ID[{2}],工序号[{3}],工艺编号[{4}],计划开工时间[{5}],计划完工时间[{6}],工单号[{7}]",
                //                                            this.cInvCode, this.iSchSdID, this.iProcessProductID, this.iWoSeqID, this.cTechNo + this.cSeqNote, this.dBegDate, this.dEndDate,this.cWoNo);
                //    SchParam.Debug(message, "SchProductRoute.ProcessSchTask工序排产完成");
                //}

                //按托盘排产，同时排同一托盘其他工序
                //当前工序设置为已排产,而且托盘号不为空,cBatchNoFlag 0 托盘资源选择未处理 1 托盘资源选择已处理 ,而且有多个资源可以选择时调用
                if (this.schProduct.cBatchNo != "" && this.cBatchNoFlag == 0  )
                    cBatchSchRoute();
            }
            catch (Exception error)
            {
                //System.Windows.Forms.Clipboard.SetText(error.StackTrace);

                if (SchParam.iSchSdID < 1)
                    throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                else
                    throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                               
                return -1;
            }

            return 1;

        }

        //资源选择,cCanScheduled = '1',先找出最早可开工日期的工序
        public int ResourceSelect(DateTime dCanBegDate,Boolean bFreeze = false)
        {
            //已下达生产任务单的，资源不用换 2014-11-7
            //if (this.iSchBatch <= 1) return 1;
            //if (this.cWoNo != "") return 1;

            

            ////找出所有选择可用的机台
            List<SchProductRouteRes> ListRouteRes = this.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });

            //调试
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
            }

            int iRouteResCountFirst = ListRouteRes.Count;

            if (iRouteResCountFirst < 1)
            {
                //System.Windows.Forms.Clipboard.SetText(error.StackTrace);
                throw new Exception(string.Format("多资源选择正排出错,订单行号：{0}，没有找到已选择的可排产资源，资源产能明细总资源数量为{1},位置SchProductRoute.ProcessSchTask！工序ID号：{2},物料编号{3}", this.iSchSdID.ToString(), SchProductRouteResList.Count.ToString(),this.cInvCode));
                return -1;
            }


            //冻结处理，还是选择原来的资源
            if (bFreeze || this.iActQty > 0)  //|| this.cWoNo != "" 冻结状态 或者完工数量大于0 ,已开工生产的，不能换资源
            {
                // 待排产数量大于0 
                if (this.iReqQty > 0)
                {
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        //调用SchProductRouteRes.TaskSchTask,加工时间重新计算
                        if (ListRouteRes[i].iResReqQty == 0)
                        {
                            ListRouteRes[i].BScheduled = 1;        //状态改为已排产      2021-03-20 JonasCheng       
                            ListRouteRes[i].cCanScheduled = "0";   //不排产                                 
                        }
                    }
                }


                return 1;
            }
            else   //计划状态任务排产，1、如果加工物料相同或模具号相同，排在同一台设备生产 2020-06-22
            {
                
                //多资源排产，重新设备资源优先级 ListRouteRes[i].iResGroupPriority
                if (ListRouteRes.Count > 1 )//(this.SchProductRouteResList.Count > 1 )  //多资源排产
                {
                    //根据前工序已排产选择的资源编号(有关联分组号的)，当前工序资源也设置了关联分组号，并且分组号相同或包含前工序分组。2022-11-02 
                    //取前工序关联分组号 cDayPlanShowType ,前工序只有一道。 
                    if (this.SchProductRoutePreList.Count == 1 && this.SchProductRoutePreList[0].cInvCode == this.cInvCode && this.SchProductRoutePreList[0].SchProductRouteResList[0].resource.cDayPlanShowType != "")
                    {
                        var schProductRouteRes = this.SchProductRoutePreList[0].SchProductRouteResList.Find(item => item.BScheduled == 1 && item.iResReqQty > 0 && item.resource.cDayPlanShowType != "");

                        if (schProductRouteRes != null)
                        {
                            foreach (var item in this.SchProductRouteResList)
                            {
                                //后续工序非本关联资源组的，不能选中排产;如果本资源没有设定分组，可以选择 点检工序： item.resource.cDayPlanShowType 1;2;3;4  schProductRouteRes.resource.cDayPlanShowType： 1;3 匹配不上？
                                if (item.resource.cDayPlanShowType != "" && !item.resource.cDayPlanShowType.Contains(schProductRouteRes.resource.cDayPlanShowType) && item.cSelected == "1")
                                {
                                    item.cSelected = "0";  //取消选择
                                    item.iResReqQty = 0;   //排程数量为0  排程数量不要取消，以免报错
                                    item.cDefine25 = "关联分组号不同取消:" + schProductRouteRes.resource.cDayPlanShowType;  //取消选择备注
                                }
                            }
                        }
                        //特殊处理，如果没有选到资源，则选第1个资源 //如果没有找到排产资源，则取第一行明细排产，有可能关联分组号不同，取消所有的任务。 2023-05-23 
                        List<SchProductRouteRes> ListRouteRes2 = this.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty > 0 ; });
                        if (ListRouteRes2.Count < 1)
                        {
                            ListRouteRes[0].cSelected = "1";
                            ListRouteRes[0].iResReqQty = this.iReqQty;   //排程数量
                        }

                    }


                    ////找出当前产品当前工序最近加工过的资源清单排序。
                    //资源最近多少天内生产的任务，按生产日期倒排，用于多资源选择，必须选择相同资源排产。  
                    //加工物料相同/或者模具号相同(同一模具同时加工出的不同零件)，已排产 

                    //同模具选相同的资源，在资源优化时处理 2023-03-24
                    ////多资源排产优先选择最近多少天内生产过的资源,7天 ,0 不考虑
                    //if (SchParam.ResSelectLastTaskDays > 0)
                    //{
                    //    //找未完工的已排产任务 &&  p.cWoNo != "" 
                    //    List<SchProductRouteRes> ResLastTaskIDList = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p)
                    //    {
                    //        return p.schProductRoute != null && p.schProductRoute.item != null && p.iResReqQty > 0 && p.cWoNo != "" && p.schProductRoute.cStatus != "4"
                    //        && (p.cInvCode == this.cWorkItemNo || (p.schProductRoute.item.cMoldNo == this.item.cMoldNo && this.item.cMoldNo != ""))
                    //        && p.iWoSeqID == this.iWoSeqID && p.BScheduled == 1 && p.dResEndDate >= dCanBegDate.AddDays(-SchParam.ResSelectLastTaskDays)
                    //        //&& (p.iProcessProductID != this.iProcessProductID && p.iSchSdID != this.iSchSdID)   //非当前工序
                    //        ;
                    //    });

                    //    if (ResLastTaskIDList != null && ResLastTaskIDList.Count > 0)
                    //    {
                    //        //必须要排序,按完工日期倒排     2020-06-10      
                    //        ResLastTaskIDList.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p2.dResEndDate, p1.dResEndDate); });

                    //        try
                    //        {
                    //            if (ResLastTaskIDList.Count > 0)  //最近有排产,重新设置可选资源的优先级
                    //            {

                    //                List<string> cResourceNoList = new List<string>(10);
                    //                double iResGroupPriority = 0.1;   //重新设置优先级

                    //                SchProductRouteRes ResCurTask = this.SchProductRouteResList.Find(delegate (SchProductRouteRes p) { return p.cResourceNo == ResLastTaskIDList[0].cResourceNo && p.cSelected == "1"; });
                    //                if (ResCurTask != null)
                    //                {
                    //                    //设置优先级，反正只有一台设备可排产。
                    //                    ResCurTask.iResGroupPriority = iResGroupPriority;

                    //                    //只有一套模具
                    //                    if (this.item != null && this.item.iMoldCount <= 1)
                    //                    {
                    //                        //加工物料相同或模具号相同，只能排在同一台设备上，其他资源可排属性设置为0 
                    //                        //for (int i = 0; i < this.SchProductRouteResList.Count; i++)
                    //                        for (int i = 0; i < ListRouteRes.Count; i++)
                    //                        {
                    //                            //if (this.SchProductRouteResList[i].cResourceNo != ResLastTaskIDList[0].cResourceNo)
                    //                            if (ListRouteRes[i].cResourceNo != ResLastTaskIDList[0].cResourceNo)
                    //                            {
                    //                                ListRouteRes[i].cCanScheduled = "0";   //不参与排产    
                    //                                ListRouteRes[i].iResReqQty = 0;
                    //                            }
                    //                            else
                    //                            {
                    //                                ListRouteRes[i].cCanScheduled = "1";   //参与排产

                    //                                ListRouteRes[i].cDefine27 = "资源选择一套模，加工物料相同或模具号相同,优先级" + iResGroupPriority.ToString() + "资源编号：" + ListRouteRes[i].cResourceNo + "模具号:" + this.item.cMoldNo + "工单号:" + ResCurTask.cWoNo + "原ID:" + ResCurTask.iProcessProductID.ToString();
                    //                            }
                    //                        }
                    //                    }
                    //                    else   //如果有多套模具时，可以选择最近生产的多台设备，同时不取消原来的资源。 
                    //                    {
                    //                        int iRowCount = ResLastTaskIDList.Count;
                    //                        for (int i = 0; i < iRowCount; i++)
                    //                        {
                    //                            //当前资源设置过优先级后，不用再处理
                    //                            if (cResourceNoList.IndexOf(ResLastTaskIDList[i].cResourceNo) > 0)
                    //                                continue;

                    //                            //设置资源优先级
                    //                            ResCurTask = this.SchProductRouteResList.Find(delegate (SchProductRouteRes p) { return p.cResourceNo == ResLastTaskIDList[i].cResourceNo; });
                    //                            if (ResCurTask != null)
                    //                            {
                    //                                ResCurTask.iResGroupPriority = iResGroupPriority;
                    //                                ResCurTask.cDefine27 = "同一产品有多套模具生产，选择最近生产的多台设备,优先级" + iResGroupPriority.ToString() + "资源编号：" + ResCurTask.cResourceNo + "模具号:" + this.item.cMoldNo + "工单号:" + ResCurTask.cWoNo + "原ID:" + ResCurTask.iProcessProductID.ToString();
                    //                            }
                    //                            else  //把第1行资源编号改成最优资源编号
                    //                            {
                    //                                this.SchProductRouteResList[0].cResourceNo = ResLastTaskIDList[i].cResourceNo;
                    //                                this.SchProductRouteResList[0].iResGroupPriority = iResGroupPriority;

                    //                                this.SchProductRouteResList[0].cDefine27 = "同一产品有多套模具生产,第一行特殊处理,优先级：" + iResGroupPriority.ToString() + "资源编号：" + this.SchProductRouteResList[0].cResourceNo + "模具号:" + this.item.cMoldNo;

                    //                            }
                    //                            //记录当前资源
                    //                            cResourceNoList.Add(ResLastTaskIDList[i].cResourceNo);
                    //                            iResGroupPriority = iResGroupPriority + 0.1;  //每次加0.1


                    //                        }
                    //                    }

                    //                }

                    //            }
                    //        }
                    //        catch (Exception error)
                    //        {
                    //            //System.Windows.Forms.Clipboard.SetText(this.ToString());

                    //            if (SchParam.iSchSdID < 1)
                    //                throw new Exception("资源选择物料最近排产模具或资源时出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：" + this.iProcessProductID + "资源编号:" + ResLastTaskIDList[0].cResourceNo.ToString() + "\n\r " + error.Message.ToString());
                    //            else
                    //                throw new Exception("资源选择物料最近排产模具或资源时出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：" + this.iProcessProductID + "资源编号:" + ResLastTaskIDList[0].cResourceNo.ToString() + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);


                    //            return -1;
                    //        }
                    //    }
                    //}
                }
            }


            //根据工序最大机台数量,如2台，选择最优的两台资源进行排产,ListRouteRes只留两台cSelected的 2013-10-21
            //TestResSchTask(ref double ai_ResReqQty, DateTime adCanBegDate)

            //重新取可排任务列表,因为上面重新选择了资源。2022-11-06
            ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });

            //按选择的机台数平均分配生产任务
            int iResCount = ListRouteRes.Count;

            //如果排程资源数为，不用选择了
            if (iResCount <= 1) return 1;

            if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1 )
                iResCount = this.iDevCountPd;

            //iTaskMinWorkTime 每台设备最小工作时间
            //最最快单件工时，测算需要多少设备
            
            //先按单件工时排序
            ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.iCapacity/p1.iBatchQty, p2.iCapacity/p1.iBatchQty); });
            
            double iCapacity = ListRouteRes[0].iCapacity;
            double iBatchQty = ListRouteRes[0].iBatchQty;

            //1、最小需要设备数量480分钟，超过1天才分给其他设备生产
            int iMinResCount = 1;        

            ////先按资源选配比例，对有些资源进行排除.在排程接口时处理

            //2、设置了产品日排产数量，根据产品日排产数量设置半成品的日排产资源数 2019-03-24 ，每有设置资源选择表达式
            if (ListRouteRes.Count > 1)  //this.cDevCountPdExp.Length < 1  &&  this.iDevCountPd < 1
            {                
                if (this.schProduct.iWorkQtyPd > 0 && this.schProduct.iWorkQtyPd < this.iReqQty )
                {
                    double iDevCount = this.schProduct.iWorkQtyPd * iCapacity  / 3600 / ListRouteRes[0].resource.iResHoursPd / ListRouteRes[0].resource.iResourceNumber / ListRouteRes[0].iBatchQty;

                    //物料档案加工难度系数
                    if (this.item != null && this.item.iItemDifficulty > 0 && this.item.iItemDifficulty != 1 ) iDevCount = iDevCount * this.item.iItemDifficulty;

                    //资源档案加工难度系数
                    if (ListRouteRes[0].resource.iResDifficulty > 0 && ListRouteRes[0].resource.iResDifficulty != 1) iDevCount = iDevCount * ListRouteRes[0].resource.iResDifficulty;

                    //工艺档案加工难度系数
                    if (this.techInfo.iTechDifficulty > 0 && this.techInfo.iTechDifficulty != 1) iDevCount = iDevCount * this.techInfo.iTechDifficulty;

                       
                   // iMinResCount = Convert.ToInt32(Math.Ceiling(this.schProduct.iWorkQtyPd * iCapacity / 3600 / ListRouteRes[0].resource.iResHoursPd));

                    iMinResCount = Convert.ToInt32(Math.Floor(iDevCount)); //Ceiling
                    //if (iResCount < iMinResCount) iResCount = iMinResCount;
                    iResCount = iMinResCount;

                }
                else
                {
                    //如果设置为0，则不考虑自动计算多机台,每资源最少工作多少小时
                    //以资源最下加工时间为准，可以每类资源都设置不同值
                    //if (SchParam.iTaskMinWorkTime > 0)
                    if (ListRouteRes[0].resource.iMinWorkTime > 0 )  
                        iMinResCount = Convert.ToInt32(this.iReqQty * iCapacity/ iBatchQty / 3600 / ListRouteRes[0].resource.iMinWorkTime);  //SchParam.iTaskMinWorkTime
                    else
                        iMinResCount = iResCount;
                }
            }
            

            if (iResCount > iMinResCount && iMinResCount <= ListRouteRes.Count)
                iResCount = iMinResCount;
            if (iResCount < 1 && ListRouteRes.Count > 0) iResCount = 1;

            //3、调用客户扩展属性工序排产数
            //iResCount = schData.algorithmExtend.GetRouteResourceCount(this, iResCount);
            //if (iResCount < 1) iResCount = 1;

            //考虑物料档案设置的模具套数iMoldCount,排产资源数最大不能超过模具套数
            if (this.item != null && this.item.iMoldCount > 0 && iResCount > this.item.iMoldCount)
                iResCount = this.item.iMoldCount;


         

            double iResReqQtyPer = this.iReqQty ;  //小数取整，尾差留到最后一批

            //分多个机台排产
            if (iResCount > 1)
            {
                iResReqQtyPer = (int)this.iReqQty / iResCount;  //小数取整，尾差留到最后一批
            
            }


            double iLeftReqQty = this.iReqQty;
            double iResReqQty = iResReqQtyPer;

            //4 按资源已排程天数选择，越少越优先,不用每个任务模拟排产一遍，节省运算量 2022-11-06
            if (SchParam.cMutResourceType != "4")  
            {
                try
                {
                    //测试排产，计算每个机台的计划开工时间，计划完工时间    
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        if (ListRouteRes[i].cCanScheduled != "1") continue;
                        //调用SchProductRouteRes.TaskSchTask,加工时间重新计算
                        ListRouteRes[i].TestResSchTask(ref iResReqQty, dCanBegDate);
                    }
                }
                catch (Exception error)
                {
                    //System.Windows.Forms.Clipboard.SetText(this.ToString());
                    if (SchParam.iSchSdID < 1)
                        throw new Exception("资源选择正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                    else
                        throw new Exception("资源选择正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);


                    return -1;
                }
            }

            //排序,找出最早开工资源或最早完工资源
            //List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" ; });

            int iSelectReturn = 0;
            //int iSelectReturn = schData.algorithmExtend.GetRouteResourceDetail(this, iResCount, ListRouteRes, SchParam.cMutResourceType);
            //if (iSelectReturn > 0  ) //子类已经选择好资源明细
            //{
            //    //选择排产资源明细是否与iResCount相等
            //    List<SchProductRouteRes> ListRouteResSelect = ListRouteRes.FindAll(delegate(SchProductRouteRes p) { return p.cCanScheduled == "1" ; });
            //    if (ListRouteResSelect.Count != iResCount) iSelectReturn = -1 ;

            //}

            //否则用系统统一规则选择资源
            if (iSelectReturn <= 0)
            {
                // //多资源选择规则 “1”最早可完工日期资源优先选择，"2" 最早可开工日期资源优先选择  
                //“3” 按资源组排产优先级选择资源,在所选资源排不下时，选择下个优先级资源
                //
                if (SchParam.cMutResourceType == "4")  //4 按资源已排程天数选择，越少越优先 2022-11-06
                {
                    ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.resource.iPlanDays, p2.resource.iPlanDays); });

                }
                else if (SchParam.cMutResourceType == "3" )  //3 按资源组排产优先级选择资源,在所选资源排不下时，选择下个优先级资源 
                {
                    //ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iResGroupPriority, p2.iResGroupPriority); });

                    if (ListRouteRes.Count > 1)
                    {

                        //每有优先级相差iMutResourceDiffHour(48小时)
                        for (int i = 0; i < ListRouteRes.Count; i++)
                        {
                            if (ListRouteRes[i].cCanScheduled != "1") continue;

                            //资源优先级考虑 + 1
                            ListRouteRes[i].cDefine38 = ListRouteRes[i].dResEndDate.AddHours((ListRouteRes[i].iResGroupPriority + 1 - 1) * SchParam.iMutResourceDiffHour);

                        }



                        //按资源优先级，排产完工时间排序 + 优先级别差异工时，每格一个级别相差iMutResourceDiffHour（48小时）
                        //List<SchProductRouteRes>  ListRouteResSort = (from c in ListRouteRes
                        //                                              orderby c.iResGroupPriority ascending, c.dResEndDate ascending //descending  
                        //                                          select c).ToList<SchProductRouteRes>();

                        //优先级转化成最早晚工日期,晚工日期相同，再考虑优先级
                        List<SchProductRouteRes> ListRouteResSort = (from c in ListRouteRes
                                                                     orderby c.cDefine38 ascending, c.iResGroupPriority ascending //descending  
                                                                     select c).ToList<SchProductRouteRes>();

                        ListRouteRes = ListRouteResSort;

                        
                        ////
                        //for (int i = 0; i < iResCount; i++)
                        //{
                        //    //只考虑前面几个资源                          

                        //    //对比优先排产的资源，排程完工日期是否差异很大，如果很大则当前任务取消排产
                        //    if (ListRouteRes[i].cCanScheduled != "0")
                        //    {
                        //        for (int k = iResCount; k < ListRouteRes.Count; k++)
                        //        {
                        //            ////当前资源
                        //            //TimeSpan ts1 = new TimeSpan(ListRouteRes[i].dResEndDate.Ticks);

                        //            ////替补资源
                        //            //TimeSpan ts2 = new TimeSpan(ListRouteRes[k].dResEndDate.Ticks);

                        //            //TimeSpan ts=ts1.Subtract(ts2).Duration();

                        //            //如果当前资源比后备资源完工时间晚了48小时，则当前资源不排
                        //            //if (ts.Days * 24 + ts.Hours >= SchParam.iMutResourceDiffHour)
                        //            Int32 liReturn = SchData.GetDateDiff("h", ListRouteRes[i].dResEndDate, ListRouteRes[k].dResEndDate);



                        //            if (liReturn >= SchParam.iMutResourceDiffHour && SchParam.iMutResourceDiffHour > 0 )
                        //            {
                        //                ListRouteRes[i].cCanScheduled = "0";   //不排产 
                        //                ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                        //                ListRouteRes[i].iResRationHour = 0;    //加工工时为0 

                        //                ListRouteRes[k].cCanScheduled = "1";   //参与排产，替代优先级高的任务 
                        //                break;
                        //            }
                        //            else
                        //            {
                        //                ListRouteRes[k].cCanScheduled = "0";   //不排产 
                        //                ListRouteRes[k].iResReqQty = 0;        //加工数量为0   
                        //                ListRouteRes[k].iResRationHour = 0;    //加工工时为0 
                        //            }


                        //        }
                        //    }


                        //}
                        //}
                    }
                    
                }
                else if (SchParam.cMutResourceType == "2") //最早可完工日期资源优先选择
                {
                    ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    //ListRouteResSort = ListRouteRes;
                }
                else
                {
                    // if (SchParam.cMutResourceType == "1" )  //最早可完工日期资源优先选择
                    ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResEndDate, p2.dResEndDate); });
                    //ListRouteResSort = ListRouteRes;
                }

                //相同资源编号，对应模具不同，可以定义两行记录。
                //资源选择时，相同资源编号只能选择一次 2021-11-01 JonasCheng 
                List<string> ResSelectList = new List<string>();

                //模具编号唯一，cViceResource1No辅助资源编号1，同一模具不能选择多个资源同时加工 2021-11-04 JonasCheng 
                //正式版本不用选择资源
                List<string> MoldSelectList = new List<string>();

                if (ListRouteRes.Count > 1)  //&& (this.cVersionNo.ToLower()) != "sureversion" 
                {
                    int j = 0;
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        if (ListRouteRes[i].cCanScheduled != "1") continue;

                        //ResSelectList.Add(ListRouteRes[i].c)

                        if (j <= iResCount)
                        {
                            if (ResSelectList.Count > 0)
                            {
                                string ResSelect = ResSelectList.Find(delegate (string s) { return s.Equals(ListRouteRes[i].cResourceNo); });

                                //之前已经选择过的资源，不能选
                                if (ResSelectList.Count > 0 && ResSelect != null && ResSelect != "")
                                {
                                    ListRouteRes[i].cCanScheduled = "0";   //不排产 
                                    ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                                    ListRouteRes[i].iResRationHour = 0;    //加工工时为0 

                                    continue;
                                }

                            }

                            ////设置了模具资源编号，而且没有用过的，可以排产
                            //if (MoldSelectList.Count > 0 && ListRouteRes[i].cViceResource1No != null && ListRouteRes[i].cViceResource1No != "")
                            //{
                            //    string MoldSelect = MoldSelectList.Find(delegate (string s) { return s.Equals(ListRouteRes[i].cViceResource1No); });

                            //    //之前已经选择过的资源，不能选
                            //    if (MoldSelectList.Count > 0 && MoldSelect != null && MoldSelect != "")
                            //    {
                            //        ListRouteRes[i].cCanScheduled = "0";   //不排产 
                            //        ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                            //        ListRouteRes[i].iResRationHour = 0;    //加工工时为0 

                            //        continue;
                            //    }

                            //    //用过的模具编号
                            //    MoldSelectList.Add(ListRouteRes[i].cViceResource1No);
                            //}

                            //增加已选择的资源
                            ResSelectList.Add(ListRouteRes[i].cResourceNo);
                        }
                       
                        //可用选择资源加1
                        j++;

                        //多余的设备,都不排产
                        if (j > iResCount)
                        {
                            ListRouteRes[i].cCanScheduled = "0";   //不排产 
                            ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                            ListRouteRes[i].iResRationHour = 0;    //加工工时为0 
                        }

                    }
                }
            }

            ////找出所有选择可用的机台,可排下的资源
            ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });

            //按选择的机台数平均分配生产任务 iRouteResCountFirst 初始选择可排资源数量为
            iResCount = ListRouteRes.Count;
            if (iResCount < 1)  //没有可用资源
            {
                //System.Windows.Forms.Clipboard.SetText(this.ToString());
                throw new Exception(string.Format("多资源选择正排出错,订单行号：{0}，没有找到已选择的可排产资源，资源产能明细总资源数量为{1},初始选择可排资源数量为{2}，位置SchProductRoute.ProcessSchTask！工序ID号：{3},物料编号{4}",this.iSchSdID.ToString(),iRouteResCountFirst.ToString(),this.SchProductRouteResList.Count.ToString(), this.cInvCode));
                return -1;

            }

            //如果有多任务排产时,分配每行排程任务数量
            ResReqQtyDispatch();

            return 1;
        }

        //多资源排产时，各资源排产数量分配 2023-06-03 
        public int ResReqQtyDispatch()
        {
            ////找出所有选择可用的机台,可排下的资源
            List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });

            //按选择的机台数平均分配生产任务
            int iResCount = ListRouteRes.Count;

            //如果只有一个资源可排产，不用分配数量
            if (iResCount <= 1) return 0;

            double iResReqQtyPer = (int)(this.iReqQty - this.iActQty) / iResCount;  //小数取整，尾差留到最后一批
            double iLeftReqQty = this.iReqQty - this.iActQty;
            double iResReqQty = iResReqQtyPer;


            //2016-06-06
            if (iResReqQtyPer < 1)
            {
                //iResCount = (int)iLeftReqQty; //this.iReqQty;
                //iResReqQtyPer = 1;
                iResCount = 1;
                iResReqQtyPer = (this.iReqQty - this.iActQty);
            }

            
            //循环分配每行任务的分配数量
            for (int i = 0; i < ListRouteRes.Count; i++)
            {
                //
                if (iLeftReqQty <= 0) continue;
           

                //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                if (i == ListRouteRes.Count)
                    iResReqQty = iLeftReqQty;
                else
                {
                    //如果最小加工批量大于1，取整,有多资源排产
                    if (ListRouteRes[i].iBatchQtyBase > 1 && ListRouteRes.Count > 1)
                    {
                        iResReqQty = Math.Ceiling(iResReqQtyPer / ListRouteRes[i].iBatchQtyBase) * ListRouteRes[i].iBatchQtyBase;
                    }
                    else
                    {
                        iResReqQty = iResReqQtyPer;
                    }
                }

                //记录每排产一个机台后，剩余的计划数量
                if (iLeftReqQty - iResReqQty > 0)
                {
                    iLeftReqQty = iLeftReqQty - iResReqQty;
                }
                else
                {
                    //最后一台机，剩下的数量都排给他
                    iResReqQty = iLeftReqQty;
                    iLeftReqQty = 0;
                }

                //调用SchProductRouteRes.TaskSchTask,加工时间重新计算
                if (this.iSchBatch == 1) //已执行生产任务单,资源计划数量不变,已减去已完工数量 2014-11-04
                {
                    iResReqQty = ListRouteRes[i].iResReqQty; //- ListRouteRes[i].iActResReqQty;                       
                }


                //最后一个资源排产时，剩余尾数数量全部排在这台
                if (iLeftReqQty > 0 && i == ListRouteRes.Count - 1)
                {
                    iResReqQty += iLeftReqQty;
                }

                //更新当前任务的已排产数量 
                ListRouteRes[i].iResReqQty = iResReqQty;
            }

            return 1;
        }
        //倒排资源选择,cCanScheduled = '1',先找出最早可开工日期的工序
        public int ResourceSelectRev(DateTime dCanBegDate, Boolean bFreeze = false)
        {
            //已下达生产任务单的，资源不用换 2014-11-7
            //if (this.iSchBatch <= 1) return 1;
            //if (this.cWoNo != "") return 1;


            ////找出所有选择可用的机台
            List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });


            //冻结处理，还是选择原来的资源
            if (bFreeze || this.iActQty > 0)  //|| this.cWoNo != "" 冻结状态 或者完工数量大于0 ,已开工生产的，不能换资源
            {
                for (int i = 0; i < ListRouteRes.Count; i++)
                {
                    //调用SchProductRouteRes.TaskSchTask,加工时间重新计算
                    if (ListRouteRes[i].iResReqQty == 0)
                    {
                        ListRouteRes[i].BScheduled = 1;        //标记为已排产 2021-03-20 JonasCheng  
                        ListRouteRes[i].cCanScheduled = "0";   //不排产        
                    }
                }

                return 1;
            }
            else   //非开工和冻结状态
            {
                //根据前工序已排产选择的资源编号(有关联分组号的)，当前工序资源也设置了关联分组号，并且分组号相同或包含前工序分组。2022-11-02 
                //取前工序关联分组号 cDayPlanShowType ,前工序只有一道。
                if (ListRouteRes.Count > 0)   //有多道选择工序
                {
                    if (this.SchProductRouteNextList.Count == 1 && this.SchProductRouteNextList[0].cInvCode == this.cInvCode)
                    {
                        var schProductRouteRes = this.SchProductRouteNextList[0].SchProductRouteResList.Find(item => item.BScheduled == 1 && item.iResReqQty > 0 && item.resource.cDayPlanShowType != "");

                        if (schProductRouteRes != null)
                        {
                            foreach (var item in this.SchProductRouteResList)
                            {
                                //后续工序非本关联资源组的，不能选中排产;如果本资源没有设定分组，可以选择
                                if (item.resource.cDayPlanShowType != "" && !(schProductRouteRes.resource.cDayPlanShowType.Contains(item.resource.cDayPlanShowType) || item.resource.cDayPlanShowType.Contains(schProductRouteRes.resource.cDayPlanShowType)) && item.cSelected == "1")
                                {
                                    item.cSelected = "0";  //取消选择
                                    item.iResReqQty = 0;   //排程数量为0
                                    item.cDefine25 = "关联分组号不同取消:" + schProductRouteRes.resource.cDayPlanShowType;  //取消选择备注
                                }
                            }

                            //特殊处理，如果没有选到资源，则选第1个资源
                            List<SchProductRouteRes> ListRouteRes2 = this.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty > 0 ; });
                            if (ListRouteRes2.Count < 1)
                            {
                                ListRouteRes[0].cSelected = "1";
                                ListRouteRes[0].iResReqQty = this.iReqQty;   //排程数量
                            }

                        }

                    }
                }
            }

            //调试
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
            }


            //根据工序最大机台数量,如2台，选择最优的两台资源进行排产,ListRouteRes只留两台cSelected的 2013-10-21
            //TestResSchTask(ref double ai_ResReqQty, DateTime adCanBegDate)



            //按选择的机台数平均分配生产任务
            int iResCount = ListRouteRes.Count;

            //如果排程资源数为，不用选择了
            if (iResCount <= 1) return 1;

            if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1)
                iResCount = this.iDevCountPd;

            //iTaskMinWorkTime 每台设备最小工作时间
            //最最快单件工时，测算需要多少设备

            //先按单件工时排序
            ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.iCapacity, p2.iCapacity); });

            double iCapacity = ListRouteRes[0].iCapacity;

            //1、最小需要设备数量480分钟，超过1天才分给其他设备生产
            int iMinResCount = 1;

            ////先按资源选配比例，对有些资源进行排除.在排程接口时处理

            //2、设置了产品日排产数量，根据产品日排产数量设置半成品的日排产资源数 2019-03-24 ，每有设置资源选择表达式
            if (ListRouteRes.Count > 1 && this.cDevCountPdExp.Length < 1)
            {
                if (this.schProduct.iWorkQtyPd > 0 && this.schProduct.iWorkQtyPd < this.iReqQty)
                {
                    //日排产资源数 = 日排产数量 * 单件工时/3600/日排产工时(24小时)/资源数/批次加工数量
                    double iDevCount = this.schProduct.iWorkQtyPd * iCapacity / 3600 / ListRouteRes[0].resource.iResHoursPd / ListRouteRes[0].resource.iResourceNumber/ ListRouteRes[0].iBatchQty;

                    //物料档案加工难度系数
                    if (this.item != null && this.item.iItemDifficulty > 0 && this.item.iItemDifficulty != 1) iDevCount = iDevCount * this.item.iItemDifficulty;

                    //资源档案加工难度系数
                    if (ListRouteRes[0].resource.iResDifficulty > 0 && ListRouteRes[0].resource.iResDifficulty != 1) iDevCount = iDevCount * ListRouteRes[0].resource.iResDifficulty;

                    //工艺档案加工难度系数
                    if (this.techInfo.iTechDifficulty > 0 && this.techInfo.iTechDifficulty != 1) iDevCount = iDevCount * this.techInfo.iTechDifficulty;


                    // iMinResCount = Convert.ToInt32(Math.Ceiling(this.schProduct.iWorkQtyPd * iCapacity / 3600 / ListRouteRes[0].resource.iResHoursPd));

                    iMinResCount = Convert.ToInt32(Math.Floor(iDevCount));  //Ceiling
                    //if (iResCount < iMinResCount) iResCount = iMinResCount;
                    iResCount = iMinResCount;

                }
                else
                {
                    //如果设置为0，则不考虑自动计算多机台,每资源最少工作多少小时
                    if (SchParam.iTaskMinWorkTime > 0)
                        iMinResCount = Convert.ToInt32(this.iReqQty * iCapacity / 3600 / SchParam.iTaskMinWorkTime);
                    else
                        iMinResCount = iResCount;
                }
            }


            if (iResCount > iMinResCount && iMinResCount <= ListRouteRes.Count)
                iResCount = iMinResCount;
            if (iResCount < 1 && ListRouteRes.Count > 0) iResCount = 1;

            ////3、调用客户扩展属性工序排产数
            //iResCount = schData.algorithmExtend.GetRouteResourceCount(this, iResCount);
            //if (iResCount < 1) iResCount = 1;
                     

          
            double iResReqQty = this.iReqQty; 


            //倒排暂时不进行测试排产。2020-04-09
            ////测试排产，计算每个机台的计划开工时间，计划完工时间    
            //for (int i = 0; i < ListRouteRes.Count; i++)
            //{
            //    if (ListRouteRes[i].cCanScheduled != "1") continue;
            //    //调用SchProductRouteRes.TaskSchTask,加工时间重新计算
            //    ListRouteRes[i].TestResSchTask(ref iResReqQty, dCanBegDate);
            //}

            //排序,找出最早开工资源或最早完工资源
            //List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" ; });

            int iSelectReturn = 0;
            //int iSelectReturn = schData.algorithmExtend.GetRouteResourceDetail(this, iResCount, ListRouteRes, SchParam.cMutResourceType);
            //if (iSelectReturn > 0) //子类已经选择好资源明细
            //{
            //    //选择排产资源明细是否与iResCount相等
            //    List<SchProductRouteRes> ListRouteResSelect = ListRouteRes.FindAll(delegate (SchProductRouteRes p) { return p.cCanScheduled == "1"; });
            //    if (ListRouteResSelect.Count != iResCount) iSelectReturn = -1;

            //}

            //否则用系统统一规则选择资源
            if (iSelectReturn <= 0)
            {
                // //多资源选择规则 “1”最早可完工日期资源优先选择，"2" 最早可开工日期资源优先选择  
                //“3” 按资源组排产优先级选择资源,在所选资源排不下时，选择下个优先级资源                 

                //倒排只能用资源优先级方式选择资源
                //if (SchParam.cMutResourceType == "3")  //3 按资源组排产优先级选择资源,在所选资源排不下时，选择下个优先级资源 
                {
                    //ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iResGroupPriority, p2.iResGroupPriority); });

                    if (ListRouteRes.Count > 1)
                    {

                        if (SchParam.cMutResourceType == "4")  //4 按资源已排程天数选择，越少越优先 
                        {
                            ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.resource.iPlanDays, p2.resource.iPlanDays); });

                        }
                        else                                    // 按资源优先级排产
                        {
                            //每有优先级相差iMutResourceDiffHour(48小时)
                            for (int i = 0; i < ListRouteRes.Count; i++)
                            {
                                if (ListRouteRes[i].cCanScheduled != "1") continue;

                                ListRouteRes[i].cDefine38 = ListRouteRes[i].dResEndDate.AddHours((ListRouteRes[i].iResGroupPriority - 1) * SchParam.iMutResourceDiffHour);

                            }



                            //按资源优先级，排产完工时间排序 + 优先级别差异工时，每格一个级别相差iMutResourceDiffHour（48小时）
                            //List<SchProductRouteRes>  ListRouteResSort = (from c in ListRouteRes
                            //                                              orderby c.iResGroupPriority ascending, c.dResEndDate ascending //descending  
                            //                                          select c).ToList<SchProductRouteRes>();

                            //优先级转化成最早晚工日期,晚工日期相同，再考虑优先级
                            List<SchProductRouteRes> ListRouteResSort = (from c in ListRouteRes
                                                                         orderby c.cDefine38 ascending, c.iResGroupPriority ascending //descending  
                                                                         select c).ToList<SchProductRouteRes>();

                            ListRouteRes = ListRouteResSort;
                        }


                        ////
                        //for (int i = 0; i < iResCount; i++)
                        //{
                        //    //只考虑前面几个资源                          

                        //    //对比优先排产的资源，排程完工日期是否差异很大，如果很大则当前任务取消排产
                        //    if (ListRouteRes[i].cCanScheduled != "0")
                        //    {
                        //        for (int k = iResCount; k < ListRouteRes.Count; k++)
                        //        {
                        //            ////当前资源
                        //            //TimeSpan ts1 = new TimeSpan(ListRouteRes[i].dResEndDate.Ticks);

                        //            ////替补资源
                        //            //TimeSpan ts2 = new TimeSpan(ListRouteRes[k].dResEndDate.Ticks);

                        //            //TimeSpan ts=ts1.Subtract(ts2).Duration();

                        //            //如果当前资源比后备资源完工时间晚了48小时，则当前资源不排
                        //            //if (ts.Days * 24 + ts.Hours >= SchParam.iMutResourceDiffHour)
                        //            Int32 liReturn = SchData.GetDateDiff("h", ListRouteRes[i].dResEndDate, ListRouteRes[k].dResEndDate);



                        //            if (liReturn >= SchParam.iMutResourceDiffHour && SchParam.iMutResourceDiffHour > 0 )
                        //            {
                        //                ListRouteRes[i].cCanScheduled = "0";   //不排产 
                        //                ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                        //                ListRouteRes[i].iResRationHour = 0;    //加工工时为0 

                        //                ListRouteRes[k].cCanScheduled = "1";   //参与排产，替代优先级高的任务 
                        //                break;
                        //            }
                        //            else
                        //            {
                        //                ListRouteRes[k].cCanScheduled = "0";   //不排产 
                        //                ListRouteRes[k].iResReqQty = 0;        //加工数量为0   
                        //                ListRouteRes[k].iResRationHour = 0;    //加工工时为0 
                        //            }


                        //        }
                        //    }


                        //}
                        //}
                    }

                }
                //else if (SchParam.cMutResourceType == "2") //最早可完工日期资源优先选择
                //{
                //    ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                //    //ListRouteResSort = ListRouteRes;
                //}
                //else
                //{
                //    // if (SchParam.cMutResourceType == "1" )  //最早可完工日期资源优先选择
                //    ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResEndDate, p2.dResEndDate); });
                //    //ListRouteResSort = ListRouteRes;
                //}



                if (ListRouteRes.Count > 1)
                {
                    int j = 0;
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        if (ListRouteRes[i].cCanScheduled != "1") continue;

                        j++;

                        //多余的设备,都不排产
                        if (j > iResCount)
                        {
                            ListRouteRes[i].cCanScheduled = "0";   //不排产 
                            ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                            ListRouteRes[i].iResRationHour = 0;    //加工工时为0 
                        }

                    }
                }
            }

            //如果有多任务排产时,分配每行排程任务数量
            ResReqQtyDispatch();

            return 1;
        }

        //同一托盘号排产，当前工序第一个任务排产时，自动选择其他任务，都选择同样的资源。2020-03-22 && p.schProduct.cWorkRouteType = this.schProduct.cWorkRouteType 
        public int cBatchResourceSelect()
        {
            //设置为已处理
            this.cBatchNoFlag = 1;

            List<SchProductRoute> ListBatchRoute = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.cVersionNo == this.cVersionNo && p.schProduct.cBatchNo == this.schProduct.cBatchNo && p.schProduct.cWorkRouteType == this.schProduct.cWorkRouteType &&  p.iWoSeqID == this.iWoSeqID && p.cBatchNoFlag ==0  && p.iSchBatch == this.iSchBatch; });  // p.iProcessID == this.iProcessID 

            if (ListBatchRoute.Count < 1) return 0;

            

            foreach (var batchRoute in ListBatchRoute)
            {
                //设置每个资源的选择结果和当前资源一样。    
                for (int i = 0; i < batchRoute.SchProductRouteResList.Count; i++)
                {
                    //资源有限产能时，同一托盘强制一样选择，无限产能资源不用管 2020-04-18
                    if (batchRoute.SchProductRouteResList[i].resource.cIsInfinityAbility == "0")
                    {
                        SchProductRouteRes BatchRouteRes = this.SchProductRouteResList.Find(delegate (SchProductRouteRes p)
                        {
                            return p.cVersionNo.Trim() == batchRoute.cVersionNo && p.cResourceNo == batchRoute.SchProductRouteResList[i].cResourceNo && p.iWoSeqID == batchRoute.iWoSeqID;
                        });

                        if (BatchRouteRes != null && BatchRouteRes.iResReqQty > 0)
                        {
                            batchRoute.SchProductRouteResList[i].cSelected = "1";
                            batchRoute.SchProductRouteResList[i].iResReqQty = batchRoute.iReqQty;
                        }
                        else
                        {
                            batchRoute.SchProductRouteResList[i].cSelected = "0";
                            batchRoute.SchProductRouteResList[i].iResReqQty = 0;
                        }
                    }

                    ////有排产数量，而且选择资源了
                    //if (this.SchProductRouteResList[i].cSelected == "1" && this.SchProductRouteResList[i].iResReqQty > 0)
                    //{
                    //    batchRoute.SchProductRouteResList[i].cSelected = "1";
                    //    batchRoute.SchProductRouteResList[i].iResReqQty = batchRoute.iReqQty;
                    //}
                    //else
                    //{
                    //    batchRoute.SchProductRouteResList[i].cSelected = "0";
                    //    batchRoute.SchProductRouteResList[i].iResReqQty = 0;
                    //}

                    //设置为已处理
                    batchRoute.cBatchNoFlag = 1;
                }

                
            }

            

            return 1;
        }

        //同一托盘同时一起排产,同一托盘物料排产开工时间连续 2020-03-28 Jonas Cheng 
        public int cBatchSchRoute()
        {
            List<SchProductRoute> ListBatchRoute = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.cVersionNo == this.cVersionNo && p.schProduct.cBatchNo == this.schProduct.cBatchNo && p.schProduct.cWorkRouteType == this.schProduct.cWorkRouteType && p.iProcessID == this.iProcessID && p.iSchSdID != this.iSchSdID && p.cBatchNoFlag != 2; });

            if (ListBatchRoute.Count < 1) return 0;

            //同一托盘选择资源
            if (this.cWoNo == "" && this.SchProductRouteResList.Count > 1)
                cBatchResourceSelect();

            //
            foreach (var batchRoute in ListBatchRoute)
            {
                //设置为已处理
                batchRoute.cBatchNoFlag = 2;

                //设置每个资源的选择结果和当前资源一样。    
                batchRoute.ProcessSchTask();

                
            }

            return 1;
        }
                

        //排当前工序前面所有工序,正排.从当前工序一直往前找，找到所有最低层工序，再一道道往后排。树节点遍历所有节点
        public int ProcessSchTaskPre(Boolean bCurTask = true,Boolean bFreeze = false)  //
        {
            try
            { 
                //先排前面工序
                foreach (SchProductRoute schProductRoute in SchProductRoutePreList)
                {
                    if (schProductRoute.bScheduled == 0)
                        schProductRoute.ProcessSchTaskPre(bCurTask, bFreeze);               

                }

                //再排本工序
                if (bCurTask)
                {
                    SchParam.ldtBeginDate = DateTime.Now;
                    ProcessSchTask(bFreeze);

                    SchParam.iWaitTime = DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;
                }
            }
            catch (Exception error)
            {
                //System.Windows.Forms.Clipboard.SetText(this.ToString());
                if (SchParam.iSchSdID< 1)
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                else
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace); 
                
                return -1;
            }

            return 1;

        }

        //排当前工序后面所有工序,正排.从当前工序一直往后找，找到油漆工序为止
        public int ProcessSchTaskNext(string cTag = "1")  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排;4 只排后续工序一道工序,用于油漆后续工序排产,每遍排一次
        {
            try
            {
                //如果本工序未排，则先排本工序
                if (this.bScheduled == 0)
                    ProcessSchTask();

                //再排前面工序 后工序只有一道，或者没有
                if (SchProductRouteNextList.Count < 1) return 1;


                SchProductRoute schProductRouteNext = SchProductRouteNextList[0];
                if (schProductRouteNext == null)
                {
                    //System.Windows.Forms.Clipboard.SetText(this.ToString());
                    throw new Exception("订单行号：" + this.iSchSdID + "请检查产品[" + this.cInvCode + "]加工物料[" + this.cWorkItemNo + "]工序号[" + this.iWoSeqID.ToString() + "]工艺路线是否完整!");
                    return -1;
                }


                if (cTag == "1")        //1 加工物料相同的所有工序,排完
                {
                    if (schProductRouteNext.cInvCode == this.cInvCode) //排到关键资源为止
                    {
                        //继续往后工序排          
                        schProductRouteNext.ProcessSchTaskNext("1");
                    }

                }
                else if (cTag == "2")   //2 截止到同一加工物料的油漆工序 50  iWoSeqID < 50 双叶特殊处理，先不用
                {
                    if (schProductRouteNext.cInvCode == this.cInvCode && schProductRouteNext.iWoSeqID < 50)
                    {
                        //继续往后工序排          
                        schProductRouteNext.ProcessSchTaskNext("2");
                    }
                }
                else if (cTag == "4")   //4 只排后续工序一道工序,用于油漆后续工序排产,每遍排一次
                {
                    if (schProductRouteNext.bScheduled == 0)
                        schProductRouteNext.ProcessSchTask();
                    else
                    {
                        schProductRouteNext.ProcessSchTaskNext("4"); 
                    }
                    
                }
            }
            catch (Exception error)
            {
                //System.Windows.Forms.Clipboard.SetText(this.ToString());
                if (SchParam.iSchSdID < 1)
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                else
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace); 
                
                return -1;
            }

            return 1;

        }


        //倒排，排当前工序(包装工序)前面所有工序.从当前工序一道道往前倒排（确定完工时间），一直排到白茬工序为止
        //bSet 1是否齐套倒排标志,如果是齐套倒排，对比计划开工日期是否比之前的开工日期早，如果更早的话（目的是齐套，晚点开工晚点完工）则不需要倒排
        public int ProcessSchTaskRevPre(string cTag = "1",string bSet = "0") //1 加工物料相同的所有工序; 2 白茬工序; 3 所有下层物料半成品工序
        {
            if (cTag == "2")   //2 截止到白茬工序
            {
                if (this.iWoSeqID < 10 && this.bScheduled == 1) return 0;  //工序已排，而且是白茬工序，退出
            }


            ////如果本工序已排，则先清除本工序 放到ProcessSchTaskRev里面去清除
            //if (this.bScheduled == 1)
            //    ProcessClearTask();

            //如果本工序计划数量为0，则不需要倒排 2022-03-25 JonasCheng  
            if (this.iReqQty <= 0)
            {
                this.bScheduled = 1; //设置为已排产
                return 0;
            }

            //倒排本工序            
            ProcessSchTaskRev(bSet);           


            //再排前面工序
            foreach (SchProductRoute schProductRoute in SchProductRoutePreList)
            {
                if (cTag == "1")        //1 加工物料相同的所有工序,排完
                {
                    if (schProductRoute.cInvCode != this.cInvCode) continue;
                    //继续往前倒排工序
                    schProductRoute.ProcessSchTaskRevPre("1");
                }
                else if (cTag == "2")   //2 截止到白茬工序,油漆和白茬， 油漆工序从50工序开始 双叶特殊处理
                {
                    if (schProductRoute.iWoSeqID < 50) continue;
                    //继续往前倒排工序
                    schProductRoute.ProcessSchTaskRevPre("2");
                }
                else if (cTag == "3")
                {
                    //继续往前倒排工序
                    schProductRoute.ProcessSchTaskRevPre("3");
                }

               
            }


            return 1;

        }

        //倒排本工序,确定完工时间,往前找可用时间段
        public int ProcessSchTaskRev(string bSet = "0" )
        {
            //产品工艺模型中工序有先有后，必须按工艺模型的先后进行排程。
            //按t_SchProductRoute.iLevel Desc 

            ////计算本工序的完工时间，考虑移动批量。
            // 整批移转时：取上工序的完工时间 
            // 批量移转时，按时：上工序开工时间 + 批量间隔时间 + 批量移转时间
            // 批量移转时，按量：上工序开工时间 + 批量间隔数量 × 产能 + 批量移转时间
            // 上工序有多道时，取最大的可开工时间

            ////如果当前工序已排，则返回   
            //if (this.bScheduled == 1) return 1;
           

            DateTime dDateTemp = this.schData.dtStart;//DateTime.Now;
            DateTime dCanEndDate = this.schData.dtStart;//DateTime.Now;    //有多道前序工序时，找最大的可开工时间


            //调试
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
            }


            try
            {
                //后序工序的开工时间，作为本工序的完工时间
                if (this.SchProductRouteNextList.Count > 0)
                {
                    foreach (SchProductRoute schProductRoute in SchProductRouteNextList)
                    {
                        //倒排，根据后工序开工时间，取前工序完工时间，考虑后工序前准备时间，前工序的后准备时间
                        //dDateTemp = schProductRoute.dBegDate;

                        dDateTemp = schProductRoute.GetPreProcessCanEndDate(this);

                        if (dCanEndDate < dDateTemp) dCanEndDate = dDateTemp;
                    }

                    //齐套分析自动倒排，已经正排过的 2022-03-25 JonasCheng
                    if (bSet == "1") 
                    {
                        //如果倒排的结束日期小于正排的结束日期，则还维持正排结果,不倒排了
                        if (dCanEndDate <= this.dEndDate)
                        {
                            this.ProcessSchTask();
                            return 1;
                        }
                                                
                    }

                    if (dCanEndDate < this.schData.dtStart)   //出错
                    {
                        //System.Windows.Forms.Clipboard.SetText(this.ToString());
                        throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：" + this.iSchSdID + "工序ID号：" + this.iProcessProductID + "\n\r " + string.Format("倒排开始时间{0}小于排产开始日期{1},无法继续倒排!", dCanEndDate, this.schData.dtStart));
                        return -1;
                    }

                }
                else  //无后道工序，取产品的完工日期
                {
                    //dCanEndDate = this.schProduct.dRequireDate;

                    dCanEndDate = this.schProduct.dDeliveryDate.Date;   //以交货日期为准,提前一天完工

                    if (dCanEndDate <= this.schData.dtStart )   //倒排开始时间小于排产开始日期，产品需求日期太小，出错
                    {
                        dCanEndDate = this.schData.dtStart.AddMonths(1);      //加一个月

                    }
                }

                //如果本工序已排，则先清除本工序 放到ProcessSchTaskRev里面去清除
                if (this.bScheduled == 1)
                    ProcessClearTask();


                //////找出所有选择可用的机台
                //List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" ; });

                ////按选择的机台数平均分配生产任务
                //int iResCount = ListRouteRes.Count;
                //double iResReqQtyPer = (int)this.iReqQty / iResCount;  //小数取整，尾差留到最后一批
                //double iLeftReqQty = this.iReqQty;
                //double iResReqQty = iResReqQtyPer;

                ////2016-06-06
                //if (iResReqQtyPer < 1)
                //{
                //    iResCount = (int)this.iReqQty;
                //    iResReqQtyPer = 1;
                //}



                //for (int i = 0; i < iResCount; i++)
                //{

                //    //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                //    if (i == iResCount )
                //        iResReqQty = iLeftReqQty;
                //    else
                //        iResReqQty = iResReqQtyPer;

                //    //记录每排产一个机台后，剩余的计划数量
                //    if (iLeftReqQty - iResReqQty > 0)
                //        iLeftReqQty = iLeftReqQty - iResReqQty;
                //    else
                //    {
                //        iLeftReqQty = 0;
                //    }

                //    //调用SchProductRouteRes.TaskSchTask,加工时间重新计算
                //    if (iResReqQty > 0)
                //        ListRouteRes[i].TaskSchTaskRev(ref iResReqQty, dCanEndDate);
                //    else
                //    {
                //        ListRouteRes[i].iResReqQty = 0;
                //        ListRouteRes[i].iResRationHour = 0;
                //        ListRouteRes[i].iResPreTime = 0;
                //        ListRouteRes[i].cDefine25 = "";
                //        ListRouteRes[i].cDefine35 = 0;
                //    }

                //}

                if (this.iProcessProductID == SchParam.iProcessProductID && this.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }

                //根据工序最大机台数量,如2台，选择最优的两台资源进行排产,ListRouteRes只留两台cSelected的 2013-10-21
                //资源选择,cCanScheduled = '1',先找出最早可开工日期的工序
                //if (this.cWoNo == "")
                ResourceSelectRev(dCanEndDate);

                ////找出所有选择可用的机台,可排下的资源
                List<SchProductRouteRes> ListRouteRes2 = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });

                //按选择的机台数平均分配生产任务
                int iResCount = ListRouteRes2.Count;
                if (iResCount < 1)  //没有可用资源
                {
                    //如果存在资源任务已完工，cCanScheduled = '0',则本工序不用排产
                    List<SchProductRouteRes> ListRouteResCan = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "0"; });

                    if (ListRouteResCan.Count < 1)
                    {
                        //System.Windows.Forms.Clipboard.SetText(this.ToString());
                        throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + "没有可排产资源编号.或排产范围太小,最早可排开始日期：'" + dCanEndDate.ToLongDateString() + "'");
                        return -1;
                    }
                    else
                    {
                        //更新工序状态为已排
                        //this.BScheduled = 1;    //0 未排 1 已排
                        foreach (var item in ListRouteResCan)
                        {
                            item.BScheduled = 1;
                        }


                        return 1;
                    }
                }


                //////倒排时，还是选择正排时选中的机台，只排时间
                List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty >0  ; });

                foreach (SchProductRouteRes LSchProductRouteRes in ListRouteRes)
                {
                    //正常倒排
                    LSchProductRouteRes.TaskSchTaskRev(ref LSchProductRouteRes.iResReqQty, dCanEndDate);

                    //2023-03-06 JonasCheng 
                    //-------如果本工序计划开工日期，比后工序的开工日期晚,而且本工序是非产能无限，则需要重排（产能无限资源则不需要重排） --------------
                    //if(    ListRouteRes[i].dResEndDate )

                    if (SchProductRouteNextList != null && this.SchProductRouteNextList.Count > 0)
                    {
                        //SchParam.iPreTaskRev // 后工序完工时间比前工序大多少小时前工序倒排 24 小时，太小了容易倒排出错  2020-01-09
                        //同一加工物料,后工序的计划完工日期小于前工序的计划完工日期，重排该工序时间
                        if ( LSchProductRouteRes.dResBegDate.Subtract(SchProductRouteNextList[0].dBegDate).TotalHours > 0 ) //&&  ListRouteRes[i].resource.cIsInfinityAbility != "1"
                        {
                            //区分当前任务资源是否无限产能，如果是无限产能，而且前工序为整批移转时,则直接修改本工序完工时间为前工序的结束时间 2017-11-26
                            //if (ListRouteRes[i].resource.cIsInfinityAbility == "1" && ListRouteRes[i].schProductRoute.cMoveType == "0")
                            //{
                            //    //当前工序的完工时间为前工序的完工时间
                            //    ListRouteRes[i].dResEndDate = SchProductRoutePreList[0].dEndDate;
                            //}
                            //else     //
                            {

                                // Int32 liReturn = SchData.GetDateDiff("h", ListRouteRes[i].dResEndDate, ListRouteRes[k].dResEndDate);

                                //SchProductRoutePreList[i].dEndDate > ListRouteRes[i].dResEndDate

                                //可排产日期 + 差异时间 ，当前工序得开工时间比后工序得开工时间晚，重排。
                                Double iDiffMinites = SchData.GetDateDiff("m",  SchProductRouteNextList[0].dBegDate,LSchProductRouteRes.dResBegDate);
                                //dCanBegDate =   ListRouteRes[i].dResBegDate.AddMinutes(iDiffMinites + 5);
                                if (iDiffMinites > 0 )
                                {

                                    ////计算前工序的批量移转时间
                                    //Double iMoveTime = this.GetProcessMoveQty(SchProductRouteNextList[0]);

                                    ////if (iMoveTime < iDiffMinites  ) iMoveTime = iDiffMinites ;

                                    ////正排日期，以后工序的开工日期 - 10分钟，进行正排,不能用dCanEndDate
                                    //dCanBegDate = SchProductRouteNextList[0].dBegDate.AddMinutes(-iMoveTime); //iDiffMinites

                                    //重新计算本工序的完工时间 = 后工序的完工时间 - 计划开工时间的差异（分钟）
                                    dCanEndDate = LSchProductRouteRes.dResEndDate.AddMinutes(-iDiffMinites); //iDiffMinites

                                    //清空之前排产
                                    LSchProductRouteRes.TaskClearTask();

                                    // 可排产时间dCanBegDate,重新排产
                                    //ListRouteRes[i].TaskSchTask(ref iResReqQty, dCanBegDate);                                  

                                    //正排当前工序，根据后工序的开工日期，正排当前工序
                                    try
                                    {
                                        LSchProductRouteRes.TaskSchTaskRev(ref LSchProductRouteRes.iResReqQty, dCanEndDate);
                                    }
                                    catch (Exception error)
                                    {
                                        //System.Windows.Forms.Clipboard.SetText(this.ToString());


                                        if (SchParam.iSchSdID < 1)
                                            throw new Exception("订单行号：" + this.iSchSdID + "倒排时，资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                                        else
                                            throw new Exception("订单行号：" + this.iSchSdID + "倒排时，资源正排工序完工时间比前工序早,需倒排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);


                                        return -1;
                                    }
                                }
                            }

                        }


                    }

                }


                //更新当前任务的计划开始时间，计划完工时间,必须数量大于0 的            
                List<SchProductRouteRes> list1 = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResReqQty > 0  ; });

                if (list1.Count > 0)
                {
                    list1.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    this.dBegDate = list1[0].dResBegDate;                 //取已排资源任务排产时间的最小开始时间
                    this.dEndDate = list1[list1.Count - 1].dResEndDate;   //取已排资源任务排产时间的最大结束时间
                }

                //取资源总的加工时间；前置时间，也应该取资源的前置准备时间
                int iLaborTime = 0;
                foreach (SchProductRouteRes lSchProductRouteRes in list1)
                {
                    iLaborTime += Convert.ToInt32(lSchProductRouteRes.iResRationHour);
                }

                this.iLaborTime = iLaborTime;
                //更新工序状态为已排
                this.BScheduled = 1;    //0 未排 1 已排

                ////记录排产日志
                //if (SchParam.APSDebug == "1")
                //{
                //    string message = string.Format(@"2、物料编号[{0}],计划ID[{1}],任务ID[{2}],工序号[{3}],工艺编号[{4}],计划开工时间[{5}],计划完工时间[{6}],工单号[{7}]",
                //                                            this.cInvCode, this.iSchSdID, this.iProcessProductID, this.iWoSeqID, this.cTechNo + this.cSeqNote, this.dBegDate, this.dEndDate, this.cWoNo);
                //    SchParam.Debug(message, "SchProductRoute.ProcessSchTaskRev工序排产完成");
                //}
            }
            catch (Exception error)
            {
                //System.Windows.Forms.Clipboard.SetText(this.ToString());
                throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：" + this.iSchSdID + "工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }

            return 1;
        }

        //正排 取得后序工序可开工时间,即本工序完工时间或第一批完工时间，根据移动方式来定       
        public DateTime GetNextProcessCanBegDate(SchProductRoute schProductRouteNext)
        {
            DateTime ldtFirstEndDate = this.dBegDate;     //后序工序可开工时间
            string cWorkType = "0";       //加工类型
            double liCapacity = 1;        //产能
            double iBatchQty = 1;         //加工批量

            try
            {
                ////如果不考虑前序工序完工时间约束，直接返回排产开始日期
                //if (SchParam.PreSeqEndDate != "1") return SchParam.dtStart;

                //如果当前工序已完工,直接返回排产结束时间，或者当前工序需求数量为0（MRP运算已扣完库存，不需要生产）2022-03-15 JonasCheng  //|| this.iReqQty == 0
                if (this.cStatus == "4" || this.iActQty > 0 || this.iReqQty == 0)
                {
                    if (this.cVersionNo != "SureVersion" )   //如果非正式版本，要考虑最早可排天数  2022-03-15 JonasCheng 
                        return SchParam.dtStart.AddDays(SchParam.dEarliestSchDateDays);
                    else
                        return SchParam.dtStart;
                }


                if (this.SchProductRouteResList.Count > 0)
                {
                    liCapacity = this.SchProductRouteResList[0].iCapacity;      //取第一个可用资源的产能
                    cWorkType = this.SchProductRouteResList[0].cWorkType;       //加工类型
                    iBatchQty = this.SchProductRouteResList[0].iBatchQty;       //加工批量
                }


                //cParellelType并行类型 ES 前工序结束后工序开始  SS 前工序开始后工序开始(差一个批次移转时间)  EE 同时结束(差一个批次移转时间)
                // 0 整批移转时：取上工序的完工时间 +  批量移转时间 + 本工序后准备时间  (单位全部用分钟)
                // 1 批量移转时，按时：上工序开工时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
                // 2 批量移转时，按量：上工序开工时间 + 批量间隔数量 × 产能 + 批量移转时间
                if (this.cParellelType == "ES")     //ES 整批移转 
                {
                    if (SchParam.NextSeqBegTime > 0)  //前工序开工时间 + 后工序可开工时间为前工序开工后多长时间(分钟) 120
                    {
                        ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveTime + this.iSeqPostTime + SchParam.NextSeqBegTime);

                        //如果大于工序完工时间,则去前工序的完工时间
                        if (ldtFirstEndDate > this.dEndDate.AddMinutes(this.iMoveTime + this.iSeqPostTime))
                            ldtFirstEndDate = this.dEndDate.AddMinutes(this.iMoveTime + this.iSeqPostTime);

                    }
                    else
                        ldtFirstEndDate = this.dEndDate.AddMinutes(this.iMoveTime + this.iSeqPostTime);
                }                
                else if (this.cParellelType == "EE")    //EE 同时结束(差一个批次移转时间，可开工时间正常取，后工序排产时，如果提前早可以倒排)
                {
                    //如果后工序已经排产，则本工序参照后工序的计划完工时间 - 1个移动批量
                    if (schProductRouteNext.BScheduled == 1)  //this.cParallelNo != ""  并行工序
                    {
                        if (this.cMoveType == "1")          //1 按时 工序开始时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
                        {
                            ldtFirstEndDate = schProductRouteNext.dEndDate.AddMinutes(-this.iMoveInterTime - this.iMoveTime - this.iSeqPostTime);  //+ this.iSeqPreTime
                        }
                        else if (this.cMoveType == "2")    //2 按量 ,计算成总工时，转换成分钟
                        {
                            if (cWorkType == "1")
                                ldtFirstEndDate = schProductRouteNext.dEndDate.AddMinutes(-this.iMoveInterQty * liCapacity / 60 / iBatchQty - this.iMoveTime - this.iSeqPostTime);  //+ this.iSeqPreTime
                            else
                                ldtFirstEndDate = schProductRouteNext.dEndDate.AddMinutes(-this.iMoveInterQty * liCapacity / 60 - this.iMoveTime - this.iSeqPostTime);  //+ this.iSeqPreTime
                        }
                    }
                    else
                    {
                        if (this.cMoveType == "1")          //1 按时 工序开始时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
                        {
                            ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveInterTime + this.iMoveTime + this.iSeqPostTime);  //+ this.iSeqPreTime
                        }
                        else if (this.cMoveType == "2")    //2 按量 ,计算成总工时，转换成分钟
                        {
                            if (cWorkType == "1")
                                ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveInterQty * liCapacity / 60 / iBatchQty + this.iMoveTime + this.iSeqPostTime);  //+ this.iSeqPreTime
                            else
                                ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveInterQty * liCapacity / 60 + this.iMoveTime + this.iSeqPostTime);  //+ this.iSeqPreTime
                        }
                    }
                }
                else //if (this.cParellelType == "SS")       //SS 前工序开始后工序开始(差一个批次移转时间)
                {
                    if (this.cMoveType == "1")          //1 按时 工序开始时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
                    {
                        ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveInterTime + this.iMoveTime + this.iSeqPostTime);  //+ this.iSeqPreTime
                    }
                    else if (this.cMoveType == "2")    //2 按量 ,计算成总工时，转换成分钟
                    {
                        if (cWorkType == "1")
                            ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveInterQty * liCapacity / 60 / iBatchQty + this.iMoveTime + this.iSeqPostTime);  //+ this.iSeqPreTime
                        else
                            ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveInterQty * liCapacity / 60 + this.iMoveTime + this.iSeqPostTime);  //+ this.iSeqPreTime
                    }
                }


                //当前工序不为空，可开工时间应 + 本工序的前准备时间
                if (schProductRouteNext != null)
                    ldtFirstEndDate = ldtFirstEndDate.AddMinutes(schProductRouteNext.iSeqPreTime);
            }
            catch (Exception error)
            {
                //System.Windows.Forms.Clipboard.SetText(this.ToString());
                throw new Exception("订单行号：" + this.iSchSdID + "资源计算时出错,位置SchProductRoute.GetNextProcessCanBegDate！订单行号：" + this.iSchSdID + "工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + " " + error.StackTrace.ToString());
               
            }


            return ldtFirstEndDate;
        }

        //计算一个批量移转时间 2017-10-30
        public double GetProcessMoveTime(SchProductRoute schProductRoutePre)
        {
            double iMoveTime = 0;
            double liCapacity = 1;        //产能

            if (schProductRoutePre.SchProductRouteResList.Count > 0)
            {
                liCapacity = schProductRoutePre.SchProductRouteResList[0].iCapacity;      //取第一个可用资源的产能
            }

            // 0 整批移转时：取上工序的完工时间 +  批量移转时间 + 本工序后准备时间  (单位全部用分钟)
            // 1 批量移转时，按时：上工序开工时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
            // 2 批量移转时，按量：上工序开工时间 + 批量间隔数量 × 产能 + 批量移转时间
            if (schProductRoutePre.cMoveType == "1")          //1 按时 工序开始时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
            {
                iMoveTime = schProductRoutePre.iMoveInterTime + schProductRoutePre.iMoveTime ;  //+ this.iSeqPreTime
            }
            else if (schProductRoutePre.cMoveType == "2")    //2 按量 ,计算成总工时，转换成分钟
            {
                iMoveTime = schProductRoutePre.iMoveInterQty * liCapacity / 60 + schProductRoutePre.iMoveTime ;  //+ this.iSeqPreTime
            }
            else                               //0 整批移转 
            {
                iMoveTime = schProductRoutePre.iMoveTime;
            }

            return iMoveTime;
        }

        //计算移转批量 2017-10-30
        public double GetProcessMoveQty(SchProductRoute schProductRoutePre)
        {
            double iMoveTime = 0;
            double liCapacity = 1;        //产能
            double ibatchqty = 1;

            if (schProductRoutePre.SchProductRouteResList.Count > 0)
            {
                liCapacity = schProductRoutePre.SchProductRouteResList[0].iCapacity;      //取第一个可用资源的产能
                ibatchqty = schProductRoutePre.SchProductRouteResList[0].iBatchQty;      //取第一个可用资源的产能



                if (liCapacity == 0) liCapacity = 1;


                // 0 整批移转时：取上工序的完工时间 +  批量移转时间 + 本工序后准备时间  (单位全部用分钟)
                // 1 批量移转时，按时：上工序开工时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
                // 2 批量移转时，按量：上工序开工时间 + 批量间隔数量 × 产能 + 批量移转时间
                if (schProductRoutePre.cMoveType == "1")          //1 按时 工序开始时间 + 批量间隔时间 + 批量移转时间 + 工序前准备时间 + this.iSeqPostTime
                {
                    iMoveTime = schProductRoutePre.iMoveInterTime;  //+ this.iSeqPreTime
                }
                else if (schProductRoutePre.cMoveType == "2")    //2 按量 ,计算成总工时，转换成分钟
                {
                    iMoveTime = schProductRoutePre.iMoveInterQty* liCapacity/ibatchqty/60 ;  //+ this.iSeqPreTime
                }
                else                               //0 整批移转 
                {
                    iMoveTime = 0;
                }
            }

            return iMoveTime;
        }


        //倒排 取得前序工序可完工时间,即本工序开工时间，根据移动方式来定
        
        public DateTime GetPreProcessCanEndDate(SchProductRoute schProductRoutePre, string cType ="1")
        {
             DateTime ldtFirstEndDate;     //前序工序可完工时间

            ////当前工序开工时间 - 移转时间 - 工序前准备时间         
            //ldtFirstEndDate = this.dBegDate.AddMinutes(- this.iMoveTime - this.iSeqPreTime);


            ////当前工序不为空，可完工时间 - 前工序的后准备时间
            //if (schProductRoutePre != null && schProductRoutePre.iSeqPostTime != null)
            //{  
            //    //schProductRoutePre.cDefine28 += ";后工序开工时间" + this.dBegDate;

            //    //schProductRoutePre.cDefine28 += ";后工序前准备时间" + this.iSeqPreTime;

            //    ldtFirstEndDate = ldtFirstEndDate.AddMinutes(-schProductRoutePre.iSeqPostTime);

            //    schProductRoutePre.cDefine28 = ";本工序后准备时间" + schProductRoutePre.iSeqPostTime;
            //    schProductRoutePre.cDefine28 += ";本工序可开工时间" + ldtFirstEndDate;

            //}

            //当前工序的完工时间按照后工序的完工时间推算，如果计算出来的开工时间比后工序的开工时间晚，则按后工序的开工时间减去偏移量计算。2023-03-06

            // 当前工序完工时间 = 后工序完工时间 - 移转时间 - 工序前准备时间 - 批量加工时间        

            //计算前工序的批量移转时间
            Double iMoveTime = this.GetProcessMoveQty(schProductRoutePre);

            ////if (iMoveTime < iDiffMinites  ) iMoveTime = iDiffMinites ;


            ////倒排日期，以前工序的完工日期 + 10分钟，进行倒排,不能用dCanBegDate
            //dCanBegDate = SchProductRoutePreList[0].dEndDate.AddMinutes(iMoveTime); //iDiffMinites

            ldtFirstEndDate = this.dEndDate.AddMinutes(-this.iMoveTime - this.iSeqPreTime - iMoveTime);


            //当前工序不为空，可完工时间 - 前工序的后准备时间
            if (schProductRoutePre != null && schProductRoutePre.iSeqPostTime != null)
            {
                //schProductRoutePre.cDefine28 += ";后工序开工时间" + this.dBegDate;

                //schProductRoutePre.cDefine28 += ";后工序前准备时间" + this.iSeqPreTime;

                ldtFirstEndDate = ldtFirstEndDate.AddMinutes(-schProductRoutePre.iSeqPostTime);

                schProductRoutePre.cDefine28 = ";本工序后准备时间" + schProductRoutePre.iSeqPostTime;
                schProductRoutePre.cDefine28 += ";本工序可开工时间" + ldtFirstEndDate;

            }

            return ldtFirstEndDate;
        }

        //清除资源已排具体任务占用时间段。
        public void ProcessClearTask()
        {
            //调用schProductRouteRes.TaskClearTask()，清除当前任务占用的时间段
            foreach (SchProductRouteRes schProductRouteRes in this.SchProductRouteResList)
            {
                schProductRouteRes.TaskClearTask();
            }

            //工序排产状态设置为未排
            this.BScheduled = 0;
        }

        //计算工序最早可排日期
        public void GetRouteEarlyBegDate()
        {
            DateTime dDateTemp = this.schData.dtStart;

            try
            {
                if (this.schProduct == null) return;

                //&& this.iProcessProductID == SchParam.iProcessProductID
                if (this.iSchSdID == SchParam.iSchSdID )
                {
                    int j = 1;
                }
                if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
                {
                    int j = 1;
                }

                this.cDefine27 = "dEarlyBegDate:" + this.dEarlyBegDate;  //清空前准备时间

                //有多道前序工序时，找最大的可开工时间
                if (this.SchProductRoutePreList != null && this.SchProductRoutePreList.Count > 0)
                {
                    foreach (SchProductRoute schProductRoutePre in SchProductRoutePreList)
                    {
                        //如果前工序未排产，则先排前工序 2014-11-13
                        if (schProductRoutePre.bScheduled == 0)
                        {
                            schProductRoutePre.ProcessSchTask();
                        }

                        dDateTemp = schProductRoutePre.GetNextProcessCanBegDate(this);
                        if (this.cStatus != "4" && this.dEarlyBegDate < dDateTemp)
                        {
                            this.dEarlyBegDate = dDateTemp;
                            this.cDefine27 += ";前工序" + schProductRoutePre.iProcessProductID + ":" + dDateTemp;
                        }

                        
                    }
                }
                else if (this.iSeqPreTime > 0 && this.cStatus != "2" )  //当前工序没开工时，考虑工序开工时间，没有前工序，只考虑本工序的前准备时间 2021-10-26 JonasCheng 
                {
                    ////如果不考虑前序工序完工时间约束，直接返回排产开始日期
                    //if (SchParam.PreSeqEndDate != "1")
                    //{
                    //    this.dEarlyBegDate = SchParam.dtStart;
                    //    return ;
                    //}
                    dDateTemp = SchParam.dtStart.AddMinutes(this.iSeqPreTime);

                    if (this.dEarlyBegDate < dDateTemp)
                    {
                        this.dEarlyBegDate = dDateTemp;
                        this.cDefine27 += ";本工序前准备时间" + this.iSeqPreTime + ":" + dDateTemp;
                    }
                }

                //考虑半成品完工日期
                //if (SchParam.cSelfEndDate == "1")
                //{
                //    // //0 全面考虑 1 不考虑所有提前期  2 考虑下层加工提前期  3 考虑子料采购提前期
                //    if (this.techInfo != null && (this.techInfo.iOrder == 0 || this.techInfo.iOrder == 2))
                //    {

                //        List<SchProduct> SchProductList = new List<SchProduct>(10);

                //        //找出当前产品下层的自制件最晚完工时间
                //        List<SchProductRouteItem> SchProductRouteItemListSelf = SchProductRouteItemList.FindAll(delegate (SchProductRouteItem p) { return p.bSelf == "1" && p.cInvCode == this.cWorkItemNo && p.cInvCodeFull == this.cWorkItemNoFull; });


                //        if (SchProductRouteItemListSelf != null && SchProductRouteItemListSelf.Count > 0)
                //        {
                //            foreach (SchProductRouteItem schProductRouteItem in SchProductRouteItemListSelf)
                //            {
                //                if (schProductRouteItem.iReqQty > 0 && schProductRouteItem.dEarlySubItemDate > dDateTemp)
                //                    dDateTemp = schProductRouteItem.dEarlySubItemDate;
                                

                //                //最早可开工日期
                //                if (this.dEarlyBegDate < dDateTemp)
                //                {
                //                    this.dEarlyBegDate = dDateTemp;
                //                    this.cDefine27 += ";采购件最晚到料日期" + ":" + dDateTemp;
                //                }

                //                //if (schProductRouteItem.bSelf != "1") continue;

                //                //SchProductList = this.schData.SchProductList.FindAll(delegate (SchProduct p) { return p.cInvCode == schProductRouteItem.cSubInvCode && p.cMiNo == this.schProduct.cMiNo && p.iSchSdID.ToString() != this.iSchSdID.ToString() && p.bScheduled.ToString() == "1"; });  //&& p.bScheduled.ToString() == "1"

                //                //if (SchProductList.Count < 1) continue;

                //                //SchProductList.Sort(delegate (SchProduct p1, SchProduct p2) { return Comparer<DateTime>.Default.Compare(p1.dEndDate, p2.dEndDate); });

                //                ////以子料开工时间 +　SchParam.NextProductBegTime（240分钟） 或 子料完工时间 
                //                //if (SchParam.NextProductBegTime < 0)
                //                //{
                //                //    dDateTemp = SchProductList[SchProductList.Count - 1].dEndDate;
                //                //}
                //                //else
                //                //{
                //                //    if (SchProductList[0].dBegDate.AddMinutes(SchParam.NextProductBegTime) > SchProductList[SchProductList.Count - 1].dEndDate)
                //                //    {
                //                //        dDateTemp = SchProductList[SchProductList.Count - 1].dEndDate;
                //                //    }
                //                //    else
                //                //    {
                //                //        dDateTemp = SchProductList[0].dBegDate.AddMinutes(SchParam.NextProductBegTime);
                //                //    }
                //                //}

                //                ////最早可开工日期
                //                //if (this.dEarlyBegDate < dDateTemp)
                //                //{
                //                //    this.dEarlyBegDate = dDateTemp;
                //                //    this.cDefine27 += ";半成品完成时间" + ":" + dDateTemp;
                //                //}
                //            }
                //        }

                //    }
                //}

                ////考虑原材料采购提前期 ,非正式版本，下单时考虑
                if (SchParam.cPurEndDate == "1" && this.cVersionNo != "SureVersion")
                {
                   
                    //产品可开工时间 + 当前物料采购提前期(下层物料最长的)，物料档案中设置了采购周期 2019-03-09
                    // //0 全面考虑 1 不考虑所有提前期  2 考虑下层加工提前期  3 考虑子料采购提前期
                    if (this.techInfo != null && (this.techInfo.iOrder == 0 || this.techInfo.iOrder == 3))
                    {
                        DateTime dPurEarliestSchDate = this.schData.dtStart;
                        //找出当前自制件下的采购物料
                        List<SchProductRouteItem> SchProductRouteItemListPur = SchProductRouteItemList.FindAll(delegate (SchProductRouteItem p) { return p.bSelf != "1" && p.cInvCode == this.cWorkItemNo && p.cInvCodeFull == this.cWorkItemNoFull; });

                        if (SchProductRouteItemListPur != null && SchProductRouteItemListPur.Count > 0)
                        {
                            ////采购件,找有净需求的物料最晚到料日期
                            foreach (SchProductRouteItem schProductRouteItem in SchProductRouteItemListPur)
                            {
                                if (schProductRouteItem.iReqQty > 0 && schProductRouteItem.dEarlySubItemDate > dPurEarliestSchDate)
                                    dPurEarliestSchDate = schProductRouteItem.dEarlySubItemDate;

                            }

                            //最早可开工日期
                            if (this.dEarlyBegDate < dPurEarliestSchDate)
                            {
                                this.dEarlyBegDate = dPurEarliestSchDate;
                                this.cDefine27 += ";采购件最晚到料日期" + ":" + dPurEarliestSchDate;
                            }
                        }
                      
                    }                    

                }

                //考虑自制件定义的采购提前期，和是否考虑采购提前期参数无关
                if (this.item != null && this.item != null && this.item.iAdvanceDate > 0)
                {
                    dDateTemp = this.schProduct.dEarliestSchDate.AddDays(this.item.iAdvanceDate);

                    //最早可开工日期
                    if (this.dEarlyBegDate < dDateTemp)
                    {
                        this.dEarlyBegDate = dDateTemp;
                        this.cDefine27 += ";产品采购提前期" + this.item.iAdvanceDate + ":" + dDateTemp;
                    }
                }

            }
            catch (Exception error)
            {
                //System.Windows.Forms.Clipboard.SetText(this.ToString());
                throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：" + this.iSchSdID + "工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + " " +error.StackTrace.ToString());
                return ;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            
            throw new NotImplementedException();
        }
    }
}
