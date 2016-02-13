using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchultzLegendWebSite.Utilities
{
    public class Page
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public Page() {}

        public Page(string name, string link)
        {
            Name = name;
            Link = link;
        }
    }
}