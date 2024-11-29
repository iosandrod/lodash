using System;
using System.Data;
using System.Runtime.Serialization;

namespace Algorithm
{
    /// <summary>
    /// 时间段
    /// </summary>
    [Serializable]
    public class ItemRouteSchSNType : IComparable, ICloneable, ISerializable
    {
        public SchData schData = null;        //所有排程数据
        public string cResourceNo = "";       //对应资源编号,要设置
        private SchProductRouteRes schProductRouteRes;    //工艺特征对应的资源任务
        public int iPosition = 0;                        //对应的工艺特征位置，资源任务的第几个工艺特征
        public int iUsedTime = 0;             //对应当前任务，当前刀具一个换刀周期累计使用时间。

        //任务资源明细的工艺特征，一个任务有几个工艺特征，就有几个工艺特征实例

        //初始化结构函数
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

            //取工艺特征属性
            GetSchSNType(as_cSchSNType);
        }

        //初始化，传入资源编号
        public ItemRouteSchSNType(string as_cSchSNType)
        {
            GetSchSNType(as_cSchSNType);
        }

        public void GetSchSNType(string as_cSchSNType)
        {
            //取工艺特征值
            //            string lsSql = @"iInterlID, cInvCode, cInvName, cSchSNTypeClsNo, cDeptNo, cWcNo, cAttributes1, cAttributes2, cAttributes3, cAttributes4, 
                                        //            cAttributes5, cAttributes6, cAttributes7, cAttributes8, cAttributes9, cAttributes10, cAttributes11, cAttributes12, 
                                        //                cAttributes13, cAttributes14, cAttributes15, cAttributes16, cSchSNType, iSchSN
                                        //FROM      dbo.t_ItemRouteSchSNType
            //                            where 1 = 1  and FResChaValueID = " + iResChaValueID;
            //            DataTable dtResChaValue = Common.GetDataTable(lsSql);

            //            GetResChaValue(dtResChaValue.Rows[0]);

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

        //iInterlID, cInvCode, cInvName, cSchSNTypeClsNo, cDeptNo, cWcNo, cAttributes1, cAttributes2, cAttributes3, cAttributes4, 
        //            cAttributes5, cAttributes6, cAttributes7, cAttributes8, cAttributes9, cAttributes10, cAttributes11, cAttributes12, 
        //                cAttributes13, cAttributes14, cAttributes15, cAttributes16, cSchSNType, iSchSN
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

        //public string cAttributes1;             //加工属性1  
        //public string cAttributes2;             //加工属性2         
        //public string cAttributes3;             //加工属性3
        //public string cAttributes4;             //加工属性4
        //public string cAttributes5;             //加工属性5
        //public string cAttributes6;             //加工属性6
        //public string cAttributes7;             //加工属性7
        //public string cAttributes8;             //加工属性8
        //public double cAttributes9;             //加工属性9
        //public double cAttributes10;            //加工属性10
        //public double cAttributes11;            //加工属性11
        //public double cAttributes12;            //加工属性12
        //public string cAttributes13;            //加工属性13
        //public string cAttributes14;            //加工属性14
        //public string cAttributes15;          //加工属性15
        //public string cAttributes16;          //加工属性16

        #endregion


        /// <summary>
        /// 实现IComparable接口，便于TimeRange类集合排序
        /// </summary>
        /// <param name="obj">要比较的TimeRange类对象</param>
        /// <returns>开始时间之差</returns>
        public int CompareTo(object obj)
        {
            if (obj is ItemRouteSchSNType)
            {
                ItemRouteSchSNType newTimeRange = (ItemRouteSchSNType)obj;

                //if (this.dBegTime == newTimeRange.dBegTime && this.dEndTime == newTimeRange.dEndTime)
                //{
                return 1;
                //}
                //else
                //{
                //    return -1;

                //}
            }
            else
            {
                throw new ArgumentException("对象非ItemRouteSchSNType类型");
            }
        }


        /// <summary>
        /// 对象拷贝
        /// </summary>
        /// <returns></returns>
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

