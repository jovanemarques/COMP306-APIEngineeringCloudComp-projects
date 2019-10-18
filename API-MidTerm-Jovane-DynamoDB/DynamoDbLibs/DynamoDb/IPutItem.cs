using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbLibs.DynamoDb
{
    public interface IPutItem
    {
        Task AddNewEntry(int id, string replayDateTime);
    }
}
