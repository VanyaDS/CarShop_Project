using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using TaskManager.OracleConnectionClasses;

namespace TaskManager
{

    public partial class AdminWindow : Window
    {
        private static BindingList<Clients> GridInfo = new BindingList<Clients>();
        public static int CurrentUserId { get; private set; }
     

        public AdminWindow()
        {

            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GridInfo.Clear();

            using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SYSADMIN", "SYSADMINPASSWORD"))
            {
                connection.Open();

                OracleParameter cur = new OracleParameter
                {
                    ParameterName = "cur",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor

                };

                using (OracleCommand command = new OracleCommand("Ivan.GetClients", connection))
                {


                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new OracleParameter[] { cur });
                    var reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow row in dt.Rows)
                    {
                        GridInfo.Add(new Clients(row["client_id"].ToString(), row["name"].ToString(), row["surname"].ToString(), row["age"].ToString(), row["login"].ToString(), row["phone_number"].ToString()));
                    }



                }
            }

            UserGrid.ItemsSource = GridInfo;

        }
        private void BlockBut_Click(object sender, RoutedEventArgs e)
        {
            if (GridInfo.Count < 1)
            {
                MessageBox.Show("No user to update");
                return;
            }
            if (UserGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Select the user to update");
                return;
            }
            CurrentUserId = Convert.ToInt32(((Clients)UserGrid.SelectedItem).Client_id);

            TaskCreation taskCreation = new TaskCreation();
            taskCreation.Show();
            this.Close();
        }

        private void findTB_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
        


        }

        private void findTB_GotFocus(object sender, RoutedEventArgs e)
        {

            resBN.Visibility = Visibility.Hidden;
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ExitAcc_Click(object sender, RoutedEventArgs e)
        {
            Authorization.SetNullLogin();
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {


            resBN.Visibility = Visibility.Visible;

        }

    }
}