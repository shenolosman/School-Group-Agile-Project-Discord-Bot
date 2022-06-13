using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Princess.Bot.Services
{
    public class TriviaQuestions
    {
        public async Task<DTOs.Question> GetAttendanceQuestions()
        {
            //Url:https://opentdb.com/api.php?amount=1&type=multiple

            var question = new DTOs.Question();

            var url = $"https://opentdb.com/api.php";
            var parameters = $"?&amount=1&type=multiple";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var questionItem = JsonConvert.DeserializeObject<DTOs.Questions>(jsonString).QuestionList.First();

                question = questionItem;
            }

            return question;

        }
    }
}
