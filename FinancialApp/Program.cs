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
        static void Main(string[] args)
        {
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
