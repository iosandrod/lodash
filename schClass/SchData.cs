using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Runtime.Serialization;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace Algorithm
{

    //排程公用数据,每次排产前初始化数据
    [Serializable]
    public class SchData: ISerializable
    {
        public SchData()
        { 
        
        }
        //定义变量
        public string cVersionNo = "";
        public string cCalculateNo = "";  //排程运算号 用户名+ 时间

        public DateTime dtStart;        //     开始排程时间
        public DateTime dtEnd;          //     排程截止时间
        public DateTime dtToday;        //     当前时间，取数据库 


        public int iCurRows = 0;            // 排程当前任务数，用于统计当前进度 ,按工序行号为准
        public int iTotalRows = 100;        // 排程总任务数 
        public int iProgress = 0;           // 进度百分比
       // public ThreadTool iProgress = 0;           // 进度百分比
        

        //public static Logger logger = new Logger();           //日志记录对象       

        public DataTable dtSchProduct = new DataTable();           //排产产品
        public DataTable dtSchProductWorkItem = new DataTable();   //加工物料
        public DataTable dtResource = new DataTable();           //所有资源，有值    
        public DataTable dt_ResourceTime = new DataTable();      //资源正常工作日历
        public DataTable dt_ResourceSpecTime = new DataTable();      //资源特殊工作日历

        public string cSchCapType = "0";   // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能
   
        //public DataTable dtResResource = new DataTable();        //关键资源，按排产顺序排序 ，有值       
        public DataTable dtSchProductRoute = new DataTable();
        public DataTable dtSchProductRouteRes = new DataTable();
        public DataTable dtSchProductRouteItem = new DataTable();
        public DataTable dtSchProductRouteResTime = new DataTable();
        public DataTable dtSchResWorkTime = new DataTable();         //合并后资源日历时间段，排程完成后，写入t_SchResWorkTime

        public DataTable dtSchProductTemp = new DataTable();         //排产结果写回
        public DataTable dtSchProductRouteTemp = new DataTable();
        public DataTable dtSchProductRouteResTemp = new DataTable();

        public DataTable dtProChaType = new DataTable();        //工艺特征类型，有值
        public DataTable dtResChaValue = new DataTable();       //工艺特征值，有值
        public DataTable dtResChaCrossTime = new DataTable();   //工艺特征转换时间 ,有值
        
        //2016-10-21
        public DataTable dtWorkCenter = new DataTable();        //工作中心
        public DataTable dtDepartment = new DataTable();        //部门
        public DataTable dtTeam = new DataTable();              //班组
        public DataTable dtPerson = new DataTable();            //员工

        public DataTable dtItem = new DataTable();              // 物料档案  用于扩展
        public DataTable dtTechInfo = new DataTable();          // 工艺档案 用于扩展

        public DataTable dtTechLearnCurves = new DataTable();          //资源任务学习曲线       2017-11-23
        public DataTable dtResTechScheduSN = new DataTable();          //资源工艺特征排产顺序

        //所有资源列表 dtResource
        public List<Resource> ResourceList = new List<Resource>(10);

        //关键资源列表
        public List<Resource> KeyResourceList = new List<Resource>(10);

        //资源组列表
        public List<Resource> TeamResourceList = new List<Resource>(10);

        //再建立他们的联系
        //排程订单产品列表 dtSchProduct
        public List<SchProduct> SchProductList = new List<SchProduct>(10);

        //加工物料信息表
        public List<SchProductWorkItem> SchProductWorkItemList = new List<SchProductWorkItem>(10);
        

        //排程订单产品工艺模型 dtSchProductRoute
        public List<SchProductRoute> SchProductRouteList = new List<SchProductRoute>(10);

        //排程订单产品子料
        public List<SchProductRouteItem> SchProductRouteItemList = new List<SchProductRouteItem>(10);

        //工序对应的可用资源列表  dtSchProductRouteRes 
        public List<SchProductRouteRes> SchProductRouteResList = new List<SchProductRouteRes>(10);

        //任务占用资源时间段 t_SchProductRouteResTime
        public List<TaskTimeRange> TaskTimeRangeList = new List<TaskTimeRange>(10);

        //所有工作中心列表 dtWorkCenter
        public List<WorkCenter> WorkCenterList = new List<WorkCenter>(10);

        //所有加工物料档案列表 dtItem
        public List<Item> ItemList = new List<Item>(10);

        //所有工艺档案列表 dtTechInfo
        public List<TechInfo> TechInfoList = new List<TechInfo>(10);

        public string CVersionNo { get => cVersionNo; set => cVersionNo = value; }

       

        //排产算法扩展
        //[System.Xml.Serialization.XmlIgnore]
        ////[System.Web.Script.Serialization.ScriptIgnore]
        //[System.Runtime.Serialization.IgnoreDataMember]
        //public APSCustomer.AlgorithmExtend algorithmExtend ;


        //公用方法
        //1、两个日期对比，返回时间差,考虑资源工作日期，节假日跳过
        public static int GetDateDiff(Resource resource, string DatePart, DateTime dt1, DateTime dt2)
        { 
            Int32 liReturn = 0  ;

            if (resource == null || resource.cResourceNo == "") return 1;

                            
            DataRow[] drDaySelect ;
            drDaySelect = resource.schData.dt_ResourceTime.Select("cResourceNo = '" + resource.cResourceNo + "' and dPeriodBegTime >= '" + dt1.ToString() + "' and dPeriodBegTime <= '" + dt2.ToString() + "'");

            drDaySelect = drDaySelect.OrderBy(x => x["dPeriodDay"]).ToArray();

            DateTime dtDayTemp;


            //返回天
            if (DatePart == "d")
            {
                if (drDaySelect.Length > 0)
                {
                    dtDayTemp = (DateTime)drDaySelect[0]["dPeriodDay"];
                    liReturn = 1;

                    //foreach (DataRow item in drDaySelect)
                   
                    for (int i = 0; i < drDaySelect.Length; i++ )
                    {
                        if ((DateTime)drDaySelect[i]["dPeriodDay"] == dtDayTemp)
                            continue;
                        else
                        {
                            liReturn++;
                            dtDayTemp = (DateTime)drDaySelect[i]["dPeriodDay"];
                        }

                    }
                    
                }
            }
            

            return liReturn;
        }


        //公用方法
        //2、两个日期对比，返回时间差
        public static int GetDateDiff(string DatePart, DateTime dt1, DateTime dt2)
        {
            Int32 liReturn = 0;

            //被减数
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);

            //减数
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);

            TimeSpan ts = ts2.Subtract(ts1);//Duration()


            //返回秒
            if (DatePart == "s")
            {
                // liReturn = Int32.Parse(ts.TotalSeconds.ToString());
                liReturn = Convert.ToInt32((ts.TotalSeconds));
            } //返回小时
            //返回分钟
            if (DatePart == "m")
            {
                //liReturn = Int64.Parse(ts.TotalMinutes.ToString() );
                liReturn = Convert.ToInt32((ts.TotalMinutes));
            } //返回小时
            else if (DatePart == "h")
            {
                //liReturn = ts.Days * 24 + ts.Hours;
                //liReturn = Int32.Parse(ts.TotalHours.ToString());
                liReturn = Convert.ToInt32((ts.TotalHours));
            } //返回天
            else if (DatePart == "d")
            {
                //liReturn = Int32.Parse(ts.TotalDays.ToString());
                liReturn = Convert.ToInt32((ts.TotalDays));
            }


            return liReturn;
        }

        public static string GetDateDiffString(System.DateTime Date1, System.DateTime Date2, string Interval)
        {
            double dblYearLen = 365;//年的长度，365天   
            double dblMonthLen = (365 / 12);//每个月平均的天数   
            System.TimeSpan objT;
            objT = Date2.Subtract(Date1);


            switch (Interval)
            {
                case "y"://返回日期的年份间隔   
                    return System.Convert.ToInt32(objT.Days / dblYearLen).ToString();
                case "M"://返回日期的月份间隔   
                    return System.Convert.ToInt32(objT.Days / dblMonthLen).ToString();
                case "d"://返回日期的天数间隔   
                    return objT.TotalDays.ToString();
                case "h"://返回日期的小时间隔   
                    return objT.TotalHours.ToString();
                case "m"://返回日期的分钟间隔   
                    return objT.TotalMinutes.ToString();
                case "s"://返回日期的秒钟间隔   
                    return objT.TotalSeconds.ToString();
                case "ms"://返回时间的微秒间隔   
                    return objT.TotalMilliseconds.ToString();
                case "all":
                    return objT.ToString();
                default:
                    break;
            }
            return "0";
        }

        //3、增加日期
        public static DateTime AddDate(string DatePart, Int32 iAdd, DateTime dt1)
        {
            DateTime dtNew = dt1;

              //返回秒
            if (DatePart == "s")
            {
                dtNew = dt1.AddSeconds(iAdd);
            } //返回小时
            //返回分钟
            if (DatePart == "m")
            {
                dtNew = dt1.AddMinutes(iAdd);
            } //返回小时
            else if (DatePart == "h")
            {
                //liReturn = ts.Days * 24 + ts.Hours;
                dtNew = dt1.AddHours(iAdd);
            } //返回天
            else if (DatePart == "d")
            {
                dtNew = dt1.AddDays(iAdd);
            }
            else if (DatePart == "m")
            {
                dtNew = dt1.AddMonths(iAdd);
            }

            return dtNew;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //((ISerializable)SchProductList).GetObjectData(info, context);

            ((ISerializable)SchProductRouteList).GetObjectData(info, context);

            ((ISerializable)SchProductRouteResList).GetObjectData(info, context);

            ((ISerializable)TaskTimeRangeList).GetObjectData(info, context);
            
        }

    }
}
