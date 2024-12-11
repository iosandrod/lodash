"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ScheduleResource = void 0;
const type_1 = require("./type");
class ScheduleResource {
    constructor(id) {
        this.resTimeStyle = 0;
        this.resTimeGroup = 4;
        this.isHasViceRes = false;
        this.viceRes = null;
        this.capacityList = new type_1.ArrayList(5);
        this.holdingTimeList = new type_1.ArrayList(10);
        this.idleAndWorkTimeList = new type_1.ArrayList(10);
        this.taskList = new type_1.ArrayList(10);
        this.tempHoldingTimeList = new type_1.ArrayList(5);
        this.tempIdleTimeList = new type_1.ArrayList(5);
        this.tempIdleDetailTimeList = new type_1.ArrayList(5);
        if (id !== undefined) {
            this.id = id;
        }
    }
    get ID() {
        return this.id;
    }
    get Type() {
        return this.type;
    }
    set Type(value) {
        this.type = value;
    }
    get Number() {
        return this.number;
    }
    set Number(value) {
        this.number = value;
    }
    get OccupyStyle() {
        return this.occupyStyle;
    }
    set OccupyStyle(value) {
        this.occupyStyle = value;
    }
    iniData() {
        // Implementation here
    }
    deepClone() {
        let temp = new ScheduleResource();
        // Deep clone implementation here
        return temp;
    }
    CompareIdleAndWorkTimeList(aarr_idleAndWorkTimeList) {
        return null;
    }
}
exports.ScheduleResource = ScheduleResource;
