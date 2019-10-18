using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbLibs.DynamoDb
{
    public class UpdateItem: IUpdateItem
    {
        private IGetItem _getItem;

        private static readonly string tableName = "TempDynamoDbTable";
        private readonly IAmazonDynamoDB _dynamoClient;
        public UpdateItem(IGetItem getItem, IAmazonDynamoDB dynamoClient)
        {
            _getItem = getItem;
            _dynamoClient = dynamoClient;
        }
        public async Task<Item> Update(int id, double price)
        {
            var response = await _getItem.GetItems(id);

            var currentPrice = response.Items.Select(p => p.Price).FirstOrDefault();

            var replyDateTime = response.Items.Select(p => p.ReplayDateTime).FirstOrDefault();

            var request = RequestBuilder(id, price, currentPrice, replyDateTime);

            var result = await UpdateItemAsync(request);

            return new Item
            {
                Id = Convert.ToInt32(result.Attributes["Id"].N),
                ReplayDateTime = result.Attributes["ReplayDateTime"].N,
                Price = Convert.ToDouble(result.Attributes["Price"].N)
            };
        }

        private UpdateItemRequest RequestBuilder(int id, double price, double currentPrice, string replyDateTime)
        {
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>{
                    { "Id", new AttributeValue
                    {
                        N = id.ToString()
                    } },
                    { "ReplayDateTime", new AttributeValue
                    {
                        N = replyDateTime
                    } }
                },
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#P", "Price" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":newprice", new AttributeValue
                    {
                        N = price.ToString()
                    } },
                    {":currprice", new AttributeValue
                    {
                        N = currentPrice.ToString()
                    } }
                },
                UpdateExpression = "SET #P = :newprice",
                ConditionExpression = "#P = :currprice",

                TableName = tableName,
                ReturnValues = "ALL_NEW"
            };
            return request;
        }

        private async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request)
        {
            var response = await _dynamoClient.UpdateItemAsync(request);
            return response;
        }
    }
}
