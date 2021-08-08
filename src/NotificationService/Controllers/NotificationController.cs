using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpPost]
        [Route("fire-and-forget")]
        public IActionResult FireAndForget(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"{client}, thank you for contacting us."));

            return Ok($"Job Id: {jobId}");
        }

        [HttpPost]
        [Route("delayed")]
        public IActionResult Delayed(string client)
        {
            string jobId = BackgroundJob.Schedule(() =>
                Console.WriteLine($"Session for client {client} has been closed."), TimeSpan.FromSeconds(60));

            return Ok($"Job Id: {jobId}");
        }

        [HttpPost]
        [Route("recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("С ДР! :)"), Cron.Daily);

            return Ok();
        }

        [HttpPost]
        [Route("continuations")]
        public IActionResult Continuations(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"Check balance logic for {client}"));

            BackgroundJob.ContinueJobWith(jobId, () =>
                Console.WriteLine($"{client}, your balance has been changed."));

            return Ok();
        }
    }
}
