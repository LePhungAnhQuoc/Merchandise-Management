namespace AnhQuoc_WPF_C4_B1
{
    public class Customer
    {
        public string IDCard { get; set; }
        public string Name { get; set; }
		public string Phone { get; set; }
        public double Point { get; set; }
        public CardType Card { get; set; }
        public bool IsGuest { get; set; }
    }
}
