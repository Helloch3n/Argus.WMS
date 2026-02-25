using Argus.WMS.Application.Contracts.Inbound.Dtos;
using Argus.WMS.Inbound;
using Argus.WMS.MasterData.Reels;
using Argus.WMS.MasterData.Reels.Dtos;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Mapperly;

namespace Argus.WMS.Mappers.Receipt
{
    [Mapper]
    public partial class ExecuteReceiptMapper : MapperBase<ExecuteReceiptInput, ReceiveInventoryArgs>
    {
        public override partial ReceiveInventoryArgs Map(ExecuteReceiptInput source);

        public override partial void Map(ExecuteReceiptInput source, ReceiveInventoryArgs destination);

        public partial void Map(ExecuteReceiptByReelInput source, ReceiveInventoryArgs destination);

    }
}
