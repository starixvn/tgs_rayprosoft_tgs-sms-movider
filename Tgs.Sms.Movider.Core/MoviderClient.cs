using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tgs.Sms.Movider.Core;
using Tgs.Sms.Movider.Models;

namespace Tgs.Sms.Movider
{
    public class MoviderClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MoviderClient> _logger;
        private readonly MoviderSerializer _moviderSerializer;

        public MoviderClient(
            HttpClient httpClient,
            ILogger<MoviderClient> logger,
            MoviderSerializer moviderSerializer)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _moviderSerializer = moviderSerializer ?? throw new ArgumentNullException(nameof(moviderSerializer));
        }

        public async ValueTask<SendSMSResponse> SendSMSAsync(SendSMSRequest request)
        {
            var parameters = new Dictionary<string, string>
            {
                ["api_key"] = request.ApiKey,
                ["api_secret"] = request.ApiSecret,
                ["to"] = request.To,
                ["text"] = request.Text
            };
            HttpContent content = new FormUrlEncodedContent(parameters!);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
            {
                CharSet = "UTF-8"
            };

            var httpResponseMessage = await _httpClient
                .PostAsync("/v1/sms", content)
                .ConfigureAwait(false);

            var responseMessage = default(string);

            using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync().Result))
            {
                responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false) ?? string.Empty;
            }

            _logger.LogInformation(responseMessage);

            _ = httpResponseMessage.EnsureSuccessStatusCode();

            var apiResponseData = _moviderSerializer.Deserialize<SendSMSResponse>(responseMessage);

            return apiResponseData;
        }
    }
}