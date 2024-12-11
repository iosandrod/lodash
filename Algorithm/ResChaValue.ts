import {
  SchData,
  SchProductWorkItem,
  SchProductRouteRes,
  DataRow,
} from './type'
export class ResChaValue {
  public schData: SchData | null = null // 所有排程数据
  public cResourceNo: string = '' // 对应资源编号,要设置
  private schProductRouteRes: SchProductRouteRes // 工艺特征对应的资源任务
  public iPosition: number = 0 // 对应的工艺特征位置，资源任务的第几个工艺特征
  public iUsedTime: number = 0 // 对应当前任务，当前刀具一个换刀周期累计使用时间。

  public FResChaValueID: number // 工艺特征内码
  public FResChaValueNo: string // 工艺特征编号
  public FResChaValueName: string // 工艺特征名称
  public FProChaTypeID: string // 特征类别
  public FResChaCycleValue: number // 换产周期 分
  public FResChaRePlaceTime: number // 每次换产时间 分
  public FResChaMemo: string // 备注
  public FSchSN: number // 按工艺特征，最优排产顺序
  public FUseFixedPlaceTime: string // 是否固定更换耗时
  public FUseChaCycleValue: string // 是否定期更换
  public cDefine1: string
  public cDefine2: string
  public cDefine3: string
  public cDefine4: string
  public cDefine5: string
  public cDefine6: string
  public cDefine7: string
  public cDefine8: string
  public cDefine9: string
  public cDefine10: string
  public cDefine11: number
  public cDefine12: number
  public cDefine13: number
  public cDefine14: number
  public cDefine15: Date
  public cDefine16: Date

  constructor()
  constructor(
    iResChaValueID: string,
    schProductRouteRes: SchProductRouteRes,
    iPosition: number,
  )
  constructor(iResChaValueID: string)
  constructor(
    iResChaValueID?: string,
    schProductRouteRes?: SchProductRouteRes,
    iPosition?: number,
  ) {
    if (iResChaValueID && iResChaValueID !== '') {
      this.GetResChaValue(iResChaValueID)
    }
    if (schProductRouteRes) {
      this.schProductRouteRes = schProductRouteRes
      this.iPosition = iPosition || 0
      this.cResourceNo = schProductRouteRes.cResourceNo
      this.schData = schProductRouteRes.schData
    }
  }

  public GetResChaValue(iResChaValueID: string): void {
    if (typeof iResChaValueID == 'string') {
      const dr: DataRow[] = this.schData.dtResChaValue.Select(
        `FResChaValueNo = '${iResChaValueID}'`,
      )
      if (dr.length < 1) return
      this.GetResChaValue(dr[0] as any)
    } else {
      let drResource: any = iResChaValueID
      try {
        this.FResChaValueID = drResource['FResChaValueID'] as number // 工艺特征内码
        this.FResChaValueNo = drResource['FResChaValueNo'].toString() // 工艺特征编号
        this.FResChaValueName = drResource['FResChaValueName'].toString() // 工艺特征名称
        this.FProChaTypeID = drResource['FProChaTypeID'].toString() // 特征类别
        this.FResChaCycleValue =
          (drResource['FResChaCycleValue'] as number) * 60 // 换产周期 分钟，统一转换成秒
        this.FResChaRePlaceTime = drResource['FResChaRePlaceTime'] as number // 平均更换耗时 秒
        this.FResChaMemo = drResource['FResChaMemo'].toString() // 备注
        this.FSchSN =
          drResource['FSchSN'] === null ? 0 : (drResource['FSchSN'] as number) // 按工艺特征，最优排产顺序
        this.FUseFixedPlaceTime = drResource['FUseFixedPlaceTime'].toString() // 是否固定更换耗时
        this.FUseChaCycleValue = drResource['FUseChaCycleValue'].toString() // 是否定期更换
        this.cDefine1 = drResource['cDefine1'].toString()
        this.cDefine2 = drResource['cDefine2'].toString()
        this.cDefine3 = drResource['cDefine3'].toString()
        this.cDefine4 = drResource['cDefine4'].toString()
        this.cDefine5 = drResource['cDefine5'].toString()
        this.cDefine6 = drResource['cDefine6'].toString()
        this.cDefine7 = drResource['cDefine7'].toString()
        this.cDefine8 = drResource['cDefine8'].toString()
        this.cDefine9 = drResource['cDefine9'].toString()
        this.cDefine10 = drResource['cDefine10'].toString()
        this.cDefine11 = drResource['cDefine11'] as number
        this.cDefine12 = drResource['cDefine12'] as number
        this.cDefine13 = drResource['cDefine13'] as number
        this.cDefine14 = drResource['cDefine14'] as number
        this.cDefine15 = drResource['cDefine15'] as Date
        this.cDefine16 = drResource['cDefine16'] as Date
      } catch (exp) {
        throw exp
      }
    }
  }

  // public GetResChaValue(drResource: DataRow): void {
  //   try {
  //     this.FResChaValueID = drResource['FResChaValueID'] as number // 工艺特征内码
  //     this.FResChaValueNo = drResource['FResChaValueNo'].toString() // 工艺特征编号
  //     this.FResChaValueName = drResource['FResChaValueName'].toString() // 工艺特征名称
  //     this.FProChaTypeID = drResource['FProChaTypeID'].toString() // 特征类别
  //     this.FResChaCycleValue = (drResource['FResChaCycleValue'] as number) * 60 // 换产周期 分钟，统一转换成秒
  //     this.FResChaRePlaceTime = drResource['FResChaRePlaceTime'] as number // 平均更换耗时 秒
  //     this.FResChaMemo = drResource['FResChaMemo'].toString() // 备注
  //     this.FSchSN =
  //       drResource['FSchSN'] === null ? 0 : (drResource['FSchSN'] as number) // 按工艺特征，最优排产顺序
  //     this.FUseFixedPlaceTime = drResource['FUseFixedPlaceTime'].toString() // 是否固定更换耗时
  //     this.FUseChaCycleValue = drResource['FUseChaCycleValue'].toString() // 是否定期更换
  //     this.cDefine1 = drResource['cDefine1'].toString()
  //     this.cDefine2 = drResource['cDefine2'].toString()
  //     this.cDefine3 = drResource['cDefine3'].toString()
  //     this.cDefine4 = drResource['cDefine4'].toString()
  //     this.cDefine5 = drResource['cDefine5'].toString()
  //     this.cDefine6 = drResource['cDefine6'].toString()
  //     this.cDefine7 = drResource['cDefine7'].toString()
  //     this.cDefine8 = drResource['cDefine8'].toString()
  //     this.cDefine9 = drResource['cDefine9'].toString()
  //     this.cDefine10 = drResource['cDefine10'].toString()
  //     this.cDefine11 = drResource['cDefine11'] as number
  //     this.cDefine12 = drResource['cDefine12'] as number
  //     this.cDefine13 = drResource['cDefine13'] as number
  //     this.cDefine14 = drResource['cDefine14'] as number
  //     this.cDefine15 = drResource['cDefine15'] as Date
  //     this.cDefine16 = drResource['cDefine16'] as Date
  //   } catch (exp) {
  //     throw exp
  //   }
  // }

  public GetChaValueChangeTime(
    as_SchProductRouteRes: SchProductRouteRes,
    as_ResChaValuePre: ResChaValue | null,
    ai_workTime: number,
    ai_cyctime: { value: number },
    bSchdule: boolean,
    as_SchProductRouteResPre: SchProductRouteRes | null,
  ): number {
    let iChaValue = 0
    try {
      if (!this.FResChaValueNo) {
        // 如果没有工艺特征，返回0
        return this.GetFResChaValue1Cyc(as_SchProductRouteRes)
      } else {
        // 当前有工艺特征
        if (this.FUseFixedPlaceTime === '1') {
          // 固定更换耗时，优先取物料资源工艺特征的转换时间
          if (
            as_ResChaValuePre &&
            as_ResChaValuePre.FResChaValueNo === this.FResChaValueNo
          ) {
            iChaValue = 0
          } else if (
            as_ResChaValuePre &&
            as_ResChaValuePre.FResChaValueNo !== this.FResChaValueNo
          ) {
            iChaValue = this.GetFResChaValue1Cyc(as_SchProductRouteRes)
            if (iChaValue === 0)
              // 如果为0，则取工艺特征档案中的转换时间
              iChaValue = this.FResChaRePlaceTime // *60;
          } else if (
            !as_ResChaValuePre ||
            (as_SchProductRouteResPre &&
              as_SchProductRouteRes.schProductRoute.schProduct.cMiNo !==
                as_SchProductRouteResPre.schProductRoute.schProduct.cMiNo)
          ) {
            iChaValue = this.GetFResChaValue1Cyc(as_SchProductRouteRes)
            if (iChaValue === 0)
              // 如果为0，则取工艺特征档案中的转换时间
              iChaValue = this.FResChaRePlaceTime // *60;
          } else {
            iChaValue = this.GetFResChaValue1Cyc(as_SchProductRouteRes)
            if (iChaValue === 0)
              // 如果为0，则取工艺特征档案中的转换时间
              iChaValue = this.FResChaRePlaceTime // *60;
          }
        } else {
          // 使用二维转换定义工艺特征转换时间 2020-08-23 Jonas Cheng
          if (as_ResChaValuePre) {
            if (as_ResChaValuePre.FResChaValueNo !== this.FResChaValueNo)
              iChaValue = this.GetTwoChaValueChangeTime(
                as_ResChaValuePre.FResChaValueNo,
              )
            else iChaValue = 0 // 相同为0
          } else iChaValue = this.FResChaRePlaceTime // 前面没有任务，取平均更换时间(秒)   * 60
        }
      }
      if (this.FUseChaCycleValue === '1' && this.FResChaCycleValue > 0) {
        if (
          as_ResChaValuePre &&
          as_ResChaValuePre.FResChaValueNo === this.FResChaValueNo
        ) {
          // 没有换刀,包含前工艺特征的时间
          const iTime = Math.floor(
            (ai_workTime + as_ResChaValuePre.iUsedTime) /
              this.FResChaCycleValue,
          ) // 计算需更换次数
          ai_cyctime.value = iTime * this.FResChaRePlaceTime // 计算换刀时间
          if (bSchdule)
            // 正式排产
            this.iUsedTime =
              as_ResChaValuePre.iUsedTime +
              ai_workTime -
              iTime * this.FResChaCycleValue
        } else {
          // 换刀了,只计算当前刀具的累计未换刀时间
          const iTime = Math.floor(ai_workTime / this.FResChaCycleValue) // 计算需更换次数
          ai_cyctime.value = iTime * this.FResChaRePlaceTime // 计算换刀时间
          if (bSchdule)
            // 正式排产
            this.iUsedTime = ai_workTime - iTime * this.FResChaCycleValue // 累计剩余时间
        }
      }
    } catch (exp) {
      throw new Error(
        '出错位置:ResChaValue.GetChaValueChangeTime 取工艺特征转换时间出错！',
      )
    }
    return iChaValue
  }

  private GetFResChaValue1Cyc(
    as_SchProductRouteRes: SchProductRouteRes,
  ): number {
    switch (this.iPosition) {
      case 1: // 工艺特征1转换时间
        return as_SchProductRouteRes.FResChaValue1Cyc
      case 2: // 工艺特征2转换时间
        return as_SchProductRouteRes.FResChaValue2Cyc
      case 3: // 工艺特征3转换时间
        return as_SchProductRouteRes.FResChaValue3Cyc
      case 4: // 工艺特征4转换时间
        return as_SchProductRouteRes.FResChaValue4Cyc
      case 5: // 工艺特征5转换时间
        return as_SchProductRouteRes.FResChaValue5Cyc
      case 6: // 工艺特征6转换时间
        return as_SchProductRouteRes.FResChaValue6Cyc
      default:
        return as_SchProductRouteRes.FResChaValue1Cyc
    }
  }

  private GetTwoChaValueChangeTime(iResChaValue1ID: string): number {
    // iResChaValue1No
    if (!iResChaValue1ID) return 0
    let iChaValue = 0
    const dr: DataRow[] = this.schData.dtResChaCrossTime.Select(
      `FProChaTypeID = '${this.FProChaTypeID}' and FResChaValue1ID = '${iResChaValue1ID}' and FResChaValue1ID = '${this.FResChaValueNo}'`,
    )
    if (dr && dr.length > 0) {
      iChaValue = parseInt(dr[0]['FResChaExcTime'].toString()) // * 60;   //分钟
    }
    if (iChaValue === 0) {
      iChaValue = this.FResChaRePlaceTime
    }
    return iChaValue
  }

  public CompareTo(obj: any): number {
    if (obj instanceof ResChaValue) {
      return 1
    } else {
      throw new Error('对象非TimeRange类型')
    }
  }

  public Clone(): ResChaValue {
    return Object.assign(Object.create(Object.getPrototypeOf(this)), this)
  }

  // public GetObjectData(
  //   info: SerializationInfo,
  //   context: StreamingContext,
  // ): void {
  //   throw new Error('Not implemented')
  // }
}
 