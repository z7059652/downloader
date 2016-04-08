using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace downloader
{

    class Program
    {
        private static string preurl = "http://wwwwwwww.nasdaq.com/earnings/report/";
        static void Main(string[] args)
        {
            WebCrawlerService wc = new WebCrawlerService();
            string[] arrUrl = args[0].Trim().Split("_".ToCharArray());
            for(int i = 0;i < arrUrl.Length;i++)
            {
                string url = preurl + arrUrl[i].ToLower();
                wc.Search(url,arrUrl[i]);
            }
        }
    }
}
