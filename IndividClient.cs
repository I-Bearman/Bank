using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class IndividClient : Client
    {
        public IndividClient(string name, uint money) : base(name, money)
        {
        }
        public override ClientType Type { get; set; } = ClientType.Individual;
        public override byte CreditRate { get; set; } = 12;
        public override byte DepositRate { get; set; } = 3;
    }
}
