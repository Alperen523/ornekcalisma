using ConsoleTest.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;

namespace ConsoleTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var step = args[0];
            //var step = "SaveFile";
            ////Thread[] threads = new Thread[10000];

            ////for (int i = 0; i < threads.Length; i++)
            ////{
            ////    threads[i] = new Thread(() => { DoSomeWork(); });
            ////}

            ////foreach (Thread thread in threads)
            ////{
            ////    thread.Start();
            ////}

            ////foreach (Thread thread in threads)
            ////{
            ////    thread.Join();
            ////}


            //double sonuc = 0;
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //Parallel.For(0, 200, new ParallelOptions { MaxDegreeOfParallelism = 20 }, i =>
            //    {
            //        DoSomeWork();
            //    }); ; ;
            //sw.Stop();
            //Console.WriteLine("Paralel İşlemin Tamamlanma Süresi : " + sw.Elapsed.TotalMilliseconds.ToString());
            //for (int i = 0; i < 100; i++)
            //{
            //    await CrateUserBulkTestAsync();

            //}

            //List<string> respList = new List<string>();
            ////var Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 4 }, async i =>
            ////    {
            ////        var response = await CrateUserBulkTestAsync();
            ////        respList.Add(response);
            ////    });

            //var tasks = Enumerable.Range(0, 4)
            //.Select(i => CrateUserBulkTestAsync());
            //List<string> allSheets = (await Task.WhenAll(tasks)).ToList();

            //foreach (var item in allSheets)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(DateTime.Now.ToString());
            switch (step)
            {
                case "SaveFile":
                    {
                        var client = new RestClient("http://localhost:6060/api/Sms/TestSmsSaveFileLogAsync");

                        var request = new RestRequest();
                        request.Method = Method.Post;
                        request.AddHeader("accept", "application/json");

                        var resp = await client.ExecuteAsync(request);
                    }
                    break;
                case "SaveDatabase":
                    {
                        var client = new RestClient("http://localhost:6060/api/Sms/TestSmsSaveDatabaseLogAsync");

                        var request = new RestRequest();
                        request.Method = Method.Post;
                        request.AddHeader("accept", "application/json");
                        var resp = await client.ExecuteAsync(request);
                    }
                    break;
                default:
                    break;
            }




        }

        public static void DoSomeWork()
        {
            //LoginRequest req = new LoginRequest();
            //req.Username = "crmapi";
            //req.Password = "C@linS";
            //var jsonText = JsonConvert.SerializeObject(req);
            //var client = new RestClient("http://localhost:44399/api/Login/Authenticate");


            //var request = new RestRequest();
            //request.Method = Method.POST;
            //request.AddHeader("Content-Type", "application/json");
            //request.AddStringBody(jsonText, DataFormat.Json);
            //var response = client.ExecuteAsync(request).GetAwaiter().GetResult();
            //Console.WriteLine(response.Content);

        }

        public static async Task<string> CrateUserBulkTestAsync()
        {
            var client = new RestClient("https://random-data-api.com/api/users/random_user");

            var request = new RestRequest();
            request.Method = Method.Get;
            //request.AddHeader("accept", "application/json");
            //request.AddHeader("content-type", "application/json");

            RandomUser response = new RandomUser();

            var resp = await client.ExecuteAsync(request);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                response = JsonConvert.DeserializeObject<RandomUser>(resp.Content);
            }

            if (response == null) return null;

            DateTime birthDate = new DateTime();
            Random rnd = new Random();

            ContactSaveRequestDto requestDto = new ContactSaveRequestDto
            {
                FirstName = response.first_name,
                LastName = response.last_name,
                ChannelId = UzmanCrm.CrmService.Common.Enums.ChannelEnum.ETicaret,
                CountryPhoneCode = "90",
                DigitalFormTypeId = UzmanCrm.CrmService.Common.Enums.DigitalFormEnum.Dijital,
                BirthDate = DateTime.TryParse(response.date_of_birth, out birthDate) ? birthDate : DateTime.Parse("1950-01-01"),
                CrmId = null,
                DoubleOptinCode = rnd.Next(10000, 99999).ToString(),
                GenderId = response.gender.ToLower() == "female" ? UzmanCrm.CrmService.Common.Enums.GenderEnum.Kadin : UzmanCrm.CrmService.Common.Enums.GenderEnum.Erkek,
                IdentificationNumber = LongRandom(50111111111, 55911111111, rnd).ToString(),
                IsKvkk = false,
                Location = "CL",
                OrganizationId = UzmanCrm.CrmService.Common.Enums.OrganizationEnum.TR,
                PersonNo = "0123456789",

                Email = new Email
                {
                    EmailAddress = response.email,
                    EmailPermit = false
                }

            };

            var clientLive = new RestClient("http://crmapi-test.vakko.com.tr/api/Contact/CustomerSave");

            var requestLive = new RestRequest();
            requestLive.Method = Method.Post;
            requestLive.AddHeader("accept", "application/json");
            requestLive.AddHeader("content-type", "application/json");
            var requestJson = JsonConvert.SerializeObject(requestDto);
            requestLive.AddStringBody(requestJson, DataFormat.Json);

            var respLive = await clientLive.ExecutePostAsync(requestLive);
            Console.WriteLine(respLive.Content);
            return respLive.Content;
        }

        static long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }


    }
}
