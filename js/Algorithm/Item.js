"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Item = void 0;
class Item {
    constructor(cInvCode, as_SchData) {
        this.schData = null; // All scheduling data
        if (as_SchData) {
            this.schData = as_SchData;
            const dr = this.schData.dtItem.Select(`cInvCode = '${cInvCode}'`);
            if (dr.length < 1)
                return;
            this.getItem(dr[0]);
        }
    }
    getItem(drItem) {
        try {
            this.iItemID = Number(drItem['iItemID']); // Material internal code
            this.cInvCode = String(drItem['cInvCode']); // Material code
            this.cInvName = String(drItem['cInvName']); // Material name
            this.cInvStd = String(drItem['cInvStd']); // Material category
            this.cItemClsNo = String(drItem['cItemClsNo']); // Category
            this.cVenCode = String(drItem['cVenCode']); // Supplier code
            this.bSale = String(drItem['bSale']); // Saleable
            this.bPurchase = String(drItem['bPurchase']); // Purchasable
            this.bSelf = String(drItem['bSelf']); // Self-made
            this.bProxyForeign = String(drItem['bProxyForeign']); // Outsourced
            this.cComUnitCode = String(drItem['cComUnitCode']); // Unit code
            this.iSafeStock = Number(drItem['iSafeStock']); // Safety stock
            this.iTopLot = Number(drItem['iTopLot']); // High warning stock
            this.iLowLot = Number(drItem['iLowLot']); // Low warning stock
            this.iIncLot = Number(drItem['iIncLot']); // Batch increment
            this.iAvgLot = Number(drItem['iAvgLot']); // Average batch
            this.cLeadTimeType = String(drItem['cLeadTimeType']); // Lead time type
            this.iAvgLeadTime = Number(drItem['iAvgLeadTime']); // Processing lead time
            this.iAdvanceDate = Number(drItem['iAdvanceDate']); // Purchase lead time
            this.cInjectItemType = String(drItem['cInjectItemType']); // Injection type
            this.cMoldNo = String(drItem['cMoldNo']); // Mold number
            this.cSubMoldNo = String(drItem['cSubMoldNo']); // Sub-mold number
            this.cMoldPosition = String(drItem['cMoldPosition']); // Mold position
            this.iMoldSubQty = Number(drItem['iMoldSubQty']); // Mold cavity count
            this.iMoldCount = Number(drItem['iMoldCount']); // Mold set count
            this.cWcNo = String(drItem['cWcNo']); // Work center
            this.cTechNo = String(drItem['cTechNo']); // Department
            this.cRouteCode = String(drItem['cRouteCode']); // Capability type
            this.cKeyResourceNo = String(drItem['cKeyResourceNo']); // Department
            this.iProSec = Number(drItem['iProSec']); // Capability type
            this.iItemDifficulty = Number(drItem['iItemDifficulty']); // Department
            this.cMaterial = String(drItem['cMaterial']); // Material
            this.cColor = String(drItem['cColor']); // Color
            this.fVolume = Number(drItem['fVolume']); // Volume
            this.flength = Number(drItem['flength']); // Length
            this.fWidth = Number(drItem['fWidth']); // Width
            this.fHeight = Number(drItem['fHeight']); // Height
            this.fNetWeight = Number(drItem['fNetWeight']); // Net weight
            this.cSize1 = String(drItem['cSize1']); // Custom item 1
            this.cSize2 = String(drItem['cSize2']); // Custom item 2
            this.cSize3 = String(drItem['cSize3']); // Custom item 3
            this.cSize4 = String(drItem['cSize4']); // Custom item 4
            this.cSize5 = String(drItem['cSize5']); // Custom item 5
            this.cSize6 = String(drItem['cSize6']); // Custom item 6
            this.cSize7 = String(drItem['cSize7']); // Custom item 7
            this.cSize8 = String(drItem['cSize8']); // Custom item 8
            this.cSize9 = String(drItem['cSize9']); // Custom item 9
            this.cSize10 = String(drItem['cSize10']); // Custom item 10
            this.cSize11 = Number(drItem['cSize11']); // Custom item 11
            this.cSize12 = Number(drItem['cSize12']); // Custom item 12
            this.cSize13 = Number(drItem['cSize13']); // Custom item 13
            this.cSize14 = Number(drItem['cSize14']); // Custom item 14
            this.cSize15 = new Date(drItem['cSize15']); // Custom item 15
            this.cSize16 = new Date(drItem['cSize16']); // Custom item 16
        }
        catch (exp) {
            throw exp;
        }
    }
}
exports.Item = Item;
