using System;
using System.Data;
using System.Runtime.Serialization;
namespace Algorithm
{
    [Serializable]
    public class ResChaValue : IComparable, ICloneable,ISerializable
    {
        public SchData schData = null;        //所有排程数据
        public string cResourceNo = "";       //对应资源编号,要设置
        private SchProductRouteRes schProductRouteRes;    //工艺特征对应的资源任务
        public int iPosition = 0;                        //对应的工艺特征位置，资源任务的第几个工艺特征
        public int iUsedTime = 0;             //对应当前任务，当前刀具一个换刀周期累计使用时间。
        public ResChaValue()
        {
        }
        public ResChaValue(string iResChaValueID, SchProductRouteRes schProductRouteRes, int iPosition)
        {
            this.schProductRouteRes = schProductRouteRes;
            this.iPosition = iPosition;
            this.cResourceNo = schProductRouteRes.cResourceNo;
            this.schData = schProductRouteRes.schData;
            if (iResChaValueID == "") return;
            GetResChaValue(iResChaValueID);
        }
        public ResChaValue(string iResChaValueID)
        {
            GetResChaValue(iResChaValueID);
        }
        public void GetResChaValue(string iResChaValueID)
        {
            DataRow[] dr = schData.dtResChaValue.Select("FResChaValueNo = '" + iResChaValueID.ToString() + "'");
            if (dr.Length < 1) return;
            GetResChaValue(dr[0]);
        }
        public void GetResChaValue(DataRow drResource)
        {
            try
            {
                FResChaValueID = (int)drResource["FResChaValueID"];                  //工艺特征内码
                FResChaValueNo = drResource["FResChaValueNo"].ToString();                //工艺特征编号
                FResChaValueName = drResource["FResChaValueName"].ToString();             //工艺特征名称
                FProChaTypeID = drResource["FProChaTypeID"].ToString();                //特征类别
                FResChaCycleValue = (int)drResource["FResChaCycleValue"] * 60;             //换产周期     分钟，统一转换成秒
                FResChaRePlaceTime = (int)drResource["FResChaRePlaceTime"];           //平均更换耗时 秒
                FResChaMemo = drResource["FResChaMemo"].ToString();                   //备注  
                FSchSN = drResource["FSchSN"]== DBNull.Value? 0:(int)drResource["FSchSN"];                                   //按工艺特征，最优排产顺序
                FUseFixedPlaceTime = drResource["FUseFixedPlaceTime"].ToString();     //是否固定更换耗时
                FUseChaCycleValue = drResource["FUseChaCycleValue"].ToString();       //是否定期更换
                cDefine1 = drResource["cDefine1"].ToString();
                cDefine2 = drResource["cDefine2"].ToString();
                cDefine3 = drResource["cDefine3"].ToString();
                cDefine4 = drResource["cDefine4"].ToString();
                cDefine5 = drResource["cDefine5"].ToString();
                cDefine6 = drResource["cDefine6"].ToString();
                cDefine7 = drResource["cDefine7"].ToString();
                cDefine8 = drResource["cDefine8"].ToString();
                cDefine9 = drResource["cDefine9"].ToString();
                cDefine10 = drResource["cDefine10"].ToString();
                cDefine11 = (double)drResource["cDefine11"];
                cDefine12 = (double)drResource["cDefine12"];
                cDefine13 = (double)drResource["cDefine13"];
                cDefine14 = (double)drResource["cDefine14"];
                cDefine15 = (DateTime)drResource["cDefine15"];
                cDefine16 = (DateTime)drResource["cDefine16"];
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }
        public int GetChaValueChangeTime(SchProductRouteRes as_SchProductRouteRes, ResChaValue as_ResChaValuePre, int ai_workTime, ref int ai_cyctime, Boolean bSchdule,SchProductRouteRes as_SchProductRouteResPre)
        {
            int iChaValue = 0;
            try
            {
                if (this.FResChaValueNo == null || this.FResChaValueNo == "")  //如果没有工艺特征，返回0
                {
                    return (int)GetFResChaValue1Cyc(as_SchProductRouteRes);
                }
                else   //当前有工艺特征
                {
                    if (this.FUseFixedPlaceTime == "1")   //固定更换耗时，优先取物料资源工艺特征的转换时间
                    {
                        if (as_ResChaValuePre != null && as_ResChaValuePre.FResChaValueNo == this.FResChaValueNo)
                        {
                            iChaValue = 0;
                        }
                        else if (as_ResChaValuePre != null && as_ResChaValuePre.FResChaValueNo != this.FResChaValueNo)
                        {
                            iChaValue = (int)GetFResChaValue1Cyc(as_SchProductRouteRes);
                            if (iChaValue == 0)                   //如果为0，则取工艺特征档案中的转换时间
                                iChaValue = this.FResChaRePlaceTime;// *60;
                        }
                        else if (as_ResChaValuePre == null || (as_SchProductRouteResPre != null && as_SchProductRouteRes.schProductRoute.schProduct.cMiNo != as_SchProductRouteResPre.schProductRoute.schProduct.cMiNo))
                        {
                            iChaValue = (int)GetFResChaValue1Cyc(as_SchProductRouteRes);
                            if (iChaValue == 0)                   //如果为0，则取工艺特征档案中的转换时间
                                iChaValue = this.FResChaRePlaceTime;// *60;
                        }
                        else
                        {
                            iChaValue = (int)GetFResChaValue1Cyc(as_SchProductRouteRes);
                            if (iChaValue == 0)                   //如果为0，则取工艺特征档案中的转换时间
                                iChaValue = this.FResChaRePlaceTime;// *60;
                        }
                    }
                    else   //使用二维转换定义工艺特征转换时间 2020-08-23 Jonas Cheng 
                    {
                        if (as_ResChaValuePre != null)
                        {
                            if (as_ResChaValuePre.FResChaValueNo != this.FResChaValueNo)
                                iChaValue = GetTwoChaValueChangeTime(as_ResChaValuePre.FResChaValueNo);
                            else
                                iChaValue = 0;  //相同为0 
                        }
                        else
                            iChaValue = this.FResChaRePlaceTime;     //前面没有任务，取平均更换时间(秒)   * 60
                    }
                }
                if (this.FUseChaCycleValue == "1" && this.FResChaCycleValue > 0)
                {
                    if (as_ResChaValuePre != null && as_ResChaValuePre.FResChaValueNo == this.FResChaValueNo)  //没有换刀,包含前工艺特征的时间
                    {
                        int iTime = (int)(ai_workTime + as_ResChaValuePre.iUsedTime) / (this.FResChaCycleValue);  //计算需更换次数
                        ai_cyctime = iTime * this.FResChaRePlaceTime;            //计算换刀时间
                        if (bSchdule)  //正式排产
                            this.iUsedTime = as_ResChaValuePre.iUsedTime + ai_workTime - iTime * this.FResChaCycleValue;
                    }
                    else  //换刀了,只计算当前刀具的累计未换刀时间
                    {
                        int iTime = (int)ai_workTime / (this.FResChaCycleValue);        //计算需更换次数
                        ai_cyctime = iTime * this.FResChaRePlaceTime;            //计算换刀时间
                        if (bSchdule) //正式排产
                            this.iUsedTime = ai_workTime - iTime * this.FResChaCycleValue;  //累计剩余时间
                    }
                }
            }
            catch (Exception exp)
            {
                throw new Exception("出错位置:ResChaValue.GetChaValueChangeTime 取工艺特征转换时间出错！"); //ResTimeRangeList1[ResTimeRangeList1.Count - 1].DEndTime.ToLongTimeString() +
                return -1;
            }
            return iChaValue;
        }
        private double GetFResChaValue1Cyc(SchProductRouteRes as_SchProductRouteRes)
        {
            switch (this.iPosition)
            {
                case 1:                 //工艺特征1转换时间
                    return as_SchProductRouteRes.FResChaValue1Cyc;
                case 2:                 //工艺特征2转换时间
                    return as_SchProductRouteRes.FResChaValue2Cyc;
                case 3:                 //工艺特征3转换时间
                    return as_SchProductRouteRes.FResChaValue3Cyc;
                case 4:                 //工艺特征4转换时间
                    return as_SchProductRouteRes.FResChaValue4Cyc;
                case 5:                 //工艺特征5转换时间
                    return as_SchProductRouteRes.FResChaValue5Cyc;
                case 6:                 //工艺特征6转换时间
                    return as_SchProductRouteRes.FResChaValue6Cyc;
                default:
                    return as_SchProductRouteRes.FResChaValue1Cyc;
            }
        }
        private int GetTwoChaValueChangeTime(string iResChaValue1ID)  //iResChaValue1No
        {
            if (iResChaValue1ID == null) return 0;
            int iChaValue = 0;
            DataRow[] dr = this.schData.dtResChaCrossTime.Select("FProChaTypeID = '" + this.FProChaTypeID + "' and FResChaValue1ID = '" + iResChaValue1ID.ToString() + "' and FResChaValue1ID = '" + this.FResChaValueNo + "'");
            if (dr != null && dr.Length > 0)
            {
                iChaValue = int.Parse(dr[0]["FResChaExcTime"].ToString());//* 60;   //分钟
            }
            if (iChaValue == 0)
            {
                iChaValue = this.FResChaRePlaceTime;
            }
            return iChaValue;
        }
        #region //属性函数封装
        public int FResChaValueID;               //工艺特征内码
        public string FResChaValueNo;               //工艺特征编号
        public string FResChaValueName;             //工艺特征名称
        public string FProChaTypeID;                //特征类别
        public int FResChaCycleValue;                 //换产周期 分   
        public int FResChaRePlaceTime;                //每次换产时间 分
        public string FResChaMemo;                   //备注  
        public int FSchSN;                        //按工艺特征，最优排产顺序
        public string FUseFixedPlaceTime;            //是否固定更换耗时   
        public string FUseChaCycleValue;             //是否定期更换
        public string cDefine1;
        public string cDefine2;
        public string cDefine3;
        public string cDefine4;
        public string cDefine5;
        public string cDefine6;
        public string cDefine7;
        public string cDefine8;
        public string cDefine9;
        public string cDefine10;
        public double cDefine11;
        public double cDefine12;
        public double cDefine13;
        public double cDefine14;
        public DateTime cDefine15;
        public DateTime cDefine16;
        #endregion
        public int CompareTo(object obj)
        {
            if (obj is ResChaValue)
            {
                ResChaValue newTimeRange = (ResChaValue)obj;
                return 1;
            }
            else
            {
                throw new ArgumentException("对象非TimeRange类型");
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