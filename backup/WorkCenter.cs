 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace Algorithm
{
    public class WorkCenter 
    {
        public SchData schData = null;        //所有排程数据      
        public List<Resource> ListResource = new List<Resource>(10);   //资源列表

        //初始化结构函数
        public WorkCenter()
        {

        }

        //public WorkCenter(string cWcNo)
        //{
        //    if (cWcNo == "") return;

        //    //取工艺特征属性
        //    GetWorkCenter(cWcNo);
        //}

        public WorkCenter(string cWcNo, SchData as_SchData)
        {
            //取工艺特征值
            //            string lsSql = @"SELECT     iInterID, cLearnCurvesNo, cLearnCurvesName, cTechNo, iDayDis1, iDayDis2, iDayDis3, iDayDis4, iDayDis5, iDayDis6, iDayDis7, iDayDis8, iDayDis9, iDayDis10, 
            //                      iDayDis11, iDayDis12, iDayDis13, iDayDis14, iDayDis15, iDayDis16, iDayDis17, iDayDis18, iDayDis19, iDayDis20, iDayDis21, iDayDis22, iDayDis23, iDayDis24, 
            //                      iDayDis25, iDayDis26, iDayDis27, iDayDis28, iDayDis29, iDayDis30, iDayDis31, iDiffCoe, iCapacity, iResPreTime, cNote, cDefine22, cDefine23, cDefine24, 
            //                      cDefine25, cDefine26
            //FROM         dbo.t_TechLearnCurves
            //               where 1 = 1  and cLearnCurvesNo = " + cLearnCurvesNo;
            //            DataTable dtResChaValue = Common.GetDataTable(lsSql);

            //            GetResChaValue(dtResChaValue.Rows[0]);

            this.schData = as_SchData;

            DataRow[] dr = schData.dtWorkCenter.Select("cWcNo = '" + cWcNo.ToString() + "'");
            if (dr.Length < 1) return;

            GetWorkCenter(dr[0]);
        }

        public void GetWorkCenter(DataRow drWorkCenter)
        {
            try
            {
                iWcID = (int)drWorkCenter["iWcID"];                          //工作中心内码
                cWcNo = drWorkCenter["cWcNo"].ToString();                    //工作中心编号
                cWcName = drWorkCenter["cWcName"].ToString();                //工作中心名称
                cWcClsNo = drWorkCenter["cWcClsNo"].ToString();              //工作中心类别

                cAbilityMode = drWorkCenter["cAbilityMode"].ToString();              //能力类型
                cDeptNo = drWorkCenter["cDeptNo"].ToString();                        //部门
                iWorkersPd = Convert.ToDouble(drWorkCenter["iWorkersPd"].ToString());              //日人工数
                iShiftPd = Convert.ToDouble(drWorkCenter["iShiftPd"].ToString());              //班次
                iDevCount = Convert.ToDouble(drWorkCenter["iDevCount"].ToString());              //设备数
                iUsage = Convert.ToDouble(drWorkCenter["iUsage"].ToString());              //利用率
                iEfficient = Convert.ToDouble(drWorkCenter["iEfficient"].ToString());              //效率(%)
                cEntrustNo = drWorkCenter["cEntrustNo"].ToString();                             //部门

                bKeyRes = drWorkCenter["bKeyRes"].ToString();                             //部门
                cStatus = drWorkCenter["cStatus"].ToString();                             //部门
                iLaborRate = Convert.ToDouble(drWorkCenter["iLaborRate"].ToString());     //人工费率
                iFixRate = Convert.ToDouble(drWorkCenter["iFixRate"].ToString());         //人工费率
                iFlexRate = Convert.ToDouble(drWorkCenter["iFlexRate"].ToString());       //人工费率

                bBackflash = drWorkCenter["bBackflash"].ToString();                             //是否缓冲点
                bWasteDirect = drWorkCenter["bWasteDirect"].ToString();                             //部门
                cDutyor = drWorkCenter["cDutyor"].ToString();                             //责任人
                bFakeRet = drWorkCenter["bFakeRet"].ToString();                           //退料点
                cNote = drWorkCenter["cNote"].ToString();                                 //备注
                cLaborRateType = drWorkCenter["cLaborRateType"].ToString();               //费率类型

                iOverHours = Convert.ToDouble(drWorkCenter["iOverHours"].ToString());       //允许加班小时
                cPrvNo = drWorkCenter["cPrvNo"].ToString();               //费率类型

                cWcDefine1 = drWorkCenter["cWcDefine1"].ToString();               //工作中心自定义项1
                cWcDefine2 = drWorkCenter["cWcDefine2"].ToString();               //工作中心自定义项2
                cWcDefine3 = drWorkCenter["cWcDefine3"].ToString();               //工作中心自定义项3
                cWcDefine4 = drWorkCenter["cWcDefine4"].ToString();               //工作中心自定义项4
                cWcDefine5 = drWorkCenter["cWcDefine5"].ToString();               //工作中心自定义项5
                cWcDefine6 = drWorkCenter["cWcDefine6"].ToString();               //工作中心自定义项6
                cWcDefine7 = drWorkCenter["cWcDefine7"].ToString();               //工作中心自定义项7
                cWcDefine8 = drWorkCenter["cWcDefine8"].ToString();               //工作中心自定义项8
                cWcDefine9 = drWorkCenter["cWcDefine9"].ToString();               //工作中心自定义项9
                cWcDefine10 = drWorkCenter["cWcDefine10"].ToString();               //工作中心自定义项10
                cWcDefine11 = Convert.ToDouble(drWorkCenter["cWcDefine11"].ToString());               //工作中心自定义项11
                cWcDefine12 = Convert.ToDouble(drWorkCenter["cWcDefine12"].ToString());               //工作中心自定义项12
                cWcDefine13 = Convert.ToDouble(drWorkCenter["cWcDefine13"].ToString());               //工作中心自定义项13
                cWcDefine14 = Convert.ToDouble(drWorkCenter["cWcDefine14"].ToString());               //工作中心自定义项14
                cWcDefine15 = (DateTime)drWorkCenter["cWcDefine15"];               //工作中心自定义项15
                cWcDefine16 = (DateTime)drWorkCenter["cWcDefine16"];               //工作中心自定义项16
                               
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
        

        #region //属性函数封装

        public   int       iWcID;      //工作中心ID 
        public   string    cWcNo;      //工作中心编号
        public   string    cWcName;      //工作中心名称
        public   string    cWcClsNo;      //类别
        //public   int       iSchemeID;      //日历类型
        public   string    cAbilityMode;      //能力类型
        public   string    cDeptNo;         //部门
        public   double    iWorkersPd;      //日人工数
        public   double    iShiftPd;        //班次
        public   double    iDevCount;       //设备数
        public   double    iUsage;          //利用率
        public   double    iEfficient;       //效率(%)
        public   string    cEntrustNo;         //部门
        public   string    bKeyRes;         //是否关键工作中心
        public   string    cStatus;         //是否关键工作中心
        public   double    iLaborRate;         //人工费率
        public   double    iFixRate;         //人工费率
        public   double    iFlexRate;        //人工费率
        public   string    bBackflash;        //是否缓冲点
        public   string    bWasteDirect;      //
        public   string    cDutyor;          //责任人
        public   string    bFakeRet;          //退料点
        public   string    cNote;             //备注
        public   string    cLaborRateType;   //费率类型
        public   double    iOverHours;       //允许加班小时
        public   string    cPrvNo;           //权限编号

        
        public string cWcDefine1;            // 工作中心自定义项1
        public string cWcDefine2;            // 工作中心自定义项2
        public string cWcDefine3;
        public string cWcDefine4;
        public string cWcDefine5;
        public string cWcDefine6;
        public string cWcDefine7;
        public string cWcDefine8;
        public string cWcDefine9;
        public string cWcDefine10;
        public double cWcDefine11;
        public double cWcDefine12;
        public double cWcDefine13;
        public double cWcDefine14;
        public DateTime cWcDefine15;
        public DateTime cWcDefine16;

        #endregion
    }
}