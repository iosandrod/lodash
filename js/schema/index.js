"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.lists = void 0;
//@ts-nocheck
const core_1 = require("@keystone-6/core");
const fields_1 = require("@keystone-6/core/fields");
exports.lists = {
    // 示例模型
    Post: (0, core_1.list)({
        fields: {
            title: (0, fields_1.text)({ isRequired: true }),
            content: (0, fields_1.text)(),
            views: (0, fields_1.integer)(),
        },
    }),
    Author: (0, core_1.list)({
        fields: {
            name: (0, fields_1.text)({ isRequired: true }),
            bio: (0, fields_1.text)(),
        },
    }),
};
