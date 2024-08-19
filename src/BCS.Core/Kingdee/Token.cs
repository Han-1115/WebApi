using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Drawing;

namespace BCS.Core.Kingdee
{
    public static class Token
    {
        /// <summary>
        /// 令牌生成器
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string CreateToken(String userName, String otp)
        {
            try
            {
                //获取配置文件信息
                int time = 40;

                string pwd = otp;

                //生成令牌密钥
                string ret = "";
                //生成4位随机字节数组
                byte[] head = GenerateRandomByteArray(4);
                // Get the current date and time
                DateTime creatDate = DateTime.Now;
                //expireDate add 30mi;
                DateTime expireDate = creatDate.AddMinutes(time);//expireDate=当前时间+time值
                byte[] creation = Encoding.Default.GetBytes(getCurruentData().ToString("X").ToUpper());
                byte[] expires = Encoding.Default.GetBytes((getCurruentData() + 30 * 60).ToString("X").ToUpper());
                byte[] user = Encoding.UTF8.GetBytes(userName);
                byte[] conbin = Copybyte(head, Copybyte(creation, Copybyte(expires, user)));
                byte[] tempconbin = Copybyte(conbin, Base64Decode(pwd));
                //哈希算法
                SHA1 sha1 = SHA1.Create();
                byte[] digest = sha1.ComputeHash(tempconbin);
                //五个重组，后面是digest转16进制
                conbin = Copybyte(conbin, Encoding.UTF8.GetBytes((Byte2Hex(digest))));
                //base64
                ret = Base64Encode(conbin);
                //对token作UrlEncode
                return ret;
            }
            catch (Exception e)
            {
                throw new Exception("发生错误!" + e.ToString());
            }
        }

        private static long getCurruentData(bool isMillisecond = false)
        {
            //return DateTimeOffset.UtcNow.ToUnixTimeSeconds(); ;
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var timeStamp = isMillisecond ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
            return timeStamp;
        }

        private static byte[] GenerateRandomByteArray(int length)
        {
            byte[] randomBytes = new byte[length];
            Random random = new Random();
            random.NextBytes(randomBytes);
            return randomBytes;
        }

        /// <summary>
        /// byte数组转化为16进制
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static string Byte2Hex(byte[] b)
        {
            string hs = "";
            string stmp = "";
            for (int n = 0; n < b.Length; n++)
            {
                //stmp = (java.lang.Integer.toHexString(b[n] & 0XFF));
                stmp = b[n].ToString("X2");
                hs = hs + stmp;
            }

            return hs.ToUpper();
        }

        /// <summary>
        /// 计算HASH值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
#pragma warning disable S1144 // Unused private types or members should be removed
        private static byte[] CreateSha1(byte[] code)
        {
            HashAlgorithm sha = new SHA1CryptoServiceProvider();
            //SHA1 sha = SHA1.Create();
            byte[] data = sha.ComputeHash(code);
            HMACSHA1 sha1 = new HMACSHA1();

            return data;
        }
#pragma warning restore S1144 // Unused private types or members should be removed

        /// <summary>
        /// 计算日期的时间戳
        /// </summary>
        /// <param name="DateTime">第一个日期和时间</param>
        /// <returns></returns>
#pragma warning disable S1144 // Unused private types or members should be removed
        private static long DateDiff(DateTime dataTime)
        {
            DateTime baseTime = DateTime.Parse("1970-01-01");
            long ret = 0;
            TimeSpan ts1 = new TimeSpan(dataTime.Ticks);
            TimeSpan ts2 = new TimeSpan(baseTime.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            ret = ts.Days * 24 * 3600 + ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
            return ret;
        }
#pragma warning restore S1144 // Unused private types or members should be removed

        /// <summary>
        /// byte数组合并
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static byte[] Copybyte(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }


        /// <summary>   
        /// Base64编码   
        /// </summary>   
        /// <param name="Message"></param>   
        /// <returns></returns>   
        private static string Base64Encode(byte[] Message)
        {
            char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
                'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
                'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
            '8','9','+','/','='};
            byte empty = (byte)0;
            //System.Collections.ArrayList byteMessage = new System.Collections.ArrayList(System.Text.Encoding.Default.GetBytes(Message));  
            System.Collections.ArrayList byteMessage = new System.Collections.ArrayList(Message);
            System.Text.StringBuilder outmessage;
            int messageLen = byteMessage.Count;
            int page = messageLen / 3;
            int use = 0;
            if ((use = messageLen % 3) > 0)
            {
                for (int i = 0; i < 3 - use; i++)
                    byteMessage.Add(empty);
                page++;
            }
            outmessage = new System.Text.StringBuilder(page * 4);
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[3];
                instr[0] = (byte)byteMessage[i * 3];
                instr[1] = (byte)byteMessage[i * 3 + 1];
                instr[2] = (byte)byteMessage[i * 3 + 2];
                int[] outstr = new int[4];
                outstr[0] = instr[0] >> 2;
                outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                if (!instr[1].Equals(empty))
                    outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                else
                    outstr[2] = 64;
                if (!instr[2].Equals(empty))
                    outstr[3] = (instr[2] & 0x3f);
                else
                    outstr[3] = 64;
                outmessage.Append(Base64Code[outstr[0]]);
                outmessage.Append(Base64Code[outstr[1]]);
                outmessage.Append(Base64Code[outstr[2]]);
                outmessage.Append(Base64Code[outstr[3]]);
            }
            return outmessage.ToString();
        }

        /// <summary>   
        /// Base64解码   
        /// </summary>   
        /// <param name="Message"></param>   
        /// <returns></returns>   
        public static byte[] Base64Decode(string Message)
        {
            if ((Message.Length % 4) != 0)
            {
                throw new ArgumentException("不是正确的BASE64编码，请检查。", "Message");
            }
            //if (!System.Text.RegularExpressions.Regex.IsMatch(Message,"^[A-Z0-9/+=]*{1}quot;", System.Text.RegularExpressions.RegexOptions.IgnoreCase))  
            //{  
            //    throw new ArgumentException("包含不正确的BASE64编码，请检查。", "Message");  
            //}  
            string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            int page = Message.Length / 4;
            System.Collections.ArrayList outMessage = new System.Collections.ArrayList(page * 3);
            char[] message = Message.ToCharArray();
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[4];
                instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                byte[] outstr = new byte[3];
                outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                if (instr[2] != 64)
                {
                    outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                }
                else
                {
                    outstr[2] = 0;
                }
                if (instr[3] != 64)
                {
                    outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                }
                else
                {
                    outstr[2] = 0;
                }
                outMessage.Add(outstr[0]);
                if (outstr[1] != 0)
                    outMessage.Add(outstr[1]);
                if (outstr[2] != 0)
                    outMessage.Add(outstr[2]);
            }
            byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
            return outbyte;
        }
    }
}