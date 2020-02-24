using Newtonsoft.Json;

namespace TestComparation.DAL.TestCycle.Json.Models
{
    /// <summary>
    /// Test Cycle Data
    /// </summary>
    public class TestCycleData
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lastTestResult")]
        public LastTestResult LastTestResult { get; set; }
    }
}
