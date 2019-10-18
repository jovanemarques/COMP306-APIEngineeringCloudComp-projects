﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbLibs.DynamoDb
{
    public class PutItem: IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        public PutItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }
        public async Task AddNewEntry(int id, string replayDateTime)
        {
            var queryRequest = RequestBuilder(id, replayDateTime);

            await PutItemAsync(queryRequest);
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoClient.PutItemAsync(request);
        }

        private PutItemRequest RequestBuilder(int id, string replayDateTime)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue{N = id.ToString()} },
                {"ReplayDateTime", new AttributeValue{N = replayDateTime} }
            };

            return new PutItemRequest
            {
                TableName = "TempDynamoDbTable",
                Item = item
            };
        }
    }
}
