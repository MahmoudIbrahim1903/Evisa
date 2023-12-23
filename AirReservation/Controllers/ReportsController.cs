using AirReservation.ViewModels;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace AirReservation.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Random _random;

        public ReportsController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _random = new Random();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Print(PrintPdfViewModel model)
        {
            string mimType = "";
            int extention = 1;
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Reports", "HotelReservationReport.rdlc");

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prSource", model?.Source);
            parameters.Add("prDestination", model?.Destination);
            parameters.Add("prPhoneNumber", model?.PhoneNumber);
            parameters.Add("prHotelName", "Toolip Hotel");
            parameters.Add("prHotelAddress", "Egypt, Cairo, 10th ramadan");
            parameters.Add("prHotelPhone", "+966123456");
            parameters.Add("prHotelMail", "Support@Toolip.com");
            parameters.Add("prBookingRef", $"{(char)_random.Next('A', 'Z' + 1)}{_random.Next(1000000,9999999)}");
            parameters.Add("prTo", model.To.ToString("dd/MMM/yyyy"));
            parameters.Add("prFrom", model.From.ToString("dd/MMM/yyyy"));
            parameters.Add("prCustomerName", model.Name);

            LocalReport report = new LocalReport(path);

            var result = report.Execute(RenderType.Pdf, extention, parameters, mimType);            
            return File(result.MainStream, "application/pdf");
        }
    }
}
