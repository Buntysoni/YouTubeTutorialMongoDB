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
    }
}
