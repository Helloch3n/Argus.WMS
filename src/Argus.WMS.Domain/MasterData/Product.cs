using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Argus.WMS.MasterData
{
    public class Product : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }

        public decimal Length { get; private set; }
        public decimal Width { get; private set; }
        public decimal Height { get; private set; }
        public decimal Volume => Length * Width * Height;
        public decimal Weight { get; private set; }

        public bool IsBatchManagementEnabled { get; private set; }
        public int? ShelfLifeDays { get; private set; }

        private Product() { }

        public Product(
            Guid id,
            string code,
            string name,
            string unit,
            decimal length,
            decimal width,
            decimal height,
            decimal weight,
            bool isBatchManagementEnabled,
            int? shelfLifeDays) : base(id)
        {
            SetCode(code);
            SetName(name);
            Unit = unit;
            UpdateDimensions(length, width, height, weight);
            SetBatchManagement(isBatchManagementEnabled, shelfLifeDays);
        }

        public void SetCode(string code)
        {
            Code = Check.NotNullOrWhiteSpace(code, nameof(code));
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public void UpdateDimensions(decimal length, decimal width, decimal height, decimal weight)
        {
            if (length <= 0 || width <= 0 || height <= 0)
            {
                throw new UserFriendlyException("长、宽、高必须大于0。");
            }

            if (weight <= 0)
            {
                throw new UserFriendlyException("重量必须大于0。");
            }

            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
        }

        public void SetBatchManagement(bool enabled, int? shelfLifeDays)
        {
            IsBatchManagementEnabled = enabled;
            ShelfLifeDays = enabled ? shelfLifeDays : null;
        }
    }
}
