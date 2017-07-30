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
        SqlConnection conn;
        SqlDataReader rdr;

        public Program()
        {
            conn = new SqlConnection("Data Source=GREG-BEE-2;Initial Catalog=spending;Integrated Security=True");
            rdr = null;
            conn.Open();
        }

        private void menu()
        {
            Console.WriteLine("Choose one of the following:\n");
            Console.WriteLine("1. Show all spending activity for current month");
            Console.WriteLine("2. Show all spending activity summarized for current month");
            Console.WriteLine("9. Quit program");

            Console.WriteLine("\n");
        }

        private void monthGeneral()
        {
            Console.WriteLine("Spending for Current Month:\n");
            DateTime today = DateTime.Today;
            Console.WriteLine(today.ToString("d")); //TODO need to get entire month
            SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date < '" + today.ToString("d") + "' ORDER BY date", conn);
            rdr = cmd.ExecuteReader();
            Console.WriteLine("\n");
            if (rdr.HasRows)
            {
                double total = 0;

                Console.WriteLine(String.Format("{0,-3} | {1,-8} | {2,-10} | {3,-15} | {4,-10}", "ID", " SPENT", "CATEGORY", "PAYMENT TYPE", "DATE"));

                while (rdr.Read())
                {
                    total += Convert.ToDouble(rdr[1]);
                    Console.WriteLine(String.Format(
                        "{0,-3} | {1,-8} | {2,-10} | {3,-15} | {4,-10}", 
                        rdr[0], " $" + rdr[1], rdr[2], rdr[3], Convert.ToDateTime(rdr[4]).ToString("MM/dd/yyyy")));
                }

                Console.WriteLine("\nTotal Spent $" + total);
                    
            }
            else
                Console.WriteLine("No results");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n---------- Financial App - Personal Spending Summary ----------\n");

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
                        p.monthGeneral();
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
            

            p.rdr.Close();
            p.conn.Close();

            Console.ReadLine();
            
        }
    }
}
