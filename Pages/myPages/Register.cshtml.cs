using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using static BlockchainQuiz.Pages.IndexModel;

namespace BlockchainQuiz.Pages.myPages
{
	public class RegisterModel : PageModel
	{
		private ParticipantInfo participant = new ParticipantInfo();
		private AdminInfo admin = new AdminInfo();
		private ErrorHandling feedback = new ErrorHandling();	
		private String connectionString = "Server=TANKISO-LTP\\SQLEXPRESS;Database=blockchainQuizzDatabase;Trusted_Connection=True;";


		public void OnGet()
		{

		}

		public void OnPost()
		{
			string userType = Request.Form["userType"];
			string confirmPass = Request.Form["confirmPassword"];
			string password = Request.Form["password"];

			if(confirmPass != password)
			{
				feedback.setErrorMessage("Passwords don't match");
				return;
			}

			if (userType == "participant")
			{
				participant.setName(Request.Form["name"]);
				participant.setPassword(password);
				participant.setStatus("Uncompleted");

				try
				{
					using (SqlConnection connection = new SqlConnection(connectionString))
					{
						connection.Open();
						String sqlUser = "INSERT INTO Users(userType) VALUES(@usertype);";
						String sqlParticipant = "INSERT INTO Participant(participant_id, participant_name, participant_password, participant_status) VALUES(@id, @name, @password, @status);";

						using (SqlCommand command = new SqlCommand(sqlUser, connection))
						{
							command.Parameters.AddWithValue("@usertype", userType);
							command.ExecuteNonQuery();					
						}

						using (SqlCommand command = new SqlCommand(sqlParticipant, connection))
						{
							command.Parameters.AddWithValue("@id", getlastUserId());
							command.Parameters.AddWithValue("@name", participant.getName());
							command.Parameters.AddWithValue("@password", PasswordValidation.HashPassword(participant.getPassword()));
							command.Parameters.AddWithValue("@status", participant.getStatus());
							command.ExecuteNonQuery();
						}
					}
					feedback.setSuccessMessage("Signed up successfully");
				}
				catch (Exception ex)
				{
					feedback.setErrorMessage(ex.Message);
					return;
				}
			}
			else if(userType == "administrator")
			{
				admin.setName(Request.Form["name"]);
				admin.setPassword(password);

				try
				{
					using (SqlConnection connection = new SqlConnection(connectionString))
					{
						connection.Open();
						String sqlUser = "INSERT INTO Users(userType) VALUES(@usertype);";
						String sqlAdmin = "INSERT INTO Administrator(admin_id, admin_name, admin_password) VALUES(@id, @name, @password);";

						using (SqlCommand command = new SqlCommand(sqlUser, connection))
						{
							command.Parameters.AddWithValue("@usertype", userType);
							command.ExecuteNonQuery();
						}

						using (SqlCommand command = new SqlCommand(sqlAdmin, connection))
						{
							command.Parameters.AddWithValue("@id", getlastUserId());
							command.Parameters.AddWithValue("@name", admin.getName());
							command.Parameters.AddWithValue("@password", PasswordValidation.HashPassword(admin.getPassword()));
							command.ExecuteNonQuery();

						}

						feedback.setSuccessMessage("Signed up successfully");
					}
				}catch (Exception ex)
				{
					feedback.setErrorMessage(ex.Message);
					return;

				}
			}
			else
			{
				feedback.setErrorMessage("Sign up Failed");

			}			
		}

		private int getlastUserId()
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM Users ORDER BY id DESC";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								return reader.GetInt32(0);

							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				feedback.setErrorMessage(ex.Message);
			}

			return 0;
		}

		public ErrorHandling getFeed()
		{
			return feedback;
		}

	}

	public class ErrorHandling
	{
		private string errorMessage = "";
		private string successMessage = "";


		public string getErrorMessage()
		{
			return errorMessage;
		}

		public void setErrorMessage(string _msg)
		{
			errorMessage = _msg;
		}

		public string getSuccessMessage()
		{
			return successMessage;
		}

		public void setSuccessMessage(string _msg)
		{
			successMessage = _msg;
		}

	}
}
