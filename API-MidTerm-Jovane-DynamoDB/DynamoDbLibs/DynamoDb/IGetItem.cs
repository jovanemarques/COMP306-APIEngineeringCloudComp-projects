﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbLibs.DynamoDb
{
    public interface IGetItem
    {
        Task<DynamoTableItems> GetItems(int? id);
    }
}
