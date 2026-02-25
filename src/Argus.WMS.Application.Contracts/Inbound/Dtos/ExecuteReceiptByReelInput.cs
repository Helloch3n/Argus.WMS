using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.WMS.Application.Contracts.Inbound.Dtos
{
    public class ExecuteReceiptByReelInput
    {
       public Guid ReceiptId { get; set; }
       public Guid ReelId { get; set; }
       public Guid LocationId { get; set; }
    }
}
