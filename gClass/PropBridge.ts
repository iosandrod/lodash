import EventEmitter from 'eventemitter3'
import { Grid } from './Grid'
import { isProxy, reactive, watchEffect } from 'vue' //
//桥梁

export class PropBridge<T = Grid> extends EventEmitter {
  //这个是Reactive
  target?: T
  props?: any //
  constructor(props, _class) {
    super() //
    this.props = props //
    //@ts-ignore
    const _grid = reactive(new Grid())
    //@ts-ignore
    this.target = _grid //
    watchEffect(() => {
      const keys = Object.keys(props) //
      for (const key of keys) {
        let sk = key.slice(0, 1).toUpperCase() + key.slice(1)
        let setFn = _grid[`set${sk}`] //
        if (typeof setFn == 'function') {
          //@ts-ignore
          setFn(props[key])
        } else {
          _grid[key] = props[key] //
        }
      }
    }) //
  }
  getTarget() {
    return this.target //
  }
}
const createObj = {}
export const createBridge = <T>(_class, props): T => {
  let createFn = _class
  if (typeof _class == 'string') {
    let construct = createObj[_class]
    createFn = construct
  }
  if (typeof createFn != 'function') {
    //
    return null as any //
  }
  let _bridge = new Proxy(new PropBridge(props, _class), {
    get(target, key) {
      let _k2 = target[key]
      if (key == 'target') {
        return target.target
      } //
      if (typeof _k2 == 'function') {
        return _k2
      } //
      let _value1 = target[key]
      if (_value1 != null) {
        return _value1 //
      }
      let _k =
        key.toString().slice(0, 1).toUpperCase() + key.toString().slice(1)
      let _k1 = 'get' + _k
      let _target = target?.target
      if (typeof _target?.[_k1] == 'function') {
        return _target[_k1]()
      }
      let _value = _target?.[key]
      if (typeof _value == 'function') {
        _value = _value.bind(_target) //
      }
      return _value //
    },
    //@ts-ignore
    set(target, key, value) {
      let _key = key.toString()
      let sk = _key.slice(0, 1).toUpperCase() + _key.slice(1)
      let _target = target?.target
      if (_target == null) {
        return true
      } //
      let setFn = _target?.[`set${sk}`] //
      if (typeof setFn == 'function') {
        _target[`set${sk}`](value) //
      }
      return true //
    },
  })
  return _bridge as T //
}
