using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using TestComparation.DAL.TestCycle.Services;
using TestComparation.DAL.TestCycle.Models;
using System.Collections.ObjectModel;
using AsyncAwaitBestPractices.MVVM;
using TestComparation.DAL.TestCycle.Json.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight.Command;
using TestComparation.DAL.TestCycle.Exceptions;

namespace TetsComparation.UI.TestCycle.ViewModel
{
    public class TestCyclesCompareViewModel : ViewModelBase, IDisposable
    {
        private readonly HttpClient _client;
        private readonly IGetCycelServise _getCycelServise;
        private TestCycleModel _featureTestCycleModel;
        private TestCycleModel _masterTestCycleModel;
        private string login = ConfigurationManager.AppSettings["login"];
        private string password = ConfigurationManager.AppSettings["password"];

        private ListViewSelectedItem _selectedItems;
        private CycleId _cycleId;
        public ListViewSelectedItem SelectedItems
        {
            get { return _selectedItems; }
            set { Set(ref _selectedItems, value); }
        }

        public CycleId TestCycleId
        {
            get { return _cycleId; }
            set { Set(ref _cycleId, value); }
        }

        /// <summary>
        /// Экземпляр тестового прогона на Feature ветке
        /// </summary>
        public TestCycleModel FeatureTestCycleModel
        {
            get { return _featureTestCycleModel; }
            set { Set(ref _featureTestCycleModel, value); }
        }        

        /// <summary>
        /// Экземпляр тестового прогона на Master ветке
        /// </summary>
        public TestCycleModel MasterTestCycleModel
        {
            get { return _masterTestCycleModel; }
            set { Set(ref _masterTestCycleModel, value); }
        }


        private bool _isBusy;

        private string _errorMeassage;

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _errorMeassage;
            }
            set
            {
                Set(ref _errorMeassage, value);
            }
        }

        /// <summary>
        /// Busy indicator
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        private IAsyncCommand _compareTestCycleCommand;
        private RelayCommand<object[]> _makeArgsCommand;
        private RelayCommand _openFaq;

        /// <summary>
        /// Сравнение тестовых прогонов
        /// </summary>
        public IAsyncCommand CompareTestCycleCommand 
        {
            get
            {
                return _compareTestCycleCommand ?? (_compareTestCycleCommand = new AsyncCommand(CompareTestCycle, CanExecuteGetWeatherData));                
            }
        }

        /// <summary>
        /// Создание строки для Developers Integration tests arguments
        /// </summary>
        public RelayCommand<object[]> MakeArgsCommand
        {
            get
            {
                return _makeArgsCommand ?? (_makeArgsCommand = new RelayCommand<object[]>(MakeArgs));                
            }
        }       

        public TestCyclesCompareViewModel()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://jira.monopoly.su/");
            _getCycelServise = new GetCycleService(_client);
            _selectedItems = new ListViewSelectedItem();
            _cycleId = new CycleId();
            
        }

         /// <summary>
        /// Открытие страницы FAQ
        /// </summary>
        public RelayCommand OpenFaq
        {
            get
            {
                return _openFaq ?? (_openFaq = new RelayCommand(() => System.Diagnostics.Process.Start("explorer.exe", "https://confluence.monopoly.su/pages/viewpage.action?pageId=55610091")));                
            }
        }

        public async Task CompareTestCycle()
        {
            IsBusy = true;
            
            var masterCycleId = TestCycleId.MasterCycleId;
            var featureCycleId = TestCycleId.FeatureCycleId;            
            string regex = @"^\d{3,4}$";            
            try
            {
                if (Regex.IsMatch(masterCycleId, regex) && Regex.IsMatch(featureCycleId, regex))
                {
                    List<TestCycleData> featureCycle = await _getCycelServise.GetTestsByCycleId(featureCycleId, login, password);
                    List<TestCycleData> masterCycle = await _getCycelServise.GetTestsByCycleId(masterCycleId, login, password);
                    TestCycleModel master = new TestCycleModel(masterCycle);
                    FeatureTestCycleModel = new TestCycleModel(featureCycle, master);
                    MasterTestCycleModel = new TestCycleModel(masterCycle, FeatureTestCycleModel);
                    ErrorMessage = null;
                }
            }
            catch(GetTestCycleException ex)
            {
                ErrorMessage = ex.Message;
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Необработанная ошибка {ex.Message}";
            }
            
            IsBusy = false;
        }

        public void MakeArgs(object[] parameters)
        {
            IsBusy = true;
            
            StringBuilder argsForJira = new StringBuilder();

            #region Жуткий костыль, пока не придумал как обойти, скрою а то страшно
            if (_selectedItems.MasterFaildTests!=null)
            {
                foreach (var cycleId in _selectedItems.MasterFaildTests)
                {
                    argsForJira.Append($"-trait \"Category={cycleId}\" ");
                }
            }

            if (_selectedItems.MasterNotExecutedTests!=null)
            {
                foreach (var cycleId in _selectedItems.MasterNotExecutedTests)
                {
                    argsForJira.Append($"-trait \"Category={cycleId}\" ");
                }
            }

            if (_selectedItems.FeatureFaildTests!=null)
            {
                foreach (var cycleId in _selectedItems.FeatureFaildTests)
                {
                    argsForJira.Append($"-trait \"Category={cycleId}\" ");
                }
            }

            if (_selectedItems.FeatureNotExecutedTests!=null)
            {
                foreach (var cycleId in _selectedItems.FeatureNotExecutedTests)
                {
                    argsForJira.Append($"-trait \"Category={cycleId}\" ");
                }
            }
            #endregion

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
