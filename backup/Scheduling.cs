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
        //排程基础数据已准备好
        public SchData schData = new SchData();
        System.Threading.Timer myTimer;


        public Scheduling(SchData schInterface)
        {
            
            schData = schInterface;

            //排产算法扩展
            //APSCustomer.AlgorithmExtend algorithmExtend;
            //schData.algorithmExtend = new APSCustomer.AlgorithmExtend(schData);
        }

        public void showProcess(object state)
        {
            // Console.WriteLine("{0} {1} keep running.", (string)state, ++TimesCalled);
        }
                  

        //排产入口函数
        public int SchMainRun(string as_SchType = "1" )
        {
            #region//提示
            //提示到期
            if (this.schData.dtToday > DateTime.Parse("2024-11-20"))
            {
                //throw new Exception("当前产品试用期已到. 如有需要,请联系震坤软件有限公司,Email: JonasCheng@zhenkunsoft.com！");
                throw new Exception("排程计算出错,不能为空值. 请检查基础数据是否正确！");
                return -1;
            }
            #endregion

            myTimer = new System.Threading.Timer(showProcess, "Processing timer event", 2000, 1000);
            // 第一个参数是：回调方法，表示要定时执行的方法，第二个参数是：回调方法要使用的信息的对象，或者为空引用，第三个参数是：调用 callback 之前延迟的时间量（以毫秒为单位），指定 Timeout.Infinite 以防止计时器开始计时。指定零 (0) 以立即启动计时器。第四个参数是：定时的时间时隔，以毫秒为单位


            //this.schData.iTotalRows = this.schData.SchProductList.Count;
            //this.schData.iTotalRows = this.schData.SchProductRouteList.Count;
            this.schData.iTotalRows = this.schData.SchProductRouteResList.Count;
            //以待排资源任务数量作为总数            

            //排产前数据准备
            if (SchRunDataPre() < 1) return -1;


            SchParam.SchType = as_SchType;  //排程调度优化方式 JonasCheng 2021-09-15 

            //排程调度优化运算 2020-08-24
            //1 单独调度优化排产  /排程方式  0 ---正常排产, 1--资源调度优化排产2020-08-25 ， 2--按工单优先级调度优化排产（正式版本）3 --按资源调度优化排产 )
            if (as_SchType == "1")   //1 单独调度优化排产
            {
                DispatchSchRun(-100);
            }
            else if (as_SchType == "2")    //2--按工单优先级调度优化排产（正式版本） 车间优化排产调用 
            {
                DispatchSchRun(-200);
                //SchRunBatchByWoSN(-100);
            }
            //else if (as_SchType == "3")   //3 --按资源调度优化排产（正式版本）
            //{
            //    DispatchSchRun(-100);
            //}           
            else                     //0 按订单排产 
            {

                //排产前处理,已下达、执行工序排产，优先执行,只处理冻结方式//1 冻结
                if (SchRunPre() < 1) return -1;

                //根据参数调用不同的算法
                //if (SchParam.cCustomer == "ShuangYe")   //双叶特殊算法
                {
                   
                    //包装任务单	计划排产优先级-4
                    //母工单	计划排产优先级1
                    //子工单	计划排产优先级1
                    //手工单	计划排产优先级-3
                    //包装子任务单	计划排产优先级-2

                    //select cDetailNote,cDefine1 from DataDictionary where  DictionaryName = 'cWoType',设置生产任务单排产顺序
                    //目前设置-4 包装子任务单 -3 手工工单 0 冻结生产任务单 1 正常生产任务单紧急 2 正常生产任务单y 6 本次新增计划

                    for (int iSchBatch = -10; iSchBatch < 20; iSchBatch++)
                    {
                        //本批是否有排产,没有则返回
                        List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate (SchProduct p1) { return (p1.iSchBatch == iSchBatch); });

                        if (schProductList.Count < 1) continue;


                        //1、先排已执行计划                
                        foreach (Resource resource in schData.ResourceList)
                        {
                            resource.bScheduled = 0;
                            resource.iSchBatch = iSchBatch;
                        }

                        //批次排产
                        //if (iSchBatch == 0)          //冻结排产
                        //    SchRunBatchByFreeze(iSchBatch);
                        //else
                            SchRunBatch(iSchBatch);    //优化排产                        
                    }

                }
                //else
                //    SchRun();

                //排程调度优化运算 2020-08-24
                if (SchParam.cDayPlanMove == "1")            //1 排程调度优化计算
                {
                    DispatchSchRun(-100);
                }
            }

            //排产后处理
            if (SchRunPost() < 1) return -1;

            myTimer = null;

            return 1;
           
        }


        //调度资源优化排产，按选择的关键资源排产顺序，优化排产 2020-08-20 ,iSchBatch -100 不考虑批次，全排产
        public int DispatchSchRun( int as_iSchBatch )
        {
           
            //同一条生产线，可以有多台关键设备，并设置排程顺序            
            string cResourceNo = "";



            //2 关键资源优化排产 继续
            try
            {


                //关键资源按排产优先级,排产顺序排序,只排排产优先级为1 最优,2 次优;只排选择参与排产的资源
                List<Resource> KeyResourceListTemp1 = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.cIsKey == "1"); });//this.schData.KeyResourceList.FindAll(delegate (Resource p1) { return ((p1.cPriorityType == "1" || p1.cPriorityType == "2") && p1.cSelected == 1) ; });

                //同一紧急度资源，按资源优先级统一排产
                KeyResourceListTemp1.Sort(delegate (Resource p1, Resource p2) { return Comparer<Int32>.Default.Compare(p1.iKeySchSN, p2.iKeySchSN); });


                foreach (Resource resource in KeyResourceListTemp1)
                {

                    ////检查关键资源排产优先级相同的,必须轮换生产。                    
                    if (resource.cResourceNo == "20005")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
                    {
                        int j = 1;

                        //continue;
                    }

                    //记录排产日志
                    if (SchParam.APSDebug == "1")
                    {
                        string message2 = string.Format(@"1、订单优先级[{0}]，资源编号[{1}]，资源名称[{2}]",
                            resource.iKeySchSN, resource.cResourceNo, resource.cResourceName);

                        SchParam.Debug(message2, "Scheduling.DispatchSchRun资源顺序排产");
                    }

                    //调度优化排产
                    resource.ResDispatchSch(as_iSchBatch);

                }

                //非关键资源列表
                List<Resource> KeyResourceListTemp3 = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.cIsKey != "1" ); });

                ////---------3、排产优先级3 批量等待,4 普通,5 最后,如包装计划排产 ---------------------------------

               // List<Resource> KeyResourceListTemp3 = this.schData.KeyResourceList.FindAll(delegate (Resource p1) { return ((p1.cPriorityType == "3" || p1.cPriorityType == "4" || p1.cPriorityType == "5") && p1.cSelected == 1); });

                //同一紧急度资源，按资源优先级统一排产
                KeyResourceListTemp3.Sort(delegate (Resource p1, Resource p2) { return Comparer<Int32>.Default.Compare(p1.iKeySchSN, p2.iKeySchSN); });

                foreach (Resource resource in KeyResourceListTemp3)
                {
                    if (resource.cResourceNo == "20005")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
                    {
                        int j = 1;

                        //continue;
                    }

                    //关键资源优化排产
                    //resource.KeyResSchTask();

                    //调度优化排产
                    resource.ResDispatchSch(as_iSchBatch);

                }


                ////3、按优先级排序，有些产品没有关键工序的，重新设置优先级

                ////schData.SchProductList.Sort(delegate (SchProduct p1, SchProduct p2) { return Comparer<int>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });

                ////int iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
                ////if (iSchPriority < 0) iSchPriority = 0;

                ////select cDetailNote,cDefine1 from DataDictionary where  DictionaryName = 'cWoType',设置生产任务单排产顺序
                ////目前设置-4 包装子任务单 -3 手工工单 0 冻结生产任务单 1 正常生产任务单紧急 2 正常生产任务单y 6 本次新增计划

                //for (int iSchBatch = -10; iSchBatch < 20; iSchBatch++)
                //{
                //    //本批是否有排产,没有则返回
                //    List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate (SchProduct p1) { return (p1.iSchBatch == iSchBatch); });

                //    if (schProductList.Count < 1) continue;


                //    //1、先排已执行计划                
                //    foreach (Resource resource in schData.ResourceList)
                //    {
                //        resource.bScheduled = 0;
                //        resource.iSchBatch = iSchBatch;
                //    }

                //    //批次排产
                //    //if (iSchBatch == 0)          //冻结排产
                //    //    SchRunBatchByFreeze(iSchBatch);
                //    //else
                //        SchRunBatch(iSchBatch);    //优化排产

                //    //0 冻结生产任务单，有一部分未冻结工序,需要和正常生产任务单一起排,更新iSchBatch为1
                //    if (iSchBatch == 0)
                //    {
                //        //本批是否有排产,没有则返回
                //        List<SchProduct> schProductListTemp = schData.SchProductList.FindAll(delegate (SchProduct p1) { return (p1.iSchBatch == 0); });

                //        if (schProductListTemp.Count > 0)
                //        {
                //            foreach (SchProduct schProduct in schProductListTemp)
                //            {
                //                schProduct.iSchBatch = 1;     //1 普通生产任务单，预留 1、2作为紧急插单用，为负数时,排在冻结之前 2019-11-26
                //                schProduct.bScheduled = 0;    //未排产
                //            }

                //        }
                //    }
                //}

            }
            catch (Exception exp)
            {
                string Message = string.Format("排程批次{0}计算出错!", as_iSchBatch) + exp.Message;
                throw new Exception(Message);
                return -1;
            }



            ////1 日计划合并到一起,按资源定义向前、向后、正常排产 
            //if (SchParam.cDayPlanMove == "1")
            //{
            //    //所有资源每天工作时间段集中处理,靠后，靠前，正常
            //    foreach (Resource resource in this.schData.ResourceList)
            //    {
            //        resource.ResSchTaskByOrder();
            //    }
            //}

            return 1;
        }

        //分批运算,已执行计划（1）, 待排计划（6）,按关键资源排产顺序，优化排产
        public int SchRunBatch(int iSchBatch)
        {
           
            //同一条生产线，可以有多台关键设备，并设置排程顺序           
            string cResourceNo = "";                        


            //排程算法策略 1 按订单优先及排产
            if (SchParam.cSchRunType == "1")              //排程算法策略 1 按订单优先及排产 ; 2 关键资源优化排产
            {
                //按产品优先级顺序正排，同一批计划，严格按订单优先级进行排产
                if (SchRunBatchBySN(iSchBatch) < 0) return -1;

                return 1;
            }
            else  //共用调度优化排产，按资源优化级进行优化
            {
                DispatchSchRun(iSchBatch);

            }

            return 1;
            

            ////2 关键资源优化排产 继续

            ////本批是否有排产,没有则返回
            //List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate(SchProduct p1) { return (p1.iSchBatch == iSchBatch && p1.bScheduled == 0 ); });

            //if (schProductList.Count < 1) return 0;

            //try
            //{

            //    //--------1、白茬关键资源工序按工艺特征排序，进行排产;记录产品的排产顺序,按先进先出原则排整个产品计划------------
            //    //for (int i = 0; i < this.schData.dtResResource.Rows.Count; i++)
            //    //{
            //    //    cResourceNo = this.schData.dtResResource.Rows[i]["cResourceNo"].ToString();
            //    //    Resource resource = schData.ResourceList.Find(delegate(Resource p1) { return (p1.cResourceNo == cResourceNo); });

            //    //调试用
            //    //List<SchProductRouteRes> schProductRouteResListTest = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == "YQ-17-07" && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID == 71 && p1.iSchBatch == 1 ); });

            //    //关键资源按排产优先级,排产顺序排序,只排排产优先级为1 最优,2 次优
            //    List<Resource> KeyResourceListTemp1 = this.schData.KeyResourceList.FindAll(delegate(Resource p1) { return (p1.cPriorityType == "1" || p1.cPriorityType == "2" ); });


            //    foreach (Resource resource in KeyResourceListTemp1)
            //    {
            //        //TaskTimeRange TaskTimeRange2 = ResTimeRangeList1[i].TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return (p1.iSchSdID == aSchProductRouteRes.iSchSdID && p1.iProcessProductID == aSchProductRouteRes.iProcessProductID); });

            //        //3 批量等待排产
            //        if (resource.iTurnsType == "3")
            //        {
            //            //resource.cPriorityType == "3";
            //            continue;
            //        }

            //        ////只排排产优先级为1 最优,2 次优
            //        //if (resource.cPriorityType != "1" && resource.cPriorityType != "2") continue;


            //        //如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
            //        if (resource.ResSchBefore() < 0)
            //        {
            //            continue;
            //        }

            //        //检查关键资源排产优先级相同的,必须轮换生产。
            //        //Resource resource = schData.ResourceList.Find(delegate(Resource p1) { return (p1.cResourceNo == cResourceNo); });
            //        List<Resource> ResourceList = new List<Resource>(10);
            //        //ResourceList = schData.ResourceList.FindAll(delegate(Resource p1) { return (p1.cIsKey == "1" && p1.iKeySchSN == resource.iKeySchSN && p1.iKeySchSN > 0 && p1.iTurnsType != "3" && p1.iTurnsType != ""&& p1.iTurnsType != "0"); });
            //        ResourceList = KeyResourceListTemp1.FindAll(delegate(Resource p1) { return (p1.cIsKey == "1" && p1.iKeySchSN == resource.iKeySchSN && p1.iKeySchSN > 0 && p1.iTurnsType != "3" && p1.iTurnsType != "" && p1.iTurnsType != "0"); });

            //        if (resource.cResourceNo == "3.04.28" || resource.cResourceNo == "BC-03-03")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
            //        {
            //            int j = 1;
            //        }

            //        ////关键资源有分组,轮换排产,第一个关键资源就已经把一组资源都已排完.
            //        if (resource.bScheduled == 1) continue;  //全部排完   2014.03.25

            //        //关键资源无分组
            //        if (ResourceList.Count <= 1)
            //        {
            //            //关键资源优化排产
            //            resource.KeyResSchTask();
            //        }
            //        else //关键资源有分组,轮换排产
            //        {
            //            int j = 0;
            //            Boolean bAllScheduled = true;

            //            //先对所有分组关键资源取未排任务,进行排序
            //            for (int m = 0; m < ResourceList.Count; m++)
            //            {
            //                ResourceList[m].GetNotSchTask();
            //            }

            //            //记录最后一次排产任务
            //            SchProductRouteRes SchProductRouteResLast = null;

            //            while (j < ResourceList.Count)
            //            {
            //                if (ResourceList[j].bScheduled != 1)  //有未排完
            //                {
            //                    if (ResourceList[j].KeyResSchTaskSingle(-1, ref SchProductRouteResLast) < 0)
            //                        ResourceList[j].bScheduled = 1;   //全部排完

            //                }
            //                else   //检查所有资源是否已排完
            //                {
            //                    bAllScheduled = true;

            //                    for (int k = 0; k < ResourceList.Count; k++)
            //                    {
            //                        if (ResourceList[k].bScheduled != 1)  //有未排完
            //                        {
            //                            bAllScheduled = false;
            //                            continue;
            //                        }
            //                    }

            //                    //所有资源全部排完
            //                    if (bAllScheduled)
            //                    {
            //                        break;
            //                    }
            //                }

            //                j++;
            //                if (j >= ResourceList.Count) j = 0;
            //            }
            //        }
            //    }
            //    //---------2、油漆工序排产,足一批240分钟才能开工, iTurnsType = "3" 批量等待排产,其他关键资源已排 ---------------------------------
              
            //    List<Resource> KeyResourceListTemp2 = this.schData.KeyResourceList.FindAll(delegate(Resource p1) { return (p1.iTurnsType == "3" ); });

            //    foreach (Resource resource in KeyResourceListTemp2)
            //    {
            //        //3 批量等待排产
            //        if (resource.iTurnsType != "3") continue;

            //        if (resource.cResourceNo == "YQ-14-01" && resource.iSchBatch > 0)  //"YQ-17-07","YQ-11-01"
            //        {
            //            int j = 1;
            //        }

                    
            //        //如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
            //        if (resource.ResSchBefore() < 0)
            //        {
            //            continue;
            //        }

            //        //油漆排产，不同颜色的分批排，轮流生产
            //        resource.KeyResBatch();
            //    }


            //    //---------3、排产优先级3 批量等待,4 普通,5 最后,如包装计划排产 ---------------------------------

            //    List<Resource> KeyResourceListTemp3 = this.schData.KeyResourceList.FindAll(delegate(Resource p1) { return (p1.cPriorityType == "3" || p1.cPriorityType == "4" || p1.cPriorityType == "5"); });

            //    foreach (Resource resource in KeyResourceListTemp3)
            //    {
            //        //3 批量等待排产
            //        if (resource.iTurnsType == "3") continue;


            //        //如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
            //        if (resource.ResSchBefore() < 0)
            //        {
            //            continue;
            //        }

            //        //检查关键资源排产优先级相同的,必须轮换生产。                    
            //        if (resource.cResourceNo == "BC-03-02")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
            //        {
            //            int j = 1;
            //        }

            //        ////关键资源有分组,轮换排产,第一个关键资源就已经把一组资源都已排完.
            //        if (resource.bScheduled == 1) continue;  //全部排完   2014.03.25
                  
            //        //关键资源优化排产
            //        resource.KeyResSchTask();
                   
            //    }




            //    //---------4、按产品工艺模型进行排产，之前已排过工序不重排,有些白茬件没有关键工序，在此排.-------------------
            //    //油漆工序要考虑配套，先正排一遍，找出最晚完工部件的完工时间，倒排其他部件油漆线的开工和完工时间。

            //    //产品订单按关键资源顺序进行排产


            //    //按优先级排序，有些产品没有关键工序的，重新设置优先级               

            //    //schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<int>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });

            //    //int iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            //    //if (iSchPriority < 0) iSchPriority = 0;

            //    schProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });

            //    double iSchPriority = schProductList[schProductList.Count - 1].iSchPriority;
            //    if (iSchPriority < 0) iSchPriority = 0;


            //    foreach (SchProduct lSchProduct in schProductList.FindAll(delegate(SchProduct p1) { return p1.iSchPriority < 0; }))
            //    {
            //        if (lSchProduct.iSchPriority < 0)
            //        {
            //            iSchPriority++;
            //            lSchProduct.iSchPriority = iSchPriority;
            //        }
            //    }

            //    //按产品优先级顺序正排
            //    if (SchRunBatchBySN(iSchBatch) < 0) return -1;
                    
            //    ////以关键资源任务优化排产顺序，决定其产品的排产顺序,部件先开工，先生产

            //    //schProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });


            //    //foreach (SchProduct lSchProduct in schProductList)
            //    //{
            //    //    //2.1 正排 调用产对象的排产方法ProductSchTask，再由他调以下各工序的排产

            //    //    //         所有产品工序全正排，之前已排过的工序不重排。
            //    //    if (lSchProduct.iSchSdID == SchParam.iSchSdID )//cWoNo == "WO141025003737")
            //    //    {
            //    //        int k = 1;
            //    //    }

            //    //    lSchProduct.ProductSchTask();


            //    //    //2.2 倒排 需配套的产品，找出最晚完工部件的完工时间，倒排至白茬工序。

            //    //    if (lSchProduct.bSet == "1")  //需配套
            //    //    {
            //    //        //包需配套，则需要倒排到白茬工序

            //    //        //以正排结果，包的第一道工序的开工时间为下层部件的完工时间，倒排到白茬工序                    
            //    //        lSchProduct.ProductSchTaskRev();
            //    //    }

            //    //}
            //}
            //catch (Exception exp)
            //{                
            //    throw new Exception(string.Format("排程批次{0}计算出错!", iSchBatch) + exp.Message );               
            //    return -1;
            //}

            

            ////1 日计划合并到一起,按资源定义向前、向后、正常排产 
            //if (SchParam.cDayPlanMove == "1")
            //{
            //    //所有资源每天工作时间段集中处理,靠后，靠前，正常
            //    foreach (Resource resource in this.schData.ResourceList)
            //    {
            //        resource.ResSchTaskByOrder();
            //    }
            //}

            return 1;
        }

        //按产品优先级顺序正排
        public int SchRunBatchBySN(int iSchBatch)
        {
            //本批是否有排产,没有则返回
            List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate(SchProduct p1) { return (p1.iSchBatch == iSchBatch && p1.bScheduled == 0); });

            if (schProductList.Count > 0)
            {
                //以关键资源任务优化排产顺序，决定其产品的排产顺序,部件先开工，先生产,如果订单优先级相同，按座次号排产

                schProductList.Sort(delegate (SchProduct p1, SchProduct p2) { if(p1.iSchPriority == p2.iSchPriority)
                                                                                        return Comparer<double>.Default.Compare(p1.iSchSN, p2.iSchSN);                    
                                                                                     else
                                                                                        return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority);
                                                                            }); 

                //int maxWorkerThreads, portThreads;
                //int workerThreads;

                ////多线程排产，生成线程池
                //List<Thread> listThread = new List<Thread>();
                //if (SchParam.iSchThread > 1)
                //{
                //    ThreadPool.SetMaxThreads(SchParam.iSchThread,SchParam.iSchThread);
                //    ThreadPool.SetMinThreads(SchParam.iSchThread,SchParam.iSchThread);


                //    //ThreadPool.SetMaxThreads(5, 5);
                //    //ThreadPool.SetMinThreads(5, 5);

                //    ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                //    ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);

                //    ////先初始化这几个线程
                //    //for (int i = 0; i < SchParam.iSchThread; i++)
                //    //{
                //    //    string name = string.Format("ThreadPool_{0}", i);
                //    //    WaitCallback method = (t) => Scheduling.PerProductSchTask(t);
                //    //    ThreadPool.QueueUserWorkItem(method, name);
                //    //}


                //}

                try
                {
                    ////定义方法
                    //WaitCallback method = (t) => Scheduling.PerProductSchTask(t);


                    foreach (SchProduct lSchProduct in schProductList)
                    //Parallel.ForEach(SchProduct lSchProduct in schProductList)
                    {
                        //2.1 正排 调用产对象的排产方法ProductSchTask，再由他调以下各工序的排产

                        //         所有产品工序全正排，之前已排过的工序不重排。
                        if (lSchProduct.iSchSdID == SchParam.iSchSdID)//cWoNo == "WO141025003737")
                        {
                            int k = 1;
                        }

                        //记录排产日志
                        if (SchParam.APSDebug == "1")
                        {
                            string message2 = string.Format(@"1、订单优先级[{0}]，物料编号[{1}] 座次类型[{2}]，座次顺序[{3}]，订单交货期[{4}]，排程批次[{5}]，工单号[{6}]",
                                lSchProduct.iPriority,lSchProduct.cInvCode, lSchProduct.cSchSNType, lSchProduct.iSchSN, lSchProduct.dDeliveryDate, lSchProduct.iSchBatch,lSchProduct.cWoNo);

                            SchParam.Debug(message2, "Scheduling.SchRunBatchBySN产品顺序排产");
                        }

                        //按当前产品排产方式， cschtype  0 正排 1 倒排
                        //正式版本不能正排 2022-06-10 JonasCheng 
                        if (lSchProduct.cSchType != "0" && lSchProduct.cSchType != "" && lSchProduct.cVersionNo.ToLower() != "sureversion")    //1  倒排,包含无限产能倒排
                        {
                            lSchProduct.ProductSchTaskInv();
                        }
                        else//--if (lSchProduct.cSchType == "0")   //正排
                        {
                            //正排运算
                            lSchProduct.ProductSchTask();

                            if (lSchProduct.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                            {
                                int i = 1;
                            }

                            //2.2 倒排 需配套的产品，找出最晚完工部件的完工时间，倒排至白茬工序。//正式版本不倒排 2021-09-29 JonasCheng

                            if (lSchProduct.bSet == "1" && SchParam.SetMinDelayTime > 0 && lSchProduct.cVersionNo.ToLower() != "sureversion")  //需配套    
                            {
                                //包需配套，则需要倒排到白茬工序

                                //以正排结果，包的第一道工序的开工时间为下层部件的完工时间，倒排到白茬工序                    
                                lSchProduct.ProductSchTaskRev("1");
                            }
                        }
                       

                        ////每一张订单排产完成,完工记录数加1
                        //this.schData.iCurRows++;
                    }

                }
                catch (Exception exp)
                {
                    string Message = string.Format("排程按顺序批次{0}计算出错!", iSchBatch) + exp.Message;
                    throw new Exception(Message);
                    //throw new Exception(string.Format("排程按顺序批次{0}计算出错!", iSchBatch) + exp.Message);
                    return -1;
                }
            }

            //如果在产工序，靠产品不能关联到工序的工艺模型，未排工序重新排产 2020-09-25 JonasCheng 
           //1、找出当前产品的最后一道工序，从此工序的前工序列表起，正排。
            List<SchProductRoute> schProductRouteTempList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p1) { return (p1.iSchBatch == iSchBatch && p1.BScheduled == 0); });
            if (schProductRouteTempList.Count < 1) return 1;

            //按工序号排序
            schProductRouteTempList.Sort(delegate (SchProductRoute p1, SchProductRoute p2) { return Comparer<DateTime>.Default.Compare(p1.dBegDate, p2.dBegDate); });

            foreach (var schProductRoute in schProductRouteTempList)
            {                  
                //从最后一道工序起，往前排,会循环调用，一直找到最低层工序正排。
                schProductRoute.ProcessSchTaskPre();
            }


            return 1;

        }


        //按工单优先级顺序正排 2020-12-24
        public int SchRunBatchByWoSN(int as_iSchBatch)
        {
            for (int iSchBatch = -10; iSchBatch < 20; iSchBatch++)
            {
                //本批是否有排产,没有则返回
                List<SchProductWorkItem> schProductWorkItemList = schData.SchProductWorkItemList.FindAll(delegate (SchProductWorkItem p1) { return (p1.iSchBatch == iSchBatch && p1.bScheduled == 0); });

                if (schProductWorkItemList.Count > 0)
                {
                    //以关键资源任务优化排产顺序，决定其产品的排产顺序,部件先开工，先生产,如果订单优先级相同，按座次号排产

                    //schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) {
                    //    if (p1.iSchPriority == p2.iSchPriority)
                    //        return Comparer<double>.Default.Compare(p1.iSchSN, p2.iSchSN);
                    //    else
                    //        return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority);
                    //});

                    if (SchParam.cProChaType1Sort == "1")  //1 按工单需求日期优化
                    {
                        //1 按工单需求日期dRequireDate 由小到大排序
                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<DateTime>.Default.Compare(p1.dRequireDate, p2.dRequireDate); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "2")  //2 订单优先级
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<double>.Default.Compare(p1.iPriority, p2.iPriority); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "3")  //3 座次优先级
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<double>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                    {
                        //ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

                        schProductWorkItemList.Sort(delegate (SchProductWorkItem p1, SchProductWorkItem p2) { return Comparer<DateTime>.Default.Compare(p1.dBegDate, p2.dBegDate); });
                    }


                    try
                    {
                        ////定义方法
                        //WaitCallback method = (t) => Scheduling.PerProductSchTask(t);


                        foreach (SchProductWorkItem lSchProductWorkItem in schProductWorkItemList)
                        //Parallel.ForEach(SchProduct lSchProduct in schProductList)
                        {
                            //2.1 正排 调用产对象的排产方法ProductSchTask，再由他调以下各工序的排产

                            //         所有产品工序全正排，之前已排过的工序不重排。
                            if (lSchProductWorkItem.iSchSdID == SchParam.iSchSdID)//cWoNo == "WO141025003737")
                            {
                                int k = 1;
                            }

                            //记录排产日志
                            if (SchParam.APSDebug == "1")
                            {
                                string message2 = string.Format(@"1、订单优先级[{0}]，物料编号[{1}] 座次类型[{2}]，座次顺序[{3}]，订单交货期[{4}]，排程批次[{5}]，工单号[{6}]",
                                    lSchProductWorkItem.iPriority, lSchProductWorkItem.cInvCode, lSchProductWorkItem.cSchSNType, lSchProductWorkItem.iSchSN, lSchProductWorkItem.dEndDate, lSchProductWorkItem.iSchBatch, lSchProductWorkItem.cWoNo);

                                SchParam.Debug(message2, "Scheduling.SchRunBatchBySN产品顺序排产");
                            }

                            //按当前产品排产方式， cschtype  0 正排 1 倒排

                            if (lSchProductWorkItem.cSchType == "0")   //正排
                            {
                                //正排运算
                                lSchProductWorkItem.ProductSchTask();


                                //2.2 倒排 需配套的产品，找出最晚完工部件的完工时间，倒排至白茬工序。

                                //if (lSchProduct.bSet == "1" && SchParam.SetMinDelayTime > 0)  //需配套 
                                //{
                                //    //包需配套，则需要倒排到白茬工序

                                //    //以正排结果，包的第一道工序的开工时间为下层部件的完工时间，倒排到白茬工序                    
                                //    lSchProduct.ProductSchTaskRev();
                                //}
                            }
                            else    //1  倒排
                            {

                                lSchProductWorkItem.ProductSchTaskInv();

                            }

                            ////每一张订单排产完成,完工记录数加1
                            this.schData.iCurRows++;
                        }

                    }
                    catch (Exception exp)
                    {
                        string Message = string.Format("排程按顺序批次{0}计算出错!", iSchBatch) + exp.Message;
                        throw new Exception(Message);
                        return -1;
                    }

                    ////如果在产工序，靠产品不能关联到工序的工艺模型，未排工序重新排产 2020-09-25 JonasCheng 
                    ////1、找出当前产品的最后一道工序，从此工序的前工序列表起，正排。
                    //List<SchProductRoute> schProductRouteTempList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p1) { return (p1.iSchBatch == iSchBatch && p1.BScheduled == 0); });
                    //if (schProductRouteTempList.Count < 1) return 1;

                    ////按工序号排序
                    //schProductRouteTempList.Sort(delegate (SchProductRoute p1, SchProductRoute p2) { return Comparer<DateTime>.Default.Compare(p1.dBegDate, p2.dBegDate); });

                    //foreach (var schProductRoute in schProductRouteTempList)
                    //{
                    //    //从最后一道工序起，往前排,会循环调用，一直找到最低层工序正排。
                    //    schProductRoute.ProcessSchTaskPre();
                    //}

                }//for循环结束                

            }


            return 1;

        }

        //每个产品排产,用于多线程排产
        public static void PerProductSchTask(object as_SchProduct)
        {
            if (as_SchProduct == null) return;

            

            SchProduct lSchProduct = (SchProduct)as_SchProduct;
    
            if (lSchProduct.cSchType == "0")   //正排
            {
                lSchProduct.ProductSchTask();


                //2.2 倒排 需配套的产品，找出最晚完工部件的完工时间，倒排至白茬工序。

                if (lSchProduct.bSet == "1" && SchParam.SetMinDelayTime > 0  && lSchProduct.cVersionNo.ToLower() != "sureversion")  //需配套
                {
                    //包需配套，则需要倒排到白茬工序

                    //以正排结果，包的第一道工序的开工时间为下层部件的完工时间，倒排到白茬工序                    
                    lSchProduct.ProductSchTaskRev("1");
                }
            }
            else    //1  倒排
            {

                lSchProduct.ProductSchTaskInv();

            }            
        }

        //判断每批是否运算完成
        public static int PerBatchEnd()
        {
            int maxWorkerThreads, workerThreads;
            int portThreads;
            while (true)
            {
                /*
                 GetAvailableThreads()：检索由 GetMaxThreads 返回的线程池线程的最大数目和当前活动数目之间的差值。
                 而GetMaxThreads 检索可以同时处于活动状态的线程池请求的数目。
                 通过最大数目减可用数目就可以得到当前活动线程的数目，如果为零，那就说明没有活动线程，说明所有线程运行完毕。
                 */
                ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
                ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
                if (maxWorkerThreads - workerThreads == 0)
                {                   
                    break;                  

                }
            }

            return 1;
        }

        //冻结计划运算,已执行计划（1）, 待排计划（6）,按关键资源排产顺序，优化排产 iSchBatch = 0 
        public int SchRunBatchByFreeze(int iSchBatch)
        {
            //按白茬关键资源先排，进行优化，一条加工路径只会有一个关键资源

            //同一条生产线，可以有多台关键设备，并设置排程顺序
            //schData.ResourceList
            //string lsSql = "SELECT cDeptno,cWcNo,cResourceNo,cResourceName,iKeySchSN FROM t_Resource WHERE iKeySchSN > 0 AND cIsKey = 1   ORDER BY cDeptno,cWcNo,iKeySchSN ";
            //DataTable dt_ResResource = Common.GetDataTable(lsSql);
            string cResourceNo = "";


            //if (schData.SchProductList.Count < 1)
            //{
            //    throw new Exception("没有产品需要排产，请先选择需要排产的产品！");
            //    return -1;
            //}

            //本批是否有排产,没有则返回
            List<SchProduct> schProductList = schData.SchProductList.FindAll(delegate(SchProduct p1) { return (p1.iSchBatch == iSchBatch && p1.bScheduled == 0); });

            if (schProductList.Count < 1) return 0;

            try
            {

                //--------1、查所有资源未排任务列表------------
                ////1、关键资源排产 给生产任务按工艺特征进行排序 
                ////SchProductRouteResList
                List<SchProductRouteRes> schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.BScheduled == 0 && p1.iSchBatch == iSchBatch && p1.iResReqQty > 0 ); });

                ////schProductRouteResList.OrderBy()
                ////schProductRouteResList.Sort()
                ////TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                //schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                //this.GetNotSchTask();

                if (schProductRouteResList.Count < 1) return 1;


                //2、按上次排产顺序cDefine34重新排序
                //取最大iSchPriorityID
                schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.cDefine34, p2.cDefine34); });
               

                SchProductRouteRes as_SchProductRouteResLast = null;
                int j = 0;

                //schData.iProgress = 30;
                int iProgressOld = schData.iProgress;
                

                int iResCount = schProductRouteResList.Count;
                //schData.iTotalRows = iResCount;


                //schProductRouteResList           
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


                    //排当前关键工序前面所有工序，正排,包含本工序

                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(true, true);

                    ////1 关键资源排产不能穿插，任务排序后，排在后面的任务开工时间不能大于前面任务。 0 允许穿插，后面任务可以排在前面任务之前
                    ////设置当前任务可开工时间为上个任务的结束时间
                    //if (SchParam.bSchKeyBySN == "1" && as_SchProductRouteResLast != null)
                    //{
                    //    schProductRouteRes.schProductRoute.dEarlyBegDate = as_SchProductRouteResLast.dResEndDate;
                    //    schProductRouteRes.dEarliestStartTime = as_SchProductRouteResLast.dResEndDate;
                    //}

                    //记录资源最大分批批次 + 1 
                    if (schProductRouteRes.resource.iTurnsTime != 0 && schProductRouteRes.iBatch > 0 && schProductRouteRes.resource.iBatch < schProductRouteRes.iBatch)
                        schProductRouteRes.resource.iBatch = schProductRouteRes.iBatch + 1;


                    ////往后排，后面白茬工序全排完。油漆工序全是10以上的工序
                    //schProductRouteRes.schProductRoute.ProcessSchTaskNext("2");  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排
                                      

                    //记录上个排产任务
                    as_SchProductRouteResLast = schProductRouteRes;

                    j++;

                    //schData.iCurRows = j;
                    //schData.iProgress = iProgressOld + (20 * j / iResCount);
                }

            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("排程批次{0}计算出错!", iSchBatch) + exp.Message);
                return -1;
            }
            
            return 1;
        }

        //双叶排程运算
        public int SchRun_ShuangYe()           
        {

            //按白茬关键资源先排，进行优化，一条加工路径只会有一个关键资源

            //同一条生产线，可以有多台关键设备，并设置排程顺序
            //schData.ResourceList
            //string lsSql = "SELECT cDeptno,cWcNo,cResourceNo,cResourceName,iKeySchSN FROM t_Resource WHERE iKeySchSN > 0 AND cIsKey = 1   ORDER BY cDeptno,cWcNo,iKeySchSN ";
            //DataTable dt_ResResource = Common.GetDataTable(lsSql);
            string cResourceNo = "";

            //if (schData.SchProductList.Count < 1)
            //{
            //    throw new Exception("没有产品需要排产，请先选择需要排产的产品！");
            //    return -1;
            //}

            //try
            //{

            //    //--------1、白茬关键资源工序按工艺特征排序，进行排产;记录产品的排产顺序,按先进先出原则排整个产品计划------------
            //    //for (int i = 0; i < this.schData.dtResResource.Rows.Count; i++)
            //    //{
            //    //    cResourceNo = this.schData.dtResResource.Rows[i]["cResourceNo"].ToString();
            //    //    Resource resource = schData.ResourceList.Find(delegate(Resource p1) { return (p1.cResourceNo == cResourceNo); });
            //        //TaskTimeRange TaskTimeRange2 = ResTimeRangeList1[i].TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return (p1.iSchSdID == aSchProductRouteRes.iSchSdID && p1.iProcessProductID == aSchProductRouteRes.iProcessProductID); });
            //    foreach (Resource resource in this.schData.KeyResourceList)
            //    {
            //        //3 批量等待排产
            //        if (resource.iTurnsType == "3") continue;
                    
            //        //如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
            //        if (resource.ResSchBefore() < 0)
            //        {
            //            continue;
            //        }

            //        //检查关键资源排产优先级相同的,必须轮换生产。
            //        //Resource resource = schData.ResourceList.Find(delegate(Resource p1) { return (p1.cResourceNo == cResourceNo); });
            //        List<Resource> ResourceList = new List<Resource>(10);
            //        ResourceList = schData.ResourceList.FindAll(delegate(Resource p1) { return (p1.cIsKey == "1" && p1.iKeySchSN == resource.iKeySchSN && p1.iKeySchSN > 0 ); });

            //        if (resource.cResourceNo == "BC-02-06" || resource.cResourceNo == "BC-02-03")  //"BC-03-03"
            //        {
            //            int j = 1;
            //        }

            //        ////关键资源有分组,轮换排产,第一个关键资源就已经把一组资源都已排完.
            //        if (resource.bScheduled == 1) continue;  //全部排完   2014.03.25

            //        //关键资源无分组
            //        if (ResourceList.Count <= 1)
            //        {
            //            //关键资源优化排产
            //            resource.KeyResSchTask();
            //        }
            //        else //关键资源有分组,轮换排产
            //        {
            //            int j = 0;
            //            Boolean bAllScheduled = true ;

            //            //先对所有分组关键资源取未排任务,进行排序
            //            for (int m = 0; m < ResourceList.Count; m++)
            //            {
            //                ResourceList[m].GetNotSchTask();
            //            }

            //            //记录最后一次排产任务
            //            SchProductRouteRes SchProductRouteResLast = null;  

            //            while (j < ResourceList.Count)
            //            {
            //                if (ResourceList[j].bScheduled != 1)  //有未排完
            //                {
            //                    if (ResourceList[j].KeyResSchTaskSingle(-1, ref SchProductRouteResLast) < 0)
            //                        ResourceList[j].bScheduled = 1;   //全部排完
                               
            //                }
            //                else   //检查所有资源是否已排完
            //                {
            //                    bAllScheduled = true; 

            //                    for (int k = 0; k < ResourceList.Count; k++)
            //                    {
            //                        if (ResourceList[k].bScheduled != 1)  //有未排完
            //                        {
            //                            bAllScheduled = false;
            //                            continue;
            //                        }
            //                    }

            //                    //所有资源全部排完
            //                    if (bAllScheduled)
            //                    {
            //                        break;
            //                    }
            //                }

            //                j++;
            //                if (j >= ResourceList.Count) j = 0;
            //            }
            //        }
            //    }
            //    //---------2、油漆工序排产,足一批240分钟才能开工, iTurnsType = "3" 批量等待排产,其他关键资源已排 ---------------------------------
            //    //List<Resource> ResourceResList = new List<Resource>(10);
            //    //ResourceResList = schData.ResourceList.FindAll(delegate(Resource p1) { return (p1.cIsKey == "1" && p1.iTurnsType == "3"); });


            //    //for (int i = 0; i < ResourceResList.Count; i++)
            //    //{    
            //    //    //油漆关键资源排产 
            //    //    ResourceResList[i].KeyResBatchSchTask();
            //    //}
              
            //    //for (int i = 0; i < this.schData.dtResResource.Rows.Count; i++)
            //    //{
            //    //    cResourceNo = this.schData.dtResResource.Rows[i]["cResourceNo"].ToString();
            //    //    Resource resource = schData.ResourceList.Find(delegate(Resource p1) { return (p1.cResourceNo == cResourceNo); });
            //        //TaskTimeRange TaskTimeRange2 = ResTimeRangeList1[i].TaskTimeRangeList.Find(delegate(TaskTimeRange p1) { return (p1.iSchSdID == aSchProductRouteRes.iSchSdID && p1.iProcessProductID == aSchProductRouteRes.iProcessProductID); });
            //    foreach (Resource resource in this.schData.KeyResourceList)
            //    {
            //        //3 批量等待排产
            //        if (resource.iTurnsType != "3") continue;

            //        if (resource.cResourceNo == "YQ-17-07")  //"YQ-17-07"
            //        {
            //            int j = 1;
            //        }

            //        //如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
            //        if (resource.ResSchBefore() < 0)
            //        {                       
            //            continue;
            //        }
                    
            //        //油漆排产，不同颜色的分批排，轮流生产
            //        resource.KeyResBatch();


            //    }

                
            //    //---------3、按产品工艺模型进行排产，之前已排过工序不重排,有些白茬件没有关键工序，在此排.-------------------
            //    //油漆工序要考虑配套，先正排一遍，找出最晚完工部件的完工时间，倒排其他部件油漆线的开工和完工时间。

            //    //产品订单按关键资源顺序进行排产


            //    //按优先级排序，有些产品没有关键工序的，重新设置优先级
            //    schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<int>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });

            //    int iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            //    if (iSchPriority < 0) iSchPriority = 0;


            //    foreach (SchProduct lSchProduct in schData.SchProductList.FindAll(delegate(SchProduct p1) { return p1.iSchPriority < 0; }))
            //    {
            //        if (lSchProduct.iSchPriority < 0)
            //        {
            //            iSchPriority++;
            //            lSchProduct.iSchPriority = iSchPriority;
            //        }
            //    }

            //    //以关键资源任务优化排产顺序，决定其产品的排产顺序,部件先开工，先生产

            //    schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<int>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });


            //    foreach (SchProduct lSchProduct in schData.SchProductList)
            //    {
            //        //2.1 正排 调用产对象的排产方法ProductSchTask，再由他调以下各工序的排产

            //        //         所有产品工序全正排，之前已排过的工序不重排。
            //        if (lSchProduct.cWoNo == "WO140823002239")
            //        { 
                    
            //        }

            //        lSchProduct.ProductSchTask();


            //        //2.2 倒排 需配套的产品，找出最晚完工部件的完工时间，倒排至白茬工序。

            //        if (lSchProduct.bSet == "True")  //需配套
            //        {
            //            //包需配套，则需要倒排到白茬工序

            //            //以正排结果，包的第一道工序的开工时间为下层部件的完工时间，倒排到白茬工序                    
            //            lSchProduct.ProductSchTaskRev();
            //        }

            //    }
            //}
            //catch (Exception exp)
            //{
            //    throw exp;
            //    return -1;
            //}


            //////1 日计划合并到一起,按资源定义向前、向后、正常排产 
            ////if (SchParam.cDayPlanMove == "1")
            ////{
            ////    //所有资源每天工作时间段集中处理,靠后，靠前，正常
            ////    foreach (Resource resource in this.schData.ResourceList)
            ////    {
            ////        resource.ResSchTaskByOrder();
            ////    }
            ////}

            return 1;
        }

        /// <summary>
        /// 排产前处理；已下达、执行工序排产，优先执行
        /// </summary>
        /// <returns></returns>
        public int SchRunPre()
        {
            ////已执行任务排产处理方式 1 冻结;2 已执行计划重排,不考虑前工序完工时间;3 已执行计划重排,考虑前工序完工时间
            //ExecTaskSchType

            ////已执行任务排产方式   1 不优化,按开工时间排序; 2 优化，按工艺特征排序优化
            //ExecTaskSort                                
            int iWorkTime = 0;

            //工序处于完工状态时，标识为已排 2014-10-30
            List<SchProductRouteRes> resSchProductRouteResListNull = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) {
                return (p.schProductRoute == null);
            });  //p.cWoNo != ""

            //如果工序为Null，直接不排产2020-09-03 JonasCheng
            if (resSchProductRouteResListNull.Count > 0)
            {
                foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResListNull)
                //int iCount = resSchProductRouteResListNull.Count;

                //for (int j = iCount -1;j > 0;j++ )
                {
                    //schProductRouteRes.iSchSN = SchParam.iSchSNMin--;
                    //schProductRouteRes.BScheduled = 1;       //"已排"                    
                    ////schProductRouteRes.schProductRoute.BScheduled = 1;   //"已排"

                    this.schData.SchProductRouteResList.Remove(schProductRouteRes);
                }
            }


            //工序处于完工状态时，标识为已排 2014-10-30
            List<SchProductRouteRes> resSchProductRouteResListComp = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { 
                return (p.schProductRoute.cStatus == "4" && p.cWoNo != ""); });  //p.cWoNo != ""

            if (resSchProductRouteResListComp.Count > 0)
            {
                foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResListComp)
                {
                    //SchParam.iSchSNMin--;
                    schProductRouteRes.iSchSN = SchParam.iSchSNMin++;           //已完工，不需要排产  2021-08-27 
                    schProductRouteRes.BScheduled = 1;       //"已排"                    
                    schProductRouteRes.schProductRoute.BScheduled = 1;   //"已排"
                }
            }


            //if (SchParam.ExecTaskSchType == "2")  //2 已执行计划重排,考虑前工序完工时间;
            //{
            //    ////按资源重排,上次已排生产任务单,按上次生产任务单计划开始时间，计划结束时间，生成时间段，占用资源

            //    //foreach (Resource resource in this.schData.ResourceList)
            //    //{
            //    //    //资源产能无限，不用重排
            //    //    //if (resource.cIsInfinityAbility == "1") continue;

            //    //    //备份资源工作日历
            //    //    resource.ResTimeRangeListBak = resource.ResTimeRangeList;

            //    //    //1、已经开始汇报了的优先重排  
            //    //    //工序处于开工状态时，固定，不重新排产 p.schProductRoute.cStatus == "1" || 暂停、
            //    //    List<SchProductRouteRes> resSchProductRouteResList1 = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cResourceNo == resource.cResourceNo && (p.schProductRoute.cStatus != "4") && p.iActResReqQty > 0 && p.cWoNo != ""; });  //p.cWoNo != ""  p.dResBegDate < SchParam.dtToday

            //    //    if (resSchProductRouteResList1.Count > 0)
            //    //    {
            //    //        //所有生产任务单号不为空的，都是已排程确认过的

            //    //        foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResList1)
            //    //        {
            //    //            resource.SchTaskFreezeInit(schProductRouteRes, schProductRouteRes.dResBegDate, schProductRouteRes.dResEndDate);

            //    //            schProductRouteRes.iSchSN = SchParam.iSchSNMin--;
            //    //        }

            //    //    }

            //    //    if (resource.cResourceNo == "BC-07-20")
            //    //    {
            //    //        iWorkTime = 0;
            //    //    }

            //    //    //2、未汇报过的重排 && p.iActResReqQty == 0 
            //    //    List<SchProductRouteRes> resSchProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cResourceNo == resource.cResourceNo && (p.schProductRoute.cStatus != "4") && p.BScheduled == 0 && p.cWoNo != ""; });  //p.cWoNo != "" && p.dResBegDate >= SchParam.dtToday

            //    //    if (resSchProductRouteResList.Count < 1) continue;

            //    //    //resSchProductRouteResList
            //    //    if (SchParam.ExecTaskSort == "2")  //2 优化，按工艺特征排序优化
            //    //        resSchProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return resource.TaskComparer(p1, p2); });
            //    //    else                               //1 按原开工时间排序 
            //    //        resSchProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return (p2.dResBegDate > p1.dResBegDate ? 1 : 0); });

            //    //    DateTime dtLastEndDate = SchParam.dtToday;
            //    //    //所有生产任务单号不为空的，都是已排程确认过的
            //    //    foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResList)
            //    //    {
            //    //        //资源产能无限，还用工序原来开工时时间
            //    //        if (resource.cIsInfinityAbility == "1")
            //    //            if (schProductRouteRes.schProductRoute.SchProductRoutePreList.Count > 0)
            //    //                dtLastEndDate = schProductRouteRes.schProductRoute.SchProductRoutePreList[0].dEndDate;
            //    //            else
            //    //                dtLastEndDate = schProductRouteRes.dResBegDate;

            //    //        if (dtLastEndDate < SchParam.dtStart)
            //    //            dtLastEndDate = SchParam.dtStart;

            //    //        //排产 
            //    //        resource.SchTaskSortInit(schProductRouteRes, dtLastEndDate, schProductRouteRes.dResEndDate);



            //    //        //更新任务开工时间，完工时间
            //    //        if (schProductRouteRes.TaskTimeRangeList.Count > 0)
            //    //        {
            //    //            //倒排，从最后一个时段开始计算时段生产数量，因为第1、2时段可能会有准备时间，换产时间
            //    //            schProductRouteRes.TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            //    //            schProductRouteRes.dResBegDate = schProductRouteRes.TaskTimeRangeList[0].DBegTime;                 //取已排资源任务排产时间段的第1段开始时间
            //    //            schProductRouteRes.dResEndDate = schProductRouteRes.TaskTimeRangeList[schProductRouteRes.TaskTimeRangeList.Count - 1].DEndTime;

            //    //            schProductRouteRes.schProductRoute.dBegDate = schProductRouteRes.dResBegDate;                 //取已排资源任务排产时间段的第1段开始时间
            //    //            schProductRouteRes.schProductRoute.dEndDate = schProductRouteRes.dResEndDate;
            //    //        }

            //    //        //记录本工序完工时间
            //    //        dtLastEndDate = schProductRouteRes.dResEndDate;

            //    //        schProductRouteRes.iSchSN = SchParam.iSchSNMin--;
            //    //    }

            //    //}

            //}
            //else if (SchParam.ExecTaskSchType == "3")  //3 已执行计划重排,不考虑前工序完工时间
            //{
            //    ////工序处于非完工状态的，全部改为“待工”状态，参与排产
            //    //List<SchProductRoute> resSchProductRouteList = this.schData.SchProductRouteList.FindAll(delegate(SchProductRoute p) { return (p.cStatus != "4" && p.cWoNo != ""); });  //p.cWoNo != ""
            //    //foreach (SchProductRoute schProductRoute in resSchProductRouteList)
            //    //{
            //    //    schProductRoute.cSourStatus = schProductRoute.cStatus;
            //    //    schProductRoute.cStatus = "0";
            //    //}

            //    ////如果有完工数量，部分完工，则从当前开工时间开始，排未完工时间。
            //    ////List<SchProductRoute> resSchProductRouteActList = this.schData.SchProductRouteList.FindAll(delegate(SchProductRoute p) { return (p.cStatus != "4" && p.iActQty > 0); });  //p.cWoNo != ""

            //    //////所有生产任务单号不为空的，都是已排程确认过的
            //    //foreach (Resource resource in this.schData.ResourceList)
            //    //{
            //    //    //资源产能无限，不用重排
            //    //    //if (resource.cIsInfinityAbility == "1") continue;

            //    //    //备份资源工作日历
            //    //    resource.ResTimeRangeListBak = resource.ResTimeRangeList;

            //    //    //1、已经开始汇报了的优先重排  
            //    //    //工序处于开工状态时，固定，不重新排产 p.schProductRoute.cStatus == "1" || 暂停、
            //    //    List<SchProductRouteRes> resSchProductRouteResList1 = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cResourceNo == resource.cResourceNo && (p.schProductRoute.cStatus != "4") && p.iActResReqQty > 0 && p.cWoNo != ""; });  //p.cWoNo != ""  p.dResBegDate < SchParam.dtToday

            //    //    if (resSchProductRouteResList1.Count > 0)
            //    //    {
            //    //        //所有生产任务单号不为空的，都是已排程确认过的

            //    //        foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResList1)
            //    //        {
            //    //            resource.SchTaskFreezeInit(schProductRouteRes, schProductRouteRes.dResBegDate, schProductRouteRes.dResEndDate);

            //    //            schProductRouteRes.iSchSN = SchParam.iSchSNMin--;
            //    //        }
            //    //    }

            //    //    //2、未汇报过的重排
            //    //    List<SchProductRouteRes> resSchProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cResourceNo == resource.cResourceNo && (p.schProductRoute.cStatus != "4" && p.iActResReqQty == 0 && p.cWoNo != ""); });  //p.cWoNo != ""

            //    //    if (resSchProductRouteResList.Count < 1) continue;


            //    //    //按原来计划开工时间排序
            //    //    resSchProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return (p2.dResBegDate > p1.dResBegDate ? 1 : 0); });


            //    //    //所有生产任务单号不为空的，都是已排程确认过的

            //    //    foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResList)
            //    //    {
            //    //        //resource.SchTaskSortInit(SchProductRouteRes as_SchProductRouteRes,  DateTime adCanBegDate, DateTime adCanEndDate)
            //    //        resource.SchTaskSortInit(schProductRouteRes, schProductRouteRes.dResBegDate, schProductRouteRes.dResEndDate);

            //    //        schProductRouteRes.iSchSN = SchParam.iSchSNMin--;
            //    //    }

            //    //}


            //}
            //else  //1 冻结               
            //{
            //    ////按资源冻结,上次已排生产任务单,按上次生产任务单计划开始时间，计划结束时间，生成时间段，占用资源

            //    //foreach (Resource resource in this.schData.ResourceList)
            //    //{
            //    //    //备份资源工作日历
            //    //    resource.ResTimeRangeListBak = resource.ResTimeRangeList;

            //    //    //工序处于开工状态时，固定，不重新排产 p.schProductRoute.cStatus == "1" || 暂停、
            //    //    List<SchProductRouteRes> resSchProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cResourceNo == resource.cResourceNo && (p.schProductRoute.cStatus == "2"); });  //p.cWoNo != ""

            //    //    if (resSchProductRouteResList.Count < 1) continue;



            //    //    //所有生产任务单号不为空的，都是已排程确认过的

            //    //    foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResList)
            //    //    {                      
            //    //        resource.SchTaskFreezeInit(schProductRouteRes, schProductRouteRes.dResBegDate, schProductRouteRes.dResEndDate);

            //    //        schProductRouteRes.iSchSN = SchParam.iSchSNMin++;//SchParam.iSchSNMin--;
            //    //    }

            //    //}

            //}

            //2--按工单优先级调度优化排产（正式版本） 车间优化排产调用             
            DispatchSchRun(-200);



            return 1;
        }


        /// <summary>
        /// 排产前数据处理;
        /// </summary>
        /// <returns></returns>
        public int SchRunDataPre()
        {
            DataRow[] dr  ;

            //1物料档案字段写入t_SchProduct 
            foreach (SchProduct schProduc in schData.SchProductList)
            {                
                 dr =  schData.dtItem.Select("cInvCode = '" + schProduc.cInvCode + "'" );

                 if (dr.Length > 0)
                 {
                     //取计划模式和累计提前期天数
                     schProduc.cPlanMode = dr[0]["cPlanMode"].ToString();
                     schProduc.iAdvanceDate = dr[0]["iAdvanceDate"] == DBNull.Value ? 30 : int.Parse(dr[0]["iAdvanceDate"].ToString());
                 }
            }

            //设置资源任务排产开始时间
            SchParam.dtResLastSchTime = DateTime.Now;


            return 1;
        }

        /// <summary>
        /// 排产后处理;
        /// </summary>
        /// <returns></returns>
        public int SchRunPost()
        {


            return 1;
        }

        public void SchRun()      //正常客户排程运算
        {
            //按优先级排序
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iPriority, p2.iPriority); });

            //产品订单按优先级进行正向排产
            foreach (SchProduct lSchProduct in schData.SchProductList)
            {
                //调用产品对象的排产方法ProductSchTask，再由他掉以下各工序的排产

                lSchProduct.ProductSchTask();


            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
