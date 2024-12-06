import * as immer from 'immer'
import { computed, isReactive, reactive, watchEffect } from 'vue'
import { MongoClient } from 'mongodb' //
import * as fs from 'fs'
import { plainToClass, Transform, Exclude, Expose } from 'class-transformer' //
import * as path from 'path'
async function tran(p3, p4) {
  let p = p3
  let p1 = p4
  let files = await fs.readFileSync(p) //
  let str = files.toString()
  const _arr = str.split('\n').map((v) => {
    return v.replace(/\r/g, '').replace(/\t/g, '')
  })
  console.log(_arr.length)
  let _arr1 = _arr.filter((v, i) => {
    const regex = /^[^a-zA-Z\[\]\{\}\(\)]*\/\//i
    if (regex.test(v)) {
      return false
    }
    let reg1 = /[a-zA-Z]/i
    let reg2 = /[{}[\]()]+/
    if (!reg1.test(v) && !reg2.test(v)) {
      return false //
    }
    return true
  })
  // console.log(_arr1.length) //
  let _arr2 = _arr1.join('\n')
  // console.log(_arr1.length)
  // console.log(_arr2)
  // fs.writeFileSync(p1, _arr2)
}
async function test1(str11?: any) {
  const _cp = path.resolve(__dirname, '../csMain')
  let allFiles = await fs.readdirSync('../csMain') //
  let allFiles1 = allFiles
    .map((v) => {
      return path.resolve(_cp, v)
    })
    .filter((v) => {
      return /.cs$/.test(v)
    })
    .map((v) => {
      let _ff = v.replace(/.cs$/g, '1.cs')
      let _ff1 = v.replace(/.cs$/g, '.ts')
      let obj = {
        a: v,
        b: _ff,
        c: _ff1,
      }
      return obj
    })
  allFiles1.forEach(async (v) => {
    let a = v.a
    let b = v.b
    let c = v.c
    if (!/1.ts$/.test(c)) {
      // let bf = await fs.readFileSync(c)
      // let s = bf.toString()
      // let np = c.replace(/backup/g, 'backup_conver') //
    }
    // console.log(bf)
    // if (/1.ts$/.test(c)) {
    //   // fs.unlinkSync(c) //
    // }
    // console.log(c)
    // fs.writeFileSync(c, '') //
    tran(a, b) //
  })
  // let p =p3|| '../schClass/Resource.cs'
  // let p1 =p4|| '../schClass/Resource1.cs'
  // let files = str11 || (await fs.readFileSync(p)) //
  // let str = files.toString()
  // const _arr = str.split('\n').map((v) => {
  //   return v.replace(/\r/g, '').replace(/\t/g, '')
  // })
  // console.log(_arr.length)
  // let _arr1 = _arr.filter((v, i) => {
  //   const regex = /^[^a-zA-Z\[\]\{\}\(\)]*\/\//i
  //   if (regex.test(v)) {
  //     return false
  //   }
  //   let reg1 = /[a-zA-Z]/i
  //   let reg2 = /[{}[\]()]+/
  //   if (!reg1.test(v) && !reg2.test(v)) {
  //     return false //
  //   }
  //   return true
  // })
  // // console.log(_arr1.length) //
  // let _arr2 = _arr1.join('\n')
  // console.log(_arr2.length)
  // console.log(_arr2)
  // fs.writeFileSync(p1, _arr2)
}
test1() //
