using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public abstract class Client
    {
        public string Name { get; set; }
        public abstract ClientType Type { get; set; }
        public uint Money { get; set; }
        public bool IsCredit { get; set; }
        public abstract byte CreditRate { get; set; }
        public bool IsDeposit { get; set; }
        public DepositType DepositType { get; set; }
        public abstract byte DepositRate { get; set; }
        public uint DepositSum { get; set; }
        public bool CreditStoryIsGood { get; set; }

        public Client()
        {
            Name = "Default Name";
            Money = 0;
        }

        protected Client(string name, uint money)
        {
            Name = name;
            Money = money;
        }
    }
}
