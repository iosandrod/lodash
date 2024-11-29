import { Base } from './Base'
import { gsDecoration } from './Util'
import { WorkCenter } from './WorkCenter'
type Context = { name: string }
export class Resource extends Base {
  iResourceID: number = 0
  cResourceNo: string = ''
  cResourceName: string = ''
  cResClsNo: string = ''
  cResourceType: string = '' // 0 主资源, 1 辅资源
  @gsDecoration({
    get: function () {
      let _this: Resource = this
      let SchParam = _this.schParam
      if (SchParam.cSchCapType === '1') {
        return Math.max(this.IResourceNumber, this.iOverResourceNumber)
      } else if (SchParam.cSchCapType === '2') {
        return Math.max(this.IResourceNumber, this.iLimitResourceNumber)
      }
      return _this.IResourceNumber
    },
  })
  IResourceNumber: number = 0
  cResOccupyType: string = '' // 0 整体, 1 单人单台
  iPreStocks: number = 0
  iPostStocks: number = 0
  iUsage: number = 0
  IEfficient: number = 0
  cResouceInformation: string = ''
  cIsInfinityAbility: string = '' // 0 产能有限, 1 产能无限
  bScheduled: number = 0
  iOverResourceNumber: number = 0
  iLimitResourceNumber: number = 0
  iOverEfficient: number = 0
  iLimitEfficient: number = 0
  iResDifficulty: number = 0
  iDistributionRate: number = 0
  cWcNo: string = ''
  @gsDecoration({})
  @gsDecoration({
    get: function (this: Resource) {
      let _this = this
      let cWcNo = _this.cWcNo
      let schData =
        _this.schData?.WorkCenterList.filter((wc) => {
          return wc.cWcNo === cWcNo //
        }) || []
      let tw = schData[0] || null
      return tw //
    }, ////
  })
  resWorkCenter: WorkCenter | null = null //
  getResWorkCenter: () => WorkCenter
  //   get ResWorkCenter(): WorkCenter | null {
  //     if (!this.cWcNo || !this.schData) {
  //       return null
  //     }

  //     const workCenters = this.schData.WorkCenterList.filter(
  //       (wc) => wc.cWcNo === this.cWcNo,
  //     )

  //     return workCenters.length > 0 ? workCenters[0] : null
  //   }
  //   get iResourceNumber(): number {
  //     if (SchParam.cSchCapType === '1') {
  //       return Math.max(this.IResourceNumber, this.iOverResourceNumber)
  //     } else if (SchParam.cSchCapType === '2') {
  //       return Math.max(this.IResourceNumber, this.iLimitResourceNumber)
  //     }
  //     return this.IResourceNumber
  //   }

  //   set iResourceNumber(value: number) {
  //     this.IResourceNumber = value
  //   }
  @gsDecoration({
    get: function () {
      //   let _this: Resource = this
      //   if (this.IEfficient <= 0) {
      //     this.IEfficient = 100
      //   }
      //   if (SchParam.cSchCapType === '1') {
      //     return Math.max(this.IEfficient, this.iOverEfficient)
      //   } else if (SchParam.cSchCapType === '2') {
      //     return Math.max(this.IEfficient, this.iLimitEfficient)
      //   }
      //   return this.IEfficient
    },
  })
  iEfficient: number
  getIEfficient: () => number
  setIDfficient: (...args) => void //
  //   get iEfficient(): number {
  //     if (this.IEfficient <= 0) {
  //       this.IEfficient = 100
  //     }
  //     if (SchParam.cSchCapType === '1') {
  //       return Math.max(this.IEfficient, this.iOverEfficient)
  //     } else if (SchParam.cSchCapType === '2') {
  //       return Math.max(this.IEfficient, this.iLimitEfficient)
  //     }
  //     return this.IEfficient
  //   }

  //   set iEfficient(value: number) {
  //     this.IEfficient = value
  //   }

  // constructor();
  // constructor(drResource: DataRow);
  // constructor(cResourceNo: string, schData: SchData);
  // constructor(arg1?: any, arg2?: SchData) {
  //     if (arg1 && typeof arg1 === 'object') {
  //         this.GetResource(arg1);
  //     } else if (typeof arg1 === 'string' && arg2) {
  //         this.schData = arg2;
  //         const rows = this.schData.dtResource.select(
  //             `cResourceNo = '${arg1}'`
  //         );
  //         if (rows.length > 0) {
  //             this.GetResource(rows[0]);
  //         }
  //     }
  // }

  // GetResource(drResource: DataRow): void {
  //     this.iResourceID = drResource['iResourceID'];
  //     this.cResourceNo = drResource['cResourceNo'];
  //     this.cResourceName = drResource['cResourceName'];
  //     this.cResClsNo = drResource['cResClsNo'];
  //     this.cResourceType = drResource['cResourceType'];
  //     this.IResourceNumber = drResource['iResourceNumber'];
  //     this.cResOccupyType = drResource['cResOccupyType'];
  //     this.iPreStocks = drResource['iPreStocks'];
  //     this.iPostStocks = drResource['iPostStocks'];
  //     this.iUsage = drResource['iUsage'];
  //     this.cResouceInformation = drResource['cResouceInformation'];
  //     this.cIsInfinityAbility = drResource['cIsInfinityAbility'];
  //     this.bScheduled = drResource['bScheduled'];
  //     this.iOverResourceNumber = drResource['iOverResourceNumber'];
  //     this.iLimitResourceNumber = drResource['iLimitResourceNumber'];
  //     this.iOverEfficient = drResource['iOverEfficient'];
  //     this.iLimitEfficient = drResource['iLimitEfficient'];
  //     this.iResDifficulty = drResource['iResDifficulty'];
  //     this.iDistributionRate = drResource['iDistributionRate'];
  //     this.cWcNo = drResource['cWcNo'];
  // }
}

// 示例全局对象
