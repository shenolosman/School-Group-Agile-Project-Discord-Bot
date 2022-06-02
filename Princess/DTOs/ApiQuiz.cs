using Newtonsoft.Json;

namespace Princess.DTOs
{
    [Serializable]
    public class Question
    {
        [JsonProperty("question")]
        public string QuestionString { get; set; }
    }
    [Serializable]
    public class Questions
    {
        [JsonProperty("results")]
        public IEnumerable<Question> QuestionList { get; set; }
    }
    public class ApiQuiz
    {
    }
}
