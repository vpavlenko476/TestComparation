using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Net;

namespace Model
{
    public class TestComparationModel: BindableBase
    {
		private readonly ObservableCollection<string> privateIndividualMasterFaildTests = new ObservableCollection<string>();
		private readonly ObservableCollection<string> privateIndividualMasterNotExecutedTests = new ObservableCollection<string>();
		private readonly ObservableCollection<string> privateIndividualFeatureFaildTests = new ObservableCollection<string>();
		private readonly ObservableCollection<string> privateIndividualFeatureNotExecutedTests = new ObservableCollection<string>();
		private string errorMeassage = null;

		/// <summary>
		/// Тесты, упавшие на мастере, но прошедшие на фиче
		/// </summary>
		public readonly ReadOnlyObservableCollection<string> IndividualMasterFaildTests;

		/// <summary>
		/// Тесты, не запущенные на мастере, но запущенные на фиче 
		/// </summary>
		public readonly ReadOnlyObservableCollection<string> IndividualMasterNotExecutedTests;

		/// <summary>
		/// Тесты, упавшие на фиче, но прошедшие на мастере
		/// </summary>
		public readonly ReadOnlyObservableCollection<string> IndividualFeatureFaildTests;

		/// <summary>
		/// Тесты, не запущенные на фиче, но запущенные на мастере 
		/// </summary>
		public readonly ReadOnlyObservableCollection<string> IndividualFeatureNotExecutedTests;
		
		/// <summary>
		/// Сообщения об ошибке
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				return errorMeassage;
			}
		}

		public TestComparationModel()
		{
			IndividualMasterFaildTests = new ReadOnlyObservableCollection<string>(privateIndividualMasterFaildTests);
			IndividualMasterNotExecutedTests = new ReadOnlyObservableCollection<string>(privateIndividualMasterNotExecutedTests);
			IndividualFeatureFaildTests = new ReadOnlyObservableCollection<string>(privateIndividualFeatureFaildTests);
			IndividualFeatureNotExecutedTests = new ReadOnlyObservableCollection<string>(privateIndividualFeatureNotExecutedTests);
		}
		
		/// <summary>
		/// Сравнение двух прогонов
		/// </summary>		
		public void TestCycleCompare(string masterCycleId, string featureCycleId)
		{
			try
			{
				TestCycle masterCycle = new TestCycle(Request.GetTestCycleResult(masterCycleId));
				TestCycle featureCycle = new TestCycle(Request.GetTestCycleResult(featureCycleId));
				privateIndividualMasterFaildTests.Clear();
				privateIndividualMasterNotExecutedTests.Clear();
				privateIndividualFeatureFaildTests.Clear();
				privateIndividualFeatureNotExecutedTests.Clear();
				foreach (string test in masterCycle.GetIndividualFaildTests(featureCycle)) privateIndividualMasterFaildTests.Add(test);
				foreach (string test in masterCycle.GetIndividualNotExecutedTest(featureCycle)) privateIndividualMasterNotExecutedTests.Add(test);
				foreach (string test in featureCycle.GetIndividualFaildTests(masterCycle)) privateIndividualFeatureFaildTests.Add(test);
				foreach (string test in featureCycle.GetIndividualNotExecutedTest(masterCycle)) privateIndividualFeatureNotExecutedTests.Add(test);
				errorMeassage = null;
			}
			catch(WebException ex)
			{
				errorMeassage = ex.Message;				
			}
			catch(Exception ex)
			{
				errorMeassage = ex.Message;
			}
			finally
			{
				RaisePropertyChanged("ErrorMessage");
			}				
		}
	}
}
