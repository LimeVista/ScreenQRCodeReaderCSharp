using System.Drawing;

namespace Lime.Utils
{
    /// <summary>
    /// 解析结果
    /// </summary>
    public class DecodeResult
    {
        public DecodeResult(Image image, string text)
        {
            QRImage = image;
            Text = text;
        }

        /// <summary>
        /// 解析后的文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// QR 图片
        /// </summary>
        public Image QRImage { get; set; }
    }
}
