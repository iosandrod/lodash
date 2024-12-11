import { DataSource } from 'typeorm'

// export const SchData: any = {}
// export type SchData = any
// export const SchParam: any = {}
export * from './SchParam'
export * from './DataTable'
// export type Resource = any
export * from './Resource'
// export class SchProductRouteRes {
//   [key: string]: any
// }
export * from './SchData'
export * from './SchParam' //
export * from './SchProductRouteRes'
export * from './Comparer'
export * from './DataTable'
// export type TaskTimeRange=
// export class TaskTimeRange {
//   [key: string]: any
// }
export * from './TaskTimeRange'
// export interface IComparable<T> {}
// export interface ICloneable {}
// export const Comparer: any = {}
// export type DateTime = any
// export const DateTime: any = {}
// export const TimeRangeAttribute: any = {}
// export class ResTimeRange {
// 	[key: string]: any;
// }
export * from './Base'
export * from './ResTimeRange'
// export type SerializationInfo = any
// export type StreamingContext = any

// export class SchProduct {
//   [key: string]: any
// }
export * from './SchProduct'
// export class SchProductWorkItem {
//   [key: string]: any
// }
export * from './SchProductWorkItem'
// export class SchProductRoute {
//   [key: string]: any
// }
export * from './SchProductRoute'
// export class WorkCenter {
// 	[key: string]: any;
// }
export * from './WorkCenter'
// export class Item {
// 	[key: string]: any;
// }
export * from './Item'
// export class TechInfo {
// 	[key: string]: any;
// }
export * from './TechInfo'
// export class SchProductRouteItem {
// 	[key: string]: any;
// }
export * from './DateTime'
export * from './SchProductRouteItem'
export class DataRow {
  [key: string]: any
}
// export * from './DataRow'
// export class ScheduleResource {
// 	[key: string]: any;
// }
export * from './ScheduleResource'
export* from './ResSourceDayCap'
export class ArrayList<T> extends Array {
  [key: string]: any
}
// export * from './ArrayList'
// export class Scheduling {
// 	static maxTimeValue: any;
// 	[key: string]: any;
// }
export * from './Scheduling'
// export class ResSourceDayCap {
// 	[key: string]: any;
// }
export * from './ResSourceDayCap'
// export class TechLearnCurves {
// 	[key: string]: any;
// }
export * from './TechLearnCurves'
// export class ResChaValue {
// 	[key: string]: any;
// }
export * from './ResChaValue' //
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
