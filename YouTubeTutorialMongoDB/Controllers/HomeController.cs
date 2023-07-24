using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using YouTubeTutorialMongoDB.Models;

namespace YouTubeTutorialMongoDB.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMongoCollection<Users> _usr;
        public HomeController()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("YouTubeTutorial");
            _usr = database.GetCollection<Users>("table1");
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var data = _usr.Find<Users>(x => x.firstname != null).ToList();
            return Ok(data);
        }

        [HttpPost("InsertUser")]
        public IActionResult InsertUser(Users model)
        {
            model.UserId = _usr.AsQueryable().Any() ? _usr.AsQueryable().OrderByDescending(x => x.UserId).FirstOrDefault().UserId + 1 : 1;

            _usr.InsertOne(model);
            return Ok(model);
        }

        [HttpPost("InsertManyUser")]
        public IActionResult InsertManyUser(List<Users> model)
        {
            _usr.InsertMany(model);
            return Ok(model);
        }

        [HttpPost("UpdateUser")]
        public IActionResult UpdateUser(Users model)
        {
            //UpdateOne
            //UpdateMany
            //Replace

            //fields what we have to get
            var fields = Builders<Users>.Projection.Include(x => x.UserId);

            //filter for data
            List<FilterDefinition<Users>> filters = new List<FilterDefinition<Users>>();
            filters.Add(Builders<Users>.Filter.Eq(x => x.UserId, model.UserId));

            //set condition
            var condition = Builders<Users>.Filter.And(filters);

            //select existing data
            var ExistingData = _usr.Find(condition).Project<Users>(fields);

            if(ExistingData != null)
            {
                //set data (values) that we need to update
                var updateDeination = new List<UpdateDefinition<Users>>();
                updateDeination.Add(Builders<Users>.Update.Set(x=>x.firstname, model.firstname));
                updateDeination.Add(Builders<Users>.Update.Set(x => x.Address.State, model.Address.State));

                var combineUpdate = Builders<Users>.Update.Combine(updateDeination);
                _usr.UpdateOne(condition, combineUpdate);
            }

            return Ok(model);
        }
    }
}
