using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetsComparation.UI.TestCycle.ViewModel
{
    public class ListViewSelectedItem : ObservableObject
    {

        private IList _masterFaildTest;
        private IList _masterNotExecutedTests;
        private IList _featureFaildTests;
        private IList _featureNotExecutedTests;    

        public IList MasterFaildTests
        {
            get { return _masterFaildTest; }
            set { Set(ref _masterFaildTest, value); }           
        }
        public IList MasterNotExecutedTests
        {
            get { return _masterNotExecutedTests; }
            set { Set(ref _masterNotExecutedTests, value); }
        }
        public IList FeatureFaildTests
        {
            get { return _featureFaildTests; }
            set { Set(ref _featureFaildTests, value); }
        }
        public IList FeatureNotExecutedTests
        {
            get { return _featureNotExecutedTests; }
            set { Set(ref _featureNotExecutedTests, value); }
        }        
    }
}
