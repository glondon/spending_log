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
            Console.WriteLine("9. Quit program");

            Console.WriteLine("\n");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n---------- Financial App - Personal Spending Summary ----------\n");

            SqlConnection conn = new SqlConnection("Data Source=GREG-BEE-2;Initial Catalog=spending;Integrated Security=True");
            SqlDataReader rdr = null;
            conn.Open();

            Program p = new Program();
            p.menu();

            string menuItem = Console.ReadLine();

            //TODO add while statement for input loop
            
            int intCheck;
            if (int.TryParse(menuItem, out intCheck))
            {
                intCheck = Int32.Parse(menuItem);

                switch (intCheck)
                {
                    case 1:
                        Console.WriteLine("Spending for Current Month:\n");
                        DateTime today = DateTime.Today;
                        Console.WriteLine(today.ToString("d")); //TODO need to get entire month
                        SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date = '" + today.ToString("d") + "' ORDER BY date", conn);
                        rdr = cmd.ExecuteReader();
                        Console.WriteLine("\n");
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                                Console.WriteLine(rdr[0] + " $" + rdr[1] + " " + rdr[2] + " " + rdr[3]);
                        }
                        else
                            Console.WriteLine("No results");
                        
                        break;
                    case 2:
                        Console.WriteLine("You chose 2");
                        break;
                    case 9:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Not a valid choice");
                        break;
                }
            }
            else
                Console.WriteLine("Not a valid integer\n");
            

            rdr.Close();
            conn.Close();

            Console.ReadLine();
            
        }
    }
}
