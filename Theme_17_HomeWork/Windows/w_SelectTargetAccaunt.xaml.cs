using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using BankNameSpase;
using LogCenterNameSpace;

namespace Theme_17_HomeWork
{
    /// <summary>
    /// Interaction logic for w_SelectTargetAccaunt.xaml
    /// </summary>
    public partial class w_SelectTargetAccaunt : Window
    {
        public ObservableCollection<Accaunt> Accaunts;
        public w_SelectTargetAccaunt()
        {
            InitializeComponent();
            //lst_Accauntss.ItemsSource = Accaunts;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.w_SelectTargetAccaunt = null;
        }

        private void lst_Accauntss_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lst_Accauntss.SelectedItem != null)
            {
            this.Tag = lst_Accauntss.SelectedItem;

            this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Accauntss.SelectedItem != null)
            {
                this.Tag = lst_Accauntss.SelectedItem;

                this.Close();
            }
        }
    }
}
