using MySql.Data.MySqlClient;

namespace PedrosCantina
{
    public class ScheduleRepository
    {
        private readonly string connectionString =
            "Server=localhost;Database=PedrosCantina;User=root;Password=root;";

        public void ShowMonthlySchedule(int year, int month)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
                SELECT
                    ws.ShiftDate,
                    ws.StartTime,
                    ws.EndTime,
                    e.FirstName,
                    e.LastName
                FROM WorkShift ws

                INNER JOIN ShiftAssignment sa
                    ON ws.ShiftId = sa.ShiftId

                INNER JOIN Employee e
                    ON sa.EmployeeId = e.EmployeeId

                WHERE YEAR(ws.ShiftDate) = @year
                AND MONTH(ws.ShiftDate) = @month

                ORDER BY
                    ws.ShiftDate,
                    ws.StartTime;";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@year", year);
            command.Parameters.AddWithValue("@month", month);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["ShiftDate"]} " +
                    $"{reader["StartTime"]} - {reader["EndTime"]} " +
                    $"{reader["FirstName"]} {reader["LastName"]}");
            }
        }
        public void ShowMonthlyWorkload(int year, int month)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
        SELECT
            e.FirstName,
            e.LastName,
            COUNT(ws.ShiftId) AS NumberOfShifts,
            COUNT(ws.ShiftId) * 5 AS HoursWorked
        FROM Employee e

        LEFT JOIN ShiftAssignment sa
            ON e.EmployeeId = sa.EmployeeId

        LEFT JOIN WorkShift ws
            ON sa.ShiftId = ws.ShiftId
            AND YEAR(ws.ShiftDate) = @year
            AND MONTH(ws.ShiftDate) = @month

        GROUP BY
            e.EmployeeId,
            e.FirstName,
            e.LastName

        ORDER BY
            HoursWorked DESC;";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@year", year);
            command.Parameters.AddWithValue("@month", month);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["FirstName"]} {reader["LastName"]} - " +
                    $"{reader["NumberOfShifts"]} shifts - " +
                    $"{reader["HoursWorked"]} hours");
            }
        }
        public void ShowAvailableEmployees(int shiftId)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
        SELECT
            e.FirstName,
            e.LastName,
            e.Phone,
            e.Email,
            e.CanLead
        FROM Employee e

        WHERE e.EmployeeId NOT IN
        (
            SELECT sa.EmployeeId
            FROM ShiftAssignment sa
            WHERE sa.ShiftId = @shiftId
        )

        ORDER BY
            e.CanLead DESC,
            e.FirstName;";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@shiftId", shiftId);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["FirstName"]} {reader["LastName"]} - " +
                    $"Phone: {reader["Phone"]} - " +
                    $"Email: {reader["Email"]}");
            }
        }
        public void ShowYearlyWorkload(int year)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
        SELECT
            e.FirstName,
            e.LastName,
            MONTH(ws.ShiftDate) AS WorkMonth,
            COUNT(ws.ShiftId) AS NumberOfShifts,
            COUNT(ws.ShiftId) * 5 AS HoursWorked
        FROM Employee e

        INNER JOIN ShiftAssignment sa
            ON e.EmployeeId = sa.EmployeeId

        INNER JOIN WorkShift ws
            ON sa.ShiftId = ws.ShiftId

        WHERE YEAR(ws.ShiftDate) = @year

        GROUP BY
            e.EmployeeId,
            e.FirstName,
            e.LastName,
            MONTH(ws.ShiftDate)

        ORDER BY
            e.EmployeeId,
            WorkMonth;";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@year", year);

            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader["FirstName"]} {reader["LastName"]} - " +
                    $"Month: {reader["WorkMonth"]} - " +
                    $"{reader["NumberOfShifts"]} shifts - " +
                    $"{reader["HoursWorked"]} hours");
            }
        }
        public void CreateShift(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
        INSERT INTO WorkShift
        (ShiftDate, StartTime, EndTime)
        VALUES
        (@date, @startTime, @endTime);";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@date", date.Date);
            command.Parameters.AddWithValue("@startTime", startTime);
            command.Parameters.AddWithValue("@endTime", endTime);

            command.ExecuteNonQuery();
        }
        public void UpdateShift(int shiftId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            using MySqlConnection connection =
                new MySqlConnection(connectionString);

            connection.Open();

            string sql = @"
        UPDATE WorkShift
        SET ShiftDate = @date,
            StartTime = @startTime,
            EndTime = @endTime
        WHERE ShiftId = @shiftId;";

            using MySqlCommand command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@date", date.Date);
            command.Parameters.AddWithValue("@startTime", startTime);
            command.Parameters.AddWithValue("@endTime", endTime);
            command.Parameters.AddWithValue("@shiftId", shiftId);

            command.ExecuteNonQuery();
        }
    }
}