using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Web.ViewModels.Admin.Reports
{
    public class PropertyTrendReportViewModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int AddedCount { get; set; }

        public int DeletedCount { get; set; }
    }
}
