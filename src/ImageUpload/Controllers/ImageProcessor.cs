using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageUpload
{
    public class ImageProcessor
    {
        const int SIZE_PREVIEW = 100;
        
        public void fromFile(string path)
        {
            try
            {
                byte[] imgRaw = File.ReadAllBytes(path);
                string[] paths = formFilePaths(path);
                Image img = null;
                try
                {
                    img = Image.FromStream(new MemoryStream(imgRaw));
                }
                catch (Exception ex)
                {
                    ex = new Exception("Failed to process file as image");
                    throw ex;
                }
                if (img != null)
                {
                    Image thumb = ImageResizeInMemory(img, SIZE_PREVIEW);
                    //SAVE WITH ORIG SIZE
                    img.Save(paths[0], ImageFormat.Png);
                    //SAVE THUMBNAIL
                    thumb.Save(paths[1], ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void from64(string str){
            try
            {
                byte[] imgRaw = Convert.FromBase64String(str);
                string[] paths = formFilePaths(str);
                Image img = null;
                try
                {
                    img = Image.FromStream(new MemoryStream(imgRaw));
                }
                catch (Exception ex)
                {
                    ex = new Exception("Failed to process file as image");
                    throw ex;
                }
                if (img != null)
                {
                    Image thumb = ImageResizeInMemory(img, SIZE_PREVIEW);
                    //SAVE WITH ORIG SIZE
                    img.Save(paths[0], ImageFormat.Png);
                    //SAVE THUMBNAIL
                    thumb.Save(paths[1], ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void fromUrl(string url)
        {
            try
            {
                Stream imgRaw = (Stream)Utils.getStreamFromURL(url);
                string[] paths = formFilePaths(url);
                if (imgRaw != null)
                {
                    if (imgRaw.CanRead) 
                    {
                        Image img = null;
                        try
                        {
                            img = Image.FromStream(imgRaw);
                        }
                        catch (Exception ex)
                        {
                            ex = new Exception("Failed to process file as image");
                            throw ex;
                        }
                        if (img != null)
                        {
                            Image thumb = ImageResizeInMemory(img, SIZE_PREVIEW);
                            //SAVE WITH ORIG SIZE
                            img.Save(paths[0], ImageFormat.Png);
                            //SAVE THUMBNAIL
                            thumb.Save(paths[1], ImageFormat.Png);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-----------------------
        string[] formFilePaths(string name)
        {
            string dir="",
                    fileNamefileName = name.GetHashCode().ToString().Replace("-", "") + "_" +
                                    DateTime.UtcNow.ToBinary().ToString() + ".png";
            dir = Utils.getWorkDirectory() + "uploaded_images/";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string fileName = name.GetHashCode().ToString().Replace("-", "") + "_" +
                                DateTime.UtcNow.ToBinary().ToString() + ".png";
            return new string[] { dir + fileName, dir +"thumb"+ fileName};
        }
        public static Bitmap ImageResizeInMemory(Image image, int squareSize)
        {
            var destRect = new Rectangle(0, 0, squareSize, squareSize);
            var destImage = new Bitmap(squareSize, squareSize);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    var w = Math.Min(image.Width, image.Height);
                    //
                    var clip = new Rectangle((int)(image.Width / 2.0 - w / 2.0), (int)(image.Height / 2.0 - w / 2.0), w, w);
                    //
                    graphics.DrawImage(image, destRect,clip, GraphicsUnit.Pixel);
                }
            }

            return destImage;
        }
    }
}