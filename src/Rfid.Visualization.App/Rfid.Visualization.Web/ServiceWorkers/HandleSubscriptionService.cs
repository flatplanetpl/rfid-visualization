using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rfid.Visualization.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rfid.Visualization.Web.ServiceWorkers
{
    public class HandleSubscriptionService : BackgroundService
    {
        private readonly ILogger<HandleSubscriptionService> _logger;
        private readonly IHubContext<NotifyStickerHub, IClinetNotifySticker> _notifyStickerHub;

        public HandleSubscriptionService(ILogger<HandleSubscriptionService> logger,
            IHubContext<NotifyStickerHub, IClinetNotifySticker> notifyStickerHub)
        {
            _logger = logger;
            _notifyStickerHub = notifyStickerHub;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken);
                await _notifyStickerHub.Clients.All.ClinetNotifySticker(DateTime.Now.ToString());

            }

        }
    }
}
