using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Onbon_Protocol_analysis
{
    public partial class Form1 : Form
    {
        class CProtolPart
        {
            public int bEnable = 0;
            public string para;//参数
            public UInt32 Leng;//数据长度
            public byte[] byteMemValue;//数值
            public string describe;//描述
        }
        class CPrototolAreaPart
        {

            public CProtolPart[] Prototol_Area_Part;
        }

        class Conbon_Protocol
        {
            public CProtolPart[] Prototol_Header;
            public CProtolPart[] Prototol_CMD;
            public int Area_Num;
            public CPrototolAreaPart[] Prototol_area_data;
            public CProtolPart Prototol_CRC;
        }
        Conbon_Protocol m_oonbon_Protocol = new Conbon_Protocol();


        struct PHY0_flag
        {
            public Int16 Rcv_state;                                    //!< 接收状态标志
            public Int16 RCV_data_num;                                 //!< 接收到个数位置
        };
        PHY0_flag PHY0_flag1;
        int Protocol_type;//0是字库卡  1是6代点阵协议   2是6代字库协议

        public string[][] Protol_header_str = new string[100][];
        int Protol_header_str_len;
        string[][] Protol_cmd_str = new string[100][];
        int Prototol_CMD_len;
        string[][] Protol_area_data_str = new string[100][];
        int Protol_area_data_str_len;
        string[][] Protol_crc_str = new string[100][];
        int Protol_crc_str_len;

        byte[] myarray;
        byte[] trsf_flg;


        struct SlistView_to_myarra
        {
            public UInt32 myarray_start;
            public UInt32 myarray_end;
        };
        SlistView_to_myarra[] listView_to_myarray;
        int data_value;
        string data;



        public void Font_Card_Protocol_string_init()
        {
			int num = 0;

            Protol_header_str[num++] = new string[] { "2", "屏地址", "也是屏号" };
            Protol_header_str[num++] = new string[] { "2", "源地址", "源地址" };
            Protol_header_str[num++] = new string[] { "5", "保留字节", "保留字节" };
            Protol_header_str[num++] = new string[] { "1", "显示模式", "显示模式"  };
            Protol_header_str[num++] = new string[] { "1", "设备类型", "设备类型"};
            Protol_header_str[num++] = new string[] { "1", "协议版本号", "协议版本号" };
            Protol_header_str[num++] = new string[] { "2", "数据域长度", "数据域长度" };
            Protol_header_str_len = num;

            num = 0;
            Protol_cmd_str[num++] = new string[] { "1", "命令分组", "命令分组" };
            Protol_cmd_str[num++] = new string[] { "1", "命令编号", "命令编号" };
            Protol_cmd_str[num++] = new string[] { "1", "控制是否回复", "控制是否回复" };
            Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
            Protol_cmd_str[num++] = new string[] { "1", "删除区域个数", "删除区域" };
            Protol_cmd_str[num++] = new string[] { "N", "删除区域ID", "删除区域ID" };
            Protol_cmd_str[num++] = new string[] { "1", "更新区域个数", "更新区域个数" };
            Prototol_CMD_len = num;

            num = 0;
            Protol_area_data_str[num++] = new string[] { "2", "区域数据长度", "区域数据长度" };
            Protol_area_data_str[num++] = new string[] { "1", "区域类型", "区域类型" };
            Protol_area_data_str[num++] = new string[] { "2", "X坐标", "X坐标" };
            Protol_area_data_str[num++] = new string[] { "2", "Y坐标", "Y坐标" };
            Protol_area_data_str[num++] = new string[] { "2", "区域宽度", "区域宽度" };
            Protol_area_data_str[num++] = new string[] { "2", "区域高度", "区域高度" };
            Protol_area_data_str[num++] = new string[] { "1", "动态区编号", "动态区编号" };
            Protol_area_data_str[num++] = new string[] { "1", "行间距", "行间距" };
            Protol_area_data_str[num++] = new string[] { "1", "动态区运行模式", "动态区运行模式" };
            Protol_area_data_str[num++] = new string[] { "2", "动态区超时时间", "超时时间" };
            Protol_area_data_str[num++] = new string[] { "1", "是否使能语音", "是否使能语音" };
            Protol_area_data_str[num++] = new string[] { "1", "发音人/发音次数", "Bit0-Bit3发音人，Bit4-Bit7播放次数" };
            Protol_area_data_str[num++] = new string[] { "1", "音量", "音量" };
            Protol_area_data_str[num++] = new string[] { "1", "语速", "语速" };
            Protol_area_data_str[num++] = new string[] { "4", "读音数据长度", "数据长度" };
            Protol_area_data_str[num++] = new string[] { "N", "读音数据", "读音数据" };
            Protol_area_data_str[num++] = new string[] { "1", "扩展位个数", "扩展位个数" };
            Protol_area_data_str[num++] = new string[] { "1", "扩展位保留位", "扩展位保留位" };
            Protol_area_data_str[num++] = new string[] { "1", "排版方式", "排版方式" };
            Protol_area_data_str[num++] = new string[] { "1", "字体对齐", "字体对齐方式" };
            Protol_area_data_str[num++] = new string[] { "1", "是否单行显示", "是否单行显示" };
            Protol_area_data_str[num++] = new string[] { "1", "是否自动换行", "是否自动换行" };
            Protol_area_data_str[num++] = new string[] { "1", "显示方式", "显示方式" };
            Protol_area_data_str[num++] = new string[] { "1", "退出方式", "退出方式" };
            Protol_area_data_str[num++] = new string[] { "1", "显示速度", "显示速度" };
            Protol_area_data_str[num++] = new string[] { "1", "停留时间", "停留时间" };
            Protol_area_data_str[num++] = new string[] { "4", "显示数据长度", "数据长度" };
            Protol_area_data_str[num++] = new string[] { "N", "显示数据", "显示数据" };
            Protol_area_data_str_len = num;

            num = 0;
            Protol_crc_str[num++] = new string[] { "2", "CRC校验", "CRC校验" };
            Protol_crc_str_len = num;


        }
        public Form1()
        {
            InitializeComponent();
            Protocol_selection.Items.Add("字库卡协议");
            Protocol_selection.Items.Add("6代点阵动态区协议");
            Protocol_selection.Items.Add("6代字库动态区协议");
            Protocol_selection.SelectedIndex = 0;
            Protocol_type = 0;
            Font_Card_Protocol_string_init();

        }
        public static int crc16(byte[] data, int size)
        {
            int crc = 0x0;
            byte data_t;
            int i = 0;
            int j = 0;
            if (data == null)
            {
                return 0;
            }
            for (j = 0; j < size; j++)
            {
                data_t = data[j];
                crc = (data_t ^ (crc));
                for (i = 0; i < 8; i++)
                {
                    if ((crc & 0x1) == 1)
                    {
                        crc = (crc >> 1) ^ 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            return crc;
        }


        public int refresh_data_listView()
        {
            UInt32 i, num, num1, num2, listView_row;

            i = 0;
            num = 0;
            num1 = 0;
            num2 = 0;
            listView_row = 0;

            listView_to_myarray = new SlistView_to_myarra[myarray.Length];


            this.data_listView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

            ListViewGroup group_data_header = new ListViewGroup();  //创建包头数据分组

            group_data_header.Header = "包头数据";  //设置组的标题。
            group_data_header.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
            this.data_listView.Groups.Add(group_data_header);    //把包头数据分组添加到listview中


            ListViewItem Protocol_data;

            for (num = 0; num < m_oonbon_Protocol.Prototol_Header.Length; num++)
            {
                if (m_oonbon_Protocol.Prototol_Header[num].bEnable == 1)
                {
                    listView_to_myarray[listView_row].myarray_start = i;

                    data = "";
                    Protocol_data = new ListViewItem();
                    Protocol_data.Group = group_data_header;
                    Protocol_data.Text = m_oonbon_Protocol.Prototol_Header[num].para;
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_Header[num].Leng; num1++)
                    {
                        data += m_oonbon_Protocol.Prototol_Header[num].byteMemValue[m_oonbon_Protocol.Prototol_Header[num].Leng - num1 - 1].ToString("X2");
                        i++;
                    }
                    Protocol_data.SubItems.Add(data);
                    Protocol_data.SubItems.Add(m_oonbon_Protocol.Prototol_Header[num].describe);
                    data_listView.Items.Add(Protocol_data);

                    listView_to_myarray[listView_row].myarray_end = i;
                    listView_row++;
                }
            }

            ListViewGroup group_cmd = new ListViewGroup();  //创建命令分组
            group_cmd.Header = "命令";  //设置组的标题。
            group_cmd.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
            this.data_listView.Groups.Add(group_cmd);    //把命令分组添加到listview中
            for (num = 0; num < m_oonbon_Protocol.Prototol_CMD.Length; num++)
            {
                if (m_oonbon_Protocol.Prototol_CMD[num].bEnable == 1)
                {
                    listView_to_myarray[listView_row].myarray_start = i;

                    data = "";
                    Protocol_data = new ListViewItem();
                    Protocol_data.Group = group_cmd;
                    Protocol_data.Text = m_oonbon_Protocol.Prototol_CMD[num].para;
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                    {
                        data += m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[m_oonbon_Protocol.Prototol_CMD[num].Leng - num1 - 1].ToString("X2");
                        i++;
                    }
                    Protocol_data.SubItems.Add(data);
                    Protocol_data.SubItems.Add(m_oonbon_Protocol.Prototol_CMD[num].describe);
                    data_listView.Items.Add(Protocol_data);

                    listView_to_myarray[listView_row].myarray_end = i;
                    listView_row++;
                }
            }


            for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
            {
                ListViewGroup grou_area_data = new ListViewGroup();  //创建命令分组
                grou_area_data.Header = "区域" + num.ToString() + "数据格式";  //设置组的标题。
                grou_area_data.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                this.data_listView.Groups.Add(grou_area_data);    //把命令分组添加到listview中
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part.Length; num1++)
                {
                    CProtolPart ProtolPart = new CProtolPart();
                    ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                    if (ProtolPart.bEnable == 1)
                    {
                        listView_to_myarray[listView_row].myarray_start = i;

                        data = "";
                        Protocol_data = new ListViewItem();
                        Protocol_data.Group = grou_area_data;
                        Protocol_data.Text = ProtolPart.para;
                        for (num2 = 0; num2 < ProtolPart.Leng; num2++)
                        {
                            data += ProtolPart.byteMemValue[ProtolPart.Leng - num2 - 1].ToString("X2");
                            i++;
                        }
                        Protocol_data.SubItems.Add(data);
                        Protocol_data.SubItems.Add(ProtolPart.describe);
                        data_listView.Items.Add(Protocol_data);

                        listView_to_myarray[listView_row].myarray_end = i;
                        listView_row++;
                    }
                }
            }

            ListViewGroup grou_CRC = new ListViewGroup();  //创建命令分组
            grou_CRC.Header = "CRC校验";  //设置组的标题。
            grou_CRC.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
            this.data_listView.Groups.Add(grou_CRC);    //把命令分组添加到listview中

            if (m_oonbon_Protocol.Prototol_CRC.bEnable == 1)
            {
                listView_to_myarray[listView_row].myarray_start = i;

                data = "";
                Protocol_data = new ListViewItem();
                Protocol_data.Group = grou_CRC;
                Protocol_data.Text = m_oonbon_Protocol.Prototol_CRC.para;
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CRC.Leng; num1++)
                {
                    data += m_oonbon_Protocol.Prototol_CRC.byteMemValue[m_oonbon_Protocol.Prototol_CRC.Leng - num1 - 1].ToString("X2");
                    i++;
                }
                Protocol_data.SubItems.Add(data);
                Protocol_data.SubItems.Add(m_oonbon_Protocol.Prototol_CRC.describe);
                data_listView.Items.Add(Protocol_data);

                listView_to_myarray[listView_row].myarray_end = i;
                listView_row++;
            }

            this.data_listView.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            return 0;
        }

        public int Font_Card_Protocol_Dynamic(byte[] myarray)
        {
            UInt32 i,num, num1, num2, num3;
            string data_str;

            i = 0;
            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            m_oonbon_Protocol.Prototol_Header = new CProtolPart[Protol_header_str_len];
            for (num = 0; num < Protol_header_str_len; num++)
            {
                m_oonbon_Protocol.Prototol_Header[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_Header[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_Header[num].Leng = Convert.ToUInt32(Protol_header_str[num][0]);
                m_oonbon_Protocol.Prototol_Header[num].para = Protol_header_str[num][1];
                m_oonbon_Protocol.Prototol_Header[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_Header[num].Leng];
                m_oonbon_Protocol.Prototol_Header[num].describe = Protol_header_str[num][2];
            }

            /*包头数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_Header.Length; num++)
            {
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_Header[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                    m_oonbon_Protocol.Prototol_Header[num].byteMemValue[num1] = myarray[i++];
                }
            }

            if ((myarray[i] != 0xa3) || (myarray[i + 1] != 0x06))
            {
                MessageBox.Show("目前版本不支持"+ myarray[i].ToString()+" "+ myarray[i+1].ToString());
                return 0;
            }

            m_oonbon_Protocol.Prototol_CMD = new CProtolPart[Prototol_CMD_len];
            for (num = 0; num < Prototol_CMD_len; num++)
            {
                m_oonbon_Protocol.Prototol_CMD[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_CMD[num].para = Protol_cmd_str[num][1];
                if (Protol_cmd_str[num][0] != "N")
                {
                    m_oonbon_Protocol.Prototol_CMD[num].Leng = Convert.ToUInt32(Protol_cmd_str[num][0]);
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_CMD[num].Leng];
                }
                else
                {
                    m_oonbon_Protocol.Prototol_CMD[num].Leng = 0XFFFFFFFF;
                }
                m_oonbon_Protocol.Prototol_CMD[num].describe = Protol_cmd_str[num][2];
            }

            /*命令数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_CMD.Length;)
            {
                if (m_oonbon_Protocol.Prototol_CMD[num].para == "删除区域个数")
                {
                    /*删除区域个数*/
                    num2 = 0;
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                    {
                        m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                        num2 = num2 + m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1];
                    }
                    num++;

                    /*删除区域ID*/
                    if (num2 != 0)
                    {
                        m_oonbon_Protocol.Prototol_CMD[num].Leng = num2;
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_CMD[num].Leng];
                        for (num1 = 0; num1 < num2; num1++)
                        {
                            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                        }
                    }
                    num++;
                }
                else
                {
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                    {
                        m_oonbon_Protocol.Prototol_CMD[num].bEnable =1;
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                    }
                    if (m_oonbon_Protocol.Prototol_CMD[num].para == "更新区域个数")
                    {
                        m_oonbon_Protocol.Area_Num = m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0];
                    }
                    num++;
                }
            }

            m_oonbon_Protocol.Prototol_area_data = new CPrototolAreaPart[m_oonbon_Protocol.Area_Num];
            for (num =0;num< m_oonbon_Protocol.Area_Num; num++)
            {
                m_oonbon_Protocol.Prototol_area_data[num] = new CPrototolAreaPart();
                m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part = new CProtolPart[Protol_area_data_str_len];
                for (num1 = 0; num1 < Protol_area_data_str_len; num1++)
                {
                    m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1] = new CProtolPart();
                    m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].para = Protol_area_data_str[num1][1];
                    if (Protol_area_data_str[num1][0] != "N")
                    {
                        m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].Leng = Convert.ToUInt32(Protol_area_data_str[num1][0]);
                        m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].byteMemValue = new byte[m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].Leng];
                    }
                    else
                    {
                        m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].Leng = 0XFFFFFFFF;
                    }
                    m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].describe = Protol_area_data_str[num1][2];
                    m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].bEnable = 0;
                }
            }


            /*区域数据格式*/
            for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
            {
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part.Length;)
                {
                    CProtolPart ProtolPart = new CProtolPart();
                    ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];

                    if (ProtolPart.para == "显示数据长度")
                    {
                        /*显示数据长度*/
                        ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                        ProtolPart.bEnable = 1;
                        ProtolPart.byteMemValue[0] = myarray[i++];
                        ProtolPart.byteMemValue[1] = myarray[i++];
                        ProtolPart.byteMemValue[2] = myarray[i++];
                        ProtolPart.byteMemValue[3] = myarray[i++];
                        num1++;

                        num3 = (UInt32)(((ProtolPart.byteMemValue[3] & 0xff) << 24) |
                                        ((ProtolPart.byteMemValue[2] & 0xff) << 16) |
                                        ((ProtolPart.byteMemValue[1] & 0xff) << 8) |
                                        ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                        /*显示数据*/
                        m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].byteMemValue = new byte[num3];
                        ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                        ProtolPart.bEnable = 1;
                        ProtolPart.Leng = num3;
                        data_str = "";
                        for (num2 = 0; num2 < num3; )
                        {
                            if (myarray[i] < 0x81)
                            {
                                ProtolPart.byteMemValue[num3 - num2 - 1] = myarray[i];
                                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                                byte[] byteArray = new byte[] { (byte)myarray[i++] };
                                string strCharacter = asciiEncoding.GetString(byteArray);
                                data_str = data_str + strCharacter;
                                num2++;
                            }
                            else
                            {
                                ProtolPart.byteMemValue[num3 - num2 - 1] = myarray[i];
                                num2++;
                                ProtolPart.byteMemValue[num3 - num2 - 1] = myarray[i+1];
                                num2++;
                                byte[] bytes = new byte[2];
                                bytes[0] = myarray[i++];
                                bytes[1] = myarray[i++];
                                System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
                                data_str = data_str + chs.GetString(bytes);
                            }
                                
                        }
                        ProtolPart.describe = data_str;
                        num1++;
                    }

                    if (ProtolPart.para == "扩展位个数")
                    {
                        /*判断扩展位个数*/
                        ProtolPart.byteMemValue[0] = myarray[i++];
                        ProtolPart.bEnable = 1;
                        num1++;
                        if (ProtolPart.byteMemValue[0] == 0)//无扩展位
                        {
                            num1 += 2;
                            continue;
                        }
                        else
                        {
                            if (ProtolPart.byteMemValue[0] > 1)//扩展位大于1
                            {
                                MessageBox.Show("目前版本不支持扩展位大于1的情况");
                                return 0;
                            }
                            else
                            {
                                /*排版方式*/
                                ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                                ProtolPart.byteMemValue[0] = myarray[i++];
                                ProtolPart.bEnable = 1;
                                num1++;
                                continue;
                            }

                        }
                    }
                    if (ProtolPart.para == "是否使能语音")
                    {
                        /*是否使能语音*/
                        ProtolPart.byteMemValue[0] = myarray[i++];
                        ProtolPart.bEnable = 1;
                        num1++;
                        if (ProtolPart.byteMemValue[0] == 0)
                        {
                            num1 += 5;
                        }
                        if (ProtolPart.byteMemValue[0] == 1)
                        {
                            /*发音人/发音次数*/
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;
                            /*音量*/
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;
                            /*语速*/
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;
                        }
                        if (ProtolPart.byteMemValue[0] == 2)
                        {
                            /*发音人/发音次数*/
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;

                            /*音量*/
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;

                            /*语速*/
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;

                            /*读音数据长度*/
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.byteMemValue[1] = myarray[i++];
                            ProtolPart.byteMemValue[2] = myarray[i++];
                            ProtolPart.byteMemValue[3] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;

                            num3 = (UInt32)(((ProtolPart.byteMemValue[3] & 0xff) << 24) |
                                            ((ProtolPart.byteMemValue[2] & 0xff) << 16) |
                                            ((ProtolPart.byteMemValue[1] & 0xff) << 8) |
                                            ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                            /*读音数据*/
                            m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1].byteMemValue = new byte[num3];
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.bEnable = 1;
                            ProtolPart.Leng = num3;
                            data_str = "";
                            for (num2 = 0; num2 < num3;)
                            {
                                if (myarray[i] < 0x81)
                                {
                                    ProtolPart.byteMemValue[num3 - num2 - 1] = myarray[i];
                                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                                    byte[] byteArray = new byte[] { (byte)myarray[i++] };
                                    string strCharacter = asciiEncoding.GetString(byteArray);
                                    data_str = data_str + strCharacter;
                                    num2++;
                                }
                                else
                                {
                                    ProtolPart.byteMemValue[num3 - num2 - 1] = myarray[i];
                                    num2++;
                                    ProtolPart.byteMemValue[num3 - num2 - 1] = myarray[i + 1];
                                    num2++;
                                    byte[] bytes = new byte[2];
                                    bytes[0] = myarray[i++];
                                    bytes[1] = myarray[i++];
                                    System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
                                    data_str = data_str + chs.GetString(bytes);
                                }
                            }
                            ProtolPart.describe = data_str;
                            num1++;
                        }
                    }
                    else
                    {
                        if (num1 < m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part.Length)
                        {
                            for (num2 = 0; num2 < ProtolPart.Leng; num2++)
                            {
                                ProtolPart.byteMemValue[num2] = myarray[i++];
                            }
                            ProtolPart.bEnable = 1;
                            num1++;
                        } 
                    }

                }
            }

            m_oonbon_Protocol.Prototol_CRC = new CProtolPart();
            m_oonbon_Protocol.Prototol_CRC.para = Protol_crc_str[0][1];
            m_oonbon_Protocol.Prototol_CRC.Leng = Convert.ToUInt32(Protol_crc_str[0][0]);
            m_oonbon_Protocol.Prototol_CRC.byteMemValue = new byte[m_oonbon_Protocol.Prototol_CRC.Leng];
            m_oonbon_Protocol.Prototol_CRC.describe = Protol_crc_str[0][2];
            m_oonbon_Protocol.Prototol_CRC.bEnable = 1;
            for (num = 0; num < m_oonbon_Protocol.Prototol_CRC.Leng; num++)
            {
                m_oonbon_Protocol.Prototol_CRC.byteMemValue[num] = myarray[i++];
            }

            refresh_data_listView();

            return 0;
        }
        public int Font_Card_Protocol_Dynamic1(byte[] myarray)
        {
            int i,area_num,num,num1,num2;

            i = 0;
            num = 0;
            num1 = 0;
            num2 = 0;
            area_num = 0;

            ListViewGroup group_Protol_header_str = new ListViewGroup();  //创建包头数据分组

            group_Protol_header_str.Header = "包头数据";  //设置组的标题。
            group_Protol_header_str.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
            this.data_listView.Groups.Add(group_Protol_header_str);    //把包头数据分组添加到listview中


            ListViewItem Protocol_data;

            /*包头数据*/
            for(num = 0;num< Protol_header_str_len; num++)
            {
                Protocol_data = new ListViewItem();
                Protocol_data.Group = group_Protol_header_str;
                Protocol_data.Text = Protol_header_str[num][1];
                if (Protol_header_str[num][0] != "N")
                {
                    int len = Convert.ToInt32(Protol_header_str[num][0]);
                    data = "";
                    data_value = 0;
                    for (num1 = 0; num1 < len; num1++)
                    {
                        data += myarray[i + len - num1 - 1].ToString("X2");
                        data_value = data_value | (int)((myarray[i + len -num1 - 1] & 0xff) << (8*(len - num1 - 1)));
                    }
                    i += len;
                }

                Protocol_data.SubItems.Add(data);
                Protocol_data.SubItems.Add(Protol_header_str[num][2]);
                data_listView.Items.Add(Protocol_data);
            }

            ListViewGroup group_cmd = new ListViewGroup();  //创建命令分组
            group_cmd.Header = "命令";  //设置组的标题。
            group_cmd.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
            this.data_listView.Groups.Add(group_cmd);    //把命令分组添加到listview中

            if ((myarray[i] != 0xa3) || (myarray[i + 1] != 0x06))
            {
                for (num = 0; num < 2; num++)
                {
                    Protocol_data = new ListViewItem();
                    Protocol_data.Group = group_cmd;
                    Protocol_data.Text = Protol_cmd_str[num][1];

                    data = myarray[i].ToString("X2");
                    data_value = (int)myarray[i];
                    i += 1;

                    Protocol_data.ForeColor = Color.Red;
                    Protocol_data.SubItems.Add(data);
                    Protocol_data.SubItems.Add(Protol_cmd_str[num][2]);
                    data_listView.Items.Add(Protocol_data);
                }
                MessageBox.Show("目前版本只支持协议发送动态区命令");
                return 0;
            }

            /*命令数据*/
            for (num = 0; num < Prototol_CMD_len; num++)
            {
                Protocol_data = new ListViewItem();
                Protocol_data.Group = group_cmd;
                Protocol_data.Text = Protol_cmd_str[num][1];

                if (Protol_cmd_str[num][0] != "N")
                {
                    int len = Convert.ToInt32(Protol_cmd_str[num][0]);
                    data = "";
                    data_value = 0;
                    for (num1 = 0; num1 < len; num1++)
                    {
                        data += myarray[i + len - num1 - 1].ToString("X2");
                        data_value = data_value | (int)((myarray[i + len -num1 - 1] & 0xff) << (8 * (len - num1 - 1)));
                    }
                    i += len;
                }
                Protocol_data.SubItems.Add(data);
                Protocol_data.SubItems.Add(Protol_cmd_str[num][2]);
                data_listView.Items.Add(Protocol_data);

                if (Protol_cmd_str[num][1] == "更新区域个数")
                {
                    area_num = data_value;
                }

                if (Protol_cmd_str[num][1] == "删除区域个数")
                {
                    num++;
                    /*删除区域ID*/
                    if (data_value != 0)
                    {
                        
                        for (num1 = 1; num1 < data_value + 1; num1++)
                        {
                            Protocol_data = new ListViewItem();
                            Protocol_data.Group = group_cmd;
                            Protocol_data.Text = Protol_cmd_str[num][1];
                            data = myarray[i++].ToString("X2");
                            Protocol_data.SubItems.Add(data);
                            Protocol_data.SubItems.Add(Protol_cmd_str[num][2]);
                            data_listView.Items.Add(Protocol_data);
                        }
                    } 
                }
            }

            /*数据格式*/
            for (num = 0; num < area_num; num++)
            {
                ListViewGroup grou_Protol_area_data_str = new ListViewGroup();  //创建命令分组
                grou_Protol_area_data_str.Header = "区域" + num.ToString() + "数据格式";  //设置组的标题。
                grou_Protol_area_data_str.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                this.data_listView.Groups.Add(grou_Protol_area_data_str);    //把命令分组添加到listview中

                for (num1 = 0; num1 < Protol_area_data_str_len; )
                {


                    if (Protol_area_data_str[num1][1] == "扩展位保留位")
                    {
                        /*判断扩展位个数*/
                        if (data_value == 0)//无扩展位
                        {
                            num1 = num1 + 2;
                        }
                        else
                        {
                            /*排版方式*/
                            Protocol_data = new ListViewItem();
                            Protocol_data.Group = grou_Protol_area_data_str;
                            Protocol_data.Text = Protol_area_data_str[num1][1];
                            data = myarray[i++].ToString("X2");
                            Protocol_data.SubItems.Add(data);
                            Protocol_data.SubItems.Add(Protol_area_data_str[num1][2]);
                            data_listView.Items.Add(Protocol_data);
                            num1++;
                            if (data_value > 1)//扩展位大于1
                            {
                                MessageBox.Show("目前版本不支持扩展位大于1的情况");
                                return 0;
                            }
                        }
                    }

                    if (Protol_area_data_str[num1][1] == "发音人/发音次数")
                    {
                        /*判断是否有语音*/
                        if (data_value == 0)//无语音
                        {
                            num1 = num1 + 5;
                        }
                        else//存在语音
                        {
                            /*发音人/发音次数*/
                            Protocol_data = new ListViewItem();
                            Protocol_data.Group = grou_Protol_area_data_str;
                            Protocol_data.Text = Protol_area_data_str[num1][1];
                            data = myarray[i++].ToString("X2");
                            Protocol_data.SubItems.Add(data);
                            Protocol_data.SubItems.Add(Protol_area_data_str[num1][2]);
                            data_listView.Items.Add(Protocol_data);
                            num1++;

                            /*音量*/
                            Protocol_data = new ListViewItem();
                            Protocol_data.Group = grou_Protol_area_data_str;
                            Protocol_data.Text = Protol_area_data_str[num1][1];
                            data = myarray[i++].ToString("X2");
                            Protocol_data.SubItems.Add(data);
                            Protocol_data.SubItems.Add(Protol_area_data_str[num1][2]);
                            data_listView.Items.Add(Protocol_data);
                            num1++;

                            /*语速*/
                            Protocol_data = new ListViewItem();
                            Protocol_data.Group = grou_Protol_area_data_str;
                            Protocol_data.Text = Protol_area_data_str[num1][1];
                            data = myarray[i++].ToString("X2");
                            Protocol_data.SubItems.Add(data);
                            Protocol_data.SubItems.Add(Protol_area_data_str[num1][2]);
                            data_listView.Items.Add(Protocol_data);
                            num1++;

                            if (data_value == 2)//播放sounddata内容
                            {
                                /*读音数据长度*/
                                Protocol_data = new ListViewItem();
                                Protocol_data.Group = grou_Protol_area_data_str;
                                Protocol_data.Text = Protol_area_data_str[num1][1];
                                data = myarray[i + 3].ToString("X2") +
                                       myarray[i + 2].ToString("X2") +
                                       myarray[i + 1].ToString("X2") +
                                       myarray[i + 0].ToString("X2");
                                data_value = (int)(((myarray[i + 3] & 0xff) << 24) |
                                                   ((myarray[i + 2] & 0xff) << 16) |
                                                   ((myarray[i + 1] & 0xff) << 8) |
                                                   ((myarray[i + 0] & 0xff) << 0));
                                i += 4;
                                Protocol_data.SubItems.Add(data);
                                Protocol_data.SubItems.Add(Protol_area_data_str[num1][2]);
                                data_listView.Items.Add(Protocol_data);
                                num1++;

                                /*读音数据*/
                                Protocol_data = new ListViewItem();
                                Protocol_data.Group = grou_Protol_area_data_str;
                                Protocol_data.Text = Protol_area_data_str[num1][1];

                                if (Protol_area_data_str[num1][0] == "N")
                                {
                                    data = "";
                                    for (num2 = 0; num2 < data_value; num2++)
                                    {
                                        data = data + myarray[i++].ToString("X2");
                                    }
                                }
                                Protocol_data.SubItems.Add(data);

                                if (Protol_area_data_str[num1][0] == "N")
                                {
                                    i = i - data_value;
                                    data = "";
                                    for (num2 = 0; num2 < data_value;)
                                    {
                                        if (myarray[i] < 0x81)
                                        {
                                            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                                            byte[] byteArray = new byte[] { (byte)myarray[i++] };
                                            string strCharacter = asciiEncoding.GetString(byteArray);
                                            data = data + strCharacter;
                                            num2++;
                                        }
                                        else
                                        {
                                            byte[] bytes = new byte[2];
                                            bytes[0] = myarray[i++];
                                            bytes[1] = myarray[i++];
                                            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
                                            data = data + chs.GetString(bytes);
                                            num2 = num2 + 2;
                                        }
                                    }
                                    Protocol_data.SubItems.Add(data);
                                }
                                else
                                {
                                    Protocol_data.SubItems.Add(Protol_area_data_str[num1][2]);
                                }
                                data_listView.Items.Add(Protocol_data);
                                num1++;
                            }
                            else
                            {
                                num1 = num1 + 2;
                            }
                        }
                    }



                    Protocol_data = new ListViewItem();
                    Protocol_data.Group = grou_Protol_area_data_str;
                    Protocol_data.Text = Protol_area_data_str[num1][1];


                    if (Protol_area_data_str[num1][0] != "N")
                    {
                        int len = Convert.ToInt32(Protol_area_data_str[num1][0]);
                        data = "";
                        data_value = 0;
                        for (num2 = 0; num2 < len; num2++)
                        {
                            data += myarray[i + len - num2 - 1].ToString("X2");
                            data_value = data_value | (int)((myarray[i + len - num2 - 1] & 0xff) << (8 * (len - num2 - 1)));
                        }
                        i += len;
                    }
                    if (Protol_area_data_str[num1][0] == "N")
                    {
                        data = "";
                        for (num2 = 0; num2 < data_value; num2++)
                        {
                            data = data + myarray[i++].ToString("X2");
                        }
                    }
                    Protocol_data.SubItems.Add(data);

                    if (Protol_area_data_str[num1][0] == "N")
                    {
                        i = i - data_value;
                        data = "";
                        for (num2 = 0; num2 < data_value; )
                        {
                            if (myarray[i] < 0x81)
                            {
                                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                                byte[] byteArray = new byte[] { (byte)myarray[i++] };
                                string strCharacter = asciiEncoding.GetString(byteArray);
                                data = data + strCharacter;
                                num2++;
                            }
                            else
                            {
                                byte[] bytes = new byte[2];
                                bytes[0] = myarray[i++];
                                bytes[1] = myarray[i++];
                                System.Text.Encoding chs = System.Text.Encoding.GetEncoding("gb2312");
                                data = data + chs.GetString(bytes);
                                num2 = num2 + 2;
                            }
                        }
                        Protocol_data.SubItems.Add(data);
                    }
                    else
                    {
                        Protocol_data.SubItems.Add(Protol_area_data_str[num1][2]);
                    }  
                    data_listView.Items.Add(Protocol_data);
                    num1++;

                }
            }


            ListViewGroup grou_CRC = new ListViewGroup();  //创建命令分组
            grou_CRC.Header = "CRC校验";  //设置组的标题。
            grou_CRC.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
            this.data_listView.Groups.Add(grou_CRC);    //把命令分组添加到listview中

            /*CRC校验*/
            num = 0;
            Protocol_data = new ListViewItem();
            Protocol_data.Group = grou_CRC;
            Protocol_data.Text = Protol_crc_str[num][1];
            data = myarray[i + 1].ToString("X2") +
                   myarray[i + 0].ToString("X2");
            data_value = (int)(((myarray[i + 1] & 0xff) << 8) |
                               ((myarray[i + 0] & 0xff) << 0));
            Protocol_data.SubItems.Add(data);

            i = i - 2;
            int mycrc = crc16(myarray, PHY0_flag1.RCV_data_num - 2);
            if (mycrc == data_value)
            {
                Protocol_data.SubItems.Add(Protol_crc_str[num][2]);
            }
            else
            {
                if (data_value == 0XFFFF)//不进行CRC校验
                {
                    Protocol_data.SubItems.Add(Protol_crc_str[num][4]);
                }
                else
                {
                    Protocol_data.ForeColor = Color.Red;
                    Protocol_data.SubItems.Add(Protol_crc_str[num][3]);
                }
                    
            }  
            data_listView.Items.Add(Protocol_data);

            return 0;
        }
        public int Data_deal_with(byte[] data, int size)
        {
            PHY0_flag1.Rcv_state = 0;
            PHY0_flag1.RCV_data_num = 0;

			myarray = new byte[size];
			trsf_flg = new byte[size];

            UInt32 i;
            byte data_t;
            UInt32 j = 0;
            if (data == null)
            {
                return 0;
            }
            for (j = 0; j < size; j++)
            {
                data_t = data[j];
                if (data_t == 0XA5)
                {
                    if ((PHY0_flag1.Rcv_state == 2) || (PHY0_flag1.Rcv_state == 3) || (PHY0_flag1.Rcv_state == 4))
                    {
                        myarray[PHY0_flag1.RCV_data_num] = data_t;
                        trsf_flg[PHY0_flag1.RCV_data_num] = 1;
                        PHY0_flag1.RCV_data_num++;
                        PHY0_flag1.Rcv_state = 4;
                    }
                    else
                    {
                        PHY0_flag1.Rcv_state = 1;
                    }


                }
                else
                {
                    if (PHY0_flag1.Rcv_state != 0)
                    {
                        switch (data_t)
                        {
                            case 0X5A:
                                if (j + 1 < size)
                                {
                                    myarray[PHY0_flag1.RCV_data_num] = data_t;
                                    trsf_flg[PHY0_flag1.RCV_data_num] = 1;
                                    PHY0_flag1.RCV_data_num++;
                                    PHY0_flag1.Rcv_state = 4;
                                }
                                break;
                            case 0XA6:
                                PHY0_flag1.Rcv_state = 2;//进入0xA6转义字节状态
                                break;
                            case 0X5B:
                                PHY0_flag1.Rcv_state = 3;//进入0x5B转义字节状态
                                break;
                            case 0X01:
                                if (PHY0_flag1.Rcv_state == 2)
                                {
                                    myarray[PHY0_flag1.RCV_data_num] = 0XA6;
                                    trsf_flg[PHY0_flag1.RCV_data_num] = 2;
                                    PHY0_flag1.RCV_data_num++;
                                    PHY0_flag1.Rcv_state = 1;
                                }
                                else
                                {
                                    if (PHY0_flag1.Rcv_state == 3)
                                    {
                                        myarray[PHY0_flag1.RCV_data_num] = 0X5B;
                                        trsf_flg[PHY0_flag1.RCV_data_num] = 2;
                                        PHY0_flag1.RCV_data_num++;
                                        PHY0_flag1.Rcv_state = 1;
                                    }
                                    else
                                    {
                                        myarray[PHY0_flag1.RCV_data_num] = 0x01;
                                        PHY0_flag1.RCV_data_num++;
                                        PHY0_flag1.Rcv_state = 4;
                                    }
                                }
                                break;
                            case 0X02:
                                if (PHY0_flag1.Rcv_state == 2)
                                {
                                    myarray[PHY0_flag1.RCV_data_num] = 0XA5;
                                    trsf_flg[PHY0_flag1.RCV_data_num] = 2;
                                    PHY0_flag1.RCV_data_num++;
                                    PHY0_flag1.Rcv_state = 1;
                                }
                                else
                                {
                                    if (PHY0_flag1.Rcv_state == 3)
                                    {
                                        myarray[PHY0_flag1.RCV_data_num] = 0X5A;
                                        trsf_flg[PHY0_flag1.RCV_data_num] = 2;
                                        PHY0_flag1.RCV_data_num++;
                                        PHY0_flag1.Rcv_state = 1;
                                    }
                                    else
                                    {
                                        myarray[PHY0_flag1.RCV_data_num] = 0x02;
                                        PHY0_flag1.RCV_data_num++;
                                        PHY0_flag1.Rcv_state = 4;
                                    }
                                }
                                break;
                            default:
                                if (PHY0_flag1.Rcv_state == 3)
                                {
                                    trsf_flg[PHY0_flag1.RCV_data_num] = 1;
                                    myarray[PHY0_flag1.RCV_data_num] = 0X5B;
                                    PHY0_flag1.RCV_data_num++;
                                }
                                else
                                {
                                    if (PHY0_flag1.Rcv_state == 2)
                                    {
                                        trsf_flg[PHY0_flag1.RCV_data_num] = 1;
                                        myarray[PHY0_flag1.RCV_data_num] = 0X6A;
                                        PHY0_flag1.RCV_data_num++;
                                    }
                                    else
                                    {
                                        trsf_flg[PHY0_flag1.RCV_data_num] = 0;
                                    }
                                }
                                myarray[PHY0_flag1.RCV_data_num] = data_t;
                                PHY0_flag1.RCV_data_num++;
                                PHY0_flag1.Rcv_state = 4;
                                break;
                        }

                    }
                }
            }

            string str, str1;
            str = "";
            for (i = 0; i < PHY0_flag1.RCV_data_num; i++)
            {
                if (trsf_flg[i] == 1)
                {
                    data_after_transform_richTextBox.SelectionColor = Color.Red;
                    j = 1;
                }
                else
                {
                    if (trsf_flg[i] == 2)
                    {
                        data_after_transform_richTextBox.SelectionColor = Color.Blue;
                    }
                    else
                    {
                        data_after_transform_richTextBox.SelectionColor = Color.Black;
                    }

                }

                str1 = myarray[i].ToString("x");
                str = (str1.Length == 1 ? "0" + str1 : str1);
                if (i == 0)
                {
                    str = str.ToUpper();
                }
                else
                {
                    str = " " + str.ToUpper();

                }
                data_after_transform_richTextBox.AppendText(str);
            }

            if (j == 1)
            {
                MessageBox.Show("有异常数据,异常数据已经标红色");
                return 0;
            }
            if (Protocol_type == 0)
            {
                Font_Card_Protocol_Dynamic(myarray);
            }
            else
            {
                MessageBox.Show("目前不支持该协议，请联系开发者");
            }
            
            return 0;
        }
        private void analysis_button_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] strCheckArray = Raw_data_textBox.Text.Split(' ');
            byte[] myarray = new byte[strCheckArray.Length];
            foreach (var tmp in strCheckArray)
            {
                myarray[i++] = System.Convert.ToByte(tmp, 16);
            }
            data_listView.Items.Clear();//每次点击事件后将ListView中的数据清空，重新显示
            data_after_transform_richTextBox.Clear();//每次点击事件后将data_after_transform_richTextBox中的数据清空，重新显示

            Data_deal_with(myarray, i);
            //out_excel_button.Visible = true;
        }

        private void Protocol_selection_SelectedIndexChanged(object sender, EventArgs e)
        {
            Protocol_type = Protocol_selection.SelectedIndex;
        }

        private void data_listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int listView_row;
            int i;
            int length = data_listView.SelectedItems.Count;
            listView_row = 0;
            for (i = 0; i < length; i++)
            {
                listView_row = data_listView.SelectedItems[i].Index;
            }
            data_after_transform_richTextBox.Clear();

            string str, str1;
            str = "";
            str1 = "";
            for (i = 0; i < PHY0_flag1.RCV_data_num; i++)
            {
                if ((i >= listView_to_myarray[listView_row].myarray_start) && (i < listView_to_myarray[listView_row].myarray_end))
                {
                    data_after_transform_richTextBox.SelectionColor = Color.Red;
                }
                else
                {
                    data_after_transform_richTextBox.SelectionColor = Color.Black;
                }

                str1 = myarray[i].ToString("x");
                str = (str1.Length == 1 ? "0" + str1 : str1);
                if (i == 0)
                {
                    str = str.ToUpper();
                }
                else
                {
                    str = " " + str.ToUpper();

                }
                data_after_transform_richTextBox.AppendText(str);
            }

        }
    }
}
