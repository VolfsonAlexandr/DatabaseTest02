using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseTest
{
    class Program
    {
        static string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\BotDatabase.mdf;Integrated Security=True";
        static Action[] actionList = { ShowUserList, AddUser, WorkWithUser, CheckExisting, FinishWorkMenu1 };
        static Action[] actionUserList = { ShowUserBounds, AddBound, UpdateUserInfo, DeleteUser, WorkWithBound, FinishWorkMenu2 };
        static Action[] actionBoundList = { ShowBoundFilters, AddFilter, UpdateBoundInfo, FinishWorkMenu3 };
        static bool isContinueMenu1 = true;
        static bool isContinueMenu2 = true;
        static bool isContinueMenu3 = true;
        static void Main(string[] args)
        {
            ShowMenu();
        }

        static void ShowMenu()
        {
            while (isContinueMenu1)
            {
                Console.Clear();
                Console.WriteLine("Для выбора пункта меню введите соответствующее число");
                string[] items = { "Посмотреть список пользователей", "Добавить пользователя", "Работать c конкретным пользователем", "Проверить наличие пользователя с заданным логином", "Закончить работу" };
                for (int i = 0; i < items.Length; i++)
                    Console.WriteLine(items[i] + " - " + i);
                int selecteditem = int.Parse(Console.ReadLine());
                actionList[selecteditem]();
            }
        }

        static void ShowUserList()
        {
            Console.Clear();
            Select("UserData");
            Console.ReadLine();
        }

        static void AddUser()
        {
            Console.Clear();
            string[] columns = {"Login", "Password", "Name", "Surname", "Birth" };
            string columnsString =  "";
            string valuesString = "'";
            for (int i = 0; i < columns.Length; i++)
            {
                columnsString += columns[i];
                columnsString += ",";
                Console.WriteLine("Введите значение " + columns[i] + " нового пользователя");
                string value = Console.ReadLine();
                valuesString += value;
                valuesString += "','";
            }
            columnsString = columnsString.Remove(columnsString.Length - 1, 1);
            valuesString = valuesString.Remove(valuesString.Length - 2, 2);
            Insert("UserData", columnsString, valuesString);
        }

        static void WorkWithUser()
        {
            isContinueMenu2 = true;
            while (isContinueMenu2)
            {
                Console.Clear();
                Console.WriteLine("Для выбора пункта меню введите соответствующее число");
                string[] items = { "Посмотреть связки пользователя", "Добавить связку", "Редактировать личную информацию пользователя", "Удалить пользователя", "Работать с конкретной связкой пользователя", "Выйти в главное меню" };
                for (int i = 0; i < items.Length; i++)
                    Console.WriteLine(items[i] + " - " + i);
                int selecteditem = int.Parse(Console.ReadLine());
                actionUserList[selecteditem]();
            }
        }

        static void CheckExisting()
        {
            Console.Clear();
            Console.WriteLine("Введите логин");
            string login = Console.ReadLine();
            Console.WriteLine(IsExist(login) ? "Такой пользователь существует" : "Такого пользователя не существует");
            Console.ReadLine();
        }

        static void FinishWorkMenu1()
        {
            isContinueMenu1 = false;
        }

        static void FinishWorkMenu2()
        {
            isContinueMenu2 = false;
        }

        static void FinishWorkMenu3()
        {
            isContinueMenu3 = false;
        }

        static void ShowUserBounds()
        {
            Console.Clear();
            Console.WriteLine("Введите Id пользователя, с информацией которого хотите работать");
            int id = int.Parse(Console.ReadLine());
            ComplexSelect("Id, Instagram, Telegram", "Bound", "WHERE User_Id = " + id.ToString());
            Console.ReadLine();
        }

        static void AddBound()
        {
            Console.Clear();
            Console.WriteLine("Введите Id пользователя, с информацией которого хотите работать");
            int id = int.Parse(Console.ReadLine());
            string[] columns = { "Instagram", "Telegram"};
            string columnsString = "User_Id,";
            string valuesString = "'"+id.ToString()+"','";
            for (int i = 0; i < columns.Length; i++)
            {
                columnsString += columns[i];
                columnsString += ",";
                Console.WriteLine("Введите значение " + columns[i] + " новой связки");
                string value = Console.ReadLine();
                valuesString += value;
                valuesString += "','";
            }
            columnsString = columnsString.Remove(columnsString.Length - 1, 1);
            valuesString = valuesString.Remove(valuesString.Length - 2, 2);
            Insert("Bound", columnsString, valuesString);
        }
        static void UpdateUserInfo()
        {
            Console.Clear();
            Console.WriteLine("Введите Id пользователя, с информацией которого хотите работать");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Выберите поле, которое Вы хотите изменить");
            string[] columns = { "Password", "Name", "Surname", "Birth" };
            for (int i = 0; i < columns.Length; i++)
            {
                Console.WriteLine(columns[i] + " - " + i);
            }
            int selectedColumn = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите новое значение этого поля");
            string newValue = Console.ReadLine();
            Update("UserData", columns[selectedColumn], newValue, "WHERE Id = " + id);
        }

        static void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("Введите Id пользователя, которого хотите удалить");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Вы уверены? (Y/N)");
            string answer = Console.ReadLine();
            if (answer == "Y")
            {
                Delete("UserData", "WHERE Id = " + id);
            }
        }

        static void WorkWithBound()
        {
            isContinueMenu3 = true;
            while (isContinueMenu3)
            {
                Console.Clear();
                Console.WriteLine("Для выбора пункта меню введите соответствующее число");
                string[] items = { "Посмотреть фильтры связки", "Добавить фильтр", "Редактировать связку" , "Выйти в предыдущее меню" };
                for (int i = 0; i < items.Length; i++)
                    Console.WriteLine(items[i] + " - " + i);
                int selecteditem = int.Parse(Console.ReadLine());
                actionBoundList[selecteditem]();
            }
        }
        static void ShowBoundFilters()
        {
            Console.Clear();
            Console.WriteLine("Введите Id связки, с которой хотите работать");
            int id = int.Parse(Console.ReadLine());
            ComplexSelect("Id, Filter", "Filter", "WHERE Bound_Id = " + id.ToString());
            Console.ReadLine();
        }

        static void AddFilter()
        {
            Console.Clear();
            Console.WriteLine("Введите Id связки, с которой хотите работать");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите фильтр");
            string filter = Console.ReadLine();
            string columns = "Bound_Id, Filter";
            string values = "'" + id.ToString() + "','" + filter + "'";
            Insert("Filter", columns, values);
        }

        static void UpdateBoundInfo()
        {
            Console.Clear();
            Console.WriteLine("Введите Id связки, с которой хотите работать");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Выберите поле, которое Вы хотите изменить");
            string[] columns = { "Instagram", "Telegram" };
            for (int i = 0; i < columns.Length; i++)
            {
                Console.WriteLine(columns[i] + " - " + i);
            }
            int selectedColumn = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите новое значение этого поля");
            string newValue = Console.ReadLine();
            Update("Bound", columns[selectedColumn], newValue, "WHERE Id = " + id);
        }



        static void Insert(string table, string columns, string values)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string insertQuery = "INSERT INTO " + table + "(" + columns + ") values(" + values + ")";
            SqlCommand sqlCommand = new SqlCommand(insertQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        static void Select(string table)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string selectQuery = "SELECT * FROM " + table;
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int columnCount = sqlDataReader.FieldCount;
            while (sqlDataReader.Read())
            {
                for (int i = 0; i < columnCount; i++)
                    Console.Write(sqlDataReader.GetValue(i).ToString() + " ");
                Console.WriteLine();
            }
            sqlConnection.Close();
        }

        static void ComplexSelect(string columns, string tables, string condition)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string selectQuery = "SELECT " + columns + " FROM " + tables + " " + condition;
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int columnCount = sqlDataReader.FieldCount;
            while (sqlDataReader.Read())
            {
                for (int i = 0; i < columnCount; i++)
                    Console.Write(sqlDataReader.GetValue(i).ToString() + " ");
                Console.WriteLine();
            }
            sqlConnection.Close();
        }

        static bool IsExist(string login)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string selectQuery = "SELECT * FROM UserData WHERE Login = '"+login+"'";
            SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int count = 0;
            while (sqlDataReader.Read())
            {
                sqlDataReader.GetValue(0);
                count++;
            }
            sqlConnection.Close();
            return count > 0;
        }

        static void Update(string table, string column, string newValue, string condition)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string updateQuery = "UPDATE " + table + " SET " + column + " = '" + newValue + "' " + condition;
            SqlCommand sqlCommand = new SqlCommand(updateQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        static void Delete(string table, string condition)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string deleteQuery = "DELETE FROM " + table + " " + condition;
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}
