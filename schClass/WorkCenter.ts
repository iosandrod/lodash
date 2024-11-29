import { Base } from './Base'
import { Resource } from './Resource'

export class WorkCenter extends Base {
  //@ts-ignore
  iWcID!: number // 工作中心ID
  cWcNo!: string // 工作中心编号
  cWcName!: string // 工作中心名称
  cWcClsNo!: string // 类别
  cAbilityMode!: string // 能力类型
  cDeptNo!: string // 部门
  iWorkersPd!: number // 日人工数
  iShiftPd!: number // 班次
  iDevCount!: number // 设备数
  iUsage!: number // 利用率
  iEfficient!: number // 效率(%)
  cEntrustNo!: string // 部门
  bKeyRes!: string // 是否关键工作中心
  cStatus!: string // 状态
  iLaborRate!: number // 人工费率
  iFixRate!: number // 固定费率
  iFlexRate!: number // 浮动费率
  bBackflash!: string // 是否缓冲点
  bWasteDirect!: string // 是否废料直销
  cDutyor!: string // 责任人
  bFakeRet!: string // 退料点
  cNote!: string // 备注
  cLaborRateType!: string // 费率类型
  iOverHours!: number // 允许加班小时
  cPrvNo!: string // 权限编号
  cWcDefine1!: string // 工作中心自定义项1
  cWcDefine2!: string
  cWcDefine3!: string
  cWcDefine4!: string
  cWcDefine5!: string
  cWcDefine6!: string
  cWcDefine7!: string
  cWcDefine8!: string
  cWcDefine9!: string
  cWcDefine10!: string
  cWcDefine11!: number
  cWcDefine12!: number
  cWcDefine13!: number
  cWcDefine14!: number
  cWcDefine15!: Date
  cWcDefine16!: Date

  listResource: Resource[] = [] // 资源列表
  //   constructor()
  //   constructor(cWcNo: string, schData: SchData)
  //   constructor(cWcNo?: string, schData?: SchData) {
  //     if (cWcNo && schData) {
  //       this.schData = schData
  //       const dr = schData.dtWorkCenter.find((row) => row.cWcNo === cWcNo)
  //       if (dr) {
  //         this.getWorkCenter(dr)
  //       }
  //     }
  //   }
  getWorkCenter(drWorkCenter: Record<string, any>): void {
    try {
      this.iWcID = drWorkCenter['iWcID']
      this.cWcNo = drWorkCenter['cWcNo']
      this.cWcName = drWorkCenter['cWcName']
      this.cWcClsNo = drWorkCenter['cWcClsNo']
      this.cAbilityMode = drWorkCenter['cAbilityMode']
      this.cDeptNo = drWorkCenter['cDeptNo']
      this.iWorkersPd = parseFloat(drWorkCenter['iWorkersPd'])
      this.iShiftPd = parseFloat(drWorkCenter['iShiftPd'])
      this.iDevCount = parseFloat(drWorkCenter['iDevCount'])
      this.iUsage = parseFloat(drWorkCenter['iUsage'])
      this.iEfficient = parseFloat(drWorkCenter['iEfficient'])
      this.cEntrustNo = drWorkCenter['cEntrustNo']
      this.bKeyRes = drWorkCenter['bKeyRes']
      this.cStatus = drWorkCenter['cStatus']
      this.iLaborRate = parseFloat(drWorkCenter['iLaborRate'])
      this.iFixRate = parseFloat(drWorkCenter['iFixRate'])
      this.iFlexRate = parseFloat(drWorkCenter['iFlexRate'])
      this.bBackflash = drWorkCenter['bBackflash']
      this.bWasteDirect = drWorkCenter['bWasteDirect']
      this.cDutyor = drWorkCenter['cDutyor']
      this.bFakeRet = drWorkCenter['bFakeRet']
      this.cNote = drWorkCenter['cNote']
      this.cLaborRateType = drWorkCenter['cLaborRateType']
      this.iOverHours = parseFloat(drWorkCenter['iOverHours'])
      this.cPrvNo = drWorkCenter['cPrvNo']
      this.cWcDefine1 = drWorkCenter['cWcDefine1']
      this.cWcDefine2 = drWorkCenter['cWcDefine2']
      this.cWcDefine3 = drWorkCenter['cWcDefine3']
      this.cWcDefine4 = drWorkCenter['cWcDefine4']
      this.cWcDefine5 = drWorkCenter['cWcDefine5']
      this.cWcDefine6 = drWorkCenter['cWcDefine6']
      this.cWcDefine7 = drWorkCenter['cWcDefine7']
      this.cWcDefine8 = drWorkCenter['cWcDefine8']
      this.cWcDefine9 = drWorkCenter['cWcDefine9']
      this.cWcDefine10 = drWorkCenter['cWcDefine10']
      this.cWcDefine11 = parseFloat(drWorkCenter['cWcDefine11'])
      this.cWcDefine12 = parseFloat(drWorkCenter['cWcDefine12'])
      this.cWcDefine13 = parseFloat(drWorkCenter['cWcDefine13'])
      this.cWcDefine14 = parseFloat(drWorkCenter['cWcDefine14'])
      this.cWcDefine15 = new Date(drWorkCenter['cWcDefine15'])
      this.cWcDefine16 = new Date(drWorkCenter['cWcDefine16'])
    } catch (error) {
      throw error
    }
  }
}
