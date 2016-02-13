using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SchultzLegendWebSite.Utilities;

namespace SchultzLegendWebSite.Models
{
    public class AccountViewModel : BaseViewModel
    {
        public string UserId { get; set; }

        public AccountViewModel() : base()
        {
        }
    }
}