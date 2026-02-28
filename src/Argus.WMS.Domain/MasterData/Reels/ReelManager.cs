using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.MasterData.Reels
{
    public class ReelManager : DomainService
    {
        private readonly IRepository<Reel, Guid> _reelRepository;

        public ReelManager(IRepository<Reel, Guid> reelRepository)
        {
            _reelRepository = reelRepository;
        }

        public async Task<Reel> CreateAsync(
            Guid id,
            string? reelNo,
            string name,
            string size,
            decimal selfWeight,
            Guid? currentLocationId)
        {
            var finalReelNo = string.IsNullOrWhiteSpace(reelNo)
                ? await GenerateReelNoAsync()
                : reelNo;

            await EnsureReelNoUniqueAsync(finalReelNo);

            return new Reel(
                id,
                finalReelNo,
                name,
                size,
                selfWeight,
                ReelStatus.Empty,
                currentLocationId);
        }

        public async Task UpdateAsync(
            Reel reel,
            string reelNo,
            string name,
            string size,
            decimal selfWeight)
        {
            if (!string.Equals(reel.ReelNo, reelNo, StringComparison.Ordinal))
            {
                await EnsureReelNoUniqueAsync(reelNo, reel.Id);
            }

            reel.Update(reelNo, name, size, selfWeight);
        }

        public async Task EnsureReelNoUniqueAsync(string reelNo, Guid? excludeReelId = null)
        {
            var exists = await _reelRepository.AnyAsync(x =>
                x.ReelNo == reelNo &&
                (!excludeReelId.HasValue || x.Id != excludeReelId.Value));

            if (exists)
            {
                throw new BusinessException("Reel:ReelNoAlreadyExists")
                    .WithData("ReelNo", reelNo);
            }
        }

        public async Task<string> GenerateReelNoAsync()
        {
            var date = Clock.Now.ToString("yyyyMMdd");
            for (var i = 0; i < 100; i++)
            {
                var reelNo = $"R-{date}-{Random.Shared.Next(1, 1000):D3}";
                var exists = await _reelRepository.AnyAsync(x => x.ReelNo == reelNo);

                if (!exists)
                {
                    return reelNo;
                }
            }

            throw new BusinessException("Reel:GenerateReelNoFailed");
        }
    }
}
