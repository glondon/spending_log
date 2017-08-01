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
            Console.WriteLine("3. Show summary results based on credit card statements");
            Console.WriteLine("4. Insert a new expense");
            Console.WriteLine("5. Update an expense");
            Console.WriteLine("6. Delete an expense");
            Console.WriteLine("7. Show year summary results");
            Console.WriteLine("9. Quit program");
            Console.WriteLine();
        }

        private void monthGeneral()
        {
            Console.WriteLine("Spending for Current Month:\n");

            SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date >= '" + getMonthBegin() + "' ORDER BY date", conn);
            rdr = cmd.ExecuteReader();
            Console.WriteLine();
            if (rdr.HasRows)
            {
                double total = 0;
                const string format = "{0,-3} | {1,-8} | {2,-10} | {3,-15} | {4,-10}";
                Console.WriteLine(String.Format(format, "ID", " SPENT", "CATEGORY", "PAYMENT TYPE", "DATE"));

                while (rdr.Read())
                {
                    total += Convert.ToDouble(rdr[1]);
                    Console.WriteLine(String.Format(format,
                        rdr[0], " $" + rdr[1], rdr[2], rdr[3], Convert.ToDateTime(rdr[4]).ToString("MM/dd/yyyy")));
                }

                Console.WriteLine("\nTotal Spent $" + total);

            }
            else
                Console.WriteLine("No results");
        }

        private void monthSummary()
        {
            Console.WriteLine("Spending Summary for Current Month:\n");
            Console.WriteLine();

            SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date >= '" + getMonthBegin() + "' ORDER BY date", conn);
            rdr = cmd.ExecuteReader();
            
            if (rdr.HasRows)
            {
                double totalDining = 0;
                double totalAlcohol = 0;
                double totalGasoline = 0;
                double totalFood = 0;

                while (rdr.Read())
                {
                    switch(rdr[2].ToString()){
                        case "dining out":
                            totalDining += Convert.ToDouble(rdr[1]);
                            break;
                        case "alcohol":
                            totalAlcohol += Convert.ToDouble(rdr[1]);
                            break;
                        case "gasoline":
                            totalGasoline += Convert.ToDouble(rdr[1]);
                            break;
                        case "food":
                            totalFood += Convert.ToDouble(rdr[1]);
                            break;
                    }
                    
                }

                double[] total = {totalDining, totalAlcohol, totalGasoline, totalFood};

                const string format = "{0,-10} {1,-15}";
                Console.WriteLine(format, "$" + totalDining, "dining out");
                Console.WriteLine(format, "$" + totalAlcohol.ToString("0.00"), "alcohol");
                Console.WriteLine(format, "$" + totalGasoline, "gasoline");
                Console.WriteLine(format, "$" + totalFood.ToString("0.00"), "food");

                Console.WriteLine("\nTotal Spent $" + total.Sum());

            }
            else
                Console.WriteLine("No results");
        }

        private string getMonthBegin()
        {
            string monthStart;

            DateTime date = DateTime.Now;
            DateTime monthBegin = new DateTime(date.Year, date.Month, 1);
            monthStart = monthBegin.ToString("d");

            return monthStart;
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
                        p.monthSummary();
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
