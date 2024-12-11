"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Base = void 0;
const type_1 = require("./type");
class Base {
    constructor() {
        this.TimeRangeAttribute = {
            Work: null,
            Overtime: null,
            MayOvertime: null,
            Maintain: null,
            Snag: null,
        };
    } //
    //   async ExecuteNonQuery(str: string, param: any) {
    //     let t = new DataTable() //
    //     let d = await t.fromSQLAsync(str, param)
    //     return d //
    //   }
    async ExecuteNonQuery(str, p) {
        let d = this.datasource;
        await d.query(str);
    }
    getParamValue(key) {
        //SchParam Select cValue FROM t_ParamValue where cParamNo  = 'cSchWo'
        // return ''
        let SchParamValue = this.SchParam.GetParam(key); //
        return SchParamValue;
    }
    //   getTableData(): any {
    //     let D = new DataTable()
    //     return D
    //   }
    GetDataTable(sql, config) {
        let D = new type_1.DataTable();
        D.executeQuery(sql);
        return D;
    }
}
exports.Base = Base;
