import {
  Resource,
  TaskTimeRange,
  // IComparable,
  // ICloneable,
  ResSourceDayCap,
  SchProductRouteRes,
  SchParam,
  Comparer,
  DateTime,
  // TimeRangeAttribute,
  DataTable,
  DataRow,
  SchProduct,
  SchProductWorkItem,
  SchProductRoute,
  SchProductRouteItem,
  WorkCenter,
  Item,
  TechInfo,
} from './type'
export class SchData {
  cVersionNo: string = ''
  cCalculateNo: string = '' // 排程运算号 用户名+ 时间
  dtStart: DateTime // 开始排程时间
  dtEnd: DateTime // 排程截止时间
  dtToday: DateTime // 当前时间，取数据库
  iCurRows: number = 0 // 排程当前任务数，用于统计当前进度 ,按工序行号为准
  iTotalRows: number = 100 // 排程总任务数
  iProgress: number = 0 // 进度百分比
  cSchCapType: string = '0' // 排程产能方案  0 ---正常产能,1--加班产能,2--极限产能
  dtSchProduct: DataTable //= new DataTable(); //排产产品
  dtSchProductWorkItem: DataTable //= new DataTable(); //加工物料
  dtResource: DataTable //= new DataTable(); //所有资源，有值
  dt_ResourceTime: DataTable // = new DataTable(); //资源正常工作日历
  dt_ResourceSpecTime: DataTable // = new DataTable(); //资源特殊工作日历
  dtSchProductRoute: DataTable // = new DataTable();
  dtSchProductRouteRes: DataTable // = new DataTable();
  dtSchProductRouteItem: DataTable //= new DataTable();
  dtSchProductRouteResTime: DataTable //= new DataTable();
  dtSchResWorkTime: DataTable //= new DataTable(); //合并后资源日历时间段，排程完成后，写入t_SchResWorkTime
  dtSchProductTemp: DataTable //= new DataTable(); //排产结果写回
  dtSchProductRouteTemp: DataTable // = new DataTable();
  dtSchProductRouteResTemp: DataTable // = new DataTable();
  dtProChaType: DataTable //= new DataTable(); //工艺特征类型，有值
  dtResChaValue: DataTable //= new DataTable(); //工艺特征值，有值
  dtResChaCrossTime: DataTable // = new DataTable(); //工艺特征转换时间 ,有值
  dtWorkCenter: DataTable //= new DataTable(); //工作中心
  dtDepartment: DataTable //= new DataTable(); //部门
  dtTeam: DataTable //= new DataTable(); //班组
  dtPerson: DataTable //= new DataTable(); //员工
  dtItem: DataTable //= new DataTable(); // 物料档案  用于扩展
  dtTechInfo: DataTable //= new DataTable(); // 工艺档案 用于扩展
  dtTechLearnCurves: DataTable // = new DataTable(); //资源任务学习曲线       2017-11-23
  dtResTechScheduSN: DataTable //= new DataTable(); //资源工艺特征排产顺序
  ResourceList: Resource[] = new Array<Resource>(10)
  KeyResourceList: Resource[] = new Array<Resource>(10)
  TeamResourceList: Resource[] = new Array<Resource>(10)
  SchProductList: SchProduct[] = new Array<SchProduct>(10)
  SchProductWorkItemList: SchProductWorkItem[] = new Array<SchProductWorkItem>(
    10,
  )
  SchProductRouteList: SchProductRoute[] = new Array<SchProductRoute>(10)
  SchProductRouteItemList: SchProductRouteItem[] = new Array<
    SchProductRouteItem
  >(10)
  SchProductRouteResList: SchProductRouteRes[] = new Array<SchProductRouteRes>(
    10,
  )
  TaskTimeRangeList: TaskTimeRange[] = new Array<TaskTimeRange>(10)
  WorkCenterList: WorkCenter[] = new Array<WorkCenter>(10)
  ItemList: Item[] = new Array<Item>(10)
  TechInfoList: TechInfo[] = new Array<TechInfo>(10)
  constructor(config: any) {
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
  static GetDateDiff(DatePart: string, dt1: DateTime, dt2: DateTime, aa: any): any {
    if (typeof DatePart == 'string') {
      let liReturn: number = 0
      let ts: number = dt2.getTime() - dt1.getTime()
      if (DatePart === 's') {
        liReturn = Math.floor(ts / 1000)
      } else if (DatePart === 'm') {
        liReturn = Math.floor(ts / (1000 * 60))
      } else if (DatePart === 'h') {
        liReturn = Math.floor(ts / (1000 * 60 * 60))
      } else if (DatePart === 'd') {
        liReturn = Math.floor(ts / (1000 * 60 * 60 * 24))
      }
      return liReturn
    } else if (typeof DatePart == 'object') {
      let resource: Resource = DatePart
      //@ts-ignore
      DatePart = dt1
      //@ts-ignore
      dt1 = dt2
      //@ts-ignore
      dt2 = aa
      //@ts-ignore
      let liReturn: number = 0
      if (!resource || resource.cResourceNo === '') return 1
      let drDaySelect: DataRow[] = resource.schData.dt_ResourceTime.Select(
        `cResourceNo = '${
          resource.cResourceNo
        }' and dPeriodBegTime >= '${dt1.toISOString()}' and dPeriodBegTime <= '${dt2.toISOString()}'`,
      )
      drDaySelect = drDaySelect.sort(
        (a, b) => a['dPeriodDay'] - b['dPeriodDay'],
      )
      let dtDayTemp: DateTime
      if (DatePart === 'd') {
        if (drDaySelect.length > 0) {
          dtDayTemp = drDaySelect[0]['dPeriodDay']
          liReturn = 1
          for (let i = 0; i < drDaySelect.length; i++) {
            if (drDaySelect[i]['dPeriodDay'] === dtDayTemp) continue
            else {
              liReturn++
              dtDayTemp = drDaySelect[i]['dPeriodDay']
            }
          }
        }
      }
      return liReturn
    }
  }

  static GetDateDiffString(Date1: DateTime, Date2: DateTime, Interval: string): string {
    const dblYearLen: number = 365 //年的长度，365天
    const dblMonthLen: number = 365 / 12 //每个月平均的天数
    const objT: number = Date2.getTime() - Date1.getTime()
    switch (Interval) {
      case 'y': //返回日期的年份间隔
        return Math.floor(objT / (dblYearLen * 24 * 60 * 60 * 1000)).toString()
      case 'M': //返回日期的月份间隔
        return Math.floor(objT / (dblMonthLen * 24 * 60 * 60 * 1000)).toString()
      case 'd': //返回日期的天数间隔
        return Math.floor(objT / (24 * 60 * 60 * 1000)).toString()
      case 'h': //返回日期的小时间隔
        return Math.floor(objT / (60 * 60 * 1000)).toString()
      case 'm': //返回日期的分钟间隔
        return Math.floor(objT / (60 * 1000)).toString()
      case 's': //返回日期的秒钟间隔
        return Math.floor(objT / 1000).toString()
      case 'ms': //返回时间的微秒间隔
        return objT.toString()
      case 'all':
        return objT.toString()
      default:
        break
    }
    return '0'
  }

  static AddDate(DatePart: string, iAdd: number, dt1: DateTime): DateTime {
    let dtNew: DateTime = new DateTime(dt1)
    if (DatePart === 's') {
      dtNew.setSeconds(dtNew.getSeconds() + iAdd)
    } else if (DatePart === 'm') {
      dtNew.setMinutes(dtNew.getMinutes() + iAdd)
    } else if (DatePart === 'h') {
      dtNew.setHours(dtNew.getHours() + iAdd)
    } else if (DatePart === 'd') {
      dtNew.setDate(dtNew.getDate() + iAdd)
    } else if (DatePart === 'M') {
      dtNew.setMonth(dtNew.getMonth() + iAdd)
    }
    return dtNew
  }

  public GetObjectData(info: any, context: any): void {
    ;(this.SchProductRouteList as any).GetObjectData(info, context)
    ;(this.SchProductRouteResList as any).GetObjectData(info, context)
    ;(this.TaskTimeRangeList as any).GetObjectData(info, context)
  }
  getDateDiff(r: any, d: any, beg: any, end: any): any {}
  GetDateDiff(...args) {
    // let a=[...args]
    //@ts-ignore
    return this.getDateDiff(...args)
  }
}
