using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;
using System.Text;
using System;
using TaskManager.OracleConnectionClasses;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace TaskManager
{

    public partial class Authorization : Window
    {
        private static bool mark = false;
        public static string CurrentId { get; private set; }

        public static void SetNullLogin()
        {
            CurrentId = null;
        }
        public Authorization()
        {
            InitializeComponent();
        }

        private void regTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            regTextBlock.Foreground = new SolidColorBrush(Colors.Aqua);
            regTextBlock.TextDecorations = TextDecorations.Underline;
        }

        private void regTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            regTextBlock.Foreground = new SolidColorBrush(Colors.White);
            regTextBlock.TextDecorations = null ;
        }

       
        private void regTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Close();
        }

        private void LoginBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (loginTB.Text == "" || passTB.Password == "")
                {
                    MessageBox.Show("Login AND password are required to enter!");
                    return;
                }

                if (!mark)
                {
                    MessageBox.Show("You will not be able to register until you correct the errors!\nPlease, try again");
                    passTB.Password = ""; loginTB.Text = "";

                }
                else
                {
                    ulong personnelNumber = 0;

                    if (ulong.TryParse(loginTB.Text, out personnelNumber))
                    {

                        using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SYSADMIN", "SYSADMINPASSWORD"))
                        {
                            connection.Open();

                            OracleParameter cur = new OracleParameter
                            {
                                ParameterName = "cur",
                                Direction = ParameterDirection.Output,
                                OracleDbType = OracleDbType.RefCursor

                            };
                            OracleParameter p_personnel_number = new OracleParameter
                            {
                                ParameterName = "p_personnel_number",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = loginTB.Text
                            };
                            OracleParameter p_password_value = new OracleParameter
                            {
                                ParameterName = "p_password_value",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = passTB.Password
                            };
                            using (OracleCommand command = new OracleCommand("Ivan.GetCurrentWorker", connection))
                            {


                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddRange(new OracleParameter[] { p_personnel_number, p_password_value, cur });
                                var reader = command.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                if (dt.Rows.Count == 0)
                                {
                                    MessageBox.Show("worker does not exist");
                                    return;
                                }
                                foreach (DataRow row in dt.Rows)
                                {

                                    CurrentId = row["personnel_number"].ToString();


                                }

                                AdminWindow mainWindow = new AdminWindow();
                                mainWindow.Show();
                                this.Close();

                            }
                        }




                    }
                    else
                    {


                        using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SYSADMIN", "SYSADMINPASSWORD"))
                        {
                            connection.Open();

                            OracleParameter cur = new OracleParameter
                            {
                                ParameterName = "cur",
                                Direction = ParameterDirection.Output,
                                OracleDbType = OracleDbType.RefCursor

                            };
                            OracleParameter p_login = new OracleParameter
                            {
                                ParameterName = "p_login",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = loginTB.Text
                            };
                            OracleParameter p_password_value = new OracleParameter
                            {
                                ParameterName = "p_password_value",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = passTB.Password
                            };
                            using (OracleCommand command = new OracleCommand("Ivan.GetCurrentUser", connection))
                            {


                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddRange(new OracleParameter[] { p_login, p_password_value, cur });
                                var reader = command.ExecuteReader();
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                if (dt.Rows.Count == 0)
                                {
                                    MessageBox.Show("user does not exist");
                                    return;
                                }
                                foreach (DataRow row in dt.Rows)
                                {

                                    CurrentId = row["client_id"].ToString();


                                }

                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                this.Close();

                            }
                        }

                    }



                }


             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void loginTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Tab)
            {
                e.Handled = true;
                return;

            }
            mark = true;
        }

        private void loginTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
            {
    
                mark = false;
                return;
            }

            if ((sender as TextBox).Text.Length < 2)
            {
                MessageBox.Show(" Login cannot contain less than two characters!");

                mark = false;
                return;
            }


            mark = true;
        }

        private void loginTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"\s{1}|\W{1}") && !Regex.IsMatch(e.Text, @"_"))
            {
                e.Handled = true;
                return;

            }
            mark = true;
        }

        private void passTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Tab)
            {
                e.Handled = true;
                return;

            }
            mark = true;
        }

        private void passTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"\s{1}|\W{1}") && !Regex.IsMatch(e.Text, @"_"))
            {
                e.Handled = true;
                return;

            }
            mark = true;
        }

        private void passTB_LostFocus(object sender, RoutedEventArgs e)
        {

            if ((sender as PasswordBox).Password == "")
            {
            
                mark = false;
                return;
            }

            if ((sender as PasswordBox).Password.Length < 6)
            {
                MessageBox.Show(" Password cannot contain less than six characters!");

                mark = false;
                return;
            }

            if (!Regex.IsMatch((sender as PasswordBox).Password, @"\w*[A-ZА-Я]{1}\w*"))
            {
                MessageBox.Show("One character of your PASSWORD must be uppercase.\nPlease, check it");

                mark = false;
                return;
            }

            if (!Regex.IsMatch((sender as PasswordBox).Password, @"\w*[0-9]{1}\w*"))
            {
                MessageBox.Show("One character of your PASSWORD must be number.\nPlease, check it");

                mark = false;
                return;
            }


            mark = true;
        }
        private void MinWindowBut_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider(); 
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty; 
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);
            return hash;
        }

    }
}
