import { Base } from './Base'
import { gsDecoration } from './Util'
export class DataTable<T = any> extends Base {
  @gsDecoration({
    get: function () {
      return this.rows
    },
    set: async function (sql: string) {
      let _this: DataTable = this
      let ds = _this._getDataSource()
      let data = await ds?.query(sql)
      _this.rows = data
      return
    },
  })
  rows: T[] = []
  getRows!: () => T[]
  setRows!: (sql: string) => Promise<void>
  constructor() {
    super() //
  }
}
