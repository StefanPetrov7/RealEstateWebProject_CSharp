using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Web.ViewModels.Admin.Reports
{
    public class ReportsDashboardViewModel
    {
        public PropertyReportViewModel Snapshot { get; set; } = new PropertyReportViewModel();

        public IEnumerable<PropertyTrendReportViewModel> Trend { get; set; } = new List<PropertyTrendReportViewModel>();
    }
}
