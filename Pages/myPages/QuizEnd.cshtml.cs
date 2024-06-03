using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlockchainQuiz.Pages.myPages
{
    public class QuizEndModel : PageModel
    {
        private int _id;
        private int _score;

        public void OnGet()
        {
			setId(int.Parse(Request.Query["id"]));
		}

        public int getId()
        {
            return _id; 
        }

		public void setId(int _i)
		{
			_id = _i;
		}
	}
}
