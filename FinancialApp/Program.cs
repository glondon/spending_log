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
        private const string toiletries = "toiletries";
        private const string clothing = "clothing";
        private const string entertainment = "entertainment";

        private const string visaChase = "visa - chase";
        private const string masterUsaa = "master - usaa";
        private const string cash = "cash";

        private string[] paymentTypes = { visaChase, masterUsaa, cash };
        private string[] categories = { dining, alcohol, gasoline, food, toiletries, clothing, entertainment };

        //master - usaa statement 18th of every month
        //visa - chase statement 15th of every month
        //visa - navyfcu statement 16th of every month

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

            using (SqlDataReader rdr = cmd.ExecuteReader()) 
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

                    rdr.Close();

                    Console.WriteLine("\nTotal Spent $" + total);

                }
                else
                    Console.WriteLine("No results");
            }
        }

        private void monthSummary()
        {
            Console.WriteLine("Spending Summary for Current Month:\n");
            Console.WriteLine();

            SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date >= '" + getMonthBegin() + "' ORDER BY date", conn);
            
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    double totalDining = 0;
                    double totalAlcohol = 0;
                    double totalGasoline = 0;
                    double totalFood = 0;
                    double totalClothing = 0;
                    double totalToiletries = 0;
                    double totalEntertainment = 0;

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
                            case toiletries:
                                totalToiletries = Convert.ToDouble(rdr[1]);
                                break;
                            case clothing:
                                totalClothing = Convert.ToDouble(rdr[1]);
                                break;
                            case entertainment:
                                totalEntertainment = Convert.ToDouble(rdr[1]);
                                break;
                        }

                    }

                    rdr.Close();

                    double[] total = { totalDining, totalAlcohol, totalGasoline, totalFood, totalClothing, totalToiletries, totalEntertainment };

                    const string format = "{0,-10} {1,-15}";
                    Console.WriteLine(format, "$" + totalDining, dining);
                    Console.WriteLine(format, "$" + totalAlcohol.ToString("0.00"), alcohol);
                    Console.WriteLine(format, "$" + totalGasoline, gasoline);
                    Console.WriteLine(format, "$" + totalFood.ToString("0.00"), food);
                    Console.WriteLine(format, "$" + totalClothing, clothing);
                    Console.WriteLine(format, "$" + totalToiletries, toiletries);
                    Console.WriteLine(format, "$" + totalEntertainment, entertainment);

                    Console.WriteLine("\nTotal Spent $" + total.Sum());

                }
                else
                    Console.WriteLine("No results");
            }
            
        }

        private void addExpense()
        {
            Console.WriteLine("Enter New Expense:\n");
            Console.WriteLine("Separate by commas: Ex -> Cost, Category, Payment Type, Date (0 for today)\n");

            string[] separater = { "," };

            string newExpense = Console.ReadLine();
            
            string[] split = newExpense.Split(separater, StringSplitOptions.RemoveEmptyEntries);
            List<string> errors = new List<string>();

            double doubleCheck;
            int intCheck;
            DateTime temp;
            double cost = 0;
            string category = "";
            string payment = "";
            DateTime date = DateTime.Now;

            if (split.Length != 4)
                errors.Add("You can only enter 4 items");
            else
            {    
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
                            if(int.TryParse(item.ToString().Trim(), out intCheck))
                            {
                                if(item.ToString().Trim() == "0")
                                {
                                    //good to go..
                                }
                                else
                                    errors.Add("Only enter 0 for today's date");
                            }
                            else
                            {
                                if (DateTime.TryParse(item.ToString().Trim(), out temp))
                                    date = Convert.ToDateTime(item.ToString().Trim());
                                else
                                    errors.Add("Date incorrectly formatted");
                            }
                            break;
                    }

                    index++;
                }
            }
                
            if(errors.Count > 0)
            {
                string[] err = errors.ToArray();

                foreach(var e in err)
                    Console.WriteLine(e);

                Console.WriteLine("Select option 4 to try again\n");
                    
            }
            else
            {
                Console.WriteLine("Inserting Values: " + cost + ", " + category + ", " + payment + ", " + date);

                string query = "INSERT INTO personal (cost, category, payment_type, date) VALUES (@cost, @category, @payment, @date)";

                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@payment", payment);
                    cmd.Parameters.AddWithValue("@date", date);

                    int result = cmd.ExecuteNonQuery();

                    if (result < 0)
                        Console.WriteLine("Error adding data...");
                    else
                        Console.WriteLine("Expense successfully added..");
                }
                
            }

        }

        private void deleteExpense()
        {
            Console.WriteLine("Delete an Expense - Enter the ID of the expense you would like to delete:\n");
            int intCheck;
            string id = Console.ReadLine();

            if (int.TryParse(id, out intCheck))
            {
                intCheck = Int32.Parse(id);
                SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("id", intCheck);
                   
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        Console.WriteLine("Confirm Deletion\n");
                        while (rdr.Read())
                        {
                            Console.WriteLine(rdr[0] + " " + rdr[1] + " " + rdr[2] + " " + rdr[3] + " " + Convert.ToDateTime(rdr[4]).ToString("MM/dd/yyyy") + "\n");
                        }

                        rdr.Close();

                        Console.WriteLine("Do you want to delete id: " + intCheck + "? Y or N");
                        string confirm = Console.ReadLine();

                        switch(confirm.ToUpper())
                        {
                            case "Y":
                                string query = "DELETE FROM personal WHERE Id = @id";
                                using (SqlCommand delete = new SqlCommand(query, conn))
                                {
                                    delete.Parameters.AddWithValue("id", intCheck);

                                    int result = delete.ExecuteNonQuery();

                                    if (result < 0)
                                        Console.WriteLine("Error deleting id: " + intCheck);
                                    else
                                        Console.WriteLine("ID: " + intCheck + " successfully deleted");
                                }
                                break;
                            case "N":
                                Console.WriteLine("Deletion cancelled - Selection option 6 to delete another item");
                                break;
                            default:
                                Console.WriteLine("You can only enter Y or N - start over...");
                                break;
                        }   
                    }
                    else
                        Console.WriteLine("Couldn't find a record matching id: " + intCheck);
                } 
            }
            else
                Console.WriteLine("Invalid ID entered - choose step 6 to try again...\n");
        }

        private void editExpense()
        {
            Console.Write("Edit expense: Enter ID of expense to edit\n");
            int idEntered;
            string id = Console.ReadLine();

            if(int.TryParse(id, out idEntered))
            {
                idEntered = Int32.Parse(id);
                SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("id", idEntered);

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        Console.WriteLine("Confirm you want to edit:\n");
                        while(rdr.Read())
                        {
                            Console.WriteLine(rdr[0] + " " + rdr[1] + " " + rdr[2] + " " + rdr[3] + " " + Convert.ToDateTime(rdr[4]).ToString("MM/dd/yyyy") + "\n");
                        }

                        rdr.Close();

                        Console.WriteLine("Do you want edit id: " + idEntered + "? Y or N");
                        string confirm = Console.ReadLine();

                        switch (confirm.ToUpper())
                        {
                            case "Y":
                                Console.WriteLine("Enter new cost - (enter 0 to leave cost the same)");
                                double costEntered;
                                bool costEdit = true;
                                string cost = Console.ReadLine();
                                if(Double.TryParse(cost, out costEntered))
                                {
                                    costEntered = Double.Parse(cost);
                                    costEdit = true;

                                    if (costEntered == 0)
                                        costEdit = false;
                                }

                                Console.WriteLine("Enter new category - (enter 0 to leave category the same)");
                                int intCheck;
                                bool categoryEdit = true;
                                string category = Console.ReadLine();                                
                                if(Int32.TryParse(category, out intCheck))
                                {
                                    intCheck = Int32.Parse(category);
                                    if(intCheck == 0)
                                        categoryEdit = false;
                                }
                                else
                                {
                                    if (categories.Contains(category))
                                    {
                                        //good to go..

                                    }
                                    else
                                        Console.WriteLine("Category doesn't exist");
                                      
                                }

                                Console.WriteLine("Enter new payment type - (enter 0 to leave payment typ the same)");
                                int typeCheck;
                                bool typeEdit = true;
                                string paymentType = Console.ReadLine();
                                if(Int32.TryParse(paymentType, out typeCheck))
                                {
                                    typeCheck = Int32.Parse(paymentType);
                                    if (typeCheck == 0)
                                        typeEdit = false;
                                }
                                else
                                {
                                    if (paymentTypes.Contains(paymentType))
                                    {
                                        //good to go..
                                    }
                                    else
                                        Console.WriteLine("Payment type doesn't exist");
                                       
                                }
                                Console.WriteLine("Enter new date - (enter 0 to leave date the same)");
                                int dateCheck;
                                bool dateEdit = true;
                                DateTime dateVal;
                                string date = Console.ReadLine();
                                if(Int32.TryParse(date, out dateCheck))
                                {
                                    dateCheck = Int32.Parse(date);
                                    if (dateCheck == 0)
                                        dateEdit = false;
                                }
                                else
                                {
                                    if (DateTime.TryParse(date, out dateVal))
                                        dateVal = Convert.ToDateTime(date);
                                    else
                                        Console.WriteLine("Date not properly formatted");
                                       
                                }
                                if (costEdit)
                                    Console.WriteLine("Cost edit true");
                                if (categoryEdit)
                                    Console.WriteLine("Category edit true");
                                if (typeEdit)
                                    Console.WriteLine("Type edit true");
                                if (dateEdit)
                                    Console.WriteLine("Date edit true");
                                break;
                            case "N":
                                Console.WriteLine("Edit cancelled - Selection option 5 to edit another item");
                                break;
                            default:
                                Console.WriteLine("You can only enter Y or N - start over...");
                                break;
                        }   
                    }
                    else
                        Console.WriteLine("ID: " + idEntered + " doesn't exist - Select option 5 to edit another item");
                }
            }
            else
                Console.WriteLine("Not a valid ID - Select option 5 to try again");
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
                        case 5:
                            p.editExpense();
                            break;
                        case 6:
                            p.deleteExpense();
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
