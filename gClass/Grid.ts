import { Column } from './Column'
import { createBridge } from './PropBridge'

interface ItemSizer {
  (index: number): number
}
type GridProps = any
interface ScrollCoords {
  scrollLeft: number
  scrollTop: number
}

interface CellInterface {
  row: number
  column: number
}

interface SelectionArea {
  start: CellInterface
  end: CellInterface
}

interface AreaProps {
  start: CellInterface
  end: CellInterface
}

interface ShapeConfig {
  color?: string
  width?: number
}

interface StylingProps {
  borderColor?: string
  borderWidth?: number
}

interface CellRangeArea {
  start: CellInterface
  end: CellInterface
}

interface RendererProps {
  rowIndex: number
  columnIndex: number
}

interface ViewPortProps {
  top: number
  left: number
  width: number
  height: number
}

interface SelectionProps {
  area: SelectionArea
}

export class Grid {
  width?: number = 500
  height?: number
  columnCount?: number
  rowCount?: number
  rowHeight?: ItemSizer
  columnWidth?: ItemSizer
  scrollbarSize: number = 13
  estimatedColumnWidth?: number
  estimatedRowHeight?: number
  onScroll?: (coords: ScrollCoords) => void
  onImmediateScroll?: (coords: ScrollCoords) => void
  showScrollbar?: boolean
  activeCell?: CellInterface | null
  selectionBackgroundColor?: string
  selectionBorderColor?: string
  selectionStrokeWidth?: number
  activeCellStrokeWidth?: number
  selections?: SelectionArea[]
  fillSelection?: SelectionArea | null
  mergedCells?: AreaProps[]
  frozenRows?: number
  rowMetadata = new WeakMap()
  columnMetadata = new WeakMap() //
  frozenColumns?: number
  snap?: boolean
  showFrozenShadow?: boolean
  shadowSettings?: ShapeConfig
  scrollThrottleTimeout?: number
  borderStyles?: StylingProps
  cellAreas?: CellRangeArea[]
  // itemRenderer?: (props: RendererProps) => React.ReactNode;
  // overlayRenderer?: (props: RendererProps) => React.ReactNode;
  // selectionRenderer?: (props: SelectionProps) => React.ReactNode;
  fillHandleProps?: Record<string, (e: any) => void>
  onViewChange?: (view: ViewPortProps) => void
  onBeforeRenderRow?: (rowIndex: number) => void
  // children?: (props: ScrollCoords) => React.ReactNode;
  // wrapper?: (children: React.ReactNode) => React.ReactNode;
  stageProps?: Omit<ShapeConfig, 'container'>
  showFillHandle?: boolean
  overscanCount?: number
  fillhandleBorderColor?: string
  showGridLines?: boolean
  gridLineColor?: string
  gridLineWidth?: number
  // gridLineRenderer?: (props: ShapeConfig) => React.ReactNode;
  shadowStroke?: string
  enableCellOverlay?: boolean
  isHiddenRow?: (rowIndex: number) => boolean
  isHiddenColumn?: (columnIndex: number) => boolean
  isHiddenCell?: (rowIndex: number, columnIndex: number) => boolean
  scale?: number
  enableSelectionDrag?: boolean
  isDraggingSelection?: boolean
  columns: Column[] = [] //
  cache: any = {} //
  data: any[] = []
  constructor() {}
  onScrollChange(scrollChangConfig: any) {}
  scrollTo({ scrollLeft, scrollTop }) {
    // if (this.container) {
    //   this.container.scrollLeft = scrollLeft;
    //   this.container.scrollTop = scrollTop;
    // }
  }
  setColumns(columns) {
    if (!Array.isArray(columns)) {
      return
    }
    columns.forEach(col => {
      this.addColumn(col)
    })
  }
  setData(data) {
    //
    let _d = new Array(10000).fill(null).map((_, i) => {
      return { rowIndex: 1 }
    })
    this.data = _d
  }
  addColumn(col) {
    let field = col.field
    let columns = this.columns
    let hasCol = columns.some(col => col.field === field)
    if (hasCol) {
      return
    }
    let _col = createBridge<Column>(Column, col) //
    this.columns.push(_col) //
  }
  removeColumn(col) {}
  setWidth(width: number) {
    this.width = width
  }
  // Scroll by a delta
  scrollBy({ deltaX, deltaY }) {
    // if (this.container) {
    //   this.container.scrollLeft += deltaX;
    //   this.container.scrollTop += deltaY;
    // }
  }

  // Scroll to a specific item
  scrollToItem({ rowIndex, columnIndex }) {
    const rowOffset = this.getRowOffset(rowIndex)
    const columnOffset = this.getColumnOffset(columnIndex)
    this.scrollTo({ scrollLeft: columnOffset, scrollTop: rowOffset })
  }

  // Reset grid after specific indices
  resetAfterIndices({ rowIndex, columnIndex }) {
    // Logic to reset cached or calculated values for given indices
    console.log('Resetting grid after indices:', rowIndex, columnIndex)
  }

  // Get the current scroll position
  getScrollPosition(config) {
    // return {
    //   scrollLeft: this.container?.scrollLeft || 0,
    //   scrollTop: this.container?.scrollTop || 0,
    // };
  }
  // Check if a cell is merged
  isMergedCell(rowIndex, columnIndex) {
    // return this.props.mergedCells?.some(
    //   ({ startRow, startCol, endRow, endCol }) =>
    //     rowIndex >= startRow &&
    //     rowIndex <= endRow &&
    //     columnIndex >= startCol &&
    //     columnIndex <= endCol
    // );
  }

  // Get cell bounds
  getCellBounds(rowIndex, columnIndex) {
    // return {
    //   top: this.getRowOffset(rowIndex),
    //   left: this.getColumnOffset(columnIndex),
    //   height: this.props.rowHeight?.(rowIndex) || 0,
    //   width: this.props.columnWidth?.(columnIndex) || 0,
    // };
  }

  // Get cell coordinates from an offset
  getCellCoordsFromOffset(offsetX, offsetY) {
    // const rowIndex = Math.floor(offsetY / this.props.rowHeight?.(0));
    // const columnIndex = Math.floor(offsetX / this.props.columnWidth?.(0));
    // return { rowIndex, columnIndex };
  }

  // Get cell offset from coordinates
  getCellOffsetFromCoords(rowIndex, columnIndex) {
    return {
      offsetX: this.getColumnOffset(columnIndex),
      offsetY: this.getRowOffset(rowIndex),
    }
  }

  // Get actual cell coordinates (handling merged cells)
  getActualCellCoords(rowIndex, columnIndex) {
    // if (this.isMergedCell(rowIndex, columnIndex)) {
    //   return this.props.mergedCells.find(({ startRow, startCol, endRow, endCol }) =>
    //     rowIndex >= startRow &&
    //     rowIndex <= endRow &&
    //     columnIndex >= startCol &&
    //     columnIndex <= endCol
    //   );
    // }
    // return { rowIndex, columnIndex };
  }

  // Focus on the container
  focus() {
    // this.container?.focus();
  }

  // Resize columns
  resizeColumns(columnIndex, newWidth) {
    console.log(`Resizing column ${columnIndex} to width ${newWidth}`)
    // Logic to update column width
  }

  // Resize rows
  resizeRows(rowIndex, newHeight) {
    console.log(`Resizing row ${rowIndex} to height ${newHeight}`)
    // Logic to update row height
  }

  // Get viewport dimensions
  getViewPort() {
    // return {
    //   width: this.props.width,
    //   height: this.props.height,
    // };
  }

  // Get relative position from offset
  getRelativePositionFromOffset(offsetX, offsetY) {
    // return {
    //   x: offsetX / this.props.scale,
    //   y: offsetY / this.props.scale,
    // };
  }
  // Scroll to the top
  scrollToTop() {
    this.scrollTo({ scrollLeft: 0, scrollTop: 0 })
  }

  // Scroll to the bottom
  scrollToBottom() {
    // this.scrollTo({
    //   scrollLeft: 0,
    //   scrollTop: this.props.rowCount * (this.props.rowHeight?.(0) || 0),
    // });
  }
  onRowHeightChange(config) {}
  onColumnWidthChange(config) {}
  // Get grid dimensions
  getDimensions() {
    // return {
    //   totalWidth: this.props.columnCount * (this.props.columnWidth?.(0) || 0),
    //   totalHeight: this.props.rowCount * (this.props.rowHeight?.(0) || 0),
    // };
  }

  // Get row offset
  getRowOffset(rowIndex) {
    // let offset = 0;
    // for (let i = 0; i < rowIndex; i++) {
    //   offset += this.props.rowHeight?.(i) || 0;
    // }
    // return offset;
  }

  // Get column offset
  getColumnOffset(columnIndex) {
    // let offset = 0;
    // for (let i = 0; i < columnIndex; i++) {
    //   offset += this.props.columnWidth?.(i) || 0;
    // }
    // return offset;
  }
}

// 示例用法：