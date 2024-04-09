namespace CollectionsLibrary
{
    public class VIPClient : Client
    {
        public VIPClient(string name, uint money) : base(name, money)
        {
        }
        public override ClientType Type { get; set; } = ClientType.VIP;
        public override byte CreditRate { get; set; } = 6;
        public override byte DepositRate { get; set; } = 9;
    }
}
