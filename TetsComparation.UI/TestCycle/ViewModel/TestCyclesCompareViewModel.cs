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
using System.Windows;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight.Command;

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

        private IAsyncCommand<object[]> _compareTestCycleCommand;

        private RelayCommand<object[]> _makeArgsCommand;
        private RelayCommand _openFaq;


        public IAsyncCommand<object[]> CompareTestCycleCommand 
        {
            get
            {
                return _compareTestCycleCommand ?? (_compareTestCycleCommand = new AsyncCommand<object[]>(CompareTestCycle, CanExecuteGetWeatherData));                
            }
        }
        public RelayCommand<object[]> MakeArgsCommand
        {
            get
            {
                return _makeArgsCommand ?? (_makeArgsCommand = new RelayCommand<object[]>(MakeArgs));                
            }
        }
        public RelayCommand OpenFaq
        {
            get
            {
                return _openFaq ?? (_openFaq = new RelayCommand(() => System.Diagnostics.Process.Start("https://confluence.monopoly.su/")));                
            }
        }



        public TestCyclesCompareViewModel()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://jira.monopoly.su/");
            _getCycelServise = new GetCycleService(_client);
        }

        public async Task CompareTestCycle(object[] parameters)
        {
            IsBusy = true;

            string regex = @"^\d{4}$";
            var featureCycleId = parameters[0].ToString();
            var masterCycleId = parameters[1].ToString();
            if (Regex.IsMatch(masterCycleId, regex) && Regex.IsMatch(featureCycleId, regex))
            {
                List<TestCycleData> featureCycle = await _getCycelServise.GetTestsByCycleId(featureCycleId, login, password);
                List<TestCycleData> masterCycle = await _getCycelServise.GetTestsByCycleId(masterCycleId, login, password);
                TestCycleModel master = new TestCycleModel(masterCycle);
                FeatureTestCycleModel = new TestCycleModel(featureCycle, master);
                MasterTestCycleModel = new TestCycleModel(masterCycle, FeatureTestCycleModel);
            }              
            
            IsBusy = false;
        }

        public void MakeArgs(object[] parameters)
        {
            IsBusy = true;
            
            StringBuilder argsForJira = new StringBuilder();
            var cycles = from fields in parameters
                         where (!(fields == DependencyProperty.UnsetValue))
                         from cycle in (ObservableCollection<object>)fields
                         select (string)cycle;
            foreach (var y in cycles) argsForJira.Append($"-trait \"Category={y}\" ");
            Clipboard.SetData(DataFormats.Text, argsForJira);

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
