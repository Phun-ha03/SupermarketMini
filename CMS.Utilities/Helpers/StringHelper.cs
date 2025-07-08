/*
    Author: Vu.The
    Email: thew0102@gmail.com
    Date: December 18, 2021
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using HtmlAgilityPack;

namespace CMS.Utilities.Helpers
{
    public class StringHelper
    {
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡö",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữü",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static bool isNumeric(string s)
        {
            try
            {
                Convert.ToInt32(s);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string RemoveSignature(string input)
        {
            try
            {
                if (input == null)
                {
                    return "";
                }
                input = input.Replace("\"", "");
                for (int i = 1; i < VietnameseSigns.Length; i++)
                {
                    for (int j = 0; j < VietnameseSigns[i].Length; j++)
                        input = input.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
            }
            return input;
        }
        public static string removeSpacer(string input)
        {
            string result = input.Replace(":--", "");
            return result;
        }
        public static string removeSpecialSignatures(string inputString)
        {
            string retVal = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(inputString))
                {
                    for (int i = 0; i < inputString.Length; i++)
                    {
                        char c = inputString[i];
                        if (c >= 'a' && c <= 'z'
                            || c >= 'A' && c <= 'Z'
                            || c >= '0' && c <= '9'
                            || c == ' ')
                        {
                            retVal += c.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
            }
            return retVal;
        }

        public static string RemoveSignatureForURL(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return input;
                }
                input = RemoveSignature(input);
                input = input.ToLower().Trim();
                input = removeSpecialSignatures(input);
                while (input.Contains("  "))
                {
                    input = input.Replace("  ", " ");
                }
                input = input.Replace(" ", "-");
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
            }
            return input;
        }

        public static string GetFullnamePrefix(string fullname)
        {
            string retVal = "N";
            try
            {
                if (string.IsNullOrEmpty(fullname)) return retVal;

                fullname = fullname.Trim();
                while (fullname.Contains("  "))
                {
                    fullname = fullname.Replace("  ", " ");
                }

                if (fullname.Contains(" "))
                {
                    var splits = fullname.Split(' ');
                    retVal = splits[0].Substring(0, 1) + splits[splits.Length - 1].Substring(0, 1);
                }
                else
                {
                    if (fullname.Length > 2)
                    {
                        retVal = fullname.Substring(0, 2);
                    }
                    else
                    {
                        retVal = fullname;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
            }
            return retVal.ToUpper();
        }

        public static string GetLead(string text, int maxLength)
        {
            string retVal = string.IsNullOrEmpty(text) ? "" : text;
            try
            {
                if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                {
                    return retVal;
                }

                retVal = text.Substring(0, maxLength);

                if(!retVal.EndsWith(" ") && retVal.Contains(" "))
                {
                    //abc 1
                    retVal = retVal.Substring(0, retVal.LastIndexOf(" ")).Trim();
                }
                else
                {
                    retVal = retVal.Trim();
                }
                retVal += "...";
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
            }
            return retVal;
        }

        public static int getFileType(string filePath)
        {
            if (filePath.Contains(".mp4"))
            {
                return 6;
            }
            else if (filePath.Contains(".mp3"))
            {
                return 5;
            }
            else if (filePath.Contains(".png") || filePath.Contains(".jpg") || filePath.Contains(".jpeg") || filePath.Contains(".gif") || filePath.Contains(".bitmap"))
            {
                return 7;
            }
            else if (filePath.Contains(".pptx") || filePath.Contains(".ppt"))
            {
                return 4;
            }
            else if (filePath.Contains(".xlsx") || filePath.Contains(".xls"))
            {
                return 2;
            }
            else if (filePath.Contains(".pdf"))
            {
                return 3;
            }
            else if (filePath.Contains(".docx") || filePath.Contains(".doc"))
            {
                return 2;
            }
            else if (filePath.Contains(".zip") || filePath.Contains(".rar"))
            {
                return 8;
            }

            return 0;
        }

        

        public static string MakeUrl(string input)
        {
            var work = input.Split(' ');
            var output = new StringBuilder(2 * input.Length);
            for (int i = 0; i < work.Length; i++)
            {
                var element = work[i];
                if (element.StartsWith("http://"))
                {
                    var site = element.Replace("http://", "");
                    element = "<a href = '" + element + "'>" + site + "</a>";
                }

                if (element.StartsWith("https://"))
                {
                    var site = element.Replace("https://", "");
                    element = "<a href = '" + element + "'>" + site + "</a>";
                }
                output.Append(element + " ");
            }
            return output.ToString();
        }
        public static bool CheckDate(DateTime input)
        {
            if (DateTime.Now.Subtract(input).TotalDays > 14) return true;
            return false;
        }

        public static string ExactValue(string str, string objName, char delimiter)
        {
            string[] codes = str.Split(delimiter);
            string result = "";
            int i = 0;
            while (i < codes.Length)
            {
                if (codes[i].Contains(objName))
                {
                    result = codes[i].Replace(objName, "");
                    result = result.Replace("=", "").Trim();
                    i = codes.Length;
                }
                i++;
            }
            return result;
        }

        public static string InjectionString(string str)
        {
            try
            {
                string text = KillChar(str).Replace("'", "''");
                return str;
            }
            catch
            {
                return "";
            }
        }

        private static string KillChar(string strInput)
        {
            try
            {
                string[] array = new string[7] { "select", "drop", ";", "--", "insert", "delete", "xp_" };
                string text = strInput.Trim();
                for (int i = 0; i < array.Length; i++)
                {
                    text = text.Replace(array[i], "");
                }

                return text;
            }
            catch
            {
                return "";
            }
        }

        public static DateTime ConvertToDateTime(string strDateTime)
        {
            string name = "vi-VN";
            try
            {
                return DateTime.Parse(strDateTime, CultureInfo.CreateSpecificCulture(name));
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static string stripHtml(string html)
        {
            return stripHtmlVs2(html);
           
        }

        public static string stripHtmlVs2(string html)
        {
            string retVal = string.Empty;
            List<string> tables = new List<string>();
            List<string> tableRaws = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(html))
                {
                    return retVal;
                }

                var endTable = html.IndexOf("</table>");
                var startTable = 0;
                var counter = 0;
                while (endTable >= 0)
                {
                    var preStr = html.Substring(0, endTable + "</table>".Length);
                    var tailStr = html.Substring(endTable + "</table>".Length);

                    startTable = preStr.LastIndexOf("<table");

                    var tableStr = preStr.Substring(startTable);
                    preStr = preStr.Substring(0, startTable);

                    counter++;
                    html = preStr + "[TABLECONTENT_" + counter + "]" + tailStr;

                    endTable = html.IndexOf("</table>");

                    tables.Add(tableStr);
                }

                html = Regex.Replace(html, "<(div|DIV)[^>]*>", "\n<div>");
                html = Regex.Replace(html, "</(div|DIV)>", "</div>\n");
                html = Regex.Replace(html, "<(p|P)[^>]*>", "\n<p>");
                html = Regex.Replace(html, "</(p|P)>", "</p>\n");
                html = Regex.Replace(html, "<(br|BR)[^>]*>", "\n<br/>");

                html = Regex.Replace(html, "<(h|H)[0-9][^>]*>", "\n<h6>");
                html = Regex.Replace(html, "</(h|H)[0-9]>", "</h6>\n");

                var document = new HtmlDocument();
                html = "<html><head><meta charset=utf-8></head><body>" + html + "</body></html>";
                //html = HttpUtility.UrlDecode(html);

                document.LoadHtml(html);

                retVal = document.DocumentNode.InnerText;

                if (!string.IsNullOrEmpty(retVal))
                {
                    var retValSplit = retVal.Split(new string[] { "\r\n", "\\r\\n", "\n", "\\n" }, StringSplitOptions.None);
                    
                    retVal = string.Empty;

                    for(int i = 0; i < retValSplit.Length; i++)
                    {
                        var line = retValSplit[i].Replace("\n", " ").Replace("\r", " ")
                            .Replace("\\n", " ").Replace("\\r", " ").Trim();
                        if (!string.IsNullOrEmpty(line))
                        {
                            if (!string.IsNullOrEmpty(retVal))
                            {
                                retVal += Environment.NewLine;
                            }
                            retVal += line;
                        }
                    }
                }

                if (tables.Count > 0)
                {
                    for (int i = 0; i < tables.Count; i++)
                    {
                        html = tables[i];

                        html = splitTableCellVs2(html);

                        html = Regex.Replace(html, "<(div|DIV)[^>]*>", " <div>");
                        html = Regex.Replace(html, "</(div|DIV)>", "</div> ");
                        html = Regex.Replace(html, "<(p|P)[^>]*>", " <p>");
                        html = Regex.Replace(html, "</(p|P)>", "</p> ");
                        html = Regex.Replace(html, "<(br|BR)[^>]*>", " <br/>");

                        html = Regex.Replace(html, "<(h|H)[0-9][^>]*>", " <h6>");
                        html = Regex.Replace(html, "</(h|H)[0-9]>", "</h6> ");

                        html = Regex.Replace(html, "<(tr|TR)[^>]*>", "\n<tr>");
                        html = Regex.Replace(html, "</(tr|TR)>", "</tr>\n");
                        html = Regex.Replace(html, "</(td|TD)>", "\t</td>");

                        document = new HtmlDocument();
                        html = "<html><head><meta charset=utf-8></head><body>" + html + "</body></html>";
                        document.LoadHtml(html);
                        tableRaws.Add(document.DocumentNode.InnerText);
                    }

                    for (int i = tables.Count - 1; i >= 0; i--)
                    {
                        retVal = retVal.Replace("[TABLECONTENT_" + (i + 1) + "]", "\n" + tableRaws[i] + "\n");
                    }
                }

                retVal = retVal.Replace("&nbsp;", " ");
                retVal = Regex.Replace(retVal, "\n{2,100}", "\n");
                retVal = Regex.Replace(retVal, ";{2,100}", ";");
                retVal = Regex.Replace(retVal, " {2,100}", ";");
                retVal = retVal.Trim().Trim('\n').Trim();

                var split = retVal.Split('\n');
                retVal = "";
                for (int i = 0; i < split.Length; i++)
                {
                    var line = split[i];
                    //line = line.Trim().Trim('\t').Trim('\n');
                    if (line.Length > 0)
                    {
                        if (retVal.Length > 0)
                        {
                            retVal += "\r\n" + line;
                        }
                        else
                        {
                            retVal += line;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString() + Environment.NewLine + Environment.NewLine + html, new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw ex;
            }
            return retVal;
        }

        public static string splitTableCell(string html)
        {
            string retVal = string.Empty;
            List<string> trList = new List<string>();
            List<List<string>> tdList = new List<List<string>>();

            html = html.Replace("<th", "<td").Replace("</th>", "</td>");
            var startTr = html.IndexOf("<tr");
            while (startTr >= 0)
            {
                var endTr = html.IndexOf("</tr>", startTr);

                var trStr = html.Substring(startTr, endTr - startTr + "</tr>".Length);
                var preStr = html.Substring(0, startTr);
                var tailStr = html.Substring(endTr + "</tr>".Length);

                html = preStr + tailStr;
                trList.Add(trStr);

                startTr = html.IndexOf("<tr");


                List<string> tds = new List<string>();

                var startTd = trStr.IndexOf("<td");
                while (startTd >= 0)
                {
                    var endTd = trStr.IndexOf("</td>", startTd);

                    var tdStr = trStr.Substring(startTd, endTd - startTd + "</td>".Length);
                    preStr = trStr.Substring(0, startTd);
                    tailStr = trStr.Substring(endTd + "</td>".Length);

                    trStr = preStr + tailStr;

                    tds.Add(tdStr);

                    startTd = trStr.IndexOf("<td");
                }
                tdList.Add(tds);
            }
            var i = 0;
            var j = 0;
            while (i < tdList.Count)
            {
                j = 0;
                while (j < tdList[i].Count)
                {
                    var tdStr = tdList[i][j];

                    while (tdStr.Contains(" ="))
                    {
                        tdStr = tdStr.Replace(" =", "=");
                    }
                    while (tdStr.Contains("= "))
                    {
                        tdStr = tdStr.Replace("= ", "=");
                    }
                    var colspan = 0;
                    var rowspan = 0;
                    var startIndex = tdStr.IndexOf("colspan");
                    if (startIndex >= 0)
                    {
                        var endIndex = tdStr.IndexOf(" ", startIndex);
                        var endIndexTmp = tdStr.IndexOf(">", startIndex);
                        if (endIndex < 0 || endIndexTmp < endIndex)
                        {
                            endIndex = endIndexTmp;
                        }
                        var colspanStr = tdStr.Substring(startIndex, endIndex - startIndex);
                        var preStr = tdStr.Substring(0, startIndex);
                        var tailStr = tdStr.Substring(endIndex);
                        tdStr = preStr + tailStr;
                        colspan = stringToInt(colspanStr.Split('=')[1]);
                    }
                    startIndex = tdStr.IndexOf("rowspan");
                    if (startIndex >= 0)
                    {
                        var endIndex = tdStr.IndexOf(" ", startIndex);
                        var endIndexTmp = tdStr.IndexOf(">", startIndex);
                        if (endIndex < 0 || endIndexTmp < endIndex)
                        {
                            endIndex = endIndexTmp;
                        }
                        var rowspanStr = tdStr.Substring(startIndex, endIndex - startIndex);
                        var preStr = tdStr.Substring(0, startIndex);
                        var tailStr = tdStr.Substring(endIndex);
                        tdStr = preStr + tailStr;
                        rowspan = stringToInt(rowspanStr.Split('=')[1]);
                    }
                    tdList[i][j] = tdStr;
                    if (colspan > 0)
                    {
                        for (int ii = 1; ii < colspan; ii++)
                        {
                            tdList[i].Insert(j + ii, tdStr);
                        }
                    }
                    if (rowspan > 0)
                    {
                        for (int ii = 1; ii < rowspan; ii++)
                        {
                            tdList[i + ii].Insert(j, tdStr);
                        }
                    }
                    j++;
                }
                i++;
            }

            retVal = "<table>";
            for (i = 0; i < tdList.Count; i++)
            {
                retVal += "<tr>";
                for (j = 0; j < tdList[i].Count; j++)
                {
                    retVal += tdList[i][j];
                }
                retVal += "</tr>";
            }
            retVal += "</table>";

            return retVal;
        }

        public static string splitTableCellVs2(string html)
        {
            string retVal = string.Empty;
            List<string> trList = new List<string>();
            List<List<string>> tdList = new List<List<string>>();

            try
            {
                html = html.Replace("<th", "<td").Replace("</th>", "</td>");
                var startTr = html.IndexOf("<tr");
                while (startTr >= 0)
                {
                    var endTr = html.IndexOf("</tr>", startTr);

                    var trStr = html.Substring(startTr, endTr - startTr + "</tr>".Length);
                    var preStr = html.Substring(0, startTr);
                    var tailStr = html.Substring(endTr + "</tr>".Length);

                    html = preStr + tailStr;
                    trList.Add(trStr);

                    startTr = html.IndexOf("<tr");


                    List<string> tds = new List<string>();

                    var startTd = trStr.IndexOf("<td");
                    while (startTd >= 0)
                    {
                        var endTd = trStr.IndexOf("</td>", startTd);

                        var tdStr = trStr.Substring(startTd, endTd - startTd + "</td>".Length);
                        preStr = trStr.Substring(0, startTd);
                        tailStr = trStr.Substring(endTd + "</td>".Length);

                        trStr = preStr + tailStr;

                        tds.Add(tdStr);

                        startTd = trStr.IndexOf("<td");
                    }

                    if(tds.Count > 0)
                    {
                        tdList.Add(tds);
                    }
                }

                var i = 0;
                var j = 0;

                while (i < tdList.Count)
                {
                    j = 0;
                    while (j < tdList[i].Count)
                    {
                        var tdStr = tdList[i][j];

                        /*if (tdStr.Contains("<table") || tdStr.Contains("[TABLECONTENT_"))
                        {
                            j++;
                            continue;
                        }*/

                        while (tdStr.Contains(" ="))
                        {
                            tdStr = tdStr.Replace(" =", "=");
                        }
                        while (tdStr.Contains("= "))
                        {
                            tdStr = tdStr.Replace("= ", "=");
                        }

                        var colspan = 1;
                        var startIndex = tdStr.IndexOf("colspan");
                        if (startIndex >= 0)
                        {
                            var endIndex = tdStr.IndexOf(" ", startIndex);
                            var endIndexTmp = tdStr.IndexOf(">", startIndex);
                            if (endIndex < 0 || endIndexTmp < endIndex)
                            {
                                endIndex = endIndexTmp;
                            }
                            var colspanStr = tdStr.Substring(startIndex, endIndex - startIndex);
                            var preStr = tdStr.Substring(0, startIndex);
                            var tailStr = tdStr.Substring(endIndex);
                            tdStr = preStr + tailStr;
                            colspan = stringToInt(colspanStr.Split('=')[1]);
                        }

                        var rowspan = 1;
                        startIndex = tdStr.IndexOf("rowspan");
                        if (startIndex >= 0)
                        {
                            var endIndex = tdStr.IndexOf(" ", startIndex);
                            var endIndexTmp = tdStr.IndexOf(">", startIndex);
                            if (endIndex < 0 || endIndexTmp < endIndex)
                            {
                                endIndex = endIndexTmp;
                            }
                            var rowspanStr = tdStr.Substring(startIndex, endIndex - startIndex);
                            var preStr = tdStr.Substring(0, startIndex);
                            var tailStr = tdStr.Substring(endIndex);
                            tdStr = preStr + tailStr;
                            rowspan = stringToInt(rowspanStr.Split('=')[1]);
                        }

                        tdList[i][j] = tdStr;
                        if (colspan > 1)
                        {
                            for (int jj = 1; jj < colspan; jj++)
                            {
                                tdList[i].Insert(j + jj, tdStr);
                            }
                        }
                        if (rowspan > 1)
                        {
                            for (int ii = 1; ii < rowspan; ii++)
                            {
                                if(i + ii < tdList.Count)
                                {
                                    for (int jj = 0; jj < colspan; jj++)
                                    {
                                        if (tdList[i + ii].Count > j + jj)
                                        {
                                            tdList[i + ii].Insert(j + jj, tdStr);
                                        }
                                        else
                                        {
                                            tdList[i + ii].Add(tdStr);
                                        }
                                    }
                                }
                            }
                        }
                        j++;
                    }
                    i++;
                }


               


                retVal = "<table>";
                for (i = 0; i < tdList.Count; i++)
                {
                    retVal += "<tr>";
                    for (j = 0; j < tdList[i].Count; j++)
                    {
                        retVal += tdList[i][j];
                    }
                    retVal += "</tr>";
                }
                retVal += "</table>";

                return retVal;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString() + Environment.NewLine + Environment.NewLine + html, new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw ex;
            }
        }

        public static int stringToInt(string input)
        {
            if (string.IsNullOrEmpty(input)) return 0;

            var validNumber = "0123456789";
            var retVal = 0;
            var inputValid = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (validNumber.Contains(input[i]))
                {
                    inputValid += input[i];
                }
            }

            if (string.IsNullOrEmpty(inputValid)) return 0;

            retVal = Convert.ToInt32(inputValid);

            return retVal;
        }

        public static string accountProtected(string account, int protectCharsAmount, string replaceChar)
        {
            var retVal = account;

            if (string.IsNullOrEmpty(account))
            {
                return string.Empty;
            }

            if (retVal.Contains("@"))
            {
                var mailSplit = retVal.Split("@");
                var headOfMail = mailSplit[0];
                var tailOfMail = mailSplit[1];

                if(headOfMail.Length <= protectCharsAmount)
                {
                    protectCharsAmount = headOfMail.Length;
                    headOfMail = "";
                    for(int i = 0; i < protectCharsAmount; i++)
                    {
                        headOfMail += replaceChar;
                    }
                }
                else
                {
                    headOfMail = headOfMail.Substring(0, headOfMail.Length - protectCharsAmount);
                    for (int i = 0; i < protectCharsAmount; i++)
                    {
                        headOfMail += replaceChar;
                    }
                }

                retVal = headOfMail + "@" + tailOfMail;
            }
            else
            {
                if (retVal.Length <= protectCharsAmount)
                {
                    protectCharsAmount = retVal.Length;
                    retVal = "";
                    for (int i = 0; i < protectCharsAmount; i++)
                    {
                        retVal += replaceChar;
                    }
                }
                else
                {
                    retVal = retVal.Substring(0, retVal.Length - protectCharsAmount);
                    for (int i = 0; i < protectCharsAmount; i++)
                    {
                        retVal += replaceChar;
                    }
                }
            }

            return retVal;
        }

        public static string removeDoubleSpace(string input)
        {
            string retVal = input;

            if (string.IsNullOrEmpty(retVal))
            {
                return retVal;
            }

            while(retVal.Contains("  "))
            {
                retVal = retVal.Replace("  ", " ");
            }

            return retVal.Trim();
        }

        public static string RemoveLLMPromptInvalidChars(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = input.Replace("{", "{{").Replace("}", "}}");
            }
            
            return input;
        }

        public static int GetDocIdFromLVNDocUrl(string docUrl)
        {
            int retVal = 0;
            if(string.IsNullOrEmpty(docUrl) || !docUrl.Contains("-"))
            {
                return 0;
            }

            //docUrl = https://luatvietnam.vn/dat-dai/luat-dat-dai-2013-83386-d1.html
            if (docUrl.StartsWith("https://"))
            {
                docUrl = docUrl.Replace("https://", "");
            }
            else if (docUrl.StartsWith("http://"))
            {
                docUrl = docUrl.Replace("http://", "");
            }
            //docUrl = luatvietnam.vn/dat-dai/luat-dat-dai-2013-83386-d1.html
            var urlLevels = docUrl.Split("/");
            if(urlLevels.Length > 0)
            {
                docUrl = urlLevels[urlLevels.Length - 1];
                //docUrl = luat-dat-dai-2013-83386-d1.html
                var docUrlSplit = docUrl.Split("-");
                if (docUrlSplit.Length > 2)
                {
                    if(docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d1.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d2.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d3.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d4.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d5.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d6.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d7.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d8.html")
                        || docUrlSplit[docUrlSplit.Length - 1].ToLower().Equals("d9.html"))
                    {
                        retVal = Convert.ToInt32(docUrlSplit[docUrlSplit.Length - 2]);
                    }
                }
            }
            
            return retVal;
        }

        public static string FormatBytes(long bytes, int decimals = 2)
        {
            if (bytes == 0) return "0 Byte";

            const int k = 1024;
            var dm = decimals < 0 ? 0 : decimals;
            var sizes = new[] { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            var i = (int)Math.Floor(Math.Log(bytes) / Math.Log(k));
            return $"{Math.Round(bytes / Math.Pow(k, i), dm)} {sizes[i]}";
        }

        public static string standardizationQuestionContent(string question)
        {
            if (string.IsNullOrEmpty(question)){
                return question; 
            }

            question = question.Replace("'", "\"").Replace("\r", " ").Trim();

            while(question.Contains("  "))
            {
                question = question.Replace("  ", " ");
            }

            question = question.Trim();

            return question;
        }
    }
}
