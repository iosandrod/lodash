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
    }
}