using Oracle.ManagedDataAccess.Client;
using System;


namespace TaskManager.OracleConnectionClasses
{
    class DBUtils
    {

        public static OracleConnection GetDBConnection(string host, int port, string service_name, string user, string password)
        {
            return DBOracleUtils.GetDBConnection(host, port, service_name, user, password);
        }
    }

    class DBOracleUtils
    {

        public static OracleConnection
            GetDBConnection(string host, int port, String service_name, String user, String password)
        {



            string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                 + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                 + service_name + ")));Password=" + password + ";User ID=" + user;


            OracleConnection conn = new OracleConnection();

            conn.ConnectionString = connString;

            return conn;
        }

    }





    public class Cars
    {
        private string id;
        private string brand;
        private string color;
        private string seats_number;
        private string transmission;
        private string adress;
        private string cost;



        public string Id { get => id; set => id = value; }
        public string Brand { get => brand; set => brand = value; }
        public string Color { get => color; set => color = value; }
        public string Seats_number { get => seats_number; set => seats_number = value; }
        public string Transmission { get => transmission; set => transmission = value; }
        public string Adress { get => adress; set => adress = value; }
        public string Cost { get => cost; set => cost = value; }


        public Cars(string id, string brand, string color, string seats_number, string transmission, string adress, string cost)
        {
            Id = id;
            Brand = brand;
            Color = color;
            Seats_number = seats_number;
            Transmission = transmission;
            Adress = adress;
            Cost = cost;
        }


    }



    public class Clients
    {
        private string name;
        private string surname;
        private string age;
        private string login;
        private string phone_number;
        private string client_id;




        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Age { get => age; set => age = value; }
        public string Login { get => login; set => login = value; }
        public string Phone_number { get => phone_number; set => phone_number = value; }
        public string Client_id { get => client_id; set => client_id = value; }

        public Clients(string client_id, string name, string surname, string age, string login, string phone_number)
        {
            Client_id = client_id;
            Name = name;
            Surname = surname;
            Age = age;
            Login = login;
            Phone_number = phone_number;
        }
    }
}


