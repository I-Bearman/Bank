using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bank
{
    class Core
    {
        public List<BankDepart> bank;

        public void CreateBank()
        {
            bank = new List<BankDepart>
            {
                new IndividBank(),
                new BusinessBank(),
                new VIPBank()
            };
        }

        public void SaveBase(string pathToBase, List<BankDepart> bank)
        {
            string json = JsonConvert.SerializeObject(bank);
            File.WriteAllText(pathToBase, json);
        }
        public void LoadBase(string pathToBase)
        {
            if (File.Exists(pathToBase))
            {
                string json = File.ReadAllText(pathToBase);
                bank = JsonConvert.DeserializeObject(json) as List<BankDepart>;
            }
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
        public bool CheckEnoughSum(Client client, uint checkSum)
        {
            bool result = client.Money >= checkSum;
            return result;
        }
        public void GetCredit(Client client, uint creditSum)
        {
            client.Money += creditSum;
            client.IsCredit = true;
        }
        public void Transfer(Client sender, Client recipient, uint transSum)
        {
            sender.Money -= transSum;
            recipient.Money += transSum;
        }
        public void MakeSimpleDeposit(Client client, uint DepositSum)
        {
            client.Money -= DepositSum;
            client.DepositType = DepositType.Simple;
            client.IsDeposit = true;
            client.DepositSum += DepositSum;
        }
        public void MakeCapitalisedDeposit(Client client, uint DepositSum)
        {
            client.Money -= DepositSum;
            client.DepositType = DepositType.Capitalisation;
            client.IsDeposit = true;
            client.DepositSum += DepositSum;
        }

        public string DepositInfo(Client client)
        {
            uint depositSum = client.DepositSum;
            int depositRate = client.DepositRate;
            float[] months = new float[12];

            months[0] = depositSum + (depositSum * depositRate / 100 / 12);
            months[0] = (float)Math.Round(months[0], 2);
            string infoString = $"1 месяц: {months[0]}\n";

            //Простой депозит
            if (client.DepositType == DepositType.Simple)
            {
                for (int i = 1; i < months.Length; i++)
                {
                    months[i] = months[i - 1] + (depositSum * depositRate / 100 / 12);
                    months[i] = (float)Math.Round(months[i], 2);
                    infoString += $"{i + 1} месяц: {months[i]}\n";
                }
            }
            //Депозит с капитализацией
            else
            {
                for (int i = 1; i < months.Length; i++)
                {
                    months[i] = months[i - 1] + (months[i - 1] * depositRate / 100 / 12);
                    months[i] = (float)Math.Round(months[i], 2);
                    infoString += $"{i + 1} месяц: {months[i]}\n";
                }
            }
            return infoString;
        }




    }
}
