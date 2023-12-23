namespace AirReservation.ViewModels
{
    public class PrintPdfViewModel
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        //public int Count { get; set; }
        public string PhoneNumber { get; set; }
        //public List<string> Names { get; set; }
    }
}
