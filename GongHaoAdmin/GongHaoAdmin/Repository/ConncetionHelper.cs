using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace GongHaoAdmin.Repository
{
    public class ConncetionHelper
    {
        protected static readonly string MHConncetionString = WebConfigurationManager.ConnectionStrings["MHConncetionString"].ConnectionString;
    }
}