using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader
{

    public class WebCrawlerService
    {
        private string _path = "..";
        private string _filename="";
        private AbsWebCrawler tc = new TableCrawler();
        public WebCrawlerService()
        {

        }
        public string path
        {
            set
            {
                _path = value;
            }
            get
            {
                return _path;
            }
        }
        public void Search(string url, string name)
        {
            string res = tc.Search(url);
            SaveFile(res, name);
        }
        private bool SaveFile(string data, string name)
        {
            if (data.Length <= 0) return false;
            _filename = path + "\\" + name+".txt";
            try
            {
                if (File.Exists(_filename))
                    File.Delete(_filename);
                FileStream fs = new FileStream(_filename, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(data);
                sw.Close();
                fs.Close();
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
            return true;
        }
    }
}
