import * as immer from 'immer'
import { computed, isReactive, reactive, watchEffect } from 'vue'
import { MongoClient } from 'mongodb'
// import {} from ''
//
export function Log(options?: any) {
    return function (target, p, decorator) {
        let oldFn: Function = decorator.value
        decorator.value = async function (...args) {
            try {
                await oldFn.apply(target, args)
            } catch (error) {
                console.log(error?.message || error)//
            }
        }
    }
}
export function gs(options?: any) {
    return function (target, p,) {
    }
}
abstract class Base {
    log: string
    logName: Function
}
class User extends Base {
    @gs({})
    username: string
    @Log({})
    async setName(str: string) {//
        // console.log(str, 'log')
        return Promise.reject('aaa')
    }

}

let u = new User()
console.log(u.log)
u.setName('xiaofeng')
u.setName('xiaoming')