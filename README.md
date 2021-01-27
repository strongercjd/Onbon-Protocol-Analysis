# 仰邦协议解析工具说明V0.1
# 1.功能描述

此工具用于解析仰邦对外开放的协议，包括字库卡协议，6代字库动态区协议，6代图文动态区协议

| 协议       | 适用的控制卡型号(包括停产的型号) |
| ---------- | -------------------------------- |
| 字库卡协议 | 5K1 5K2 5MK 5KX<br>6K1 6K2 6K3<br>6K1-YY 6K2-YY 6K3-YY |
| 6代字库动态区 | 6Q3 6Q3L |
| 6代图文动态区 | 6E1 6E2 6E3 6E1X 6E2X<br>6Q1 6Q2 6Q2L <BR>6Q3 6Q3L |

使用该工具的流程，使用仰邦动态区开发工具，向控制发送显示内容，使用串口或者网口抓取数据包，将数据包复制进入此工具，此工具将会自动解析协议内容

解析数据的数据格式：以字节为单位的十六进制形式，小于0x10的数据，比如5，必须是05，不能是5

数据间隔可以是空格，逗号，冒号，下划线，连接线或无间隔，等等都是支持的

A5 00 01 02 03 04 05 06 07 08 09 5a

A5,00,01,02,03,04,05,06,07,08,09 5a

A5:00:01:02:03:04:05;06:07:08:09:5a

A5_00_01_02_03_04_05_06_07_08_09_5a

A5-00-01-02-03-04-05-06-07-08-09-5a

A5000102030405060708095a

混合间隔模式也是支持的

A501:02-030405*0607,08,095a



联系开发者

微信


![](https://gitee.com/strongercjd/Onbon-Protocol-Analysis/raw/master/image/006.jpg)

# 2.协议区分

字库卡协议


![](https://gitee.com/strongercjd/Onbon-Protocol-Analysis/raw/master/image/001.png)

6代字库动态区协议


![](https://gitee.com/strongercjd/Onbon-Protocol-Analysis/raw/master/image/005.png)

6代图文动态区协议


![](https://gitee.com/strongercjd/Onbon-Protocol-Analysis/raw/master/image/004.png)

# 3.抓包说明

网口抓包

百度：wireshark


![](https://gitee.com/strongercjd/Onbon-Protocol-Analysis/raw/master/image/003.png)

串口抓包

百度：串口抓包


![](https://gitee.com/strongercjd/Onbon-Protocol-Analysis/raw/master/image/002.png)
