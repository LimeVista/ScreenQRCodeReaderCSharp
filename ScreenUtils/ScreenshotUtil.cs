using System.Windows.Forms;
using System.Drawing;

namespace Lime.Utils
{
    /// <summary>
    /// 截屏工具类
    /// </summary>
    public static class ScreenshotUtil
    {
        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public static int ScreenWidth { get; private set; }

        /// <summary>
        /// 屏幕高度
        /// </summary>
        public static int ScreenHeight { get; private set; }

        /// <summary>
        /// 截屏
        /// </summary>
        public static Image Screenshot()
        {
            var image = new Bitmap(ScreenWidth, ScreenHeight);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(ScreenWidth, ScreenHeight));
            }
            return image;
        }

        static ScreenshotUtil()
        {
            ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
        }
    }
}
