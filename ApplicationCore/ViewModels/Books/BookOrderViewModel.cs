namespace ApplicationCore.ViewModels.Books
{
    public class BookOrderViewModel
    {
        public int Isbn { get; set; }
        public string TiTle { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public int CusId { get; set; }
        public int? Quatity { get; set; }
        public int? TotalPrice { get; set; }
    }
}