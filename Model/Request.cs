using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;

namespace Model
{
    internal static class Request
    {
		/// <summary>
		/// Метод для парсинга json прогона
		/// </summary>
		/// <param name="cycleId">Номер прогона из Jira</param>
		/// <returns>Коллекия TestCycleJsonModel на основе json модели</returns>
		public static List<TestCycleJsonModel> GetTestCycleResult(string cycleId)
        {
            string login = ConfigurationManager.AppSettings["login"];
            string password = ConfigurationManager.AppSettings["password"];
            string jsonBody = null;
            Uri jiraUrl = new Uri($"https://jira.monopoly.su/rest/tests/1.0/testrun/{cycleId}/testrunitems?fields=id,index,issue&os_username={login}&os_password={password}");
            try
            {
                var request = WebRequest.Create(jiraUrl);
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        jsonBody = json.Replace("$", "");
                        streamReader.Close();                        
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.Forbidden)
                    {
                        throw new WebException($"Неверные логин/пароль");
                    }
                    else if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new WebException($"Указанного прогона {cycleId} не сущетсвует");
                    }                    
                }
            }
            return JsonConvert.DeserializeObject<List<TestCycleJsonModel>>(jsonBody);
        }            
    }
}
