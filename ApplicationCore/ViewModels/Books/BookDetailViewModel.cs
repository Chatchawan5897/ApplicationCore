namespace ApplicationCore.ViewModels.Books
{
    public class BookDetailViewModel
    {
        public int Isbn { get; set; }
        public string TiTle { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
        public int CusId { get; set; }
        public string CusName { get; set; }
        public string CusAddress { get; set; }
        public string CusEmail { get; set; }
        public int? Quatity { get; set; }
        public int? TotalPrice { get; set; }
    }
}