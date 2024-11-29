import { DataTable } from './DataTable'; // 假设有类似 DataTable 的实现
// import { Resource, SchProduct, SchProductWorkItem, SchProductRoute, SchProductRouteItem, SchProductRouteRes, TaskTimeRange, WorkCenter, Item, TechInfo } from './Models';
export class SchData {
    // 定义变量
    cVersionNo: string = '';
    cCalculateNo: string = ''; // 排程运算号 用户名+时间
    dtStart: Date = new Date();
    dtEnd: Date = new Date();
    dtToday: Date = new Date();

    iCurRows: number = 0; // 排程当前任务数，用于统计当前进度
    iTotalRows: number = 100; // 排程总任务数
    iProgress: number = 0; // 进度百分比
    // DataTable 类的占位
    dtSchProduct = new DataTable();
    dtSchProductWorkItem = new DataTable();
    dtResource = new DataTable();
    dt_ResourceTime = new DataTable();
    dt_ResourceSpecTime = new DataTable();
    cSchCapType: string = '0'; // 排程产能方案
    dtSchProductRoute = new DataTable();
    dtSchProductRouteRes = new DataTable();
    dtSchProductRouteItem = new DataTable();
    dtSchProductRouteResTime = new DataTable();
    dtSchResWorkTime = new DataTable();
    dtSchProductTemp = new DataTable();
    dtSchProductRouteTemp = new DataTable();
    dtSchProductRouteResTemp = new DataTable();
    dtProChaType = new DataTable();
    dtResChaValue = new DataTable();
    dtResChaCrossTime = new DataTable();
    dtWorkCenter = new DataTable();
    dtDepartment = new DataTable();
    dtTeam = new DataTable();
    dtPerson = new DataTable();
    dtItem = new DataTable();
    dtTechInfo = new DataTable();
    dtTechLearnCurves = new DataTable();
    dtResTechScheduSN = new DataTable();
    // 列表
    ResourceList: Resource[] = [];
    KeyResourceList: Resource[] = [];
    TeamResourceList: Resource[] = [];
    SchProductList: SchProduct[] = [];
    SchProductWorkItemList: SchProductWorkItem[] = [];
    SchProductRouteList: SchProductRoute[] = [];
    SchProductRouteItemList: SchProductRouteItem[] = [];
    SchProductRouteResList: SchProductRouteRes[] = [];
    TaskTimeRangeList: TaskTimeRange[] = [];
    WorkCenterList: WorkCenter[] = [];
    ItemList: Item[] = [];
    TechInfoList: TechInfo[] = [];

    // 公用方法
    
     GetDateDiffSimple(datePart: string, dt1: Date, dt2: Date): number {
        const diffMs = dt2.getTime() - dt1.getTime();

        switch (datePart) {
            case 's':
                return Math.floor(diffMs / 1000);
            case 'm':
                return Math.floor(diffMs / (1000 * 60));
            case 'h':
                return Math.floor(diffMs / (1000 * 60 * 60));
            case 'd':
                return Math.floor(diffMs / (1000 * 60 * 60 * 24));
            default:
                return 0;
        }
    }

     AddDate(datePart: string, iAdd: number, dt1: Date): Date {
        const newDate = new Date(dt1);
        switch (datePart) {
            case 's':
                newDate.setSeconds(newDate.getSeconds() + iAdd);
                break;
            case 'm':
                newDate.setMinutes(newDate.getMinutes() + iAdd);
                break;
            case 'h':
                newDate.setHours(newDate.getHours() + iAdd);
                break;
            case 'd':
                newDate.setDate(newDate.getDate() + iAdd);
                break;
            default:
                break;
        }
        return newDate;
    }
}
