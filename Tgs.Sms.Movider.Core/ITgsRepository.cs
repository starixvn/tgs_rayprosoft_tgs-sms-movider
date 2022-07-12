using System;
using System.Threading.Tasks;

namespace Tgs.Sms.Movider
{
    public interface ITgsRepository
    {
        public ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId);
    }
}