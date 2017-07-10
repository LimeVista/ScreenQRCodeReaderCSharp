using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lime.Utils
{
    public class ChineseCode
    {
        public static bool IsChineseCharacter(string chineseStr) {

            foreach (char c in chineseStr) {
                //是否是Unicode编码,除了"�"这个字符.这个字符要另外处理  
                if ((c >= '\u0000' && c < '\uFFFD') || ((c > '\uFFFD' && c < '\uFFFF'))) {
                    continue;
                }
                else
                    return false;
            }
            return true;
        }

        public static bool IsSpecialCharacter(string str) {
            //是"�"这个特殊字符的乱码情况  
            if (str.Contains("ï¿½")) {
                return true;
            }
            return false;
        }

        public static string Encode(string str) {
            bool isCn = IsChineseCharacter(str);
            bool isSC = IsSpecialCharacter(str);
            isCn = isSC ? true : isCn;
            if (isCn) {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                return Encoding.UTF8.GetString(bytes);
            }
            return str;
        }
    }
}
