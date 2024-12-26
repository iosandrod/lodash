"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.createBridge = exports.PropBridge = void 0;
const eventemitter3_1 = __importDefault(require("eventemitter3"));
const Grid_1 = require("./Grid");
const vue_1 = require("vue"); //
//桥梁
class PropBridge extends eventemitter3_1.default {
    constructor(props, _class) {
        super(); //
        this.props = props; //
        //@ts-ignore
        const _grid = (0, vue_1.reactive)(new Grid_1.Grid());
        //@ts-ignore
        this.target = _grid; //
        (0, vue_1.watchEffect)(() => {
            const keys = Object.keys(props); //
            for (const key of keys) {
                let sk = key.slice(0, 1).toUpperCase() + key.slice(1);
                let setFn = _grid[`set${sk}`]; //
                if (typeof setFn == 'function') {
                    //@ts-ignore
                    setFn(props[key]);
                }
                else {
                    _grid[key] = props[key]; //
                }
            }
        }); //
    }
    getTarget() {
        return this.target; //
    }
}
exports.PropBridge = PropBridge;
const createObj = {};
const createBridge = (_class, props) => {
    let createFn = _class;
    if (typeof _class == 'string') {
        let construct = createObj[_class];
        createFn = construct;
    }
    if (typeof createFn != 'function') {
        //
        return null; //
    }
    let _bridge = new Proxy(new PropBridge(props, _class), {
        get(target, key) {
            let _k2 = target[key];
            if (key == 'target') {
                return target.target;
            } //
            if (typeof _k2 == 'function') {
                return _k2;
            } //
            let _value1 = target[key];
            if (_value1 != null) {
                return _value1; //
            }
            let _k = key.toString().slice(0, 1).toUpperCase() + key.toString().slice(1);
            let _k1 = 'get' + _k;
            let _target = target?.target;
            if (typeof _target?.[_k1] == 'function') {
                return _target[_k1]();
            }
            let _value = _target?.[key];
            if (typeof _value == 'function') {
                _value = _value.bind(_target); //
            }
            return _value; //
        },
        //@ts-ignore
        set(target, key, value) {
            let _key = key.toString();
            let sk = _key.slice(0, 1).toUpperCase() + _key.slice(1);
            let _target = target?.target;
            if (_target == null) {
                return true;
            } //
            let setFn = _target?.[`set${sk}`]; //
            if (typeof setFn == 'function') {
                _target[`set${sk}`](value); //
            }
            return true; //
        },
    });
    return _bridge; //
};
exports.createBridge = createBridge;
