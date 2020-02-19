using System.Collections.Generic;
using System.Linq;

namespace Model
{
    internal class TestCycle
    {
		/// <summary>
		/// Все непрошедшие тесты
		/// </summary>
        public List<string> AllFaildTests { get; private set; }

		/// <summary>
		/// Все прошедшие тесты
		/// </summary>
        public List<string> AllPassTests { get; private set; }

		/// <summary>
		/// Все запущенные тесты
		/// </summary>
        public List<string> AllExecutedTests { get; private set; }      
		
        public TestCycle(List<TestCycleJsonModel> testCycleResult)
        {
            AllExecutedTests = testCycleResult.Select(e => e.lastTestResult.testCase.key).ToList<string>();
            AllFaildTests = testCycleResult
                .Where(e => (e.lastTestResult.testResultStatusId == 23))
                .Select(e => e.lastTestResult.testCase.key).ToList<string>();
            AllPassTests = testCycleResult
                .Where(e => (e.lastTestResult.testResultStatusId == 22))
                .Select(e => e.lastTestResult.testCase.key).ToList<string>();            
        }

		/// <summary>
		/// Тесты, упавшие в данном TestCуcle, но прошедшие в cycleToCompare
		/// </summary>		
		public List<string> GetIndividualFaildTests(TestCycle cycleToCompare) => this.AllFaildTests.Except(cycleToCompare?.AllFaildTests).ToList<string>();

		/// <summary>
		/// Тесты, не запущенные в данном TestCуcle, но запущенные в cycleToCompare
		/// </summary>		
		public List<string> GetIndividualNotExecutedTest(TestCycle cycleToCompare) => cycleToCompare?.AllExecutedTests.Except(this.AllExecutedTests).ToList<string>();              
    }
}
