import { DateTime, SchData, SchProduct,SchProductRoute} from './type';

export class SchProductRouteItem  {
    schData: SchData | null = null;
    bScheduled: number = 0;
    cVersionNo: string;
    iSchSdID: number;
    iProcessProductID: number;
    iEntryID: number;
    cWoNo: string;
    iItemID: number;
    cInvCode: string;
    cInvCodeFull: string;
    cSubInvCode: string;
    cSubInvCodeFull: string;
    bSelf: string;
    iWoSeqID: number;
    cUtterType: string;
    cSubRelate: string;
    iQtyPer: number;
    iScrapt: number;
    iReqQty: number;
    iNormalQty: number;
    iScrapQty: number;
    iProQty: number;
    iKeepQty: number;
    iPlanQty: number;
    dReqDate: DateTime;
    dForeInDate: DateTime;
    dEarlySubItemDate: DateTime;
    iAdvanceDate: number;
    schProduct: SchProduct;
    schProductRoute: SchProductRoute;
    iSchBatch: number = 6;

    getObjectData(info: any, context: any): void {
        throw new Error("Method not implemented.");
    }
}

