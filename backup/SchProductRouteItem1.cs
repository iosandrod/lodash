using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace Algorithm
{
    [Serializable]
    public class SchProductRouteItem:ISerializable 
    {
        #region //SchProductRouteItem属性定义
        public SchData schData = null;        //所有排程数据
        public int bScheduled = 0;            //是否已排产 0 未排，1 已排
        public string cVersionNo;       //排程版本号
        public int iSchSdID;            //产品号
        public int iProcessProductID;   //任务号
        public int iEntryID;             //明细ID
        public string cWoNo;             //工单号
        public int iItemID;              //
        public string cInvCode;          //加工物料
        public string cInvCodeFull;      //
        public string cSubInvCode;       //子料编号
        public string cSubInvCodeFull;    //
        public string bSelf;              // 1 自制件 0 采购件
        public int iWoSeqID;             //工序号
        public string cUtterType;        //发料类型
        public string cSubRelate;         //工艺编号
        public double iQtyPer;          //单件用量
        public double iScrapt;          //损耗率
        public double iReqQty;          //需求数量
        public double iNormalQty;       //标准用量
        public double iScrapQty;       //标准损坏数量
        public double iProQty;          //已领料数量
        public double iKeepQty;         //预留数量
        public double iPlanQty;          //需计划数量
        public DateTime dReqDate;       //子料需求时间
        public DateTime dForeInDate;      //预计入库日期
        public DateTime dEarlySubItemDate;   //最晚到料时间
        public double iAdvanceDate;             //采购提前期
        public SchProduct schProduct;            //有值
        public SchProductRoute schProductRoute;  //有值
        public int iSchBatch = 6;      //排产批次
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}