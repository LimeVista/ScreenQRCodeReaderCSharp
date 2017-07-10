using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Lime.Utils
{
    public static class ScreenshotUtil
    {
        private static int _dw, _dh;

        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public static int ScreenWidth {
            get { return _dw; }
        }

        /// <summary>
        /// 屏幕高度
        /// </summary>
        public static int ScreenHeight {
            get { return _dh; }
        }

        /// <summary>
        /// 截图
        /// </summary>
        public static Image Screenshot {
            get {
                Image img = new Bitmap(_dw, _dh);
                using (Graphics g = Graphics.FromImage(img))
                    g.CopyFromScreen(0, 0, 0, 0, new Size(_dw, _dh));
                return img;
            }
        }

        static ScreenshotUtil() {
            _dw = Screen.PrimaryScreen.Bounds.Width;
            _dh = Screen.PrimaryScreen.Bounds.Height;
        }
    }
}
