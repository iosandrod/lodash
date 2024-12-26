"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// 自定义纯数字字符集，设置长度为10（可根据需求调整）
// const generateNumericId = customAlphabet('0123456789', 10);
// const id1 = generateNumericId(); // 例如: 3849205817
// const id2 = generateNumericId(); // 例如: 1749023856
// console.log(id1, id2);
let obj = Array(20)
    .fill(null)
    .map((_, i) => {
    let randomStr = Math.random().toString(36).slice(-6);
    let _str = `${i}_${randomStr}`;
    return { str: _str };
})
    .reduce((acc, _v) => {
    let v = _v.str;
    //@ts-ignore
    acc[v] = {};
    return acc;
}, {});
// console.log(obj)
let _obj = {
    '0_5jnnzi': {},
    '1_v39h1q': {},
    '2_kwcyvf': {},
    '3_qqjugh': {},
    '4_e8rrtf': {},
    '5_5z6chf': {},
    '6_7f37v2': {},
    '7_5cvxms': {},
    '8_mi0vxx': {},
    '9_j8bmbf': {},
    '10_7eg1yv': {},
    '11_tv1jq4': {},
    '12_wx223a': {},
    '13_vfz28d': {},
    '14_dcpabr': {},
    '15_bn1175': {},
    '16_svlkic': {},
    '17_jrkbdc': {},
    '18_zd7exx': {},
    '19_tflbsf': {},
};
let _sort = Object.keys(_obj).map((v) => { return { n: v.split('_')[0], k: v }; }).sort((a, b) => {
    let num = a.n > b.n ? 1 : -1;
    return num; //
}).map(v => v.k);
console.log(_sort); // //
