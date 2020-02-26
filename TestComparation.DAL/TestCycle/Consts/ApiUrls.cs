using System;
using System.Collections.Generic;
using System.Text;

namespace TestComparation.DAL.TestCycle.Consts
{
    /// <summary>
    /// Urls
    /// </summary>
    public static class  ApiUrls
    {
        /// <summary>
        /// Url для получения тестов из тестового прогона
        /// </summary>
        /// <remarks>
        /// {0} - cycel Id
        /// {1} - Jira login
        /// {2} - Jira password
        /// </remarks>
        public const string GetTestsInCycle = "rest/tests/1.0/testrun/{0}/testrunitems?fields=id,index,issue&os_username={1}&os_password={2}";
    }
}
