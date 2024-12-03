using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace Algorithm
{
    public class ScheduleResource
    {
        public long ID
        {
            get
            {
                return id;
            }
        }
        private long id;
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }
        public int isKey;
        private int number;
        public int OccupyStyle
        {
            get
            {
                return occupyStyle;
            }
            set
            {
                occupyStyle = value;
            }
        }
        public ArrayList capacityList = new ArrayList(5);
        private int occupyStyle;
        private int type;
        public string name;
        public int resTimeStyle = 0;
        public int resTimeGroup = 4;
        public bool isHasViceRes = false;
        public ScheduleResource viceRes = null;
        public ScheduleResource()
        {
        }
        public ScheduleResource(long id)
        {
            this.id = id;
        }
        public void iniData()
        {
        }
        public ScheduleResource deepClone()
        {
            ScheduleResource temp = new ScheduleResource();//临时加工单元实体
            return temp;
        }
        public ArrayList CompareIdleAndWorkTimeList(ArrayList aarr_idleAndWorkTimeList)
        {
            return null;
        }
        public ArrayList holdingTimeList = new ArrayList(10);
        public ArrayList idleAndWorkTimeList = new ArrayList(10);
        public ArrayList taskList = new ArrayList(10);
        public ArrayList tempHoldingTimeList = new ArrayList(5);
        public ArrayList tempIdleTimeList = new ArrayList(5);
        public ArrayList tempIdleDetailTimeList = new ArrayList(5);
    }
}