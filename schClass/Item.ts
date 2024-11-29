class SchData {
	dtItem: { [key: string]: any }[] = []; // 模拟DataTable
}

class Item {
	schData: SchData | null = null;

	iItemID!: number;
	cInvCode!: string;
	cInvName!: string;
	cInvStd!: string;
	cItemClsNo!: string;
	cVenCode!: string;
	bSale!: string;
	bPurchase!: string;
	bSelf!: string;
	bProxyForeign!: string;
	cComUnitCode!: string;
	iSafeStock!: number;
	iTopLot!: number;
	iLowLot!: number;
	iIncLot!: number;
	iAvgLot!: number;
	cLeadTimeType!: string;
	iAvgLeadTime!: number;
	iAdvanceDate!: number;
	cInjectItemType!: string;
	cMoldNo!: string;
	cSubMoldNo!: string;
	cMoldPosition!: string;
	iMoldSubQty!: number;
	iMoldCount!: number;
	cWcNo!: string;
	cTechNo!: string;
	cRouteCode!: string;
	cKeyResourceNo!: string;
	iProSec!: number;
	iItemDifficulty!: number;
	cMaterial!: string;
	cColor!: string;
	fVolume!: number;
	flength!: number;
	fWidth!: number;
	fHeight!: number;
	fNetWeight!: number;
	cSize1!: string;
	cSize2!: string;
	cSize3!: string;
	cSize4!: string;
	cSize5!: string;
	cSize6!: string;
	cSize7!: string;
	cSize8!: string;
	cSize9!: string;
	cSize10!: string;
	cSize11!: number;
	cSize12!: number;
	cSize13!: number;
	cSize14!: number;

	constructor();
	constructor(cInvCode: string, schData: SchData);
	constructor(cInvCode?: string, schData?: SchData) {
		if (cInvCode && schData) {
			this.schData = schData;
			const dr = schData.dtItem.filter((item) => item.cInvCode === cInvCode);
			if (dr.length > 0) {
				this.getItem(dr[0]);
			}
		}
	}

	getItem(drItem: { [key: string]: any }): void {
		try {
			this.iItemID = drItem['iItemID'];
			this.cInvCode = drItem['cInvCode'];
			this.cInvName = drItem['cInvName'];
			this.cInvStd = drItem['cInvStd'];
			this.cItemClsNo = drItem['cItemClsNo'];
			this.cVenCode = drItem['cVenCode'];
			this.bSale = drItem['bSale'];
			this.bPurchase = drItem['bPurchase'];
			this.bSelf = drItem['bSelf'];
			this.bProxyForeign = drItem['bProxyForeign'];
			this.cComUnitCode = drItem['cComUnitCode'];
			this.iSafeStock = parseFloat(drItem['iSafeStock']);
			this.iTopLot = parseFloat(drItem['iTopLot']);
			this.iLowLot = parseFloat(drItem['iLowLot']);
			this.iIncLot = parseFloat(drItem['iIncLot']);
			this.iAvgLot = parseFloat(drItem['iAvgLot']);
			this.cLeadTimeType = drItem['cLeadTimeType'];
			this.iAvgLeadTime = parseFloat(drItem['iAvgLeadTime']);
			this.iAdvanceDate = parseFloat(drItem['iAdvanceDate']);
			this.cInjectItemType = drItem['cInjectItemType'];
			this.cMoldNo = drItem['cMoldNo'];
			this.cSubMoldNo = drItem['cSubMoldNo'];
			this.cMoldPosition = drItem['cMoldPosition'];
			this.iMoldSubQty = parseFloat(drItem['iMoldSubQty']);
			this.iMoldCount = parseInt(drItem['iMoldCount'], 10);
			this.cWcNo = drItem['cWcNo'];
			this.cTechNo = drItem['cTechNo'];
			this.cRouteCode = drItem['cRouteCode'];
			this.cKeyResourceNo = drItem['cKeyResourceNo'];
			this.iProSec = parseFloat(drItem['iProSec']);
			this.iItemDifficulty = parseFloat(drItem['iItemDifficulty']);
			this.cMaterial = drItem['cMaterial'];
			this.cColor = drItem['cColor'];
			this.fVolume = parseFloat(drItem['fVolume']);
			this.flength = parseFloat(drItem['flength']);
			this.fWidth = parseFloat(drItem['fWidth']);
			this.fHeight = parseFloat(drItem['fHeight']);
			this.fNetWeight = parseFloat(drItem['fNetWeight']);
			this.cSize1 = drItem['cSize1'];
			this.cSize2 = drItem['cSize2'];
			this.cSize3 = drItem['cSize3'];
			this.cSize4 = drItem['cSize4'];
			this.cSize5 = drItem['cSize5'];
			this.cSize6 = drItem['cSize6'];
			this.cSize7 = drItem['cSize7'];
			this.cSize8 = drItem['cSize8'];
			this.cSize9 = drItem['cSize9'];
			this.cSize10 = drItem['cSize10'];
			this.cSize11 = parseFloat(drItem['cSize11']);
			this.cSize12 = parseFloat(drItem['cSize12']);
			this.cSize13 = parseFloat(drItem['cSize13']);
			this.cSize14 = parseFloat(drItem['cSize14']);
		} catch (error) {
			console.error('Error setting item properties:', error);
		}
	}
}
