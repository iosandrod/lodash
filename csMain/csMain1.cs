using System.Data;
using System.Data.SqlClient;
using Algorithm;
using APSRun;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace APSRun
{
    public class SchInterface
    {
        public async Task<Object> SetInit(dynamic input)
        {
            string ConnectString = (string)input.ConnectString;
            ServerUri = (string)input.ServerUri;
            APSRun.SqlPro.ConnectionStrings = ConnectString.ToString();
            this.ConnectString = ConnectString.ToString();
            return ConnectString + "__123456 _" + ServerUri;
        }
        public async Task<string> SetInitB(string ConnectString, string ServerUri = "ws://192.168.1.15:9000", string Company = "", string cUser = "admin")
        {
            APSRun.SqlPro.ConnectionStrings = ConnectString;
            this.ConnectString = ConnectString;
            this.ServerUri = ServerUri;
            this.dlShowProcess = new DLShowProcess(ShowProcess);
            return "123456";
        }
        public SchData schData = new SchData();
        public DateTime ldtBegDate;
        public APSRun.SqlPro sqlPro = new APSRun.SqlPro();
        public string RationHourUnit = "1";     //
        public string ConnectString = "";
        public string ServerUri = "ws://192.168.1.15:9000";//"ws://localhost:9000";
        public SocketIOClient.SocketIO client = null;
        public int iProgress = 0;
        public int iBatchRowCount = 50000;   //每8万行保存一次
        public string name = "GetSchOperationProgress";  //发送排程进度事件
        public int schPrecent;                 //排程进度百分比
        public string message;     //排程事件信息
        public string User = "admin";
        public string Company = "001";
        public string socketId = "123";
        public delegate void DLShowProcess(string schPrecent, string message);
        public DLShowProcess dlShowProcess;
        public void ShowProcess(string schPrecent, string message)
        {
            try
            {
                this.SendAsync("schPrecent:" + schPrecent.ToString());
                this.SendAsync("message:" + message.ToString());
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        public async Task<object> Start(dynamic input)
        {
            string cVersionNo = (string)input.cVersionNo;
            DateTime adte_Start = DateTime.Parse(input.adte_Start);
            DateTime adte_End = DateTime.Parse(input.adte_End);
            Boolean bShowTips = true;
            string cSchType = (string)input.cSchType;
            string cTechSchType = (string)input.cTechSchType;
            string iCurSchRunTimes = (string)input.iCurSchRunTimes;
            User = (string)input.User;
            ConnectString = (string)input.ConnectString;
            ServerUri = (string)input.ServerUri;
            Company = (string)input.Company;
            socketId = (string)input.socketID;
            ldtBegDate = adte_Start;
            APSRun.SqlPro.ConnectionStrings = ConnectString;
            this.ConnectString = ConnectString;
            this.ServerUri = ServerUri;
            this.dlShowProcess = new DLShowProcess(ShowProcess);
            Console.WriteLine("ConnectString " + ConnectString);
            Console.WriteLine("this.ServerUri " + this.ServerUri);
            message = "开始智能排程";
            schPrecent = 0;
            this.dlShowProcess(schPrecent.ToString(), message);
            message = "取排程参数";
            schPrecent = 1;
            this.dlShowProcess(schPrecent.ToString(), message);
            Console.WriteLine("进入排程函数");
            if (cSchType == "2")
            {
                cVersionNo = "SureVersion";
            }
            this.schData.cVersionNo = cVersionNo;           
            this.schData.dtStart = adte_Start;
            this.schData.dtEnd = adte_End;
            this.schData.dtToday = DateTime.Today;//Global.CurDataTime;//System.Convert.ToDateTime(Common.Common.GetDatabaseDateTime());  //取当前时间
            this.schData.cCalculateNo = User + ";" + this.schData.dtToday;  //排程运算号  2022-01-21
            Console.WriteLine(this.schData.dtToday);
            SchParam.cVersionNo = cVersionNo;
            SchParam.dtStart = adte_Start;
            SchParam.dtEnd = adte_End;
            SchParam.dtToday = this.schData.dtToday;
            SchParam.bReSchedule = "0"; // Global.GetUserParam("bReSchedule");   // 1 重排; 0 第一次排 2022-01-21
            SchParam.cSchCapType = "1";   // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能  2019-03-07
            SchParam.cSchType = cSchType;   // //排程方式  0 ---正常排产, 1--资源调度优化排产2020-08-25 ， 2--按工单优先级调度优化排产（正式版本）3 --按资源调度优化排产 
            SchParam.cTechSchType = cTechSchType;   //工艺段排产方式  0    普通排产 1    注塑排产 2    装配排产 3    表面处理 4    委外排产
            SchParam.iCurSchRunTimes = iCurSchRunTimes;   //当前排程次数
            Algorithm.SchParam.dtParamValue = SqlPro.GetDataTable("select * from t_ParamValue where 1 = 1 ", "");//ToDataTable(DBServerProvider.SqlDapper.QueryList<object>("select * from t_ParamValue where 1 = 1 ",null));//系统参数表 //APSCommon.SqlPro.GetDataTable("select * from t_ParamValue where 1 = 1 ", "t_ParamValue");//系统参数表
            Algorithm.SchParam.GetSchParams();
            Console.WriteLine("初始化参数完成");
            message = "初始化参数完成";
            schPrecent = 2;
            this.dlShowProcess(schPrecent.ToString(), message);
            if (SchParam.bReSchedule != "1")   //0 第一次排
            {
                Console.WriteLine("bReSchedule != 1 0第一次排");
                message = "第一次排";
                schPrecent = 2;
                this.dlShowProcess(schPrecent.ToString(), message);
                try
                {
                    string lsSql2 = string.Format(@"EXECUTE P_SchedulePrePre '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", this.schData.cVersionNo, this.schData.dtStart.ToString(), this.schData.dtEnd.ToString(), SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo, SchParam.iCurSchRunTimes);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP
                    SqlPro.ExecuteNonQuery(lsSql2, null);
                    string lsSql = string.Format(@"EXECUTE P_SchedulePre '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", this.schData.cVersionNo, this.schData.dtStart.ToString(), this.schData.dtEnd.ToString(), SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo, SchParam.iCurSchRunTimes);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP
                    message = "1、排程前准备工作已完成";
                    schPrecent = 3;
                    this.dlShowProcess(schPrecent.ToString(), message);
                    SqlPro.ExecuteNonQuery(lsSql, null);
                    Console.WriteLine("执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中");
                }
                catch (Exception excp)
                {
                    message = "排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString();
                    schPrecent = 100;
                    this.dlShowProcess(schPrecent.ToString(), message);
                    Console.WriteLine("排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString());
                    throw (excp);
                    return -1;
                }
                schData.iProgress = 5;   //1、过程P_SchedulePre  5%
                Console.WriteLine("完成5%");
                message = "1、排程前准备工作已完成:";
                schPrecent = schData.iProgress;
                this.dlShowProcess(schPrecent.ToString(), message);
                Console.WriteLine(bShowTips);
                if (bShowTips)
                {
                }
                else  //不显示出错信息
                {
                }
            }
            try
            {
                if (GetResourceList() < 0) return -1;
            }
            catch (Exception excp)
            {
                schPrecent = 100;
                this.dlShowProcess(schPrecent.ToString(), message);
                Console.WriteLine("排产计算出错！位置2、生成资源工作列表及资源工作日历,出错内容：" + excp.ToString());
                throw (excp);
                return -1;
            }
            schData.iProgress = 15;   //1、过程GetResourceList  10%
            Console.WriteLine("过程GetResourceList  15%");
            message = "2、资源工作日历已完成";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            if (GetSchData() < 0) return -1;
            schData.iProgress = 20;   //1、过程GetSchData  20%
            Console.WriteLine("过程GetSchData  20%");
            message = "3、生成订单产品工艺模型列表";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            Console.WriteLine("开个线程统计排程明细进度");
            if (this.ResSchTaskInit() < 0) return -1;
            schData.iProgress = 30;   //1、过程GetSchData  30%
            Console.WriteLine("过程GetSchData  30%");
            message = "4、已开工任务排程...";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            try
            {
                Scheduling scheduling = new Scheduling(this.schData);
                System.Threading.Thread threadRows = new System.Threading.Thread(new System.Threading.ThreadStart(ShowSchProgress));
                threadRows.Start();
                Thread.Sleep(200);
                if (scheduling.SchMainRun(cSchType) < 0) return -1;
            }
            catch (Exception excp)
            {
                message = "排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString();
                schPrecent = 100;
                this.dlShowProcess(schPrecent.ToString(), message);
                Console.WriteLine("排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString());
                throw (excp);
                return -1;
            }
            schData.iProgress = 80;   //1、过程GetSchData  80%
            Console.WriteLine("过程GetSchData  80%");
            message = "5、产品订单按优先级进行排产";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            #region//增加提示
            #endregion
            if (SaveSchData() < 0)
            {
                return -1;
            }
            schData.iProgress = 90;   //1、过程GetSchData  90%
            message = "6、排程结果写回数据库";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            try
            {
                string lsSql = string.Format(@"EXECUTE P_SchedulePost '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", this.schData.cVersionNo, this.schData.dtStart.ToString(), this.schData.dtEnd.ToString(), this.schData.dtToday.ToString(), SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo, SchParam.iCurSchRunTimes);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP
                var result = (SqlPro.ExecuteNonQuery(lsSql, null));
            }
            catch (Exception excp)
            {
                Console.WriteLine("排产计算出错！位置GetResourceList(),出错内容：" + excp.ToString());
                throw (excp);
                return -1;
            }
            schData.iProgress = 95;   //1、过程GetSchData  %
            message = "7、排程后处理完成";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            Console.WriteLine("过程GetSchData  95%");
            try
            {
                string lsSql = string.Format(@"EXECUTE P_SchKPI '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", this.schData.cVersionNo, ldtBegDate, DateTime.Now, this.schData.cCalculateNo, "", SchParam.cSchType, SchParam.cTechSchType);
                var result = (SqlPro.ExecuteNonQuery(lsSql, null));
            }
            catch (Exception excp)
            {
                Console.WriteLine("排产后记录KPI" + excp.Message.ToString());
                throw (excp);
                return -1;
            }
            schData.iProgress = 98;   //1、过程GetSchData  100%
            message = "8、排产后记录KPI完成";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            Console.WriteLine("排程完毕  100%");
            schData.iProgress = 100;
            message = "排程完毕";
            schPrecent = schData.iProgress;
            this.dlShowProcess(schPrecent.ToString(), message);
            return 1;
        }
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
        #region // 1、生成资源工作列表及资源工作日历
        public int GetResourceList()
        {
            Console.WriteLine("取生成了资源工作日历的资源列表");
            string lsResAddWhere = String.Format(@"  and cStatus <> '3' and (cResourceNo in (select distinct cResourceNo 
            string lstg_Sql = string.Format("EXECUTE P_GetSchDataResource '{0}','{1}','{2}','{3}','{4}','{5}'", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo);
            schData.dtResource = SqlPro.GetDataTable(lstg_Sql, null);
            string lsSql = "SELECT FProChaTypeID,FResChaTypeName,cParentClsNo,bEnd,cBarCode,cNote,cMacNo FROM t_ProChaType where isnull(bEnd,1) = '1'";
            this.schData.dtProChaType = SqlPro.GetDataTable(lsSql, null);//APSCommon.SqlPro.GetDataTable(lsSql, "t_ProChaType");
            Console.WriteLine("取工艺特征类型");
            lsSql = @"SELECT     FResChaValueID, FResChaValueNo, FResChaValueName, FProChaTypeID,isnull(FResChaCycleValue,0) as  FResChaCycleValue, isnull(FResChaRePlaceTime,0) as FResChaRePlaceTime, FResChaMemo,isnull(FUseFixedPlaceTime,'1') as FUseFixedPlaceTime, 
                            FSchSN, isnull(FUseChaCycleValue,'0') as FUseChaCycleValue, cDefine1, cDefine2, cDefine3, cDefine4, cDefine5, cDefine6, cDefine7, cDefine8, cDefine9, cDefine10, isnull(cDefine11,0) as cDefine11, isnull(cDefine12,0) as cDefine12, isnull(cDefine13,0) as cDefine13, 
                            isnull(cDefine14,0) as cDefine14,isnull(cDefine15,'1900-01-01') cDefine15,isnull(cDefine16,'1900-01-01')  cDefine16
                        FROM         dbo.t_ResChaValue  with (nolock) where 1 = 1 ";
            this.schData.dtResChaValue = SqlPro.GetDataTable(lsSql, null);//APSCommon.SqlPro.GetDataTable(lsSql, "t_ResChaValue");
            Console.WriteLine("取工艺特征值");
            lsSql = "SELECT FProChaTypeID,FResChaValue1ID,FResChaValue2ID,FResChaExcTime FROM dbo.t_ResChaCrossTime  with (nolock) where 1 = 1";
            this.schData.dtResChaCrossTime = SqlPro.GetDataTable(lsSql, null); //APSCommon.SqlPro.GetDataTable(lsSql, "t_ResChaCrossTime");
            Console.WriteLine("取工艺特征转换时间");
            lsSql = "SELECT FProChaTypeID,FResChaValue1ID,FResChaValue2ID,FResChaExcTime FROM dbo.t_ResChaCrossTime  with (nolock) where 1 = 1";
            this.schData.dtResChaCrossTime = SqlPro.GetDataTable(lsSql, null); //APSCommon.SqlPro.GetDataTable(lsSql, "t_ResChaCrossTime");
            Console.WriteLine("取工艺特征转换时间");
            Console.WriteLine("取排程参数");
            lstg_Sql = "";
            lstg_Sql = "Update t_SchProduct set  dEarLiestSchDate = '" + this.schData.dtStart + "' where dEarLiestSchDate < '" + this.schData.dtStart + "' and isnull(cWoNo,'') = ''";
            SqlPro.ExecuteNonQuery(lstg_Sql, null);
            Console.WriteLine("更新未排订单排程可开始时间");
            try
            {
                Console.WriteLine("schData.dtStart - schData.dtEnd" + schData.dtStart.AddDays(-20) + schData.dtEnd);
                lstg_Sql = string.Format(@"EXECUTE P_GetResWorkTime '{0}','{1}','{2}','{3}','{4}','{5}'", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP
                DataTable dt_ResourceTime = SqlPro.GetDataTable(lstg_Sql, null);
                this.schData.dt_ResourceTime = dt_ResourceTime;
                this.schData.dt_ResourceTime.DefaultView.Sort = "iPeriodTimeID asc,dPeriodDay asc";
                schData.iProgress = 13;
                message = "2.1、资源工作日历生成完成";
                schPrecent = schData.iProgress;
                this.dlShowProcess(schPrecent.ToString(), message);
                Console.WriteLine("调用过程P_GetResWorkTime生成所有资源的工作日历");
                lstg_Sql = @"SELECT * FROM t_SchResWorkTime  with (nolock) WHERE cType = '1' and cSourceType = '2' AND cVersionNo = '" + schData.cVersionNo + "'";
                DataTable dt_ResourceSpecTime = SqlPro.GetDataTable(lstg_Sql, null);//DataTable dt_ResourceSpecTime = APSCommon.SqlPro.GetDataTable(lstg_Sql, "t_SchResWorkTime");
                this.schData.dt_ResourceSpecTime = dt_ResourceSpecTime;
                this.schData.dt_ResourceSpecTime.DefaultView.Sort = "dPeriodDay asc";
                string cWorkCenter;
                WorkCenter lobj_WorkCenter;
                Console.WriteLine("3、生成工作中心");
                foreach (DataRow drWorkCenter in this.schData.dtWorkCenter.Rows)
                {
                    cWorkCenter = drWorkCenter["cWcNo"].ToString();
                    lobj_WorkCenter = new Algorithm.WorkCenter(cWorkCenter, this.schData);
                    this.schData.WorkCenterList.Add(lobj_WorkCenter);
                }
                string cResourceNoOld = "";
                string cResourceNo;
                Resource lobj_Resource;
                ResTimeRange lResTimeRange;
                int ResTimeRangeType;
                Console.WriteLine("开始按资源循环，生成资源对象，同时生成资源时间段");
                foreach (DataRow drResource in this.schData.dtResource.Rows)
                {
                    cResourceNo = drResource["cResourceNo"].ToString();
                    if (cResourceNo == "gys20039")
                    {
                        int m = 1;
                    }
                    lobj_Resource = new Resource(cResourceNo, this.schData);
                    this.schData.ResourceList.Add(lobj_Resource);
                    if (lobj_Resource.bTeamResource != "1")
                    {
                        DataRow[] drResTime = dt_ResourceTime.Select("cResourceNo = '" + cResourceNo + "'");
                        foreach (DataRow drResTimeRange in drResTime)
                        {
                            lResTimeRange = new ResTimeRange();
                            lResTimeRange.CResourceNo = cResourceNo;
                            lResTimeRange.resource = lobj_Resource;
                            lResTimeRange.CIsInfinityAbility = drResource["cIsInfinityAbility"].ToString();
                            lResTimeRange.iPeriodID = (int)drResTimeRange["iPeriodTimeID"];
                            lResTimeRange.DBegTime = (DateTime)drResTimeRange["dPeriodBegTime"];
                            lResTimeRange.DEndTime = (DateTime)drResTimeRange["dPeriodEndTime"];
                            lResTimeRange.dPeriodDay = (DateTime)drResTimeRange["dPeriodDay"];
                            lResTimeRange.FShiftType = drResTimeRange["FShiftType"].ToString();
                            if (lResTimeRange.FShiftType == "")
                            {
                                lResTimeRange.FShiftType = "A班";      //班次 A班 夜班 中班等 
                            }
                            lResTimeRange.Attribute = (Algorithm.TimeRangeAttribute)(int.Parse(drResTimeRange["cTimeRangeType"].ToString()));
                            lResTimeRange.GetNoWorkTaskTimeRange(lResTimeRange.DBegTime, lResTimeRange.DEndTime, true);
                            lobj_Resource.ResTimeRangeList.Add(lResTimeRange);
                        }
                        DataRow[] drResSpecTime = dt_ResourceSpecTime.Select("cResourceNo = '" + cResourceNo + "'");
                        foreach (DataRow drResTimeRange in drResSpecTime)
                        {
                            lResTimeRange = new ResTimeRange();
                            lResTimeRange.CResourceNo = cResourceNo;
                            lResTimeRange.resource = lobj_Resource;
                            lResTimeRange.iPeriodID = int.Parse(drResTimeRange["iPeriodTimeID"].ToString());//drResource["iPeriodTimeID"]; 
                            lResTimeRange.CIsInfinityAbility = drResource["cIsInfinityAbility"].ToString();
                            lResTimeRange.DBegTime = (DateTime)drResTimeRange["dPeriodBegTime"];
                            lResTimeRange.DEndTime = (DateTime)drResTimeRange["dPeriodEndTime"];
                            lResTimeRange.Attribute = (Algorithm.TimeRangeAttribute)(int.Parse(drResTimeRange["cTimeRangeType"].ToString()));
                            lResTimeRange.HoldingTime = Convert.ToInt32(float.Parse(drResTimeRange["iHoldingTime"].ToString())) * 60;
                            lobj_Resource.ResSpecTimeRangeList.Add(lResTimeRange);
                        }
                        if (drResSpecTime.Length > 0)
                        {
                            lobj_Resource.MergeTimeRange();
                        }
                        lobj_Resource.getResSourceDayCapList();
                    }
                }
                this.schData.KeyResourceList = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.cIsKey == "1" && p1.iKeySchSN > 0); });
                this.schData.KeyResourceList.Sort(delegate (Resource p1, Resource p2) { return Comparer<int>.Default.Compare(p1.iKeySchSN, p2.iKeySchSN); });
                this.schData.TeamResourceList = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.bTeamResource == "1"); });
                if (this.schData.TeamResourceList.Count > 0)
                {
                    foreach (Resource TeamResource in this.schData.TeamResourceList)
                    {
                        List<Resource> ResourceSubList = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.cTeamResourceNo == TeamResource.cResourceNo); });
                        TeamResource.TeamResourceList = ResourceSubList;
                        foreach (Resource TeamResourceSub in ResourceSubList)
                        {
                            TeamResourceSub.TeamResource = TeamResource;
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置GetResourceList(),出错内容：" + ex1.ToString());
                throw (ex1);
                return -1;
            }
            return 1;
        }
        #endregion
        #region//2、生成订单产品工艺模型列表,先分别填充SchProductList，SchProductRouteList，SchProductRouteResList, SchProductRouteResTimeList
        public int GetSchData()
        {
            Console.WriteLine("开始第二步骤");
            #region//2.0 SQL准备
            string cSchWo = GetParamValue("cSchWo");
            Console.WriteLine("cSchWo取值完毕");
            string lsSchProductRouteItem = string.Format(@" SELECT    isnull(iSchBatch,1) as iSchBatch, cVersionNo, iSchSdID, iProcessProductID, iInterID, iEntryID, cWoNo, cInvCode, cInvCodeFull, isnull(iBomLevel,0) as iBomLevel, cLevelInfo, cLevelPath,  cSubInvCode, 
                            cSubInvCodeFull, iSeqID, cUtterType, cSubRelate, isnull(iQtyPer,0) as iQtyPer, isnull(iParentQty,0) as iParentQty, isnull(iSubQty,0) as iSubQty, isnull(iScrapt,0) as iScrapt, iNormalQty, iRetPercent, iReqQty, dReqDate, iProQty, iScrapQty, 
                            isnull(iNormalScrapQty,0) as iNormalScrapQty, isnull(iKeepQty,0) as iKeepQty, isnull(iPlanQty,0) as iPlanQty , cWhNo, cPacNo, iRetOffsetLt, cNote, isnull(cGetItemType,'0') as cGetItemType,isnull(bself,'0') as bself,isnull(dForeInDate,getdate()) as dForeInDate
                        FROM         dbo.t_SchProductRouteItem  with (nolock)
                        where 1 = 1 and  iSchSdID in (select iSchSdID from t_SchProduct where cVersionNo = '{0}' and isnull(cSelected,'0') = '1'  and isnull(iSchQty,0) > 0 and isnull(cWoNo,'') = '' ) 
                        and cVersionNo = '{0}' ", schData.cVersionNo);
            if (cSchWo == "1")
                lsSchProductRouteItem += string.Format(@" 
                    union
                       SELECT    isnull(iSchBatch,6) as iSchBatch, cVersionNo, iSchSdID, iProcessProductID, iInterID, iEntryID, cWoNo, cInvCode, cInvCodeFull, isnull(iBomLevel,0) as iBomLevel, cLevelInfo, cLevelPath,  cSubInvCode, 
                            cSubInvCodeFull, iSeqID, cUtterType, cSubRelate, isnull(iQtyPer,0) as iQtyPer, isnull(iParentQty,0) as iParentQty, isnull(iSubQty,0) as iSubQty, isnull(iScrapt,0) as iScrapt, iNormalQty, iRetPercent, iReqQty, dReqDate, iProQty, iScrapQty, 
                            isnull(iNormalScrapQty,0) as iNormalScrapQty, isnull(iKeepQty,0) as iKeepQty, isnull(iPlanQty,0) as iPlanQty , cWhNo, cPacNo, iRetOffsetLt, cNote, isnull(cGetItemType,'0') as cGetItemType,isnull(bself,'0') as bself,isnull(dForeInDate,getdate()) as dForeInDate
                        FROM         dbo.t_SchProductRouteItem  with (nolock)
                        where 1 = 1 and  iSchSdID in (select t_SchProduct.iSchSdID from t_SchProduct inner join (select cWoNo from t_WorkOrder where cStatus in ('I','A','G') ) b on (t_SchProduct.cWoNo = b.cWoNo )  where cVersionNo = 'SureVersion'   ) 
                                    and cVersionNo = 'SureVersion'                        
                                    and iSchSdID in ( select iSchSdID from t_SchProduct WHERE  cVersionNo = 'SureVersion' AND isnull(cSelected,'0') = '1' )
                            order by cVersionNo,iSchSdID,iProcessProductID,cInvCodeFull,cSubInvCodeFull ");
            SchParam.cSelfEndDate = GetParamValue("cSelfEndDate");
            string lsSchProductRouteResTime = string.Format(@"select isnull(iSchBatch,6) as iSchBatch,cVersionNo,iSchSdID,iProcessProductID,isnull(iInterID,0) as iInterID,isnull(iWoProcessID,0) iWoProcessID,isnull(iResProcessID,0) as iResProcessID,isnull(cWoNo,'') cWoNo ,isnull(iResourceID,0) as iResourceID,cResourceNo,cResourceName,iTimeID,dResBegDate,dResEndDate,iResReqQty,isnull(iResRationHour,0) as iResRationHour,isnull(cSimulateVer,'') as cSimulateVer,isnull(cNote,'') as cNote,isnull(cTaskType,'1') as cTaskType
                    from t_SchProductRouteResTime  with (nolock) where  1 = 1 and iSchSdID in (Select iSchSdID from t_SchProduct where cVersionNo = '{0}' and isnull(cSelected,'0') = '1'  and isnull(iSchQty,0) > 0 and isnull(cWoNo,'') = ''  ) 
                        and cVersionNo = '{0}'  order by cVersionNo,iSchSdID,iProcessProductID,cResourceNo,iTimeID ", schData.cVersionNo);
            string lsItem = string.Format(@"
                     select  a.iItemID, a.cInvCode, a.cInvName,a.cInvStd,a.cItemClsNo, isnull(a.cVenCode,'') as cVenCode, isnull(a.bSale,'0') as bSale,isnull(a.bPurchase,'0') as bPurchase,isnull(a.bSelf,'1') as bSelf,isnull(a.bProxyForeign,'0') as bProxyForeign,isnull(a.cComUnitCode,'') as cComUnitCode, isnull(a.cWcNo,'') as cWcNo,isnull(a.iProSec,'') as iProSec,isnull(a.iPriority,'') as iPriority, 
                        isnull(a.iSafeStock,'') as iSafeStock, isnull(a.iTopLot,'') as  iTopLot,isnull(a.iLowLot,'') as iLowLot,isnull(a.iIncLot,'') as iIncLot,isnull(a.iAvgLot,'') as  iAvgLot,isnull(a.cLeadTimeType,'') as cLeadTimeType,isnull(a.iAvgLeadTime,'') as iAvgLeadTime ,isnull(a.iAdvanceDate,'') as iAdvanceDate , isnull(a.cRouteCode,'') as cRouteCode , isnull(a.cPlanMode,'') as cPlanMode, 
                        isnull(a.cWorkRouteType,'') as cWorkRouteType,isnull(a.cTechNo,'') as cTechNo,isnull(a.cKeyResourceNo,'') as cKeyResourceNo,isnull(a.cInjectItemType,'') as cInjectItemType,isnull(a.cMoldNo,'') as cMoldNo,isnull(a.cSubMoldNo,'') as cSubMoldNo,isnull(a.cMoldPosition,'') as cMoldPosition,isnull(a.iMoldSubQty,0) as iMoldSubQty,isnull(a.iMoldCount,0) as iMoldCount,
                        isnull(a.cMaterial,'') as cMaterial,isnull(a.cColor,'') as cColor,isnull(a.fVolume,0) as fVolume,isnull(a.flength,0) as flength,isnull(a.fWidth,0) as fWidth,isnull(a.fHeight,0) as fHeight,isnull(a.fNetWeight,0) as fNetWeight, isnull(a.iItemDifficulty,1 ) as iItemDifficulty , isnull(a.cSize1,'') as cSize1,isnull(a.cSize2,'') as cSize2,isnull(a.cSize3,'') as cSize3,isnull(a.cSize4,'') as cSize4,isnull(a.cSize5,'') as cSize5,isnull(a.cSize6,'') as cSize6 ,
                        isnull(a.cSize7,'') as cSize7,isnull(a.cSize8,'') as cSize8,isnull(a.cSize9,'') as cSize9,isnull(a.cSize10,'') as cSize10,isnull(a.cSize11,0) as cSize11,isnull(a.cSize12,0) as cSize12,isnull(a.cSize13,0) as cSize13,isnull(a.cSize14,0) as cSize14,isnull(a.cSize15,'') as cSize15,isnull(a.cSize16,'') as cSize16
                    FROM t_Item a  with (nolock)
                        where a.cInvCode in (select distinct cWorkItemNo from t_SchProductRoute )", schData.cVersionNo);  //and isnull(a.bSchedule,1) = '1' 
            string lsWorkCenter = string.Format(@" select * from t_WorkCenter ");
            string lsDepartment = string.Format(@" select * from t_Department ");
            string lsPerson = string.Format(@" select  a.cPsn_Num, a.cPsn_Name, a.cDepCode, a.iRecordID, a.rPersonType, 
                a.rSex, a.dBirthDate, a.rNativePlace, a.rNational, a.rhealthStatus, 
                a.rMarriStatus, a.vIDNo, a.MPicture, a.rPerResidence, a.vAliaName, 
                a.dJoinworkDate, a.dEnterDate, a.dRegularDate, a.vSSNo, a.rworkAttend, 
                a.vCardNo, a.rtbmRule, a.rCheckInFlag, a.dLastDate, a.hrts, a.vstatus1, 
                a.nstatus2, a.bPsnPerson, a.cPsnMobilePhone, a.cPsnFPhone, a.cPsnOPhone, 
                a.cPsnInPhone, a.cPsnEmail, a.cPsnPostAddr, a.cPsnPostCode, a.cPsnFAddr, 
                a.cPsnQQCode, a.cPsnURL, a.CpsnOSeat, a.dEnterUnitDate, a.cPsnProperty, 
                a.cPsnBankCode, a.cPsnAccount, a.pk_hr_hi_person, a.bProbation, a.cDutyclass, 
                a.bTakeTM, a.MPictureqpb, a.rIDType, a.rCountry, a.dLeaveDate, 
                a.rFigure, a.rWorkStatus, a.EmploymentForm, a.rPersonParameters, a.bDutyLock, 
                a.bpsnshop, a.cPosition, a.cEnglishName, a.cEducation, a.cReservefundNo, 
                a.fCreditQuantity, a.iCreDate, a.cCreGrade, a.iLowRate, a.cOfferGrade, 
                a.iOfferRate, a.dPValidDate, a.dPInValidDate, a.cPsnDefine1,
                a.cPsnDefine2,a.cPsnDefine3,a.cPsnDefine4,a.cPsnDefine5,a.cPsnDefine6,a.cPsnDefine7,
                a.cPsnDefine8,a.cPsnDefine9,a.cPsnDefine10,a.cPsnDefine11,a.cPsnDefine12,a.cPsnDefine13,
                a.cPsnDefine14,a.cPsnDefine15,a.cPsnDefine16,a.cMemberShip,a.cClasses,a.blacklist, 
                a.blacklistNote,a.cBusDepCode
                 from t_Person  a  with (nolock) inner join t_team b  with (nolock) on (a.cDutyclass = b.cTeamNo)");
            string lsteam = string.Format(@" select * from t_team ");
            string lsTechInfo = string.Format(@" 
                    SELECT   iInterID, cTechNo, cTechName, isnull(cResClsNo,'') as cResClsNo,isnull(cWcNo,'') as cWcNo, isnull(cDeptNo,'') as cDeptNo,isnull(cResourceNo,'') as cResourceNo, isnull(cTechReq,'') as cTechReq, isnull(cNote,'') as cNote, isnull(cTechDefine1,'') as cTechDefine1, 
                                    isnull(cTechDefine2,'') as  cTechDefine2, isnull(cTechDefine3,'') as cTechDefine3,isnull(cTechDefine4,'') as  cTechDefine4,  isnull(cTechDefine5,'') as cTechDefine5, isnull(cTechDefine6,'') as  cTechDefine6, isnull(cTechDefine7,'') as cTechDefine7, isnull(cTechDefine8,'') as cTechDefine8, isnull(cTechDefine9,'') as cTechDefine9, 
                                    isnull(cTechDefine10,'') as cTechDefine10, isnull(cTechDefine11,0) as cTechDefine11, isnull(cTechDefine12,0) as cTechDefine12, isnull(cTechDefine13,0) as cTechDefine13, isnull(cTechDefine14,0) as cTechDefine14, isnull(cTechDefine15,'') as cTechDefine15,isnull(cTechDefine16,'') as  cTechDefine16, 
                                    isnull(cFormula,'') as cFormula, isnull(cFormula2,'') as cFormula2, isnull(iSeqPretime,0) as iSeqPretime, isnull(iSeqPostTime,0) as iSeqPostTime, isnull(cAttributeValue1,'') as  cAttributeValue1, 
                                    isnull(cAttributeValue2,'') as cAttributeValue2, isnull(cAttributeValue3,'') as cAttributeValue3, isnull(cAttributeValue4,'') as cAttributeValue4, isnull(cAttributeValue5,'') as cAttributeValue5, isnull(cAttributeValue6,'') as cAttributeValue6, isnull(cAttributeValue7,'') as cAttributeValue7, isnull(cAttributeValue8,'') as cAttributeValue8, 
                                    isnull(cAttributeValue9,'') as cAttributeValue9, isnull(cAttributeValue10,'') as cAttributeValue10, isnull(iTechValue,0) as iTechValue, isnull(iOrder,0) as iOrder, isnull(iTechDifficulty,1) as iTechDifficulty,isnull(iSeqPretime,0) as iSeqPretime,isnull(iSeqPostTime,0) as iSeqPostTime
                    FROM      dbo.t_TechInfo  with (nolock)
                        ");
            string lsTechLearnCurves = string.Format(@" SELECT     iInterID, cLearnCurvesNo, cLearnCurvesName, cTechNo, isnull(iDayDis1,0) as iDayDis1, isnull(iDayDis2,0) as iDayDis2, isnull(iDayDis3,0) as iDayDis3, isnull(iDayDis4,0) as iDayDis4, isnull(iDayDis5,0) as iDayDis5,
                      isnull(iDayDis6,0) as iDayDis6, isnull(iDayDis7,0) as iDayDis7, isnull(iDayDis8,0) as iDayDis8,  isnull(iDayDis9,0) as iDayDis9,  isnull(iDayDis10,0) as iDayDis10, 
                      isnull(iDayDis11,0) as iDayDis11, isnull(iDayDis12,0) as iDayDis12, isnull(iDayDis13,0) as iDayDis13,  isnull(iDayDis14,0) as iDayDis14,  isnull(iDayDis15,0) as iDayDis15, 
                      isnull(iDayDis16,0) as iDayDis16, isnull(iDayDis17,0) as iDayDis17, isnull(iDayDis18,0) as iDayDis18, isnull(iDayDis19,0) as iDayDis19, isnull(iDayDis20,0) as iDayDis20, 
                      isnull(iDayDis21,0) as iDayDis21, isnull(iDayDis22,0) as iDayDis22, isnull(iDayDis23,0) as iDayDis23, isnull(iDayDis24,0) as iDayDis24, 
                      isnull(iDayDis25,0) as iDayDis25, isnull(iDayDis26,0) as iDayDis26, isnull(iDayDis27,0) as iDayDis27, isnull(iDayDis28,0) as iDayDis28, 
                      isnull(iDayDis29,0) as iDayDis29, isnull(iDayDis30,0) as iDayDis30, isnull(iDayDis31,0) as iDayDis31 , isnull(iDiffCoe,0) as iDiffCoe, isnull(iCapacity,0) as iCapacity, isnull(iResPreTime,0) as iResPreTime, cNote, cDefine22, cDefine23, cDefine24, 
                      cDefine25, cDefine26
                        FROM         dbo.t_TechLearnCurves   with (nolock) ");
            string lsResTechScheduSN = string.Format(@" select * from t_ResTechScheduSN ");
            #endregion
            Console.WriteLine("sql准备完毕");
            try
            {
                Console.WriteLine("2.10 t_TechInfo");
                schData.dtTechInfo = SqlPro.GetDataTable(lsTechInfo, null);//APSCommon.SqlPro.GetDataTable(lsTechInfo, "t_TechInfo");
                string cTechNo;
                TechInfo lobj_TechInfo;
                foreach (DataRow drTechInfo in this.schData.dtTechInfo.Rows)
                {
                    cTechNo = drTechInfo["cTechNo"].ToString();
                    lobj_TechInfo = new Algorithm.TechInfo(cTechNo, this.schData);
                    this.schData.TechInfoList.Add(lobj_TechInfo);
                }
                Console.WriteLine("2.0 t_Item 已确认的生产任务单取SureVersion版本数据");
                schData.dtItem = SqlPro.GetDataTable(lsItem, null);//APSCommon.SqlPro.GetDataTable(lsItem, "t_Item");
                string cInvCode;
                Item lobj_Item;
                foreach (DataRow drItem in this.schData.dtItem.Rows)
                {
                    cInvCode = drItem["cInvCode"].ToString();
                    lobj_Item = new Algorithm.Item(cInvCode, this.schData);
                    this.schData.ItemList.Add(lobj_Item);
                }
                string lstg_Sql = "";
                Console.WriteLine("2.1 填充SchProductList");
                #region//2.1 填充SchProductList
                {
                    Console.WriteLine("2.1 填充SchProductList");
                    lstg_Sql = string.Format(@"EXECUTE P_GetSchProductWorkItem '{0}','{1}','{2}','{3}','{4}','{5}'", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo); // @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName ";
                    schData.dtSchProductWorkItem = SqlPro.GetDataTable(lstg_Sql, null);
                    foreach (DataRow dr in schData.dtSchProductWorkItem.Rows)
                    {
                        SchProductWorkItem lSchProductWorkItem = new SchProductWorkItem();
                        lSchProductWorkItem.iSchSdID = (int)dr["iSchSdID"];
                        lSchProductWorkItem.iBomAutoID = (int)dr["iBomAutoID"];
                        lSchProductWorkItem.cVersionNo = dr["cVersionNo"].ToString();
                        lSchProductWorkItem.iInterID = (int)dr["iInterID"];
                        lSchProductWorkItem.iSdLineID = (int)dr["iSdLineID"];
                        lSchProductWorkItem.iSeqID = (int)dr["iSeqID"];
                        lSchProductWorkItem.cCustNo = dr["cCustNo"].ToString();
                        lSchProductWorkItem.cPriorityType = (int)dr["cPriorityType"];
                        lSchProductWorkItem.cStatus = dr["cStatus"].ToString();
                        lSchProductWorkItem.iItemID = -1; //(int)dr["iItemID"];  //无用
                        lSchProductWorkItem.cInvCode = dr["cInvCode"].ToString().Trim();
                        lSchProductWorkItem.cInvName = dr["cInvName"].ToString();
                        lSchProductWorkItem.cInvStd = dr["cInvStd"].ToString();
                        lSchProductWorkItem.iReqQty = Convert.ToDouble(dr["iReqQty"]);
                        lSchProductWorkItem.dBegDate = (DateTime)dr["dBegDate"];
                        lSchProductWorkItem.dEndDate = (DateTime)dr["dEndDate"];
                        if (lSchProductWorkItem.iSchSdID == SchParam.iSchSdID)
                        {
                            int m;
                        }
                        lSchProductWorkItem.dCanBegDate = dr["dCanBegDate"] == DBNull.Value ? DateTime.Today : (DateTime)dr["dCanBegDate"];
                        lSchProductWorkItem.dCanEndDate = dr["dCanEndDate"] == DBNull.Value ? (DateTime)dr["dEndDate"] : (DateTime)dr["dCanEndDate"];
                        lSchProductWorkItem.cMiNo = dr["cMiNo"].ToString();
                        lSchProductWorkItem.iPriority = Convert.ToDouble(dr["iPriority"]);
                        lSchProductWorkItem.cWoNo = dr["cWoNo"].ToString();
                        lSchProductWorkItem.cColor = dr["cColor"].ToString();
                        lSchProductWorkItem.cNote = dr["cNote"].ToString();
                        lSchProductWorkItem.cType = dr["cType"].ToString();
                        lSchProductWorkItem.cSchType = dr["cSchType"].ToString();    //2016-10-17 签入
                        lSchProductWorkItem.iSchPriority = Convert.ToDouble(dr["iPriority"]);    //2016-12-07 签入
                        lSchProductWorkItem.iSchBatch = (int)dr["iSchBatch"];
                        lSchProductWorkItem.iWorkQtyPd = Convert.ToDouble(dr["iWorkQtyPd"]);
                        lSchProductWorkItem.cBatchNo = dr["cBatchNo"].ToString();            //托盘号，不为空时，按托盘排产，同一托盘物料工艺路线类型一样，同一工序必须选择同一资源排产2020-03-22。
                        lSchProductWorkItem.iSchSN = Convert.ToDouble(dr["iSchSN"]);         //排产座次
                        lSchProductWorkItem.cWorkRouteType = dr["cWorkRouteType"].ToString();   //工艺路线类型
                        lSchProductWorkItem.cSchSNType = dr["cSchSNType"].ToString();           //座次编号
                        lSchProductWorkItem.cAttributes1 = dr["cAttributes1"].ToString();       //加工属性1
                        lSchProductWorkItem.cAttributes2 = dr["cAttributes2"].ToString();
                        lSchProductWorkItem.cAttributes3 = dr["cAttributes3"].ToString();
                        lSchProductWorkItem.cAttributes4 = dr["cAttributes4"].ToString();
                        lSchProductWorkItem.cAttributes5 = dr["cAttributes5"].ToString();
                        lSchProductWorkItem.cAttributes6 = dr["cAttributes6"].ToString();
                        lSchProductWorkItem.cAttributes7 = dr["cAttributes7"].ToString();
                        lSchProductWorkItem.cAttributes8 = dr["cAttributes8"].ToString();
                        lSchProductWorkItem.cAttributes9 = Convert.ToDouble(dr["cAttributes9"]);         //加工属性9
                        lSchProductWorkItem.cAttributes10 = Convert.ToDouble(dr["cAttributes10"]);
                        lSchProductWorkItem.cAttributes11 = Convert.ToDouble(dr["cAttributes11"]);
                        lSchProductWorkItem.cAttributes12 = Convert.ToDouble(dr["cAttributes12"]);
                        lSchProductWorkItem.cAttributes13 = dr["cAttributes13"].ToString();
                        lSchProductWorkItem.cAttributes14 = dr["cAttributes14"].ToString();
                        lSchProductWorkItem.cAttributes15 = dr["cAttributes15"].ToString();
                        lSchProductWorkItem.cAttributes16 = dr["cAttributes16"].ToString();
                        lSchProductWorkItem.schData = this.schData;
                        schData.SchProductWorkItemList.Add(lSchProductWorkItem);
                    }
                }
                {
                    lstg_Sql = string.Format(@"EXECUTE P_GetSchDataProduct '{0}','{1}','{2}','{3}','{4}','{5}' ", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo); // @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName ";
                    schData.dtSchProduct = SqlPro.GetDataTable(lstg_Sql, null);
                    foreach (DataRow dr in schData.dtSchProduct.Rows)
                    {
                        SchProduct lSchProduct = new SchProduct();
                        lSchProduct.iSchSdID = (int)dr["iSchSdID"];
                        lSchProduct.cVersionNo = dr["cVersionNo"].ToString();
                        lSchProduct.iInterID = (int)dr["iInterID"];
                        lSchProduct.iSdLineID = (int)dr["iSdLineID"];
                        lSchProduct.iSeqID = (int)dr["iSeqID"];
                        lSchProduct.iModelID = (int)dr["iModelID"];
                        lSchProduct.cCustNo = dr["cCustNo"].ToString();
                        lSchProduct.cCustName = dr["cCustName"].ToString();
                        lSchProduct.cSTCode = dr["cSTCode"].ToString();
                        lSchProduct.cBusType = dr["cBusType"].ToString();
                        lSchProduct.cPriorityType = (int)dr["cPriorityType"];
                        lSchProduct.cStatus = dr["cStatus"].ToString();
                        lSchProduct.cRequireType = dr["cRequireType"].ToString();
                        lSchProduct.iItemID = -1; //(int)dr["iItemID"];  //无用
                        lSchProduct.cInvCode = dr["cInvCode"].ToString().Trim();
                        lSchProduct.cInvName = dr["cInvName"].ToString();
                        lSchProduct.cInvStd = dr["cInvStd"].ToString();
                        lSchProduct.cUnitCode = dr["cUnitCode"].ToString();
                        lSchProduct.iReqQty = Convert.ToDouble(dr["iReqQty"]);
                        lSchProduct.dRequireDate = (DateTime)dr["dRequireDate"];
                        lSchProduct.dDeliveryDate = (DateTime)dr["dDeliveryDate"];
                        lSchProduct.dEarliestSchDate = dr["dEarliestSchDate"] == DBNull.Value ? DateTime.Today : (DateTime)dr["dEarliestSchDate"];
                        lSchProduct.cSchStatus = dr["cSchStatus"].ToString();
                        lSchProduct.cMiNo = dr["cMiNo"].ToString();
                        lSchProduct.iPriority = Convert.ToDouble(dr["iPriority"]);
                        lSchProduct.cSelected = dr["cSelected"].ToString();
                        lSchProduct.cWoNo = dr["cWoNo"].ToString();
                        lSchProduct.iPlanQty = Convert.ToDouble(dr["iPlanQty"]);
                        lSchProduct.cNeedSet = dr["cNeedSet"].ToString();
                        lSchProduct.iFHQuantity = Convert.ToDouble(dr["iFHQuantity"]);
                        lSchProduct.iKPQuantity = Convert.ToDouble(dr["iKPQuantity"]);
                        lSchProduct.iSourceLineID = (int)dr["iSourceLineID"];
                        lSchProduct.cColor = dr["cColor"].ToString();
                        lSchProduct.cNote = dr["cNote"].ToString();
                        lSchProduct.bSet = dr["bSet"].ToString();//APSCommon.SqlPro.GetSqlDataInt("select isnull(bSet,0) as bSet  from t_Item where cInvCode = '" + dr["cInvCode"].ToString() + "'");
                        lSchProduct.cType = dr["cType"].ToString();
                        lSchProduct.cSchType = dr["cSchType"].ToString();    //2016-10-17 签入
                        lSchProduct.iSchPriority = Convert.ToDouble(dr["iPriority"]);    //2016-12-07 签入
                        lSchProduct.iSchBatch = (int)dr["iSchBatch"];
                        lSchProduct.iDeliveryDays = (int)dr["iDeliveryDays"];
                        lSchProduct.cScheduled = dr["cScheduled"].ToString();
                        lSchProduct.iWorkQtyPd = Convert.ToDouble(dr["iWorkQtyPd"]);
                        Convert.ToDouble(dr["iWorkQtyPd"]);
                        lSchProduct.cBatchNo = dr["cBatchNo"].ToString();            //托盘号，不为空时，按托盘排产，同一托盘物料工艺路线类型一样，同一工序必须选择同一资源排产2020-03-22。
                        lSchProduct.iSchSN = Convert.ToDouble(dr["iSchSN"]);         //排产座次
                        lSchProduct.cGroupSN = Convert.ToDouble(dr["cGroupSN"]);    //分组号       
                        lSchProduct.cGroupQty = Convert.ToDouble(dr["cGroupQty"]);  //分组数量
                        lSchProduct.cCustomize = dr["cCustomize"].ToString();        //是否定制，自动生成工艺路线的
                        lSchProduct.cWorkRouteType = dr["cWorkRouteType"].ToString();   //工艺路线类型
                        lSchProduct.cSchSNType = dr["cSchSNType"].ToString();           //座次编号
                        lSchProduct.cAttributes1 = dr["cAttributes1"].ToString();       //加工属性1
                        lSchProduct.cAttributes2 = dr["cAttributes2"].ToString();
                        lSchProduct.cAttributes3 = dr["cAttributes3"].ToString();
                        lSchProduct.cAttributes4 = dr["cAttributes4"].ToString();
                        lSchProduct.cAttributes5 = dr["cAttributes5"].ToString();
                        lSchProduct.cAttributes6 = dr["cAttributes6"].ToString();
                        lSchProduct.cAttributes7 = dr["cAttributes7"].ToString();
                        lSchProduct.cAttributes8 = dr["cAttributes8"].ToString();
                        lSchProduct.cAttributes9 = Convert.ToDouble(dr["cAttributes9"]);         //加工属性9
                        lSchProduct.cAttributes10 = Convert.ToDouble(dr["cAttributes10"]);
                        lSchProduct.cAttributes11 = Convert.ToDouble(dr["cAttributes11"]);
                        lSchProduct.cAttributes12 = Convert.ToDouble(dr["cAttributes12"]);
                        lSchProduct.cAttributes13 = dr["cAttributes13"].ToString();
                        lSchProduct.cAttributes14 = dr["cAttributes14"].ToString();
                        lSchProduct.cAttributes15 = dr["cAttributes15"].ToString();
                        lSchProduct.cAttributes16 = dr["cAttributes16"].ToString();
                        lSchProduct.schData = this.schData;
                        schData.SchProductList.Add(lSchProduct);
                    }
                }
                #endregion
                Console.WriteLine("2.2 填充SchProductRouteList 改用存储过程取数，提供速度");
                #region//2.2 填充SchProductRouteList 改用存储过程取数，提供速度
                lstg_Sql = string.Format(@"EXECUTE P_GetSchDataProductRoute '{0}','{1}','{2}','{3}','{4}','{5}' ", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo); // @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName ";
                schData.dtSchProductRoute = SqlPro.GetDataTable(lstg_Sql, null);
                foreach (DataRow dr in schData.dtSchProductRoute.Rows)
                {
                    SchProductRoute lSchProductRoute = new SchProductRoute();
                    lSchProductRoute.iSchSdID = (int)dr["iSchSdID"];
                    lSchProductRoute.cVersionNo = dr["cVersionNo"].ToString();
                    lSchProductRoute.iModelID = (int)dr["iModelID"];
                    lSchProductRoute.iProcessProductID = (int)dr["iProcessProductID"];
                    lSchProductRoute.cWoNo = dr["cWoNo"].ToString();
                    lSchProductRoute.iInterID = (int)dr["iInterID"];
                    lSchProductRoute.iWoProcessID = (int)dr["iWoProcessID"];
                    lSchProductRoute.iItemID = -1; //(int)dr["iItemID"];
                    lSchProductRoute.cInvCode = dr["cInvCode"].ToString().Trim();
                    lSchProductRoute.iWorkItemID = -1;//(int)dr["iWorkItemID"];
                    lSchProductRoute.cWorkItemNo = dr["cWorkItemNo"].ToString();
                    lSchProductRoute.item = schData.ItemList.Find(delegate (Algorithm.Item p) { return p.cInvCode == lSchProductRoute.cWorkItemNo; });
                    lSchProductRoute.iProcessID = (int)dr["iProcessID"];
                    lSchProductRoute.iWoSeqID = (int)dr["iWoSeqID"];
                    lSchProductRoute.cTechNo = dr["cTechNo"].ToString();
                    lSchProductRoute.techInfo = schData.TechInfoList.Find(delegate (Algorithm.TechInfo p) { return p.cTechNo == lSchProductRoute.cTechNo; });
                    lSchProductRoute.cSeqNote = dr["cSeqNote"].ToString();
                    lSchProductRoute.cWcNo = dr["cWcNo"].ToString();
                    lSchProductRoute.iNextSeqID = (int)dr["iNextSeqID"];
                    lSchProductRoute.cPreProcessID = dr["cPreProcessID"].ToString();
                    lSchProductRoute.cPostProcessID = dr["cPostProcessID"].ToString();
                    lSchProductRoute.cPreProcessItem = dr["cPreProcessItem"].ToString();
                    lSchProductRoute.cPostProcessItem = dr["cPostProcessItem"].ToString();
                    lSchProductRoute.iAutoID = (int)dr["iAutoID"];
                    lSchProductRoute.cLevelInfo = dr["cLevelInfo"].ToString();
                    lSchProductRoute.iLevel = (int)dr["iLevel"];
                    lSchProductRoute.iParentItemID = (int)dr["iParentItemID"];
                    lSchProductRoute.cParentItemNo = dr["cParentItemNo"].ToString();
                    lSchProductRoute.cCompSeq = dr["cCompSeq"].ToString();
                    lSchProductRoute.cMoveType = dr["cMoveType"].ToString();
                    lSchProductRoute.iMoveInterTime = Convert.ToDouble(dr["iMoveInterTime"]);
                    lSchProductRoute.iMoveInterQty = Convert.ToDouble(dr["iMoveInterQty"]);
                    lSchProductRoute.iSeqPreTime = Convert.ToDouble(dr["iSeqPreTime"]);
                    lSchProductRoute.iSeqPostTime = Convert.ToDouble(dr["iSeqPostTime"]);
                    lSchProductRoute.iLaborTime = Convert.ToDouble(dr["iLaborTime"]);
                    lSchProductRoute.iLeadTime = Convert.ToDouble(dr["iLeadTime"]);
                    lSchProductRoute.cStatus = dr["cStatus"].ToString();
                    lSchProductRoute.iPriority = (int)dr["iPriority"];
                    lSchProductRoute.iReqQty = Convert.ToDouble(dr["iReqQty"]);
                    lSchProductRoute.iReqQtyOld = Convert.ToDouble(dr["iReqQtyOld"]);
                    lSchProductRoute.iActQty = Convert.ToDouble(dr["iActQty"]);
                    lSchProductRoute.iRealHour = Convert.ToDouble(dr["iRealHour"]);
                    lSchProductRoute.dBegDate = (DateTime)dr["dBegDate"];
                    lSchProductRoute.dEndDate = (DateTime)dr["dEndDate"];
                    lSchProductRoute.dActBegDate = (DateTime)dr["dActBegDate"];
                    lSchProductRoute.dActEndDate = (DateTime)dr["dActEndDate"];
                    lSchProductRoute.cNote = dr["cNote"].ToString();
                    lSchProductRoute.iDevCountPd = Convert.ToInt32(float.Parse(dr["iDevCountPd"].ToString()));    //2017-11-02 同步
                    lSchProductRoute.cDevCountPdExp = dr["cDevCountPdExp"].ToString();
                    lSchProductRoute.cParellelType = dr["cParellelType"].ToString();  //并行类型 ES 前工序结束后工序开始  SS 前工序开始后工序开始(差一个批次移转时间)  EE 同时结束(差一个批次移转时间)
                    lSchProductRoute.cParallelNo = dr["cParallelNo"].ToString();   //并行码
                    lSchProductRoute.cKeyBrantch = dr["cKeyBrantch"].ToString();   //关键分支
                    lSchProductRoute.iCapacity = Convert.ToDecimal(float.Parse(dr["iCapacity"].ToString()));
                    lSchProductRoute.cCapacityExp = dr["cCapacityExp"].ToString();
                    lSchProductRoute.iAdvanceDate = Convert.ToDecimal(float.Parse(dr["iAdvanceDate"].ToString()));    //BOM本层材料最长采购周期(按订单生产物料),排程时考虑
                    lSchProductRoute.iSchBatch = (int)dr["iSchBatch"];
                    lSchProductRoute.schData = this.schData;
                    schData.SchProductRouteList.Add(lSchProductRoute);
                }
                #endregion
                Console.WriteLine("2.3 填充TaskTimeRangeList 暂时不需要，每次排产重新生成时间段");
                #region//2.3 填充TaskTimeRangeList 暂时不需要，每次排产重新生成时间段
                #endregion
                Console.WriteLine("2.4 填充SchProductRouteResList");
                #region//2.4 填充SchProductRouteResList
                lstg_Sql = string.Format(@"EXECUTE P_GetSchDataProductRouteRes '{0}','{1}','{2}','{3}','{4}','{5}' ", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo); // @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName ";
                schData.dtSchProductRouteRes = SqlPro.GetDataTable(lstg_Sql, null);
                foreach (DataRow dr in schData.dtSchProductRouteRes.Rows)
                {
                    SchProductRouteRes lSchProductRouteRes = new SchProductRouteRes();
                    lSchProductRouteRes.schData = this.schData;
                    lSchProductRouteRes.iSchSdID = (int)dr["iSchSdID"];
                    lSchProductRouteRes.cVersionNo = dr["cVersionNo"].ToString();
                    lSchProductRouteRes.iProcessProductID = (int)dr["iProcessProductID"];
                    lSchProductRouteRes.iProcessID = (int)dr["iProcessID"];
                    lSchProductRouteRes.iResProcessID = (int)dr["iResProcessID"];
                    lSchProductRouteRes.iResourceAbilityID = (int)dr["iResourceAbilityID"];
                    lSchProductRouteRes.cWoNo = dr["cWoNo"].ToString();
                    lSchProductRouteRes.iItemID = -1;// (int)dr["iItemID"];
                    lSchProductRouteRes.cInvCode = dr["cInvCode"].ToString().Trim();
                    lSchProductRouteRes.iWoSeqID = (int)dr["iWoSeqID"];
                    lSchProductRouteRes.cTechNo = dr["cTechNo"].ToString();
                    lSchProductRouteRes.cSeqNote = dr["cSeqNote"].ToString();
                    lSchProductRouteRes.iResGroupNo = dr["iResGroupNo"].ToString();
                    lSchProductRouteRes.iResGroupPriority = (int)dr["iResGroupPriority"];
                    lSchProductRouteRes.cSelected = dr["cSelected"].ToString();
                    lSchProductRouteRes.iResourceID = Convert.ToDouble(dr["iResourceID"]);
                    lSchProductRouteRes.cResourceNo = dr["cResourceNo"].ToString();
                    lSchProductRouteRes.cResourceName = dr["cResourceName"].ToString();
                    lSchProductRouteRes.cTeamResourceNo = dr["cTeamResourceNo"].ToString();
                    lSchProductRouteRes.iResReqQty = Convert.ToDouble(dr["iResReqQty"]);
                    lSchProductRouteRes.iResReqQtyOld = Convert.ToDouble(dr["iResReqQtyOld"]);
                    lSchProductRouteRes.dResBegDate = Convert.ToDateTime(dr["dResBegDate"]);
                    lSchProductRouteRes.dResEndDate = Convert.ToDateTime(dr["dResEndDate"]);
                    if (GetParamValue("HourMinSecond") == "1") //如果是显示小时
                    {
                        lSchProductRouteRes.iResRationHour = Convert.ToDouble(dr["iResRationHour"]) * 60;
                    }
                    else
                    {
                        lSchProductRouteRes.iResRationHour = Convert.ToDouble(dr["iResRationHour"]);
                    }
                    lSchProductRouteRes.iViceResource1ID = dr["iViceResource1ID"] == DBNull.Value ? -1 : (int)dr["iViceResource1ID"];
                    lSchProductRouteRes.cViceResource1No = dr["cViceResource1No"].ToString();
                    lSchProductRouteRes.iViceResource2ID = dr["iViceResource2ID"] == DBNull.Value ? -1 : (int)dr["iViceResource2ID"];
                    lSchProductRouteRes.cViceResource2No = dr["cViceResource2No"].ToString();
                    lSchProductRouteRes.iViceResource3ID = dr["iViceResource3ID"] == DBNull.Value ? -1 : (int)dr["iViceResource3ID"];
                    lSchProductRouteRes.cViceResource3No = dr["cViceResource3No"].ToString();
                    lSchProductRouteRes.cWorkType = dr["cWorkType"].ToString();
                    lSchProductRouteRes.iBatchQty = Convert.ToDouble(dr["iBatchQty"]);
                    lSchProductRouteRes.iBatchQtyBase = Convert.ToDouble(dr["iBatchQtyBase"]);
                    lSchProductRouteRes.iBatchWorkTime = Convert.ToDouble(dr["iBatchWorkTime"]);
                    lSchProductRouteRes.iBatchInterTime = Convert.ToDouble(dr["iBatchInterTime"]);
                    lSchProductRouteRes.iResPreTime = Convert.ToDouble(dr["iResPreTime"]);
                    lSchProductRouteRes.iResPreTimeOld = Convert.ToDouble(dr["iResPreTimeOld"]);
                    lSchProductRouteRes.cResPreTimeExp = dr["cResPreTimeExp"].ToString();
                    lSchProductRouteRes.iPriorityRes = Convert.ToInt32(dr["iPriorityRes"]);            //本次排产优先级  
                    lSchProductRouteRes.iPriorityResLast = Convert.ToInt32(dr["iPriorityResLast"]);     //上次排产优先级  
                    double iCapacity = 0;
                    if (GetParamValue("MinOrHour") == "1") //如果是分钟
                    {
                        iCapacity = Convert.ToDouble(dr["iCapacity"]) * 60;
                    }
                    else if (GetParamValue("MinOrHour") == "2") //如果是小时
                    {
                        iCapacity = Convert.ToDouble(dr["iCapacity"]) * 3600;
                    }
                    else
                    {
                        iCapacity = Convert.ToDouble(dr["iCapacity"]);
                    }
                    lSchProductRouteRes.iCapacity = Convert.ToDouble(iCapacity.ToString());
                    lSchProductRouteRes.cCapacityExp = dr["cCapacityExp"].ToString();
                    lSchProductRouteRes.cIsInfinityAbility = Convert.ToDouble(dr["cIsInfinityAbility"]);
                    lSchProductRouteRes.iResPostTime = Convert.ToDouble(dr["iResPostTime"]);
                    lSchProductRouteRes.iCycTime = Convert.ToDouble(dr["iCycTime"]);
                    lSchProductRouteRes.iProcessPassRate = Convert.ToDouble(dr["iProcessPassRate"]);
                    lSchProductRouteRes.iEfficiency = Convert.ToDouble(dr["iEfficiency"]);
                    lSchProductRouteRes.iHoursPd = Convert.ToDouble(dr["iHoursPd"]);
                    lSchProductRouteRes.iWorkQtyPd = Convert.ToDouble(dr["iWorkQtyPd"]);
                    lSchProductRouteRes.iWorkersPd = Convert.ToDouble(dr["iWorkersPd"]);
                    lSchProductRouteRes.iDevCountPd = Convert.ToDouble(dr["iDevCountPd"]);
                    lSchProductRouteRes.cLearnCurvesNo = dr["cLearnCurvesNo"].ToString();
                    if (GetParamValue("HourMinSecond") == "0") //如果是分钟
                    {
                        lSchProductRouteRes.iLaborTime = Convert.ToDouble(dr["iLaborTime"]);
                    }
                    else if (GetParamValue("HourMinSecond") == "1") //如果是小时
                    {
                        lSchProductRouteRes.iLaborTime = Convert.ToDouble(dr["iLaborTime"]) * 60;
                    }
                    lSchProductRouteRes.iLeadTime = Convert.ToDouble(dr["iLeadTime"]);
                    lSchProductRouteRes.iActResReqQty = Convert.ToDouble(dr["iActResReqQty"]);
                    lSchProductRouteRes.iActResRationHour = Convert.ToDouble(dr["iActResRationHour"]);
                    lSchProductRouteRes.dActResBegDate = Convert.ToDateTime(dr["dActResBegDate"]);
                    lSchProductRouteRes.dActResEndDate = Convert.ToDateTime(dr["dActResEndDate"]);
                    string cBatch = dr["cDefine24"].ToString() == "" ? "-1" : dr["cDefine24"].ToString();
                    int iBatch;
                    if (int.TryParse(cBatch, out iBatch))
                        lSchProductRouteRes.iBatch = iBatch;
                    else
                        lSchProductRouteRes.iBatch = -1;
                    lSchProductRouteRes.FResChaValue1ID = dr["FResChaValue1ID"].ToString();
                    lSchProductRouteRes.FResChaValue2ID = dr["FResChaValue2ID"].ToString();
                    lSchProductRouteRes.FResChaValue3ID = dr["FResChaValue3ID"].ToString();
                    lSchProductRouteRes.FResChaValue4ID = dr["FResChaValue4ID"].ToString();
                    lSchProductRouteRes.FResChaValue5ID = dr["FResChaValue5ID"].ToString();
                    lSchProductRouteRes.FResChaValue6ID = dr["FResChaValue6ID"].ToString();
                    lSchProductRouteRes.resource = schData.ResourceList.Find(delegate (Resource p) { return p.cResourceNo == lSchProductRouteRes.cResourceNo; });
                    if (lSchProductRouteRes.resource == null)
                    {
                        throw new Exception("排程ID[" + lSchProductRouteRes.iSchSdID + "," + lSchProductRouteRes.iProcessProductID + "] 资源编号[" + lSchProductRouteRes.cResourceNo + "]不存在,注意区分大小写和空格,或者资源没有定义工作日历!");
                    }
                    if (lSchProductRouteRes.resource.FProChaType1ID != "-1" && lSchProductRouteRes.resource.FProChaType1ID != "")
                        lSchProductRouteRes.resChaValue1 = new ResChaValue(lSchProductRouteRes.FResChaValue1ID, lSchProductRouteRes, 1);
                    if (lSchProductRouteRes.resource.FProChaType2ID != "-1" && lSchProductRouteRes.resource.FProChaType2ID != "")
                        lSchProductRouteRes.resChaValue2 = new ResChaValue(lSchProductRouteRes.FResChaValue2ID, lSchProductRouteRes, 2);
                    if (lSchProductRouteRes.resource.FProChaType3ID != "-1" && lSchProductRouteRes.resource.FProChaType3ID != "")
                        lSchProductRouteRes.resChaValue3 = new ResChaValue(lSchProductRouteRes.FResChaValue3ID, lSchProductRouteRes, 3);
                    if (lSchProductRouteRes.resource.FProChaType4ID != "-1" && lSchProductRouteRes.resource.FProChaType4ID != "")
                        lSchProductRouteRes.resChaValue4 = new ResChaValue(lSchProductRouteRes.FResChaValue4ID, lSchProductRouteRes, 4);
                    if (lSchProductRouteRes.resource.FProChaType5ID != "-1" && lSchProductRouteRes.resource.FProChaType5ID != "")
                        lSchProductRouteRes.resChaValue5 = new ResChaValue(lSchProductRouteRes.FResChaValue5ID, lSchProductRouteRes, 5);
                    if (lSchProductRouteRes.resource.FProChaType6ID != "-1" && lSchProductRouteRes.resource.FProChaType6ID != "")
                        lSchProductRouteRes.resChaValue6 = new ResChaValue(lSchProductRouteRes.FResChaValue6ID, lSchProductRouteRes, 6);
                    lSchProductRouteRes.FResChaValue1Cyc = Convert.ToDouble(dr["FResChaValue1Cyc"]);
                    lSchProductRouteRes.FResChaValue2Cyc = Convert.ToDouble(dr["FResChaValue2Cyc"]);
                    lSchProductRouteRes.FResChaValue3Cyc = Convert.ToDouble(dr["FResChaValue3Cyc"]);
                    lSchProductRouteRes.FResChaValue4Cyc = Convert.ToDouble(dr["FResChaValue4Cyc"]);
                    lSchProductRouteRes.FResChaValue5Cyc = Convert.ToDouble(dr["FResChaValue5Cyc"]);
                    lSchProductRouteRes.FResChaValue6Cyc = Convert.ToDouble(dr["FResChaValue6Cyc"]);
                    lSchProductRouteRes.cDefine22 = dr["cDefine22"].ToString();
                    lSchProductRouteRes.cDefine23 = dr["cDefine23"].ToString();
                    lSchProductRouteRes.cDefine34 = Convert.ToDouble(dr["cDefine34"]);
                    lSchProductRouteRes.cDefine35 = 0;//Convert.ToDouble(dr["cDefine35"]);
                    lSchProductRouteRes.iSchBatch = (int)dr["iSchBatch"];
                    lSchProductRouteRes.TaskTimeRangeList = new List<TaskTimeRange>();//schData.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.iSchSdID == lSchProductRouteRes.iSchSdID && p.iProcessProductID == lSchProductRouteRes.iProcessProductID && p.iResProcessID == lSchProductRouteRes.iResProcessID; });
                    schData.SchProductRouteResList.Add(lSchProductRouteRes);
                }
                #endregion
                Console.WriteLine("2.5 填充SchProductRouteItemList");
                #region//2.5 填充SchProductRouteItemList
                if (SchParam.cSelfEndDate == "1")
                {
                    schData.dtSchProductRouteItem = SqlPro.GetDataTable(lsSchProductRouteItem, null);
                    foreach (DataRow dr in schData.dtSchProductRouteItem.Rows)
                    {
                        SchProductRouteItem lSchProductRouteItem = new SchProductRouteItem();
                        lSchProductRouteItem.schData = this.schData;
                        lSchProductRouteItem.iSchSdID = (int)dr["iSchSdID"];
                        lSchProductRouteItem.cVersionNo = dr["cVersionNo"].ToString();
                        lSchProductRouteItem.iProcessProductID = (int)dr["iProcessProductID"];
                        if (lSchProductRouteItem.iProcessProductID < 0) continue;
                        lSchProductRouteItem.iEntryID = (int)dr["iEntryID"];
                        lSchProductRouteItem.cWoNo = dr["cWoNo"].ToString();
                        lSchProductRouteItem.cInvCode = dr["cInvCode"].ToString().Trim();
                        lSchProductRouteItem.iWoSeqID = (int)dr["iSeqID"];
                        lSchProductRouteItem.cInvCodeFull = dr["cInvCodeFull"].ToString();
                        lSchProductRouteItem.cSubInvCode = dr["cSubInvCode"].ToString();
                        lSchProductRouteItem.cSubInvCodeFull = dr["cSubInvCodeFull"].ToString();
                        lSchProductRouteItem.bSelf = dr["bSelf"].ToString();
                        lSchProductRouteItem.cUtterType = dr["cUtterType"].ToString();
                        lSchProductRouteItem.cSubRelate = dr["cSubRelate"].ToString();
                        lSchProductRouteItem.iQtyPer = Convert.ToDouble(dr["iQtyPer"]);
                        lSchProductRouteItem.iScrapt = Convert.ToDouble(dr["iScrapt"]);
                        lSchProductRouteItem.iReqQty = Convert.ToDouble(dr["iReqQty"]);
                        lSchProductRouteItem.iNormalQty = Convert.ToDouble(dr["iNormalQty"]);
                        lSchProductRouteItem.iScrapQty = Convert.ToDouble(dr["iScrapQty"]);
                        lSchProductRouteItem.iProQty = Convert.ToDouble(dr["iProQty"]);
                        lSchProductRouteItem.iKeepQty = Convert.ToDouble(dr["iKeepQty"]);
                        lSchProductRouteItem.iPlanQty = Convert.ToDouble(dr["iPlanQty"]);
                        lSchProductRouteItem.dReqDate = Convert.ToDateTime(dr["dReqDate"]);
                        lSchProductRouteItem.dForeInDate = Convert.ToDateTime(dr["dForeInDate"]);
                        lSchProductRouteItem.iSchBatch = (int)dr["iSchBatch"];
                        lSchProductRouteItem.schProductRoute = schData.SchProductRouteList.Find(delegate (SchProductRoute p) { return p.iProcessProductID == lSchProductRouteItem.iProcessProductID; });
                        if (lSchProductRouteItem.schProductRoute == null)
                        {
                            continue;
                        }
                        schData.SchProductRouteItemList.Add(lSchProductRouteItem);
                    }
                }
                #endregion
                Console.WriteLine("2.6 建立对象之间的关系");
                #region//2.6 建立对象之间的关系
                int k = 0;
                foreach (SchProductRoute lSchProductRoute in schData.SchProductRouteList)
                {
                    lSchProductRoute.SchProductRouteResList = schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.iSchSdID == lSchProductRoute.iSchSdID && p.iProcessProductID == lSchProductRoute.iProcessProductID; });
                    if (lSchProductRoute.SchProductRouteResList.Count > 0)
                    {
                        foreach (SchProductRouteRes lSchProductRouteRes in lSchProductRoute.SchProductRouteResList)
                        {
                            lSchProductRouteRes.schProductRoute = lSchProductRoute;
                        }
                    }
                    else
                    {
                        lSchProductRoute.dBegDate = this.schData.dtStart;
                        lSchProductRoute.dEndDate = this.schData.dtStart.AddMinutes(1);
                        lSchProductRoute.BScheduled = 1;  //无工序，不参与排产
                    }
                    if (SchParam.cSelfEndDate == "1")
                        lSchProductRoute.SchProductRouteItemList = schData.SchProductRouteItemList.FindAll(delegate (SchProductRouteItem p) { return p.iSchSdID == lSchProductRoute.iSchSdID && p.iProcessProductID == lSchProductRoute.iProcessProductID; });
                    if (lSchProductRoute.cStatus != "2" && lSchProductRoute.cStatus != "4" && lSchProductRoute.cPreProcessItem != "")
                    {
                        GetSchProductRouteList(lSchProductRoute, true);
                    }  //执行工序重排
                    else if (SchParam.ExecTaskSchType != "1" && lSchProductRoute.cStatus != "4" && lSchProductRoute.cPreProcessItem != "")
                    {
                        GetSchProductRouteList(lSchProductRoute, true);
                    }
                    if (lSchProductRoute.cVersionNo != "SureVersion")
                    {
                        List<SchProductRouteRes> ListRouteRes = lSchProductRoute.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });
                        int iRandom;
                        int iCount = ListRouteRes.Count, iRowCount = ListRouteRes.Count;
                        int iResCount = lSchProductRoute.iDevCountPd;
                        if (iResCount == 0) iResCount = 2;
                        if (ListRouteRes.Count > 2 && iResCount < iCount)
                        {
                            for (int i = iRowCount - 1; i >= 0; i--)
                            {
                                if (ListRouteRes[i].resource.iDistributionRate >= 100) continue;
                                Random rd = new Random();
                                iRandom = rd.Next(1, 100);   //(生成1~100之间的随机数，不包括100)
                                if (iRandom > ListRouteRes[i].resource.iDistributionRate)     //如果随机结果大于设置值，则取消当前资源选择
                                {
                                    ListRouteRes[i].cSelected = "0";
                                    ListRouteRes[i].cCanScheduled = "0";   //不排产 
                                    ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                                    ListRouteRes[i].iResRationHour = 0;    //加工工时为0 
                                    iCount--;
                                }
                                if (iCount <= iResCount) break;
                            }
                        }
                    }
                    k++;
                }
                foreach (SchProduct lSchProduct in schData.SchProductList)
                {
                    lSchProduct.SchProductRouteList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.iSchSdID == lSchProduct.iSchSdID; });
                    foreach (SchProductRoute lSchProductRoute in lSchProduct.SchProductRouteList)
                    {
                        lSchProductRoute.schProduct = lSchProduct;
                    }
                }
                foreach (SchProductWorkItem lSchProductWorkItem in schData.SchProductWorkItemList)
                {
                    lSchProductWorkItem.SchProductRouteList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.iSchSdID == lSchProductWorkItem.iSchSdID && p.cWoNo == lSchProductWorkItem.cWoNo; });
                    foreach (SchProductRoute lSchProductRoute in lSchProductWorkItem.SchProductRouteList)
                    {
                        lSchProductRoute.schProductWorkItem = lSchProductWorkItem;
                    }
                }
                #endregion
                Console.WriteLine("2.7 其他基础资料表");
                #region//2.7 其他基础资料表
                schData.dtWorkCenter = SqlPro.GetDataTable(lsWorkCenter, null);//APSCommon.SqlPro.GetDataTable(lsWorkCenter, "t_WorkCenter");
                schData.dtDepartment = SqlPro.GetDataTable(lsDepartment, null);//APSCommon.SqlPro.GetDataTable(lsDepartment, "t_Department");
                schData.dtPerson = SqlPro.GetDataTable(lsPerson, null); //APSCommon.SqlPro.GetDataTable(lsPerson, "t_Person");
                schData.dtTeam = SqlPro.GetDataTable(lsteam, null); //APSCommon.SqlPro.GetDataTable(lsteam, "t_team");
                schData.dtTechLearnCurves = SqlPro.GetDataTable(lsTechLearnCurves, null); //APSCommon.SqlPro.GetDataTable(lsTechLearnCurves, "t_TechLearnCurves");
                schData.dtResTechScheduSN = SqlPro.GetDataTable(lsResTechScheduSN, null); //APSCommon.SqlPro.GetDataTable(lsResTechScheduSN, "t_ResTechScheduSN");
                #endregion
                List<SchProductRoute> SchProductRouteList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p1) { return p1.cDevCountPdExp != "" && p1.cWoNo == ""; });
                Console.WriteLine("第二步骤结束");
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置第二步骤,出错内容：" + ex1.ToString());
                throw ex1;
                return -1;
            }
            return 1;
        }
        private int ResSchTaskInit()
        {
            return 1;
        }
        private int GetSchProductRouteList(SchProductRoute as_SchProductRoute, Boolean bPreProcess = true)
        {
            int iSchSdID = as_SchProductRoute.iSchSdID;
            string cPreProcessItem;
            if (bPreProcess)   //找前序工序
                cPreProcessItem = as_SchProductRoute.cPreProcessItem;
            else
                cPreProcessItem = as_SchProductRoute.cPostProcessItem;
            if (cPreProcessItem == "") return -1;
            string[] ProcessItem = cPreProcessItem.Split('/');
            for (int i = 0; i < ProcessItem.Length; i++)
            {
                if (ProcessItem[i] == "") continue;
                SchProductRoute SchProductRoute1 = schData.SchProductRouteList.Find(delegate (SchProductRoute p) { return p.iSchSdID == iSchSdID && p.iProcessProductID == int.Parse(ProcessItem[i]); });
                if (SchProductRoute1 == null)
                {
                }
                else
                {
                    if (bPreProcess)  //找前序工序,传入待工工序
                    {
                        as_SchProductRoute.SchProductRoutePreList.Add(SchProductRoute1);
                        SchProductRoute1.SchProductRouteNextList.Add(as_SchProductRoute);
                    }
                    else       //找后序工序,暂不用
                    {
                        as_SchProductRoute.SchProductRouteNextList.Add(SchProductRoute1);
                        SchProductRoute1.SchProductRoutePreList.Add(as_SchProductRoute);
                    }
                }
            }
            return 1;
        }
        #endregion
        #region //4、排程结果写回数据库
        public int SaveSchData()
        {
            try
            {
                schData.dtResource.Dispose();
                schData.dt_ResourceSpecTime.Dispose();
                schData.dtSchProductRouteItem.Dispose();
                schData.dtSchResWorkTime.Dispose();
                schData.dtProChaType.Dispose();
                schData.dtResChaValue.Dispose();
                schData.dtResChaCrossTime.Dispose();
                schData.dtWorkCenter.Dispose();
                schData.dtDepartment.Dispose();
                schData.dtTeam.Dispose();
                schData.dtPerson.Dispose();
                schData.dtItem.Dispose();
                schData.dtTechInfo.Dispose();
                schData.dtTechLearnCurves.Dispose();
                schData.dtResTechScheduSN.Dispose();
                int li_return;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Console.WriteLine("排程任务时间段明细 t_SchProductRouteResTimeTemp，更新到t_SchProductRouteResTime");
                if (SaveDataResTime("SaveDataResTime") < 0) return -1;
                Console.WriteLine("2、保存t_SchProductRouteResTemp");
                if (SaveDataSchRouteRes("SaveDataSchRouteRes") < 0) return -1;
                Console.WriteLine("3、保存t_SchProductRouteTemp");
                if (SaveDataSchRoute("SaveDataSchRoute") < 0) return -1;
                Console.WriteLine("4、保存t_SchProductTemp");
                if (SaveSchProduct("SaveSchProduct") < 0) return -1;
                Console.WriteLine("5、保存t_SchProductWorkItemTemp");
                if (SaveSchProductWorkItem("SaveSchProductWorkItem") < 0) return -1;
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置4、排程结果写回数据库,出错内容：" + ex1.ToString());
                throw (ex1);
                return -1;
            }
            return 1;
        }
        public int SaveDataResTime(object cType)
        {
            int li_return;
            Console.WriteLine("开始保存排程任务时间段明细");
            try
            {
                string lsSql2 = "truncate table t_SchProductRouteResTimeTemp ";
                Console.WriteLine("ExcuteNonQuery开始");
                int iReturnNum = SqlPro.ExecuteNonQuery(lsSql2, null);
                Console.WriteLine("ExcuteNonQuery跳过");
                string lsSchProductRouteResTime = @"select cVersionNo,iSchSdID,iProcessProductID,isnull(iInterID,0) as iInterID,isnull(iWoProcessID,0) iWoProcessID,
                    isnull(iResProcessID,0) as iResProcessID,isnull(cWoNo,'') cWoNo ,isnull(iResourceID,0) as iResourceID,cResourceNo,cResourceName,
                    iTimeID,iPeriodTimeID,dResBegDate,dResEndDate,iResReqQty,isnull(iResRationHour,0) as iResRationHour,isnull(iResRealRationHour,0) as iResRealRationHour,isnull(cSimulateVer,'') as cSimulateVer,isnull(cNote,'') as cNote,isnull(cTaskType,'1') as cTaskType,dPeriodDay,isnull(FShiftType,'A班') FShiftType  
                        from t_SchProductRouteResTimeTemp where  1 = 2 ";
                DataTable dtSchProductRouteResTime = SqlPro.GetDataTable(lsSchProductRouteResTime, null); //APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteResTimeTemp where 1 = 2");
                int iTime = 1;
                foreach (SchProduct schProduct in schData.SchProductList)
                {
                    foreach (SchProductRoute schProductRoute in schProduct.SchProductRouteList)
                    {
                        foreach (SchProductRouteRes schProductRouteRes in schProductRoute.SchProductRouteResList)
                        {
                            schData.TaskTimeRangeList.AddRange(schProductRouteRes.TaskTimeRangeList);//.FindAll(delegate(TaskTimeRange p2) { return p2.cTaskType == 1; }));
                        }
                    }
                }
                Console.WriteLine("排程任务时段写回数据库");
                Console.WriteLine("进入foreach");
                foreach (TaskTimeRange lTaskTimeRange in schData.TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.cTaskType != 0; }))
                {
                    if (lTaskTimeRange.iProcessProductID == -1) continue;
                    if (lTaskTimeRange.resource.cIsInfinityAbility == "1") continue;
                    DataRow dr = dtSchProductRouteResTime.NewRow();
                    dr["iSchSdID"] = lTaskTimeRange.iSchSdID;
                    dr["cVersionNo"] = lTaskTimeRange.cVersionNo;
                    dr["iProcessProductID"] = lTaskTimeRange.iProcessProductID;
                    dr["iResProcessID"] = lTaskTimeRange.iResProcessID;
                    dr["cResourceNo"] = lTaskTimeRange.CResourceNo;
                    dr["cResourceName"] = lTaskTimeRange.resource.cResourceName;
                    dr["cWoNo"] = lTaskTimeRange.schProductRouteRes.cWoNo;
                    dr["iResReqQty"] = lTaskTimeRange.iResReqQty;
                    dr["dResBegDate"] = lTaskTimeRange.DBegTime;
                    dr["dResEndDate"] = lTaskTimeRange.DEndTime;
                    dr["cTaskType"] = lTaskTimeRange.cTaskType.ToString();    //任务时间类型： 0 空闲， 1 加工时间 2 维修时间  ---3 前准备时间 4 后准备时间 ，暂时没用
                    dr["iResRationHour"] = Math.Round((lTaskTimeRange.AllottedTime / 60.00), 2); //(Global.RationHourUnit == "3" ? lTaskTimeRange.AllottedTime : (Global.RationHourUnit == "1" ? lTaskTimeRange.AllottedTime / 3600.00 : lTaskTimeRange.AllottedTime / 60.00));//lTaskTimeRange.iResRationHour;
                    dr["iResRealRationHour"] = Math.Round((lTaskTimeRange.WorkTimeAct / 60.00), 2);    //有效工时
                    dr["iPeriodTimeID"] = lTaskTimeRange.resTimeRange.iPeriodID;
                    dr["dPeriodDay"] = lTaskTimeRange.resTimeRange.dPeriodDay;      //时段所属日期
                    dr["FShiftType"] = lTaskTimeRange.resTimeRange.FShiftType;      //班次 A班 夜班 中班等 
                    dr["iTimeID"] = iTime;
                    dr["iInterID"] = -1;// 特殊处理 ，否则无法保存 
                    dr["iWoProcessID"] = -1;//  特殊处理 ，否则无法保存
                    dr["iResourceID"] = -1;//  特殊处理 ，否则无法保存
                    dr["cSimulateVer"] = -1;//  特殊处理 ，否则无法保存
                    dr["cNote"] = -1;//  特殊处理 ，否则无法保存
                    dtSchProductRouteResTime.Rows.Add(dr);
                    iTime++;
                }
                    Console.WriteLine("批量保存数据库");
                    if (SqlBulkCopyResTime(dtSchProductRouteResTime, "dbo.t_SchProductRouteResTimeTemp") < 1) return -1;
                Console.WriteLine("排程任务时段写回数据库执行完毕");
                schData.TaskTimeRangeList.Clear();
                dtSchProductRouteResTime.Dispose();
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存资源任务排程时间段明细，单独线程执行,出错内容：" + ex1.ToString());
                return -1;
            }
            return 1;
        }
        public int SqlBulkCopyResTime(DataTable dt, string dtName)
        {
            SqlConnection connection = new SqlConnection(ConnectString); //new SqlConnection(Global.ConnectString);
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dtName;  //"dbo.[User]";//目标表，就是说您将要将数据插入到哪个表中去
                    bulkCopy.ColumnMappings.Add("iSchSdID", "iSchSdID");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("cVersionNo", "cVersionNo");
                    bulkCopy.ColumnMappings.Add("iProcessProductID", "iProcessProductID");
                    bulkCopy.ColumnMappings.Add("iResProcessID", "iResProcessID");
                    bulkCopy.ColumnMappings.Add("cResourceNo", "cResourceNo");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("cWoNo", "cWoNo");
                    bulkCopy.ColumnMappings.Add("iResReqQty", "iResReqQty");
                    bulkCopy.ColumnMappings.Add("dResBegDate", "dResBegDate");
                    bulkCopy.ColumnMappings.Add("dResEndDate", "dResEndDate");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("cTaskType", "cTaskType");
                    bulkCopy.ColumnMappings.Add("iResRationHour", "iResRationHour");
                    bulkCopy.ColumnMappings.Add("iResRealRationHour", "iResRealRationHour");
                    bulkCopy.ColumnMappings.Add("iPeriodTimeID", "iPeriodTimeID");
                    bulkCopy.ColumnMappings.Add("iTimeID", "iTimeID");
                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存2317,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            return 1;
        }
        public int SaveDataSchRouteRes(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID;
            int li_return;
            try
            {
                SchProductRouteRes lSchProductRouteRes;
                SchProductRoute lSchProductRoute;
                string lsSql2 = "truncate table  t_SchProductRouteResTemp";
                SqlPro.ExecuteNonQuery(lsSql2, null);
                DataTable dtSchProductRouteResTemp = SqlPro.GetDataTable("select * from t_SchProductRouteResTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteResTemp where 1 = 2");
                foreach (DataRow dr in schData.dtSchProductRouteRes.Rows)
                {
                    cVersionNo = dr["cVersionNo"].ToString().Trim();
                    iSchSdID = (int)dr["iSchSdID"];
                    iProcessProductID = (int)dr["iProcessProductID"];
                    iResProcessID = (int)dr["iResProcessID"];
                    lSchProductRouteRes = schData.SchProductRouteResList.Find(delegate (SchProductRouteRes p2) { return p2.iSchSdID == iSchSdID && p2.iProcessProductID == iProcessProductID && p2.iResProcessID == iResProcessID; });
                    if (lSchProductRouteRes == null) continue;
                    if (lSchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && lSchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || lSchProductRouteRes.iProcessProductID == 193864 && lSchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                    {
                    }
                    if (lSchProductRouteRes.schProductRoute == null)
                    {
                        message = "订单行号：" + lSchProductRouteRes.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + lSchProductRouteRes.iProcessProductID + "工单号:" + lSchProductRouteRes.cWoNo + "\n\r " + "没有对应的工序明细,请检查是否完工！";
                        schPrecent = schData.iProgress;
                        this.dlShowProcess(schPrecent.ToString(), message);
                        return -1;
                    }
                    cStatus = lSchProductRouteRes.schProductRoute.cStatus;
                    if (dr["iSchSdID"].ToString() == SchParam.iSchSdID.ToString() && dr["iProcessProductID"].ToString() == SchParam.iProcessProductID.ToString())
                    {
                        int m = 1;
                    }
                    if (lSchProductRouteRes.cWoNo == "WO141028001168")
                    {
                        int k = 0;
                    }
                    DataRow drNew = dtSchProductRouteResTemp.NewRow();
                    drNew["cVersionNo"] = dr["cVersionNo"].ToString().Trim();
                    drNew["iSchSdID"] = (int)dr["iSchSdID"];
                    drNew["iProcessProductID"] = (int)dr["iProcessProductID"];
                    drNew["iResProcessID"] = (int)dr["iResProcessID"];
                    drNew["cStatus"] = cStatus;
                    drNew["dResBegDate"] = lSchProductRouteRes.dResBegDate;
                    drNew["dResEndDate"] = lSchProductRouteRes.dResEndDate;
                    drNew["iResReqQty"] = lSchProductRouteRes.iResReqQty;
                    drNew["iResRationHour"] = Math.Round((lSchProductRouteRes.iResRationHour) / 60, 2);
                    drNew["iResPreTime"] = Math.Round((lSchProductRouteRes.iResPreTime) / 60, 2);
                    drNew["iResPostTime"] = Math.Round((lSchProductRouteRes.iResPostTime) / 60, 2);
                    drNew["iCycTime"] = lSchProductRouteRes.iCycTime;
                    drNew["cDefine35"] = lSchProductRouteRes.iSchSN;               //最新任务排产顺序
                    drNew["cDefine24"] = lSchProductRouteRes.iBatch.ToString();
                    drNew["cDefine25"] = lSchProductRouteRes.cResourceNo + " " + lSchProductRouteRes.cDefine25;
                    if (lSchProductRouteRes.dCanResBegDate < SchParam.dtToday)
                    {
                        drNew["dResCanBegDate"] = lSchProductRouteRes.dResBegDate;    //可开工时间
                        drNew["iResWaitTime"] = 0;                                   //等待时间
                    }
                    else if (lSchProductRouteRes.dCanResBegDate > lSchProductRouteRes.dResBegDate) //Convert.ToDateTime("2000-01-01"))
                    {
                        drNew["dResCanBegDate"] = lSchProductRouteRes.dCanResBegDate;    //可开工时间
                        drNew["iResWaitTime"] = 0;                                   //等待时间
                    }
                    else
                    {
                        drNew["dResCanBegDate"] = lSchProductRouteRes.dCanResBegDate;    //可开工时间
                        drNew["iResWaitTime"] = lSchProductRouteRes.iResWaitTime;      //等待时间
                    }
                    drNew["cDefine27"] = lSchProductRouteRes.cDefine27;         //排程开工时间
                    drNew["cDefine37"] = lSchProductRouteRes.cDefine37 < this.schData.dtStart ? this.schData.dtStart : lSchProductRouteRes.cDefine37;         //排程结束时间
                    drNew["cDefine38"] = lSchProductRouteRes.cDefine38 < this.schData.dtStart ? this.schData.dtStart : lSchProductRouteRes.cDefine38;         //排程优先级最早可排时间
                    if (lSchProductRouteRes.schProductRoute.schProductWorkItem != null)
                    {
                        drNew["cSdOrderNo"] = lSchProductRouteRes.schProductRoute.schProductWorkItem.cSdOrderNo;
                        drNew["cPriorityType"] = lSchProductRouteRes.schProductRoute.schProductWorkItem.cPriorityType;
                        drNew["iPriority"] = lSchProductRouteRes.schProductRoute.schProductWorkItem.iPriority;
                    }
                    else
                    {
                        drNew["cSdOrderNo"] = lSchProductRouteRes.schProductRoute.schProduct.cSdOrderNo;
                        drNew["cPriorityType"] = lSchProductRouteRes.schProductRoute.schProduct.cPriorityType;
                        drNew["iPriority"] = lSchProductRouteRes.schProductRoute.schProduct.iPriority;
                    }
                    drNew["cWoNo"] = lSchProductRouteRes.cWoNo;
                    drNew["iPriorityResLast"] = lSchProductRouteRes.iPriorityRes;   //记录上次排产优先级
                    drNew["iPriorityRes"] = lSchProductRouteRes.iSchSN;
                    drNew["iSchBatch"] = lSchProductRouteRes.iSchBatch;
                    drNew["iTimeCount"] = lSchProductRouteRes.TaskTimeRangeList.Count.ToString();
                    drNew["iSchSN"] = lSchProductRouteRes.iSchSN;
                    drNew["cResourceNo"] = lSchProductRouteRes.cResourceNo;
                    drNew["cResourceName"] = lSchProductRouteRes.resource.cResourceName;
                    drNew["cTeamResourceNo"] = lSchProductRouteRes.cTeamResourceNo;
                    dtSchProductRouteResTemp.Rows.Add(drNew);
                }
                if (SqlBulkCopyRouteRes(dtSchProductRouteResTemp, "dbo.t_SchProductRouteResTemp") < 0) return -1;   //2021-08-24 xx
                schData.dtSchProductRouteRes.Dispose();
                schData.SchProductRouteResList.Clear();
                dtSchProductRouteResTemp.Dispose();
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存资源任务明细t_SchProductRouteResTemp，单独线程执行2515,出错内容：" + ex1.ToString());
                return -1;
            }
            return 1;
        }
        public int SqlBulkCopyRouteRes(DataTable dt, string dtName)
        {
            SqlConnection connection = new SqlConnection(ConnectString);
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dtName;  //"dbo.[User]";//目标表，就是说您将要将数据插入到哪个表中去
                    bulkCopy.ColumnMappings.Add("cVersionNo", "cVersionNo");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("iSchSdID", "iSchSdID");//数据源中的列名与目标表的属性的映射关系                    
                    bulkCopy.ColumnMappings.Add("iProcessProductID", "iProcessProductID");
                    bulkCopy.ColumnMappings.Add("iResProcessID", "iResProcessID");
                    bulkCopy.ColumnMappings.Add("cStatus", "cStatus");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("dResBegDate", "dResBegDate");
                    bulkCopy.ColumnMappings.Add("dResEndDate", "dResEndDate");
                    bulkCopy.ColumnMappings.Add("iResReqQty", "iResReqQty");
                    bulkCopy.ColumnMappings.Add("iResPreTime", "iResPreTime");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("iResPostTime", "iResPostTime");
                    bulkCopy.ColumnMappings.Add("iResRationHour", "iResRationHour");
                    bulkCopy.ColumnMappings.Add("dResCanBegDate", "dResCanBegDate");  //任务最早可开工时间  2019-12-09
                    bulkCopy.ColumnMappings.Add("iResWaitTime", "iResWaitTime");      //工序等待时间
                    bulkCopy.ColumnMappings.Add("iCycTime", "iCycTime");
                    bulkCopy.ColumnMappings.Add("cDefine35", "cDefine35");            //最新任务排产顺序
                    bulkCopy.ColumnMappings.Add("cDefine24", "cDefine24");
                    bulkCopy.ColumnMappings.Add("cDefine25", "cDefine25");
                    bulkCopy.ColumnMappings.Add("cDefine26", "cDefine26");
                    bulkCopy.ColumnMappings.Add("cDefine36", "cDefine36");
                    bulkCopy.ColumnMappings.Add("cDefine33", "cDefine33");
                    bulkCopy.ColumnMappings.Add("cDefine27", "cDefine27");
                    bulkCopy.ColumnMappings.Add("cDefine37", "cDefine37");
                    bulkCopy.ColumnMappings.Add("cResourceNo", "cResourceNo");
                    bulkCopy.ColumnMappings.Add("cTeamResourceNo", "cTeamResourceNo");
                    bulkCopy.ColumnMappings.Add("cSdOrderNo", "cSdOrderNo");
                    bulkCopy.ColumnMappings.Add("cPriorityType", "cPriorityType");
                    bulkCopy.ColumnMappings.Add("cWoNo", "cWoNo");
                    bulkCopy.ColumnMappings.Add("iPriorityResLast", "iPriorityResLast");
                    bulkCopy.ColumnMappings.Add("iPriorityRes", "iPriorityRes");
                    bulkCopy.ColumnMappings.Add("iSchBatch", "iSchBatch");
                    bulkCopy.ColumnMappings.Add("iTimeCount", "iTimeCount");
                    bulkCopy.ColumnMappings.Add("iSchSN", "iSchSN");
                    bulkCopy.ColumnMappings.Add("iPriority", "iPriority");
                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存2568,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            return 1;
        }
        public int SaveDataSchRoute(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID;
            int li_return;
            DataRow[] SchProductRouteRows = schData.dtSchProductRoute.Select("cStatus not in ('4')");
            string lsSql2 = "truncate table  t_SchProductRouteTemp";
            SqlPro.ExecuteNonQuery(lsSql2, null);
            DataTable dtSchProductRouteTemp =SqlPro.GetDataTable("select * from t_SchProductRouteTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteTemp where 1 = 2");
            try
            {
                SchProductRoute lSchProductRoute;
                foreach (DataRow dr in SchProductRouteRows)//schData.dtSchProductRoute.Rows
                {
                    cVersionNo = dr["cVersionNo"].ToString();
                    iSchSdID = (int)dr["iSchSdID"];
                    iProcessProductID = (int)dr["iProcessProductID"];
                    cStatus = dr["cStatus"].ToString();
                    lSchProductRoute = schData.SchProductRouteList.Find(delegate (SchProductRoute p2) { return p2.iSchSdID == iSchSdID && p2.iProcessProductID == iProcessProductID; });
                    if (lSchProductRoute == null) continue;
                    if (lSchProductRoute.dBegDate < SchParam.dtToday.AddDays(-1)) continue;
                    cStatus = lSchProductRoute.cStatus;
                    DataRow drNew = dtSchProductRouteTemp.NewRow();
                    drNew["cVersionNo"] = dr["cVersionNo"].ToString().Trim();
                    drNew["iSchSdID"] = (int)dr["iSchSdID"];
                    drNew["iProcessProductID"] = (int)dr["iProcessProductID"];
                    drNew["cStatus"] = cStatus;
                    drNew["dBegDate"] = lSchProductRoute.dBegDate;
                    drNew["dEndDate"] = lSchProductRoute.dEndDate;
                    drNew["iLaborTime"] = (RationHourUnit == "3" ? lSchProductRoute.iLaborTime : (RationHourUnit == "1" ? lSchProductRoute.iLaborTime / 3600 : lSchProductRoute.iLaborTime / 60));
                    drNew["iLeadTime"] = lSchProductRoute.iLeadTime;
                    drNew["iSeqPreTime"] = lSchProductRoute.iSeqPreTime;
                    drNew["iSeqPostTime"] = lSchProductRoute.iSeqPostTime;
                    drNew["cWoNo"] = lSchProductRoute.cWoNo;     //增加工单号 2021-11-30 Jonas Cheng 
                    drNew["cDefine27"] = "";//lSchProductRoute.cDefine27.Substring(120);         //排程开始时间
                    drNew["cDefine28"] = "";//lSchProductRoute.cDefine28.Substring(120);         //倒排开工日期
                    if (lSchProductRoute.dEarlyBegDate <= lSchProductRoute.dBegDate)
                        drNew["dCanBegDate"] = lSchProductRoute.dBegDate;
                    else
                        drNew["dCanBegDate"] = lSchProductRoute.dEarlyBegDate;  //工序最早可排产时间,用于分析工序异常等待时间  2019-12-09 Jonas Cheng 
                    dtSchProductRouteTemp.Rows.Add(drNew);
                }
                if (SqlBulkCopySchRoute(dtSchProductRouteTemp, "dbo.t_SchProductRouteTemp") < 0) return -1;  //2021-08-24
                schData.dtSchProductRoute.Dispose();
                schData.SchProductRouteList.Clear();
                dtSchProductRouteTemp.Dispose();
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存工序明细t_SchProductRouteTemp，单独线程执行,出错内容：" + ex1.ToString());
                return -1;
            }
            return 1;
        }
        public int SqlBulkCopySchRoute(DataTable dt, string dtName)
        {
            SqlConnection connection = new SqlConnection(ConnectString);
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dtName;  //"dbo.[User]";//目标表，就是说您将要将数据插入到哪个表中去
                    bulkCopy.ColumnMappings.Add("cVersionNo", "cVersionNo");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("iSchSdID", "iSchSdID");//数据源中的列名与目标表的属性的映射关系                    
                    bulkCopy.ColumnMappings.Add("iProcessProductID", "iProcessProductID");
                    bulkCopy.ColumnMappings.Add("cStatus", "cStatus");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("dBegDate", "dBegDate");
                    bulkCopy.ColumnMappings.Add("dEndDate", "dEndDate");
                    bulkCopy.ColumnMappings.Add("iLaborTime", "iLaborTime");
                    bulkCopy.ColumnMappings.Add("iLeadTime", "iLeadTime");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("iSeqPreTime", "iSeqPreTime");
                    bulkCopy.ColumnMappings.Add("iSeqPostTime", "iSeqPostTime");
                    bulkCopy.ColumnMappings.Add("dCanBegDate", "dCanBegDate");
                    bulkCopy.ColumnMappings.Add("cWoNo", "cWoNo");
                    bulkCopy.ColumnMappings.Add("cDefine27", "cDefine27");      //可开工时间
                    bulkCopy.ColumnMappings.Add("cDefine28", "cDefine28");      //倒排开工日期
                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存2754,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            return 1;
        }
        public int SaveSchProduct(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID;
            int li_return;
            if (schData.dtSchProduct == null || schData.dtSchProduct.Rows.Count < 1)
            {
                return 1;
            }
            DataRow[] SchProductRows = schData.dtSchProduct.Select("cStatus not in ('F','C')");
            string lsSql2 = "truncate table  t_SchProductTemp";
            SqlPro.ExecuteNonQuery(lsSql2, null);
            DataTable dtSchProductTemp = SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2");
            try
            {
                SchProduct lSchProduct;
                foreach (DataRow dr in SchProductRows)
                {
                    cVersionNo = dr["cVersionNo"].ToString();
                    iSchSdID = (int)dr["iSchSdID"];
                    lSchProduct = schData.SchProductList.Find(delegate (SchProduct p2) { return p2.iSchSdID == iSchSdID && p2.cVersionNo == cVersionNo; });
                    if (lSchProduct == null) continue;
                    DataRow drNew = dtSchProductTemp.NewRow();
                    drNew["cVersionNo"] = cVersionNo;                        //排程版本号
                    drNew["iSchSdID"] = iSchSdID;                            //排程ID
                    drNew["dRequireDate"] = lSchProduct.dRequireDate;         //预计完工时间
                    drNew["dEarliestSchDate"] = lSchProduct.dEarliestSchDate; //排程开始时间
                    drNew["dEndDate"] = (lSchProduct.dEndDate < DateTime.Parse("1901-12-1") ? lSchProduct.dRequireDate : lSchProduct.dEndDate);       //预计完工时间
                    drNew["dBegDate"] = (lSchProduct.dBegDate < DateTime.Parse("1901-12-1") ? schData.dtStart : lSchProduct.dBegDate);          //排程开始时间
                    drNew["cSchStatus"] = "1";                             //已排产     
                    drNew["iPriority"] = lSchProduct.iSchPriority;         //排程优先级 反写
                    dtSchProductTemp.Rows.Add(drNew);
                    if (dtSchProductTemp.Rows.Count > iBatchRowCount)
                    {
                            li_return = SqlPro.BulkInsert(dtSchProductTemp, "t_SchProductTemp");
                            if (li_return < 0) return -1;
                            dtSchProductTemp = SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2");
                    }
                }
                    if (SqlBulkCopySch(dtSchProductTemp, "dbo.t_SchProductTemp") < 0) return -1;  //2021-08-24
                schData.dtSchProduct.Dispose();
                schData.SchProductList.Clear();
                dtSchProductTemp.Dispose();
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存排程订单信息2909,出错内容：" + ex1.ToString());
                return -1;
            }
            return 1;
        }
        public int SaveSchProductWorkItem(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID, iBomAutoID;
            int li_return;
            if (schData.dtSchProductWorkItem.Rows.Count < 1) return 1;
            DataRow[] SchProductWorkItemRows = schData.dtSchProductWorkItem.Select("cStatus not in ('F','C')");
            string lsSql2 = "truncate table  t_SchProductWorkItemTemp";
            SqlPro.ExecuteNonQuery(lsSql2, null);
            DataTable dtSchProductWorkItemTemp = SqlPro.GetDataTable("select * from t_SchProductWorkItemTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductWorkItemTemp where 1 = 2");
            try
            {
                SchProductWorkItem lSchProductWorkItem;
                foreach (DataRow dr in SchProductWorkItemRows)
                {
                    cVersionNo = dr["cVersionNo"].ToString();
                    iSchSdID = (int)dr["iSchSdID"];
                    iBomAutoID = (int)dr["iBomAutoID"];
                    lSchProductWorkItem = schData.SchProductWorkItemList.Find(delegate (SchProductWorkItem p2) { return p2.iSchSdID == iSchSdID && p2.cVersionNo == cVersionNo && p2.iBomAutoID == iBomAutoID; });
                    if (lSchProductWorkItem == null) continue;
                    DataRow drNew = dtSchProductWorkItemTemp.NewRow();
                    drNew["cVersionNo"] = cVersionNo;                        //排程版本号
                    drNew["iSchSdID"] = iSchSdID;                            //排程ID
                    drNew["iBomAutoID"] = iBomAutoID;                            //排程ID
                    drNew["dBegDate"] = lSchProductWorkItem.dBegDate;         //预计完工时间
                    drNew["dEndDate"] = lSchProductWorkItem.dEndDate;         //排程开始时间
                    if (iSchSdID == SchParam.iSchSdID)
                    {
                        int j = 1;
                    }
                    drNew["dCanBegDate"] = (lSchProductWorkItem.dCanBegDate < DateTime.Parse("1901-12-1") ? schData.dtStart : lSchProductWorkItem.dCanBegDate);         //预计完工时间
                    drNew["dCanEndDate"] = (lSchProductWorkItem.dCanEndDate < DateTime.Parse("1901-12-1") ? schData.dtStart : lSchProductWorkItem.dCanEndDate);          //排程开始时间
                    drNew["cWoNo"] = lSchProductWorkItem.cWoNo;                           //工单号
                    drNew["cInvCode"] = lSchProductWorkItem.cInvCode;                     //物料编号
                    drNew["cSchSNType"] = lSchProductWorkItem.cSchSNType;                   //座次类别
                    drNew["iSchSN"] = lSchProductWorkItem.iSchSN;                          //座次号
                    drNew["iPriority"] = lSchProductWorkItem.iPriority;                   //订单优先级 反写
                    drNew["iWoPriorityResLast"] = lSchProductWorkItem.iWoPriorityResLast; //排程优先级 反写
                    dtSchProductWorkItemTemp.Rows.Add(drNew);
                    if (dtSchProductWorkItemTemp.Rows.Count > iBatchRowCount)
                    {
                        SqlPro.BulkInsert(dtSchProductWorkItemTemp, "dbo.t_SchProductWorkItemTemp");
                    }
                }
                    if (SqlBulkCopySchWorkItem(dtSchProductWorkItemTemp, "dbo.t_SchProductWorkItemTemp") < 0) return -1;
                schData.dtSchProduct.Dispose();
                schData.SchProductList.Clear();
                dtSchProductWorkItemTemp.Dispose();
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存排程工单信息3031,出错内容：" + ex1.ToString());
                return -1;
            }
            return 1;
        }
        public int SqlBulkCopySch(DataTable dt, string dtName)
        {
            SqlConnection connection = new SqlConnection(ConnectString);
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dtName;  //"dbo.[User]";//目标表，就是说您将要将数据插入到哪个表中去
                    bulkCopy.ColumnMappings.Add("cVersionNo", "cVersionNo");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("iSchSdID", "iSchSdID");//数据源中的列名与目标表的属性的映射关系                    
                    bulkCopy.ColumnMappings.Add("dRequireDate", "dRequireDate");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("dEarliestSchDate", "dEarliestSchDate");
                    bulkCopy.ColumnMappings.Add("dBegDate", "dBegDate");
                    bulkCopy.ColumnMappings.Add("dEndDate", "dEndDate");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("cSchStatus", "cSchStatus");
                    bulkCopy.ColumnMappings.Add("iPriority", "iPriority");
                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存3068,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            return 1;
        }
        public int SqlBulkCopySchWorkItem(DataTable dt, string dtName)
        {
            SqlConnection connection = new SqlConnection(ConnectString);
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectString, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dtName;  //"dbo.[User]";//目标表，就是说您将要将数据插入到哪个表中去
                    bulkCopy.ColumnMappings.Add("cVersionNo", "cVersionNo");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("iSchSdID", "iSchSdID");//数据源中的列名与目标表的属性的映射关系     
                    bulkCopy.ColumnMappings.Add("iBomAutoID", "iBomAutoID");//数据源中的列名与目标表的属性的映射关系                
                    bulkCopy.ColumnMappings.Add("cWoNo", "cWoNo");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("dCanBegDate", "dCanBegDate");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("dCanEndDate", "dCanEndDate");
                    bulkCopy.ColumnMappings.Add("dBegDate", "dBegDate");
                    bulkCopy.ColumnMappings.Add("dEndDate", "dEndDate");//数据源中的列名与目标表的属性的映射关系
                    bulkCopy.ColumnMappings.Add("iPriority", "iPriority");
                    bulkCopy.ColumnMappings.Add("iWoPriorityResLast", "iWoPriorityResLast");  //最新排产顺序                    
                    bulkCopy.ColumnMappings.Add("cInvCode", "cInvCode");
                    bulkCopy.ColumnMappings.Add("cSchSNType", "cSchSNType");
                    bulkCopy.ColumnMappings.Add("iSchSN", "iSchSN");
                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存3113,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }
            return 1;
        }
        public static int PerBatchEnd()
        {
            int maxWorkerThreads, workerThreads;
            int portThreads;
            return 1;
        }
        public string GetRunTime(string cType = "m")
        {
            return "";
        }
        public string GetcCalculateNo()
        {
            return this.schData.cCalculateNo;
        }
        public void ShowSchProgress()
        {
            int iLastRows = 0;
            if (schData.iTotalRows < 1) return;
            while (schData.iCurRows <= schData.iTotalRows)
            {
                if (iLastRows != schData.iCurRows)
                {
                    schData.iProgress = 30 + (int)schData.iCurRows * 50 / schData.iTotalRows;   //1、过程GetSchData  100%
                    if (this.dlShowProcess != null)
                        this.dlShowProcess(this.schData.iProgress.ToString(), "5、排程计算进行中，当前记录/总记录数[" + schData.iCurRows + "/" + schData.iTotalRows + "] ");
                    iLastRows = schData.iCurRows;
                }
                if (schData.iProgress < 80)
                    Thread.Sleep(1000);
            }
        }
        #endregion
        public string GetParamValue(string param)
        {
            return "";
        }
        public async Task SendAsync2(string message, string topic = "apsRun")
        {
            if (client == null)
            {
                client = new SocketIOClient.SocketIO(this.ServerUri, new SocketIOClient.SocketIOOptions
                {
                    ConnectionTimeout = TimeSpan.FromSeconds(2),
                    Transport = SocketIOClient.Transport.TransportProtocol.WebSocket,  //特别增加，否则连不上
                });
                client.OnConnected += (sender, e) =>
                {
                    Console.WriteLine("Connected to the server.");
                };
                client.OnConnected += async (sender, e) =>
                {
                    Console.WriteLine("Connected to the server");
                    await client.EmitAsync("apsRun", "Hello, server!");
                };
                client.OnError += (sender, e) =>
                {
                    Console.WriteLine("Error: " + e);
                };
                client.On("chat message", response =>
                {
                    Console.WriteLine("Received message: " + response.GetValue<string>());
                });
                var cts = new System.Threading.CancellationTokenSource(10000);
                try
                {
                    await client.ConnectAsync();
                    Console.WriteLine("message连接成功 " + this.ServerUri);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("message连接失败" + this.ServerUri);
                }
            }
            try
            {
                if (client.Connected == false)
                    await client.ConnectAsync();
                message += ";User:" + this.User;
                message += ";Company:" + this.Company;
                message += ";socketId:" + this.socketId;
                await client.EmitAsync(topic, message);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Connection attempt timed out.");
            }
        }
        public async Task SendAsync(string message, string topic = "apsRun")
        {
            message += ";User:" + this.User;
            message += ";Company:" + this.Company;
            message += ";socketId:" + this.socketId;
            message += ";topic:" + topic;
            Console.WriteLine("message连接成功 " + this.ServerUri +  " 内容：" + message);
                    string url = this.ServerUri + $"api/apsMessage?message={Uri.EscapeDataString(message)}"; // 将消息作为查询参数添加到URL中
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            HttpResponseMessage response = await client.GetAsync(url);
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("响应内容: " + responseBody);
                        }
                        catch (HttpRequestException e)
                        {
                            Console.WriteLine("请求出错: " + e.Message);
                        }
                    }
        }
    }
}