using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CardexLogTransformer.Business;
using CardexLogTransformer.Properties;
using Microsoft.Data.SqlClient;

namespace CardexLogTransformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        private string DataStore => ServerNameTextBox.Text;
        private string Database => DatabaseNameTextBox.Text;
        private string SqlUser => SqlUserTextBox.Text;
        private string SqlPassword => SqlPasswordBox.Password;
        private string ConnectionString
        {
            get
            {
                //Data Source=.;Initial Catalog=InvDvp;Integrated Security=False;User ID=sa;Password=123
                if (string.IsNullOrEmpty(DataStore) || string.IsNullOrEmpty(SqlUser) || string.IsNullOrEmpty(SqlPassword)) return "";
                return $"\nData Source={DataStore};Integrated Security=False;User ID={SqlUser};Password={SqlPassword};{(string.IsNullOrEmpty(Database) ? "" : $"Initial Catalog={Database};")}TrustServerCertificate=True;";
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            Settings.Default.SettingChanging += Default_SettingChanging;
            Settings.Default.SettingsSaving += Default_SettingsSaving;

        }

        private void Default_SettingsSaving(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Default_SettingChanging(object sender, System.Configuration.SettingChangingEventArgs e)
        {
           
        }


        #region Methods

        #region Events

        protected async void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {

            if (!TestConnectionButton.IsEnabled) return;

            try
            {
                TestConnectionButton.IsEnabled = false;

                var connectionError = await CheckDatabaseConnectionAsync();

                if (connectionError == null)
                {
                    MessageBox.Show("Connection successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(connectionError.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                TestConnectionButton.IsEnabled=true;
            }
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("MainTabControl_SelectionChanged");

            foreach (var item in e.AddedItems)
            {
                if (item is TabItem tabItem)
                {
                    switch (tabItem.Header)
                    {
                        case TabItems.Files:
                            LoadFiles();
                            break;
                        case TabItems.LogConfiguration:
                           LoadLogConfiguration();
                            break;
                        case TabItems.DatabaseSettings:
                            LoadDbSettings();
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is TabItem tabItem) 
                {
                    switch (tabItem.Header)
                    {
                        case TabItems.Files:
                            SaveFiles();
                            break;
                        case TabItems.LogConfiguration:
                            SaveLogConfiguration();
                            break;
                        case TabItems.DatabaseSettings:
                            SaveDbSettings();
                            break;
                        default:
                            break;
                    }

                }
            }

        }

        private void DbSettings_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConnectionStingTextBlock.Text = ConnectionString;
        }



        private void SqlPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ConnectionStingTextBlock.Text = ConnectionString;
        }

        #endregion

        private Task<Exception?> CheckDatabaseConnectionAsync()
        {
            string connectionString = ConnectionString;
            return Task.Run(() =>
            {
                try
                {
                    //Task.Delay(3000).Wait();
                    
                    
                    using var connection = new SqlConnection(connectionString);
                    connection.Open();
                    return null; // Connection succeeded
                }
                catch (Exception ex) 
                {
                    return ex; // Connection failed
                }
            });
        }

        private void ReadAndConvertAndInsert()
        {
            var path = "";
            IEnumerable<string> lines = new FileReader(path).Read();
            foreach (string item in lines)
            {
                var convertor = new LogConvertor();
                var data = convertor.Convert(item);

            }
        }


        private void LoadFiles()
        {

           

        }

        private void SaveFiles()
        {

        }

        private void LoadLogConfiguration()
        {


        }

        private void SaveLogConfiguration()
        {

        }


        private void LoadDbSettings()
        {
            ServerNameTextBox.Text = Settings.Default[nameof(DataStore)] as string;
            SqlUserTextBox.Text = Settings.Default[nameof(SqlUser)] as string;
            SqlPasswordBox.Password = Settings.Default[nameof(SqlPassword)] as string;
            DatabaseNameTextBox.Text = Settings.Default[nameof(Database)] as string;

        }

        private void SaveDbSettings()
        {
            Settings.Default[nameof(DataStore)] = DataStore;
            Settings.Default[nameof(SqlUser)] = SqlUser;
            Settings.Default[nameof(SqlPassword)] = SqlPassword;
            Settings.Default[nameof(Database)] = Database;
            Settings.Default.Save();
        }


        #endregion

        #region Nested Classes

        static class TabItems
        {
            public const string Files = "Files";
            public const string LogConfiguration= "Log Configuration";
            public const string DatabaseSettings = "Database Settings";
        }

        #endregion





    }
}