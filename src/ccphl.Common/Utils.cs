using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Configuration;
using System.Web;
using System.Security.Cryptography;

namespace ccphl.Common
{
    public class Utils
    {
        #region 系统版本
        /// <summary>
        /// 版本信息类
        /// </summary>
        public class VersionInfo
        {
            public int FileMajorPart
            {
                get { return 3; }
            }
            public int FileMinorPart
            {
                get { return 0; }
            }
            public int FileBuildPart
            {
                get { return 0; }
            }
            public string ProductName
            {
                get { return "DTcms"; }
            }
            public int ProductType
            {
                get { return 0; }
            }
        }
        public static string GetVersion()
        {
            return DTKeys.ASSEMBLY_VERSION;
        }
        #endregion

        #region MD5加密
        public static string MD5(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');

            }
            return str;
        }
        #endregion

        #region 对象转换处理
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }
        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="speater">分隔符</param>
        /// <returns>String</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
                return StrToBool(expression, defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
                return StrToFloat(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的string类型结果</returns>
        public static string ObjectToStr(object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString().Trim();
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.Length < 1)
            {
                return "";
            }
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        #endregion

        #region 生成指定长度的字符串
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }

            return ReturnStr;
        }
        #endregion

        #region 生成日期随机码
        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            #region
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            #endregion
        }
        #endregion

        #region 生成随机字母或数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");//yyyyMMddHHmmssms
            return num + Number(2).ToString();
        }
        private static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//这里用uint作为生成的随机数  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }
        #endregion

        #region 截取字符长度
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        #endregion

        #region 清除HTML标记
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        #endregion

        #region 清除HTML标记且返回相应的长度
        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }
        #endregion

        #region TXT代码转换成HTML格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把TXT代码转换成HTML格式
        public static String ToHtml(string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            //sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }
        #endregion

        #region HTML代码转换成TXT格式
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(String Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }
        #endregion

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        
        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary> 
        /// 检查过滤设定的危险字符
        /// </summary> 
        /// <param name="InText">要过滤的字符串 </param> 
        /// <returns>如果参数存在不安全字符，则返回true </returns> 
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Htmls(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region 获得配置文件节点XML文件的绝对路径
        public static string GetXmlMapPath(string xmlName)
        {
            return GetMapPath(ConfigurationManager.AppSettings[xmlName].ToString());
        }
        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除上传的文件
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string fullpath = GetMapPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
           
        }

        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="_dirpath">文件相对路径</param>
        public static bool DeleteDirectory(string _dirpath)
        {
            if (string.IsNullOrEmpty(_dirpath))
            {
                return false;
            }
            string fullpath = GetMapPath(_dirpath);
            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改指定文件夹名称
        /// </summary>
        /// <param name="old_dirpath">旧相对路径</param>
        /// <param name="new_dirpath">新相对路径</param>
        /// <returns>bool</returns>
        public static bool MoveDirectory(string old_dirpath, string new_dirpath)
        {
            if (string.IsNullOrEmpty(old_dirpath))
            {
                return false;
            }
            string fulloldpath = GetMapPath(old_dirpath);
            string fullnewpath = GetMapPath(new_dirpath);
            if (Directory.Exists(fulloldpath))
            {
                Directory.Move(fulloldpath, fullnewpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filepath)
        {
            return _filepath.Substring(_filepath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得远程字符串
        /// </summary>
        public static string GetDomainStr(string key, string uriPath)
        {
            string result = CacheHelper.Get(key) as string;
            if (result == null)
            {
                System.Net.WebClient client = new System.Net.WebClient();
                try
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    result = client.DownloadString(uriPath);
                }
                catch
                {
                    result = "暂时无法连接!";
                }
                CacheHelper.Insert(key, result, 60);
            }

            return result;
        }

        #endregion

        #region 读取或写入cookie
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟)</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }
        #endregion

        #region 替换指定的字符串
        /// <summary>
        /// 替换指定的字符串
        /// </summary>
        /// <param name="originalStr">原字符串</param>
        /// <param name="oldStr">旧字符串</param>
        /// <param name="newStr">新字符串</param>
        /// <returns></returns>
        public static string ReplaceStr(string originalStr, string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return "";
            }
            return originalStr.Replace(oldStr, newStr);
        }
        #endregion

        #region 显示分页
        /// <summary>
        /// 返回分页页码
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="linkUrl">链接地址，__id__代表页码</param>
        /// <param name="centSize">中间页码数量</param>
        /// <returns></returns>
        public static string OutPageList(int pageSize, int pageIndex, long totalCount, string linkUrl, int centSize)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            long pageCount = totalCount / pageSize;
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            StringBuilder pageStr = new StringBuilder();
            string pageId = "__id__";
            string firstBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex - 1).ToString()) + "\">«上一页</a>";
            string lastBtn = "<a href=\"" + ReplaceStr(linkUrl, pageId, (pageIndex + 1).ToString()) + "\">下一页»</a>";
            string firstStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, "1") + "\">1</a>";
            string lastStr = "<a href=\"" + ReplaceStr(linkUrl, pageId, pageCount.ToString()) + "\">" + pageCount.ToString() + "</a>";

            if (pageIndex <= 1)
            {
                firstBtn = "<span class=\"disabled\">«上一页</span>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<span class=\"disabled\">下一页»</span>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<span class=\"current\">1</span>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<span class=\"current\">" + pageCount.ToString() + "</span>";
            }
            int firstNum = pageIndex - (centSize / 2); //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            long lastNum = pageIndex + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;
            pageStr.Append("<span>共" + totalCount + "记录</span>");
            pageStr.Append(firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<span>...</span>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<span class=\"current\">" + i + "</span>");
                }
                else
                {
                    pageStr.Append("<a href=\"" + ReplaceStr(linkUrl, pageId, i.ToString()) + "\">" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - ((centSize / 2)))
            {
                pageStr.Append("<span>...</span>");
            }
            pageStr.Append(lastStr + lastBtn);
            return pageStr.ToString();
        }
        #endregion

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }

        /// <summary>
        /// 组合URL参数
        /// </summary>
        /// <param name="_url">页面地址</param>
        /// <param name="_keys">参数名称</param>
        /// <param name="_values">参数值</param>
        /// <returns>String</returns>
        public static string CombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]) && _values[i] != "0")
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }
        #endregion

        #region URL请求数据
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }
        
        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// 执行URL获取页面内容
        /// </summary>
        public static string UrlExecute(string urlPath)
        {
            if (string.IsNullOrEmpty(urlPath))
            {
                return "error";
            }
            StringWriter sw = new StringWriter();
            try
            {
                HttpContext.Current.Server.Execute(urlPath, sw);
                return sw.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }
        #endregion

        #region 操作权限菜单
        /// <summary>
        /// 获取操作权限
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, int> ActionType()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("显示", 1);
            dic.Add("查看", 2);
            dic.Add("新增", 4);
            dic.Add("编辑", 8);
            dic.Add("删除", 16);
            dic.Add("审核", 32);
            dic.Add("回复", 64);
            dic.Add("确认", 128);
            dic.Add("取消", 256);
            dic.Add("作废", 512);
            dic.Add("生成", 1024);
            dic.Add("安装", 2048);
            dic.Add("卸载", 4096);
            dic.Add("备份", 8192);
            dic.Add("还原", 16384);
            dic.Add("替换", 32768);
            return dic;
        }
        public static List<ColumnType> ColumnTypeList()
        {
            List<ColumnType> clist = new List<ColumnType>();
            clist.Add(new ColumnType { Id=1,Name="内容管理",Url="article/article_list.aspx",Authority=31});
            clist.Add(new ColumnType { Id = 2, Name = "栏目管理", Url = "article/column_list.aspx", Authority = 31 });
            clist.Add(new ColumnType { Id = 3, Name = "评论管理", Url = "article/comment_list.aspx", Authority = 115 });
            return clist;
        }
        /// <summary>
        /// 系统级别 栏目类
        /// </summary>
        public class ColumnType
        {
            public int Id { get;set;}
            public string Name { get; set; }
            public string Url { get; set; }
            public int Authority { get; set; }
        }
        #endregion

        #region 替换URL
        /// <summary>
        /// 替换扩展名
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            return urlPage;
        }
        /// <summary>
        /// 替换扩展名，如没有扩展名替换默认首页
        /// </summary>
        public static string GetUrlExtension(string urlPage, string staticExtension, bool defaultVal)
        {
            int indexNum = urlPage.LastIndexOf('.');
            if (indexNum > 0)
            {
                return urlPage.Replace(urlPage.Substring(indexNum), "." + staticExtension);
            }
            if (defaultVal)
            {
                if (urlPage.EndsWith("/"))
                {
                    return urlPage + "index." + staticExtension;
                }
                else
                {
                    return urlPage + "/index." + staticExtension;
                }
            }
            return urlPage;
        }
        #endregion

        #region 汉字转拼音
        /// <summary>
        /// 拼音字典
        /// </summary>
        private static readonly Dictionary<int, string> CodeCollections = new Dictionary<int, string> {  
             { -20319, "a" }, { -20317, "ai" }, { -20304, "an" }, { -20295, "ang" }, { -20292, "ao" }, { -20283, "ba" }, { -20265, "bai" },   
             { -20257, "ban" }, { -20242, "bang" }, { -20230, "bao" }, { -20051, "bei" }, { -20036, "ben" }, { -20032, "beng" }, { -20026, "bi" }  
              ,{ -20002, "bian" }, { -19990, "biao" }, { -19986, "bie" }, { -19982, "bin" }, { -19976, "bing" }, { -19805, "bo" },   
             { -19784, "bu" }, { -19775, "ca" }, { -19774, "cai" }, { -19763, "can" }, { -19756, "cang" }, { -19751, "cao" }, { -19746, "ce" },  
             { -19741, "ceng" }, { -19739, "cha" }, { -19728, "chai" }, { -19725, "chan" }, { -19715, "chang" }, { -19540, "chao" },   
             { -19531, "che" }, { -19525, "chen" }, { -19515, "cheng" }, { -19500, "chi" }, { -19484, "chong" }, { -19479, "chou" },   
             { -19467, "chu" }, { -19289, "chuai" }, { -19288, "chuan" }, { -19281, "chuang" }, { -19275, "chui" }, { -19270, "chun" },  
             { -19263, "chuo" }, { -19261, "ci" }, { -19249, "cong" }, { -19243, "cou" }, { -19242, "cu" }, { -19238, "cuan" },   
             { -19235, "cui" }, { -19227, "cun" }, { -19224, "cuo" }, { -19218, "da" }, { -19212, "dai" }, { -19038, "dan" }, { -19023, "dang" },  
             { -19018, "dao" }, { -19006, "de" }, { -19003, "deng" }, { -18996, "di" }, { -18977, "dian" }, { -18961, "diao" }, { -18952, "die" }  
              ,{ -18783, "ding" }, { -18774, "diu" }, { -18773, "dong" }, { -18763, "dou" }, { -18756, "du" }, { -18741, "duan" },   
             { -18735, "dui" }, { -18731, "dun" }, { -18722, "duo" }, { -18710, "e" }, { -18697, "en" }, { -18696, "er" }, { -18526, "fa" },  
             { -18518, "fan" }, { -18501, "fang" }, { -18490, "fei" }, { -18478, "fen" }, { -18463, "feng" }, { -18448, "fo" }, { -18447, "fou" }  
              ,{ -18446, "fu" }, { -18239, "ga" }, { -18237, "gai" }, { -18231, "gan" }, { -18220, "gang" }, { -18211, "gao" }, { -18201, "ge" },  
             { -18184, "gei" }, { -18183, "gen" }, { -18181, "geng" }, { -18012, "gong" }, { -17997, "gou" }, { -17988, "gu" }, { -17970, "gua" }  
              ,{ -17964, "guai" }, { -17961, "guan" }, { -17950, "guang" }, { -17947, "gui" }, { -17931, "gun" }, { -17928, "guo" },  
             { -17922, "ha" }, { -17759, "hai" }, { -17752, "han" }, { -17733, "hang" }, { -17730, "hao" }, { -17721, "he" }, { -17703, "hei" },  
             { -17701, "hen" }, { -17697, "heng" }, { -17692, "hong" }, { -17683, "hou" }, { -17676, "hu" }, { -17496, "hua" },   
             { -17487, "huai" }, { -17482, "huan" }, { -17468, "huang" }, { -17454, "hui" }, { -17433, "hun" }, { -17427, "huo" },   
             { -17417, "ji" }, { -17202, "jia" }, { -17185, "jian" }, { -16983, "jiang" }, { -16970, "jiao" }, { -16942, "jie" },   
             { -16915, "jin" }, { -16733, "jing" }, { -16708, "jiong" }, { -16706, "jiu" }, { -16689, "ju" }, { -16664, "juan" },   
             { -16657, "jue" }, { -16647, "jun" }, { -16474, "ka" }, { -16470, "kai" }, { -16465, "kan" }, { -16459, "kang" }, { -16452, "kao" },  
             { -16448, "ke" }, { -16433, "ken" }, { -16429, "keng" }, { -16427, "kong" }, { -16423, "kou" }, { -16419, "ku" }, { -16412, "kua" }  
              ,{ -16407, "kuai" }, { -16403, "kuan" }, { -16401, "kuang" }, { -16393, "kui" }, { -16220, "kun" }, { -16216, "kuo" },   
             { -16212, "la" }, { -16205, "lai" }, { -16202, "lan" }, { -16187, "lang" }, { -16180, "lao" }, { -16171, "le" }, { -16169, "lei" },   
             { -16158, "leng" }, { -16155, "li" }, { -15959, "lia" }, { -15958, "lian" }, { -15944, "liang" }, { -15933, "liao" },   
             { -15920, "lie" }, { -15915, "lin" }, { -15903, "ling" }, { -15889, "liu" }, { -15878, "long" }, { -15707, "lou" }, { -15701, "lu" },  
             { -15681, "lv" }, { -15667, "luan" }, { -15661, "lue" }, { -15659, "lun" }, { -15652, "luo" }, { -15640, "ma" }, { -15631, "mai" },  
             { -15625, "man" }, { -15454, "mang" }, { -15448, "mao" }, { -15436, "me" }, { -15435, "mei" }, { -15419, "men" },   
             { -15416, "meng" }, { -15408, "mi" }, { -15394, "mian" }, { -15385, "miao" }, { -15377, "mie" }, { -15375, "min" },   
             { -15369, "ming" }, { -15363, "miu" }, { -15362, "mo" }, { -15183, "mou" }, { -15180, "mu" }, { -15165, "na" }, { -15158, "nai" },   
             { -15153, "nan" }, { -15150, "nang" }, { -15149, "nao" }, { -15144, "ne" }, { -15143, "nei" }, { -15141, "nen" }, { -15140, "neng" }  
             ,{ -15139, "ni" }, { -15128, "nian" }, { -15121, "niang" }, { -15119, "niao" }, { -15117, "nie" }, { -15110, "nin" },   
             { -15109, "ning" }, { -14941, "niu" }, { -14937, "nong" }, { -14933, "nu" }, { -14930, "nv" }, { -14929, "nuan" }, { -14928, "nue" }  
             ,{ -14926, "nuo" }, { -14922, "o" }, { -14921, "ou" }, { -14914, "pa" }, { -14908, "pai" }, { -14902, "pan" }, { -14894, "pang" },  
             { -14889, "pao" }, { -14882, "pei" }, { -14873, "pen" }, { -14871, "peng" }, { -14857, "pi" }, { -14678, "pian" },   
             { -14674, "piao" }, { -14670, "pie" }, { -14668, "pin" }, { -14663, "ping" }, { -14654, "po" }, { -14645, "pu" }, { -14630, "qi" },  
             { -14594, "qia" }, { -14429, "qian" }, { -14407, "qiang" }, { -14399, "qiao" }, { -14384, "qie" }, { -14379, "qin" },  
             { -14368, "qing" }, { -14355, "qiong" }, { -14353, "qiu" }, { -14345, "qu" }, { -14170, "quan" }, { -14159, "que" },   
             { -14151, "qun" }, { -14149, "ran" }, { -14145, "rang" }, { -14140, "rao" }, { -14137, "re" }, { -14135, "ren" }, { -14125, "reng" }  
             ,{ -14123, "ri" }, { -14122, "rong" }, { -14112, "rou" }, { -14109, "ru" }, { -14099, "ruan" }, { -14097, "rui" }, { -14094, "run" }  
             ,{ -14092, "ruo" }, { -14090, "sa" }, { -14087, "sai" }, { -14083, "san" }, { -13917, "sang" }, { -13914, "sao" }, { -13910, "se" }  
             ,{ -13907, "sen" }, { -13906, "seng" }, { -13905, "sha" }, { -13896, "shai" }, { -13894, "shan" }, { -13878, "shang" },   
             { -13870, "shao" }, { -13859, "she" }, { -13847, "shen" }, { -13831, "sheng" }, { -13658, "shi" }, { -13611, "shou" },  
             { -13601, "shu" }, { -13406, "shua" }, { -13404, "shuai" }, { -13400, "shuan" }, { -13398, "shuang" }, { -13395, "shui" },  
             { -13391, "shun" }, { -13387, "shuo" }, { -13383, "si" }, { -13367, "song" }, { -13359, "sou" }, { -13356, "su" },   
             { -13343, "suan" }, { -13340, "sui" }, { -13329, "sun" }, { -13326, "suo" }, { -13318, "ta" }, { -13147, "tai" }, { -13138, "tan" },  
             { -13120, "tang" }, { -13107, "tao" }, { -13096, "te" }, { -13095, "teng" }, { -13091, "ti" }, { -13076, "tian" },   
             { -13068, "tiao" }, { -13063, "tie" }, { -13060, "ting" }, { -12888, "tong" }, { -12875, "tou" }, { -12871, "tu" },   
             { -12860, "tuan" }, { -12858, "tui" }, { -12852, "tun" }, { -12849, "tuo" }, { -12838, "wa" }, { -12831, "wai" }, { -12829, "wan" }  
             ,{ -12812, "wang" }, { -12802, "wei" }, { -12607, "wen" }, { -12597, "weng" }, { -12594, "wo" }, { -12585, "wu" }, { -12556, "xi" }  
             ,{ -12359, "xia" }, { -12346, "xian" }, { -12320, "xiang" }, { -12300, "xiao" }, { -12120, "xie" }, { -12099, "xin" },   
             { -12089, "xing" }, { -12074, "xiong" }, { -12067, "xiu" }, { -12058, "xu" }, { -12039, "xuan" }, { -11867, "xue" },   
             { -11861, "xun" }, { -11847, "ya" }, { -11831, "yan" }, { -11798, "yang" }, { -11781, "yao" }, { -11604, "ye" }, { -11589, "yi" },  
             { -11536, "yin" }, { -11358, "ying" }, { -11340, "yo" }, { -11339, "yong" }, { -11324, "you" }, { -11303, "yu" },   
             { -11097, "yuan" }, { -11077, "yue" }, { -11067, "yun" }, { -11055, "za" }, { -11052, "zai" }, { -11045, "zan" },  
             { -11041, "zang" }, { -11038, "zao" }, { -11024, "ze" }, { -11020, "zei" }, { -11019, "zen" }, { -11018, "zeng" },   
             { -11014, "zha" }, { -10838, "zhai" }, { -10832, "zhan" }, { -10815, "zhang" }, { -10800, "zhao" }, { -10790, "zhe" },   
             { -10780, "zhen" }, { -10764, "zheng" }, { -10587, "zhi" }, { -10544, "zhong" }, { -10533, "zhou" }, { -10519, "zhu" },   
             { -10331, "zhua" }, { -10329, "zhuai" }, { -10328, "zhuan" }, { -10322, "zhuang" }, { -10315, "zhui" }, { -10309, "zhun" },   
             { -10307, "zhuo" }, { -10296, "zi" }, { -10281, "zong" }, { -10274, "zou" }, { -10270, "zu" }, { -10262, "zuan" }, { -10260, "zui" }  
             ,{ -10256, "zun" }, { -10254, "zuo" } };
        ///<summary>   
        ///汉字转拼音   
        ///</summary>   
        ///<param  name="txt"> 需要转换的汉字 </param>   
        ///<returns> 返回汉字对应的拼音 </returns>   
        public static string ToPinYin(string txt)
        {
            txt = txt.Trim();
            byte[] arr = new byte[2];   //每个汉字为2字节   
            StringBuilder result = new StringBuilder();//使用StringBuilder优化字符串连接  
            int charCode = 0;
            int arr1 = 0;
            int arr2 = 0;
            char[] arrChar = txt.ToCharArray();
            for (int j = 0; j < arrChar.Length; j++)   //遍历输入的字符   
            {
                arr = System.Text.Encoding.Default.GetBytes(arrChar[j].ToString());//根据系统默认编码得到字节码   
                if (arr.Length == 1)//如果只有1字节说明该字符不是汉字，结束本次循环   
                {
                    result.Append(arrChar[j].ToString());
                    continue;

                }
                arr1 = (short)(arr[0]);   //取字节1   
                arr2 = (short)(arr[1]);   //取字节2   
                charCode = arr1 * 256 + arr2 - 65536;//计算汉字的编码   

                if (charCode > -10254 || charCode < -20319)  //如果不在汉字编码范围内则不改变   
                {
                    result.Append(arrChar[j]);
                }
                else
                {
                    //根据汉字编码范围查找对应的拼音并保存到结果中   
                    //由于charCode的键不一定存在，所以要找比他更小的键上一个键  
                    if (!CodeCollections.ContainsKey(charCode))
                    {
                        for (int i = charCode; i <= 0; --i)
                        {
                            if (CodeCollections.ContainsKey(i))
                            {
                                result.Append(" " + CodeCollections[i] + " ");
                                break;
                            }
                        }
                    }
                    else
                    {
                        result.Append(" " + CodeCollections[charCode] + " ");
                    }
                }
            }
            return result.ToString();
        }
        ///<summary>   
        ///汉字转拼音首字母，如果是一个英文字母则直接返回大写字母
        ///</summary>   
        ///<param  name="txt"> 需要转换的汉字 </param>   
        ///<returns> 返回汉字对应的首大写字母序列</returns>   
        public static string ToPypellCode(string txt)
        {
            txt = txt.Trim();
            byte[] arr = new byte[2];   //每个汉字为2字节   
            StringBuilder result = new StringBuilder();//使用StringBuilder优化字符串连接  
            long iCnChar = 0;
            int arr1 = 0;
            int arr2 = 0;
            char[] arrChar = txt.ToCharArray();
            for (int j = 0; j < arrChar.Length; j++)   //遍历输入的字符   
            {
                arr = System.Text.Encoding.Default.GetBytes(arrChar[j].ToString());//根据系统默认编码得到字节码   
                if (arr.Length == 1)//如果只有1字节说明该字符不是汉字，结束本次循环   
                {
                    result.Append(arrChar[j].ToString());
                    continue;

                }
                arr1 = (short)(arr[0]);   //取字节1   
                arr2 = (short)(arr[1]);   //取字节2   
                iCnChar = arr1 * 256 + arr2;//计算汉字的编码   
                if ((iCnChar >= 45217) && (iCnChar <= 45252))
                {
                    result.Append("A");
                    continue;
                }
                else if ((iCnChar >= 45253) && (iCnChar <= 45760))
                {
                    result.Append("B");
                    continue;
                }
                else if ((iCnChar >= 45761) && (iCnChar <= 46317))
                {
                    result.Append("C");
                    continue;
                }
                else if ((iCnChar >= 46318) && (iCnChar <= 46825))
                {
                    result.Append("D");
                    continue;
                }
                else if ((iCnChar >= 46826) && (iCnChar <= 47009))
                {
                    result.Append("E");
                    continue;
                }
                else if ((iCnChar >= 47010) && (iCnChar <= 47296))
                {
                    result.Append("F");
                    continue;
                }
                else if ((iCnChar >= 47297) && (iCnChar <= 47613))
                {
                    result.Append("G");
                    continue;
                }
                else if ((iCnChar >= 47614) && (iCnChar <= 48118))
                {
                    result.Append("H");
                    continue;
                }
                else if ((iCnChar >= 48119) && (iCnChar <= 49061))
                {
                    result.Append("J");
                    continue;
                }
                else if ((iCnChar >= 49062) && (iCnChar <= 49323))
                {
                    result.Append("K");
                    continue;
                }
                else if ((iCnChar >= 49324) && (iCnChar <= 49895))
                {
                    result.Append("L");
                    continue;
                }
                else if ((iCnChar >= 49896) && (iCnChar <= 50370))
                {
                    result.Append("M");
                    continue;
                }
                else if ((iCnChar >= 50371) && (iCnChar <= 50613))
                {
                    result.Append("N");
                    continue;
                }
                else if ((iCnChar >= 50614) && (iCnChar <= 50621))
                {
                    result.Append("O");
                    continue;
                }
                else if ((iCnChar >= 50622) && (iCnChar <= 50905))
                {
                    result.Append("P");
                    continue;
                }
                else if ((iCnChar >= 50906) && (iCnChar <= .51386))
                {
                    result.Append("Q");
                    continue;
                }
                else if ((iCnChar >= 51387) && (iCnChar <= 51445))
                {
                    result.Append("R");
                    continue;
                }
                else if ((iCnChar >= 51446) && (iCnChar <= 52217))
                {
                    result.Append("S");
                    continue;
                }
                else if ((iCnChar >= 52218) && (iCnChar <= 52697))
                {
                    result.Append("T");
                    continue;
                }
                else if ((iCnChar >= 52698) && (iCnChar <= 52979))
                {
                    result.Append("W");
                    continue;
                }
                else if ((iCnChar >= 52980) && (iCnChar <= 53640))
                {
                    result.Append("X");
                    continue;
                }
                else if ((iCnChar >= 53689) && (iCnChar <= 54480))
                {
                    result.Append("Y");
                    continue;
                }
                else if ((iCnChar >= 54481) && (iCnChar <= 55289))
                {
                    result.Append("Z");
                    continue;
                }
                else
                {
                    result.Append("I");
                    continue;
                }
            }
            return result.ToString();
        }
        #endregion
    }
}
