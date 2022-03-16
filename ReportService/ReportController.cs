using Microsoft.AspNetCore.Mvc;
using MYSQLStoreAPI.ReportService;

namespace MYSQLStoreAPI.ReportService
{   
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController:ControllerBase
    {
         private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var pdfFile = _reportService.GeneratePdfReport();
            return File(pdfFile,"application/octet-stream", "SimplePdf.pdf");
        }
    }
}