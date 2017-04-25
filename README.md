# WebQQWeChat [![star this repo](http://github-svg-buttons.herokuapp.com/star.svg?user=huoshan12345&repo=WebQQWeChat&style=flat&background=1081C1)](https://github.com/huoshan12345/WebQQWeChat) [![fork this repo](http://github-svg-buttons.herokuapp.com/fork.svg?user=huoshan12345&repo=WebQQWeChat&style=flat&background=1081C1)](https://github.com/huoshan12345/WebQQWeChat/fork) [![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/huoshan12345/WebQQWeChat/blob/master/LICENSE.TXT)

这个项目是[网页QQ](http://web2.qq.com/)和[网页微信](https://web.wechat.com/)相关协议的.net实现。基于此的一些功能组件。

## 主要模块介绍
### 1. [WebQQCore](https://github.com/huoshan12345/WebQQWeChat/tree/master/src/WebQQCore) [![.net core](https://img.shields.io/badge/color-1.0.1-ff69b4.svg?maxAge=2592000&label=.net%20core%20)](https://www.microsoft.com/net/download) [![.net](https://img.shields.io/badge/color-4.5.1-ff69b4.svg?maxAge=2592000&label=.net%20)](https://www.microsoft.com/net/download)
**移植于[iqq webqq-core](https://github.com/im-qq/webqq-core.git) 在此对作者表示衷心的感谢**  
网页QQ协议的C#实现。项目作为类库相当于网页QQ的sdk，内置了茉莉图灵机器人，和可用于开发聊天机器人、消息推送、群自动管理等软件。项目作为控制台程序可用于演示使用流程
![webqqcore-ubuntu](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webqqcore-ubuntu.png)

### 2. [WebWeChat](https://github.com/huoshan12345/WebQQWeChat/tree/master/src/WebWeChat)(未完成) [![.net core](https://img.shields.io/badge/color-1.0.1-ff69b4.svg?maxAge=2592000&label=.net%20core%20)](https://www.microsoft.com/net/download) [![.net](https://img.shields.io/badge/color-4.6.1-ff69b4.svg?maxAge=2592000&label=.net%20)](https://www.microsoft.com/net/download)
**网页微信协议主要参考了[WeixinBot](https://github.com/Urinx/WeixinBot) 在此对作者表示衷心的感谢**  
仿照WebQQCore的架构，网页微信协议的C#实现。项目作为类库相当于网页微信的sdk，项目作为控制台程序可用于演示使用流程  
![webwechat-win](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webwechat-win.png)

### 3. [BatchHangQQ](https://github.com/huoshan12345/WebQQWeChat/tree/master/src/BatchHangQQ) [![.net](https://img.shields.io/badge/color-4.5.1-ff69b4.svg?maxAge=2592000&label=.net%20)](https://www.microsoft.com/net/download)
一个winform的简单批量挂qq程序，依赖于WebQQCore，可运行于win平台（注：由于目前还不能直接引用.net core的程序集，所以此处引用的是dll）
![BatchHangQQ](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/BatchHangQQ.png)

## 开发环境
1. [Visual Studio 2017](https://www.visualstudio.com/zh-hans/downloads/)  
2. [.NET Core 1.0](https://www.microsoft.com/net/download)
