"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __exportStar = (this && this.__exportStar) || function(m, exports) {
    for (var p in m) if (p !== "default" && !Object.prototype.hasOwnProperty.call(exports, p)) __createBinding(exports, m, p);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ArrayList = exports.DataRow = void 0;
// export const SchData: any = {}
// export type SchData = any
// export const SchParam: any = {}
__exportStar(require("./SchParam"), exports);
__exportStar(require("./DataTable"), exports);
// export type Resource = any
__exportStar(require("./Resource"), exports);
// export class SchProductRouteRes {
//   [key: string]: any
// }
__exportStar(require("./SchData"), exports);
__exportStar(require("./SchParam"), exports); //
__exportStar(require("./SchProductRouteRes"), exports);
__exportStar(require("./Comparer"), exports);
__exportStar(require("./DataTable"), exports);
// export type TaskTimeRange=
// export class TaskTimeRange {
//   [key: string]: any
// }
__exportStar(require("./TaskTimeRange"), exports);
// export interface IComparable<T> {}
// export interface ICloneable {}
// export const Comparer: any = {}
// export type DateTime = any
// export const DateTime: any = {}
// export const TimeRangeAttribute: any = {}
// export class ResTimeRange {
// 	[key: string]: any;
// }
__exportStar(require("./Base"), exports);
__exportStar(require("./ResTimeRange"), exports);
// export type SerializationInfo = any
// export type StreamingContext = any
// export class SchProduct {
//   [key: string]: any
// }
__exportStar(require("./SchProduct"), exports);
// export class SchProductWorkItem {
//   [key: string]: any
// }
__exportStar(require("./SchProductWorkItem"), exports);
// export class SchProductRoute {
//   [key: string]: any
// }
__exportStar(require("./SchProductRoute"), exports);
// export class WorkCenter {
// 	[key: string]: any;
// }
__exportStar(require("./WorkCenter"), exports);
// export class Item {
// 	[key: string]: any;
// }
__exportStar(require("./Item"), exports);
// export class TechInfo {
// 	[key: string]: any;
// }
__exportStar(require("./TechInfo"), exports);
// export class SchProductRouteItem {
// 	[key: string]: any;
// }
__exportStar(require("./DateTime"), exports);
__exportStar(require("./SchProductRouteItem"), exports);
class DataRow {
}
exports.DataRow = DataRow;
// export * from './DataRow'
// export class ScheduleResource {
// 	[key: string]: any;
// }
__exportStar(require("./ScheduleResource"), exports);
__exportStar(require("./ResSourceDayCap"), exports);
class ArrayList extends Array {
}
exports.ArrayList = ArrayList;
// export * from './ArrayList'
// export class Scheduling {
// 	static maxTimeValue: any;
// 	[key: string]: any;
// }
__exportStar(require("./Scheduling"), exports);
// export class ResSourceDayCap {
// 	[key: string]: any;
// }
__exportStar(require("./ResSourceDayCap"), exports);
// export class TechLearnCurves {
// 	[key: string]: any;
// }
__exportStar(require("./TechLearnCurves"), exports);
// export class ResChaValue {
// 	[key: string]: any;
// }
__exportStar(require("./ResChaValue"), exports); //
// const executeQueryTypeORM = async (dataSource: DataSource, query: string): Promise<{ columns: { name: string; type: string }[]; rows: Record<string, any>[] }> => {
//   const rawData = await dataSource.query(query);
//   if (rawData.length === 0) {
//       return { columns: [], rows: [] };
//   }
//   const columns = Object.keys(rawData[0]).map(name => ({
//       name,
//       type: typeof rawData[0][name] // Infers the type from the first row
//   }));
//   return { columns, rows: rawData };
// };
// async function fromSQLAsync(
// 	sql: string,
// 	executeQuery: (query: string) => Promise<{ columns: { name: string; type: string }[]; rows: Record<string, any>[] }>,
// ): Promise<DataTable> {
// 	const { columns, rows } = await executeQuery(sql);
// 	const dataTable = new DataTable();
// 	columns.forEach((col) => dataTable.addColumn(col.name, col.type));
// 	rows.forEach((row) => dataTable.addRow(row));
// 	return dataTable;
// }
// export class Sq
// export const SqlPro = {
// 	GetDataTable: async function (this: DataSource, str: string) {
// 		let d =await this.query(str);
// 		// return new DataTable();
// 	},
// };
// export class _SqlPro {
//   ExecuteNonQuery() {}
// }
// export const SqlPro = new _SqlPro();
// export const GetDataTable = async function (str: string, param: any) {
//   //
// 	let t = new DataTable();
// 	let d = await t.fromSQLAsync(str, param);
// 	return d; //
// };
// export const ExecuteNonQuery=async function (str: string, param: any) {
//   let t = new DataTable();
//   let d = await t.fromSQLAsync(str, param);
//   return d; //
// };
