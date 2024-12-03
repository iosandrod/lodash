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
        //public int iWcID;
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

        //排程资源数量取值
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
                //如果资源效率没有设置,默认为100% 2019-08-29
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
        public string iTurnsType { get; set; }            //轮换类型	 0 不轮换 1 按加工时间轮换 2 按任务数轮换
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
        //public string FProChaType7ID { get; set; }
        //public string FProChaType8ID { get; set; }
        //public string FProChaType9ID { get; set; }
        //public string FProChaType10ID { get; set; }
        //public string FProChaType11ID { get; set; }
        //public string FProChaType12ID { get; set; }
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
        //public List<SchProductRouteRes> SchProductRouteResList = new List<SchProductRouteRes>(10);

        //初始化
        public Resource()
        {

        }

        //初始化，传入资源明细行记录
        public Resource(DataRow drResource)
        {
            GetResource(drResource);
        }

        //初始化，传入资源编号
        public Resource(string cResourceNo, SchData as_SchData)
        {
            //            string lsSql = @"select iResourceID,cResourceNo,cResourceName,cResClsNo,cResourceType,iResourceNumber,cResOccupyType,isnull(iPreStocks,0) as iPreStocks,isnull(iPostStocks,0) iPostStocks,iUsage,iEfficient,isnull(cResouceInformation,'') as cResouceInformation ,cIsInfinityAbility,mResourcePicture,isnull(iWcID,-1) iWcID,cWcNo,cDeptNo,isnull(cDeptName,'') cDeptName,cStatus,isnull(iSchemeID,-1) iSchemeID,isnull(iCacheTime,0) iCacheTime,isnull(iLastBatchPercent,0) iLastBatchPercent ,
            //                    cIsKey,isnull(iKeySchSN,-1) as iKeySchSN,cNeedChanged,cProChaType1Sort,isnull(FProChaType1ID,-1) FProChaType1ID, isnull(FProChaType2ID,-1) FProChaType2ID,isnull(FProChaType3ID,-1) FProChaType3ID,isnull(FProChaType4ID,-1) FProChaType4ID,isnull(FProChaType5ID,-1) FProChaType5ID,isnull(FProChaType6ID,-1) FProChaType6ID,isnull(FProChaType7ID,-1) FProChaType7ID,isnull(FProChaType8ID,-1) FProChaType8ID,isnull(FProChaType9ID,-1) FProChaType9ID,isnull(FProChaType10ID,-1) FProChaType10ID,isnull(FProChaType11ID,-1) FProChaType11ID,isnull(FProChaType12ID,-1) FProChaType12ID,isnull(cDefine1,'') as cDefine1,isnull(cDefine2,'') as cDefine2,isnull(cDefine3,'') as cDefine3,isnull(cDefine4,'') as cDefine4,
            //                    isnull(cDefine5,'') as cDefine5,isnull(cDefine6,'') as cDefine6,isnull(cDefine7,'') as cDefine7,isnull(cDefine8,'') as cDefine8,isnull(cDefine9,'') as cDefine9,isnull(cDefine10,'') as cDefine10,isnull(cDefine11,0) as cDefine11,isnull(cDefine12,0) as cDefine12,isnull(cDefine13,0) as cDefine13,isnull(cDefine14,0) as cDefine14,isnull(cDefine15,'1900-01-01') as cDefine15,isnull(cDefine16,'1900-01-01') as cDefine16
            //                    from t_Resource 
            //                    where 1 = 1 and cResourceNo = '" + cResourceNo + "'";
            //            DataTable dtResource = Common.GetDataTable(lsSql);

            //            GetResource(dtResource.Rows[0]);
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
            //cResouceInformation  = drResource["cResouceInformation"].ToString();
            cIsInfinityAbility = drResource["cIsInfinityAbility"].ToString();
            if (cIsInfinityAbility == "") cIsInfinityAbility = "0";              //产能是否无限 0 否 1 是

            //public int iWcID;
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
            //FProChaType7ID = drResource["FProChaType7ID"].ToString();
            //FProChaType8ID = drResource["FProChaType8ID"].ToString();
            //FProChaType9ID = drResource["FProChaType9ID"].ToString();
            //FProChaType10ID = drResource["FProChaType10ID"].ToString();
            //FProChaType11ID = drResource["FProChaType11ID"].ToString();
            //FProChaType12ID = drResource["FProChaType12ID"].ToString();
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


        //当前资源所有时间可用段，初始由日历生成,没有关联任务 
        public List<ResTimeRange> ResTimeRangeList = new List<ResTimeRange>(10);

        //资源日产能计划列表 2023-10-05 
        public List<ResSourceDayCap> ResSourceDayCapList = new List<ResSourceDayCap>(10);

        //当前资源所有时间可用段备份,用于排产完成后，所有任务按每天集中处理，向前靠2，向后靠3，或正常1,重排
        public List<ResTimeRange> ResTimeRangeListBak = new List<ResTimeRange>(10);

        //当前资源所有特殊时间段（加班，休息，维修，报养），初始化时写入，更新ResTimeRangeList
        public List<ResTimeRange> ResSpecTimeRangeList = new List<ResTimeRange>(10);

        //当前资源未排产任务列表
        public List<SchProductRouteRes> schProductRouteResList = new List<SchProductRouteRes>(10);



        //资源任务时间段，包括空闲时段，和已排任务时段
        //public List<TaskTimeRange> TaskTimeRangeList = new List<TaskTimeRange>(10);
        public List<TaskTimeRange> GetTaskTimeRangeList(Boolean OrderASC = true)  //DateTime dBegDate
        {
            List<TaskTimeRange> ListTaskTimeRangeAll = new List<TaskTimeRange>(10);

            

            foreach (ResTimeRange ResTimeRange1 in this.ResTimeRangeList)
            {
                //ListTaskTimeRangeAll.AddRange(ResTimeRange1.TaskTimeRangeList);
                ListTaskTimeRangeAll.AddRange(ResTimeRange1.WorkTimeRangeList);
            }

            //排序
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

        //根据实际获取工作时间段
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
                    //ListTaskTimeRangeAll.AddRange(ResTimeRange1.TaskTimeRangeList);
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
                            //ListTaskTimeRangeAll.AddRange(ResTimeRange1.TaskTimeRangeList);
                            break;
                        }
                    }
                }
            }

            //排序
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

        //获取所有未排任务明细，按工艺特征排序
        public List<SchProductRouteRes> GetNotSchTask()
        {
            //1、关键资源排产 给生产任务按工艺特征进行排序 
            //SchProductRouteResList
            schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.BScheduled == 0 && p1.iSchBatch == this.iSchBatch ); });


            //按工艺特征优化排序，或者按物料排产顺序排序
            schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });

            return schProductRouteResList;
        }

        #region//资源时段初始化处理
        //合并时间段,正常时间段和有调整的时间段合并
        public void MergeTimeRange()
        {
            //1、先处理加班请况
            //找出当前资源所有加班时段
            List<ResTimeRange> ResSpecTimeRangeList1 = this.ResSpecTimeRangeList.FindAll(delegate(ResTimeRange p1) { return p1.Attribute == TimeRangeAttribute.Work || p1.Attribute == TimeRangeAttribute.Overtime || p1.Attribute == TimeRangeAttribute.MayOvertime; });

            ResSpecTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

            DateTime dCanBegDate = this.schData.dtStart;
            DateTime dCanEndDate = this.schData.dtEnd;

            //特殊资源工作日历,加班的
            foreach (ResTimeRange resTimeRange in ResSpecTimeRangeList1)
            {
                //1、增加可用时间段               
                //找出包含当前时间段的所有工作时段
                //List<ResTimeRange> lResTimeRangeList = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime <= resTimeRange.DBegTime && p2.DBegTime <= resTimeRange.DEndTime; });
                //List<ResTimeRange> lResTimeRangeList = lResTimeRangeList2.FindAll(delegate(ResTimeRange p2) { return  p2.DBegTime > resTimeRange.DEndTime; });
                dCanBegDate = this.schData.dtStart;
                dCanEndDate = this.schData.dtEnd;

                List<ResTimeRange> lResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime <= resTimeRange.DBegTime; });
                lResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });


                if (lResTimeRangeList1.Count > 0)
                    dCanBegDate = lResTimeRangeList1[0].DBegTime;  //由大到小排,取小于特殊时段开始日期最大的一个时间段

                //lResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime >= resTimeRange.DEndTime; });
                //lResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare( p1.DBegTime,p2.DBegTime); });

                //if (lResTimeRangeList1.Count > 0)
                //    dCanEndDate = lResTimeRangeList1[0].DEndTime;  //由小到大排,取大于特殊时段结束日期最小的一个时间段

                List<ResTimeRange> lResTimeRangeList = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime >= dCanBegDate; });
                lResTimeRangeList.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                ResTimeRange resLastTimeRange = null;  //上一时段




                //找出两工作时间段之间的空闲时间段，生成新的加班时间段 
                int iCount = lResTimeRangeList.Count;

                for (int i = 0; i < iCount; i++)
                {
                    ResTimeRange resWorkTimeRange = lResTimeRangeList[i];

                    if (i > 0) //找结束时间段,从第二段起，每段都生成一个空时间段
                    {
                        ResTimeRange resAddTimeRange = new ResTimeRange(); //

                        //时段开始日期
                        if (resTimeRange.DBegTime < resLastTimeRange.DEndTime)
                            resAddTimeRange.DBegTime = resLastTimeRange.DEndTime;  //新增加时段的开始时间
                        else
                            resAddTimeRange.DBegTime = resTimeRange.DBegTime;


                        //时段结束日期
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
                        //资源产能无限 0 有限，1 无限
                        resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility;


                        //resAddTimeRange.DBegTime = (DateTime)drResTimeRange["dPeriodBegTime"];
                        //resAddTimeRange.DEndTime = (DateTime)drResTimeRange["dPeriodEndTime"];                        
                        resAddTimeRange.Attribute = resTimeRange.Attribute;
                        resAddTimeRange.GetNoWorkTaskTimeRange(resAddTimeRange.DBegTime, resAddTimeRange.DEndTime, true);

                        //生成空的任务段                        
                        this.ResTimeRangeList.Add(resAddTimeRange);
                    }

                    resLastTimeRange = resWorkTimeRange;
                }
            }

            //2、后处理维修请况
            //找出当前资源所有维修时段
            List<ResTimeRange> ResSpecTimeRangeList2 = this.ResSpecTimeRangeList.FindAll(delegate(ResTimeRange p1) { return p1.Attribute == TimeRangeAttribute.Maintain || p1.Attribute == TimeRangeAttribute.Snag; });

            ResSpecTimeRangeList2.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

            //特殊资源工作日历,维修的
            foreach (ResTimeRange resTimeRange in ResSpecTimeRangeList2)
            {

                List<ResTimeRange> lResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime <= resTimeRange.DBegTime; });
                lResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });


                if (lResTimeRangeList1.Count > 0)
                    dCanBegDate = lResTimeRangeList1[0].DBegTime;  //由大到小排,取小于特殊时段开始日期最大的一个时间段
                else
                    dCanBegDate = this.schData.dtStart;

                //lResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime >= resTimeRange.DEndTime; });
                //lResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                //if (lResTimeRangeList1.Count > 0)
                //    dCanEndDate = lResTimeRangeList1[0].DEndTime;  //由小到大排,取大于特殊时段结束日期最小的一个时间段

                ////找出包含当前时间段的所有工作时段
                List<ResTimeRange> lResTimeRangeList = ResTimeRangeList.FindAll(delegate(ResTimeRange p2) { return p2.DBegTime >= dCanBegDate; }); //&& p2.DEndTime <= dCanEndDate
                lResTimeRangeList.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                ResTimeRange resLastTimeRange = null;  //上一时段

                //找出两工作时间段之间的空闲时间段，生成新的加班时间段 
                int iCount = lResTimeRangeList.Count;

                for (int i = 0; i < iCount; i++)
                {
                    ResTimeRange resWorkTimeRange = lResTimeRangeList[i];

                    if (resTimeRange.DEndTime >= resWorkTimeRange.DBegTime)
                    {
                        //从当前工作时间段中，排除其工作时间段
                        DeleteTimeRangeSub(resWorkTimeRange, resTimeRange);
                    }
                    else       //特殊处理超过时段结束时间，退出循环
                    {
                        break;
                    }
                }

            }
        }

        //增加工作时段
        public void MergeTimeRangeSub(ResTimeRange resWorkTimeRange, ResTimeRange resSpecTimeRange)
        {
            ResTimeRange resAddTimeRange = new ResTimeRange();
            //resAddTimeRange
            resAddTimeRange.CResourceNo = cResourceNo;
            resAddTimeRange.resource = this;
            //资源产能无限 0 有限，1 无限
            resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility;
            resAddTimeRange.Attribute = resSpecTimeRange.Attribute;


            if (resWorkTimeRange == null)   //增加一个时段
            {
                resAddTimeRange.DBegTime = resSpecTimeRange.DBegTime;
                resAddTimeRange.DEndTime = resSpecTimeRange.DEndTime;
            }
            else                        //
            {
                //加班时段 与 工作时段头部重叠
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

            //增加工作时段列表
            if (resAddTimeRange != null)
            {
                //生成空的任务段
                resAddTimeRange.GetNoWorkTaskTimeRange(resAddTimeRange.DBegTime, resAddTimeRange.DEndTime, false);
                this.ResTimeRangeList.Add(resAddTimeRange);
            }
        }

        //增加一个时段
        public void AddTimeRange(ResTimeRange resLastTimeRange, ResTimeRange resWorkTimeRange)
        {
            ResTimeRange resAddTimeRange = new ResTimeRange();
            //resAddTimeRange
            resAddTimeRange.CResourceNo = cResourceNo;
            resAddTimeRange.resource = this;
            //资源产能无限 0 有限，1 无限
            resAddTimeRange.CIsInfinityAbility = this.cIsInfinityAbility;
            //resAddTimeRange.Attribute = resSpecTimeRange.Attribute;
            resAddTimeRange.DBegTime = resLastTimeRange.DEndTime;
            resAddTimeRange.DEndTime = resLastTimeRange.DBegTime;

            //生成空的任务段
            resAddTimeRange.GetNoWorkTaskTimeRange(resAddTimeRange.DBegTime, resAddTimeRange.DEndTime, false);

            this.ResTimeRangeList.Add(resAddTimeRange);

        }

        //排除一个工作时段，设为不可用
        public void DeleteTimeRangeSub(ResTimeRange resWorkTimeRange, ResTimeRange resSpecTimeRange)
        {

            //resWorkTimeRange.GetNoWorkTaskTimeRange(DateTime dBegDate,DateTime dEndDate,Boolean bCreate = false )
            TaskTimeRange resAddTimeRange = new TaskTimeRange();

            //ResTimeRange resAddTimeRange = new ResTimeRange(); //

            //时段开始日期
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


            //时段结束日期
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


            ////维修时段 与 工作时段头部重叠
            //if (resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime && resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime)
            //{               
            //    resAddTimeRange = resWorkTimeRange.GetNoWorkTaskTimeRange(resSpecTimeRange.DBegTime, resWorkTimeRange.DEndTime); 

            //}//维修时段 与 工作时段尾部重叠
            //else if (resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime && resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime)
            //{               
            //    resAddTimeRange = resWorkTimeRange.GetNoWorkTaskTimeRange(resWorkTimeRange.DBegTime, resSpecTimeRange.DEndTime);

            //}//维修时段 与 工作时段全部不重叠，在工作时段之外
            //else if (resWorkTimeRange.DBegTime <= resSpecTimeRange.DBegTime && resWorkTimeRange.DEndTime > resSpecTimeRange.DBegTime)
            //{
            //    resAddTimeRange = null;

            //}//全部重叠,在工作时段之内,resSpecTimeRange全段不可用
            //else
            //{                
            //    resAddTimeRange = resWorkTimeRange.GetNoWorkTaskTimeRange(resSpecTimeRange.DBegTime, resSpecTimeRange.DEndTime); 
            //}

            ////TaskTimeRangeSplit(TaskTimeRange aToltalTaskRange, TaskTimeRange aNewTaskRange )

            //增加工作时段列表
            if (resAddTimeRange != null)
            {
                //分割当前工作时间段，去掉维修时间
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

        //生成资源日产能列表 2023-10-05 JonasCheng
        public void getResSourceDayCapList()
        {
            this.ResSourceDayCapList = new List<ResSourceDayCap>(10);

            ResSourceDayCap resSourceDayCap = new ResSourceDayCap();
            DateTime ldt_todayLast = DateTime.Now;

            // 使用LINQ按照成绩范围进行分组
            var groupedResTimeRange = this.ResTimeRangeList.GroupBy(resTimeRange => (resTimeRange.cResourceNo, resTimeRange.dPeriodDay));

            //var averageScore = groupedResTimeRange.Sum(resTimeRange => resTimeRange.a);
           // List<TaskTimeRange> resTimeRangeListAll = this.GetTaskTimeRangeList();

            //排序，由大到小
            this.ResTimeRangeList.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

            foreach (ResTimeRange ResTimeRange1 in this.ResTimeRangeList)
            {
                //每个资源每一天只生成一行记录
                if (ldt_todayLast != ResTimeRange1.dPeriodDay)
                {
                    resSourceDayCap = new ResSourceDayCap();
                    resSourceDayCap.dPeriodDay = ResTimeRange1.dPeriodDay;
                    resSourceDayCap.DBegTime = ResTimeRange1.DBegTime;
                    this.ResSourceDayCapList.Add(resSourceDayCap);
                }

                //
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

                //if (adCanBegDate < SchParam.dtStart) adCanBegDate = SchParam.dtStart;


                //--------2.1 找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于dSchStartDate--------------------------
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime >= adCanBegDate; });
                //必须要排序
                ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });


                //--------2.1 对所有可用资源段循环，在资源段内排生产任务-----------------------------------------------------
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    //不用作限制，所有时段都可排
                    if (ResTimeRangeList1[i].DBegTime > adCanEndDate) break;

                    //--------2.2 调用资源段排程-----------------------------------------------------
                    //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用                    
                    ResTimeRangeList1[i].TimeSchTaskFreezeInit(as_SchProductRouteRes, ref adCanBegDate, ref adCanEndDate);

                }

                //--------2.3 如果任务剩余工作时间大于0 ，则说明任务没有排完，提示排产失败-----------------------------------------------------                

                as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++;  //排产顺序号
                as_SchProductRouteRes.BScheduled = 1; //设为已排
                as_SchProductRouteRes.schProductRoute.BScheduled = 1; //设为已排

                //记录排产日志
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

        //已下达任务优化排产
        public int SchTaskSortInit(SchProductRouteRes as_SchProductRouteRes, DateTime adCanBegDate, DateTime adCanEndDate)
        {

            //先排前面
            as_SchProductRouteRes.schProductRoute.ProcessSchTaskPre(false);

            //加工时间重算,减去已完工部分
            int ai_workTime = 0;
            //Convert.ToInt32((as_SchProductRouteRes.iResReqQty - as_SchProductRouteRes.iActResReqQty) * as_SchProductRouteRes.iCapacity);//Convert.ToInt32(as_SchProductRouteRes.iResRationHour);   //工时已经秒
            double ai_ResReqQty = (as_SchProductRouteRes.iResReqQty - as_SchProductRouteRes.iActResReqQty);
            int iBatchCount = 0;

            if (as_SchProductRouteRes.cWorkType == "1")   //批量加工 
            {
                //不足一批的小数部分，当一批计算
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


            //int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;   //任务可以开始排产日期,任务有中断时，重新设置值


            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }

            try
            {
                //--------1.1 返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开-------------------------------
                //DateTime dtEndDate = adCanBegDate;
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                //DateTime adCanEndDateTest = adCanBegDate;
                int ai_workTimeTest = ai_workTime;
                int ai_disWorkTime = ai_workTime;
                int ai_ResPreTime = 0;   //资源换产时间
                int ai_CycTimeTol = 0;   //换刀时间
                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;


                //返回adCanBegDateTask，为任务可开始排产时间,作为正式排产开始日期;adCanBegDateTest 任务完工时间
                int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate);
                if (li_Return < 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                    throw new Exception(cError);
                    //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + adCanBegDateTest.ToLongTimeString() + ",请检查工作日历!");
                    return -1;
                }

                ////计算用adCanBegDate
                adCanBegDate = adCanBegDateTask;


                //--------2.1 找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于dSchStartDate--------------------------
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime > adCanBegDate; });
                //必须要排序
                ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                Boolean bFirtTime = true;            //是否第一个排产时间段

                //--------2.1 对所有可用资源段循环，在资源段内排生产任务-----------------------------------------------------
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    if (ai_workTime == 0) break;

                    //--------2.2 调用资源段排程-----------------------------------------------------
                    //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                    //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                    ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime,false);

                    if (bFirtTime)
                    {
                        //bFirtTime = false;
                        dtBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                    }

                }

                //--------2.3 如果任务剩余工作时间大于0 ，则说明任务没有排完，提示排产失败-----------------------------------------------------                
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    //
                    //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() + ",请检查工作日历!");
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


        //资源排产前处理
        public int ResSchBefore()
        {
            if (this.cResourceNo == "YQ-17-07")  //"YQ-17-07"
            {
                int j = 1;
            }

            //如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
            if (this.iSchBatch < 0)
            {
                //关键资源优化排产,不考虑分组和轮换
                this.KeyResSchTask();
                return -1;      //本批次不继续排产
            }

            return 1;
        }


        //资源排产后处理
        public int ResSchAfter()
        {            
            return 1;
        }

        //资源调度优化排产 2020-08-20 JonasCheng,正式版本 iSchBatch -100 全排
        //----------0、资源调度优化排产，按资源排产----------------
        public int ResDispatchSch(int iSchBatch)
        {
            //按资源任务状态（执行 2、待排产 1、计划 0、暂停 5）、同种状态任务优先级顺序排产/按资源工艺特征优化，
            //考虑前任务计划开工时间/不考虑；
            //
            //List<string> listSeqStatus = new List<string>{"2", "1", "0","5" };
            SchProductRouteRes SchProductRouteResPre = null;
            List<SchProductRouteRes> ListSchProductRouteRes = new List<SchProductRouteRes>();

            DateTime LastCanBegDate  = DateTime.Now;

            try
            {

                ////检查关键资源排产优先级相同的,必须轮换生产。                    
                if (this.cResourceNo == "42001")// "BC-04-06" || resource.cResourceNo == "BC-04-07")  //"BC-03-02"
                {
                    int j = 1;
                }

                ////1、第一次排产时调用，按资源优化排产，按排产批次分批优化排产 iSchBatch -100
                ////2、只排所有已下达任务订单 iSchBatch -200
                if (iSchBatch == -100)   //调度优化排产,第2次排产  iSchBatch =  -100-------------------------------------------------------------------------------
                {

                    //按资源的任务优先级 执行 2、待排产 1、计划 0、暂停 5
                    //foreach (var item in listSeqStatus)
                    //{
                    //按工艺特征优化排产 0 / 按任务优先级优化排产 1 (默认)

                    //按工序状态查找任务明细
                    //ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.schProductRoute.cStatus == item && p1.iResReqQty > 0; });

                    //当前资源所有任务，重新排产 2021-09-15 Jonas Cheng 
                    ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0; });


                    if (ListSchProductRouteRes.Count > 0)
                    {
                        //当前任务先设置为未排产 
                        foreach (var SchProductRouteResTemp in ListSchProductRouteRes)
                        {
                            ////正常排产时，调度优化
                            //if (SchParam.cSchType != "1")
                            //    SchProductRouteResTemp.iPriorityRes = Int32.Parse(SchProductRouteResTemp.iSchSN.ToString());  //设置资源任务排产顺序
                            //else
                            //    SchProductRouteResTemp.iPriorityRes = Int32.Parse(SchProductRouteResTemp.schProductRoute.schProduct.iSchSN.ToString());  //设置资源任务排产顺序

                            //如果工序已完工，不用排了
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

                                //清空上次排产时间
                                SchProductRouteResTemp.TaskClearTask();
                            }
                        }

                        //9 按资源定义排程优化方式为准

                        //if (this.cProChaType1Sort == "0")  //1 按资源优先级优化
                        //{
                        //    //1 按资源任务优先级iPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes); });

                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "1")  //1 按工单优先级优化
                        //{
                        //    //1 按工单任务优先级iWoPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iWoPriorityRes, p2.schProductRoute.schProductWorkItem.iWoPriorityRes); });

                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "2")  //2 订单优先级
                        //{
                        //    //1 按资源任务优先级iPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority); });

                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "3")  //3 座次优先级
                        //{
                        //    //1 按资源任务优先级iPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "4") //1 按工艺特征优化排序
                        //{
                        //    //按工艺特征优化排序，或者按物料排产顺序排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                        //}
                        //else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                        //{

                        //    //ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}

                        //调用资源任务排序函数
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return ResTaskComparer(p1, p2); });

                        //每个任务排产，只能按排在上个任务之后，按顺序生产                    
                        //foreach (SchProductRouteRes as_SchProductRouteRes in ListSchProductRouteRes)
                        for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                        {
                            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                            {
                                SchProductRouteResPre = ListSchProductRouteRes[i];
                                continue;
                            }

                            //调试
                            if (ListSchProductRouteRes[i].iSchSdID == SchParam.iSchSdID && ListSchProductRouteRes[i].iProcessProductID == SchParam.iProcessProductID)
                            {
                                int m;
                                m = 1;
                            }

                            //直接赋值，取当前资源的上个任务，用于计算换模时间
                            ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;

                            //this.ResDispatchSchTask(as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true)
                            ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);

                            //记录上个任务的结束时间
                            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;

                            //记录上次加工任务
                            SchProductRouteResPre = ListSchProductRouteRes[i];

                        }

                    }

                }
                else if (iSchBatch == -200)    //只排正式版本，智能排产控制台正常排产前调用,替代原来冻结任务功能
                {
                    //当前资源所有任务，重新排产 2021-09-27 Jonas Cheng 
                    ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0 && p1.cVersionNo.Trim().ToLower() == "sureversion"; });


                    if (ListSchProductRouteRes.Count > 0)
                    {

                        //9 按资源定义排程优化方式为准                        
                        //调用资源任务排序函数
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2){ return ResTaskComparer(p1, p2);});


                        //每个任务排产，只能按排在上个任务之后，按顺序生产
                        for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                        {
                            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                            {
                                SchProductRouteResPre = ListSchProductRouteRes[i];
                                continue;
                            }

                            //调试
                            if (ListSchProductRouteRes[i].iSchSdID == SchParam.iSchSdID && ListSchProductRouteRes[i].iProcessProductID == SchParam.iProcessProductID)
                            {
                                int m;
                                m = 1;
                            }


                            //直接赋值，取当前资源的上个任务，用于计算换模时间
                            ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;

                            //考虑前工序的最早可开工时间 2021-09-27 
                            if (SchParam.ExecTaskSchType == "2")  //2 已执行计划重排,考虑前工序完工时间，不考虑前个任务单结束时间，如果前面有空闲可用时间，可以插单;
                            {
                                LastCanBegDate = SchParam.dtStart;
                                LastCanBegDate = this.GetTaskCanBegDate(ListSchProductRouteRes[i], LastCanBegDate);
                            }

                            //this.ResDispatchSchTask(as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true)
                            ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);

                            //记录上个任务的结束时间
                            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;

                            //记录上次加工任务
                            SchProductRouteResPre = ListSchProductRouteRes[i];

                        }

                    }
                }
                else  //1、第一次排产时调用，按资源优化排产，按排产批次分批优化排产 iSchBatch -100
                {                   
                    //按工序状态查找任务明细
                    ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iSchBatch == iSchBatch && p1.iResReqQty > 0; });


                    if (ListSchProductRouteRes.Count > 0)
                    {
                        //当前任务先设置为未排产 
                        foreach (var SchProductRouteResTemp in ListSchProductRouteRes)
                        {
                            SchProductRouteResTemp.iPriorityRes = SchProductRouteResTemp.iSchSN;  //设置资源任务排产顺序
                            SchProductRouteResTemp.cDefine25 = "";                                 //工艺特征转换说明清空
                            SchProductRouteResTemp.iResPreTime = 0;                                //前准备时间清空，重算
                            SchProductRouteResTemp.SchProductRouteResPre = null;                   //清空资源前一个任务

                            SchProductRouteResTemp.BScheduled = 0;
                            SchProductRouteResTemp.schProductRoute.BScheduled = 0;

                            //清空上次排产时间
                            SchProductRouteResTemp.TaskClearTask();
                        }

                        //if (this.cProChaType1Sort == "0")  //1 按资源优先级优化
                        //{
                        //    //1 按资源任务优先级iPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes); });

                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "1")  //1 按工单优先级优化
                        //{
                        //    //1 按工单任务优先级iWoPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iWoPriorityRes, p2.schProductRoute.schProductWorkItem.iWoPriorityRes); });


                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "2")  //2 订单优先级
                        //{
                        //    //1 按资源任务优先级iPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority); });

                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "3")  //3 座次优先级
                        //{
                        //    //1 按资源任务优先级iPriorityRes 由小到大排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                        //    // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}
                        //else if (this.cProChaType1Sort == "4") //1 按工艺特征优化排序
                        //{
                        //    //按工艺特征优化排序，或者按物料排产顺序排序
                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                        //}
                        //else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                        //{

                        //    //ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

                        //    ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                        //}

                        //调用资源任务排序函数
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return ResTaskComparer(p1, p2); });


                        //每个任务排产，只能按排在上个任务之后，按顺序生产                    
                        //foreach (SchProductRouteRes as_SchProductRouteRes in ListSchProductRouteRes)
                        for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                        {

                            if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                            {
                                SchProductRouteResPre = ListSchProductRouteRes[i];
                                continue;
                            }

                            //调试
                            if (ListSchProductRouteRes[i].iSchSdID == SchParam.iSchSdID && ListSchProductRouteRes[i].iProcessProductID == SchParam.iProcessProductID)
                            {
                                int m;
                                m = 1;
                            }

                            //直接赋值，取当前资源的上个任务，用于计算换模时间
                            ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;

                            //考虑前工序的最早可开工时间 2021-09-27 
                            if (SchParam.ExecTaskSchType == "2")  //2 已执行计划重排,考虑前工序完工时间;
                                LastCanBegDate = this.GetTaskCanBegDate(ListSchProductRouteRes[i], LastCanBegDate);

                            //this.ResDispatchSchTask(as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true)
                            ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);

                            //记录上个任务的结束时间
                            LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;

                            //记录上次加工任务
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

        //按资源排产时，取当前任务对应工序的可开工时间 2021-09-27 JonasCheng
        public DateTime GetTaskCanBegDate(SchProductRouteRes schProductRouteRes, DateTime LastCanBegDate)
        {
            DateTime dCanBegDate = LastCanBegDate;
            DateTime dCanBegDateTemp = LastCanBegDate;

            //循环前工序任务,找到最晚可开工日期
            foreach (SchProductRoute schProductRoutePre in schProductRouteRes.schProductRoute.SchProductRoutePreList)
            {
                if (schProductRoutePre.BScheduled == 0) schProductRoutePre.ProcessSchTask();

                dCanBegDateTemp = schProductRoutePre.GetNextProcessCanBegDate(schProductRouteRes.schProductRoute);

                if (dCanBegDateTemp > dCanBegDate) dCanBegDate = dCanBegDateTemp;


            } 

            return dCanBegDate;
        }

        //按选择资源优化排产(正式版本)
        public int ResDispatchSchWo(int iSchBatch)
        {
            //按资源任务状态（执行 2、待排产 1、计划 0、暂停 5）、同种状态任务优先级顺序排产/按资源工艺特征优化，
            //考虑前任务计划开工时间/不考虑；
            //
            //List<string> listSeqStatus = new List<string> { "2","0"};
            SchProductRouteRes SchProductRouteResPre = null;
            List<SchProductRouteRes> ListSchProductRouteRes = new List<SchProductRouteRes>();

            DateTime LastCanBegDate = DateTime.Now;

            ////1、按资源的任务优先级 执行 2、待排产 1、计划 0、暂停 5
            //foreach (var item in listSeqStatus)
            //{
                //按工艺特征优化排产 0 / 按任务优先级优化排产 1 (默认)

                //按工序状态查找任务明细
                ListSchProductRouteRes = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0; });


                if (ListSchProductRouteRes.Count > 0)
                {
                    //当前任务先设置为未排产 
                    foreach (var SchProductRouteResTemp in ListSchProductRouteRes)
                    {
                        ////正常排产时，调度优化
                        //if (SchParam.cSchType != "1")
                        //    SchProductRouteResTemp.iPriorityRes = Int32.Parse(SchProductRouteResTemp.iSchSN.ToString());  //设置资源任务排产顺序
                        //else
                        //    SchProductRouteResTemp.iPriorityRes = Int32.Parse(SchProductRouteResTemp.schProductRoute.schProduct.iSchSN.ToString());  //设置资源任务排产顺序


                        SchProductRouteResTemp.cDefine25 = "";                                 //工艺特征转换说明清空
                        SchProductRouteResTemp.iResPreTime = 0;                                //前准备时间清空，重算
                        SchProductRouteResTemp.SchProductRouteResPre = null;                   //清空资源前一个任务

                        SchProductRouteResTemp.BScheduled = 0;
                        SchProductRouteResTemp.schProductRoute.BScheduled = 0;

                        //清空上次排产时间
                        SchProductRouteResTemp.TaskClearTask();
                    }


                //9 按资源定义排程优化方式为准
                if (SchParam.cProChaType1Sort == "9")  //9 按资源定义排程优化方式为准
                {
                    if (this.cProChaType1Sort == "0")  //1 按资源优先级优化
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (this.cProChaType1Sort == "1")  //1 按工单优先级优化
                    {
                        //1 按工单任务优先级iWoPriorityRes 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iWoPriorityRes, p2.schProductRoute.schProductWorkItem.iWoPriorityRes); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (this.cProChaType1Sort == "2")  //2 订单优先级
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (this.cProChaType1Sort == "3")  //3 座次优先级
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (this.cProChaType1Sort == "4") //1 按工艺特征优化排序
                    {
                        //按工艺特征优化排序，或者按物料排产顺序排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                    }
                    else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                    {

                        //ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                }
                else  //以排产参数定义工艺特征为准，所有资源统一一种参数
                {

                    //0 按资源工艺特征优化排产，
                    if (SchParam.cProChaType1Sort == "0")  //1 按资源任务优先级优化
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "1")  //1 按工单需求日期优化
                    {
                        //1 按工单需求日期优化 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "2")  //2 订单需求日期 2022-04-06 JonasCheng
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "3")  //3 座次优先级
                    {
                        //1 按资源任务优先级iPriorityRes 由小到大排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                        // ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }
                    else if (SchParam.cProChaType1Sort == "4") //1 按工艺特征优化排序
                    {
                        //按工艺特征优化排序，或者按物料排产顺序排序
                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                    }
                    else //if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化
                    {

                        //ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

                        ListSchProductRouteRes.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
                    }

                }


                //每个任务排产，只能按排在上个任务之后，按顺序生产                    
                //foreach (SchProductRouteRes as_SchProductRouteRes in ListSchProductRouteRes)
                for (int i = 0; i < ListSchProductRouteRes.Count; i++)
                {

                    if (ListSchProductRouteRes[i].schProductRoute.BScheduled == 1)
                    {
                        SchProductRouteResPre = ListSchProductRouteRes[i];
                        continue;
                    }

                    //直接赋值，取当前资源的上个任务，用于计算换模时间
                    ListSchProductRouteRes[i].SchProductRouteResPre = SchProductRouteResPre;

                    //this.ResDispatchSchTask(as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true)
                    ListSchProductRouteRes[i].DispatchSchTask(ref ListSchProductRouteRes[i].iResReqQty, LastCanBegDate, SchProductRouteResPre);

                    //记录上个任务的结束时间
                    LastCanBegDate = ListSchProductRouteRes[i].dResEndDate;

                    //记录上次加工任务
                    SchProductRouteResPre = ListSchProductRouteRes[i];

                }


                //}


            }

            return 1;
        }



        //----------0.1、正排 给资源分配任务，生成资源任务时段占用列表----------------
        public int ResDispatchSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true)
        {
            //int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;   //任务可以开始排产日期,任务有中断时，重新设置值

            DateTime ldtBeginDate = DateTime.Now;
            string message = "";

            //SchParam.iSchSNMax + 1 == SchParam.iProcessProductID ||
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
                //--------1.1 返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开-------------------------------
                //DateTime dtEndDate = adCanBegDate;
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                //DateTime adCanEndDateTest = adCanBegDate;
                int ai_workTimeTest = ai_workTime;
                int ai_disWorkTime = ai_workTime;
                //int ai_ResPreTime = 0;
                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;

                SchParam.ldtBeginDate = DateTime.Now;

                //返回adCanBegDateTask，为任务可开始排产时间,作为正式排产开始日期;adCanBegDateTest 任务完工时间
                int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate, bReCalWorkTime);
                if (li_Return < 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                    throw new Exception(cError);
                    //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + adCanBegDateTest.ToLongTimeString() + ",请检查工作日历!");
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


                ////计算用adCanBegDate
                adCanBegDate = adCanBegDateTask;


                //--------2.1 找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于dSchStartDate--------------------------
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate (ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime > adCanBegDate; });
                //必须要排序
                ResTimeRangeList1.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                Boolean bFirtTime = true;            //是否第一个排产时间段

                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.3、ResTimeRangeList1 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }

                //--------2.1 对所有可用资源段循环，在资源段内排生产任务-----------------------------------------------------
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    //if (ai_workTime <= 0) break;
                    //如果当前时段可用时间比最小空闲时间还小而且当前任务排不下时，直接选择下个时间段。减少循环，提高排产效率。2020-1-6 JonasCheng
                    //
                    if (bFirtTime && ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime && ai_workTime > ResTimeRangeList1[i].AvailableTime)
                        continue;

                    DateTime ldtBeginDateRessource = DateTime.Now;
                    if (as_SchProductRouteRes.cSeqNote == "折弯")
                    {
                        ldtBeginDateRessource = DateTime.Now;
                    }



                    //--------2.2 调用资源段排程-----------------------------------------------------
                    //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                    //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                    ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime);
                    Double iWaitTime = DateTime.Now.Subtract(ldtBeginDateRessource).TotalMilliseconds;

                    if (as_SchProductRouteRes.cSeqNote == "折弯")
                    {
                        iWaitTime = iWaitTime;
                    }

                    if (bFirtTime)
                    {
                        //bFirtTime = false;
                        dtBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                    }

                    if (ai_workTime <= 0) break;

                    ////--------2.3 检查任务是否连续，否则清除，重排-----------------------------------------------------
                    //if (this.cIsInfinityAbility == "0")   //资源产能有限,检查任务是否连续
                    //{
                    //    //未排完，而且中间是否间隔有其他任务，则必须从下个时间段开始排
                    //    if (SchParam.TaskSchNotBreak == 1 && ai_workTime > 0)   //未排完不能中断
                    //    {
                    //        if (ResTimeRangeList1[i].CheckTaskOverlap(as_SchProductRouteRes, adCanBegDate, false) < 0)  //任务有重叠
                    //        {
                    //            //之前已生成的时间段也清除
                    //            as_SchProductRouteRes.TaskClearTask();

                    //            //当前任务重新再排
                    //            //adCanBegDate = TaskTimeRange3.DEndTime;
                    //            ai_workTime = ai_workTimeTask;
                    //            //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                    //            ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, adCanBegDateTask);
                    //        }

                    //    }
                    //}

                }

                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.4、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }

                //--------2.3 如果任务剩余工作时间大于0 ，则说明任务没有排完，提示排产失败-----------------------------------------------------                
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    //
                    //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() + ",请检查工作日历!");
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

                    //记录排产日志
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



        //= new ArrayList<ResTimeRange>(10)
        //----------1、正排 给资源分配任务，生成资源任务时段占用列表----------------

        public int ResSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate, ref int ai_ResPreTime, ref int ai_CycTimeTol, Boolean bReCalWorkTime = true , SchProductRouteRes as_SchProductRouteResPre = null )
        {
            //int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;   //任务可以开始排产日期,任务有中断时，重新设置值

            DateTime ldtBeginDate = DateTime.Now;
            string message = "";
            double iWaitTime;

            //SchParam.iSchSNMax + 1 == SchParam.iProcessProductID ||
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
                //--------1.1 返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开-------------------------------
                //DateTime dtEndDate = adCanBegDate;
                DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
                //DateTime adCanEndDateTest = adCanBegDate;
                int ai_workTimeTest = ai_workTime;
                int ai_disWorkTime = ai_workTime;
                //int ai_ResPreTime = 0;
                DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;

                SchParam.ldtBeginDate = DateTime.Now;
                ldtBeginDate = DateTime.Now;

                //返回adCanBegDateTask，为任务可开始排产时间,作为正式排产开始日期;adCanBegDateTest 任务完工时间
                //4 按资源已排程天数选择，越少越优先,不用每个任务模拟排产一遍，节省运算量 2022-11-23
                //if (SchParam.cMutResourceType != "4")
                //{
                    int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate, bReCalWorkTime, true, as_SchProductRouteResPre);
                    if (li_Return < 0)
                    {
                        string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                        throw new Exception(cError);
                        //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + adCanBegDateTest.ToLongTimeString() + ",请检查工作日历!");
                        return -1;
                    }
                //}


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


                ////计算用adCanBegDate
                adCanBegDate = adCanBegDateTask;


                //--------2.1 找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于dSchStartDate--------------------------
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.AvailableTime > 0 && p.DEndTime > adCanBegDate; });
                //必须要排序
                ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                Boolean bFirtTime = true;            //是否第一个排产时间段

                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.3、ResTimeRangeList1 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }

                //--------2.1 对所有可用资源段循环，在资源段内排生产任务-----------------------------------------------------
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    //if (ai_workTime <= 0) break;
                    //如果当前时段可用时间比最小空闲时间还小而且当前任务排不下时，直接选择下个时间段。减少循环，提高排产效率。2020-1-6 JonasCheng
                    //
                    if (bFirtTime && ResTimeRangeList1[i].AvailableTime < SchParam.PeriodLeftTime && ai_workTime > ResTimeRangeList1[i].AvailableTime )
                        continue;

                    DateTime ldtBeginDateRessource = DateTime.Now;
                    if (as_SchProductRouteRes.cSeqNote == "折弯")
                    {
                        ldtBeginDateRessource = DateTime.Now;
                    }

                    DateTime ldtBeginDate2 = DateTime.Now;

                    //--------2.2 调用资源段排程-----------------------------------------------------
                    //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                    //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                    try
                    {
                        
                        
                        ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime);
                    }
                    catch (Exception error)
                    {
                        throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResTimeRangeList1！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                        return -1;
                    }

                    //ldtEndedDate = DateTime.Now;

                    //interval = ldtEndedDate - ldtBeginDate2;//计算间隔时间

                    //double iWaitTime = interval.TotalMilliseconds;//DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;

                    //Double iWaitTime = DateTime.Now.Subtract(ldtBeginDateRessource).TotalMilliseconds;

                    //if (as_SchProductRouteRes.cSeqNote == "折弯")
                    //{
                    //    iWaitTime = iWaitTime;
                    //}

                    if (bFirtTime)
                    {
                        //bFirtTime = false;
                        dtBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        as_SchProductRouteRes.dResLeanBegDate = ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                    }

                    if (ai_workTime <= 0) break;

                    ////--------2.3 检查任务是否连续，否则清除，重排-----------------------------------------------------
                    //if (this.cIsInfinityAbility == "0")   //资源产能有限,检查任务是否连续
                    //{
                    //    //未排完，而且中间是否间隔有其他任务，则必须从下个时间段开始排
                    //    if (SchParam.TaskSchNotBreak == 1 && ai_workTime > 0)   //未排完不能中断
                    //    {
                    //        if (ResTimeRangeList1[i].CheckTaskOverlap(as_SchProductRouteRes, adCanBegDate, false) < 0)  //任务有重叠
                    //        {
                    //            //之前已生成的时间段也清除
                    //            as_SchProductRouteRes.TaskClearTask();

                    //            //当前任务重新再排
                    //            //adCanBegDate = TaskTimeRange3.DEndTime;
                    //            ai_workTime = ai_workTimeTask;
                    //            //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                    //            ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, adCanBegDateTask);
                    //        }

                    //    }
                    //}

                }


                //ldtEndedDate = DateTime.Now;
                //interval = ldtEndedDate - ldtBeginDate;//计算间隔时间

                //SchParam.iWaitTime2 = interval.TotalMilliseconds;//DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;

                if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.schProductRoute.iSchSdID == SchParam.iSchSdID || as_SchProductRouteRes.iProcessProductID == 193864 && as_SchProductRouteRes.schProductRoute.iSchSdID == 1070)  //调试断点1 SchProduct
                {
                    message = string.Format(@"3.4、TimeSchTask 排产顺序[{0}],计划ID[{1}],任务ID[{2}],资源编号[{3}],开始排产时间[{4}],完成排产时间[{5}]",
                                                            as_SchProductRouteRes.iSchSN, as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.cResourceNo, DateTime.Now, SchData.GetDateDiffString(ldtBeginDate, DateTime.Now, "ms"));
                    SchParam.Debug(message, "资源运算");
                    ldtBeginDate = DateTime.Now;
                }

                //--------2.3 如果任务剩余工作时间大于0 ，则说明任务没有排完，提示排产失败-----------------------------------------------------                
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    //
                    //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() + ",请检查工作日历!");
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

                    //记录排产日志
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

                //ldtEndedDate = DateTime.Now;
                //interval = ldtEndedDate - ldtBeginDate;//计算间隔时间

                //SchParam.iWaitTime3 = interval.TotalMilliseconds;//DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;
            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }


            return 1; //剩下未排时间

        }

        //----------2、倒排 给资源分配任务，生成资源任务时段占用列表----------------
        public int ResSchTaskRev(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, DateTime adCanBegDate)
        {
            //int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeTask = ai_workTime;
            DateTime adCanBegDateTask = adCanBegDate;    //任务可以开始排产日期,任务有中断时，重新设置值
            DateTime dtBegDate = adCanBegDate, dtEndDate = adCanBegDate;

            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;

                List<ResTimeRange> ResTimeRangeListTest = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime <= adCanBegDate; });
            }

            //#if DEBUG
            //            System.IO.StreamWriter _file = new System.IO.StreamWriter(@"C:\ResourceDebug.txt", true);           // 测试用 
            //#endif
            ////返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开
            //DateTime dtEndDate = adCanEndDate;
            //DateTime dSchStartDate = GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanEndDate, true, ref dtEndDate);

            //--------1.1 返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开-------------------------------
            //DateTime dtEndDate = adCanBegDate;
            DateTime adCanBegDateTest = adCanBegDate; //= GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);
            //DateTime adCanEndDateTest = adCanBegDate;
            int ai_workTimeTest = ai_workTime;
            int ai_ResPreTime = 0;                //资源前准备时间
            int ai_CycTimeTol = 0;                //换刀时间  ,倒排时不用


            SchParam.ldtBeginDate = DateTime.Now;

            try
            {
                //SchParam.Debug("1.11111111 ResSchTaskRev TestResSchTask", "资源运算");

                //返回adCanBegDateTask，为任务可开始排产时间,作为正式排产开始日期;adCanBegDateTest 任务完工时间
                int li_Return = this.TestResSchTask(as_SchProductRouteRes, ref ai_workTimeTest, ref adCanBegDateTest, ref adCanBegDateTask, true, ref ai_ResPreTime, ref ai_CycTimeTol, ref dtBegDate, ref dtEndDate);
                if (li_Return < 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, adCanBegDateTest);
                    throw new Exception(cError);
                    //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],最大时间" + adCanBegDateTest.ToLongTimeString() + ",请检查工作日历!");
                    return -1;
                }
                //SchParam.iWaitTime5 = DateTime.Now.Subtract(SchParam.ldtBeginDate).TotalMilliseconds;

                //SchParam.Debug("1.22222222 ResSchTaskRev TestResSchTask", "资源运算");

                ////计算用adCanBegDate
                adCanBegDate = adCanBegDateTask;

                //找出所有 可用时间大于0的时间段 , 注意必须时段开始时间必须小于adCanEndDate
                List<ResTimeRange> ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate (ResTimeRange p) { return p.AvailableTime > 0 && p.DBegTime <= adCanBegDate; });
                //必须要排序
                ResTimeRangeList1.Sort(delegate (ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });

           
           
                Boolean bFirtTime = true;            //是否第一个排产时间段

                //SchParam.Debug("1.33333333 ResSchTaskRev TestResSchTask", "资源运算");

                //倒排,从最大时段开始排
                for (int i = 0; i < ResTimeRangeList1.Count; i++)
                {
                    if (ai_workTime == 0) break;

                    //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                    //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                    if(as_SchProductRouteRes.schProductRoute.schProduct.cSchType != "2" )  //有限产能倒排
                        ResTimeRangeList1[i].TimeSchTaskRev(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref bFirtTime);
                    else    //无限产能倒排
                        ResTimeRangeList1[i].TimeSchTaskRevInfinite(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, true, ref bFirtTime);

                    ////未排完，而且中间是否间隔有其他任务，则必须从下个时间段开始排
                    //if (this.cIsInfinityAbility == "0")   //资源产能有限,检查任务是否连续
                    //{
                    //    if (SchParam.TaskSchNotBreak == 1 && ai_workTime > 0)   //未排完不能中短
                    //    {
                    //        //任务是否连续，有重叠
                    //        if (ResTimeRangeList1[i].CheckTaskOverlap(as_SchProductRouteRes, adCanEndDate, true) < 0)
                    //        {
                    //            //之前已生成的时间段也清除
                    //            as_SchProductRouteRes.TaskClearTask();

                    //            //当前任务重新再排
                    //            //adCanBegDate = TaskTimeRange3.DEndTime;
                    //            ai_workTime = ai_workTimeTask;
                    //            //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                    //            ResTimeRangeList1[i].TimeSchTaskRev(as_SchProductRouteRes, ref ai_workTime, ref adCanEndDate, ai_workTimeTask, adCanEndDateTask);
                    //        }

                    //    }
                    //}

                }

                //SchParam.Debug("1.4444444444 ResSchTaskRev TestResSchTask", "资源运算");

                //如果任务剩余工作时间大于0 ，则说明任务没有排完，提示排产失败
                if (ai_workTime > 0)
                {
                    string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                    throw new Exception(cError);
                    //throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],生产时间[" + ai_workTimeTask / 3600 + "]小时,最大可用时间[" + adCanBegDate.ToShortDateString() + " " + adCanBegDate.ToLongTimeString() + "],请检查工作日历,或单件产能太大!"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +

                    return -1;
                }
                else
                {
                    as_SchProductRouteRes.iSchSN = SchParam.iSchSNMax++;  //排产顺序号
                    as_SchProductRouteRes.BScheduled = 1; //设为已排

                    //记录排产日志
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

                //SchParam.Debug("1.55555555 ResSchTaskRev TestResSchTask", "资源运算");

            }
            catch (Exception error)
            {
                throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源倒排计算时出错,位置Resource.ResSchTaskRev！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                return -1;
            }

            //#if DEBUG
            //            _file.Close();           // 测试用 
            //#endif

            return 1; //剩下未排时间

        }


        //----------1、正排 给资源分配任务，生成资源任务时段占用列表----------------
        public int TestResSchTask(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, ref DateTime adCanBegDateTask, Boolean bSchRev, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref DateTime dtBegDate, ref DateTime dtEndDate, Boolean bShowTips = true, Boolean bReCalWorkTime = true, SchProductRouteRes as_SchProductRouteResPre = null )
        {
            //int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeTask = ai_workTime;
            int ai_disWorkTime = ai_workTime;
            DateTime adCanBegDateTask2 = adCanBegDateTask;
            //DateTime dtEndDate = adCanBegDate;            
            //int ai_ResPreTime = 0;              //资源前准备时间
            int ai_ResPostTime = 0;             //资源后准备时间
            dtEndDate = adCanBegDate;

            DateTime ldtBeginDate = DateTime.Now;

            //SchParam.Debug("2.11111 TestResSchTask", "资源运算");

            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
            {
                int i = 1;
            }

            try
            {
                //--------1.1 返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开-------------------------------

                //DateTime dSchStartDate = GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);

                //计算用adCanBegDate
                //adCanBegDate = dSchStartDate;

                //--------2.1 找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于dSchStartDate--------------------------
                List<ResTimeRange> ResTimeRangeList1 = new List<ResTimeRange>();

                if (bSchRev == false)   //正排
                {

                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DEndTime > adCanBegDateTask2; }); // p.AvailableTime > 0 &&

                    //SchParam.Debug("2.2222 TestResSchTask bSchRev", "资源运算");

                    //测试排产时，把已排任务也放进去，中间空闲时间段排不下时，只能另找时间段排产。2019-08-30 Jonas 
                    if (ResTimeRangeList.Count < 1)
                    {
                        if (bShowTips)  //显示提示
                        {
                            string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],请检查是否有工作日历或当前资源是资源组!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID);
                            throw new Exception(cError);
                        }
                        return -1;
                    }


                    //必须要排序
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                    Boolean bFirtTime = true;            //是否第一个排产时间段
                    ResTimeRange resTimeRangeNext = null;       //下个空闲时间段，同时传入    


                    //DateTime ldtEndDate22 = DateTime.Now;
                    //TimeSpan interval22 = ldtEndDate22 - ldtBeginDate;
                    //SchParam.iWaitTime2 = interval22.TotalMilliseconds;//计算间隔时间

                    //SchParam.Debug("2.33333333333 TestResSchTask bSchRev", "资源运算");

                    //--------2.1 对所有可用资源段循环，在资源段内排生产任务-----------------------------------------------------
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;

                        ////如果不是第一个时间段，当前空闲时间段小于待排任务，而且已经排了其他任务，排不下。此时需要重新选择时间段 2019-09-09

                        //if (ResTimeRangeList1[i].WorkTimeRangeList.Count > 0 )
                        //{
                        //    if (ResTimeRangeList1[i].NotWorkTime == 0)
                        //    {
                        //        bFirtTime = true;                                 //是否第一个排产时间段
                        //        ai_workTime = ai_workTimeTask;                    //返回原值      
                        //        adCanBegDate = ResTimeRangeList1[i].DEndTime; //ResTimeRangeList1[i + 1].DBegTime;     //重置可开工时间,已排任务之后  
                        //        continue;
                        //    }

                        //    //在已排任务中，找是否小于当前空闲时间段的任务,如果找到，说明中间插了任务，必须得重排

                        //    List<TaskTimeRange> ResTimeRangeListTemp = ResTimeRangeList1[i].WorkTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.DBegTime < ResTimeRangeList1[i].DEndTime ; });  //p.AvailableTime > 0

                        //    if (ResTimeRangeListTemp.Count > 0 )
                        //    {
                        //        bFirtTime = true;                                 //是否第一个排产时间段
                        //        ai_workTime = ai_workTimeTask;                    //返回原值      
                        //        adCanBegDate = ResTimeRangeListTemp[ResTimeRangeListTemp.Count - 1].DEndTime; //ResTimeRangeList1[i + 1].DBegTime;     //重置可开工时间,已排任务之后
                        //        continue;
                        //    }
                        //}

                        //if (i < ResTimeRangeList1.Count - 1)
                        //SchParam.Debug("2.444444 TestResSchTask ResTimeRangeList1.Count" + ResTimeRangeList1.Count + " i " + i.ToString() , "资源运算");

                        //传入下个空闲时间段
                        if (i < ResTimeRangeList1.Count - 1)
                            resTimeRangeNext = ResTimeRangeList1[i + 1];
                        else
                            resTimeRangeNext = null;

                        //如果当前时间段已经排满不可用，直接重头开始 2022-08-28 JonasCheng
                        if (ResTimeRangeList1[i].AvailableTime <= 0 && ResTimeRangeList1[i].AllottedTime > 0 )
                        {
                            //任务ai_ResPreTime                            
                            bFirtTime = true;   //是否第一个排产时间段

                            ai_workTime = ai_workTimeTask;            //返回原值
                            adCanBegDate = ResTimeRangeList1[i].DEndTime;      //adCanBegDateTask;        //重排可开始时间，重当前时段点开始
                            adCanBegDateTask = ResTimeRangeList1[i].DEndTime;  //重新设置任务可开始时间,并返回

                            continue;

                        }

                        //SchParam.Debug("2.5555555 TestResSchTask resTimeRangeNext " , "资源运算");

                        try
                        {
                            SchParam.ldtBeginDate = DateTime.Now;
                            //--------2.2 调用资源段排程-----------------------------------------------------


                            //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                            //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。 试排
                            ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime, resTimeRangeNext, as_SchProductRouteResPre);


                            //DateTime ldtEndedDate = DateTime.Now;

                            //SchParam.iWaitTime =(ldtEndedDate - SchParam.ldtBeginDate).TotalMilliseconds;


                            //DateTime ldtEndDate5 = DateTime.Now;
                            //TimeSpan interval5 = ldtEndDate5 - ldtBeginDate;
                            //SchParam.iWaitTime5 = interval5.TotalMilliseconds;//计算间隔时间
                        }
                        catch (Exception error)
                        {
                            throw new Exception("时间段排程出错,订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置SchProductRoute.ProcessSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                            return -1;
                        }

                        //SchParam.Debug("2.6666 TestResSchTask TimeSchTask " , "资源运算");

                        if (bFirtTime)
                        {
                            //bFirtTime = false;    //去掉
                            dtBegDate = adCanBegDate;// adCanBegDateTask;//ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate;//adCanBegDateTask; //ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        }


                        ////有其他排产任务,而且排不下时 2019-09-09 Jonas Cheng 
                        //if (this.WorkTimeRangeList.Count > 0 && this.NotWorkTime < ai_workTime)
                        //{
                        //    bFirtTime = true;   //是否第一个排产时间段
                        //    ai_workTime = ai_workTimeTask;            //返回原值      
                        //    adCanBegDate = this.DEndTime;             //重置可开工时间
                        //}

                        //SchParam.Debug("2.777777777 TestResSchTask TimeSchTask " , "资源运算");

                    }


                    //DateTime ldtEndDate3 = DateTime.Now;
                    //TimeSpan interval3 = ldtEndDate3 - ldtBeginDate;
                    //SchParam.iWaitTime3 = interval3.TotalMilliseconds;//计算间隔时间

                    dtEndDate = adCanBegDate;   //工序完工时间
                }
                else              //倒排
                {
                    //找出所有 可用时间大于0的时间段 , 注意必须时段开始时间必须小于adCanEndDate
                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime < adCanBegDateTask2; });  //p.AvailableTime > 0 
                    //必须要排序
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });

                    Boolean bFirtTime = true;            //是否第一个排产时间段

                    //SchParam.Debug("2.88888888 TestResSchTask TimeSchTask " , "资源运算");

                    //倒排,从最大时段开始排
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;

                        //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                        //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                        if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType != "2")  //有限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRev(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);
                        else    //无限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRevInfinite(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);


                        if (bFirtTime) dtEndDate = adCanBegDate; //adCanBegDateTask;//ResTimeRangeList1[i].DEndTime;   //为true，第一个工序

                        //SchParam.Debug("2.9999999 TestResSchTask TimeSchTask ", "资源运算");

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

                        //SchParam.Debug("2.10000000000 TestResSchTask TimeSchTask ", "资源运算");
                    }

                    dtBegDate = adCanBegDate;   //工序完工时间

                }


                //--------2.3 如果任务剩余工作时间大于0 ，则说明任务没有排完，提示排产失败-----------------------------------------------------                
                if (ai_workTime > 0)
                {
                    if (bShowTips)  //显示提示
                    {
                        string cError = string.Format("订单行号：{0} ,加工物料[{1}]在资源[{2}]无法排下,任务号[{3}],单件产能[{4}],加工数量[{5}],加工工时[{6}],未排工时[{7}],最大可排时间[{8}],请检查工作日历或单件产能、计划数量太大!", as_SchProductRouteRes.iSchSdID, as_SchProductRouteRes.cInvCode, as_SchProductRouteRes.cResourceNo, as_SchProductRouteRes.iProcessProductID, as_SchProductRouteRes.iCapacity, as_SchProductRouteRes.iResReqQty, ai_workTimeTask / 3600, ai_workTime / 3600, ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime);
                        throw new Exception(cError);
                    }
                    return -1;
                }

                //SchParam.Debug("2.12222222222 TestResSchTask TimeSchTask ", "资源运算");
            }
            catch (Exception error)
            {
                if (bShowTips)   //显示提示
                {
                    throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "资源正排计算时出错,位置Resource.ResSchTask！工序ID号：" + as_SchProductRouteRes.iProcessProductID + "\n\r " + error.Message.ToString());
                }
                
                return -1;
            }


            //DateTime ldtEndDate2 = DateTime.Now;
            //TimeSpan interval2 = ldtEndDate2 - ldtBeginDate;
            //SchParam.iWaitTime4 = interval2.TotalMilliseconds;//计算间隔时间


            //SchParam.Debug("2.1233333333333 TestResSchTask TimeSchTask ", "资源运算");

            return 1; //剩下未排时间

        }

        //测试可排产的开始日期
        public int TestResSchTaskNew(SchProductRouteRes as_SchProductRouteRes, ref int ai_workTime, ref DateTime adCanBegDate, ref DateTime adCanBegDateTask, Boolean bSchRev, ref int ai_ResPreTime, ref int ai_CycTimeTol, ref DateTime dtBegDate, ref DateTime dtEndDate, Boolean bShowTips = true, Boolean bReCalWorkTime = true, SchProductRouteRes as_SchProductRouteResPre = null)
        {
            //int taskallottedTime = 0;   //任务在本时间段内 总安排时间
            int ai_workTimeTask = ai_workTime;
            int ai_disWorkTime = ai_workTime;
            DateTime adCanBegDateTask2 = adCanBegDateTask;
            //DateTime dtEndDate = adCanBegDate;            
            //int ai_ResPreTime = 0;              //资源前准备时间
            int ai_ResPostTime = 0;             //资源后准备时间
            dtEndDate = adCanBegDate;


            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID && as_SchProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
            {
                int i = 1;
            }

            try
            {
                //--------1.1 返回可开始排产的时间点,保证整个任务可以排下，而且中间没有其他任务断开-------------------------------

                //DateTime dSchStartDate = GetSchStartDate(as_SchProductRouteRes, ai_workTime, adCanBegDate, false, ref dtEndDate);

                //计算用adCanBegDate
                //adCanBegDate = dSchStartDate;

                //--------2.1 找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于dSchStartDate--------------------------
                List<ResTimeRange> ResTimeRangeList1 = new List<ResTimeRange>();

                if (bSchRev == false)   //正排
                {

                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DEndTime > adCanBegDateTask2; }); // p.AvailableTime > 0 &&
                    //必须要排序
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                    Boolean bFirtTime = true;            //是否第一个排产时间段

                    //--------2.1 对所有可用资源段循环，在资源段内排生产任务-----------------------------------------------------
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;

                        //--------2.2 调用资源段排程-----------------------------------------------------
                        //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                        //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。 试排
                        ResTimeRangeList1[i].TimeSchTask(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref ai_ResPreTime, ref ai_CycTimeTol, ref bFirtTime, ref ai_disWorkTime, bReCalWorkTime);

                        if (bFirtTime)
                        {
                            //bFirtTime = false;
                            dtBegDate = adCanBegDate;// adCanBegDateTask;//ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                            as_SchProductRouteRes.dResLeanBegDate = adCanBegDate;//adCanBegDateTask; //ResTimeRangeList1[i].DBegTime;   //为true，第一个工序
                        }



                    }

                    dtEndDate = adCanBegDate;   //工序完工时间
                }
                else              //倒排
                {
                    //找出所有 可用时间大于0的时间段 , 注意必须时段开始时间必须小于adCanEndDate
                    ResTimeRangeList1 = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime < adCanBegDateTask2; });  //p.AvailableTime > 0 
                    //必须要排序
                    ResTimeRangeList1.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });

                    Boolean bFirtTime = true;            //是否第一个排产时间段

                    //倒排,从最大时段开始排
                    for (int i = 0; i < ResTimeRangeList1.Count; i++)
                    {
                        if (ai_workTime == 0) break;

                        //找第1段时，要考虑 可开工的时间点 >= adCanBgDate,时间段的前一不分不可用
                        //调用时间段ResTimeRange对象的TimeSchTask方法，循环给可用时间段排产，直到排完为止。
                        if (as_SchProductRouteRes.schProductRoute.schProduct.cSchType != "2")  //有限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRev(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);
                        else    //无限产能倒排
                            ResTimeRangeList1[i].TimeSchTaskRevInfinite(as_SchProductRouteRes, ref ai_workTime, ref adCanBegDate, ai_workTimeTask, ref adCanBegDateTask, false, ref bFirtTime);

                        if (bFirtTime) dtEndDate = adCanBegDate; //adCanBegDateTask;//ResTimeRangeList1[i].DEndTime;   //为true，第一个工序
                    }

                    dtBegDate = adCanBegDate;   //工序完工时间

                }


                //--------2.3 如果任务剩余工作时间大于0 ，则说明任务没有排完，提示排产失败-----------------------------------------------------                
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


          //根据传入的时间和开始日期，找到可以排下整个任务的可用时间段,返回可开始排产的时间点 2016-05-25       
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
                //找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于adCanBgDate 
                ListReturn = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DEndTime > adStartDate && p.AvailableTime > 0; }); // p.AvailableTime > 0 
                //必须要排序,
                ListReturn.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

                if (ListReturn.Count > 0)
                    dtEndDate = ListReturn[0].DEndTime;
            }
            else                     //倒排
            {
                //找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于adCanBgDate
                ListReturn = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime < adStartDate && p.AvailableTime > 0; }); // p.AvailableTime > 0 
                //必须要排序 ,倒排
                ListReturn.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });

                if (ListReturn.Count > 0)
                    dtEndDate = ListReturn[0].DBegTime;
            }

            return dtEndDate;        


        }
        //根据传入的时间和开始日期，找到可以排下整个任务的可用时间段,返回可开始排产的时间点        
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
            //DateTime dtEndDate;                      //预计完工时间

            if (as_SchProductRouteRes.iProcessProductID == SchParam.iProcessProductID)  //调试断点1 SchProduct
            {
                int i = 1;
            }

            //if (bSchRev == false)    //正排
            //{
            //    //找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于adCanBgDate
            //    ListReturn = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DEndTime > adStartDate; }); // p.AvailableTime > 0 
            //    //必须要排序,
            //    ListReturn.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

            //}
            //else                     //倒排
            //{
            //    //找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于adCanBgDate
            //    ListReturn = ResTimeRangeList.FindAll(delegate(ResTimeRange p) { return p.DBegTime < adStartDate; }); // p.AvailableTime > 0 
            //    //必须要排序 ,倒排
            //    ListReturn.Sort(delegate(ResTimeRange p1, ResTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });
            //}

            ////循环试排，直到找到能完全排下为止，返回排程开始时间即可。
            //for (int i = 0; i < ListReturn.Count; i++)
            //{
            //    //
            //    if (ai_workTime == ai_workTimeOld)
            //    {
            //        TaskTimeRangeBeg = null;
            //        ResTimeRangeBeg = ListReturn[i];
            //    }

            //    //如果没排完，中间又有不可用时间段，直接重来，考虑下个时段
            //    if (ListReturn[i].AvailableTime == 0)
            //    {
            //        ai_workTime = ai_workTimeOld;
            //        continue;
            //    }


            //    //遍历时段内的所有任务，递减ai_workTime，直到全部排完              

            //    if (bSchRev == false)    //正排 ,要考虑换产时间,在此试排时必须考虑，返回；正式排产时再算
            //    {
            //        ListReturn[i].TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

            //        foreach (TaskTimeRange TaskTimeRange1 in ListReturn[i].TaskTimeRangeList)
            //        {
            //            if (ListReturn[i].CIsInfinityAbility == "0")    //产能有限
            //            {
            //                if (TaskTimeRange1.cTaskType == 1 && ai_workTime > 0)  //没排完，有其他任务，则重来，考虑下个时段
            //                {
            //                    ai_workTime = ai_workTimeOld;   //继续同一时段的其他空任务开始试排
            //                    ResTimeRangeBeg = ListReturn[i];
            //                    TaskTimeRangeBeg = TaskTimeRange1;
            //                    continue;
            //                }
            //            }


            //            if (TaskTimeRange1.cTaskType == 0) //空闲时段
            //            {
            //                if (TaskTimeRangeBeg == null)
            //                    TaskTimeRangeBeg = TaskTimeRange1;

            //                ai_workTime -= TaskTimeRange1.AvailableTime;   //继续同一时段的其他空任务开始试排
            //                if (ai_workTime < 0)
            //                {
            //                    ai_workTime = 0;    //退出循环
            //                    TaskTimeRangeEnd = TaskTimeRange1;
            //                    dtEndDate = TaskTimeRange1.DBegTime;
            //                    break;
            //                }
            //                continue;
            //            }

            //        }
            //    }
            //    else           //倒排
            //    {
            //        ListReturn[i].TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p2.DBegTime, p1.DBegTime); });

            //        foreach (TaskTimeRange TaskTimeRange1 in ListReturn[i].TaskTimeRangeList)
            //        {
            //            if (ListReturn[i].CIsInfinityAbility == "0")  //有限产能
            //            {
            //                if (TaskTimeRange1.cTaskType == 1 && ai_workTime > 0)  //没排完，有其他任务，则重来，考虑下个时段
            //                {
            //                    ai_workTime = ai_workTimeOld;   //继续同一时段的其他空任务开始试排
            //                    ResTimeRangeBeg = ListReturn[i];
            //                    TaskTimeRangeBeg = TaskTimeRange1;

            //                    continue;
            //                }
            //            }

            //            if (TaskTimeRange1.cTaskType == 0) //空闲时段
            //            {
            //                if (TaskTimeRangeBeg == null)
            //                    TaskTimeRangeBeg = TaskTimeRange1;

            //                ai_workTime -= TaskTimeRange1.AvailableTime;   //继续同一时段的其他空任务开始试排
            //                if (ai_workTime < 0)
            //                {
            //                    ai_workTime = 0;    //退出循环
            //                    TaskTimeRangeEnd = TaskTimeRange1;
            //                    dtEndDate = TaskTimeRange1.DBegTime;
            //                    break;
            //                }
            //                continue;
            //            }

            //        }
            //    }

            //    //直到排完为止,记录结束时间段
            //    if (ai_workTime == 0)
            //    {
            //        ResTimeRangeEnd = ListReturn[i];
            //        break;
            //    }
            //}

            ////直到最后，都没找到可用时间段，无法排下,提示异常
            //if (ai_workTime > 0)
            //{
            //    //
            //    throw new Exception("订单行号：" + as_SchProductRouteRes.iSchSdID + "加工物料[" + as_SchProductRouteRes.cInvCode + "]在资源[" + as_SchProductRouteRes.cResourceNo + "]无法排下,任务号[" + as_SchProductRouteRes.iProcessProductID + "],生产时间[" + ai_workTimeOld / 3600 + "]小时,最大可用时间[" + ListReturn[ListReturn.Count - 1].DBegTime.ToShortDateString() + " " + ListReturn[ListReturn.Count - 1].DBegTime.ToLongTimeString() + "],请检查工作日历,或单件产能太大!"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +

            //    return dtBegDate;
            //}

            //返回可排开始时间
            if (bSchRev == false)    //正排
            {
                return TaskTimeRangeBeg.DBegTime;
            }
            else                     //倒排
            {
                return TaskTimeRangeBeg.DEndTime;
            }

        }

        //--------正排 资源任务排第一个时间段时，找出上一排产任务，并计算换产时间--------------------------------------------
        public int GetChangeTime(SchProductRouteRes as_SchProductRouteRes, int ai_workTime, DateTime adStartDate, ref int iCycTimeTol, Boolean bSchdule, SchProductRouteRes as_SchProductRouteResPre = null )
        {
            int iPreTime = 0;      //前准备时间(换产时间)
            //int iCycTimeTol = 0;   //中间定期更换时间（换刀时间）

            //维修工单(cType == "PSH")，不考虑工艺特性换产时间cNeedChanged
            if (this.cNeedChanged == "1" && as_SchProductRouteRes.cWoNo != "" && as_SchProductRouteRes.schProductRoute.schProduct.cType == "PSH") return 0;

            //找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于adCanBgDate,4小时内的任务，才认为是相邻任务
            //List<TaskTimeRange> ListReturn = this.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.DEndTime <= adStartDate && p.cTaskType == 1; }); // p.AvailableTime > 0 
            //List<TaskTimeRange> ListReturn = this.GetTaskTimeRangeList(false).FindAll(delegate(TaskTimeRange p) { return p.DEndTime <= adStartDate && p.DEndTime.AddHours(4) > adStartDate && p.cTaskType == 1; }); // p.AvailableTime > 0 
            //GetTaskTimeRangeList(DateTime dBegDate, Boolean bSchRev = false, Boolean OrderASC = true)

            /////取as_SchProductRouteRes.SchProductRouteResPre,调度优化时设置
            //as_SchProductRouteResPre = as_SchProductRouteRes.SchProductRouteResPre;

            //指定了前工序，取前工序最后一个时间段
            if (as_SchProductRouteResPre == null)
            {
                //按当前资源查找上一个最近已排产任务
                List<SchProductRouteRes> ListSchProductRouteResAll = this.schData.SchProductRouteResList.FindAll(delegate (SchProductRouteRes p1) { return p1.cResourceNo == this.cResourceNo && p1.iResReqQty > 0 && p1.BScheduled == 1 && p1.dResBegDate <= adStartDate; });
                ListSchProductRouteResAll.Sort(delegate (SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p2.dResBegDate, p1.dResBegDate); });
               
                if (ListSchProductRouteResAll.Count > 0 )
                    as_SchProductRouteResPre = ListSchProductRouteResAll[0];

                //TaskTimeRange TaskTimeRangePre = null;   //前一个生产任务,也可能没有任务，也要计算换产时间

                //List<TaskTimeRange> ListReturn = this.GetTaskTimeRangeList(adStartDate, false, true); //.FindAll(delegate (TaskTimeRange p) { return p.DEndTime <= adStartDate && p.DEndTime.AddHours(4) > adStartDate && p.cTaskType == 1; }); // p.AvailableTime > 0 

                //if (ListReturn.Count > 0)
                //{
                //    ////必须要排序,取最近一个任务
                //    //ListReturn.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare( p2.DBegTime,p1.DBegTime); });

                //    TaskTimeRangePre = ListReturn[0];


                //    as_SchProductRouteResPre = TaskTimeRangePre.schProductRouteRes;

                //}
            }

            
            //清空备注
            cTimeNote = "";

            //计算换产时间,中间定期更换时间（换刀时间）
            //启用资源工艺特征,计算换线时间---------------------------------------------------------------------------
            if (SchParam.ResProcessCharCount > 0 )
                iPreTime = GetChangeTime(as_SchProductRouteRes, ai_workTime, as_SchProductRouteResPre, ref iCycTimeTol, bSchdule);



            //前准备时间 有工艺特征变化,增加资源前准备时间(开关机时间) 2014-05-10  
            if (iPreTime > 0)
            {
                iPreTime += int.Parse(this.iResPreTime.ToString());
                cTimeNote += " 资源前准备时间:[" + this.iResPreTime.ToString() + "];";

            }
            //换料时间 前后两个任务物料有变化，给换料时间
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

            //考虑工艺路线中设置的前置时间
            if (as_SchProductRouteRes.iResPreTimeOld > 0)
            {
                iPreTime += (int)as_SchProductRouteRes.iResPreTimeOld;
                cTimeNote += " 工艺路线资源前准备时间:[" + as_SchProductRouteRes.iResPreTimeOld.ToString() + "];";
            }

            //cTimeNote写入资源任务@cDefine25字段中
            as_SchProductRouteRes.cDefine25 = cTimeNote;

            return iPreTime;  //返回换产时间
        }

        //传入两个任务,计算换产时间  TaskTimeRange TaskTimeRangePre
        public int GetChangeTime(SchProductRouteRes as_SchProductRouteRes, int ai_workTime, SchProductRouteRes as_SchProductRouteResPre , ref int iCycTimeTol, Boolean bSchdule)
        {
            int iPreTime = 0;     //前准备时间(换产时间)           
            iCycTimeTol = 0;      //中间定期更换时间（换刀时间）
            
           
            int[] iCycTime = new Int32[12];
            int[] iChaValue = new Int32[12];

            try
            {

                //资源任务12个工艺特征循环，取每工工艺特征的转换时间

                //计算各工艺特征的换产时间、定期更换总时间
                if (as_SchProductRouteResPre != null)
                {
                    //as_SchProductRouteRes.iSchSN
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
                    //if (this.FProChaType7ID != "-1" && this.FProChaType7ID != "" && as_SchProductRouteRes.resChaValue7 != null && SchParam.ResProcessCharCount > 6)
                    //{
                    //    cTimeNote += "  工艺特征7:" + as_SchProductRouteRes.resChaValue7.FResChaValueNo;
                    //    if (as_SchProductRouteResPre.resChaValue7 != null) cTimeNote += " 前工艺特征7:" + as_SchProductRouteResPre.resChaValue7.FResChaValueNo;
                    //    iChaValue[6] = as_SchProductRouteRes.resChaValue7.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue7, ai_workTime, ref iCycTime[6], bSchdule, as_SchProductRouteResPre);
                    //}
                    //if (this.FProChaType8ID != "-1" && this.FProChaType8ID != "" && as_SchProductRouteRes.resChaValue8 != null && SchParam.ResProcessCharCount > 7)
                    //{
                    //    cTimeNote += " 工艺特征8:" + as_SchProductRouteRes.resChaValue8.FResChaValueNo;
                    //    if (as_SchProductRouteResPre.resChaValue8 != null) cTimeNote += " 前工艺特征8:" + as_SchProductRouteResPre.resChaValue8.FResChaValueNo;
                    //    iChaValue[7] = as_SchProductRouteRes.resChaValue8.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue8, ai_workTime, ref iCycTime[7], bSchdule, as_SchProductRouteResPre);
                    //}
                    //if (this.FProChaType9ID != "-1" && this.FProChaType9ID != "" && as_SchProductRouteRes.resChaValue9 != null && SchParam.ResProcessCharCount > 8)
                    //{
                    //    cTimeNote += " 工艺特征9:" + as_SchProductRouteRes.resChaValue9.FResChaValueNo;
                    //    if (as_SchProductRouteResPre.resChaValue9 != null) cTimeNote += " 前工艺特征9:" + as_SchProductRouteResPre.resChaValue9.FResChaValueNo;
                    //    iChaValue[8] = as_SchProductRouteRes.resChaValue9.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue9, ai_workTime, ref iCycTime[8], bSchdule, as_SchProductRouteResPre);
                    //}

                    //if (this.FProChaType10ID != "-1" && this.FProChaType10ID != "" && as_SchProductRouteRes.resChaValue10 != null && SchParam.ResProcessCharCount > 9)
                    //{
                    //    cTimeNote += "  工艺特征10:" + as_SchProductRouteRes.resChaValue10.FResChaValueNo;
                    //    if (as_SchProductRouteResPre.resChaValue10 != null) cTimeNote += " 前工艺特征10:" + as_SchProductRouteResPre.resChaValue10.FResChaValueNo;
                    //    iChaValue[9] = as_SchProductRouteRes.resChaValue10.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue10, ai_workTime, ref iCycTime[9], bSchdule, as_SchProductRouteResPre);
                    //}
                    //if (this.FProChaType11ID != "-1" && this.FProChaType11ID != "" && as_SchProductRouteRes.resChaValue11 != null && SchParam.ResProcessCharCount > 10)
                    //{ 
                    //    cTimeNote += " 工艺特征11:" + as_SchProductRouteRes.resChaValue11.FResChaValueNo;
                    //    if (as_SchProductRouteResPre.resChaValue11 != null) cTimeNote += " 前工艺特征11:" + as_SchProductRouteResPre.resChaValue11.FResChaValueNo;
                    //    iChaValue[10] = as_SchProductRouteRes.resChaValue11.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue11, ai_workTime, ref iCycTime[10], bSchdule, as_SchProductRouteResPre);
                    //}
                    //if (this.FProChaType12ID != "-1" && this.FProChaType12ID != "" && as_SchProductRouteResPre.resChaValue12 != null && SchParam.ResProcessCharCount > 11)
                    //{
                    //    cTimeNote += " 工艺特征12:" + as_SchProductRouteRes.resChaValue12.FResChaValueNo;
                    //    if (as_SchProductRouteResPre.resChaValue12 != null) cTimeNote += " 前工艺特征12:" + as_SchProductRouteResPre.resChaValue12.FResChaValueNo;
                    //    iChaValue[11] = as_SchProductRouteRes.resChaValue12.GetChaValueChangeTime(as_SchProductRouteRes, as_SchProductRouteResPre.resChaValue12, ai_workTime, ref iCycTime[11], bSchdule, as_SchProductRouteResPre);
                    //}

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
                    //if (this.FProChaType7ID != "-1" && this.FProChaType7ID != "" && as_SchProductRouteRes.resChaValue7 != null)
                    //    iChaValue[6] = as_SchProductRouteRes.resChaValue7.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[6], bSchdule, null);
                    //if (this.FProChaType8ID != "-1" && this.FProChaType8ID != "" && as_SchProductRouteRes.resChaValue8 != null)
                    //    iChaValue[7] = as_SchProductRouteRes.resChaValue8.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[7], bSchdule, null);
                    //if (this.FProChaType9ID != "-1" && this.FProChaType9ID != "" && as_SchProductRouteRes.resChaValue9 != null)
                    //    iChaValue[8] = as_SchProductRouteRes.resChaValue9.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[8], bSchdule, null);
                    //if (this.FProChaType10ID != "-1" && this.FProChaType10ID != "" && as_SchProductRouteRes.resChaValue10 != null)
                    //    iChaValue[9] = as_SchProductRouteRes.resChaValue10.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[9], bSchdule, null);
                    //if (this.FProChaType11ID != "-1" && this.FProChaType11ID != "" && as_SchProductRouteRes.resChaValue11 != null)
                    //    iChaValue[10] = as_SchProductRouteRes.resChaValue11.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[10], bSchdule, null);
                    //if (this.FProChaType12ID != "-1" && this.FProChaType12ID != "" && as_SchProductRouteRes.resChaValue12 != null)
                    //    iChaValue[11] = as_SchProductRouteRes.resChaValue12.GetChaValueChangeTime(as_SchProductRouteRes, null, ai_workTime, ref iCycTime[11], bSchdule, null);

                }

                //根据系统参数，取累计值，还是最大值
                //for (int i = 0; i < 12; i++)
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


        //---------3、关键资源排产(所有工序),先按工艺特征优化排序，再依次排各关键任务的生产时间，同时排关键工序前面工序----------------
        public int KeyResSchTask(int iCount = 1)
        {
            ////1、关键资源排产 给生产任务按工艺特征进行排序 
            ////SchProductRouteResList
            //List<SchProductRouteRes> schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo); });

            ////schProductRouteResList.OrderBy()
            ////schProductRouteResList.Sort()
            ////TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

            //schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
            this.GetNotSchTask();

            if (schProductRouteResList.Count < 1) return 1;
                       

            //2、排当前关键工序前面所有工序，正排;白茬工序全排
            //取最大iSchPriorityID
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });
            double iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            if (iSchPriority < 0) iSchPriority = 0;

            SchProductRouteRes as_SchProductRouteResLast = null;
           
            //schProductRouteResList           
            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResList)
            {
                if (schProductRouteRes.iProcessProductID == SchParam.iProcessProductID && schProductRouteRes.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }

                //排当前关键工序前面所有工序，正排
                schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);

                //1 关键资源排产不能穿插，任务排序后，排在后面的任务开工时间不能大于前面任务。 0 允许穿插，后面任务可以排在前面任务之前
                //设置当前任务可开工时间为上个任务的结束时间
                if (SchParam.bSchKeyBySN == "1" && as_SchProductRouteResLast != null)
                {
                    schProductRouteRes.schProductRoute.dEarlyBegDate = as_SchProductRouteResLast.dResEndDate;
                    schProductRouteRes.dEarliestStartTime = as_SchProductRouteResLast.dResEndDate;
                }   

                //往后排，后面白茬工序全排完。油漆工序全是10以上的工序
                schProductRouteRes.schProductRoute.ProcessSchTaskNext("2");  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排

                //以关键资源任务优化排产顺序，决定其产品的排产顺序,部件先开工，先生产
                if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0)
                {
                    iSchPriority++;
                    schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority;
                }
                //schProductRouteRes.TaskSchTask(ref schProductRouteRes.iResReqQty, this.schData.dtStart);

                //记录上个排产任务
                as_SchProductRouteResLast = schProductRouteRes;
            }

            return 1;
        }


        //---------3.1、关键资源排产前准备,先按工艺特征优化排序，再依次排各关键任务的生产时间，同时排关键工序前面工序-----暂不用----------------
        public int KeyResSchTaskPre()    
        {
            //1、关键资源排产 给生产任务按工艺特征进行排序 
            //SchProductRouteResList
            List<SchProductRouteRes> schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.iSchBatch == this.iSchBatch ); });

            //schProductRouteResList.OrderBy()
            //schProductRouteResList.Sort()
            //TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });

            schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });

            //2、排当前关键工序前面所有工序，正排;白茬工序全排
            //取最大iSchPriorityID
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });
            double iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            if (iSchPriority < 0) iSchPriority = 0;

            

            return 1;
        }

        //---------3.2、关键资源排产,一次只排1个任务,用于轮流排产----------------
        public int KeyResSchTaskSingle(double as_iTurmsTime, ref SchProductRouteRes as_SchProductRouteResLast)
        {
            ////1、关键资源排产 给生产任务按工艺特征进行排序 
            ////SchProductRouteResList
            //List<SchProductRouteRes> schProductRouteResList = this.schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo); });

            ////schProductRouteResList.OrderBy()
            ////schProductRouteResList.Sort()
            ////TaskTimeRangeList.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare(p1.DBegTime, p2.DBegTime); });
            
            ////按工艺特征排序
            //schProductRouteResList.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });

            //2、排当前关键工序前面所有工序，正排;白茬工序全排
            //取最大iSchPriorityID
            schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<double>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });
            double iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            if (iSchPriority < 0) iSchPriority = 0;


            ////schProductRouteResList           
            //foreach (SchProductRouteRes schProductRouteRes in schProductRouteResList)
            //{

            //所有待排生产计划
            //List<SchProductRouteRes> schProductRouteResListNo = schProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.bScheduled == 0); });

            //if (schProductRouteResListNo.Count < 1) return -1;
                    

            if (this.iTurnsType == "1"  )   //1 按轮换时间  2 按任务数量
            {
                //轮换加工时间
                if (as_iTurmsTime < 0) as_iTurmsTime = this.iTurnsTime;

                //轮换加工时间设置为负数时，取上一排产任务的换模时间，作为本次轮换时间，适用于一主一辅设备，主设备停产换模，给辅助设备生产。
                if (this.iTurnsTime <= 0 && as_SchProductRouteResLast != null)
                {
                    as_iTurmsTime = as_SchProductRouteResLast.iResPreTime;

                    //主设备任务已排完,不用再轮换了，辅助设备全部排完
                    if (as_SchProductRouteResLast.resource.cResourceNo != this.cResourceNo && as_SchProductRouteResLast.resource.bScheduled == 1)
                    {
                        as_iTurmsTime = 99999999999; 
                    }
                }

                //没有换产时间，其他资源继续排产
                if (as_iTurmsTime == 0) return 1;

                double iTolWorkTime = 0;
               

                //累计任务加工时间 < 轮换时间
                while (iTolWorkTime < as_iTurmsTime )
                {
                    SchProductRouteRes schProductRouteRes = schProductRouteResList.Find(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0); });

                    if (schProductRouteRes == null)
                    {
                        SchParam.Debug(string.Format("关键资源{0}轮换排产,轮换批次[{1}],总工时[{2}],所有任务已排完.", this.cResourceNo, this.iBatch.ToString(), (iTolWorkTime / 60).ToString()), "关键资源轮换生产");
                
                        //排产分批批次加1
                        this.iBatch++;
                        return -1;
                    }

                    //排当前关键工序前面所有工序，正排,不包含本工序
                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);

                    //1 关键资源排产不能穿插，任务排序后，排在后面的任务开工时间不能大于前面任务。 0 允许穿插，后面任务可以排在前面任务之前
                    //设置当前任务可开工时间为上个任务的结束时间
                    if (SchParam.bSchKeyBySN == "1" && as_SchProductRouteResLast != null)
                    {
                        schProductRouteRes.schProductRoute.dEarlyBegDate = as_SchProductRouteResLast.dResEndDate;
                        schProductRouteRes.dEarliestStartTime = as_SchProductRouteResLast.dResEndDate;
                    }                        

                    //包含本工序，往后排，后面白茬工序全排完。油漆工序全是10以上的工序
                    schProductRouteRes.schProductRoute.ProcessSchTaskNext("2");  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排

                    //以关键资源任务优化排产顺序，决定其产品的排产顺序,部件先开工，先生产
                    if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0)
                    {
                        iSchPriority++;
                        schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority;
                    }
                    schProductRouteRes.iBatch = this.iBatch;

                    //累计总加工时间,包含前准备时间
                    iTolWorkTime +=  schProductRouteRes.iResRationHour;
                    //as_PreTime = schProductRouteRes.iResPreTime;
                    as_SchProductRouteResLast = schProductRouteRes;
                }

                //排产分批批次加1
                this.iBatch++;

                SchParam.Debug(string.Format("关键资源{0}轮换排产,轮换批次[{1}],总工时[{2}]", this.cResourceNo, this.iBatch.ToString(), (iTolWorkTime/60).ToString()), "关键资源轮换生产");
                
            }
            //else if (this.iTurnsType == "0")                        //不轮换
            //{
            //    //关键资源优化排产
            //    this.KeyResSchTask();
            //}
            else if (this.iTurnsType == "2")                       //按任务数量轮换
            {
                //轮换任务数量
                if (as_iTurmsTime < 0) as_iTurmsTime = this.iTurnsTime;
                double iTolWorks = 0;
                as_iTurmsTime = 1;

                //as_iTurmsTime  = 0 时，表示工艺特征没切换，换产时间小于10分钟，就一直排产

                //累计任务加工时间 < 轮换任务数量
                while (iTolWorks < as_iTurmsTime || this.iTurnsTime == 0)
                {
                    SchProductRouteRes schProductRouteRes = schProductRouteResList.Find(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0); });

                    if (schProductRouteRes == null)
                    {
                        //排产分批批次加1
                        this.iBatch++;

                        return -1;
                    }

                    //排当前关键工序前面所有工序，正排
                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);

                    //1 关键资源排产不能穿插，任务排序后，排在后面的任务开工时间不能大于前面任务。 0 允许穿插，后面任务可以排在前面任务之前
                    //设置当前任务可开工时间为上个任务的结束时间
                    if (SchParam.bSchKeyBySN == "1" && as_SchProductRouteResLast != null)
                    {
                        schProductRouteRes.schProductRoute.dEarlyBegDate = as_SchProductRouteResLast.dResEndDate;
                        schProductRouteRes.dEarliestStartTime = as_SchProductRouteResLast.dResEndDate;
                    }   

                    //往后排，后面白茬工序全排完。油漆工序全是10以上的工序
                    schProductRouteRes.schProductRoute.ProcessSchTaskNext("2");  //1 加工物料相同的所有工序; 2 油漆工序;3 排后面所有工序，后面节点有其他未排工序时，往前排

                    //以关键资源任务优化排产顺序，决定其产品的排产顺序,部件先开工，先生产
                    if (schProductRouteRes.schProductRoute.schProduct.iSchPriority < 0)
                    {
                        iSchPriority++;
                        schProductRouteRes.schProductRoute.schProduct.iSchPriority = iSchPriority;
                    }

                    //累计总加工时间,包含前准备时间
                    iTolWorks += 1;
                    //as_PreTime = schProductRouteRes.iResPreTime;
                    as_SchProductRouteResLast = schProductRouteRes;

                    //换模时间最少10分钟
                    if (this.cBatch2WorkTime < 10) this.cBatch2WorkTime = 10;

                    //如果前准备时间大于10分钟,退出本次分批
                    if (this.iTurnsTime == 0 && as_SchProductRouteResLast.iResPreTime >= this.cBatch2WorkTime * 60)
                    {
                        //排产分批批次加1
                        this.iBatch++;
                        //continue;
                        return 1;
                    }
                }

                //排产分批批次加1
                this.iBatch++;

            }

            return 1;
        }

        //分批次排产
        public int KeyResBatch( )
        {
            SchProductRouteRes as_SchProductRouteResLast = new SchProductRouteRes();           

            //2、排当前关键工序前面所有工序，正排;白茬工序全排
            ////取最大iSchPriorityID
            //schData.SchProductList.Sort(delegate(SchProduct p1, SchProduct p2) { return Comparer<int>.Default.Compare(p1.iSchPriority, p2.iSchPriority); });
            //int iSchPriority = schData.SchProductList[schData.SchProductList.Count - 1].iSchPriority;
            //if (iSchPriority < 0) iSchPriority = 0;

            try
            {

                List<SchProductRouteRes> schProductRouteResListNoTest = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.iWoSeqID == this.iBatchWoSeqID && p1.iSchBatch == this.iSchBatch); });
                
                List<SchProductRouteRes> schProductRouteResListNoSeq = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo &&  p1.schProductRoute.BScheduled == 0 && p1.iSchBatch == this.iSchBatch); });
                

                //所有待排生产计划, 油漆工序iWoSeqID 51为油漆首工序

                List<SchProductRouteRes> schProductRouteResListNo = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID == this.iBatchWoSeqID  &&p1.iSchBatch == this.iSchBatch); });
                
                if (schProductRouteResListNo.Count < 1) return -1;

                //油漆工序前面工序全部排完
                foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListNo)
                {
                    //排当前关键工序前面所有工序，正排,不包含当前工序
                    schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);

                    schProductRouteRes.schProductRoute.GetRouteEarlyBegDate();
                    //schProductRouteRes.dEarliestStartTime = schProductRouteRes.schProductRoute.GetNextProcessCanBegDate(schProductRouteRes.schProductRoute);

                    //schProductRouteRes.schProductRoute.dEarlyBegDate = schProductRouteRes.dEarliestStartTime;
                    schProductRouteRes.dEarliestStartTime = schProductRouteRes.schProductRoute.dEarlyBegDate;

                    //schProductRouteRes.schProductRoute.dEarlyBegDate = schProductRouteRes.schProductRoute.GetPreProcessCanEndDate();

                    ////如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
                    //if (this.iSchBatch < 0)
                    //{
                    //    schProductRouteRes.schProductRoute.ProcessSchTaskNext("1");
                    //}
                }

                ////如果是手工工单 -2,包装子任务单-1，直接排，不用分批,排完后面工序 2014-11-07
                //if (this.iSchBatch < 0) return 1;

                //按待排工序可开工时间排序
                //schProductRouteResListNo.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dEarliestStartTime, p2.dEarliestStartTime); });

                ////取当前资源所有已排任务，维修工单，冻结工单
                //List<SchProductRouteRes> schProductRouteResListSched = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 1); });
                //schProductRouteResListSched.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

                //if (schProductRouteResListSched.Count > 0)
                //    this.dBatchBegDate = schProductRouteResListSched[schProductRouteResListSched.Count - 1].dResEndDate;


                //2014-11-24
                schProductRouteResListNo.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });
                
                //先排正在执行的这一批,冻结状态,批次号固定为1,排51工序及其后续工序所有任务
                if(this.iSchBatch == 1 )
                {
                    //&& p1.schProductRoute.BScheduled == 0
                    List<SchProductRouteRes> schProductRouteResListFirst = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo  && p1.iWoSeqID == this.iBatchWoSeqID && p1.iSchBatch == this.iSchBatch && p1.iBatch == 1); });

                    if (schProductRouteResListFirst.Count > 0 )
                        KeyResBatchSchTaskSubFirst(1, ref as_SchProductRouteResLast, schProductRouteResListFirst);
                        //KeyResBatchSchTask("", this.iBatchWoSeqID, true, this.iTurnsTime, schProductRouteResListFirst, as_SchProductRouteResLast);

                }

                //只有一种分批
                if (this.cBatch2Filter == "" && this.cBatch3Filter == "" && this.cBatch4Filter == "" && this.cBatch5Filter == "")
                {
                    KeyResBatchSchTask(this.cBatch1Filter, this.iBatchWoSeqID, true, this.iTurnsTime, schProductRouteResListNo, as_SchProductRouteResLast);
                }
                else
                {
                    Boolean bExist = true;
                    int iBatchCount = 1;

                    //分批生产循环
                    while (bExist)
                    {
                        //第一批循环,只排一批
                        if (this.cBatch1Filter != "")
                            KeyResBatchSchTask(this.cBatch1Filter, this.iBatchWoSeqID, false,this.cBatch1WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);

                        //第二批循环,只排一批
                        if (this.cBatch2Filter != "")
                            KeyResBatchSchTask(this.cBatch2Filter, this.iBatchWoSeqID, false, this.cBatch2WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);

                        //第三批循环,只排一批
                        if (this.cBatch3Filter != "")
                            KeyResBatchSchTask(this.cBatch3Filter, this.iBatchWoSeqID, false, this.cBatch3WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);

                        //第四批循环,只排一批
                        if (this.cBatch4Filter != "")
                            KeyResBatchSchTask(this.cBatch4Filter, this.iBatchWoSeqID, false, this.cBatch4WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);

                        //第五批循环,只排一批
                        if (this.cBatch5Filter != "")
                            KeyResBatchSchTask(this.cBatch5Filter, this.iBatchWoSeqID, false, this.cBatch5WorkTime * 60,schProductRouteResListNo,as_SchProductRouteResLast);

                        iBatchCount++;

                        //判断是否还有未排任务,未完继续循环
                        //List<Resource> ResourceResList = new List<Resource>(10);
                        //ResourceResList = schData.ResourceList.FindAll(delegate(Resource p1) { return (p1.cIsKey == "1" && p1.iTurnsType == "3" && p1.bScheduled == 0 && p1.iwo); });

                        //if (ResourceResList.Count < 1)
                        //    bExist = false;

                        //List<SchProductRouteRes> schProductRouteResListNo2 = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID == this.iBatchWoSeqID && p1.iSchBatch == this.iSchBatch); });

                        List<SchProductRouteRes> schProductRouteResListNo2 = schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 ); });

                        if (schProductRouteResListNo2.Count < 1)
                        {
                            bExist = false;
                            break;
                        }

                        if (iBatchCount > 20)
                        {
                            //throw new Exception(string.Format(@"资源[" + this.cResourceNo + "]轮流分批生产超过20批,可能分批条件设置不全,请重新设置!"));
                            //剩下的一次排完
                            KeyResBatchSchTask("", this.iBatchWoSeqID, true, 0, schProductRouteResListNo, as_SchProductRouteResLast);
                            break;
                        }
                    }
                    
                    //有些没有定义工艺特征的，最后全排
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

        //---------3.3关键油漆资源排产,足一个批量才开始排产,每次只排一批 ----------------------
        public int KeyResBatchSchTask(string cBatchFilter, int iBatchWoSeqID, Boolean bSchAll , int as_iTurmsTime, List<SchProductRouteRes> as_schProductRouteResListNo, SchProductRouteRes as_SchProductRouteResLast)
        {          

            //所有待排生产计划, 油漆工序iWoSeqID 51为油漆首工序
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

            //按待排工序可开工时间排序

            schProductRouteResListNo.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.schProductRoute.dEarlyBegDate, p2.schProductRoute.dEarlyBegDate); });

            double iTolWorkTime = 0;
            //轮换加工时间
            if (as_iTurmsTime < 1 )
                as_iTurmsTime = this.iTurnsTime;

            //如果没设置轮换时间，取240分
            if (as_iTurmsTime < 1)
                as_iTurmsTime = 240 * 60;

           
            for (int i = 0; i < schProductRouteResListNo.Count; i++)
            {
                SchProductRouteRes schProductRouteRes = schProductRouteResListNo[i];

                //重新计算任务总工时，不考虑准备时间
                schProductRouteRes.iResRationHour = schProductRouteRes.iResReqQty * schProductRouteRes.iCapacity;

                if (bSchAll)
                {                    
                    iTolWorkTime += schProductRouteRes.iResRationHour;
                }
                else
                {
                    //KLB0 双叶色,KLB4 白色,才计算产能工时 //YQ-11-01	大板件底漆1特殊处理
                    //if ( this.cResourceNo == "YQ-11-01" && ( schProductRouteRes.resChaValue1.FResChaValueNo == "KLB0" || schProductRouteRes.resChaValue1.FResChaValueNo == "KLB4"))
                    //    iTolWorkTime += schProductRouteRes.iResRationHour;
                    //else
                        iTolWorkTime += schProductRouteRes.iResRationHour;
                }

                //调试
                if (schProductRouteRes.iSchSdID == SchParam.iSchSdID && schProductRouteRes.iProcessProductID == SchParam.iProcessProductID)
                {
                    int j;
                }
                
                //标记所属排产批次
                schProductRouteRes.iBatch = this.iBatch;

                //满一批次,批次号加1
                if (iTolWorkTime > as_iTurmsTime)
                {
                    ////这些设备
                    //if (this.cResourceNo == "YQ-15-01"|| this.cResourceNo == "YQ-12-11" || this.cResourceNo == "YQ-24-01")
                    //{ 
                    //    //可开工时间4小时内的，排到这一批
                    //    DateTime dBatchEndDate = schProductRouteRes.schProductRoute.dEarlyBegDate.AddHours(4);

                    //    List<SchProductRouteRes> schProductRouteResListAdd = schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return p1.iBatch != this.iBatch && p1.schProductRoute.dEarlyBegDate <= dBatchEndDate; });

                    //    foreach (var schProductRouteResAdd in schProductRouteResListAdd)
                    //    {
                    //        schProductRouteResAdd.iBatch = this.iBatch;

                    //        iTolWorkTime += schProductRouteResAdd.iResRationHour;

                    //        SchParam.Debug(string.Format("资源编号{0}，增加分批{1},分批条件{2}，工单号{3},计划ID{4}", this.cResourceNo, this.iBatch, cBatchFilter, schProductRouteResAdd.cWoNo, schProductRouteResAdd.iResProcessID), "资源分批");

                    //    }

                    //    //schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0) && p1.iWoSeqID == iBatchWoSeqID && p1.iSchBatch == this.iSchBatch; });
                    //}

                    as_SchProductRouteResLast = schProductRouteRes;

                    //this.dBatchBegDate = schProductRouteRes.dEarliestStartTime;  //这批的最后一个件，作为油漆开始时间

                    SchParam.Debug(string.Format("资源编号{0}，分批{1},分批条件{2}，分批工时{3},累计工时{4},总任务数{5}", this.cResourceNo, this.iBatch, cBatchFilter, (as_iTurmsTime / 60).ToString(), (iTolWorkTime / 60).ToString(),schProductRouteResListNo.Count.ToString()),"资源分批");

                    //排一个批次
                    KeyResBatchSchTaskSub(this.iBatch, ref as_SchProductRouteResLast, as_schProductRouteResListNo,cBatchFilter);

                    //清零,重新开始计时
                    iTolWorkTime = 0;
                    // 
                    this.iBatch++;

                    //不是排所有,排一批则返回
                    if (!bSchAll) return this.iBatch;
                }



                //最后一个任务,所在的批次，可能不足一批
                if (i == schProductRouteResListNo.Count - 1)
                {
                    //if (this.iSchBatch == 1)    //生产任务单，最后一批不满，则不排,和待排任务一起同排
                    //{
                    //    schProductRouteResListLast = as_schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0) && p1.iWoSeqID == iBatchWoSeqID && p1.iSchBatch == this.iSchBatch && p1.iBatch == this.iBatch; });

                    //    foreach (SchProductRouteRes item in schProductRouteResListLast)
                    //    {
                    //        item.iBatch = 0;
                    //        item.schProductRoute.schProduct.SetSchBatch(6);
                    //    }

                    //}
                    //else
                    { 
                        as_SchProductRouteResLast = schProductRouteRes;

                        SchParam.Debug(string.Format("资源编号{0}最后一批，分批{1},分批条件{2}，分批工时{3},累计工时{4},总任务数{5}", this.cResourceNo, this.iBatch, cBatchFilter, (as_iTurmsTime / 60).ToString(), (iTolWorkTime / 60).ToString(), schProductRouteResListNo.Count.ToString()), "资源分批");


                        //最后一个批次排产
                        KeyResBatchSchTaskSub(this.iBatch, ref as_SchProductRouteResLast, as_schProductRouteResListNo, cBatchFilter);

                        this.iBatch++;

                        //不是排所有,排一批则返回
                        if (!bSchAll) return this.iBatch;
                    }
                }

            }
            

            return 1;
        }
        

         //---------3.3关键油漆资源排产,足一个批量才开始排产----------------------
        public int KeyResBatchSchTaskSub(int as_iBatch, ref SchProductRouteRes as_SchProductRouteResLast, List<SchProductRouteRes> as_schProductRouteResListNo, string cBatchFilter)
        {
            //每个批次进行排产
           
            //所有待排生产计划, 油漆工序iWoSeqID 51为油漆首工序
            //List<SchProductRouteRes> schProductRouteResListBatch = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.bScheduled == 0 && p1.iWoSeqID == this.iBatchWoSeqID && p1.iBatch == iBatch && p1.iSchBatch == this.iSchBatch); });
            List<SchProductRouteRes> schProductRouteResListBatch = as_schProductRouteResListNo.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID == this.iBatchWoSeqID && p1.iBatch == as_iBatch && p1.iSchBatch == this.iSchBatch); });
            

            //schProductRouteResListNo = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.bScheduled == 0 && cBatchFilter.IndexOf(p1.FResChaValue1ID) >= 0) && p1.iWoSeqID == iBatchWoSeqID && p1.iSchBatch == this.iSchBatch; });

            if (schProductRouteResListBatch.Count < 1) return -1;

            //按待排工序工艺特征重新排序                
            //schProductRouteResListBatch.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });


            //找本批中所有白茬工序的最晚完工时间 //前工序最晚开工时间
            //DateTime dMaxEndDate = SchParam.dtToday;
            DateTime dEndDateTemp = SchParam.dtToday;


            //接上批排产结束时间
            if (this.dBatchEndDate > SchParam.dtToday)
                dEndDateTemp = this.dBatchEndDate;

            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListBatch)
            {
                //排前面所有工序
                dEndDateTemp = schProductRouteRes.schProductRoute.dEarlyBegDate;

                if (dEndDateTemp > this.dBatchBegDate )  //这批的最后一个件，作为油漆开始时间)
                    this.dBatchBegDate = dEndDateTemp;
            }

            //每批开工时间加上 每批换产时间 + 每批维修时间
            this.dBatchBegDate = this.dBatchBegDate.AddMinutes(this.iPreStocks);
            this.dBatchBegDate = this.dBatchBegDate.AddMinutes(this.iPostStocks);

            //设置所有工序的最早可排日期
            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListBatch)
            {
                //排前面所有工序
                schProductRouteRes.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
            }

            //排本批每个任务排所有前工序
            foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListBatch)
            {
                //排前面所有工序
                //schProductRouteRes.schProductRoute.ProcessSchTaskPre(false);


                if (schProductRouteRes.iSchSdID == SchParam.iSchSdID && schProductRouteRes.iProcessProductID == SchParam.iProcessProductID)
                {
                    int j;
                }

                ////只排当前工序
                schProductRouteRes.schProductRoute.ProcessSchTask();

                //schProductRouteRes.schProductRoute.ProcessSchTaskNext();     

                //标记后续工序都为这个批次
                List<SchProductRouteRes> schProductRouteResListWorkItem = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iWoSeqID > this.iBatchWoSeqID  && p1.cInvCode == schProductRouteRes.cInvCode && p1.iSchSdID ==  schProductRouteRes.iSchSdID  ); });
                foreach (SchProductRouteRes schProductRouteResNext in schProductRouteResListWorkItem)
                {
                    schProductRouteResNext.iBatch = as_iBatch;

                    //记录关键工序排产顺序
                    schProductRouteResNext.iSchSN = schProductRouteRes.iSchSN ;
                }

                //记录最后一个任务
                as_SchProductRouteResLast = schProductRouteRes;

            }
            

            ////关键工序后，每个任务排后序工序,一遍一遍的排,最多10遍
            //for (int i = 0; i < 10; i++)
            //{    
            //    foreach (SchProductRouteRes schProductRouteRes in schProductRouteResListBatch)
            //    {
            //        schProductRouteRes.schProductRoute.ProcessSchTaskNext("4");              
            //    }

            //}

            //按序号递增52,53,54,55 ,油漆最大工序号为90    2014-12-23
            //int iNextSeqID = this.iBatchWoSeqID + 1;

            List<SchProductRouteRes> schProductRouteResListNext = new List<SchProductRouteRes>(10);

            List<SchProductRouteRes> schProductRouteResListNextAdd = new List<SchProductRouteRes>(10);
            

            for (int iNextSeqID = this.iBatchWoSeqID + 1; iNextSeqID < 90; iNextSeqID++)
            {
                schProductRouteResListNext = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.iWoSeqID == iNextSeqID); });


                if (schProductRouteResListNext.Count > 0)
                {
                    //按关键工序排产顺序排
                    schProductRouteResListNext.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                    ////
                    //foreach (SchProductRouteRes ResNext in schProductRouteResListNext)
                    //{
                    //    ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
                    //}

                    //当前工序52、53进行排产
                    foreach (SchProductRouteRes ResNext in schProductRouteResListNext)
                    {
                        //设置最大可排时间
                        ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;

                        //排本工序
                        ResNext.schProductRoute.ProcessSchTask();

                        //取本批工序最大完成时间
                        if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate)
                            this.dBatchBegDate = ResNext.schProductRoute.dEndDate;
                    }
                }

                //排工艺特征分批条件满足,而且可排产日期在本批范围的，一起排产,但没有分批工序                
                if (cBatchFilter != "")
                    schProductRouteResListNextAdd = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && cBatchFilter.IndexOf(p1.FResChaValue1ID) >= 0) && p1.FResChaValue1ID != "" && p1.iWoSeqID == iNextSeqID && p1.iSchBatch == this.iSchBatch && p1.iBatch != as_iBatch && p1.schProductRoute.dEarlyBegDate <= this.dBatchBegDate; });
                else    //为空时,不带条件
                    schProductRouteResListNextAdd = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0) && p1.iWoSeqID == iNextSeqID && p1.iSchBatch == this.iSchBatch && p1.FResChaValue1ID != "" && p1.iBatch != as_iBatch && p1.schProductRoute.dEarlyBegDate <= this.dBatchBegDate; });


                //
                if (schProductRouteResListNextAdd.Count > 0)
                {
                    //按关键工序排产顺序排
                    schProductRouteResListNextAdd.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                    //
                    //foreach (SchProductRouteRes ResNext in schProductRouteResListNext)
                    //{
                    //    ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
                    //}

                    //当前工序52、53进行排产
                    foreach (SchProductRouteRes ResNext in schProductRouteResListNextAdd)
                    {
                        ResNext.iBatch = as_iBatch;

                        //设置最大可排时间
                        ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;

                        //排本工序
                        ResNext.schProductRoute.ProcessSchTask();

                        //取本批工序最大完成时间
                        if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate)
                            this.dBatchBegDate = ResNext.schProductRoute.dEndDate;
                    }
                }


            } 




            //设置本批开工时间dBatchBegDate,完工时间dBatchEndDate ,下一批接着排 
            //schProductRouteResListBatch.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });
            //this.dBatchBegDate = schProductRouteResListBatch[0].dResBegDate; 
            //this.dBatchEndDate = schProductRouteResListBatch[schProductRouteResListBatch.Count - 1].dResEndDate;
            //this.dBatchBegDate = this.dBatchEndDate; 

            //取当前资源所有已排任务，维修工单，冻结工单
            List<SchProductRouteRes> schProductRouteResListSched = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 1 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.schProductRoute.cStatus != "4"); });
            schProductRouteResListSched.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

            if (schProductRouteResListSched.Count > 0)
                this.dBatchBegDate = schProductRouteResListSched[schProductRouteResListSched.Count - 1].dResEndDate;

            return 1;
        }

        //---------3.4 关键油漆资源 油漆当前已执行的这一批排产----------------------
        public int KeyResBatchSchTaskSubFirst(int as_iBatch, ref SchProductRouteRes as_SchProductRouteResLast, List<SchProductRouteRes> as_schProductRouteResListNo)
        {
            //每个批次进行排产

            //按待排工序工艺特征重新排序                
            //schProductRouteResListBatch.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return TaskComparer(p1, p2); });


            //找本批中所有白茬工序的最晚完工时间 //前工序最晚开工时间
            //DateTime dMaxEndDate = SchParam.dtToday;
            DateTime dEndDateTemp = SchParam.dtToday;

            this.dBatchBegDate = dEndDateTemp;
            //按序号递增52,53,54,55 ,油漆最大工序号为80    2014-12-23
            //int iNextSeqID = this.iBatchWoSeqID + 1;

            List<SchProductRouteRes> schProductRouteResListNext = new List<SchProductRouteRes>(10);


            for (int iNextSeqID = this.iBatchWoSeqID ; iNextSeqID < 80; iNextSeqID++)
            {
                schProductRouteResListNext = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 0 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.iWoSeqID == iNextSeqID); });



                if (schProductRouteResListNext.Count > 0)
                {
                    //按关键工序排产顺序排
                    schProductRouteResListNext.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<int>.Default.Compare(p1.iSchSN, p2.iSchSN); });

                    ////
                    //foreach (SchProductRouteRes ResNext in schProductRouteResListNext)
                    //{
                    //    ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;
                    //}

                    //当前工序52、53进行排产
                    foreach (SchProductRouteRes ResNext in schProductRouteResListNext)
                    {
                        //设置最大可排时间
                        ResNext.schProductRoute.dEarlyBegDate = this.dBatchBegDate;

                        //排本工序
                        ResNext.schProductRoute.ProcessSchTask();

                        //取本批工序最大完成时间
                        if (this.dBatchBegDate < ResNext.schProductRoute.dEndDate)
                            this.dBatchBegDate = ResNext.schProductRoute.dEndDate;
                    }
                }
            }
            
            //取当前资源所有已排任务，维修工单，冻结工单
            List<SchProductRouteRes> schProductRouteResListSched = schData.SchProductRouteResList.FindAll(delegate(SchProductRouteRes p1) { return (p1.cResourceNo == this.cResourceNo && p1.schProductRoute.BScheduled == 1 && p1.iSchBatch == this.iSchBatch && p1.iBatch == as_iBatch && p1.schProductRoute.cStatus != "4"); });
            schProductRouteResListSched.Sort(delegate(SchProductRouteRes p1, SchProductRouteRes p2) { return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate); });

            if (schProductRouteResListSched.Count > 0)
                this.dBatchBegDate = schProductRouteResListSched[schProductRouteResListSched.Count - 1].dResEndDate;

            return 1;
        }

        
        //--------4、清除资源已排具体任务占用时间段,传入工序-----------------------
        public void ResClearTask(SchProductRoute aSchProductRoute)
        {
            //调用TimeTaskClear(SchProductRouteRes as_SchProductRouteRes)，清除当前任务占用的时间段
            foreach (SchProductRouteRes as_SchProductRouteRes in aSchProductRoute.SchProductRouteResList)
            {
                this.ResClearTask(as_SchProductRouteRes);
            }
        }

        //清除资源已排具体任务占用时间段,传入资源工序任务
        public void ResClearTask(SchProductRouteRes as_SchProductRouteRes)
        {
            //找出任务对应TaskTimeRange列表
            List<TaskTimeRange> taskTimeRangeList1 = as_SchProductRouteRes.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.iSchSdID == as_SchProductRouteRes.iSchSdID && p.iProcessProductID == as_SchProductRouteRes.iProcessProductID && p.iResProcessID == as_SchProductRouteRes.iResProcessID; });

            //调用ResTimeRange的TimeTaskClear，清除当前任务占用的时间段
            foreach (TaskTimeRange taskTimeRange in taskTimeRangeList1)
            {
                taskTimeRange.TaskTimeRangeClear(as_SchProductRouteRes);
            }

            //资源工序状态设为未排产
            as_SchProductRouteRes.BScheduled = 0;
            //as_SchProductRouteRes.schProductRoute.BScheduled = 0;
        }

        //按资源物料排产顺序进行排序(cProChaType1Sort == "1") 或 按工艺特征排产顺序进行优化
        public int TaskComparer(SchProductRouteRes p1, SchProductRouteRes p2)
        {
            if (this.iSchBatch != 1)     //非执行生产任务单
            {
                if (this.cIsKey == "1" && this.cProChaType1Sort == "1")
                {
                    if (Comparer<double>.Default.Compare(p1.iResourceID, p2.iResourceID) == 0)
                    {
                        //顺序相同,按工艺特征1顺序
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
                                            //if (p1.resChaValue7 != null && p2.resChaValue7 != null && Comparer<int>.Default.Compare(p1.resChaValue7.FSchSN, p2.resChaValue7.FSchSN) == 0)
                                            //{
                                            //    if (p1.resChaValue8 != null && p2.resChaValue8 != null && Comparer<int>.Default.Compare(p1.resChaValue8.FSchSN, p2.resChaValue8.FSchSN) == 0)
                                            //    {
                                            //        if (p1.resChaValue9 != null && p2.resChaValue9 != null && Comparer<int>.Default.Compare(p1.resChaValue9.FSchSN, p2.resChaValue9.FSchSN) == 0)
                                            //        {
                                            //            if (p1.resChaValue10 != null && p2.resChaValue10 != null && Comparer<int>.Default.Compare(p1.resChaValue10.FSchSN, p2.resChaValue10.FSchSN) == 0)
                                            //            {
                                            //                if (p1.resChaValue11 != null && p2.resChaValue11 != null && Comparer<int>.Default.Compare(p1.resChaValue11.FSchSN, p2.resChaValue11.FSchSN) == 0)
                                            //                {
                                            //                    if (p1.resChaValue12 != null && p2.resChaValue12 != null && Comparer<int>.Default.Compare(p1.resChaValue12.FSchSN, p2.resChaValue12.FSchSN) == 0)
                                            //                    {
                                            //                        return 1;
                                            //                    }
                                            //                    else 
                                            //                    {
                                            //                        if (p1.resChaValue12 == null || p2.resChaValue12 == null)
                                            //                            return 1;
                                            //                        else
                                            //                            return Comparer<int>.Default.Compare(p1.resChaValue12.FSchSN, p2.resChaValue12.FSchSN);
                                            //                    }
                                            //                }
                                            //                else
                                            //                {
                                            //                    if (p1.resChaValue11 == null || p2.resChaValue11 == null)
                                            //                        return 1;
                                            //                    else
                                            //                        return Comparer<int>.Default.Compare(p1.resChaValue11.FSchSN, p2.resChaValue11.FSchSN);
                                            //                }
                                            //            }
                                            //            else
                                            //            {
                                            //                if (p1.resChaValue10 == null || p2.resChaValue10 == null)
                                            //                    return 1;
                                            //                else
                                            //                    return Comparer<int>.Default.Compare(p1.resChaValue10.FSchSN, p2.resChaValue10.FSchSN);
                                            //            }
                                            //        }
                                            //        else
                                            //        {
                                            //            if (p1.resChaValue9 == null || p2.resChaValue9 == null)
                                            //                return 1;
                                            //            else
                                            //                return Comparer<int>.Default.Compare(p1.resChaValue9.FSchSN, p2.resChaValue9.FSchSN);
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        if (p1.resChaValue8 == null || p2.resChaValue8 == null)
                                            //            return 1;
                                            //        else
                                            //            return Comparer<int>.Default.Compare(p1.resChaValue8.FSchSN, p2.resChaValue8.FSchSN);
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    if (p1.resChaValue7 == null || p2.resChaValue7 == null)
                                            //        return 1;
                                            //    else
                                            //        return Comparer<int>.Default.Compare(p1.resChaValue7.FSchSN, p2.resChaValue7.FSchSN);
                                            //}
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
                    //顺序相同,按工艺特征1顺序                  
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


        //按系统参数设置，对资源任务进行排序。2021-09-22 JonasCheng
        public int ResTaskComparer(SchProductRouteRes p1, SchProductRouteRes p2)
        {
            if (SchParam.cProChaType1Sort == "5")  //1 按计划开工时间优化，不考虑批次，按原来排产开工时间的先后顺序不动。
            {
                return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
            }
            else if (SchParam.cProChaType1Sort == "0")  //1 按资源任务优先级优化，不考虑批次号  
            {
                //1 按资源任务优先级iPriorityRes 由小到大排序
                if (Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes) == 0)
                    return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                else
                    return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes);
            }
            else if (SchParam.cProChaType1Sort == "1")  //1 按工单需求日期优化，不考虑批次号
            {
                //1 按工单需求日期iWoPriorityRes 由小到大排序
                if (p1.schProductRoute.schProductWorkItem == null || p2.schProductRoute.schProductWorkItem == null)
                {
                    if (Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority) == 0)
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    else
                        return Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority);
                }
                else
                {
                    //if (Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate) == 0)
                    //    return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    //else
                    //if (p1.schProductRoute.schProductWorkItem.dRequireDate > p2.schProductRoute.schProductWorkItem.dRequireDate) return 1;
                    //int iReturn = Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate);
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
               
                //if (SchParam.cProChaType1Sort == "0")  //1 按资源任务优先级优化
                //{
                //    //1 按资源任务优先级iPriorityRes 由小到大排序
                //    if (Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes) == 0 )
                //        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                //    else
                //        return Comparer<Int32>.Default.Compare(p1.iPriorityRes, p2.iPriorityRes);                    
                //}
                //else if (SchParam.cProChaType1Sort == "1")  //1 按工单需求日期优化
                //{
                //    //1 按工单需求日期iWoPriorityRes 由小到大排序
                //    if (p1.schProductRoute.schProductWorkItem == null || p2.schProductRoute.schProductWorkItem == null)
                //    {
                //        if (Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority) == 0)
                //            return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                //        else
                //            return Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority);
                //    }
                //    else
                //    {
                //        //if (Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate) == 0)
                //        //    return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                //        //else
                //        //if (p1.schProductRoute.schProductWorkItem.dRequireDate > p2.schProductRoute.schProductWorkItem.dRequireDate) return 1;
                //        //int iReturn = Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate);
                //        return Comparer<DateTime>.Default.Compare(p1.schProductRoute.schProductWorkItem.dRequireDate, p2.schProductRoute.schProductWorkItem.dRequireDate);
                //    }

                //}
                //else if (SchParam.cProChaType1Sort == "2")  //2 订单优先级
                //{
                //    if (p1.schProductRoute.schProductWorkItem == null || p2.schProductRoute.schProductWorkItem == null)
                //    {
                //        if (Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority) == 0)
                //            return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                //        else
                //            return Comparer<double>.Default.Compare(p1.schProductRoute.schProduct.iPriority, p2.schProductRoute.schProduct.iPriority);
                //    }
                //    else
                //    {
                //        if (Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority) == 0)
                //            return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                //        else
                //            return Comparer<double>.Default.Compare(p1.schProductRoute.schProductWorkItem.iPriority, p2.schProductRoute.schProductWorkItem.iPriority);
                //    }

                //}
                if (SchParam.cProChaType1Sort == "3")  //3 座次优先级
                {                    
                    if (Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN)== 0)
                        return Comparer<DateTime>.Default.Compare(p1.dResBegDate, p2.dResBegDate);
                    else
                        return Comparer<Int32>.Default.Compare(p1.iSchSN, p2.iSchSN);

                }
                else if (SchParam.cProChaType1Sort == "4") //1 按工艺特征优化排序
                {
                    //按工艺特征优化排序，或者按物料排产顺序排序
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


        //按资源排产



    }
}
