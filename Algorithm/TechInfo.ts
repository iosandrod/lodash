import { SchData } from './type' // Assuming SchData is defined in a separate file

export class TechInfo {
  public schData: SchData | null = null

  constructor()
  constructor(cTechNo: string, as_SchData: SchData)
  constructor(cTechNo?: string, as_SchData?: SchData) {
    if (cTechNo && as_SchData) {
      this.schData = as_SchData
      const dr = this.schData.dtTechInfo.filter(
        (row) => row.cTechNo === cTechNo,
      )
      if (dr.length < 1) return
      this.GetTechInfo(dr[0])
    }
  }

  public GetTechInfo(drTechInfo: any): void {
    try {
      this.iInterID = drTechInfo.iInterID
      this.cTechNo = drTechInfo.cTechNo
      this.cTechName = drTechInfo.cTechName
      this.cResClsNo = drTechInfo.cResClsNo
      this.cWcNo = drTechInfo.cWcNo
      this.cDeptNo = drTechInfo.cDeptNo
      this.cResourceNo = drTechInfo.cResourceNo
      this.cTechReq = drTechInfo.cTechReq
      this.cNote = drTechInfo.cNote
      this.cFormula = drTechInfo.cFormula
      this.cFormula2 = drTechInfo.cFormula2
      this.iTechValue = Number(drTechInfo.iTechValue)
      this.iOrder = Number(drTechInfo.iOrder)
      this.iTechDifficulty = Number(drTechInfo.iTechDifficulty)
      this.iSeqPretime = Number(drTechInfo.iSeqPretime)
      this.iSeqPostTime = Number(drTechInfo.iSeqPostTime)
      this.cTechDefine1 = drTechInfo.cTechDefine1
      this.cTechDefine2 = drTechInfo.cTechDefine2
      this.cTechDefine3 = drTechInfo.cTechDefine3
      this.cTechDefine4 = drTechInfo.cTechDefine4
      this.cTechDefine5 = drTechInfo.cTechDefine5
      this.cTechDefine6 = drTechInfo.cTechDefine6
      this.cTechDefine7 = drTechInfo.cTechDefine7
      this.cTechDefine8 = drTechInfo.cTechDefine8
      this.cTechDefine9 = drTechInfo.cTechDefine9
      this.cTechDefine10 = drTechInfo.cTechDefine10
      this.cTechDefine11 = Number(drTechInfo.cTechDefine11)
      this.cTechDefine12 = Number(drTechInfo.cTechDefine12)
      this.cTechDefine13 = Number(drTechInfo.cTechDefine13)
      this.cTechDefine14 = Number(drTechInfo.cTechDefine14)
      this.cTechDefine15 = new Date(drTechInfo.cTechDefine15)
      this.cTechDefine16 = new Date(drTechInfo.cTechDefine16)
      this.cAttributeValue1 = drTechInfo.cAttributeValue1
      this.cAttributeValue2 = drTechInfo.cAttributeValue2
      this.cAttributeValue3 = drTechInfo.cAttributeValue3
      this.cAttributeValue4 = drTechInfo.cAttributeValue4
      this.cAttributeValue5 = drTechInfo.cAttributeValue5
      this.cAttributeValue6 = drTechInfo.cAttributeValue6
      this.cAttributeValue7 = drTechInfo.cAttributeValue7
      this.cAttributeValue8 = drTechInfo.cAttributeValue8
      this.cAttributeValue9 = drTechInfo.cAttributeValue9
      this.cAttributeValue10 = drTechInfo.cAttributeValue10
    } catch (exp) {
      throw exp
    }
  }

  public iInterID: number
  public cTechNo: string
  public cTechName: string
  public cResClsNo: string
  public cWcNo: string
  public cDeptNo: string
  public cResourceNo: string
  public cTechReq: string
  public cNote: string
  public cFormula: string
  public cFormula2: string
  public iTechValue: number
  public iOrder: number
  public iTechDifficulty: number
  public iSeqPretime: number
  public iSeqPostTime: number
  public cTechDefine1: string
  public cTechDefine2: string
  public cTechDefine3: string
  public cTechDefine4: string
  public cTechDefine5: string
  public cTechDefine6: string
  public cTechDefine7: string
  public cTechDefine8: string
  public cTechDefine9: string
  public cTechDefine10: string
  public cTechDefine11: number
  public cTechDefine12: number
  public cTechDefine13: number
  public cTechDefine14: number
  public cTechDefine15: Date
  public cTechDefine16: Date
  public cAttributeValue1: string
  public cAttributeValue2: string
  public cAttributeValue3: string
  public cAttributeValue4: string
  public cAttributeValue5: string
  public cAttributeValue6: string
  public cAttributeValue7: string
  public cAttributeValue8: string
  public cAttributeValue9: string
  public cAttributeValue10: string
}
