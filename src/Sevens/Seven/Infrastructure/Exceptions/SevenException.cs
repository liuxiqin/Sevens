﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Infrastructure.Exceptions
{
    /// <summary>
    /// 系统内不可预期的错误
    /// </summary>
    public class SevenException : ApplicationException
    {
        public SevenException(string message)
            : base(message)
        {

        }
    }
}
