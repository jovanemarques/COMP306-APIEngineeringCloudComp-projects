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
        public DynamoDbController(IDynamoDbExamples dynamoDbExamples)
        {
            _dynamoDbExamples = dynamoDbExamples;
        }
        [Route("createtable")]
        public IActionResult CreateDynamoDbTable()
        {
            _dynamoDbExamples.CreateDynamoDbTable();
            return Ok();
        }
    }
}