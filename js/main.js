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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
// console.log(server, 'testServer') //
require("reflect-metadata");
const feathers_1 = require("@feathersjs/feathers");
const socket_io_client_1 = __importDefault(require("socket.io-client"));
const socketio_client_1 = __importDefault(require("@feathersjs/socketio-client"));
const auth = __importStar(require("@feathersjs/authentication-client"));
const commons_1 = require("@feathersjs/commons");
const client_1 = require("@feathersjs/transport-commons/client");
const errors_1 = require("@feathersjs/errors");
const typebox_1 = require("@sinclair/typebox");
const ajv_1 = __importDefault(require("ajv"));
const ajv_formats_1 = __importDefault(require("ajv-formats"));
const authentication_client_1 = require("@feathersjs/authentication-client");
class myAuthentication extends auth.AuthenticationClient {
    async authenticate(authentication, params) {
        if (!authentication) {
            return this.reAuthenticate();
        }
        const promise = this.service
            .create(authentication, params)
            .then((authResult) => {
            const { accessToken } = authResult;
            this.authenticated = true;
            this.app.emit('login', authResult);
            this.app.emit('authenticated', authResult);
            return this.setAccessToken(accessToken).then(() => authResult);
        })
            .catch((error) => this.handleError(error, 'authenticate'));
        this.app.set('authentication', promise);
        return promise;
    } //
} ////
const init = (_options = {}) => {
    const options = Object.assign({}, authentication_client_1.defaults, _options);
    // const { Authentication } = options
    return (app) => {
        const authentication = new myAuthentication(app, options);
        app.authentication = authentication;
        app.authenticate = authentication.authenticate.bind(authentication);
        app.reAuthenticate = authentication.reAuthenticate.bind(authentication);
        app.logout = authentication.logout.bind(authentication);
        app.hooks([authentication_client_1.hooks.authentication(), authentication_client_1.hooks.populateHeader()]);
    };
};
class MyService extends client_1.Service {
    async create(data, params) {
        // let res = await super.create(data, params)
        // return res//
        // return {}
        return this.send('create', data, params.query || {}, params.route || {});
    }
    send(method, ...args) {
        return new Promise((resolve, reject) => {
            const route = args.pop();
            let path = this.path;
            // console.log(route, path, method)
            if (route) {
                Object.keys(route).forEach((key) => {
                    path = path.replace(`:${key}`, route[key]);
                });
            }
            args.unshift(method, path);
            const socketTimeout = this.connection.flags?.timeout || this.connection._opts?.ackTimeout;
            // console.log(socketTimeout)
            //connectTimeout==null
            if (socketTimeout !== undefined) {
                args.push(function (timeoutError, error, data) {
                    return timeoutError || error ? reject((0, errors_1.convert)(timeoutError || error)) : resolve(data);
                });
            }
            else {
                args.push(function (error, data) {
                    return error ? reject((0, errors_1.convert)(error)) : resolve(data);
                });
            }
            this.connection[this.method](...args); //
        });
    }
}
const defaultEventMap = {
    create: 'created',
    update: 'updated',
    patch: 'patched',
    remove: 'removed'
};
function createMClient(connection, authenticationOptions = {}) {
    const client = (0, feathers_1.feathers)();
    // let oldService = client.service//
    client.configure(connection);
    //@ts-ignore
    client.defaultService = function (name) {
        // console.log('这里创建了Service实例')//
        const events = Object.values(defaultEventMap);
        let _connection = client.io;
        const settings = Object.assign({}, authenticationOptions, {
            events,
            name,
            connection: _connection,
            method: 'emit'
        });
        let s = new MyService(settings);
        // console.log(s.connection, 'testPath')
        return s;
    };
    client.service = function (location) {
        // console.log('我是重载的注册')
        const path = (0, commons_1.stripSlashes)(location);
        const current = this.services.hasOwnProperty(path) ? this.services[path] : undefined;
        // console.log(current,'testCurrent')
        if (current == null) {
            let _obj = this.defaultService(path);
            this.use(path, _obj);
            return this.service(path);
        }
        return current;
        // return oldService.call(this, location)
    };
    client.configure(init()); //
    //@ts-ignore
    let s1 = client.service('messages', {
        methods: ['get', 'create', 'update', 'patch', 'remove']
    });
    //@ts-ignore
    client.service('users', {
        methods: ['get', 'create', 'update', 'patch', 'remove'] //
    });
    return client;
}
async function createUser(client) {
    client.service('users').create({ email: `${Date.now()} `, password: '123456', })
        .then(res => {
        console.log("用户创建成功");
    })
        .catch(err => { console.log('发生了错误', err.message); });
}
async function auth1() {
    const connection = (0, socketio_client_1.default)((0, socket_io_client_1.default)('http://localhost:3030/api/v1')); //
    let client = createMClient(connection);
    client.authenticate({
        strategy: 'local',
        email: '1',
        password: '1'
    }).then(res => {
    }).catch(err => {
    });
}
async function main() {
    // 创建一个 HTTP 服务器import { Type, Static } from "@sinclair/typebox";
    // 1️⃣ 定义用户角色枚举
    const RoleEnum = typebox_1.Type.Enum({
        ADMIN: "admin",
        USER: "user",
        MODERATOR: "moderator",
    });
    // 2️⃣ 定义嵌套的地址对象
    const AddressSchema = typebox_1.Type.Object({
        street: typebox_1.Type.String(),
        city: typebox_1.Type.String(),
        zip: typebox_1.Type.String({ minLength: 5, maxLength: 10 }),
        country: typebox_1.Type.String(),
    });
    // 3️⃣ 定义用户对象
    const UserSchema = typebox_1.Type.Object({
        id: typebox_1.Type.String({ format: "uuid" }),
        name: typebox_1.Type.String({ minLength: 3, maxLength: 50 }),
        email: typebox_1.Type.String({ format: "email" }),
        age: typebox_1.Type.Integer({ minimum: 18, maximum: 60 }),
        isActive: typebox_1.Type.Boolean(),
        roles: typebox_1.Type.Union([
            RoleEnum,
            typebox_1.Type.Literal("guest"),
            typebox_1.Type.String({ minLength: 3 }) // 也可以传入自定义角色名
        ]),
        address: AddressSchema,
        metadata: typebox_1.Type.Optional(typebox_1.Type.Record(typebox_1.Type.String(), typebox_1.Type.Any())), // 可选的额外数据
    });
    // 5️⃣ 运行时校验
    const ajv = new ajv_1.default();
    (0, ajv_formats_1.default)(ajv); // 添加格式校验（email、uuid 等）
    const validate = ajv.compile(UserSchema);
    const userData = {
        id: "550e8400-e29b-41d4-a716-446655440000",
        name: "Alice",
        email: "alice@example.com",
        age: 30,
        isActive: true,
        roles: 's',
        address: {
            street: "123 Main St",
            city: "New York",
            zip: "10001",
            country: "USA",
        },
        metadata: { verified: true, lastLogin: "2024-03-02" }, //
    };
    // 6️⃣ 验证数据
    if (validate(userData)) {
        console.log("✅ 数据合法");
    }
    else {
        console.log("❌ 数据不合法", validate.errors);
    }
}
main(); ////       
