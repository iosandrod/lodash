using System;
using System.Data;
using System.Runtime.Serialization;
namespace Algorithm
{
    [Serializable]
    public class ItemRouteSchSNType : IComparable, ICloneable, ISerializable
    {
        public SchData schData = null;        //所有排程数据
        public string cResourceNo = "";       //对应资源编号,要设置
        private SchProductRouteRes schProductRouteRes;    //工艺特征对应的资源任务
        public int iPosition = 0;                        //对应的工艺特征位置，资源任务的第几个工艺特征
        public int iUsedTime = 0;             //对应当前任务，当前刀具一个换刀周期累计使用时间。
        public ItemRouteSchSNType()
        {
        }
        public ItemRouteSchSNType(string as_cSchSNType, SchProductRouteRes schProductRouteRes, int iPosition)
        {
            this.schProductRouteRes = schProductRouteRes;
            this.iPosition = iPosition;
            this.cResourceNo = schProductRouteRes.cResourceNo;
            this.schData = schProductRouteRes.schData;
            if (as_cSchSNType == "") return;
            GetSchSNType(as_cSchSNType);
        }
        public ItemRouteSchSNType(string as_cSchSNType)
        {
            GetSchSNType(as_cSchSNType);
        }
        public void GetSchSNType(string as_cSchSNType)
        {
            DataRow[] dr = schData.dtResChaValue.Select("cInvCode = '" + as_cSchSNType.ToString() + "'");
            if (dr.Length < 1) return;
            GetSchSNType(dr[0]);
        }
        public void GetSchSNType(DataRow drResource)
        {
            try
            {
                iInterlID = (int)drResource["iInterlID"];                  //座次内码
                cInvCode = drResource["cInvCode"].ToString();                //座次分类编号
                cInvName = drResource["cInvName"].ToString();             //座次分类名称
                cSchSNTypeClsNo = drResource["cSchSNTypeClsNo"].ToString();                //特征类别
                cWcNo = drResource["cWcNo"].ToString();                //特征类别
                iSchSN =  (int) drResource["iSchSN"];                  //排产顺序
                cSchType = drResource["cSchType"].ToString();         //座次排产方案  0 正常顺序生产 1 同模同时生产(同模左中右)  2 同资源生产(排在同一资源连续生产)  3 容器生产 
                cSchBatchQty =  (int) drResource["cSchBatchQty"];     //容器每批数量
        }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        #region //属性函数封装
        public int    iInterlID;                //座次内码
        public string cInvCode;                 //座次分类编号
        public string cInvName;                 //座次分类名称
        public string FProChaTypeID;            //特征类别
        public string cSchSNTypeClsNo;          //换产周期 分   
        public string cDeptNo;                  //每次换产时间 分
        public string FResChaMemo;              //备注  
        public string cWcNo;                    //按工艺特征，最优排产顺序
        public int    iSchSN;                   //排产顺序
        public string cSchType;                 //座次排产方案 0 正常顺序生产 1 同模同时生产(同模左中右)  2 同资源生产(排在同一资源连续生产)  3 容器生产 
        public double cSchBatchQty;             //容器每批数量 
        public Resource resource;               //排产资源编号
        public DateTime dResBegTime ;           //排产开始时间
        public DateTime dResEndTime;            //排产结束时间
        #endregion
        public int CompareTo(object obj)
        {
            if (obj is ItemRouteSchSNType)
            {
                ItemRouteSchSNType newTimeRange = (ItemRouteSchSNType)obj;
                return 1;
            }
            else
            {
                throw new ArgumentException("对象非ItemRouteSchSNType类型");
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}