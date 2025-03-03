class Base {}
class Column extends Base {}
class Table extends Base {
  columns = []
  rows = 0
  rowHeight = 30
  headerHeight = 30
  groupHeaderHeight = 30
  enableGroups = false
  freezeColumns = 0
  experimental: {
    disableAccessibilityTree?: boolean
    disableMinimumCellWidth?: boolean
    paddingRight?: number
    paddingBottom?: number
    enableFirefoxRescaling?: boolean
    enableSafariRescaling?: boolean
    kineticScrollPerfHack?: boolean
    isSubGrid?: boolean
    strict?: boolean
    scrollbarWidthOverride?: number
    hyperWrapping?: boolean
    renderStrategy?: 'single-buffer' | 'double-buffer' | 'direct'
  } = {}
  nonGrowWidth
  clientSize
  className
  onVisibleRegionChanged
  scrollRef
  preventDiagonalScrolling
  rightElement
  rightElementProps
  overscrollX
  overscrollY
  initialSize
  smoothScrollX = true
  smoothScrollY = true
  isDraggable=false
}
