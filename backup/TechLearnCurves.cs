using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Algorithm
{
    public class TechLearnCurves 
    {
        public SchData schData = null;        //所有排程数据
        private SchProductRouteRes schProductRouteRes;    //工艺特征对应的资源任务
        public string cResourceNo = "";       //对应资源编号,要设置

        //初始化结构函数
        public TechLearnCurves()
        {

        }

        public TechLearnCurves(string cLearnCurvesNo, SchProductRouteRes schProductRouteRes)
        {
            this.schProductRouteRes = schProductRouteRes;           
            this.cResourceNo = schProductRouteRes.cResourceNo;
            this.schData = schProductRouteRes.schData;

            if (cLearnCurvesNo == "") return;

            //取工艺特征属性
            GetTechLearnCurves(cLearnCurvesNo);
        }

        ////初始化，传入资源编号
        //public TechLearnCurves(string cLearnCurvesNo)
        //{
        //    GetTechLearnCurves(cLearnCurvesNo);
        //}
                
        public void GetTechLearnCurves(string cLearnCurvesNo)
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

            DataRow[] dr = schData.dtTechLearnCurves.Select("cLearnCurvesNo = '" + cLearnCurvesNo.ToString() + "'");
            if (dr.Length < 1) return;

            GetTechLearnCurves(dr[0]);
        }

        public void GetTechLearnCurves(DataRow drTechLearnCurves)
        {
            try
            {
                iInterID = (int)drTechLearnCurves["iInterID"];                                  //工艺特征内码
                cLearnCurvesNo = drTechLearnCurves["cLearnCurvesNo"].ToString();                //工艺特征编号
                cLearnCurvesName = drTechLearnCurves["cLearnCurvesName"].ToString();                //工艺特征编号
                cTechNo = drTechLearnCurves["cTechNo"].ToString();                               //工艺编号
                iDayDis1 = Convert.ToDouble(drTechLearnCurves["iDayDis1"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis2 = Convert.ToDouble(drTechLearnCurves["iDayDis2"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis3 = Convert.ToDouble(drTechLearnCurves["iDayDis3"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis4 = Convert.ToDouble(drTechLearnCurves["iDayDis4"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis5 = Convert.ToDouble(drTechLearnCurves["iDayDis5"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis6 = Convert.ToDouble(drTechLearnCurves["iDayDis6"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis7 = Convert.ToDouble(drTechLearnCurves["iDayDis7"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis8 = Convert.ToDouble(drTechLearnCurves["iDayDis8"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis9 = Convert.ToDouble(drTechLearnCurves["iDayDis9"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis10 = Convert.ToDouble(drTechLearnCurves["iDayDis10"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis11 = Convert.ToDouble(drTechLearnCurves["iDayDis11"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis12 = Convert.ToDouble(drTechLearnCurves["iDayDis12"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis13 = Convert.ToDouble(drTechLearnCurves["iDayDis13"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis14 = Convert.ToDouble(drTechLearnCurves["iDayDis14"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis15 = Convert.ToDouble(drTechLearnCurves["iDayDis15"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis16 = Convert.ToDouble(drTechLearnCurves["iDayDis16"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis17 = Convert.ToDouble(drTechLearnCurves["iDayDis17"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis18 = Convert.ToDouble(drTechLearnCurves["iDayDis18"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis19 = Convert.ToDouble(drTechLearnCurves["iDayDis19"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis20 = Convert.ToDouble(drTechLearnCurves["iDayDis20"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis21 = Convert.ToDouble(drTechLearnCurves["iDayDis21"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis22 = Convert.ToDouble(drTechLearnCurves["iDayDis22"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis23 = Convert.ToDouble(drTechLearnCurves["iDayDis23"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis24 = Convert.ToDouble(drTechLearnCurves["iDayDis24"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis25 = Convert.ToDouble(drTechLearnCurves["iDayDis25"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis26 = Convert.ToDouble(drTechLearnCurves["iDayDis26"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis27 = Convert.ToDouble(drTechLearnCurves["iDayDis27"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis28 = Convert.ToDouble(drTechLearnCurves["iDayDis28"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis29 = Convert.ToDouble(drTechLearnCurves["iDayDis29"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis30 = Convert.ToDouble(drTechLearnCurves["iDayDis30"].ToString());                 //学习曲线第1天标准产能折扣
                iDayDis31 = Convert.ToDouble(drTechLearnCurves["iDayDis31"].ToString());                 //学习曲线第1天标准产能折扣
    
                cDefine1 = drTechLearnCurves["cDefine22"].ToString();
                cDefine2 = drTechLearnCurves["cDefine23"].ToString();
                cDefine3 = drTechLearnCurves["cDefine24"].ToString();
                cDefine4 = drTechLearnCurves["cDefine25"].ToString();
                cDefine5 = drTechLearnCurves["cDefine26"].ToString();
               
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }

        //获取每天产能折扣
        public double GetDayDisValue(DateTime dtBegDate,DateTime dtCurDate)
        {
            double iDayDis = 1;

            //如果工序实际开工日期不为空，取实际开工日期，作为计划开始日期
            if (this.schProductRouteRes.dActResBegDate != null && this.schProductRouteRes.dActResBegDate > DateTime.Parse("1900-01-01"))
                dtBegDate = this.schProductRouteRes.dActResBegDate;
            else if (this.schProductRouteRes.schProductRoute.dActBegDate != null && this.schProductRouteRes.schProductRoute.dActBegDate > DateTime.Parse("1900-01-01"))
                dtBegDate = this.schProductRouteRes.dActResBegDate;

            int iDays = SchData.GetDateDiff(this.schProductRouteRes.resource,"d",dtBegDate, dtCurDate );

            //iDays = iDays + 1;
            //处理非工作日



            if (iDays <= 1)
            {
                iDayDis = this.iDayDis1;
            }
            else if (iDays == 2)
            {
                iDayDis = this.iDayDis2;
            }
            else if (iDays == 3)
            {
                iDayDis = this.iDayDis3;
            }
            else if (iDays == 4)
            {
                iDayDis = this.iDayDis4;
            }
            else if (iDays == 5)
            {
                iDayDis = this.iDayDis5;
            }
            else if (iDays == 6)
            {
                iDayDis = this.iDayDis6;
            }
            else if (iDays == 7)
            {
                iDayDis = this.iDayDis7;
            }
            else if (iDays == 8)
            {
                iDayDis = this.iDayDis8;
            }
            else if (iDays == 9)
            {
                iDayDis = this.iDayDis9;
            }
            else if (iDays == 10)
            {
                iDayDis = this.iDayDis10;
            }
            else if (iDays == 11)
            {
                iDayDis = this.iDayDis11;
            }
            else if (iDays == 12)
            {
                iDayDis = this.iDayDis12;
            }
            else if (iDays == 13)
            {
                iDayDis = this.iDayDis13;
            }
            else if (iDays == 14)
            {
                iDayDis = this.iDayDis14;
            }
            else if (iDays == 15)
            {
                iDayDis = this.iDayDis15;
            }
            else if (iDays == 16)
            {
                iDayDis = this.iDayDis16;
            }
            else if (iDays == 17)
            {
                iDayDis = this.iDayDis17;
            }
            else if (iDays == 18)
            {
                iDayDis = this.iDayDis18;
            }
            else if (iDays == 19)
            {
                iDayDis = this.iDayDis19;
            }
            else if (iDays == 20)
            {
                iDayDis = this.iDayDis20;
            }
            else if (iDays == 21)
            {
                iDayDis = this.iDayDis21;
            }
            else if (iDays == 22)
            {
                iDayDis = this.iDayDis22;
            }
            else if (iDays == 23)
            {
                iDayDis = this.iDayDis23;
            }
            else if (iDays == 24)
            {
                iDayDis = this.iDayDis24;
            }
            else if (iDays == 25)
            {
                iDayDis = this.iDayDis25;
            }
            else if (iDays == 26)
            {
                iDayDis = this.iDayDis26;
            }
            else if (iDays == 27)
            {
                iDayDis = this.iDayDis27;
            }
            else if (iDays == 28)
            {
                iDayDis = this.iDayDis28;
            }
            else if (iDays == 29)
            {
                iDayDis = this.iDayDis29;
            }
            else if (iDays == 30)
            {
                iDayDis = this.iDayDis30;
            }
            else 
            {
                iDayDis = this.iDayDis31;
            }

            

            return iDayDis;
        }

        //获取10天内，相同物料的学习天数
        public Int32 GetWorkItemDays(SchProductRouteRes schProductRouteRes,DateTime dtBegDate)
        {

            Int32 liReturn = 0;

            //if (schProductRouteRes.resource == null || schProductRouteRes.resource.cResourceNo == "") return 0;


            ////DataRow[] drDaySelect;
            ////drDaySelect = schProductRouteRes.resource.schData.dt_ResourceTime.Select("cResourceNo = '" + schProductRouteRes.resource.cResourceNo + "' and dPeriodBegTime >= '" + dtBegDate.AddDays(-10).ToString() + "' and dPeriodBegTime <= '" + dtBegDate.ToString() + "' ");

            ////drDaySelect = drDaySelect.OrderBy(x => x["dPeriodDay"]).ToArray();
            ////找出所有 可用时间大于0的时间段 , 注意必须时段结束时间必须大于adCanBgDate,4小时内的任务，才认为是相邻任务
            ////List<TaskTimeRange> ListReturn = this.TaskTimeRangeList.FindAll(delegate(TaskTimeRange p) { return p.DEndTime <= adStartDate && p.cTaskType == 1; }); // p.AvailableTime > 0 
            //List<TaskTimeRange> ListReturn = schProductRouteRes.resource.GetTaskTimeRangeList(false).FindAll(delegate(TaskTimeRange p) { return p.DEndTime <= schProductRouteRes.dResLeanBegDate && p.DEndTime.AddDays(10) > schProductRouteRes.dResLeanBegDate && p.cTaskType == 1; }); // p.AvailableTime > 0 
            //TaskTimeRange TaskTimeRangePre = null;   //前一个生产任务,也可能没有任务，也要计算换产时间

            ////if (ListReturn.Count > 0)
            ////{
            ////    ////必须要排序,取最近一个任务
            ////    //ListReturn.Sort(delegate(TaskTimeRange p1, TaskTimeRange p2) { return Comparer<DateTime>.Default.Compare( p2.DBegTime,p1.DBegTime); });

            ////    TaskTimeRangePre = ListReturn[0];

            ////    //int iDays = SchData.GetDateDiff(this.schProductRouteRes.resource, "d", dtBegDate, dtCurDate);

            ////}


            //DateTime dtDayTemp;


            ////返回天            
            //if (ListReturn.Count > 0)
            //{
            //    dtDayTemp = (DateTime)ListReturn[0].DBegTime;
            //    liReturn = 1;

            //    //foreach (DataRow item in drDaySelect)

            //    for (int i = 0; i < drDaySelect.Length; i++)
            //    {
            //        if ((DateTime)drDaySelect[i]["dPeriodDay"] == dtDayTemp)
            //            continue;
            //        else
            //        {
            //            liReturn++;
            //            dtDayTemp = (DateTime)drDaySelect[i]["dPeriodDay"];
            //        }

            //    }

            //}



            return liReturn;
        }


        #region //属性函数封装

        
        public int iInterID;                          //学习曲线类型ID
        public string cLearnCurvesNo;                 //学习曲线类型
        public string cLearnCurvesName;               //学习曲线名称
        public string cTechNo;                        //工艺编号
        public double iDayDis1;                          //学习曲线第1天标准产能折扣
        public double iDayDis2;                          //学习曲线第1天标准产能折扣
        public double iDayDis3;                          //学习曲线第1天标准产能折扣
        public double iDayDis4;                          //学习曲线第1天标准产能折扣
        public double iDayDis5;                          //学习曲线第1天标准产能折扣
        public double iDayDis6;                          //学习曲线第1天标准产能折扣
        public double iDayDis7;                          //学习曲线第1天标准产能折扣
        public double iDayDis8;                          //学习曲线第1天标准产能折扣
        public double iDayDis9;                          //学习曲线第1天标准产能折扣
        public double iDayDis10;                          //学习曲线第1天标准产能折扣
        public double iDayDis11;                          //学习曲线第1天标准产能折扣
        public double iDayDis12;                          //学习曲线第1天标准产能折扣
        public double iDayDis13;                          //学习曲线第1天标准产能折扣
        public double iDayDis14;                          //学习曲线第1天标准产能折扣
        public double iDayDis15;                          //学习曲线第1天标准产能折扣
        public double iDayDis16;                          //学习曲线第1天标准产能折扣
        public double iDayDis17;                          //学习曲线第1天标准产能折扣
        public double iDayDis18;                          //学习曲线第1天标准产能折扣
        public double iDayDis19;                          //学习曲线第1天标准产能折扣
        public double iDayDis20;                          //学习曲线第1天标准产能折扣
        public double iDayDis21;                          //学习曲线第1天标准产能折扣
        public double iDayDis22;                          //学习曲线第1天标准产能折扣
        public double iDayDis23;                          //学习曲线第1天标准产能折扣
        public double iDayDis24;                          //学习曲线第1天标准产能折扣
        public double iDayDis25;                          //学习曲线第1天标准产能折扣
        public double iDayDis26;                          //学习曲线第1天标准产能折扣
        public double iDayDis27;                          //学习曲线第1天标准产能折扣
        public double iDayDis28;                          //学习曲线第1天标准产能折扣
        public double iDayDis29;                          //学习曲线第1天标准产能折扣
        public double iDayDis30;                          //学习曲线第1天标准产能折扣
        public double iDayDis31;                          //学习曲线第1天标准产能折扣
        public double iDiffCoe;                        //加工难度系数
	    public double iCapacity;                       //单件工时（暂不用）
	    public double iResPreTime;                     //资源前准备时间（暂不用）

        public double iResCapacity;                    //资源的单件工时
        public DateTime dtBegDate;                     //学习曲线开始日期
        public DateTime dtCurDate;                     //学习曲线排产日期
        public int      iDays;                         //学习曲线第几天
        public double   iDayCapacity;                  //学习曲线第几天
        public double   iDayHourDis;                   //学习曲线当前天数折扣

        public string cDefine1;
        public string cDefine2;
        public string cDefine3;
        public string cDefine4;
        public string cDefine5;

        #endregion
    }
}
