using Azure.Storage.Queues;
using System.Text.Json;

namespace AzureStorageQueue
{
    public class WeatherDataBackgroundTask : BackgroundService
    {
        private readonly ILogger<WeatherDataBackgroundTask> _logger;
        private readonly QueueClient _queueClient;

        public WeatherDataBackgroundTask(ILogger<WeatherDataBackgroundTask> logger, QueueClient queueClient)
        {
            _logger = logger;
            _queueClient = queueClient;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Reading from queue");
                var queueMessage = await _queueClient.ReceiveMessageAsync();

                if (queueMessage.Value != null)
                {
                    var weatherData = JsonSerializer.Deserialize<WeatherForecast>(queueMessage.Value.MessageText);
                    _logger.LogInformation("New Mesasge Read: {weatherData}", weatherData.Summary);

                    //delete the message from the queue
                    await _queueClient.DeleteMessageAsync(queueMessage.Value.MessageId, queueMessage.Value.PopReceipt);
                }

                //Let's wait for 10 seconds to pick the next record from the queue
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
