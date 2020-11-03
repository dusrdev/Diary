using Diary.Logic;
using System;
using System.ComponentModel;
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
        private Brush Green { get; set; }
        private Brush Red { get; set; }

        public MainWindow() {
            InitializeComponent();
            Green = (Brush)FindResource("DSGreen");
            Red = (Brush)FindResource("Red");
            Adb = new ActiveDatabase();
            Invoke(() => txtUsername.Focus());
        }

        protected override void OnClosing(CancelEventArgs e) {
            Logger.ExportLog();
            base.OnClosing(e);
        }

        #region TabPanel

        private void CollapseAllPages() {
            Invoke(() => MainPage.Visibility = Visibility.Collapsed);
            Invoke(() => SearchPage.Visibility = Visibility.Collapsed);
            Invoke(() => MorePage.Visibility = Visibility.Collapsed);
            Invoke(() => InfoPage.Visibility = Visibility.Collapsed);
            Invoke(() => btnMain.IsEnabled = true);
            Invoke(() => btnSearch.IsEnabled = true);
            Invoke(() => btnMore.IsEnabled = true);
            Invoke(() => btnInfo.IsEnabled = true);
        }

        private void btnMain_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => btnMain.IsEnabled = false);
            Invoke(() => MainPage.Visibility = Visibility.Visible);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => btnSearch.IsEnabled = false);
            Invoke(() => SearchPage.Visibility = Visibility.Visible);
        }

        private void btnMore_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => btnMore.IsEnabled = false);
            Invoke(() => MorePage.Visibility = Visibility.Visible);
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e) {
            CollapseAllPages();
            Invoke(() => btnInfo.IsEnabled = false);
            Invoke(() => InfoPage.Visibility = Visibility.Visible);
        }

        #endregion TabPanel

        #region MainPage

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            var title = txtTitle.Text;
            var body = txtBody.Text;
            Invoke(() => lblMainStatus.Foreground = Red);
            Invoke(() => lblMainStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body)) {
                Invoke(() => lblMainStatus.Content = "Both title and body cannot be empty!");
            } else {
                Adb.AddEntree(title, body);
                Invoke(() => lblMainStatus.Foreground = Green);
                Invoke(() => lblMainStatus.Content = "Successfully added the entry!");
            }
        }

        private void txtBody_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
            Invoke(() => lblMainStatus.Visibility = Visibility.Collapsed);
        }

        private void txtBody_TextChanged(object sender, TextChangedEventArgs e) {
            var lines = txtBody.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length;
            var words = txtBody.Text.Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length;
            string metrics = $"{txtBody.Text.Length} characters, {words} words, {lines} lines.";
            Invoke(() => lblMetrics.Content = metrics);
        }


        #endregion MainPage

        #region SearchPage

        private void btnSearchQuery_Click(object sender, RoutedEventArgs e) {
            var query = txtSearch.Text;
            Invoke(() => lblSearchStatus.Foreground = Red);
            Invoke(() => lblSearchStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(query)) {
                Invoke(() => lblSearchStatus.Content = "Cannot search with empty text!");
            } else {
                var results = Adb.Search(query);
                if (results == null || results.Count == 0) {
                    Invoke(() => lblSearchStatus.Content = "Found no results :(");
                } else {
                    Invoke(() => lblSearchStatus.Foreground = Green);
                    Invoke(() => lblSearchStatus.Content = $"Found {results.Count} results!");
                    Invoke(() => lstSearchResults.ItemsSource = results);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e) {
            Invoke(() => lblSearchStatus.Visibility = Visibility.Collapsed);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                btnSearchQuery_Click(sender, e);
            }
        }

        #endregion SearchPage

        #region MorePage

        private void btnRemoveEntree_Click(object sender, RoutedEventArgs e) {
            var id = txtRemoveEntree.Text;
            Invoke(() => lblRemoveStatus.Foreground = Red);
            Invoke(() => lblRemoveStatus.Visibility = Visibility.Visible);
            if (string.IsNullOrEmpty(id)) {
                Invoke(() => lblRemoveStatus.Content = "Please enter ID!");
            } else {
                bool status = int.TryParse(id, out int convertedID);
                if (!status) {
                    Invoke(() => lblRemoveStatus.Content = "ID must be numeric!");
                } else {
                    bool removeStatus = Adb.RemoveEntree(convertedID);
                    if (!removeStatus) {
                        Invoke(() => lblRemoveStatus.Content = "Failed to remove entry, confirm ID!");
                    } else {
                        Invoke(() => lblRemoveStatus.Foreground = Green);
                        Invoke(() => lblRemoveStatus.Content = "Successfully removed entry!");
                    }
                }
            }
        }

        private void btnExportDiary_Click(object sender, RoutedEventArgs e) {
            Invoke(() => lblExportStatus.Foreground = Red);
            Invoke(() => lblExportStatus.Visibility = Visibility.Visible);
            var exportResults = Adb.ExportEntrees();
            if (!exportResults) {
                Invoke(() => lblExportStatus.Content = "Failed to export user Diary!");
            } else {
                Invoke(() => lblExportStatus.Foreground = Green);
                Invoke(() => lblExportStatus.Content = "Successfully export user Diary!");
            }
        }

        private void btnRemoveUser_Click(object sender, RoutedEventArgs e) {
            var password = pswBoxRemove.Password;
            Invoke(() => lblRemoveUserStatus.Foreground = Red);
            Invoke(() => lblRemoveUserStatus.Visibility = Visibility.Visible);
            if (password != Adb.CurrentUser.Password) {
                Invoke(() => lblRemoveUserStatus.Content = "Wrong password!");
            } else {
                bool removeStatus = Adb.RemoveCurrentUser();
                if (!removeStatus) {
                    Invoke(() => lblRemoveUserStatus.Content = "Failed to remove user, check log!");
                } else {
                    Invoke(() => lblRemoveUserStatus.Foreground = Green);
                    Invoke(() => lblRemoveUserStatus.Content = "Successfully removed user!");
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

        private void btnCreate_Click(object sender, RoutedEventArgs e) {
            var username = txtUsername.Text;
            var password = pswBox.Password;
            Invoke(() => lblLoginStatus.Visibility = Visibility.Visible);
            Invoke(() => lblLoginStatus.Foreground = Red);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
                Invoke(() => lblLoginStatus.Content = "Both username and password cannot be empty!");
            } else {
                bool createResult = Adb.AddUser(new User(username, password));
                if (!createResult) {
                    Invoke(() => lblLoginStatus.Content = "Failed to create user, check log!");
                } else {
                    Invoke(() => LoginPage.Visibility = Visibility.Collapsed);
                    Invoke(() => GeneralPage.Visibility = Visibility.Visible);
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            var username = txtUsername.Text;
            var password = pswBox.Password;
            Invoke(() => lblLoginStatus.Visibility = Visibility.Visible);
            Invoke(() => lblLoginStatus.Foreground = Red);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
                Invoke(() => lblLoginStatus.Content = "Both username and password cannot be empty!");
            } else {
                bool loginResult = Adb.Login(username, password);
                if (!loginResult) {
                    Invoke(() => lblLoginStatus.Content = "Wrong Credentials!");
                } else {
                    Invoke(() => LoginPage.Visibility = Visibility.Collapsed);
                    Invoke(() => GeneralPage.Visibility = Visibility.Visible);
                }
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                Invoke(() => pswBox.Focus());
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
