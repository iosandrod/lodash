﻿using System;
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
        public int iAutoID { get; set; }
        public string cLevelInfo { get; set; }
        public int iLevel { get; set; }
        public int iParentItemID { get; set; }        //父项ID
        public string cParentItemNo { get; set; }     //父项编号
        public string cParentItemNoFull { get; set; }     //父项编号全路径
        public string cParellelType { get; set; }      //并行类型 ES 前工序结束后工序开始  SS 前工序开始后工序开始(差一个批次移转时间)  EE 同时结束(差一个批次移转时间)  
        public string cParallelNo { get; set; }        //并行码
        public string cKeyBrantch { get; set; }        //关键分支
        public string cCompSeq { get; set; }           //完工工序
        public string cMoveType { get; set; }           //移转方式 0 整体 1 按时 2 按量 
        public double iMoveInterTime { get; set; }       //移动间隔时间
        public double iMoveInterQty { get; set; }        //移动间隔数量
        public double iMoveTime { get; set; }            //移动时间
        public int iDevCountPd = 1;        //用于加工本工序最大资源数
        public string cDevCountPdExp { get; set; }    //排产资源数表达式
        private double ISeqPretime;
        public double iSeqPreTime { get {
                if (this.ISeqPretime == null) this.ISeqPretime = 0;
                return this.ISeqPretime;
                }   
                set { ISeqPretime = value; } 
        }        //工序前准备时间
        private double ISeqPostTime;
        public double iSeqPostTime
        {
            get
            {
                if (this.ISeqPostTime == null) this.ISeqPostTime = 0;
                return this.ISeqPostTime;
            }
            set { ISeqPostTime = value; }
        }        //工序后准备时间
        public decimal iCapacity { get; set; }
        public string cCapacityExp { get; set; }        //单件工时表达式
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
                if (this.iSchSdID == SchParam.iSchSdID)
                {
                    DateTime dt = this.dBegDate;
                    DateTime dt2 = this.dEndDate;
                }
                if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
                {
                    DateTime dt = this.dBegDate;
                    DateTime dt2 = this.dEndDate;
                }
            }
        }
        #endregion
        public SchProduct schProduct;          //工序对应产品信息  
        public SchProductWorkItem schProductWorkItem;          //工序对应工单信息  
        public Item item;                      //加工物料信息
        public TechInfo techInfo;              //工艺信息
        public List<SchProductRouteItem> SchProductRouteItemList = new List<SchProductRouteItem>(10);
        public List<SchProductRouteRes> SchProductRouteResList = new List<SchProductRouteRes>(10);
        public List<SchProductRoute> SchProductRoutePreList = new List<SchProductRoute>(10);
        public List<SchProductRoute> SchProductRouteNextList = new List<SchProductRoute>(10);
        public int ProcessSchTask(Boolean bFreeze = false)
        {
            if (this.bScheduled == 1) return 1;
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
                j = 1;
            }
            if (this.cParallelNo != "" && this.cKeyBrantch != "1" && this.SchProductRouteNextList.Count > 0 && this.SchProductRouteNextList[0].bScheduled != 1)
            {
                return 1;
            }
            DateTime dDateTemp = this.schData.dtStart;//DateTime.Now;
            DateTime dCanBegDate = this.schData.dtStart;//DateTime.Now;    //有多道前序工序时，找最大的可开工时间   
            DateTime dCanBegDateProcess = this.schData.dtStart;  //工序可开工时间
            try
            {
                {
                    if (dCanBegDate < this.dEarlySubItemDate)
                        dCanBegDate = this.dEarlySubItemDate;
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
                    if (this.cVersionNo.ToLower() == "sureversion" && this.dFirstBegDate > schData.dtStart && dCanBegDate < this.dFirstBegDate.AddDays(-SchParam.dLastBegDateBeforeDays))
                        dCanBegDate = this.dFirstBegDate.AddDays(-SchParam.dLastBegDateBeforeDays);
                    this.GetRouteEarlyBegDate();
                    if (dCanBegDate < this.dEarlyBegDate)
                        dCanBegDate = this.dEarlyBegDate;
                }
                if (this.cVersionNo.ToLower() == "sureversion"   ) //非待工状态  //|| this.iActQty > 0 || ( this.cStatus != "0" 
                {
                    if (SchParam.UseAPS == "3" && this.schProductWorkItem != null && this.schProductWorkItem.cStatus == "I" && this.SchProductRouteResList.Count > 1)
                    {
                        try
                        {
                            ResourceSelect(dCanBegDate, bFreeze);
                        }
                        catch (Exception error)
                        {
                            if (SchParam.iSchSdID < 1)
                                throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                            else
                                throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                            return -1;
                        }
                    }
                    List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQtyOld > 0 ; });
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
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
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
                                if (SchParam.iSchSdID < 1)
                                    throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                                else
                                    throw new Exception("多资源选择出错，订单行号：" + this.iSchSdID + ",位置SchProductRoute.ResourceSelect！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                                return -1;
                            }
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
                        if ( SchParam.iSchSdID < 1   )
                            throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                        else
                            throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                        return -1;
                    }
                    List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });
                    int iResCount = ListRouteRes.Count;
                    if (iResCount < 1 )  //没有可用资源
                    {
                        List<SchProductRouteRes> ListRouteResCan = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "0"; });
                        if (ListRouteResCan.Count < 1)
                        {
                            throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + "没有可排产资源编号.或排产范围太小,开始日期：'" + dCanBegDate.ToLongDateString() + "'");
                            return -1;
                        }
                        else
                        {
                            foreach (var item in ListRouteResCan)
                            {
                                item.BScheduled = 1;
                            }
                            this.BScheduled = 1;    //0 未排 1 已排
                            return 1;
                        }                       
                    }                    
                    dCanBegDateProcess = dCanBegDate;
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        double iResReqQty = ListRouteRes[i].iResReqQty;
                        try
                        {
                            DateTime ldtBeginDate = DateTime.Now;
                            ListRouteRes[i].TaskSchTask(ref iResReqQty, dCanBegDate);
                            DateTime ldtEndedDate = DateTime.Now;
                            Double iWaitTime2 = DateTime.Now.Subtract(ldtBeginDate).TotalMilliseconds;
                            TimeSpan interval = ldtEndedDate - ldtBeginDate;//计算间隔时间
                        }
                        catch (Exception error)
                        {
                            if (SchParam.iSchSdID < 1)
                                throw new Exception("资源任务正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                            else
                                throw new Exception("资源任务正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                            return -1;
                        }
                        if (SchProductRoutePreList != null && this.SchProductRoutePreList.Count > 0)
                        {
                            foreach (var schProductRoutePre in SchProductRoutePreList)
                            {
                                if (schProductRoutePre.bScheduled != 1) continue;
                                if (SchParam.iPreTaskRev > 0  && ListRouteRes[i].dResEndDate.Subtract(schProductRoutePre.dEndDate).TotalHours > SchParam.iPreTaskRev) //&&  ListRouteRes[i].resource.cIsInfinityAbility != "1"
                                {
                                    {
                                        Double iDiffMinites = SchData.GetDateDiff("m", schProductRoutePre.dEndDate, ListRouteRes[i].dResEndDate) - schProductRoutePre.iSeqPostTime - ListRouteRes[i].schProductRoute.iSeqPreTime ;
                                        if (iDiffMinites > SchParam.iPreTaskRev * 60 )
                                        {
                                            Double iMoveTime = this.GetProcessMoveQty(schProductRoutePre);
                                            dCanBegDate = ListRouteRes[i].dResEndDate.AddMinutes(-iMoveTime - schProductRoutePre.iSeqPostTime - ListRouteRes[i].schProductRoute.iSeqPreTime); //iDiffMinites
                                            schProductRoutePre.ProcessClearTask() ;
                                            try
                                            {
                                                schProductRoutePre.ProcessSchTaskRev("0");
                                            }
                                            catch (Exception error)
                                            {
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
                List<SchProductRouteRes> list1 = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResReqQty > 0; });
                if (list1.Count > 0)
                {
                    list1.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    this.dBegDate = list1[0].dResBegDate;                 //取已排资源任务排产时间的最小开始时间
                    this.dEndDate = list1[list1.Count - 1].dResEndDate;   //取已排资源任务排产时间的最大结束时间
                }
                else     //
                {
                    List<SchProductRouteRes> list2 = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResReqQty > 0; });
                    if (list2.Count > 0)
                    {
                        list2.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        this.dBegDate = list2[0].dResBegDate;                 //取已排资源任务排产时间的最小开始时间
                        this.dEndDate = list2[list2.Count - 1].dResEndDate;   //取已排资源任务排产时间的最大结束时间
                    }
                }
                int iLaborTime = 0;
                foreach (SchProductRouteRes lSchProductRouteRes in list1)
                {
                    iLaborTime += Convert.ToInt32(lSchProductRouteRes.iResRationHour);
                }
                this.iLaborTime = iLaborTime;
                this.BScheduled = 1;    //0 未排 1 已排
                if (this.schProduct.cBatchNo != "" && this.cBatchNoFlag == 0  )
                    cBatchSchRoute();
            }
            catch (Exception error)
            {
                if (SchParam.iSchSdID < 1)
                    throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                else
                    throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                return -1;
            }
            return 1;
        }
        public int ResourceSelect(DateTime dCanBegDate,Boolean bFreeze = false)
        {
            List<SchProductRouteRes> ListRouteRes = this.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
            }
            int iRouteResCountFirst = ListRouteRes.Count;
            if (iRouteResCountFirst < 1)
            {
                throw new Exception(string.Format("多资源选择正排出错,订单行号：{0}，没有找到已选择的可排产资源，资源产能明细总资源数量为{1},位置SchProductRoute.ProcessSchTask！工序ID号：{2},物料编号{3}", this.iSchSdID.ToString(), SchProductRouteResList.Count.ToString(),this.cInvCode));
                return -1;
            }
            if (bFreeze || this.iActQty > 0)  //|| this.cWoNo != "" 冻结状态 或者完工数量大于0 ,已开工生产的，不能换资源
            {
                if (this.iReqQty > 0)
                {
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
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
                if (ListRouteRes.Count > 1 )//(this.SchProductRouteResList.Count > 1 )  //多资源排产
                {
                    if (this.SchProductRoutePreList.Count == 1 && this.SchProductRoutePreList[0].cInvCode == this.cInvCode && this.SchProductRoutePreList[0].SchProductRouteResList[0].resource.cDayPlanShowType != "")
                    {
                        var schProductRouteRes = this.SchProductRoutePreList[0].SchProductRouteResList.Find(item => item.BScheduled == 1 && item.iResReqQty > 0 && item.resource.cDayPlanShowType != "");
                        if (schProductRouteRes != null)
                        {
                            foreach (var item in this.SchProductRouteResList)
                            {
                                if (item.resource.cDayPlanShowType != "" && !item.resource.cDayPlanShowType.Contains(schProductRouteRes.resource.cDayPlanShowType) && item.cSelected == "1")
                                {
                                    item.cSelected = "0";  //取消选择
                                    item.iResReqQty = 0;   //排程数量为0  排程数量不要取消，以免报错
                                    item.cDefine25 = "关联分组号不同取消:" + schProductRouteRes.resource.cDayPlanShowType;  //取消选择备注
                                }
                            }
                        }
                        List<SchProductRouteRes> ListRouteRes2 = this.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty > 0 ; });
                        if (ListRouteRes2.Count < 1)
                        {
                            ListRouteRes[0].cSelected = "1";
                            ListRouteRes[0].iResReqQty = this.iReqQty;   //排程数量
                        }
                    }
                }
            }
            ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });
            int iResCount = ListRouteRes.Count;
            if (iResCount <= 1) return 1;
            if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1 )
                iResCount = this.iDevCountPd;
            ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.iCapacity/p1.iBatchQty, p2.iCapacity/p1.iBatchQty); });
            double iCapacity = ListRouteRes[0].iCapacity;
            double iBatchQty = ListRouteRes[0].iBatchQty;
            int iMinResCount = 1;        
            if (ListRouteRes.Count > 1)  //this.cDevCountPdExp.Length < 1  &&  this.iDevCountPd < 1
            {                
                if (this.schProduct.iWorkQtyPd > 0 && this.schProduct.iWorkQtyPd < this.iReqQty )
                {
                    double iDevCount = this.schProduct.iWorkQtyPd * iCapacity  / 3600 / ListRouteRes[0].resource.iResHoursPd / ListRouteRes[0].resource.iResourceNumber / ListRouteRes[0].iBatchQty;
                    if (this.item != null && this.item.iItemDifficulty > 0 && this.item.iItemDifficulty != 1 ) iDevCount = iDevCount * this.item.iItemDifficulty;
                    if (ListRouteRes[0].resource.iResDifficulty > 0 && ListRouteRes[0].resource.iResDifficulty != 1) iDevCount = iDevCount * ListRouteRes[0].resource.iResDifficulty;
                    if (this.techInfo.iTechDifficulty > 0 && this.techInfo.iTechDifficulty != 1) iDevCount = iDevCount * this.techInfo.iTechDifficulty;
                    iMinResCount = Convert.ToInt32(Math.Floor(iDevCount)); //Ceiling
                    iResCount = iMinResCount;
                }
                else
                {
                    if (ListRouteRes[0].resource.iMinWorkTime > 0 )  
                        iMinResCount = Convert.ToInt32(this.iReqQty * iCapacity/ iBatchQty / 3600 / ListRouteRes[0].resource.iMinWorkTime);  //SchParam.iTaskMinWorkTime
                    else
                        iMinResCount = iResCount;
                }
            }
            if (iResCount > iMinResCount && iMinResCount <= ListRouteRes.Count)
                iResCount = iMinResCount;
            if (iResCount < 1 && ListRouteRes.Count > 0) iResCount = 1;
            if (this.item != null && this.item.iMoldCount > 0 && iResCount > this.item.iMoldCount)
                iResCount = this.item.iMoldCount;
            double iResReqQtyPer = this.iReqQty ;  //小数取整，尾差留到最后一批
            if (iResCount > 1)
            {
                iResReqQtyPer = (int)this.iReqQty / iResCount;  //小数取整，尾差留到最后一批
            }
            double iLeftReqQty = this.iReqQty;
            double iResReqQty = iResReqQtyPer;
            if (SchParam.cMutResourceType != "4")  
            {
                try
                {
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        if (ListRouteRes[i].cCanScheduled != "1") continue;
                        ListRouteRes[i].TestResSchTask(ref iResReqQty, dCanBegDate);
                    }
                }
                catch (Exception error)
                {
                    if (SchParam.iSchSdID < 1)
                        throw new Exception("资源选择正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                    else
                        throw new Exception("资源选择正排出错,订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace);
                    return -1;
                }
            }
            int iSelectReturn = 0;
            if (iSelectReturn <= 0)
            {
                if (SchParam.cMutResourceType == "4")  //4 按资源已排程天数选择，越少越优先 2022-11-06
                {
                    ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.resource.iPlanDays, p2.resource.iPlanDays); });
                }
                else if (SchParam.cMutResourceType == "3" )  //3 按资源组排产优先级选择资源,在所选资源排不下时，选择下个优先级资源 
                {
                    if (ListRouteRes.Count > 1)
                    {
                        for (int i = 0; i < ListRouteRes.Count; i++)
                        {
                            if (ListRouteRes[i].cCanScheduled != "1") continue;
                            ListRouteRes[i].cDefine38 = ListRouteRes[i].dResEndDate.AddHours((ListRouteRes[i].iResGroupPriority + 1 - 1) * SchParam.iMutResourceDiffHour);
                        }
                        List<SchProductRouteRes> ListRouteResSort = (from c in ListRouteRes
                                                                     orderby c.cDefine38 ascending, c.iResGroupPriority ascending //descending  
                                                                     select c).ToList<SchProductRouteRes>();
                        ListRouteRes = ListRouteResSort;
                    }
                }
                else if (SchParam.cMutResourceType == "2") //最早可完工日期资源优先选择
                {
                    ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                }
                else
                {
                    ListRouteRes.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResEndDate, p2.dResEndDate); });
                }
                List<string> ResSelectList = new List<string>();
                List<string> MoldSelectList = new List<string>();
                if (ListRouteRes.Count > 1)  //&& (this.cVersionNo.ToLower()) != "sureversion" 
                {
                    int j = 0;
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        if (ListRouteRes[i].cCanScheduled != "1") continue;
                        if (j <= iResCount)
                        {
                            if (ResSelectList.Count > 0)
                            {
                                string ResSelect = ResSelectList.Find(delegate (string s) { return s.Equals(ListRouteRes[i].cResourceNo); });
                                if (ResSelectList.Count > 0 && ResSelect != null && ResSelect != "")
                                {
                                    ListRouteRes[i].cCanScheduled = "0";   //不排产 
                                    ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                                    ListRouteRes[i].iResRationHour = 0;    //加工工时为0 
                                    continue;
                                }
                            }
                            ResSelectList.Add(ListRouteRes[i].cResourceNo);
                        }
                        j++;
                        if (j > iResCount)
                        {
                            ListRouteRes[i].cCanScheduled = "0";   //不排产 
                            ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                            ListRouteRes[i].iResRationHour = 0;    //加工工时为0 
                        }
                    }
                }
            }
            ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });
            iResCount = ListRouteRes.Count;
            if (iResCount < 1)  //没有可用资源
            {
                throw new Exception(string.Format("多资源选择正排出错,订单行号：{0}，没有找到已选择的可排产资源，资源产能明细总资源数量为{1},初始选择可排资源数量为{2}，位置SchProductRoute.ProcessSchTask！工序ID号：{3},物料编号{4}",this.iSchSdID.ToString(),iRouteResCountFirst.ToString(),this.SchProductRouteResList.Count.ToString(), this.cInvCode));
                return -1;
            }
            ResReqQtyDispatch();
            return 1;
        }
        public int ResReqQtyDispatch()
        {
            List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });
            int iResCount = ListRouteRes.Count;
            if (iResCount <= 1) return 0;
            double iResReqQtyPer = (int)(this.iReqQty - this.iActQty) / iResCount;  //小数取整，尾差留到最后一批
            double iLeftReqQty = this.iReqQty - this.iActQty;
            double iResReqQty = iResReqQtyPer;
            if (iResReqQtyPer < 1)
            {
                iResCount = 1;
                iResReqQtyPer = (this.iReqQty - this.iActQty);
            }
            for (int i = 0; i < ListRouteRes.Count; i++)
            {
                if (iLeftReqQty <= 0) continue;
                if (i == ListRouteRes.Count)
                    iResReqQty = iLeftReqQty;
                else
                {
                    if (ListRouteRes[i].iBatchQtyBase > 1 && ListRouteRes.Count > 1)
                    {
                        iResReqQty = Math.Ceiling(iResReqQtyPer / ListRouteRes[i].iBatchQtyBase) * ListRouteRes[i].iBatchQtyBase;
                    }
                    else
                    {
                        iResReqQty = iResReqQtyPer;
                    }
                }
                if (iLeftReqQty - iResReqQty > 0)
                {
                    iLeftReqQty = iLeftReqQty - iResReqQty;
                }
                else
                {
                    iResReqQty = iLeftReqQty;
                    iLeftReqQty = 0;
                }
                if (this.iSchBatch == 1) //已执行生产任务单,资源计划数量不变,已减去已完工数量 2014-11-04
                {
                    iResReqQty = ListRouteRes[i].iResReqQty; //- ListRouteRes[i].iActResReqQty;                       
                }
                if (iLeftReqQty > 0 && i == ListRouteRes.Count - 1)
                {
                    iResReqQty += iLeftReqQty;
                }
                ListRouteRes[i].iResReqQty = iResReqQty;
            }
            return 1;
        }
        public int ResourceSelectRev(DateTime dCanBegDate, Boolean bFreeze = false)
        {
            List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });
            if (bFreeze || this.iActQty > 0)  //|| this.cWoNo != "" 冻结状态 或者完工数量大于0 ,已开工生产的，不能换资源
            {
                for (int i = 0; i < ListRouteRes.Count; i++)
                {
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
                if (ListRouteRes.Count > 0)   //有多道选择工序
                {
                    if (this.SchProductRouteNextList.Count == 1 && this.SchProductRouteNextList[0].cInvCode == this.cInvCode)
                    {
                        var schProductRouteRes = this.SchProductRouteNextList[0].SchProductRouteResList.Find(item => item.BScheduled == 1 && item.iResReqQty > 0 && item.resource.cDayPlanShowType != "");
                        if (schProductRouteRes != null)
                        {
                            foreach (var item in this.SchProductRouteResList)
                            {
                                if (item.resource.cDayPlanShowType != "" && !(schProductRouteRes.resource.cDayPlanShowType.Contains(item.resource.cDayPlanShowType) || item.resource.cDayPlanShowType.Contains(schProductRouteRes.resource.cDayPlanShowType)) && item.cSelected == "1")
                                {
                                    item.cSelected = "0";  //取消选择
                                    item.iResReqQty = 0;   //排程数量为0
                                    item.cDefine25 = "关联分组号不同取消:" + schProductRouteRes.resource.cDayPlanShowType;  //取消选择备注
                                }
                            }
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
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
            }
            int iResCount = ListRouteRes.Count;
            if (iResCount <= 1) return 1;
            if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1)
                iResCount = this.iDevCountPd;
            ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.iCapacity, p2.iCapacity); });
            double iCapacity = ListRouteRes[0].iCapacity;
            int iMinResCount = 1;
            if (ListRouteRes.Count > 1 && this.cDevCountPdExp.Length < 1)
            {
                if (this.schProduct.iWorkQtyPd > 0 && this.schProduct.iWorkQtyPd < this.iReqQty)
                {
                    double iDevCount = this.schProduct.iWorkQtyPd * iCapacity / 3600 / ListRouteRes[0].resource.iResHoursPd / ListRouteRes[0].resource.iResourceNumber/ ListRouteRes[0].iBatchQty;
                    if (this.item != null && this.item.iItemDifficulty > 0 && this.item.iItemDifficulty != 1) iDevCount = iDevCount * this.item.iItemDifficulty;
                    if (ListRouteRes[0].resource.iResDifficulty > 0 && ListRouteRes[0].resource.iResDifficulty != 1) iDevCount = iDevCount * ListRouteRes[0].resource.iResDifficulty;
                    if (this.techInfo.iTechDifficulty > 0 && this.techInfo.iTechDifficulty != 1) iDevCount = iDevCount * this.techInfo.iTechDifficulty;
                    iMinResCount = Convert.ToInt32(Math.Floor(iDevCount));  //Ceiling
                    iResCount = iMinResCount;
                }
                else
                {
                    if (SchParam.iTaskMinWorkTime > 0)
                        iMinResCount = Convert.ToInt32(this.iReqQty * iCapacity / 3600 / SchParam.iTaskMinWorkTime);
                    else
                        iMinResCount = iResCount;
                }
            }
            if (iResCount > iMinResCount && iMinResCount <= ListRouteRes.Count)
                iResCount = iMinResCount;
            if (iResCount < 1 && ListRouteRes.Count > 0) iResCount = 1;
            double iResReqQty = this.iReqQty; 
            int iSelectReturn = 0;
            if (iSelectReturn <= 0)
            {
                {
                    if (ListRouteRes.Count > 1)
                    {
                        if (SchParam.cMutResourceType == "4")  //4 按资源已排程天数选择，越少越优先 
                        {
                            ListRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.resource.iPlanDays, p2.resource.iPlanDays); });
                        }
                        else                                    // 按资源优先级排产
                        {
                            for (int i = 0; i < ListRouteRes.Count; i++)
                            {
                                if (ListRouteRes[i].cCanScheduled != "1") continue;
                                ListRouteRes[i].cDefine38 = ListRouteRes[i].dResEndDate.AddHours((ListRouteRes[i].iResGroupPriority - 1) * SchParam.iMutResourceDiffHour);
                            }
                            List<SchProductRouteRes> ListRouteResSort = (from c in ListRouteRes
                                                                         orderby c.cDefine38 ascending, c.iResGroupPriority ascending //descending  
                                                                         select c).ToList<SchProductRouteRes>();
                            ListRouteRes = ListRouteResSort;
                        }
                    }
                }
                if (ListRouteRes.Count > 1)
                {
                    int j = 0;
                    for (int i = 0; i < ListRouteRes.Count; i++)
                    {
                        if (ListRouteRes[i].cCanScheduled != "1") continue;
                        j++;
                        if (j > iResCount)
                        {
                            ListRouteRes[i].cCanScheduled = "0";   //不排产 
                            ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                            ListRouteRes[i].iResRationHour = 0;    //加工工时为0 
                        }
                    }
                }
            }
            ResReqQtyDispatch();
            return 1;
        }
        public int cBatchResourceSelect()
        {
            this.cBatchNoFlag = 1;
            List<SchProductRoute> ListBatchRoute = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.cVersionNo == this.cVersionNo && p.schProduct.cBatchNo == this.schProduct.cBatchNo && p.schProduct.cWorkRouteType == this.schProduct.cWorkRouteType &&  p.iWoSeqID == this.iWoSeqID && p.cBatchNoFlag ==0  && p.iSchBatch == this.iSchBatch; });  // p.iProcessID == this.iProcessID 
            if (ListBatchRoute.Count < 1) return 0;
            foreach (var batchRoute in ListBatchRoute)
            {
                for (int i = 0; i < batchRoute.SchProductRouteResList.Count; i++)
                {
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
                    batchRoute.cBatchNoFlag = 1;
                }
            }
            return 1;
        }
        public int cBatchSchRoute()
        {
            List<SchProductRoute> ListBatchRoute = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.cVersionNo == this.cVersionNo && p.schProduct.cBatchNo == this.schProduct.cBatchNo && p.schProduct.cWorkRouteType == this.schProduct.cWorkRouteType && p.iProcessID == this.iProcessID && p.iSchSdID != this.iSchSdID && p.cBatchNoFlag != 2; });
            if (ListBatchRoute.Count < 1) return 0;
            if (this.cWoNo == "" && this.SchProductRouteResList.Count > 1)
                cBatchResourceSelect();
            foreach (var batchRoute in ListBatchRoute)
            {
                batchRoute.cBatchNoFlag = 2;
                batchRoute.ProcessSchTask();
            }
            return 1;
        }
        public int ProcessSchTaskPre(Boolean bCurTask = true,Boolean bFreeze = false)  //
        {
            try
            { 
                foreach (SchProductRoute schProductRoute in SchProductRoutePreList)
                {
                    if (schProductRoute.bScheduled == 0)
                        schProductRoute.ProcessSchTaskPre(bCurTask, bFreeze);               
                }
                if (bCurTask)
                {
                    SchParam.ldtBeginDate = DateTime.Now;
                    ProcessSchTask(bFreeze);
                    SchParam.iWaitTime = DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;
                }
            }
            catch (Exception error)
            {
                if (SchParam.iSchSdID< 1)
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                else
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace); 
                return -1;
            }
            return 1;
        }
        public int ProcessSchTaskNext(string cTag = "1")  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排;4 只排后续工序一道工序,用于油漆后续工序排产,每遍排一次
        {
            try
            {
                if (this.bScheduled == 0)
                    ProcessSchTask();
                if (SchProductRouteNextList.Count < 1) return 1;
                SchProductRoute schProductRouteNext = SchProductRouteNextList[0];
                if (schProductRouteNext == null)
                {
                    throw new Exception("订单行号：" + this.iSchSdID + "请检查产品[" + this.cInvCode + "]加工物料[" + this.cWorkItemNo + "]工序号[" + this.iWoSeqID.ToString() + "]工艺路线是否完整!");
                    return -1;
                }
                if (cTag == "1")        //1 加工物料相同的所有工序,排完
                {
                    if (schProductRouteNext.cInvCode == this.cInvCode) //排到关键资源为止
                    {
                        schProductRouteNext.ProcessSchTaskNext("1");
                    }
                }
                else if (cTag == "2")   //2 截止到同一加工物料的油漆工序 50  iWoSeqID < 50 双叶特殊处理，先不用
                {
                    if (schProductRouteNext.cInvCode == this.cInvCode && schProductRouteNext.iWoSeqID < 50)
                    {
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
                if (SchParam.iSchSdID < 1)
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                else
                    throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + "明细信息:" + error.StackTrace); 
                return -1;
            }
            return 1;
        }
        public int ProcessSchTaskRevPre(string cTag = "1",string bSet = "0") //1 加工物料相同的所有工序; 2 白茬工序; 3 所有下层物料半成品工序
        {
            if (cTag == "2")   //2 截止到白茬工序
            {
                if (this.iWoSeqID < 10 && this.bScheduled == 1) return 0;  //工序已排，而且是白茬工序，退出
            }
            if (this.iReqQty <= 0)
            {
                this.bScheduled = 1; //设置为已排产
                return 0;
            }
            ProcessSchTaskRev(bSet);           
            foreach (SchProductRoute schProductRoute in SchProductRoutePreList)
            {
                if (cTag == "1")        //1 加工物料相同的所有工序,排完
                {
                    if (schProductRoute.cInvCode != this.cInvCode) continue;
                    schProductRoute.ProcessSchTaskRevPre("1");
                }
                else if (cTag == "2")   //2 截止到白茬工序,油漆和白茬， 油漆工序从50工序开始 双叶特殊处理
                {
                    if (schProductRoute.iWoSeqID < 50) continue;
                    schProductRoute.ProcessSchTaskRevPre("2");
                }
                else if (cTag == "3")
                {
                    schProductRoute.ProcessSchTaskRevPre("3");
                }
            }
            return 1;
        }
        public int ProcessSchTaskRev(string bSet = "0" )
        {
            DateTime dDateTemp = this.schData.dtStart;//DateTime.Now;
            DateTime dCanEndDate = this.schData.dtStart;//DateTime.Now;    //有多道前序工序时，找最大的可开工时间
            if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
            {
                int j;
            }
            try
            {
                if (this.SchProductRouteNextList.Count > 0)
                {
                    foreach (SchProductRoute schProductRoute in SchProductRouteNextList)
                    {
                        dDateTemp = schProductRoute.GetPreProcessCanEndDate(this);
                        if (dCanEndDate < dDateTemp) dCanEndDate = dDateTemp;
                    }
                    if (bSet == "1") 
                    {
                        if (dCanEndDate <= this.dEndDate)
                        {
                            this.ProcessSchTask();
                            return 1;
                        }
                    }
                    if (dCanEndDate < this.schData.dtStart)   //出错
                    {
                        throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：" + this.iSchSdID + "工序ID号：" + this.iProcessProductID + "\n\r " + string.Format("倒排开始时间{0}小于排产开始日期{1},无法继续倒排!", dCanEndDate, this.schData.dtStart));
                        return -1;
                    }
                }
                else  //无后道工序，取产品的完工日期
                {
                    dCanEndDate = this.schProduct.dDeliveryDate.Date;   //以交货日期为准,提前一天完工
                    if (dCanEndDate <= this.schData.dtStart )   //倒排开始时间小于排产开始日期，产品需求日期太小，出错
                    {
                        dCanEndDate = this.schData.dtStart.AddMonths(1);      //加一个月
                    }
                }
                if (this.bScheduled == 1)
                    ProcessClearTask();
                if (this.iProcessProductID == SchParam.iProcessProductID && this.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }
                ResourceSelectRev(dCanEndDate);
                List<SchProductRouteRes> ListRouteRes2 = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "1"; });
                int iResCount = ListRouteRes2.Count;
                if (iResCount < 1)  //没有可用资源
                {
                    List<SchProductRouteRes> ListRouteResCan = SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1" && p.cCanScheduled == "0"; });
                    if (ListRouteResCan.Count < 1)
                    {
                        throw new Exception("订单行号：" + this.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + this.iProcessProductID + "\n\r " + "没有可排产资源编号.或排产范围太小,最早可排开始日期：'" + dCanEndDate.ToLongDateString() + "'");
                        return -1;
                    }
                    else
                    {
                        foreach (var item in ListRouteResCan)
                        {
                            item.BScheduled = 1;
                        }
                        return 1;
                    }
                }
                List<SchProductRouteRes> ListRouteRes = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cSelected == "1" && p.iResReqQty >0  ; });
                foreach (SchProductRouteRes LSchProductRouteRes in ListRouteRes)
                {
                    LSchProductRouteRes.TaskSchTaskRev(ref LSchProductRouteRes.iResReqQty, dCanEndDate);
                    if (SchProductRouteNextList != null && this.SchProductRouteNextList.Count > 0)
                    {
                        if ( LSchProductRouteRes.dResBegDate.Subtract(SchProductRouteNextList[0].dBegDate).TotalHours > 0 ) //&&  ListRouteRes[i].resource.cIsInfinityAbility != "1"
                        {
                            {
                                Double iDiffMinites = SchData.GetDateDiff("m",  SchProductRouteNextList[0].dBegDate,LSchProductRouteRes.dResBegDate);
                                if (iDiffMinites > 0 )
                                {
                                    dCanEndDate = LSchProductRouteRes.dResEndDate.AddMinutes(-iDiffMinites); //iDiffMinites
                                    LSchProductRouteRes.TaskClearTask();
                                    try
                                    {
                                        LSchProductRouteRes.TaskSchTaskRev(ref LSchProductRouteRes.iResReqQty, dCanEndDate);
                                    }
                                    catch (Exception error)
                                    {
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
                List<SchProductRouteRes> list1 = SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return p1.iSchSdID == this.iSchSdID && p1.iProcessProductID == this.iProcessProductID && p1.iResReqQty > 0  ; });
                if (list1.Count > 0)
                {
                    list1.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    this.dBegDate = list1[0].dResBegDate;                 //取已排资源任务排产时间的最小开始时间
                    this.dEndDate = list1[list1.Count - 1].dResEndDate;   //取已排资源任务排产时间的最大结束时间
                }
                int iLaborTime = 0;
                foreach (SchProductRouteRes lSchProductRouteRes in list1)
                {
                    iLaborTime += Convert.ToInt32(lSchProductRouteRes.iResRationHour);
                }
                this.iLaborTime = iLaborTime;
                this.BScheduled = 1;    //0 未排 1 已排
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + this.iSchSdID + "资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskRev！订单行号：" + this.iSchSdID + "工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1;
        }
        public DateTime GetNextProcessCanBegDate(SchProductRoute schProductRouteNext)
        {
            DateTime ldtFirstEndDate = this.dBegDate;     //后序工序可开工时间
            string cWorkType = "0";       //加工类型
            double liCapacity = 1;        //产能
            double iBatchQty = 1;         //加工批量
            try
            {
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
                if (this.cParellelType == "ES")     //ES 整批移转 
                {
                    if (SchParam.NextSeqBegTime > 0)  //前工序开工时间 + 后工序可开工时间为前工序开工后多长时间(分钟) 120
                    {
                        ldtFirstEndDate = this.dBegDate.AddMinutes(this.iMoveTime + this.iSeqPostTime + SchParam.NextSeqBegTime);
                        if (ldtFirstEndDate > this.dEndDate.AddMinutes(this.iMoveTime + this.iSeqPostTime))
                            ldtFirstEndDate = this.dEndDate.AddMinutes(this.iMoveTime + this.iSeqPostTime);
                    }
                    else
                        ldtFirstEndDate = this.dEndDate.AddMinutes(this.iMoveTime + this.iSeqPostTime);
                }                
                else if (this.cParellelType == "EE")    //EE 同时结束(差一个批次移转时间，可开工时间正常取，后工序排产时，如果提前早可以倒排)
                {
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
                if (schProductRouteNext != null)
                    ldtFirstEndDate = ldtFirstEndDate.AddMinutes(schProductRouteNext.iSeqPreTime);
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + this.iSchSdID + "资源计算时出错,位置SchProductRoute.GetNextProcessCanBegDate！订单行号：" + this.iSchSdID + "工序ID号：" + this.iProcessProductID + "\n\r " + error.Message.ToString() + " " + error.StackTrace.ToString());
            }
            return ldtFirstEndDate;
        }
        public double GetProcessMoveTime(SchProductRoute schProductRoutePre)
        {
            double iMoveTime = 0;
            double liCapacity = 1;        //产能
            if (schProductRoutePre.SchProductRouteResList.Count > 0)
            {
                liCapacity = schProductRoutePre.SchProductRouteResList[0].iCapacity;      //取第一个可用资源的产能
            }
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
        public DateTime GetPreProcessCanEndDate(SchProductRoute schProductRoutePre, string cType ="1")
        {
             DateTime ldtFirstEndDate;     //前序工序可完工时间
            Double iMoveTime = this.GetProcessMoveQty(schProductRoutePre);
            ldtFirstEndDate = this.dEndDate.AddMinutes(-this.iMoveTime - this.iSeqPreTime - iMoveTime);
            if (schProductRoutePre != null && schProductRoutePre.iSeqPostTime != null)
            {
                ldtFirstEndDate = ldtFirstEndDate.AddMinutes(-schProductRoutePre.iSeqPostTime);
                schProductRoutePre.cDefine28 = ";本工序后准备时间" + schProductRoutePre.iSeqPostTime;
                schProductRoutePre.cDefine28 += ";本工序可开工时间" + ldtFirstEndDate;
            }
            return ldtFirstEndDate;
        }
        public void ProcessClearTask()
        {
            foreach (SchProductRouteRes schProductRouteRes in this.SchProductRouteResList)
            {
                schProductRouteRes.TaskClearTask();
            }
            this.BScheduled = 0;
        }
        public void GetRouteEarlyBegDate()
        {
            DateTime dDateTemp = this.schData.dtStart;
            try
            {
                if (this.schProduct == null) return;
                if (this.iSchSdID == SchParam.iSchSdID )
                {
                    int j = 1;
                }
                if (this.iSchSdID == SchParam.iSchSdID && this.iProcessProductID == SchParam.iProcessProductID)
                {
                    int j = 1;
                }
                this.cDefine27 = "dEarlyBegDate:" + this.dEarlyBegDate;  //清空前准备时间
                if (this.SchProductRoutePreList != null && this.SchProductRoutePreList.Count > 0)
                {
                    foreach (SchProductRoute schProductRoutePre in SchProductRoutePreList)
                    {
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
                    dDateTemp = SchParam.dtStart.AddMinutes(this.iSeqPreTime);
                    if (this.dEarlyBegDate < dDateTemp)
                    {
                        this.dEarlyBegDate = dDateTemp;
                        this.cDefine27 += ";本工序前准备时间" + this.iSeqPreTime + ":" + dDateTemp;
                    }
                }
                if (SchParam.cPurEndDate == "1" && this.cVersionNo != "SureVersion")
                {
                    if (this.techInfo != null && (this.techInfo.iOrder == 0 || this.techInfo.iOrder == 3))
                    {
                        DateTime dPurEarliestSchDate = this.schData.dtStart;
                        List<SchProductRouteItem> SchProductRouteItemListPur = SchProductRouteItemList.FindAll(delegate (SchProductRouteItem p) { return p.bSelf != "1" && p.cInvCode == this.cWorkItemNo && p.cInvCodeFull == this.cWorkItemNoFull; });
                        if (SchProductRouteItemListPur != null && SchProductRouteItemListPur.Count > 0)
                        {
                            foreach (SchProductRouteItem schProductRouteItem in SchProductRouteItemListPur)
                            {
                                if (schProductRouteItem.iReqQty > 0 && schProductRouteItem.dEarlySubItemDate > dPurEarliestSchDate)
                                    dPurEarliestSchDate = schProductRouteItem.dEarlySubItemDate;
                            }
                            if (this.dEarlyBegDate < dPurEarliestSchDate)
                            {
                                this.dEarlyBegDate = dPurEarliestSchDate;
                                this.cDefine27 += ";采购件最晚到料日期" + ":" + dPurEarliestSchDate;
                            }
                        }
                    }                    
                }
                if (this.item != null && this.item != null && this.item.iAdvanceDate > 0)
                {
                    dDateTemp = this.schProduct.dEarliestSchDate.AddDays(this.item.iAdvanceDate);
                    if (this.dEarlyBegDate < dDateTemp)
                    {
                        this.dEarlyBegDate = dDateTemp;
                        this.cDefine27 += ";产品采购提前期" + this.item.iAdvanceDate + ":" + dDateTemp;
                    }
                }
            }
            catch (Exception error)
            {
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