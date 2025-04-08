using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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

namespace LanguageSchool
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    //-------------------------------------------------------------------------------------------//

    public partial class ClientPage : Page
    {
        int CountRecords; //количество всех записей
        int CountPage = 0; //количество записей на странице
        int CurrentPage = 0; // текущая страница
        List<Client> CurrentPageList = new List<Client>(); //список клиентов на этой странице
        List<Client> TableList = new List<Client>(); //список клиентов весь
        int maxRecords = 0; //максимальное количество записей на странице

        //-------------------------------------------------------------------------------------------//

        private void changePage(int direction, int recordsPerPage1, int? selectedPage)
        {
            //Console.WriteLine(CurrentPageList);
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            int recordsPerPage = recordsPerPage1;

            if (recordsPerPage == 0)
            {
                recordsPerPage = CountRecords;
                CountPage = 1;
            } 
            else
            {
            CountPage = CountRecords / recordsPerPage;
                if (CountRecords % recordsPerPage > 0)
                {
                    CountPage++;
                }

            }



            Boolean Ifupdate = true;

            int RecordsEdge;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    //RecordsEdge = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    RecordsEdge = Math.Min(CurrentPage * recordsPerPage + recordsPerPage, CountRecords);

                    for (int i = CurrentPage * recordsPerPage; i < RecordsEdge; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            //RecordsEdge = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            RecordsEdge = Math.Min(CurrentPage * recordsPerPage + recordsPerPage, CountRecords);
                            for (int i = CurrentPage * recordsPerPage; i < RecordsEdge; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }

                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            RecordsEdge = CurrentPage * recordsPerPage + recordsPerPage < CountRecords ? CurrentPage * recordsPerPage + recordsPerPage : CountRecords;
                            for (int i = CurrentPage * recordsPerPage; i < RecordsEdge; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }

            }

            if (Ifupdate)
            {
                ListOrientation.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    ListOrientation.Items.Add(i);
                }
                ListOrientation.SelectedIndex = CurrentPage;

                RecordsEdge = CurrentPage * recordsPerPage + recordsPerPage < CountRecords ? CurrentPage * recordsPerPage + recordsPerPage: CountRecords;
                Count.Text = CountRecords.ToString();
                AllRecords.Text = " из " + LanguageSHEntities.getContext().Client.ToList().Count;

                ClientLV.ItemsSource = CurrentPageList.ToList();
                ClientLV.Items.Refresh();
            }
            

        }

        //===========================================================================================//

        private void Update()
        {
            var currentClient = LanguageSHEntities.getContext().Client.ToList();

            //=======================================================================================
            
            if (TheFilter.SelectedIndex == 0)
            {
                currentClient = currentClient.ToList();
            }

            if (TheFilter.SelectedIndex == 1)
            {
                currentClient = currentClient.Where(p => (p.GenderFull == "мужской")).ToList();
            }

            if (TheFilter.SelectedIndex == 2)
            {
                currentClient = currentClient.Where(p => (p.GenderFull == "женский")).ToList();
            }

            

            currentClient = currentClient.Where(p =>
            p.LastName.ToLower().Contains(TheSearch.Text.ToLower())
            || p.Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "").Contains(TheSearch.Text)
            || p.Email.ToLower().Contains(TheSearch.Text.ToLower())
            ).ToList();
            

            


            if (TheSort.SelectedIndex == 0)
            {
                currentClient = currentClient.ToList();
            }

            if (TheSort.SelectedIndex == 1)
            {
                currentClient = currentClient.OrderBy(p => p.LastName).ToList();
            }

            if (TheSort.SelectedIndex == 2)
            {
                currentClient = currentClient.OrderByDescending(p => p.LastSignUpDate).ToList();
            }


            if (TheSort.SelectedIndex == 3)
            {
                currentClient = currentClient.OrderByDescending(p => p.SignUpCount).ToList();

            }


            //==============================================================================================
          
            ClientLV.ItemsSource = currentClient;

            TableList = currentClient;


            changePage(0, maxRecords, 0);
        }

        //-------------------------------------------------------------------------------------------//

        public ClientPage()
        {
            InitializeComponent();
            ClientLV.ItemsSource = LanguageSHEntities.getContext().Client.ToList();

            RecordsCountPerPageCB.SelectedIndex = 0;
            ListOrientation.SelectedIndex = 1;

            TheFilter.SelectedIndex = 0;
            TheSort.SelectedIndex = 0;

            //changePage(0, maxRecords, 0);
            Update();
        }
        
        //-------------------------------------------------------------------------------------------//

        private void toLeft_Click(object sender, RoutedEventArgs e)
        {
            changePage(1, maxRecords, null);
        }

        //-------------------------------------------------------------------------------------------//


        private void toRight_Click(object sender, RoutedEventArgs e)
        {
            changePage(2, maxRecords, null);
        }
        
        //-------------------------------------------------------------------------------------------//

        private void RecordsCountPerPageCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indexRec = RecordsCountPerPageCB.SelectedIndex;

            switch(indexRec)
            {
                case 0:
                    maxRecords = 0;
                    break;
                case 1:
                    maxRecords = 10;
                    break;
                case 2:
                    maxRecords = 50;
                    break;
                case 3:
                    maxRecords = 200;
                    break;
            }
            changePage(0, maxRecords, 0);

        }

        private void ListOrientation_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changePage(0, maxRecords, Convert.ToInt32(ListOrientation.SelectedItem.ToString()) - 1);
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                LanguageSHEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ClientLV.ItemsSource = LanguageSHEntities.getContext().Client.ToList();
            }
            Update();
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Client;

            if (currentClient.LastSignUp != "нет")
            {
                MessageBox.Show("Невозможно удалить данные, так как существуют записи на эту услугу");
            }
            else
            {

                if (MessageBox.Show("Вы точно хотите удалить это?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        LanguageSHEntities.getContext().Client.Remove(currentClient);
                        LanguageSHEntities.getContext().SaveChanges();
                        ClientLV.ItemsSource = LanguageSHEntities.getContext().Service.ToList();
                        Update();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }

                }
            }
        }
        private void TheSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void TheSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();

        }

        private void TheFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

    }
}
