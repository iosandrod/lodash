"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Grid = void 0;
const Column_1 = require("./Column");
const PropBridge_1 = require("./PropBridge");
class Grid {
    constructor() {
        this.width = 500;
        this.scrollbarSize = 13;
        this.rowMetadata = new WeakMap();
        this.columnMetadata = new WeakMap(); //
        this.columns = []; //
        this.cache = {}; //
        this.data = [];
    }
    onScrollChange(scrollChangConfig) { }
    scrollTo({ scrollLeft, scrollTop }) {
        // if (this.container) {
        //   this.container.scrollLeft = scrollLeft;
        //   this.container.scrollTop = scrollTop;
        // }
    }
    setColumns(columns) {
        if (!Array.isArray(columns)) {
            return;
        }
        columns.forEach(col => {
            this.addColumn(col);
        });
    }
    setData(data) {
        //
        let _d = new Array(10000).fill(null).map((_, i) => {
            return { rowIndex: 1 };
        });
        this.data = _d;
    }
    addColumn(col) {
        let field = col.field;
        let columns = this.columns;
        let hasCol = columns.some(col => col.field === field);
        if (hasCol) {
            return;
        }
        let _col = (0, PropBridge_1.createBridge)(Column_1.Column, col); //
        this.columns.push(_col); //
    }
    removeColumn(col) { }
    setWidth(width) {
        this.width = width;
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
        const rowOffset = this.getRowOffset(rowIndex);
        const columnOffset = this.getColumnOffset(columnIndex);
        this.scrollTo({ scrollLeft: columnOffset, scrollTop: rowOffset });
    }
    // Reset grid after specific indices
    resetAfterIndices({ rowIndex, columnIndex }) {
        // Logic to reset cached or calculated values for given indices
        console.log('Resetting grid after indices:', rowIndex, columnIndex);
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
        };
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
        console.log(`Resizing column ${columnIndex} to width ${newWidth}`);
        // Logic to update column width
    }
    // Resize rows
    resizeRows(rowIndex, newHeight) {
        console.log(`Resizing row ${rowIndex} to height ${newHeight}`);
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
        this.scrollTo({ scrollLeft: 0, scrollTop: 0 });
    }
    // Scroll to the bottom
    scrollToBottom() {
        // this.scrollTo({
        //   scrollLeft: 0,
        //   scrollTop: this.props.rowCount * (this.props.rowHeight?.(0) || 0),
        // });
    }
    onRowHeightChange(config) { }
    onColumnWidthChange(config) { }
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
exports.Grid = Grid;
// 示例用法：
