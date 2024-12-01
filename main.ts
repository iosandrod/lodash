import * as immer from 'immer'
import { computed, isReactive, reactive, watchEffect } from 'vue'
import { MongoClient } from 'mongodb'
import { } from 'class-validator'
import { } from 'class-transformer'//
// import {} from ''
//class-transformer
import { plainToClass, Transform, Exclude, Expose } from 'class-transformer';

class User {
    @Expose()
    name: string;

    @Expose()
    email: string;

    @Transform(({ value }) => value.toUpperCase(), { toPlainOnly: true })
    @Expose()
    role: string;

    constructor(name: string, email: string, role: string) {
        this.name = name;
        this.email = email;
        this.role = role;
    }
}

const user = new User('john Doe', 'john@example.com', 'admin');
console.log(user);
// 将类实例转换为普通对象时应用转换逻辑
const plainObject = plainToClass(User, user);
console.log(plainObject);  // 输出: { name: 'John Doe', email: 'john@example.com', role: 'ADMIN' }
// role 被转换为大写字母
