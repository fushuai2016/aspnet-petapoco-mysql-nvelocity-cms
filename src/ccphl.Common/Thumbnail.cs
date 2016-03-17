using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace ccphl.Common
{
	/// <summary>
	/// Thumbnail ��ժҪ˵����
	/// </summary>
	public class Thumbnail
	{
		private Image srcImage;
		private string srcFileName;		
		
		/// <summary>
		/// ����
		/// </summary>
		/// <param name="FileName">ԭʼͼƬ·��</param>
		public bool SetImage(string FileName)
		{
			srcFileName = Utils.GetMapPath(FileName);
			try
			{
				srcImage = Image.FromFile(srcFileName);
			}
			catch
			{
				return false;
			}
			return true;

		}

		/// <summary>
		/// �ص�
		/// </summary>
		/// <returns></returns>
		public bool ThumbnailCallback()
		{
			return false;
		}

		/// <summary>
		/// ��������ͼ,��������ͼ��Image����
		/// </summary>
		/// <param name="Width">����ͼ���</param>
		/// <param name="Height">����ͼ�߶�</param>
		/// <returns>����ͼ��Image����</returns>
		public Image GetImage(int Width,int Height)
		{
			Image img;
			Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback); 
 			img = srcImage.GetThumbnailImage(Width,Height,callb, IntPtr.Zero);
 			return img;
		}

		/// <summary>
		/// ��������ͼ
		/// </summary>
		/// <param name="Width"></param>
		/// <param name="Height"></param>
		public void SaveThumbnailImage(int Width,int Height)
		{
			switch(Path.GetExtension(srcFileName).ToLower())
			{
				case ".png":
					SaveImage(Width, Height, ImageFormat.Png);
					break;
				case ".gif":
					SaveImage(Width, Height, ImageFormat.Gif);
					break;
				default:
					SaveImage(Width, Height, ImageFormat.Jpeg);
					break;
			}
		}

		/// <summary>
		/// ��������ͼ������
		/// </summary>
		/// <param name="Width">����ͼ�Ŀ��</param>
		/// <param name="Height">����ͼ�ĸ߶�</param>
		/// <param name="imgformat">�����ͼ���ʽ</param>
		/// <returns>����ͼ��Image����</returns>
		public void SaveImage(int Width,int Height, ImageFormat imgformat)
		{
            if (imgformat != ImageFormat.Gif && (srcImage.Width > Width) || (srcImage.Height > Height))
            {
                Image img;
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                img = srcImage.GetThumbnailImage(Width, Height, callb, IntPtr.Zero);
                srcImage.Dispose();
                img.Save(srcFileName, imgformat);
                img.Dispose();
            }
		}

		#region Helper

		/// <summary>
		/// ����ͼƬ
		/// </summary>
		/// <param name="image">Image ����</param>
		/// <param name="savePath">����·��</param>
		/// <param name="ici">ָ����ʽ�ı�������</param>
		private static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
		{
			//���� ԭͼƬ ����� EncoderParameters ����
			EncoderParameters parameters = new EncoderParameters(1);
			parameters.Param[0] = new EncoderParameter(Encoder.Quality, ((long) 100));
			image.Save(savePath, ici, parameters);
			parameters.Dispose();
		}

		/// <summary>
		/// ��ȡͼ���������������������Ϣ
		/// </summary>
		/// <param name="mimeType">��������������Ķ���;�����ʼ�����Э�� (MIME) ���͵��ַ���</param>
		/// <returns>����ͼ���������������������Ϣ</returns>
		private static ImageCodecInfo GetCodecInfo(string mimeType)
		{
			ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
			foreach(ImageCodecInfo ici in CodecInfo)
			{
				if(ici.MimeType == mimeType)
                    return ici;
			}
			return null;
		}

		/// <summary>
		/// �����³ߴ�
		/// </summary>
		/// <param name="width">ԭʼ���</param>
		/// <param name="height">ԭʼ�߶�</param>
		/// <param name="maxWidth">����¿��</param>
		/// <param name="maxHeight">����¸߶�</param>
		/// <returns></returns>
		private static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
		{
            //�˴�2012-02-05�޸Ĺ�=================
            if (maxWidth <= 0)
                maxWidth = width;
            if (maxHeight <= 0)
                maxHeight = height;
            //����2012-02-05�޸Ĺ�=================
			decimal MAX_WIDTH = (decimal)maxWidth;
			decimal MAX_HEIGHT = (decimal)maxHeight;
			decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

			int newWidth, newHeight;
			decimal originalWidth = (decimal)width;
			decimal originalHeight = (decimal)height;
			
			if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT) 
			{
				decimal factor;
				// determine the largest factor 
				if (originalWidth / originalHeight > ASPECT_RATIO) 
				{
					factor = originalWidth / MAX_WIDTH;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				} 
				else 
				{
					factor = originalHeight / MAX_HEIGHT;
					newWidth = Convert.ToInt32(originalWidth / factor);
					newHeight = Convert.ToInt32(originalHeight / factor);
				}	  
			} 
			else 
			{
				newWidth = width;
				newHeight = height;
			}
			return new Size(newWidth,newHeight);			
		}

        /// <summary>
        /// �õ�ͼƬ��ʽ
        /// </summary>
        /// <param name="name">�ļ�����</param>
        /// <returns></returns>
        public static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }
		#endregion

		/// <summary>
		/// ����С������
		/// </summary>
		/// <param name="image">ͼƬ����</param>
		/// <param name="newFileName">�µ�ַ</param>
		/// <param name="newSize">���Ȼ���</param>
		public static void MakeSquareImage(Image image, string newFileName, int newSize)
		{	
			int i = 0;
			int width = image.Width;
			int height = image.Height;
			if (width > height)
				i = height;
			else
				i = width;

            Bitmap b = new Bitmap(newSize, newSize);

			try
			{
				Graphics g = Graphics.FromImage(b);
                //���ø�������ֵ��
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //���ø�����,���ٶȳ���ƽ���̶�
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
				//���������ͼ�沢��͸������ɫ���
				g.Clear(Color.Transparent);
				if (width < height)
					g.DrawImage(image,  new Rectangle(0, 0, newSize, newSize), new Rectangle(0, (height-width)/2, width, width), GraphicsUnit.Pixel);
				else
					g.DrawImage(image, new Rectangle(0, 0, newSize, newSize), new Rectangle((width-height)/2, 0, height, height), GraphicsUnit.Pixel);

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(newFileName).ToString().ToLower()));
			}
			finally
			{
				image.Dispose();
				b.Dispose();
			}
		}

        /// <summary>
        /// ����С������
        /// </summary>
        /// <param name="fileName">ͼƬ�ļ���</param>
        /// <param name="newFileName">�µ�ַ</param>
        /// <param name="newSize">���Ȼ���</param>
        public static void MakeSquareImage(string fileName, string newFileName, int newSize)
        {
            MakeSquareImage(Image.FromFile(fileName), newFileName, newSize);
        }

        /// <summary>
        /// ����Զ��С������
		/// </summary>
		/// <param name="url">ͼƬurl</param>
		/// <param name="newFileName">�µ�ַ</param>
		/// <param name="newSize">���Ȼ���</param>
        public static void MakeRemoteSquareImage(string url, string newFileName, int newSize)
        {
            Stream stream = GetRemoteImage(url);
            if (stream == null)
                return;
            Image original = Image.FromStream(stream);
            stream.Close();
            MakeSquareImage(original, newFileName, newSize);
        }

		/// <summary>
		/// ��������ͼ
		/// </summary>
		/// <param name="original">ͼƬ����</param>
		/// <param name="newFileName">��ͼ·��</param>
		/// <param name="maxWidth">�����</param>
		/// <param name="maxHeight">���߶�</param>
        public static void MakeThumbnailImage(Image original, string newFileName, int maxWidth, int maxHeight)
		{
			Size _newSize = ResizeImage(original.Width,original.Height,maxWidth, maxHeight);

            using (Image displayImage = new Bitmap(original, _newSize))
            {
                try
                {
                    displayImage.Save(newFileName, original.RawFormat);
                }
                finally
                {
                    original.Dispose();
                }
            }
		}

        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="newFileName">��ͼ·��</param>
        /// <param name="maxWidth">�����</param>
        /// <param name="maxHeight">���߶�</param>
        public static void MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight)
        {
            //2012-02-05�޸Ĺ���֧���滻
            byte[] imageBytes = File.ReadAllBytes(fileName);
            Image img = Image.FromStream(new System.IO.MemoryStream(imageBytes));
            MakeThumbnailImage(img, newFileName, maxWidth, maxHeight);
            //ԭ��
            //MakeThumbnailImage(Image.FromFile(fileName), newFileName, maxWidth, maxHeight);
        }

        #region 2012-2-19 ��������ͼƬ����ͼ����
        /// <summary>
        /// ��������ͼ
        /// </summary>
        /// <param name="fileName">Դͼ·��������·����</param>
        /// <param name="newFileName">����ͼ·��������·����</param>
        /// <param name="width">����ͼ���</param>
        /// <param name="height">����ͼ�߶�</param>
        /// <param name="mode">��������ͼ�ķ�ʽ</param>    
        public static void MakeThumbnailImage(string fileName, string newFileName, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(fileName);
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://ָ���߿����ţ����ܱ��Σ�                
                    break;
                case "W"://ָ�����߰�����                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://ָ���ߣ�������
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://ָ���߿�ü��������Σ�                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //�½�һ��bmpͼƬ
            Bitmap b = new Bitmap(towidth, toheight);
            try
            {
                //�½�һ������
                Graphics g = Graphics.FromImage(b);
                //���ø�������ֵ��
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //���ø�����,���ٶȳ���ƽ���̶�
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //��ջ�������͸������ɫ���
                g.Clear(Color.Transparent);
                //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

                SaveImage(b, newFileName, GetCodecInfo("image/" + GetFormat(newFileName).ToString().ToLower()));
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                b.Dispose();
            }
        }
        #endregion

        #region ͼƬ�ü�����
        /// <summary>
        /// �ü�ͼƬ������
        /// </summary>
        /// <param name="fileName">Դͼ·��������·����</param>
        /// <param name="newFileName">����ͼ·��������·����</param>
        /// <param name="maxWidth">����ͼ���</param>
        /// <param name="maxHeight">����ͼ�߶�</param>
        /// <param name="cropWidth">�ü����</param>
        /// <param name="cropHeight">�ü��߶�</param>
        /// <param name="X">X��</param>
        /// <param name="Y">Y��</param>
        public static bool MakeThumbnailImage(string fileName, string newFileName, int maxWidth, int maxHeight, int cropWidth, int cropHeight, int X, int Y)
        {
            byte[] imageBytes = File.ReadAllBytes(fileName);
            Image originalImage = Image.FromStream(new System.IO.MemoryStream(imageBytes));
            Bitmap b = new Bitmap(cropWidth, cropHeight);
            try
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    //���ø�������ֵ��
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //���ø�����,���ٶȳ���ƽ���̶�
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //��ջ�������͸������ɫ���
                    g.Clear(Color.Transparent);
                    //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
                    g.DrawImage(originalImage, new Rectangle(0, 0, cropWidth, cropHeight), X, Y, cropWidth, cropHeight, GraphicsUnit.Pixel);
                    Image displayImage = new Bitmap(b, maxWidth, maxHeight);
                    SaveImage(displayImage, newFileName, GetCodecInfo("image/" + GetFormat(newFileName).ToString().ToLower()));
                    return true;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                b.Dispose();
            }
        }
        /// <summary>  
        /// ����ѹ��ͼƬ
        /// </summary>  
        /// <param name="sFile">ԭͼƬ</param>  
        /// <param name="dFile">ѹ���󱣴�λ��</param>    
        /// <returns></returns>  
        public static bool Compression(string sFile, string dFile)
        {
            byte[] imageBytes = File.ReadAllBytes(sFile);
            Image iSource = Image.FromStream(new System.IO.MemoryStream(imageBytes));
            ImageFormat tFormat = iSource.RawFormat;
            int tw = iSource.Width;
            int th = iSource.Height;
            Bitmap bmp = new Bitmap(tw, th);
            Graphics g = Graphics.FromImage(bmp);
            try
            {
                g.DrawImage(iSource, new Rectangle(0, 0, tw, th), new Rectangle(0, 0, tw, th), GraphicsUnit.Pixel);
                g.Dispose();
                PropertyItem[] pt = iSource.PropertyItems;//����ͼƬ������Ϣ
                for (int i = 0; i < pt.Length; i++)
                {
                    PropertyItem p = pt[i];
                    bmp.SetPropertyItem(p);
                }
                bmp.Save(dFile, tFormat);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                bmp.Dispose();
            } 
        }
        /// <summary>  
        /// �ü�ͼƬ  
        /// </summary>  
        /// <param name="sFile">ԭͼƬ</param>  
        /// <param name="dFile">ѹ���󱣴�λ��</param> 
        /// <param name="widthShare">��ռ�� 4:3 ֵΪ4</param>
        /// <param name="heightShare">��ռ�� 4:3 ֵΪ3</param>
        /// <param name="cutType">�ü����� 0 ��� 1 ����</param>   
        /// <returns></returns>  
        public static bool Crop(string sFile, string dFile,int widthShare,int heightShare, int cutType)
        {
            int w = widthShare;
            int h = heightShare;

            byte[] imageBytes = File.ReadAllBytes(sFile);
            Image iSource = Image.FromStream(new System.IO.MemoryStream(imageBytes));
            ImageFormat tFormat = iSource.RawFormat;
            int tw = iSource.Width;
            int th = iSource.Height;

            int newW = 0;
            int newH = 0;
            int starX = 0;
            int starY = 0;
            int endX = 0;
            int endY = 0;

            if (cutType == 0)//���ü� ���ػ�����
            {
                //���㻭����С
                if (tw >= th)//ԭͼ����ڸ�
                {
                    if (w >= h)//�ü����� ����ڸ�
                    {
                        newW = tw;
                        newH = (int)((tw * h) / w);
                        starX = 0;
                        starY = (int)((newH-th)/2);
                        if (newH < th)
                        {
                            newH = th;
                            newW = (int)((w * th) / h);
                            starX = (int)((newW-tw)/2);
                            starY = 0;
                        }
                    }
                    else
                    {
                        newW = tw;
                        newH = (int)((tw * h) / w);
                        starX = 0;
                        starY = (int)((newH - th) / 2);
                    }
                }
                else//ԭͼ��С�ڸ�
                {
                    if (w >= h)//�ü����� ����ڸ�
                    {
                        newH = th;
                        newW = (int)((w * th) / h);
                        starX = (int)((newW - tw) / 2);
                        starY = 0;
                    }
                    else
                    {
                        newH = th;
                        newW = (int)((w * th) / h);
                        starX = (int)((newW - tw) / 2);
                        starY = 0;
                        if (newW < tw)
                        {
                            newW = tw;
                            newH = (int)((h * tw) / w);
                            starX = 0;
                            starY = (int)((newH - th) / 2);
                        }
                    }
                }
                Bitmap bmp = new Bitmap(newW, newH, PixelFormat.Format32bppRgb);
                Graphics g = Graphics.FromImage(bmp);
                try
                {
                    g.Clear(Color.White);
                    g.DrawImage(iSource, new Rectangle(starX, starY, tw, th), new Rectangle(0, 0, tw, th), GraphicsUnit.Pixel);
                    g.Dispose();
                  
                    bmp.Save(dFile, tFormat);
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    iSource.Dispose();
                    bmp.Dispose();
                }

            }
            else//���вü� ���ػ����
            {
                //���㻭����С
                if (tw >= th)
                {
                    if (w >= h)//�ü����� ����ڸ�
                    {
                        int x1 = (int)((w * th) / h);
                        starX = (int)((tw - x1) / 2);
                        starY = 0;
                        endX = x1;
                        endY = th;
                        newW = x1;
                        newH = th;
                        if (x1 > tw)
                        {
                            int y1 = (int)((h * tw) / w);
                            starX = 0;
                            starY = (int)((th - y1) / 2);
                            endX = tw;
                            endY = y1;
                            newW = tw;
                            newH = y1;
                        }
                  
                    }
                    else
                    {
                        int x1 = (int)((w * th) / h);
                        starX = (int)((tw - x1) / 2);
                        starY = 0;
                        endX = x1;
                        endY = th;
                        newW = x1;
                        newH = th;
                    }
                }
                else
                {
                    if (w >= h)//�ü����� ����ڸ�
                    {
                        int y1 = (int)((h * tw) / w);
                        starX = 0;
                        starY = (int)((th - y1) / 2);
                        endX = tw;
                        endY = y1;
                        newW = tw;
                        newH = y1;
                    }
                    else
                    {
                        int y1 = (int)((h * tw) / w);
                        starX = 0;
                        starY = (int)((th - y1) / 2);
                        endX = tw;
                        endY = y1;
                        newW = tw;
                        newH = y1;
                        if (y1 > th)
                        {
                            int x1 = (int)((w * th) / h);
                            starX = (int)((tw - x1) / 2);
                            starY = 0;
                            endX = x1;
                            endY = th;
                            newW = x1;
                            newH = th;
                        }
                    }
                }
                Bitmap bmp = new Bitmap(newW, newH, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmp);
                try
                {
                    g.Clear(Color.White);
                    g.DrawImage(iSource, new Rectangle(0, 0, endX, endY), new Rectangle(starX, starY, endX, endY), GraphicsUnit.Pixel);
                    g.Dispose();
                   
                    bmp.Save(dFile, tFormat);
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    iSource.Dispose();
                    bmp.Dispose();
                }
            }
        }
        #endregion

        /// <summary>
        /// ����Զ������ͼ
        /// </summary>
        /// <param name="url">ͼƬURL</param>
        /// <param name="newFileName">��ͼ·��</param>
        /// <param name="maxWidth">�����</param>
        /// <param name="maxHeight">���߶�</param>
        public static void MakeRemoteThumbnailImage(string url, string newFileName, int maxWidth, int maxHeight)
        {
            Stream stream = GetRemoteImage(url);
            if(stream == null)
                return;
            Image original = Image.FromStream(stream);
            stream.Close();
            MakeThumbnailImage(original, newFileName, maxWidth, maxHeight);
        }

        /// <summary>
        /// ��ȡͼƬ��
        /// </summary>
        /// <param name="url">ͼƬURL</param>
        /// <returns></returns>
        private static Stream GetRemoteImage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentLength = 0;
            request.Timeout = 20000;
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            }
            catch
            {
                return null;
            }
        }
	}
}
