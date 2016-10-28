# WebQQWeChat [![star this repo](http://github-svg-buttons.herokuapp.com/star.svg?user=huoshan12345&repo=iQQ.Net&style=flat&background=1081C1)](https://github.com/huoshan12345/iQQ.Net) [![fork this repo](http://github-svg-buttons.herokuapp.com/fork.svg?user=huoshan12345&repo=iQQ.Net&style=flat&background=1081C1)](https://github.com/huoshan12345/iQQ.Net/fork) [![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/huoshan12345/iQQ.Net/blob/master/LICENSE.TXT)

这个项目是[webQQ](http://web2.qq.com/)和[web微信](https://web.wechat.com/)相关协议的.net实现。基于此的一些功能组件。

##主要模块介绍
####1. [WebQQCore](https://github.com/huoshan12345/WebQQWeChat/tree/master/src/WebQQCore)
移植于[iqq webqq-core](https://github.com/im-qq/webqq-core.git)，webqq协议的C#实现。
基于.net core开发，可跨平台运行
![webqqcore-ubuntu](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webqqcore-ubuntu.png)

####2. [WebWeChat](https://github.com/huoshan12345/WebQQWeChat/tree/master/src/WebWeChat)(未完成)
仿照WebQQCore的架构，web微信协议的C#实现。
基于.net core开发，可跨平台运行
![webwechat-win](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webwechat-win.png)

####3. [BatchHangQQ](https://github.com/huoshan12345/WebQQWeChat/tree/master/src/BatchHangQQ)
一个winform的简单批量挂qq程序，依赖于WebQQCore  
基于.net framework开发，可运行于win平台（注：由于目前还不能直接引用.net core的程序集，所以此处引用的是dll）
![BatchHangQQ](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/BatchHangQQ.png)

##开发环境
1. [Visual Studio 2015 with Update 3](https://www.visualstudio.com/zh-hans/downloads/)  
2. [.NET Core 1.0](https://www.microsoft.com/net/download)
