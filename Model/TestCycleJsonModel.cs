
namespace Model
{
    internal class TestCycleJsonModel
    {
        public int index { get; set; }
        public int id { get; set; }
        public LastTestResult lastTestResult { get; set; }

        public class LastTestResult
        {
            public int EnvironmentId { get; set; }
            public int id { get; set; }
            public string userKey { get; set; }
            public TestCase testCase { get; set; }
            public int testResultStatusId { get; set; }
        }

        public class TestCase
        {
            public string name { get; set; }
            public int id { get; set; }
            public int majorVersion { get; set; }
            public int projectId { get; set; }
            public int folderId { get; set; }
            public string key { get; set; }
            public int priorityId { get; set; }
        }
    }
}
