using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TestComparation.DAL.TestCycle.Json.Models;
using TestComparation.DAL.TestCycle.Consts;
using System.Collections.Generic;
using System.Net;
using TestComparation.DAL.TestCycle.Exceptions;

namespace TestComparation.DAL.TestCycle.Services
{
    /// <inheritdoc cref="IGetCycelServise"/>
    public class GetCycleService : IGetCycelServise
    {
        private readonly HttpClient _client;

        public GetCycleService(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc cref="IGetCycelServise.GetTestsByCycleId"/>
        public async Task<List<TestCycleData>> GetTestsByCycleId(string cycleId, string login, string password)
        {
            List<TestCycleData> testCycleData = null;
            var response = await _client.GetAsync(string.Format(ApiUrls.GetTestsInCycle, cycleId.ToString(), login, password));
            if(response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var jsonBody = jsonData.Replace("$", "");
                testCycleData = JsonConvert.DeserializeObject<List<TestCycleData>>(jsonBody);
            }
            else if(response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new GetTestCycleException($"Неверные логин/пароль");
            }
            else if(response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new GetTestCycleException($"Указанного прогона {cycleId} не сущетсвует");
            }
           
            return testCycleData;
        }
    }
}
