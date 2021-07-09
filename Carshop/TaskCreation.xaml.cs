using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data;
using TaskManager.OracleConnectionClasses;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace TaskManager
{

    public partial class TaskCreation : Window
    {
        private string selectedValue;
        public static bool mark = false;
        public TaskCreation()
        {
            InitializeComponent();
        }

        private void AddBut_Click(object sender, RoutedEventArgs e)
        {

            if  (surnameTB.Text == "" || nameTB.Text == "" || ageTB.Text == ""  ||  phoneTB.Text == "")
            {
                MessageBox.Show("All fields are required to enter!");
                return;
            }

            if (!mark)
            {
                MessageBox.Show("You will not be able to update until you correct the errors!\nPlease, try again");
                surnameTB.Text = ""; nameTB.Text = "";  phoneTB.Text = ""; ageTB.Text = ""; 
            }
            else
            {

            




                using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SYSADMIN", "SYSADMINPASSWORD"))
                {
                connection.Open();

                OracleParameter p_id = new OracleParameter
                {
                    ParameterName = "p_id",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Int32,
                    Value = AdminWindow.CurrentUserId

                };
                OracleParameter p_name = new OracleParameter
                {
                    ParameterName = "p_name",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.NVarchar2,
                    Value = nameTB.Text
                };
                OracleParameter p_surname = new OracleParameter
                {
                    ParameterName = "p_surname",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.NVarchar2,
                    Value = surnameTB.Text
                };
                OracleParameter p_age = new OracleParameter
                {
                    ParameterName = "p_age",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.Int32,
                    Value = Convert.ToInt32(ageTB.Text)
                };
                OracleParameter p_phone_number = new OracleParameter
                {
                    ParameterName = "p_phone_number",
                    Direction = ParameterDirection.Input,
                    OracleDbType = OracleDbType.NVarchar2,
                    Value = phoneTB.Text
                };
                    using (OracleCommand command = new OracleCommand("Ivan.UpdateClient", connection))
                    {


                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { p_id, p_name, p_surname, p_age, p_phone_number });
                        command.ExecuteNonQuery();
                        MessageBox.Show("User has been updated!");
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();

                        this.Close();


                    }
                }
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton item)
            {
                selectedValue = item.Content.ToString();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
       
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow admin = new AdminWindow();
            admin.Show();
            this.Close();
        }

        private void MinWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        

        private void datePR_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
      
        }
      
        private void Window_Closed(object sender, EventArgs e)
        {
            
        }



        private void NameTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Space || e.Key == Key.Tab)
            {
                e.Handled = true;
                return;

            }
            mark = true;
        }

        private void NameTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {


            if (Regex.IsMatch(e.Text, @"\d{1}|\s{1}|\W{1}"))
            {
                e.Handled = true;
                return;

            }
            mark = true;
        }

        private void NameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
                mark = false;
                return;
            }

            if ((sender as TextBox).Text.Length < 2)
            {
                MessageBox.Show(" Name and surname cannot contain less than two characters!");

                mark = false;
                return;
            }

            if (!Regex.IsMatch((sender as TextBox).Text, @"^[A-ZА-Я]{1}\w*"))
            {
                MessageBox.Show("The first letter of NAME and SURNAME must be uppercase.\nPlease, check it");

                mark = false;
                return;
            }
            mark = true;
        }

        private void ageTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((sender as TextBox).Text.Length <= 2 && (sender as TextBox).Text.Length >= 0)
            {
                if (!Regex.IsMatch(e.Text, @"\d{1}"))
                {
                    e.Handled = true;
                    return;

                }
                mark = true;
            }
            else
            {
                MessageBox.Show("Invalid age length");
                ageTB.Text = "";
             
                mark = false;
                return;
            }
            mark = true;

        }



        private void phoneTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text.Length < 7)
            {
                MessageBox.Show("phone cannot contain less than 6 characters!");

                mark = false;
                return;
            }

            if (!Regex.IsMatch((sender as TextBox).Text, @"^(\+375|80)(29|25|44|33)(\d{3})(\d{2})(\d{2})$"))
            {
                MessageBox.Show("Invalid phone number.\nPlease, check it");
                phoneTB.Text = "";
                mark = false;
                return;
            }
            mark = true;


        }


    }
}
