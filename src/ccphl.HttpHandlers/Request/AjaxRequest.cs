using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Model;
using System.IO;
using ccphl.Common;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ccphl.HttpHandlers
{
    /// <summary>
    /// Ajax处理类
    /// </summary>
    public class AjaxRequest : IRequest
    {
        public override void ProcessRequest(System.Web.HttpContext context, RequestContext RequestContext)
        {
            //请求类型
            string type = Utils.ObjectToStr(RequestContext.RouteData.Values["type"]);
            switch (type)
            {
                case "getVCode":
                    GetVerificationCode(context, RequestContext);
                    break;
                case "getVCodeForImg":
                    GetVerificationCode1(context, RequestContext);
                    break;
                default:
                    break;
            }
        }
       

        /// <summary>
        /// 生成验证码
        /// </summary>
        private void GetVerificationCode(System.Web.HttpContext context, RequestContext requestContext)
        {
            int number;
            char code;
            string checkCode = String.Empty;

            var random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }

            string imgSrc = string.Empty;
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            var image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random1 = new Random();
                //清空图片背景色 
                g.Clear(Color.White);
                //画图片的背景噪音线 
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random1.Next(image.Width);
                    int x2 = random1.Next(image.Width);
                    int y1 = random1.Next(image.Height);
                    int y2 = random1.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                var font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                var brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点 
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                var ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                var result = checkCode + "|" + Convert.ToBase64String(ms.ToArray());
                Utils.WriteCookie("vCode", Utils.UrlEncode(checkCode));
                Json(result);
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }


        /// <summary>
        /// 生成验证码
        /// </summary>
        private void GetVerificationCode1(System.Web.HttpContext context, RequestContext requestContext)
        {
            int number;
            char code;
            string checkCode = String.Empty;

            var random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }

            string imgSrc = string.Empty;
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            var image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random1 = new Random();
                //清空图片背景色 
                g.Clear(Color.White);
                //画图片的背景噪音线 
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random1.Next(image.Width);
                    int x2 = random1.Next(image.Width);
                    int y1 = random1.Next(image.Height);
                    int y2 = random1.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                var font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                var brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点 
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                var ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

                Utils.WriteCookie("vCode", Utils.MD5(checkCode.ToLower()));
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                context.Response.BinaryWrite(ms.ToArray());

            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }


    }
}
