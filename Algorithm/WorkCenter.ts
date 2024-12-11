import { SchData, Resource, Base } from './type' // Assuming SchData is defined in a separate file

export class WorkCenter extends Base {
  public schData: SchData | null = null
  public ListResource: Resource[] = new Array(10)

  constructor()
  constructor(cWcNo: string, as_SchData: SchData)
  constructor(cWcNo?: string, as_SchData?: SchData) {
    super()
    if (cWcNo && as_SchData) {
      this.schData = as_SchData
      const dr = this.schData.dtWorkCenter.filter((row) => row.cWcNo === cWcNo)
      if (dr.length < 1) return
      this.GetWorkCenter(dr[0])
    }
  }

  public GetWorkCenter(drWorkCenter: any): void {
    try {
      this.iWcID = drWorkCenter.iWcID
      this.cWcNo = drWorkCenter.cWcNo
      this.cWcName = drWorkCenter.cWcName
      this.cWcClsNo = drWorkCenter.cWcClsNo
      this.cAbilityMode = drWorkCenter.cAbilityMode
      this.cDeptNo = drWorkCenter.cDeptNo
      this.iWorkersPd = Number(drWorkCenter.iWorkersPd)
      this.iShiftPd = Number(drWorkCenter.iShiftPd)
      this.iDevCount = Number(drWorkCenter.iDevCount)
      this.iUsage = Number(drWorkCenter.iUsage)
      this.iEfficient = Number(drWorkCenter.iEfficient)
      this.cEntrustNo = drWorkCenter.cEntrustNo
      this.bKeyRes = drWorkCenter.bKeyRes
      this.cStatus = drWorkCenter.cStatus
      this.iLaborRate = Number(drWorkCenter.iLaborRate)
      this.iFixRate = Number(drWorkCenter.iFixRate)
      this.iFlexRate = Number(drWorkCenter.iFlexRate)
      this.bBackflash = drWorkCenter.bBackflash
      this.bWasteDirect = drWorkCenter.bWasteDirect
      this.cDutyor = drWorkCenter.cDutyor
      this.bFakeRet = drWorkCenter.bFakeRet
      this.cNote = drWorkCenter.cNote
      this.cLaborRateType = drWorkCenter.cLaborRateType
      this.iOverHours = Number(drWorkCenter.iOverHours)
      this.cPrvNo = drWorkCenter.cPrvNo
      this.cWcDefine1 = drWorkCenter.cWcDefine1
      this.cWcDefine2 = drWorkCenter.cWcDefine2
      this.cWcDefine3 = drWorkCenter.cWcDefine3
      this.cWcDefine4 = drWorkCenter.cWcDefine4
      this.cWcDefine5 = drWorkCenter.cWcDefine5
      this.cWcDefine6 = drWorkCenter.cWcDefine6
      this.cWcDefine7 = drWorkCenter.cWcDefine7
      this.cWcDefine8 = drWorkCenter.cWcDefine8
      this.cWcDefine9 = drWorkCenter.cWcDefine9
      this.cWcDefine10 = drWorkCenter.cWcDefine10
      this.cWcDefine11 = Number(drWorkCenter.cWcDefine11)
      this.cWcDefine12 = Number(drWorkCenter.cWcDefine12)
      this.cWcDefine13 = Number(drWorkCenter.cWcDefine13)
      this.cWcDefine14 = Number(drWorkCenter.cWcDefine14)
      this.cWcDefine15 = new Date(drWorkCenter.cWcDefine15)
      this.cWcDefine16 = new Date(drWorkCenter.cWcDefine16)
    } catch (exp) {
      throw exp
    }
  }

  // Properties
  public iWcID: number
  public cWcNo: string
  public cWcName: string
  public cWcClsNo: string
  public cAbilityMode: string
  public cDeptNo: string
  public iWorkersPd: number
  public iShiftPd: number
  public iDevCount: number
  public iUsage: number
  public iEfficient: number
  public cEntrustNo: string
  public bKeyRes: string
  public cStatus: string
  public iLaborRate: number
  public iFixRate: number
  public iFlexRate: number
  public bBackflash: string
  public bWasteDirect: string
  public cDutyor: string
  public bFakeRet: string
  public cNote: string
  public cLaborRateType: string
  public iOverHours: number
  public cPrvNo: string
  public cWcDefine1: string
  public cWcDefine2: string
  public cWcDefine3: string
  public cWcDefine4: string
  public cWcDefine5: string
  public cWcDefine6: string
  public cWcDefine7: string
  public cWcDefine8: string
  public cWcDefine9: string
  public cWcDefine10: string
  public cWcDefine11: number
  public cWcDefine12: number
  public cWcDefine13: number
  public cWcDefine14: number
  public cWcDefine15: Date
  public cWcDefine16: Date
}
