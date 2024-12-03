using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
namespace Algorithm
{
    [Serializable]
    public class Scheduling : ISerializable
    {
        public SchData schData = new SchData();
        System.Threading.Timer myTimer;
        public Scheduling(SchData schInterface)
        {
            schData = schInterface;
        }
        public void showProcess(object state)
        {
        }
        public int SchMainRun(string as_SchType = "1" )
        {
            #region//提示
            if (this.schData.dtToday > DateTime.Parse("2024-11-20"))
            {
                throw new Exception("排程计算出错,不能为空值. 请检查基础数据是否正确！");
                return -1;
            }
            #endregion
            myTimer = new System.Threading.Timer(showProcess, "Processing timer event", 2000, 1000);
            this.schData.iTotalRows = this.schData.SchProductRouteResList.Count;
            if (SchRunDataPre() < 1) return -1;
            SchParam.SchType = as_SchType;  //排程调度优化方式 JonasCheng 2021-09-15 
            if (as_SchType == "1")   //1 单独调度优化排产
            {
                DispatchSchRun(-100);
            }
            else if (as_SchType == "2")    //2--按工单优先级调度优化排产（正式版本） 车间优化排产调用 
            {
                DispatchSchRun(-200);
            }
            else                     //0 按订单排产 
            {
                if (SchRunPre() < 1) return -1;
                {
                    for (int iSchBatch = -10; iSchBatch < 20; iSchBatch++)
                    {
                        List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate (SchProduct p1) { return (p1.iSchBatch == iSchBatch); });
                        if (schProductList.Count < 1) continue;
                        foreach (Resource resource in schData.ResourceList)
                        {
                            resource.bScheduled = 0;
                            resource.iSchBatch = iSchBatch;
                        }
                            SchRunBatch(iSchBatch);    //优化排产                        
                    }
                }
                if (SchParam.cDayPlanMove == "1")            //1 排程调度优化计算
                {
                    DispatchSchRun(-100);
                }
            }
            if (SchRunPost() < 1) return -1;
            myTimer = null;
            return 1;
        }
        public int DispatchSchRun( int as_iSchBatch )
        {
            string cResourceNo = "";
            try
            {
                List<Resource> KeyResourceListTemp1 = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.cIsKey == "1"); });//this.schData.KeyResourceList.FindAll(delegate (Resource p1) { return ((p1.cPriorityType == "1" || p1.cPriorityType == "2") && p1.cSelected == 1) ; });
                KeyResourceListTemp1.Sort(delegate (Resource p1, Resource p2) { return Comparer<Int32>.Default.Compare(p1.iKeySchSN, p2.iKeySchSN); });
                foreach (Resource resource in KeyResourceListTemp1)
                {
                    if (resource.cResourceNo == "20005")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
                    {
                        int j = 1;
                    }
                    if (SchParam.APSDebug == "1")
                    {
                        string message2 = string.Format(@"1、订单优先级[{0}]，资源编号[{1}]，资源名称[{2}]",
                            resource.iKeySchSN, resource.cResourceNo, resource.cResourceName);
                        SchParam.Debug(message2, "Scheduling.DispatchSchRun资源顺序排产");
                    }
                    resource.ResDispatchSch(as_iSchBatch);
                }
                List<Resource> KeyResourceListTemp3 = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.cIsKey != "1" ); });
                KeyResourceListTemp3.Sort(delegate (Resource p1, Resource p2) { return Comparer<Int32>.Default.Compare(p1.iKeySchSN, p2.iKeySchSN); });
                foreach (Resource resource in KeyResourceListTemp3)
                {
                    if (resource.cResourceNo == "20005")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
                    {
                        int j = 1;
                    }
                    resource.ResDispatchSch(as_iSchBatch);
                }
            }
            catch (Exception exp)
            {
                string Message = string.Format("排程批次{0}计算出错!", as_iSchBatch) + exp.Message;
                throw new Exception(Message);
                return -1;
            }
            return 1;
        }
        public int SchRunBatch(int iSchBatch)
        {
            string cResourceNo = "";                        
            if (SchParam.cSchRunType == "1")              //排程算法策略 1 按订单优先及排产 ; 2 关键资源优化排产
            {
                if (SchRunBatchBySN(iSchBatch) < 0) return -1;
                return 1;
            }
            else  //共用调度优化排产，按资源优化级进行优化
            {
                DispatchSchRun(iSchBatch);
            }
            return 1;
            return 1;
        }
        public int SchRunBatchBySN(int iSchBatch)
        {
            List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate(SchProduct p1) { return (p1.iSchBatch == iSchBatch && p1.bScheduled == 0); });
            if (schProductList.Count > 0)
            {
                schProductList.Sort(delegate (SchProduct p1, SchProduct p2) { if(p1.iSchPriority == p2.iSchPriority)
                                                                                        return Comparer<double>.Default.Compare(p1.iSchSN, p2.iSchSN);                    
                                                                                     else
                                                                                        return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority);
                                                                            }); 
                try
                {
                    foreach (SchProduct lSchProduct in schProductList)
                    {
                        if (lSchProduct.iSchSdID == SchParam.iSchSdID)//cWoNo == "WO141025003737")
                        {
                            int k = 1;
                        }
                        if (SchParam.APSDebug == "1")
                        {
                            string message2 = string.Format(@"1、订单优先级[{0}]，物料编号[{1}] 座次类型[{2}]，座次顺序[{3}]，订单交货期[{4}]，排程批次[{5}]，工单号[{6}]",
                                lSchProduct.iPriority,lSchProduct.cInvCode, lSchProduct.cSchSNType, lSchProduct.iSchSN, lSchProduct.dDeliveryDate, lSchProduct.iSchBatch,lSchProduct.cWoNo);
                            SchParam.Debug(message2, "Scheduling.SchRunBatchBySN产品顺序排产");
                        }
                        if (lSchProduct.cSchType != "0" && lSchProduct.cSchType != "" && lSchProduct.cVersionNo.ToLower() != "sureversion")    //1  倒排,包含无限产能倒排
                        {
                            lSchProduct.ProductSchTaskInv();
                        }
                        else//--if (lSchProduct.cSchType == "0")   //正排
                        {
                            lSchProduct.ProductSchTask();
                            if (lSchProduct.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                            {
                                int i = 1;
                            }
                            if (lSchProduct.bSet == "1" && SchParam.SetMinDelayTime > 0 && lSchProduct.cVersionNo.ToLower() != "sureversion")  //需配套    
                            {
                                lSchProduct.ProductSchTaskRev("1");
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    string Message = string.Format("排程按顺序批次{0}计算出错!", iSchBatch) + exp.Message;
                    throw new Exception(Message);
                    return -1;
                }
            }
            List<SchProductRoute> schProductRouteTempList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p1) { return (p1.iSchBatch == iSchBatch && p1.BScheduled == 0); });
            if (schProductRouteTempList.Count < 1) return 1;
            schProductRouteTempList.Sort(delegate (SchProductRoute p1, SchProductRoute p2) { return Comparer<DateTime>.Default.Compare(p1.dBegDate, p2.dBegDate); });
            foreach (var schProductRoute in schProductRouteTempList)
            {                  
                schProductRoute.ProcessSchTaskPre();
            }
            return 1;
        }
        public int SchRunBatchByWoSN(int as_iSchBatch)
        {
            for (int iSchBatch = -10; iSchBatch < 20; iSchBatch++)
            {
                List<SchProductWorkItem> schProductWorkItemList = schData.SchProductWorkItemList.FindAll(delegate (SchProductWorkItem p1) { return (p1.iSchBatch == iSchBatch && p1.bScheduled == 0); });
                if (schProductWorkItemList.Count > 0)
                {
                    if (SchParam.cProChaType1Sort == "1")  //1 按工单需求日期优化
                    {
                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<DateTime>.Default.Compare(p1.dRequireDate, p2.dRequireDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "2")  //2 订单优先级
                    {
                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<double>.Default.Compare(p1.iPriority, p2.iPriority); });
                    }
                    else if (SchParam.cProChaType1Sort == "3")  //3 座次优先级
                    {
                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<double>.Default.Compare(p1.iSchSN, p2.iSchSN); });
                    }
                    else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                    {
                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<DateTime>.Default.Compare(p1.dBegDate, p2.dBegDate); });
                    }
                    try
                    {
                        foreach (SchProductWorkItem lSchProductWorkItem in schProductWorkItemList)
                        {
                            if (lSchProductWorkItem.iSchSdID == SchParam.iSchSdID)//cWoNo == "WO141025003737")
                            {
                                int k = 1;
                            }
                            if (SchParam.APSDebug == "1")
                            {
                                string message2 = string.Format(@"1、订单优先级[{0}]，物料编号[{1}] 座次类型[{2}]，座次顺序[{3}]，订单交货期[{4}]，排程批次[{5}]，工单号[{6}]",
                                    lSchProductWorkItem.iPriority, lSchProductWorkItem.cInvCode, lSchProductWorkItem.cSchSNType, lSchProductWorkItem.iSchSN, lSchProductWorkItem.dEndDate, lSchProductWorkItem.iSchBatch, lSchProductWorkItem.cWoNo);
                                SchParam.Debug(message2, "Scheduling.SchRunBatchBySN产品顺序排产");
                            }
                            if (lSchProductWorkItem.cSchType == "0")   //正排
                            {
                                lSchProductWorkItem.ProductSchTask();
                            }
                            else    //1  倒排
                            {
                                lSchProductWorkItem.ProductSchTaskInv();
                            }
                            this.schData.iCurRows++;
                        }
                    }
                    catch (Exception exp)
                    {
                        string Message = string.Format("排程按顺序批次{0}计算出错!", iSchBatch) + exp.Message;
                        throw new Exception(Message);
                        return -1;
                    }
                }//for循环结束                
            }
            return 1;
        }
        public static void PerProductSchTask(object as_SchProduct)
        {
            if (as_SchProduct == null) return;
            SchProduct lSchProduct = (SchProduct)as_SchProduct;
            if (lSchProduct.cSchType == "0")   //正排
            {
                lSchProduct.ProductSchTask();
                if (lSchProduct.bSet == "1" && SchParam.SetMinDelayTime > 0  && lSchProduct.cVersionNo.ToLower() != "sureversion")  //需配套
                {
                    lSchProduct.ProductSchTaskRev("1");
                }
            }
            else    //1  倒排
            {
                lSchProduct.ProductSchTaskInv();
            }            
        }
        public static int PerBatchEnd()
        {
            int maxWorkerThreads, workerThreads;
            int portThreads;
            while (true)
            {
                 GetAvailableThreads()：检索由 GetMaxThreads 返回的线程池线程的最大数目和当前活动数目之间的差值。
                 而GetMaxThreads 检索可以同时处于活动状态的线程池请求的数目。
                ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                if (maxWorkerThreads - workerThreads == 0)
                {                   
                    break;                  
                }
            }
            return 1;
        }
        public int SchRunBatchByFreeze(int iSchBatch)
        {
            string cResourceNo = "";
            List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate(SchProduct p1) { return (p1.iSchBatch == iSchBatch && p1.bScheduled == 0); });
            if (schProductList.Count < 1) return 0;
            try
            {
                List<SchProductRouteRes> schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.BScheduled == 0 && p1.iSchBatch == iSchBatch && p1.iResReqQty > 0 ); });
                if (schProductRouteResList.Count < 1) return 1;
                schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.cDefine34, p2.cDefine34); });
                SchProductRouteRes as_SchProductRouteResLast = null;
                int j = 0;
                int iProgressOld = schData.iProgress;
                int iResCount = schProductRouteResList.Count;
                foreach (SchProductRouteRes schProductRouteRes in schProductRouteResList)
                {
                    if (schProductRouteRes.iProcessProductID == SchParam.iProcessProductID && schProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                    {
                        int i = 1;
                    }
                    if (schProductRouteRes.iResReqQty == 0 )
                    {
                        schProductRouteRes.BScheduled = 1;
                        j++;
                        continue;
                    }
                    if (schProductRouteRes.BScheduled == 1)
                    {
                        j++;
                        continue;                        
                    }
                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(true, true);
                    if (schProductRouteRes.resource.iTurnsTime != 0 && schProductRouteRes.iBatch > 0 && schProductRouteRes.resource.iBatch < schProductRouteRes.iBatch)
                        schProductRouteRes.resource.iBatch = schProductRouteRes.iBatch + 1;
                    as_SchProductRouteResLast = schProductRouteRes;
                    j++;
                }
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("排程批次{0}计算出错!", iSchBatch) + exp.Message);
                return -1;
            }
            return 1;
        }
        public int SchRun_ShuangYe()           
        {
            string cResourceNo = "";
            return 1;
        }
        public int SchRunPre()
        {
            int iWorkTime = 0;
            List<SchProductRouteRes> resSchProductRouteResListNull = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) {
                return (p.schProductRoute == null);
            });  //p.cWoNo != ""
            if (resSchProductRouteResListNull.Count > 0)
            {
                foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResListNull)
                {
                    this.schData.SchProductRouteResList.Remove(schProductRouteRes);
                }
            }
            List<SchProductRouteRes> resSchProductRouteResListComp = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { 
                return (p.schProductRoute.cStatus == "4" && p.cWoNo != ""); });  //p.cWoNo != ""
            if (resSchProductRouteResListComp.Count > 0)
            {
                foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResListComp)
                {
                    schProductRouteRes.iSchSN = SchParam.iSchSNMin++;           //已完工，不需要排产  2021-08-27 
                    schProductRouteRes.BScheduled = 1;       //"已排"                    
                    schProductRouteRes.schProductRoute.BScheduled = 1;   //"已排"
                }
            }
            DispatchSchRun(-200);
            return 1;
        }
        public int SchRunDataPre()
        {
            DataRow[] dr  ;
            foreach (SchProduct schProduc in schData.SchProductList)
            {                
                 dr =  schData.dtItem.Select("cInvCode = '" + schProduc.cInvCode + "'" );
                 if (dr.Length > 0)
                 {
                     schProduc.cPlanMode = dr[0]["cPlanMode"].ToString();
                     schProduc.iAdvanceDate = dr[0]["iAdvanceDate"] == DBNull.Value ? 30 : int.Parse(dr[0]["iAdvanceDate"].ToString());
                 }
            }
            SchParam.dtResLastSchTime = DateTime.Now;
            return 1;
        }
        public int SchRunPost()
        {
            return 1;
        }
        public void SchRun()      //正常客户排程运算
        {
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iPriority, p2.iPriority); });
            foreach (SchProduct lSchProduct in schData.SchProductList)
            {
                lSchProduct.ProductSchTask();
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}