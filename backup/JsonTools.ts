
export class JsonTools {
  static objectToJson(obj: any): string {
    return JSON.stringify(obj)
  }

  static jsonToObject<T>(jsonString: string, obj: T): T {
    return JSON.parse(jsonString) as T
  }

  static objectToJson2(obj: any): string {
    return JSON.stringify(obj)
  }

  static jsonToObject2<T>(jsonString: string, obj: T): T {
    return JSON.parse(jsonString) as T
  }
}

export default JsonTools
