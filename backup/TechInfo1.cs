 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Algorithm
{
    public class TechInfo 
    {
        public SchData schData = null;        //所有排程数据      
        public TechInfo()
        {
        }
        public TechInfo(string cTechNo, SchData as_SchData)
        {
            this.schData = as_SchData;
            DataRow[] dr = schData.dtTechInfo.Select("cTechNo = '" + cTechNo.ToString() + "'");
            if (dr.Length < 1) return;
            GetTechInfo(dr[0]);
        }
        public void GetTechInfo(DataRow drTechInfo)
        {
            try
            {
                iInterID = (int)drTechInfo["iInterID"];                    //工艺内码
                cTechNo = drTechInfo["cTechNo"].ToString();            //工艺编号
                cTechName = drTechInfo["cTechName"].ToString();            //工艺名称
                cResClsNo = drTechInfo["cResClsNo"].ToString();              //工艺类别
                cWcNo = drTechInfo["cWcNo"].ToString();         //工作中心
                cDeptNo = drTechInfo["cDeptNo"].ToString();             //部门编号
                cResourceNo = drTechInfo["cResourceNo"].ToString();     //资源编号
                cTechReq = drTechInfo["cTechReq"].ToString();           //工艺要求
                cNote = drTechInfo["cNote"].ToString();                   //备注
                cFormula = drTechInfo["cFormula"].ToString();       //公式1
                cFormula2 = drTechInfo["cFormula2"].ToString();              //公式2
                iTechValue = Convert.ToDouble(drTechInfo["iTechValue"].ToString());         //工艺单价
                iOrder = Convert.ToDouble(drTechInfo["iOrder"].ToString());               ////0 全面考虑 1 不考虑所有提前期  //2 不考虑下层加工提前期  3 不考虑子料采购提前期 //工键工序顺序
                iTechDifficulty = Convert.ToDouble(drTechInfo["iTechDifficulty"].ToString());               //工艺加工难度系数
                iSeqPretime = Convert.ToDouble(drTechInfo["iSeqPretime"].ToString());               //工序前准备时间
                iSeqPostTime = Convert.ToDouble(drTechInfo["iSeqPostTime"].ToString());               //工序后准备时间
                cTechDefine1 = drTechInfo["cTechDefine1"].ToString();               //工艺自定义项1
                cTechDefine2 = drTechInfo["cTechDefine2"].ToString();               //工艺自定义项2
                cTechDefine3 = drTechInfo["cTechDefine3"].ToString();               //工艺自定义项3
                cTechDefine4 = drTechInfo["cTechDefine4"].ToString();               //工艺自定义项4
                cTechDefine5 = drTechInfo["cTechDefine5"].ToString();               //工艺自定义项5
                cTechDefine6 = drTechInfo["cTechDefine6"].ToString();               //工艺自定义项6
                cTechDefine7 = drTechInfo["cTechDefine7"].ToString();               //工艺自定义项7
                cTechDefine8 = drTechInfo["cTechDefine8"].ToString();               //工艺自定义项8
                cTechDefine9 = drTechInfo["cTechDefine9"].ToString();               //工艺自定义项9
                cTechDefine10 = drTechInfo["cTechDefine10"].ToString();               //工艺自定义项10
                cTechDefine11 = Convert.ToDouble(drTechInfo["cTechDefine11"].ToString());               //工艺自定义项11
                cTechDefine12 = Convert.ToDouble(drTechInfo["cTechDefine12"].ToString());               //工艺自定义项12
                cTechDefine13 = Convert.ToDouble(drTechInfo["cTechDefine13"].ToString());               //工艺自定义项13
                cTechDefine14 = Convert.ToDouble(drTechInfo["cTechDefine14"].ToString());               //工艺自定义项14
                cTechDefine15 = (DateTime)drTechInfo["cTechDefine15"];               //工艺自定义项15
                cTechDefine16 = (DateTime)drTechInfo["cTechDefine16"];               //工艺自定义项16
                cAttributeValue1 = drTechInfo["cAttributeValue1"].ToString();               //工艺特性1
                cAttributeValue2 = drTechInfo["cAttributeValue2"].ToString();               //工艺特性2
                cAttributeValue3 = drTechInfo["cAttributeValue3"].ToString();               //工艺特性3
                cAttributeValue4 = drTechInfo["cAttributeValue4"].ToString();               //工艺特性4
                cAttributeValue5 = drTechInfo["cAttributeValue5"].ToString();               //工艺特性5
                cAttributeValue6 = drTechInfo["cAttributeValue6"].ToString();               //工艺特性6
                cAttributeValue7 = drTechInfo["cAttributeValue7"].ToString();               //工艺特性7
                cAttributeValue8 = drTechInfo["cAttributeValue8"].ToString();               //工艺特性8
                cAttributeValue9 = drTechInfo["cAttributeValue9"].ToString();               //工艺特性9
                cAttributeValue10 = drTechInfo["cAttributeValue10"].ToString();             //工艺特性10
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
        #region //属性函数封装
        public int iInterID { get; set; }               //工艺ID 
        public string cTechNo  { get; set; }             //工艺编号
        public string cTechName { get; set; }         //工艺名称
        public string cResClsNo { get; set; }           //工艺类别   
        public string cWcNo { get; set; }               //工作中心
        public string cDeptNo { get; set; }             //部门编号
        public string cResourceNo { get; set; }         //资源编号
        public string cTechReq { get; set; }            //工艺要求
        public string cNote { get; set; }                //备注
        public string cFormula { get; set; }            //公式1
        public string cFormula2 { get; set; }         //公式2
        public double iTechValue { get; set; }            //工艺单价
        public double iOrder { get; set; }                //工键工序顺序
        public double iTechDifficulty { get; set; }        //工艺加工难度系数
        public double iSeqPretime { get; set; }         //工艺前准备时间
        public double iSeqPostTime { get; set; }        //工艺后准备时间
        public string cTechDefine1 { get; set; }               // 工艺自定义项1
        public string cTechDefine2 { get; set; }                // 工艺自定义项2
        public string cTechDefine3 { get; set; }
        public string cTechDefine4 { get; set; }
        public string cTechDefine5 { get; set; }
        public string cTechDefine6 { get; set; }
        public string cTechDefine7 { get; set; }
        public string cTechDefine8 { get; set; }
        public string cTechDefine9 { get; set; }
        public string cTechDefine10 { get; set; }
        public double cTechDefine11 { get; set; }
        public double cTechDefine12 { get; set; }
        public double cTechDefine13 { get; set; }
        public double cTechDefine14 { get; set; }
        public DateTime cTechDefine15 { get; set; }
        public DateTime cTechDefine16 { get; set; }    
        public string cAttributeValue1 { get; set; }               // 工艺特性1
        public string cAttributeValue2 { get; set; }                // 工艺特性2
        public string cAttributeValue3 { get; set; }
        public string cAttributeValue4 { get; set; }
        public string cAttributeValue5 { get; set; }
        public string cAttributeValue6 { get; set; }
        public string cAttributeValue7 { get; set; }
        public string cAttributeValue8 { get; set; }
        public string cAttributeValue9 { get; set; }
        public string cAttributeValue10 { get; set; }    
        #endregion
    }
}