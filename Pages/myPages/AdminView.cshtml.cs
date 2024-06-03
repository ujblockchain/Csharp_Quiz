using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BlockchainQuiz.Pages.myPages
{
    public class AdminViewModel : PageModel
    {
		private List<ParticipantInfo> listUsers = new List<ParticipantInfo>();
		private ErrorHandling feedback = new ErrorHandling();
		private List<Options> options = new List<Options>();
		private String connectionString = "Server=TANKISO-LTP\\SQLEXPRESS;Database=blockchainQuizzDatabase;Trusted_Connection=True;";

		public void OnGet()
        {
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql1 = "SELECT * FROM Participant";
					using (SqlCommand command = new SqlCommand(sql1, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								ParticipantInfo part = new ParticipantInfo();
								part.setId(reader.GetInt32(0));
								part.setName(reader.GetString(1));
								part.setStatus(reader.GetString(3));

								listUsers.Add(part);
							}
						}
					}

					String sql2 = "SELECT * FROM Options;";
					using (SqlCommand command = new SqlCommand(sql2, connection))
					{
	
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								Options option = new Options();
								option.setId(reader.GetInt32(0));
								option.setQn(reader.GetString(1));
								option.setA(reader.GetString(2));
								option.setB(reader.GetString(3));
								option.setC(reader.GetString(4));
								option.setD(reader.GetString(5));
								option.setCorrect(reader.GetString(6));

								options.Add(option);
							}
						}
					}

				}



			}catch (Exception ex) {
				feedback.setErrorMessage(ex.Message);
			}

		}

		public List<ParticipantInfo> getParticipants()
		{
			return listUsers;
		}

		public ErrorHandling getFeed()
		{
			return feedback;
		}

		public List<Options> getOptions()
		{
			return options;
		}
	}
}
