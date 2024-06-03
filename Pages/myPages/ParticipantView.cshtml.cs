using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Xml.Linq;

namespace BlockchainQuiz.Pages.myPages
{
    public class ParticipantViewModel : PageModel
    {
		private int n = 0;
		private ParticipantInfo participant = new ParticipantInfo();
		private List<Options> options = new List<Options>();
		private ErrorHandling feedback = new ErrorHandling();
		private String connectionString = "Server=TANKISO-LTP\\SQLEXPRESS;Database=blockchainQuizzDatabase;Trusted_Connection=True;";
		public void OnGet()
        {
			participant = getParticipantId(int.Parse(Request.Query["id"]));
			LoadOptions();
        }

		public void OnPost()
		{
			participant = getParticipantId(int.Parse(Request.Form["id"]));
			setN(int.Parse(Request.Form["no"]));
			LoadOptions();
			int j = getN() + 1;
			setN(j);		
						
		}

		public Options getSingleOption()
		{
			if(getN() < options.Count())
			{
				return options[getN()];
			}
			else
			{
				updateParticipantScore("Completed", participant.getId());
				Response.Redirect($"/myPages/QuizEnd?id={participant.getId()}");
				return options[0];
			}
		}

		public int getN()
		{
			return n;	
		}

		public void setN(int _n)
		{
			n = _n;
		}

		private ParticipantInfo getParticipantId(int id)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				string sql = "SELECT participant_id, participant_name, participant_password, participant_status FROM Participant WHERE participant_id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
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

		private void LoadOptions()
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
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
		}

		public ErrorHandling getFeed()
		{
			return feedback;
		}

       public ParticipantInfo GetParticipant()
	   {
			return participant;
	   }

		public List<Options> getOptions()
		{
			return options;
		}

		public void updateParticipantScore(string status, int id)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				string sql = "UPDATE Participant SET participant_status = @status WHERE participant_id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					command.Parameters.AddWithValue("@status", status);
					command.ExecuteNonQuery();
				}
			}
		}
	}
}
