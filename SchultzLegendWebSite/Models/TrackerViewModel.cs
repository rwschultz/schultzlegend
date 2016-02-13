using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchultzLegendWebSite.Controllers;
using SchultzLegendWebSite.Utilities;

namespace SchultzLegendWebSite.Models
{
    public class TrackerViewModel : AccountViewModel
    {
        public List<Page> TrackerPages { get; set; }

        public int TaskTotal { get; set; }

        public int TaskComplete { get; set; }

        public TrackerViewModel() : base()
        {
            ProgressTracker = true;
            TrackerPages = TrackerController.GetTrackerPages();
        }
    }

    public class TaskViewModel : TrackerViewModel
    {
        public List<Task> Tasks { get; set; }

        public TaskViewModel() : base()
        {
        }
    }
}