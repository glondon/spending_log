using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FinancialApp
{
    class Program
    {
        public void menu()
        {
            Console.WriteLine("Choose one of the following:\n");
            Console.WriteLine("1. Show all spending activity for current month");
            Console.WriteLine("2. Show all spending activity summarized for current month");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n---------- Financial App - Personal Spending Summary ----------\n");
            Program p = new Program();
            p.menu();

            SqlConnection conn = new SqlConnection("Data Source=GREG-BEE-2;Initial Catalog=spending;Integrated Security=True");
            SqlDataReader rdr = null;
 
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM personal", conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
                Console.WriteLine(rdr[0] + " $" + rdr[1]);

            rdr.Close();
            conn.Close();

            Console.ReadLine();
            
        }
    }
}
