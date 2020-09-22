using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using Rfid.Visualization.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace Rfid.Visualization.Web.ServiceWorkers
{
    public class HandleSubscriptionService : BackgroundService
    {
        private readonly ILogger<HandleSubscriptionService> _logger;
        private readonly IHubContext<NotifyStickerHub, IClinetNotifySticker> _notifyStickerHub;
        private readonly IConfiguration _configuration;
        private IManagedMqttClient _mqttClient;

        public HandleSubscriptionService(ILogger<HandleSubscriptionService> logger,
            IHubContext<NotifyStickerHub, IClinetNotifySticker> notifyStickerHub,
            IConfiguration configuration)
        {
            _logger = logger;
            _notifyStickerHub = notifyStickerHub;
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                                            .WithTcpServer(_configuration["MqttServerUrl"])
                                            .Build())
                    
                    .Build();

            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("testtopic").Build());
            _mqttClient.ApplicationMessageReceivedHandler= new MqttApplicationMessageReceivedHandlerDelegate(ProcessEvent);
            await _mqttClient.StartAsync(options);

        }

        private void ProcessEvent(MqttApplicationMessageReceivedEventArgs obj)
        {
            var payload = Encoding.UTF8.GetString(obj.ApplicationMessage.Payload);
            _notifyStickerHub.Clients.All.ClinetNotifySticker(payload);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

        }
    }
}
