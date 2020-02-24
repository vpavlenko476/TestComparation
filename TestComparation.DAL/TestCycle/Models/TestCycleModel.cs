using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestComparation.DAL.TestCycle.Json.Models;

namespace TestComparation.DAL.TestCycle.Models
{
    public class TestCycleModel
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

        /// <summary>
        /// Тесты, упавшие в данном TestCуcle, но прошедшие в cycleToCompare
        /// </summary>		
        public List<string> IndividualFaildTests { get; private set; }

        /// <summary>
        /// Тесты, не запущенные в данном TestCуcle, но запущенные в cycleToCompare
        /// </summary>		
        public List<string> IndividualNotExecutedTest { get; private set; }

        public TestCycleModel(List<TestCycleData> testCycleData)
        {
            AllExecutedTests = testCycleData.Select(e => e.LastTestResult.TestCase.Key).ToList<string>();
            AllFaildTests = testCycleData
                .Where(e => (e.LastTestResult.TestResultStatusId == 23))
                .Select(e => e.LastTestResult.TestCase.Key).ToList<string>();
            AllPassTests = testCycleData
                .Where(e => (e.LastTestResult.TestResultStatusId == 22))
                .Select(e => e.LastTestResult.TestCase.Key).ToList<string>();
        }

        public TestCycleModel(List<TestCycleData> testCycleData, TestCycleModel cycleToCompare): this(testCycleData)
        {
            IndividualFaildTests = this.AllFaildTests.Except(cycleToCompare?.AllFaildTests).ToList<string>();
            IndividualNotExecutedTest = cycleToCompare?.AllExecutedTests.Except(this.AllExecutedTests).ToList<string>();
        }                
    }
}
