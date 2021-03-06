﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Commands
{
    public interface ICommandBus
    {
        /// <summary>
        ///  注册订阅事件处理器
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="handler"></param>
        void RegisterHandler<TCommand>(Type commandType, ICommandHandler<TCommand> handler) where TCommand : ICommand;

        /// <summary>
        /// 取消注册订阅事件处理器
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="commandType"></param>
        void UnRegister<TCommand>(Type commandType) where TCommand : class, ICommand;

        /// <summary>
        /// Send command to commandbus
        /// </summary>
        /// <param name="processCommand"></param>
        void Send(ICommand processCommand);


        /// <summary>
        /// Dispatch the command
        /// </summary>
        /// <param name="processCommand"></param>
        void Dispatch(ICommand processCommand);

        int GetLength();

    }
}
