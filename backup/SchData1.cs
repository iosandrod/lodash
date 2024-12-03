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
    [Serializable]
    public class SchData: ISerializable
    {
        public SchData()
        { 
        }
        public string cVersionNo = "";
        public string cCalculateNo = "";  //排程运算号 用户名+ 时间
        public DateTime dtStart;        //     开始排程时间
        public DateTime dtEnd;          //     排程截止时间
        public DateTime dtToday;        //     当前时间，取数据库 
        public int iCurRows = 0;            // 排程当前任务数，用于统计当前进度 ,按工序行号为准
        public int iTotalRows = 100;        // 排程总任务数 
        public int iProgress = 0;           // 进度百分比
        public DataTable dtSchProduct = new DataTable();           //排产产品
        public DataTable dtSchProductWorkItem = new DataTable();   //加工物料
        public DataTable dtResource = new DataTable();           //所有资源，有值    
        public DataTable dt_ResourceTime = new DataTable();      //资源正常工作日历
        public DataTable dt_ResourceSpecTime = new DataTable();      //资源特殊工作日历
        public string cSchCapType = "0";   // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能
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
        public DataTable dtWorkCenter = new DataTable();        //工作中心
        public DataTable dtDepartment = new DataTable();        //部门
        public DataTable dtTeam = new DataTable();              //班组
        public DataTable dtPerson = new DataTable();            //员工
        public DataTable dtItem = new DataTable();              // 物料档案  用于扩展
        public DataTable dtTechInfo = new DataTable();          // 工艺档案 用于扩展
        public DataTable dtTechLearnCurves = new DataTable();          //资源任务学习曲线       2017-11-23
        public DataTable dtResTechScheduSN = new DataTable();          //资源工艺特征排产顺序
        public List<Resource> ResourceList = new List<Resource>(10);
        public List<Resource> KeyResourceList = new List<Resource>(10);
        public List<Resource> TeamResourceList = new List<Resource>(10);
        public List<SchProduct> SchProductList = new List<SchProduct>(10);
        public List<SchProductWorkItem> SchProductWorkItemList = new List<SchProductWorkItem>(10);
        public List<SchProductRoute> SchProductRouteList = new List<SchProductRoute>(10);
        public List<SchProductRouteItem> SchProductRouteItemList = new List<SchProductRouteItem>(10);
        public List<SchProductRouteRes> SchProductRouteResList = new List<SchProductRouteRes>(10);
        public List<TaskTimeRange> TaskTimeRangeList = new List<TaskTimeRange>(10);
        public List<WorkCenter> WorkCenterList = new List<WorkCenter>(10);
        public List<Item> ItemList = new List<Item>(10);
        public List<TechInfo> TechInfoList = new List<TechInfo>(10);
        public string CVersionNo { get => cVersionNo; set => cVersionNo = value; }
        public static int GetDateDiff(Resource resource, string DatePart, DateTime dt1, DateTime dt2)
        { 
            Int32 liReturn = 0  ;
            if (resource == null || resource.cResourceNo == "") return 1;
            DataRow[] drDaySelect ;
            drDaySelect = resource.schData.dt_ResourceTime.Select("cResourceNo = '" + resource.cResourceNo + "' and dPeriodBegTime >= '" + dt1.ToString() + "' and dPeriodBegTime <= '" + dt2.ToString() + "'");
            drDaySelect = drDaySelect.OrderBy(x => x["dPeriodDay"]).ToArray();
            DateTime dtDayTemp;
            if (DatePart == "d")
            {
                if (drDaySelect.Length > 0)
                {
                    dtDayTemp = (DateTime)drDaySelect[0]["dPeriodDay"];
                    liReturn = 1;
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
        public static int GetDateDiff(string DatePart, DateTime dt1, DateTime dt2)
        {
            Int32 liReturn = 0;
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            TimeSpan ts = ts2.Subtract(ts1);//Duration()
            if (DatePart == "s")
            {
                liReturn = Convert.ToInt32((ts.TotalSeconds));
            } //返回小时
            if (DatePart == "m")
            {
                liReturn = Convert.ToInt32((ts.TotalMinutes));
            } //返回小时
            else if (DatePart == "h")
            {
                liReturn = Convert.ToInt32((ts.TotalHours));
            } //返回天
            else if (DatePart == "d")
            {
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
        public static DateTime AddDate(string DatePart, Int32 iAdd, DateTime dt1)
        {
            DateTime dtNew = dt1;
            if (DatePart == "s")
            {
                dtNew = dt1.AddSeconds(iAdd);
            } //返回小时
            if (DatePart == "m")
            {
                dtNew = dt1.AddMinutes(iAdd);
            } //返回小时
            else if (DatePart == "h")
            {
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
            ((ISerializable)SchProductRouteList).GetObjectData(info, context);
            ((ISerializable)SchProductRouteResList).GetObjectData(info, context);
            ((ISerializable)TaskTimeRangeList).GetObjectData(info, context);
        }
    }
}