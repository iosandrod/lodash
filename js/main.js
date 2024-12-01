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
// import {} from ''
//class-transformer
const class_transformer_1 = require("class-transformer");
class User {
    constructor(name, email, role) {
        this.name = name;
        this.email = email;
        this.role = role;
    }
}
__decorate([
    (0, class_transformer_1.Expose)(),
    __metadata("design:type", String)
], User.prototype, "name", void 0);
__decorate([
    (0, class_transformer_1.Expose)(),
    __metadata("design:type", String)
], User.prototype, "email", void 0);
__decorate([
    (0, class_transformer_1.Transform)(({ value }) => value.toUpperCase(), { toPlainOnly: true }),
    (0, class_transformer_1.Expose)(),
    __metadata("design:type", String)
], User.prototype, "role", void 0);
const user = new User('john Doe', 'john@example.com', 'admin');
console.log(user);
// 将类实例转换为普通对象时应用转换逻辑
const plainObject = (0, class_transformer_1.plainToClass)(User, user);
console.log(plainObject); // 输出: { name: 'John Doe', email: 'john@example.com', role: 'ADMIN' }
// role 被转换为大写字母
