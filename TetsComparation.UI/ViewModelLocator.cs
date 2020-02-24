using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using TetsComparation.UI.TestCycle.ViewModel;

namespace TetsComparation.UI
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {

            }
            else
            {

            }

            SimpleIoc.Default.Register<TestCyclesCompareViewModel>();
        }

        public TestCyclesCompareViewModel TestCyclesCompareMain
        {
            get { return SimpleIoc.Default.GetInstance<TestCyclesCompareViewModel>(); }
        }
    }
}
