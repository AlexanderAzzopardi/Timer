using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using StatusCheckerController;
using TimerSpecModel;
using System.Collections.Generic;

namespace sample.microservice.useraccount.Controllers
{
    [ApiController]
    public class TimerController : ControllerBase
    { 
        public static async void AddTimer(DaprClient client)
        {
            Console.WriteLine("{0} Creating timer.\n", DateTime.Now.ToString("h:mm:ss.fff"));
            int timersOnline = await client.GetStateAsync<int>("statestore", "timersOnline");
            await client.SaveStateAsync("statestore", "timersOnline", timersOnline += 1);
        }
        public static async void EndTimer(DaprClient client)
        {
            int timersOnline = await client.GetStateAsync<int>("statestore", "timersOnline");
            await client.SaveStateAsync("statestore", "timersOnline", timersOnline -= 1);
        }

        [HttpGet("timerchecker")]
        public async void TimerCheck([FromServices] DaprClient client)
        {
            int timersOnline = await client.GetStateAsync<int>("statestore", "timersOnline");
            Console.WriteLine("Timers currently running: {0}\n", timersOnline);
        }
        
        //Below is the current timers that have been created.
        [HttpGet("temperaturetimer")]
        public void TemperatureTimer(TimerSpec Timer, [FromServices] DaprClient client)
        {
            var autoEvent = new AutoResetEvent(false);
            AddTimer(client);
            var stateTimer = new Timer(StatusChecker.CheckTempStatus, autoEvent, Timer.timeDelay, Timer.timeInterval);
            autoEvent.WaitOne();
            stateTimer.Dispose();
            EndTimer(client);
        }

        [HttpGet("heighttimer")]
        public void HeightTimer(TimerSpec Timer, [FromServices] DaprClient client)
        {
            var autoEvent = new AutoResetEvent(false);
            AddTimer(client);
            var stateTimer = new Timer(StatusChecker.CheckHeightStatus, autoEvent, Timer.timeDelay, Timer.timeInterval);
            autoEvent.WaitOne();
            stateTimer.Dispose();
            EndTimer(client);
        }
    }
}
