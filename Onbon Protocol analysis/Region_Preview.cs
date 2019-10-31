using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Onbon_Protocol_analysis
{
    public partial class Region_Preview : Form
    {
        Protocol_Analysis oProtocol_Analysis = new Protocol_Analysis();

        public Region_Preview()
        {
            InitializeComponent();
            
        }
        public void Region_Preview_star()
        {
            UInt32 num = 0;
            UInt32 Form_Width = 0;
            UInt32 Form_Height = 0;
            Protocol_Analysis.Cregion_parameter m_region_par = new Protocol_Analysis.Cregion_parameter();
            Protocol_Analysis.Cregion_parameter m_origin = new Protocol_Analysis.Cregion_parameter();

            m_origin.X = 40;
            m_origin.Y = 40;

            this.Width = 300;
            this.Height = 150;

            for (num = 0; num < oProtocol_Analysis.m_region_par.Length; num++)
            {
                m_region_par = Onbon_Protocol_Form.oProtocol_Analysis.m_region_par[num];
                if (m_region_par.bEnable == 1)
                {
                    if (Form_Width < m_region_par.X + m_region_par.width)
                        Form_Width = m_region_par.X + m_region_par.width;
                    if (Form_Height < m_region_par.Y + m_region_par.height)
                        Form_Height = m_region_par.Y +m_region_par.height;
                }
            }
            if ((Form_Width + m_origin.X + 20) > this.Width)
                this.Width = (int)(Form_Width + m_origin.X + 20);
            if ((Form_Height + m_origin.Y + 60) > this.Height)
                this.Height = (int)(Form_Height + m_origin.Y + 60);


            Pen p = new Pen(Color.Black, 1);
            Graphics grp = CreateGraphics();
            Font myFont = new Font("宋体", 12, FontStyle.Bold);

            Brush bush;
            bush = new SolidBrush(Color.Blue);

            grp.DrawString("区域位置预览", myFont, bush, this.Width/2-60, 0);

            for (num = 0; num < oProtocol_Analysis.m_region_par.Length; num++)
            {
                m_region_par = Onbon_Protocol_Form.oProtocol_Analysis.m_region_par[num];
                if (m_region_par.bEnable == 1)
                {
                    grp.DrawRectangle(p, m_region_par.X + m_origin.X, m_region_par.Y + m_origin.Y, m_region_par.width, m_region_par.height);
                    grp.DrawString(m_region_par.ID.ToString(), myFont, bush, m_region_par.width/2+ m_region_par.X +m_origin.X-8, m_region_par.height/2 + m_region_par.Y + m_origin.Y-8);
                }
            }
        }
    }
}
