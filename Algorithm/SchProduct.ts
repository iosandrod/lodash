import { Base } from "./Base";
import { DateTime, SchProductRoute } from "./type";

export class SchProduct extends Base{
    schData: any = null; // 所有排程数据
    bScheduled: number = 0; // 产品排产状态
    iSchSdID: number;
    cVersionNo: string;
    iInterID: number;
    iSdLineID: number;
    iSeqID: number;
    iModelID: number;
    cSdOrderNo: string;
    cCustNo: string;
    cCustName: string;
    cSTCode: string;
    cBusType: string;
    cPriorityType: number;
    cStatus: string;
    cSales: string;
    cRequireType: string;
    iItemID: number;
    cInvCode: string;
    cInvName: string;
    cInvStd: string;
    cUnitCode: string;
    iReqQty: number;
    dRequireDate: DateTime; // 需求日期
    dDeliveryDate: DateTime; // 最早交货日期
    dEarliestSchDate: DateTime; // 最早可排日期
    dBegDate: DateTime; // 排程开工时间
    dEndDate: DateTime; // 排程完工时间
    cSchStatus: string;
    cMiNo: string;
    iPriority: number = -1; // 产品优先级
    cSelected: string;
    cWoNo: string;
    iPlanQty: number;
    cNeedSet: string;
    iFHQuantity: number;
    iKPQuantity: number;
    iSourceLineID: number;
    cColor: string;
    cNote: string;
    bSet: string = "0"; // 是否需配套 1 是，0 否
    iSchPriority: number = -1; // 产品排产优先级,以关键工序的优先级，决定产品排产优先级
    iSchBatch: number = 6; // 排产批次
    cType: string = ""; // 计划类型
    cSchType: string = "0"; // 排产类型 0 正排 1 倒排 2 无限产能倒排
    cPlanMode: string = "2"; // 计划类型 2 按订单生产 3 按库存生产 (暂不用)
    cScheduled: string = "0"; // 1 已排产确认，不能换资源 ; 0 未排产确认
    iAdvanceDate: number; // 累计提前期天数 (暂不用)
    cBatchNo: string = ""; // 托盘号，不为空时，按托盘排产，同一托盘物料工艺路线类型一样，同一工序必须选择同一资源排产2020-03-22。
    iSchSN: number = 0; // 排产座次顺序
    cSchSNType: string; // 排产座次
    cGroupSN: number = 0; // 分组号
    cGroupQty: number = 0; // 分组数量
    cCustomize: string = ""; // 是否定制，自动生成工艺路线的
    cWorkRouteType: string = ""; // 工艺路线类型
    cAttributes1: string = ""; // 加工属性1
    cAttributes2: string = ""; // 加工属性2
    cAttributes3: string = ""; // 加工属性3
    cAttributes4: string = ""; // 加工属性4
    cAttributes5: string = ""; // 加工属性5
    cAttributes6: string = ""; // 加工属性6
    cAttributes7: string = ""; // 加工属性7
    cAttributes8: string = ""; // 加工属性8
    cAttributes9: number = 0; // 加工属性9
    cAttributes10: number = 0; // 加工属性10
    cAttributes11: number = 0; // 加工属性11
    cAttributes12: number = 0; // 加工属性12
    cAttributes13: string = ""; // 加工属性13
    cAttributes14: string = ""; // 加工属性14
    cAttributes15: string = ""; // 加工属性15
    cAttributes16: string = ""; // 加工属性16
    iDeliveryDays: number; // 交货天数,最早可排日期
    iWorkQtyPd: number = 0; // 日排产数量 2019-03-24

    SchProductRouteList: SchProductRoute[] = new Array(10);

    // 正排，给产品工艺模型中的每个工序排程
    public ProductSchTask(): number {
        try {
            if (this.SchProductRouteList.length < 1) return 1; // 有些产品没有工序，跳过

            // 1、找出当前产品的最后一道工序，从此工序的前工序列表起，正排。
            let schProductRouteTemp = this.SchProductRouteList.filter(p1 => p1.iSchSdID === this.iSchSdID && p1.cWorkItemNo.trim().toLowerCase() === this.cInvCode.trim().toLowerCase());

            // 按工序号排序
            schProductRouteTemp.sort((p1, p2) => p1.iWoSeqID - p2.iWoSeqID);

            if (schProductRouteTemp.length < 1) return 1;

            // 取产品第一道工序,此工序生产前需配套，下层部件必须完工
            let schProductRouteLast = schProductRouteTemp[schProductRouteTemp.length - 1];

            // 从最后一道工序起，往前排,会循环调用，一直找到最低层工序正排。
            schProductRouteLast.ProcessSchTaskPre();

            // 更新当前订单产品的计划完工时间，取所有工序中最大完工时间 ,增加工序数量大于0 条件 2022-03-25 JonasCheng
            let list1 = this.SchProductRouteList.filter(p1 => p1.iSchSdID === this.iSchSdID && p1.iReqQty > 0);

            if (list1.length > 0) {
                list1.sort((p1, p2) => p1.dEndDate.getTime() - p2.dEndDate.getTime());

                this.dBegDate = list1[0].dBegDate; // 取已排任务中排产完成时间最大的
                this.dEndDate = list1[list1.length - 1].dEndDate; // 取已排任务中排产完成时间最大的

                this.bScheduled = 1; // 标为已排产
            }
        } catch (error) {
            throw new Error(`产品正排计算时出错,位置SchProduct.ProductSchTask！产品编号[${this.cInvCode}]\n\r ${error.message}`);
            return -1;
        }

        return 1;
    }

    // 倒排，纯倒排。从最后一道工序起，给产品工艺模型中的每个工序排程 2016-10-21
    public ProductSchTaskInv(): number {
        try {
            if (this.SchProductRouteList.length < 1) return 1; // 有些产品没有工序，跳过

            // 1、找出当前产品的最后一道工序，从此工序的前工序列表起，正排。
            let schProductRouteTemp = this.SchProductRouteList.filter(p1 => p1.iSchSdID === this.iSchSdID && p1.cWorkItemNo.trim().toLowerCase() === this.cInvCode.trim().toLowerCase());

            // 按工序号排序
            schProductRouteTemp.sort((p1, p2) => p1.iWoSeqID - p2.iWoSeqID);

            if (schProductRouteTemp.length < 1) return 1;

            // 取产品第一道工序,此工序生产前需配套，下层部件必须完工
            let schProductRouteLast = schProductRouteTemp[schProductRouteTemp.length - 1];

            // 从最后一道工序起，往前排,会循环调用，一直找到最低层工序正排。
            schProductRouteLast.ProcessSchTaskRevPre("3"); // 1 加工物料相同的所有工序; 2 白茬工序; 3 所有下层物料半成品工序

            // 更新当前订单产品的计划完工时间，取所有工序中最大完工时间 ,增加工序数量大于0 条件 2022-03-25 JonasCheng
            let list1 = this.SchProductRouteList.filter(p1 => p1.iSchSdID === this.iSchSdID && p1.iReqQty > 0);

            if (list1.length > 0) {
                list1.sort((p1, p2) => p1.dEndDate.getTime() - p2.dEndDate.getTime());

                this.dBegDate = list1[0].dBegDate; // 取已排任务中排产完成时间最大的
                this.dEndDate = list1[list1.length - 1].dEndDate; // 取已排任务中排产完成时间最大的

                this.bScheduled = 1; // 标为已排产
            }
        } catch (error) {
            throw new Error(`产品正排计算时出错,位置SchProduct.ProductSchTask！产品编号[${this.cInvCode}]\n\r ${error.message}`);
            return -1;
        }

        return 1;
    }

    // 倒排，考虑配套，找出最晚完工部件的完工时间，倒排其他部件油漆线的开工和完工时间。
    // 增加bSet 1是否齐套倒排标志,如果是齐套倒排，对比计划开工日期是否比之前的开工日期早，如果更早的话（目的是齐套，晚点开工晚点完工）则不需要倒排。 2022-03-25 JonasCheng
    public ProductSchTaskRev(bSet: string = "0"): number {
        const SchParam = this.SchParam
        try {
            if (this.SchProductRouteList.length < 1) return 1; // 有些产品没有工序，跳过

            // 1、找出当前产品的第一道工序，从此工序的前工序列表起，倒排。
            let schProductRouteTemp = this.SchProductRouteList.filter(p1 => p1.iSchSdID === this.iSchSdID && p1.cWorkItemNo.trim() === this.cInvCode);

            // 按工序号排序
            schProductRouteTemp.sort((p1, p2) => p1.iWoSeqID - p2.iWoSeqID);

            if (schProductRouteTemp.length < 1) return 1;

            // 取产品第一道工序,此工序生产前需配套，下层部件必须完工
            let schProductRouteFirst = schProductRouteTemp[0];

            // 配套最大延期时间 ,清除产品本身已排工序，开工时间延后 SchParam.SetMinDelayTime(如24小时)，再排。
            // 所有下层部件的倒排开始时间最少延后24小时。
            // if (SchParam.SetMinDelayTime > 0)
            // {
            //     DateTime dEarlyBegDate = schProductRouteFirst.dBegDate.AddHours(SchParam.SetMinDelayTime);
            //     schProductRouteFirst.dEarlyBegDate = dEarlyBegDate;

            //     // 清除产品本身已排工序
            //     foreach (SchProductRoute SchProductRoute1 in schProductRouteTemp)
            //     {
            //         SchProductRoute1.ProcessClearTask();
            //     }

            //     // 产品本身工序重新正排
            //     schProductRouteFirst.ProcessSchTaskNext("1"); // "1" 往后排同一物料所有工序

            // }

            if (this.iSchSdID === SchParam.iSchSdID) { // 调试断点1 SchProduct
                let i = 1;
            }

            let dtCanBegDate: DateTime;
            // 当前产品后面工序无需重排，只需重排前面零件工序，完工时间超过SetMinDelayTime时间的部件 2021-10-27 JonasCheng
            // 2、产品第一道工序,所有前工序倒排，自动一道一道往前排，直到白茬工序
            if (SchParam.SetMinDelayTime > 0) {
                // DiffMaxTime = 8; // 需配套生产最大相差时间 需配套生产物料，最大相差时间（小时），如8小时 ,越大计算次数越小，越快
                // SetMinDelayTime = 24; // 配套最少延期时间 需配套生产物料，最后一部件生产完工后，最大延期多长时间，装配开始,越大，缺料情况越少。
                for (let schProductRoute of schProductRouteFirst.SchProductRoutePreList) {
                    // 前工序的计划开工时间 + 最大齐套时间(小时) + 前工序的后准备时间(分) + 工序移转时间(分) + 后工序的前准备时间
                    dtCanBegDate = new DateTime(schProductRoute.dBegDate.getTime() + (SchParam.SetMinDelayTime + schProductRoute.iSeqPostTime / 60 + schProductRoute.iMoveTime / 60 + schProductRouteFirst.iSeqPreTime / 60) * 3600000);

                    // 之前已排产，且工序完工时间与产品第一道工序开工时间，相差不超过4小时，则不需要再倒排。(提前4小时完工算正常的,避免多次运算) //SchParam.DiffMaxTime
                    if (schProductRoute.BScheduled === 1 && dtCanBegDate > schProductRouteFirst.dBegDate)
                        continue;

                    if (schProductRoute.iProcessProductID === SchParam.iProcessProductID && schProductRoute.iSchSdID === SchParam.iSchSdID) { // 调试断点1 SchProduct
                        let i = 1;
                    }
                    // 倒排前面工序,计划数量大于0才需要倒排
                    if (schProductRoute.iReqQty > 0)
                        schProductRoute.ProcessSchTaskRevPre(SchParam.SchTaskRevTag, bSet); // 1 加工物料相同的所有工序; 2 白茬工序 ; 3 所有半成品工序
                }
            }
            // 3、更新当前订单产品的计划完工时间，取所有工序中最大完工时间 ,增加工序数量大于0 条件 2022-03-25 JonasCheng
            let list1 = this.SchProductRouteList.filter(p1 => p1.iSchSdID === this.iSchSdID && p1.iReqQty > 0);

            if (list1.length > 0) {
                list1.sort((p1, p2) => p1.dEndDate.getTime() - p2.dEndDate.getTime());

                this.dBegDate = list1[0].dBegDate; // 取已排任务中排产完成时间最大的
                this.dEndDate = list1[list1.length - 1].dEndDate; // 取已排任务中排产完成时间最大的
            }
        } catch (error) {
            throw new Error(`产品倒排计算时出错,位置SchProduct.ProductSchTaskRev！排程ID[${this.iSchSdID}],产品编号[${this.cInvCode}]\n\r ${error.message}`);
            return -1;
        }

        return 1;
    }

    // 清除资源已排具体任务占用时间段。
    public ProductClearTask(ai_SchSdID: number, ai_taskID: number): void {
        // 调用schProductRouteRes.TaskClearTask()，清除当前任务占用的时间段
        for (let schProductRoute of this.SchProductRouteList) {
            schProductRoute.ProcessClearTask();
        }

        // 产品排产状态设置为未排
        this.bScheduled = 0;
    }

    // 重新设置排产批次
    public SetSchBatch(as_iSchBatch: number): void {
        this.iSchBatch = as_iSchBatch;

        for (let schProductRoute of this.SchProductRouteList) {
            schProductRoute.iSchBatch = as_iSchBatch;

            for (let schProductRouteRes of schProductRoute.SchProductRouteResList) {
                schProductRouteRes.iSchBatch = as_iSchBatch;
            }
        }
    }

    public GetObjectData(info: any, context: any): void {
        throw new Error("Method not implemented.");
    }
}

