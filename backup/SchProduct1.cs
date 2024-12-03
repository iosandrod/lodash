using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
namespace Algorithm
{
    [Serializable]
    public class SchProduct : ISerializable// : IComparable
    {
        #region //SchProduct属性定义
        public SchData schData = null;        //所有排程数据
        public int bScheduled = 0;            //产品排产状态
        public int iSchSdID{ get; set; }
        public string cVersionNo { get; set; }
        public int iInterID { get; set; }
        public int iSdLineID { get; set; }
        public int iSeqID { get; set; }
        public int iModelID { get; set; }
        public string cSdOrderNo { get; set; } 
        public string cCustNo { get; set; }
        public string cCustName { get; set; }
        public string cSTCode { get; set; }
        public string cBusType { get; set; } 
        public int cPriorityType{ get; set; }
        public string cStatus { get; set; }
        public string cSales { get; set; } 
        public string cRequireType { get; set; }
        public int iItemID { get; set; }
        public string cInvCode { get; set; }
        public string cInvName { get; set; }
        public string cInvStd { get; set; }
        public string cUnitCode { get; set; }
        public double iReqQty { get; set; }      
        public DateTime dRequireDate{ get; set; }     //需求日期
        public DateTime dDeliveryDate{ get; set; }     //最早交货日期
        public DateTime dEarliestSchDate{ get; set; }  //最早可排日期
        public DateTime dBegDate { get; set; }       //排程开工时间
        public DateTime dEndDate { get; set; }        //排程完工时间
        public string cSchStatus { get; set; }
        public string cMiNo { get; set; } 
        public double iPriority = -1;      //产品优先级        
        public string cSelected { get; set; }
        public string cWoNo { get; set; } 
        public double iPlanQty { get; set; }
        public string cNeedSet { get; set; }
        public double iFHQuantity { get; set; }
        public double iKPQuantity { get; set; }
        public int iSourceLineID { get; set; }
        public string cColor { get; set; }
        public string cNote { get; set; } 
        public string bSet = "0"; //是否需配套 1 是，0 否
        public double iSchPriority = -1;  // //产品排产优先级,以关键工序的优先级，决定产品排产优先级 
        public int iSchBatch = 6;      //排产批次
        public string cType = "";      //计划类型
        public string cSchType = "0";      //排产类型   0 正排  1 倒排 2 无限产能倒排
        public string cPlanMode = "2";      //计划类型   2 按订单生产  3 按库存生产 (暂不用)
        public string cScheduled = "0";     //1 已排产确认，不能换资源 ; 0 未排产确认
        public int iAdvanceDate { get; set; }    //累计提前期天数 (暂不用)      
        public string cBatchNo = "";             //托盘号，不为空时，按托盘排产，同一托盘物料工艺路线类型一样，同一工序必须选择同一资源排产2020-03-22。
        public double iSchSN = 0;                 //排产座次顺序
        public string cSchSNType ;                //排产座次
        public double cGroupSN = 0;             //分组号       
        public double cGroupQty = 0;             //分组数量
        public string cCustomize = "";             //是否定制，自动生成工艺路线的
        public string cWorkRouteType = "";           //工艺路线类型
        public string cAttributes1 = "";             //加工属性1
        public string cAttributes2 = "";             //加工属性2
        public string cAttributes3 = "";             //加工属性3
        public string cAttributes4 = "";             //加工属性4
        public string cAttributes5 = "";             //加工属性5
        public string cAttributes6 = "";             //加工属性6
        public string cAttributes7 = "";             //加工属性7
        public string cAttributes8 = "";             //加工属性8
        public double cAttributes9 = 0;             //加工属性9
        public double cAttributes10 = 0;             //加工属性10
        public double cAttributes11 = 0;             //加工属性11
        public double cAttributes12 = 0;             //加工属性12
        public string cAttributes13 = "";             //加工属性13
        public string cAttributes14 = "";             //加工属性14
        public string cAttributes15 = "";             //加工属性15
        public string cAttributes16 = "";             //加工属性16
        public int iDeliveryDays { get; set; }          //交货天数,最早可排日期
        public double iWorkQtyPd { get; set; }          //日排产数量  2019-03-24
        #endregion
        public List<SchProductRoute> SchProductRouteList = new List<SchProductRoute>(10);
        public int ProductSchTask()
        {
            try
            {
                if (this.SchProductRouteList.Count < 1) return 1;  //有些产品没有工序，跳过
                List<SchProductRoute> schProductRouteTemp = SchProductRouteList.FindAll(delegate(SchProductRoute p1) { return p1.iSchSdID == this.iSchSdID && p1.cWorkItemNo.Trim().ToLower() == this.cInvCode.Trim().ToLower(); });
                schProductRouteTemp.Sort(delegate(SchProductRoute p1, SchProductRoute p2) { return Comparer<int>.Default.Compare(p1.iWoSeqID, p2.iWoSeqID); });
                if (schProductRouteTemp.Count < 1) return 1;
                SchProductRoute schProductRouteLast = schProductRouteTemp[schProductRouteTemp.Count - 1];
                schProductRouteLast.ProcessSchTaskPre();
                List<SchProductRoute> list1 = SchProductRouteList.FindAll(delegate(SchProductRoute p1) { return p1.iSchSdID == this.iSchSdID && p1.iReqQty > 0 ; });
                if (list1.Count > 0)
                {
                    list1.Sort(delegate(SchProductRoute p1, SchProductRoute p2) { return Comparer<DateTime>.Default.Compare(p1.dEndDate, p2.dEndDate); });
                    this.dBegDate = list1[0].dBegDate;                  //取已排任务中排产完成时间最大的
                    this.dEndDate = list1[list1.Count - 1].dEndDate;   //取已排任务中排产完成时间最大的
                    this.bScheduled = 1;  //标为已排产
                }
            }
            catch (Exception error)
            {
                throw new Exception("产品正排计算时出错,位置SchProduct.ProductSchTask！产品编号[" + this.cInvCode + "]\n\r " + error.Message.ToString());
                return -1;
            }
            return 1;
        }
        public int ProductSchTaskInv()
        {
            try
            {
                if (this.SchProductRouteList.Count < 1) return 1;  //有些产品没有工序，跳过
                List<SchProductRoute> schProductRouteTemp = SchProductRouteList.FindAll(delegate(SchProductRoute p1) { return p1.iSchSdID == this.iSchSdID && p1.cWorkItemNo.Trim().ToLower() == this.cInvCode.Trim().ToLower(); });
                schProductRouteTemp.Sort(delegate(SchProductRoute p1, SchProductRoute p2) { return Comparer<int>.Default.Compare(p1.iWoSeqID, p2.iWoSeqID); });
                if (schProductRouteTemp.Count < 1) return 1;
                SchProductRoute schProductRouteLast = schProductRouteTemp[schProductRouteTemp.Count - 1];
                schProductRouteLast.ProcessSchTaskRevPre("3");//1 加工物料相同的所有工序; 2 白茬工序; 3 所有下层物料半成品工序
                List<SchProductRoute> list1 = SchProductRouteList.FindAll(delegate(SchProductRoute p1) { return p1.iSchSdID == this.iSchSdID && p1.iReqQty > 0 ; });
                if (list1.Count > 0)
                {
                    list1.Sort(delegate(SchProductRoute p1, SchProductRoute p2) { return Comparer<DateTime>.Default.Compare(p1.dEndDate, p2.dEndDate); });
                    this.dBegDate = list1[0].dBegDate;                  //取已排任务中排产完成时间最大的
                    this.dEndDate = list1[list1.Count - 1].dEndDate;   //取已排任务中排产完成时间最大的
                    this.bScheduled = 1;  //标为已排产
                }
            }
            catch (Exception error)
            {
                throw new Exception("产品正排计算时出错,位置SchProduct.ProductSchTask！产品编号[" + this.cInvCode + "]\n\r " + error.Message.ToString());
                return -1;
            }
            return 1;
        }
        public int ProductSchTaskRev(string bSet = "0")
        {
            try
            {
                if (this.SchProductRouteList.Count < 1) return 1;  //有些产品没有工序，跳过
                List<SchProductRoute> schProductRouteTemp = SchProductRouteList.FindAll(delegate (SchProductRoute p1) { return p1.iSchSdID == this.iSchSdID && p1.cWorkItemNo.Trim() == this.cInvCode; });
                schProductRouteTemp.Sort(delegate (SchProductRoute p1, SchProductRoute p2) { return Comparer<int>.Default.Compare(p1.iWoSeqID, p2.iWoSeqID); });
                if (schProductRouteTemp.Count < 1) return 1;
                SchProductRoute schProductRouteFirst = schProductRouteTemp[0];
                if (this.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                {
                    int i = 1;
                }
                DateTime dtCanBegDate;
                if (SchParam.SetMinDelayTime > 0)
                { 
                    foreach (SchProductRoute schProductRoute in schProductRouteFirst.SchProductRoutePreList)
                    {                        
                        dtCanBegDate = schProductRoute.dBegDate.AddHours(SchParam.SetMinDelayTime + schProductRoute.iSeqPostTime / 60 + schProductRoute.iMoveTime/60 + schProductRouteFirst.iSeqPreTime / 60);
                        if (schProductRoute.BScheduled == 1 && dtCanBegDate > schProductRouteFirst.dBegDate)
                            continue;
                        if (schProductRoute.iProcessProductID == SchParam.iProcessProductID && schProductRoute.iSchSdID == SchParam.iSchSdID)  //调试断点1 SchProduct
                        {
                            int i = 1;
                        }
                        if (schProductRoute.iReqQty > 0 )
                            schProductRoute.ProcessSchTaskRevPre(SchParam.SchTaskRevTag,bSet);    ////1 加工物料相同的所有工序; 2 白茬工序 ; 3 所有半成品工序
                    }
                }
                List<SchProductRoute> list1 = SchProductRouteList.FindAll(delegate(SchProductRoute p1) { return p1.iSchSdID == this.iSchSdID && p1.iReqQty > 0 ; });
                if (list1.Count > 0)
                {
                    list1.Sort(delegate(SchProductRoute p1, SchProductRoute p2) { return Comparer<DateTime>.Default.Compare(p1.dEndDate, p2.dEndDate); });
                    this.dBegDate = list1[0].dBegDate;                  //取已排任务中排产完成时间最大的
                    this.dEndDate = list1[list1.Count - 1].dEndDate;    //取已排任务中排产完成时间最大的
                }
            }
            catch (Exception error)
            {
                throw new Exception("产品倒排计算时出错,位置SchProduct.ProductSchTaskRev！排程ID[" + this.iSchSdID + "],产品编号[" + this.cInvCode + "]\n\r "  + error.Message.ToString());
                return -1;
            }
            return 1;
        }
        public void ProductClearTask(int ai_SchSdID, int ai_taskID)
        {
            foreach (SchProductRoute schProductRoute in this.SchProductRouteList)
            {
                schProductRoute.ProcessClearTask();
            }
            this.bScheduled = 0;
        }
        public void SetSchBatch(int as_iSchBatch)
        {
            this.iSchBatch = as_iSchBatch;
            foreach (SchProductRoute schProductRoute in this.SchProductRouteList)
            {
                schProductRoute.iSchBatch = as_iSchBatch;
                foreach (SchProductRouteRes schProductRouteRes in schProductRoute.SchProductRouteResList)
                {
                    schProductRouteRes.iSchBatch = as_iSchBatch;
                }
            }
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}