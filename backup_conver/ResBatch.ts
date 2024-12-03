import { SchProductRouteRes } from './type' // Assuming SchProductRouteRes is defined in a separate file

export class ResBatch {
  public ListSchProductRouteRes: SchProductRouteRes[] = []

  constructor(cWcNo?: string) {
    if (cWcNo === '') return

    // Initialize ListSchProductRouteRes with a capacity of 10
    this.ListSchProductRouteRes = new Array<SchProductRouteRes>(10)
  }
}
