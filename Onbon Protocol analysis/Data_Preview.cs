using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Onbon_Protocol_analysis
{
    public partial class Data_Preview : Form
    {
        Protocol_Analysis oProtocol_Analysis = new Protocol_Analysis();
        public Data_Preview()
        {
            InitializeComponent();
        }

        public void Data_Preview_star(byte[] myarray,int ID,int LED_type)
        {
            UInt32 data_r,data_g,data_b;
            int point_r, point_g, point_b;

            UInt32 num = 0;
            UInt32 left_bit = 0;
            UInt32 right_bit = 0;

            data_r = 0;
            data_g = 0;
            data_b = 0;

            Protocol_Analysis.Cregion_parameter m_region_par = new Protocol_Analysis.Cregion_parameter();
            Protocol_Analysis.Cregion_parameter m_region = new Protocol_Analysis.Cregion_parameter();

            for (num = 0; num < oProtocol_Analysis.m_region_par.Length; num++)
            {
                m_region_par = Onbon_Protocol_Form.oProtocol_Analysis.m_region_par[num];
                if ((m_region_par.bEnable == 1)&&(m_region_par.ID == ID))
                {
                    m_region.X = m_region_par.X;
                    m_region.Y = m_region_par.Y;
                    m_region.width = m_region_par.width;
                    m_region.height = m_region_par.height;
                    break;
                }
            }
            if ((m_region.X % 32) != 0)
            {
                left_bit = (m_region.X % 32);
            }
            if (((m_region.X + m_region.width) % 32) != 0)
            {
                right_bit = (32- ((m_region.X + m_region.width) % 32));
            }
            m_region.width = m_region.width + left_bit + right_bit;


            Bitmap bmp = new Bitmap((int)(m_region.width), (int)m_region.height);
            for (int Y = 0; Y < m_region.height; Y++)
            {
                for (int X = 0; X < m_region.width / 8;)
                {
                    data_r = (((UInt32)myarray[m_region.width/8 * Y + X + 0]) << 0) |
                           (((UInt32)myarray[m_region.width/8 * Y + X + 1]) << 8) |
                           (((UInt32)myarray[m_region.width/8 * Y + X + 2]) << 16) |
                           (((UInt32)myarray[m_region.width/8 * Y + X + 3]) << 24);
                    if ((LED_type == 1) || (LED_type == 2))
                    {
                        data_g = (((UInt32)myarray[m_region.width/8* m_region.height + m_region.width / 8 * Y + X + 0]) << 0) |
                           (((UInt32)myarray[m_region.width / 8 * m_region.height + m_region.width / 8 * Y + X + 1]) << 8) |
                           (((UInt32)myarray[m_region.width / 8 * m_region.height + m_region.width / 8 * Y + X + 2]) << 16) |
                           (((UInt32)myarray[m_region.width / 8 * m_region.height + m_region.width / 8 * Y + X + 3]) << 24);
                    }
                    if (LED_type == 2)
                    {
                        data_b = (((UInt32)myarray[m_region.width / 8 * m_region.height*2 + m_region.width / 8 * Y + X + 0]) << 0) |
                           (((UInt32)myarray[m_region.width / 8 * m_region.height*2 + m_region.width / 8 * Y + X + 1]) << 8) |
                           (((UInt32)myarray[m_region.width / 8 * m_region.height*2 + m_region.width / 8 * Y + X + 2]) << 16) |
                           (((UInt32)myarray[m_region.width / 8 * m_region.height*2 + m_region.width / 8 * Y + X + 3]) << 24);
                    }
                    for (int bit = 0; bit < 32; bit++)
                    {
                        point_r = 0;
                        point_g = 0;
                        point_b = 0;
                        if (((data_r >> (32 - bit)) & 0x01) == 0)
                        {
                            point_r = 255;
                        }
                        if ((LED_type == 1) || (LED_type == 2))
                        {
                            if (((data_g >> (32 - bit)) & 0x01) == 0)
                            {
                                point_g = 255;
                            }
                        }
                        if (LED_type == 2)
                        {
                            if (((data_b >> (32 - bit)) & 0x01) == 0)
                            {
                                point_b = 255;
                            }
                        }
                        bmp.SetPixel(X * 8 + bit, Y, Color.FromArgb(255, point_r, point_g, point_b));
                    }
                    X += 4;
                }
            }

            /*裁剪图片*/
            Bitmap target = new Bitmap((int)(m_region.width - left_bit - right_bit), (int)m_region.height);
            target = Cut(bmp, (int)left_bit, 0, (int)(m_region.width - left_bit - right_bit), (int)m_region.height);

            data_pictureBox.Image = target;

            //bmp.Save("D:\\test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

        }

        /// <summary>
        /// 剪裁 -- 用GDI+
        /// </summary>
        /// <param name="b">原始Bitmap</param>
        /// <param name="StartX">开始坐标X</param>
        /// <param name="StartY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Bitmap</returns>
        public static Bitmap Cut(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveImageDialog = new SaveFileDialog();
            saveImageDialog.Title = "图片保存";
            saveImageDialog.Filter = @"jpeg|*.jpg|bmp|*.bmp";
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveImageDialog.FileName.ToString();
                if (fileName != "" && fileName != null)
                {
                    string fileExtName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToString();
                    System.Drawing.Imaging.ImageFormat imgformat = null;


                    if (fileExtName != "")
                    {
                        switch (fileExtName)
                        {
                            case "jpg":
                                imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case "bmp":
                                imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            default:
                                imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                        }


                        try
                        {
                            Bitmap bit = new Bitmap(data_pictureBox.Image);
                            MessageBox.Show(fileName);
                            data_pictureBox.Image.Save(fileName, imgformat);
                        }
                        catch
                        {


                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// BitmapHelper
    /// </summary>
    public static class BitmapScaleHelper
    {
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="bitmap">原图片</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
        {
            if (bitmap.Width == width && bitmap.Height == height)
            {
                return bitmap;
            }

            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, width, height);
            }

            return scaledBitmap;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="bitmap">原图片</param>
        /// <param name="size">新图片大小</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSize(size.Width, size.Height);
        }

        /// <summary>
        /// 按比例来缩放
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="ratio">ratio大于1,则放大;否则缩小</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, float ratio)
        {
            return bitmap.ScaleToSize((int)(bitmap.Width * ratio), (int)(bitmap.Height * ratio));
        }

        /// <summary>
        /// 按给定长度/宽度等比例缩放
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleProportional(this Bitmap bitmap, int width, int height)
        {
            float proportionalWidth, proportionalHeight;

            if (width.Equals(0))
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }
            else if (height.Equals(0))
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else if (((float)width) / bitmap.Size.Width * bitmap.Size.Height <= height)
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }

            return bitmap.ScaleToSize((int)proportionalWidth, (int)proportionalHeight);
        }

        /// <summary>
        /// 按给定长度/宽度缩放,同时可以设置背景色
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="backgroundColor">背景色</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns></returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Color backgroundColor, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(backgroundColor);

                var proportionalBitmap = bitmap.ScaleProportional(width, height);

                var imagePosition = new Point((int)((width - proportionalBitmap.Width) / 2m), (int)((height - proportionalBitmap.Height) / 2m));
                g.DrawImage(proportionalBitmap, imagePosition);
            }

            return scaledBitmap;
        }
    }
}
