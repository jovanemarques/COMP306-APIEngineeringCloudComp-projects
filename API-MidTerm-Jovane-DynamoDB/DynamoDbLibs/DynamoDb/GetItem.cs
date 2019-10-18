﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbLibs.DynamoDb
{
    public class GetItem : IGetItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        public GetItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }
        public async Task<DynamoTableItems> GetItems(int? id)
        {
            var queryRequest = RequestBuilder(id);

            var result = await ScanAsync(queryRequest);

            return new DynamoTableItems
            {
                Items = result.Items.Select(Map).ToList()
            };
        }

        private Item Map(Dictionary<string, AttributeValue> result)
        {
            return new Item
            {
                Id = Convert.ToInt32(result["Id"].N),
                ReplayDateTime = result["ReplayDateTime"].N,
                Price = result.Keys.Contains("Price") ? Convert.ToDouble(result["Price"].N) : 0.0
            };
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await _dynamoClient.ScanAsync(request);
            return response;
        }

        private ScanRequest RequestBuilder(int? id)
        {
            if (!id.HasValue)
            {
                return new ScanRequest
                {
                    TableName = "TempDynamoDbTable"
                };
            }

            return new ScanRequest
            {
                TableName = "TempDynamoDbTable",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":v_Id", new AttributeValue{N = id.ToString()}
                    }
                },
                FilterExpression = "Id = :v_Id",
                ProjectionExpression = "Id, ReplayDateTime, Price"
            };
        }
    }
}
