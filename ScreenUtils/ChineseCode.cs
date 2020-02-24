using System.Text;

namespace Lime.Utils
{
    public class ChineseCode
    {
        /// <summary>
        /// 判断是否为 Unicode 编码，除了"�"这个字符，该字符另行处理
        /// </summary>
        /// <param name="chineseStr">输入串</param>
        /// <returns>是否为 Unicode 编码</returns>
        public static bool IsChineseCharacter(string chineseStr)
        {
            foreach (char c in chineseStr)
            {
                //是否是Unicode编码,除了"�"这个字符.这个字符要另外处理  
                if ((c >= '\u0000' && c < '\uFFFD') || ((c > '\uFFFD' && c < '\uFFFF')))
                {
                    continue;
                }
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 判断特殊字符"�"
        /// </summary>
        /// <param name="str">输入串</param>
        /// <returns>是否存在"�"</returns>
        public static bool IsSpecialCharacter(string str) => str.Contains("ï¿½");

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string Encode(string str)
        {
            bool isCn = IsChineseCharacter(str);
            bool isSC = IsSpecialCharacter(str);
            if (isSC || isCn)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                return Encoding.UTF8.GetString(bytes);
            }
            return str;
        }
    }
}
