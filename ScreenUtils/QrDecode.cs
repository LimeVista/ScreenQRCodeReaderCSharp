using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace Lime.Utils
{
    /// <summary>
    /// 解析二维码
    /// </summary>
    public class QRDecode
    {
        private QRDecode() { }

        /// <summary>
        /// 二维码解析
        /// </summary>
        /// <param name="bitmap">二维码图片</param>
        /// <returns>return Zxing libary Result</returns>
        public static Result DecoderResult(Bitmap bitmap)
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(bitmap);
            return result;
        }

        /// <summary>
        /// 二维码解析
        /// </summary>
        /// <param name="bitmap">二维码图片</param>
        /// <returns>二维码解析结果</returns>
        public static string Decoder(Bitmap bitmap)
        {
            var result = DecoderResult(bitmap);
            return result?.Text;
        }

        /// <summary>
        /// 多个二维码解析
        /// </summary>
        /// <param name="bitmap">二维码图片</param>
        /// <returns>二维码解析结果</returns>
        public static Result[] MultipleDecoder(Bitmap bitmap)
        {
            var reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            return reader.DecodeMultiple(bitmap);
        }

        /// <summary>
        /// 多个二维码解析，自定义版本
        /// </summary>
        /// <param name="bitmap">二维码图片</param>
        /// <returns>二维码解析结果</returns>
        public static List<DecodeResult> MultipleDecoderCustom(Bitmap bitmap)
        {
            var res = MultipleDecoder(bitmap);
            if (res == null) return null;
            List<DecodeResult> drs = new List<DecodeResult>(res.Length);
            foreach (var r in res)
            {
                var image = ImageCorp.Corp(bitmap, r.ResultPoints, r.Text.Length);
                drs.Add(new DecodeResult(image, r.Text));
            }
            return drs;
        }

        public static Result DecoderHybrid(Bitmap bitmap)
        {
            //提高识别率
            var source = new BitmapLuminanceSource(bitmap);
            var bmp = new BinaryBitmap(new HybridBinarizer(source));
            return new MultiFormatReader().decode(bmp);
        }
    }
}
