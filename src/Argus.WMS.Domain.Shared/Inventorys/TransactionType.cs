using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.WMS.Inventorys
{
    public enum TransactionType
    {
        Receipt = 0,
        Issue = 1,
        Transfer = 2,
        Adjust = 3
    }
}
