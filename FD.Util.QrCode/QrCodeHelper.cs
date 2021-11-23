using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.QrCode
{
    public class QrCodeHelper
    {
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="data">二维码内容</param>
        /// <returns></returns>
        public Bitmap GenerateQRCode(string data)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }


        /// <summary>
        /// 生成文字图片
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Image ConvertStringToImage(string text)
        {
            Bitmap image = new Bitmap(200, 20, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(image);

            try
            {
                Font font = new Font("SimHei", 14, FontStyle.Regular);

                g.Clear(Color.White);

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;                

                Rectangle rectangle = new Rectangle(0, 0, 200, 20);
                //
                g.DrawString(text, font, new SolidBrush(Color.FromArgb(255, 152, 24)), rectangle, format);

                return image;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 生成带文字二维码图片
        /// </summary>
        /// <param name="data">二维码中要传递的数据</param>
        /// <param name="title">二维码上显示的文字说明</param>
        public byte[] GenerateQrCode(string data, string title)
        {
            using (Image codeImage = GenerateQRCode(data), strImage = ConvertStringToImage(title))
            {
                Image img = CombineImage(200, 200, codeImage, 0, 0, strImage, 0, 180);
                using (var stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Jpeg);
                    //输出图片流
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// 生成二维码图片流
        /// </summary>
        /// <param name="str1">二维码中要传递的数据</param>
        /// <param name="str2">二维码上显示的文字说明</param>
        public Image GenerateQrCodeImg(string data, string text)
        {
            Image img;
            using (Image codeImage = GenerateQRCode(data), strImage = ConvertStringToImage(text))
            {
                img = CombineImage(200, 200, codeImage, 0, 0, strImage, 0, 180);
            }
            return img;
        }

        /// <summary>
        /// 在画板中合并二维码图片和文字图片
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imgLeft"></param>
        /// <param name="imgLeft_left"></param>
        /// <param name="imgLeft_top"></param>
        /// <param name="imgRight"></param>
        /// <param name="imgRight_left"></param>
        /// <param name="imgRight_top"></param>
        /// <returns></returns>
        private Image CombineImage(int width, int height, Image imgLeft, int imgLeft_left, int imgLeft_top, Image imgRight, int imgRight_left, int imgRight_top)
        {
            Bitmap image = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            Graphics g = Graphics.FromImage(image);

            try
            {
                g.Clear(Color.White);
                g.DrawImage(imgLeft, imgLeft_left, imgLeft_top, 200, 200);
                g.DrawImage(imgRight, imgRight_left, imgRight_top, imgRight.Width, imgRight.Height);

                return image;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                g.Dispose();
            }
        }
 
    }
}
