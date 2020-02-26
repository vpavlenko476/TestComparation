using System.Collections.Generic;
using System.Threading.Tasks;
using TestComparation.DAL.TestCycle.Json.Models;

namespace TestComparation.DAL.TestCycle.Services
{
    /// <summary>
    /// Сервис для получения тестового прогона из Jira
    /// </summary>
    public interface IGetCycelServise
    {
        /// <summary>
        /// Получение тестов их тестового прогона по CycleId
        /// </summary>
        /// <param name="cycleId">jira cycleId</param>
        /// <param name="login">jira login</param>
        /// <param name="password">jira password</param>
        /// <returns></returns>
        Task<List<TestCycleData>> GetTestsByCycleId(string cycleId, string login, string password);
    }
}


