# WebQQWeChat [![star this repo](http://github-svg-buttons.herokuapp.com/star.svg?user=huoshan12345&repo=iQQ.Net&style=flat&background=1081C1)](https://github.com/huoshan12345/iQQ.Net) [![fork this repo](http://github-svg-buttons.herokuapp.com/fork.svg?user=huoshan12345&repo=iQQ.Net&style=flat&background=1081C1)](https://github.com/huoshan12345/iQQ.Net/fork) [![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/huoshan12345/iQQ.Net/blob/master/LICENSE.TXT)

这个项目是[webQQ](http://web2.qq.com/)）和[web微信](https://web.wechat.com/)相关协议的.net实现。基于此的一些功能组件。

## Useage
#### WebQQCore
移植于[iqq webqq-core](https://github.com/im-qq/webqq-core.git)，webqq协议的C#实现。
基于.net core开发，可运行于win、linux、mac等平台
![webqqcore-ubuntu](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webqqcore-ubuntu.png)

#### WebWeChat(未完成)
仿照WebQQCore的架构，web微信协议的C#实现。
基于.net core开发，可运行于win、linux、mac等平台

#### BatchHangQQ
一个winform的简单批量挂qq程序，依赖于WebQQCore
基于.net framework开发，可运行于win平台（注：由于目前还不能直接引用.net core的程序集，所以此处引用的是dll）
![BatchHangQQ](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/BatchHangQQ.png)

### Develop
Visual Studio 2015 is recommended.
