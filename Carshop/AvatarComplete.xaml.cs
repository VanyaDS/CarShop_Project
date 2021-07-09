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
using System.Windows.Shapes;

namespace TaskManager
{
    
    public partial class AvatarComplete : Window
    {
       
        public AvatarComplete()
        {
            InitializeComponent();
        }

        private void MinWindowBut_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

            if (Authorization.CurrentId == null)
            {
                Authorization authorization = new Authorization();
                authorization.Show();
                this.Close();
            }
            this.Close();
        }

        private void Ava_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{

            

            //if (Authorization.CurrentLogin != null)
            //{
            //    using (K_ProjectEntities db = new K_ProjectEntities())
            //    {
            //        UserSetting setting = db.UserSettings.Find(Authorization.CurrentLogin);
            //        if (setting != null)
            //        {
            //            setting.avatar = setting.avatar = ((Image)((sender as Button).Content)).Source.ToString();
            //            db.Entry(setting).State = EntityState.Modified;
            //            db.SaveChanges();
            //        }
            //        else
            //        {
            //            setting = new UserSetting();
            //            setting.login = Authorization.CurrentLogin;

            //            setting.avatar = ((Image)((sender as Button).Content)).Source.ToString();
            //            db.UserSettings.Add(setting);
            //            db.SaveChanges();
            //        }


            //        foreach (Window window in Application.Current.Windows)
            //        {
            //            if (window.GetType() == typeof(MainWindow))
            //            {
            //                (window as MainWindow).testIM.Source = new BitmapImage(new Uri(db.UserSettings.Find(Authorization.CurrentLogin).avatar));
            //            }
            //        }
            //    }
            //    this.Close();
            //}
            //else
            //{
            //    using (K_ProjectEntities db = new K_ProjectEntities())
            //    {
            //        UserSetting setting = new UserSetting();
            //        setting.login = Registration.CurrentLogin;

            //        setting.avatar = ((Image)((sender as Button).Content)).Source.ToString();

            //        db.UserSettings.Add(setting);
            //        db.SaveChanges();
            //    }

            //    Authorization authorization = new Authorization();
            //    authorization.Show();
            //    this.Close();

            //}

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);

            //}
        }
    }
}
