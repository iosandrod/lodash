using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Linq;
using System.Linq.Expressions;
namespace Algorithm
{
    [Serializable]
    public class Resource: ISerializable
    {
        #region //Resource属性定义
        public SchData schData = null;        //所有排程数据
        public int iResourceID { get; set; }               //资源物料排产顺序
        public string cResourceNo { get; set; }            //资源编号
        public string cResourceName { get; set; }
        public string cResClsNo { get; set; }             //资源类别 
        public string cResourceType { get; set; }         //0 主资源 1 辅资源
        private int   IResourceNumber;          //资源数量
        public string cResOccupyType { get; set; }       //0 整体 1 单人单台,单件工时/资源数量
        public double iPreStocks { get; set; }            //每批换产时间(分)
        public double iPostStocks { get; set; }           //每批维修时间(分) 
        public double iUsage { get; set; }              //资源使用率
        private double IEfficient;            //资源效率 不能为0 
        public string cResouceInformation { get; set; }    //资源简称
        public string cIsInfinityAbility { get; set; }     //0 产能有限 1 产能无限 
        public int bScheduled = 0;            //任务是否已排产 0 未排，1 已排
        public int iOverResourceNumber { get; set; }     //加班资源数    对应排程产能方案为加班
        public int iLimitResourceNumber { get; set; }     //极限资源数   对应排程产能方案为极限
        public double iOverEfficient { get; set; }        //加班效率     对应排程产能方案为加班
        public double iLimitEfficient { get; set; }       //极限效率     对应排程产能方案为极限
        public double iResDifficulty { get; set; }        //资源加工难度系数     资源组时,可以设置产能区分
        public double iDistributionRate { get; set; }     //资源选择比例     资源组时,可以设置资源选择比例，比如本厂必选，100，委外厂商10%，20%等，对于已下达的任务，不能重新选择
        public string cWcNo { get; set; }                 //工作中心
        private WorkCenter resWorkCenter = null;    //工作中心对象  //返回当前资源对应的工作中心   
        public WorkCenter ResWorkCenter
        {
            get {                              
                if (this.cWcNo == "") this.resWorkCenter = null;
                List<WorkCenter> ListWorkCenter = this.schData.WorkCenterList.FindAll(delegate(WorkCenter p1) { return p1.cWcNo == this.cWcNo; });
                if (ListWorkCenter.Count > 0)
                {
                    this.resWorkCenter = ListWorkCenter[0];                
                }
                else
                    this.resWorkCenter = null;
                return this.resWorkCenter;
            }
        }
        public int iResourceNumber
         {
             get {
                 if (SchParam.cSchCapType == "1")  ///加班资源数
                 {
                     if (this.IResourceNumber > this.iOverResourceNumber)
                         return this.IResourceNumber;
                     else
                         return this.iOverResourceNumber;
                 }
                 else if (SchParam.cSchCapType == "2")  ///极限资源数
                 {
                     if (this.IResourceNumber > this.iLimitResourceNumber)
                         return this.IResourceNumber;
                     else
                         return this.iLimitResourceNumber;
                 }
                 else
                     return this.IResourceNumber;
             }
             set { IResourceNumber = value; }
        }
        public  double iEfficient            //资源效率 不能为0 
        {
            get
            {
                if (this.IEfficient <= 0) this.IEfficient = 100;
                if (SchParam.cSchCapType == "1")  ///加班资源数
                {
                    if (this.IEfficient > this.iOverEfficient)
                        return this.IEfficient;
                    else
                        return this.iOverEfficient;
                }
                else if (SchParam.cSchCapType == "2")  ///极限资源数
                {
                    if (this.IEfficient > this.iLimitEfficient)
                        return this.IEfficient;
                    else
                        return this.iLimitEfficient;
                }
                else
                    return this.IEfficient;
            }
            set { IEfficient = value; }
        }
        public string cDeptNo { get; set; }               //生产部门
        public string cDeptName { get; set; }
        public string cStatus { get; set; }                //资源状态
        public int iSchemeID { get; set; }                 //日历ID
        public double iCacheTime { get; set; }                //资源缓冲时间（分钟）
        public double iLastBatchPercent { get; set; }       //最后一批分批百分比，不超过，则作为一批处理
        public string cIsKey { get; set; }                 //关键资源
        public int iKeySchSN { get; set; }                 //关键排产顺序
        public string cNeedChanged { get; set; }           //维修单无换产时间
        public DateTime dMaxExeDate { get; set; }         //最大可排时间,最当前最大开工工序完工时间
        public string cProChaType1Sort { get; set; }      //关键资源是否 1 按资源物料排产排序，进行优化生产，0 否则按工艺特征排序进行优化
        public string cDayPlanShowType { get; set; }       //关联分组号，分组号相同或包含的，前后工序间关联分组号相同，资源才可以选择参与排产。2022-11-2
        public int iChangeTime { get; set; }            //换料时间(秒)
        public int iResPreTime { get; set; }            //前准备时间(秒),生成产品工艺模型时，如果工艺路线没有定义，取资源档案
        public string iTurnsType { get; set; }            //轮换类型 0 不轮换 1 按加工时间轮换 2 按任务数轮换
        public int iTurnsTime { get; set; }              //轮换时间/任务数(分)
        public string cTeamNo { get; set; }              //班组1
        public string cTeamNo2 { get; set; }              //班组2
        public string cTeamNo3 { get; set; }              //班组3 
        public string cBatch1Filter { get; set; }         //批次1过滤    
        public string cBatch2Filter{ get; set; }         //批次2过滤
        public string cBatch3Filter { get; set; }        //批次3过滤
        public string cBatch4Filter { get; set; }        //批次4过滤
        public string cBatch5Filter { get; set; }        //批次5过滤
        public int iBatchWoSeqID { get; set; }         //批次工序号
        public int cBatch1WorkTime { get; set; }         //批次1加工时间    
        public int cBatch2WorkTime { get; set; }         //批次2加工时间
        public int cBatch3WorkTime { get; set; }        //批次3加工时间
        public int cBatch4WorkTime { get; set; }        //批次4加工时间
        public int cBatch5WorkTime { get; set; }        //批次5加工时间
        public string cPriorityType { get; set; }        //排产优先级
        public string cResBarCode { get; set; }          //资源条码号
        public string cTeamResourceNo { get; set; }          //资源组编码
        public string bTeamResource { get; set; }             //是否资源组 0 设备 1 资源组
        public string cSuppliyMode { get; set; }              //供料方式 0 独自供料 1 资源组集中供料    
        public string cResOperator { get; set; }              //资源操作员  
        public string cResManager { get; set; }              //资源管理员
        public List<Resource> TeamResourceList = new List<Resource>(10);   //资源组列表
        public Resource TeamResource { get; set; }            //资源组
        public string bAllocated = "0";          //"1"资源组在同一个任务中已分配 , "0" 未分配
        public int iResWorkersPd { get; set; }        //资源日排产工人数
        public double iResHoursPd { get; set; }          //资源日排产工时
        public double iResOverHoursPd { get; set; }          //资源日加班工时
        public double iLabRate { get; set; }              //人工费率
        public double iPowerRate { get; set; }              //电费费率
        public double iOtherRate { get; set; }              //其他费率
        public double iMinWorkTime { get; set; }            //最小加工分拆时间，大于此时间时，拆分多个资源生产
        public string FProChaType1ID { get; set; }           //资源工艺特征类型1   
        public string FProChaType2ID { get; set; }            //资源工艺特征类型2   
        public string FProChaType3ID { get; set; }
        public string FProChaType4ID { get; set; }
        public string FProChaType5ID { get; set; }
        public string FProChaType6ID { get; set; }
        public string cDefine1{ get; set; }
        public string cDefine2{ get; set; }
        public string cDefine3{ get; set; }
        public string cDefine4{ get; set; }
        public string cDefine5{ get; set; }
        public string cDefine6{ get; set; }
        public string cDefine7{ get; set; }
        public string cDefine8{ get; set; }
        public string cDefine9{ get; set; }
        public string cDefine10{ get; set; }
        public double cDefine11{ get; set; }
        public double cDefine12{ get; set; }
        public double cDefine13{ get; set; }
        public double cDefine14{ get; set; }
        public DateTime cDefine15{ get; set; }
        public DateTime cDefine16{ get; set; }
        public int iBatch = 2;               //当前批量生产批号,从2开始,当前正在加工的为1批
        public DateTime dBatchBegDate { get; set; }       //批量加工开始时间   
        public DateTime dBatchEndDate { get; set; }       //批量加工完工时间，下批排产时只能从此时间往后排
        public string cTimeNote = "";        //资源换产时间说明
        public int iSchBatch = 6;      //排产批次,批次换时，重新写值
        public int cSelected = 1;      //是否选择参与排产 1 ,0 不参与优化排产 (系统参数，只排选择资源 1，所有资源都参与排产，优先排选择资源 )
        public double iSchHours = 0;          //已排任务工时
        public double iPlanDays = 0;      //计划天数 = 已排任务工时/日排产工时,用于资源选择，越小越优先
        #endregion
        #region//资源初始化
        public Resource()
        {
        }
        public Resource(DataRow drResource)
        {
            GetResource(drResource);
        }
        public Resource(string cResourceNo, SchData as_SchData)
        {
            this.schData = as_SchData;
            DataRow[] dr = schData.dtResource.Select("cResourceNo = '" + cResourceNo + "'");
            if (dr.Length < 1) return;
            GetResource(dr[0]);
        }
        public void GetResource(DataRow drResource)
        {
            iResourceID = (int)drResource["iResourceID"];                //资源物料排产顺序
            cResourceNo = drResource["cResourceNo"].ToString();          //资源编号
            cResourceName = drResource["cResourceName"].ToString();
            cResClsNo = drResource["cResClsNo"].ToString();             //资源类别
            cResourceType = drResource["cResourceType"].ToString();
            iResourceNumber = (int)drResource["iResourceNumber"];
            cResOccupyType = drResource["cResOccupyType"].ToString();
            iPreStocks =  Convert.ToDouble(drResource["iPreStocks"]);                //每批换产时间(分)
            iPostStocks =  Convert.ToDouble(drResource["iPostStocks"]);              //每批维修时间(分)
            iUsage = Convert.ToDouble(drResource["iUsage"]);                //资源使用率
            iEfficient = Convert.ToDouble(drResource["iEfficient"]);            //资源效率
            iResDifficulty = Convert.ToDouble(drResource["iResDifficulty"]);            //资源加工难度系数
            iDistributionRate = Convert.ToDouble(drResource["iDistributionRate"]);      //资源选择比例     资源组时,可以设置资源选择比例，比如本厂必选，100，委外厂商10%，20%等，对于已下达的任务，不能重新选择
            if (iEfficient == 0) iEfficient = 100;
            cIsInfinityAbility = drResource["cIsInfinityAbility"].ToString();
            if (cIsInfinityAbility == "") cIsInfinityAbility = "0";              //产能是否无限 0 否 1 是
            cWcNo = drResource["cWcNo"].ToString();                 //工作中心
            cDeptNo = drResource["cDeptNo"].ToString();               //生产部门
            cDeptName = drResource["cDeptName"].ToString();
            cStatus = drResource["cStatus"].ToString();                //资源状态
            iSchemeID = (int)drResource["iResourceNumber"];                 //日历ID
            iCacheTime = Convert.ToDouble(drResource["iCacheTime"]);                //资源缓冲时间（分钟）
            iLastBatchPercent = Convert.ToDouble(drResource["iLastBatchPercent"]);       //最后一批分批百分比，不超过，则作为一批处理
            cIsKey = drResource["cIsKey"].ToString();                 //关键资源
            iKeySchSN = (int)drResource["iKeySchSN"];                 //关键资源排产顺序
            cNeedChanged = drResource["cNeedChanged"].ToString();           //需要轮换，后面有机台机器生产，轮流生产一个批量（后续资源缓冲时间）
            iChangeTime = Convert.ToInt32(drResource["iChangeTime"]) * 60;     //换料时间（分钟）
            iResPreTime = Convert.ToInt32(drResource["iResPreTime"]) * 60;     //前准备时间（分钟）
            iTurnsType = drResource["iTurnsType"].ToString();                //轮换类型
            iTurnsTime = Convert.ToInt32(drResource["iTurnsTime"]) * 60;       //轮换加工时间/数量 （分钟）
            iLabRate = Convert.ToDouble(drResource["iLastBatchPercent"]);      //人工费
            cTeamNo = drResource["cTeamNo"].ToString();       //班组1
            cTeamNo2 = drResource["cTeamNo2"].ToString();     //班组2
            cTeamNo3 = drResource["cTeamNo3"].ToString();     //班组3
            cBatch1Filter = drResource["cBatch1Filter"].ToString();       //批次1过滤
            cBatch2Filter = drResource["cBatch2Filter"].ToString();       //批次2过滤
            cBatch3Filter = drResource["cBatch3Filter"].ToString();       //批次3过滤
            cBatch4Filter = drResource["cBatch4Filter"].ToString();       //批次4过滤
            cBatch5Filter = drResource["cBatch5Filter"].ToString();       //批次5过滤
            iBatchWoSeqID = Convert.ToInt32(drResource["iBatchWoSeqID"]); //批次工序号
            cBatch1WorkTime = Convert.ToInt32(drResource["cBatch1WorkTime"]); //批次1加工时间
            cBatch2WorkTime = Convert.ToInt32(drResource["cBatch2WorkTime"]); //批次2加工时间
            cBatch3WorkTime = Convert.ToInt32(drResource["cBatch3WorkTime"]); //批次3加工时间
            cBatch4WorkTime = Convert.ToInt32(drResource["cBatch4WorkTime"]); //批次4加工时间
            cBatch5WorkTime = Convert.ToInt32(drResource["cBatch5WorkTime"]); //批次5加工时间
            cPriorityType = drResource["cPriorityType"].ToString();       //排产优先级
            cResBarCode = drResource["cResBarCode"].ToString();           //资源条码号
            cTeamResourceNo = drResource["cTeamResourceNo"].ToString();         //资源组编码
            bTeamResource = drResource["bTeamResource"].ToString();             //是否资源组 0 设备 1 资源组
            cSuppliyMode = drResource["cSuppliyMode"].ToString();               //供料方式 0 独自供料 1 资源组集中供料    
            cResOperator = drResource["cResOperator"].ToString();               //资源操作员  
            cResManager = drResource["cResManager"].ToString();                 //资源管理员
            iOverResourceNumber = Convert.ToInt32(drResource["iOverResourceNumber"]);      //加班资源数
            iLimitResourceNumber = Convert.ToInt32(drResource["iLimitResourceNumber"]);     //极限资源数
            iOverEfficient = Convert.ToDouble(drResource["iOverEfficient"]);                //加班效率
            iLimitEfficient = Convert.ToDouble(drResource["iLimitEfficient"]);              //极限效率
            if (iOverEfficient == 0) iOverEfficient = 100;
            if (iLimitEfficient == 0) iLimitEfficient = 100;
            iResWorkersPd = Convert.ToInt32(drResource["iResWorkersPd"]);           //日排产人数
            iResHoursPd = Convert.ToDouble(drResource["iResHoursPd"]);                //日排产工时
            iResOverHoursPd = Convert.ToDouble(drResource["iResOverHoursPd"]);                //日加班工时
            iPowerRate = Convert.ToDouble(drResource["iPowerRate"]);                //电费费率
            iOtherRate = Convert.ToDouble(drResource["iOtherRate"]);                //其他费率
            iMinWorkTime = Convert.ToDouble(drResource["iMinWorkTime"]);            //资源任务最小分拆时间(分)，大于此时间时，拆分多个资源生产，为0时不考虑。系统参数也有最小分拆时间
            if (iMinWorkTime < 1) iMinWorkTime = SchParam.iTaskMinWorkTime;          //取资源最小排产时间 
            cProChaType1Sort = drResource["cProChaType1Sort"].ToString();       //是否按工艺特征1排序，进行优化生产。
            FProChaType1ID = drResource["FProChaType1ID"].ToString();            //资源工艺特征类型1   
            FProChaType2ID = drResource["FProChaType2ID"].ToString();            //资源工艺特征类型2   
            FProChaType3ID = drResource["FProChaType3ID"].ToString();
            FProChaType4ID = drResource["FProChaType4ID"].ToString();
            FProChaType5ID = drResource["FProChaType5ID"].ToString();
            FProChaType6ID = drResource["FProChaType6ID"].ToString();
            cDefine1 = drResource["cResDefine1"].ToString();
            cDefine2 = drResource["cResDefine2"].ToString();
            cDefine3 = drResource["cResDefine3"].ToString();
            cDefine4 = drResource["cResDefine4"].ToString();
            cDefine5 = drResource["cResDefine5"].ToString();
            cDefine6 = drResource["cResDefine6"].ToString();
            cDefine7 = drResource["cResDefine7"].ToString();
            cDefine8 = drResource["cResDefine8"].ToString();
            cDefine9 = drResource["cResDefine9"].ToString();
            cDefine10 = drResource["cResDefine10"].ToString();
            cDefine11 = (double)drResource["cResDefine11"];
            cDefine12 = (double)drResource["cResDefine12"];
            cDefine13 = (double)drResource["cResDefine13"];
            cDefine14 = (double)drResource["cResDefine14"];
            cDefine15 = (DateTime)drResource["cResDefine15"];
            cDefine16 = (DateTime)drResource["cResDefine16"];
            cDayPlanShowType = drResource["cDayPlanShowType"].ToString();
            dMaxExeDate = (DateTime)drResource["dMaxExeDate"];
        }
        #endregion
        public List<ResTimeRange> ResTimeRangeList = new List<ResTimeRange>(10);
        public List<ResSourceDayCap> ResSourceDayCapList = new List<ResSourceDayCap>(10);
        public List<ResTimeRange> ResTimeRangeListBak = new List<ResTimeRange>(10);
        public List<ResTimeRange> ResSpecTimeRangeList = new List<ResTimeRange>(10);
        public List<SchProductRouteRes> schProductRouteResList = new List<SchProductRouteRes>(10);
        public List<TaskTimeRange> GetTaskTimeRangeList(Boolean OrderASC = true)  //DateTime dBegDate
        {
            List<TaskTimeRange> ListTaskTimeRangeAll = new List<TaskTimeRange>(10);
            foreach (ResTimeRange ResTimeRange1 in this.ResTimeRangeList)
            {
                ListTaskTimeRangeAll.AddRange(ResTimeRange1.WorkTimeRangeList);
            }
            if (OrderASC)   //升序
            {
                ListTaskTimeRangeAll.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            }
            else             //降序
            {
                ListTaskTimeRangeAll.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
            }
            return ListTaskTimeRangeAll;
        }
        public List<TaskTimeRange> GetTaskTimeRangeList(DateTime dBegDate, Boolean bSchRev = false, Boolean OrderASC = true)
        {
            List<TaskTimeRange> ListTaskTimeRangeAll = new List<TaskTimeRange>(10);
            if (bSchRev == false)  //正排,时段结束时间>= dBegDate
            {
                foreach (ResTimeRange ResTimeRange1 in this.ResTimeRangeList.FindAll(delegate (ResTimeRange p1) { return (p1.DEndTime >= dBegDate); }))
                {
                    if (ResTimeRange1.DEndTime >= dBegDate)
                    {
                        if (ResTimeRange1.WorkTimeRangeList.Count > 0)
                        {
                            ListTaskTimeRangeAll.AddRange(ResTimeRange1.WorkTimeRangeList);
                            break;
                        }
                    }
                }
            }
            else                   //倒排 时段开始时间<= dBegDate
            {
                foreach (ResTimeRange ResTimeRange1 in this.ResTimeRangeList.FindAll(delegate(ResTimeRange p1) { return (p1.DBegTime <= dBegDate); }))
                {
                    if (ResTimeRange1.DBegTime <= dBegDate)
                    {
                        if (ResTimeRange1.WorkTimeRangeList.Count > 0)
                        {
                            ListTaskTimeRangeAll.AddRange(ResTimeRange1.WorkTimeRangeList);
                            break;
                        }
                    }
                }
            }
            if (OrderASC)   //升序
            {
                ListTaskTimeRangeAll.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            }
            else             //降序
            {
                ListTaskTimeRangeAll.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
            }
            return ListTaskTimeRangeAll;
        }
        public List<SchProductRouteRes> GetNotSchTask()
        {
            schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.BScheduled == 0 && p1.iSchBatch == this.iSchBatch ); });
            schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
            return schProductRouteResList;
        }
        #region//资源时段初始化处理
        public void MergeTimeRange()
        {
            List<ResTimeRange> ResSpecTimeRangeList1 = this.ResSpecTimeRangeList.FindAll(delegate(ResTimeRange p1) { return p1.Attribute == TimeRangeAttribute.Work || p1.Attribute == TimeRangeAttribute.Overtime || p1.Attribute == TimeRangeAttribute.MayOvertime; });
            ResSpecTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            DateTime dCanBegDate = this.schData.dtStart;
            DateTime dCanEndDate = this.schData.dtEnd;
            foreach (ResTimeRange resTimeRange in ResSpecTimeRangeList1)
            {
                dCanBegDate = this.schData.dtStart;
                dCanEndDate = this.schData.dtEnd;
                List<ResTimeRange> lResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime <= resTimeRange.DBegTime; });
                lResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
                if (lResTimeRangeList1.Count > 0)
                    dCanBegDate = lResTimeRangeList1[0].DBegTime;  //由大到小排,取小于特殊时段开始日期最大的一个时间段
                List<ResTimeRange> lResTimeRangeList = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime >= dCanBegDate; });
                lResTimeRangeList.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                ResTimeRange resLastTimeRange = null;  //上一时段
                int iCount = lResTimeRangeList.Count;
                for (int i = 0; i < iCount; i++)
                {
                    ResTimeRange resWorkTimeRange = lResTimeRangeList[i];
                    if (i > 0) //找结束时间段,从第二段起，每段都生成一个空时间段
                    {
                        ResTimeRange resAddTimeRange = new ResTimeRange(); //
                        if (resTimeRange.DBegTime < resLastTimeRange.DEndTime)
                            resAddTimeRange.DBegTime = resLastTimeRange.DEndTime;  //新增加时段的开始时间
                        else
                            resAddTimeRange.DBegTime = resTimeRange.DBegTime;
                        if (resTimeRange.DEndTime > resWorkTimeRange.DBegTime)
                        {
                            resAddTimeRange.DEndTime = resWorkTimeRange.DBegTime;
                        }
                        else if (resTimeRange.DEndTime > resAddTimeRange.DBegTime)
                        {
                            resAddTimeRange.DEndTime = resTimeRange.DEndTime;
                        }
                        else
                        {
                            break;
                        }
                        resAddTimeRange.CResourceNo = cResourceNo;
                        resAddTimeRange.resource = this;
                        resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility;
                        resAddTimeRange.Attribute = resTimeRange.Attribute;
                        resAddTimeRange.GetNoWorkTaskTimeRange(resAddTimeRange.DBegTime, resAddTimeRange.DEndTime, true);
                        this.ResTimeRangeList.Add(resAddTimeRange);
                    }
                    resLastTimeRange = resWorkTimeRange;
                }
            }
            List<ResTimeRange> ResSpecTimeRangeList2 = this.ResSpecTimeRangeList.FindAll(delegate(ResTimeRange p1) { return p1.Attribute == TimeRangeAttribute.Maintain || p1.Attribute == TimeRangeAttribute.Snag; });
            ResSpecTimeRangeList2.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            foreach (ResTimeRange resTimeRange in ResSpecTimeRangeList2)
            {
                List<ResTimeRange> lResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime <= resTimeRange.DBegTime; });
                lResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
                if (lResTimeRangeList1.Count > 0)
                    dCanBegDate = lResTimeRangeList1[0].DBegTime;  //由大到小排,取小于特殊时段开始日期最大的一个时间段
                else
                    dCanBegDate = this.schData.dtStart;
                List<ResTimeRange> lResTimeRangeList = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime >= dCanBegDate; }); //&& p2.DEndTime <= dCanEndDate
                lResTimeRangeList.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                ResTimeRange resLastTimeRange = null;  //上一时段
                int iCount = lResTimeRangeList.Count;
                for (int i = 0; i < iCount; i++)
                {
                    ResTimeRange resWorkTimeRange = lResTimeRangeList[i];
                    if (resTimeRange.DEndTime >= resWorkTimeRange.DBegTime)
                    {
                        DeleteTimeRangeSub(resWorkTimeRange, resTimeRange);
                    }
                    else       //特殊处理超过时段结束时间，退出循环
                    {
                        break;
                    }
                }
            }
        }
        public void MergeTimeRangeSub(ResTimeRange resWorkTimeRange, ResTimeRange resSpecTimeRange)
        {
            ResTimeRange resAddTimeRange = new ResTimeRange();
            resAddTimeRange.CResourceNo = cResourceNo;
            resAddTimeRange.resource = this;
            resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility;
            resAddTimeRange.Attribute = resSpecTimeRange.Attribute;
            if (resWorkTimeRange == null)   //增加一个时段
            {
                resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime;
                resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime;
            }
            else                        //
            {
                if (resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime && resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime)
                {
                    resAddTimeRange.DBegTime = resWorkTimeRange.DEndTime;
                    resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime;
                }//加班时段 与 工作时段尾部重叠
                else if (resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime && resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime)
                {
                    resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime;
                    resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime;
                }//加班时段 与 工作时段全部不重叠，在工作时段之外
                else if (resWorkTimeRange.DEndTime < resSpecTimeRange.DBegTime || resWorkTimeRange.DBegTime > resSpecTimeRange.DEndTime)  //&& resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime
                {
                    resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime;
                    resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime;
                }//全部重叠,不用增加时段，在工作时段之内
                else
                {
                    resAddTimeRange = null;
                }
            }
            if (resAddTimeRange != null)
            {
                resAddTimeRange.GetNoWorkTaskTimeRange(resAddTimeRange.DBegTime, resAddTimeRange.DEndTime, false);
                this.ResTimeRangeList.Add(resAddTimeRange);
            }
        }
        public void AddTimeRange(ResTimeRange resLastTimeRange, ResTimeRange resWorkTimeRange)
        {
            ResTimeRange resAddTimeRange = new ResTimeRange();
            resAddTimeRange.CResourceNo = cResourceNo;
            resAddTimeRange.resource = this;
            resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility;
            resAddTimeRange.DBegTime = resLastTimeRange.DEndTime;
            resAddTimeRange.DEndTime = resLastTimeRange.DBegTime;
            resAddTimeRange.GetNoWorkTaskTimeRange(resAddTimeRange.DBegTime, resAddTimeRange.DEndTime, false);
            this.ResTimeRangeList.Add(resAddTimeRange);
        }
        public void DeleteTimeRangeSub(ResTimeRange resWorkTimeRange, ResTimeRange resSpecTimeRange)
        {
            TaskTimeRange resAddTimeRange = new TaskTimeRange();
            if (resSpecTimeRange.DBegTime < resWorkTimeRange.DBegTime)
            {
                resAddTimeRange.DBegTime = resWorkTimeRange.DBegTime;
            }
            else if (resSpecTimeRange.DBegTime < resWorkTimeRange.DEndTime)
            {
                resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime;  //新增加时段的开始时间
            }
            else             //                          
            {
                return;
            }
            if (resSpecTimeRange.DEndTime > resWorkTimeRange.DEndTime)      //与时段结束日期比较
            {
                resAddTimeRange.DEndTime = resWorkTimeRange.DEndTime;
            }
            else if (resSpecTimeRange.DEndTime > resAddTimeRange.DBegTime)  //与时段开始日期比较
            {
                resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime;
            }
            else
            {
                return;
            }
            if (resAddTimeRange != null)
            {
                if (resWorkTimeRange.TaskTimeRangeList.Count > 0)
                {
                    resAddTimeRange = resWorkTimeRange.GetNoWorkTaskTimeRange(resAddTimeRange.DBegTime, resAddTimeRange.DEndTime, false);
                    resAddTimeRange.AllottedTime = resAddTimeRange.HoldingTime;
                    resAddTimeRange.Attribute = resSpecTimeRange.Attribute;  //
                    resAddTimeRange.cTaskType = 2;      //0 空闲时间段 1 工作时间段 2 维修时间段
                    resWorkTimeRange.TaskTimeRangeSplit(resWorkTimeRange.TaskTimeRangeList[0], resAddTimeRange);
                }
            }
        }
        public void getResSourceDayCapList()
        {
            this.ResSourceDayCapList = new List<ResSourceDayCap>(10);
            ResSourceDayCap resSourceDayCap = new ResSourceDayCap();
            DateTime ldt_todayLast = DateTime.Now;
            var groupedResTimeRange = this.ResTimeRangeList.GroupBy(resTimeRange => (resTimeRange.cResourceNo, resTimeRange.dPeriodDay));
            this.ResTimeRangeList.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            foreach (ResTimeRange ResTimeRange1 in this.ResTimeRangeList)
            {
                if (ldt_todayLast != ResTimeRange1.dPeriodDay)
                {
                    resSourceDayCap = new ResSourceDayCap();
                    resSourceDayCap.dPeriodDay = ResTimeRange1.dPeriodDay;
                    resSourceDayCap.DBegTime = ResTimeRange1.DBegTime;
                    this.ResSourceDayCapList.Add(resSourceDayCap);
                }
                resSourceDayCap.ResTimeRangeList.Add(ResTimeRange1);               
                ResTimeRange1.resSourceDayCap = resSourceDayCap;
                ldt_todayLast = ResTimeRange1.dPeriodDay;
            }
        }
        #endregion
        #region  //冻结 上次排产已确认生成任务单，初始化时占用
        public int SchTaskFreezeInit(SchProductRouteRes as_SchProductRouteRes, DateTime adCanBegDate, DateTime adCanEndDate)
        {
            try
            {
                if (as_SchProductRouteRes.BScheduled == 1) return 0; //完工工序已排，不处理
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime >= adCanBegDate; });
                ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    if (ResTimeRangeList1[i].DBegTime > adCanEndDate) break;
                    ResTimeRangeList1[i].TimeSchTaskFreezeInit(as_SchProductRouteRes, ref adCanBegDate, ref adCanEndDate);
                }
                as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++;  //排产顺序号
                as_SchProductRouteRes.BScheduled = 1; //设为已排
                as_SchProductRouteRes.schProductRoute.BScheduled = 1; //设为已排
                if (SchParam.APSDebug == "1")
                {
                    string message2 = string.Format(@"3、排产顺序[{0}]，资源编号[{1}],物料编号[{2}], 座次号[{3}]，座次顺序[{4}]，任务优先级[{5}]，订单优先级[{6}],工序[{7}],排程批次[{8}]，工单号[{9}]",
                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.schProductRoute.schProduct.cSchSNType, as_SchProductRouteRes.schProductRoute.schProduct.iSchSN, 
                            as_SchProductRouteRes.iPriorityRes, as_SchProductRouteRes.schProductRoute.schProduct.iPriority, as_SchProductRouteRes.iWoSeqID + as_SchProductRouteRes.cSeqNote.Trim(), as_SchProductRouteRes.iSchBatch, as_SchProductRouteRes.cWoNo);
                    message2 += string.Format(@"计划ID[{0}],任务ID[{1}],计划开工时间[{2}],计划完工时间[{3}]",
                        as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.dResBegDate, as_SchProductRouteRes.dResEndDate);
                    message2 += string.Format(@"可开工时间[{0}],前准备时间[{1}],工艺特征1[{2}],工艺特征2[{3}]",
                        as_SchProductRouteRes.dCanResBegDate, as_SchProductRouteRes.iResPreTime, as_SchProductRouteRes.FResChaValue1ID, as_SchProductRouteRes.FResChaValue2ID);
                    SchParam.Debug(message2, "SchProductRouteRes.SchTaskFreezeInit工序排产完成");
                }
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1;
        }
        public int SchTaskSortInit(SchProductRouteRes as_SchProductRouteRes, DateTime adCanBegDate, DateTime adCanEndDate)
        {
            as_SchProductRouteRes.schProductRoute.ProcessSchTaskPre(false);
            int ai_workTime = 0;
            double ai_ResReqQty = (as_SchProductRouteRes.iResReqQty - as_SchProductRouteRes.iActResReqQty);
            int iBatchCount = 0;
            if (as_SchProductRouteRes.cWorkType == "1")   //批量加工 
            {
                if (ai_ResReqQty / as_SchProductRouteRes.iBatchQty < Convert.ToInt32(ai_ResReqQty / as_SchProductRouteRes.iBatchQty))
                    iBatchCount = Convert.ToInt32(ai_ResReqQty / as_SchProductRouteRes.iBatchQty) + 1;
                else
                    iBatchCount = Convert.ToInt32(ai_ResReqQty / as_SchProductRouteRes.iBatchQty);
                if (iBatchCount < 1) iBatchCount = 1;
                ai_workTime = Convert.ToInt32(iBatchCount * as_SchProductRouteRes.iCapacity + (iBatchCount - 1) * as_SchProductRouteRes.iBatchInterTime);
            }
            else                          // ‘0’单件加工 
            {
                ai_workTime = Convert.ToInt32(ai_ResReqQty * as_SchProductRouteRes.iCapacity);
            }
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;   //任务可以开始排产日期,任务有中断时，重新设置值
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            try
            {
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                int ai_workTimeTest = ai_workTime;
                int ai_disWorkTime = ai_workTime;
                int ai_ResPreTime = 0;   //资源换产时间
                int ai_CycTimeTol = 0;   //换刀时间
                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;
                int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate);
                if (li_Return < 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                    throw new Exception(cError);
                    return -1;
                }
                adCanBegDate = adCanBegDateTask;
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime > adCanBegDate; });
                ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                Boolean bFirtTime = true;            //是否第一个排产时间段
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    if (ai_workTime == 0) break;
                    ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime,false);
                    if (bFirtTime)
                    {
                        dtBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                    }
                }
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    return -1;
                }
                else
                {
                    as_SchProductRouteRes.iSchSN = SchParam.iSchSNMin--;
                    as_SchProductRouteRes.BScheduled = 1; //设为已排
                    as_SchProductRouteRes.schProductRoute.BScheduled = 1; //设为已排
                }
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1; //剩下未排时间
        }
        #endregion
        public int ResSchBefore()
        {
            if (this.cResourceNo == "YQ-17-07")  //"YQ-17-07"
            {
                int j = 1;
            }
            if (this.iSchBatch < 0)
            {
                this.KeyResSchTask();
                return -1;      //本批次不继续排产
            }
            return 1;
        }
        public int ResSchAfter()
        {            
            return 1;
        }
        public int ResDispatchSch(int iSchBatch)
        {
            SchProductRouteRes SchProductRouteResPre = null;
            List<SchProductRouteRes> ListSchProductRouteRes = new List<SchProductRouteRes>();
            DateTime LastCanBegDate  = DateTime.Now;
            try
            {
                if (this.cResourceNo == "42001")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
                {
                    int j = 1;
                }
                if (iSchBatch == -100)   //调度优化排产,第2次排产  iSchBatch =  -100-------------------------------------------------------------------------------
                {
                    ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0; });
                    if (ListSchProductRouteRes.Count > 0)
                    {
                        foreach (var SchProductRouteResTemp in ListSchProductRouteRes)
                        {
                            if (SchProductRouteResTemp.schProductRoute.cStatus == "4")
                            {
                                SchProductRouteResTemp.iSchSN = SchParam.iSchSNMax++;  //排产顺序号
                                SchProductRouteResTemp.BScheduled = 1;
                                SchProductRouteResTemp.schProductRoute.BScheduled = 1;
                            }
                            else
                            {
                                SchProductRouteResTemp.cDefine25 = "";                                 //工艺特征转换说明清空
                                SchProductRouteResTemp.iResPreTime = 0;                                //前准备时间清空，重算
                                SchProductRouteResTemp.SchProductRouteResPre = null;                   //清空资源前一个任务
                                SchProductRouteResTemp.BScheduled = 0;
                                SchProductRouteResTemp.schProductRoute.BScheduled = 0;
                                SchProductRouteResTemp.TaskClearTask();
                            }
                        }
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return ResTaskComparer(p1, p2); });
                        for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                        {
                            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                            {
                                SchProductRouteResPre = ListSchProductRouteRes[i];
                                continue;
                            }
                            if (ListSchProductRouteRes[i].iSchSdID == SchParam.iSchSdID && ListSchProductRouteRes[i].iProcessProductID == SchParam.iProcessProductID)
                            {
                                int m;
                                m = 1;
                            }
                            ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;
                            ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);
                            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;
                            SchProductRouteResPre = ListSchProductRouteRes[i];
                        }
                    }
                }
                else if (iSchBatch == -200)    //只排正式版本，智能排产控制台正常排产前调用,替代原来冻结任务功能
                {
                    ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0 && p1.cVersionNo.Trim().ToLower() == "sureversion"; });
                    if (ListSchProductRouteRes.Count > 0)
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2){ return ResTaskComparer(p1, p2);});
                        for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                        {
                            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                            {
                                SchProductRouteResPre = ListSchProductRouteRes[i];
                                continue;
                            }
                            if (ListSchProductRouteRes[i].iSchSdID == SchParam.iSchSdID && ListSchProductRouteRes[i].iProcessProductID == SchParam.iProcessProductID)
                            {
                                int m;
                                m = 1;
                            }
                            ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;
                            if (SchParam.ExecTaskSchType == "2")  //2 已执行计划重排,考虑前工序完工时间，不考虑前个任务单结束时间，如果前面有空闲可用时间，可以插单;
                            {
                                LastCanBegDate = SchParam.dtStart;
                                LastCanBegDate = this.GetTaskCanBegDate(ListSchProductRouteRes[i], LastCanBegDate);
                            }
                            ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);
                            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;
                            SchProductRouteResPre = ListSchProductRouteRes[i];
                        }
                    }
                }
                else  //1、第一次排产时调用，按资源优化排产，按排产批次分批优化排产 iSchBatch -100
                {                   
                    ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iSchBatch == iSchBatch && p1.iResReqQty > 0; });
                    if (ListSchProductRouteRes.Count > 0)
                    {
                        foreach (var SchProductRouteResTemp in ListSchProductRouteRes)
                        {
                            SchProductRouteResTemp.iPriorityRes = SchProductRouteResTemp.iSchSN;  //设置资源任务排产顺序
                            SchProductRouteResTemp.cDefine25 = "";                                 //工艺特征转换说明清空
                            SchProductRouteResTemp.iResPreTime = 0;                                //前准备时间清空，重算
                            SchProductRouteResTemp.SchProductRouteResPre = null;                   //清空资源前一个任务
                            SchProductRouteResTemp.BScheduled = 0;
                            SchProductRouteResTemp.schProductRoute.BScheduled = 0;
                            SchProductRouteResTemp.TaskClearTask();
                        }
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return ResTaskComparer(p1, p2); });
                        for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                        {
                            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                            {
                                SchProductRouteResPre = ListSchProductRouteRes[i];
                                continue;
                            }
                            if (ListSchProductRouteRes[i].iSchSdID == SchParam.iSchSdID && ListSchProductRouteRes[i].iProcessProductID == SchParam.iProcessProductID)
                            {
                                int m;
                                m = 1;
                            }
                            ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;
                            if (SchParam.ExecTaskSchType == "2")  //2 已执行计划重排,考虑前工序完工时间;
                                LastCanBegDate = this.GetTaskCanBegDate(ListSchProductRouteRes[i], LastCanBegDate);
                            ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);
                            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;
                            SchProductRouteResPre = ListSchProductRouteRes[i];
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return 1;
        }
        public DateTime GetTaskCanBegDate(SchProductRouteRes schProductRouteRes, DateTime LastCanBegDate)
        {
            DateTime dCanBegDate = LastCanBegDate;
            DateTime dCanBegDateTemp = LastCanBegDate;
            foreach (SchProductRoute schProductRoutePre in schProductRouteRes.schProductRoute.SchProductRoutePreList)
            {
                if (schProductRoutePre.BScheduled == 0) schProductRoutePre.ProcessSchTask();
                dCanBegDateTemp = schProductRoutePre.GetNextProcessCanBegDate(schProductRouteRes.schProductRoute);
                if (dCanBegDateTemp > dCanBegDate) dCanBegDate = dCanBegDateTemp;
            } 
            return dCanBegDate;
        }
        public int ResDispatchSchWo(int iSchBatch)
        {
            SchProductRouteRes SchProductRouteResPre = null;
            List<SchProductRouteRes> ListSchProductRouteRes = new List<SchProductRouteRes>();
            DateTime LastCanBegDate = DateTime.Now;
                ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0; });
                if (ListSchProductRouteRes.Count > 0)
                {
                    foreach (var SchProductRouteResTemp in ListSchProductRouteRes)
                    {
                        SchProductRouteResTemp.cDefine25 = "";                                 //工艺特征转换说明清空
                        SchProductRouteResTemp.iResPreTime = 0;                                //前准备时间清空，重算
                        SchProductRouteResTemp.SchProductRouteResPre = null;                   //清空资源前一个任务
                        SchProductRouteResTemp.BScheduled = 0;
                        SchProductRouteResTemp.schProductRoute.BScheduled = 0;
                        SchProductRouteResTemp.TaskClearTask();
                    }
                if (SchParam.cProChaType1Sort == "9")  //9 按资源定义排程优化方式为准
                {
                    if (this.cProChaType1Sort == "0")  //1 按资源优先级优化
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes); });
                    }
                    else if (this.cProChaType1Sort == "1")  //1 按工单优先级优化
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iWoPriorityRes, p2.schProductRoute.schProductWorkItem.iWoPriorityRes); });
                    }
                    else if (this.cProChaType1Sort == "2")  //2 订单优先级
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority); });
                    }
                    else if (this.cProChaType1Sort == "3")  //3 座次优先级
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN); });
                    }
                    else if (this.cProChaType1Sort == "4") //1 按工艺特征优化排序
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                    }
                    else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                }
                else  //以排产参数定义工艺特征为准，所有资源统一一种参数
                {
                    if (SchParam.cProChaType1Sort == "0")  //1 按资源任务优先级优化
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes); });
                    }
                    else if (SchParam.cProChaType1Sort == "1")  //1 按工单需求日期优化
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "2")  //2 订单需求日期 2022-04-06 JonasCheng
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "3")  //3 座次优先级
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN); });
                    }
                    else if (SchParam.cProChaType1Sort == "4") //1 按工艺特征优化排序
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                    }
                    else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                    {
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                }
                for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                {
                    if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                    {
                        SchProductRouteResPre = ListSchProductRouteRes[i];
                        continue;
                    }
                    ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;
                    ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);
                    LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;
                    SchProductRouteResPre = ListSchProductRouteRes[i];
                }
            }
            return 1;
        }
        public int ResDispatchSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true)
        {
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;   //任务可以开始排产日期,任务有中断时，重新设置值
            DateTime ldtBeginDate = DateTime.Now;
            string message = "";
            if (as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID || as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
            {
                message = string.Format(@"3.1、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                        as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                SchParam.Debug(message, "资源运算");
                ldtBeginDate = DateTime.Now;
            }
            try
            {
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                int ai_workTimeTest = ai_workTime;
                int ai_disWorkTime = ai_workTime;
                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;
                SchParam.ldtBeginDate = DateTime.Now;
                int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate, bReCalWorkTime);
                if (li_Return < 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                    throw new Exception(cError);
                    return -1;
                }
                SchParam.iWaitTime = DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.2、TestResSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                adCanBegDate = adCanBegDateTask;
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate (ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime > adCanBegDate; });
                ResTimeRangeList1.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                Boolean bFirtTime = true;            //是否第一个排产时间段
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.3、ResTimeRangeList1 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    if (bFirtTime && ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime && ai_workTime > ResTimeRangeList1[i].AvailableTime)
                        continue;
                    DateTime ldtBeginDateRessource = DateTime.Now;
                    if (as_SchProductRouteRes.cSeqNote == "折弯")
                    {
                        ldtBeginDateRessource = DateTime.Now;
                    }
                    ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime);
                    Double iWaitTime = DateTime.Now.Subtract(ldtBeginDateRessource).TotalMilliseconds;
                    if (as_SchProductRouteRes.cSeqNote == "折弯")
                    {
                        iWaitTime = iWaitTime;
                    }
                    if (bFirtTime)
                    {
                        dtBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                    }
                    if (ai_workTime <= 0) break;
                }
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.4、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    return -1;
                }
                else
                {
                    as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++;  //排产顺序号
                    if (as_SchProductRouteRes.iSchSN == SchParam.iProcessProductID)
                    {
                        int i = 1;
                    }
                    as_SchProductRouteRes.BScheduled = 1; //设为已排
                    if (SchParam.APSDebug == "1")
                    {
                        string message2 = string.Format(@"3、排产顺序[{0}]，资源编号[{1}],物料编号[{2}], 座次号[{3}]，座次顺序[{4}]，任务优先级[{5}]，订单优先级[{6}],工序[{7}],排程批次[{8}]，工单号[{9}]",
                           as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.schProductRoute.schProduct.cSchSNType, as_SchProductRouteRes.schProductRoute.schProduct.iSchSN, as_SchProductRouteRes.iPriorityRes, as_SchProductRouteRes.schProductRoute.schProduct.iPriority, as_SchProductRouteRes.iWoSeqID + as_SchProductRouteRes.cSeqNote.Trim(), as_SchProductRouteRes.iSchBatch, as_SchProductRouteRes.cWoNo);
                        message2 += string.Format(@"计划ID[{0}],任务ID[{1}],计划开工时间[{2}],计划完工时间[{3}]",
                            as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.dResBegDate, as_SchProductRouteRes.dResEndDate);
                        message2 += string.Format(@"可开工时间[{0}],前准备时间[{1}],工艺特征1[{2}],工艺特征2[{3}]",
                            as_SchProductRouteRes.dCanResBegDate, as_SchProductRouteRes.iResPreTime, as_SchProductRouteRes.FResChaValue1ID, as_SchProductRouteRes.FResChaValue2ID);
                        SchParam.Debug(message2, "Resource.ResDispatchSchTask工序排产完成");
                    }
                }
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.5、运算完成 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1; //剩下未排时间
        }
        public int ResSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true , SchProductRouteRes as_SchProductRouteResPre = null )
        {
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;   //任务可以开始排产日期,任务有中断时，重新设置值
            DateTime ldtBeginDate = DateTime.Now;
            string message = "";
            double iWaitTime;
            if ( as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID && as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID ||as_SchProductRouteRes.iResourceAbilityID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
            {
                message = string.Format(@"3.1、排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                        as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                SchParam.Debug(message, "资源运算");
                ldtBeginDate = DateTime.Now;
            }
            try
            {
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                int ai_workTimeTest = ai_workTime;
                int ai_disWorkTime = ai_workTime;
                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;
                SchParam.ldtBeginDate = DateTime.Now;
                ldtBeginDate = DateTime.Now;
                    int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate, bReCalWorkTime, true, as_SchProductRouteResPre);
                    if (li_Return < 0)
                    {
                        string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                        throw new Exception(cError);
                        return -1;
                    }
                DateTime ldtEndedDate = DateTime.Now;
                TimeSpan interval = ldtEndedDate - ldtBeginDate;//计算间隔时间
                SchParam.iWaitTime = interval.TotalMilliseconds;//DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.2、TestResSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                adCanBegDate = adCanBegDateTask;
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime > adCanBegDate; });
                ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                Boolean bFirtTime = true;            //是否第一个排产时间段
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.3、ResTimeRangeList1 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    if (bFirtTime && ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime && ai_workTime > ResTimeRangeList1[i].AvailableTime )
                        continue;
                    DateTime ldtBeginDateRessource = DateTime.Now;
                    if (as_SchProductRouteRes.cSeqNote == "折弯")
                    {
                        ldtBeginDateRessource = DateTime.Now;
                    }
                    DateTime ldtBeginDate2 = DateTime.Now;
                    try
                    {
                        ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime);
                    }
                    catch (Exception error)
                    {
                        throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResTimeRangeList1！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                        return -1;
                    }
                    if (bFirtTime)
                    {
                        dtBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                    }
                    if (ai_workTime <= 0) break;
                }
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.4、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    return -1;
                }
                else
                {
                    as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++;  //排产顺序号
                    if (as_SchProductRouteRes.iSchSN == SchParam.iProcessProductID)
                    {
                        int i = 1;
                    }
                    as_SchProductRouteRes.BScheduled = 1; //设为已排
                    if (SchParam.APSDebug == "1")
                    {
                        string message2 = string.Format(@"3、排产顺序[{0}]，资源编号[{1}],物料编号[{2}], 座次号[{3}]，座次顺序[{4}]，任务优先级[{5}]，订单优先级[{6}],工序[{7}],排程批次[{8}]，工单号[{9}]",
                           as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.schProductRoute.schProduct.cSchSNType, as_SchProductRouteRes.schProductRoute.schProduct.iSchSN, as_SchProductRouteRes.iPriorityRes, as_SchProductRouteRes.schProductRoute.schProduct.iPriority, as_SchProductRouteRes.iWoSeqID + as_SchProductRouteRes.cSeqNote.Trim(), as_SchProductRouteRes.iSchBatch, as_SchProductRouteRes.cWoNo);
                        message2 += string.Format(@"计划ID[{0}],任务ID[{1}],计划开工时间[{2}],计划完工时间[{3}]",
                            as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID,  as_SchProductRouteRes.dResBegDate, as_SchProductRouteRes.dResEndDate);
                        message2 += string.Format(@"可开工时间[{0}],前准备时间[{1}],工艺特征1[{2}],工艺特征2[{3}]", 
                            as_SchProductRouteRes.dCanResBegDate, as_SchProductRouteRes.iResPreTime, as_SchProductRouteRes.FResChaValue1ID, as_SchProductRouteRes.FResChaValue2ID);
                        SchParam.Debug(message2, "Resource.ResSchTask工序排产完成");
                    }
                }
                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.5、运算完成 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1; //剩下未排时间
        }
        public int ResSchTaskRev(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate)
        {
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;    //任务可以开始排产日期,任务有中断时，重新设置值
            DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
                List<ResTimeRange> ResTimeRangeListTest = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime <= adCanBegDate; });
            }
            DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
            int ai_workTimeTest = ai_workTime;
            int ai_ResPreTime = 0;                //资源前准备时间
            int ai_CycTimeTol = 0;                //换刀时间  ,倒排时不用
            SchParam.ldtBeginDate = DateTime.Now;
            try
            {
                int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate);
                if (li_Return < 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                    throw new Exception(cError);
                    return -1;
                }
                adCanBegDate = adCanBegDateTask;
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate (ResTimeRange p) { return p.AvailableTime > 0 && p.DBegTime <= adCanBegDate; });
                ResTimeRangeList1.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
                Boolean bFirtTime = true;            //是否第一个排产时间段
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    if (ai_workTime == 0) break;
                    if(as_SchProductRouteRes.schProductRoute.schProduct.cSchType != "2" )  //有限产能倒排
                        ResTimeRangeList1[i].TimeSchTaskRev(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref bFirtTime);
                    else    //无限产能倒排
                        ResTimeRangeList1[i].TimeSchTaskRevInfinite(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref bFirtTime);
                }
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    return -1;
                }
                else
                {
                    as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++;  //排产顺序号
                    as_SchProductRouteRes.BScheduled = 1; //设为已排
                    if (SchParam.APSDebug == "1")
                    {
                        string message2 = string.Format(@"3、排产顺序[{0}]，资源编号[{1}],物料编号[{2}], 座次号[{3}]，座次顺序[{4}]，任务优先级[{5}]，订单优先级[{6}],工序[{7}],排程批次[{8}]，工单号[{9}]",
                           as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.schProductRoute.schProduct.cSchSNType, as_SchProductRouteRes.schProductRoute.schProduct.iSchSN, as_SchProductRouteRes.iPriorityRes, as_SchProductRouteRes.schProductRoute.schProduct.iPriority, as_SchProductRouteRes.iWoSeqID + as_SchProductRouteRes.cSeqNote.Trim(), as_SchProductRouteRes.iSchBatch, as_SchProductRouteRes.cWoNo);
                        message2 += string.Format(@"计划ID[{0}],任务ID[{1}],计划开工时间[{2}],计划完工时间[{3}]",
                            as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.dResBegDate, as_SchProductRouteRes.dResEndDate);
                        message2 += string.Format(@"可开工时间[{0}],前准备时间[{1}],工艺特征1[{2}],工艺特征2[{3}]",
                            as_SchProductRouteRes.dCanResBegDate, as_SchProductRouteRes.iResPreTime, as_SchProductRouteRes.FResChaValue1ID, as_SchProductRouteRes.FResChaValue2ID);
                        SchParam.Debug(message2, "SchProductRouteRes.ResSchTaskRev工序排产完成");
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源倒排计算时出错,位置Resource.ResSchTaskRev！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }
            return 1; //剩下未排时间
        }
        public int TestResSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, ref DateTime adCanBegDateTask, Boolean bSchRev, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref DateTime dtBegDate, ref DateTime dtEndDate, Boolean bShowTips = true, Boolean bReCalWorkTime = true, SchProductRouteRes as_SchProductRouteResPre = null )
        {
            int ai_workTimeTask = ai_workTime;
            int ai_disWorkTime = ai_workTime;
            DateTime adCanBegDateTask2 = adCanBegDateTask;
            int ai_ResPostTime = 0;             //资源后准备时间
            dtEndDate = adCanBegDate;
            DateTime ldtBeginDate = DateTime.Now;
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            try
            {
                List<ResTimeRange> ResTimeRangeList1 = new List<ResTimeRange>();
                if (bSchRev == false)   //正排
                {
                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DEndTime > adCanBegDateTask2; }); // p.AvailableTime > 0 &&
                    if (ResTimeRangeList.Count < 1)
                    {
                        if (bShowTips)  //显示提示
                        {
                            string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],请检查是否有工作日历或当前资源是资源组!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID);
                            throw new Exception(cError);
                        }
                        return -1;
                    }
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    Boolean bFirtTime = true;            //是否第一个排产时间段
                    ResTimeRange resTimeRangeNext = null;       //下个空闲时间段，同时传入    
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;
                        if (i < ResTimeRangeList1.Count - 1)
                            resTimeRangeNext = ResTimeRangeList1[i + 1];
                        else
                            resTimeRangeNext = null;
                        if (ResTimeRangeList1[i].AvailableTime <= 0 && ResTimeRangeList1[i].AllottedTime > 0 )
                        {
                            bFirtTime = true;   //是否第一个排产时间段
                            ai_workTime = ai_workTimeTask;            //返回原值
                            adCanBegDate = ResTimeRangeList1[i].DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
                            adCanBegDateTask = ResTimeRangeList1[i].DEndTime;  //重新设置任务可开始时间,并返回
                            continue;
                        }
                        try
                        {
                            SchParam.ldtBeginDate = DateTime.Now;
                            ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime, resTimeRangeNext, as_SchProductRouteResPre);
                        }
                        catch (Exception error)
                        {
                            throw new Exception("时间段排程出错,订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                            return -1;
                        }
                        if (bFirtTime)
                        {
                            dtBegDate = adCanBegDate;// adCanBegDateTask;//ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate;//adCanBegDateTask; //ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        }
                    }
                    dtEndDate = adCanBegDate;   //工序完工时间
                }
                else              //倒排
                {
                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime < adCanBegDateTask2; });  //p.AvailableTime > 0 
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
                    Boolean bFirtTime = true;            //是否第一个排产时间段
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;
                        if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType != "2")  //有限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRev(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);
                        else    //无限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRevInfinite(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);
                        if (bFirtTime) dtEndDate = adCanBegDate; //adCanBegDateTask;//ResTimeRangeList1[i].DEndTime;   //为true，第一个工序
                        if (bFirtTime)
                        {
                            bFirtTime = false;
                            dtBegDate = adCanBegDate;// adCanBegDateTask;//ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate;//adCanBegDateTask; //ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        }
                        else if (i + 1 < ResTimeRangeList1.Count)  //如果不是第一个时间段，当前空闲时间段小于待排任务，而且已经排了其他任务，排不下。此时需要重新选择时间段 2019-09-09
                        {
                            if (ResTimeRangeList1[i + 1].WorkTimeRangeList.Count > 0 && ResTimeRangeList1[i + 1].NotWorkTime < ai_workTime)
                            {
                                bFirtTime = true;                                 //是否第一个排产时间段
                                ai_workTime = ai_workTimeTask;                    //返回原值      
                                adCanBegDate = ResTimeRangeList1[i + 1].WorkTimeRangeList[ResTimeRangeList1[i + 1].WorkTimeRangeList.Count - 1].DEndTime; //ResTimeRangeList1[i + 1].DBegTime;     //重置可开工时间,已排任务之后
                            }
                        }
                    }
                    dtBegDate = adCanBegDate;   //工序完工时间
                }
                if (ai_workTime > 0)
                {
                    if (bShowTips)  //显示提示
                    {
                        string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                        throw new Exception(cError);
                    }
                    return -1;
                }
            }
            catch (Exception error)
            {
                if (bShowTips)   //显示提示
                {
                    throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                }
                return -1;
            }
            return 1; //剩下未排时间
        }
        public int TestResSchTaskNew(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, ref DateTime adCanBegDateTask, Boolean bSchRev, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref DateTime dtBegDate, ref DateTime dtEndDate, Boolean bShowTips = true, Boolean bReCalWorkTime = true, SchProductRouteRes as_SchProductRouteResPre = null)
        {
            int ai_workTimeTask = ai_workTime;
            int ai_disWorkTime = ai_workTime;
            DateTime adCanBegDateTask2 = adCanBegDateTask;
            int ai_ResPostTime = 0;             //资源后准备时间
            dtEndDate = adCanBegDate;
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            try
            {
                List<ResTimeRange> ResTimeRangeList1 = new List<ResTimeRange>();
                if (bSchRev == false)   //正排
                {
                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DEndTime > adCanBegDateTask2; }); // p.AvailableTime > 0 &&
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                    Boolean bFirtTime = true;            //是否第一个排产时间段
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;
                        ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime);
                        if (bFirtTime)
                        {
                            dtBegDate = adCanBegDate;// adCanBegDateTask;//ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate;//adCanBegDateTask; //ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        }
                    }
                    dtEndDate = adCanBegDate;   //工序完工时间
                }
                else              //倒排
                {
                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime < adCanBegDateTask2; });  //p.AvailableTime > 0 
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
                    Boolean bFirtTime = true;            //是否第一个排产时间段
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;
                        if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType != "2")  //有限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRev(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);
                        else    //无限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRevInfinite(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);
                        if (bFirtTime) dtEndDate = adCanBegDate; //adCanBegDateTask;//ResTimeRangeList1[i].DEndTime;   //为true，第一个工序
                    }
                    dtBegDate = adCanBegDate;   //工序完工时间
                }
                if (ai_workTime > 0)
                {
                    if (bShowTips)  //显示提示
                    {
                        string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                        throw new Exception(cError);
                    }
                    return -1;
                }
            }
            catch (Exception error)
            {
                if (bShowTips)   //显示提示
                {
                    throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                }
                return -1;
            }
            return 1; //剩下未排时间
        }
        public DateTime GetEarlyStartDate(DateTime adStartDate, Boolean bSchRev)
        {
            List<ResTimeRange> ListReturn = new List<ResTimeRange>(10);            
            int iCanSchTime = 0;                   //每时段可排时间
            ResTimeRange ResTimeRangeBeg = null;   //开始资源时间段
            ResTimeRange ResTimeRangeEnd = null;   //结束资源时间段   
            DateTime dtBegDate;                     //预计开始时间
            TaskTimeRange TaskTimeRangeBeg = null;            //开始空闲时间段
            TaskTimeRange TaskTimeRangeEnd = null;            //结束空闲时间段           
            DateTime dtEndDate = adStartDate;                      //预计完工时间
            if (bSchRev == false)    //正排
            {
                ListReturn = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DEndTime > adStartDate && p.AvailableTime > 0; }); // p.AvailableTime > 0 
                ListReturn.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
                if (ListReturn.Count > 0)
                    dtEndDate = ListReturn[0].DEndTime;
            }
            else                     //倒排
            {
                ListReturn = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime < adStartDate && p.AvailableTime > 0; }); // p.AvailableTime > 0 
                ListReturn.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
                if (ListReturn.Count > 0)
                    dtEndDate = ListReturn[0].DBegTime;
            }
            return dtEndDate;        
        }
        public DateTime GetSchStartDate(SchProductRouteRes as_SchProductRouteRes, int ai_workTime, DateTime adStartDate, Boolean bSchRev, ref DateTime dtEndDate)
        {
            List<ResTimeRange> ListReturn = new List<ResTimeRange>(10);
            int ai_workTimeOld = ai_workTime;      //
            int iCanSchTime = 0;                   //每时段可排时间
            ResTimeRange ResTimeRangeBeg = null;   //开始资源时间段
            ResTimeRange ResTimeRangeEnd = null;   //结束资源时间段   
            DateTime dtBegDate;                     //预计开始时间
            TaskTimeRange TaskTimeRangeBeg = null;            //开始空闲时间段
            TaskTimeRange TaskTimeRangeEnd = null;            //结束空闲时间段           
            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }
            if (bSchRev == false)    //正排
            {
                return TaskTimeRangeBeg.DBegTime;
            }
            else                     //倒排
            {
                return TaskTimeRangeBeg.DEndTime;
            }
        }
        public int GetChangeTime(SchProductRouteRes as_SchProductRouteRes, int ai_workTime, DateTime adStartDate, ref int iCycTimeTol, Boolean bSchdule, SchProductRouteRes as_SchProductRouteResPre = null )
        {
            int iPreTime = 0;      //前准备时间(换产时间)
            if (this.cNeedChanged == "1" && as_SchProductRouteRes.cWoNo != "" && as_SchProductRouteRes.schProductRoute.schProduct.cType == "PSH") return 0;
            if (as_SchProductRouteResPre == null)
            {
                List<SchProductRouteRes> ListSchProductRouteResAll = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0 && p1.BScheduled == 1 && p1.dResBegDate <= adStartDate; });
                ListSchProductRouteResAll.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p2.dResBegDate, p1.dResBegDate); });
                if (ListSchProductRouteResAll.Count > 0 )
                    as_SchProductRouteResPre = ListSchProductRouteResAll[0];
            }
            cTimeNote = "";
            if (SchParam.ResProcessCharCount > 0 )
                iPreTime = GetChangeTime(as_SchProductRouteRes, ai_workTime, as_SchProductRouteResPre, ref iCycTimeTol, bSchdule);
            if (iPreTime > 0)
            {
                iPreTime += int.Parse(this.iResPreTime.ToString());
                cTimeNote += " 资源前准备时间:[" + this.iResPreTime.ToString() + "];";
            }
            if (as_SchProductRouteResPre != null)
            {
                if (as_SchProductRouteRes.schProductRoute.cWorkItemNo != as_SchProductRouteResPre.schProductRoute.cWorkItemNo)
                {
                    iPreTime += int.Parse(this.iChangeTime.ToString());
                    cTimeNote += " 换料时间:[" + this.iChangeTime.ToString() + "];";
                }
            }
            else   //前面没有物料,也给换料时间
            {
                iPreTime += int.Parse(this.iChangeTime.ToString());
                cTimeNote += " 换料时间:[" + this.iChangeTime.ToString() + "];";
            }
            if (as_SchProductRouteRes.iResPreTimeOld > 0)
            {
                iPreTime += (int)as_SchProductRouteRes.iResPreTimeOld;
                cTimeNote += " 工艺路线资源前准备时间:[" + as_SchProductRouteRes.iResPreTimeOld.ToString() + "];";
            }
            as_SchProductRouteRes.cDefine25 = cTimeNote;
            return iPreTime;  //返回换产时间
        }
        public int GetChangeTime(SchProductRouteRes as_SchProductRouteRes, int ai_workTime, SchProductRouteRes as_SchProductRouteResPre , ref int iCycTimeTol, Boolean bSchdule)
        {
            int iPreTime = 0;     //前准备时间(换产时间)           
            iCycTimeTol = 0;      //中间定期更换时间（换刀时间）
            int[] iCycTime = new Int32[12];
            int[] iChaValue = new Int32[12];
            try
            {
                if (as_SchProductRouteResPre != null)
                {
                    cTimeNote = "任务号排产顺序:[" + SchParam.iSchSNMax + "]" + "前任务号:[" + as_SchProductRouteResPre.iSchSdID + "] [" + as_SchProductRouteResPre.iProcessProductID + "],排产顺序：[" + as_SchProductRouteResPre.iSchSN + "]";
                    if (this.FProChaType1ID != "-1" && this.FProChaType1ID != "" && as_SchProductRouteRes.resChaValue1 != null && SchParam.ResProcessCharCount > 0)
                    {
                        cTimeNote += " 工艺特征1:" + as_SchProductRouteRes.resChaValue1.FResChaValueNo;
                        if (as_SchProductRouteResPre.resChaValue1  != null) cTimeNote += " 前工艺特征1:" + as_SchProductRouteResPre.resChaValue1.FResChaValueNo ;
                        iChaValue[0] = as_SchProductRouteRes.resChaValue1.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue1, ai_workTime, ref iCycTime[0], bSchdule, as_SchProductRouteResPre);
                    }
                    if (this.FProChaType2ID != "-1" && this.FProChaType2ID != "" && as_SchProductRouteRes.resChaValue2 != null && SchParam.ResProcessCharCount > 1)
                    {
                        cTimeNote += " 工艺特征2:" + as_SchProductRouteRes.resChaValue2.FResChaValueNo;
                        if (as_SchProductRouteResPre.resChaValue2 != null) cTimeNote += " 前工艺特征2:" + as_SchProductRouteResPre.resChaValue2.FResChaValueNo;
                        iChaValue[1] = as_SchProductRouteRes.resChaValue2.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue2, ai_workTime, ref iCycTime[1], bSchdule, as_SchProductRouteResPre);
                    }
                    if (this.FProChaType3ID != "-1" && this.FProChaType3ID != "" && as_SchProductRouteRes.resChaValue3 != null && SchParam.ResProcessCharCount > 2)
                    {
                        cTimeNote += " 工艺特征3:" + as_SchProductRouteRes.resChaValue3.FResChaValueNo;
                        if (as_SchProductRouteResPre.resChaValue3 != null) cTimeNote += " 前工艺特征3:" + as_SchProductRouteResPre.resChaValue3.FResChaValueNo;
                        iChaValue[2] = as_SchProductRouteRes.resChaValue3.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue3, ai_workTime, ref iCycTime[2], bSchdule, as_SchProductRouteResPre);
                    }
                    if (this.FProChaType4ID != "-1" && this.FProChaType4ID != "" && as_SchProductRouteRes.resChaValue4 != null && SchParam.ResProcessCharCount > 3)
                    {
                        cTimeNote += " 工艺特征4:" + as_SchProductRouteResPre.resChaValue4.FResChaValueNo;
                        if (as_SchProductRouteResPre.resChaValue4 != null) cTimeNote += " 前工艺特征4:" + as_SchProductRouteResPre.resChaValue4.FResChaValueNo;
                        iChaValue[3] = as_SchProductRouteRes.resChaValue4.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue4, ai_workTime, ref iCycTime[3], bSchdule, as_SchProductRouteResPre);
                    }
                    if (this.FProChaType5ID != "-1" && this.FProChaType5ID != "" && as_SchProductRouteRes.resChaValue5 != null && SchParam.ResProcessCharCount > 4)
                    {
                        cTimeNote += " 工艺特征5:" + as_SchProductRouteResPre.resChaValue5.FResChaValueNo;
                        if (as_SchProductRouteResPre.resChaValue5 != null) cTimeNote += " 前工艺特征5:" + as_SchProductRouteResPre.resChaValue5.FResChaValueNo;
                        iChaValue[4] = as_SchProductRouteRes.resChaValue5.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue5, ai_workTime, ref iCycTime[4], bSchdule, as_SchProductRouteResPre);
                    }
                    if (this.FProChaType6ID != "-1" && this.FProChaType6ID != "" && as_SchProductRouteRes.resChaValue6 != null && SchParam.ResProcessCharCount > 5)
                    {
                        cTimeNote += " 工艺特征6:" + as_SchProductRouteRes.resChaValue6.FResChaValueNo;
                        if (as_SchProductRouteResPre.resChaValue6 != null) cTimeNote += " 前工艺特征6:" + as_SchProductRouteResPre.resChaValue6.FResChaValueNo;
                        iChaValue[5] = as_SchProductRouteRes.resChaValue6.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue6, ai_workTime, ref iCycTime[5], bSchdule, as_SchProductRouteResPre);
                    }
                }
                else          //前面没有任务
                {
                    if (this.FProChaType1ID != "-1" && this.FProChaType1ID != "" && as_SchProductRouteRes.resChaValue1 != null)
                        iChaValue[0] = as_SchProductRouteRes.resChaValue1.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[0], bSchdule, null);
                    if (this.FProChaType2ID != "-1" && this.FProChaType2ID != "" && as_SchProductRouteRes.resChaValue2 != null)
                        iChaValue[1] = as_SchProductRouteRes.resChaValue2.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[1], bSchdule, null);
                    if (this.FProChaType3ID != "-1" && this.FProChaType3ID != "" && as_SchProductRouteRes.resChaValue3 != null)
                        iChaValue[2] = as_SchProductRouteRes.resChaValue3.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[2], bSchdule, null);
                    if (this.FProChaType4ID != "-1" && this.FProChaType4ID != "" && as_SchProductRouteRes.resChaValue4 != null)
                        iChaValue[3] = as_SchProductRouteRes.resChaValue4.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[3], bSchdule, null);
                    if (this.FProChaType5ID != "-1" && this.FProChaType5ID != "" && as_SchProductRouteRes.resChaValue5 != null)
                        iChaValue[4] = as_SchProductRouteRes.resChaValue5.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[4], bSchdule, null);
                    if (this.FProChaType6ID != "-1" && this.FProChaType6ID != "" && as_SchProductRouteRes.resChaValue6 != null)
                        iChaValue[5] = as_SchProductRouteRes.resChaValue6.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[5], bSchdule, null);
                }
                for (int i = 0; i < 7; i++)
                {
                    if (iChaValue[i] > 0)
                    {
                        cTimeNote += " 特征" + (i + 1).ToString() + ": 前准备时间[" + iChaValue[i] + "];";
                        iPreTime += iChaValue[i];         //前准备时间(换产时间)
                    }
                    if (iCycTime[i] > 0)
                    {
                        cTimeNote += " 特征" + (i + 1).ToString() + ": 换刀时间[" + iCycTime[i] + "];";
                        iCycTimeTol += iCycTime[i];       //中间定期更换时间（换刀时间）
                    }
                }
            }
            catch (Exception exp)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "出错位置:Resource.GetChangeTime 工艺特征换产时间计算出错！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                return -1;
            }
            return iPreTime;  //返回换产时间           
        }
        public int KeyResSchTask(int iCount = 1)
        {
            this.GetNotSchTask();
            if (schProductRouteResList.Count < 1) return 1;
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });
            double iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            if (iSchPriority < 0) iSchPriority = 0;
            SchProductRouteRes as_SchProductRouteResLast = null;
            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResList)
            {
                if (schProductRouteRes.iProcessProductID == SchParam.iProcessProductID && schProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }
                schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);
                if (SchParam.bSchKeyBySN == "1" && as_SchProductRouteResLast != null)
                {
                    schProductRouteRes.schProductRoute.dEarlyBegDate = as_SchProductRouteResLast.dResEndDate;
                    schProductRouteRes.dEarliestStartTime = as_SchProductRouteResLast.dResEndDate;
                }   
                schProductRouteRes.schProductRoute.ProcessSchTaskNext("2");  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排
                if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0)
                {
                    iSchPriority++;
                    schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority;
                }
                as_SchProductRouteResLast = schProductRouteRes;
            }
            return 1;
        }
        public int KeyResSchTaskPre()    
        {
            List<SchProductRouteRes> schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.iSchBatch == this.iSchBatch ); });
            schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });
            double iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            if (iSchPriority < 0) iSchPriority = 0;
            return 1;
        }
        public int KeyResSchTaskSingle(double as_iTurmsTime, ref SchProductRouteRes as_SchProductRouteResLast)
        {
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });
            double iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            if (iSchPriority < 0) iSchPriority = 0;
            if (this.iTurnsType == "1"  )   //1 按轮换时间  2 按任务数量
            {
                if (as_iTurmsTime < 0) as_iTurmsTime = this.iTurnsTime;
                if (this.iTurnsTime <= 0 && as_SchProductRouteResLast != null)
                {
                    as_iTurmsTime = as_SchProductRouteResLast.iResPreTime;
                    if (as_SchProductRouteResLast.resource.cResourceNo != this.cResourceNo && as_SchProductRouteResLast.resource.bScheduled == 1)
                    {
                        as_iTurmsTime = 99999999999; 
                    }
                }
                if (as_iTurmsTime == 0) return 1;
                double iTolWorkTime = 0;
                while (iTolWorkTime < as_iTurmsTime )
                {
                    SchProductRouteRes schProductRouteRes = schProductRouteResList.Find(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0); });
                    if (schProductRouteRes == null)
                    {
                        SchParam.Debug(string.Format("关键资源{0}轮换排产,轮换批次[{1}],总工时[{2}],所有任务已排完.", this.cResourceNo, this.iBatch.ToString(), (iTolWorkTime / 60).ToString()), "关键资源轮换生产");
                        this.iBatch++;
                        return -1;
                    }
                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);
                    if (SchParam.bSchKeyBySN == "1" && as_SchProductRouteResLast != null)
                    {
                        schProductRouteRes.schProductRoute.dEarlyBegDate = as_SchProductRouteResLast.dResEndDate;
                        schProductRouteRes.dEarliestStartTime = as_SchProductRouteResLast.dResEndDate;
                    }                        
                    schProductRouteRes.schProductRoute.ProcessSchTaskNext("2");  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排
                    if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0)
                    {
                        iSchPriority++;
                        schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority;
                    }
                    schProductRouteRes.iBatch = this.iBatch;
                    iTolWorkTime +=  schProductRouteRes.iResRationHour;
                    as_SchProductRouteResLast = schProductRouteRes;
                }
                this.iBatch++;
                SchParam.Debug(string.Format("关键资源{0}轮换排产,轮换批次[{1}],总工时[{2}]", this.cResourceNo, this.iBatch.ToString(), (iTolWorkTime/60).ToString()), "关键资源轮换生产");
            }
            else if (this.iTurnsType == "2")                       //按任务数量轮换
            {
                if (as_iTurmsTime < 0) as_iTurmsTime = this.iTurnsTime;
                double iTolWorks = 0;
                as_iTurmsTime = 1;
                while (iTolWorks < as_iTurmsTime || this.iTurnsTime == 0)
                {
                    SchProductRouteRes schProductRouteRes = schProductRouteResList.Find(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0); });
                    if (schProductRouteRes == null)
                    {
                        this.iBatch++;
                        return -1;
                    }
                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);
                    if (SchParam.bSchKeyBySN == "1" && as_SchProductRouteResLast != null)
                    {
                        schProductRouteRes.schProductRoute.dEarlyBegDate = as_SchProductRouteResLast.dResEndDate;
                        schProductRouteRes.dEarliestStartTime = as_SchProductRouteResLast.dResEndDate;
                    }   
                    schProductRouteRes.schProductRoute.ProcessSchTaskNext("2");  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排
                    if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0)
                    {
                        iSchPriority++;
                        schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority;
                    }
                    iTolWorks += 1;
                    as_SchProductRouteResLast = schProductRouteRes;
                    if (this.cBatch2WorkTime < 10) this.cBatch2WorkTime = 10;
                    if (this.iTurnsTime == 0 && as_SchProductRouteResLast.iResPreTime >= this.cBatch2WorkTime * 60)
                    {
                        this.iBatch++;
                        return 1;
                    }
                }
                this.iBatch++;
            }
            return 1;
        }
        public int KeyResBatch( )
        {
            SchProductRouteRes as_SchProductRouteResLast = new SchProductRouteRes();           
            try
            {
                List<SchProductRouteRes> schProductRouteResListNoTest = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.iWoSeqID == this.iBatchWoSeqID && p1.iSchBatch == this.iSchBatch); });
                List<SchProductRouteRes> schProductRouteResListNoSeq = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo &&  p1.schProductRoute.BScheduled == 0 && p1.iSchBatch == this.iSchBatch); });
                List<SchProductRouteRes> schProductRouteResListNo = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID == this.iBatchWoSeqID  &&p1.iSchBatch == this.iSchBatch); });
                if (schProductRouteResListNo.Count < 1) return -1;
                foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListNo)
                {
                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);
                    schProductRouteRes.schProductRoute.GetRouteEarlyBegDate();
                    schProductRouteRes.dEarliestStartTime = schProductRouteRes.schProductRoute.dEarlyBegDate;
                }
                schProductRouteResListNo.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                if(this.iSchBatch == 1 )
                {
                    List<SchProductRouteRes> schProductRouteResListFirst = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo  && p1.iWoSeqID == this.iBatchWoSeqID && p1.iSchBatch == this.iSchBatch && p1.iBatch == 1); });
                    if (schProductRouteResListFirst.Count > 0 )
                        KeyResBatchSchTaskSubFirst(1, ref as_SchProductRouteResLast, schProductRouteResListFirst);
                }
                if (this.cBatch2Filter == "" && this.cBatch3Filter == "" && this.cBatch4Filter == "" && this.cBatch5Filter == "")
                {
                    KeyResBatchSchTask(this.cBatch1Filter, this.iBatchWoSeqID, true, this.iTurnsTime, schProductRouteResListNo, as_SchProductRouteResLast);
                }
                else
                {
                    Boolean bExist = true;
                    int iBatchCount = 1;
                    while (bExist)
                    {
                        if (this.cBatch1Filter != "")
                            KeyResBatchSchTask(this.cBatch1Filter, this.iBatchWoSeqID, false,this.cBatch1WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);
                        if (this.cBatch2Filter != "")
                            KeyResBatchSchTask(this.cBatch2Filter, this.iBatchWoSeqID, false, this.cBatch2WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);
                        if (this.cBatch3Filter != "")
                            KeyResBatchSchTask(this.cBatch3Filter, this.iBatchWoSeqID, false, this.cBatch3WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);
                        if (this.cBatch4Filter != "")
                            KeyResBatchSchTask(this.cBatch4Filter, this.iBatchWoSeqID, false, this.cBatch4WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);
                        if (this.cBatch5Filter != "")
                            KeyResBatchSchTask(this.cBatch5Filter, this.iBatchWoSeqID, false, this.cBatch5WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);
                        iBatchCount++;
                        List<SchProductRouteRes> schProductRouteResListNo2 = schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 ); });
                        if (schProductRouteResListNo2.Count < 1)
                        {
                            bExist = false;
                            break;
                        }
                        if (iBatchCount > 20)
                        {
                            KeyResBatchSchTask("", this.iBatchWoSeqID, true, 0, schProductRouteResListNo, as_SchProductRouteResLast);
                            break;
                        }
                    }
                    KeyResBatchSchTask("", this.iBatchWoSeqID, true, 0, schProductRouteResListNo, as_SchProductRouteResLast);
                }
            }
            catch (Exception exp2)
            {
                throw exp2;
                return -1;
            }
            return 1;
        }
        public int KeyResBatchSchTask(string cBatchFilter, int iBatchWoSeqID, Boolean bSchAll , int as_iTurmsTime, List<SchProductRouteRes> as_schProductRouteResListNo, SchProductRouteRes as_SchProductRouteResLast)
        {          
            List<SchProductRouteRes> schProductRouteResListNo = new List<SchProductRouteRes>(10);
            List<SchProductRouteRes> schProductRouteResListLast = new List<SchProductRouteRes>(10);   //生产任务单最后一批
            if (cBatchFilter != "")
                schProductRouteResListNo = as_schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && cBatchFilter.IndexOf(p1.FResChaValue1ID) >= 0) && p1.iWoSeqID == iBatchWoSeqID && p1.iSchBatch == this.iSchBatch; });
            else    //为空时,不带条件
                schProductRouteResListNo = as_schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0) && p1.iWoSeqID == iBatchWoSeqID && p1.iSchBatch == this.iSchBatch; });
            if (schProductRouteResListNo.Count < 1) return -1;
            if (this.cResourceNo == "YQ-14-01" && this.iSchBatch > 0 )  //"YQ-17-07","YQ-11-01"
            {
                int j = 1;
            }
            schProductRouteResListNo.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.schProductRoute.dEarlyBegDate, p2.schProductRoute.dEarlyBegDate); });
            double iTolWorkTime = 0;
            if (as_iTurmsTime < 1 )
                as_iTurmsTime = this.iTurnsTime;
            if (as_iTurmsTime < 1)
                as_iTurmsTime = 240 * 60;
            for (int i = 0; i < schProductRouteResListNo.Count; i++)
            {
                SchProductRouteRes schProductRouteRes = schProductRouteResListNo[i];
                schProductRouteRes.iResRationHour = schProductRouteRes.iResReqQty * schProductRouteRes.iCapacity;
                if (bSchAll)
                {                    
                    iTolWorkTime += schProductRouteRes.iResRationHour;
                }
                else
                {
                        iTolWorkTime += schProductRouteRes.iResRationHour;
                }
                if (schProductRouteRes.iSchSdID == SchParam.iSchSdID && schProductRouteRes.iProcessProductID == SchParam.iProcessProductID)
                {
                    int j;
                }
                schProductRouteRes.iBatch = this.iBatch;
                if (iTolWorkTime > as_iTurmsTime)
                {
                    as_SchProductRouteResLast = schProductRouteRes;
                    SchParam.Debug(string.Format("资源编号{0}，分批{1},分批条件{2}，分批工时{3},累计工时{4},总任务数{5}", this.cResourceNo, this.iBatch, cBatchFilter, (as_iTurmsTime / 60).ToString(), (iTolWorkTime / 60).ToString(),schProductRouteResListNo.Count.ToString()),"资源分批");
                    KeyResBatchSchTaskSub(this.iBatch, ref as_SchProductRouteResLast, as_schProductRouteResListNo,cBatchFilter);
                    iTolWorkTime = 0;
                    this.iBatch++;
                    if (!bSchAll) return this.iBatch;
                }
                if (i == schProductRouteResListNo.Count - 1)
                {
                    { 
                        as_SchProductRouteResLast = schProductRouteRes;
                        SchParam.Debug(string.Format("资源编号{0}最后一批，分批{1},分批条件{2}，分批工时{3},累计工时{4},总任务数{5}", this.cResourceNo, this.iBatch, cBatchFilter, (as_iTurmsTime / 60).ToString(), (iTolWorkTime / 60).ToString(), schProductRouteResListNo.Count.ToString()), "资源分批");
                        KeyResBatchSchTaskSub(this.iBatch, ref as_SchProductRouteResLast, as_schProductRouteResListNo, cBatchFilter);
                        this.iBatch++;
                        if (!bSchAll) return this.iBatch;
                    }
                }
            }
            return 1;
        }
        public int KeyResBatchSchTaskSub(int as_iBatch, ref SchProductRouteRes as_SchProductRouteResLast, List<SchProductRouteRes> as_schProductRouteResListNo, string cBatchFilter)
        {
            List<SchProductRouteRes> schProductRouteResListBatch = as_schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID == this.iBatchWoSeqID && p1.iBatch == as_iBatch && p1.iSchBatch == this.iSchBatch); });
            if (schProductRouteResListBatch.Count < 1) return -1;
            DateTime dEndDateTemp = SchParam.dtToday;
            if (this.dBatchEndDate > SchParam.dtToday)
                dEndDateTemp = this.dBatchEndDate;
            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListBatch)
            {
                dEndDateTemp = schProductRouteRes.schProductRoute.dEarlyBegDate;
                if (dEndDateTemp > this.dBatchBegDate )  //这批的最后一个件，作为油漆开始时间)
                    this.dBatchBegDate = dEndDateTemp;
            }
            this.dBatchBegDate = this.dBatchBegDate.AddMinutes(this.iPreStocks);
            this.dBatchBegDate = this.dBatchBegDate.AddMinutes(this.iPostStocks);
            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListBatch)
            {
                schProductRouteRes.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
            }
            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListBatch)
            {
                if (schProductRouteRes.iSchSdID == SchParam.iSchSdID && schProductRouteRes.iProcessProductID == SchParam.iProcessProductID)
                {
                    int j;
                }
                schProductRouteRes.schProductRoute.ProcessSchTask();
                List<SchProductRouteRes> schProductRouteResListWorkItem = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID > this.iBatchWoSeqID  && p1.cInvCode == schProductRouteRes.cInvCode && p1.iSchSdID ==  schProductRouteRes.iSchSdID  ); });
                foreach (SchProductRouteRes schProductRouteResNext in schProductRouteResListWorkItem)
                {
                    schProductRouteResNext.iBatch = as_iBatch;
                    schProductRouteResNext.iSchSN = schProductRouteRes.iSchSN ;
                }
                as_SchProductRouteResLast = schProductRouteRes;
            }
            List<SchProductRouteRes> schProductRouteResListNext = new List<SchProductRouteRes>(10);
            List<SchProductRouteRes> schProductRouteResListNextAdd = new List<SchProductRouteRes>(10);
            for (int iNextSeqID = this.iBatchWoSeqID + 1; iNextSeqID < 90; iNextSeqID++)
            {
                schProductRouteResListNext = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.iWoSeqID == iNextSeqID); });
                if (schProductRouteResListNext.Count > 0)
                {
                    schProductRouteResListNext.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iSchSN, p2.iSchSN); });
                    foreach (SchProductRouteRes ResNext in schProductRouteResListNext)
                    {
                        ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
                        ResNext.schProductRoute.ProcessSchTask();
                        if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate)
                            this.dBatchBegDate = ResNext.schProductRoute.dEndDate;
                    }
                }
                if (cBatchFilter != "")
                    schProductRouteResListNextAdd = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && cBatchFilter.IndexOf(p1.FResChaValue1ID) >= 0) && p1.FResChaValue1ID != "" && p1.iWoSeqID == iNextSeqID && p1.iSchBatch == this.iSchBatch && p1.iBatch != as_iBatch && p1.schProductRoute.dEarlyBegDate <= this.dBatchBegDate; });
                else    //为空时,不带条件
                    schProductRouteResListNextAdd = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0) && p1.iWoSeqID == iNextSeqID && p1.iSchBatch == this.iSchBatch && p1.FResChaValue1ID != "" && p1.iBatch != as_iBatch && p1.schProductRoute.dEarlyBegDate <= this.dBatchBegDate; });
                if (schProductRouteResListNextAdd.Count > 0)
                {
                    schProductRouteResListNextAdd.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iSchSN, p2.iSchSN); });
                    foreach (SchProductRouteRes ResNext in schProductRouteResListNextAdd)
                    {
                        ResNext.iBatch = as_iBatch;
                        ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
                        ResNext.schProductRoute.ProcessSchTask();
                        if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate)
                            this.dBatchBegDate = ResNext.schProductRoute.dEndDate;
                    }
                }
            } 
            List<SchProductRouteRes> schProductRouteResListSched = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 1 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.schProductRoute.cStatus != "4"); });
            schProductRouteResListSched.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
            if (schProductRouteResListSched.Count > 0)
                this.dBatchBegDate = schProductRouteResListSched[schProductRouteResListSched.Count - 1].dResEndDate;
            return 1;
        }
        public int KeyResBatchSchTaskSubFirst(int as_iBatch, ref SchProductRouteRes as_SchProductRouteResLast, List<SchProductRouteRes> as_schProductRouteResListNo)
        {
            DateTime dEndDateTemp = SchParam.dtToday;
            this.dBatchBegDate = dEndDateTemp;
            List<SchProductRouteRes> schProductRouteResListNext = new List<SchProductRouteRes>(10);
            for (int iNextSeqID = this.iBatchWoSeqID ; iNextSeqID < 80; iNextSeqID++)
            {
                schProductRouteResListNext = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.iWoSeqID == iNextSeqID); });
                if (schProductRouteResListNext.Count > 0)
                {
                    schProductRouteResListNext.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iSchSN, p2.iSchSN); });
                    foreach (SchProductRouteRes ResNext in schProductRouteResListNext)
                    {
                        ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
                        ResNext.schProductRoute.ProcessSchTask();
                        if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate)
                            this.dBatchBegDate = ResNext.schProductRoute.dEndDate;
                    }
                }
            }
            List<SchProductRouteRes> schProductRouteResListSched = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 1 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.schProductRoute.cStatus != "4"); });
            schProductRouteResListSched.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
            if (schProductRouteResListSched.Count > 0)
                this.dBatchBegDate = schProductRouteResListSched[schProductRouteResListSched.Count - 1].dResEndDate;
            return 1;
        }
        public void ResClearTask(SchProductRoute aSchProductRoute)
        {
            foreach (SchProductRouteRes as_SchProductRouteRes in aSchProductRoute.SchProductRouteResList)
            {
                this.ResClearTask(as_SchProductRouteRes);
            }
        }
        public void ResClearTask(SchProductRouteRes as_SchProductRouteRes)
        {
            List<TaskTimeRange> taskTimeRangeList1 = as_SchProductRouteRes.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.iSchSdID == as_SchProductRouteRes.iSchSdID && p.iProcessProductID == as_SchProductRouteRes.iProcessProductID && p.iResProcessID == as_SchProductRouteRes.iResProcessID; });
            foreach (TaskTimeRange taskTimeRange in taskTimeRangeList1)
            {
                taskTimeRange.TaskTimeRangeClear(as_SchProductRouteRes);
            }
            as_SchProductRouteRes.BScheduled = 0;
        }
        public int TaskComparer(SchProductRouteRes p1, SchProductRouteRes p2)
        {
            if (this.iSchBatch != 1)     //非执行生产任务单
            {
                if (this.cIsKey == "1" && this.cProChaType1Sort == "1")
                {
                    if (Comparer<double>.Default.Compare(p1.iResourceID, p2.iResourceID) == 0)
                    {
                        if (p1.resChaValue1 != null && p2.resChaValue1 != null && Comparer<int>.Default.Compare(p1.resChaValue1.FSchSN, p2.resChaValue1.FSchSN) == 0)
                            return 1;
                        else
                            if (p1.resChaValue1 == null || p2.resChaValue1 == null)
                                return 1;
                            else
                                return Comparer<int>.Default.Compare(p1.resChaValue1.FSchSN, p2.resChaValue1.FSchSN);
                    }
                    else
                    {
                        return Comparer<double>.Default.Compare(p1.iResourceID, p2.iResourceID);
                    }
                }
                else     //按工艺特征排序
                {
                    if (p1.resChaValue1 != null && p2.resChaValue1 != null && Comparer<int>.Default.Compare(p1.resChaValue1.FSchSN, p2.resChaValue1.FSchSN) == 0)
                    {
                        if (p1.resChaValue2 != null && p2.resChaValue2 != null && Comparer<int>.Default.Compare(p1.resChaValue2.FSchSN, p2.resChaValue2.FSchSN) == 0)
                        {
                            if (p1.resChaValue3 != null && p2.resChaValue3 != null && Comparer<int>.Default.Compare(p1.resChaValue3.FSchSN, p2.resChaValue3.FSchSN) == 0)
                            {
                                if (p1.resChaValue4 != null && p2.resChaValue4 != null && Comparer<int>.Default.Compare(p1.resChaValue4.FSchSN, p2.resChaValue4.FSchSN) == 0)
                                {
                                    if (p1.resChaValue5 != null && p2.resChaValue5 != null && Comparer<int>.Default.Compare(p1.resChaValue5.FSchSN, p2.resChaValue5.FSchSN) == 0)
                                    {
                                        if (p1.resChaValue6 != null && p2.resChaValue6 != null && Comparer<int>.Default.Compare(p1.resChaValue6.FSchSN, p2.resChaValue6.FSchSN) == 0)
                                        {
                                            return 1;
                                        }
                                        else
                                        {
                                            if (p1.resChaValue6 == null || p2.resChaValue6 == null)
                                                return 1;
                                            else
                                                return Comparer<int>.Default.Compare(p1.resChaValue6.FSchSN, p2.resChaValue6.FSchSN);
                                        }
                                    }
                                    else
                                    {
                                        if (p1.resChaValue5 == null || p2.resChaValue5 == null)
                                            return 1;
                                        else
                                            return Comparer<int>.Default.Compare(p1.resChaValue5.FSchSN, p2.resChaValue5.FSchSN);
                                    }
                                }
                                else
                                {
                                    if (p1.resChaValue4 == null || p2.resChaValue4 == null)
                                        return 1;
                                    else
                                    return Comparer<int>.Default.Compare(p1.resChaValue4.FSchSN, p2.resChaValue4.FSchSN);
                                }
                            }
                            else
                            {
                                if (p1.resChaValue3 == null || p2.resChaValue3 == null)
                                    return 1;
                                else
                                return Comparer<int>.Default.Compare(p1.resChaValue3.FSchSN, p2.resChaValue3.FSchSN);
                            }
                        }
                        else
                        {
                            if (p1.resChaValue2 == null || p2.resChaValue2 == null)
                                return 1;
                            else
                            return Comparer<int>.Default.Compare(p1.resChaValue2.FSchSN, p2.resChaValue2.FSchSN);
                        }
                    }
                    else
                        if (p1.resChaValue1 == null || p2.resChaValue1 == null)
                            return 1;
                        else
                            return Comparer<int>.Default.Compare(p1.resChaValue1.FSchSN, p2.resChaValue1.FSchSN);
                }
            }
            else      //已执行生产任务单,按已排计划生产批次、计划开工时间先后排序 2014-12-01
            {
                if (Comparer<string>.Default.Compare(p1.schProductRoute.schProduct.cMiNo, p2.schProductRoute.schProduct.cMiNo) == 0)
                {
                    if (Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate) == 0 )
                        return 1;
                    else
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                }
                else
                {
                    return Comparer<string>.Default.Compare(p1.schProductRoute.schProduct.cMiNo, p2.schProductRoute.schProduct.cMiNo);
                }
            }
            return 1;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        public int ResTaskComparer(SchProductRouteRes p1, SchProductRouteRes p2)
        {
            if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化，不考虑批次，按原来排产开工时间的先后顺序不动。
            {
                return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
            }
            else if (SchParam.cProChaType1Sort == "0")  //1 按资源任务优先级优化，不考虑批次号  
            {
                if (Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes) == 0)
                    return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                else
                    return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes);
            }
            else if (SchParam.cProChaType1Sort == "1")  //1 按工单需求日期优化，不考虑批次号
            {
                if (p1.schProductRoute.schProductWorkItem == null || p2.schProductRoute.schProductWorkItem == null)
                {
                    if (Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority) == 0)
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    else
                        return Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority);
                }
                else
                {
                    return Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate);
                }
            }
            else if (SchParam.cProChaType1Sort == "2")  //2 订单优先级，不考虑批次号
            {
                if (p1.schProductRoute.schProductWorkItem == null || p2.schProductRoute.schProductWorkItem == null)
                {
                    if (Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority) == 0)
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    else
                        return Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority);
                }
                else
                {
                    if (Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority) == 0)
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    else
                        return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority);
                }
            }
            else if (Comparer<double>.Default.Compare(p1.iSchBatch, p2.iSchBatch) == 0) //其他情况 排程批次相同 第1 考虑批次
            {
                if (SchParam.cProChaType1Sort == "3")  //3 座次优先级
                {                    
                    if (Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN)== 0)
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    else
                        return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN);
                }
                else if (SchParam.cProChaType1Sort == "4") //1 按工艺特征优化排序
                {
                    if (this.TaskComparer(p1, p2) == 0)
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    else
                        return this.TaskComparer(p1, p2);
                }
                else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                {                   
                    return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                }
            }
            else //排程批次不相同，直接按批次大小返回
            {
                return Comparer<double>.Default.Compare(p1.iSchBatch, p2.iSchBatch);
            }
            return 1;
        }
    }
}