using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Data.DataServices;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Web.ViewModels.Admin.Reports;

namespace RealEstateApp.Web.Areas.Admin.Controllers
{
    public class ReportsController : BaseAdminController
    {
        private readonly IPropertyService propertyService;   
        private readonly IValidationService validationService;

        public ReportsController(IPropertyService propertyService, IValidationService validationService)
        {
            this.propertyService = propertyService;
            this.validationService = validationService; 
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            PropertyReportViewModel snapshot = await this.propertyService.GetPropertyStatusReportAsync();
            IEnumerable<PropertyTrendReportViewModel> trend = await this.propertyService.GetPropertyTrendReportAsync();

            ReportsDashboardViewModel reportDashboard = new ReportsDashboardViewModel
            {
                Snapshot = snapshot,
                Trend = trend
            };

            return View(reportDashboard);
        }
    }
}
