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
    public class QrDecode
    {
        private QrDecode() {}

        /// <summary>
        /// 二维码解析
        /// </summary>
        /// <param name="bitmap">二维码图片</param>
        /// <returns>return Zxing libary Result</returns>
        public static Result DecoderResult(Bitmap bitmap) {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(bitmap);
            // do something with the result
            // if (result != null) {
            //     txtDecoderType.Text = result.BarcodeFormat.ToString();
            //     txtDecoderContent.Text = result.Text;
            // }
            return result;
        }

        /// <summary>
        /// 二维码解析
        /// </summary>
        /// <param name="bitmap">二维码图片</param>
        /// <returns>二维码解析结果</returns>
        public static string Decoder(Bitmap bitmap) {
            var result = DecoderResult(bitmap);
            return result == null ? null : result.Text;
        }

        /// <summary>
        /// 多个二维码解析
        /// </summary>
        /// <param name="bitmap">二维码图片</param>
        /// <returns>return Zxing libary Result</returns>
        public static Result[] MultipleDecoder(Bitmap bitmap) {
            IMultipleBarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            return reader.DecodeMultiple(bitmap);
        }

        public static List<DecodeResult> MultipleDecoderCustom(Bitmap bitmap) {
            var res = MultipleDecoder(bitmap);
            if (res == null) return null;
            List<DecodeResult> drs = new List<DecodeResult>(res.Length);
            foreach (var r in res) {
                Image img = ImageCorp.Corp(bitmap, r.ResultPoints, r.Text.Length);
                drs.Add(new DecodeResult(img, r.Text));
            }
            return drs;
        }

        public static Result DecoderHybrid(Bitmap bitmap) {
            //提高识别率
            LuminanceSource source = new BitmapLuminanceSource(bitmap);
            BinaryBitmap bmp = new BinaryBitmap(new HybridBinarizer(source));
            return new MultiFormatReader().decode(bmp);
        }
    }
}
