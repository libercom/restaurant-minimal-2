using Newtonsoft.Json;
using ProjectB.Models;
using System.Collections.Concurrent;
using System.Text;

namespace ProjectB.Services
{
    public class BaseService
    {
        private readonly ConcurrentQueue<Client> queue;
        private readonly string _apiEndpoint;
        private readonly ILogger<BaseService> _logger;

        public BaseService(IConfiguration configuration, ILogger<BaseService> logger)
        {
            queue = new ConcurrentQueue<Client>();
            _apiEndpoint = configuration.GetSection("ApiEndpoint").Value;
            _logger = logger;

            Start();
        }

        public async Task Start()
        {
            await Task.Delay(1000);

            for (int i = 0; i < 2; i++)
            {
                Task.Run(Send);
            }
        }

        public void Push(Client value)
        {
            queue.Enqueue(value);
        }

        private async Task Send()
        {
            while (true)
            {
                if (!queue.IsEmpty)
                {
                    await Task.Delay(3000);

                    queue.TryDequeue(out var value);

                    var httpClient = new HttpClient();
                    var serializedValue = JsonConvert.SerializeObject(value);

                    await httpClient.PostAsync(_apiEndpoint, new StringContent(serializedValue, Encoding.UTF8, "application/json"));
                }
            }
        }
    }
}
