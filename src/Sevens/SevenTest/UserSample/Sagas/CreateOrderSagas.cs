using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Commands;
using Seven.Events;
using SevenTest.UserSample.ApplicationEvents;
using SevenTest.UserSample.Commands;
using SevenTest.UserSample.Dmains;

namespace SevenTest.UserSample.ProcessManager
{
    /// <summary>
    /// 创建订单流程
    /// </summary>
    public class CreateOrderSagas : ICommandHandler<CreateOrderCommand>,
            ICommandHandler<CreatedOrderCancelCommand>,
            ICommandHandler<ReduceInventoryCommand>,
            ICommandHandler<CancelReduceInventoryCommand>,
            IEventHandler<OrderCreated>,
            IEventHandler<CreateOrderCanceled>,
            IEventHandler<InvertoryOutCheckoutFailed>
    {
        private readonly ICommandService _commandService;

        public CreateOrderSagas(ICommandService commandService)
        {
            _commandService = commandService;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="commandContext"></param>
        /// <param name="command"></param>
        public void Handle(ICommandContext commandContext, CreateOrderCommand command)
        {
            commandContext.Add(new OrderAggregateRoot(Guid.NewGuid().ToString()));
        }

        /// <summary>
        /// 创建订单成功后，修改商品的库存命令
        /// </summary>
        /// <param name="evnt"></param>
        public void Handle(OrderCreated evnt)
        {
            _commandService.Send(new ReduceInventoryCommand());
        }

        /// <summary>
        /// 修改商品的库存
        /// </summary>
        /// <param name="commandContext"></param>
        /// <param name="command"></param>
        public void Handle(ICommandContext commandContext, ReduceInventoryCommand command)
        {
            commandContext.Get<ProductAggregateRoot>(command.ProductId).ReduceInventory(command.Quantity);
        }

        /// <summary>
        /// 商品库存不足事件处理，发出销毁订单的命令
        /// </summary>
        /// <param name="evnt"></param>
        public void Handle(InvertoryOutCheckoutFailed evnt)
        {
            _commandService.Send(new CancelReduceInventoryCommand());
        }

        /// <summary>
        /// 销毁订单
        /// </summary>
        /// <param name="commandContext"></param>
        /// <param name="command"></param>
        public void Handle(ICommandContext commandContext, CreatedOrderCancelCommand command)
        {
            commandContext.Get<OrderAggregateRoot>(command.AggregateRootId).CreateOrderCancel();
        }

        /// <summary>
        /// 订单销毁成功后，补偿商品库存
        /// </summary>
        /// <param name="evnt"></param>
        public void Handle(CreateOrderCanceled evnt)
        {
            _commandService.Send(new CancelReduceInventoryCommand());
        }

        /// <summary>
        /// 商品补偿
        /// </summary>
        /// <param name="commandContext"></param>
        /// <param name="command"></param>
        public void Handle(ICommandContext commandContext, CancelReduceInventoryCommand command)
        {
            commandContext.Get<ProductAggregateRoot>(command.AggregateRootId).ReduceInventory(command.Quantity);
        }
    }
}
