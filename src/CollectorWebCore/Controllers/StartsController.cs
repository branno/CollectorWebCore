using System;
using CollectorWebCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollectorWebCore.Controllers
{
    [Route("api/[controller]")]
    public class StartsController : Controller
    {
        // POST api/starts
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var s = Singleton.Instance;
            if (s._dateStarted == null)
            {
                s._dateStarted = DateTime.Now;
            }
            s.AddStart(new Start());
        }
    }
}