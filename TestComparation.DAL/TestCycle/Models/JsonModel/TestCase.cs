using Newtonsoft.Json;

namespace TestComparation.DAL.TestCycle.Json.Models
{
    public class TestCase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("majorVersion")]
        public int MajorVersion { get; set; }

        [JsonProperty("projectId")]
        public int ProjectId { get; set; }

        [JsonProperty("folderId")]
        public int FolderId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("priorityId")]
        public int PriorityId { get; set; }
    }
}
