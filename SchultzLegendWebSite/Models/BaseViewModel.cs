using System.Collections.Generic;
using SchultzLegendWebSite.Controllers;
using SchultzLegendWebSite.Utilities;
namespace SchultzLegendWebSite.Models

{
    public class BaseViewModel
    {
        public Page CurrentPage { get; set; }

        public bool ProgressTracker { get; set; }

        public string Email { get; set; }

        public BaseViewModel() : base()
        {
            ProgressTracker = false;
        }
    }
}