using System.Net.Http.Headers;
using Newtonsoft.Json;
using Princess.DTOs;

namespace Princess.Services;

public class TriviaQuestions
{
    public async Task<Question> GetAttendanceQuestions()
    {
        //Url:https://opentdb.com/api.php?amount=1&type=multiple

        var question = new Question();

        var url = "https://opentdb.com/api.php";
        var parameters = "?&amount=1&type=multiple";

        var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync(parameters).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var questionItem = JsonConvert.DeserializeObject<Questions>(jsonString).QuestionList.First();

            question = questionItem;
        }

        return question;
    }
}