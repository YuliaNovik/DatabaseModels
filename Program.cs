using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels
{
    class Program
    {
        static string GetConnectionString(string connectionStringName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfiguration config = configurationBuilder.Build();
            return config["ConnectionStrings:" + connectionStringName];
        }

        static void ProductInfo(string _h, string _l, string _m)
        {
            string cs = GetConnectionString("AdventureWorksLT2017");
            
            string query = "SELECT TOP (10)  ProductID,  Name, ProductNumber, Color, StandardCost  " +
                "FROM SalesLT.Product  WHERE Name LIKE @h OR Name LIKE @l OR Name Like @m ";

            using (SqlConnection conn = new SqlConnection(cs)) { 

            SqlCommand cmd = new SqlCommand(query,conn);

                cmd.Parameters.AddWithValue("h", _h);
                cmd.Parameters.AddWithValue("l", _l);
                cmd.Parameters.AddWithValue("m", _m);
                conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("CONNECTED - READ PRODUCT INFORMATION\n\n\n" +
                       "      ID          Name                       Number         Color             Cost\n " +
                       "-----------------------------------------------------------------------------------\n");

                while (reader.Read())
             {
                    int id = (int)reader["ProductID"];
                    string name = (string)reader["Name"];
                    string productNum = (string)reader["ProductNumber"];
                    string color = (string)reader["Color"].ToString();
                    decimal cost = (decimal)reader["StandardCost"];

                   
                    Console.WriteLine(
                        $"{id,8} {name,-35} {productNum,-15} {color, -15}  {cost,-20}");

                }
             
            }
           
        }


        static void CustomerAddress()
        {
            string cs = GetConnectionString("AdventureWorksLT2017");

            string query = "SELECT TOP 20 FirstName, AddressType FROM SalesLT.Customer " +
                "JOIN SalesLT.CustomerAddress ON SalesLT.Customer.CustomerID = SalesLT.CustomerAddress.CustomerID ";

            using (SqlConnection conn = new SqlConnection(cs))
            {

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("CONNECTED - READ CUSTOMER ADDRESS\n\n\n" +
                       "  Name                    Address\n " +
                         "---------------------------------\n");

                while (reader.Read())
                {
                    string fName = (string)reader["FirstName"];
                    string address = (string)reader["AddressType"];
                    


                    Console.WriteLine(
                        $"{fName,-25} {address,-50}");

                }

            }

        }




        static void Sum()
        {
            string cs = GetConnectionString("AdventureWorksLT2017");

            string query = "SELECT SUM(StandardCost) FROM SalesLT.Product";

            using (SqlConnection conn = new SqlConnection(cs))
            {

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                decimal sum = (decimal)cmd.ExecuteScalar();
                decimal sum1 = Math.Round(sum, 2);

                Console.WriteLine("CONNECTED - CALCULATE PRODUCT COST INFORMATION\n\n\n" +
                      $"Total Standard Product Cost:  ${sum1}");

            }

        }



        static void ProductInfoDisconnected()
        {
            string cs = GetConnectionString("AdventureWorksLT2017");

            string query = "SELECT TOP(10)   ProductID,  Name, ProductNumber, Color, StandardCost  " +
                "FROM SalesLT.Product  WHERE Name LIKE 'H%' OR Name LIKE 'M%' OR Name Like 'L%'  ";

            Console.WriteLine("DISCONNECTED - READ PRODUCT INFORMATION\n\n\n" +
                     "      ID          Name                       Number             Cost\n " +
                     "------------------------------------------------------------------------\n");

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Product");
                DataTable tblProduct = ds.Tables["Product"];

                foreach (DataRow row in tblProduct.Rows)
                {
                    Console.WriteLine($"{row["ProductID"],8} {row["Name"],-35} {row["ProductNumber"],-15} {row["StandardCost"],-10}");
                }

            }

        }


        static void CustomerAddressDisconnected()
        {
            string cs = GetConnectionString("AdventureWorksLT2017");

            string query = "SELECT TOP 20 FirstName, AddressType FROM SalesLT.Customer " +
                "JOIN SalesLT.CustomerAddress ON SalesLT.Customer.CustomerID = SalesLT.CustomerAddress.CustomerID ";

            Console.WriteLine("DISCONNECTED - READ CUSTOMER ADDRESS\n\n\n" +
                     "   Name                    Adress\n " +
                     "-------------------------------------\n");

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable tblCustomer = ds.Tables[0];

                foreach (DataRow row in tblCustomer.Rows)
                {
                    Console.WriteLine($"{row["FirstName"],-25} {row["AddressType"],-50}");
                }

            }

        }


        static void SumDisconnected()
        {
            string cs = GetConnectionString("AdventureWorksLT2017");

            string query = "SELECT SUM(StandardCost) FROM SalesLT.Product";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "SalesLT.Product");
                DataTable tblProduct = ds.Tables["SalesLT.Product"];

                decimal sum1 = (decimal)tblProduct.Rows[0].ItemArray[0];
                decimal sum2 =  Math.Round(sum1, 2);

                Console.WriteLine("CONNECTED - CALCULATE PRODUCT COST INFORMATION\n\n\n" +
                    $"Total Standard Product Cost:  ${sum2 ,8}");

            }

        }




        static void Main(string[] args)
        {
             bool flag = true;
            while (flag)
            {

            Console.WriteLine("PLEASE SELECT ONE OF THE FOLLOWING OPTIONS\n" +
                               "-----------------------------------------------\n" +
                               "1 - Connected    - Read and Display Product Information\n" +
                               "2 - Connected    - Read and Display Customer Address Information\n" +
                               "3 - Connected    - Calculate Product Total Cost\n" +
                               "4 - Disconnected - Read and Display Product Information\n" +
                               "5 - Disconnected - Read and Display Customer Address Information\n" +
                               "6 - Disconnected    - Calculate Product Total Cost\n" +
                               "Q - Quit");
            
            string select = Console.ReadLine();

            if (select == "q" || select == "Q")
            {
                Environment.Exit(0);
            }
            else if (select == "1")
            {
                ProductInfo("H%", "L%", "M%");
            }
            else if (select == "2")
            {
                CustomerAddress();
            }

            else if (select == "3")
            {
                Sum();
            }

            else if (select == "4")
            {
                ProductInfoDisconnected();
            }

            else if (select == "5")
            {
                CustomerAddressDisconnected();
            }

            else if (select == "6")
            {
                SumDisconnected();
            }

            Console.ReadKey();

            }
        }
    }
}
