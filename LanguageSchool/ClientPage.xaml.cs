using System;
using System.Collections.Generic;
using System.Linq;
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
        int CountPage; //количество записей на странице
        int CurrentPage = 0; // текущая страница
        List<Client> CurrentPageList = new List<Client>(); //список клиентов на этой странице
        List<Client> TableList = new List<Client>(); //для подсчета записей используем count
        int maxRecords; //максимальное количество записей на странице

        //-------------------------------------------------------------------------------------------//

        private void changePage(int direction, int recordsPerPage, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (recordsPerPage == 0)
            {
                recordsPerPage = CountRecords;
                CountPage = 1;
            } 
            else
            {
            if (CountRecords % recordsPerPage > 0)
            {
            CountPage = CountRecords / 10;
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
        
        //-------------------------------------------------------------------------------------------//
        
        public ClientPage()
        {
            InitializeComponent();
            ClientLV.ItemsSource = LanguageSHEntities.getContext().Client.ToList();
            changePage(0, 0, null);
        }
        
        //-------------------------------------------------------------------------------------------//

        private void toLeft_Click(object sender, RoutedEventArgs e)
        {
            changePage(0, maxRecords, null);
        }

        //-------------------------------------------------------------------------------------------//


        private void toRight_Click(object sender, RoutedEventArgs e)
        {
            changePage(1, maxRecords, null);
        }
        
        //-------------------------------------------------------------------------------------------//

        private void RecordsCountPerPageCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indexRec = RecordsCountPerPageCB.SelectedIndex;

            switch(indexRec)
            {
                case 0:
                    changePage(0, 0, null);
                    break;
                case 1:
                    changePage(0, 10, null);
                    break;
                case 2:
                    changePage(0, 50, null);
                    break;
                case 3:
                    changePage(0, 200, null);
                    break;
            }
        }
    }
}
