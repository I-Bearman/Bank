using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Core core = new Core();
        string pathToBase = "ClientBase.json";
        public MainWindow()
        {
            InitializeComponent();
            core.CreateBank();
            //core.LoadBase(pathToBase);
            ClientTypeLB.ItemsSource = Enum.GetNames(typeof(ClientType)).ToList();
            ClientTypeLB.SelectedIndex = 0; //default
            DepositTypeCB.ItemsSource = Enum.GetNames(typeof(DepositType)).Where(n => n != "None").ToList();
            ClientListReload();
        }

        private void ClientListReload()
        {
            List<Client> clients = core.bank[ClientTypeLB.SelectedIndex].Clients;
            ClientList.ItemsSource = clients;
            RecipientCB.ItemsSource = clients;
            ClientTypeLB.Items.Refresh();
        }
        private void ClientTypeLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientListReload();
        }


        private void CreateNewClientButton_Click(object sender, RoutedEventArgs e)
        {
            string clientName = ClientNameInput.Text;
            if (clientName == "")
                MessageBox.Show("Введите Имя клиента", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (!uint.TryParse(MoneySumInput.Text, out uint clientMoney))
                MessageBox.Show("Введите в качестве Накопления клиента целочисленное значение", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                bool creditStoreISGood = (bool)CreditStoryStatusInput.IsChecked;
                switch (ClientTypeLB.SelectedIndex)
                {
                    case 0:
                        core.CreateIndividClient(clientName, clientMoney, creditStoreISGood);
                        break;
                    case 1:
                        core.CreateBusinessClient(clientName, clientMoney, creditStoreISGood);
                        break;
                    case 2:
                        core.CreateVIPClient(clientName, clientMoney, creditStoreISGood);
                        break;
                    default:
                        break;
                }
                ClientList.Items.Refresh();
                RecipientCB.Items.Refresh();
            }
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientInfo.ItemsSource = ClientList.SelectedItems;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            core.SaveBase(pathToBase, core.bank);
        }

        private void GetCreditButton_Click(object sender, RoutedEventArgs e)
        {
            Client senderC = ClientList.SelectedItem as Client;

            //Содержит 2 проверки:
            //- выбран ли клиент из списка
            //- верное ли число введено в поле Суммы
            if (senderC == null)
                MessageBox.Show("Не Выбран клиент из списка", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                bool parseResult = uint.TryParse(CreditSumTB.Text, out uint resultSum);
                if (!parseResult)
                    MessageBox.Show("Введите целочисленную положительную сумму кредита", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    core.GetCredit(senderC, resultSum);
                    ClientInfo.Items.Refresh();
                    MessageBox.Show("Кредит выдан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            Client senderC = ClientList.SelectedItem as Client;
            Client recipient = RecipientCB.SelectedItem as Client;
     
            if (senderC == recipient)
                MessageBox.Show("Клиент не может сделать перевод самому себе", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                //Содержит 4 проверки последовательно:
                //- выбран ли клиент из списка
                //- выбран ли получатель из комбобокса
                //- верное ли число введено в поле Суммы
                //- у клиента достаточно денег для перевода
                if (senderC == null)
                    MessageBox.Show("Не Выбран клиент из списка", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (recipient == null)
                    MessageBox.Show("Не Выбран получатель трансфера", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    bool parseResult = uint.TryParse(TransSumTB.Text, out uint resultSum);
                    if (!parseResult)
                        MessageBox.Show("Введите целочисленную положительную сумму трансфера", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else if (!core.CheckEnoughSum(senderC, resultSum))
                        MessageBox.Show("Клиент не имеет нужной суммы", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                    {
                        core.Transfer(senderC, recipient, resultSum);
                        ClientInfo.Items.Refresh();
                        MessageBox.Show("Трансфер произведён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
