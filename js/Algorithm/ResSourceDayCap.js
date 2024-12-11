"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ResSourceDayCap = void 0;
const type_1 = require("./type");
class ResSourceDayCap extends type_1.Base {
    constructor(as_ResourceNo, adBegTime, adEndTime) {
        super();
        this.cResourceNo = ''; // 对应资源ＩＤ号,要设置
        this.cIsInfinityAbility = '0'; // 0 产能有限，1 产能无限
        this.resource = null; // 时段对应的资源 有值
        this.holdingTime = 0; // 时段总长, dDEndTimeTime - dBegTime,单位为秒
        this.allottedTime = 0; // 已分配时间,包括维修、故障时间
        this.maintainTime = 0; // 维修、故障时间
        this.availableTime = 0; // 时段内可用时间，计算出来
        this.WorkTimeAct = 0; // 学习曲线折扣,有效加工时间
        this.notWorkTime = 0; // 时段内空闲时间，计算出来,用于检查
        this.iPeriodID = 1; // 时段ID，排程完成写回数据库时，重新生成，唯一
        this.iTaskCount = 0; // 当日已排任务总数
        this.iTaskMaxCount = 0; // 当日可排任务总数
        this.ResTimeRangeList = new Array(10);
        this.taskTimeRangeList = new Array(10);
        this.WorkTimeRangeList = new Array(10);
        this.iSchSdID = -1; // 记录更新、新增时间段的任务ID
        this.iProcessProductID = -1;
        this.iResProcessID = -1;
        this.iSchSNMax = -1;
        if (as_ResourceNo) {
            this.cResourceNo = as_ResourceNo;
        }
        if (adBegTime && adEndTime) {
            this.dBegTime = adBegTime;
            this.dEndTime = adEndTime;
            this.AllottedTime = 0;
        }
    }
    get CResourceNo() {
        return this.cResourceNo;
    }
    set CResourceNo(value) {
        this.cResourceNo = value;
    }
    get CIsInfinityAbility() {
        return this.cIsInfinityAbility;
    }
    set CIsInfinityAbility(value) {
        this.cIsInfinityAbility = value;
    }
    get DBegTime() {
        return this.dBegTime;
    }
    set DBegTime(value) {
        if (value < new Date(2000, 0, 1)) {
            throw new Error(`资源编号${this.cResourceNo}时间段开始日期${value.toString()}不能小于2000-01-01日,开始时间${this.dBegTime.toLocaleDateString()},结束时间${this.dEndTime.toLocaleDateString()}`);
        }
        this.dBegTime = value;
        this.holdingTime = this.HoldingTime;
    }
    get DEndTime() {
        return this.dEndTime;
    }
    set DEndTime(value) {
        if (value < new Date(2000, 0, 1)) {
            throw new Error(`资源编号${this.cResourceNo}时间段结束日期${value.toString()}不能小于2000-01-01日,开始时间${this.dBegTime.toLocaleDateString()},结束时间${this.dEndTime.toLocaleDateString()}`);
        }
        if (value <= this.dBegTime) {
            throw new Error(`资源编号${this.cResourceNo}时间段结束日期${value.toString()}不能小于时段开始时间,开始时间${this.dBegTime.toLocaleDateString()},结束时间${this.dEndTime.toLocaleDateString()}`);
        }
        this.dEndTime = value;
        this.holdingTime = this.HoldingTime;
    }
    get HoldingTime() {
        if (this.dEndTime && this.dBegTime && this.dEndTime > this.dBegTime) {
            const its = this.dEndTime.getTime() - this.dBegTime.getTime();
            if (its > 0) {
                return Math.floor(its / 1000); // 如果没有设置，则时长缺省为结束时间与开始时间之差
            }
            else {
                throw new Error('the timerange no set');
            }
        }
        else {
            return 0;
        }
    }
    get AllottedTime() {
        if (this.constructor.name === 'TaskTimeRange') {
            return this.allottedTime;
        }
        else {
            let allottedTimeTemp = 0;
            if (this.CIsInfinityAbility !== '1' &&
                this.WorkTimeRangeList.length > 0) {
                // 有限产能，统计时段内已分配任务时间。
                for (const taskTimeRange of this.WorkTimeRangeList) {
                    allottedTimeTemp += taskTimeRange.allottedTime;
                }
                if (this.allottedTime < allottedTimeTemp) {
                    this.allottedTime = this.allottedTime;
                }
            }
            return allottedTimeTemp;
        }
    }
    set AllottedTime(value) {
        if (value >= 0) {
            this.allottedTime = value;
        }
        else {
            throw new Error('时间段已分配时间必须大于0');
        }
    }
    get AvailableTime() {
        if (this.CIsInfinityAbility === '1') {
            // 无限产能。
            return this.holdingTime;
        }
        else {
            // 有限产能，统计时段内已分配任务时间
            if (this.constructor.name === 'TaskTimeRange') {
                const its = this.dEndTime.getTime() - this.dBegTime.getTime();
                if (its > 0) {
                    return Math.floor(its / 1000); // 如果没有设置，则时长缺省为结束时间与开始时间之差
                }
                return this.holdingTime;
            }
            else if (this.holdingTime - this.AllottedTime >= 0) {
                return this.holdingTime - this.AllottedTime;
            }
            else if (this.holdingTime - this.AllottedTime <= 30) {
                // 计算有误差，小于1秒
                return 0;
            }
            else {
                throw new Error('出错位置：排程时取时段内可用时间出错TimeSchTask.AvailableTime！');
                return 0;
            }
        }
    }
    get NotWorkTime() {
        if (this.constructor.name === 'TaskTimeRange') {
            return this.AvailableTime; // 可用时间
        }
        else {
            // "Algorithm.ResTimeRange" 资源时段的占用时间，取所有已分配任务的已占用时间
            return this.AvailableTime; // 可用时间
        }
    }
    set NotWorkTime(value) {
        // 只有TaskTimeRange才可以设置AllottedTime
    }
    get MaintainTime() {
        this.maintainTime = 0;
        for (const taskTimeRange of this.taskTimeRangeList.filter((p1) => p1.cTaskType === 2)) {
            this.maintainTime += taskTimeRange.HoldingTime;
        }
        return this.maintainTime;
    }
    get Attribute() {
        return this.attribute;
    }
    set Attribute(value) {
        this.attribute = value;
    }
    get TaskTimeRangeList() {
        return this.taskTimeRangeList;
    }
    set TaskTimeRangeList(value) {
        if (this.cResourceNo === 'gys20097') {
            let i = 0;
        }
        this.taskTimeRangeList = value;
    }
    compareTo(obj) {
        if (obj instanceof type_1.ResTimeRange) {
            const newTimeRange = obj;
            if (this.dBegTime === newTimeRange.dBegTime &&
                this.dEndTime === newTimeRange.dEndTime) {
                return 1;
            }
            else {
                return -1;
            }
        }
        else {
            throw new Error('对象非TimeRange类型');
        }
    }
    clone() {
        return Object.assign(Object.create(Object.getPrototypeOf(this)), this);
    }
}
exports.ResSourceDayCap = ResSourceDayCap;
