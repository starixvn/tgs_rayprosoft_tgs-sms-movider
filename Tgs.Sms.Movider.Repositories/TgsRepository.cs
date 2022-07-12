using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgs.Sms.Movider.Core;
using Tgs.Sms.Movider.Repositories.Entities;

namespace Tgs.Sms.Movider.Repositories
{
    internal class TgsRepository : ITgsRepository
    {
        private readonly TgsContext _tgsContext;
        private readonly MoviderSerializer _moviderSerializer;

        public TgsRepository(TgsContext tgsContext, MoviderSerializer moviderSerializer)
        {
            _tgsContext = tgsContext ?? throw new ArgumentNullException(nameof(tgsContext));
            _moviderSerializer = moviderSerializer ?? throw new ArgumentNullException(nameof(moviderSerializer));
        }

        public async ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId)
        {
            var brand = await _tgsContext.Brands
                .FindAsync(brandId)
                .ConfigureAwait(false);

            if (brand == null)
                return null;

            return new BrandInfo(
                brand.Id,
                brand.Code,
                brand.EnableSms,
                brand.SmsVendorCode,
                string.IsNullOrWhiteSpace(brand.SmsVendorSettings)
                    ? null
                    : _moviderSerializer.Deserialize<MoviderSettings>(brand.SmsVendorSettings));
        }
    }
}
