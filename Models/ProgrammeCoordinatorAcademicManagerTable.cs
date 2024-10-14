using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Models
{
	public class ProgrammeCoordinatorAcademicManagerTable

	{
		// Connection string for connecting to the SQL database
		public static string con_string = "Server=tcp:sql-databaset-utorial.database.windows.net,1433;Initial Catalog=MyCloudDatabase;Persist Security Info=False;User ID=thaniamathews;Password=SummerMe26;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

		// SQLConnection objct. for managing the database connection
		public static SqlConnection con = new SqlConnection(con_string);


		// properties for storing the user information
		public string Name { get; set; }

		public string Surname { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }


		// Method for inserting user data into the database
		public int insert_ProgrammeCoordinatorAcademicManager(ProgrammeCoordinatorAcademicManagerTable model)
		{
			string sql = "INSERT INTO ProgrammeCoordinatorAcademicManagerTable (UserName, UserSurname, UserEmail, UserPassword) VALUES (@Name, @Surname, @Email, @Password)";

			// create SQLCommand object with SQL query and SQLConnection
			using SqlCommand cmd = new SqlCommand(sql, con);

			// adding parameters to the SQL query
			cmd.Parameters.AddWithValue("@Name", model.Name);
			cmd.Parameters.AddWithValue("@Surname", model.Surname);
			cmd.Parameters.AddWithValue("@Email", model.Email);
			cmd.Parameters.AddWithValue("@Password", model.Password);

			// open the database connection
			con.Open();

			// executing the SQL query + getting the number of rows affected 
			int resultAffected = cmd.ExecuteNonQuery();

			// close the database connection
			con.Close();

			// returns the number of rows affected by the query
			return resultAffected;
		}
	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
