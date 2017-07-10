using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lime.Utils
{
    public class DecodeResult
    {
        public DecodeResult(Image img,string text) {
            QrImage = img;
            Text = text;
        }
        public string Text { get; set; }
        public Image QrImage { get; set; }
    }
}
