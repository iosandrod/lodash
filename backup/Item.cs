 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace Algorithm
{
    public class Item 
    {
        public SchData schData = null;        //所有排程数据      
        

        //初始化结构函数
        public Item()
        {

        }

        //public WorkCenter(string cWcNo)
        //{
        //    if (cWcNo == "") return;

        //    //取工艺特征属性
        //    GetWorkCenter(cWcNo);
        //}

        public Item(string cInvCode, SchData as_SchData)
        {
            //select  a.iItemID, a.cInvCode, a.cInvName,a.cInvStd,a.cItemClsNo, isnull(a.cVenCode,'') as cVenCode, isnull(a.bSale,'0') as bSale,isnull(a.bPurchase,'0') as bPurchase,isnull(a.bSelf,'1') as bSelf,isnull(a.bProxyForeign,'0') as bProxyForeign,isnull(a.cComUnitCode,'') as cComUnitCode,isnull(a.cWcNo,'') as cWcNo,isnull(a.iProSec,'') as iProSec,isnull(a.iPriority,'') as iPriority, 
            //   isnull(a.cWorkRouteType,'') as cWorkRouteType,isnull(a.cTechNo,'') as cTechNo,isnull(a.cKeyResourceNo,'') as cKeyResourceNo,isnull(a.cInjectItemType,'') as cInjectItemType,isnull(a.cMoldNo,'') as cMoldNo,isnull(a.cSubMoldNo,'') as cSubMoldNo,isnull(a.cMoldPosition,'') as cMoldPosition,isnull(a.iMoldSubQty,0) as iMoldSubQty,
            //   isnull(a.cMaterial,'') as cMaterial,isnull(a.cColor,'') as cColor,isnull(a.fVolume,0) as fVolume,isnull(a.flength,0) as flength,isnull(a.fWidth,0) as fWidth,isnull(a.fHeight,0) as fHeight,isnull(a.fNetWeight,0) as fNetWeight, isnull(a.iItemDifficulty,1 ) as iItemDifficulty , isnull(a.cSize1,'') as cSize1,isnull(a.cSize2,'') as cSize2,isnull(a.cSize3,'') as cSize3,isnull(a.cSize4,'') as cSize4,isnull(a.cSize5,'') as cSize5,isnull(a.cSize6,'') as cSize6 ,
            //   isnull(a.cSize7,'') as cSize7,isnull(a.cSize8,'') as cSize8,isnull(a.cSize9,'') as cSize9,isnull(a.cSize10,'') as cSize10,isnull(a.cSize11,0) as cSize11,isnull(a.cSize12,0) as cSize12,isnull(a.cSize13,0) as cSize13,isnull(a.cSize14,0) as cSize14,isnull(a.cSize15,'') as cSize15,isnull(a.cSize16,'') as cSize16
            //   FROM t_Item a 
            //   where a.cInvCode in (select distinct cWorkItemNo from t_SchProductRoute  )		

            this.schData = as_SchData;

            DataRow[] dr = schData.dtItem.Select("cInvCode = '" + cInvCode.ToString() + "'");
            if (dr.Length < 1) return;

            GetItem(dr[0]);
        }

        public void GetItem(DataRow drItem)
        {
            try
            {
                //select  a.iItemID, a.cInvCode, a.cInvName,a.cInvStd,a.cItemClsNo, isnull(a.cVenCode,'') as cVenCode, isnull(a.bSale,'0') as bSale,isnull(a.bPurchase,'0') as bPurchase,isnull(a.bSelf,'1') as bSelf,isnull(a.bProxyForeign,'0') as bProxyForeign,isnull(a.cComUnitCode,'') as cComUnitCode,isnull(a.cWcNo,'') as cWcNo,isnull(a.iProSec,'') as iProSec,isnull(a.iPriority,'') as iPriority, 
                //   isnull(a.cWorkRouteType,'') as cWorkRouteType,isnull(a.cTechNo,'') as cTechNo,isnull(a.cKeyResourceNo,'') as cKeyResourceNo,isnull(a.cInjectItemType,'') as cInjectItemType,isnull(a.cMoldNo,'') as cMoldNo,isnull(a.cSubMoldNo,'') as cSubMoldNo,isnull(a.cMoldPosition,'') as cMoldPosition,isnull(a.iMoldSubQty,0) as iMoldSubQty,
                //   isnull(a.cMaterial,'') as cMaterial,isnull(a.cColor,'') as cColor,isnull(a.fVolume,0) as fVolume,isnull(a.flength,0) as flength,isnull(a.fWidth,0) as fWidth,isnull(a.fHeight,0) as fHeight,isnull(a.fNetWeight,0) as fNetWeight, isnull(a.iItemDifficulty,1 ) as iItemDifficulty , isnull(a.cSize1,'') as cSize1,isnull(a.cSize2,'') as cSize2,isnull(a.cSize3,'') as cSize3,isnull(a.cSize4,'') as cSize4,isnull(a.cSize5,'') as cSize5,isnull(a.cSize6,'') as cSize6 ,
                //   isnull(a.cSize7,'') as cSize7,isnull(a.cSize8,'') as cSize8,isnull(a.cSize9,'') as cSize9,isnull(a.cSize10,'') as cSize10,isnull(a.cSize11,0) as cSize11,isnull(a.cSize12,0) as cSize12,isnull(a.cSize13,0) as cSize13,isnull(a.cSize14,0) as cSize14,isnull(a.cSize15,'') as cSize15,isnull(a.cSize16,'') as cSize16
                //   FROM t_Item a 
                //   where a.cInvCode in (select distinct cWorkItemNo from t_SchProductRoute  )		


        //         public int iItemID;          //物料ID 
        //public string cInvCode;      //物料编号
        //public string cInvName;      //物料名称
        //public string cInvStd;       //物料规格
        //public string cItemClsNo;      //类别
        //public string cVenCode;      //供应商编号
        //public string bSale;         //可销售
        //public string bPurchase;     //可采购
        //public string bSelf;         //自制
        //public string bProxyForeign;    //委外

        //public string cComUnitCode;     //单位
        //public double iSafeStock;       //安全库存
        //public double iTopLot;         //库存高警
        //public double iLowLot;         //库存低警
        //public double iIncLot;         //批量增量
        //public double iAvgLot;         //平均批量
        //public string cLeadTimeType;         //提前期类型
        //public double iAvgLeadTime;        //加工提前期
        //public double iAdvanceDate;        //累计提前期
        //public string cInjectItemType;      //注塑类型
        //public string cMoldNo;             //模具编号
        //public string cSubMoldNo;          //子模具编号
        //public string cMoldPosition;      //模具位置
        //public double iMoldSubQty;       //模腔数
        //public string cWcNo;             //工作中心
        //public string cTechNo;           //工艺编号
        //public string cRouteCode;           //工艺路线号
        //public string cKeyResourceNo;           //关键资源
        //public double iProSec;           //单件工时
        //public double iItemDifficulty;           //加工难度系数
               

                iItemID = (int)drItem["iItemID"];                    //物料内码
                cInvCode = drItem["cInvCode"].ToString();            //物料编号
                cInvName = drItem["cInvName"].ToString();            //物料名称
                cInvStd = drItem["cInvStd"].ToString();              //物料类别

                cItemClsNo = drItem["cItemClsNo"].ToString();         //类别
                cVenCode = drItem["cVenCode"].ToString();             //供应商编号
                bSale = drItem["bSale"].ToString();                   //可销售
                bPurchase = drItem["bPurchase"].ToString();           //可采购
                bSelf = drItem["bSelf"].ToString();                   //自制
                bProxyForeign = drItem["bProxyForeign"].ToString();       //委外
                cComUnitCode = drItem["cComUnitCode"].ToString();              //单位编号
                iSafeStock = Convert.ToDouble(drItem["iSafeStock"].ToString());         //安全库存
                iTopLot = Convert.ToDouble(drItem["iTopLot"].ToString());               //库存高警
                iLowLot = Convert.ToDouble(drItem["iLowLot"].ToString());               //库存低警
                iIncLot = Convert.ToDouble(drItem["iIncLot"].ToString());               //批量增量
                iAvgLot = Convert.ToDouble(drItem["iAvgLot"].ToString());               //平均批量


                cLeadTimeType = drItem["cLeadTimeType"].ToString();                     //提前期类型
                iAvgLeadTime = Convert.ToDouble(drItem["iAvgLeadTime"].ToString());     //加工提前期
                iAdvanceDate = Convert.ToDouble(drItem["iAdvanceDate"].ToString());     //采购提前期
                cInjectItemType = drItem["cInjectItemType"].ToString();                 //注塑类型
                cMoldNo = drItem["cMoldNo"].ToString();                                 //模具编号
                cSubMoldNo = drItem["cSubMoldNo"].ToString();                           //子模具编号
                cMoldPosition = drItem["cMoldPosition"].ToString();                     //模具编号位置
                iMoldSubQty = Convert.ToDouble(drItem["iMoldSubQty"].ToString());       //模腔数
                iMoldCount = Convert.ToInt32(drItem["iMoldCount"].ToString());         //模具套数
                
                cWcNo = drItem["cWcNo"].ToString();                                     //工作中心
                cTechNo = drItem["cTechNo"].ToString();                        //部门
                cRouteCode = drItem["cRouteCode"].ToString();              //能力类型
                cKeyResourceNo = drItem["cKeyResourceNo"].ToString();                        //部门

                iProSec = Convert.ToDouble(drItem["iProSec"].ToString());             //能力类型
                iItemDifficulty = Convert.ToDouble(drItem["iItemDifficulty"].ToString());                       //部门

                cMaterial = drItem["cMaterial"].ToString();                                     //材质
                cColor = drItem["cColor"].ToString();                                           //颜色
                fVolume = Convert.ToDouble(drItem["fVolume"].ToString());                       //体积
                flength = Convert.ToDouble(drItem["flength"].ToString());                       //长度
                fWidth = Convert.ToDouble(drItem["fWidth"].ToString());                         //宽度
                fHeight = Convert.ToDouble(drItem["fHeight"].ToString());                       //高度
                fNetWeight = Convert.ToDouble(drItem["fNetWeight"].ToString());                 //净重
                   
                cSize1 = drItem["cSize1"].ToString();               //物料自定义项1
                cSize2 = drItem["cSize2"].ToString();               //物料自定义项2
                cSize3 = drItem["cSize3"].ToString();               //物料自定义项3
                cSize4 = drItem["cSize4"].ToString();               //物料自定义项4
                cSize5 = drItem["cSize5"].ToString();               //物料自定义项5
                cSize6 = drItem["cSize6"].ToString();               //物料自定义项6
                cSize7 = drItem["cSize7"].ToString();               //物料自定义项7
                cSize8 = drItem["cSize8"].ToString();               //物料自定义项8
                cSize9 = drItem["cSize9"].ToString();               //物料自定义项9
                cSize10 = drItem["cSize10"].ToString();               //物料自定义项10
                cSize11 = Convert.ToDouble(drItem["cSize11"].ToString());               //物料自定义项11
                cSize12 = Convert.ToDouble(drItem["cSize12"].ToString());               //物料自定义项12
                cSize13 = Convert.ToDouble(drItem["cSize13"].ToString());               //物料自定义项13
                cSize14 = Convert.ToDouble(drItem["cSize14"].ToString());               //物料自定义项14
                cSize15 = (DateTime)drItem["cSize15"];               //物料自定义项15
                cSize16 = (DateTime)drItem["cSize16"];               //物料自定义项16
                               
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
        

        #region //属性函数封装

        //


        public int iItemID { get; set; }        //物料ID 
        public string cInvCode { get; set; }    //物料编号
        public string cInvName { get; set; }      //物料名称
        public string cInvStd { get; set; }       //物料规格
        public string cItemClsNo { get; set; }     //类别
        public string cVenCode { get; set; }      //供应商编号
        public string bSale { get; set; }         //可销售
        public string bPurchase { get; set; }     //可采购
        public string bSelf { get; set; }         //自制
        public string bProxyForeign { get; set; }    //委外

        public string cComUnitCode { get; set; }     //单位
        public double iSafeStock { get; set; }       //安全库存
        public double iTopLot{ get; set; }         //库存高警
        public double iLowLot { get; set; }         //库存低警
        public double iIncLot { get; set; }         //批量增量
        public double iAvgLot { get; set; }         //平均批量
        public string cLeadTimeType { get; set; }        //提前期类型
        public double iAvgLeadTime { get; set; }        //加工提前期
        public double iAdvanceDate { get; set; }        //累计提前期
        public string cInjectItemType { get; set; }      //注塑类型
        public string cMoldNo { get; set; }             //模具编号
        public string cSubMoldNo { get; set; }          //子模具编号
        public string cMoldPosition { get; set; }     //模具位置
        public double iMoldSubQty { get; set; }       //模腔数
        public int iMoldCount { get; set; }         //模具套数
        public string cWcNo { get; set; }            //工作中心
        public string cTechNo { get; set; }          //工艺编号
        public string cRouteCode { get; set; }           //工艺路线号
        public string cKeyResourceNo { get; set; }           //关键资源
        public double iProSec { get; set; }           //单件工时
        public double iItemDifficulty { get; set; }           //加工难度系数

        public string cMaterial { get; set; }        //材质
        public string cColor { get; set; }            //颜色
        public double fVolume{ get; set; }           //体积
        public double flength { get; set; }           //长度
        public double fWidth { get; set; }            //宽度
        public double fHeight { get; set; }          //高度
        public double fNetWeight { get; set; }        //净重


        public string cSize1 { get; set; }            // 物料自定义项1
        public string cSize2 { get; set; }            // 物料自定义项2
        public string cSize3 { get; set; }
        public string cSize4 { get; set; }
        public string cSize5 { get; set; }
        public string cSize6 { get; set; }
        public string cSize7 { get; set; }
        public string cSize8 { get; set; }
        public string cSize9 { get; set; }
        public string cSize10 { get; set; }
        public double cSize11 { get; set; }
        public double cSize12 { get; set; }
        public double cSize13 { get; set; }
        public double cSize14 { get; set; }
        public DateTime cSize15 { get; set; }
        public DateTime cSize16 { get; set; }

        #endregion
    }
}