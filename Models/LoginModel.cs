using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*START*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

namespace PROG_POE2.Models
{
	public class LoginModel 
	{
		// creating connection string - to connect to sql database
		public static string con_string = "Server=tcp:st10381071.database.windows.net,1433;Initial Catalog=ProgDatabase;Persist Security Info=False;User ID=mathewsthania;Password=SummerMe26;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

		// creating method to select lecturer - by name,email and password
		public int SelectLecturer(string Name, string Email, string Password)
		{
			// Default UserID value if user is not found in database
			int UserID = -1;

			using (SqlConnection con = new SqlConnection(con_string))
			{
				// defining the sql query to select the UserID from the UserTable 
				string sql = "SELECT UserID FROM LecturerTable WHERE UserName = @Name AND UserEmail = @Email AND UserPassword = @Password";
				SqlCommand cmd = new SqlCommand(sql, con);

				cmd.Parameters.AddWithValue("@Name", Name);
				cmd.Parameters.AddWithValue("@Email", Email);
				cmd.Parameters.AddWithValue("@Password", Password);


				try
				{
					con.Open(); // opening the connection for the database

					object result = cmd.ExecuteScalar(); // execuing the SQL query and get the returning result

					// if statmenet - to check if the result is not null and not DBnull
					if (result != null && result != DBNull.Value)
					{
						// converting the result to an integer and setting it as the UserID
						UserID = Convert.ToInt32(result);
					}
				}

				catch (Exception ex)
				{
					throw ex;
				}

			}

			return UserID;
		}

		// creating method to select programmecoordinatoracademicmanager - by name,email and password
		public int SelectProgrammeCoordinatorAcademicManager(string Name, string Email, string Password)
		{
			// Default UserID value if user is not found in database
			int UserID = -1;

			using (SqlConnection con = new SqlConnection(con_string))
			{
				// defining the sql query to select the UserID from the UserTable 
				string sql = "SELECT UserID FROM ProgrammeCoordinatorAcademicManagerTable WHERE UserName = @Name AND UserEmail = @Email AND UserPassword = @Password";
				SqlCommand cmd = new SqlCommand(sql, con);

				cmd.Parameters.AddWithValue("@Name", Name);
				cmd.Parameters.AddWithValue("@Email", Email);
				cmd.Parameters.AddWithValue("@Password", Password);


				try
				{
					con.Open(); // opening the connection for the database

					object result = cmd.ExecuteScalar(); // execuing the SQL query and get the returning result

					// if statmenet - to check if the result is not null and not DBnull
					if (result != null && result != DBNull.Value)
					{
						// converting the result to an integer and setting it as the UserID
						UserID = Convert.ToInt32(result);
					}
				}

				catch (Exception ex)
				{
					throw ex;
				}

			}

			return UserID;
		}

	}
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<*THE*END*OF*FILE*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//