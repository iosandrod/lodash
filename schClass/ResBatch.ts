class ResBatch {
  // 定义资源任务列表
  public ListSchProductRouteRes: SchProductRouteRes[] = []

  // 属性封装
  public iWcID?: number // 工作中心ID
  public cWcNo?: string // 工作中心编号
  public cWcName?: string // 工作中心名称
  public cWcClsNo?: string // 类别
  public cAbilityMode?: string // 能力类型
  public cDeptNo?: string // 部门
  public iWorkersPd?: number // 日人工数
  public iShiftPd?: number // 班次
  public iDevCount?: number // 设备数
  public iUsage?: number // 利用率
  public iEfficient?: number // 效率(%)
  public cEntrustNo?: string // 部门
  public bKeyRes?: string // 是否关键工作中心
  public cStatus?: string // 工作中心状态
  public iLaborRate?: number // 人工费率
  public iFixRate?: number // 固定费率
  public iFlexRate?: number // 灵活费率
  public bBackflash?: string // 是否缓冲点
  public bWasteDirect?: string // 是否废料
  public cDutyor?: string // 责任人
  public bFakeRet?: string // 退料点
  public cNote?: string // 备注
  public cLaborRateType?: string // 费率类型
  public iOverHours?: number // 允许加班小时
  public cPrvNo?: string // 权限编号
  public cWcDefine1?: string // 工作中心自定义项1
  public cWcDefine2?: string
  public cWcDefine3?: string
  public cWcDefine4?: string
  public cWcDefine5?: string
  public cWcDefine6?: string
  public cWcDefine7?: string
  public cWcDefine8?: string
  public cWcDefine9?: string
  public cWcDefine10?: string
  public cWcDefine11?: number
  public cWcDefine12?: number
  public cWcDefine13?: number
  public cWcDefine14?: number
  public cWcDefine15?: Date
  public cWcDefine16?: Date

  // 构造函数
  constructor()
  constructor(cWcNo?: string)
  constructor(cWcNo?: string) {
    this.ListSchProductRouteRes = []
    if (cWcNo && cWcNo !== '') {
      // 初始化操作，例如调用获取数据的方法
      // this.getResBatch(cWcNo);
    }
  }

  // 示例方法（逻辑需要自己补充）
  private getResBatch(cWcNo: string): void {
    // 示例：模拟从数据源获取数据并设置属性
    // 注意：需要根据实际数据结构实现逻辑
    console.log(`Fetching data for work center: ${cWcNo}`)
    // 设置属性为模拟数据
    this.cWcNo = cWcNo
  }
}

// 示例接口，定义 SchProductRouteRes
interface SchProductRouteRes {
  // 根据实际需求定义
  id: number
  name: string
}
