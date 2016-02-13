using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchultzLegendWebSite.Utilities
{
    public class Task
    {
        public string Name { get; set; }

        public string UserId { get; set; }

        public int Parent { get; set; }

        public int TaskId { get; set; }

        public sbyte Complete { get; set; }

        public int Priority { get; set; }

        public List<Task> Children { get; set; }

        public Task()
        {
            Children = new List<Task>();
        }
    }
}