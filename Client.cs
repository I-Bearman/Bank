namespace Bank
{
    public abstract class Client
    {
        public string Name { get; set; }
        public abstract ClientType Type { get; set; }
        public uint Money { get; set; }
        public bool IsCredit { get; set; }
        public abstract byte CreditRate { get; set; }
        public uint CreditSum { get; set; }
        public bool IsDeposit { get; set; }
        public DepositType DepositType { get; set; }
        public abstract byte DepositRate { get; set; }
        public uint DepositSum { get; set; }
        public string DepositInfo { get; set; }
        public bool CreditStoryIsGood { get; set; }

        protected Client(string name, uint money)
        {
            Name = name;
            Money = money;
        }
    }
}
