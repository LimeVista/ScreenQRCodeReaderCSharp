using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MainWpf.Utils
{
    public static class Convert
    {
        /// <summary>
        /// 把Bitmap数据转换为WPF所支持的数据格式
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <returns>BitmapImage</returns>
        public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetHbitmap();
            BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            //release resource  
            DeleteObject(ptr);
            return result;
        }


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}
