namespace ApplicationCore.ViewModels.Transaction
{
    public class TranDetailViewModel
    {
        public int Isbn { get; set; }
        public int? CusId { get; set; }
        public int? Quatity { get; set; }
        public int? TotalPrice { get; set; }
        public string TiTle { get; set; }
        public string CusName { get; set; }
    }
}