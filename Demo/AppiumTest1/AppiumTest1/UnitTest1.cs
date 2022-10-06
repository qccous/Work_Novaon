using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Interfaces;
using System;
using KAutoHelper;
using System.IO;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.VisualBasic;
using OpenQA.Selenium.Interactions;
using System.Xml.Linq;
using OpenQA.Selenium.Appium.MultiTouch;
using System.Drawing;
using System.Threading;
using System.Text.Unicode;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using System.Collections;
using Image = System.Drawing.Image;
using Tesseract;
using IronOcr;
using DocumentFormat.OpenXml.Wordprocessing;
using Patagames.Ocr;

namespace AppiumTest1
{
    public class Tests
    {
        #region Declare
        private AppiumDriver<AndroidElement> _driver;
        private Process process = new Process();
        private ProcessStartInfo info = new ProcessStartInfo();
        string uid = "";
        string access_token = "";
        string session_cookies_string = "";
        #endregion

        [SetUp]
        public void Setup()
        {
            var appPath = @"E:\1_Work\Work_Novaon\Facebook.apk";
            //Platform, device, app
            var driverOption = new AppiumOptions();
            try
            {
                driverOption.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
                driverOption.AddAdditionalCapability(MobileCapabilityType.DeviceName, "emulator-5554");
                driverOption.AddAdditionalCapability(MobileCapabilityType.App, appPath);
                _driver = new AndroidDriver<AndroidElement>(new Uri("http://localhost:4723/wd/hub"), driverOption);
                var contexts = ((IContextAware)_driver).Contexts;
string webviewContext = null;
            for (var i = 0; i < contexts.Count; i++)
            {
                Console.WriteLine(contexts[i]);
                if (contexts[i].Contains("WEBVIEW"))
                {
                    webviewContext = contexts[i];
                    break;
                }
            }
            ((IContextAware)_driver).Context = webviewContext;
            }
            catch (Exception)
            {

                throw;
            }

            
            Login();
        }
        public void Login()
        {
            try
            {
                #region Login
                try
                {
                    _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    _driver.FindElementByXPath("//android.widget.EditText[@content-desc=\"Tên người dùng\"]").SendKeys("kuyet178@gmail.com");
                    _driver.FindElementByXPath("//android.widget.EditText[@content-desc=\"Mật khẩu\"]").SendKeys("qccous13");
                    _driver.FindElementByAccessibilityId("Đăng nhập").Click();
                    _driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.Button[1]").Click();
                    _driver.FindElementByXPath("//android.view.ViewGroup[@content-desc=\"Từ chối\"]").Click();
                    _driver.FindElementByXPath("//android.widget.Button[@content-desc=\"Tìm kiếm trên Facebook\"]").Click();
                    _driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup/androidx.recyclerview.widget.RecyclerView/android.view.ViewGroup[3]").Click();
                }
                catch (Exception)
                {

                    throw;
                }

                //    _driver.FindElementByXPath("//android.view.ViewGroup[@content-desc=\"Đi tới trang cá nhân\"]").Click();
                #endregion
                //Screenshot();
                #region ScrollToFindMenuButton
                var locate = new Point();
                Size screenResolution = _driver.Manage().Window.Size;
                int countMenu = 1;
                ArrayList statusArrLst = new ArrayList();
                while (countMenu > 0)
                {
                    locate.X = 0;
                    locate.Y = 0;
                    while (locate.X == 0)
                    {
                        PressAndMove(500, 1000, 1080, 272);
                        locate = GetBound("//android.view.ViewGroup[@content-desc=\"Menu bài viết\"]");
                        var searchLocate = GetBound("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout");
                        if (locate.Y > 0)
                        {
                            PressAndMove(0, locate.Y, 0, searchLocate.Y + 120);
                            LongPress(searchLocate.X, 331);
                            string status = GetStatus();

                            if (String.IsNullOrEmpty(status))
                            {
                                break;
                            }
                            statusArrLst.Add(status);
                            //TakeScreenshot(_driver, new Point(0, screenResolution.Height), new Size(screenResolution.Width, screenResolution.Height));
                            TakeScreenshot(_driver, new Point(0, screenResolution.Height - locate.Y), new Size(screenResolution.Width, locate.Y));
                            // ExtractTextFromIMG();
                            countMenu++;
                            break;
                        }
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetStatus()
        {
            try
            {
                _driver.FindElementByXPath("//android.widget.Button[@content-desc=\"Sao chép văn bản\"]/android.widget.TextView").Click();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
            //var regex = new Regex("data=(.*?)");
            var regex = new Regex("data=\"(.*?)\"");
            string raw_Status = RunAdbCommand(" shell am broadcast -a clipper.get");
            string proc_Status = string.Join(" ", Regex.Split(raw_Status, @"(?:\r\n|\n|\r|\\)"));
            if (regex.Match(proc_Status).Success)
            {
                string result = regex.Match(proc_Status).Groups[1].Value;
                return result;
            }
            return null;
        }
        public void TakeScreenshot(IWebDriver driver, Point point, Size size)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".jpg";
                Byte[] byteArray = ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;
                Bitmap screenshot = new Bitmap(new MemoryStream(byteArray));
                Rectangle croppedImage = new Rectangle(point, size);
                screenshot = screenshot.Clone(croppedImage, screenshot.PixelFormat);
                screenshot.Save(@"E:\1_Work\Work_Novaon\IMG\" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                var result = ExtractTextFromIMG(fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string ExtractTextFromIMG(string filename)
        {

            var path = @"./tessdata";
            //your sample image location
            var sourceFilePath = @"E:\1_Work\Work_Novaon\IMG\" + filename;
            using (var engine = new TesseractEngine(path, "eng"))
            {
                engine.SetVariable("user_defined_dpi", "70"); //set dpi for supressing warning
                using (var img = Pix.LoadFromFile(sourceFilePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();

                        return text;
                    }
                }
            }
        }
        public static string UnicodeToUTF8(string strFrom)
        {
            byte[] bytSrc;
            byte[] bytDestination;
            string strTo = String.Empty;

            bytSrc = Encoding.Unicode.GetBytes(strFrom);
            bytDestination = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, bytSrc);
            strTo = Encoding.ASCII.GetString(bytDestination);

            return strTo;
        }
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public Point GetBound(string xpath)
        {
            try
            {
                //C1
                var locate = _driver.FindElementByXPath(xpath).Location;
                return locate;
                //C2
                //var pageSource = _driver.PageSource;
                //var xml = new XmlDocument();
                //xml.LoadXml(pageSource);
                //var root = xml.DocumentElement;
                //var nodes = root.SelectSingleNode(xpath);
            }
            catch (Exception)
            {
                return new Point();
            }

        }
        public void LongPress(int x, int y)
        {
            var touchAction = new TouchAction(_driver);
            touchAction.LongPress(x, y);
            touchAction.Release();
            touchAction.Perform();
        }
        public void PressAndMove(int x1, int y1, int x2, int y2)
        {
            var touchAction = new TouchAction(_driver);
            touchAction.LongPress(x1, y1);

            touchAction.MoveTo(x2, y2);

            touchAction.Release();

            touchAction.Perform();
        }
        public void ScrollingDownByAdb(int start_x, int start_y, int end_x, int end_y)
        {
            try
            {
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.FileName = @"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe";
                info.Arguments = string.Format(" shell input swipe {0} {1} {2} {3}", start_x, start_y, end_x, end_y);
                process.StartInfo = info;
                process.Start();
                process.WaitForExit();
                string error = process.StandardError.ReadToEnd().ToString();
                string output = process.StandardOutput.ReadToEnd().ToString();
                process.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public string RunAdbCommand(string adbCommand)
        {
            var result = "";
            try
            {
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.FileName = @"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe";
                info.Arguments = adbCommand;
                process.StartInfo = info;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                process.Start();
                process.WaitForExit();
                string error = process.StandardError.ReadToEnd().ToString();
                result = process.StandardOutput.ReadToEnd().ToString();
                process.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
        #region authentication
        public void getAuthentication()
        {
            string text = File.ReadAllText(@"E:\1_Work\Work_Novaon\File\com.facebook.katana\authentication");
            //string pageSource = _driver.PageSource;
            //Đọc file authencation,lấy ra  uuid, lấy ra access_token, sesstion stirng -> lưu vào file  txt trên máy
            //RunAdbCommand("root");
            //RunAdbCommand(@"pull /data/data/com.facebook.katana/app_light_prefs/com.facebook.katana E:\1_Work\Work_Novaon\File");
            //RunAdbCommand(@"pull /data/data/com.facebook.katana/databases E:\1_Work\Work_Novaon\File");
            //RunAdbCommand(@"pull /data/data/com.facebook.katana/app_sessionless_gatekeepers E:\1_Work\Work_Novaon\File");
            //Pull, push access_code, uid, session
            //getAuthentication();

            //get uid
            var regex = new Regex("c_user\",\"value\":\"([0-9]+)\"");
            if (regex.Match(text).Success)
            {
                uid = regex.Match(text).Groups[1].Value;
                CreateFolder(uid);
                CreateAuthenticationFolder(uid, uid);
                CreateFile(uid, uid, "uid", uid);
                PushFile(string.Format("data/PushData/{0}/uid", uid), string.Format(@"E:\1_Work\Work_Novaon\File\{0}\authentication_{1}\uid", uid, uid));
                PushFile(string.Format("data/PushData/{0}/authentication", uid), string.Format(@"E:\1_Work\Work_Novaon\File\com.facebook.katana\authentication", uid, uid));
            }
            //get access token
            regex = new Regex("(access_token)(.*?)(uid)");
            var regexLetter = new Regex("[a-zA-Z ]+");
            if (regex.Match(text).Success)
            {
                string token = regex.Match(text).Groups[2].Value.Trim('\0');
                string filterToken = regexLetter.Replace(token, "");
                access_token = Regex.Replace(filterToken, @"[^0-9]+", "");
                CreateFile(uid, uid, "access_token", access_token);
                PushFile(string.Format("data/PushData/{0}/access_token", uid), string.Format(@"E:\1_Work\Work_Novaon\File\{0}\authentication_{1}\access_token", uid, uid));
            }
            //get session
            regex = new Regex("(session_cookies_string)(.*?)(session_key)");
            if (regex.Match(text).Success)
            {
                session_cookies_string = regex.Match(text).Groups[2].Value.Trim('\0').Replace("\u0003l", "").Replace("\u0005", "");
                CreateFile(uid, uid, "session_cookies_string", session_cookies_string);
                PushFile(string.Format("data/PushData/{0}/session_cookies_string", uid), string.Format(@"E:\1_Work\Work_Novaon\File\{0}\authentication_{1}\session_cookies_string", uid, uid));
            }
        }
        public void PushFile(string folderPathAndroid, string filePath)
        {
            _driver.PushFile(folderPathAndroid, new FileInfo(filePath));
        }
        public void CreateFile(string uid, string folderName, string fileName, string content)
        {
            TextWriter tw = new StreamWriter(string.Format(@"E:\1_Work\Work_Novaon\File\{0}\authentication_{1}\{2}", uid, folderName, fileName), true);
            tw.WriteLine(content);
            tw.Close();
        }
        public void CreateFolder(string folderName)
        {
            string folderPath = string.Format(@"E:\1_Work\Work_Novaon\File\{0}", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine(folderPath);
            }
        }
        public void CreateAuthenticationFolder(string uid, string folderName)
        {
            string folderPath = string.Format(@"E:\1_Work\Work_Novaon\File\{0}\authentication_{1}", uid, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine(folderPath);
            }
        }
        #endregion
        [Test]
        public void Test1()
        {

            Assert.Pass();
        }
    }
}