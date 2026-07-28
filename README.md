# UserLogin 登录注册系统

一个简单的Unity用户登录注册功能实现，并给用户添加了会员属性，使用C#开发服务端，MySQL作为数据库，protobuf生成协议包。注：本项目为面试作品。

## 1. 项目说明
本项目实现了一个基于Unity客户端 + C#服务端 + MySQL数据库的登录注册系统，包含：
- Unity客户端：登录界面、注册界面、用户界面
- 服务端：CenterServer、LoginServer
- 数据库：MySQL，存储用户信息
服务端参考[https://www.bilibili.com/video/BV1mGjmzqEpq]16-25集搭建。

## 2. 数据库类型
项目使用MySQL作为数据库，相关配置如下，可在[UserLogin_Servers/CenterServer/DB/DBMgr.cs]中设置：
- 数据库名：`userlogin`(可在[UserLogin_Servers/CenterServer/DB/Table/AccountTable.cs]中设置)
- 默认连接地址：`127.0.0.1`
- 默认端口：`3306`
- 默认账号：`root`
- 默认密码：`123456`

数据库脚本位于[数据/db.sql]，项目仅使用一张用户表`users`保存所有用户信息，表结构如下：
- `id`：主键，自增
- `username`：用户名，唯一
- `password`：密码
- `phoneNumber`：手机号，唯一
- `gender`：性别
- `isMember`：是否是会员，默认值为 `0`

## 3. 串联方式
- Unity客户端与服务端之间通过TCP Socket + Google Protobuf自定义协议包进行通信。
- 服务端通过protobuf消息接收与回复客户端请求，通过SqlSugar访问和修改MySQL数据库。

## 4. 部署与执行方式
- Unity客户端：使用`Unity2022.3.62f2c1`版本打开[UserLogin]项目，运行客户端。
- 服务端：使用Visual Studio打开[UserLogin_Servers/UserLogin_Servers.slnx]解决方案，依次启动CenterServer、LoginServer项目。
- 数据库：根据数据库类型中的修改MySQL设置或相关文件，创建`userlogin`数据库，使用[数据/db.sql]文件创建表结构。

如需修改协议包，修改[数据/UserEntity.proto]文件，使用protobuf工具生成C#代码，替换[UserLogin_Servers/Network/Proto/UserEntity.cs]文件。

## 5. AI应用
- 快速生成客户端基本功能代码，如注册验证、服务端交互逻辑
- 在服务端添加数据库会员属性的控制逻辑
- 定位开发过程中出现的bug并提出修复建议
- 通过AI生图快速实现界面与UI原型
使用AI减少了重复编码与排查错误的时间，并且能够快速完成基础功能搭建，让项目开发与调试效率得到显著提升。
