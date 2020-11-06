using Diary.Logic;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Diary.Logic.Native;

namespace Diary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private ActiveDatabase Adb { get; set; }
        private Brush Red { get; set; }

        public MainWindow() {
            InitializeComponent();
            Red = (Brush)FindResource("Red");
            Adb = new ActiveDatabase();
            Invoke(() => TxtUsername.Focus());
        }

        #region TabPanel

        private void CollapseAllPages() {
            Invoke(() => MainPage.Visibility = Visibility.Collapsed);
            Invoke(() => SearchPage.Visibility = Visibility.Collapsed);
            Invoke(() => MorePage.Visibility = Visibility.Collapsed);
            Invoke(() => InfoPage.Visibility = Visibility.Collapsed);
            Invoke(() => BtnMain.IsEnabled = true);
            Invoke(() => BtnSearch.IsEnabled = true);
            Invoke(() => BtnMore.IsEnabled = true);
            Invoke(() => BtnInfo.IsEnabled = true);
        }

        private void btnMain_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => BtnMain.IsEnabled = false);
            Invoke(() => MainPage.Visibility = Visibility.Visible);
            Invoke(() => TxtTitle.Focus());
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => BtnSearch.IsEnabled = false);
            Invoke(() => SearchPage.Visibility = Visibility.Visible);
            Invoke(() => TxtSearch.Focus());
        }

        private void btnMore_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => BtnMore.IsEnabled = false);
            Invoke(() => MorePage.Visibility = Visibility.Visible);
            Invoke(() => TxtRemoveEntree.Focus());
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => BtnInfo.IsEnabled = false);
            Invoke(() => InfoPage.Visibility = Visibility.Visible);
        }

        #endregion TabPanel

        #region MainPage

        private async void btnSave_Click(object sender, RoutedEventArgs e) {
            var title = TxtTitle.Text;
            var body = TxtBody.Text;
            Invoke(() => LblMainStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body)) {
                Invoke(() => LblMainStatus.Foreground = Red);
                Invoke(() => LblMainStatus.Content = "Both title and body cannot be empty!");
            } else {
                var add = await Task.Run(() => Adb.AddEntree(title, body));
                if (add) {
                    Invoke(() => LblMainStatus.Content = "Successfully added the entry!");
                } else {
                    Invoke(() => LblMainStatus.Foreground = Red);
                    Invoke(() => LblMainStatus.Content = "Failed to add entry!");
                }
            }
        }

        private void txtBody_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            Invoke(() => LblMainStatus.Visibility = Visibility.Collapsed);
        }

        private void txtBody_TextChanged(object sender, TextChangedEventArgs e) {
            var body = TxtBody.Text;
            var lines = body.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length;
            var words = body.Replace(Environment.NewLine, " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
            var metrics = $"{TxtBody.Text.Length} characters, {words} words, {lines} lines.";
            Invoke(() => LblMetrics.Content = metrics);
        }


        #endregion MainPage

        #region SearchPage

        private async void btnSearchQuery_Click(object sender, RoutedEventArgs e) {
            var query = TxtSearch.Text;
            Invoke(() => LblSearchStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(query)) {
                Invoke(() => LblSearchStatus.Foreground = Red);
                Invoke(() => LblSearchStatus.Content = "Cannot search with empty text!");
            } else {
                var results = await Task.Run(() => Adb.Search(query));
                if (results == null || results.Count == 0) {
                    Invoke(() => LblSearchStatus.Content = "Found no results :(");
                } else {
                    Invoke(() => LblSearchStatus.Content = $"Found {results.Count} results!");
                    Invoke(() => LstSearchResults.ItemsSource = results);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e) {
            Invoke(() => LblSearchStatus.Visibility = Visibility.Collapsed);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnSearchQuery_Click(sender, e);
            }
        }

        #endregion SearchPage

        #region MorePage

        private async void btnRemoveEntree_Click(object sender, RoutedEventArgs e) {
            var id = TxtRemoveEntree.Text;
            Invoke(() => LblRemoveStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(id)) {
                Invoke(() => LblRemoveStatus.Foreground = Red);
                Invoke(() => LblRemoveStatus.Content = "Please enter ID!");
            } else {
                var status = int.TryParse(id, out var convertedId);
                if (!status) {
                    Invoke(() => LblRemoveStatus.Foreground = Red);
                    Invoke(() => LblRemoveStatus.Content = "ID must be numeric!");
                } else {
                    var removeStatus = await Task.Run(() => Adb.RemoveEntree(convertedId));
                    if (!removeStatus) {
                        Invoke(() => LblRemoveStatus.Foreground = Red);
                        Invoke(() => LblRemoveStatus.Content = "Failed to remove entry, confirm ID!");
                    } else {
                        Invoke(() => LblRemoveStatus.Content = "Successfully removed entry!");
                    }
                }
            }
        }

        private async void btnExportDiary_Click(object sender, RoutedEventArgs e) {
            Invoke(() => LblExportStatus.Visibility = Visibility.Visible);
            var exportResults = await Task.Run(() => Adb.ExportEntrees());
            if (!exportResults) {
                Invoke(() => LblExportStatus.Foreground = Red);
                Invoke(() => LblExportStatus.Content = "Failed to export user Diary!");
            } else {
                Invoke(() => LblExportStatus.Content = "Successfully export user Diary!");
            }
        }

        private async void btnRemoveUser_Click(object sender, RoutedEventArgs e) {
            var password = PswBoxRemove.Password;
            Invoke(() => LblRemoveUserStatus.Visibility = Visibility.Visible);
            if (password != Adb.CurrentUser.Password) {
                Invoke(() => LblRemoveUserStatus.Foreground = Red);
                Invoke(() => LblRemoveUserStatus.Content = "Wrong password!");
            } else {
                var removeStatus = await Task.Run(() => Adb.RemoveCurrentUser());
                if (!removeStatus) {
                    Invoke(() => LblRemoveUserStatus.Foreground = Red);
                    Invoke(() => LblRemoveUserStatus.Content = "Failed to remove user, check log!");
                } else {
                    Invoke(() => LblRemoveUserStatus.Content = "Successfully removed user!");
                }
            }
        }

        private void pswBoxRemove_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnRemoveUser_Click(sender, e);
            }
        }

        private void txtRemoveEntree_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnRemoveEntree_Click(sender, e);
            }
        }

        #endregion MorePage

        #region LoginPage

        private async void btnCreate_Click(object sender, RoutedEventArgs e) {
            var username = TxtUsername.Text;
            var password = PswBox.Password;
            Invoke(() => LblLoginStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
                Invoke(() => LblLoginStatus.Foreground = Red);
                Invoke(() => LblLoginStatus.Content = "Both username and password cannot be empty!");
            } else {
                var createResult = await Task.Run(() => Adb.AddUser(new User(username, password)));
                if (!createResult) {
                    Invoke(() => LblLoginStatus.Foreground = Red);
                    Invoke(() => LblLoginStatus.Content = "Failed to create user, check log!");
                } else {
                    Invoke(() => LoginPage.Visibility = Visibility.Collapsed);
                    Invoke(() => GeneralPage.Visibility = Visibility.Visible);
                }
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e) {
            var username = TxtUsername.Text;
            var password = PswBox.Password;
            Invoke(() => LblLoginStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
                Invoke(() => LblLoginStatus.Foreground = Red);
                Invoke(() => LblLoginStatus.Content = "Both username and password cannot be empty!");
            } else {
                var loginResult = await Task.Run(() => Adb.Login(username, password));
                if (!loginResult) {
                    Invoke(() => LblLoginStatus.Foreground = Red);
                    Invoke(() => LblLoginStatus.Content = "Wrong Credentials!");
                } else {
                    Invoke(() => LoginPage.Visibility = Visibility.Collapsed);
                    Invoke(() => GeneralPage.Visibility = Visibility.Visible);
                }
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                Invoke(() => PswBox.Focus());
            }
        }

        private void pswBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnLogin_Click(sender, e);
            }
        }

        #endregion LoginPage
    }
}
