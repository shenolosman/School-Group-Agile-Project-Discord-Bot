using Newtonsoft.Json;

namespace Princess.DTOs;

[Serializable]
public class Question
{
    [JsonProperty("category")] public string Category { get; set; }

    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("difficulty")] public string Difficulty { get; set; }

    [JsonProperty("question")] public string QuestionString { get; set; }

    [JsonProperty("correct_answer")] public string CorrectAnswer { get; set; }

    [JsonProperty("incorrect_answers")] public string[] IncorrectAnswers { get; set; } = new string[3];
}

[Serializable]
public class Questions
{
    [JsonProperty("results")] public IEnumerable<Question> QuestionList { get; set; }
}