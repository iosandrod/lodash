"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@keystone-6/core");
const fields_1 = require("@keystone-6/core/fields");
const auth_1 = require("@keystone-6/auth");
const session_1 = require("@keystone-6/core/session");
// 会话配置
const sessionConfig = (0, session_1.statelessSessions)({
    secret: 'adfasfasfasfasfasfdasfasfdafdasadsfdsasd',
    maxAge: 60 * 60 * 24 * 30, // 会话过期时间 (30 天)
});
// 身份验证配置
const { withAuth } = (0, auth_1.createAuth)({
    listKey: 'User',
    identityField: 'email',
    secretField: 'password',
    sessionData: 'id name email', // 会话中包含的数据
});
// 数据模型定义
const lists = {
    User: {
        fields: {
            name: (0, fields_1.text)({ validation: { isRequired: true } }),
            email: (0, fields_1.text)({ validation: { isRequired: true } }),
            password: (0, fields_1.password)(),
            posts: (0, fields_1.relationship)({ ref: 'Post.author', many: true }), // 关联的文章
        },
        ui: {
            listView: { initialColumns: ['name', 'email'] }, // 管理界面初始显示的列
        },
    },
    Post: {
        fields: {
            title: (0, fields_1.text)({ validation: { isRequired: true } }),
            content: (0, fields_1.text)(),
            author: (0, fields_1.relationship)({ ref: 'User.posts' }),
            createdAt: (0, fields_1.timestamp)({ defaultValue: { kind: 'now' } }), // 创建时间
        },
        ui: {
            listView: { initialColumns: ['title', 'author', 'createdAt'] },
        },
    },
};
// Keystone 配置
exports.default = withAuth((0, core_1.config)({
    db: {
        provider: 'sqlite',
        url: 'file:./keystone.db', // 数据库文件路径
    },
    server: {
        cors: { origin: ['http://localhost:3000'], credentials: true }, // 允许跨域
    },
    //@ts-ignore-
    lists,
    session: sessionConfig,
}));
