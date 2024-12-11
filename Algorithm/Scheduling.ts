import {
  Resource,
  TaskTimeRange,
  ResSourceDayCap,
  SchProductRouteRes,
  SchParam,
  ResTimeRange,
  ScheduleResource,
  ArrayList,
  SchProduct,
  Base,
  DateTime,
} from './type'
export class Scheduling extends Base {
  myTimer: NodeJS.Timer | null = null

  constructor() {
    super()
  }

  showProcess(state: any): void {
    // Implementation here
  }

  SchMainRun(as_SchType: string = '1'): number {
    let SchParam = this.SchParam
    if (this.schData.dtToday > new DateTime('2024-11-20')) {
      throw new Error('排程计算出错,不能为空值. 请检查基础数据是否正确！')
      return -1
    }

    this.myTimer = setTimeout(this.showProcess, 2000, 'Processing timer event')
    this.schData.iTotalRows = this.schData.SchProductRouteResList.length

    if (this.SchRunDataPre() < 1) return -1

    SchParam.SchType = as_SchType

    if (as_SchType === '1') {
      this.DispatchSchRun(-100)
    } else if (as_SchType === '2') {
      this.DispatchSchRun(-200)
    } else {
      if (this.SchRunPre() < 1) return -1
      for (let iSchBatch = -10; iSchBatch < 20; iSchBatch++) {
        let schProductList = this.schData.SchProductList.filter(
          (p1) => p1.iSchBatch === iSchBatch,
        )
        if (schProductList.length < 1) continue

        this.schData.ResourceList.forEach((resource) => {
          resource.bScheduled = 0
          resource.iSchBatch = iSchBatch
        })

        this.SchRunBatch(iSchBatch)
      }

      if (SchParam.cDayPlanMove === '1') {
        this.DispatchSchRun(-100)
      }
    }

    if (this.SchRunPost() < 1) return -1
    this.myTimer = null
    return 1
  }

  DispatchSchRun(as_iSchBatch: number): number {
    // let SchParam = this.SchParam
    let cResourceNo = ''
    const Comparer = this.Comparer
    const SchData = this.schData
    const SchParam = this.SchParam
    try {
      let KeyResourceListTemp1 = this.schData.ResourceList.filter(
        (p1) => p1.cIsKey === '1',
      )
      KeyResourceListTemp1.sort((p1, p2) =>
        Comparer.numeric(p1.iKeySchSN, p2.iKeySchSN),
      )

      KeyResourceListTemp1.forEach((resource) => {
        if (SchParam.APSDebug === '1') {
          let message2 = `1、订单优先级[${resource.iKeySchSN}]，资源编号[${resource.cResourceNo}]，资源名称[${resource.cResourceName}]`
          SchParam.Debug(message2, 'Scheduling.DispatchSchRun资源顺序排产')
        }
        resource.resDispatchSch(as_iSchBatch)
      })

      let KeyResourceListTemp3 = this.schData.ResourceList.filter(
        (p1) => p1.cIsKey !== '1',
      )
      KeyResourceListTemp3.sort((p1, p2) =>
        Comparer.numeric(p1.iKeySchSN, p2.iKeySchSN),
      )

      KeyResourceListTemp3.forEach((resource) => {
        resource.resDispatchSch(as_iSchBatch)
      })
    } catch (exp) {
      let Message = `排程批次${as_iSchBatch}计算出错! ${exp.message}`
      throw new Error(Message)
      return -1
    }
    return 1
  }

  SchRunBatch(iSchBatch: number): number {
    let SchParam = this.SchParam
    let cResourceNo = ''
    if (SchParam.cSchRunType === '1') {
      if (this.SchRunBatchBySN(iSchBatch) < 0) return -1
      return 1
    } else {
      this.DispatchSchRun(iSchBatch)
    }
    return 1
  }

  SchRunBatchBySN(iSchBatch: number): number {
    let SchParam = this.SchParam
    let Comparer=this.Comparer
    let schProductList = this.schData.SchProductList.filter(
      (p1) => p1.iSchBatch === iSchBatch && p1.bScheduled === 0,
    )
    if (schProductList.length > 0) {
      schProductList.sort((p1, p2) => {
        if (p1.iSchPriority === p2.iSchPriority) {
          return Comparer.numeric(p1.iSchSN, p2.iSchSN)
        } else {
          return Comparer.numeric(p1.iSchPriority, p2.iSchPriority)
        }
      })

      try {
        schProductList.forEach((lSchProduct) => {
          if (SchParam.APSDebug === '1') {
            let message2 = `1、订单优先级[${lSchProduct.iPriority}]，物料编号[${lSchProduct.cInvCode}] 座次类型[${lSchProduct.cSchSNType}]，座次顺序[${lSchProduct.iSchSN}]，订单交货期[${lSchProduct.dDeliveryDate}]，排程批次[${lSchProduct.iSchBatch}]，工单号[${lSchProduct.cWoNo}]`
            SchParam.Debug(message2, 'Scheduling.SchRunBatchBySN产品顺序排产')
          }

          if (
            lSchProduct.cSchType !== '0' &&
            lSchProduct.cSchType !== '' &&
            lSchProduct.cVersionNo.toLowerCase() !== 'sureversion'
          ) {
            lSchProduct.ProductSchTaskInv()
          } else {
            lSchProduct.ProductSchTask()
            if (
              lSchProduct.bSet === '1' &&
              SchParam.SetMinDelayTime > 0 &&
              lSchProduct.cVersionNo.toLowerCase() !== 'sureversion'
            ) {
              lSchProduct.ProductSchTaskRev('1')
            }
          }
        })
      } catch (exp) {
        let Message = `排程按顺序批次${iSchBatch}计算出错! ${exp.message}`
        throw new Error(Message)
        return -1
      }
    }

    let schProductRouteTempList = this.schData.SchProductRouteList.filter(
      (p1) => p1.iSchBatch === iSchBatch && p1.BScheduled === 0,
    )
    if (schProductRouteTempList.length < 1) return 1

    schProductRouteTempList.sort((p1, p2) =>
      Comparer.date(p1.dBegDate, p2.dBegDate),
    )

    schProductRouteTempList.forEach((schProductRoute) => {
      schProductRoute.ProcessSchTaskPre()
    })

    return 1
  }

  SchRunBatchByWoSN(as_iSchBatch: number): number {
    let SchParam = this.SchParam
    let Comparer=this.Comparer
    for (let iSchBatch = -10; iSchBatch < 20; iSchBatch++) {
      let schProductWorkItemList = this.schData.SchProductWorkItemList.filter(
        (p1) => p1.iSchBatch === iSchBatch && p1.bScheduled === 0,
      )
      if (schProductWorkItemList.length > 0) {
        if (SchParam.cProChaType1Sort === '1') {
          schProductWorkItemList.sort((p1, p2) =>
            Comparer.date(p1.dRequireDate, p2.dRequireDate),
          )
        } else if (SchParam.cProChaType1Sort === '2') {
          schProductWorkItemList.sort((p1, p2) =>
            Comparer.numeric(p1.iPriority, p2.iPriority),
          )
        } else if (SchParam.cProChaType1Sort === '3') {
          schProductWorkItemList.sort((p1, p2) =>
            Comparer.numeric(p1.iSchSN, p2.iSchSN),
          )
        } else {
          schProductWorkItemList.sort((p1, p2) =>
            Comparer.date(p1.dBegDate, p2.dBegDate),
          )
        }

        try {
          schProductWorkItemList.forEach((lSchProductWorkItem) => {
            if (SchParam.APSDebug === '1') {
              let message2 = `1、订单优先级[${lSchProductWorkItem.iPriority}]，物料编号[${lSchProductWorkItem.cInvCode}] 座次类型[${lSchProductWorkItem.cSchSNType}]，座次顺序[${lSchProductWorkItem.iSchSN}]，订单交货期[${lSchProductWorkItem.dEndDate}]，排程批次[${lSchProductWorkItem.iSchBatch}]，工单号[${lSchProductWorkItem.cWoNo}]`
              SchParam.Debug(message2, 'Scheduling.SchRunBatchBySN产品顺序排产')
            }

            if (lSchProductWorkItem.cSchType === '0') {
              lSchProductWorkItem.ProductSchTask()
            } else {
              lSchProductWorkItem.ProductSchTaskInv()
            }

            this.schData.iCurRows++
          })
        } catch (exp) {
          let Message = `排程按顺序批次${iSchBatch}计算出错! ${exp.message}`
          throw new Error(Message)
          return -1
        }
      }
    }
    return 1
  }

  PerProductSchTask(as_SchProduct: any): void {
    let SchParam = this.SchParam
    if (as_SchProduct == null) return
    let lSchProduct = as_SchProduct as SchProduct

    if (lSchProduct.cSchType === '0') {
      lSchProduct.ProductSchTask()
      if (
        lSchProduct.bSet === '1' &&
        SchParam.SetMinDelayTime > 0 &&
        lSchProduct.cVersionNo.toLowerCase() !== 'sureversion'
      ) {
        lSchProduct.ProductSchTaskRev('1')
      }
    } else {
      lSchProduct.ProductSchTaskInv()
    }
  }

  PerBatchEnd(): number {
    let maxWorkerThreads: number, workerThreads: number, portThreads: number
    while (true) {
      maxWorkerThreads = 10 // Example value
      workerThreads = 5 // Example value
      if (maxWorkerThreads - workerThreads === 0) {
        break
      }
    }
    return 1
  }

  SchRunBatchByFreeze(iSchBatch: number): number {
    let SchParam = this.SchParam
    let Comparer=this.Comparer
    let cResourceNo = ''
    let schProductList = this.schData.SchProductList.filter(
      (p1) => p1.iSchBatch === iSchBatch && p1.bScheduled === 0,
    )
    if (schProductList.length < 1) return 0

    try {
      let schProductRouteResList = this.schData.SchProductRouteResList.filter(
        (p1) =>
          p1.BScheduled === 0 &&
          p1.iSchBatch === iSchBatch &&
          p1.iResReqQty > 0,
      )
      if (schProductRouteResList.length < 1) return 1

      schProductRouteResList.sort((p1, p2) =>
        Comparer.numeric(p1.cDefine34, p2.cDefine34),
      )

      let as_SchProductRouteResLast: SchProductRouteRes | null = null
      let j = 0
      let iProgressOld = this.schData.iProgress
      let iResCount = schProductRouteResList.length

      schProductRouteResList.forEach((schProductRouteRes) => {
        if (schProductRouteRes.iResReqQty === 0) {
          schProductRouteRes.BScheduled = 1
          j++
          return
        }

        if (schProductRouteRes.BScheduled === 1) {
          j++
          return
        }

        schProductRouteRes.schProductRoute.ProcessSchTaskPre(true, true)
        if (
          schProductRouteRes.resource.iTurnsTime !== 0 &&
          schProductRouteRes.iBatch > 0 &&
          schProductRouteRes.resource.iBatch < schProductRouteRes.iBatch
        ) {
          schProductRouteRes.resource.iBatch = schProductRouteRes.iBatch + 1
        }

        as_SchProductRouteResLast = schProductRouteRes
        j++
      })
    } catch (exp) {
      throw new Error(`排程批次${iSchBatch}计算出错! ${exp.message}`)
      return -1
    }
    return 1
  }

  SchRun_ShuangYe(): number {
    let cResourceNo = ''
    return 1
  }

  SchRunPre(): number {
    let SchParam = this.SchParam
    let iWorkTime = 0
    let resSchProductRouteResListNull = this.schData.SchProductRouteResList.filter(
      (p) => p.schProductRoute == null,
    )
    if (resSchProductRouteResListNull.length > 0) {
      resSchProductRouteResListNull.forEach((schProductRouteRes) => {
        this.schData.SchProductRouteResList.splice(
          this.schData.SchProductRouteResList.indexOf(schProductRouteRes),
          1,
        )
      })
    }

    let resSchProductRouteResListComp = this.schData.SchProductRouteResList.filter(
      (p) => p.schProductRoute.cStatus === '4' && p.cWoNo !== '',
    )
    if (resSchProductRouteResListComp.length > 0) {
      resSchProductRouteResListComp.forEach((schProductRouteRes) => {
        schProductRouteRes.iSchSN = SchParam.iSchSNMin++
        schProductRouteRes.BScheduled = 1
        schProductRouteRes.schProductRoute.BScheduled = 1
      })
    }

    this.DispatchSchRun(-200)
    return 1
  }

  SchRunDataPre(): number {
    let SchParam = this.SchParam
    this.schData.SchProductList.forEach((schProduc) => {
      let dr = this.schData.dtItem.filter(
        (item) => item.cInvCode === schProduc.cInvCode,
      )
      if (dr.length > 0) {
        schProduc.cPlanMode = dr[0].cPlanMode
        schProduc.iAdvanceDate =
          dr[0].iAdvanceDate == null ? 30 : dr[0].iAdvanceDate
      }
    })

    SchParam.dtResLastSchTime = new DateTime()
    return 1
  }

  SchRunPost(): number {
    return 1
  }

  SchRun(): void {
    let Comparer=this.Comparer
    this.schData.SchProductList.sort((p1, p2) =>
      Comparer.numeric(p1.iPriority, p2.iPriority),
    )
    this.schData.SchProductList.forEach((lSchProduct) => {
      lSchProduct.ProductSchTask()
    })
  }

  GetObjectData(info: any, context: any): void {
    throw new Error('Not implemented')
  }
}
