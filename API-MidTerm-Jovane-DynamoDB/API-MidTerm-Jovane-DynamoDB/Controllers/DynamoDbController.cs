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
        private readonly IUpdateItem _updateItem;
        public DynamoDbController(IDynamoDbExamples dynamoDbExamples, IPutItem putItem, IGetItem getItem, IUpdateItem updateItem)
        {
            _dynamoDbExamples = dynamoDbExamples;
            _putItem = putItem;
            _getItem = getItem;
            _updateItem = updateItem;
        }
        [Route("createtable")]
        public IActionResult CreateDynamoDbTable()
        {
            _dynamoDbExamples.CreateDynamoDbTable();
            return Ok();
        }
        [Route("putitems")]
        public IActionResult PutItem([FromQuery] int Id, string replayDateTime, double price)
        {
            _putItem.AddNewEntry(Id, replayDateTime, price);
            return Ok();
        }
        [Route("getitems")]
        public async Task<IActionResult> GetItems([FromQuery] int? id)
        {
            var response = await _getItem.GetItems(id);

            return Ok(response);
        }
        [HttpPut]
        [Route("updateitem")]
        public async Task<IActionResult> UpdateItem([FromQuery] int id, double price)
        {
            var response = await _updateItem.Update(id, price);
            return Ok(response);
        }
    }
}