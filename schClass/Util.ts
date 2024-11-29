//装饰

//装饰器
export type Decorator = {
  get?: Function
  set?: Function
  validate?: Function//
}
function getFormat(key: string) {
  let _key = key.slice(0, 1).toUpperCase() + key.slice(1)
  _key = 'get' + _key
  return _key
}
function setFormat(key: string) {
  let _key = key.slice(0, 1).toLowerCase() + key.slice(1)
  _key = 'set' + _key
  return _key
}
export function gsDecoration(options: Decorator) {
  //
  return function (target, propertyKey) {
    let _key = getFormat(propertyKey)
    let get = options['get']
    if (get) {
      target[_key] = get
    }
    let set = options['set'] //
    let _key1 = setFormat(propertyKey) //
    if (set) {
      target[_key1] = set //
    }
  }
}


export function  GetDateDiff(resource: Resource | null, datePart: string, dt1: Date, dt2: Date): number {
    if (!resource || !resource.cResourceNo) return 1;

    const filteredRows = resource.schData.dt_ResourceTime.rows.filter(
        (row) =>
            row.cResourceNo === resource.cResourceNo &&
            row.dPeriodBegTime >= dt1 &&
            row.dPeriodBegTime <= dt2
    );

    let uniqueDays = new Set(
        filteredRows.map((row) => new Date(row.dPeriodDay).toDateString())
    );

    if (datePart === 'd') {
        return uniqueDays.size;
    }

    return 0;
}

export function   GetDateDiffSimple(datePart: string, dt1: Date, dt2: Date): number {
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
