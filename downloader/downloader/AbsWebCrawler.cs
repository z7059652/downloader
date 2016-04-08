using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downloader
{
    public abstract class AbsWebCrawler
    {
        public AbsWebCrawler()
        { 
        }
        public abstract string Search(string url);
    }
}
