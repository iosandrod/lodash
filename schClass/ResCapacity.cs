using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Algorithm
{
    public class ResCapacity
    {
        public int FResourceID;     // 资源ＩＤ

        public int FProductID;// 产品ＩＤ

        public int FProcessID;// 产品工艺模型中的工序号

        public float FEfficiency;   //效率

        public float FCapacity1;   //产能 10 秒/PCS 

        public float FCapacity2;

        public float FPreSetoutTime;  //前置准备时间

        public float FPostSetoutTime; //后置准备时间

        public float FProcessPassRate;  //合格率

        public ArrayList viceResIDList; //存储副资源引用

        public int FPriority;           //主资源优先级

        public string FTechCharacter1 = "";    //产品当前工艺 工艺特征1
        public string FTechCharacter2 = "";    //产品当前工艺 工艺特征2
        public string FTechCharacter3 = "";    //产品当前工艺 工艺特征3
        public string FTechCharacter4 = "";    //产品当前工艺 工艺特征4
        public string FTechCharacter5 = "";    //产品当前工艺 工艺特征5
        public string FTechCharacter6 = "";    //产品当前工艺 工艺特征6


        public int FWorkType = 0;       //生产方式  0 单件生产 / 1 按批生产
        
        public int FBatchWorkTime = 0;       //每批生产时间

        public int FBatchQty = 0;            //每批生产数量

        public int FBatchInterTime = 0;       //每批间隔时间

        private int iPreTaskID = 0;        //前任务编号,用于取前置准备时间

        public int IPreTaskID
        {
            get { return iPreTaskID; }
            set { iPreTaskID = value; }
        }

        private int iPreTaskTime = 0;      //与前任务间隔时间,用于是否考虑前置准备时间

        public int IPreTaskTime
        {
            get { return iPreTaskTime; }
            set { iPreTaskTime = value; }
        }

        private int iTaskID = 0;         //任务编号

        public int ITaskID
        {
            get { return iTaskID; }
            set { iTaskID = value; }
        }

        private float planQty = 0;      //计划生产数量

        public float PlanQty
        {
            get { return planQty; }
            set { planQty = value; 

                  //计算加工时间
                  if ( FWorkType == 0 )    //单件生产
                  {
                      workTime = planQty * FCapacity1 ;   
                  }
                  else if ( FWorkType == 1 )  //按批生产
                  {
                     // workTime = planQty / ;
                  }
                  else
                  {                      
                      workTime =    1;
                  }
            }
        }

        private float workTime = 0;      //计划加工时间（不包括前、后准备时间）

        public float WorkTime
        {
            get { return workTime; }
            set { workTime = value; }
        }

       
    }


}
