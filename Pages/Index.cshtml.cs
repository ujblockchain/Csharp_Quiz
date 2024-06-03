using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace BlockchainQuiz.Pages
{
	public class IndexModel : PageModel
	{
		private ParticipantInfo participant = new ParticipantInfo();
		private ErrorHandling feedback = new ErrorHandling();
		private String connectionString = "Server=TANKISO-LTP\\SQLEXPRESS;Database=blockchainQuizzDatabase;Trusted_Connection=True;";

		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
		}

		public void OnPost() {
			string name = Request.Form["name"];
			string pass = Request.Form["password"];

			try
			{
				AdminInfo administrator = getAdmin(PasswordValidation.HashPassword(pass));
				if (administrator != null && administrator.getName() == name)
				{
					HttpContext.Session.SetString("Administrator", "True");
					Response.Redirect("/myPages/AdminView");										
				}
				else
				{
					ParticipantInfo participant = getParticipant(PasswordValidation.HashPassword(pass));
					if(participant != null && participant.getName() == name) {
						HttpContext.Session.SetString("Participant", "True");
						Response.Redirect("/myPages/ParticipantView/?id="+ participant.getId());
					}
					else
					{
						feedback.setErrorMessage("Wrong credentials");
					}					
				}
			}
			catch (Exception ex)
			{
				feedback.setErrorMessage(ex.Message);
				return;
	
			}
	
		}

		public ErrorHandling getFeed()
		{
			return feedback;
		}

		private AdminInfo getAdmin(string _pass)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT admin_id, admin_name, admin_password FROM Administrator INNER JOIN Users ON Administrator.admin_id = Users.id WHERE admin_password = @password"; 
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@password", _pass);

					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							AdminInfo admin = new AdminInfo();
							admin.setId(reader.GetInt32(0));
							admin.setName(reader.GetString(1));
							admin.setPassword(reader.GetString(2));
							return admin; 
						}
					}
				}
			}
			return null;
		}

		private ParticipantInfo getParticipant(string _pass)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				string sql = "SELECT participant_id, participant_name, participant_password, participant_status FROM Participant INNER JOIN Users ON Participant.participant_id = Users.id WHERE participant_password = @password";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@password", _pass);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							ParticipantInfo part = new ParticipantInfo();
							part.setId(reader.GetInt32(0));
							part.setName(reader.GetString(1));
							part.setPassword(reader.GetString(2));
							part.setStatus(reader.GetString(3));
							return part;
						}
					}
				}
			}
			return null;
		}
	}


	public class ParticipantInfo
	{
		private int id;
		private string name;
		private string password;
		private string status;

		public int getId()
		{
			return id;
		}

		public void setId(int _id)
		{
			id = _id;
		}

		public string getName()
		{
			return name;
		}

		public void setName(string _name)
		{
			name = _name;
		}

		public string getPassword()
		{
			return password;
		}

		public void setPassword(string _password)
		{
			password = _password;
		}

		public string getStatus()
		{
			return status;
		}

		public void setStatus(string _status)
		{
			status = _status;
		}
	}

	public class AdminInfo
	{
		private int id;
		private string name;
		private string password;

		public int getId()
		{
			return id;
		}

		public void setId(int _id)
		{
			id = _id;
		}

		public string getName()
		{
			return name;
		}

		public void setName(string _name)
		{
			name = _name;
		}

		public string getPassword()
		{
			return password;
		}

		public void setPassword(string _password)
		{
			password = _password;
		}
	}

	public class PasswordValidation()
	{
		public static string HashPassword(string password)
		{
			byte[] salt = new byte[password.Length];
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password, salt: salt, prf: KeyDerivationPrf.HMACSHA256, iterationCount: 100000, numBytesRequested: 256 / 8));

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
