namespace AnhQuoc_WPF_C4_B1
{
    public class AccountInfo
    {
        public string Id { get; set; }
        public Account Account { get; set; }

        public bool Status { get; set; }
        public int Role { get; set; }

        public AccountInfo()
        {
            Account = new Account();
            Status = true;
        }
    }
}
