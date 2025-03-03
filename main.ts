import { createBridge } from './gClass/PropBridge'
// import * as immer from 'immer'
import { computed, isReactive, nextTick, reactive, ref, toRaw, watch, watchEffect } from 'vue'
import { MongoClient } from 'mongodb' //
import * as fs from 'fs'
// import {MongoClient, ObjectId} from 'mongodb'
// import { plainToClass, Transform, Exclude, Expose } from 'class-transformer' //
import * as path from 'path'
import { Grid } from './gClass/Grid'
import { nanoid } from 'nanoid'
import * as React from 'react' //

import { graphql, list, group } from '@keystone-6/core'
// console.log('lists', lists)
import * as keystone from '@keystone-6/core'
// console.log(keystone)
import { getContext } from '@keystone-6/core/context'
import * as pg from 'pg'
import { Pool } from 'pg'
import { config } from '@keystone-6/core'
import { text, password, timestamp, relationship } from '@keystone-6/core/fields'
import { createAuth } from '@keystone-6/auth'
import { statelessSessions } from '@keystone-6/core/session'

import { ApolloError, ApolloServer } from 'apollo-server-express'
import * as server from 'apollo-server-express'
import Bull from 'bull'//
import { EventEmitter } from 'events'
// console.log(server, 'testServer') //
import 'reflect-metadata';
import mongoose from 'mongoose';
// import socketio from '@feathersjs/socketio-client'
import knex from 'knex'
//@ts-ignore
import http from 'http';
import { Server, Socket } from 'socket.io';
import { feathers, Params } from '@feathersjs/feathers'
import { ObjectType, Field, Resolver, Query, Subscription, buildSchema } from 'type-graphql';

import nodemailer from 'nodemailer';
import io from 'socket.io-client'
import socketio from '@feathersjs/socketio-client'
import { createClient } from 'feathers-chat'
import * as auth from '@feathersjs/authentication-client'
import { stripSlashes } from '@feathersjs/commons'
import { Service } from '@feathersjs/transport-commons/client'
import { convert, FeathersError } from '@feathersjs/errors'
import {Static, Type} from '@sinclair/typebox'
import Ajv from "ajv";
import addFormats from "ajv-formats";
import { AuthenticationClientOptions, defaults, hooks } from '@feathersjs/authentication-client'
import { AuthenticationRequest, AuthenticationResult } from '@feathersjs/authentication'
class myAuthentication extends auth.AuthenticationClient {
  async authenticate(authentication?: AuthenticationRequest, params?: Params): Promise<AuthenticationResult> {
    if (!authentication) {
      return this.reAuthenticate()
    }
    const promise = this.service
      .create(authentication, params)
      .then((authResult: AuthenticationResult) => {
        const { accessToken } = authResult
        this.authenticated = true
        this.app.emit('login', authResult)
        this.app.emit('authenticated', authResult)
        return this.setAccessToken(accessToken).then(() => authResult)
      })
      .catch((error: FeathersError) => this.handleError(error, 'authenticate'))
    this.app.set('authentication', promise)
    return promise
  }//
}////
const init = (_options: Partial<AuthenticationClientOptions> = {}) => {
  const options: AuthenticationClientOptions = Object.assign({}, defaults, _options)
  // const { Authentication } = options
  return (app: any) => {
    const authentication = new myAuthentication(app, options)
    app.authentication = authentication
    app.authenticate = authentication.authenticate.bind(authentication)
    app.reAuthenticate = authentication.reAuthenticate.bind(authentication)
    app.logout = authentication.logout.bind(authentication)
    app.hooks([hooks.authentication(), hooks.populateHeader()])
  }
}
class MyService extends Service {
  async create(data: Partial<any>, params?: Params): Promise<any> {
    // let res = await super.create(data, params)
    // return res//
    // return {}
    return this.send('create', data, params.query || {}, params.route || {})
  }
  send<X = any>(method: string, ...args: any[]) {
    return new Promise<X>((resolve, reject) => {
      const route: Record<string, any> = args.pop()
      let path = this.path
      // console.log(route, path, method)
      if (route) {
        Object.keys(route).forEach((key) => {
          path = path.replace(`:${key}`, route[key])
        })
      }
      args.unshift(method, path)

      const socketTimeout = this.connection.flags?.timeout || this.connection._opts?.ackTimeout
      // console.log(socketTimeout)
      //connectTimeout==null
      if (socketTimeout !== undefined) {
        args.push(function (timeoutError: any, error: any, data: any) {
          return timeoutError || error ? reject(convert(timeoutError || error)) : resolve(data)
        })
      } else {
        args.push(function (error: any, data: any) {
          return error ? reject(convert(error)) : resolve(data)
        })
      }
      this.connection[this.method](...args)//
    })
  }
}
const defaultEventMap = {
  create: 'created',
  update: 'updated',
  patch: 'patched',
  remove: 'removed'
}

function createMClient(connection: any, authenticationOptions = {}) {
  const client = feathers();
  // let oldService = client.service//
  client.configure(connection);

  //@ts-ignore
  client.defaultService = function (name) {
    // console.log('这里创建了Service实例')//
    const events = Object.values(defaultEventMap)
    let _connection = client.io
    const settings: any = Object.assign({}, authenticationOptions, {
      events,
      name,
      connection: _connection,//
      method: 'emit'
    })
    let s = new MyService(settings) as any
    // console.log(s.connection, 'testPath')
    return s
  }
  client.service = function (location) {
    // console.log('我是重载的注册')
    const path = stripSlashes(location);
    const current = this.services.hasOwnProperty(path) ? this.services[path] : undefined;
    // console.log(current,'testCurrent')
    if (current == null) {
      let _obj = this.defaultService(path)
      this.use(path, _obj)
      return this.service(path)
    }
    return current
    // return oldService.call(this, location)
  }
  client.configure(init());//
  //@ts-ignore
  let s1 = client.service('messages', {
    methods: ['get', 'create', 'update', 'patch', 'remove']
  })
  //@ts-ignore
  client.service('users', {
    methods: ['get', 'create', 'update', 'patch', 'remove']//
  })
  return client;
}
async function createUser(client: any) {
  client.service('users').create({ email: `${Date.now()} `, password: '123456', })
    .then(res => {
      console.log("用户创建成功")
    })
    .catch(err => { console.log('发生了错误', err.message) })
}
async function auth1(){
  const connection = socketio(io('http://localhost:3030/api/v1')) //
  let client = createMClient(connection)
  client.authenticate({
    strategy: 'local', 
    email:'1',
    password:'1'      
  }).then(res=>{ 
  }).catch(err=>{//
  })
}
async function main() {
  // 创建一个 HTTP 服务器import { Type, Static } from "@sinclair/typebox";


// 1️⃣ 定义用户角色枚举
const RoleEnum = Type.Enum({
  ADMIN: "admin",
  USER: "user",
  MODERATOR: "moderator",
});

// 2️⃣ 定义嵌套的地址对象
const AddressSchema = Type.Object({
  street: Type.String(),
  city: Type.String(),
  zip: Type.String({ minLength: 5, maxLength: 10 }),
  country: Type.String(),
});

// 3️⃣ 定义用户对象
const UserSchema = Type.Object({
  id: Type.String({ format: "uuid" }),
  name: Type.String({ minLength: 3, maxLength: 50 }),
  email: Type.String({ format: "email" }),
  age: Type.Integer({ minimum: 18, maximum: 60 }), // 限制 18~60 岁
  isActive: Type.Boolean(),
  roles: Type.Union([
    RoleEnum,                // 允许使用枚举角色
    Type.Literal("guest"),   // 额外允许 "guest"
    Type.String({ minLength: 3 }) // 也可以传入自定义角色名
  ]),
  address: AddressSchema,   // 嵌套对象
  metadata: Type.Optional(Type.Record(Type.String(), Type.Any())), // 可选的额外数据
});

// 4️⃣ 提取 TypeScript 类型
type User = Static<typeof UserSchema>;
// 5️⃣ 运行时校验
const ajv = new Ajv();
addFormats(ajv); // 添加格式校验（email、uuid 等）
const validate = ajv.compile(UserSchema);

const userData: User = {
  id: "550e8400-e29b-41d4-a716-446655440000",
  name: "Alice",
  email: "alice@example.com",
  age: 30,
  isActive: true,
  roles: 's',  // 也可以是 "moderator"、"user"、"guest" 或其他自定义字符串
  address: {
    street: "123 Main St",
    city: "New York",
    zip: "10001",
    country: "USA",
  },
  metadata: { verified: true, lastLogin: "2024-03-02" },//
};

// 6️⃣ 验证数据
if (validate(userData)) {
  console.log("✅ 数据合法");
} else {
  console.log("❌ 数据不合法", validate.errors);
}
}
main() ////       