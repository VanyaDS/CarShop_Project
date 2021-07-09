using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Data;
using System.ComponentModel;
using Oracle.ManagedDataAccess.Client;
using TaskManager.OracleConnectionClasses;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace TaskManager
{

    public partial class MainWindow : Window
    {

        private static int index;
        public static BindingList<Cars> GridInfo = new BindingList<Cars>();
        private static BindingList<Cars> SearchGridInfo = new BindingList<Cars>();
        

        public MainWindow()
        {
       
            InitializeComponent();


        }
        
 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            
           
            GridInfo.Clear();
            using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SHOP_CLIENT", "CLIENTPASSWORD"))
            {
                connection.Open();

                OracleParameter cur = new OracleParameter
                {
                    ParameterName = "cur",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor

                };

                using (OracleCommand command = new OracleCommand("Ivan.GetCars", connection))
                {


                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new OracleParameter[] { cur });
                    var reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow row in dt.Rows)
                    {

                        // CurrentId = row["client_id"].ToString();

                        GridInfo.Add(new Cars(row["id"].ToString(), row["brand"].ToString(), row["color"].ToString(), row["seats_number"].ToString(), row["transmission"].ToString(), row["adress"].ToString(), row["cost"].ToString()));
                        

                    }

                   

                }
            }
        
            TaskGrid.ItemsSource = GridInfo;
            GridInfo.ListChanged += GridInfo_ListChanged;
        }

        private void GridInfo_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemMoved)
            {

            }
        }



        private void TaskGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            index = TaskGrid.SelectedIndex;

        }

        private void TaskGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
       {
            
       }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
         
           

        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
           
            SearchGridInfo.Clear();

            using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SHOP_CLIENT", "CLIENTPASSWORD"))
            {
                connection.Open();

                OracleParameter cur = new OracleParameter
                {
                    ParameterName = "cur",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor

                };

                OracleParameter super_param = new OracleParameter
                {
                    ParameterName = "super_param",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.NVarchar2,
                    Value = findTB.Text
                };

                using (OracleCommand command = new OracleCommand("Ivan.GetCarsSearch", connection))
                {


                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new OracleParameter[] { super_param, cur });
                    var reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow row in dt.Rows)
                    {
                        SearchGridInfo.Add(new Cars(row["id"].ToString(), row["brand"].ToString(), row["color"].ToString(), row["seats_number"].ToString(), row["transmission"].ToString(), row["adress"].ToString().ToLower(), row["cost"].ToString()));

                    }



                }
            }

            TaskGrid.ItemsSource = SearchGridInfo;

            resBN.Visibility = Visibility.Visible;

        }

        private void findTB_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (findTB.Text == "")
            {
                TaskGrid.ItemsSource = GridInfo;
                resBN.Visibility = Visibility.Hidden;
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            findTB.Text = "";
            TaskGrid.ItemsSource = GridInfo;
            resBN.Visibility = Visibility.Hidden;


        }

        private void findTB_GotFocus(object sender, RoutedEventArgs e)
        {
            resBN.Visibility = Visibility.Hidden;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void resBN2_Click(object sender, RoutedEventArgs e)
        {
            resBN2.Visibility = Visibility.Hidden;
        }

      

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void DelAll_Click(object sender, RoutedEventArgs e)
        {

        }


        private void ExitAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            Authorization.SetNullLogin();
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (OwnedWindows.Count == 0)
            {
                AvatarComplete avatar = new AvatarComplete();
                avatar.Owner = this;
                avatar.Show();
            }
          
     
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {


            if (GridInfo.Count < 1)
            {
                MessageBox.Show("No car to order");
                return;
            }
            if (TaskGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Select a car to order");
                return;
            }

            List<string> salesman_Id = new List<string>();
            
            
            using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SHOP_CLIENT", "CLIENTPASSWORD"))
                {

                        
                connection.Open();

                using (OracleCommand command1 = new OracleCommand("Ivan.GetSalesmen", connection))
                {
                    command1.CommandType = CommandType.StoredProcedure;

                    OracleParameter cur = new OracleParameter
                    {
                        ParameterName = "cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor

                    };

                    command1.Parameters.AddRange(new OracleParameter[] { cur });
                    
                    DataTable dt = new DataTable();
                    var reader = command1.ExecuteReader();
                    dt.Load(reader);

                    foreach (DataRow row in dt.Rows)
                    {
                        salesman_Id.Add(row["personnel_number"].ToString());


                    }

                }





                Random random = new Random();
                using (OracleCommand command2 = new OracleCommand("Ivan.AddOrder", connection))
                    {
                        command2.CommandType = CommandType.StoredProcedure;

                    OracleParameter p_worker_id = new OracleParameter
                    {
                        ParameterName = "p_worker_id",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(salesman_Id[random.Next(0, salesman_Id.Count-1)])
                    };
                    OracleParameter p_client_id = new OracleParameter
                    {
                        ParameterName = "p_client_id",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(Authorization.CurrentId)
                        };
                    OracleParameter p_ordering_date = new OracleParameter
                    {
                        ParameterName = "p_ordering_date",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = DateTime.Today
                        };
                    OracleParameter p_car_id = new OracleParameter
                    {
                        ParameterName = "p_car_id",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(((Cars)TaskGrid.SelectedItem).Id)
                    };

                    command2.Parameters.AddRange(new OracleParameter[] { p_worker_id, p_client_id, p_ordering_date, p_car_id });
                    command2.ExecuteNonQuery();

                    
                  
                }

                OracleParameter cur2 = new OracleParameter
                {
                    ParameterName = "cur",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor

                };

                using (OracleCommand command = new OracleCommand("Ivan.GetCars", connection))
                {


                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(new OracleParameter[] { cur2 });
                    var reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    GridInfo.Clear();
                    foreach (DataRow row in dt.Rows)
                    {

        

                        GridInfo.Add(new Cars(row["id"].ToString(), row["brand"].ToString(), row["color"].ToString(), row["seats_number"].ToString(), row["transmission"].ToString(), row["adress"].ToString(), row["cost"].ToString()));


                    }
                    TaskGrid.ItemsSource = GridInfo;


                }
                MessageBox.Show("You have ordered a car!");

            }
            
        
        }
    }
  }
