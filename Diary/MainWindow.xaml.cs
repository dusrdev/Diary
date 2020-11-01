using Diary.Logic;
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
using static Diary.Logic.Native;

namespace Diary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void CollapseAllPages() {
            InvokeA(() => MainPage.Visibility = Visibility.Collapsed);
            InvokeA(() => SearchPage.Visibility = Visibility.Collapsed);
            InvokeA(() => MorePage.Visibility = Visibility.Collapsed);
            InvokeA(() => InfoPage.Visibility = Visibility.Collapsed);
            InvokeA(() => btnMain.IsEnabled = true);
            InvokeA(() => btnSearch.IsEnabled = true);
            InvokeA(() => btnMore.IsEnabled = true);
            InvokeA(() => btnInfo.IsEnabled = true);
        }

        private void btnMain_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            InvokeA(() => btnMain.IsEnabled = false);
            InvokeA(() => MainPage.Visibility = Visibility.Visible);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            InvokeA(() => btnSearch.IsEnabled = false);
            InvokeA(() => SearchPage.Visibility = Visibility.Visible);
        }

        private void btnMore_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            InvokeA(() => btnMore.IsEnabled = false);
            InvokeA(() => MorePage.Visibility = Visibility.Visible);
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            InvokeA(() => btnInfo.IsEnabled = false);
            InvokeA(() => InfoPage.Visibility = Visibility.Visible);
        }
    }
}
