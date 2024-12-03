//@ts-nocheck
export class SchProductRoute {
  schData: any = null // 所有排程数据
  private bScheduled: number = 0 // 是否已排产 0 未排，1 已排
  cVersionNo!: string // 排程版本
  iSchSdID!: number // 排程产品ID
  iModelID!: number // 产品模型ID
  iProcessProductID!: number // 工艺模型中的任务号
  cWoNo!: string // 工单号
  iInterID!: number // 工单内码
  iWoProcessID!: number
  iItemID!: number // 产品ID
  cInvCode!: string // 产品编号
  iWorkItemID!: number // 加工物料ID
  cWorkItemNo!: string // 加工物料编号
  cWorkItemNoFull!: string // 加工物料编号全路径
  iProcessID!: number
  iWoSeqID!: number // 工序号
  cTechNo!: string // 工艺编号
  cSeqNote!: string // 工艺说明
  cWcNo!: string // 工作中心
  iNextSeqID!: number // 后序工序
  cPreProcessID!: string
  cPostProcessID!: string
  cPreProcessItem!: string // 前工序列表，iSchSdID相同，iProcessProductID号
  cPostProcessItem!: string // 后工序列表，iSchSdID相同，iProcessProductID号
  iAutoID!: number
  cLevelInfo!: string
  iLevel!: number
  iParentItemID!: number // 父项ID
  cParentItemNo!: string // 父项编号
  cParentItemNoFull!: string // 父项编号全路径
  cParellelType!: string // 并行类型
  cParallelNo!: string // 并行码
  cKeyBrantch!: string // 关键分支
  cCompSeq!: string // 完工工序
  cMoveType!: string // 移转方式
  iMoveInterTime!: number // 移动间隔时间
  iMoveInterQty!: number // 移动间隔数量
  iMoveTime!: number // 移动时间
  iDevCountPd: number = 1 // 用于加工本工序最大资源数
  cDevCountPdExp!: string // 排产资源数表达式

  private ISeqPretime: number = 0
  get iSeqPreTime(): number {
    return this.ISeqPretime || 0
  }
  set iSeqPreTime(value: number) {
    this.ISeqPretime = value
  } // 工序前准备时间

  private ISeqPostTime: number = 0
  get iSeqPostTime(): number {
    return this.ISeqPostTime || 0
  }
  set iSeqPostTime(value: number) {
    this.ISeqPostTime = value
  } // 工序后准备时间

  iCapacity!: number
  cCapacityExp!: string // 单件工时表达式
  iProcessPassRate!: number
  iEfficiency!: number
  iHoursPd!: number
  iWorkQtyPd!: number
  iWorkersPd!: number
  iLaborTime!: number // 计划生产总工时
  iLeadTime!: number // 提前期
  cStatus!: string // 工序状态
  cSourStatus!: string // 原始工序状态
  iPriority!: number // 工序优先级
  iReqQty!: number // 计划生产数量
  iReqQtyOld!: number // 原计划生产数量
  iActQty!: number // 实际生产数量
  iRealHour!: number // 实际加工工时
  dBegDate!: Date // 计划开工时间
  dEndDate!: Date // 计划完工时间
  dActBegDate!: Date // 实际开工时间
  dActEndDate!: Date // 实际完工时间
  dEarlyBegDate!: Date // 最早可开始时间
  dEarlySubItemDate!: Date // 最早材料到料时间
  iAdvanceDate!: number // 加工件累计提前期
  iAvgLeadTime!: number // 加工提前期
  iItemDifficulty!: number // 物料加工难度系数
  cNote!: string
  cDefine22!: string
  cDefine23!: string
  cDefine24!: string
  cDefine25!: string
  cDefine26!: string
  cDefine27!: string
  cDefine28!: string
  cDefine29!: string
  cDefine30!: string
  cDefine31!: string
  cDefine32!: number
  cDefine33!: number
  cDefine34!: number // 上次排产顺序
  cDefine35!: number // 本次排产顺序
  cDefine36!: Date
  cDefine37!: Date
  iSchBatch: number = 6 // 排产批次
  cBatchNoFlag: number = 0 // 0 托盘资源选择未处理
  dCanBegDate!: Date // 工序最早可开工日期
  dFirstBegDate!: Date // 第一次开工日期
  dFirstEndDate!: Date // 第一次完工日期
  public get bScheduled(): number {
    return this._bScheduled
  }

  public set bScheduled(value: number) {
    if (value === 1 && this._bScheduled === 0) {
      if (this.schData.iCurRows < this.schData.iTotalRows) {
        const iCount = this.SchProductRouteResList.filter(
          (item) => item.iResReqQty === 0,
        ).length
        this.schData.iCurRows += iCount
      }
    } else if (value === 0 && this._bScheduled === 1) {
      const iCount = this.SchProductRouteResList.filter(
        (item) => item.iResReqQty === 0,
      ).length
      this.schData.iCurRows = Math.max(0, this.schData.iCurRows - iCount)
    }
    this._bScheduled = value

    if (this.iSchSdID === SchParam.iSchSdID) {
      const dt = this.dBegDate
      const dt2 = this.dEndDate
    }

    if (
      this.iSchSdID === SchParam.iSchSdID &&
      this.iProcessProductID === SchParam.iProcessProductID
    ) {
      const dt = this.dBegDate
      const dt2 = this.dEndDate
    }
  }

  public ProcessSchTask(bFreeze: boolean = false): number {
    if (this._bScheduled === 1) return 1

    if (
      this.iSchSdID === SchParam.iSchSdID &&
      this.iProcessProductID === SchParam.iProcessProductID
    ) {
      let j = 1
    }

    if (
      this.cParallelNo !== '' &&
      this.cKeyBrantch !== '1' &&
      this.SchProductRouteNextList.length > 0 &&
      this.SchProductRouteNextList[0].bScheduled !== 1
    ) {
      return 1
    }

    let dCanBegDate = this.schData.dtStart

    if (dCanBegDate < this.dEarlySubItemDate) {
      dCanBegDate = this.dEarlySubItemDate
    }
    if (this.schProduct && dCanBegDate < this.schProduct.dEarliestSchDate) {
      dCanBegDate = this.schProduct.dEarliestSchDate
    }
    if (
      this.schProductWorkItem &&
      dCanBegDate < this.schProductWorkItem.dCanBegDate
    ) {
      dCanBegDate = this.schProductWorkItem.dCanBegDate
    }

    if (
      this.cVersionNo.toLowerCase() === 'sureversion' &&
      this.dFirstBegDate > this.schData.dtStart &&
      dCanBegDate <
        this.dFirstBegDate.setDate(
          this.dFirstBegDate.getDate() - SchParam.dLastBegDateBeforeDays,
        )
    ) {
      dCanBegDate = new Date(this.dFirstBegDate)
      dCanBegDate.setDate(
        dCanBegDate.getDate() - SchParam.dLastBegDateBeforeDays,
      )
    }

    this.GetRouteEarlyBegDate()
    if (dCanBegDate < this.dEarlyBegDate) {
      dCanBegDate = this.dEarlyBegDate
    }

    return 1 // Implementation continues based on logic.
  }
  public resourceSelect(dCanBegDate: Date, bFreeze: boolean = false): number {
    const listRouteRes = this.schProductRouteResList.filter(
      (p) => p.cSelected === '1',
    )
    if (
      this.iSchSdID === SchParam.iSchSdID &&
      this.iProcessProductID === SchParam.iProcessProductID
    ) {
      let j: number
    }
    const iRouteResCountFirst = listRouteRes.length
    if (iRouteResCountFirst < 1) {
      throw new Error(
        `多资源选择正排出错,订单行号：${this.iSchSdID}，没有找到已选择的可排产资源，资源产能明细总资源数量为${this.schProductRouteResList.length},位置SchProductRoute.ProcessSchTask！工序ID号：${this.cInvCode}`,
      )
      return -1
    }
    if (bFreeze || this.iActQty > 0) {
      if (this.iReqQty > 0) {
        listRouteRes.forEach((res) => {
          if (res.iResReqQty === 0) {
            res.bScheduled = 1
            res.cCanScheduled = '0'
          }
        })
      }
      return 1
    } else {
      if (listRouteRes.length > 1) {
        if (
          this.schProductRoutePreList.length === 1 &&
          this.schProductRoutePreList[0].cInvCode === this.cInvCode &&
          this.schProductRoutePreList[0].schProductRouteResList[0].resource
            .cDayPlanShowType !== ''
        ) {
          const schProductRouteRes = this.schProductRoutePreList[0].schProductRouteResList.find(
            (item) =>
              item.bScheduled === 1 &&
              item.iResReqQty > 0 &&
              item.resource.cDayPlanShowType !== '',
          )
          if (schProductRouteRes) {
            this.schProductRouteResList.forEach((item) => {
              if (
                item.resource.cDayPlanShowType &&
                !item.resource.cDayPlanShowType.includes(
                  schProductRouteRes.resource.cDayPlanShowType,
                ) &&
                item.cSelected === '1'
              ) {
                item.cSelected = '0'
                item.iResReqQty = 0
                item.cDefine25 = `关联分组号不同取消:${schProductRouteRes.resource.cDayPlanShowType}`
              }
            })
          }
          const listRouteRes2 = this.schProductRouteResList.filter(
            (p) => p.cSelected === '1' && p.iResReqQty > 0,
          )
          if (listRouteRes2.length < 1) {
            listRouteRes[0].cSelected = '1'
            listRouteRes[0].iResReqQty = this.iReqQty
          }
        }
      }
    }

    const updatedListRouteRes = this.schProductRouteResList.filter(
      (p) => p.cSelected === '1',
    )
    let iResCount = updatedListRouteRes.length

    if (iResCount <= 1) return 1
    if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1)
      iResCount = this.iDevCountPd

    updatedListRouteRes.sort(
      (p1, p2) => p1.iCapacity / p1.iBatchQty - p2.iCapacity / p2.iBatchQty,
    )

    let iCapacity = updatedListRouteRes[0].iCapacity
    let iBatchQty = updatedListRouteRes[0].iBatchQty
    let iMinResCount = 1

    if (updatedListRouteRes.length > 1) {
      if (
        this.schProduct.iWorkQtyPd > 0 &&
        this.schProduct.iWorkQtyPd < this.iReqQty
      ) {
        let iDevCount =
          (this.schProduct.iWorkQtyPd * iCapacity) /
          3600 /
          updatedListRouteRes[0].resource.iResHoursPd /
          updatedListRouteRes[0].resource.iResourceNumber /
          updatedListRouteRes[0].iBatchQty

        if (this.item?.iItemDifficulty && this.item.iItemDifficulty !== 1)
          iDevCount *= this.item.iItemDifficulty
        if (
          updatedListRouteRes[0].resource.iResDifficulty &&
          updatedListRouteRes[0].resource.iResDifficulty !== 1
        )
          iDevCount *= updatedListRouteRes[0].resource.iResDifficulty
        if (
          this.techInfo.iTechDifficulty &&
          this.techInfo.iTechDifficulty !== 1
        )
          iDevCount *= this.techInfo.iTechDifficulty

        iMinResCount = Math.floor(iDevCount)
        iResCount = iMinResCount
      } else {
        if (updatedListRouteRes[0].resource.iMinWorkTime > 0) {
          iMinResCount = Math.floor(
            (this.iReqQty * iCapacity) /
              iBatchQty /
              3600 /
              updatedListRouteRes[0].resource.iMinWorkTime,
          )
        } else {
          iMinResCount = iResCount
        }
      }
    }

    if (
      iResCount > iMinResCount &&
      iMinResCount <= updatedListRouteRes.length
    ) {
      iResCount = iMinResCount
    }
    if (iResCount < 1 && updatedListRouteRes.length > 0) iResCount = 1
    if (this.item?.iMoldCount > 0 && iResCount > this.item.iMoldCount)
      iResCount = this.item.iMoldCount

    let iResReqQtyPer = this.iReqQty
    if (iResCount > 1) {
      iResReqQtyPer = Math.floor(this.iReqQty / iResCount)
    }

    let iLeftReqQty = this.iReqQty
    let iResReqQty = iResReqQtyPer

    if (SchParam.cMutResourceType !== '4') {
      try {
        updatedListRouteRes.forEach((res) => {
          if (res.cCanScheduled !== '1') return
          res.testResSchTask(iResReqQty, dCanBegDate)
        })
      } catch (error) {
        const errMsg = `资源选择正排出错,订单行号：${this.iSchSdID}资源正排计算时出错,位置SchProductRoute.ResourceSelect.TestResSchTask！工序ID号：${this.iProcessProductID}`
        throw new Error(
          SchParam.iSchSdID < 1 ? errMsg : `${errMsg}\n${error.message}`,
        )
      }
    }

    this.resReqQtyDispatch()
    return 1
  }

  public resReqQtyDispatch(): number {
    const listRouteRes = this.schProductRouteResList.filter(
      (p) => p.cSelected === '1' && p.cCanScheduled === '1',
    )
    const iResCount = listRouteRes.length

    if (iResCount <= 1) return 0

    const iResReqQtyPer = Math.floor((this.iReqQty - this.iActQty) / iResCount)
    listRouteRes.forEach((res, index) => {
      res.iResReqQty =
        index === iResCount - 1
          ? this.iReqQty - iResReqQtyPer * index
          : iResReqQtyPer
    })

    return 1
  }
  public resourceSelectRev(
    dCanBegDate: Date,
    bFreeze: boolean = false,
  ): number {
    const ListRouteRes = this.SchProductRouteResList.filter(
      (p) => p.cSelected === '1',
    )
    if (bFreeze || this.iActQty > 0) {
      ListRouteRes.forEach((res) => {
        if (res.iResReqQty === 0) {
          res.BScheduled = 1
          res.cCanScheduled = '0'
        }
      })
      return 1
    } else {
      if (ListRouteRes.length > 0) {
        if (
          this.SchProductRouteNextList.length === 1 &&
          this.SchProductRouteNextList[0].cInvCode === this.cInvCode
        ) {
          const schProductRouteRes = this.SchProductRouteNextList[0].SchProductRouteResList.find(
            (item) =>
              item.BScheduled === 1 &&
              item.iResReqQty > 0 &&
              item.resource.cDayPlanShowType !== '',
          )
          if (schProductRouteRes) {
            this.SchProductRouteResList.forEach((item) => {
              if (
                item.resource.cDayPlanShowType !== '' &&
                !(
                  schProductRouteRes.resource.cDayPlanShowType.includes(
                    item.resource.cDayPlanShowType,
                  ) ||
                  item.resource.cDayPlanShowType.includes(
                    schProductRouteRes.resource.cDayPlanShowType,
                  )
                ) &&
                item.cSelected === '1'
              ) {
                item.cSelected = '0'
                item.iResReqQty = 0
                item.cDefine25 = `关联分组号不同取消:${schProductRouteRes.resource.cDayPlanShowType}`
              }
            })
            const ListRouteRes2 = this.SchProductRouteResList.filter(
              (p) => p.cSelected === '1' && p.iResReqQty > 0,
            )
            if (ListRouteRes2.length < 1) {
              ListRouteRes[0].cSelected = '1'
              ListRouteRes[0].iResReqQty = this.iReqQty
            }
          }
        }
      }
    }

    if (
      this.iSchSdID === SchParam.iSchSdID &&
      this.iProcessProductID === SchParam.iProcessProductID
    ) {
      let j = 0 // Variable is not used, preserved for compatibility
    }

    let iResCount = ListRouteRes.length
    if (iResCount <= 1) return 1

    if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1) {
      iResCount = this.iDevCountPd
    }

    ListRouteRes.sort((p1, p2) => p1.iCapacity - p2.iCapacity)
    const iCapacity = ListRouteRes[0].iCapacity

    // Handle additional logic here for iMinResCount, iResReqQty, and sorting resources
    //...

    this.resReqQtyDispatch()
    return 1
  }

  public cBatchResourceSelect(): number {
    this.cBatchNoFlag = 1

    const ListBatchRoute = schData.SchProductRouteList.filter(
      (p) =>
        p.cVersionNo === this.cVersionNo &&
        p.schProduct.cBatchNo === this.schProduct.cBatchNo &&
        p.schProduct.cWorkRouteType === this.schProduct.cWorkRouteType &&
        p.iWoSeqID === this.iWoSeqID &&
        p.cBatchNoFlag === 0 &&
        p.iSchBatch === this.iSchBatch,
    )

    if (ListBatchRoute.length < 1) return 0

    ListBatchRoute.forEach((batchRoute) => {
      batchRoute.SchProductRouteResList.forEach((res) => {
        if (res.resource.cIsInfinityAbility === '0') {
          const BatchRouteRes = this.SchProductRouteResList.find(
            (p) =>
              p.cVersionNo.trim() === batchRoute.cVersionNo &&
              p.cResourceNo === res.cResourceNo &&
              p.iWoSeqID === batchRoute.iWoSeqID,
          )
          if (BatchRouteRes && BatchRouteRes.iResReqQty > 0) {
            res.cSelected = '1'
            res.iResReqQty = batchRoute.iReqQty
          } else {
            res.cSelected = '0'
            res.iResReqQty = 0
          }
        }
        batchRoute.cBatchNoFlag = 1
      })
    })

    return 1
  }
  public resourceSelectRev(
    dCanBegDate: Date,
    bFreeze: boolean = false,
  ): number {
    const ListRouteRes = this.SchProductRouteResList.filter(
      (p) => p.cSelected === '1',
    )
    if (bFreeze || this.iActQty > 0) {
      ListRouteRes.forEach((res) => {
        if (res.iResReqQty === 0) {
          res.BScheduled = 1
          res.cCanScheduled = '0'
        }
      })
      return 1
    } else {
      if (ListRouteRes.length > 0) {
        if (
          this.SchProductRouteNextList.length === 1 &&
          this.SchProductRouteNextList[0].cInvCode === this.cInvCode
        ) {
          const schProductRouteRes = this.SchProductRouteNextList[0].SchProductRouteResList.find(
            (item) =>
              item.BScheduled === 1 &&
              item.iResReqQty > 0 &&
              item.resource.cDayPlanShowType !== '',
          )
          if (schProductRouteRes) {
            this.SchProductRouteResList.forEach((item) => {
              if (
                item.resource.cDayPlanShowType !== '' &&
                !(
                  schProductRouteRes.resource.cDayPlanShowType.includes(
                    item.resource.cDayPlanShowType,
                  ) ||
                  item.resource.cDayPlanShowType.includes(
                    schProductRouteRes.resource.cDayPlanShowType,
                  )
                ) &&
                item.cSelected === '1'
              ) {
                item.cSelected = '0'
                item.iResReqQty = 0
                item.cDefine25 = `关联分组号不同取消:${schProductRouteRes.resource.cDayPlanShowType}`
              }
            })
            const ListRouteRes2 = this.SchProductRouteResList.filter(
              (p) => p.cSelected === '1' && p.iResReqQty > 0,
            )
            if (ListRouteRes2.length < 1) {
              ListRouteRes[0].cSelected = '1'
              ListRouteRes[0].iResReqQty = this.iReqQty
            }
          }
        }
      }
    }

    if (
      this.iSchSdID === SchParam.iSchSdID &&
      this.iProcessProductID === SchParam.iProcessProductID
    ) {
      let j = 0 // Variable is not used, preserved for compatibility
    }

    let iResCount = ListRouteRes.length
    if (iResCount <= 1) return 1

    if (iResCount > this.iDevCountPd && this.iDevCountPd >= 1) {
      iResCount = this.iDevCountPd
    }

    ListRouteRes.sort((p1, p2) => p1.iCapacity - p2.iCapacity)
    const iCapacity = ListRouteRes[0].iCapacity

    // Handle additional logic here for iMinResCount, iResReqQty, and sorting resources
    //...

    this.resReqQtyDispatch()
    return 1
  }

  public cBatchResourceSelect(): number {
    this.cBatchNoFlag = 1

    const ListBatchRoute = schData.SchProductRouteList.filter(
      (p) =>
        p.cVersionNo === this.cVersionNo &&
        p.schProduct.cBatchNo === this.schProduct.cBatchNo &&
        p.schProduct.cWorkRouteType === this.schProduct.cWorkRouteType &&
        p.iWoSeqID === this.iWoSeqID &&
        p.cBatchNoFlag === 0 &&
        p.iSchBatch === this.iSchBatch,
    )

    if (ListBatchRoute.length < 1) return 0

    ListBatchRoute.forEach((batchRoute) => {
      batchRoute.SchProductRouteResList.forEach((res) => {
        if (res.resource.cIsInfinityAbility === '0') {
          const BatchRouteRes = this.SchProductRouteResList.find(
            (p) =>
              p.cVersionNo.trim() === batchRoute.cVersionNo &&
              p.cResourceNo === res.cResourceNo &&
              p.iWoSeqID === batchRoute.iWoSeqID,
          )
          if (BatchRouteRes && BatchRouteRes.iResReqQty > 0) {
            res.cSelected = '1'
            res.iResReqQty = batchRoute.iReqQty
          } else {
            res.cSelected = '0'
            res.iResReqQty = 0
          }
        }
        batchRoute.cBatchNoFlag = 1
      })
    })

    return 1
  }

  ProcessSchTaskPre(
    bCurTask: boolean = true,
    bFreeze: boolean = false,
  ): number {
    try {
      this.SchProductRoutePreList.forEach((schProductRoute) => {
        if (schProductRoute.bScheduled === 0) {
          schProductRoute.ProcessSchTaskPre(bCurTask, bFreeze)
        }
      })

      if (bCurTask) {
        SchProductRoute.SchParam.ldtBeginDate = new Date()
        this.ProcessSchTask(bFreeze)
        SchProductRoute.SchParam.iWaitTime =
          new Date().getTime() - SchProductRoute.SchParam.ldtBeginDate.getTime()
      }
    } catch (error) {
      const errorMessage = `订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message}`
      if (SchProductRoute.SchParam.iSchSdID < 1) {
        throw new Error(errorMessage)
      } else {
        throw new Error(`${errorMessage}明细信息:${error.stack}`)
      }
    }
    return 1
  }

  ProcessSchTaskNext(cTag: string = '1'): number {
    try {
      if (this.bScheduled === 0) {
        this.ProcessSchTask()
      }

      if (this.SchProductRouteNextList.length < 1) return 1

      const schProductRouteNext = this.SchProductRouteNextList[0]

      if (!schProductRouteNext) {
        throw new Error(
          `订单行号：${this.iSchSdID}请检查产品[${this.cInvCode}]加工物料[${this.iWoSeqID}]工序号[${this.iWoSeqID}]工艺路线是否完整!`,
        )
      }

      if (cTag === '1') {
        if (schProductRouteNext.cInvCode === this.cInvCode) {
          schProductRouteNext.ProcessSchTaskNext('1')
        }
      } else if (cTag === '2') {
        if (
          schProductRouteNext.cInvCode === this.cInvCode &&
          schProductRouteNext.iWoSeqID < 50
        ) {
          schProductRouteNext.ProcessSchTaskNext('2')
        }
      } else if (cTag === '4') {
        if (schProductRouteNext.bScheduled === 0) {
          schProductRouteNext.ProcessSchTask()
        } else {
          schProductRouteNext.ProcessSchTaskNext('4')
        }
      }
    } catch (error) {
      const errorMessage = `订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message}`
      if (SchProductRoute.SchParam.iSchSdID < 1) {
        throw new Error(errorMessage)
      } else {
        throw new Error(`${errorMessage}明细信息:${error.stack}`)
      }
    }
    return 1
  }
  ProcessSchTask(bFreeze: boolean = false): void {
    // Implementation required
  }

  ProcessSchTaskPre(
    bCurTask: boolean = true,
    bFreeze: boolean = false,
  ): number {
    try {
      this.SchProductRoutePreList.forEach((schProductRoute) => {
        if (schProductRoute.bScheduled === 0) {
          schProductRoute.ProcessSchTaskPre(bCurTask, bFreeze)
        }
      })

      if (bCurTask) {
        SchProductRoute.SchParam.ldtBeginDate = new Date()
        this.ProcessSchTask(bFreeze)
        SchProductRoute.SchParam.iWaitTime =
          new Date().getTime() - SchProductRoute.SchParam.ldtBeginDate.getTime()
      }
    } catch (error) {
      const errorMessage = `订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message}`
      if (SchProductRoute.SchParam.iSchSdID < 1) {
        throw new Error(errorMessage)
      } else {
        throw new Error(`${errorMessage}明细信息:${error.stack}`)
      }
    }
    return 1
  }

  ProcessSchTaskNext(cTag: string = '1'): number {
    try {
      if (this.bScheduled === 0) {
        this.ProcessSchTask()
      }

      if (this.SchProductRouteNextList.length < 1) return 1

      const schProductRouteNext = this.SchProductRouteNextList[0]

      if (!schProductRouteNext) {
        throw new Error(
          `订单行号：${this.iSchSdID}请检查产品[${this.cInvCode}]加工物料[${this.iWoSeqID}]工序号[${this.iWoSeqID}]工艺路线是否完整!`,
        )
      }

      if (cTag === '1') {
        if (schProductRouteNext.cInvCode === this.cInvCode) {
          schProductRouteNext.ProcessSchTaskNext('1')
        }
      } else if (cTag === '2') {
        if (
          schProductRouteNext.cInvCode === this.cInvCode &&
          schProductRouteNext.iWoSeqID < 50
        ) {
          schProductRouteNext.ProcessSchTaskNext('2')
        }
      } else if (cTag === '4') {
        if (schProductRouteNext.bScheduled === 0) {
          schProductRouteNext.ProcessSchTask()
        } else {
          schProductRouteNext.ProcessSchTaskNext('4')
        }
      }
    } catch (error) {
      const errorMessage = `订单行号：${this.iSchSdID}资源倒排计算时出错,位置SchProductRoute.ProcessSchTaskNext！工序ID号：${this.iProcessProductID}\n\r ${error.message}`
      if (SchProductRoute.SchParam.iSchSdID < 1) {
        throw new Error(errorMessage)
      } else {
        throw new Error(`${errorMessage}明细信息:${error.stack}`)
      }
    }
    return 1
  }
  public ProcessSchTaskRevPre(cTag: string = '1', bSet: string = '0'): number {
    if (cTag === '2') {
      if (this.iWoSeqID < 10 && this.bScheduled === 1) return 0
    }

    if (this.iReqQty <= 0) {
      this.bScheduled = 1
      return 0
    }

    this.ProcessSchTaskRev(bSet)

    for (const schProductRoute of this.SchProductRoutePreList) {
      if (cTag === '1') {
        if (schProductRoute.cInvCode !== this.cInvCode) continue
        schProductRoute.ProcessSchTaskRevPre('1')
      } else if (cTag === '2') {
        if (schProductRoute.iWoSeqID < 50) continue
        schProductRoute.ProcessSchTaskRevPre('2')
      } else if (cTag === '3') {
        schProductRoute.ProcessSchTaskRevPre('3')
      }
    }

    return 1
  }

  public ProcessSchTaskRev(bSet: string = '0'): number {
    let dDateTemp: Date = this.schData.dtStart
    let dCanEndDate: Date = this.schData.dtStart

    try {
      if (this.SchProductRouteNextList.length > 0) {
        for (const schProductRoute of this.SchProductRouteNextList) {
          dDateTemp = schProductRoute.GetPreProcessCanEndDate(this)
          if (dCanEndDate < dDateTemp) dCanEndDate = dDateTemp
        }

        if (bSet === '1' && dCanEndDate <= this.dEndDate) {
          this.ProcessSchTask()
          return 1
        }

        if (dCanEndDate < this.schData.dtStart) {
          throw new Error(
            `Order Line No: ${this.iSchSdID} Error in scheduling process during reverse calculation. Start time ${dCanEndDate} is earlier than scheduling start date ${this.schData.dtStart}.`,
          )
        }
      } else {
        dCanEndDate = this.schProduct.dDeliveryDate
        if (dCanEndDate <= this.schData.dtStart) {
          dCanEndDate = new Date(this.schData.dtStart)
          dCanEndDate.setMonth(dCanEndDate.getMonth() + 1)
        }
      }

      if (this.bScheduled === 1) this.ProcessClearTask()
      this.ResourceSelectRev(dCanEndDate)

      const availableResources = this.SchProductRouteResList.filter(
        (res) => res.cSelected === '1' && res.cCanScheduled === '1',
      )

      if (availableResources.length < 1) {
        const resWithoutSchedule = this.SchProductRouteResList.filter(
          (res) => res.cSelected === '1' && res.cCanScheduled === '0',
        )

        if (resWithoutSchedule.length < 1) {
          throw new Error(
            `Order Line No: ${this.iSchSdID} No schedulable resources available.`,
          )
        } else {
          for (const res of resWithoutSchedule) {
            res.BScheduled = 1
          }
          return 1
        }
      }

      // Further task assignment logic here...

      return 1
    } catch (error) {
      throw new Error(
        `Order Line No: ${this.iSchSdID} Reverse scheduling error: ${error.message}`,
      )
    }
  }

  public GetPreProcessCanEndDate(schProductRoute: SchProductRoute): Date {
    // Placeholder implementation
    return new Date()
  }
  public GetProcessMoveTime(schProductRoutePre: SchProductRoute): number {
    let iMoveTime = 0
    let liCapacity = 1 // 产能
    if (schProductRoutePre.SchProductRouteResList.length > 0) {
      liCapacity = schProductRoutePre.SchProductRouteResList[0].iCapacity
    }

    if (schProductRoutePre.cMoveType === '1') {
      iMoveTime =
        schProductRoutePre.iMoveInterTime + schProductRoutePre.iMoveTime
    } else if (schProductRoutePre.cMoveType === '2') {
      iMoveTime =
        (schProductRoutePre.iMoveInterQty * liCapacity) / 60 +
        schProductRoutePre.iMoveTime
    } else {
      iMoveTime = schProductRoutePre.iMoveTime
    }

    return iMoveTime
  }

  public GetProcessMoveQty(schProductRoutePre: SchProductRoute): number {
    let iMoveTime = 0
    let liCapacity = 1
    let ibatchqty = 1

    if (schProductRoutePre.SchProductRouteResList.length > 0) {
      liCapacity = schProductRoutePre.SchProductRouteResList[0].iCapacity
      ibatchqty = schProductRoutePre.SchProductRouteResList[0].iBatchQty

      if (liCapacity === 0) liCapacity = 1

      if (schProductRoutePre.cMoveType === '1') {
        iMoveTime = schProductRoutePre.iMoveInterTime
      } else if (schProductRoutePre.cMoveType === '2') {
        iMoveTime =
          (schProductRoutePre.iMoveInterQty * liCapacity) / ibatchqty / 60
      }
    }

    return iMoveTime
  }

  public GetPreProcessCanEndDate(
    schProductRoutePre: SchProductRoute,
    cType: string = '1',
  ): Date {
    let ldtFirstEndDate: Date
    const iMoveTime = this.GetProcessMoveQty(schProductRoutePre)
    ldtFirstEndDate = new Date(schProductRoutePre.dEndDate.getTime())
    ldtFirstEndDate.setMinutes(
      ldtFirstEndDate.getMinutes() - iMoveTime - schProductRoutePre.iSeqPreTime,
    )

    if (schProductRoutePre.iSeqPostTime) {
      ldtFirstEndDate.setMinutes(
        ldtFirstEndDate.getMinutes() - schProductRoutePre.iSeqPostTime,
      )
      schProductRoutePre.cDefine28 = `;本工序后准备时间${schProductRoutePre.iSeqPostTime};本工序可开工时间${ldtFirstEndDate}`
    }

    return ldtFirstEndDate
  }

  public ProcessClearTask(schProductRouteResList: SchProductRouteRes[]): void {
    for (const res of schProductRouteResList) {
      res.TaskClearTask()
    }
  }

  public GetRouteEarlyBegDate(
    schProductRoute: SchProductRoute,
    SchParam: { dtStart: Date; iSchSdID: number },
  ): void {
    let dDateTemp = SchParam.dtStart

    try {
      if (!schProductRoute.schData) return

      schProductRoute.cDefine27 = `dEarlyBegDate:${schProductRoute.dEarlyBegDate}`
      if (
        schProductRoute.SchProductRoutePreList &&
        schProductRoute.SchProductRoutePreList.length > 0
      ) {
        for (const preRoute of schProductRoute.SchProductRoutePreList) {
          if (!preRoute.bScheduled) {
            // 调用其他方法进行调度
          }

          dDateTemp = new Date(preRoute.dEndDate) // 假设这里是某种日期获取逻辑
          if (
            schProductRoute.cStatus !== '4' &&
            schProductRoute.dEarlyBegDate < dDateTemp
          ) {
            schProductRoute.dEarlyBegDate = dDateTemp
            schProductRoute.cDefine27 += `;前工序${preRoute.iProcessProductID}:${dDateTemp}`
          }
        }
      }
    } catch (error) {
      throw new Error(`资源倒排计算时出错: ${error.message}`)
    }
  }
}
