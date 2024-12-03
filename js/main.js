"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
const fs = __importStar(require("fs"));
const path = __importStar(require("path"));
async function tran(p3, p4) {
    let p = p3;
    let p1 = p4;
    let files = await fs.readFileSync(p); //
    let str = files.toString();
    const _arr = str.split('\n').map((v) => {
        return v.replace(/\r/g, '').replace(/\t/g, '');
    });
    // console.log(_arr.length)
    let _arr1 = _arr.filter((v, i) => {
        const regex = /^[^a-zA-Z\[\]\{\}\(\)]*\/\//i;
        if (regex.test(v)) {
            return false;
        }
        let reg1 = /[a-zA-Z]/i;
        let reg2 = /[{}[\]()]+/;
        if (!reg1.test(v) && !reg2.test(v)) {
            return false; //
        }
        return true;
    });
    // console.log(_arr1.length) //
    let _arr2 = _arr1.join('\n');
    console.log(_arr2.length);
    // console.log(_arr2)
    // fs.writeFileSync(p1, _arr2)
}
async function test1(str11) {
    const _cp = path.resolve(__dirname, '../backup');
    let allFiles = await fs.readdirSync('../backup'); //
    let allFiles1 = allFiles
        .map((v) => {
        return path.resolve(_cp, v);
    })
        .filter((v) => {
        return /.cs$/.test(v);
    })
        .map((v) => {
        let _ff = v.replace(/.cs$/g, '1.cs');
        let _ff1 = v.replace(/.cs$/g, '.ts');
        let obj = {
            a: v,
            b: _ff,
            c: _ff1,
        };
        return obj;
    });
    // console.log(allFiles1)
    allFiles1.forEach(async (v) => {
        let a = v.a;
        let b = v.b;
        let c = v.c;
        if (!/1.ts$/.test(c)) {
            let bf = await fs.readFileSync(c);
            let s = bf.toString();
            let np = c.replace(/backup/g, 'backup_conver'); //
            // console.log(np)
            // console.log(c)//
            // fs.writeFileSync(np, s) //////
        }
        // console.log(bf)
        // if (/1.ts$/.test(c)) {
        //   // fs.unlinkSync(c) //
        // }
        // console.log(c)
        // fs.writeFileSync(c, '') //
        // tran(a, b) //
    });
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
test1(); //
