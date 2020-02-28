using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using TestComparation.DAL.TestCycle.Services;
using TestComparation.DAL.TestCycle.Models;
using AsyncAwaitBestPractices.MVVM;
using TestComparation.DAL.TestCycle.Json.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight.Command;
using TestComparation.DAL.TestCycle.Exceptions;
using TetsComparation.UI.TestCycle.Views.Bahaviors;
using System.Diagnostics;

namespace TetsComparation.UI.TestCycle.ViewModel
{
    public class TestCyclesCompareViewModel : ViewModelBase, IDisposable
    {
        private readonly HttpClient _client;
        private readonly IGetCycelServise _getCycelServise;
        private TestCycleModel _featureTestCycleModel;
        private TestCycleModel _masterTestCycleModel;        
        private CycleId _cycleId;
        private MultiSelectBehavior _multiSelect;
        private bool _isBusy;
        private string _errorMeassage;
        private string login = ConfigurationManager.AppSettings["login"];
        private string password = ConfigurationManager.AppSettings["password"];
        private IAsyncCommand _compareTestCycleCommand;
        private RelayCommand _makeArgsCommand;
        private RelayCommand _openFaq;

        /// <summary>
        /// ListView SelectedItems
        /// </summary>
        public MultiSelectBehavior MultiSelect
        {
            get { return _multiSelect; }
            set { Set(ref _multiSelect, value); }
        }       

        /// <summary>
        /// CycleIds из TextBox
        /// </summary>
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
        public RelayCommand MakeArgsCommand
        {
            get
            {
                return _makeArgsCommand ?? (_makeArgsCommand = new RelayCommand(MakeArgs));                
            }
        }       

        public TestCyclesCompareViewModel()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://jira.monopoly.su/");
            _getCycelServise = new GetCycleService(_client);            
            _cycleId = new CycleId();
            _multiSelect = new MultiSelectBehavior();
        }
      
        /// <summary>
        /// Открытие страницы FAQ
        /// </summary>
        public RelayCommand OpenFaq
        {
            get
            {               
                return _openFaq ?? (_openFaq = new RelayCommand(() =>
                {
                    Process proc = new Process();
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.FileName = "https://confluence.monopoly.su/pages/viewpage.action?pageId=55610091";
                    proc.Start();                   
                }
                ));                
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
        
        public void MakeArgs()
        {
            IsBusy = true;
            
            StringBuilder argsForJira = new StringBuilder($"-trait \"Category=");

            argsForJira.Append(string.Join($"\" -trait \"Category=", _multiSelect.SelectedItems));
            argsForJira.Append("\"");
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
