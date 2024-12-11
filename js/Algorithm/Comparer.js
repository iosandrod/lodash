"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Comparer = void 0;
class Comparer {
    constructor(c) {
        if (c != null) {
            this.Default = new Comparer();
        }
    }
    Compare(...args) {
    }
    default(a, b) {
    }
    // Default(..args):any{
    //     return this.default(...args)
    // }
    numeric(a, b) {
    }
    date(a, b) {
        return 0;
    }
}
exports.Comparer = Comparer;
