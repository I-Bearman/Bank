using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Core
    {
        public List<BankDepart> bank;

        public List<BankDepart> CreateBank()
        {
            bank = new List<BankDepart>
            {
                new IndividBank(),
                new BusinessBank(),
                new VIPBank()
            };
            return bank;
        }

        public void CreateIndividClient(string name, uint money, bool creditStoreISGood)
        {
            Client client = new IndividClient(name, money);
            if (creditStoreISGood)
                GoodCreditStorySetup(client);
            bank[(int)ClientType.Individual].Clients.Add(client);
        }
        public void CreateBusinessClient(string name, uint money, bool creditStoreISGood)
        {
            Client client = new BusinessClient(name, money);
            if (creditStoreISGood)
                GoodCreditStorySetup(client);
            bank[(int)ClientType.Business].Clients.Add(client);
        }
        public void CreateVIPClient(string name, uint money, bool creditStoreISGood)
        {
            Client client = new VIPClient(name, money);
            if (creditStoreISGood)
                GoodCreditStorySetup(client);
            bank[(int)ClientType.VIP].Clients.Add(client);
        }
        private Client GoodCreditStorySetup(Client client)
        {
            client.CreditStoryIsGood = true;
            client.CreditRate -= 3;
            client.DepositRate += 3;
            return client;
        }


    }
}
