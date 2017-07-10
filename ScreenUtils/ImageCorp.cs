using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ZXing;

namespace Lime.Utils
{
    public class ImageCorp
    {

       public static Image Corp(Image image,ResultPoint[] points) {
            return Corp(((Bitmap)image),points);
        }

        public static Bitmap Corp(Bitmap image, ResultPoint[] points) {
            ImageRect rectImage = corpRect(points);
            Rectangle rect = new Rectangle((int)rectImage.Cx, (int)rectImage.Cy, (int)rectImage.Width, (int)rectImage.Height);
            return image.Clone(rect, image.PixelFormat);
        }

        public static Bitmap Corp(Bitmap image, ResultPoint[] points,int textLength) {
            ImageRect rectImage = corpRect(points,textLength);
            Rectangle rect = new Rectangle((int)rectImage.Cx, (int)rectImage.Cy, (int)rectImage.Width, (int)rectImage.Height);
            return image.Clone(rect, image.PixelFormat);
        }

        protected class ImageRect
        {
            public ImageRect(float minX,float minY,
                float maxX,float maxY){
                if (maxX < minX || maxY < minY) {
                    throw new Exception("Data out of normal range. The minimum value must be less than the maximum value");
                }
                MaxX = maxX;
                MaxY = maxY;
                MinX = minX;
                MinY = minY;
            }

            public float Cx { get { return MinX; } }
            public float Cy { get{ return MinY; } }
            public float Width { get { return MaxX - MinX; } }
            public float Height { get { return MaxY - MinY; } }
            public float MaxX { get; }
            public float MaxY { get; }
            public float MinX { get; }
            public float MinY { get; }
        }

        /// <summary>
        /// 剪裁图片
        /// </summary>
        /// <param name="rps"></param>
        /// <returns></returns>
        protected static ImageRect corpRect(ResultPoint[] rps) {
            float minX = Int32.MaxValue, minY = Int32.MaxValue, maxX = 0, maxY = 0;
            foreach(ResultPoint p in rps) {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }

            // make it 20% larger
            float margin = (maxX - minX) * 0.20f;
            return new ImageRect(minX - margin, minY - margin, 
                maxX + margin, maxY + margin);
        }

        /// <summary>
        /// 剪裁图片
        /// </summary>
        /// <param name="rps">二维码关键点</param>
        /// <param name="textLength">文本长度</param>
        /// <returns></returns>
        protected static ImageRect corpRect(ResultPoint[] rps,int textLength) {
            float minX = Int32.MaxValue, minY = Int32.MaxValue, maxX = 0, maxY = 0;
            foreach (ResultPoint p in rps) {
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }
            float margin;
            if (textLength>=10)
                // make it 20% larger
                margin = (maxX - minX) * 0.20f;
            else
                // make it 20% larger
                margin = (maxX - minX) * 0.25f;
            return new ImageRect(minX - margin, minY - margin,
                maxX + margin, maxY + margin);
        }
    }
}
