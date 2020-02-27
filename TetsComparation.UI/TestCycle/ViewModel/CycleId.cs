using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace TetsComparation.UI.TestCycle.ViewModel
{
    public class CycleId : ObservableObject
    {
        private string _featureCycleId;
        private string _masterCycleId;

        public string FeatureCycleId
        {
            get { return _featureCycleId; }
            set { Set(ref _featureCycleId, value); }
        }
         public string MasterCycleId
        {
            get { return _masterCycleId; }
            set { Set(ref _masterCycleId, value); }
        }

           
    }
}
