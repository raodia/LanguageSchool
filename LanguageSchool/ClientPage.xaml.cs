﻿using System;
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
                Count.Text = RecordsEdge.ToString();
                AllRecords.Text = " из " + CountRecords.ToString();

                ClientLV.ItemsSource = CurrentPageList.ToList();
                ClientLV.Items.Refresh();
            }
            

        }

        //===========================================================================================//

        private void Update()
        {
            var currentClient = LanguageSHEntities.getContext().Client.ToList();


            /*
            if (TheFilter.SelectedIndex == 0)
            {
                currentClient = currentClient.ToList();
            }

            if (TheFilter.SelectedIndex == 1)
            {
                currentClient = currentClient.Where(p => (p.Atype == "ЗАО")).ToList();
            }

            if (TheFilter.SelectedIndex == 2)
            {
                currentClient = currentClient.Where(p => (p.Atype == "МКК")).ToList();
            }

            if (TheFilter.SelectedIndex == 3)
            {
                currentClient = currentClient.Where(p => (p.Atype == "МФО")).ToList();
            }

            if (TheFilter.SelectedIndex == 4)
            {
                currentClient = currentClient.Where(p => (p.Atype == "ОАО")).ToList();
            }

            if (TheFilter.SelectedIndex == 5)
            {
                currentClient = currentClient.Where(p => (p.Atype == "ООО")).ToList();
            }

            if (TheFilter.SelectedIndex == 6)
            {
                currentClient = currentClient.Where(p => (p.Atype == "ПАО")).ToList();
            }

            currentClient = currentClient.Where(p =>
            p.Title.ToLower().Contains(TheSearch.Text.ToLower())
            || p.Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "").Contains(TheSearch.Text)
            || p.Email.ToLower().Contains(TheSearch.Text.ToLower())
            ).ToList();
            */

            /*


            if (TheSort.SelectedIndex == 0)
            {
                currentClient = currentClient.ToList();
            }

            if (TheSort.SelectedIndex == 1)
            {
                currentClient = currentClient.OrderByDescending(p => p.Title).ToList();
            }

            if (TheSort.SelectedIndex == 2)
            {
                currentClient = currentClient.OrderBy(p => p.Title).ToList();
            }


            if (TheSort.SelectedIndex == 3)
            {
                currentClient = currentClient.OrderByDescending(p => p.DiscountInt).ToList();
            }

            if (TheSort.SelectedIndex == 4)
            {
                currentClient = currentClient.OrderBy(p => p.DiscountInt).ToList();
            }

            if (TheSort.SelectedIndex == 5)
            {
                currentClient = currentClient.OrderByDescending(p => p.Priority).ToList();
            }

            if (TheSort.SelectedIndex == 6)
            {
                currentClient = currentClient.OrderBy(p => p.Priority).ToList();
            }

            */

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

        }
    }
}
