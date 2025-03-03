import { config } from '@keystone-6/core'
import { text, password, timestamp, relationship } from '@keystone-6/core/fields'
import { createAuth } from '@keystone-6/auth'
import { statelessSessions } from '@keystone-6/core/session'

// 会话配置
const sessionConfig = statelessSessions({
  secret: 'adfasfasfasfasfasfdasfasfdafdasadsfdsasd', // 替换成你自己的密钥
  maxAge: 60 * 60 * 24 * 30, // 会话过期时间 (30 天)
})

// 身份验证配置
const { withAuth } = createAuth({
  listKey: 'User', // 用户列表
  identityField: 'email', // 用户身份字段
  secretField: 'password', // 密码字段
  sessionData: 'id name email', // 会话中包含的数据
})

// 数据模型定义
const lists = {
  User: {
    fields: {
      name: text({ validation: { isRequired: true } }), // 用户姓名
      email: text({ validation: { isRequired: true } }), // 邮箱
      password: password(), // 密码
      posts: relationship({ ref: 'Post.author', many: true }), // 关联的文章
    },
    ui: {
      listView: { initialColumns: ['name', 'email'] }, // 管理界面初始显示的列
    },
  },
  Post: {
    fields: {
      title: text({ validation: { isRequired: true } }), // 标题
      content: text(), // 内容
      author: relationship({ ref: 'User.posts' }), // 文章作者
      createdAt: timestamp({ defaultValue: { kind: 'now' } }), // 创建时间
    },
    ui: {
      listView: { initialColumns: ['title', 'author', 'createdAt'] },
    },
  },
}

// Keystone 配置
export default withAuth(
  config({
    db: {
      provider: 'sqlite', // 使用 SQLite 数据库
      url: 'file:./keystone.db', // 数据库文件路径
    },
    server: {
      cors: { origin: ['http://localhost:3000'], credentials: true }, // 允许跨域
    },
    //@ts-ignore-
    lists,
    session: sessionConfig,
  })
)
