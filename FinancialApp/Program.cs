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
        private const string tobacco = "tobacco";
        private const string tips = "tips";
        private const string utilities = "utilities";
        private const string coffee = "coffee";
        private const string travel = "travel";
        private const string bank = "bank";
        private const string tools = "tools";
        private const string vehicle = "vehicle";
        private const string gifts = "gifts";
        private const string education = "education";
        private const string insurance = "insurance";

        private const string visaChase = "visa - chase";
        private const string masterUsaa = "master - usaa";
        private const string visaNavyfcu = "visa - navyfcu";
        private const string cash = "cash";

        private string[] paymentTypes = { visaChase, masterUsaa, visaNavyfcu, cash };
        private string[] categories = { dining, alcohol, gasoline, food, toiletries, clothing, entertainment, tobacco, tips,
                                        utilities, coffee, travel, bank, tools, vehicle, gifts, education, insurance };

        const string defaultFormat = "{0,-5} | {1,-8} | {2,-13} | {3,-15} | {4,-10}";

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
            Console.WriteLine("14. View summary for a particular month");
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
                    Console.WriteLine(String.Format(defaultFormat, "ID", " SPENT", "CATEGORY", "PAYMENT TYPE", "DATE"));

                    while (rdr.Read())
                    {
                        total += Convert.ToDouble(rdr[1]);
                        Console.WriteLine(String.Format(defaultFormat,
                            rdr[0], " $" + rdr[1], rdr[2], rdr[3], Convert.ToDateTime(rdr[4]).ToString("MM/dd/yyyy")));
                    }

                    rdr.Close();

                    Console.WriteLine("\nTotal Spent $" + total);

                }
                else
                    Console.WriteLine("No results");
            }
        }

        private void monthSummary(int month = 0)
        {
            string currentMonth = "Current Month";
            DateTime today = DateTime.Today;
            DateTime beginMonth = Convert.ToDateTime(getMonthBegin());
            DateTime endMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);

            if (month != 0)
            {
                currentMonth = displayMonth(month);
                beginMonth = Convert.ToDateTime(getMonthBegin(month));
                endMonth = new DateTime(today.Year, month, 1).AddMonths(1).AddDays(-1);
            }

            Console.WriteLine("Spending Summary for "+ currentMonth + ":\n\n");

            SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date BETWEEN '"
                + beginMonth + "' AND '" + endMonth + "' ORDER BY date", conn);
            
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
                    double totalTobacco = 0;
                    double totalTips = 0;
                    double totalUtilities = 0;
                    double totalCoffee = 0;
                    double totalTravel = 0;
                    double totalBank = 0;
                    double totalTools = 0;
                    double totalVehicle = 0;
                    double totalGifts = 0;
                    double totalEducation = 0;
                    double totalInsurance = 0;

                    while (rdr.Read())
                    {
                        switch (rdr[2].ToString().Trim())
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
                                totalToiletries += Convert.ToDouble(rdr[1]);
                                break;
                            case clothing:
                                totalClothing += Convert.ToDouble(rdr[1]);
                                break;
                            case entertainment:
                                totalEntertainment += Convert.ToDouble(rdr[1]);
                                break;
                            case tobacco:
                                totalTobacco += Convert.ToDouble(rdr[1]);
                                break;
                            case tips:
                                totalTips += Convert.ToDouble(rdr[1]);
                                break;
                            case utilities:
                                totalUtilities += Convert.ToDouble(rdr[1]);
                                break;
                            case coffee:
                                totalCoffee += Convert.ToDouble(rdr[1]);
                                break;
                            case travel:
                                totalTravel += Convert.ToDouble(rdr[1]);
                                break;
                            case bank:
                                totalBank += Convert.ToDouble(rdr[1]);
                                break;
                            case tools:
                                totalTools += Convert.ToDouble(rdr[1]);
                                break;
                            case vehicle:
                                totalVehicle += Convert.ToDouble(rdr[1]);
                                break;
                            case gifts:
                                totalGifts += Convert.ToDouble(rdr[1]);
                                break;
                            case education:
                                totalEducation += Convert.ToDouble(rdr[1]);
                                break;
                            case insurance:
                                totalInsurance += Convert.ToDouble(rdr[1]);
                                break;

                        }

                    }

                    rdr.Close();

                    double[] total = { totalDining, totalAlcohol, totalGasoline, totalFood, totalClothing, totalToiletries,
                                       totalEntertainment, totalTobacco, totalTips, totalUtilities, totalCoffee, totalTravel,
                                       totalBank, totalTools, totalVehicle, totalGifts, totalEducation, totalInsurance };

                    const string format = "{0,-10} {1,-15}";
                    Console.WriteLine(format, "$" + totalDining, dining);
                    Console.WriteLine(format, "$" + totalAlcohol.ToString("0.00"), alcohol);
                    Console.WriteLine(format, "$" + totalGasoline, gasoline);
                    Console.WriteLine(format, "$" + totalFood.ToString("0.00"), food);
                    Console.WriteLine(format, "$" + totalClothing, clothing);
                    Console.WriteLine(format, "$" + totalToiletries, toiletries);
                    Console.WriteLine(format, "$" + totalEntertainment, entertainment);
                    Console.WriteLine(format, "$" + totalTobacco, tobacco);
                    Console.WriteLine(format, "$" + totalTips, tips);
                    Console.WriteLine(format, "$" + totalUtilities, utilities);
                    Console.WriteLine(format, "$" + totalCoffee, coffee);
                    Console.WriteLine(format, "$" + totalTravel, travel);
                    Console.WriteLine(format, "$" + totalBank, bank);
                    Console.WriteLine(format, "$" + totalTools, tools);
                    Console.WriteLine(format, "$" + totalVehicle, vehicle);
                    Console.WriteLine(format, "$" + totalGifts, gifts);
                    Console.WriteLine(format, "$" + totalEducation, education);
                    Console.WriteLine(format, "$" + totalInsurance, insurance);

                    Console.WriteLine("\nTotal Spent for " + currentMonth + " $" + total.Sum());

                }
                else
                    Console.WriteLine("No results for " + currentMonth + "\n");
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

        private void showMonth()
        {
            Console.WriteLine("Enter a month: 1 - 12\n");
            int month;

            string m = Console.ReadLine();

            if (int.TryParse(m, out month))
            {
                if (month < 1 || month > 12)
                    Console.WriteLine("Integer must be between 1 and 12\n");
                else
                {
                    DateTime today = DateTime.Today;
                    DateTime endMonth = new DateTime(today.Year, month, 1).AddMonths(1).AddDays(-1);

                    SqlCommand cmd = new SqlCommand("SELECT * FROM personal WHERE date BETWEEN '"
                        + getMonthBegin(month) + "' AND '" + endMonth + "' ORDER BY date", conn);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            double total = 0;
                            Console.WriteLine("Displaying results for " + displayMonth(month) + " " + today.Year + "\n");
                            Console.WriteLine(String.Format(defaultFormat, "ID", " SPENT", "CATEGORY", "PAYMENT TYPE", "DATE"));

                            while (rdr.Read())
                            {
                                total += Convert.ToDouble(rdr[1]);
                                Console.WriteLine(String.Format(defaultFormat,
                                    rdr[0], " $" + rdr[1], rdr[2], rdr[3], Convert.ToDateTime(rdr[4]).ToString("MM/dd/yyyy")));
                            }

                            rdr.Close();

                            Console.WriteLine("\nTotal Spent $" + total);
                        }
                        else
                            Console.WriteLine("No results for: " + displayMonth(month) + " " + today.Year + "\n");
                    }
                }
                  
            }
            else
                Console.WriteLine("Not a valid integer\n");

        }

        private void showMonthSummary()
        {
            Console.WriteLine("Enter a month: 1 - 12\n");
            int month;

            string m = Console.ReadLine();

            if (int.TryParse(m, out month))
            {
                if (month < 1 || month > 12)
                    Console.WriteLine("Integer must be between 1 and 12\n");
                else
                    monthSummary(month);
            }
        }

        private string displayMonth(int month)
        {
            string monthString = "";

            switch(month)
            {
                case 1:
                    monthString = "January";
                    break;
                case 2:
                    monthString = "February";
                    break;
                case 3:
                    monthString = "March";
                    break;
                case 4:
                    monthString = "April";
                    break;
                case 5:
                    monthString = "May";
                    break;
                case 6:
                    monthString = "June";
                    break;
                case 7:
                    monthString = "July";
                    break;
                case 8:
                    monthString = "August";
                    break;
                case 9:
                    monthString = "September";
                    break;
                case 10:
                    monthString = "October";
                    break;
                case 11:
                    monthString = "November";
                    break;
                case 12:
                    monthString = "December";
                    break;
            }

            return monthString;
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
                                bool costEdit = false;
                                string cost = Console.ReadLine();
                                if(Double.TryParse(cost, out costEntered))
                                {
                                    costEntered = Double.Parse(cost);
                                    costEdit = true;

                                    if (costEntered == 0)
                                        costEdit = false;
                                }
                                else
                                {
                                    costEdit = false;
                                    Console.WriteLine("Invalid cost entered..");
                                }

                                Console.WriteLine("Enter new category - (enter 0 to leave category the same)");
                                int intCheck;
                                bool categoryEdit = false;
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
                                        categoryEdit = true;
                                    else
                                    {
                                        Console.WriteLine("Category doesn't exist");
                                        categoryEdit = false;
                                    } 
                                }

                                Console.WriteLine("Enter new payment type - (enter 0 to leave payment typ the same)");
                                int typeCheck;
                                bool typeEdit = false;
                                string paymentType = Console.ReadLine();
                                if (Int32.TryParse(paymentType, out typeCheck))
                                {
                                    typeCheck = Int32.Parse(paymentType);
                                    if (typeCheck == 0)
                                        typeEdit = false;
                                }
                                else
                                {
                                    if (paymentTypes.Contains(paymentType))
                                        typeEdit = true;
                                    else
                                    {
                                        Console.WriteLine("Payment type doesn't exist");
                                        typeEdit = false;
                                    }
                                }

                                Console.WriteLine("Enter new date - (enter 0 to leave date the same)");
                                int dateCheck;
                                bool dateEdit = false;
                                DateTime dateVal = DateTime.Now;
                                string date = Console.ReadLine();
                                if (Int32.TryParse(date, out dateCheck))
                                {
                                    dateCheck = Int32.Parse(date);
                                    if (dateCheck == 0)
                                        dateEdit = false;
                                }
                                else
                                {
                                    if (DateTime.TryParse(date, out dateVal))
                                        dateEdit = true;
                                    else
                                    {
                                        dateEdit = false;
                                        Console.WriteLine("Invalid date entered..");
                                    }
                                       
                                }

                                if(costEdit || categoryEdit || typeEdit || dateEdit)
                                {
                                    string query = "Update personal ";
                                    List<string> toUpdate = new List<string>();

                                    if (costEdit)
                                    {
                                        Console.WriteLine("Updating Cost to: " + cost);
                                        toUpdate.Add("cost");
                                    }
                                        
                                    if (categoryEdit)
                                    {
                                        Console.WriteLine("Updating Category to: " + category);
                                        toUpdate.Add("category");
                                    }
                                        
                                    if (typeEdit)
                                    {
                                        Console.WriteLine("Updating Payment Type to: " + paymentType);
                                        toUpdate.Add("type");
                                    }
                                        
                                    if (dateEdit)
                                    {
                                        Console.WriteLine("Updating Date to: " + dateVal);
                                        toUpdate.Add("date");
                                    }

                                    string[] updateArray = toUpdate.ToArray();
                                    string last = updateArray.Last();

                                    foreach(var update in updateArray)
                                    {
                                        if(update == last)
                                        {
                                            switch(update)
                                            {
                                                case "cost":
                                                    query += "SET cost = @cost ";
                                                    break;
                                                case "category":
                                                    query += "SET category = @category ";
                                                    break;
                                                case "type":
                                                    query += "SET payment_type = @type ";
                                                    break;
                                                case "date":
                                                    query += "SET date = @date ";
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch(update)
                                            {
                                                case "cost":
                                                    query += "SET cost = @cost, ";
                                                    break;
                                                case "category":
                                                    query += "SET category = @category, ";
                                                    break;
                                                case "type":
                                                    query += "SET payment_type = @type, ";
                                                    break;
                                                case "date":
                                                    query += "SET date = @date, ";
                                                    break;
                                            }
                                        }
                                           
                                    }

                                    query += "WHERE id = @id";

                                    using (SqlCommand update = new SqlCommand(query, conn))
                                    {
                                        update.Parameters.AddWithValue("id", idEntered);

                                        if (costEdit)
                                            update.Parameters.AddWithValue("cost", cost);
                                        if (categoryEdit)
                                            update.Parameters.AddWithValue("category", category);
                                        if (typeEdit)
                                            update.Parameters.AddWithValue("type", paymentType);
                                        if (dateEdit)
                                            update.Parameters.AddWithValue("date", dateVal);

                                        int result = update.ExecuteNonQuery();

                                        if (result < 0)
                                            Console.WriteLine("Problem updaing id: " + idEntered);
                                        else
                                            Console.WriteLine("ID: " + idEntered + " updated...");
                                    }
                                        
                                }
                                else
                                    Console.WriteLine("Nothing was selected to be updated. Operation aborted..");
                                
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

        private string getMonthBegin(int month = 0)
        {
            string monthStart;
            DateTime monthBegin;
            DateTime date = DateTime.Now;

            if(month != 0)
                monthBegin = new DateTime(date.Year, month, 1);
            else
                monthBegin = new DateTime(date.Year, date.Month, 1);

            monthStart = monthBegin.ToString("d");

            return monthStart;
        }

        private void viewPaymentType()
        {
            Console.WriteLine("Enter payment type to view\n");

            string type = Console.ReadLine();

            if (paymentTypes.Contains(type.Trim()))
            {
                Console.WriteLine("What month would you like to view? - enter 1 - 12\n");

                string m = Console.ReadLine();
                int month;

                if (int.TryParse(m, out month))
                {
                    if (month < 1 || month > 12)
                        Console.WriteLine(month + " is out of range\n");
                    else
                    {
                        DateTime today = DateTime.Today;
                        DateTime beginMonth = DateTime.Today;
                        DateTime endMonth = DateTime.Today;
                        string toSearch = "";

                        switch (type.Trim())
                        {
                            case visaChase:
                                toSearch = visaChase;
                                beginMonth = new DateTime(today.Year, month, 15);
                                endMonth = new DateTime(today.Year, month + 1, 14);
                                break;
                            case masterUsaa:
                                toSearch = masterUsaa;
                                beginMonth = new DateTime(today.Year, month, 18);
                                endMonth = new DateTime(today.Year, month + 1, 17);
                                break;
                            case visaNavyfcu:
                                toSearch = visaNavyfcu;
                                beginMonth = new DateTime(today.Year, month, 16);
                                endMonth = new DateTime(today.Year, month + 1, 15);
                                break;
                            case cash:
                                toSearch = cash;
                                beginMonth = new DateTime(today.Year, month, 1);
                                endMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
                                break;
                        }

                        string query = "SELECT * FROM personal WHERE date BETWEEN '"
                        + beginMonth + "' AND '" + endMonth + "' AND payment_type = '" + toSearch + "' ORDER BY date";

                        SqlCommand cmd = new SqlCommand(query, conn);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                double total = 0;
                                Console.WriteLine(String.Format(defaultFormat, "ID", " SPENT", "CATEGORY", "PAYMENT TYPE", "DATE"));

                                while (rdr.Read())
                                {
                                    total += Convert.ToDouble(rdr[1]);
                                    Console.WriteLine(String.Format(defaultFormat,
                                        rdr[0], " $" + rdr[1], rdr[2], rdr[3], Convert.ToDateTime(rdr[4]).ToString("MM/dd/yyyy")));
                                }

                                rdr.Close();

                                Console.WriteLine("\nTotal Spent using (" + toSearch + ") $" + total);
                            }
                            else
                                Console.WriteLine("No results for (" + toSearch + ") in " + displayMonth(month) + "\n");
                        }
                    }
                }
                else
                    Console.WriteLine(m + " is not a valid integer value\n");
            }
            else
                Console.WriteLine(type + " is not a vaild Payment Type\n");
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
                        case 3:
                            p.viewPaymentType();
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
                        case 12:
                            p.showMonth();
                            break;
                        case 14:
                            p.showMonthSummary();
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
