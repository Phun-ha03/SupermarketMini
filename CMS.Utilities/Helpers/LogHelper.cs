/*
    Author: Vu.The
    Email: thew0102@gmail.com
    Date: December 18, 2021
 */

using System;
using System.IO;

namespace CMS.Utilities.Helpers
{
    public class LogHelper
    {
        public static void writeLog(string logContent, string methodName)
        {
            //get filename
            string currentDate = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\Logs\\" + currentDate + "Log.txt";
            string strLogString;
            strLogString = "\r\n";
            strLogString += "===========  " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + methodName + "  ==============" + "\r\n";
            strLogString += logContent + "\r\n";
            strLogString += "===================================================================";
            try
            {
                System.Text.Encoding charset = System.Text.Encoding.GetEncoding(65001); //"UTF-8"
                if (!File.Exists(strFileName))
                {
                    FileStream oFile;
                    oFile = File.Create(strFileName);
                    StreamWriter oReader = new StreamWriter(oFile, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                    oFile.Close();
                }
                else
                {
                    // Append text in file when file exitsed
                    FileStream oFile1 = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (oFile1.Length > 1048576)
                    {
                        oFile1.Close();
                        File.Delete(strFileName);
                    }
                    else
                    {
                        oFile1.Close();
                    }
                    StreamWriter oReader = new StreamWriter(strFileName, true, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void writeGetDocChunksRequestLog(string logContent, string methodName)
        {
            //get filename
            string currentDate = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\DocChunksRequestLogs\\" + currentDate + "Log.txt";
            string strLogString;
            strLogString = "\r\n";
            strLogString += "===========  " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + methodName + "  ==============" + "\r\n";
            strLogString += logContent + "\r\n";
            strLogString += "===================================================================";
            try
            {
                System.Text.Encoding charset = System.Text.Encoding.GetEncoding(65001); //"UTF-8"
                if (!File.Exists(strFileName))
                {
                    FileStream oFile;
                    oFile = File.Create(strFileName);
                    StreamWriter oReader = new StreamWriter(oFile, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                    oFile.Close();
                }
                else
                {
                    // Append text in file when file exitsed
                    FileStream oFile1 = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (oFile1.Length > 1048576)
                    {
                        oFile1.Close();
                        File.Delete(strFileName);
                    }
                    else
                    {
                        oFile1.Close();
                    }
                    StreamWriter oReader = new StreamWriter(strFileName, true, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void writeJobErrorLog(string logContent, string methodName)
        {
            //get filename
            string currentDate = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\JobErrorLogs\\" + currentDate + "Log.txt";
            string strLogString;
            strLogString = "\r\n";
            strLogString += "===========  " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + methodName + "  ==============" + "\r\n";
            strLogString += logContent + "\r\n";
            strLogString += "===================================================================";
            try
            {
                System.Text.Encoding charset = System.Text.Encoding.GetEncoding(65001); //"UTF-8"
                if (!File.Exists(strFileName))
                {
                    FileStream oFile;
                    oFile = File.Create(strFileName);
                    StreamWriter oReader = new StreamWriter(oFile, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                    oFile.Close();
                }
                else
                {
                    // Append text in file when file exitsed
                    FileStream oFile1 = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (oFile1.Length > 1048576)
                    {
                        oFile1.Close();
                        File.Delete(strFileName);
                    }
                    else
                    {
                        oFile1.Close();
                    }
                    StreamWriter oReader = new StreamWriter(strFileName, true, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void writeBackgroundJobErrorLog(string logContent, string methodName)
        {
            //get filename
            string currentDate = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\BackgroundJobErrorLogs\\" + currentDate + "Log.txt";
            string strLogString;
            strLogString = "\r\n";
            strLogString += "===========  " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + methodName + "  ==============" + "\r\n";
            strLogString += logContent + "\r\n";
            strLogString += "===================================================================";
            try
            {
                System.Text.Encoding charset = System.Text.Encoding.GetEncoding(65001); //"UTF-8"
                if (!File.Exists(strFileName))
                {
                    FileStream oFile;
                    oFile = File.Create(strFileName);
                    StreamWriter oReader = new StreamWriter(oFile, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                    oFile.Close();
                }
                else
                {
                    // Append text in file when file exitsed
                    FileStream oFile1 = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (oFile1.Length > 1048576)
                    {
                        oFile1.Close();
                        File.Delete(strFileName);
                    }
                    else
                    {
                        oFile1.Close();
                    }
                    StreamWriter oReader = new StreamWriter(strFileName, true, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void writeRelevantQuestionLog(string logContent, string methodName)
        {
            //get filename
            string currentDate = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\RelevantQuestionLogs\\" + currentDate + "Log.txt";
            string strLogString;
            strLogString = "\r\n";
            strLogString += "===========  " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + methodName + "  ==============" + "\r\n";
            strLogString += logContent + "\r\n";
            strLogString += "===================================================================";
            try
            {
                System.Text.Encoding charset = System.Text.Encoding.GetEncoding(65001); //"UTF-8"
                if (!File.Exists(strFileName))
                {
                    FileStream oFile;
                    oFile = File.Create(strFileName);
                    StreamWriter oReader = new StreamWriter(oFile, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                    oFile.Close();
                }
                else
                {
                    // Append text in file when file exitsed
                    FileStream oFile1 = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (oFile1.Length > 1048576)
                    {
                        oFile1.Close();
                        File.Delete(strFileName);
                    }
                    else
                    {
                        oFile1.Close();
                    }
                    StreamWriter oReader = new StreamWriter(strFileName, true, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void writeCheckQuestionSemanticResultLog(string logContent, string methodName)
        {
            //get filename
            string currentDate = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\CheckQuestionSemanticLogs\\" + currentDate + "Log.txt";
            string strLogString;
            strLogString = "\r\n";
            strLogString += "===========  " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + methodName + "  ==============" + "\r\n";
            strLogString += logContent + "\r\n";
            strLogString += "===================================================================";
            try
            {
                System.Text.Encoding charset = System.Text.Encoding.GetEncoding(65001); //"UTF-8"
                if (!File.Exists(strFileName))
                {
                    FileStream oFile;
                    oFile = File.Create(strFileName);
                    StreamWriter oReader = new StreamWriter(oFile, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                    oFile.Close();
                }
                else
                {
                    // Append text in file when file exitsed
                    FileStream oFile1 = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (oFile1.Length > 1048576)
                    {
                        oFile1.Close();
                        File.Delete(strFileName);
                    }
                    else
                    {
                        oFile1.Close();
                    }
                    StreamWriter oReader = new StreamWriter(strFileName, true, charset);
                    oReader.WriteLine(strLogString);
                    oReader.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

       
    }
}
