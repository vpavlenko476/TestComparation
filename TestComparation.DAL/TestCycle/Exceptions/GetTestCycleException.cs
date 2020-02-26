using System;
using System.Collections.Generic;
using System.Text;

namespace TestComparation.DAL.TestCycle.Exceptions
{
    /// <summary>
    /// Исключение для ошибок при запросе тестого прогона
    /// </summary>
    public class GetTestCycleException: Exception
    {
        public GetTestCycleException(string meassage) : base(meassage)
        { 
        }
    }
}
