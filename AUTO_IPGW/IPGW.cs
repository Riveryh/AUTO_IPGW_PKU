﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Security;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.Win32;  

namespace AUTO_IPGW
{
    static class IPGW
    {

        public const int FREE = 1;
        public const int GLOBAL = 2;
        public const int DISCONNECT = 3;
        public const int DIS_ALL = 4;

        public enum CONNECTIION_RESULT_CODE
        {
            SUCCESS,
            FAILED,
            TIME_OUT,
            NETWORK_ERROR,
            FREE_SUCCESS,
            GLOBAL_SUCCESS,
            DISCONNECT_SUCCESS,
            AUTHENTAION_ERROR
        };

        public static CONNECTIION_RESULT_CODE lastResultCode;

        public static Dictionary<CONNECTIION_RESULT_CODE, string> parseCode
            = new Dictionary<CONNECTIION_RESULT_CODE, string>
        {
            {CONNECTIION_RESULT_CODE.SUCCESS, "连接成功"},
            {CONNECTIION_RESULT_CODE.FAILED, "连接失败"},
            {CONNECTIION_RESULT_CODE.TIME_OUT, "连接超时"},
            {CONNECTIION_RESULT_CODE.FREE_SUCCESS,"免费网连接成功"},
            {CONNECTIION_RESULT_CODE.GLOBAL_SUCCESS,"收费网连接成功"},
            {CONNECTIION_RESULT_CODE.DISCONNECT_SUCCESS,"断开成功"},
            {CONNECTIION_RESULT_CODE.NETWORK_ERROR,"网络故障"},
            {CONNECTIION_RESULT_CODE.AUTHENTAION_ERROR,"用户名或密码错误"}
        };

        public static string balance_hours;

        //option:
        //1 free
        //2 global
        static public bool IPGW_connect(int option,string userName, string password){
            
            string loginUrl = "https://its.pku.edu.cn/cas/login";
            string magic = "|;kiDrqvfi7d$v0p5Fg72Vwbv2;|";
            bool loggedIn = false;

            //填写POST表单数据
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("username1", userName);
            parameters.Add("password", password);
            parameters.Add("fwrd", "free");
            parameters.Add("iprd", "open");
            parameters.Add("imageField.x", "7");
            parameters.Add("imageField.y", "3");
            parameters.Add("_currentStateId", "viewLoginForm");
            parameters.Add("_eventId", "submit");
            parameters.Add("username", userName + magic + password + magic + "2");

            //提交POST请求
            CookieCollection cookies = new CookieCollection();
            HttpWebResponse response;
            try
            {
                response = WebRequestFunction.CreatePostHttpResponse(loginUrl, parameters, null, null, Encoding.UTF8, cookies);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Connection Failed!\n"+e.Message);
                lastResultCode = CONNECTIION_RESULT_CODE.NETWORK_ERROR;
                return false;
            }


            //将POST返回HEADER打印到CONSOLE
            Console.WriteLine(response.Headers);
                        
            string freeUrl   = "https://its.pku.edu.cn/netportal/ipgwopen";
            string globalUrl = "https://its.pku.edu.cn/netportal/ipgwopenall";
            string disUrl    = "https://its.pku.edu.cn/netportal/ipgwclose";
            string disAllUrl = "https://its.pku.edu.cn/netportal/ipgwcloseall";
            string openUrl;
            switch (option)
            {
                case FREE:
                    openUrl = freeUrl; break;
                case GLOBAL:
                    openUrl = globalUrl; break;
                case DISCONNECT:
                    openUrl = disUrl; break;
                case DIS_ALL:
                    openUrl = disAllUrl; break;
                default:
                    return false;
            }

            //将登陆得到的cookie写到控制台上，并且检查是否登陆成功，
            //登陆成功时，服务器返回的cookie有一项名为"loginuid"。
            foreach (Cookie cookie in response.Cookies)
            {
                Console.WriteLine("cookie:"+cookie);
                if (cookie.Name == "loginuid")
                {
                    loggedIn = true;
                }
            }
            //如果未登录成果返回错误提示
            if (!loggedIn)
            {
                //MessageBox.Show("Password or username incorrect!");
                lastResultCode = CONNECTIION_RESULT_CODE.AUTHENTAION_ERROR;
                return false;
            }
            response.Close();
            
            //提交联网或断网GET请求
            response = WebRequestFunction.CreateGetHttpResponse(openUrl, null, null, response.Cookies);
            Console.WriteLine(response.Cookies);
            
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string ret = sr.ReadToEnd();
            Console.WriteLine(ret);
            sr.Close();
            response.Close();
            
            string[] substr = ret.Split(new char[] { '<','>' });
            foreach (var __s in substr)
            {
                if (__s.Contains("小时")&&(!__s.Contains("包")))
                    balance_hours = __s;
            }
            
            Console.WriteLine(balance_hours);

            if (!ret.Contains("网络连接成功"))
            {
                if (!(option == DISCONNECT || option == DIS_ALL))
                {
                    //MessageBox.Show("联网失败");
                    lastResultCode = CONNECTIION_RESULT_CODE.FAILED;
                    return false;
                }
            }

            bool isGlobal = false;
            if (ret.Contains("收费地址"))
            {
                isGlobal = true;
            }

            /*
            string url = "https://its.pku.edu.cn/cas/login";
            string ret = AUTO_IPGW.Program.PostWebRequest(url, Encoding.UTF8);
            //MessageBox.Show(ret);*/

            
            //FileStream MyFileStream1 = new FileStream(@"output.html", FileMode.Create);
            //StreamWriter sw = new StreamWriter(MyFileStream1);
            //sw.Write(ret);
            //关闭StreamWriter 
            //sw.Flush();
            //sw.Close();
            //关闭FileStream
            //MyFileStream1.Flush();
            //MyFileStream1.Close();
            

            //弹出相应提示
            switch (option)
            {
                case FREE:
                    //MessageBox.Show("连接免费网成功!"); break;
                    lastResultCode = CONNECTIION_RESULT_CODE.FREE_SUCCESS; break;
                case GLOBAL:
                    if (isGlobal)
                    {
                        //MessageBox.Show("连接收费网成功!\n本月剩余："+balance_hours); break;
                        lastResultCode = CONNECTIION_RESULT_CODE.GLOBAL_SUCCESS;
                        parseCode[CONNECTIION_RESULT_CODE.GLOBAL_SUCCESS] = ("收费网连接成功\n本月剩余："+ balance_hours); 
                        break;
                    }
                    else
                    {
                        //MessageBox.Show("连接成功!\n但收费网连接失败!"); break;
                        lastResultCode = CONNECTIION_RESULT_CODE.FREE_SUCCESS;
                        break;
                    }
                case DISCONNECT:
                    //MessageBox.Show("断开成功!");break;
                    lastResultCode = CONNECTIION_RESULT_CODE.DISCONNECT_SUCCESS;
                    break;
                case DIS_ALL:
                    //MessageBox.Show("断开所有链接成功!"); break;
                    lastResultCode = CONNECTIION_RESULT_CODE.DISCONNECT_SUCCESS;
                    break;
                default:
                    lastResultCode = CONNECTIION_RESULT_CODE.FAILED;
                    return false;
            }
            return true;
        }

        static public bool saveUserInfo(string username, string password)
        {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey software = key.CreateSubKey("software\\AUTO_IPGW");
            software.SetValue("username",Encrypt(username));
            software.SetValue("password",Encrypt(password));
            return true;
        }

        static public bool readUserInfo(ref string username, ref string password)
        {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey software;
            try
            {
                software = key.OpenSubKey("software\\AUTO_IPGW", true);
                username = Decrypt(software.GetValue("username").ToString());
                password = Decrypt(software.GetValue("password").ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }



        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)  
        {  
            return true; //总是接受  
        }

        //local data encrption key
        static private string __key__ = "A39CN289A91JF8VN478XH9183JN59Z02";

        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(__key__);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string toDecrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(__key__);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            byte[] resultArray;
            try
            {
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception ex)
            {
                return "";
            }
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            

            Application.Run(new MainForm());
           

        }
        
    }
}
