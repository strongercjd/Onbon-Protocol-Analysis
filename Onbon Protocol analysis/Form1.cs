﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Onbon_Protocol_analysis
{
    public partial class Onbon_Protocol_Form : Form
    {
        struct SlistView_to_myarra
        {
            public UInt32 myarray_start;
            public UInt32 myarray_end;
        };
        SlistView_to_myarra[] listView_to_myarray;

        public UInt32 UI_language = 0;//0是中文，1是英文

        public static Protocol_Analysis oProtocol_Analysis = new Protocol_Analysis();

        public Onbon_Protocol_Form()
        {
            InitializeComponent();
            Protocol_selection.Items.Add("字库卡协议");
            Protocol_selection.Items.Add("6代字库动态区协议");
            Protocol_selection.Items.Add("6代图文动态区协议");
            Protocol_selection.SelectedIndex = 0;
            LED_type_comboBox.SelectedIndex = 0;
            data_preview_comboBox.SelectedIndex = 0;
            data_after_transform_richTextBox.ReadOnly = true;
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
            {
                oProtocol_Analysis.language = 0;
                UI_language = 0;
            }
            else
            {
                oProtocol_Analysis.language = 1;
                UI_language = 1;
            }


            UpDataMainFormUILanguage();
#if DEBUG
            this.Width = 1307;
#else
            this.Width = 1198;
#endif

        }
        public int refresh_data_after_transform_richTextBox()
        {
            UInt32 i, j;
            string str, str1;
            str = "";
            i = 0;
            j = 0;

            data_after_transform_richTextBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            for (i = 0; i < oProtocol_Analysis.PHY0_flag1.RCV_data_num; i++)
            {
                if (oProtocol_Analysis.trsf_flg[i] == 1)
                {
                    data_after_transform_richTextBox.SelectionColor = Color.Red;
                    j = 1;
                }
                else
                {
                    if (oProtocol_Analysis.trsf_flg[i] == 2)
                    {
                        data_after_transform_richTextBox.SelectionColor = Color.Blue;
                    }
                    else
                    {
                        data_after_transform_richTextBox.SelectionColor = Color.Black;
                    }

                }

                str1 = oProtocol_Analysis.myarray[i].ToString("x");
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
                if (UI_language == 0)
                {
                    MessageBox.Show("有异常数据,异常数据已经标红色");
                }
                if (UI_language == 1)
                {
                    MessageBox.Show("There is abnormal data. The abnormal data has been marked in red");
                }

                return 0;
            }
            return 0;
        }
        public int refresh_data_listView()
        {
            UInt32 i, num, num1, num2, num3, listView_row;

            i = 0;
            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            listView_row = 0;
            string data;


            Protocol_Analysis.Conbon_Protocol m_oonbon_Protocol = new Protocol_Analysis.Conbon_Protocol();
            m_oonbon_Protocol = oProtocol_Analysis.m_oonbon_Protocol;

            listView_to_myarray = new SlistView_to_myarra[oProtocol_Analysis.myarray.Length];


            this.data_listView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

            try
            {
                ListViewGroup group_data_header = new ListViewGroup();  //创建包头数据分组

                if (UI_language == 0)
                {
                    group_data_header.Header = "包头数据";  //设置组的标题。
                }
                if (UI_language == 1)
                {
                    group_data_header.Header = "Header package";  //设置组的标题。
                }

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


                if (m_oonbon_Protocol.Prototol_CMD != null)
                {
                    ListViewGroup group_cmd = new ListViewGroup();  //创建命令分组
                    if (UI_language == 0)
                    {
                        group_cmd.Header = "命令";  //设置组的标题。
                    }
                    if (UI_language == 1)
                    {
                        group_cmd.Header = "command";  //设置组的标题。
                    }

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

                    if (oProtocol_Analysis.Protocol_type == 2)
                    {
                        for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
                        {
                            ListViewGroup grou_area_data = new ListViewGroup();  //创建命令分组
                            if (UI_language == 0)
                            {
                                grou_area_data.Header = "区域" + num.ToString() + "数据格式";  //设置组的标题。
                            }
                            if (UI_language == 1)
                            {
                                grou_area_data.Header = "data format of area" + num.ToString();  //设置组的标题。
                            }

                            grou_area_data.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                            this.data_listView.Groups.Add(grou_area_data);    //把命令分组添加到listview中

                            Protocol_Analysis.CSixImagePrototolAreaPart SixImagePrototolAreaPart = new Protocol_Analysis.CSixImagePrototolAreaPart();
                            SixImagePrototolAreaPart = m_oonbon_Protocol.SixImage_Prototol_area_data[num];
                            for (num1 = 0; num1 < SixImagePrototolAreaPart.Prototol_Area_Para.Length; num1++)
                            {
                                Protocol_Analysis.CProtolPart ProtolPart = new Protocol_Analysis.CProtolPart();
                                ProtolPart = SixImagePrototolAreaPart.Prototol_Area_Para[num1];
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



                            for (num1 = 0; num1 < SixImagePrototolAreaPart.SixImage_Prototol_Page_Part.Length; num1++)
                            {

                                ListViewGroup grou_page_data = new ListViewGroup();  //创建命令分组
                                if (UI_language == 0)
                                {
                                    grou_page_data.Header = "区域" + num.ToString() + "的第" + num1.ToString() + "页" + "数据格式";  //设置组的标题。
                                }
                                if (UI_language == 1)
                                {
                                    grou_page_data.Header = "Page " + num1.ToString() + " data format for area " + num.ToString();  //设置组的标题。
                                }

                                grou_page_data.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                                this.data_listView.Groups.Add(grou_page_data);    //把命令分组添加到listview中

                                Protocol_Analysis.CSixImagePrototolPagePart SixImagePrototolPagePart = new Protocol_Analysis.CSixImagePrototolPagePart();
                                SixImagePrototolPagePart = SixImagePrototolAreaPart.SixImage_Prototol_Page_Part[num1];

                                for (num2 = 0; num2 < SixImagePrototolPagePart.Prototol_Page_Part.Length; num2++)
                                {
                                    Protocol_Analysis.CProtolPart ProtolPart = new Protocol_Analysis.CProtolPart();
                                    ProtolPart = SixImagePrototolPagePart.Prototol_Page_Part[num2];

                                    if (ProtolPart.bEnable == 1)
                                    {
                                        listView_to_myarray[listView_row].myarray_start = i;

                                        data = "";
                                        Protocol_data = new ListViewItem();
                                        Protocol_data.Group = grou_page_data;
                                        Protocol_data.Text = ProtolPart.para;
                                        for (num3 = 0; num3 < ProtolPart.Leng; num3++)
                                        {
                                            data += ProtolPart.byteMemValue[ProtolPart.Leng - num3 - 1].ToString("X2");
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
                        }


                        ListViewGroup grou_display_data = new ListViewGroup();  //创建命令分组
                        if (UI_language == 0)
                        {
                            grou_display_data.Header = "数据区域";  //设置组的标题。
                        }
                        if (UI_language == 1)
                        {
                            grou_display_data.Header = "data of area";  //设置组的标题。
                        }

                        grou_display_data.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                        this.data_listView.Groups.Add(grou_display_data);    //把命令分组添加到listview中

                        for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
                        {
                            Protocol_Analysis.CSixImagePrototolAreaPart SixImagePrototolAreaPart = new Protocol_Analysis.CSixImagePrototolAreaPart();
                            SixImagePrototolAreaPart = m_oonbon_Protocol.SixImage_Prototol_area_data[num];

                            for (num1 = 0; num1 < SixImagePrototolAreaPart.Page_Num; num1++)
                            {

                                Protocol_Analysis.CSixImagePrototolPagePart SixImagePrototolPagePart = new Protocol_Analysis.CSixImagePrototolPagePart();
                                SixImagePrototolPagePart = SixImagePrototolAreaPart.SixImage_Prototol_Page_Part[num1];

                                listView_to_myarray[listView_row].myarray_start = i;
                                data = "";

                                Protocol_data = new ListViewItem();
                                Protocol_data.Group = grou_display_data;

                                if (UI_language == 0)
                                {
                                    Protocol_data.Text = "显示数据";
                                }
                                if (UI_language == 1)
                                {
                                    Protocol_data.Text = "Display data";
                                }

                                for (num2 = 0; num2 < SixImagePrototolPagePart.len; num2++)
                                {
                                    data += m_oonbon_Protocol.Disply_Data[SixImagePrototolPagePart.offset + num2].ToString("X2");
                                    i++;
                                }
                                Protocol_data.SubItems.Add(data);
                                if (UI_language == 0)
                                {
                                    Protocol_data.SubItems.Add("区域" + num.ToString() + "的第" + num1.ToString() + "页" + "数据格式");
                                }
                                if (UI_language == 1)
                                {
                                    Protocol_data.SubItems.Add("Page " + num1.ToString() + " data format for area " + num.ToString());
                                }

                                data_listView.Items.Add(Protocol_data);

                                listView_to_myarray[listView_row].myarray_end = i;
                                listView_row++;

                            }
                        }



                    }
                    else
                    {

                        if (m_oonbon_Protocol.Prototol_File != null)
                        {
                            ListViewGroup group_file = new ListViewGroup();  //创建命令分组
                            if (UI_language == 0)
                            {
                                group_file.Header = "文件格式";  //设置组的标题。
                            }
                            if (UI_language == 1)
                            {
                                group_file.Header = "File";  //设置组的标题。
                            }

                            group_file.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                            this.data_listView.Groups.Add(group_file);    //把命令分组添加到listview中
                            for (num = 0; num < m_oonbon_Protocol.Prototol_File.Length; num++)
                            {
                                if (m_oonbon_Protocol.Prototol_File[num].bEnable == 1)
                                {
                                    listView_to_myarray[listView_row].myarray_start = i;

                                    data = "";
                                    Protocol_data = new ListViewItem();
                                    Protocol_data.Group = group_file;
                                    Protocol_data.Text = m_oonbon_Protocol.Prototol_File[num].para;
                                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_File[num].Leng; num1++)
                                    {
                                        data += m_oonbon_Protocol.Prototol_File[num].byteMemValue[m_oonbon_Protocol.Prototol_File[num].Leng - num1 - 1].ToString("X2");
                                        i++;
                                    }
                                    Protocol_data.SubItems.Add(data);
                                    Protocol_data.SubItems.Add(m_oonbon_Protocol.Prototol_File[num].describe);
                                    data_listView.Items.Add(Protocol_data);

                                    listView_to_myarray[listView_row].myarray_end = i;
                                    listView_row++;
                                }
                            }

                        }


                        for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
                        {
                            ListViewGroup grou_area_data = new ListViewGroup();  //创建命令分组
                            if (UI_language == 0)
                            {
                                grou_area_data.Header = "区域" + num.ToString() + "数据格式";  //设置组的标题。
                            }
                            if (UI_language == 1)
                            {
                                grou_area_data.Header = "data format of area" + num.ToString();  //设置组的标题。
                            }

                            grou_area_data.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                            this.data_listView.Groups.Add(grou_area_data);    //把命令分组添加到listview中
                            for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part.Length; num1++)
                            {
                                Protocol_Analysis.CProtolPart ProtolPart = new Protocol_Analysis.CProtolPart();
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
                    }

                }


                if (m_oonbon_Protocol.Prototol_File_CRC != null)
                {
                    ListViewGroup File_CRC = new ListViewGroup();  //创建命令分组
                    if (UI_language == 0)
                    {
                        File_CRC.Header = "文件CRC校验";  //设置组的标题。
                    }
                    if (UI_language == 1)
                    {
                        File_CRC.Header = "File CRC check";  //设置组的标题。
                    }

                    File_CRC.HeaderAlignment = HorizontalAlignment.Left;//设置组标题文本的对齐方式。（默认为Left）
                    this.data_listView.Groups.Add(File_CRC);    //把命令分组添加到listview中
                    if (m_oonbon_Protocol.Prototol_File_CRC.bEnable == 1)
                    {
                        listView_to_myarray[listView_row].myarray_start = i;

                        data = "";
                        Protocol_data = new ListViewItem();
                        Protocol_data.Group = File_CRC;
                        Protocol_data.Text = m_oonbon_Protocol.Prototol_File_CRC.para;
                        for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_File_CRC.Leng; num1++)
                        {
                            data += m_oonbon_Protocol.Prototol_File_CRC.byteMemValue[m_oonbon_Protocol.Prototol_File_CRC.Leng - num1 - 1].ToString("X2");
                            i++;
                        }
                        Protocol_data.SubItems.Add(data);
                        if (UI_language == 0)
                        {
                            if (m_oonbon_Protocol.Prototol_File_CRC.describe == "CRC校验错误")
                            {
                                Protocol_data.ForeColor = Color.Red;
                            }
                        }
                        if (UI_language == 1)
                        {
                            if (m_oonbon_Protocol.Prototol_File_CRC.describe == "CRC error")
                            {
                                Protocol_data.ForeColor = Color.Red;
                            }
                        }

                        Protocol_data.SubItems.Add(m_oonbon_Protocol.Prototol_File_CRC.describe);
                        data_listView.Items.Add(Protocol_data);

                        listView_to_myarray[listView_row].myarray_end = i;
                        listView_row++;
                    }
                }




                if (m_oonbon_Protocol.Prototol_CRC != null)
                {
                    ListViewGroup grou_CRC = new ListViewGroup();  //创建命令分组
                    if (UI_language == 0)
                    {
                        grou_CRC.Header = "CRC校验";  //设置组的标题。
                    }
                    if (UI_language == 1)
                    {
                        grou_CRC.Header = "CRC check";  //设置组的标题。
                    }

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
                        if (UI_language == 0)
                        {
                            if (m_oonbon_Protocol.Prototol_CRC.describe == "CRC校验错误")
                            {
                                Protocol_data.ForeColor = Color.Red;
                            }
                        }
                        if (UI_language == 1)
                        {
                            if (m_oonbon_Protocol.Prototol_CRC.describe == "CRC error")
                            {
                                Protocol_data.ForeColor = Color.Red;
                            }
                        }

                        Protocol_data.SubItems.Add(m_oonbon_Protocol.Prototol_CRC.describe);
                        data_listView.Items.Add(Protocol_data);

                        listView_to_myarray[listView_row].myarray_end = i;
                        listView_row++;
                    }
                }
            }
            catch (Exception ex)
            {
            }


            this.data_listView.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            return 0;
        }

        private void analysis_button_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            int data;
            string str = Raw_data_textBox.Text;
            char[] arr = str.ToCharArray(0, str.Length);
            byte[] myarray = new byte[str.Length];
            data = 1;
            for (UInt32 num = 0; num < arr.Length; num++)
            {
                if (((arr[num] >= '0') && (arr[num] <= '9')) ||
                    ((arr[num] >= 'a') && (arr[num] <= 'f')) ||
                    ((arr[num] >= 'A') && (arr[num] <= 'F'))
                    )
                {
                    if (((arr[num + 1] >= '0') && (arr[num + 1] <= '9')) ||
                        ((arr[num + 1] >= 'a') && (arr[num + 1] <= 'f')) ||
                        ((arr[num + 1] >= 'A') && (arr[num + 1] <= 'F'))
                        )
                    {
                        data = 1;
                        if ((arr[num] >= '0') && (arr[num] <= '9'))
                        {
                            data = arr[num] - '0';
                        }
                        if ((arr[num] >= 'a') && (arr[num] <= 'f'))
                        {
                            data = arr[num] - 'a' + 10;
                        }
                        if ((arr[num] >= 'A') && (arr[num] <= 'F'))
                        {
                            data = arr[num] - 'A' + 10;
                        }

                        if ((arr[num + 1] >= '0') && (arr[num + 1] <= '9'))
                        {
                            data = data * 16 + (arr[num + 1] - '0');
                        }
                        if ((arr[num + 1] >= 'a') && (arr[num + 1] <= 'f'))
                        {
                            data = data * 16 + (arr[num + 1] - 'a' + 10);
                        }
                        if ((arr[num + 1] >= 'A') && (arr[num + 1] <= 'F'))
                        {
                            data = data * 16 + (arr[num + 1] - 'A' + 10);
                        }
                        myarray[i++] = (byte)data;
                        num++;
                    }
                    else
                    {
                        Raw_data_textBox.Clear();
                        for (UInt32 num1 = 0; num1 < arr.Length; num1++)
                        {
                            if (num1 == num)
                            {
                                Raw_data_textBox.SelectionColor = Color.Red;

                                Raw_data_textBox.AppendText(arr[num1].ToString());
                                num1++;
                                Raw_data_textBox.AppendText(arr[num1].ToString());
                            }
                            else
                            {
                                Raw_data_textBox.SelectionColor = Color.Black;
                                Raw_data_textBox.AppendText(arr[num1].ToString());
                            }

                        }
                        data_listView.Items.Clear();//每次点击事件后将ListView中的数据清空，重新显示
                        data_after_transform_richTextBox.Clear();//每次点击事件后将data_after_transform_richTextBox中的数据清空，重新显示
                        oProtocol_Analysis.Data_deal_with(myarray, i);//转义数据
                        refresh_data_after_transform_richTextBox();//将转义之后的数据显示在文本框内
                        if (UI_language == 0)
                        {
                            MessageBox.Show("数据格式错误，错误的数据在文本框中已经变红");
                        }
                        if (UI_language == 1)
                        {
                            MessageBox.Show("The data format is wrong. The wrong data has turned red in the text box");
                        }

                        return;
                    }
                }
            }

            /*string[] strCheckArray = Raw_data_textBox.Text.Split(' ');
            byte[] myarray = new byte[strCheckArray.Length];
            foreach (var tmp in strCheckArray)
            {
                myarray[i++] = System.Convert.ToByte(tmp, 16);
            }*/
            data_listView.Items.Clear();//每次点击事件后将ListView中的数据清空，重新显示
            data_after_transform_richTextBox.Clear();//每次点击事件后将data_after_transform_richTextBox中的数据清空，重新显示


            i = oProtocol_Analysis.Data_deal_with(myarray, i);//转义数据

            refresh_data_after_transform_richTextBox();//将转义之后的数据显示在文本框内

            if (i != 0)
            {
                switch (i)
                {
                    case 1:
                        if (UI_language == 0)
                        {
                            MessageBox.Show("数据不完整，没有帧头0XA5，有帧尾0X5A");
                        }
                        if (UI_language == 1)
                        {
                            MessageBox.Show("Incomplete data, no frame head 0xa5, frame tail 0x5a");
                        }

                        break;
                    case 2:
                        if (UI_language == 0)
                        {
                            MessageBox.Show("数据不完整，有帧头0XA5，没有帧尾0X5A，注意使用Wireshark抓包，数据长度大于1500，IP分包问题");
                        }
                        if (UI_language == 1)
                        {
                            MessageBox.Show("The data is incomplete. There is a frame header of 0xa5 and no frame tail of 0x5a. Pay attention to the use of Wireshark to grab packets. The data length is greater than 1500. IP subcontracting is a problem");
                        }

                        break;
                    case 3:
                        if (UI_language == 0)
                        {
                            MessageBox.Show("数据不完整，没有帧头0XA5，有帧尾0X5A");
                        }
                        if (UI_language == 1)
                        {
                            MessageBox.Show("Incomplete data, no frame head 0xa5, frame tail 0x5a");
                        }

                        break;
                    case 4:
                        if (UI_language == 0)
                        {
                            MessageBox.Show("这是仰邦协议的ping命令，仰邦卡(除Y和YQ系列)都会响应此命令，开发者不用分析此数据包，回包也不用分析");
                        }
                        if (UI_language == 1)
                        {
                            MessageBox.Show("This is the ping command of the onbon protocol. When the onbon card goes out of the Y and YQ series, it will respond to this command. The developer does not care about this data package, nor does the command return the package.");
                        }

                        break;
                    default:
                        if (UI_language == 0)
                        {
                            MessageBox.Show("数据错误");
                        }
                        if (UI_language == 1)
                        {
                            MessageBox.Show("data error");
                        }

                        break;
                }
                return;
            }


            switch (oProtocol_Analysis.Protocol_type)
            {
                case 0:
                    j = oProtocol_Analysis.Font_Card_Protocol_Deal_With();//解析数据
                    break;
                case 1:
                    j = oProtocol_Analysis.Six_Font_Protocol_Deal_With();//解析数据
                    break;
                case 2:
                    j = oProtocol_Analysis.Six_Image_Protocol_Deal_With();//解析数据
                    break;
                default:
                    j = 2;
                    break;
            }
            if (j == 2)
            {

                if (UI_language == 0)
                {
                    MessageBox.Show("目前不支持该协议，请联系开发者");
                }
                if (UI_language == 1)
                {
                    MessageBox.Show("This protocol is not supported at present. Please contact the developer");
                }

            }
            refresh_data_listView();
            out_excel_button.Enabled = true;
            region_preview_button.Enabled = true;
        }

        private void Protocol_selection_SelectedIndexChanged(object sender, EventArgs e)
        {
            oProtocol_Analysis.Protocol_type = Protocol_selection.SelectedIndex;
        }

        private void data_listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int listView_row;
            int j;

            string richTextBoxstr;
            richTextBoxstr = "";
            string str_star, str_red, str_end;


            int length = data_listView.SelectedItems.Count;
            listView_row = 0;
            for (j = 0; j < length; j++)
            {
                listView_row = data_listView.SelectedItems[j].Index;
            }

            richTextBoxstr = data_after_transform_richTextBox.Text + " ";

            str_star = richTextBoxstr.Substring(0, (int)((listView_to_myarray[listView_row].myarray_start) * 3));
            str_red = richTextBoxstr.Substring((int)((listView_to_myarray[listView_row].myarray_start) * 3), (int)((listView_to_myarray[listView_row].myarray_end - listView_to_myarray[listView_row].myarray_start) * 3));
            str_end = richTextBoxstr.Remove(0, (int)((listView_to_myarray[listView_row].myarray_end) * 3));


            data_after_transform_richTextBox.Clear();

            data_after_transform_richTextBox.SelectionColor = Color.Black;
            data_after_transform_richTextBox.AppendText(str_star);

            data_after_transform_richTextBox.SelectionColor = Color.Red;
            data_after_transform_richTextBox.AppendText(str_red);

            data_after_transform_richTextBox.SelectionColor = Color.Black;
            data_after_transform_richTextBox.AppendText(str_end);

            if (str_red.Length > 100)
            {
                if (data_preview_comboBox.SelectedIndex != 0)
                {
                    int i = 0;
                    str_red = str_red.Substring(0, str_red.Length - 1);
                    string[] strCheckArray = str_red.Split(' ');
                    byte[] myarray = new byte[strCheckArray.Length];
                    foreach (var tmp in strCheckArray)
                    {
                        myarray[i++] = System.Convert.ToByte(tmp, 16);
                    }
                    Data_Preview Data_Pre = new Data_Preview();
                    Data_Pre.Show();
                    Data_Pre.Data_Preview_star(myarray, data_preview_comboBox.SelectedIndex - 1, LED_type_comboBox.SelectedIndex);
                }
            }
        }
        /// <summary>
        /// 执行导出数据
        /// </summary>
        public void ExportToExecl()
        {
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "xls";

            sfd.Filter = "Excel文件(*.xls)|*.xls";
            if (UI_language == 0)
            {
                sfd.FileName = "仰邦协议解析结果输出";
            }
            if (UI_language == 1)
            {
                sfd.FileName = "Analysis result output of onbon protocol";
            }


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                DoExport(this.data_listView, sfd.FileName);
            }
        }

        /// <summary>
        /// 具体导出的方法
        /// </summary>
        /// <param name="listView">ListView</param>
        /// <param name="strFileName">导出到的文件名</param>
        private void DoExport(ListView listView, string strFileName)
        {
            int rowNum = listView.Items.Count;
            int columnNum = listView.Items[0].SubItems.Count;
            int rowIndex = 1;
            int columnIndex = 0;


            if (rowNum == 0 || string.IsNullOrEmpty(strFileName))
            {
                return;
            }

            if (rowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                if (xlApp == null)
                {
                    if (UI_language == 0)
                    {
                        MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel");
                    }
                    if (UI_language == 1)
                    {
                        MessageBox.Show("Unable to create excel object, maybe excel is not installed in your system");
                    }

                    return;
                }
                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = true;
                xlApp.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
                //将ListView的列名导入Excel表第一行
                foreach (ColumnHeader dc in listView.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.Text;
                }
                //将ListView中的数据导入Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        //注意这个在导出的时候加了“\t” 的目的就是避免导出的数据显示为科学计数法。可以放在每行的首尾。
                        xlApp.Cells[rowIndex, columnIndex] = Convert.ToString(listView.Items[i].SubItems[j].Text) + "\t";
                    }
                }

                //例外需要说明的是用strFileName,Excel.XlFileFormat.xlExcel9795保存方式时 当你的Excel版本不是95、97 而是2003、2007 时导出的时候会报一个错误：异常来自 HRESULT:0x800A03EC。 解决办法就是换成strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal。
                xlBook.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlApp = null;
                xlBook = null;
                MessageBox.Show("OK");
            }

        }
        private void out_excel_button_Click(object sender, EventArgs e)
        {
            analysis_button.Enabled = false;
            out_excel_button.Enabled = false;
            Protocol_selection.Enabled = false;

            ExportToExecl();

            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            foreach (System.Diagnostics.Process p in process)
            {
                if (!p.HasExited)  // 如果程序没有关闭，结束程序
                {
                    p.Kill();
                    p.WaitForExit();
                }
            }

            analysis_button.Enabled = true;
            out_excel_button.Enabled = true;
            Protocol_selection.Enabled = true;
        }

        private void 联系开发者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Developer_Inf nf = new Developer_Inf();
            nf.Show();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str2 = Environment.CurrentDirectory;
            str2 = str2 + "\\README.pdf";
            if (System.IO.File.Exists(str2))
            {
                System.Diagnostics.Process.Start(str2);
            }
            else
            {
                //MessageBox.Show("不存在README.pdf");
                System.Diagnostics.Process.Start("https://github.com/strongercjd/Onbon-Protocol-Analysis/blob/master/README.md");
            }

        }

        private void region_preview_button_Click(object sender, EventArgs e)
        {
            Region_Preview Region_Pre = new Region_Preview();
            Region_Pre.Show();
            Region_Pre.Region_Preview_star();
        }

        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_language = 0;
            oProtocol_Analysis.language = 0;
            UpDataMainFormUILanguage();
        }

        private void 英文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_language = 1;
            oProtocol_Analysis.language = 1;
            UpDataMainFormUILanguage();
        }
        private void UpDataMainFormUILanguage()
        {
            if (UI_language == 0)
            {
                label1.Text = "除去帧头帧尾，转义后的数据";
                label2.Text = "协议选择";
                analysis_button.Text = "开始解析";
                out_excel_button.Text = "输出Excel";
                data_after_transform_richTextBox.Text = "      仰邦协议解析工具（C）2019";
                this.columnHeader1.Text = "参数";
                this.columnHeader2.Text = "数据";
                this.columnHeader3.Text = "描述";

                语言ToolStripMenuItem.Text = "语言";
                中文ToolStripMenuItem.Text = "中文简体";
                英文ToolStripMenuItem.Text = "英文";
                帮助ToolStripMenuItem.Text = "帮助";
                联系开发者ToolStripMenuItem.Text = "联系开发者";
                this.Text = "onbon协议解析 V0.2";

                Protocol_selection.Items.Clear();
                Protocol_selection.Items.Add("字库卡协议");
                Protocol_selection.Items.Add("6代字库动态区协议");
                Protocol_selection.Items.Add("6代图文动态区协议");
                Protocol_selection.SelectedIndex = 0;
            }
            if (UI_language == 1)
            {
                label1.Text = "Remove the head and tail of the frame, and escape the data";
                label2.Text = "Protocol selection";
                analysis_button.Text = "Start parsing";
                out_excel_button.Text = "Output Excel";
                data_after_transform_richTextBox.Text = "      onbon protocol resolution tool (c) 2019";
                this.columnHeader1.Text = "parameter";
                this.columnHeader2.Text = "data";
                this.columnHeader3.Text = "describe";

                语言ToolStripMenuItem.Text = "language";
                中文ToolStripMenuItem.Text = "Chinese";
                英文ToolStripMenuItem.Text = "English";
                帮助ToolStripMenuItem.Text = "Help";
                联系开发者ToolStripMenuItem.Text = "Contact developer";
                this.Text = "onbon Protocol analysis V0.2";

                Protocol_selection.Items.Clear();
                Protocol_selection.Items.Add("Onbon_Font_controller_protocol");
                Protocol_selection.Items.Add("6th_font_dynamic_program_protocol");
                Protocol_selection.Items.Add("6th_photo_word_dynamic_program_protocol");
                Protocol_selection.SelectedIndex = 0;
            }
        }

        private void 语言ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }

    public class Protocol_Analysis
    {
        #region 变量和类的定义
        public class CProtolPart
        {
            public int bEnable = 0;
            public string para;//参数
            public UInt32 Leng;//数据长度
            public byte[] byteMemValue;//数值
            public string describe;//描述
        }
        public class CPrototolAreaPart
        {
            public CProtolPart[] Prototol_Area_Part;
        }
        public class CSixImagePrototolPagePart
        {
            public UInt32 offset;
            public UInt32 len;
            public CProtolPart[] Prototol_Page_Part;
        }
        public class CSixImagePrototolAreaPart
        {
            public int Page_Num;
            public CProtolPart[] Prototol_Area_Para;
            public CSixImagePrototolPagePart[] SixImage_Prototol_Page_Part;
        }

        public class Conbon_Protocol
        {
            public CProtolPart[] Prototol_Header;
            public CProtolPart[] Prototol_CMD;
            public CProtolPart[] Prototol_File;
            public int Area_Num;
            public CPrototolAreaPart[] Prototol_area_data;

            public CSixImagePrototolAreaPart[] SixImage_Prototol_area_data;
            public byte[] Disply_Data;

            public CProtolPart Prototol_File_CRC;

            public CProtolPart Prototol_CRC;
        }
        public Conbon_Protocol m_oonbon_Protocol = new Conbon_Protocol();

        public class Cregion_parameter
        {
            public int bEnable = 0;
            public UInt32 ID;

            public UInt32 X;
            public UInt32 Y;
            public UInt32 width;
            public UInt32 height;
        }

        public int language = 0;//0表示中文，1表示英文

        public Cregion_parameter[] m_region_par = new Cregion_parameter[40];


        public struct PHY0_flag
        {
            public Int16 Rcv_state;                                    //!< 接收状态标志
            public UInt32 RCV_data_num;                                 //!< 接收到个数位置
        };
        public PHY0_flag PHY0_flag1;
        public int Protocol_type;//0是字库卡  1是6代点阵协议   2是6代字库协议

        public string[][] Protol_header_str = new string[100][];
        int Protol_header_str_len;
        string[][] Protol_cmd_str = new string[100][];
        int Prototol_CMD_len;
        string[][] Protol_area_data_str = new string[100][];
        int Protol_area_data_str_len;
        string[][] Protol_Page_data_str = new string[100][];
        int Protol_Page_data_str_len;
        string[][] Protol_crc_str = new string[100][];
        int Protol_crc_str_len;
        string[][] Protol_cmd_filecrc_str = new string[100][];
        int Prototol_CMD_filecrc_len;
        string[][] Protol_File_str = new string[100][];//文件
        int Prototol_File_len;

        public byte[] myarray;
        public byte[] trsf_flg;

        int data_value;
        string data;

        #endregion 变量和类的定义

        public void m_region_par_init()
        {
            for (int num = 0; num < m_region_par.Length; num++)
            {
                m_region_par[num] = new Cregion_parameter();
                m_region_par[num].bEnable = 0;
            }
        }



        #region Six_Image_string_init
        public void Six_Image_Protocol_header_string_init()
        {
            int num = 0;
            if (language == 0)
            {
                Protol_header_str[num++] = new string[] { "2", "屏地址", "也是屏号" };
                Protol_header_str[num++] = new string[] { "2", "源地址", "源地址" };
                Protol_header_str[num++] = new string[] { "1", "协议版本号", "协议版本号" };
                Protol_header_str[num++] = new string[] { "1", "保留字节", "保留字节" };
                Protol_header_str[num++] = new string[] { "2", "设备类型", "设备类型" };
                Protol_header_str[num++] = new string[] { "4", "保留字节", "保留字节" };
                Protol_header_str[num++] = new string[] { "4", "数据域长度", "数据域长度" };
                Protol_header_str_len = num;
            }
            if (language == 1)
            {
                Protol_header_str[num++] = new string[] { "2", "screen address", " also the screen number" };
                Protol_header_str[num++] = new string[] { "2", "source address", "source address" };
                Protol_header_str[num++] = new string[] { "1", "protocal version", "protocal version" };
                Protol_header_str[num++] = new string[] { "1", "reserved Byte", "reserved Byte" };
                Protol_header_str[num++] = new string[] { "2", "device type", "device type" };
                Protol_header_str[num++] = new string[] { "4", "reserved Byte", "reserved Byte" };
                Protol_header_str[num++] = new string[] { "4", "Data field length", "Data field length" };
                Protol_header_str_len = num;
            }

        }
        public void Six_Image_Protocol_A7_00_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "更新图文动态区" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "更新图文动态区" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "2", "更新区域个数", "更新区域个数" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "update text&image dynamic area" };
                Protol_cmd_str[num++] = new string[] { "1", "command item", "update text&image dynamic area" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "2", "update area quantity", "update area quantity" };
                Prototol_CMD_len = num;
            }

        }
        public void Six_Image_Protocol_Page_data_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_Page_data_str[num++] = new string[] { "4", "数据长度", "数据长度" };
                Protol_Page_data_str[num++] = new string[] { "1", "数据页类型", "数据页类型" };
                Protol_Page_data_str[num++] = new string[] { "1", "显示方式", "显示方式" };
                Protol_Page_data_str[num++] = new string[] { "1", "退出方式", "退出方式" };
                Protol_Page_data_str[num++] = new string[] { "1", "速度等级", "速度等级" };
                Protol_Page_data_str[num++] = new string[] { "2", "停留时间", "停留时间" };
                Protol_Page_data_str[num++] = new string[] { "1", "重复次数", "重复次数" };
                Protol_Page_data_str[num++] = new string[] { "2", "有效长度", "有效长度" };
                Protol_Page_data_str[num++] = new string[] { "9", "保留字节", "保留字节" };
                Protol_Page_data_str[num++] = new string[] { "4", "本页数据偏移量", "本页数据偏移量" };
                Protol_Page_data_str[num++] = new string[] { "4", "本页数据长度", "本页数据长度" };
                Protol_Page_data_str_len = num;
            }
            if (language == 1)
            {
                Protol_Page_data_str[num++] = new string[] { "4", "data length", "data length" };
                Protol_Page_data_str[num++] = new string[] { "1", "data page type", "data page type" };
                Protol_Page_data_str[num++] = new string[] { "1", "display mode", "display mode" };
                Protol_Page_data_str[num++] = new string[] { "1", "exit mode", "exit mode" };
                Protol_Page_data_str[num++] = new string[] { "1", "speed level", "speed level" };
                Protol_Page_data_str[num++] = new string[] { "2", "stay time", "stay time" };
                Protol_Page_data_str[num++] = new string[] { "1", "repeat number", "repeat number" };
                Protol_Page_data_str[num++] = new string[] { "2", "valid length", "valid length" };
                Protol_Page_data_str[num++] = new string[] { "9", "reserved Byte", "reserved Byte" };
                Protol_Page_data_str[num++] = new string[] { "4", "Data offset on this page", "Data offset on this page" };
                Protol_Page_data_str[num++] = new string[] { "4", "data length", "data length" };
                Protol_Page_data_str_len = num;
            }

        }
        public void Six_Image_Protocol_area_data_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_area_data_str[num++] = new string[] { "4", "区域数据长度", "区域数据长度" };
                Protol_area_data_str[num++] = new string[] { "1", "区域序号", "区域序号" };
                Protol_area_data_str[num++] = new string[] { "1", "动态区运行模式", "动态区运行模式" };
                Protol_area_data_str[num++] = new string[] { "2", "动态区超时时间", "动态区超时时间" };
                Protol_area_data_str[num++] = new string[] { "1", "和异步节目的关系", "和异步节目的关系" };
                Protol_area_data_str[num++] = new string[] { "2", "关联异步节目个数", "关联异步节目个数" };
                Protol_area_data_str[num++] = new string[] { "N", "异步节目编号", "异步节目编号" };
                Protol_area_data_str[num++] = new string[] { "1", "是否覆盖", "是否覆盖" };
                Protol_area_data_str[num++] = new string[] { "4", "保留字节", "保留字节" };
                Protol_area_data_str[num++] = new string[] { "1", "区域类型", "区域类型" };
                Protol_area_data_str[num++] = new string[] { "2", "X坐标", "X坐标" };
                Protol_area_data_str[num++] = new string[] { "2", "Y坐标", "Y坐标" };
                Protol_area_data_str[num++] = new string[] { "2", "区域宽度", "区域宽度" };
                Protol_area_data_str[num++] = new string[] { "2", "区域高度", "区域高度" };
                Protol_area_data_str[num++] = new string[] { "1", "是否有边框", "是否有边框" };
                Protol_area_data_str[num++] = new string[] { "1", "是否有背景", "是否有背景" };
                Protol_area_data_str[num++] = new string[] { "1", "透明度", "透明度" };
                Protol_area_data_str[num++] = new string[] { "1", "前景背景是否相同", "前景背景是否相同" };
                Protol_area_data_str[num++] = new string[] { "1", "是否使能语音", "是否使能语音" };
                Protol_area_data_str[num++] = new string[] { "1", "发音人", "发音人" };
                Protol_area_data_str[num++] = new string[] { "1", "音量", "音量" };
                Protol_area_data_str[num++] = new string[] { "1", "语速", "语速" };
                Protol_area_data_str[num++] = new string[] { "1", "编码格式", "编码格式" };
                Protol_area_data_str[num++] = new string[] { "4", "重播次数", "重播次数" };
                Protol_area_data_str[num++] = new string[] { "4", "重默间隔", "重播间隔" };
                Protol_area_data_str[num++] = new string[] { "1", "语音参数保留长度", "语音参数保留长度" };
                Protol_area_data_str[num++] = new string[] { "1", "数字判断", "数字判断" };
                Protol_area_data_str[num++] = new string[] { "1", "语种判断", "语种判断" };
                Protol_area_data_str[num++] = new string[] { "1", "字母判断", "字母判断" };
                Protol_area_data_str[num++] = new string[] { "4", "读音数据长度", "读音数据长度" };
                Protol_area_data_str[num++] = new string[] { "N", "读音数据", "读音数据" };
                Protol_area_data_str[num++] = new string[] { "5", "保留字节", "保留字节" };
                Protol_area_data_str[num++] = new string[] { "2", "数据页数", "数据页数" };
                Protol_area_data_str_len = num;
            }
            if (language == 1)
            {
                Protol_area_data_str[num++] = new string[] { "4", "area data length", "area data length" };
                Protol_area_data_str[num++] = new string[] { "1", "Area number", "Area number" };
                Protol_area_data_str[num++] = new string[] { "1", "Dynamic zone operation mode", "Dynamic zone operation mode" };
                Protol_area_data_str[num++] = new string[] { "2", "dynamic area timeout", "dynamic area timeout" };
                Protol_area_data_str[num++] = new string[] { "1", "relation to asyn program", "relation to asyn program" };
                Protol_area_data_str[num++] = new string[] { "2", "the related quantity to asyn program", "the related quantity to asyn program" };
                Protol_area_data_str[num++] = new string[] { "N", "item number of asyn program", "item number of asyn program" };
                Protol_area_data_str[num++] = new string[] { "1", "cover or not", "cover or not" };
                Protol_area_data_str[num++] = new string[] { "4", "reserved Byte", "reserved Byte" };
                Protol_area_data_str[num++] = new string[] { "1", "area type", "area type" };
                Protol_area_data_str[num++] = new string[] { "2", "X coordinate", "X coordinate" };
                Protol_area_data_str[num++] = new string[] { "2", "Y coordinate", "Y coordinate" };
                Protol_area_data_str[num++] = new string[] { "2", "Area width", "Area width" };
                Protol_area_data_str[num++] = new string[] { "2", "Area height", "Area height" };
                Protol_area_data_str[num++] = new string[] { "1", "with frame boarder or not", "with frame boarder or not" };
                Protol_area_data_str[num++] = new string[] { "1", "support backgroud or not", "support backgroud or not" };
                Protol_area_data_str[num++] = new string[] { "1", "transparency", "transparency" };
                Protol_area_data_str[num++] = new string[] { "1", "Whether the foreground background is the same", "Whether the foreground background is the same" };
                Protol_area_data_str[num++] = new string[] { "1", "use audio or not", "use audio or not" };
                Protol_area_data_str[num++] = new string[] { "1", "Pronunciation people", "Pronunciation people" };
                Protol_area_data_str[num++] = new string[] { "1", "volume", "volume" };
                Protol_area_data_str[num++] = new string[] { "1", "speak speed", "speak speed" };
                Protol_area_data_str[num++] = new string[] { "1", "Coding format", "Coding format" };
                Protol_area_data_str[num++] = new string[] { "4", "repeat times", "repeat times" };
                Protol_area_data_str[num++] = new string[] { "4", "default interval", "default interval" };
                Protol_area_data_str[num++] = new string[] { "1", "Speech parameter retention lengthl", "Speech parameter retention length" };
                Protol_area_data_str[num++] = new string[] { "1", "Digital judgment", "Digital judgment" };
                Protol_area_data_str[num++] = new string[] { "1", "Languages judgment", "Languages judgment" };
                Protol_area_data_str[num++] = new string[] { "1", "letter judgment", "letter judgment" };
                Protol_area_data_str[num++] = new string[] { "4", "pronunciation data length", "pronunciation data length" };
                Protol_area_data_str[num++] = new string[] { "N", "pronunciation data ", "pronunciation data" };
                Protol_area_data_str[num++] = new string[] { "5", "reserved Byte", "reserved Byte" };
                Protol_area_data_str[num++] = new string[] { "2", "data page", "data page" };

                Protol_area_data_str_len = num;
            }



        }
        #endregion Six_Image_string_init


        #region Six_Font_string_init
        public void Six_Font_Protocol_header_string_init()
        {
            int num = 0;
            if (language == 0)
            {
                Protol_header_str[num++] = new string[] { "2", "屏地址", "也是屏号" };
                Protol_header_str[num++] = new string[] { "2", "源地址", "源地址" };
                Protol_header_str[num++] = new string[] { "1", "协议版本号", "协议版本号" };
                Protol_header_str[num++] = new string[] { "1", "保留字节", "保留字节" };
                Protol_header_str[num++] = new string[] { "2", "设备类型", "设备类型" };
                Protol_header_str[num++] = new string[] { "4", "保留字节", "保留字节" };
                Protol_header_str[num++] = new string[] { "4", "数据域长度", "数据域长度" };
                Protol_header_str_len = num;
            }
            if (language == 1)
            {
                Protol_header_str[num++] = new string[] { "2", "screen address", " also the screen number" };
                Protol_header_str[num++] = new string[] { "2", "source address", "source address" };
                Protol_header_str[num++] = new string[] { "1", "protocal version", "protocal version" };
                Protol_header_str[num++] = new string[] { "1", "reserved Byte", "reserved Byte" };
                Protol_header_str[num++] = new string[] { "2", "device type", "device type" };
                Protol_header_str[num++] = new string[] { "4", "reserved Byte", "reserved Byte" };
                Protol_header_str[num++] = new string[] { "4", "Data field length", "Data field length" };
                Protol_header_str_len = num;
            }


        }
        public void Six_Font_Protocol_A7_06_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "更新字库动态区" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "更新字库动态区" };
                Protol_cmd_str[num++] = new string[] { "1", "掉电是否保留", "掉电是否保留" };
                Protol_cmd_str[num++] = new string[] { "1", "删除区域个数", "删除区域个数" };
                Protol_cmd_str[num++] = new string[] { "N", "删除的区域ID", "删除的区域ID" };
                Protol_cmd_str[num++] = new string[] { "1", "更新区域个数", "更新区域个数" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "update font library area" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "update font library area" };
                Protol_cmd_str[num++] = new string[] { "1", "save or not after power off", "save or not after power off" };
                Protol_cmd_str[num++] = new string[] { "1", "delete area quantity", "delete area quantity" };
                Protol_cmd_str[num++] = new string[] { "N", "delete area ID", "delete area ID" };
                Protol_cmd_str[num++] = new string[] { "1", "update area quantity", "update area quantity" };
                Prototol_CMD_len = num;
            }

        }
        public void Six_Font_Protocol_area_data_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_area_data_str[num++] = new string[] { "4", "区域数据长度", "区域数据长度" };
                Protol_area_data_str[num++] = new string[] { "1", "区域序号", "区域序号" };
                Protol_area_data_str[num++] = new string[] { "1", "动态区运行模式", "动态区运行模式" };
                Protol_area_data_str[num++] = new string[] { "2", "动态区超时时间", "动态区超时时间" };
                Protol_area_data_str[num++] = new string[] { "1", "和异步节目的关系", "和异步节目的关系" };
                Protol_area_data_str[num++] = new string[] { "2", "关联异步节目个数", "关联异步节目个数" };
                Protol_area_data_str[num++] = new string[] { "N", "异步节目编号", "异步节目编号" };
                Protol_area_data_str[num++] = new string[] { "1", "是否覆盖", "是否覆盖" };
                Protol_area_data_str[num++] = new string[] { "4", "保留字节", "保留字节" };
                Protol_area_data_str[num++] = new string[] { "1", "区域类型", "区域类型" };
                Protol_area_data_str[num++] = new string[] { "2", "X坐标", "X坐标" };
                Protol_area_data_str[num++] = new string[] { "2", "Y坐标", "Y坐标" };
                Protol_area_data_str[num++] = new string[] { "2", "区域宽度", "区域宽度" };
                Protol_area_data_str[num++] = new string[] { "2", "区域高度", "区域高度" };
                Protol_area_data_str[num++] = new string[] { "1", "是否有边框", "是否有边框" };
                Protol_area_data_str[num++] = new string[] { "8", "保留字节", "保留字节" };
                Protol_area_data_str[num++] = new string[] { "1", "文字风格布局模式", "文字风格布局模式" };
                Protol_area_data_str[num++] = new string[] { "1", "行间距", "行间距" };
                Protol_area_data_str[num++] = new string[] { "1", "行字体对齐方式", "行字体对齐方式" };
                Protol_area_data_str[num++] = new string[] { "1", "字间距", "字间距" };
                Protol_area_data_str[num++] = new string[] { "6", "保留字节", "保留字节" };
                Protol_area_data_str[num++] = new string[] { "1", "是否单行显示", "是否单行显示" };
                Protol_area_data_str[num++] = new string[] { "1", "是否自动换行", "是否自动换行" };
                Protol_area_data_str[num++] = new string[] { "1", "显示方式", "显示方式" };
                Protol_area_data_str[num++] = new string[] { "1", "退出方式", "退出方式" };
                Protol_area_data_str[num++] = new string[] { "1", "速度等级", "速度等级" };
                Protol_area_data_str[num++] = new string[] { "1", "停留时间", "停留时间" };
                Protol_area_data_str[num++] = new string[] { "1", "是否使能语音", "是否使能语音" };
                Protol_area_data_str[num++] = new string[] { "1", "发音人", "发音人" };
                Protol_area_data_str[num++] = new string[] { "1", "音量", "音量" };
                Protol_area_data_str[num++] = new string[] { "1", "语速", "语速" };
                Protol_area_data_str[num++] = new string[] { "1", "编码格式", "编码格式" };
                Protol_area_data_str[num++] = new string[] { "4", "重播次数", "重播次数" };
                Protol_area_data_str[num++] = new string[] { "4", "重默间隔", "重播间隔" };
                Protol_area_data_str[num++] = new string[] { "1", "语音参数保留长度", "语音参数保留长度" };
                Protol_area_data_str[num++] = new string[] { "4", "读音数据长度", "读音数据长度" };
                Protol_area_data_str[num++] = new string[] { "N", "读音数据", "读音数据" };
                Protol_area_data_str[num++] = new string[] { "4", "显示数据长度", "显示数据长度" };
                Protol_area_data_str[num++] = new string[] { "N", "显示数据", "显示数据" };
                Protol_area_data_str_len = num;
            }
            if (language == 1)
            {
                Protol_area_data_str[num++] = new string[] { "4", "area data length", "area data length" };
                Protol_area_data_str[num++] = new string[] { "1", "area number", "area number" };
                Protol_area_data_str[num++] = new string[] { "1", "dynamic area running mode", "dynamic area running mode" };
                Protol_area_data_str[num++] = new string[] { "2", "dynamic area timeout", "dynamic area timeout" };
                Protol_area_data_str[num++] = new string[] { "1", "relation to asyn program", "relation to asyn program" };
                Protol_area_data_str[num++] = new string[] { "2", "the related quantity to asyn program", "the related quantity to asyn program" };
                Protol_area_data_str[num++] = new string[] { "N", "item number of asyn program", "item number of asyn program" };
                Protol_area_data_str[num++] = new string[] { "1", "cover or not", "cover or not" };
                Protol_area_data_str[num++] = new string[] { "4", "reserved Byte", "reserved Byte" };
                Protol_area_data_str[num++] = new string[] { "1", "area type", "area type" };
                Protol_area_data_str[num++] = new string[] { "2", "X coordinate", "X coordinate" };
                Protol_area_data_str[num++] = new string[] { "2", "Y coordinate", "Y coordinate" };
                Protol_area_data_str[num++] = new string[] { "2", "Area width", "Area width" };
                Protol_area_data_str[num++] = new string[] { "2", "Area height", "Area height" };
                Protol_area_data_str[num++] = new string[] { "1", "with frame boarder or not", "with frame boarder or not" };
                Protol_area_data_str[num++] = new string[] { "8", "reserved Byte", "reserved Byte" };
                Protol_area_data_str[num++] = new string[] { "1", "Text style layout mode ", "Text style layout mode " };
                Protol_area_data_str[num++] = new string[] { "1", "Line spacing", "Line spacing" };
                Protol_area_data_str[num++] = new string[] { "1", "Line font alignment", "Line font alignment" };
                Protol_area_data_str[num++] = new string[] { "1", "words space", "word space" };
                Protol_area_data_str[num++] = new string[] { "6", "reserved Byte", "reserved Byte" };
                Protol_area_data_str[num++] = new string[] { "1", "Whether single line display", "Whether single line display" };
                Protol_area_data_str[num++] = new string[] { "1", "weather auto wrap lines", "weather auto wrap lines" };
                Protol_area_data_str[num++] = new string[] { "1", "display mode", "display mode" };
                Protol_area_data_str[num++] = new string[] { "1", "exit mode", "exit mode" };
                Protol_area_data_str[num++] = new string[] { "1", "speed level", "speed level" };
                Protol_area_data_str[num++] = new string[] { "1", "stay time", "stay time" };
                Protol_area_data_str[num++] = new string[] { "1", "use audio or not", "use audio or not" };
                Protol_area_data_str[num++] = new string[] { "1", "Pronunciation people", "Pronunciation people" };
                Protol_area_data_str[num++] = new string[] { "1", "volume", "volume" };
                Protol_area_data_str[num++] = new string[] { "1", "speak speed", "speak speed" };
                Protol_area_data_str[num++] = new string[] { "1", "Coding format", "Coding format" };
                Protol_area_data_str[num++] = new string[] { "4", "repeat times", "repeat times" };
                Protol_area_data_str[num++] = new string[] { "4", "repeat interval", "repeat interval" };
                Protol_area_data_str[num++] = new string[] { "1", "Speech parameter retention length", "Speech parameter retention length" };
                Protol_area_data_str[num++] = new string[] { "4", "pronunciation data length", "pronunciation data length" };
                Protol_area_data_str[num++] = new string[] { "N", "pronunciation data", "pronunciation data" };
                Protol_area_data_str[num++] = new string[] { "4", "displaying data length", "displaying data length" };
                Protol_area_data_str[num++] = new string[] { "N", "display data", "display data" };
                Protol_area_data_str_len = num;
            }

        }
        #endregion Six_Font_string_init

        #region Font_string_init
        public void Font_Card_Protocol_header_string_init()
        {
            int num = 0;

            if (language == 0)
            {
                Protol_header_str[num++] = new string[] { "2", "屏地址", "也是屏号" };
                Protol_header_str[num++] = new string[] { "2", "源地址", "源地址" };
                Protol_header_str[num++] = new string[] { "3", "保留字节", "保留字节" };
                Protol_header_str[num++] = new string[] { "1", "是否有条码", "是否有条码" };
                Protol_header_str[num++] = new string[] { "N", "条码", "条码" };
                Protol_header_str[num++] = new string[] { "1", "校验模式", "校验模式" };
                Protol_header_str[num++] = new string[] { "1", "显示模式", "显示模式" };
                Protol_header_str[num++] = new string[] { "1", "设备类型", "设备类型" };
                Protol_header_str[num++] = new string[] { "1", "协议版本号", "协议版本号" };
                Protol_header_str[num++] = new string[] { "2", "数据域长度", "数据域长度" };
                Protol_header_str_len = num;
            }
            if (language == 1)
            {
                Protol_header_str[num++] = new string[] { "2", "screen address", " also the screen number" };
                Protol_header_str[num++] = new string[] { "2", "source address", "source address" };
                Protol_header_str[num++] = new string[] { "3", "reserved Byte", "reserved Byte" };
                Protol_header_str[num++] = new string[] { "1", "bar code or not", "bar code or not" };
                Protol_header_str[num++] = new string[] { "N", "bar code", "bar code" };
                Protol_header_str[num++] = new string[] { "1", "verify mode", "verify mode" };
                Protol_header_str[num++] = new string[] { "1", "display mode", "display mode" };
                Protol_header_str[num++] = new string[] { "1", "device type", "device type" };
                Protol_header_str[num++] = new string[] { "1", "protocol version", "protocol version" };
                Protol_header_str[num++] = new string[] { "2", "Data field length", "Data field length" };
                Protol_header_str_len = num;
            }

        }

        public void Font_Card_Protocol_A1_00_cmd_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "格式化命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "格式化命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "formatting order" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "formatting order" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Prototol_CMD_len = num;
            }

        }
        public void Font_Card_Protocol_A1_05_cmd_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "开始写文件" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "开始写文件" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "1", "文件覆盖方式", "文件覆盖方式" };
                Protol_cmd_str[num++] = new string[] { "4", "文件名", "文件名" };
                Protol_cmd_str[num++] = new string[] { "4", "文件长度", "文件长度" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "start write file" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "start write file" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "1", "File overwrite mode", "File overwrite mode" };
                Protol_cmd_str[num++] = new string[] { "4", "file name", "file name" };
                Protol_cmd_str[num++] = new string[] { "4", "file length", "file length" };
                Prototol_CMD_len = num;
            }

        }
        public void Font_Card_Protocol_A1_06_cmd_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "写文件" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "写文件" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "4", "文件名", "文件名" };
                Protol_cmd_str[num++] = new string[] { "1", "是否是最后一包", "是否是最后一包" };
                Protol_cmd_str[num++] = new string[] { "2", "包号", "包号" };
                Protol_cmd_str[num++] = new string[] { "2", "包长", "包长" };
                Protol_cmd_str[num++] = new string[] { "4", "起始位置", "起始位置" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "write file" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "write file" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "4", "file name", "file name" };
                Protol_cmd_str[num++] = new string[] { "1", " the last package or not", " the last package or not" };
                Protol_cmd_str[num++] = new string[] { "2", "pocket number", "pocket number" };
                Protol_cmd_str[num++] = new string[] { "2", "pocket length", "pocket length" };
                Protol_cmd_str[num++] = new string[] { "4", "The starting position", "The starting position" };
                Prototol_CMD_len = num;
            }

        }

        public void Font_Card_Protocol_A1_06_pro_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_File_str[num++] = new string[] { "1", "文件类型", "文件类型" };
                Protol_File_str[num++] = new string[] { "4", "文件名", "文件名" };
                Protol_File_str[num++] = new string[] { "4", "文件长度", "文件长度" };
                Protol_File_str[num++] = new string[] { "1", "节目优先级", "节目优先级" };
                Protol_File_str[num++] = new string[] { "2", "节目播放方式", "节目播放方式" };
                Protol_File_str[num++] = new string[] { "1", "节目重复播放次数", "节目重复播放次数" };
                Protol_File_str[num++] = new string[] { "8", "节目生命周期", "节目生命周期" };
                Protol_File_str[num++] = new string[] { "1", "节目的星期属性", "节目的星期属性" };
                Protol_File_str[num++] = new string[] { "1", "定时节目位", "定时节目位" };
                Protol_File_str[num++] = new string[] { "1", "节目播放时段组数", "节目播放时段组数" };
                Protol_File_str[num++] = new string[] { "6", "播放组", "播放组" };
                Protol_File_str[num++] = new string[] { "1", "区域个数", "区域个数" };
                Prototol_File_len = num;
            }
            if (language == 1)
            {
                Protol_File_str[num++] = new string[] { "1", "file type", "file type" };
                Protol_File_str[num++] = new string[] { "4", "file name", "file name" };
                Protol_File_str[num++] = new string[] { "4", "file length", "file length" };
                Protol_File_str[num++] = new string[] { "1", "Program priority", "Program priority" };
                Protol_File_str[num++] = new string[] { "2", "Program broadcast mode", "Program broadcast mode" };
                Protol_File_str[num++] = new string[] { "1", "program repeat times", "program repeat times" };
                Protol_File_str[num++] = new string[] { "8", "Program life cycle", "Program life cycle" };
                Protol_File_str[num++] = new string[] { "1", "program property", "program property" };
                Protol_File_str[num++] = new string[] { "1", "timed program", "timed program" };
                Protol_File_str[num++] = new string[] { "1", "display group number", "display group number" };
                Protol_File_str[num++] = new string[] { "6", "display group", "display group" };
                Protol_File_str[num++] = new string[] { "1", "area quantity", "area quantity" };
                Prototol_File_len = num;
            }

        }
        public void Font_Card_Protocol_A1_06_pra_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_File_str[num++] = new string[] { "1", "文件类型", "文件类型" };
                Protol_File_str[num++] = new string[] { "4", "文件名", "文件名" };
                Protol_File_str[num++] = new string[] { "4", "文件长度", "文件长度" };
                Protol_File_str[num++] = new string[] { "8", "控制器名称", "控制器名称" };
                Protol_File_str[num++] = new string[] { "2", "控制器地址", "控制器地址" };
                Protol_File_str[num++] = new string[] { "1", "波特率", "波特率" };
                Protol_File_str[num++] = new string[] { "2", "屏幕宽度", "屏幕宽度" };
                Protol_File_str[num++] = new string[] { "2", "屏幕高度", "屏幕高度" };
                Protol_File_str[num++] = new string[] { "1", "颜色模式", "颜色模式" };
                Protol_File_str[num++] = new string[] { "1", "镜像属性", "镜像属性" };
                Protol_File_str[num++] = new string[] { "1", "OE极性", "OE极性" };
                Protol_File_str[num++] = new string[] { "1", "数据极性", "数据极性" };
                Protol_File_str[num++] = new string[] { "1", "行顺序", "行顺序" };
                Protol_File_str[num++] = new string[] { "1", "CLK分频倍数", "CLK分频倍数" };
                Protol_File_str[num++] = new string[] { "1", "OE宽度", "OE宽度" };
                Protol_File_str[num++] = new string[] { "1", "OE提前角", "OE提前角" };
                Protol_File_str[num++] = new string[] { "1", "错误处理模式", "错误处理模式" };
                Protol_File_str[num++] = new string[] { "1", "通讯超时时间", "通讯超时时间" };
                Protol_File_str[num++] = new string[] { "1", "控制器运行模式", "控制器运行模式" };
                Protol_File_str[num++] = new string[] { "1", "日志模式", "日志模式" };
                Protol_File_str[num++] = new string[] { "1", "语言版本", "语言版本" };
                Protol_File_str[num++] = new string[] { "1", "点阵模式", "点阵模式" };
                Protol_File_str[num++] = new string[] { "6", "保留字节", "保留字节" };
                //Protol_File_str[num++] = new string[] { "2", "CRC16校验值", "此工具不检测此校验值是否正确" };
                Prototol_File_len = num;
            }
            if (language == 1)
            {
                Protol_File_str[num++] = new string[] { "1", "文件类型", "文件类型" };
                Protol_File_str[num++] = new string[] { "4", "文件名", "文件名" };
                Protol_File_str[num++] = new string[] { "4", "文件长度", "文件长度" };
                Protol_File_str[num++] = new string[] { "8", "控制器名称", "控制器名称" };
                Protol_File_str[num++] = new string[] { "2", "控制器地址", "控制器地址" };
                Protol_File_str[num++] = new string[] { "1", "波特率", "波特率" };
                Protol_File_str[num++] = new string[] { "2", "屏幕宽度", "屏幕宽度" };
                Protol_File_str[num++] = new string[] { "2", "屏幕高度", "屏幕高度" };
                Protol_File_str[num++] = new string[] { "1", "颜色模式", "颜色模式" };
                Protol_File_str[num++] = new string[] { "1", "镜像属性", "镜像属性" };
                Protol_File_str[num++] = new string[] { "1", "OE极性", "OE极性" };
                Protol_File_str[num++] = new string[] { "1", "数据极性", "数据极性" };
                Protol_File_str[num++] = new string[] { "1", "行顺序", "行顺序" };
                Protol_File_str[num++] = new string[] { "1", "CLK分频倍数", "CLK分频倍数" };
                Protol_File_str[num++] = new string[] { "1", "OE宽度", "OE宽度" };
                Protol_File_str[num++] = new string[] { "1", "OE提前角", "OE提前角" };
                Protol_File_str[num++] = new string[] { "1", "错误处理模式", "错误处理模式" };
                Protol_File_str[num++] = new string[] { "1", "通讯超时时间", "通讯超时时间" };
                Protol_File_str[num++] = new string[] { "1", "控制器运行模式", "控制器运行模式" };
                Protol_File_str[num++] = new string[] { "1", "日志模式", "日志模式" };
                Protol_File_str[num++] = new string[] { "1", "语言版本", "语言版本" };
                Protol_File_str[num++] = new string[] { "1", "点阵模式", "点阵模式" };
                Protol_File_str[num++] = new string[] { "6", "保留字节", "保留字节" };
                //Protol_File_str[num++] = new string[] { "2", "CRC16校验值", "此工具不检测此校验值是否正确" };
                Prototol_File_len = num;
            }

        }

        public void Font_Card_Protocol_A1_06_cmd_filecrc_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_filecrc_str[num++] = new string[] { "2", "文件校验", "文件校验" };

                Prototol_CMD_filecrc_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_filecrc_str[num++] = new string[] { "2", "File CRC", "File CRC" };

                Prototol_CMD_filecrc_len = num;
            }

        }

        public void Font_Card_Protocol_A2_00_cmd_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "ping命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "ping命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "ping order" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "ping order" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Prototol_CMD_len = num;
            }

        }
        public void Font_Card_Protocol_A2_01_cmd_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "系统复位命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "系统复位命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "System reset command" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "System reset command" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Prototol_CMD_len = num;
            }

        }
        public void Font_Card_Protocol_A1_01_cmd_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "删除文件命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "删除文件命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令处理状态", "命令处理状态" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "2", "删除文件个数", "删除文件个数" };
                Protol_cmd_str[num++] = new string[] { "4", "文件名", "文件名" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "Delete file command" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "Delete file command" };
                Protol_cmd_str[num++] = new string[] { "1", "Command processing status", "Command processing status" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "2", "delete file quantity", "delete file quantity" };
                Protol_cmd_str[num++] = new string[] { "4", "file name", "file name" };
                Prototol_CMD_len = num;
            }

        }

        public void Font_Card_Protocol_A1_02_cmd_string_init()
        {
            int num = 0;

            num = 0;

            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "控制器状态命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "控制器状态命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令处理状态", "命令处理状态" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器开关机状态", "控制器开关机状态" };
                Protol_cmd_str[num++] = new string[] { "1", "亮度", "亮度" };
                Protol_cmd_str[num++] = new string[] { "8", "控制器时间", "控制器时间" };
                Protol_cmd_str[num++] = new string[] { "1", "节目个数", "节目个数" };
                Protol_cmd_str[num++] = new string[] { "4", "当前播放节目名", "当前播放节目名" };
                Protol_cmd_str[num++] = new string[] { "1", "特殊动态区标志", "特殊动态区标志" };
                Protol_cmd_str[num++] = new string[] { "1", "特殊动态区页数", "特殊动态区页数" };
                Protol_cmd_str[num++] = new string[] { "1", "动态区个数", "动态区个数" };
                Protol_cmd_str[num++] = new string[] { "N", "动态区ID", "动态区ID" };
                Protol_cmd_str[num++] = new string[] { "16", "条码", "条码" };
                Protol_cmd_str[num++] = new string[] { "12", "网络ID", "网络ID" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "control status order" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "control status order" };
                Protol_cmd_str[num++] = new string[] { "1", "Command processing status", "Command processing status" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "1", "the state of control", "power on/off" };
                Protol_cmd_str[num++] = new string[] { "1", "lightness", "lightness" };
                Protol_cmd_str[num++] = new string[] { "8", "time of control", "time of control(BCD)" };
                Protol_cmd_str[num++] = new string[] { "1", "number of programs", "number of programs" };
                Protol_cmd_str[num++] = new string[] { "4", "displaying program'name", "displaying program'name" };
                Protol_cmd_str[num++] = new string[] { "1", "flag of special dynamic", "flag of special dynamic" };
                Protol_cmd_str[num++] = new string[] { "1", "pages of special dynamic", "pages of special dynamic" };
                Protol_cmd_str[num++] = new string[] { "1", "number of dynamics", "number of dynamics" };
                Protol_cmd_str[num++] = new string[] { "N", "dynamic area ID", "dynamic area ID" };
                Protol_cmd_str[num++] = new string[] { "16", "bar code", "bar code" };
                Protol_cmd_str[num++] = new string[] { "12", "Network ID", "Network ID" };
                Prototol_CMD_len = num;
            }

        }

        public void Font_Card_Protocol_A3_00_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "强制开关机命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "强制开关机命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "1", "开关机状态", "0x01开机  0x02关机" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "Mandatory switch machine command" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "Mandatory switch machine command" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "1", "power on/off status", "0x01 power on  0x02 power off" };
                Prototol_CMD_len = num;
            }

        }
        public void Font_Card_Protocol_A3_01_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "定时开关机" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "定时开关机" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "1", "定时器组数", "定时器组数" };
                Protol_cmd_str[num++] = new string[] { "2", "开机时间", "开机时间（BCD码）" };
                Protol_cmd_str[num++] = new string[] { "2", "关机时间", "关机时间（BCD码）" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "timing power on/off" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "timing power on/off" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "1", "Number of timer sets", "Number of timer sets" };
                Protol_cmd_str[num++] = new string[] { "2", "time of power on ", "time of power on（BCD）" };
                Protol_cmd_str[num++] = new string[] { "2", "time of power off", "time of power off（BCD）" };
                Prototol_CMD_len = num;
            }

        }

        public void Font_Card_Protocol_A3_04_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "锁定/解锁节目" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "锁定/解锁节目" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "1", "锁定状态保存方式", "0x00掉电不保存  0x01掉电保存" };
                Protol_cmd_str[num++] = new string[] { "1", "锁定状态", "0x00解锁  0x01锁定" };
                Protol_cmd_str[num++] = new string[] { "4", "节目名", "节目名" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "Lock/unlock programs" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "Lock/unlock programs" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "1", "lock status save mode", "0x00:don't save if power off  0x01:save if power off" };
                Protol_cmd_str[num++] = new string[] { "1", "locked or not", "0x00:unlock  0x01:lock" };
                Protol_cmd_str[num++] = new string[] { "4", "program name", "program name" };
                Prototol_CMD_len = num;
            }

        }
        public void Font_Card_Protocol_A3_08_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "取消定时开关机命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "取消定时开关机命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "cancel timing power on/off order" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "cancel timing power on/off order" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Prototol_CMD_len = num;
            }

        }

        public void Font_Card_Protocol_A3_10_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "清屏命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "清屏命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制器是否回复", "控制器是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "clear screen command" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "clear screen command" };
                Protol_cmd_str[num++] = new string[] { "1", "Whether the controller reply", "Whether the controller reply" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Prototol_CMD_len = num;
            }

        }

        public void Font_Card_Protocol_A3_06_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "动态区更新命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "动态区更新命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制是否回复", "控制是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "1", "删除区域个数", "删除区域" };
                Protol_cmd_str[num++] = new string[] { "N", "删除区域ID", "删除区域ID" };
                Protol_cmd_str[num++] = new string[] { "1", "更新区域个数", "更新区域个数" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "update the dynamic " };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "update the dynamic" };
                Protol_cmd_str[num++] = new string[] { "1", "reply or not", "reply or not" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "1", "delete area quantity", "delete area" };
                Protol_cmd_str[num++] = new string[] { "N", "delete area ID", "delete area ID" };
                Protol_cmd_str[num++] = new string[] { "1", "update area quantity", "update area quantity" };
                Prototol_CMD_len = num;
            }

        }
        public void Font_Card_Protocol_A2_03_cmd_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_cmd_str[num++] = new string[] { "1", "命令分组", "校时命令" };
                Protol_cmd_str[num++] = new string[] { "1", "命令编号", "校时命令" };
                Protol_cmd_str[num++] = new string[] { "1", "控制是否回复", "控制是否回复" };
                Protol_cmd_str[num++] = new string[] { "2", "保留字节", "保留字节" };
                Protol_cmd_str[num++] = new string[] { "8", "控制器时间", "控制器时间" };
                Prototol_CMD_len = num;
            }
            if (language == 1)
            {
                Protol_cmd_str[num++] = new string[] { "1", "The command group", "timing order" };
                Protol_cmd_str[num++] = new string[] { "1", "command number", "timing order" };
                Protol_cmd_str[num++] = new string[] { "1", "reply or not", "reply or not" };
                Protol_cmd_str[num++] = new string[] { "2", "reserved Byte", "reserved Byte" };
                Protol_cmd_str[num++] = new string[] { "8", "time of control", "time of control" };
                Prototol_CMD_len = num;
            }

        }

        public void Font_Card_Protocol_area_data_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
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
            }
            if (language == 1)
            {
                Protol_area_data_str[num++] = new string[] { "2", "area data length", "area data length" };
                Protol_area_data_str[num++] = new string[] { "1", "area type", "area type" };
                Protol_area_data_str[num++] = new string[] { "2", "X coordinate", "X coordinate" };
                Protol_area_data_str[num++] = new string[] { "2", "Y coordinate", "Y coordinate" };
                Protol_area_data_str[num++] = new string[] { "2", "Area width", "Area width" };
                Protol_area_data_str[num++] = new string[] { "2", "Area height", "Area height" };
                Protol_area_data_str[num++] = new string[] { "1", "dynamic area items", "dynamic area items" };
                Protol_area_data_str[num++] = new string[] { "1", "line space", "line space" };
                Protol_area_data_str[num++] = new string[] { "1", "dynamic area running mode", "dynamic area running mode" };
                Protol_area_data_str[num++] = new string[] { "2", "dynamic area timeout", "overtime time" };
                Protol_area_data_str[num++] = new string[] { "1", "use audio or not", "use audio or not" };
                Protol_area_data_str[num++] = new string[] { "1", "Pronunciation people/Number of pronunciations", "Bit0-Bit3Pronunciation people，Bit4-Bit7 Number of plays" };
                Protol_area_data_str[num++] = new string[] { "1", "volume", "volume" };
                Protol_area_data_str[num++] = new string[] { "1", "speak speed", "speak speed" };
                Protol_area_data_str[num++] = new string[] { "4", "pronunciation data length", "data length" };
                Protol_area_data_str[num++] = new string[] { "N", "pronunciation data", "pronunciation data" };
                Protol_area_data_str[num++] = new string[] { "1", "extension bits quantity", "extension bits quantity" };
                Protol_area_data_str[num++] = new string[] { "1", "reserved", "reserved" };
                Protol_area_data_str[num++] = new string[] { "1", "Typography", "Typography" };
                Protol_area_data_str[num++] = new string[] { "1", "Font alignment", "the way of Font alignment" };
                Protol_area_data_str[num++] = new string[] { "1", "single line to display", "single line to display" };
                Protol_area_data_str[num++] = new string[] { "1", "Whether to wrap", "Whether to wrap" };
                Protol_area_data_str[num++] = new string[] { "1", "display mode", "display mode" };
                Protol_area_data_str[num++] = new string[] { "1", "exit mode", "exit mode" };
                Protol_area_data_str[num++] = new string[] { "1", "display speed", "display speed" };
                Protol_area_data_str[num++] = new string[] { "1", "stay time", "stay time" };
                Protol_area_data_str[num++] = new string[] { "4", "display data length", "data length" };
                Protol_area_data_str[num++] = new string[] { "N", "display data", "display data" };
                Protol_area_data_str_len = num;
            }

        }

        public void Font_Card_Protocol_CRC_string_init()
        {
            int num = 0;

            num = 0;
            if (language == 0)
            {
                Protol_crc_str[num++] = new string[] { "2", "CRC校验", "CRC校验" };
                Protol_crc_str_len = num;
            }
            if (language == 1)
            {
                Protol_crc_str[num++] = new string[] { "2", "CRC check", "CRC check" };
                Protol_crc_str_len = num;
            }

        }

        #endregion Font_string_init

        public static int crc16(byte[] data, UInt32 size)
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


        #region Six_Image_Protocol_Analysis
        public int Six_Image_Protocol_Deal_With()
        {
            UInt32 i, num, num1, num2, num3;
            string data_str;
            int flg = 0;

            i = 0;
            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";
            try
            {
                m_oonbon_Protocol = new Conbon_Protocol();

                Six_Font_Protocol_header_string_init();

                m_oonbon_Protocol.Prototol_Header = new CProtolPart[Protol_header_str_len];
                for (num = 0; num < Protol_header_str_len; num++)
                {
                    m_oonbon_Protocol.Prototol_Header[num] = new CProtolPart();
                    m_oonbon_Protocol.Prototol_Header[num].bEnable = 0;
                    m_oonbon_Protocol.Prototol_Header[num].para = Protol_header_str[num][1];
                    m_oonbon_Protocol.Prototol_Header[num].Leng = Convert.ToUInt32(Protol_header_str[num][0]);
                    m_oonbon_Protocol.Prototol_Header[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_Header[num].Leng];
                    m_oonbon_Protocol.Prototol_Header[num].describe = Protol_header_str[num][2];
                }

                /*包头数据*/
                for (num = 0; num < m_oonbon_Protocol.Prototol_Header.Length; num++)
                {
                    if (m_oonbon_Protocol.Prototol_Header[num].para == "协议版本号")
                    {
                        m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] = myarray[i++];
                        if (m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] != 0xF0)
                        {
                            MessageBox.Show("协议版本号不是0xf0，不是6代字库协议数据，请重选协议类型");
                            return 1;
                        }
                        continue;
                    }
                    m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_Header[num].Leng; num1++)
                    {
                        m_oonbon_Protocol.Prototol_Header[num].byteMemValue[num1] = myarray[i++];
                    }
                }
                m_region_par_init();
                switch (myarray[i + 1])
                {
                    case 0xa7:
                        switch (myarray[i + 2])
                        {
                            case 0x00://更新点阵动态区
                                Six_Image_Protocol_A7_00_cmd_string_init();
                                Six_Image_Protocol_area_data_string_init();
                                Six_Image_Protocol_Page_data_string_init();
                                i = Six_Image_A7_00_Protocol(myarray, i);
                                break;
                            default:
                                flg = 1;
                                break;
                        }
                        break;
                    default:
                        flg = 1;
                        break;
                }
                if (flg == 1)
                {
                    MessageBox.Show("目前版本不支持" + myarray[i + 1].ToString("X2") + " " + myarray[i + 2].ToString("X2"));
                    return 1;
                }
                else
                {
                    if (i != 0)
                    {
                        Font_Card_Protocol_CRC_string_init();

                        m_oonbon_Protocol.Prototol_CRC = new CProtolPart();
                        m_oonbon_Protocol.Prototol_CRC.para = Protol_crc_str[0][1];
                        m_oonbon_Protocol.Prototol_CRC.Leng = Convert.ToUInt32(Protol_crc_str[0][0]);
                        m_oonbon_Protocol.Prototol_CRC.byteMemValue = new byte[m_oonbon_Protocol.Prototol_CRC.Leng];
                        m_oonbon_Protocol.Prototol_CRC.describe = Protol_crc_str[0][2];
                        m_oonbon_Protocol.Prototol_CRC.bEnable = 1;

                        data_value = (int)(((myarray[i + 1] & 0xff) << 8) |
                                          ((myarray[i + 0] & 0xff) << 0));
                        int mycrc = crc16(myarray, PHY0_flag1.RCV_data_num - 2);
                        if (mycrc == data_value)
                        {
                            m_oonbon_Protocol.Prototol_CRC.describe = Protol_crc_str[0][2];
                        }
                        else
                        {
                            if (data_value == 0xffff)
                            {
                                m_oonbon_Protocol.Prototol_CRC.describe = "不进行CRC校验";
                            }
                            else
                            {
                                if (language == 0)
                                {
                                    m_oonbon_Protocol.Prototol_CRC.describe = "CRC校验错误";
                                }
                                if (language == 1)
                                {
                                    m_oonbon_Protocol.Prototol_CRC.describe = "CRC error";
                                }


                            }
                        }

                        for (num = 0; num < m_oonbon_Protocol.Prototol_CRC.Leng; num++)
                        {
                            m_oonbon_Protocol.Prototol_CRC.byteMemValue[num] = myarray[i++];
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;
        }

        public UInt32 Six_Image_A7_00_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;


            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

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

                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                }
                if (m_oonbon_Protocol.Prototol_CMD[num].para == "更新区域个数")
                {
                    m_oonbon_Protocol.Area_Num = m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0];
                }
                num++;

            }

            i = Six_Image_Area_Data_Protocol(myarray, i);

            return i;
        }

        public UInt32 Six_Image_Area_Data_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            Cregion_parameter region_par = new Cregion_parameter();

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";


            /*区域数据格式*/
            m_oonbon_Protocol.SixImage_Prototol_area_data = new CSixImagePrototolAreaPart[m_oonbon_Protocol.Area_Num];
            for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
            {
                m_oonbon_Protocol.SixImage_Prototol_area_data[num] = new CSixImagePrototolAreaPart();
                CSixImagePrototolAreaPart AreaPart = new CSixImagePrototolAreaPart();
                AreaPart = m_oonbon_Protocol.SixImage_Prototol_area_data[num];

                AreaPart.Prototol_Area_Para = new CProtolPart[Protol_area_data_str_len];
                for (num1 = 0; num1 < Protol_area_data_str_len; num1++)
                {
                    AreaPart.Prototol_Area_Para[num1] = new CProtolPart();
                    CProtolPart ProtolPart = new CProtolPart();
                    ProtolPart = AreaPart.Prototol_Area_Para[num1];

                    ProtolPart.para = Protol_area_data_str[num1][1];
                    if (Protol_area_data_str[num1][0] != "N")
                    {
                        ProtolPart.Leng = Convert.ToUInt32(Protol_area_data_str[num1][0]);
                        ProtolPart.byteMemValue = new byte[ProtolPart.Leng];
                    }
                    else
                    {
                        ProtolPart.Leng = 0XFFFFFFFF;
                    }
                    ProtolPart.describe = Protol_area_data_str[num1][2];
                    ProtolPart.bEnable = 0;
                }

                for (num1 = 0; num1 < AreaPart.Prototol_Area_Para.Length; num1++)
                {
                    CProtolPart ProtolPart = new CProtolPart();
                    ProtolPart = AreaPart.Prototol_Area_Para[num1];

                    if ((ProtolPart.para == "X坐标") || (ProtolPart.para == "Y坐标") || (ProtolPart.para == "区域宽度") || (ProtolPart.para == "区域高度"))
                    {
                        ProtolPart = AreaPart.Prototol_Area_Para[num1];
                        ProtolPart.bEnable = 1;
                        ProtolPart.byteMemValue[0] = myarray[i++];
                        ProtolPart.byteMemValue[1] = myarray[i++];
                        num3 = (UInt32)(((ProtolPart.byteMemValue[1] & 0xff) << 8) | ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                        ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString();
                        if (ProtolPart.para == "Y坐标")
                        {
                            region_par.Y = num3;
                        }
                        if (ProtolPart.para == "区域高度")
                        {
                            region_par.height = num3;
                        }
                        if (ProtolPart.para == "X坐标")
                        {
                            region_par.X = num3;
                        }
                        if (ProtolPart.para == "区域宽度")
                        {
                            region_par.width = num3;
                        }
                        continue;
                    }
                    if (ProtolPart.para == "是否有边框")
                    {
                        ProtolPart.bEnable = 1;
                        ProtolPart.byteMemValue[0] = myarray[i++];
                        region_par.ID = (UInt32)ProtolPart.byteMemValue[0];
                        if (ProtolPart.byteMemValue[0] == 1)
                        {
                            MessageBox.Show("不支持解析带有边框的动态区数据");
                            return 0;
                        }
                        else
                        {
                            continue;
                        }

                    }
                    if (ProtolPart.para == "区域序号")
                    {
                        ProtolPart.bEnable = 1;
                        ProtolPart.byteMemValue[0] = myarray[i++];
                        region_par.ID = (UInt32)ProtolPart.byteMemValue[0];
                        continue;
                    }
                    if (ProtolPart.para == "关联异步节目个数")
                    {
                        /*关联异步节目个数*/
                        num2 = 0;
                        for (num3 = 0; num3 < ProtolPart.Leng; num3++)
                        {
                            ProtolPart.bEnable = 1;
                            ProtolPart.byteMemValue[num3] = myarray[i++];
                            num2 = num2 + ProtolPart.byteMemValue[num3];
                        }
                        num1++;

                        /*关联异步节目ID*/
                        if (num2 != 0)
                        {
                            ProtolPart = AreaPart.Prototol_Area_Para[num1];
                            ProtolPart.Leng = num2 * 2;//关联的异步节目的ID编号是2个字节
                            ProtolPart.byteMemValue = new byte[ProtolPart.Leng];
                            for (num3 = 0; num3 < num2 * 2; num3++)
                            {
                                ProtolPart.bEnable = 1;
                                ProtolPart.byteMemValue[num3] = myarray[i++];
                            }
                        }
                        continue;
                    }

                    if (ProtolPart.para == "是否使能语音")
                    {
                        /*是否使能语音*/
                        ProtolPart.byteMemValue[0] = myarray[i++];
                        ProtolPart.bEnable = 1;
                        num1++;

                        if (ProtolPart.byteMemValue[0] == 0)//不使能语音
                        {
                            num1 += 11;
                            continue;
                        }
                        else
                        {
                            if (ProtolPart.byteMemValue[0] != 1)
                            {
                                MessageBox.Show("是否使能语音字节只能为0、1");
                                return 0;
                            }
                            num3 = num1;
                            for (; num1 < num3 + 6; num1++)
                            {
                                ProtolPart = AreaPart.Prototol_Area_Para[num1];
                                for (num2 = 0; num2 < ProtolPart.Leng; num2++)
                                {
                                    ProtolPart.byteMemValue[num2] = myarray[i++];
                                }
                                ProtolPart.bEnable = 1;
                            }
                            /*语音参数保留长度*/
                            ProtolPart = AreaPart.Prototol_Area_Para[num1];
                            ProtolPart.bEnable = 1;
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            num1++;
                            if (ProtolPart.byteMemValue[0] == 0)
                            {
                                num1 += 3;
                            }
                            else
                            {
                                if (ProtolPart.byteMemValue[0] != 3)
                                {
                                    MessageBox.Show("目前版本不支持语音参数保留长度 不等于3或不等于0 的情况");
                                    return 1;
                                }
                                else
                                {
                                    num3 = num1;
                                    for (; num1 < num3 + 3; num1++)
                                    {
                                        ProtolPart = AreaPart.Prototol_Area_Para[num1];
                                        for (num2 = 0; num2 < ProtolPart.Leng; num2++)
                                        {
                                            ProtolPart.byteMemValue[num2] = myarray[i++];
                                        }
                                        ProtolPart.bEnable = 1;
                                    }
                                }
                            }

                            /*读音数据长度*/
                            ProtolPart = AreaPart.Prototol_Area_Para[num1];
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
                            ProtolPart = AreaPart.Prototol_Area_Para[num1];
                            ProtolPart.byteMemValue = new byte[num3];
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
                            continue;
                        }
                    }


                    for (num2 = 0; num2 < ProtolPart.Leng; num2++)
                    {
                        ProtolPart.byteMemValue[num2] = myarray[i++];
                    }
                    ProtolPart.bEnable = 1;

                    if (ProtolPart.para == "数据页数")
                    {
                        AreaPart.Page_Num = (int)(
                                        ((ProtolPart.byteMemValue[1] & 0xff) << 8) |
                                        ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                    }
                }

                AreaPart.SixImage_Prototol_Page_Part = new CSixImagePrototolPagePart[AreaPart.Page_Num];
                for (num1 = 0; num1 < AreaPart.Page_Num; num1++)
                {
                    AreaPart.SixImage_Prototol_Page_Part[num1] = new CSixImagePrototolPagePart();
                    CSixImagePrototolPagePart PagePart = new CSixImagePrototolPagePart();
                    PagePart = AreaPart.SixImage_Prototol_Page_Part[num1];

                    PagePart.Prototol_Page_Part = new CProtolPart[Protol_Page_data_str_len];
                    for (num2 = 0; num2 < Protol_Page_data_str_len; num2++)
                    {
                        PagePart.Prototol_Page_Part[num2] = new CProtolPart();
                        CProtolPart ProtolPart = new CProtolPart();
                        ProtolPart = PagePart.Prototol_Page_Part[num2];

                        ProtolPart.para = Protol_Page_data_str[num2][1];
                        ProtolPart.Leng = Convert.ToUInt32(Protol_Page_data_str[num2][0]);
                        ProtolPart.byteMemValue = new byte[ProtolPart.Leng];
                        ProtolPart.describe = Protol_Page_data_str[num2][2];
                        ProtolPart.bEnable = 0;
                    }

                    for (num2 = 0; num2 < PagePart.Prototol_Page_Part.Length; num2++)
                    {
                        CProtolPart ProtolPart = new CProtolPart();
                        ProtolPart = PagePart.Prototol_Page_Part[num2];
                        for (num3 = 0; num3 < ProtolPart.Leng; num3++)
                        {
                            ProtolPart.byteMemValue[num3] = myarray[i++];
                        }
                        ProtolPart.bEnable = 1;
                        if (ProtolPart.para == "本页数据偏移量")
                        {
                            PagePart.offset = (UInt32)(((ProtolPart.byteMemValue[3] & 0xff) << 24) |
                                                       ((ProtolPart.byteMemValue[2] & 0xff) << 16) |
                                                       ((ProtolPart.byteMemValue[1] & 0xff) << 8) |
                                                       ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                        }
                        if (ProtolPart.para == "本页数据长度")
                        {
                            PagePart.len = (UInt32)(((ProtolPart.byteMemValue[3] & 0xff) << 24) |
                                                       ((ProtolPart.byteMemValue[2] & 0xff) << 16) |
                                                       ((ProtolPart.byteMemValue[1] & 0xff) << 8) |
                                                       ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                        }
                    }
                }
                for (num3 = 0; num3 < m_region_par.Length; num3++)
                {
                    if (m_region_par[num3].bEnable != 1)
                    {
                        m_region_par[num3].bEnable = 1;

                        m_region_par[num3].ID = region_par.ID;
                        m_region_par[num3].X = region_par.X;
                        m_region_par[num3].Y = region_par.Y;
                        m_region_par[num3].width = region_par.width;
                        m_region_par[num3].height = region_par.height;

                        break;
                    }
                }


            }


            m_oonbon_Protocol.Disply_Data = new byte[PHY0_flag1.RCV_data_num - i - 2];
            for (num = 0; num < m_oonbon_Protocol.Disply_Data.Length; num++)
            {
                m_oonbon_Protocol.Disply_Data[num] = myarray[i++];
            }
            return i;
        }

        #endregion Six_Image_Protocol_Analysis


        #region Six_Font_Protocol_Analysis

        public int Six_Font_Protocol_Deal_With()
        {
            UInt32 i, num, num1, num2, num3;
            string data_str;
            int flg = 0;

            i = 0;
            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            m_oonbon_Protocol = new Conbon_Protocol();

            Six_Font_Protocol_header_string_init();

            m_oonbon_Protocol.Prototol_Header = new CProtolPart[Protol_header_str_len];
            for (num = 0; num < Protol_header_str_len; num++)
            {
                m_oonbon_Protocol.Prototol_Header[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_Header[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_Header[num].para = Protol_header_str[num][1];
                m_oonbon_Protocol.Prototol_Header[num].Leng = Convert.ToUInt32(Protol_header_str[num][0]);
                m_oonbon_Protocol.Prototol_Header[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_Header[num].Leng];
                m_oonbon_Protocol.Prototol_Header[num].describe = Protol_header_str[num][2];
            }

            /*包头数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_Header.Length; num++)
            {
                if (m_oonbon_Protocol.Prototol_Header[num].para == "协议版本号")
                {
                    m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                    m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] = myarray[i++];
                    if (m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] != 0xF0)
                    {
                        MessageBox.Show("协议版本号不是0xf0，不是6代字库协议数据，请重选协议类型");
                        return 1;
                    }
                    continue;
                }
                m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_Header[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_Header[num].byteMemValue[num1] = myarray[i++];
                }
            }
            m_region_par_init();
            switch (myarray[i + 1])
            {
                case 0xa7:
                    switch (myarray[i + 2])
                    {
                        case 0x06://更新字库动态区
                            Six_Font_Protocol_A7_06_cmd_string_init();
                            Six_Font_Protocol_area_data_string_init();
                            i = Six_Font_A7_06_Protocol(myarray, i);//可以调用ping命令格式
                            break;
                        default:
                            flg = 1;
                            break;
                    }
                    break;
                default:
                    flg = 1;
                    break;
            }
            if (flg == 1)
            {
                MessageBox.Show("目前版本不支持" + myarray[i + 1].ToString("X2") + " " + myarray[i + 2].ToString("X2"));
                return 1;
            }
            else
            {
                if (i != 0)
                {
                    Font_Card_Protocol_CRC_string_init();

                    m_oonbon_Protocol.Prototol_CRC = new CProtolPart();
                    m_oonbon_Protocol.Prototol_CRC.para = Protol_crc_str[0][1];
                    m_oonbon_Protocol.Prototol_CRC.Leng = Convert.ToUInt32(Protol_crc_str[0][0]);
                    m_oonbon_Protocol.Prototol_CRC.byteMemValue = new byte[m_oonbon_Protocol.Prototol_CRC.Leng];
                    m_oonbon_Protocol.Prototol_CRC.describe = Protol_crc_str[0][2];
                    m_oonbon_Protocol.Prototol_CRC.bEnable = 1;

                    data_value = (int)(((myarray[i + 1] & 0xff) << 8) |
                                      ((myarray[i + 0] & 0xff) << 0));
                    int mycrc = crc16(myarray, PHY0_flag1.RCV_data_num - 2);
                    if (mycrc == data_value)
                    {
                        m_oonbon_Protocol.Prototol_CRC.describe = Protol_crc_str[0][2];
                    }
                    else
                    {
                        if (data_value == 0xffff)
                        {
                            m_oonbon_Protocol.Prototol_CRC.describe = "不进行CRC校验";
                        }
                        else
                        {
                            if (language == 0)
                            {
                                m_oonbon_Protocol.Prototol_CRC.describe = "CRC校验错误";
                            }
                            if (language == 1)
                            {
                                m_oonbon_Protocol.Prototol_CRC.describe = "CRC error";
                            }
                        }
                    }

                    for (num = 0; num < m_oonbon_Protocol.Prototol_CRC.Leng; num++)
                    {
                        m_oonbon_Protocol.Prototol_CRC.byteMemValue[num] = myarray[i++];
                    }

                }



            }

            return 0;
        }

        public UInt32 Six_Font_A7_06_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

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
                        m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                    }
                    if (m_oonbon_Protocol.Prototol_CMD[num].para == "更新区域个数")
                    {
                        m_oonbon_Protocol.Area_Num = m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0];
                    }
                    num++;
                }
            }

            i = Six_Font_Area_Data_Protocol(myarray, i);

            return i;
        }

        public UInt32 Six_Font_Area_Data_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            Cregion_parameter region_par = new Cregion_parameter();

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";
            try
            {
                m_oonbon_Protocol.Prototol_area_data = new CPrototolAreaPart[m_oonbon_Protocol.Area_Num];
                for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
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
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part.Length; num1++)
                    {
                        CProtolPart ProtolPart = new CProtolPart();
                        ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                        if ((ProtolPart.para == "X坐标") || (ProtolPart.para == "Y坐标") || (ProtolPart.para == "区域宽度") || (ProtolPart.para == "区域高度"))
                        {
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.bEnable = 1;
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.byteMemValue[1] = myarray[i++];
                            num3 = (UInt32)(((ProtolPart.byteMemValue[1] & 0xff) << 8) | ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                            ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString();

                            if (ProtolPart.para == "Y坐标")
                            {
                                region_par.Y = num3;
                            }
                            if (ProtolPart.para == "区域高度")
                            {
                                region_par.height = num3;
                            }
                            if (ProtolPart.para == "X坐标")
                            {
                                region_par.X = num3;
                            }
                            if (ProtolPart.para == "区域宽度")
                            {
                                region_par.width = num3;
                            }

                            continue;
                        }
                        if (ProtolPart.para == "区域序号")
                        {
                            ProtolPart.bEnable = 1;
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            region_par.ID = (UInt32)ProtolPart.byteMemValue[0];
                            continue;
                        }
                        if ((ProtolPart.para == "显示数据长度"))
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
                            continue;
                        }


                        if (ProtolPart.para == "关联异步节目个数")
                        {
                            /*关联异步节目个数*/
                            num2 = 0;
                            for (num3 = 0; num3 < ProtolPart.Leng; num3++)
                            {
                                ProtolPart.bEnable = 1;
                                ProtolPart.byteMemValue[num3] = myarray[i++];
                                num2 = num2 + ProtolPart.byteMemValue[num3];
                            }
                            num1++;

                            /*关联异步节目ID*/
                            if (num2 != 0)
                            {
                                ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                                ProtolPart.Leng = num2 * 2;//关联异步节目编号ID是2个字节
                                ProtolPart.byteMemValue = new byte[ProtolPart.Leng];
                                for (num1 = 0; num1 < num2 * 2; num1++)
                                {
                                    ProtolPart.bEnable = 1;
                                    ProtolPart.byteMemValue[num1] = myarray[i++];
                                }
                            }
                            continue;
                        }

                        if (ProtolPart.para == "是否使能语音")
                        {
                            /*是否使能语音*/
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;

                            if (ProtolPart.byteMemValue[0] == 0)//不使能语音
                            {
                                num1 += 8;
                                continue;
                            }
                            if ((ProtolPart.byteMemValue[0] != 1) && (ProtolPart.byteMemValue[0] != 2))
                            {
                                MessageBox.Show("是否使能语音字节只能为0、1、2");
                                return 0;
                            }
                            if (ProtolPart.byteMemValue[0] == 1)//播放显示文本内容
                            {
                                num3 = num1;
                                for (; num1 < num3 + 6; num1++)
                                {
                                    ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                                    for (num2 = 0; num2 < ProtolPart.Leng; num2++)
                                    {
                                        ProtolPart.byteMemValue[num2] = myarray[i++];
                                    }
                                    ProtolPart.bEnable = 1;
                                }
                                /*语音参数保留长度*/
                                ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                                ProtolPart.bEnable = 1;
                                ProtolPart.byteMemValue[0] = myarray[i++];
                                if (ProtolPart.byteMemValue[0] > 0)
                                {
                                    MessageBox.Show("目前版本不支持语音参数保留长度大于0的情况");
                                    return 1;
                                }
                                num1 += 2;
                                continue;
                            }
                            if (ProtolPart.byteMemValue[0] == 2)//播放sounddata文本内容
                            {
                                num3 = num1;
                                for (; num1 < num3 + 6; num1++)
                                {
                                    ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                                    for (num2 = 0; num2 < ProtolPart.Leng; num2++)
                                    {
                                        ProtolPart.byteMemValue[num2] = myarray[i++];
                                    }
                                    ProtolPart.bEnable = 1;
                                }
                                /*语音参数保留长度*/
                                ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                                ProtolPart.bEnable = 1;
                                ProtolPart.byteMemValue[0] = myarray[i++];
                                num1++;
                                if (ProtolPart.byteMemValue[0] > 0)
                                {
                                    MessageBox.Show("目前版本不支持语音参数保留长度大于0的情况");
                                    return 1;
                                }

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
                                continue;
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
                            }
                        }

                    }

                    for (num3 = 0; num3 < m_region_par.Length; num3++)
                    {
                        if (m_region_par[num3].bEnable != 1)
                        {
                            m_region_par[num3].bEnable = 1;

                            m_region_par[num3].ID = region_par.ID;
                            m_region_par[num3].X = region_par.X;
                            m_region_par[num3].Y = region_par.Y;
                            m_region_par[num3].width = region_par.width;
                            m_region_par[num3].height = region_par.height;

                            break;
                        }
                    }
                }

                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        #endregion Six_Font_Protocol_Analysis

        #region Font_Card_Protocol_Analysis

        public int Font_Card_Protocol_Deal_With()
        {
            UInt32 i, num, num1, num2, num3;
            string data_str;
            int flg = 0;

            i = 0;
            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            Font_Card_Protocol_header_string_init();

            m_oonbon_Protocol = new Conbon_Protocol();

            m_oonbon_Protocol.Prototol_Header = new CProtolPart[Protol_header_str_len];
            for (num = 0; num < Protol_header_str_len; num++)
            {
                m_oonbon_Protocol.Prototol_Header[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_Header[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_Header[num].para = Protol_header_str[num][1];
                if (m_oonbon_Protocol.Prototol_Header[num].para == Protol_header_str[4][1])
                {
                    m_oonbon_Protocol.Prototol_Header[num].Leng = 16;
                }
                else
                {
                    m_oonbon_Protocol.Prototol_Header[num].Leng = Convert.ToUInt32(Protol_header_str[num][0]);
                }
                m_oonbon_Protocol.Prototol_Header[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_Header[num].Leng];
                m_oonbon_Protocol.Prototol_Header[num].describe = Protol_header_str[num][2];
            }

            /*包头数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_Header.Length; num++)
            {

                if (m_oonbon_Protocol.Prototol_Header[num].para == Protol_header_str[8][1])
                {
                    m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                    m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] = myarray[i++];
                    if (m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] != 0x02)
                    {
                        if (language == 0)
                        {
                            MessageBox.Show("协议版本号不是0x02，不是字库卡协议数据，请重选协议类型");
                        }
                        if (language == 1)
                        {
                            MessageBox.Show("The protocol version number is not 0x02, it is not the protocol data of the library card. Please re select the protocol type");
                        }
                        return 1;
                    }
                    continue;
                }
                if (m_oonbon_Protocol.Prototol_Header[num].para == Protol_header_str[3][1])
                {
                    m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                    m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] = myarray[i++];
                    num++;
                    if (m_oonbon_Protocol.Prototol_Header[num].byteMemValue[0] == 0x01)
                    {
                        m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                        data_str = "";
                        for (num2 = 0; num2 < 16; num2++)
                        {
                            m_oonbon_Protocol.Prototol_Header[num].byteMemValue[16 - num2 - 1] = myarray[i];
                            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                            byte[] byteArray = new byte[] { (byte)myarray[i++] };
                            string strCharacter = asciiEncoding.GetString(byteArray);
                            data_str = data_str + strCharacter;
                        }
                        m_oonbon_Protocol.Prototol_Header[num].describe = data_str;
                    }
                    continue;
                }
                else
                {
                    m_oonbon_Protocol.Prototol_Header[num].bEnable = 1;
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_Header[num].Leng; num1++)
                    {
                        m_oonbon_Protocol.Prototol_Header[num].byteMemValue[num1] = myarray[i++];
                    }
                }

            }

            m_region_par_init();

            switch (myarray[i])
            {
                case 0xa1:
                    switch (myarray[i + 1])
                    {
                        case 0x00://格式化
                            Font_Card_Protocol_A1_00_cmd_string_init();
                            i = Font_Card_A2_00_Protocol(myarray, i);//可以调用ping命令格式
                            break;
                        case 0x01://删除文件
                            Font_Card_Protocol_A1_01_cmd_string_init();
                            i = Font_Card_A1_01_Protocol(myarray, i);
                            break;
                        case 0x02://控制器状态命令
                            Font_Card_Protocol_A1_02_cmd_string_init();
                            i = Font_Card_A1_02_Protocol(myarray, i);
                            break;
                        case 0x05://开始写文件
                            Font_Card_Protocol_A1_05_cmd_string_init();
                            i = Font_Card_A1_02_Protocol(myarray, i);
                            break;
                        case 0x06://写文件
                            Font_Card_Protocol_A1_06_cmd_string_init();
                            i = Font_Card_A1_06_Protocol(myarray, i);
                            break;
                        default:
                            flg = 1;
                            break;
                    }
                    break;
                case 0xa2:
                    switch (myarray[i + 1])
                    {
                        case 0x00://ping命令
                            Font_Card_Protocol_A2_00_cmd_string_init();
                            i = Font_Card_A2_00_Protocol(myarray, i);
                            break;
                        case 0x01://系统复位命令
                            Font_Card_Protocol_A2_01_cmd_string_init();
                            i = Font_Card_A2_00_Protocol(myarray, i);
                            break;
                        case 0x03://校时命令
                            Font_Card_Protocol_A2_03_cmd_string_init();
                            i = Font_Card_A2_03_Protocol(myarray, i);
                            break;
                        default:
                            flg = 1;
                            break;
                    }
                    break;

                case 0xa3:
                    switch (myarray[i + 1])
                    {
                        case 0x00://强制开关机命令
                            Font_Card_Protocol_A3_00_cmd_string_init();
                            i = Font_Card_A2_00_Protocol(myarray, i);
                            break;
                        case 0x01://定制开关机命令
                            Font_Card_Protocol_A3_01_cmd_string_init();
                            i = Font_Card_A3_01_Protocol(myarray, i);
                            break;
                        case 0x04://锁定/解锁节目
                            Font_Card_Protocol_A3_04_cmd_string_init();
                            i = Font_Card_A2_00_Protocol(myarray, i);
                            break;
                        case 0x06://更新动态区命令
                            Font_Card_Protocol_A3_06_cmd_string_init();
                            Font_Card_Protocol_area_data_string_init();
                            i = Font_Card_A3_06_Protocol(myarray, i);
                            break;
                        case 0x08://取消定时开关机命令
                            Font_Card_Protocol_A3_08_cmd_string_init();
                            i = Font_Card_A2_00_Protocol(myarray, i);
                            break;
                        case 0x10://清屏命令
                            Font_Card_Protocol_A3_10_cmd_string_init();
                            i = Font_Card_A2_00_Protocol(myarray, i);
                            break;
                        default:
                            flg = 1;
                            break;
                    }
                    break;
                default:
                    flg = 1;
                    break;
            }
            if (flg == 1)
            {
                if (language == 0)
                {
                    MessageBox.Show("目前版本不支持" + myarray[i].ToString("X2") + " " + myarray[i + 1].ToString("X2"));
                }
                if (language == 1)
                {
                    MessageBox.Show("The current version does not support " + myarray[i].ToString("X2") + " " + myarray[i + 1].ToString("X2"));
                }
                return 1;
            }
            else
            {
                try
                {
                    if (i != 0)
                    {
                        Font_Card_Protocol_CRC_string_init();

                        m_oonbon_Protocol.Prototol_CRC = new CProtolPart();
                        m_oonbon_Protocol.Prototol_CRC.para = Protol_crc_str[0][1];
                        m_oonbon_Protocol.Prototol_CRC.Leng = Convert.ToUInt32(Protol_crc_str[0][0]);
                        m_oonbon_Protocol.Prototol_CRC.byteMemValue = new byte[m_oonbon_Protocol.Prototol_CRC.Leng];
                        m_oonbon_Protocol.Prototol_CRC.describe = Protol_crc_str[0][2];
                        m_oonbon_Protocol.Prototol_CRC.bEnable = 1;

                        data_value = (int)(((myarray[i + 1] & 0xff) << 8) |
                                          ((myarray[i + 0] & 0xff) << 0));
                        int mycrc = crc16(myarray, PHY0_flag1.RCV_data_num - 2);
                        if (mycrc == data_value)
                        {
                            m_oonbon_Protocol.Prototol_CRC.describe = Protol_crc_str[0][2];
                        }
                        else
                        {
                            if (data_value == 0xffff)
                            {
                                m_oonbon_Protocol.Prototol_CRC.describe = "不进行CRC校验";
                            }
                            else
                            {
                                if (language == 0)
                                {
                                    m_oonbon_Protocol.Prototol_CRC.describe = "CRC校验错误";
                                }
                                if (language == 1)
                                {
                                    m_oonbon_Protocol.Prototol_CRC.describe = "CRC error";
                                }
                            }
                        }

                        for (num = 0; num < m_oonbon_Protocol.Prototol_CRC.Leng; num++)
                        {
                            m_oonbon_Protocol.Prototol_CRC.byteMemValue[num] = myarray[i++];
                        }
                    }


                }
                catch (Exception ex)
                {
                }
            }

            return 0;
        }

        public UInt32 Font_Card_A1_01_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            data_value = (int)(((myarray[i + 7] & 0xff) << 8) |//需要删除的文件个数
                               ((myarray[i + 6] & 0xff) << 0));

            m_oonbon_Protocol.Prototol_CMD = new CProtolPart[Prototol_CMD_len + data_value - 1];

            for (num = 0; num < Prototol_CMD_len + data_value - 1; num++)
            {
                m_oonbon_Protocol.Prototol_CMD[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 0;
                if (num > Prototol_CMD_len)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].para = "文件名";
                    m_oonbon_Protocol.Prototol_CMD[num].Leng = 4;
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[4];
                    m_oonbon_Protocol.Prototol_CMD[num].describe = "文件名";
                }
                else
                {
                    m_oonbon_Protocol.Prototol_CMD[num].para = Protol_cmd_str[num][1];
                    m_oonbon_Protocol.Prototol_CMD[num].Leng = Convert.ToUInt32(Protol_cmd_str[num][0]);
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_CMD[num].Leng];
                    m_oonbon_Protocol.Prototol_CMD[num].describe = Protol_cmd_str[num][2];
                }
            }

            /*命令数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_CMD.Length;)
            {
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                }
            }

            return i;

        }

        public UInt32 Font_Card_A2_00_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            m_oonbon_Protocol.Prototol_CMD = new CProtolPart[Prototol_CMD_len];
            for (num = 0; num < Prototol_CMD_len; num++)
            {
                m_oonbon_Protocol.Prototol_CMD[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_CMD[num].para = Protol_cmd_str[num][1];

                m_oonbon_Protocol.Prototol_CMD[num].Leng = Convert.ToUInt32(Protol_cmd_str[num][0]);
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_CMD[num].Leng];

                m_oonbon_Protocol.Prototol_CMD[num].describe = Protol_cmd_str[num][2];
            }

            /*命令数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_CMD.Length;)
            {
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                }
            }

            return i;

        }


        public UInt32 Font_Card_A1_02_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            m_oonbon_Protocol.Prototol_CMD = new CProtolPart[Prototol_CMD_len];
            for (num = 0; num < Prototol_CMD_len - 3; num++)
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
            num = 0;

            /*命令分组*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num++;

            /*命令编号*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num++;

            /*命令处理状态*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num++;

            /*保留字节*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[1] = myarray[i++];
            num++;

            /*控制器开关机状态*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            if (m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] == 1)
            {
                m_oonbon_Protocol.Prototol_CMD[num].describe = "开机";
            }
            else
            {
                if (m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] == 2)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].describe = "关机";
                }
                else
                {
                    m_oonbon_Protocol.Prototol_CMD[num].describe = "参数错误";
                }
            }
            num++;

            /*当前亮度*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num++;

            /*控制器时间*/
            for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
            {
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[m_oonbon_Protocol.Prototol_CMD[num].Leng - num1 - 1] = myarray[i++];
            }

            i -= m_oonbon_Protocol.Prototol_CMD[num].Leng;
            data_str = data_str + myarray[i++].ToString("X2");
            data_str = myarray[i++].ToString("X2") + data_str;
            data_str = data_str + "年";

            data_str = data_str + myarray[i++].ToString("X2");
            data_str = data_str + "月";

            data_str = data_str + myarray[i++].ToString("X2");
            data_str = data_str + "日";

            data_str = data_str + myarray[i++].ToString("X2");
            data_str = data_str + "时";

            data_str = data_str + myarray[i++].ToString("X2");
            data_str = data_str + "分";

            data_str = data_str + myarray[i++].ToString("X2");
            data_str = data_str + "秒";

            switch (myarray[i++])
            {
                case 1:
                    data_str = data_str + "星期一";
                    break;
                case 2:
                    data_str = data_str + "星期二";
                    break;
                case 3:
                    data_str = data_str + "星期三";
                    break;
                case 4:
                    data_str = data_str + "星期四";
                    break;
                case 5:
                    data_str = data_str + "星期五";
                    break;
                case 6:
                    data_str = data_str + "星期六";
                    break;
                case 7:
                    data_str = data_str + "星期日";
                    break;
                default:
                    data_str = data_str + "星期错误";
                    break;
            }
            m_oonbon_Protocol.Prototol_CMD[num].describe = data_str;
            num++;


            /*节目个数*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num++;

            /*当前播放的节目名*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
            {
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[m_oonbon_Protocol.Prototol_CMD[num].Leng - num1 - 1] = myarray[i++];
            }
            num++;

            /*特殊动态区标志*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num++;

            /*特殊动态区页数*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num++;

            /*动态区个数*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];
            num1 = m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0];
            num++;

            /*动态区ID号*/
            if (num1 != 0)
            {
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                m_oonbon_Protocol.Prototol_CMD[num].Leng = num1;
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[num1];
                for (num2 = 0; num2 < num1; num2++)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num2] = myarray[i++];
                }
            }
            num++;

            /*条码*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].Leng = 16;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[16];
            data_str = "";
            for (num2 = 0; num2 < 16; num2++)
            {
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[16 - num2 - 1] = myarray[i];
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)myarray[i++] };
                string strCharacter = asciiEncoding.GetString(byteArray);
                data_str = data_str + strCharacter;
            }
            m_oonbon_Protocol.Prototol_CMD[num].describe = data_str;
            num++;

            /*自定义网络ID*/
            m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
            m_oonbon_Protocol.Prototol_CMD[num].Leng = 12;
            m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[12];
            data_str = "";
            for (num2 = 0; num2 < 12; num2++)
            {
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[12 - num2 - 1] = myarray[i];
                if (myarray[i] != 0)
                {
                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    byte[] byteArray = new byte[] { (byte)myarray[i++] };
                    string strCharacter = asciiEncoding.GetString(byteArray);
                    data_str = data_str + strCharacter;
                }
                else
                {
                    i++;
                }

            }
            if (myarray[i - 1] != 0)
            {
                m_oonbon_Protocol.Prototol_CMD[num].describe = data_str;
            }
            num++;


            return i;

        }

        public UInt32 Font_Card_A1_06_pro_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2;
            string data_str;
            int File_CRC_flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            data_str = "";

            m_oonbon_Protocol.Prototol_File = new CProtolPart[Prototol_File_len];
            for (num = 0; num < Prototol_File_len; num++)
            {
                m_oonbon_Protocol.Prototol_File[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_File[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_File[num].para = Protol_File_str[num][1];
                m_oonbon_Protocol.Prototol_File[num].Leng = Convert.ToUInt32(Protol_File_str[num][0]);
                m_oonbon_Protocol.Prototol_File[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_File[num].Leng];
                m_oonbon_Protocol.Prototol_File[num].describe = Protol_File_str[num][2];
            }

            /*命令数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_File.Length;)
            {
                if (m_oonbon_Protocol.Prototol_File[num].para == "节目播放时段组数")
                {
                    /*节目播放时段组数*/
                    num2 = 0;
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_File[num].Leng; num1++)
                    {
                        m_oonbon_Protocol.Prototol_File[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_File[num].byteMemValue[num1] = myarray[i++];
                        num2 = num2 + m_oonbon_Protocol.Prototol_File[num].byteMemValue[num1];
                    }
                    num++;

                    /*播放组*/
                    if (num2 != 0)
                    {
                        if (num2 == 1)
                        {
                            for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_File[num].Leng; num1++)
                            {
                                m_oonbon_Protocol.Prototol_File[num].bEnable = 1;
                                m_oonbon_Protocol.Prototol_File[num].byteMemValue[num1] = myarray[i++];
                            }
                            i -= m_oonbon_Protocol.Prototol_File[num].Leng;

                            data_str = data_str + "开始：";

                            data_str = data_str + myarray[i++].ToString("X2");
                            data_str = myarray[i++].ToString("X2") + data_str;
                            data_str = data_str + "年";

                            data_str = data_str + myarray[i++].ToString("X2");
                            data_str = data_str + "月";

                            data_str = data_str + myarray[i++].ToString("X2");
                            data_str = data_str + "日";


                            data_str = data_str + "   结束：";

                            data_str = data_str + myarray[i++].ToString("X2");
                            data_str = myarray[i++].ToString("X2") + data_str;
                            data_str = data_str + "年";

                            data_str = data_str + myarray[i++].ToString("X2");
                            data_str = data_str + "月";

                            data_str = data_str + myarray[i++].ToString("X2");
                            data_str = data_str + "日";

                            m_oonbon_Protocol.Prototol_File[num].describe = data_str;

                        }
                        else
                        {
                            MessageBox.Show("节目播放时段组数只能为0或1");
                            return 0;
                        }

                    }
                    num++;
                    continue;
                }
                else
                {
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_File[num].Leng; num1++)
                    {
                        m_oonbon_Protocol.Prototol_File[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_File[num].byteMemValue[num1] = myarray[i++];
                    }
                    if (m_oonbon_Protocol.Prototol_File[num].para == "区域个数")
                    {
                        m_oonbon_Protocol.Area_Num = m_oonbon_Protocol.Prototol_File[num].byteMemValue[0];
                    }
                    if (m_oonbon_Protocol.Prototol_File[num].para == "文件名")
                    {
                        System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                        byte[] byteArray0 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[0] };
                        byte[] byteArray1 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[1] };
                        byte[] byteArray2 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[2] };
                        byte[] byteArray3 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[3] };
                        m_oonbon_Protocol.Prototol_File[num].describe = "文件名:" +
                            asciiEncoding.GetString(byteArray0) +
                            asciiEncoding.GetString(byteArray1) +
                            asciiEncoding.GetString(byteArray2) +
                            asciiEncoding.GetString(byteArray3);
                    }
                    num++;
                }
            }
            i = Font_Card_Area_Data_Protocol(myarray, i);
            if (File_CRC_flg == 1)
            {
                /*文件校验*/
                m_oonbon_Protocol.Prototol_File_CRC = new CProtolPart();
                m_oonbon_Protocol.Prototol_File_CRC.para = Protol_cmd_filecrc_str[0][1];
                m_oonbon_Protocol.Prototol_File_CRC.Leng = Convert.ToUInt32(Protol_cmd_filecrc_str[0][0]);
                m_oonbon_Protocol.Prototol_File_CRC.byteMemValue = new byte[m_oonbon_Protocol.Prototol_File_CRC.Leng];
                m_oonbon_Protocol.Prototol_File_CRC.describe = Protol_cmd_filecrc_str[0][2];
                m_oonbon_Protocol.Prototol_File_CRC.bEnable = 1;


                if (language == 0)
                {
                    m_oonbon_Protocol.Prototol_File_CRC.describe = "工具不检查文件的CRC值";
                }
                if (language == 1)
                {
                    m_oonbon_Protocol.Prototol_File_CRC.describe = "This tool does not check the CRC value of the file";
                }


                for (num = 0; num < m_oonbon_Protocol.Prototol_File_CRC.Leng; num++)
                {
                    m_oonbon_Protocol.Prototol_File_CRC.byteMemValue[num] = myarray[i++];
                }
            }


            return i;


        }
        public UInt32 Font_Card_A1_06_pra_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2;
            string data_str;
            int File_CRC_flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            data_str = "";

            m_oonbon_Protocol.Prototol_File = new CProtolPart[Prototol_File_len];
            for (num = 0; num < Prototol_File_len; num++)
            {
                m_oonbon_Protocol.Prototol_File[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_File[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_File[num].para = Protol_File_str[num][1];
                m_oonbon_Protocol.Prototol_File[num].Leng = Convert.ToUInt32(Protol_File_str[num][0]);
                m_oonbon_Protocol.Prototol_File[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_File[num].Leng];
                m_oonbon_Protocol.Prototol_File[num].describe = Protol_File_str[num][2];
            }

            /*文件数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_File.Length;)
            {
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_File[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_File[num].bEnable = 1;
                    m_oonbon_Protocol.Prototol_File[num].byteMemValue[num1] = myarray[i++];
                }
                if (m_oonbon_Protocol.Prototol_File[num].para == "文件名")
                {
                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    byte[] byteArray0 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[0] };
                    byte[] byteArray1 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[1] };
                    byte[] byteArray2 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[2] };
                    byte[] byteArray3 = new byte[] { (byte)m_oonbon_Protocol.Prototol_File[num].byteMemValue[3] };
                    m_oonbon_Protocol.Prototol_File[num].describe = "文件名:" +
                        asciiEncoding.GetString(byteArray0) +
                        asciiEncoding.GetString(byteArray1) +
                        asciiEncoding.GetString(byteArray2) +
                        asciiEncoding.GetString(byteArray3);
                }
                if (m_oonbon_Protocol.Prototol_File[num].para == "屏幕宽度")
                {
                    num2 = (UInt32)(((m_oonbon_Protocol.Prototol_File[num].byteMemValue[1] & 0xff) << 8) | ((m_oonbon_Protocol.Prototol_File[num].byteMemValue[0] & 0xff) << 0));
                    m_oonbon_Protocol.Prototol_File[num].describe = "屏幕宽度:" + num2.ToString();
                }
                if (m_oonbon_Protocol.Prototol_File[num].para == "屏幕高度")
                {
                    num2 = (UInt32)(((m_oonbon_Protocol.Prototol_File[num].byteMemValue[1] & 0xff) << 8) | ((m_oonbon_Protocol.Prototol_File[num].byteMemValue[0] & 0xff) << 0));
                    m_oonbon_Protocol.Prototol_File[num].describe = "屏幕高度:" + num2.ToString();
                }
                num++;
            }
            if (File_CRC_flg == 1)
            {
                /*文件校验*/
                m_oonbon_Protocol.Prototol_File_CRC = new CProtolPart();
                m_oonbon_Protocol.Prototol_File_CRC.para = Protol_cmd_filecrc_str[0][1];
                m_oonbon_Protocol.Prototol_File_CRC.Leng = Convert.ToUInt32(Protol_cmd_filecrc_str[0][0]);
                m_oonbon_Protocol.Prototol_File_CRC.byteMemValue = new byte[m_oonbon_Protocol.Prototol_File_CRC.Leng];
                m_oonbon_Protocol.Prototol_File_CRC.describe = Protol_cmd_filecrc_str[0][2];
                m_oonbon_Protocol.Prototol_File_CRC.bEnable = 1;


                if (language == 0)
                {
                    m_oonbon_Protocol.Prototol_File_CRC.describe = "工具不检查文件的CRC值";
                }
                if (language == 1)
                {
                    m_oonbon_Protocol.Prototol_File_CRC.describe = "This tool does not check the CRC value of the file";
                }


                for (num = 0; num < m_oonbon_Protocol.Prototol_File_CRC.Leng; num++)
                {
                    m_oonbon_Protocol.Prototol_File_CRC.byteMemValue[num] = myarray[i++];
                }
            }


            return i;


        }
        public UInt32 Font_Card_A1_06_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1;

            int File_CRC_flg = 0;
            string File_type = "P";


            num = 0;
            num1 = 0;

            m_oonbon_Protocol.Prototol_CMD = new CProtolPart[Prototol_CMD_len];
            for (num = 0; num < Prototol_CMD_len; num++)
            {
                m_oonbon_Protocol.Prototol_CMD[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_CMD[num].para = Protol_cmd_str[num][1];
                m_oonbon_Protocol.Prototol_CMD[num].Leng = Convert.ToUInt32(Protol_cmd_str[num][0]);
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_CMD[num].Leng];
                m_oonbon_Protocol.Prototol_CMD[num].describe = Protol_cmd_str[num][2];
            }
            try
            {
                /*命令数据*/
                for (num = 0; num < m_oonbon_Protocol.Prototol_CMD.Length;)
                {
                    if (m_oonbon_Protocol.Prototol_CMD[num].para == "是否是最后一包")
                    {

                        m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] = myarray[i++];

                        /*只有最后一包数据才会有文件校验*/
                        if (m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] == 1)
                        {
                            File_CRC_flg = 1;
                        }
                        else
                        {
                            File_CRC_flg = 0;
                        }
                        num++;
                        continue;
                    }
                    for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                    {
                        m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                        if (m_oonbon_Protocol.Prototol_CMD[num].para == "文件名")
                        {
                            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                            byte[] byteArray0 = new byte[] { (byte)m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0] };
                            byte[] byteArray1 = new byte[] { (byte)m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[1] };
                            byte[] byteArray2 = new byte[] { (byte)m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[2] };
                            byte[] byteArray3 = new byte[] { (byte)m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[3] };
                            m_oonbon_Protocol.Prototol_CMD[num].describe = "文件名:" +
                                asciiEncoding.GetString(byteArray0) +
                                asciiEncoding.GetString(byteArray1) +
                                asciiEncoding.GetString(byteArray2) +
                                asciiEncoding.GetString(byteArray3);
                            File_type = asciiEncoding.GetString(byteArray0);
                        }
                    }
                    num++;
                }
                switch (File_type)
                {
                    case "P":
                        Font_Card_Protocol_area_data_string_init();
                        Font_Card_Protocol_A1_06_cmd_filecrc_string_init();
                        Font_Card_Protocol_A1_06_pro_string_init();
                        Protol_area_data_str[0][0] = "4";//特殊处理，发送节目时区域数据长度是4个字节，动态区是2个字节
                        i = Font_Card_A1_06_pro_Protocol(myarray, i);//解析数据
                        break;
                    case "C":
                        Font_Card_Protocol_A1_06_cmd_filecrc_string_init();
                        Font_Card_Protocol_A1_06_pra_string_init();
                        i = Font_Card_A1_06_pra_Protocol(myarray, i);//解析数据
                        break;
                    default:
                        if (language == 0)
                        {
                            MessageBox.Show("目前版本不支持解析" + File_type + "文件");
                        }
                        if (language == 1)
                        {
                            MessageBox.Show("The current version do not supports " + File_type + "file");
                        }
                        return 0;
                        break;
                }

                if (File_CRC_flg == 1)
                {
                    /*文件校验*/
                    m_oonbon_Protocol.Prototol_File_CRC = new CProtolPart();
                    m_oonbon_Protocol.Prototol_File_CRC.para = Protol_cmd_filecrc_str[0][1];
                    m_oonbon_Protocol.Prototol_File_CRC.Leng = Convert.ToUInt32(Protol_cmd_filecrc_str[0][0]);
                    m_oonbon_Protocol.Prototol_File_CRC.byteMemValue = new byte[m_oonbon_Protocol.Prototol_File_CRC.Leng];
                    m_oonbon_Protocol.Prototol_File_CRC.describe = Protol_cmd_filecrc_str[0][2];
                    m_oonbon_Protocol.Prototol_File_CRC.bEnable = 1;


                    if (language == 0)
                    {
                        m_oonbon_Protocol.Prototol_File_CRC.describe = "工具不检查文件的CRC值";
                    }
                    if (language == 1)
                    {
                        m_oonbon_Protocol.Prototol_File_CRC.describe = "This tool does not check the CRC value of the file";
                    }


                    for (num = 0; num < m_oonbon_Protocol.Prototol_File_CRC.Leng; num++)
                    {
                        m_oonbon_Protocol.Prototol_File_CRC.byteMemValue[num] = myarray[i++];
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return i;
        }
        public UInt32 Font_Card_A2_03_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            m_oonbon_Protocol.Prototol_CMD = new CProtolPart[Prototol_CMD_len];
            for (num = 0; num < Prototol_CMD_len; num++)
            {
                m_oonbon_Protocol.Prototol_CMD[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 0;
                m_oonbon_Protocol.Prototol_CMD[num].para = Protol_cmd_str[num][1];

                m_oonbon_Protocol.Prototol_CMD[num].Leng = Convert.ToUInt32(Protol_cmd_str[num][0]);
                m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_CMD[num].Leng];

                m_oonbon_Protocol.Prototol_CMD[num].describe = Protol_cmd_str[num][2];
            }

            /*命令数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_CMD.Length;)
            {

                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                    if (m_oonbon_Protocol.Prototol_CMD[num].para == "控制器时间")
                    {
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[m_oonbon_Protocol.Prototol_CMD[num].Leng - num1 - 1] = myarray[i++];
                    }
                    else
                    {
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                    }

                }
                if (m_oonbon_Protocol.Prototol_CMD[num].para == "控制器时间")
                {
                    i -= m_oonbon_Protocol.Prototol_CMD[num].Leng;
                    data_str = data_str + myarray[i++].ToString("X2");
                    data_str = myarray[i++].ToString("X2") + data_str;
                    data_str = data_str + "年";

                    data_str = data_str + myarray[i++].ToString("X2");
                    data_str = data_str + "月";

                    data_str = data_str + myarray[i++].ToString("X2");
                    data_str = data_str + "日";

                    data_str = data_str + myarray[i++].ToString("X2");
                    data_str = data_str + "时";

                    data_str = data_str + myarray[i++].ToString("X2");
                    data_str = data_str + "分";

                    data_str = data_str + myarray[i++].ToString("X2");
                    data_str = data_str + "秒";

                    switch (myarray[i++])
                    {
                        case 1:
                            data_str = data_str + "星期一";
                            break;
                        case 2:
                            data_str = data_str + "星期二";
                            break;
                        case 3:
                            data_str = data_str + "星期三";
                            break;
                        case 4:
                            data_str = data_str + "星期四";
                            break;
                        case 5:
                            data_str = data_str + "星期五";
                            break;
                        case 6:
                            data_str = data_str + "星期六";
                            break;
                        case 7:
                            data_str = data_str + "星期日";
                            break;
                        default:
                            data_str = data_str + "星期错误";
                            break;
                    }
                    m_oonbon_Protocol.Prototol_CMD[num].describe = data_str;
                }
                num++;

            }

            return i;

        }

        public UInt32 Font_Card_A3_01_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

            data_value = (int)(myarray[i + 5]);// 定时开关机组数


            m_oonbon_Protocol.Prototol_CMD = new CProtolPart[Prototol_CMD_len + (data_value * 2) - 2];

            for (num = 0; num < Prototol_CMD_len + (data_value * 2) - 2; num++)
            {
                m_oonbon_Protocol.Prototol_CMD[num] = new CProtolPart();
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 0;
                if (num > Prototol_CMD_len)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].para = "开机时间";
                    m_oonbon_Protocol.Prototol_CMD[num].Leng = 2;
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[2];
                    m_oonbon_Protocol.Prototol_CMD[num].describe = "开机时间";
                    num++;

                    m_oonbon_Protocol.Prototol_CMD[num].para = "关机时间";
                    m_oonbon_Protocol.Prototol_CMD[num].Leng = 2;
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[2];
                    m_oonbon_Protocol.Prototol_CMD[num].describe = "关机时间";
                }
                else
                {
                    m_oonbon_Protocol.Prototol_CMD[num].para = Protol_cmd_str[num][1];
                    m_oonbon_Protocol.Prototol_CMD[num].Leng = Convert.ToUInt32(Protol_cmd_str[num][0]);
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue = new byte[m_oonbon_Protocol.Prototol_CMD[num].Leng];
                    m_oonbon_Protocol.Prototol_CMD[num].describe = Protol_cmd_str[num][2];
                }
            }

            /*命令数据*/
            for (num = 0; num < m_oonbon_Protocol.Prototol_CMD.Length;)
            {
                m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                for (num1 = 0; num1 < m_oonbon_Protocol.Prototol_CMD[num].Leng; num1++)
                {
                    m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                }
            }

            return i;

        }
        public UInt32 Font_Card_Area_Data_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;

            Cregion_parameter region_par = new Cregion_parameter();

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;

            data_str = "";
            try
            {
                m_oonbon_Protocol.Prototol_area_data = new CPrototolAreaPart[m_oonbon_Protocol.Area_Num];
                for (num = 0; num < m_oonbon_Protocol.Area_Num; num++)
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

                        if ((ProtolPart.para == Protol_area_data_str[3][1]) || (ProtolPart.para == Protol_area_data_str[5][1]))
                        {
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.bEnable = 1;
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.byteMemValue[1] = myarray[i++];
                            num3 = (UInt32)(((ProtolPart.byteMemValue[1] & 0xff) << 8) | ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                            if (language == 0)
                            {
                                ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString() + "(像素为单位)";
                            }
                            if (language == 1)
                            {
                                ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString() + "(unit: pixel)";
                            }

                            num1++;

                            if (ProtolPart.para == Protol_area_data_str[3][1])
                            {
                                region_par.Y = num3;
                            }
                            if (ProtolPart.para == Protol_area_data_str[5][1])
                            {
                                region_par.height = num3;
                            }
                            continue;
                        }
                        if ((ProtolPart.para == Protol_area_data_str[2][1]) || (ProtolPart.para == Protol_area_data_str[4][1]))
                        {
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.bEnable = 1;
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.byteMemValue[1] = myarray[i++];
                            if ((ProtolPart.byteMemValue[1] >> 7) == 1)
                            {
                                num3 = (UInt32)((((ProtolPart.byteMemValue[1] & (~0x80)) & 0xff) << 8) | ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                                if (language == 0)
                                {
                                    ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString() + "(像素为单位)";
                                }
                                if (language == 1)
                                {
                                    ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString() + "(unit: pixel)";
                                }

                                if (ProtolPart.para == Protol_area_data_str[2][1])
                                {
                                    region_par.X = num3;
                                }
                                if (ProtolPart.para == Protol_area_data_str[4][1])
                                {
                                    region_par.width = num3;
                                }
                            }
                            else
                            {
                                num3 = (UInt32)(((ProtolPart.byteMemValue[1] & 0xff) << 8) | ((ProtolPart.byteMemValue[0] & 0xff) << 0));
                                if (language == 0)
                                {
                                    ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString() + "(字节为单位)";
                                }
                                if (language == 1)
                                {
                                    ProtolPart.describe = ProtolPart.describe + "：" + num3.ToString() + "(unit: byte(8 pixels))";
                                }


                                if (ProtolPart.para == Protol_area_data_str[2][1])
                                {
                                    region_par.X = num3 * 8;
                                }
                                if (ProtolPart.para == Protol_area_data_str[4][1])
                                {
                                    region_par.width = num3 * 8;
                                }

                            }

                            num1++;
                            continue;
                        }

                        if (ProtolPart.para == Protol_area_data_str[6][1])
                        {
                            ProtolPart = m_oonbon_Protocol.Prototol_area_data[num].Prototol_Area_Part[num1];
                            ProtolPart.bEnable = 1;
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            region_par.ID = (UInt32)ProtolPart.byteMemValue[0];
                            num1++;
                            continue;
                        }

                        if (ProtolPart.para == Protol_area_data_str[26][1])
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

                        if (ProtolPart.para == Protol_area_data_str[16][1])
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
                                    if (language == 0)
                                    {
                                        MessageBox.Show("目前版本不支持扩展位大于1的情况");
                                    }
                                    if (language == 1)
                                    {
                                        MessageBox.Show("The current version does not support the case where the extension is greater than 1");
                                    }
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
                        if (ProtolPart.para == Protol_area_data_str[10][1])
                        {
                            /*是否使能语音*/
                            ProtolPart.byteMemValue[0] = myarray[i++];
                            ProtolPart.bEnable = 1;
                            num1++;
                            if (ProtolPart.byteMemValue[0] == 0)
                            {
                                num1 += 5;
                                continue;
                            }
                            if ((ProtolPart.byteMemValue[0] != 1) && (ProtolPart.byteMemValue[0] != 2))
                            {
                                if (language == 0)
                                {
                                    MessageBox.Show("是否使能语音字节只能为0、1、2");
                                }
                                if (language == 1)
                                {
                                    MessageBox.Show("Enable voice byte can only be 0, 1, 2");
                                }
                                return 0;
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
                                num1++;
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

                    for (num3 = 0; num3 < m_region_par.Length; num3++)
                    {
                        if (m_region_par[num3].bEnable != 1)
                        {
                            m_region_par[num3].bEnable = 1;

                            m_region_par[num3].ID = region_par.ID;
                            m_region_par[num3].X = region_par.X;
                            m_region_par[num3].Y = region_par.Y;
                            m_region_par[num3].width = region_par.width;
                            m_region_par[num3].height = region_par.height;

                            break;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
            }

            return i;
        }

        public UInt32 Font_Card_A3_06_Protocol(byte[] myarray, UInt32 i)
        {
            UInt32 num, num1, num2, num3;
            string data_str;
            int flg = 0;

            num = 0;
            num1 = 0;
            num2 = 0;
            num3 = 0;
            data_str = "";

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
                if (m_oonbon_Protocol.Prototol_CMD[num].para == Protol_cmd_str[4][1])
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
                    if ((num2 != 0) && (num2 != 0xff))
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
                        m_oonbon_Protocol.Prototol_CMD[num].bEnable = 1;
                        m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[num1] = myarray[i++];
                    }
                    if (m_oonbon_Protocol.Prototol_CMD[num].para == Protol_cmd_str[6][1])
                    {
                        m_oonbon_Protocol.Area_Num = m_oonbon_Protocol.Prototol_CMD[num].byteMemValue[0];
                    }
                    num++;
                }
            }

            i = Font_Card_Area_Data_Protocol(myarray, i);

            return i;
        }

        #endregion Font_Card_Protocol_Analysis





        public int Data_deal_with(byte[] data, int size)
        {
            UInt32 flg_5A, flg_A5;
            PHY0_flag1.Rcv_state = 0;
            PHY0_flag1.RCV_data_num = 0;
            int size_temp;
            byte[] ping_data = { 0xfe, 0xff, 0x00, 0x80, 0x00, 0x00, 0xfe, 0x00, 0x01, 0x00, 0x04, 0x00, 0x23, 0xb8, 0xe1, 0xa2, 0x00, 0x00, 0x00 };

            size_temp = size;

            for (UInt32 num = 0; num < size; num++)//如果末尾有多个0X5A，只留1个0X5A
            {
                if (data[size - num - 1] == 0x5A)
                {
                    if (data[size - num - 2] == 0x5A)
                    {
                        size_temp--;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            size = size_temp;

            myarray = new byte[size];
            trsf_flg = new byte[size];

            UInt32 i;
            byte data_t;
            UInt32 j = 0;
            flg_5A = 0;
            flg_A5 = 0;
            if (data == null)
            {
                return 0;
            }
            for (j = 0; j < size; j++)
            {
                data_t = data[j];
                if (data_t == 0XA5)
                {
                    flg_A5 = 1;
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
                                flg_5A = 1;
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
            if ((flg_5A == 1) && (flg_A5 == 0))
            {
                return 1;//收到帧尾，没有收到帧头
            }
            if ((flg_5A == 0) && (flg_A5 == 1))
            {
                return 2;//收到帧头，没有收到帧尾
            }
            if ((flg_5A == 0) && (flg_A5 == 0))
            {
                return 3;//帧头帧尾都没有收到
            }
            for (j = 0; j < ping_data.Length; j++)
            {
                if (myarray[j] != ping_data[j])
                    break;
            }
            if (j == ping_data.Length)
                return 4;//这一包数据是ping命令
            if ((flg_5A == 1) && (flg_A5 == 1))
            {
                return 0;//收到帧头帧尾
            }
            return 0;

        }

    }

}
