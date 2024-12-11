"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SchData = void 0;
const type_1 = require("./type");
class SchData {
    constructor(config) {
        this.cVersionNo = '';
        this.cCalculateNo = ''; // 排程运算号 用户名+ 时间
        this.iCurRows = 0; // 排程当前任务数，用于统计当前进度 ,按工序行号为准
        this.iTotalRows = 100; // 排程总任务数
        this.iProgress = 0; // 进度百分比
        this.cSchCapType = '0'; // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能
        this.ResourceList = new Array(10);
        this.KeyResourceList = new Array(10);
        this.TeamResourceList = new Array(10);
        this.SchProductList = new Array(10);
        this.SchProductWorkItemList = new Array(10);
        this.SchProductRouteList = new Array(10);
        this.SchProductRouteItemList = new Array(10);
        this.SchProductRouteResList = new Array(10);
        this.TaskTimeRangeList = new Array(10);
        this.WorkCenterList = new Array(10);
        this.ItemList = new Array(10);
        this.TechInfoList = new Array(10);
        // this.dtSchProduct.datasource = config.datasource;
        // this.dtSchProductWorkItem.datasource = config.datasource;
        // this.dtResource.datasource = config.datasource;
        // this.dt_ResourceTime.datasource = config.datasource;
        // this.dtSchProductRoute.datasource = config.datasource;
        // this.dtSchProductRouteRes.datasource = config.datasource;
        // this.dtSchProductRouteResTime.datasource = config.datasource; //
        // this.dtProChaType.datasource = config.datasource;
        // this.dtSchProductRouteTemp.datasource = config.datasource;
        // this.dtResTechScheduSN.datasource = config.datasource;
        // this.dtTechInfo.datasource = config.datasource; //
    }
    static GetDateDiff(DatePart, dt1, dt2, aa) {
        if (typeof DatePart == 'string') {
            let liReturn = 0;
            let ts = dt2.getTime() - dt1.getTime();
            if (DatePart === 's') {
                liReturn = Math.floor(ts / 1000);
            }
            else if (DatePart === 'm') {
                liReturn = Math.floor(ts / (1000 * 60));
            }
            else if (DatePart === 'h') {
                liReturn = Math.floor(ts / (1000 * 60 * 60));
            }
            else if (DatePart === 'd') {
                liReturn = Math.floor(ts / (1000 * 60 * 60 * 24));
            }
            return liReturn;
        }
        else if (typeof DatePart == 'object') {
            let resource = DatePart;
            //@ts-ignore
            DatePart = dt1;
            //@ts-ignore
            dt1 = dt2;
            //@ts-ignore
            dt2 = aa;
            //@ts-ignore
            let liReturn = 0;
            if (!resource || resource.cResourceNo === '')
                return 1;
            let drDaySelect = resource.schData.dt_ResourceTime.Select(`cResourceNo = '${resource.cResourceNo}' and dPeriodBegTime >= '${dt1.toISOString()}' and dPeriodBegTime <= '${dt2.toISOString()}'`);
            drDaySelect = drDaySelect.sort((a, b) => a['dPeriodDay'] - b['dPeriodDay']);
            let dtDayTemp;
            if (DatePart === 'd') {
                if (drDaySelect.length > 0) {
                    dtDayTemp = drDaySelect[0]['dPeriodDay'];
                    liReturn = 1;
                    for (let i = 0; i < drDaySelect.length; i++) {
                        if (drDaySelect[i]['dPeriodDay'] === dtDayTemp)
                            continue;
                        else {
                            liReturn++;
                            dtDayTemp = drDaySelect[i]['dPeriodDay'];
                        }
                    }
                }
            }
            return liReturn;
        }
    }
    static GetDateDiffString(Date1, Date2, Interval) {
        const dblYearLen = 365; //年的长度，365天
        const dblMonthLen = 365 / 12; //每个月平均的天数
        const objT = Date2.getTime() - Date1.getTime();
        switch (Interval) {
            case 'y': //返回日期的年份间隔
                return Math.floor(objT / (dblYearLen * 24 * 60 * 60 * 1000)).toString();
            case 'M': //返回日期的月份间隔
                return Math.floor(objT / (dblMonthLen * 24 * 60 * 60 * 1000)).toString();
            case 'd': //返回日期的天数间隔
                return Math.floor(objT / (24 * 60 * 60 * 1000)).toString();
            case 'h': //返回日期的小时间隔
                return Math.floor(objT / (60 * 60 * 1000)).toString();
            case 'm': //返回日期的分钟间隔
                return Math.floor(objT / (60 * 1000)).toString();
            case 's': //返回日期的秒钟间隔
                return Math.floor(objT / 1000).toString();
            case 'ms': //返回时间的微秒间隔
                return objT.toString();
            case 'all':
                return objT.toString();
            default:
                break;
        }
        return '0';
    }
    static AddDate(DatePart, iAdd, dt1) {
        let dtNew = new type_1.DateTime(dt1);
        if (DatePart === 's') {
            dtNew.setSeconds(dtNew.getSeconds() + iAdd);
        }
        else if (DatePart === 'm') {
            dtNew.setMinutes(dtNew.getMinutes() + iAdd);
        }
        else if (DatePart === 'h') {
            dtNew.setHours(dtNew.getHours() + iAdd);
        }
        else if (DatePart === 'd') {
            dtNew.setDate(dtNew.getDate() + iAdd);
        }
        else if (DatePart === 'M') {
            dtNew.setMonth(dtNew.getMonth() + iAdd);
        }
        return dtNew;
    }
    GetObjectData(info, context) {
        ;
        this.SchProductRouteList.GetObjectData(info, context);
        this.SchProductRouteResList.GetObjectData(info, context);
        this.TaskTimeRangeList.GetObjectData(info, context);
    }
    getDateDiff(r, d, beg, end) { }
    GetDateDiff(...args) {
        // let a=[...args]
        //@ts-ignore
        return this.getDateDiff(...args);
    }
}
exports.SchData = SchData;
