var __defProp = Object.defineProperty;
var __getOwnPropDesc = Object.getOwnPropertyDescriptor;
var __getOwnPropNames = Object.getOwnPropertyNames;
var __hasOwnProp = Object.prototype.hasOwnProperty;
var __export = (target, all) => {
  for (var name in all)
    __defProp(target, name, { get: all[name], enumerable: true });
};
var __copyProps = (to, from, except, desc) => {
  if (from && typeof from === "object" || typeof from === "function") {
    for (let key of __getOwnPropNames(from))
      if (!__hasOwnProp.call(to, key) && key !== except)
        __defProp(to, key, { get: () => from[key], enumerable: !(desc = __getOwnPropDesc(from, key)) || desc.enumerable });
  }
  return to;
};
var __toCommonJS = (mod) => __copyProps(__defProp({}, "__esModule", { value: true }), mod);

// keystone.ts
var keystone_exports = {};
__export(keystone_exports, {
  default: () => keystone_default
});
module.exports = __toCommonJS(keystone_exports);
var import_core = require("@keystone-6/core");
var import_fields = require("@keystone-6/core/fields");
var import_auth = require("@keystone-6/auth");
var import_session = require("@keystone-6/core/session");
var sessionConfig = (0, import_session.statelessSessions)({
  secret: "adfasfasfasfasfasfdasfasfdafdasadsfdsasd",
  // 替换成你自己的密钥
  maxAge: 60 * 60 * 24 * 30
  // 会话过期时间 (30 天)
});
var { withAuth } = (0, import_auth.createAuth)({
  listKey: "User",
  // 用户列表
  identityField: "email",
  // 用户身份字段
  secretField: "password",
  // 密码字段
  sessionData: "id name email"
  // 会话中包含的数据
});
var lists = {
  User: {
    fields: {
      name: (0, import_fields.text)({ validation: { isRequired: true } }),
      // 用户姓名
      email: (0, import_fields.text)({ validation: { isRequired: true } }),
      // 邮箱
      password: (0, import_fields.password)(),
      // 密码
      posts: (0, import_fields.relationship)({ ref: "Post.author", many: true })
      // 关联的文章
    },
    ui: {
      listView: { initialColumns: ["name", "email"] }
      // 管理界面初始显示的列
    }
  },
  Post: {
    fields: {
      title: (0, import_fields.text)({ validation: { isRequired: true } }),
      // 标题
      content: (0, import_fields.text)(),
      // 内容
      author: (0, import_fields.relationship)({ ref: "User.posts" }),
      // 文章作者
      createdAt: (0, import_fields.timestamp)({ defaultValue: { kind: "now" } })
      // 创建时间
    },
    ui: {
      listView: { initialColumns: ["title", "author", "createdAt"] }
    }
  }
};
var keystone_default = withAuth(
  (0, import_core.config)({
    db: {
      provider: "sqlite",
      // 使用 SQLite 数据库
      url: "file:./keystone.db"
      // 数据库文件路径
    },
    server: {
      cors: { origin: ["http://localhost:3000"], credentials: true }
      // 允许跨域
    },
    //@ts-ignore-
    lists,
    session: sessionConfig
  })
);
//# sourceMappingURL=config.js.map
