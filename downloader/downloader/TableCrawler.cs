using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace downloader
{
    public class TableCrawler:AbsWebCrawler
    {
        public TableCrawler()
        {

        }
        private const string labelStart = "<div class=\"genTable\">";
        private const string labelEnd = "</div>";
        private const string label = "Quarterly Earnings Surprise History";
        private const string divRegex = @"<div class=""genTable"">(.|\W)*?</div>";
        private const string titleRegex = @"<th>(.|\W)*?</th>";
        private const string trRegex = @"<tr>(.|\W)*?</tr>";
        private const string tdRegex = @"<td>(.|\W)*?</td>";
        private const string theadRegex = @"<thead>(.|\W)*?</thead>";
        private const string tab = "\t\t";
        private string getHtml(string url)
        {
            string html = "";
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse hwrs = (HttpWebResponse)hwr.GetResponse();
                Stream stream = hwrs.GetResponseStream();
                StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(hwrs.CharacterSet));
                html = sr.ReadToEnd();
                sr.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return html;
        }
        private string getSpecifyContentByRegex(string html)
        {
            try
            {
                Regex reg = new Regex(@"(\n|\r|\t)+");
                string str = reg.Replace(html, "");
                MatchCollection matches = Regex.Matches(str,divRegex);
                foreach(var ma in matches)
                {
                    Match mat = Regex.Match(ma.ToString().Trim(), label);
                    if (mat.Success)
                        return ma.ToString().Trim();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }
        private bool IsValid(string str)
        {
            if (str == null || str.Length <= 0)
                return false;
            return true;
        }
        public override string Search(string url)
        {
            string str = getHtml(url);
            if (!IsValid(str))
                return "";
            string str_get = getSpecifyContentByRegex(str);
            if(!IsValid(str_get))
                str_get = getSpecifyContent(str, labelStart, labelEnd);
            if (!IsValid(str_get))
                return "";
            string res = getTableResult(str_get);
            Console.WriteLine(res);
            return res;
        }
        private string getTitle(string str)
        {
            string res="";
            try
            {
                MatchCollection matches = Regex.Matches(str, titleRegex);
                foreach(var ma in matches)
                {
                    Regex reg = new Regex(@"<br>");
                    string temp = reg.Replace(ma.ToString(), " ");
                    temp = temp.Substring(4, temp.Length - 4 - 5);
                    res += temp + "  ";
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return res+"\r\n";
        }
        private string getTableContent(string str)
        {
            string res = "";
            try
            {
                MatchCollection matches = Regex.Matches(str, trRegex);
                foreach (var ma in matches)
                {
                    MatchCollection tdmatches = Regex.Matches(ma.ToString(), tdRegex);
                    foreach(var td in tdmatches)
                    {
                        string tdstr = td.ToString().Trim();
                        string temp = tdstr.Substring(4,tdstr.Length-9);
                        res += temp + tab;
                    }
                    res += "\r\n";
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return res;
        }
        private string getTableResult(string html)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Regex reg = new Regex(@"(\n|\r|\t)+");
                string str = reg.Replace(html, "");
                MatchCollection matches = Regex.Matches(str, theadRegex);
                if (matches.Count != 1)
                    return "";
                string title = getTitle(matches[0].Value);
                sb.Append(title);
                string content = getTableContent(str);
                Console.WriteLine(title);
                sb.Append(content);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return sb.ToString();
        }
        private string getSpecifyContent(string istr, string startString, string endString)
        {
            if (!IsValid(istr)) return "";
            int iBodyStart = -1, iBodyEnd = -1;
            while(true)
            {
                iBodyStart = istr.IndexOf(startString, 0);               //begin position
                if (iBodyStart == -1)
                    break;
                iBodyStart += startString.Length;                           //fisrt length
                iBodyEnd = istr.IndexOf(endString, iBodyStart);         //second
                if (iBodyEnd == -1)
                    break;
                iBodyEnd += endString.Length;                              //second length
                string strResult = istr.Substring(iBodyStart, iBodyEnd - iBodyStart - 1);
                istr = istr.Substring(iBodyStart,istr.Length-iBodyStart-1);
                if(strResult.IndexOf(label,0) != -1)
                    return strResult;
            }
            return null;
        }


    }
}
