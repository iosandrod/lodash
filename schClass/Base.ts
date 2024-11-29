import { DataSource } from 'typeorm'
import {SchData} from './SchData'
import {_SchParam} from './SchParam'
export class Base {
  schData: SchData | null = null
  schParam:_SchParam
  _getDataSource(): DataSource {
    return null as any
  }
}
