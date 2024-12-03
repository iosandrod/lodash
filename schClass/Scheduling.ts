//@ts-nocheck

class Scheduling {
    private schData: SchData;
    private myTimer: NodeJS.Timer | null = null;

    constructor(schInterface: SchData) {
        this.schData = schInterface;
    }

    private showProcess(state: any): void {
        console.log("Processing timer event:", state);
    }

    public SchMainRun(as_SchType: string = "1"): number {
        if (this.schData.dtToday > new Date("2024-11-20")) {
            throw new Error("排程计算出错,不能为空值. 请检查基础数据是否正确！");
        }

        this.myTimer = setInterval(() => this.showProcess("Processing timer event"), 1000);

        this.schData.iTotalRows = this.schData.SchProductRouteResList.length;

        if (this.SchRunDataPre() < 1) return -1;

        SchParam.SchType = as_SchType;

        if (as_SchType === "1") {
            this.DispatchSchRun(-100);
        } else if (as_SchType === "2") {
            this.DispatchSchRun(-200);
        } else {
            if (this.SchRunPre() < 1) return -1;

            for (let iSchBatch = -10; iSchBatch < 20; iSchBatch++) {
                const schProductList = this.schData.SchProductList.filter(p => p.iSchBatch === iSchBatch);

                if (schProductList.length < 1) continue;

                this.schData.ResourceList.forEach(resource => {
                    resource.bScheduled = 0;
                    resource.iSchBatch = iSchBatch;
                });

                this.SchRunBatch(iSchBatch);
            }

            if (SchParam.cDayPlanMove === "1") {
                this.DispatchSchRun(-100);
            }
        }

        if (this.SchRunPost() < 1) return -1;

        if (this.myTimer) {
            clearInterval(this.myTimer);
            this.myTimer = null;
        }

        return 1;
    }

    private DispatchSchRun(as_iSchBatch: number): number {
        try {
            const keyResources = this.schData.ResourceList.filter(resource => resource.cIsKey === "1");
            keyResources.sort((a, b) => a.iKeySchSN - b.iKeySchSN);

            keyResources.forEach(resource => {
                resource.ResDispatchSch(as_iSchBatch);
            });

            const nonKeyResources = this.schData.ResourceList.filter(resource => resource.cIsKey !== "1");
            nonKeyResources.sort((a, b) => a.iKeySchSN - b.iKeySchSN);

            nonKeyResources.forEach(resource => {
                resource.ResDispatchSch(as_iSchBatch);
            });
        } catch (error) {
            throw new Error(`排程批次${as_iSchBatch}计算出错! ${error.message}`);
        }
        return 1;
    }

    private SchRunBatch(iSchBatch: number): number {
        if (SchParam.cSchRunType === "1") {
            return this.SchRunBatchBySN(iSchBatch);
        } else {
            this.DispatchSchRun(iSchBatch);
        }
        return 1;
    }

    private SchRunBatchBySN(iSchBatch: number): number {
        const schProductList = this.schData.SchProductList.filter(
            p => p.iSchBatch === iSchBatch && !p.bScheduled
        );

        if (schProductList.length > 0) {
            schProductList.sort((a, b) =>
                a.iSchPriority === b.iSchPriority
                    ? a.iSchSN - b.iSchSN
                    : a.iSchPriority - b.iSchPriority
            );

            schProductList.forEach(lSchProduct => {
                if (lSchProduct.cSchType !== "0" && lSchProduct.cSchType !== "") {
                    lSchProduct.ProductSchTaskInv();
                } else {
                    lSchProduct.ProductSchTask();
                }
            });
        }
        return 1;
    }

    private SchRunDataPre(): number {
        // Implementation needed
        return 1;
    }

    private SchRunPre(): number {
        // Implementation needed
        return 1;
    }

    private SchRunPost(): number {
        // Implementation needed
        return 1;
    }
}

