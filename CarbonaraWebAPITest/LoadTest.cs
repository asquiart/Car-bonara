using System.Net;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace CarbonaraWebAPITest
{
    public class LoadTest
    {
        private readonly ITestOutputHelper TestOutput;
        public LoadTest(ITestOutputHelper output)
        {
            TestOutput = output;
        }



        [Fact]
        public void MetaTest()
        {
            Assert.True(true);
        }

       // [Fact]
        public void Test9Requests()
        {
            TestServerLoad(9, 2000);
        }

       // [Fact]
        public void Test100Requests()
        {
            TestServerLoad(100, 4000);
        }

        private void TestServerLoad(int numberOfRequests, int maxTimePerRequest)
        {
            ServicePointManager.DefaultConnectionLimit = numberOfRequests + 1; //To enable the wanted amount of requests
            const string pingApi = "https://www.car-bonara.de/api/stationdatabase/getall"; //API to request

            var info = new ServerLoadInfo(TestOutput);
            var runnerInfo = new ServerLoadInfo.RunnerInfo();

            HttpClient client = new HttpClient();
            Task[] tasks = new Task[numberOfRequests];
            client.Timeout = new TimeSpan(0, 0, 0, 0, maxTimePerRequest);

            int maxTime = 0;
            int minTime = int.MaxValue;
            
            //Create n requests
            for (int i = 0; i < numberOfRequests; i++)
            {
                tasks[i] = Task.Run(async () =>
                    {
                        int nr = 0;
                        lock (runnerInfo)
                        {
                            nr = runnerInfo.GetRunnerId();
                        }

                        long startedTime = Environment.TickCount64;
                        var result = await client.GetAsync(pingApi); //Request
                        long requestTime = Environment.TickCount64 - startedTime; //Calculate necessary time

                        if (requestTime > maxTime)
                            maxTime = (int)requestTime;
                        if (requestTime < minTime)
                            minTime = (int)requestTime;

                        lock (info)
                        {
                            info.AddRequest(requestTime, nr, result); //Add info of run
                        }
                        //Test that request was successful and below the necessary time
                        Assert.True(result.IsSuccessStatusCode);
                        Assert.True(requestTime < maxTimePerRequest);
                    }
                );
            }
            Task.WaitAll(tasks); //Wait for all to run to the end


            info.PrintResult(); //Print results
            TestOutput.WriteLine($"Minimum: {minTime}ms. Maximum: {maxTime}ms");
        }

    }

}


class ServerLoadInfo
{
    private readonly ITestOutputHelper TestOutput;
    public ServerLoadInfo(ITestOutputHelper output)
    {
        TestOutput = output;
    }

    private long MeanTime { get; set; } = 0;
    private int RunItems { get; set; } = 0;

    public void AddRequest(long time, int nr, HttpResponseMessage response)
    {
        MeanTime = (MeanTime * RunItems + time) / ++RunItems;
        string state = response.IsSuccessStatusCode ? "" : $"[ERROR {response.StatusCode}] -> {response.ReasonPhrase}";
        TestOutput.WriteLine($"[{nr}] Response in {time}ms" + state);
    }

    public void PrintResult()
    {
        TestOutput.WriteLine($"Mean Time to request {RunItems} Requests: {MeanTime}ms");
    }

    public class RunnerInfo
    {
        int runner = 0;
        public int GetRunnerId()
        {
            return ++runner;
        }
    }
}
