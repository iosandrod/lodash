import { DataSource } from 'typeorm'
import { SchParam } from './SchParam'
import { Comparer, DataTable, DateTime, SchData } from './type'

export class Base {
  SchParam: SchParam
  schData: SchData
  datasource: DataSource
  Comparer: Comparer
  DateTime: DateTime
  TimeRangeAttribute = {
    Work: null,
    Overtime: null,
    MayOvertime: null,
    Maintain: null,
    Snag: null,
  }
  constructor() {} //
  //   async ExecuteNonQuery(str: string, param: any) {
  //     let t = new DataTable() //
  //     let d = await t.fromSQLAsync(str, param)
  //     return d //
  //   }
  async ExecuteNonQuery(str, p?: any) {
    let d = this.datasource
    await d.query(str)
  }
  getParamValue(key: string): any {
    //SchParam Select cValue FROM t_ParamValue where cParamNo  = 'cSchWo'
    // return ''
    let SchParamValue = this.SchParam.GetParam(key) //
    return SchParamValue
  }

//   getTableData(): any {
//     let D = new DataTable()
//     return D
//   }
  GetDataTable(sql: string, config: any): any {
    let D = new DataTable()
	D.executeQuery(sql)
    return D
  }
}
