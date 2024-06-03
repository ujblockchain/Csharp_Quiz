using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;

namespace BlockchainQuiz.Pages.myPages
{
    public class EditModel : PageModel
    {
		private Options option = new Options();
		private ErrorHandling feedback = new ErrorHandling();
		private String connectionString = "Server=TANKISO-LTP\\SQLEXPRESS;Database=blockchainQuizzDatabase;Trusted_Connection=True;";

		public void OnGet()
        {
			int id = int.Parse(Request.Query["id"]);

			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM Options WHERE options_id=@id;";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								option.setId(reader.GetInt32(0));
								option.setQn(reader.GetString(1));
								option.setA(reader.GetString(2));
								option.setB(reader.GetString(3));
								option.setC(reader.GetString(4));
								option.setD(reader.GetString(5));
								option.setCorrect(reader.GetString(6));
							}
						}
					}
				}

			}
			catch (Exception ex)
			{
				feedback.setErrorMessage(ex.Message);
			}

		}

		public void OnPost()
		{
			option.setId(int.Parse(Request.Form["id"]));
			option.setQn(Request.Form["question"]);
			option.setA(Request.Form["option1"]);
			option.setB(Request.Form["option2"]);
			option.setC( Request.Form["option3"]);
			option.setD(Request.Form["option4"]);
			option.setCorrect(Request.Form["correct"]);

	
			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE Options SET options_qn =@qn, option_a = @a, option_b = @b, option_c = @c, option_d = @d, option_correct = @correct WHERE options_id = @id;";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", option.getId());
						command.Parameters.AddWithValue("@qn", option.getQn());
						command.Parameters.AddWithValue("@a", option.getA());
						command.Parameters.AddWithValue("@b", option.getB());
						command.Parameters.AddWithValue("@c", option.getC());
						command.Parameters.AddWithValue("@d", option.getD());
						command.Parameters.AddWithValue("@correct", option.getCorrect());
						command.ExecuteNonQuery();
					}
				}
				feedback.setSuccessMessage("Question edited successfully");
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


		public Options getOption()
		{
			return option;
		}
	}
}
