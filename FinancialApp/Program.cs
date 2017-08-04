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

        private const string dining = "dining out";
        private const string alcohol = "alcohol";
        private const string food = "food";
        private const string gasoline = "gasoline";

        private const string visaChase = "visa - chase";
        private const string masterUsaa = "master - usaa";
        private const string cash = "cash";

        private string[] paymentTypes = { visaChase, masterUsaa, cash };
        private string[] categories = { dining, alcohol, gasoline, food };

        public Program()
        {
            conn = new SqlConnection("Data Source=GREG-BEE-2;Initial Catalog=spending;Integrated Security=True");
            conn.Open();
        }

        private void menu()
        {
            Console.WriteLine("Choose one of the following:\n");
            Console.WriteLine("1.  Show all spending activity for current month");
            Console.WriteLine("2.  Show all spending activity summarized for current month");
            Console.WriteLine("3.  Show summary results based on credit card statements");
            Console.WriteLine("4.  Insert a new expense");
            Console.WriteLine("5.  Update an expense");
            Console.WriteLine("6.  Delete an expense");
            Console.WriteLine("7.  View year summary results");
            Console.WriteLine("8.  View today's spending");
            Console.WriteLine("9.  Quit program");
            Console.WriteLine("10. Show menu");
            Console.WriteLine("11. View a certain date");
            Console.WriteLine("12. View a particular month");
            Console.WriteLine("13. View a certain date range");
            Console.WriteLine();
        }

        private void monthGeneral()
        {
            Console.WriteLine("Spending for Current Month:\n");
            Console.WriteLine();

            SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date >= '" + getMonthBegin() + "' ORDER BY date", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            using (rdr) 
            {
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

            rdr.Close();
            
        }

        private void monthSummary()
        {
            Console.WriteLine("Spending Summary for Current Month:\n");
            Console.WriteLine();

            SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date >= '" + getMonthBegin() + "' ORDER BY date", conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            
            using(rdr)
            {
                if (rdr.HasRows)
                {
                    double totalDining = 0;
                    double totalAlcohol = 0;
                    double totalGasoline = 0;
                    double totalFood = 0;

                    while (rdr.Read())
                    {
                        switch (rdr[2].ToString())
                        {
                            case dining:
                                totalDining += Convert.ToDouble(rdr[1]);
                                break;
                            case alcohol:
                                totalAlcohol += Convert.ToDouble(rdr[1]);
                                break;
                            case gasoline:
                                totalGasoline += Convert.ToDouble(rdr[1]);
                                break;
                            case food:
                                totalFood += Convert.ToDouble(rdr[1]);
                                break;
                        }

                    }

                    double[] total = { totalDining, totalAlcohol, totalGasoline, totalFood };

                    const string format = "{0,-10} {1,-15}";
                    Console.WriteLine(format, "$" + totalDining, dining);
                    Console.WriteLine(format, "$" + totalAlcohol.ToString("0.00"), alcohol);
                    Console.WriteLine(format, "$" + totalGasoline, gasoline);
                    Console.WriteLine(format, "$" + totalFood.ToString("0.00"), food);

                    Console.WriteLine("\nTotal Spent $" + total.Sum());

                }
                else
                    Console.WriteLine("No results");
            }

            rdr.Close();
            
        }

        private void addExpense()
        {
            Console.WriteLine("Enter New Expense:\n");
            Console.WriteLine("Separate by commas: Ex -> Cost, Category, Payment Type, Date\n");

            bool validating = true;
            string[] separater = { "," };

            string newExpense = Console.ReadLine();
            
            string[] split = newExpense.Split(separater, StringSplitOptions.RemoveEmptyEntries);
            List<string> errors = new List<string>();

            while(validating)
            {
                if (split.Length != 4)
                    errors.Add("You must only enter 4 characters");
                else
                {
                    double doubleCheck;
                    DateTime temp;
                    double cost;
                    string category;
                    string payment;
                    DateTime date;

                    int index = 0;
                    foreach (var item in split)
                    {
                        switch(index)
                        {
                            case 0:
                                if (Double.TryParse(item.ToString().Trim(), out doubleCheck))
                                    cost = Convert.ToDouble(item.ToString().Trim());
                                else
                                    errors.Add("Cost must be of type double");
                                break;
                            case 1:
                                if (categories.Contains(item.ToString().Trim()))
                                    category = item.ToString().Trim();
                                else
                                    errors.Add("Category doesn't exist");
                                break;
                            case 2:
                                if (paymentTypes.Contains(item.ToString().Trim()))
                                    payment = item.ToString().Trim();
                                else
                                    errors.Add("Payment Type doesn't exist");
                                break;
                            case 3:
                                //TODO validate date
                                break;

                        }

                        index++;
                    }
                    
                    
                }

                //TODO fix - only showing 1 error... needs indexing again...
                if(errors.ToArray().Length > 0)
                {
                    foreach(var error in errors)
                        Console.WriteLine(error + "\n");

                    Console.WriteLine("Select option 4 to try again\n");

                    validating = false;
                    
                }
                else
                {
                    Console.WriteLine("Success...\n");
                }
            }
            
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

            while(true)
            {
                string menuItem = Console.ReadLine();
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
                        case 4:
                            p.addExpense();
                            break;
                        case 9:
                            Environment.Exit(0);
                            break;
                        case 10:
                            p.menu();
                            break;
                        default:
                            Console.WriteLine("Not a valid choice");
                            break;
                    }
                }
                else
                    Console.WriteLine("Not a valid integer\n");
            }

            p.conn.Close();

            Console.ReadLine();
            
        }
    }
}
