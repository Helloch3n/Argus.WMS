using Argus.WMS.BillNumbers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Argus.WMS.Outbound
{
    public class OutboundOrderManager : DomainService
    {
        private readonly IRepository<OutboundOrder, Guid> _outboundOrderRepository;
        private readonly IBillNumberGenerator _billNumberGenerator;

        public OutboundOrderManager(
            IRepository<OutboundOrder, Guid> outboundOrderRepository,
            IBillNumberGenerator billNumberGenerator)
        {
            _outboundOrderRepository = outboundOrderRepository;
            _billNumberGenerator = billNumberGenerator;
        }

        public async Task<OutboundOrder> CreateOrderAsync(
            string? sourceOrderNo,
            string? customerName,
            List<(string ProductCode, decimal TargetLength, int Quantity)> items)
        {
            var orderNo = await _billNumberGenerator.GetNextNumberAsync("Out");
            var orderId = GuidGenerator.Create();

            var order = new OutboundOrder(orderId, orderNo, sourceOrderNo, customerName);

            foreach (var item in items)
            {
                order.AddItem(GuidGenerator.Create(), item.ProductCode, item.TargetLength, item.Quantity);
            }

            await _outboundOrderRepository.InsertAsync(order, autoSave: true);

            return order;
        }
    }
}