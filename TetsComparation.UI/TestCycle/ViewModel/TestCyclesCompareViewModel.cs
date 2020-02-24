using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using TestComparation.DAL.TestCycle.Services;
using TestComparation.DAL.TestCycle.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TestComparation.DAL.TestCycle.Json.Models;
using System.Collections.Generic;
using System.Configuration;

namespace TetsComparation.UI.TestCycle.ViewModel
{


    public class TestCyclesCompareViewModel : ViewModelBase, IDisposable
    {
        private readonly HttpClient _client;
        private readonly IGetCycelServise _getCycelServise;
        private TestCycleModel _featureTestCycleModel;
        private TestCycleModel _masterTestCycleModel;
        string login = ConfigurationManager.AppSettings["login"];
        string password = ConfigurationManager.AppSettings["password"];

        public TestCycleModel FeatureTestCycleModel
        {
            get { return _featureTestCycleModel; }
            set { Set(ref _featureTestCycleModel, value); }
        }
        public TestCycleModel MasterTestCycleModel
        {
            get { return _masterTestCycleModel; }
            set { Set(ref _masterTestCycleModel, value); }
        }

        private bool _isBusy;
        /// <summary>
        /// Busy indicator
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private ICommand _compareTestCycleCommand;
        public ICommand CompareTestCycleCommand 
        {
            get { return _compareTestCycleCommand ?? (_compareTestCycleCommand = new AsyncCommand(CompareTestCycle, CanExecuteGetWeatherData)); }
        }

        public TestCyclesCompareViewModel()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://jira.monopoly.su/");
            _getCycelServise = new GetCycleService(_client);
        }

        public async Task CompareTestCycle()
        {
            IsBusy = true;
            List<TestCycleData> featureCycle = await _getCycelServise.GetTestsByCycleId("2292", login, password);
            List<TestCycleData> masterCycle = await _getCycelServise.GetTestsByCycleId("1652", login, password);
            TestCycleModel master = new TestCycleModel(masterCycle);
            FeatureTestCycleModel = new TestCycleModel(featureCycle, master);            
            MasterTestCycleModel = new TestCycleModel(masterCycle, FeatureTestCycleModel);
            
            IsBusy = false;
        }

        private bool CanExecuteGetWeatherData(object? arg)
        {
            return !IsBusy;
        }
        public void Dispose()
        {
            _client?.Dispose();
        }

    }
}
