using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
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
