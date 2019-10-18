using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DynamoDbLibs.DynamoDb
{
    public class DynamoDbExamples : IDynamoDbExamples
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private static readonly string tableName = "TempDynamoDbTable";

        public DynamoDbExamples(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public void CreateDynamoDbTable()
        {
            try
            {
                CreateTempTable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void CreateTempTable()
        {
            Console.WriteLine("Creating Table");

            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "ReplayDateTime",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH" // Partition Key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "ReplayDateTime",
                        KeyType = "Range" // Sort Key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = tableName
            };
            var response = _dynamoDbClient.CreateTableAsync(request);

            WaitUntilTableReady(tableName);
        }

        private void WaitUntilTableReady(string tableName)
        {
            string status = null;
            do
            {
                Thread.Sleep(5000);
                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            } while (status != "ACTIVE");
            {
                Console.WriteLine("Table Created Successfully");
            }
        }
    }
}
