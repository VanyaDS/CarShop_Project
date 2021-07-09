using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;
using System;
using Oracle.ManagedDataAccess.Client;
using TaskManager.OracleConnectionClasses;
using System.Data;

namespace TaskManager
{
    
    public partial class Registration : Window
    {
            
    public static string CurrentId { get; private set; }
        private static bool mark = false;

        private string SelectedRadioButValue { get; set; }
        public Registration()
        {
            InitializeComponent();
        }

      

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passTB.Password == "" || loginTB.Text == "" || SurnameTB.Text == "" || NameTB.Text == "" || ageTB.Text == "" || patronymicTB.Text == "" || adressTB.Text == "" || phoneTB.Text == "")
                {
                    MessageBox.Show("All fields are required to enter!");
                    return;
                }

                if (!mark)
                {
                    MessageBox.Show("You will not be able to register until you correct the errors!\nPlease, try again");
                    SurnameTB.Text = ""; NameTB.Text = "";  passTB.Password = ""; loginTB.Text = ""; ageTB.Text = ""; patronymicTB.Text = ""; adressTB.Text = ""; phoneTB.Text = "";
                }
                else
                {
                    using (OracleConnection connection = DBUtils.GetDBConnection("192.168.56.104", 1521, "Car_shop", "SYSADMIN", "SYSADMINPASSWORD"))
                    {
                        connection.Open();
                        using (OracleCommand command = new OracleCommand("Ivan.AddClient", connection))
                        {


                            command.CommandType = CommandType.StoredProcedure;

                            OracleParameter name = new OracleParameter
                            {
                                ParameterName = "p_name",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = NameTB.Text
                            };
                            OracleParameter surname = new OracleParameter
                            {
                                ParameterName = "p_surname",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = SurnameTB.Text
                            };
                            OracleParameter patronymic = new OracleParameter
                            {
                                ParameterName = "p_patronymic",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = patronymicTB.Text
                            };
                            OracleParameter adress = new OracleParameter
                            {
                                ParameterName = "p_adress",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = adressTB.Text
                            };
                            OracleParameter age = new OracleParameter
                            {
                                ParameterName = "p_age",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.Int32,
                                 Value = Convert.ToInt32(ageTB.Text)
                               
                            };

                            OracleParameter phone_number = new OracleParameter
                            {
                                ParameterName = "p_phone_number",
                                Direction = ParameterDirection.Input,
                                OracleDbType = OracleDbType.NVarchar2,
                                Value = phoneTB.Text
                            };
                            OracleParameter new_id = new OracleParameter
                            {
                                ParameterName = "new_id",
                                Direction = ParameterDirection.Output,
                                OracleDbType = OracleDbType.Int32,
                                Value = 0
                           
                            };

                            command.Parameters.AddRange(new OracleParameter[] { name, surname, patronymic, adress, age, phone_number, new_id });
                            command.ExecuteNonQuery();
                            CurrentId = command.Parameters["new_id"].Value.ToString();

                            using (OracleCommand command2 = new OracleCommand("Ivan.AddAccout", connection))
                            {
                                command2.CommandType = CommandType.StoredProcedure;

                                OracleParameter p_client_id = new OracleParameter
                                {
                                    ParameterName = "p_client_id",
                                    Direction = ParameterDirection.Input,
                                    OracleDbType = OracleDbType.Int32,
                                    Value = Convert.ToInt32(CurrentId)
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


                                command2.Parameters.AddRange(new OracleParameter[] { p_client_id, p_login, p_password_value });
                                command2.ExecuteNonQuery();


                            }



                        }


                        Authorization avatar = new Authorization();
                        avatar.Show();
                        this.Close();
                        mark = false;

                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton item)
            {
                SelectedRadioButValue = item.Content.ToString();
            }
        }

        private void NameTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key==Key.Space || e.Key==Key.Tab)  
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
            if((sender as TextBox).Text=="")
            {
                mark = false;
                return;
            }
                      
                if ((sender as TextBox).Text.Length < 2)
                {
                    MessageBox.Show(" Name, surname and patronymic cannot contain less than two characters!");
                    
                mark = false;
                return;
                }

                if (!Regex.IsMatch((sender as TextBox).Text, @"^[A-ZА-Я]{1}\w*"))
                {
                    MessageBox.Show("The first letter of your NAME, SURNAME and PATRONIMYC must be uppercase.\nPlease, check it");
                   
                mark = false;
                return;
                }
            mark = true;
            }
        private void loginTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text.Length < 2)
            {
                MessageBox.Show("phone cannot contain less than 6 characters!");

                mark = false;
                return;
            }

            if (!Regex.IsMatch((sender as TextBox).Text, @"^[a-zA-Z][a-zA-Z0-9-_\.]{1,30}$"))
            {
                MessageBox.Show("Invalid login.\nPlease, check it");
                phoneTB.Text = "";
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

        private void CloseBut_Click(object sender, RoutedEventArgs e)
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

        private void adressTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           
        }

        private void adressTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text.Length < 2)
            {
                MessageBox.Show("Adress cannot contain less than 2 characters!");

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

        private void ageTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((sender as TextBox).Text.Length <=2 && (sender as TextBox).Text.Length >= 0)
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
    }
 }

