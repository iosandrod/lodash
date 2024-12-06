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



//1、排程算法基础数据准备；


//2、排程算法排程结果数据输出


namespace APSRun
{
    public class SchInterface
    {
              

        public async Task<Object> SetInit(dynamic input)
        {
            string ConnectString = (string)input.ConnectString;
            ServerUri = (string)input.ServerUri;
            //string ConnectString,string ServerUri
            APSRun.SqlPro.ConnectionStrings = ConnectString.ToString();
            this.ConnectString = ConnectString.ToString();
            //this.ServerUri = ServerUri;
            //webSocketClient.ServerUri = new Uri(ServerUri);

            //this.dlShowProcess = new DLShowProcess(ShowProcess);
            return ConnectString + "__123456 _" + ServerUri;
        }



        //初始化数据库链接,先调SetInit  再调Start
        //public async Task<string> SetInitB(string ConnectString, string ServerUri = "ws://localhost:9000", string Company = "", string cUser = "admin")
        public async Task<string> SetInitB(string ConnectString, string ServerUri = "ws://192.168.1.15:9000", string Company = "", string cUser = "admin")
        {
            APSRun.SqlPro.ConnectionStrings = ConnectString;
            this.ConnectString = ConnectString;
            this.ServerUri = ServerUri;
            //webSocketClient.ServerUri = new Uri(ServerUri);

            this.dlShowProcess = new DLShowProcess(ShowProcess);
            return "123456";
        }
        //定义变量
        //所有排程数据都放在SchData中


        public SchData schData = new SchData();
        public DateTime ldtBegDate;
        public APSRun.SqlPro sqlPro = new APSRun.SqlPro();
        public string RationHourUnit = "1";     //
        public string ConnectString = "";
        public string ServerUri = "ws://192.168.1.15:9000";//"ws://localhost:9000";
        //public WebSocketClient webSocketClient = new WebSocketClient();

        public SocketIOClient.SocketIO client = null;


        //public FrmProgress frmProgress = new FrmProgress();  进度条

        public int iProgress = 0;
        public int iBatchRowCount = 50000;   //每8万行保存一次

        public string name = "GetSchOperationProgress";  //发送排程进度事件
        public int schPrecent;                 //排程进度百分比
        public string message;     //排程事件信息
        public string User = "admin";

        //string ConnectString = (string)input.ConnectString;
        //string ServerUri = (string)input.ServerUri;
        public string Company = "001";
        public string socketId = "123";


        //var host = CreateHostBuilder(args).Build();
        //var hubContext = host.Services.GetService(typeof(IHubContext<MessageHub>));

        //private readonly IHubContext<MessageHub> _hubContext;
        public delegate void DLShowProcess(string schPrecent, string message);
        public DLShowProcess dlShowProcess;

        //public SchInterface(IHubContext<MessageHub> hubContext)
        //{
        //    _hubContext = hubContext;
        //    dlShowProcess = new DLShowProcess(ShowProcess);
        //}

        //发送进度信息到前端
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
            //webSocketClient.
            //_hubContext.Clients.All.SendAsync(name, schPrecent, message + " " + this.GetRunTime());
        }


        //public string cVersionNo = "";
        //public DateTime dtStart;
        //public DateTime dtEnd;

        //private DataTable dtSchProduct = new DataTable();
        //private DataTable dtResource = new DataTable();
        //private DataTable dtSchProductRoute = new DataTable();
        //private DataTable dtSchProductRouteRes = new DataTable();
        //private DataTable dtSchProductRouteResTime = new DataTable();  
        //private DateTable dtSchResWorkTime = new DataTable();

        ////资源列表 dtResource
        //public List<Resource> ResourceList = new List<Resource>(10);

        ////再建立他们的联系
        ////排程订单产品列表 dtSchProduct
        //public List<SchProduct> SchProductList = new List<SchProduct>(10);

        ////排程订单产品工艺模型 dtSchProductRoute
        //public List<SchProductRoute> SchProductRouteList = new List<SchProductRoute>(10);

        ////工序对应的可用资源列表  dtSchProductRouteRes 
        //public List<SchProductRouteRes> SchProductRouteResList = new List<SchProductRouteRes>(10);

        ////任务占用资源时间段 t_SchProductRouteResTime
        //public List<TaskTimeRange> TaskTimeRangeList = new List<TaskTimeRange>(10);
        //   const payload = {
        //        cVersionNo:"NewVersion",
        //        adte_Start:"2024-07-29 16:07:00",
        //        adte_End:"2024-12-29 16:07:00",
        //        cSchType:"0",
        //        cTechSchType:"0";
        //        iCurSchRunTimes:"0";
        //        User:"admin",
        //        ConnectString:"",
        //        ServerUri:"ws://localhost:9000",
        //        Company:"0001"

        //}

        //public async Task<int>   Start(string cVersionNo, DateTime adte_Start, DateTime adte_End, Boolean bShowTips = true, string cSchType = "0", string cTechSchType = "0", string iCurSchRunTimes = "0",string User = "admin",string ConnectString = "", string ServerUri = "ws://localhost:9000", string Company = "")
        //(dynamic input )
        public async Task<object> Start(dynamic input)
        {
            //string cVersionNo, DateTime adte_Start, DateTime adte_End,  string cSchType = "0", string cTechSchType = "0", 
            //string iCurSchRunTimes = "0", string User = "admin", string ConnectString = "", string ServerUri = "ws://localhost:9000", string Company = ""

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
            //this.webSocketClient.ServerUri = new Uri(ServerUri);

            this.dlShowProcess = new DLShowProcess(ShowProcess);

            Console.WriteLine("ConnectString " + ConnectString);
            Console.WriteLine("this.ServerUri " + this.ServerUri);


            //if (SaveDataResTime("SaveDataResTime") < 0) return -1;

            //return 1;
            //设置Algorithm项目的数据库连接字符串
            //Algorithm.Common.ConnectionStrings = APS.Global.ConnectString;

            //开始排程 发送信息
            message = "开始智能排程";
            schPrecent = 0;
            // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);

            //设置超时时间
            //GetSqlDapper(UserContext.Current.getCompanyID()).SetTimout(720000);


            message = "取排程参数";
            schPrecent = 1;
            // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            Console.WriteLine("进入排程函数");

            //2车间优化排产时，只能是正式版本 "SureVersion"
            if (cSchType == "2")
            {
                cVersionNo = "SureVersion";
                //cTechSchType = "";        //严格按选的工艺类型排产 2021-12-11 JonasCheng
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
            //SchParam.bReSchedule = "0";//Global.GetUserParam("bReSchedule");   // 1 重排; 0 第一次排
            SchParam.bReSchedule = "0"; // Global.GetUserParam("bReSchedule");   // 1 重排; 0 第一次排 2022-01-21

            SchParam.cSchCapType = "1";   // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能  2019-03-07
            SchParam.cSchType = cSchType;   // //排程方式  0 ---正常排产, 1--资源调度优化排产2020-08-25 ， 2--按工单优先级调度优化排产（正式版本）3 --按资源调度优化排产 
            SchParam.cTechSchType = cTechSchType;   //工艺段排产方式  0    普通排产 1    注塑排产 2    装配排产 3    表面处理 4    委外排产
            SchParam.iCurSchRunTimes = iCurSchRunTimes;   //当前排程次数

            //取排程参数  ExecuteReader

            //var reader = DBServerProvider.SqlDapper.ExecuteReader("select * from t_ParamValue where 1 = 1 ", null);
            //DataTable table = new DataTable();
            //table.Load(reader);
            //DataTable dt =


            //Algorithm.SchParam.dtParamValue = GetSqlDapper(UserContext.Current.getCompanyID()).GetDataTable("select * from t_ParamValue where 1 = 1 ", null);//ToDataTable(DBServerProvider.SqlDapper.QueryList<object>("select * from t_ParamValue where 1 = 1 ",null));//系统参数表 //APSCommon.SqlPro.GetDataTable("select * from t_ParamValue where 1 = 1 ", "t_ParamValue");//系统参数表
            Algorithm.SchParam.dtParamValue = SqlPro.GetDataTable("select * from t_ParamValue where 1 = 1 ", "");//ToDataTable(DBServerProvider.SqlDapper.QueryList<object>("select * from t_ParamValue where 1 = 1 ",null));//系统参数表 //APSCommon.SqlPro.GetDataTable("select * from t_ParamValue where 1 = 1 ", "t_ParamValue");//系统参数表


            // 1、取排程参数
            Algorithm.SchParam.GetSchParams();

            Console.WriteLine("初始化参数完成");
            message = "初始化参数完成";
            schPrecent = 2;
            // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);


            if (SchParam.bReSchedule != "1")   //0 第一次排
            {
                Console.WriteLine("bReSchedule != 1 0第一次排");

                message = "第一次排";
                schPrecent = 2;
                this.dlShowProcess(schPrecent.ToString(), message);
                //SendAsync(name, schPrecent, message);
                //0、排程前后台处理
                try
                {
                    //因为执行时间长，分开两个过程处理 2021-10-27 JonasCheng 
                    //1、执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中

                    string lsSql2 = string.Format(@"EXECUTE P_SchedulePrePre '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", this.schData.cVersionNo, this.schData.dtStart.ToString(), this.schData.dtEnd.ToString(), SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo, SchParam.iCurSchRunTimes);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP
                    SqlPro.ExecuteNonQuery(lsSql2, null);



                    //执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中
                    string lsSql = string.Format(@"EXECUTE P_SchedulePre '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", this.schData.cVersionNo, this.schData.dtStart.ToString(), this.schData.dtEnd.ToString(), SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo, SchParam.iCurSchRunTimes);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP

                    message = "1、排程前准备工作已完成";
                    schPrecent = 3;
                    //SendAsync(name, schPrecent, message);
                    this.dlShowProcess(schPrecent.ToString(), message);
                    SqlPro.ExecuteNonQuery(lsSql, null);
                    Console.WriteLine("执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中");



                    //string lsSql = @"EXECUTE P_SchedulePre @cVersion,@dSchBegDate,@dSchEndDate,@cSchType,@cTechSchType,@cHostName";
                    //SqlParameter[] lsSqlParameters = new SqlParameter[6]{
                    //                                                         Common.GenerateSqlParameter("@cVersion", SqlDbType.VarChar, this.schData.cVersionNo ),
                    //                                                         Common.GenerateSqlParameter("@dSchBegDate", SqlDbType.VarChar, this.schData.dtStart.ToString() ),
                    //                                                         Common.GenerateSqlParameter("@dSchEndDate", SqlDbType.VarChar, this.schData.dtEnd.ToString() ) ,
                    //                                                         Common.GenerateSqlParameter("@cSchType", SqlDbType.VarChar, SchParam.cSchType),
                    //                                                         Common.GenerateSqlParameter("@cTechSchType", SqlDbType.VarChar, SchParam.cTechSchType),
                    //                                                         Common.GenerateSqlParameter("@cHostName", SqlDbType.VarChar, Global.User)
                    //                                                        };
                    //APSCommon.SqlPro.ExecuteNonQuery(CommandType.Text, lsSql, ref lsSqlParameters);
                }
                catch (Exception excp)
                {
                    message = "排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString();
                    schPrecent = 100;
                    // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
                    this.dlShowProcess(schPrecent.ToString(), message);
                    Console.WriteLine("排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString());
                    throw (excp);
                    //Messagezk.Show(APS.Common.TransPro.TransString(string.Format("排产前处理出错！{0}", excp.Message.ToString())), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return -1;
                }

                //Global.logger.Info(TransPro.TransString("P_SchedulePre执行完成"), "排程运算");

                schData.iProgress = 5;   //1、过程P_SchedulePre  5%
                Console.WriteLine("完成5%");
                message = "1、排程前准备工作已完成:";
                schPrecent = schData.iProgress;
                // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
                this.dlShowProcess(schPrecent.ToString(), message);
                //显示进度
                //this.frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "1、排程前准备工作已完成:" + this.GetRunTime(), schData.iProgress });

                Console.WriteLine(bShowTips);
                //检查是否有出错信息
                if (bShowTips)
                {
                    //if (CheckErrorPro.ShowError("Cal", "APS", "") < 0) return -1;
                }
                else  //不显示出错信息

                {
                    //if (CheckErrorPro.GetErrorCount("Cal", "APS", "") > 0) return -1;
                }

            }

            // 2、生成资源工作列表及资源工作日历
            try
            {
                if (GetResourceList() < 0) return -1;
            }
            catch (Exception excp)
            {
                schPrecent = 100;
                // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
                this.dlShowProcess(schPrecent.ToString(), message);
                Console.WriteLine("排产计算出错！位置2、生成资源工作列表及资源工作日历,出错内容：" + excp.ToString());
                //Messagezk.Show(APS.Common.TransPro.TransString(string.Format("排产前处理出错！{0}", excp.Message.ToString())), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw (excp);

                return -1;
            }

            schData.iProgress = 15;   //1、过程GetResourceList  10%
            Console.WriteLine("过程GetResourceList  15%");

            message = "2、资源工作日历已完成";
            schPrecent = schData.iProgress;
            // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            //frmProgress.iProgress = schData.iProgress;
            //显示进度
            //////frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "2、资源工作日历已完成:" + this.GetRunTime(), schData.iProgress });

            //frmProgress.UpdateProgress(frmProgress, new System.ComponentModel.ProgressChangedEventArgs(schData.iProgress, null));


            // 3、生成订单产品工艺模型列表,先分别填充SchProductList，SchProductRouteList，SchProductRouteResList
            if (GetSchData() < 0) return -1;



            schData.iProgress = 20;   //1、过程GetSchData  20%
            Console.WriteLine("过程GetSchData  20%");

            message = "3、生成订单产品工艺模型列表";
            schPrecent = schData.iProgress;
            // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            //显示进度
            //frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "3、生成订单产品工艺模型列表:" + this.GetRunTime(), schData.iProgress });
            //frmProgress.UpdateProgress(frmProgress, new System.ComponentModel.ProgressChangedEventArgs(schData.iProgress, null));


            //4、已开工任务排程

            //开个线程统计排程明细进度
            //System.Threading.Thread threadRows = new System.Threading.Thread(new System.Threading.ThreadStart(ShowSchProgress));
            //threadRows.Start();
            Console.WriteLine("开个线程统计排程明细进度");
            //Thread.Sleep(200);



            if (this.ResSchTaskInit() < 0) return -1;



            schData.iProgress = 30;   //1、过程GetSchData  30%

            Console.WriteLine("过程GetSchData  30%");

            message = "4、已开工任务排程...";

            schPrecent = schData.iProgress;

            // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            //frmProgress.UpdateProgress(frmProgress, new System.ComponentModel.ProgressChangedEventArgs(schData.iProgress, null));
            //显示进度
            //frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "4、已开工任务排程:" + this.GetRunTime(), schData.iProgress });


            //5、产品订单按优先级进行排产

            try
            {
                Scheduling scheduling = new Scheduling(this.schData);

                //开个线程统计排程明细进度
                System.Threading.Thread threadRows = new System.Threading.Thread(new System.Threading.ThreadStart(ShowSchProgress));
                threadRows.Start();
                Thread.Sleep(200);

                if (scheduling.SchMainRun(cSchType) < 0) return -1;
            }
            catch (Exception excp)
            {
                message = "排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString();
                schPrecent = 100;
                // _hubContext.Clients.All.SendAsync(name, schPrecent, message);
                this.dlShowProcess(schPrecent.ToString(), message);
                Console.WriteLine("排产计算出错！位置1、排产前处理出错,出错内容：" + excp.ToString());
                //Messagezk.Show(APS.Common.TransPro.TransString(string.Format("排产前处理出错！{0}", excp.Message.ToString())), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw (excp);

                return -1;
            }

            //Global.logger.Info(TransPro.TransString("SchMainRun执行完成"), "排程运算");

            schData.iProgress = 80;   //1、过程GetSchData  80%

            Console.WriteLine("过程GetSchData  80%");

            message = "5、产品订单按优先级进行排产";
            schPrecent = schData.iProgress;
            //_hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);


            //frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "5、产品订单按优先级进行排产:" + this.GetRunTime(), schData.iProgress });

            #region//增加提示
            //if (Global.cCustomer == "ShuangYe")
            //{
            //if (Global.CurDataTime > DateTime.Parse("2020-6-28"))
            //{
            //    APS.Common.Messagezk.Show("排产运算出错,有数据为Null值,请检查基础数据!");
            //    return -1;
            //}
            //}
            #endregion

            //6、排程结果写回数据库
            if (SaveSchData() < 0)
            {
                //frmProgress.Dispose();
                return -1;
            }


            schData.iProgress = 90;   //1、过程GetSchData  90%
            message = "6、排程结果写回数据库";
            schPrecent = schData.iProgress;
            //_hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            //frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "6、排程结果写回数据库:" + this.GetRunTime(), schData.iProgress });
            //frmProgress.UpdateProgress(frmProgress, new System.ComponentModel.ProgressChangedEventArgs(schData.iProgress, null));

            //7、排程完成后，后台数据库处理
            try
            {

                //执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中

                string lsSql = string.Format(@"EXECUTE P_SchedulePost '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", this.schData.cVersionNo, this.schData.dtStart.ToString(), this.schData.dtEnd.ToString(), this.schData.dtToday.ToString(), SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo, SchParam.iCurSchRunTimes);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP
                var result = (SqlPro.ExecuteNonQuery(lsSql, null));

            }
            catch (Exception excp)
            {
                Console.WriteLine("排产计算出错！位置GetResourceList(),出错内容：" + excp.ToString());
                throw (excp);
                //frmProgress.Dispose();
                //Messagezk.Show(APS.Common.TransPro.TransString(string.Format("排产后处理出错！{0}", excp.Message.ToString())), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }

            schData.iProgress = 95;   //1、过程GetSchData  %
            message = "7、排程后处理完成";
            schPrecent = schData.iProgress;
            //_hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            Console.WriteLine("过程GetSchData  95%");
            //frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "7、排程后处理完成:" + this.GetRunTime(), schData.iProgress });
            //8、排程完成后，记录KPI
            try
            {

                ////执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中
                //string lsSql = string.Format(@"EXECUTE P_SchedulePost '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", this.schData.cVersionNo, ldtBegDate, DateTime.Now, Global.User, "", SchParam.cSchType, SchParam.cTechSchType);
                //var result = (DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql, null));

                //执行P_GetNewSchVersionData过程返回查询结果，写入表t_SchProduct中  2022-01-21
                string lsSql = string.Format(@"EXECUTE P_SchKPI '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", this.schData.cVersionNo, ldtBegDate, DateTime.Now, this.schData.cCalculateNo, "", SchParam.cSchType, SchParam.cTechSchType);
                var result = (SqlPro.ExecuteNonQuery(lsSql, null));

                //string lsSql = @"EXECUTE P_SchKPI @cVersion,@dSchBegDate,@dSchEndDate,@cUser,@sErrText,@cSchType,@cTechSchType";
                //SqlParameter[] lsSqlParameters = new SqlParameter[7]{
                //                                                         Common.GenerateSqlParameter("@cVersion", SqlDbType.VarChar, this.schData.cVersionNo ),
                //                                                         Common.GenerateSqlParameter("@dSchBegDate", SqlDbType.VarChar, ldtBegDate ),
                //                                                         Common.GenerateSqlParameter("@dSchEndDate", SqlDbType.VarChar, DateTime.Now ),
                //                                                         Common.GenerateSqlParameter("@cUser", SqlDbType.VarChar, Global.User),
                //                                                         Common.GenerateSqlParameter("@sErrText", SqlDbType.VarChar, "" ),
                //                                                         Common.GenerateSqlParameter("@cSchType", SqlDbType.VarChar, SchParam.cSchType),
                //                                                         Common.GenerateSqlParameter("@cTechSchType", SqlDbType.VarChar, SchParam.cTechSchType)
                //                                                        };
                //APSCommon.SqlPro.ExecuteNonQuery(CommandType.Text, lsSql, ref lsSqlParameters);
            }
            catch (Exception excp)
            {
                //frmProgress.Dispose();
                Console.WriteLine("排产后记录KPI" + excp.Message.ToString());
                throw (excp);
                //Messagezk.Show(APS.Common.TransPro.TransString(string.Format("排产后记录KPI！{0}", excp.Message.ToString())), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            schData.iProgress = 98;   //1、过程GetSchData  100%
            message = "8、排产后记录KPI完成";
            schPrecent = schData.iProgress;
            //_hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            //frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "8、排产后记录KPI完成:" + this.GetRunTime(), schData.iProgress });
            Console.WriteLine("排程完毕  100%");
            schData.iProgress = 100;
            message = "排程完毕";
            schPrecent = schData.iProgress;
            //_hubContext.Clients.All.SendAsync(name, schPrecent, message);
            this.dlShowProcess(schPrecent.ToString(), message);
            return 1;



        }


        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
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
            //string lsResAddWhere = String.Format(@"  and cStatus <> '3' and (cResourceNo in (SELECT cResourceNo FROM t_SchProductRouteRes WHERE cVersionNo = '{0}' or cVersionNo = 'SureVersion'
            //     UNION SELECT cViceResource1No FROM t_SchProductRouteRes WHERE (cVersionNo = '{0}' or cVersionNo = 'SureVersion' ) and isnull(cViceResource1No,'') <> '' 
            //     UNION SELECT cViceResource2No FROM t_SchProductRouteRes WHERE (cVersionNo = '{0}' or cVersionNo = 'SureVersion' ) and isnull(cViceResource2No,'') <> '' 
            //     UNION SELECT cViceResource3No FROM t_SchProductRouteRes WHERE (cVersionNo = '{0}' or cVersionNo = 'SureVersion' ) and isnull(cViceResource3No,'') <> '') or isnull(cTeamResourceNo,'') <> '') ", this.schData.cVersionNo);

            ////取生成了资源工作日历的资源列表
            ///
            Console.WriteLine("取生成了资源工作日历的资源列表");
            string lsResAddWhere = String.Format(@"  and cStatus <> '3' and (cResourceNo in (select distinct cResourceNo 
            //                                                                                    FROM t_SchResWorkTime 
                     //                                                                                       WHERE cVersionNo = '{0}'
                     //                                                                                         AND cType = '1'         
                     //                                                                                         AND cSourceType = '1') or isnull(cTeamResourceNo,'') <> '') ", this.schData.cVersionNo);
            ////dtResResource //关键资源，按排产顺序排序     
            //string lsSql = "SELECT cDeptno,cWcNo,cResourceNo,cResourceName,isnull(iKeySchSN,-1) as iKeySchSN FROM t_Resource WHERE iKeySchSN >= 0 AND cIsKey = 1 and cStatus <> '3'" + lsResAddWhere + "  ORDER BY cDeptno,cWcNo,iKeySchSN ";
            //this.schData.dtResResource = APSCommon.SqlPro.GetDataTable(lsSql, "t_Resource");

            //dtResource //所有资源
            //Console.WriteLine("cVersionNo:" + this.schData.cVersionNo);
            ////非停用资源，参与本次排产 --2020-12-22
            //string lsSql = @"select iResourceID,cResourceNo,cResourceName,cResClsNo,cResourceType,iResourceNumber,isnull(cResOccupyType,'0') as cResOccupyType,isnull(iPreStocks,0) as iPreStocks,isnull(iPostStocks,0) iPostStocks,isnull(iUsage,100) as iUsage,isnull(iEfficient,100) as iEfficient,isnull(cResouceInformation,'') as cResouceInformation ,isnull(cIsInfinityAbility,0) as cIsInfinityAbility,mResourcePicture,isnull(iWcID,-1) iWcID,cWcNo,cDeptNo,isnull(cDeptName,'') cDeptName,cStatus,isnull(iSchemeID,-1) iSchemeID,isnull(iCacheTime,0) iCacheTime,isnull(iLastBatchPercent,0) iLastBatchPercent ,
            //        cIsKey,isnull(iKeySchSN,-1) as iKeySchSN,isnull(cNeedChanged,0) as cNeedChanged,cProChaType1Sort,isnull(FProChaType1ID,-1) FProChaType1ID, isnull(FProChaType2ID,-1) FProChaType2ID,isnull(FProChaType3ID,-1) FProChaType3ID,isnull(FProChaType4ID,-1) FProChaType4ID,isnull(FProChaType5ID,-1) FProChaType5ID,isnull(FProChaType6ID,-1) FProChaType6ID,isnull(FProChaType7ID,-1) FProChaType7ID,isnull(FProChaType8ID,-1) FProChaType8ID,isnull(FProChaType9ID,-1) FProChaType9ID,
            //        isnull(FProChaType10ID,-1) FProChaType10ID,isnull(FProChaType11ID,-1) FProChaType11ID,isnull(FProChaType12ID,-1) FProChaType12ID,isnull(cResDefine1,'') as cResDefine1,isnull(cResDefine2,'') as cResDefine2,isnull(cResDefine3,'') as cResDefine3,isnull(cResDefine4,'') as cResDefine4,
            //        isnull(cResDefine5,'') as cResDefine5,isnull(cResDefine6,'') as cResDefine6,isnull(cResDefine7,'') as cResDefine7,isnull(cResDefine8,'') as cResDefine8,isnull(cResDefine9,'') as cResDefine9,isnull(cResDefine10,'') as cResDefine10,isnull(cResDefine11,0) as cResDefine11,isnull(cResDefine12,0) as cResDefine12,isnull(cResDefine13,0) as cResDefine13,isnull(cResDefine14,0) as cResDefine14,isnull(cResDefine15,'1900-01-01') as cResDefine15,isnull(cResDefine16,'1900-01-01') as cResDefine16,
            //        isnull(cDayPlanShowType,'1') as cDayPlanShowType,dMaxExeDate,isnull(iChangeTime,0) as iChangeTime ,isnull(iResPreTime,0) as iResPreTime,isnull(iTurnsType,'0') as iTurnsType,isnull(iTurnsTime,1) as iTurnsTime,
            //        isnull(iLabRate,0) as iLabRate,isnull(cTeamNo,'') as cTeamNo,isnull(cTeamNo2,'') as cTeamNo2,isnull(cTeamNo3,'') as cTeamNo3,isnull(cBatch1Filter,'') as cBatch1Filter,isnull(cBatch2Filter,'') as cBatch2Filter,isnull(cBatch3Filter,'') as cBatch3Filter,isnull(cBatch4Filter,'') as cBatch4Filter,isnull(cBatch5Filter,'') as cBatch5Filter,isnull(iBatchWoSeqID,10) as iBatchWoSeqID,
            //        isnull(iTurnsTime,0) as cBatch1WorkTime,isnull(cBatch2WorkTime,isnull(iTurnsTime,0)) as cBatch2WorkTime,isnull(cBatch3WorkTime,isnull(iTurnsTime,0)) as cBatch3WorkTime,isnull(cBatch4WorkTime,isnull(iTurnsTime,0)) as cBatch4WorkTime,isnull(cBatch5WorkTime,isnull(iTurnsTime,0)) as cBatch5WorkTime,isnull(cPriorityType,'2') as cPriorityType, isnull(cResBarCode,'') as cResBarCode,
            //         isnull(cTeamResourceNo,'')  as cTeamResourceNo,isnull(bTeamResource,0) as bTeamResource,isnull(cSuppliyMode,0) as cSuppliyMode,isnull(cResOperator,'') as cResOperator,isnull(cResManager,'')  as cResManager,
            //        isnull(iOverResourceNumber,1) as iOverResourceNumber,isnull(iLimitResourceNumber,1) as iLimitResourceNumber, isnull(iOverEfficient,100) as iOverEfficient, isnull(iLimitEfficient,100) as iLimitEfficient,isnull(iResDifficulty,1) as iResDifficulty,isnull(iDistributionRate,100)  as iDistributionRate,
            //        isnull(iResWorkersPd,1) as iResWorkersPd  ,isnull(iResHoursPd,8) as iResHoursPd , isnull(iResOverHoursPd,0) as iResOverHoursPd , isnull(iPowerRate,0) as iPowerRate,isnull(iOtherRate,0) as iOtherRate,isnull(iMinWorkTime,0) as iMinWorkTime
            //        from t_Resource  with (nolock)
            //        where 1 = 1 ";
            //lsSql += lsResAddWhere + " ORDER BY isnull(cPriorityType,'2') ,isnull(iKeySchSN,-1) ";

            //Console.WriteLine("lsSql:" + lsSql);
            //lsSql += " and  isnull(cStatus,'0') <> '3'  and  isnull(cSchSelected,'0') = '1' " + " ORDER BY isnull(cPriorityType,'2') ,isnull(iKeySchSN,-1) ";


            //放到资源工作日历后取数
            //this.schData.dtResource = APSCommon.SqlPro.GetDataTable(lsSql, "t_Resource");

            // 资源档案改到用过程取数 2022-01-21
            string lstg_Sql = string.Format("EXECUTE P_GetSchDataResource '{0}','{1}','{2}','{3}','{4}','{5}'", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo);
            schData.dtResource = SqlPro.GetDataTable(lstg_Sql, null);

            //取工艺特征类型


            string lsSql = "SELECT FProChaTypeID,FResChaTypeName,cParentClsNo,bEnd,cBarCode,cNote,cMacNo FROM t_ProChaType where isnull(bEnd,1) = '1'";
            this.schData.dtProChaType = SqlPro.GetDataTable(lsSql, null);//APSCommon.SqlPro.GetDataTable(lsSql, "t_ProChaType");
            Console.WriteLine("取工艺特征类型");
            //取工艺特征值


            lsSql = @"SELECT     FResChaValueID, FResChaValueNo, FResChaValueName, FProChaTypeID,isnull(FResChaCycleValue,0) as  FResChaCycleValue, isnull(FResChaRePlaceTime,0) as FResChaRePlaceTime, FResChaMemo,isnull(FUseFixedPlaceTime,'1') as FUseFixedPlaceTime, 
                            FSchSN, isnull(FUseChaCycleValue,'0') as FUseChaCycleValue, cDefine1, cDefine2, cDefine3, cDefine4, cDefine5, cDefine6, cDefine7, cDefine8, cDefine9, cDefine10, isnull(cDefine11,0) as cDefine11, isnull(cDefine12,0) as cDefine12, isnull(cDefine13,0) as cDefine13, 
                            isnull(cDefine14,0) as cDefine14,isnull(cDefine15,'1900-01-01') cDefine15,isnull(cDefine16,'1900-01-01')  cDefine16
                        FROM         dbo.t_ResChaValue  with (nolock) where 1 = 1 ";
            this.schData.dtResChaValue = SqlPro.GetDataTable(lsSql, null);//APSCommon.SqlPro.GetDataTable(lsSql, "t_ResChaValue");
            Console.WriteLine("取工艺特征值");
            //取工艺特征转换时间


            lsSql = "SELECT FProChaTypeID,FResChaValue1ID,FResChaValue2ID,FResChaExcTime FROM dbo.t_ResChaCrossTime  with (nolock) where 1 = 1";
            this.schData.dtResChaCrossTime = SqlPro.GetDataTable(lsSql, null); //APSCommon.SqlPro.GetDataTable(lsSql, "t_ResChaCrossTime");
            Console.WriteLine("取工艺特征转换时间");
            //取工艺特征转换时间



            lsSql = "SELECT FProChaTypeID,FResChaValue1ID,FResChaValue2ID,FResChaExcTime FROM dbo.t_ResChaCrossTime  with (nolock) where 1 = 1";
            this.schData.dtResChaCrossTime = SqlPro.GetDataTable(lsSql, null); //APSCommon.SqlPro.GetDataTable(lsSql, "t_ResChaCrossTime");
            Console.WriteLine("取工艺特征转换时间");
            // //取排程参数


            // 2022-01-21
            //Algorithm.SchParam.dtParamValue = DBServerProvider.SqlDapper.GetDataTable("select * from t_ParamValue where 1 = 1 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_ParamValue where 1 = 1 ", "t_ParamValue");//系统参数表

            Console.WriteLine("取排程参数");

            //更新未排订单排程可开始时间


            lstg_Sql = "";

            lstg_Sql = "Update t_SchProduct set  dEarLiestSchDate = '" + this.schData.dtStart + "' where dEarLiestSchDate < '" + this.schData.dtStart + "' and isnull(cWoNo,'') = ''";
            SqlPro.ExecuteNonQuery(lstg_Sql, null);
            Console.WriteLine("更新未排订单排程可开始时间");
            try
            {
                Console.WriteLine("schData.dtStart - schData.dtEnd" + schData.dtStart.AddDays(-20) + schData.dtEnd);


                lstg_Sql = string.Format(@"EXECUTE P_GetResWorkTime '{0}','{1}','{2}','{3}','{4}','{5}'", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo);  //'1'全部 '2'自制件MRP运算  '3'采购件MRP

                //1、调用过程P_GetResWorkTime生成所有资源的工作日历
                DataTable dt_ResourceTime = SqlPro.GetDataTable(lstg_Sql, null);
                this.schData.dt_ResourceTime = dt_ResourceTime;
                this.schData.dt_ResourceTime.DefaultView.Sort = "iPeriodTimeID asc,dPeriodDay asc";


                schData.iProgress = 13;
                message = "2.1、资源工作日历生成完成";
                schPrecent = schData.iProgress;
                this.dlShowProcess(schPrecent.ToString(), message);
                //SendAsync(name, schPrecent, message);

                Console.WriteLine("调用过程P_GetResWorkTime生成所有资源的工作日历");
                //取资源特殊工作日历数据


                lstg_Sql = @"SELECT * FROM t_SchResWorkTime  with (nolock) WHERE cType = '1' and cSourceType = '2' AND cVersionNo = '" + schData.cVersionNo + "'";
                DataTable dt_ResourceSpecTime = SqlPro.GetDataTable(lstg_Sql, null);//DataTable dt_ResourceSpecTime = APSCommon.SqlPro.GetDataTable(lstg_Sql, "t_SchResWorkTime");

                this.schData.dt_ResourceSpecTime = dt_ResourceSpecTime;
                this.schData.dt_ResourceSpecTime.DefaultView.Sort = "dPeriodDay asc";

                //Console.WriteLine("取生成了资源工作日历的资源列表");
                ////取生成了资源工作日历的资源列表 //or isnull(cTeamResourceNo,'') <> ''
                //lsResAddWhere = String.Format(@"  and cStatus <> '3' and (cResourceNo in (select distinct cResourceNo 
                //                                                                                FROM t_SchResWorkTime 
                //                                                                                            WHERE cVersionNo = '{0}'
                //                                                                                              AND cType = '1'         
                //                                                                                              AND cSourceType = '1') ) ", this.schData.cVersionNo);
                //////dtResResource //关键资源，按排产顺序排序     
                ////string lsSql = "SELECT cDeptno,cWcNo,cResourceNo,cResourceName,isnull(iKeySchSN,-1) as iKeySchSN FROM t_Resource WHERE iKeySchSN >= 0 AND cIsKey = 1 and cStatus <> '3'" + lsResAddWhere + "  ORDER BY cDeptno,cWcNo,iKeySchSN ";
                ////this.schData.dtResResource = APSCommon.SqlPro.GetDataTable(lsSql, "t_Resource");

                ////dtResource //所有资源

                //lsSql = @"select iResourceID,cResourceNo,cResourceName,cResClsNo,cResourceType,iResourceNumber,isnull(cResOccupyType,'0') as cResOccupyType,isnull(iPreStocks,0) as iPreStocks,isnull(iPostStocks,0) iPostStocks,isnull(iUsage,100) as iUsage,isnull(iEfficient,100) as iEfficient,isnull(cResouceInformation,'') as cResouceInformation ,isnull(cIsInfinityAbility,0) as cIsInfinityAbility,mResourcePicture,isnull(iWcID,-1) iWcID,cWcNo,cDeptNo,isnull(cDeptName,'') cDeptName,cStatus,isnull(iSchemeID,-1) iSchemeID,isnull(iCacheTime,0) iCacheTime,isnull(iLastBatchPercent,0) iLastBatchPercent ,
                //    cIsKey,isnull(iKeySchSN,-1) as iKeySchSN,isnull(cNeedChanged,0) as cNeedChanged,cProChaType1Sort,isnull(FProChaType1ID,-1) FProChaType1ID, isnull(FProChaType2ID,-1) FProChaType2ID,isnull(FProChaType3ID,-1) FProChaType3ID,isnull(FProChaType4ID,-1) FProChaType4ID,isnull(FProChaType5ID,-1) FProChaType5ID,isnull(FProChaType6ID,-1) FProChaType6ID,isnull(FProChaType7ID,-1) FProChaType7ID,isnull(FProChaType8ID,-1) FProChaType8ID,isnull(FProChaType9ID,-1) FProChaType9ID,
                //    isnull(FProChaType10ID,-1) FProChaType10ID,isnull(FProChaType11ID,-1) FProChaType11ID,isnull(FProChaType12ID,-1) FProChaType12ID,isnull(cResDefine1,'') as cResDefine1,isnull(cResDefine2,'') as cResDefine2,isnull(cResDefine3,'') as cResDefine3,isnull(cResDefine4,'') as cResDefine4,
                //    isnull(cResDefine5,'') as cResDefine5,isnull(cResDefine6,'') as cResDefine6,isnull(cResDefine7,'') as cResDefine7,isnull(cResDefine8,'') as cResDefine8,isnull(cResDefine9,'') as cResDefine9,isnull(cResDefine10,'') as cResDefine10,isnull(cResDefine11,0) as cResDefine11,isnull(cResDefine12,0) as cResDefine12,isnull(cResDefine13,0) as cResDefine13,isnull(cResDefine14,0) as cResDefine14,isnull(cResDefine15,'1900-01-01') as cResDefine15,isnull(cResDefine16,'1900-01-01') as cResDefine16,
                //    isnull(cDayPlanShowType,'1') as cDayPlanShowType,dMaxExeDate,isnull(iChangeTime,0) as iChangeTime ,isnull(iResPreTime,0) as iResPreTime,isnull(iTurnsType,'0') as iTurnsType,isnull(iTurnsTime,1) as iTurnsTime,
                //    isnull(iLabRate,0) as iLabRate,isnull(cTeamNo,'') as cTeamNo,isnull(cTeamNo2,'') as cTeamNo2,isnull(cTeamNo3,'') as cTeamNo3,isnull(cBatch1Filter,'') as cBatch1Filter,isnull(cBatch2Filter,'') as cBatch2Filter,isnull(cBatch3Filter,'') as cBatch3Filter,isnull(cBatch4Filter,'') as cBatch4Filter,isnull(cBatch5Filter,'') as cBatch5Filter,isnull(iBatchWoSeqID,10) as iBatchWoSeqID,
                //    isnull(iTurnsTime,0) as cBatch1WorkTime,isnull(cBatch2WorkTime,isnull(iTurnsTime,0)) as cBatch2WorkTime,isnull(cBatch3WorkTime,isnull(iTurnsTime,0)) as cBatch3WorkTime,isnull(cBatch4WorkTime,isnull(iTurnsTime,0)) as cBatch4WorkTime,isnull(cBatch5WorkTime,isnull(iTurnsTime,0)) as cBatch5WorkTime,isnull(cPriorityType,'2') as cPriorityType, isnull(cResBarCode,'') as cResBarCode,
                //     isnull(cTeamResourceNo,'')  as cTeamResourceNo,isnull(bTeamResource,0) as bTeamResource,isnull(cSuppliyMode,0) as cSuppliyMode,isnull(cResOperator,'') as cResOperator,isnull(cResManager,'')  as cResManager,
                //    isnull(iOverResourceNumber,1) as iOverResourceNumber,isnull(iLimitResourceNumber,1) as iLimitResourceNumber, isnull(iOverEfficient,100) as iOverEfficient, isnull(iLimitEfficient,100) as iLimitEfficient,isnull(iResDifficulty,1) as iResDifficulty,isnull(iDistributionRate,100)  as iDistributionRate,
                //    isnull(iResWorkersPd,1) as iResWorkersPd  ,isnull(iResHoursPd,8) as iResHoursPd , isnull(iResOverHoursPd,0) as iResOverHoursPd , isnull(iPowerRate,0) as iPowerRate,isnull(iOtherRate,0) as iOtherRate,isnull(iMinWorkTime,0) as iMinWorkTime
                //    from t_Resource with (nolock)
                //    where 1 = 1 ";
                //lsSql += lsResAddWhere + " ORDER BY cPriorityType,isnull(iKeySchSN,-1) ";
                ////lsSql += " and  isnull(cStatus,'0') <> '3'  and  isnull(cSchSelected,'0') = '1' " + " ORDER BY isnull(cPriorityType,'2') ,isnull(iKeySchSN,-1) ";


                ////取资源列表 2020-01-14 Jonas Cheng 
                //this.schData.dtResource = DBServerProvider.SqlDapper.GetDataTable(lsSql, null);//this.schData.dtResource = APSCommon.SqlPro.GetDataTable(lsSql, "t_Resource");


                string cWorkCenter;
                WorkCenter lobj_WorkCenter;
                ////3、生成工作中心
                Console.WriteLine("3、生成工作中心");
                //按工作中心循环，生成工作中心对象
                foreach (DataRow drWorkCenter in this.schData.dtWorkCenter.Rows)
                {
                    cWorkCenter = drWorkCenter["cWcNo"].ToString();

                    lobj_WorkCenter = new Algorithm.WorkCenter(cWorkCenter, this.schData);
                    this.schData.WorkCenterList.Add(lobj_WorkCenter);

                }


                //2、循环每个资源，生成资源对象，并加到资源列表中


                string cResourceNoOld = "";
                string cResourceNo;

                Resource lobj_Resource;
                ResTimeRange lResTimeRange;
                int ResTimeRangeType;

                Console.WriteLine("开始按资源循环，生成资源对象，同时生成资源时间段");

                //按资源循环，生成资源对象，同时生成资源时间段
                foreach (DataRow drResource in this.schData.dtResource.Rows)
                {
                    cResourceNo = drResource["cResourceNo"].ToString();

                    if (cResourceNo == "gys20039")
                    {
                        int m = 1;
                    }

                    lobj_Resource = new Resource(cResourceNo, this.schData);
                    this.schData.ResourceList.Add(lobj_Resource);

                    //非资源组,资源组没有时间段的

                    if (lobj_Resource.bTeamResource != "1")
                    {

                        DataRow[] drResTime = dt_ResourceTime.Select("cResourceNo = '" + cResourceNo + "'");

                        //ResTimeRange lResTimeRangePre = null ;
                        //if (drResTime.Length < 1)
                        //{
                        //    //Messagezk.Show("排产计算出错！位置GetResourceList(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //    throw new Exception("位置GetResourceList(),出错内容：资源编号【"+ cResourceNo + "】没有定义工作日期时间段");
                        //    return -1;
                        //}

                        //取资源工作时间段
                        foreach (DataRow drResTimeRange in drResTime)
                        {
                            lResTimeRange = new ResTimeRange();

                            //lResTimeRange.ResTimeRangePre = lResTimeRangePre; //前一时间段                         

                            lResTimeRange.CResourceNo = cResourceNo;
                            lResTimeRange.resource = lobj_Resource;

                            //资源产能无限 0 有限，1 无限
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


                            //lResTimeRange.ResTimeRangeType = Convert.ToInt32(dt_ResourceTime.Rows[row]["cTimeRangeType"]);
                            lResTimeRange.Attribute = (Algorithm.TimeRangeAttribute)(int.Parse(drResTimeRange["cTimeRangeType"].ToString()));

                            //转换为秒
                            //lResTimeRange.HoldingTime = Convert.ToInt32(float.Parse(drResTimeRange["iHoldingTime"].ToString())) * 60 ; 


                            //生成空的任务时间段
                            lResTimeRange.GetNoWorkTaskTimeRange(lResTimeRange.DBegTime, lResTimeRange.DEndTime, true);

                            ////记录上一时间段的后时间段
                            //if (lResTimeRangePre != null)
                            //    lResTimeRangePre.ResTimeRangePost = lResTimeRange;

                            //添加资源时段
                            lobj_Resource.ResTimeRangeList.Add(lResTimeRange);

                            ////记录当前资源时间段
                            //lResTimeRangePre = lResTimeRange;

                        }

                        DataRow[] drResSpecTime = dt_ResourceSpecTime.Select("cResourceNo = '" + cResourceNo + "'");
                        //取资源特殊时间段
                        foreach (DataRow drResTimeRange in drResSpecTime)
                        {
                            lResTimeRange = new ResTimeRange();
                            lResTimeRange.CResourceNo = cResourceNo;
                            lResTimeRange.resource = lobj_Resource;

                            lResTimeRange.iPeriodID = int.Parse(drResTimeRange["iPeriodTimeID"].ToString());//drResource["iPeriodTimeID"]; 
                            //资源产能无限 0 有限，1 无限
                            lResTimeRange.CIsInfinityAbility = drResource["cIsInfinityAbility"].ToString();


                            lResTimeRange.DBegTime = (DateTime)drResTimeRange["dPeriodBegTime"];
                            lResTimeRange.DEndTime = (DateTime)drResTimeRange["dPeriodEndTime"];
                            //lResTimeRange.ResTimeRangeType = Convert.ToInt32(dt_ResourceTime.Rows[row]["cTimeRangeType"]);
                            lResTimeRange.Attribute = (Algorithm.TimeRangeAttribute)(int.Parse(drResTimeRange["cTimeRangeType"].ToString()));

                            lResTimeRange.HoldingTime = Convert.ToInt32(float.Parse(drResTimeRange["iHoldingTime"].ToString())) * 60;

                            //资源特殊日历,加班、维修等
                            lobj_Resource.ResSpecTimeRangeList.Add(lResTimeRange);

                        }

                        //合并工作日历,处理特殊工作日历
                        if (drResSpecTime.Length > 0)
                        {
                            lobj_Resource.MergeTimeRange();
                        }

                        //生成资源日限产时间段，每天一个资源一个时间段                        
                        lobj_Resource.getResSourceDayCapList();
                    }

                }

                //生成关键资源列表
                this.schData.KeyResourceList = this.schData.ResourceList.FindAll(delegate (Resource p1) { return (p1.cIsKey == "1" && p1.iKeySchSN > 0); });

                //关键资源按排产优先级排序
                this.schData.KeyResourceList.Sort(delegate (Resource p1, Resource p2) { return Comparer<int>.Default.Compare(p1.iKeySchSN, p2.iKeySchSN); });

                //取资源组对应的资源列表 bTeamResource
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
                //Messagezk.Show("排产计算出错！位置GetResourceList(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //      //2.1 t_SchProduct 已确认的生产任务单取SureVersion版本数据
            //      string lsSchProduct = string.Format(@"SELECT isnull(iSchBatch,6) as iSchBatch,iSchSdID, cVersionNo, isnull(iInterID,0) as iInterID, isnull(iSdLineID,-1) as iSdLineID, isnull(iSeqID,-1) as iSeqID,isnull(iModelID,-1) as  iModelID, 
            //              cSdOrderNo, cCustNo, cCustName, cSTCode, cBusType, cPriorityType, cStatus, isnull(cPersonCode,'') as cPersonCode, 
            //              cRequireType, isnull(iItemID,0) as iItemID, isnull(t_SchProduct.cInvCode,t_SchProduct.cWorkRouteType) as cInvCode,  cInvName,  isnull(cInvStd,'') as cInvStd, cUnitCode,  isnull(iReqQty,iSchQty) as iReqQty, 
            //              dRequireDate, dDeliveryDate, dEarliestSchDate, isnull(cSchStatus,'') as cSchStatus, isnull(cMiNo,'') as cMiNo, 
            //              isnull(iPriority,0) as iPriority , isnull(cSelected,'0') as cSelected, isnull(cWoNo,'') as cWoNo,
            //              isnull(iSchQty,0) as iPlanQty, isnull(cNeedSet,'0') as cNeedSet, isnull(iFHQuantity,0) as iFHQuantity, 
            //              isnull(iKPQuantity,0) as iKPQuantity, isnull(iSourceLineID,0) as iSourceLineID, isnull(cColor,'') as cColor, 
            //              isnull(cNote,'') cNote,cNeedSet as bSet,dBegDate,isnull(dEndDate,dRequireDate) as dEndDate,isnull(cType,'MPS') as  cType, isnull(cSchType,'0') as cSchType,isnull(DATEDIFF(day, dEarliestSchDate, dRequireDate ),0) as iDeliveryDays,'0' as cScheduled,isnull(iWorkQtyPd,0) as  iWorkQtyPd,
            //              isnull(cBatchNo,'') as cBatchNo,isnull(cWorkRouteType,'') as cWorkRouteType,isnull(iSchSN,0) as iSchSN,isnull(cGroupSN,0) as cGroupSN,isnull(cGroupQty,0) cGroupQty,isnull(cCustomize,0) cCustomize,isnull(cAttributes1,'') cAttributes1 ,isnull(cAttributes2,'') cAttributes2,isnull(cAttributes3,'') cAttributes3,
            //              isnull(cAttributes4,'') cAttributes4,isnull(cAttributes5,'') cAttributes5,isnull(cAttributes6,'') cAttributes6,isnull(cAttributes7,'') cAttributes7 ,isnull(cAttributes8,'') cAttributes8,isnull(cAttributes9,0) cAttributes9,isnull(cAttributes10,0) cAttributes10,isnull(cAttributes11,0) cAttributes11,
            //              isnull(cAttributes12,0) cAttributes12,isnull(cAttributes13,'') cAttributes13,isnull(cAttributes14,'') cAttributes14,isnull(cAttributes15,'') cAttributes15,isnull(cAttributes16,'') cAttributes16                    
            //                  FROM dbo.t_SchProduct 
            //              where 1 = 1 AND cVersionNo = '{0}' and isnull(cSelected,'0') = '1' and isnull(iSchQty,0) > 0 and isnull(cWoNo,'') = ''", schData.cVersionNo);


            //      //包含已下达生产任务单
            //      if (cSchWo == "1")
            //          lsSchProduct += @" union
            //              SELECT isnull(iSchBatch,6) as iSchBatch,iSchSdID, cVersionNo, isnull(iInterID,0) as iInterID, isnull(iSdLineID,-1) as iSdLineID, isnull(iSeqID,-1) as iSeqID,isnull(iModelID,-1) as  iModelID, 
            //              cSdOrderNo, cCustNo, cCustName, cSTCode, cBusType, cPriorityType, cStatus, isnull(cPersonCode,'') as cPersonCode, 
            //              cRequireType, isnull(iItemID,0) as iItemID, isnull(t_SchProduct.cInvCode,t_SchProduct.cWorkRouteType) as cInvCode, cInvName,  isnull(cInvStd,'') as cInvStd, cUnitCode, isnull(iReqQty,iSchQty) as iReqQty, 
            //              dRequireDate, dDeliveryDate, dEarliestSchDate, isnull(cSchStatus,'') as cSchStatus, isnull(cMiNo,'') as cMiNo, 
            //              isnull(iPriority,0) as iPriority , isnull(cSelected,'0') as cSelected, isnull(t_SchProduct.cWoNo,'') as cWoNo,
            //              isnull(iSchQty,0) as iPlanQty, isnull(cNeedSet,'0') as cNeedSet, isnull(iFHQuantity,0) as iFHQuantity, 
            //              isnull(iKPQuantity,0) as iKPQuantity, isnull(iSourceLineID,0) as iSourceLineID, isnull(cColor,'') as cColor, 
            //              isnull(cNote,'') cNote,cNeedSet as bSet,dBegDate,isnull(dEndDate,dRequireDate) as dEndDate,isnull(cType,'MPS') as  cType, isnull(cSchType,'0') as cSchType,isnull(DATEDIFF(day, dEarliestSchDate, dRequireDate ),0) as iDeliveryDays,isnull(cScheduled,'0') as cScheduled,isnull(iWorkQtyPd,0) as  iWorkQtyPd,
            //              isnull(cBatchNo,'') as cBatchNo,isnull(cWorkRouteType,'') as cWorkRouteType,isnull(iSchSN,0) as iSchSN,isnull(cGroupSN,0) as cGroupSN,isnull(cGroupQty,0) cGroupQty,isnull(cCustomize,0) cCustomize,isnull(cAttributes1,'') cAttributes1 ,isnull(cAttributes2,'') cAttributes2,isnull(cAttributes3,'') cAttributes3,
            //              isnull(cAttributes4,'') cAttributes4,isnull(cAttributes5,'') cAttributes5,isnull(cAttributes6,'') cAttributes6,isnull(cAttributes7,'') cAttributes7 ,isnull(cAttributes8,'') cAttributes8,isnull(cAttributes9,0) cAttributes9,isnull(cAttributes10,0) cAttributes10,isnull(cAttributes11,0) cAttributes11,
            //              isnull(cAttributes12,0) cAttributes12,isnull(cAttributes13,'') cAttributes13,isnull(cAttributes14,'') cAttributes14,isnull(cAttributes15,'') cAttributes15,isnull(cAttributes16,'') cAttributes16                    
            //                  FROM dbo.t_SchProduct inner join (select cWoNo from t_WorkOrder where cStatus in ('I','A','G') ) b on (t_SchProduct.cWoNo = b.cWoNo )                                                
            //              where 1 = 1 AND cVersionNo = 'SureVersion'      
            //                --and cStatus in ('I','A','G')            
            //                and iSchSdID in ( select iSchSdID from t_SchProduct WHERE  cVersionNo = 'SureVersion' AND  isnull(cSelected,'0') = '1' )  ---cColor
            //              order by cVersionNo,iPriority,dDeliveryDate";


            //       //2.2 t_SchProductRoute 注意必须这样排序，保证最低层工序优先排程 cVersionNo,iSchSdID,iLevel Desc ,cParentItemNo,cWorkItemNo,iProcessProductID
            //       string lsSchProductRoute = string.Format(@"SELECT  isnull(iSchBatch,6) as iSchBatch,cVersionNo,iSchSdID,iModelID,iProcessProductID,cWoNo,iInterID,iWoProcessID,iItemID,cInvCode,iWorkItemID,cWorkItemNo,iProcessID,iWoSeqID,cTechNo,cSeqNote,cWcNo,iNextSeqID,cPreProcessID,cPostProcessID,cPreProcessItem,cPostProcessItem,X,Y,isnull(iAutoID,-1) iAutoID,isnull(cLevelInfo,'') as cLevelInfo,
            //                  isnull(iLevel,0) as iLevel,iParentItemID,cParentItemNo,isnull(iRouteInterID,0) as iRouteInterID,iProcessSumQty,iQtyPer,iParentQty,cSourceCode,iSourceInterID,iSourceEntryID,isnull(cEasID,'') as cEasID, isnull(cParellelType,'0') as cParellelType,isnull(cParallelNo,'') as cParallelNo,isnull(iSeqRatio,100) as iSeqRatio,isnull(cKeyBrantch,'1'),cCompSeq,cIsReport,cMoveType,isnull(iMoveInterTime,0) as iMoveInterTime,isnull(iMoveInterQty,0) as iMoveInterQty,isnull(iMoveTime,0) as iMoveTime,isnull(iSeqPreTime,0) as iSeqPreTime,
            //                  isnull(iSeqPostTime,0) as iSeqPostTime,isnull(cWorkType,'0') cWorkType,isnull(iBatchQty,0) as iBatchQty ,isnull(iBatchWorkTime,0) as iBatchWorkTime,isnull(iBatchInterTime,0) as iBatchInterTime,isnull(iResPreTime,0) as iResPreTime,isnull(cResPreTimeExp,'') as cResPreTimeExp,isnull(iCapacity,1) as iCapacity,isnull(cCapacityExp,'') as cCapacityExp,isnull(iResPostTime,0) as iResPostTime,isnull(cResPostTimeExp,'') as cResPostTimeExp,isnull(iProcessPassRate,100) iProcessPassRate,isnull(iEfficiency,100) as iEfficiency,
            //   isnull(iHoursPd,8) as iHoursPd,isnull(iWorkQtyPd,1) as iWorkQtyPd,isnull(iWorkersPd,1) as iWorkersPd,isnull(iDevCountPd,1) as iDevCountPd,isnull(iLaborTime,0) as iLaborTime,isnull(iLeadTime,0) as iLeadTime,isnull(cStatus,'0') as cStatus,isnull(iPriority,0) as iPriority,isnull(iReqQty,1) as iReqQty ,isnull(iReqQty,1) as iReqQtyOld,isnull(iActQty,0) as iActQty,isnull(iRealHour,0) as  iRealHour ,dBegDate,dEndDate,isnull(dActBegDate,'1900-01-01') as dActBegDate,isnull(dActEndDate,'1900-01-01') as dActEndDate,isnull(cNote,'') as  cNote,
            //                  isnull(cDevCountPdExp,'') as cDevCountPdExp,isnull(iResPreTime,0) as iResPreTime,isnull(cResPreTimeExp,'') as cResPreTimeExp, isnull(iResPostTime,0) as iResPostTime ,isnull(cResPostTimeExp,'') as cResPostTimeExp,isnull(iCapacity,0) as iCapacity, isnull(cCapacityExp,'') as cCapacityExp,isnull(iAdvanceDate,0) as iAdvanceDate,isnull(dEarlySubItemDate,'1900-01-01') as dEarlySubItemDate
            //                  from t_SchProductRoute 
            //                  where 1 = 1 and  iSchSdID in (select iSchSdID from t_SchProduct where cVersionNo = '{0}' and isnull(cSelected,'0') = '1'  and isnull(iSchQty,0) > 0 and isnull(cWoNo,'') = '' ) 
            //                  and cVersionNo = '{0}' ", schData.cVersionNo);
            //       //包含已下达生产任务单,开工工序只取大于前1天的工序,其他都不取


            //       if (cSchWo == "1")
            //           lsSchProductRoute += string.Format(@" 
            //              union
            //                  SELECT  isnull(iSchBatch,6) as iSchBatch,cVersionNo,iSchSdID,iModelID,iProcessProductID,cWoNo,iInterID,iWoProcessID,iItemID,cInvCode,iWorkItemID,cWorkItemNo,iProcessID,iWoSeqID,cTechNo,cSeqNote,cWcNo,iNextSeqID,cPreProcessID,cPostProcessID,cPreProcessItem,cPostProcessItem,X,Y,isnull(iAutoID,-1) iAutoID,isnull(cLevelInfo,'') as cLevelInfo,
            //                  isnull(iLevel,0) as iLevel,iParentItemID,cParentItemNo,isnull(iRouteInterID,0) as iRouteInterID,iProcessSumQty,iQtyPer,iParentQty,cSourceCode,iSourceInterID,iSourceEntryID,isnull(cEasID,'') as cEasID, isnull(cParellelType,'0') as cParellelType,isnull(cParallelNo,'') as cParallelNo,isnull(iSeqRatio,100) as iSeqRatio,isnull(cKeyBrantch,'1'),cCompSeq,cIsReport,cMoveType,isnull(iMoveInterTime,0) as iMoveInterTime,isnull(iMoveInterQty,0) as iMoveInterQty,isnull(iMoveTime,0) as iMoveTime,isnull(iSeqPreTime,0) as iSeqPreTime,
            //                  isnull(iSeqPostTime,0) as iSeqPostTime,isnull(cWorkType,'0') cWorkType,isnull(iBatchQty,0) as iBatchQty ,isnull(iBatchWorkTime,0) as iBatchWorkTime,isnull(iBatchInterTime,0) as iBatchInterTime,isnull(iResPreTime,0) as iResPreTime,isnull(cResPreTimeExp,'') as cResPreTimeExp,isnull(iCapacity,1) as iCapacity,isnull(cCapacityExp,'') as cCapacityExp,isnull(iResPostTime,0) as iResPostTime,isnull(cResPostTimeExp,'') as cResPostTimeExp,isnull(iProcessPassRate,100) iProcessPassRate,isnull(iEfficiency,100) as iEfficiency,
            //   isnull(iHoursPd,8) as iHoursPd,isnull(iWorkQtyPd,1) as iWorkQtyPd,isnull(iWorkersPd,1) as iWorkersPd,isnull(iDevCountPd,1) as iDevCountPd,isnull(iLaborTime,0) as iLaborTime,isnull(iLeadTime,0) as iLeadTime,isnull(cStatus,'0') as cStatus,isnull(iPriority,0) as iPriority, case when isnull(iReqQty,1) - isnull(iActQty,0) > 0 then isnull(iReqQty,1) - isnull(iActQty,0) else 0 end  as iReqQty ,iReqQty as iReqQtyOld,isnull(iActQty,0) as iActQty,isnull(iRealHour,0) as  iRealHour ,dBegDate,dEndDate,isnull(dActBegDate,'1900-01-01') as dActBegDate,isnull(dActEndDate,'1900-01-01') as dActEndDate,isnull(cNote,'') as  cNote,
            //                  isnull(cDevCountPdExp,'') as cDevCountPdExp,isnull(iResPreTime,0) as iResPreTime,isnull(cResPreTimeExp,'') as cResPreTimeExp, isnull(iResPostTime,0) as iResPostTime ,isnull(cResPostTimeExp,'') as cResPostTimeExp,isnull(iCapacity,0) as iCapacity, isnull(cCapacityExp,'') as cCapacityExp,isnull(iAdvanceDate,0) as iAdvanceDate,isnull(dEarlySubItemDate,'1900-01-01') as dEarlySubItemDate
            //                  from t_SchProductRoute 
            //                  where 1 = 1 and  iSchSdID in (select t_SchProduct.iSchSdID from t_SchProduct inner join (select cWoNo from t_WorkOrder where cStatus in ('I','A','G') ) b on (t_SchProduct.cWoNo = b.cWoNo )  where cVersionNo = 'SureVersion'   ) 
            //                  and cVersionNo = 'SureVersion'                        
            //                  and iSchSdID in ( select iSchSdID from t_SchProduct WHERE  cVersionNo = 'SureVersion' AND  isnull(cSelected,'0') = '1' )
            //order by cVersionNo,iSchSdID,iLevel Desc ,cParentItemNo,cWorkItemNo,iProcessProductID ");




            //       //2.3 t_SchProductRouteRes  工序资源表


            //       string lsSchProductRouteRes = string.Format(@"SELECT   isnull(iSchBatch,6) as iSchBatch,cVersionNo, iSchSdID, iProcessProductID, iProcessID, iResProcessID, iResourceAbilityID, cWoNo, iItemID, cInvCode, 
            //                  iWoSeqID, cTechNo, isnull(cSeqNote,cTechNo) as cSeqNote, iResGroupNo, iResGroupPriority, cSelected, isnull(iResourceID,99999) as iResourceID, cResourceNo,isnull(cTeamResourceNo,'') as cTeamResourceNo,
            //                  cResourceName, case when (iResReqQty - isnull(iActResReqQty,0) ) > 0 then (iResReqQty - isnull(iActResReqQty,0) ) else 0 end as iResReqQty,iResReqQty as iResReqQtyOld, isnull(dResBegDate,'1900-01-01') as dResBegDate, isnull(dResEndDate,'1900-01-01') as dResEndDate, isnull(iResRationHour,0) as iResRationHour ,isnull(cViceResource1No,'') as cViceResource1No, 
            //                  isnull(cViceResource2No,'') as cViceResource2No, isnull(cViceResource3No,'') as cViceResource3No, cWorkType, isnull(iBatchQty,1) as iBatchQty, isnull(iBatchWorkTime,0) as iBatchWorkTime, 
            //                  isnull(iBatchInterTime,0) as iBatchInterTime, isnull(iResPreTime,0) as iResPreTime,isnull(iResPreTimeOld,0) as iResPreTimeOld, isnull(cResPreTimeExp,'') as cResPreTimeExp, isnull(iCapacity,0) as iCapacity , isnull(cCapacityExp,'') as cCapacityExp, isnull(iResPostTime,0) as iResPostTime, isnull(cResPostTimeExp,'') as cResPostTimeExp, isnull(iCycTime,0) as iCycTime, 
            //                  isnull(iProcessPassRate,100) as iProcessPassRate, isnull(iEfficiency,100) as iEfficiency, isnull(iHoursPd,8) as iHoursPd, isnull(iWorkQtyPd,1) as iWorkQtyPd, isnull(iWorkersPd,1) as iWorkersPd, isnull(iDevCountPd,1) as iDevCountPd, isnull(iLaborTime,0) as iLaborTime, isnull(iLeadTime,0) as iLeadTime, 
            //                  isnull(FResChaValue1ID,-1) as FResChaValue1ID, isnull(FResChaValue2ID,-1) as FResChaValue2ID, isnull(FResChaValue3ID,-1) as FResChaValue3ID , isnull(FResChaValue4ID,-1) as  FResChaValue4ID, isnull(FResChaValue5ID,-1) as FResChaValue5ID, isnull(FResChaValue6ID,-1) as FResChaValue6ID, 
            //                  isnull(FResChaValue7ID,-1) as FResChaValue7ID, isnull(FResChaValue8ID,-1) as FResChaValue8ID, isnull(FResChaValue9ID,-1) as FResChaValue9ID, isnull(FResChaValue10ID,-1) as FResChaValue10ID, isnull(FResChaValue11ID,-1) as FResChaValue11ID, isnull(FResChaValue12ID,-1) as FResChaValue12ID, 
            //                  isnull(FResChaValue1Cyc,0) as FResChaValue1Cyc, isnull(FResChaValue2Cyc,0) as FResChaValue2Cyc, isnull(FResChaValue3Cyc,0) as FResChaValue3Cyc, isnull(FResChaValue4Cyc,0) as FResChaValue4Cyc, isnull(FResChaValue5Cyc,0) as FResChaValue5Cyc, 
            //                  isnull(FResChaValue6Cyc,0) as FResChaValue6Cyc, isnull(FResChaValue7Cyc,0) as FResChaValue7Cyc, isnull(FResChaValue8Cyc,0) as FResChaValue8Cyc, isnull(FResChaValue9Cyc,0) as FResChaValue9Cyc, isnull(FResChaValue10Cyc,0) as FResChaValue10Cyc, 
            //                  isnull(FResChaValue11Cyc,0) as FResChaValue11Cyc, isnull(FResChaValue12Cyc,0) as FResChaValue12Cyc, isnull(cResourceNote,'') as cResourceNote, isnull(cDefine22,'') as cDefine22, isnull(cDefine23,'') as cDefine23, isnull(cDefine24,'0') as cDefine24, isnull(cDefine25,'') as cDefine25, isnull(cDefine26,'') as cDefine26, 
            //                  isnull(cDefine27,'') as cDefine27, isnull(cDefine28,'') as cDefine28, isnull(cDefine29,'') as cDefine29, isnull(cDefine30,'') as cDefine30, isnull(cDefine31,0) as cDefine31, isnull(cDefine32,0) as cDefine32, isnull(cDefine33,'') as cDefine33, isnull(cDefine34,99000000) as cDefine34, isnull(cDefine35,0) as cDefine35, isnull(cDefine36,'1900-01-01') as cDefine36, 
            //                  isnull(cDefine37,'1900-01-01') as cDefine37,isnull(cIsInfinityAbility,0) as cIsInfinityAbility,ISNULL(iViceResource1ID,-1) AS iViceResource1ID,ISNULL(iViceResource2ID,-1) AS iViceResource2ID,ISNULL(iViceResource3ID,-1) AS iViceResource3ID,
            //                  isnull(iActResReqQty,0) as iActResReqQty,isnull(iActResRationHour,0) as iActResRationHour,isnull(dActResBegDate,'1900-01-01') as dActResBegDate,isnull(dActResEndDate,'1900-01-01') as dActResEndDate,isnull(cLearnCurvesNo,'') as cLearnCurvesNo
            //                  FROM      t_SchProductRouteRes where 1 = 1 and iSchSdID in (select iSchSdID from t_SchProduct where cVersionNo = '{0}' and isnull(cSelected,'0') = '1'  and isnull(iSchQty,0) > 0 and isnull(cWoNo,'') = ''  ) 
            //                           and cVersionNo = '{0}'  ", schData.cVersionNo);  //and isnull(cSelected,'0') = '1' 2020-03-22



            //      //包含已下达生产任务单,开工工序只取大于前1天的工序,其他都不取


            //      if (cSchWo == "1")
            //           lsSchProductRouteRes += string.Format(@"
            //                  union

            //                  SELECT  isnull(iSchBatch,6) as iSchBatch, cVersionNo, iSchSdID, iProcessProductID, iProcessID, iResProcessID, iResourceAbilityID, cWoNo, iItemID, cInvCode, 
            //iWoSeqID, cTechNo, isnull(cSeqNote,cTechNo) as cSeqNote, iResGroupNo, iResGroupPriority, cSelected, isnull(iResourceID,99999) as iResourceID, cResourceNo, isnull(cTeamResourceNo,'') as cTeamResourceNo,
            //cResourceName, (case when iResReqQty - isnull(iActResReqQty,0) > 0 then iResReqQty - isnull(iActResReqQty,0) else 0 end ) as iResReqQty, iResReqQty as iResReqQtyOld,isnull(dResBegDate,'1900-01-01') as dResBegDate, isnull(dResEndDate,'1900-01-01') as dResEndDate, isnull(iResRationHour,0) * 60 as iResRationHour ,isnull(cViceResource1No,'') as cViceResource1No, 
            //isnull(cViceResource2No,'') as cViceResource2No, isnull(cViceResource3No,'') as cViceResource3No, cWorkType, isnull(iBatchQty,1) as iBatchQty, isnull(iBatchWorkTime,0) as iBatchWorkTime, 
            //isnull(iBatchInterTime,0) as iBatchInterTime, isnull(iResPreTime,0) * 60 as iResPreTime,isnull(iResPreTimeOld,0) as iResPreTimeOld,  isnull(cResPreTimeExp,'') as cResPreTimeExp, isnull(iCapacity,0) as iCapacity , isnull(cCapacityExp,'') as cCapacityExp, isnull(iResPostTime,0)  as iResPostTime, isnull(cResPostTimeExp,'') as cResPostTimeExp, isnull(iCycTime,0) as iCycTime, 
            //isnull(iProcessPassRate,100) as iProcessPassRate, isnull(iEfficiency,100) as iEfficiency, isnull(iHoursPd,8) as iHoursPd, isnull(iWorkQtyPd,1) as iWorkQtyPd, isnull(iWorkersPd,1) as iWorkersPd, isnull(iDevCountPd,1) as iDevCountPd, isnull(iLaborTime,0) as iLaborTime, isnull(iLeadTime,0) as iLeadTime, 
            //isnull(FResChaValue1ID,-1) as FResChaValue1ID, isnull(FResChaValue2ID,-1) as FResChaValue2ID, isnull(FResChaValue3ID,-1) as FResChaValue3ID , isnull(FResChaValue4ID,-1) as  FResChaValue4ID, isnull(FResChaValue5ID,-1) as FResChaValue5ID, isnull(FResChaValue6ID,-1) as FResChaValue6ID, 
            //isnull(FResChaValue7ID,-1) as FResChaValue7ID, isnull(FResChaValue8ID,-1) as FResChaValue8ID, isnull(FResChaValue9ID,-1) as FResChaValue9ID, isnull(FResChaValue10ID,-1) as FResChaValue10ID, isnull(FResChaValue11ID,-1) as FResChaValue11ID, isnull(FResChaValue12ID,-1) as FResChaValue12ID, 
            //isnull(FResChaValue1Cyc,0) as FResChaValue1Cyc, isnull(FResChaValue2Cyc,0) as FResChaValue2Cyc, isnull(FResChaValue3Cyc,0) as FResChaValue3Cyc, isnull(FResChaValue4Cyc,0) as FResChaValue4Cyc, isnull(FResChaValue5Cyc,0) as FResChaValue5Cyc, 
            //isnull(FResChaValue6Cyc,0) as FResChaValue6Cyc, isnull(FResChaValue7Cyc,0) as FResChaValue7Cyc, isnull(FResChaValue8Cyc,0) as FResChaValue8Cyc, isnull(FResChaValue9Cyc,0) as FResChaValue9Cyc, isnull(FResChaValue10Cyc,0) as FResChaValue10Cyc, 
            //isnull(FResChaValue11Cyc,0) as FResChaValue11Cyc, isnull(FResChaValue12Cyc,0) as FResChaValue12Cyc, isnull(cResourceNote,'') as cResourceNote, isnull(cDefine22,'') as cDefine22, isnull(cDefine23,'') as cDefine23, isnull(cDefine24,'0') as cDefine24, isnull(cDefine25,'') as cDefine25, isnull(cDefine26,'') as cDefine26, 
            //isnull(cDefine27,'') as cDefine27, isnull(cDefine28,'') as cDefine28, isnull(cDefine29,'') as cDefine29, isnull(cDefine30,'') as cDefine30, isnull(cDefine31,0) as cDefine31, isnull(cDefine32,0) as cDefine32, isnull(cDefine33,'') as cDefine33,  isnull(cDefine34,99000000)  as cDefine34, isnull(cDefine35,0) as cDefine35, isnull(cDefine36,'1900-01-01') as cDefine36, 
            //isnull(cDefine37,'1900-01-01') as cDefine37,isnull(cIsInfinityAbility,0) as cIsInfinityAbility,ISNULL(iViceResource1ID,-1) AS iViceResource1ID,ISNULL(iViceResource2ID,-1) AS iViceResource2ID,ISNULL(iViceResource3ID,-1) AS iViceResource3ID,
            //                  isnull(iActResReqQty,0) as iActResReqQty,isnull(iActResRationHour,0) as iActResRationHour,isnull(dActResBegDate,'1900-01-01') as dActResBegDate,isnull(dActResEndDate,'1900-01-01') as dActResEndDate,isnull(cLearnCurvesNo,'') as cLearnCurvesNo
            //FROM      t_SchProductRouteRes where 1 = 1 and iSchSdID in (select t_SchProduct.iSchSdID from t_SchProduct inner join (select cWoNo from t_WorkOrder where cStatus in ('I','A','G') ) b on (t_SchProduct.cWoNo = b.cWoNo )  where cVersionNo = 'SureVersion'   ) 
            //         and cVersionNo = 'SureVersion' and isnull(cSelected,'0') = '1' 
            //                           and iSchSdID in ( select iSchSdID from t_SchProduct WHERE  cVersionNo = 'SureVersion' AND isnull(cSelected,'0') = '1' )
            //   order by cVersionNo,iSchSdID,iProcessProductID,iResGroupNo, iResGroupPriority ,cResourceNo ");


            //2.4 lsSchProductRouteItem 注意必须这样排序，保证最低层工序优先排程 cVersionNo,iSchSdID,iLevel Desc ,cParentItemNo,cWorkItemNo,iProcessProductID
            string lsSchProductRouteItem = string.Format(@" SELECT    isnull(iSchBatch,1) as iSchBatch, cVersionNo, iSchSdID, iProcessProductID, iInterID, iEntryID, cWoNo, cInvCode, cInvCodeFull, isnull(iBomLevel,0) as iBomLevel, cLevelInfo, cLevelPath,  cSubInvCode, 
                            cSubInvCodeFull, iSeqID, cUtterType, cSubRelate, isnull(iQtyPer,0) as iQtyPer, isnull(iParentQty,0) as iParentQty, isnull(iSubQty,0) as iSubQty, isnull(iScrapt,0) as iScrapt, iNormalQty, iRetPercent, iReqQty, dReqDate, iProQty, iScrapQty, 
                            isnull(iNormalScrapQty,0) as iNormalScrapQty, isnull(iKeepQty,0) as iKeepQty, isnull(iPlanQty,0) as iPlanQty , cWhNo, cPacNo, iRetOffsetLt, cNote, isnull(cGetItemType,'0') as cGetItemType,isnull(bself,'0') as bself,isnull(dForeInDate,getdate()) as dForeInDate
                        FROM         dbo.t_SchProductRouteItem  with (nolock)
                        where 1 = 1 and  iSchSdID in (select iSchSdID from t_SchProduct where cVersionNo = '{0}' and isnull(cSelected,'0') = '1'  and isnull(iSchQty,0) > 0 and isnull(cWoNo,'') = '' ) 
                        and cVersionNo = '{0}' ", schData.cVersionNo);
            //包含已下达生产任务单,开工工序只取大于前1天的工序,其他都不取

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



            //2.5 t_Item 已确认的生产任务单取SureVersion版本数据
            //             string lsItem = string.Format(@"SELECT  a.cInvCode,a. cInvName,a. cInvStd,a. iItemID,a. cEnglishName,a. cItemClsNo,a. cVenCode,a. cReplaceItem,a. bSale,a. bPurchase,a. bSelf,a. bProxyForeign,a. bComsume,a. bService,a. 
            //                      bEquipment,a. bFixExch,a. iTaxRate,a. cDefWhNo,a. cPosition,a. bCommission,a. bModelCompleted,a. iGroupType,a. cGroupCode,a. cComUnitCode,a. cAssComUnitCode,a. 
            //                      cSAComUnitCode,a. cPUComUnitCode,a. cSTComUnitCode,a. cEnterprise,a. cAddress,a. cFile,a. cLabel,a. cCheckOut,a. cLicence,a. cLevel,a. cLinear,a. cWGroupCode,a. fGrossWeight,a. 
            //                      fNetWeight,a. cVGroupCode,a. fVolume,a. flength,a. fWidth,a. fHeight,a. cPackingType,a. fDiameter,a. fMinDiameter,a. fQtyPerBox,a. fPicQtyPer,a. fWeightPerM,a. iSafeStock,a. iTopLot,a. 
            //                      iLowLot,a. iIncLot,a. iAvgLot,a. cLeadTimeType,a. iAvgLeadTime,a. cValueType,a. iInvSCost,a. iInvSPrice,a. iInvNCost,a. iInvRCost,a. iInvHSCost,a. iInvLSCost,a. iExpSaleRate,a. bInvType,a. 
            //                      iInvMPCost,a. iInvSaleCost,a. iInvSCost1,a. iInvSCost2,a. iInvSCost3,a. cPriceGroup,a. cQuality,a. bBarCode,a. cBarCode,a. bSerial,a. cColor,a. cTemperature,a. cMaterial,a. 
            //                      cPlanMethod,a. cInvDepCode,a. cInvPersonCode,a. cPurPersonCode,a. cSize1,a. cSize2,a. cSize3,a. cSize4,a. cSize5,a. cSize6,a. cSize7,a. cSize8,a. cSize9,a. cSize10,a. cSize11,a. cSize12,a. cSize13,a. cSize14,a. cSize15,a. cSize16,a.
            //                      cItemType,a. cProductType,a. cUseStatus,a. iYieldage,a. iShrinkage,a. cWcNo,a. iProSec,a. fOrderUpLimit,a. fOutExcess,a. fInExcess,a. cPrvNo,a. bInvQuality,a. iMassDate,a. iWarnDays,a. cCreatePerson,a. 
            //                      cModifyPerson,a. dModifyDate,a. bInvBatch,a. bCutMantissa,a. cInvABC,a. fExpensesExch,a. iOverStock,a. iTopSum,a. iLowSum,a. iAdvanceDate,a. bMps,a. bSet,a. bSchedule,a. cWorkRouteType,a. cBarCodeType,a. fVagQuantity,a. 
            //                      cWhKeeper,a. cWhMan ,a.bSmall,a.iBarCodeQty,a.iBarMaxID,a.cInjectItemType,a.cMoldNo,a.cSubMoldNo,a.cMoldPosition,a.iMoldSubQty,a.cModifyNote,a.iMadeRate,a.dLastMadeDate,a.iPriority,a.
            //                      cRouteCode,a.cTechNo,a.cPlanMode,a.cKeyResourceNo,a.fQuota,a.cRWhMan,a.bReportItem,a.cReportWhNo,a.fSettleCost,a.cSettleAdress,a.fManuCost,a.iDensity,a.cMaterialType,a.cMatCode,a.iMatCost,a.iLabCost,a.iManuCost,a.
            //                      iOtherCost,a.iLevelMatCost,a.iLevelLabCost,a.iLevelManuCost,a.iLevelOtherCost,a.bCost,a.cBomCode,a.bUsePriceType
            //                    FROM t_Item a 
            //                    where a.cInvCode in (select distinct cInvCode from dbo.t_SchProduct b )                                            
            //                        and isnull(a.bSchedule,1) = '1'  ", schData.cVersionNo);

            string lsItem = string.Format(@"
                     select  a.iItemID, a.cInvCode, a.cInvName,a.cInvStd,a.cItemClsNo, isnull(a.cVenCode,'') as cVenCode, isnull(a.bSale,'0') as bSale,isnull(a.bPurchase,'0') as bPurchase,isnull(a.bSelf,'1') as bSelf,isnull(a.bProxyForeign,'0') as bProxyForeign,isnull(a.cComUnitCode,'') as cComUnitCode, isnull(a.cWcNo,'') as cWcNo,isnull(a.iProSec,'') as iProSec,isnull(a.iPriority,'') as iPriority, 
                        isnull(a.iSafeStock,'') as iSafeStock, isnull(a.iTopLot,'') as  iTopLot,isnull(a.iLowLot,'') as iLowLot,isnull(a.iIncLot,'') as iIncLot,isnull(a.iAvgLot,'') as  iAvgLot,isnull(a.cLeadTimeType,'') as cLeadTimeType,isnull(a.iAvgLeadTime,'') as iAvgLeadTime ,isnull(a.iAdvanceDate,'') as iAdvanceDate , isnull(a.cRouteCode,'') as cRouteCode , isnull(a.cPlanMode,'') as cPlanMode, 
                        isnull(a.cWorkRouteType,'') as cWorkRouteType,isnull(a.cTechNo,'') as cTechNo,isnull(a.cKeyResourceNo,'') as cKeyResourceNo,isnull(a.cInjectItemType,'') as cInjectItemType,isnull(a.cMoldNo,'') as cMoldNo,isnull(a.cSubMoldNo,'') as cSubMoldNo,isnull(a.cMoldPosition,'') as cMoldPosition,isnull(a.iMoldSubQty,0) as iMoldSubQty,isnull(a.iMoldCount,0) as iMoldCount,
                        isnull(a.cMaterial,'') as cMaterial,isnull(a.cColor,'') as cColor,isnull(a.fVolume,0) as fVolume,isnull(a.flength,0) as flength,isnull(a.fWidth,0) as fWidth,isnull(a.fHeight,0) as fHeight,isnull(a.fNetWeight,0) as fNetWeight, isnull(a.iItemDifficulty,1 ) as iItemDifficulty , isnull(a.cSize1,'') as cSize1,isnull(a.cSize2,'') as cSize2,isnull(a.cSize3,'') as cSize3,isnull(a.cSize4,'') as cSize4,isnull(a.cSize5,'') as cSize5,isnull(a.cSize6,'') as cSize6 ,
                        isnull(a.cSize7,'') as cSize7,isnull(a.cSize8,'') as cSize8,isnull(a.cSize9,'') as cSize9,isnull(a.cSize10,'') as cSize10,isnull(a.cSize11,0) as cSize11,isnull(a.cSize12,0) as cSize12,isnull(a.cSize13,0) as cSize13,isnull(a.cSize14,0) as cSize14,isnull(a.cSize15,'') as cSize15,isnull(a.cSize16,'') as cSize16
                    FROM t_Item a  with (nolock)
                        where a.cInvCode in (select distinct cWorkItemNo from t_SchProductRoute )", schData.cVersionNo);  //and isnull(a.bSchedule,1) = '1' 

            //2.6 t_WorkCenter 
            string lsWorkCenter = string.Format(@" select * from t_WorkCenter ");


            //2.7 t_Department
            string lsDepartment = string.Format(@" select * from t_Department ");

            //2.8 t_Person
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

            //2.9 t_team
            string lsteam = string.Format(@" select * from t_team ");

            //2.10 t_TechInfo
            string lsTechInfo = string.Format(@" 
                    SELECT   iInterID, cTechNo, cTechName, isnull(cResClsNo,'') as cResClsNo,isnull(cWcNo,'') as cWcNo, isnull(cDeptNo,'') as cDeptNo,isnull(cResourceNo,'') as cResourceNo, isnull(cTechReq,'') as cTechReq, isnull(cNote,'') as cNote, isnull(cTechDefine1,'') as cTechDefine1, 
                                    isnull(cTechDefine2,'') as  cTechDefine2, isnull(cTechDefine3,'') as cTechDefine3,isnull(cTechDefine4,'') as  cTechDefine4,  isnull(cTechDefine5,'') as cTechDefine5, isnull(cTechDefine6,'') as  cTechDefine6, isnull(cTechDefine7,'') as cTechDefine7, isnull(cTechDefine8,'') as cTechDefine8, isnull(cTechDefine9,'') as cTechDefine9, 
                                    isnull(cTechDefine10,'') as cTechDefine10, isnull(cTechDefine11,0) as cTechDefine11, isnull(cTechDefine12,0) as cTechDefine12, isnull(cTechDefine13,0) as cTechDefine13, isnull(cTechDefine14,0) as cTechDefine14, isnull(cTechDefine15,'') as cTechDefine15,isnull(cTechDefine16,'') as  cTechDefine16, 
                                    isnull(cFormula,'') as cFormula, isnull(cFormula2,'') as cFormula2, isnull(iSeqPretime,0) as iSeqPretime, isnull(iSeqPostTime,0) as iSeqPostTime, isnull(cAttributeValue1,'') as  cAttributeValue1, 
                                    isnull(cAttributeValue2,'') as cAttributeValue2, isnull(cAttributeValue3,'') as cAttributeValue3, isnull(cAttributeValue4,'') as cAttributeValue4, isnull(cAttributeValue5,'') as cAttributeValue5, isnull(cAttributeValue6,'') as cAttributeValue6, isnull(cAttributeValue7,'') as cAttributeValue7, isnull(cAttributeValue8,'') as cAttributeValue8, 
                                    isnull(cAttributeValue9,'') as cAttributeValue9, isnull(cAttributeValue10,'') as cAttributeValue10, isnull(iTechValue,0) as iTechValue, isnull(iOrder,0) as iOrder, isnull(iTechDifficulty,1) as iTechDifficulty,isnull(iSeqPretime,0) as iSeqPretime,isnull(iSeqPostTime,0) as iSeqPostTime
                    FROM      dbo.t_TechInfo  with (nolock)
                        ");

            //2.11 t_TechInfo
            string lsTechLearnCurves = string.Format(@" SELECT     iInterID, cLearnCurvesNo, cLearnCurvesName, cTechNo, isnull(iDayDis1,0) as iDayDis1, isnull(iDayDis2,0) as iDayDis2, isnull(iDayDis3,0) as iDayDis3, isnull(iDayDis4,0) as iDayDis4, isnull(iDayDis5,0) as iDayDis5,
                      isnull(iDayDis6,0) as iDayDis6, isnull(iDayDis7,0) as iDayDis7, isnull(iDayDis8,0) as iDayDis8,  isnull(iDayDis9,0) as iDayDis9,  isnull(iDayDis10,0) as iDayDis10, 
                      isnull(iDayDis11,0) as iDayDis11, isnull(iDayDis12,0) as iDayDis12, isnull(iDayDis13,0) as iDayDis13,  isnull(iDayDis14,0) as iDayDis14,  isnull(iDayDis15,0) as iDayDis15, 
                      isnull(iDayDis16,0) as iDayDis16, isnull(iDayDis17,0) as iDayDis17, isnull(iDayDis18,0) as iDayDis18, isnull(iDayDis19,0) as iDayDis19, isnull(iDayDis20,0) as iDayDis20, 
                      isnull(iDayDis21,0) as iDayDis21, isnull(iDayDis22,0) as iDayDis22, isnull(iDayDis23,0) as iDayDis23, isnull(iDayDis24,0) as iDayDis24, 
                      isnull(iDayDis25,0) as iDayDis25, isnull(iDayDis26,0) as iDayDis26, isnull(iDayDis27,0) as iDayDis27, isnull(iDayDis28,0) as iDayDis28, 
                      isnull(iDayDis29,0) as iDayDis29, isnull(iDayDis30,0) as iDayDis30, isnull(iDayDis31,0) as iDayDis31 , isnull(iDiffCoe,0) as iDiffCoe, isnull(iCapacity,0) as iCapacity, isnull(iResPreTime,0) as iResPreTime, cNote, cDefine22, cDefine23, cDefine24, 
                      cDefine25, cDefine26
                        FROM         dbo.t_TechLearnCurves   with (nolock) ");

            //2.12 t_TechInfo
            string lsResTechScheduSN = string.Format(@" select * from t_ResTechScheduSN ");

            #endregion
            Console.WriteLine("sql准备完毕");
            try
            {
                //2.10 t_TechInfo
                Console.WriteLine("2.10 t_TechInfo");
                schData.dtTechInfo = SqlPro.GetDataTable(lsTechInfo, null);//APSCommon.SqlPro.GetDataTable(lsTechInfo, "t_TechInfo");
                //按加工物料循环，加工物料对象 2019-03-09
                string cTechNo;
                TechInfo lobj_TechInfo;
                foreach (DataRow drTechInfo in this.schData.dtTechInfo.Rows)
                {
                    cTechNo = drTechInfo["cTechNo"].ToString();

                    lobj_TechInfo = new Algorithm.TechInfo(cTechNo, this.schData);
                    this.schData.TechInfoList.Add(lobj_TechInfo);
                }
                //2.0 t_Item 已确认的生产任务单取SureVersion版本数据
                Console.WriteLine("2.0 t_Item 已确认的生产任务单取SureVersion版本数据");
                schData.dtItem = SqlPro.GetDataTable(lsItem, null);//APSCommon.SqlPro.GetDataTable(lsItem, "t_Item");


                //按加工物料循环，加工物料对象 2019-03-09
                string cInvCode;
                Item lobj_Item;

                foreach (DataRow drItem in this.schData.dtItem.Rows)
                {
                    cInvCode = drItem["cInvCode"].ToString();

                    lobj_Item = new Algorithm.Item(cInvCode, this.schData);
                    this.schData.ItemList.Add(lobj_Item);

                }

                string lstg_Sql = "";
                //SqlParameter[] lsp_SqlParameters;
                Console.WriteLine("2.1 填充SchProductList");
                #region//2.1 填充SchProductList
                //2.1.2 填充SchProductRouteItem,加工物料表
                //if (SchParam.cSchType == "2" || SchParam.cSchType == "3")  // //排程方式  0 ---正常排产, 1--资源调度优化排产2020-08-25 ， 2--按工单优先级调度优化排产（正式版本）3 --按资源调度优化排产 )
                {
                    Console.WriteLine("2.1 填充SchProductList");
                    //schData.dtSchProduct = APSCommon.SqlPro.GetDataTable(lsSchProduct, "t_SchProduct");
                    lstg_Sql = string.Format(@"EXECUTE P_GetSchProductWorkItem '{0}','{1}','{2}','{3}','{4}','{5}'", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo); // @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName ";
                    //lsp_SqlParameters = new SqlParameter[6]{
                    //                                        Common.GenerateSqlParameter("@cVersionNo", SqlDbType.VarChar, schData.cVersionNo),
                    //                                        Common.GenerateSqlParameter("@dBeginDate", SqlDbType.DateTime, schData.dtStart.AddDays(-20)), //取提前20天的工作日历，以便上次已下达的生产任务，生成工作时间段。
                    //                                        Common.GenerateSqlParameter("@dEndDate", SqlDbType.DateTime, schData.dtEnd),
                    //                                        Common.GenerateSqlParameter("@cSchType", SqlDbType.VarChar, SchParam.cSchType),
                    //                                        Common.GenerateSqlParameter("@cTechSchType", SqlDbType.VarChar, SchParam.cTechSchType),
                    //                                        Common.GenerateSqlParameter("@cHostName", SqlDbType.VarChar, Global.User)
                    //                                       };
                    //1、调用过程P_GetResWorkTime生成所有资源的工作日历
                    //schData.dtSchProductWorkItem = APSCommon.SqlPro.GetDataTable(lstg_Sql, lsp_SqlParameters);
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
                        //lSchProductWorkItem.iModelID = (int)dr["iModelID"];
                        lSchProductWorkItem.cCustNo = dr["cCustNo"].ToString();
                        //lSchProductWorkItem.cCustName = dr["cCustName"].ToString();
                        //lSchProductWorkItem.cSTCode = dr["cSTCode"].ToString();
                        // lSchProductWorkItem.cBusType = dr["cBusType"].ToString();
                        lSchProductWorkItem.cPriorityType = (int)dr["cPriorityType"];
                        lSchProductWorkItem.cStatus = dr["cStatus"].ToString();
                        // lSchProductWorkItem.cRequireType = dr["cRequireType"].ToString();
                        lSchProductWorkItem.iItemID = -1; //(int)dr["iItemID"];  //无用
                        lSchProductWorkItem.cInvCode = dr["cInvCode"].ToString().Trim();
                        lSchProductWorkItem.cInvName = dr["cInvName"].ToString();
                        lSchProductWorkItem.cInvStd = dr["cInvStd"].ToString();
                        //lSchProductWorkItem.cUnitCode = dr["cUnitCode"].ToString();
                        lSchProductWorkItem.iReqQty = Convert.ToDouble(dr["iReqQty"]);
                        lSchProductWorkItem.dBegDate = (DateTime)dr["dBegDate"];
                        lSchProductWorkItem.dEndDate = (DateTime)dr["dEndDate"];

                        //调试
                        if (lSchProductWorkItem.iSchSdID == SchParam.iSchSdID)
                        {
                            int m;
                        }

                        lSchProductWorkItem.dCanBegDate = dr["dCanBegDate"] == DBNull.Value ? DateTime.Today : (DateTime)dr["dCanBegDate"];
                        lSchProductWorkItem.dCanEndDate = dr["dCanEndDate"] == DBNull.Value ? (DateTime)dr["dEndDate"] : (DateTime)dr["dCanEndDate"];
                        //lSchProductWorkItem.cSchStatus = dr["cSchStatus"].ToString();

                        lSchProductWorkItem.cMiNo = dr["cMiNo"].ToString();
                        lSchProductWorkItem.iPriority = Convert.ToDouble(dr["iPriority"]);
                        //lSchProductWorkItem.cSelected = dr["cSelected"].ToString();
                        lSchProductWorkItem.cWoNo = dr["cWoNo"].ToString();
                        //lSchProductWorkItem.iPlanQty = Convert.ToDouble(dr["iPlanQty"]);
                        //lSchProductWorkItem.cNeedSet = dr["cNeedSet"].ToString();
                        //lSchProductWorkItem.iFHQuantity = Convert.ToDouble(dr["iFHQuantity"]);
                        //lSchProductWorkItem.iKPQuantity = Convert.ToDouble(dr["iKPQuantity"]);
                        //lSchProductWorkItem.iSourceLineID = (int)dr["iSourceLineID"];
                        lSchProductWorkItem.cColor = dr["cColor"].ToString();
                        lSchProductWorkItem.cNote = dr["cNote"].ToString();
                        //lSchProductWorkItem.bSet = dr["bSet"].ToString();//APSCommon.SqlPro.GetSqlDataInt("select isnull(bSet,0) as bSet  from t_Item where cInvCode = '" + dr["cInvCode"].ToString() + "'");
                        lSchProductWorkItem.cType = dr["cType"].ToString();

                        lSchProductWorkItem.cSchType = dr["cSchType"].ToString();    //2016-10-17 签入
                        lSchProductWorkItem.iSchPriority = Convert.ToDouble(dr["iPriority"]);    //2016-12-07 签入
                                                                                                 //排产批次
                        lSchProductWorkItem.iSchBatch = (int)dr["iSchBatch"];

                        //lSchProductWorkItem.iDeliveryDays = (int)dr["iDeliveryDays"];

                        //lSchProductWorkItem.cScheduled = dr["cScheduled"].ToString();

                        lSchProductWorkItem.iWorkQtyPd = Convert.ToDouble(dr["iWorkQtyPd"]);

                        //Convert.ToDouble(dr["iWorkQtyPd"]);

                        lSchProductWorkItem.cBatchNo = dr["cBatchNo"].ToString();            //托盘号，不为空时，按托盘排产，同一托盘物料工艺路线类型一样，同一工序必须选择同一资源排产2020-03-22。
                        lSchProductWorkItem.iSchSN = Convert.ToDouble(dr["iSchSN"]);         //排产座次
                        //lSchProductWorkItem.cGroupSN = Convert.ToDouble(dr["cGroupSN"]);    //分组号       
                        //lSchProductWorkItem.cGroupQty = Convert.ToDouble(dr["cGroupQty"]);  //分组数量
                        //lSchProductWorkItem.cCustomize = dr["cCustomize"].ToString();        //是否定制，自动生成工艺路线的
                        lSchProductWorkItem.cWorkRouteType = dr["cWorkRouteType"].ToString();   //工艺路线类型

                        lSchProductWorkItem.cSchSNType = dr["cSchSNType"].ToString();           //座次编号
                                                                                                //lSchProductWorkItem.cSchSNType = dr["cSchSNType"].ToString();           //座次编号
                                                                                                //lSchProductWorkItem.cSchSNType = dr["cSchSNType"].ToString();           //座次编号

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



                        //if (lSchProduct.cVersionNo == "SureVersion") 
                        //    lSchProduct.iSchBatch = 1;
                        //else
                        //    lSchProduct.iSchBatch = 6;

                        //传入资源列表
                        //lSchProduct.ResourceList = schData.ResourceList;

                        lSchProductWorkItem.schData = this.schData;
                        schData.SchProductWorkItemList.Add(lSchProductWorkItem);

                    }

                }
                //else  //正常排产调用       //排程方式  0 ---正常排产, 1--资源调度优化排产2020-08-25 ， 2--按工单优先级调度优化排产（正式版本）

                {
                    //schData.dtSchProduct = APSCommon.SqlPro.GetDataTable(lsSchProduct, "t_SchProduct");

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
                                                                                         //排产批次
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



                        //if (lSchProduct.cVersionNo == "SureVersion") 
                        //    lSchProduct.iSchBatch = 1;
                        //else
                        //    lSchProduct.iSchBatch = 6;

                        //传入资源列表
                        //lSchProduct.ResourceList = schData.ResourceList;

                        lSchProduct.schData = this.schData;
                        schData.SchProductList.Add(lSchProduct);

                    }
                }
                #endregion


                Console.WriteLine("2.2 填充SchProductRouteList 改用存储过程取数，提供速度");
                #region//2.2 填充SchProductRouteList 改用存储过程取数，提供速度
                //schData.dtSchProductRoute = APSCommon.SqlPro.GetDataTable(lsSchProductRoute, "t_SchProductRoute");
                lstg_Sql = string.Format(@"EXECUTE P_GetSchDataProductRoute '{0}','{1}','{2}','{3}','{4}','{5}' ", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo); // @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName ";
                schData.dtSchProductRoute = SqlPro.GetDataTable(lstg_Sql, null);

                //lstg_Sql = @"EXECUTE P_GetSchDataProductRoute @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName";
                //lsp_SqlParameters = new SqlParameter[6]{
                //                                        Common.GenerateSqlParameter("@cVersionNo", SqlDbType.VarChar, schData.cVersionNo),
                //                                        Common.GenerateSqlParameter("@dBeginDate", SqlDbType.DateTime, schData.dtStart.AddDays(-20)), //取提前20天的工作日历，以便上次已下达的生产任务，生成工作时间段。
                //                                        Common.GenerateSqlParameter("@dEndDate", SqlDbType.DateTime, schData.dtEnd),
                //                                        Common.GenerateSqlParameter("@cSchType", SqlDbType.VarChar, SchParam.cSchType),
                //                                        Common.GenerateSqlParameter("@cTechSchType", SqlDbType.VarChar, SchParam.cTechSchType),
                //                                        Common.GenerateSqlParameter("@cHostName", SqlDbType.VarChar, Global.User)
                //                                    };
                ////1、调用过程P_GetResWorkTime生成所有资源的工作日历
                //schData.dtSchProductRoute = APSCommon.SqlPro.GetDataTable(lstg_Sql, lsp_SqlParameters);


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
                    //加工物料对象   2019-03-11
                    lSchProductRoute.item = schData.ItemList.Find(delegate (Algorithm.Item p) { return p.cInvCode == lSchProductRoute.cWorkItemNo; });



                    lSchProductRoute.iProcessID = (int)dr["iProcessID"];
                    lSchProductRoute.iWoSeqID = (int)dr["iWoSeqID"];
                    lSchProductRoute.cTechNo = dr["cTechNo"].ToString();
                    //工艺信息对象   2019-03-11
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

                    //lSchProductRoute.iResPreTime = Convert.ToInt32(float.Parse(dr["iResPreTime"].ToString())) * 60;  //工序前准备时间

                    //lSchProductRoute.cResPreTimeExp = dr["cResPreTimeExp"].ToString();                          //工序前准备时间表达式

                    //lSchProductRoute.iResPostTime = Convert.ToInt32(float.Parse(dr["iResPostTime"].ToString())) * 60;  //工序前准备时间

                    //lSchProductRoute.cResPostTimeExp = dr["cResPostTimeExp"].ToString();                          //工序前准备时间表达式

                    lSchProductRoute.iCapacity = Convert.ToDecimal(float.Parse(dr["iCapacity"].ToString()));
                    lSchProductRoute.cCapacityExp = dr["cCapacityExp"].ToString();

                    //2020-06-02
                    lSchProductRoute.iAdvanceDate = Convert.ToDecimal(float.Parse(dr["iAdvanceDate"].ToString()));    //BOM本层材料最长采购周期(按订单生产物料),排程时考虑
                                                                                                                      //       lSchProductRoute.dEarlySubItemDate = (DateTime)dr["dEarlySubItemDate"];                           //材料最晚到料日期 


                    //lSchProductRoute.cDefine22 = dr["cDefine22"].ToString();
                    //lSchProductRoute.cDefine23 = dr["cDefine23"].ToString();
                    //lSchProductRoute.cDefine24 = dr["cDefine24"].ToString();
                    //lSchProductRoute.cDefine25 = dr["cDefine25"].ToString();
                    //lSchProductRoute.cDefine26 = dr["cDefine26"].ToString();
                    //lSchProductRoute.cDefine27 = dr["cDefine27"].ToString();
                    //lSchProductRoute.cDefine28 = dr["cDefine28"].ToString();
                    //lSchProductRoute.cDefine29 = dr["cDefine29"].ToString();
                    //lSchProductRoute.cDefine30 = dr["cDefine30"].ToString();
                    //lSchProductRoute.cDefine31 = dr["cDefine31"].ToString();
                    //lSchProductRoute.cDefine32 = dr["cDefine32"].ToString();
                    //lSchProductRoute.cDefine33 = dr["cDefine33"].ToString();
                    //lSchProductRoute.cDefine34 = Convert.ToDouble(dr["cDefine34"]);
                    //lSchProductRoute.cDefine35 = Convert.ToDouble(dr["cDefine35"]);
                    //lSchProductRoute.cDefine36 = dr["cDefine36"].ToString();
                    //lSchProductRoute.cDefine37 = dr["cDefine37"].ToString();

                    //排产批次
                    lSchProductRoute.iSchBatch = (int)dr["iSchBatch"];
                    //if (lSchProductRoute.cVersionNo == "SureVersion")
                    //    lSchProductRoute.iSchBatch = 1;
                    //else
                    //    lSchProductRoute.iSchBatch = 6;

                    //传入资源列表b
                    //lSchProductRoute.ResourceList = schData.ResourceList;

                    //传入工序列表
                    //lSchProductRoute.SchProductRouteList = schData.SchProductRouteList;

                    lSchProductRoute.schData = this.schData;

                    schData.SchProductRouteList.Add(lSchProductRoute);

                }
                #endregion

                Console.WriteLine("2.3 填充TaskTimeRangeList 暂时不需要，每次排产重新生成时间段");
                #region//2.3 填充TaskTimeRangeList 暂时不需要，每次排产重新生成时间段

                //schData.dtSchProductRouteResTime = APSCommon.SqlPro.GetDataTable(lsSchProductRouteResTime, "t_SchProductRouteResTime");
                //foreach (DataRow dr in schData.dtSchProductRouteResTime.Rows)
                //{
                //    TaskTimeRange lTaskTimeRange = new TaskTimeRange();

                //    lTaskTimeRange.iSchSdID = (int)dr["iSchSdID"];
                //    lTaskTimeRange.cVersionNo = dr["cVersionNo"].ToString();
                //    lTaskTimeRange.iProcessProductID = (int)dr["iProcessProductID"];
                //    //lTaskTimeRange.iProcessID = (int)dr["iProcessID"];
                //    lTaskTimeRange.iResProcessID = (int)dr["iResProcessID"];  //以iResourceAbilityID为准
                //    //lTaskTimeRange.iResourceAbilityID = (int)dr["iResourceAbilityID"];
                //    lTaskTimeRange.cWoNo = dr["cWoNo"].ToString();
                //    //lTaskTimeRange.iItemID = (int)dr["iItemID"];

                //    lTaskTimeRange.CResourceNo = dr["cResourceNo"].ToString();
                //    lTaskTimeRange.iResReqQty = Convert.ToDouble(dr["iResReqQty"]);
                //    lTaskTimeRange.DBegTime = Convert.ToDateTime(dr["dResBegDate"]);
                //    lTaskTimeRange.DEndTime = Convert.ToDateTime(dr["dResEndDate"]);
                //    lTaskTimeRange.AllottedTime = Convert.ToInt32(Convert.ToDouble(dr["iResRationHour"]));
                //    lTaskTimeRange.iResRationHour = Convert.ToInt32(Convert.ToDouble(dr["iResRationHour"]));

                //    //传入资源列表
                //    //lTaskTimeRange.ResourceList = ResourceList;

                //    lTaskTimeRange.schData = this.schData;
                //    lTaskTimeRange.resource = this.schData.ResourceList.Find(delegate(Resource p) { return p.cResourceNo == lTaskTimeRange.CResourceNo; });
                //    //schData.TaskTimeRangeList.Add(lTaskTimeRange);

                //}
                #endregion

                Console.WriteLine("2.4 填充SchProductRouteResList");
                #region//2.4 填充SchProductRouteResList

                lstg_Sql = string.Format(@"EXECUTE P_GetSchDataProductRouteRes '{0}','{1}','{2}','{3}','{4}','{5}' ", schData.cVersionNo, schData.dtStart.AddDays(-20), schData.dtEnd, SchParam.cSchType, SchParam.cTechSchType, this.schData.cCalculateNo); // @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName ";
                schData.dtSchProductRouteRes = SqlPro.GetDataTable(lstg_Sql, null);

                ////schData.dtSchProductRouteRes = APSCommon.SqlPro.GetDataTable(lsSchProductRouteRes, "t_SchProductRouteRes");
                //lstg_Sql = @"EXECUTE P_GetSchDataProductRouteRes @cVersionNo,@dBeginDate,@dEndDate,@cSchType,@cTechSchType,@cHostName  ";
                //lsp_SqlParameters = new SqlParameter[6]{
                //                                                        Common.GenerateSqlParameter("@cVersionNo", SqlDbType.VarChar, schData.cVersionNo),
                //                                                        Common.GenerateSqlParameter("@dBeginDate", SqlDbType.DateTime, schData.dtStart.AddDays(-20)), //取提前20天的工作日历，以便上次已下达的生产任务，生成工作时间段。
                //                                                        Common.GenerateSqlParameter("@dEndDate", SqlDbType.DateTime, schData.dtEnd),
                //                                                        Common.GenerateSqlParameter("@cSchType", SqlDbType.VarChar, SchParam.cSchType),
                //                                                        Common.GenerateSqlParameter("@cTechSchType", SqlDbType.VarChar, SchParam.cTechSchType),
                //                                                        Common.GenerateSqlParameter("@cHostName", SqlDbType.VarChar, Global.User)
                //                                                    };
                ////1、调用过程P_GetResWorkTime生成所有资源的工作日历
                //schData.dtSchProductRouteRes = APSCommon.SqlPro.GetDataTable(lstg_Sql, lsp_SqlParameters);

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

                    //单件工时
                    if (GetParamValue("HourMinSecond") == "1") //如果是显示小时

                    {
                        lSchProductRouteRes.iResRationHour = Convert.ToDouble(dr["iResRationHour"]) * 60;
                    }
                    else
                    {
                        lSchProductRouteRes.iResRationHour = Convert.ToDouble(dr["iResRationHour"]);
                    }

                    //lSchProductRouteRes.iResRationHour = Convert.ToDouble(dr["iResRationHour"]);

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
                    //单件工时
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

                    //lSchProductRouteRes.iCapacity = Convert.ToDouble(dr["iCapacity"]);
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

                    //总工时


                    if (GetParamValue("HourMinSecond") == "0") //如果是分钟

                    {
                        lSchProductRouteRes.iLaborTime = Convert.ToDouble(dr["iLaborTime"]);
                    }
                    else if (GetParamValue("HourMinSecond") == "1") //如果是小时

                    {
                        lSchProductRouteRes.iLaborTime = Convert.ToDouble(dr["iLaborTime"]) * 60;
                    }

                    //lSchProductRouteRes.iLaborTime = Convert.ToDouble(dr["iLaborTime"]);
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

                    //改为只支持6个工艺特征
                    lSchProductRouteRes.FResChaValue1ID = dr["FResChaValue1ID"].ToString();
                    lSchProductRouteRes.FResChaValue2ID = dr["FResChaValue2ID"].ToString();
                    lSchProductRouteRes.FResChaValue3ID = dr["FResChaValue3ID"].ToString();
                    lSchProductRouteRes.FResChaValue4ID = dr["FResChaValue4ID"].ToString();
                    lSchProductRouteRes.FResChaValue5ID = dr["FResChaValue5ID"].ToString();
                    lSchProductRouteRes.FResChaValue6ID = dr["FResChaValue6ID"].ToString();
                    //lSchProductRouteRes.FResChaValue7ID = dr["FResChaValue7ID"].ToString();
                    //lSchProductRouteRes.FResChaValue8ID = dr["FResChaValue8ID"].ToString();
                    //lSchProductRouteRes.FResChaValue9ID = dr["FResChaValue9ID"].ToString();
                    //lSchProductRouteRes.FResChaValue10ID = dr["FResChaValue10ID"].ToString();
                    //lSchProductRouteRes.FResChaValue11ID = dr["FResChaValue11ID"].ToString();
                    //lSchProductRouteRes.FResChaValue12ID = dr["FResChaValue12ID"].ToString();

                    //从资源列表中找到当前资源，给该资源排此任务


                    lSchProductRouteRes.resource = schData.ResourceList.Find(delegate (Resource p) { return p.cResourceNo == lSchProductRouteRes.cResourceNo; });

                    if (lSchProductRouteRes.resource == null)
                    {
                        throw new Exception("排程ID[" + lSchProductRouteRes.iSchSdID + "," + lSchProductRouteRes.iProcessProductID + "] 资源编号[" + lSchProductRouteRes.cResourceNo + "]不存在,注意区分大小写和空格,或者资源没有定义工作日历!");

                    }

                    //资源任务工艺特征 FProChaType1ID != "-1" && this.FProChaType1ID != "" 
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
                    //if (lSchProductRouteRes.resource.FProChaType7ID != "-1" && lSchProductRouteRes.resource.FProChaType7ID != "")
                    //    lSchProductRouteRes.resChaValue7 = new ResChaValue(lSchProductRouteRes.FResChaValue7ID, lSchProductRouteRes, 7);
                    //if (lSchProductRouteRes.resource.FProChaType8ID != "-1" && lSchProductRouteRes.resource.FProChaType8ID != "")
                    //    lSchProductRouteRes.resChaValue8 = new ResChaValue(lSchProductRouteRes.FResChaValue8ID, lSchProductRouteRes, 8);
                    //if (lSchProductRouteRes.resource.FProChaType9ID != "-1" && lSchProductRouteRes.resource.FProChaType9ID != "")
                    //    lSchProductRouteRes.resChaValue9 = new ResChaValue(lSchProductRouteRes.FResChaValue9ID, lSchProductRouteRes, 9);
                    //if (lSchProductRouteRes.resource.FProChaType10ID != "-1" && lSchProductRouteRes.resource.FProChaType10ID != "")
                    //    lSchProductRouteRes.resChaValue10 = new ResChaValue(lSchProductRouteRes.FResChaValue10ID, lSchProductRouteRes, 10);
                    //if (lSchProductRouteRes.resource.FProChaType11ID != "-1" && lSchProductRouteRes.resource.FProChaType11ID != "")
                    //    lSchProductRouteRes.resChaValue11 = new ResChaValue(lSchProductRouteRes.FResChaValue11ID, lSchProductRouteRes, 11);
                    //if (lSchProductRouteRes.resource.FProChaType12ID != "-1" && lSchProductRouteRes.resource.FProChaType12ID != "")
                    //    lSchProductRouteRes.resChaValue12 = new ResChaValue(lSchProductRouteRes.FResChaValue12ID, lSchProductRouteRes, 12);

                    lSchProductRouteRes.FResChaValue1Cyc = Convert.ToDouble(dr["FResChaValue1Cyc"]);
                    lSchProductRouteRes.FResChaValue2Cyc = Convert.ToDouble(dr["FResChaValue2Cyc"]);
                    lSchProductRouteRes.FResChaValue3Cyc = Convert.ToDouble(dr["FResChaValue3Cyc"]);
                    lSchProductRouteRes.FResChaValue4Cyc = Convert.ToDouble(dr["FResChaValue4Cyc"]);
                    lSchProductRouteRes.FResChaValue5Cyc = Convert.ToDouble(dr["FResChaValue5Cyc"]);
                    lSchProductRouteRes.FResChaValue6Cyc = Convert.ToDouble(dr["FResChaValue6Cyc"]);
                    //lSchProductRouteRes.FResChaValue7Cyc = Convert.ToDouble(dr["FResChaValue7Cyc"]);
                    //lSchProductRouteRes.FResChaValue8Cyc = Convert.ToDouble(dr["FResChaValue8Cyc"]);
                    //lSchProductRouteRes.FResChaValue9Cyc = Convert.ToDouble(dr["FResChaValue9Cyc"]);
                    //lSchProductRouteRes.FResChaValue10Cyc = Convert.ToDouble(dr["FResChaValue10Cyc"]);
                    //lSchProductRouteRes.FResChaValue11Cyc = Convert.ToDouble(dr["FResChaValue11Cyc"]);
                    //lSchProductRouteRes.FResChaValue12Cyc = Convert.ToDouble(dr["FResChaValue12Cyc"]);

                    //lSchProductRouteRes.cResourceNote = dr["cResourceNote"].ToString();
                    lSchProductRouteRes.cDefine22 = dr["cDefine22"].ToString();
                    lSchProductRouteRes.cDefine23 = dr["cDefine23"].ToString();
                    //lSchProductRouteRes.cDefine24 = dr["cDefine24"].ToString();
                    //lSchProductRouteRes.cDefine25 = dr["cDefine25"].ToString();
                    //lSchProductRouteRes.cDefine26 = dr["cDefine26"].ToString();
                    //lSchProductRouteRes.cDefine27 = dr["cDefine27"].ToString();
                    //lSchProductRouteRes.cDefine28 = dr["cDefine28"].ToString();
                    //lSchProductRouteRes.cDefine29 = dr["cDefine29"].ToString();
                    //lSchProductRouteRes.cDefine30 = dr["cDefine30"].ToString();
                    //lSchProductRouteRes.cDefine31 = dr["cDefine31"].ToString();
                    //lSchProductRouteRes.cDefine32 = dr["cDefine32"].ToString();
                    //lSchProductRouteRes.cDefine33 = dr["cDefine33"].ToString();
                    lSchProductRouteRes.cDefine34 = Convert.ToDouble(dr["cDefine34"]);
                    lSchProductRouteRes.cDefine35 = 0;//Convert.ToDouble(dr["cDefine35"]);
                                                      //lSchProductRouteRes.cDefine36 = dr["cDefine36"].ToString();
                                                      //lSchProductRouteRes.cDefine37 = dr["cDefine37"].ToString();

                    //ResChaValue(int iResChaValueID,SchProductRouteRes schProductRouteRes, int iPosition)

                    //传入资源列表
                    //lSchProductRouteRes.ResourceList = schData.ResourceList;




                    //排产批次
                    lSchProductRouteRes.iSchBatch = (int)dr["iSchBatch"];
                    //if (lSchProductRouteRes.cVersionNo.Trim() == "SureVersion")
                    //    lSchProductRouteRes.iSchBatch = 1;
                    //else
                    //    lSchProductRouteRes.iSchBatch = 6;

                    ////传入任务资源时间段,只取当前任务的已排时间段。


                    lSchProductRouteRes.TaskTimeRangeList = new List<TaskTimeRange>();//schData.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.iSchSdID == lSchProductRouteRes.iSchSdID && p.iProcessProductID == lSchProductRouteRes.iProcessProductID && p.iResProcessID == lSchProductRouteRes.iResProcessID; });
                                                                                      //lSchProductRouteRes.TaskTimeRangeList = schData.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.iSchSdID == lSchProductRouteRes.iSchSdID && p.iProcessProductID == lSchProductRouteRes.iProcessProductID && p.iResProcessID == lSchProductRouteRes.iResProcessID; });

                    //lSchProductRouteRes.schData = this.schData;
                    schData.SchProductRouteResList.Add(lSchProductRouteRes);

                    ////建立资源任务对象 与 任务时间段连系


                    //foreach (TaskTimeRange taskTimeRange1 in lSchProductRouteRes.TaskTimeRangeList)
                    //{
                    //    taskTimeRange1.schProductRouteRes = lSchProductRouteRes;
                    //}

                }
                #endregion
                Console.WriteLine("2.5 填充SchProductRouteItemList");
                //lsSchProductRouteItem
                #region//2.5 填充SchProductRouteItemList

                if (SchParam.cSelfEndDate == "1")
                {
                    schData.dtSchProductRouteItem = SqlPro.GetDataTable(lsSchProductRouteItem, null);
                    // 2022-01-21
                    //APSCommon.SqlPro.GetDataTable(lsSchProductRouteItem, "t_SchProductRouteItem"); 
                    foreach (DataRow dr in schData.dtSchProductRouteItem.Rows)
                    {
                        SchProductRouteItem lSchProductRouteItem = new SchProductRouteItem();
                        lSchProductRouteItem.schData = this.schData;

                        lSchProductRouteItem.iSchSdID = (int)dr["iSchSdID"];
                        lSchProductRouteItem.cVersionNo = dr["cVersionNo"].ToString();
                        lSchProductRouteItem.iProcessProductID = (int)dr["iProcessProductID"];

                        //没有对应的工序ID,不增加

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

                        //排产批次
                        lSchProductRouteItem.iSchBatch = (int)dr["iSchBatch"];
                        //if (lSchProductRouteItem.cVersionNo == "SureVersion")
                        //    lSchProductRouteItem.iSchBatch = 1;
                        //else
                        //    lSchProductRouteItem.iSchBatch = 6;

                        //传入资源列表
                        //lSchProductRouteRes.ResourceList = schData.ResourceList;

                        //从资源列表中找到当前资源，给该资源排此任务


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


                ////产品工序资源与任务时段表SchProductRouteResTime关系
                //foreach (SchProductRouteRes lSchProductRouteRes in SchProductRouteResList)
                //{
                //    //任务资源时间段列表，有些已确认的生产任务单，已经有资源占用时间段。而且不用重排
                //    lSchProductRouteRes.TaskTimeRangeList = TaskTimeRangeList.FindAll(delegate(TaskTimeRange p2) { return p2.iSchSdID == lSchProductRouteRes.iSchSdID && p2.iProcessProductID == lSchProductRouteRes.iProcessProductID && p2.iResProcessID == lSchProductRouteRes.iResProcessID; });

                //}

                int k = 0;

                //产品工序与资源产能表关系；//找每道工序的前工序和后工序对象


                foreach (SchProductRoute lSchProductRoute in schData.SchProductRouteList)
                {
                    //资源产能列表
                    lSchProductRoute.SchProductRouteResList = schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.iSchSdID == lSchProductRoute.iSchSdID && p.iProcessProductID == lSchProductRoute.iProcessProductID; });

                    //每到工序对应的产品
                    if (lSchProductRoute.SchProductRouteResList.Count > 0)
                    {

                        foreach (SchProductRouteRes lSchProductRouteRes in lSchProductRoute.SchProductRouteResList)
                        {
                            lSchProductRouteRes.schProductRoute = lSchProductRoute;
                        }
                    }
                    else
                    {
                        //不参与排产，时间默认为当前开工时间，完工时间
                        lSchProductRoute.dBegDate = this.schData.dtStart;
                        lSchProductRoute.dEndDate = this.schData.dtStart.AddMinutes(1);
                        lSchProductRoute.BScheduled = 1;  //无工序，不参与排产
                    }

                    //工序子料列表
                    if (SchParam.cSelfEndDate == "1")
                        lSchProductRoute.SchProductRouteItemList = schData.SchProductRouteItemList.FindAll(delegate (SchProductRouteItem p) { return p.iSchSdID == lSchProductRoute.iSchSdID && p.iProcessProductID == lSchProductRoute.iProcessProductID; });

                    ////工序子料对应的产品工艺


                    //foreach (SchProductRouteItem lSchProductRouteItem in lSchProductRoute.SchProductRouteItemList)
                    //{
                    //    lSchProductRouteItem.schProductRoute = lSchProductRoute;
                    //}


                    ////找每道工序的前工序 //lSchProductRoute.cPreProcessItem = dr["cPreProcessItem"].ToString();
                    ////lSchProductRoute.SchProductRoutePreList
                    //lSchProductRoute.SchProductRoutePreList = GetSchProductRouteList(lSchProductRoute.iSchSdID, lSchProductRoute.cPreProcessItem);

                    //if (lSchProductRoute.iProcessProductID == SchParam.iProcessProductID && lSchProductRoute.iSchSdID == SchParam.iSchSdID)
                    //{
                    //    int k = 0;
                    //}

                    //找后工序列表,同时后工序的前工序列表，增加本工序。 lSchProductRoute.cPostProcessItem = dr["cPostProcessItem"].ToString();
                    //lSchProductRoute.SchProductRouteNextList = GetSchProductRouteList(lSchProductRoute.iSchSdID, lSchProductRoute.cPostProcessItem,);

                    //待工工序查找前序工序，同时设置找到工序为后序工序;执行或完工工序不用处理

                    if (lSchProductRoute.cStatus != "2" && lSchProductRoute.cStatus != "4" && lSchProductRoute.cPreProcessItem != "")
                    {
                        GetSchProductRouteList(lSchProductRoute, true);
                    }  //执行工序重排
                    else if (SchParam.ExecTaskSchType != "1" && lSchProductRoute.cStatus != "4" && lSchProductRoute.cPreProcessItem != "")
                    {
                        GetSchProductRouteList(lSchProductRoute, true);
                    }

                    //先按资源选配比例，对有些资源进行排除.jonascheng 2019-03-19
                    ////找出所有选择可用的机台
                    if (lSchProductRoute.cVersionNo != "SureVersion")
                    {
                        List<SchProductRouteRes> ListRouteRes = lSchProductRoute.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p) { return p.cSelected == "1"; });

                        int iRandom;
                        int iCount = ListRouteRes.Count, iRowCount = ListRouteRes.Count;
                        int iResCount = lSchProductRoute.iDevCountPd;

                        if (iResCount == 0) iResCount = 2;


                        //多资源选择，而且可选资源大于可排资源数时

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
                                    //ListRouteRes.Remove(ListRouteRes[i]);
                                    ListRouteRes[i].cCanScheduled = "0";   //不排产 
                                    ListRouteRes[i].iResReqQty = 0;        //加工数量为0   
                                    ListRouteRes[i].iResRationHour = 0;    //加工工时为0 

                                    //ListRouteRes[i].cCanScheduled = "0";  //不能参与排产                        
                                    iCount--;
                                }

                                //如果可选择的资源不足时
                                if (iCount <= iResCount) break;
                            }
                        }
                    }



                    k++;
                }

                //产品订单与产品工序关系


                foreach (SchProduct lSchProduct in schData.SchProductList)
                {
                    //产品工序列表
                    lSchProduct.SchProductRouteList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.iSchSdID == lSchProduct.iSchSdID; });

                    //每到工序对应的产品


                    foreach (SchProductRoute lSchProductRoute in lSchProduct.SchProductRouteList)
                    {
                        lSchProductRoute.schProduct = lSchProduct;
                    }
                }

                //产品工单与产品工序关系


                foreach (SchProductWorkItem lSchProductWorkItem in schData.SchProductWorkItemList)
                {
                    //产品工序列表
                    lSchProductWorkItem.SchProductRouteList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p) { return p.iSchSdID == lSchProductWorkItem.iSchSdID && p.cWoNo == lSchProductWorkItem.cWoNo; });

                    //每到工序对应的生产任务单
                    foreach (SchProductRoute lSchProductRoute in lSchProductWorkItem.SchProductRouteList)
                    {
                        lSchProductRoute.schProductWorkItem = lSchProductWorkItem;
                    }
                }


                ////资源与任务对应关系 SchProductRouteResList
                //foreach (Resource lResource in schData.ResourceList)
                //{
                //    //产品工序列表
                //    lResource.SchProductRouteResList = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cResourceNo == lResource.cResourceNo; });

                //}

                #endregion
                Console.WriteLine("2.7 其他基础资料表");
                #region//2.7 其他基础资料表



                //2.6 t_WorkCenter 
                schData.dtWorkCenter = SqlPro.GetDataTable(lsWorkCenter, null);//APSCommon.SqlPro.GetDataTable(lsWorkCenter, "t_WorkCenter");

                //2.7 t_Department
                schData.dtDepartment = SqlPro.GetDataTable(lsDepartment, null);//APSCommon.SqlPro.GetDataTable(lsDepartment, "t_Department");

                //2.8 t_Person

                schData.dtPerson = SqlPro.GetDataTable(lsPerson, null); //APSCommon.SqlPro.GetDataTable(lsPerson, "t_Person");

                //2.9 t_team
                schData.dtTeam = SqlPro.GetDataTable(lsteam, null); //APSCommon.SqlPro.GetDataTable(lsteam, "t_team");



                //2.11 t_TechLearnCurves
                schData.dtTechLearnCurves = SqlPro.GetDataTable(lsTechLearnCurves, null); //APSCommon.SqlPro.GetDataTable(lsTechLearnCurves, "t_TechLearnCurves");

                //2.12 t_ResTechScheduSN
                schData.dtResTechScheduSN = SqlPro.GetDataTable(lsResTechScheduSN, null); //APSCommon.SqlPro.GetDataTable(lsResTechScheduSN, "t_ResTechScheduSN");



                #endregion

                //计算工序明细排产资源数 CalDevCountPD(SchProductRoute lSchProductRoute, string colName = "iDevCountPd", string cExpress = "")
                List<SchProductRoute> SchProductRouteList = schData.SchProductRouteList.FindAll(delegate (SchProductRoute p1) { return p1.cDevCountPdExp != "" && p1.cWoNo == ""; });

                //foreach (SchProductRoute lSchProductRoute in SchProductRouteList)
                //{
                //    Expression.CalDevCountPD(lSchProductRoute, "iDevCountPd", lSchProductRoute.cDevCountPdExp);

                //    string sUpdatesql = string.Format("update t_SchProductRoute set iDevCountPd = {0} where cVersionNo = '{1}' and iSchSdID = {2} and iProcessProductID = {3}", lSchProductRoute.iDevCountPd, lSchProductRoute.cVersionNo, lSchProductRoute.iSchSdID, lSchProductRoute.iProcessProductID);

                //    GetSqlDapper(UserContext.Current.getCompanyID()).ExcuteNonQuery(sUpdatesql, null);
                //    //APSCommon.SqlPro.ExecuteNonQuery(sUpdatesql);

                //    int iCount = lSchProductRoute.iDevCountPd;
                //}

                Console.WriteLine("第二步骤结束");
                //Global.logger.Info(TransPro.TransString("GetSchData执行完成"), "排程运算");

            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置第二步骤,出错内容：" + ex1.ToString());
                throw ex1;
                //Messagezk.Show("排产计算出错！位置GetSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            return 1;

        }

        //开工工序先排,占用相应时间段

        private int ResSchTaskInit()
        {
            //#region//2.5 按资源,上次已排生产任务单,按上次生产任务单计划开始时间，计划结束时间，生成时间段，占用资源


            //foreach (Resource resource in this.schData.ResourceList)
            //{
            //    //备份资源工作日历
            //    resource.ResTimeRangeListBak = resource.ResTimeRangeList;

            //    //工序处于开工状态时，固定，不重新排产 p.schProductRoute.cStatus == "1" || 暂停、

            //    List<SchProductRouteRes> resSchProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p) { return p.cResourceNo == resource.cResourceNo && (p.schProductRoute.cStatus == "2"); });  //p.cWoNo != ""

            //    if (resSchProductRouteResList.Count < 1) continue;

            //    if (resource.cResourceNo == "BC-03-03")
            //    {
            //        int j = 1;
            //    }
            //    //所有生产任务单号不为空的，都是已排程确认过的


            //    foreach (SchProductRouteRes schProductRouteRes in resSchProductRouteResList)
            //    {
            //        resource.ResSchTaskInit(schProductRouteRes, schProductRouteRes.dResBegDate, schProductRouteRes.dResEndDate);
            //    }



            //}



            //#endregion


            return 1;
        }

        //cPreProcessItem
        private int GetSchProductRouteList(SchProductRoute as_SchProductRoute, Boolean bPreProcess = true)
        {

            //List<SchProductRoute> ListReturn = new List<SchProductRoute>(10);
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

                //如果没找到，先不处理 2020-11-14
                if (SchProductRoute1 == null)
                {
                    //throw new Exception("订单排程号:[" + iSchSdID + ", [" + as_SchProductRoute.iProcessProductID + "],后序工序号不存在[" + cPreProcessItem + "]工序ID号：" + ProcessItem[i].ToString() + ",工艺模型不完整!");
                    //return -1;
                }
                else
                {

                    if (bPreProcess)  //找前序工序,传入待工工序
                    {
                        //ListReturn.Add(SchProductRoute1);
                        //后工序列表,增加找到的工序SchProductRoute1
                        as_SchProductRoute.SchProductRoutePreList.Add(SchProductRoute1);

                        //后工序的前工序列表,增加本工序

                        SchProductRoute1.SchProductRouteNextList.Add(as_SchProductRoute);
                    }
                    else       //找后序工序,暂不用

                    {
                        //ListReturn.Add(SchProductRoute1);
                        //后工序列表,增加找到的工序SchProductRoute1
                        as_SchProductRoute.SchProductRouteNextList.Add(SchProductRoute1);

                        //后工序的前工序列表,增加本工序

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
                //内存消耗过大，先对所有代进行垃圾回收。 ----2019-07-02
                //先把表数据清空
                //schData.dtSchProduct.Dispose();
                schData.dtResource.Dispose();
                schData.dt_ResourceSpecTime.Dispose();
                //schData.dtSchProductRoute.Dispose();
                //schData.dtSchProductRouteRes.Dispose();
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
                //回收垃圾
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //4.4 保存数据到数据库 t_SchProductRouteRes，t_SchProductRoute，t_SchProduct


                //1、排程任务时间段明细 t_SchProductRouteResTimeTemp，更新到t_SchProductRouteResTime   

                Console.WriteLine("排程任务时间段明细 t_SchProductRouteResTimeTemp，更新到t_SchProductRouteResTime");
                if (SaveDataResTime("SaveDataResTime") < 0) return -1;

                Console.WriteLine("2、保存t_SchProductRouteResTemp");
                //2、保存t_SchProductRouteResTemp
                //WaitCallback method2 = (t) => SaveDataSchRouteRes(t);
                //ThreadPool.QueueUserWorkItem(method2, "SaveDataSchRouteRes");
                if (SaveDataSchRouteRes("SaveDataSchRouteRes") < 0) return -1;

                Console.WriteLine("3、保存t_SchProductRouteTemp");
                //3、保存t_SchProductRouteTemp
                //WaitCallback method3 = (t) => SaveDataSchRoute(t);
                //ThreadPool.QueueUserWorkItem(method3, "SaveDataSchRoute");
                if (SaveDataSchRoute("SaveDataSchRoute") < 0) return -1;

                Console.WriteLine("4、保存t_SchProductTemp");
                ////4、保存t_SchProductTemp
                ////WaitCallback method4 = (t) => SaveSchProduct(t);
                ////ThreadPool.QueueUserWorkItem(method4, "SaveSchProduct");

                if (SaveSchProduct("SaveSchProduct") < 0) return -1;

                Console.WriteLine("5、保存t_SchProductWorkItemTemp");
                ////5、保存t_SchProductWorkItemTemp
                ////WaitCallback method4 = (t) => SaveSchProductWorkItem(t);
                ////ThreadPool.QueueUserWorkItem(method4, "SaveSchProductWorkItem");

                if (SaveSchProductWorkItem("SaveSchProductWorkItem") < 0) return -1;


                //Global.logger.Info(TransPro.TransString("SaveSchData执行完成"), "排程运算");

                //Messagezk.Show("排产计算成功,请产看相关报表！", Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex1)
            {

                Console.WriteLine("排产计算出错！位置4、排程结果写回数据库,出错内容：" + ex1.ToString());
                throw (ex1);
                //Messagezk.Show("排产计算完成,结果写回数据库出错！位置SaveSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            return 1;
        }

        //保存资源任务排程时间段明细，单独线程执行
        public int SaveDataResTime(object cType)
        {
            int li_return;
            Console.WriteLine("开始保存排程任务时间段明细");
            try
            {
                //删除正式版本数据
                //string lsSql2 = "delete from t_SchProductRouteResTime ";
                string lsSql2 = "truncate table t_SchProductRouteResTimeTemp ";
                //ToDataTable(DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null));  //DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null)
                //if (APSCommon.SqlPro.ExecuteNonQuery(lsSql2) < 0) return -1;
                Console.WriteLine("ExcuteNonQuery开始");
                int iReturnNum = SqlPro.ExecuteNonQuery(lsSql2, null);
                //if (DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null) < 0) return -1;

                //DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null);


                Console.WriteLine("ExcuteNonQuery跳过");
                string lsSchProductRouteResTime = @"select cVersionNo,iSchSdID,iProcessProductID,isnull(iInterID,0) as iInterID,isnull(iWoProcessID,0) iWoProcessID,
                    isnull(iResProcessID,0) as iResProcessID,isnull(cWoNo,'') cWoNo ,isnull(iResourceID,0) as iResourceID,cResourceNo,cResourceName,
                    iTimeID,iPeriodTimeID,dResBegDate,dResEndDate,iResReqQty,isnull(iResRationHour,0) as iResRationHour,isnull(iResRealRationHour,0) as iResRealRationHour,isnull(cSimulateVer,'') as cSimulateVer,isnull(cNote,'') as cNote,isnull(cTaskType,'1') as cTaskType,dPeriodDay,isnull(FShiftType,'A班') FShiftType  
                        from t_SchProductRouteResTimeTemp where  1 = 2 ";
                // DataTable dtSchProductRouteResTime = APSCommon.SqlPro.GetDataTable(lsSchProductRouteResTime, "t_SchProductRouteResTime");

                DataTable dtSchProductRouteResTime = SqlPro.GetDataTable(lsSchProductRouteResTime, null); //APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteResTimeTemp where 1 = 2");

                //cTaskType = 0;           //任务时间类型： 0 空闲，1 前准备时间 2 加工时间 3 后准备时间


                //所有非空闲的时间段，写回数据库
                int iTime = 1;

                ////把资源任务列表中的已排任务，都加入schData.TaskTimeRangeList                
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

                //排程任务时段写回数据库
                Console.WriteLine("排程任务时段写回数据库");
                Console.WriteLine("进入foreach");
                foreach (TaskTimeRange lTaskTimeRange in schData.TaskTimeRangeList.FindAll(delegate (TaskTimeRange p2) { return p2.cTaskType != 0; }))
                {

                    if (lTaskTimeRange.iProcessProductID == -1) continue;

                    //资源产能无限,排产时间段不用写回数据库,提供效率 
                    if (lTaskTimeRange.resource.cIsInfinityAbility == "1") continue;

                    DataRow dr = dtSchProductRouteResTime.NewRow();


                    dr["iSchSdID"] = lTaskTimeRange.iSchSdID;
                    dr["cVersionNo"] = lTaskTimeRange.cVersionNo;
                    dr["iProcessProductID"] = lTaskTimeRange.iProcessProductID;
                    dr["iResProcessID"] = lTaskTimeRange.iResProcessID;
                    dr["cResourceNo"] = lTaskTimeRange.CResourceNo;
                    //dr["cWoNo"] = lTaskTimeRange.cWoNo;
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

                    //增加新行
                    dtSchProductRouteResTime.Rows.Add(dr);

                    ////-------------特殊处理，分多次保存，如果临时表长度大于90000行时，保存一次 2019-07-05--------------------

                    //if (dtSchProductRouteResTime.Rows.Count > iBatchRowCount)
                    //{
                    //    if (!Global.bCS)
                    //    {
                    //        Console.WriteLine("SaveTable");
                    //        li_return = GetSqlDapper(UserContext.Current.getCompanyID()).BulkInsert(dtSchProductRouteResTime, "t_SchProductRouteResTime");

                    //        if (li_return < 0) return -1;
                    //        dtSchProductRouteResTime = GetSqlDapper(UserContext.Current.getCompanyID()).GetDataTable("select * from t_SchProductRouteResTime where 1 = 2", null);// APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteResTime where 1 = 2");
                    //    }
                    //    //else
                    //    //{
                    //    //    ////批量保存数据库
                    ///if (SqlBulkCopyResTime(dtSchProductRouteResTime, "dbo.t_SchProductRouteResTime") < 1) return -1;
                    //    //}
                    //}

                    iTime++;
                }


                ////保存dtSchProductRouteResTime表数据到数据库               
                //if (!Global.bCS)
                //{
                //    Console.WriteLine("保存dtSchProductRouteResTime表数据到数据库");
                //    li_return = SqlPro.BulkInsert(dtSchProductRouteResTime, "t_SchProductRouteResTimeTemp");
                //    if (li_return < 0) return -1;
                //}
                //else   //数据库直连方式
                //{
                    //批量保存数据库
                    Console.WriteLine("批量保存数据库");
                    if (SqlBulkCopyResTime(dtSchProductRouteResTime, "dbo.t_SchProductRouteResTimeTemp") < 1) return -1;
                    //if (DBServerProvider.SqlDapper.BulkInsert(dtSchProductRouteResTime, "dbo.t_SchProductRouteResTimeTemp") < 1) return -1;  2021-08-24
                    //SqlPro.BulkInsert(dtSchProductRouteResTime, "dbo.t_SchProductRouteResTimeTemp");
                // }

                Console.WriteLine("排程任务时段写回数据库执行完毕");
                //清空类表数据TaskTimeRangeList内存
                schData.TaskTimeRangeList.Clear();
                dtSchProductRouteResTime.Dispose();

            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存资源任务排程时间段明细，单独线程执行,出错内容：" + ex1.ToString());
                //Messagezk.Show("排产计算完成,dtSchProductRouteResTime写回数据库出错！位置SaveSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }



            return 1;
        }

        //大数据量批量保存
        public int SqlBulkCopyResTime(DataTable dt, string dtName)
        {
            //string ConnectString = Configuration.GetConnectionString();// "server=120.77.62.33;database=ZKAPS_5.0;user=sa;password=zkaps#1;";
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

                    //DataTable dt = GetDataTableData();//数据源数据
                    //bulkCopy.BatchSize = 3;
                    //Stopwatch stopwatch = new Stopwatch();//跑表，该类可以进行时间的统计
                    //stopwatch.Start();//跑表开始
                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中

                    //Response.Write("插入数据所用时间:" + stopwatch.Elapsed);//跑表结束，Elapsed是统计到的时间
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存2317,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }



            return 1;
        }


        //保存资源任务明细t_SchProductRouteResTemp，单独线程执行
        public int SaveDataSchRouteRes(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID;
            int li_return;

            //int iSchSdID = -1;
            //int iProcessProductID = -1 ;

            //4.1 写回t_SchProductRouteRes

            try
            {
                //schData.dtSchProductRouteRes.Rows

                SchProductRouteRes lSchProductRouteRes;
                SchProductRoute lSchProductRoute;

                //有正式版本数据，可能会存在主键重复,全清 JonasCheng 2019-12-18
                //string lsSql2 = "delete from t_SchProductRouteResTemp";  // where cVersionNo = '" + schData.cVersionNo + "'";
                string lsSql2 = "truncate table  t_SchProductRouteResTemp";
                //if (APSCommon.SqlPro.ExecuteNonQuery(lsSql2) < 0) return -1;  ToDataTable(DBServerProvider.SqlDapper.QueryList<object>("select * from t_SchProductRouteResTemp where 1 = 2 ", null));
                //if (DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null) < 0) return -1; 2021 8-24
                SqlPro.ExecuteNonQuery(lsSql2, null);
                DataTable dtSchProductRouteResTemp = SqlPro.GetDataTable("select * from t_SchProductRouteResTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteResTemp where 1 = 2");



                foreach (DataRow dr in schData.dtSchProductRouteRes.Rows)
                {


                    cVersionNo = dr["cVersionNo"].ToString().Trim();
                    iSchSdID = (int)dr["iSchSdID"];
                    iProcessProductID = (int)dr["iProcessProductID"];
                    iResProcessID = (int)dr["iResProcessID"];

                    //SchProductRouteRes lSchProductRouteRes = new SchProductRouteRes();
                    lSchProductRouteRes = schData.SchProductRouteResList.Find(delegate (SchProductRouteRes p2) { return p2.iSchSdID == iSchSdID && p2.iProcessProductID == iProcessProductID && p2.iResProcessID == iResProcessID; });



                    //iResReqQty,dResBegDate,dResEndDate,iResRationHour,iResPreTime,iResPostTime
                    if (lSchProductRouteRes == null) continue;

                    if (lSchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && lSchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || lSchProductRouteRes.iProcessProductID == 193864 && lSchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                    {
                        //string message = string.Format(@"3.4、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                        //                                        lSchProductRouteRes.iSchSN, lSchProductRouteRes.iSchSdID, lSchProductRouteRes.iProcessProductID, lSchProductRouteRes.cResourceNo, DateTime.Now, DateTime.Now);
                        //SchParam.Debug(message, "资源运算");                        
                    }

                    //dr["iResReqQty"] = lSchProductRouteRes.iResReqQty;
                    //dr["dResBegDate"] = lSchProductRouteRes.dResBegDate;
                    //dr["dResEndDate"] = lSchProductRouteRes.dResEndDate;
                    ////总工时包含资源前准备时间，后准备时间
                    //dr["iResRationHour"] = Math.Round((lSchProductRouteRes.iResRationHour) / 60,2);//Math.Round((Global.RationHourUnit == "3" ? (lSchProductRouteRes.iResRationHour) : (Global.RationHourUnit == "1" ? (lSchProductRouteRes.iResRationHour ) / 3600 : (lSchProductRouteRes.iResRationHour ) / 60)),2); //(Global.RationHourUnit == "3" ? (lSchProductRouteRes.iResRationHour + lSchProductRouteRes.iResPreTime + lSchProductRouteRes.iResPostTime) : (Global.RationHourUnit == "1" ? (lSchProductRouteRes.iResRationHour + lSchProductRouteRes.iResPreTime + lSchProductRouteRes.iResPostTime) / 3600 : (lSchProductRouteRes.iResRationHour + lSchProductRouteRes.iResPreTime + lSchProductRouteRes.iResPostTime) / 60));
                    //dr["iResPreTime"] = Math.Round(lSchProductRouteRes.iResPreTime/60,2);      //单位 秒 转换成分钟


                    //dr["iResPostTime"] = Math.Round(lSchProductRouteRes.iResPostTime/60,2);   //单位 秒 转换成分钟,保留两位小数

                    //dr["iCycTime"] = lSchProductRouteRes.iCycTime;
                    //dr["cDefine35"] = lSchProductRouteRes.iSchSN;            //排产顺序

                    //开工时间小于当前日期前一天的，不反写,有些任务没有分配到计划数量，但计划开工日期小于当天，也要返写。2021-03-31 JonasCheng 
                    //if (lSchProductRouteRes.dResBegDate < SchParam.dtToday.AddDays(-1)) continue;


                    if (lSchProductRouteRes.schProductRoute == null)
                    {

                        message = "订单行号：" + lSchProductRouteRes.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + lSchProductRouteRes.iProcessProductID + "工单号:" + lSchProductRouteRes.cWoNo + "\n\r " + "没有对应的工序明细,请检查是否完工！";
                        schPrecent = schData.iProgress;
                        //SendAsync(name, schPrecent, message);
                        this.dlShowProcess(schPrecent.ToString(), message);
                        //throw new Exception("订单行号：" + lSchProductRouteRes.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + lSchProductRouteRes.iProcessProductID + "工单号:" + lSchProductRouteRes.cWoNo + "\n\r " + "没有对应的工序明细,请检查是否完工！");
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

                    //已经执行或完工工序,不反写

                    //if (SchParam.ExecTaskSchType == "1")    //开工冻结,不用反写
                    //    if (cStatus == "2" || cStatus == "4") continue;
                    //    else
                    //        if (cStatus == "4") continue;
                    //工序完工不反写  2021-11-07 

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

                    //lSchProductRouteRes.dEarliestStartTime

                    drNew["cDefine35"] = lSchProductRouteRes.iSchSN;               //最新任务排产顺序
                    drNew["cDefine24"] = lSchProductRouteRes.iBatch.ToString();
                    drNew["cDefine25"] = lSchProductRouteRes.cResourceNo + " " + lSchProductRouteRes.cDefine25;
                    //drNew["cDefine26"] = lSchProductRouteRes.cSelected;

                    //特殊处理
                    if (lSchProductRouteRes.dCanResBegDate < SchParam.dtToday)
                    {
                        drNew["dResCanBegDate"] = lSchProductRouteRes.dResBegDate;    //可开工时间
                        drNew["iResWaitTime"] = 0;                                   //等待时间
                    }
                    else if (lSchProductRouteRes.dCanResBegDate > lSchProductRouteRes.dResBegDate) //Convert.ToDateTime("2000-01-01"))
                    {
                        //drNew["dResCanBegDate"] = lSchProductRouteRes.dResBegDate;     //可开工时间
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
                    //drNew["cDefine28"] = lSchProductRouteRes.cDefine38.ToString();         //
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

                    //增加资源编号，资源组编号字段返回
                    drNew["cResourceNo"] = lSchProductRouteRes.cResourceNo;
                    drNew["cResourceName"] = lSchProductRouteRes.resource.cResourceName;
                    drNew["cTeamResourceNo"] = lSchProductRouteRes.cTeamResourceNo;

                    dtSchProductRouteResTemp.Rows.Add(drNew);

                    //-------------特殊处理，分多次保存，如果临时表长度大于90000行时，保存一次 2019-07-05--------------------                    
                    //if (dtSchProductRouteResTemp.Rows.Count > iBatchRowCount)
                    //{
                    //    if (!Global.bCS)
                    //    {
                    //        li_return = GetSqlDapper(UserContext.Current.getCompanyID()).BulkInsert(dtSchProductRouteResTemp, "t_SchProductRouteResTemp");
                    //        if (li_return < 0) return -1;

                    //        dtSchProductRouteResTemp = DBServerProvider.SqlDapper.GetDataTable("select * from t_SchProductRouteResTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteResTemp where 1 = 2");
                    //    }
                    //    //else
                    //    //{
                        //    ////批量保存数据库
                        //    if (SqlBulkCopyRouteRes(dtSchProductRouteResTemp, "dbo.t_SchProductRouteResTemp") < 0) return -1;
                    //    //}

                    //}




                }

                //超过10万行记录时，会报内存溢出错误 2019-07-05                

                //批量保存数据库
                if (SqlBulkCopyRouteRes(dtSchProductRouteResTemp, "dbo.t_SchProductRouteResTemp") < 0) return -1;   //2021-08-24 xx

                //if (SqlPro.BulkInsert(dtSchProductRouteResTemp, "dbo.t_SchProductRouteResTemp") < 0) return -1;
               


                //清理dtSchProductRouteRes
                schData.dtSchProductRouteRes.Dispose();
                schData.SchProductRouteResList.Clear();
                dtSchProductRouteResTemp.Dispose();

                ////回收垃圾
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存资源任务明细t_SchProductRouteResTemp，单独线程执行2515,出错内容：" + ex1.ToString());
                //Messagezk.Show("排产计算完成,dtSchProductRouteRes写回数据库出错！位置SaveSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }

            return 1;
        }

        //大数据量批量保存
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

                    //Response.Write("插入数据所用时间:" + stopwatch.Elapsed);//跑表结束，Elapsed是统计到的时间
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存2568,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }



            return 1;
        }

        //保存工序明细t_SchProductRouteTemp，单独线程执行
        public int SaveDataSchRoute(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID;
            int li_return;

            //已经执行或完工工序,不反写

            DataRow[] SchProductRouteRows = schData.dtSchProductRoute.Select("cStatus not in ('4')");

            //有正式版本数据，可能会存在主键重复,全清 JonasCheng 2019-12-18
            //string lsSql2 = "delete from t_SchProductRouteTemp "  //where cVersionNo = '" + schData.cVersionNo + "'";

            string lsSql2 = "truncate table  t_SchProductRouteTemp";
            //if (APSCommon.SqlPro.ExecuteNonQuery(lsSql2) < 0) return -1;  //ToDataTable(DBServerProvider.SqlDapper.QueryList<object>("select * from t_SchProductRouteResTemp where 1 = 2 ", null));

            //if (DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null) < 0) return -1; 2021-08-24
            SqlPro.ExecuteNonQuery(lsSql2, null);
            DataTable dtSchProductRouteTemp =SqlPro.GetDataTable("select * from t_SchProductRouteTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteTemp where 1 = 2");


            try
            {
                SchProductRoute lSchProductRoute;
                //4.2 写回t_SchProductRoute
                foreach (DataRow dr in SchProductRouteRows)//schData.dtSchProductRoute.Rows
                {
                    cVersionNo = dr["cVersionNo"].ToString();
                    iSchSdID = (int)dr["iSchSdID"];
                    iProcessProductID = (int)dr["iProcessProductID"];
                    cStatus = dr["cStatus"].ToString();

                    //SchProductRouteRes lSchProductRouteRes = new SchProductRouteRes();
                    lSchProductRoute = schData.SchProductRouteList.Find(delegate (SchProductRoute p2) { return p2.iSchSdID == iSchSdID && p2.iProcessProductID == iProcessProductID; });

                    //dBegDate,dEndDate,iLaborTime,iLeadTime,iReqQty,iSeqPreTime,iSeqPostTime
                    if (lSchProductRoute == null) continue;

                    //dr["dBegDate"] = lSchProductRoute.dBegDate;
                    //dr["dEndDate"] = lSchProductRoute.dEndDate;
                    //dr["iLaborTime"] = (Global.RationHourUnit == "3" ? lSchProductRoute.iLaborTime : (Global.RationHourUnit == "1" ? lSchProductRoute.iLaborTime / 3600 : lSchProductRoute.iLaborTime / 60));
                    //dr["iLeadTime"] = lSchProductRoute.iLeadTime;

                    //dr["iSeqPreTime"] = lSchProductRoute.iSeqPreTime;
                    //dr["iSeqPostTime"] = lSchProductRoute.iSeqPostTime;

                    //开工时间小于当前日期前一天的，不反写
                    if (lSchProductRoute.dBegDate < SchParam.dtToday.AddDays(-1)) continue;

                    cStatus = lSchProductRoute.cStatus;

                    //已经执行或完工工序,不反写

                    //if (SchParam.ExecTaskSchType == "1")    //开工冻结,不用反写
                    //    if (cStatus == "2" || cStatus == "4") continue;
                    //    else
                    //        if (cStatus == "4") continue;

                    //sSqlUpdSchProductRoute = string.Format(@"exec P_UpdSchProductRoute '{0}',{1},{2},'{3}','{4}','{5}',{6},{7},{8},{9}", cVersionNo, iSchSdID, iProcessProductID, cStatus, lSchProductRoute.dBegDate, lSchProductRoute.dEndDate, (Global.RationHourUnit == "3" ? lSchProductRoute.iLaborTime : (Global.RationHourUnit == "1" ? lSchProductRoute.iLaborTime / 3600 : lSchProductRoute.iLaborTime / 60)), lSchProductRoute.iLeadTime, lSchProductRoute.iSeqPreTime,lSchProductRoute.iSeqPostTime);
                    //APSCommon.SqlPro.ExecuteNonQuery(sSqlUpdSchProductRoute);

                    DataRow drNew = dtSchProductRouteTemp.NewRow();
                    drNew["cVersionNo"] = dr["cVersionNo"].ToString().Trim();
                    drNew["iSchSdID"] = (int)dr["iSchSdID"];
                    drNew["iProcessProductID"] = (int)dr["iProcessProductID"];
                    //drNew["iResProcessID"] = (int)dr["iResProcessID"];
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

                    ////-------------特殊处理，分多次保存，如果临时表长度大于90000行时，保存一次 2019-07-05--------------------
                    ////超过10万行记录时，会报内存溢出错误 2019-07-05      
                    //if (dtSchProductRouteTemp.Rows.Count > iBatchRowCount)
                    //{
                    //    //保存t_SchProductRouteResTemp表数据到数据库               
                    //    if (!Global.bCS)
                    //    {
                    //        li_return = GetSqlDapper(UserContext.Current.getCompanyID()).BulkInsert(dtSchProductRouteTemp, "t_SchProductRouteTemp");
                    //        if (li_return < 0) return -1;
                    //        dtSchProductRouteTemp = GetSqlDapper(UserContext.Current.getCompanyID()).GetDataTable("select * from t_SchProductRouteTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductRouteTemp where 1 = 2");
                    //    }
                    //    //else   //数据库直连方式
                    //    //{
                    //    //批量保存数据库
                    //    if (SqlBulkCopySchRoute(dtSchProductRouteTemp, "dbo.t_SchProductRouteTemp") < 0) return -1;
                    //}

                    //}


                }

                //保存t_SchProductRouteTemp表数据到数据库

                //if (!Global.bCS)
                //{
                //    li_return = GetSqlDapper(UserContext.Current.getCompanyID()).BulkInsert(dtSchProductRouteTemp, "t_SchProductRouteTemp");
                //    if (li_return < 0) return -1;
                //}
                //else   //数据库直连方式
                //{
                //批量保存数据库
                if (SqlBulkCopySchRoute(dtSchProductRouteTemp, "dbo.t_SchProductRouteTemp") < 0) return -1;  //2021-08-24
                //if (DBServerProvider.SqlDapper.BulkInsert(dtSchProductRouteTemp, "dbo.t_SchProductRouteTemp") < 0) return -1;
                //SqlPro.BulkInsert(dtSchProductRouteTemp, "dbo.t_SchProductRouteTemp");



                //}


                //清理dtSchProductRoute
                schData.dtSchProductRoute.Dispose();
                schData.SchProductRouteList.Clear();
                dtSchProductRouteTemp.Dispose();

                //回收垃圾
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存工序明细t_SchProductRouteTemp，单独线程执行,出错内容：" + ex1.ToString());

                //Messagezk.Show("排产计算完成,t_SchProductRouteTemp写回数据库出错！位置SaveSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            return 1;

        }

        //大数据量批量保存
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

                    //Response.Write("插入数据所用时间:" + stopwatch.Elapsed);//跑表结束，Elapsed是统计到的时间
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存2754,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }



            return 1;
        }

        //保存排程订单信息
        public int SaveSchProduct(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID;
            int li_return;

            ////4.3 写回t_SchProduct

            //try
            //{

            //    foreach (DataRow dr in schData.dtSchProduct.Rows)
            //    {
            //        iSchSdID = (int)dr["iSchSdID"];
            //        cVersionNo = dr["cVersionNo"].ToString();

            //        SchProduct lSchProduct = schData.SchProductList.Find(delegate(SchProduct p2) { return p2.iSchSdID == iSchSdID; });

            //        //dBegDate,dEndDate,iLaborTime,iLeadTime,iReqQty,iSeqPreTime,iSeqPostTime
            //        if (lSchProduct == null) continue;

            //        //dr["dRequireDate"] = lSchProduct.dRequireDate;         //预计完工时间
            //        //dr["dEarliestSchDate"] = lSchProduct.dEarliestSchDate; //排程开始时间


            //        //dr["dEndDate"] = (lSchProduct.dEndDate < DateTime.Parse("1901-12-1") ? lSchProduct.dRequireDate : lSchProduct.dEndDate);       //预计完工时间
            //        //dr["dBegDate"] = (lSchProduct.dBegDate < DateTime.Parse("1901-12-1") ? schData.dtStart : lSchProduct.dBegDate);          //排程开始时间


            //        //dr["cSchStatus"] = "1";                             //已排产     
            //        //dr["iPriority"] = lSchProduct.iSchPriority;         //排程优先级 反写

            //        sSqlUpdSchProduct = string.Format(@"exec P_UpdSchProduct '{0}',{1},'{2}','{3}','{4}',{5}", cVersionNo, iSchSdID, "1", (lSchProduct.dBegDate < DateTime.Parse("1901-12-1") ? schData.dtStart : lSchProduct.dBegDate), (lSchProduct.dEndDate < DateTime.Parse("1901-12-1") ? lSchProduct.dRequireDate : lSchProduct.dEndDate), lSchProduct.iSchPriority);
            //        APSCommon.SqlPro.ExecuteNonQuery(sSqlUpdSchProduct);

            //    }

            //    //清理dtSchProductRoute
            //    schData.dtSchProduct.Dispose();
            //    schData.SchProductList.Clear();
            //}
            //catch (Exception ex1)
            //{

            //    Messagezk.Show("排产计算完成,dtSchProduct写回数据库出错！位置SaveSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return -1;
            //}

            if (schData.dtSchProduct == null || schData.dtSchProduct.Rows.Count < 1)
            {
                return 1;
            }

            //已经执行或完工工序,不反写

            DataRow[] SchProductRows = schData.dtSchProduct.Select("cStatus not in ('F','C')");

            //有正式版本数据，可能会存在主键重复,全清 JonasCheng 2019-12-18
            //string lsSql2 = "delete from t_SchProductRouteTemp "  //where cVersionNo = '" + schData.cVersionNo + "'";

            string lsSql2 = "truncate table  t_SchProductTemp";
            //if (APSCommon.SqlPro.ExecuteNonQuery(lsSql2) < 0) return -1; 
            //if (DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null) < 0) return -1; // 2021-08-24 xx
            SqlPro.ExecuteNonQuery(lsSql2, null);
            DataTable dtSchProductTemp = SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2");

            try
            {
                SchProduct lSchProduct;
                //4.2 写回t_SchProductRoute
                foreach (DataRow dr in SchProductRows)
                {
                    cVersionNo = dr["cVersionNo"].ToString();
                    iSchSdID = (int)dr["iSchSdID"];

                    lSchProduct = schData.SchProductList.Find(delegate (SchProduct p2) { return p2.iSchSdID == iSchSdID && p2.cVersionNo == cVersionNo; });

                    //dBegDate,dEndDate,iLaborTime,iLeadTime,iReqQty,iSeqPreTime,iSeqPostTime
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

                    //-------------特殊处理，分多次保存，如果临时表长度大于90000行时，保存一次 2019-07-05--------------------
                    //超过10万行记录时，会报内存溢出错误 2019-07-05      
                    if (dtSchProductTemp.Rows.Count > iBatchRowCount)
                    {
                        //保存t_SchProductRouteResTemp表数据到数据库               
                        //if (!Global.bCS)
                        //{
                            li_return = SqlPro.BulkInsert(dtSchProductTemp, "t_SchProductTemp");
                            if (li_return < 0) return -1;
                            dtSchProductTemp = SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductTemp where 1 = 2");
                        //}
                        //else   //数据库直连方式
                        //{
                        //    //批量保存数据库
                        //    if (SqlBulkCopySch(dtSchProductTemp, "dbo.t_SchProductRouteTemp") < 0) return -1;
                        //}

                    }

                }

                //保存t_SchProductTemp表数据到数据库

                //if (!Global.bCS)
                //{
                //    li_return = SqlPro.BulkInsert(dtSchProductTemp, "t_SchProductTemp");
                //    if (li_return < 0) return -1;
                //}
                //else   //数据库直连方式
                //{
                    //批量保存数据库
                    if (SqlBulkCopySch(dtSchProductTemp, "dbo.t_SchProductTemp") < 0) return -1;  //2021-08-24
                    //if (DBServerProvider.SqlDapper.BulkInsert(dtSchProductTemp, "dbo.t_SchProductTemp") < 0) return -1; 
                    //li_return = SqlPro.BulkInsert(dtSchProductTemp, "dbo.t_SchProductTemp");
                    //if (li_return < 0) return -1;
                //}

                //清理dtSchProductRoute
                schData.dtSchProduct.Dispose();
                schData.SchProductList.Clear();
                dtSchProductTemp.Dispose();

            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存排程订单信息2909,出错内容：" + ex1.ToString());
                //Messagezk.Show("排产计算完成,t_SchProductTemp写回数据库出错！位置SaveSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            return 1;

        }


        //保存排程工单信息
        public int SaveSchProductWorkItem(object cType)
        {
            string sSqlUpdSchProductRoute = "";
            string sSqlUpdSchProductRouteRes = "";
            string sSqlUpdSchProduct = "";
            string cVersionNo = "";
            string cStatus = "";
            int iSchSdID, iProcessProductID, iResProcessID, iBomAutoID;
            int li_return;

            ////4.4 写回t_SchProductWorkItem

            if (schData.dtSchProductWorkItem.Rows.Count < 1) return 1;
            //已经执行或完工工序,不反写

            DataRow[] SchProductWorkItemRows = schData.dtSchProductWorkItem.Select("cStatus not in ('F','C')");

            //有正式版本数据，可能会存在主键重复,全清 JonasCheng 2019-12-18
            //string lsSql2 = "delete from t_SchProductRouteTemp "  //where cVersionNo = '" + schData.cVersionNo + "'";

            string lsSql2 = "truncate table  t_SchProductWorkItemTemp";
            //if (DBServerProvider.SqlDapper.ExcuteNonQuery(lsSql2, null) < 0) return -1;  //2021-08-24
            SqlPro.ExecuteNonQuery(lsSql2, null);

            DataTable dtSchProductWorkItemTemp = SqlPro.GetDataTable("select * from t_SchProductWorkItemTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductWorkItemTemp where 1 = 2");

            try
            {
                SchProductWorkItem lSchProductWorkItem;
                //4.2 写回t_SchProductRoute
                foreach (DataRow dr in SchProductWorkItemRows)
                {
                    cVersionNo = dr["cVersionNo"].ToString();
                    iSchSdID = (int)dr["iSchSdID"];
                    iBomAutoID = (int)dr["iBomAutoID"];

                    lSchProductWorkItem = schData.SchProductWorkItemList.Find(delegate (SchProductWorkItem p2) { return p2.iSchSdID == iSchSdID && p2.cVersionNo == cVersionNo && p2.iBomAutoID == iBomAutoID; });

                    //dBegDate,dEndDate,iLaborTime,iLeadTime,iReqQty,iSeqPreTime,iSeqPostTime
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


                    //drNew["cSchStatus"] = "1";                                         //已排产   
                    drNew["cWoNo"] = lSchProductWorkItem.cWoNo;                           //工单号
                    drNew["cInvCode"] = lSchProductWorkItem.cInvCode;                     //物料编号
                    drNew["cSchSNType"] = lSchProductWorkItem.cSchSNType;                   //座次类别
                    drNew["iSchSN"] = lSchProductWorkItem.iSchSN;                          //座次号
                    drNew["iPriority"] = lSchProductWorkItem.iPriority;                   //订单优先级 反写
                    //drNew["iWoPriorityRes"] = lSchProductWorkItem.iWoPriorityRes;         //排程优先级 反写
                    drNew["iWoPriorityResLast"] = lSchProductWorkItem.iWoPriorityResLast; //排程优先级 反写

                    dtSchProductWorkItemTemp.Rows.Add(drNew);

                    //-------------特殊处理，分多次保存，如果临时表长度大于90000行时，保存一次 2019-07-05--------------------
                    //超过10万行记录时，会报内存溢出错误 2019-07-05      
                    if (dtSchProductWorkItemTemp.Rows.Count > iBatchRowCount)
                    {
                        ////保存t_SchProductRouteResTemp表数据到数据库               
                        //if (!Global.bCS)
                        //{
                        //    li_return = GetSqlDapper(UserContext.Current.getCompanyID()).BulkInsert(dtSchProductWorkItemTemp, "t_SchProductWorkItemTemp");
                        //    if (li_return < 0) return -1;
                        //    dtSchProductWorkItemTemp = GetSqlDapper(UserContext.Current.getCompanyID()).GetDataTable("select * from t_SchProductWorkItemTemp where 1 = 2 ", null);//APSCommon.SqlPro.GetDataTable("select * from t_SchProductWorkItemTemp where 1 = 2");
                        //}
                        //else   //数据库直连方式
                        //{
                        //批量保存数据库
                        SqlPro.BulkInsert(dtSchProductWorkItemTemp, "dbo.t_SchProductWorkItemTemp");
                            //if (SqlBulkCopySch(dtSchProductTemp, "dbo.t_SchProductRouteTemp") < 0) return -1;
                        //}

                    }

                }

                //保存t_SchProductWorkItemTemp表数据到数据库

                //if (!Global.bCS)
                //{
                //    li_return = GetSqlDapper(UserContext.Current.getCompanyID()).BulkInsert(dtSchProductWorkItemTemp, "t_SchProductWorkItemTemp");
                //    if (li_return < 0) return -1;
                //}
                //else   //数据库直连方式
                //{
                    //批量保存数据库
                    if (SqlBulkCopySchWorkItem(dtSchProductWorkItemTemp, "dbo.t_SchProductWorkItemTemp") < 0) return -1;
                    //if (SqlPro.BulkInsert(dtSchProductWorkItemTemp, "dbo.t_SchProductWorkItemTemp") < 0) return -1;
                //}

                //清理dtSchProductRoute
                schData.dtSchProduct.Dispose();
                schData.SchProductList.Clear();
                dtSchProductWorkItemTemp.Dispose();

            }
            catch (Exception ex1)
            {
                Console.WriteLine("排产计算出错！位置保存排程工单信息3031,出错内容：" + ex1.ToString());

                //Messagezk.Show("排产计算完成,t_SchProductWorkItemTemp写回数据库出错！位置SaveSchData(),出错内容：" + ex1.ToString(), Global.Tips, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            return 1;

        }


        //大数据量批量保存
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

                    //Response.Write("插入数据所用时间:" + stopwatch.Elapsed);//跑表结束，Elapsed是统计到的时间
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存3068,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }



            return 1;
        }


        //大数据量批量保存
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
                    //bulkCopy.ColumnMappings.Add("cSchStatus", "cSchStatus");
                    bulkCopy.ColumnMappings.Add("iPriority", "iPriority");
                    //bulkCopy.ColumnMappings.Add("iWOPriorityRes", "iWOPriorityRes");
                    bulkCopy.ColumnMappings.Add("iWoPriorityResLast", "iWoPriorityResLast");  //最新排产顺序                    
                    bulkCopy.ColumnMappings.Add("cInvCode", "cInvCode");
                    bulkCopy.ColumnMappings.Add("cSchSNType", "cSchSNType");
                    bulkCopy.ColumnMappings.Add("iSchSN", "iSchSN");


                    bulkCopy.WriteToServer(dt);//将数据源数据写入到目标表中

                    //Response.Write("插入数据所用时间:" + stopwatch.Elapsed);//跑表结束，Elapsed是统计到的时间
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("排产计算出错！位置大数据量批量保存3113,出错内容：" + ex.ToString());
                throw new Exception(ex.Message);
            }



            return 1;
        }

        

        //判断每批是否运算完成
        public static int PerBatchEnd()
        {
            int maxWorkerThreads, workerThreads;
            int portThreads;
            //while (true)
            //{
            //    /*
            //     GetAvailableThreads()：检索由 GetMaxThreads 返回的线程池线程的最大数目和当前活动数目之间的差值。
            //     而GetMaxThreads 检索可以同时处于活动状态的线程池请求的数目。
            //     通过最大数目减可用数目就可以得到当前活动线程的数目，如果为零，那就说明没有活动线程，说明所有线程运行完毕。
            //     */
            //    ThreadPool.GetMaxThreads(out maxWorkerThreads, out portThreads);
            //    ThreadPool.GetAvailableThreads(out workerThreads, out portThreads);
            //    if (maxWorkerThreads - workerThreads == 0)
            //    {
            //        break;

            //    }
            //}

            return 1;
        }



        //统计运算时间长度
        public string GetRunTime(string cType = "m")
        {
            // Console.WriteLine(ldtBegDate.ToString() + Global.CurDataTime.ToString());
            // string iTotal = Common.DateDiff(ldtBegDate, Global.CurDataTime, "all");
            //string sRunTime = "共耗时" + iTotal.ToString() + "  ";// +"分钟";

            //if (cType == "m")
            //{
            //    if (iTotal < 2)
            //    {
            //        iTotal = APS.Common.Common.DateDiff(ldtBegDate, Global.CurDataTime, "s");
            //        sRunTime = "共耗时" + iTotal.ToString() + "秒";

            //    }
            //}
            //else    //显示秒数
            //{
            //    iTotal = APS.Common.Common.DateDiff(ldtBegDate, Global.CurDataTime, "s");
            //    sRunTime = "共耗时" + iTotal.ToString() + "秒";            
            //}

            //return sRunTime;
            return "";
        }

        //统计运算时间长度
        public string GetcCalculateNo()
        {
            return this.schData.cCalculateNo;
        }

        //显示排产
        public void ShowSchProgress()
        {
            //iCurRows = 0;            // 排程当前任务数，用于统计当前进度 
            //iTotalRows = 100;        // 排程总任务数 

            int iLastRows = 0;
            if (schData.iTotalRows < 1) return;

            while (schData.iCurRows <= schData.iTotalRows)
            {

                if (iLastRows != schData.iCurRows)
                {

                    schData.iProgress = 30 + (int)schData.iCurRows * 50 / schData.iTotalRows;   //1、过程GetSchData  100%
                    //if (frmProgress.IsHandleCreated)      //创建了窗口句柄           
                    //    frmProgress.BeginInvoke(frmProgress.showProgressDelegate, new object[] { "5、排程计算进行中，当前记录/总记录数[" + schData.iCurRows + "/" + schData.iTotalRows + "] " + this.GetRunTime(), schData.iProgress });
                    if (this.dlShowProcess != null)
                        this.dlShowProcess(this.schData.iProgress.ToString(), "5、排程计算进行中，当前记录/总记录数[" + schData.iCurRows + "/" + schData.iTotalRows + "] ");

                    iLastRows = schData.iCurRows;
                }
                if (schData.iProgress < 80)
                    Thread.Sleep(1000);
            }

        }

        //// 显示进度条的委托声明
        //delegate void ShowProgressDelegate(int totalStep, int currentStep);

        //// 显示进度条
        //void ShowProgress(int totalStep, int currentStep)
        //{
        //    _Progress.Maximum = totalStep;
        //    _Progress.Value = currentStep;
        //}x

        //// 执行任务的委托声明
        //delegate void RunTaskDelegate(int seconds);

        //// 执行任务
        //void RunTask(int seconds)
        //{
        //    ShowProgressDelegate showProgress = new ShowProgressDelegate(ShowProgress);

        //    // 每 1 / 4 秒 显示进度一次
        //    for (int i = 0; i < seconds * 4; i++)
        //    {
        //        Thread.Sleep(250);

        //        // 显示进度条
        //        this.Invoke(showProgress, new object[] { seconds * 4, i + 1 });
        //    }
        //}


        #endregion

        //调用WS发工排程进度信息

        //返回系统参数
        public string GetParamValue(string param)
        {

            return "";
        }

        //发送消息
        public async Task SendAsync2(string message, string topic = "apsRun")
        {
            if (client == null)
            {
                client = new SocketIOClient.SocketIO(this.ServerUri, new SocketIOClient.SocketIOOptions
                {
                    ConnectionTimeout = TimeSpan.FromSeconds(2),
                    Transport = SocketIOClient.Transport.TransportProtocol.WebSocket,  //特别增加，否则连不上
                    //transports: ['websocket'],//
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

                // 设置连接超时（例如10秒）
                var cts = new System.Threading.CancellationTokenSource(10000);

                try
                {

                    //await client.ConnectAsync();
                    await client.ConnectAsync();
                    //await client.EmitAsync("apsRun", "Hello,APS正在运行 server!");

                    Console.WriteLine("message连接成功 " + this.ServerUri);
                }
                catch (OperationCanceledException)
                {
                    //Console.WriteLine("Connection attempt timed out.");
                    Console.WriteLine("message连接失败" + this.ServerUri);
                }
            }

            //发送消息
            try
            {
                if (client.Connected == false)
                    await client.ConnectAsync();
                //"schPrecent:" + schPrecent.ToString()

                //因为多账套,多用户同时使用,需要返回用户和公司
                message += ";User:" + this.User;
                message += ";Company:" + this.Company;
                message += ";socketId:" + this.socketId;

                //User = (string)input.User;           //用户
                //Company = (string)input.Company;     //公司

                //await client.EmitAsync("apsRun", "Hello,APS正在运行 server!");
                await client.EmitAsync(topic, message);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Connection attempt timed out.");
            }


            //Console.ReadLine();

            // await client.DisconnectAsync();
        }

        //通过http方式发请求到Nodejs后端
        public async Task SendAsync(string message, string topic = "apsRun")
        {
            //因为多账套,多用户同时使用,需要返回用户和公司
            message += ";User:" + this.User;
            message += ";Company:" + this.Company;
            message += ";socketId:" + this.socketId;
            message += ";topic:" + topic;

            Console.WriteLine("message连接成功 " + this.ServerUri +  " 内容：" + message);

          

            // 创建一个HttpClient实例
        
                    // Node.js后端API的URL（带查询参数）
                    //string message = "Hello from C#!";
                    string url = this.ServerUri + $"api/apsMessage?message={Uri.EscapeDataString(message)}"; // 将消息作为查询参数添加到URL中

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // 发送GET请求
                            HttpResponseMessage response = await client.GetAsync(url);

                            // 确保请求成功（状态码在200-299范围内）
                            response.EnsureSuccessStatusCode();

                            // 读取并输出响应内容
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