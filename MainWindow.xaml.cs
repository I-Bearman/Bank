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
        public MainWindow()
        {
            InitializeComponent();
            core.CreateBank();
            ClientTypeLB.ItemsSource = Enum.GetNames(typeof(ClientType)).ToList();
            ClientTypeLB.SelectedIndex = 0; //default
            ClientListReload();
        }

        private void ClientListReload()
        {
            ClientList.ItemsSource = core.bank[ClientTypeLB.SelectedIndex].Clients;
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
                MessageBox.Show("Клиент сохранён", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                ClientList.Items.Refresh();
            }
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientInfo.ItemsSource = ClientList.SelectedItems;
        }
    }
}
