using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace PedrosCantina
{
    public class EmployeeRepository
    {
        private readonly string connectionString =
            "Server=localhost;Database=PedrosCantina;User=root;Password=root;";
        public List<Employee> GetAll()
        {
            List<Employee> employees = new List<Employee>();

            using MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = "SELECT * FROM Employee;";

            using MySqlCommand command = new MySqlCommand(sql, connection);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Employee employee = new Employee
                {
                    EmployeeId = reader.GetInt32("EmployeeId"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Phone = reader.GetString("Phone"),
                    Email = reader.GetString("Email"),
                    CanLead = reader.GetBoolean("CanLead")
                };

                employees.Add(employee);
            }

            return employees;
        }
        public Employee GetById(int id)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = "SELECT * FROM Employee WHERE EmployeeId = @id;";

            using MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);

            using MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Employee
                {
                    EmployeeId = reader.GetInt32("EmployeeId"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Phone = reader.GetString("Phone"),
                    Email = reader.GetString("Email"),
                    CanLead = reader.GetBoolean("CanLead")
                };
            }

            return null;
        }
        public void Create(Employee employee)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
        INSERT INTO Employee
        (FirstName, LastName, Phone, Email, CanLead)
        VALUES
        (@firstName, @lastName, @phone, @email, @canLead);";

            using MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@firstName", employee.FirstName);
            command.Parameters.AddWithValue("@lastName", employee.LastName);
            command.Parameters.AddWithValue("@phone", employee.Phone);
            command.Parameters.AddWithValue("@email", employee.Email);
            command.Parameters.AddWithValue("@canLead", employee.CanLead);

            command.ExecuteNonQuery();
        }
        public void Update(Employee employee)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
        UPDATE Employee
        SET FirstName = @firstName,
            LastName = @lastName,
            Phone = @phone,
            Email = @email,
            CanLead = @canLead
        WHERE EmployeeId = @employeeId;";

            using MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@firstName", employee.FirstName);
            command.Parameters.AddWithValue("@lastName", employee.LastName);
            command.Parameters.AddWithValue("@phone", employee.Phone);
            command.Parameters.AddWithValue("@email", employee.Email);
            command.Parameters.AddWithValue("@canLead", employee.CanLead);
            command.Parameters.AddWithValue("@employeeId", employee.EmployeeId);

            command.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string sql = "DELETE FROM Employee WHERE EmployeeId = @id;";

            using MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }

}

