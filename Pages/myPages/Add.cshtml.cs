using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace BlockchainQuiz.Pages.myPages
{
    public class AddModel : PageModel
    {
		private Options option = new Options();
		private ErrorHandling feedback = new ErrorHandling();
		private String connectionString = "Server=TANKISO-LTP\\SQLEXPRESS;Database=blockchainQuizzDatabase;Trusted_Connection=True;";

		public void OnGet()
        {

        }

		public void OnPost() {
			string qn = Request.Form["question"];
			string op1 = Request.Form["option1"];
			string op2 = Request.Form["option2"];
			string op3 = Request.Form["option3"];
			string op4 = Request.Form["option4"];
			string correct = Request.Form["correct"];

			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "INSERT INTO Options(options_qn, option_a, option_b, option_c, option_d, option_correct) VALUES(@qn, @a, @b, @c, @d, @correct);";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@qn", qn);
						command.Parameters.AddWithValue("@a", op1);
						command.Parameters.AddWithValue("@b", op2);
						command.Parameters.AddWithValue("@c", op3);
						command.Parameters.AddWithValue("@d", op4);
						command.Parameters.AddWithValue("@correct", correct);
						command.ExecuteNonQuery();
					}
				}
				feedback.setSuccessMessage("Question added successfully");
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
	}

    public class Options
    {
        private int options_id;
        private string options_qn;
        private string options_a;
		private string options_b;
		private string options_c;
		private string options_d;
		private string options_correct;

        public int getId()
        {
            return options_id;  
        }

        public void setId(int id) {
            this.options_id = id;
        }

        public string getQn()
        {
            return options_qn;  
        }

        public void setQn(string qn)
        {
            options_qn = qn;    
        }

        public string getA()
        {
            return options_a;
        }

        public void setA(string a) { 
            options_a = a;
        }

		public string getB()
		{
			return options_b;
		}

		public void setB(string b)
		{
			options_b = b;
		}

		public string getC()
		{
			return options_c;
		}

		public void setC(string c)
		{
			options_c = c;
		}

		public string getD()
		{
			return options_d;
		}

		public void setD(string d)
		{
			options_d = d;
		}

		public string getCorrect()
		{
			return options_correct;
		}

		public void setCorrect(string correct)
		{
			options_correct = correct;
		}

	}
}
