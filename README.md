# iQQ.Net [![star this repo](http://github-svg-buttons.herokuapp.com/star.svg?user=huoshan12345&repo=iQQ.Net&style=flat&background=1081C1)](https://github.com/huoshan12345/iQQ.Net)[![fork this repo](http://github-svg-buttons.herokuapp.com/fork.svg?user=huoshan12345&repo=iQQ.Net&style=flat&background=1081C1)](https://github.com/huoshan12345/iQQ.Net/fork)[![license](https://img.shields.io/github/license/mashape/apistatus.svg?maxAge=2592000)](https://github.com/huoshan12345/iQQ.Net/blob/master/LICENSE.TXT)

这个项目是是基于iqq项目（其中的webqq-core）的.Net Core移植版。
和基于此的一些功能组件。

## iqq项目

* iqq ui: https://github.com/im-qq/iqq.git
* iqq webqq-core: https://github.com/im-qq/webqq-core.git

## Useage

#### WebQQCore（可用，已切换到二维码登录）
编译为dll即可引用到别的项目中；编译为exe则是一个控制台程序，可演示qq登录等基本功能。
* win平台（[基于.net core on Win](https://www.microsoft.com/net/core#windows)）  
![webqqcore-win](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webqqcore-win.png)

* linux平台（[基于.Net Core on Unbuntu](https://www.microsoft.com/net/core#ubuntu)）  
![webqqcore-ubuntu](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webqqcore-ubuntu.png)

* mac平台（[基于.Net Core on Mac](https://www.microsoft.com/net/core#macos)）  
![webqqcore-mac](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/webqqcore-mac.png)

#### BatchHangQQ（已切换到二维码登录）
一个基于winform的简单批量挂qq程序
* win平台（基于.net framework 4.5.1）  
（由于目前还不能直接引用.net core的程序集，所以此处引用的是dll）
![BatchHangQQ](https://raw.githubusercontent.com/huoshan12345/iQQ.Net/master/pic/BatchHangQQ.png)

## Develop
Visual Studio 2015 is recommended.