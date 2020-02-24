using System.Collections.Generic;
using System.Threading.Tasks;
using TestComparation.DAL.TestCycle.Json.Models;

namespace TestComparation.DAL.TestCycle.Services
{
    /// <summary>
    /// Service to get Jira testcycle
    /// </summary>
    public interface IGetCycelServise
    {
        /// <summary>
        /// Get tests in cycle by cycle Id
        /// </summary>
        /// <param name="cycleId">jira cycleId</param>
        /// <param name="login">jira login</param>
        /// <param name="password">jira password</param>
        /// <returns></returns>
        Task<List<TestCycleData>> GetTestsByCycleId(string cycleId, string login, string password);
    }
}


