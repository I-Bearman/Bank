namespace CollectionsLibrary
{
    public class BusinessClient : Client
    {
        public BusinessClient(string name, uint money) : base(name, money)
        {
        }
        public override ClientType Type { get; set; } = ClientType.Business;
        public override byte CreditRate { get; set; } = 9;
        public override byte DepositRate { get; set; } = 6;
    }
}
