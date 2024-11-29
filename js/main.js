"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.gs = exports.Log = void 0;
// import {} from ''
//
function Log(options) {
    return function (target, p, decorator) {
        let oldFn = decorator.value;
        decorator.value = async function (...args) {
            try {
                await oldFn.apply(target, args);
            }
            catch (error) {
                console.log(error?.message || error); //
            }
        };
    };
}
exports.Log = Log;
function gs(options) {
    return function (target, p) {
    };
}
exports.gs = gs;
class Base {
}
class User extends Base {
    async setName(str) {
        // console.log(str, 'log')
        return Promise.reject('aaa');
    }
}
__decorate([
    gs({}),
    __metadata("design:type", String)
], User.prototype, "username", void 0);
__decorate([
    Log({}),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [String]),
    __metadata("design:returntype", Promise)
], User.prototype, "setName", null);
let u = new User();
console.log(u.log);
u.setName('xiaofeng');
u.setName('xiaoming');
