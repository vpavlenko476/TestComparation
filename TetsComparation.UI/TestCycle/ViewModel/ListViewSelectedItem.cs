using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace TetsComparation.UI.TestCycle.ViewModel
{
    public class ListViewSelectedItem : ObservableObject
    {

        private string _masterFaildTest;
        private string _masterNotExecutedTests;
        private string _featureFaildTests;
        private string _featureNotExecutedTests;

        public string MasterFaildTests
        {
            get { return _masterFaildTest; }
            set { Set(ref _masterFaildTest, value); }
        }
        public string MasterNotExecutedTests
        {
            get { return _masterNotExecutedTests; }
            set { Set(ref _masterNotExecutedTests, value); }
        }
        public string FeatureFaildTests
        {
            get { return _featureFaildTests; }
            set { Set(ref _featureFaildTests, value); }
        }
        public string FeatureNotExecutedTests
        {
            get { return _featureNotExecutedTests; }
            set { Set(ref _featureNotExecutedTests, value); }
        }
    }
}
