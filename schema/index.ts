//@ts-nocheck
import { list } from '@keystone-6/core'
import { text, integer } from '@keystone-6/core/fields'

export const lists = {
  // 示例模型
  Post: list({ 
    fields: {
      title: text({ isRequired: true }),
      content: text(),
      views: integer(),
    },
  }),
  Author: list({
    fields: {
      name: text({ isRequired: true }),
      bio: text(),
    },
  }),
}
