using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Princess.Bot.Services
{
    public class TriviaQuestions
    {
        public async Task<List<DTOs.Question>> GetAttendanceQuestions()
        {
            //Url:https://opentdb.com/api.php?amount=10

            var questions = new List<DTOs.Question>();

            var url = $"https://opentdb.com/api.php";
            var parameters = $"?&amount=10";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var questionList = JsonConvert.DeserializeObject<DTOs.Questions>(jsonString);

                if (questionList != null)
                {
                    questions.AddRange(questionList.QuestionList);
                }

            }
            return questions.ToList();
        }
    }
}