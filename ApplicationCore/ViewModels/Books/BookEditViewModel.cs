namespace ApplicationCore.ViewModels.Books
{
    public class BookEditViewModel
    {
        public int Isbn { get; set; }
        public string TiTle { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }
    }
}