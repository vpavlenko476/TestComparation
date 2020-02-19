using Model;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ViewModel
{
    public class TestComparationViewModel : BindableBase
    {
        private readonly static TestComparationModel model = new TestComparationModel();

		/// <summary>
		/// Сравнение двух прогонов
		/// </summary>
        public DelegateCommand<object[]> CompareCommand { get; }
		
		/// <summary>
		/// Формирование в буфере строки arguments
		/// </summary>
        public DelegateCommand<object[]> MakeArgsCommand { get; }

		/// <summary>
		/// Открытие страницы FAQ
		/// </summary>
		public DelegateCommand OpenFaq { get; }

		/// <summary>
		/// Номер прогона ветки мастера
		/// </summary>
        public static string MasterCycleId { get; set; }

		/// <summary>
		/// Номер прогона ветки фичи
		/// </summary>
        public static string FeatureCycleId { get; set; }

		/// <summary>
		/// Сообщения об ошибке
		/// </summary>
        public string ErrorMessage
        {
            get
            {
                return model.ErrorMessage;
            }
        }
		
		public ReadOnlyObservableCollection<string> IndividualFeatureFaildTests
        {
            get
            {
                return model.IndividualFeatureFaildTests;
            }
        }
        public ReadOnlyObservableCollection<string> IndividualFeatureNotExecutedTests
        {
            get
            {
                return model.IndividualFeatureNotExecutedTests;
            }
        }
		
		public ReadOnlyObservableCollection<string> IndividualMasterFaildTests
        {
            get
            {
                return model.IndividualMasterFaildTests;
            }
        }
        public ReadOnlyObservableCollection<string> IndividualMasterNotExecutedTests
        {
            get
            {
                return model.IndividualMasterNotExecutedTests;
            }
        }
				
		public TestComparationViewModel()
        {
            model.PropertyChanged += Model_PropertyChanged;

            CompareCommand = new DelegateCommand<object[]>(parameters=> 
			{
            string regex = @"^\d{4}$";
            MasterCycleId = parameters[0].ToString();            
            FeatureCycleId = parameters[1].ToString();              
            if (Regex.IsMatch(MasterCycleId,regex) && Regex.IsMatch(FeatureCycleId, regex))
                {
                    model.TestCycleCompare(MasterCycleId, FeatureCycleId);
                }  
            });

            MakeArgsCommand = new DelegateCommand<object[]>(parameters =>
            {
                StringBuilder argsForJira = new StringBuilder();                
                var cycles = from fields in parameters
                             where (!(fields == DependencyProperty.UnsetValue))
                             from cycle in (ObservableCollection<object>)fields                            
                             select (string)cycle;                
                foreach (var y in cycles) argsForJira.Append($"-trait \"Category={y}\" ");               
                Clipboard.SetData(DataFormats.Text, argsForJira);
            });

			OpenFaq = new DelegateCommand(() => System.Diagnostics.Process.Start("https://confluence.monopoly.su/"));
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }
    }
}
