using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class VIPClient : Client
    {
            public override byte CreditRate { get; set; } = 6;
            public override byte DepositRate { get; set; } = 9;
    }
}
