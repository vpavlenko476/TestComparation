using Newtonsoft.Json;

namespace TestComparation.DAL.TestCycle.Json.Models
{
    public class LastTestResult
    {
        [JsonProperty("environmentId")]
        public int EnvironmentId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userKey")]
        public string UserKey { get; set; }

        [JsonProperty("testCase")]
        public TestCase TestCase { get; set; }

        [JsonProperty("testResultStatusId")]
        public int TestResultStatusId { get; set; }
    }
}
