using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamoDbLibs.DynamoDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_MidTerm_Jovane_DynamoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamoDbController : ControllerBase
    {
        private readonly IDynamoDbExamples _dynamoDbExamples;
        private readonly IPutItem _putItem;
        private readonly IGetItem _getItem;
        public DynamoDbController(IDynamoDbExamples dynamoDbExamples, IPutItem putItem, IGetItem getItem)
        {
            _dynamoDbExamples = dynamoDbExamples;
            _putItem = putItem;
            _getItem = getItem;
        }
        [Route("createtable")]
        public IActionResult CreateDynamoDbTable()
        {
            _dynamoDbExamples.CreateDynamoDbTable();
            return Ok();
        }
        [Route("putitems")]
        public IActionResult PutItem([FromQuery] int Id, string replayDateTime)
        {
            _putItem.AddNewEntry(Id, replayDateTime);
            return Ok();
        }
        [Route("getitems")]
        public async Task<IActionResult> GetItems([FromQuery] int? id)
        {
            var response = await _getItem.GetItems(id);

            return Ok(response);
        }
    }
}