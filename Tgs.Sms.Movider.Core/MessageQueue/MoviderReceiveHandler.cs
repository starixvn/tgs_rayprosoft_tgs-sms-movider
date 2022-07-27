using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tgs.MessageQueue;
using Tgs.Sms.Protos;

namespace Tgs.Sms.Movider.MessageQueue
{
    public class MoviderReceiveHandler : IMessageHandler
    {
        private readonly ITgsRepository _tgsRepository;
        private readonly ILogger<MoviderReceiveHandler> _logger;
        private readonly MoviderClient _moviderClient;

        public MoviderReceiveHandler(
            ITgsRepository tgsRepository,
            ILogger<MoviderReceiveHandler> logger,
            MoviderClient moviderClient)
        {
            _tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _moviderClient = moviderClient ?? throw new ArgumentNullException(nameof(moviderClient));
        }

        public async ValueTask HandleAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            var shortMsg = ShortMessage.Parser.ParseFrom(data);
            var brandId = shortMsg.BrandId.ToGuid();

            _logger.LogInformation(
                "Receive sms: [{nameof(shortMsg.BrandId)}] {brandId} " +
                "[{nameof(shortMsg.Destination)}] {shortMsg.Destination} " +
                "[{nameof(shortMsg.Content)}] {shortMsg.Content}",
                nameof(shortMsg.BrandId),
                brandId,
                nameof(shortMsg.Destination),
                shortMsg.Destination,
                nameof(shortMsg.Content),
                shortMsg.Content);

            var brandInfo = await _tgsRepository
                .GetBrandInfoByIdAsync(brandId)
                .ConfigureAwait(false);

            if (brandInfo == null)
            {
                _logger.LogInformation($"Can't find brand info ({brandId})");

                return;
            }

            if (!brandInfo.SmsEnabled || brandInfo.SmsVendorSettings == null)
            {
                _logger.LogWarning("Brand setting warn");

                return;
            }

            var request = new SendSMSRequest
            {
                ApiKey = brandInfo.SmsVendorSettings.ApiKey,
                ApiSecret = brandInfo.SmsVendorSettings.ApiSecret,
                To = shortMsg.Destination,
                Text = shortMsg.Content
            };

            var response = await _moviderClient.SendSMSAsync(request).ConfigureAwait(false);

            if (response is null)
                throw new Exception("Sms send fail");

            _logger.LogInformation("Sms sent.");
        }
    }
}