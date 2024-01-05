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
            var randomCost = 200.10 + (_random.NextDouble() * (400.10 - 200.10));
            randomCost = Math.Round(randomCost, 2);

            string mimType = "";
            int extention = 1;
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "Reports", "HotelReservationReport.rdlc");
            string downloadFileName = string.IsNullOrEmpty(model.Name) ? "Print.pdf" : $"{model.Name}.pdf";

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "prSource", model?.Source },
                { "prDestination", model?.Destination },
                { "prHotelName", "Toolip Hotel" },
                { "prHotelAddress", "Egypt, Cairo, 10th ramadan" },
                { "prHotelPhone", "+966123456" },
                { "prHotelMail", "Support@Toolip.com" },
                { "prBookingRef", $"{GetRandomCharacters(1)}{_random.Next(1000000, 9999999)}" },
                { "prTo", model.To.ToString("dd/MMM/yyyy") },
                { "prFrom", model.From.ToString("dd/MMM/yyyy") },
                { "prCustomerName", model.Name },
                { "prCustomerID", model.ID },
                { "prBookingNumber", $"{_random.Next(100, 999)}{GetRandomCharacters(2)}{_random.Next(0, 9)}" },
                { "prVendorLocator", $"{GetRandomCharacters(5)}{_random.Next(0, 9)}" },
                { "prNowDate", DateTime.Now.ToString("dd/MM/yyyy") },
                { "prCost", randomCost.ToString() }
            };


            LocalReport report = new LocalReport(path);

            var result = report.Execute(RenderType.Pdf, extention, parameters, mimType);            
            return File(result.MainStream, "application/pdf");
        }

        private string GetRandomCharacters(int charactersCount)
        {
            string result = string.Empty;

            for (int i = 0; i < charactersCount; i++)
                result += (char)_random.Next('A', 'Z' + 1);
            
            return result;
        }
    }
}
