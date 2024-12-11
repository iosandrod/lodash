import { Base } from './Base'

export class DataTable extends Base {
  forEach(arg0: (dr: any) => void) {
    // throw new Error('Method not implemented.');
    this.rows.forEach(arg0)
  }
  columns: { name: string; type: string }[] = []
  rows: any[] = []
  constructor() {
    super()
    // this.datasource = config.datasourcce; //
    //this.dlShowProcess = this.ShowProcess.bind(this);
  }
  DefaultView: any = {
    Sort: '',
  }
  /**
   * Adds a column to the DataTable.
   * @param name The name of the column.
   * @param type The type of the column (e.g., 'string', 'number').
   */
  addColumn(name: string, type: string): void {
    if (this.columns.some((col) => col.name === name)) {
      // throw new Error(`Column with name '${name}' already exists.`);
    }
    this.columns.push({ name, type })
  } //
  // async query(q:string) {
  // 	// let
  // 	//
  // }
  /**
   * Adds a row to the DataTable.
   * @param row An object representing a row, with keys matching column names.
   */
  addRow(row: Record<string, any>): void {
    const newRow: Record<string, any> = {}

    this.columns.forEach(({ name, type }) => {
      // if (!(name in row)) {
      // 	throw new Error(`Missing value for column '${name}' in row.`);
      // }

      const value = row[name]

      // if (type === 'string' && typeof value !== 'string') {
      // 	throw new Error(`Value for column '${name}' must be a string.`);
      // } else if (type === 'number' && typeof value !== 'number') {
      // 	throw new Error(`Value for column '${name}' must be a number.`);
      // }

      newRow[name] = value
    })

    this.rows.push(newRow)
  }

  /**
   * Retrieves all rows in the DataTable.
   * @returns An array of rows.
   */
  getRows(): Record<string, any>[] {
    return this.rows
  }

  /**
   * Retrieves all columns in the DataTable.
   * @returns An array of columns.
   */
  getColumns(): { name: string; type: string }[] {
    return this.columns
  } //
  /**
   * Filters rows based on a condition.
   * @param predicate A function that takes a row and returns true to include it.
   * @returns An array of rows that satisfy the condition.
   */
  filterRows(
    predicate: (row: Record<string, any>) => boolean,
  ): Record<string, any>[] {
    return this.rows.filter(predicate)
  }

  /**
   * Clears all rows from the DataTable.
   */
  clearRows(): void {
    this.rows = []
  }
  Select(condition: string): any[] {
    // const conditionFunction = new Function('row', `return ${condition};`);
    // return this.rows.filter((row) => {
    // 	try {
    // 		return conditionFunction(row);
    // 	} catch {
    // 		throw new Error(`Invalid condition: ${condition}`);
    // 	}
    // });
    return []
  }
  async executeQuery(
    query: string,
  ): Promise<{
    columns: { name: string; type: string }[]
    rows: Record<string, any>[]
  }> {
    let dataSource = this.datasource
    const rawData = await dataSource.query(query)

    if (rawData.length === 0) {
      return
      // return { columns: [], rows: [] };
    }
    const columns = Object.keys(rawData[0]).map((name) => ({
      name,
      type: typeof rawData[0][name], // Infers the type from the first row
    }))
    this.columns = columns
    this.rows = rawData
    // return { columns, rows: rawData };
  }
  //   async fromSQLAsync(
  //     sql: string,
  //     executeQuery: (
  //       query: string,
  //     ) => Promise<{
  //       columns: { name: string; type: string }[]
  //       rows: Record<string, any>[]
  //     }>,
  //   ): Promise<DataTable> {
  //     const { columns, rows } = await executeQuery(sql)
  //     const dataTable = new DataTable()
  //     columns.forEach((col) => dataTable.addColumn(col.name, col.type))
  //     rows.forEach((row) => dataTable.addRow(row))
  //     return dataTable
  //   }
  filter(fn: any): any[] {
    let rows = this.rows
    return rows.filter(fn)
  }
}
