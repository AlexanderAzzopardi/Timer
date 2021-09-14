# Timer
A microservice which allows you to create timers which checks a value every set amount of time. It also allows you to check how many timers are running at one time. You are able to customise the timers intial delay time, intermidiate delay time and what conditions cause the timers to break. By removing one line of code you can also make the timers run indefinitly.


# Prerequisites
#### Installation of Docker 
![Docker](https://github.com/AlexanderAzzopardi/UnitConvertor/blob/main/Saved%20Pictures/DockerLogo.jfif)
> <https://docs.docker.com/engine/install/>

#### Installation of Dapr 
![Dapr](https://github.com/AlexanderAzzopardi/UnitConvertor/blob/main/Saved%20Pictures/DaprLogo.jfif)
> <https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/getting-started>

# Redis Store
When setting up a redis store you need to create a .yaml file 

    apiVersion: dapr.io/v1alpha1
    kind: Component
    metadata:
      name: statestore
      namespace: default
    spec:
      type: state.redis
      version: v1
      metadata:
      - name: redisHost
        value: localhost:6379
      - name: redisPassword
        value: ""
      - name: enableTLS
        value: true # Optional. Allowed: true, false.
      - name: failover
        value: true # Optional. Allowed: true, false.

# Running Timer microservice
To run the useraccount microservice you need to run it via dapr.

> dapr run --app-id "account-service" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" -- dotnet run --urls="http://+:5001"

## Creating a Timer
When creating a brand new timer you need to create a method in the *TimerController.cs* which creates the timer while calling the method *Function* each interation.

    [HttpGet("timer")]
    public void Timer(TimerSpec Timer, [FromServices] DaprClient client)
    {
        var autoEvent = new AutoResetEvent(false);
        AddTimer(client);
        var stateTimer = new Timer(StatusChecker.Function, autoEvent, Timer.timeDelay, Timer.timeInterval);
        autoEvent.WaitOne();
        stateTimer.Dispose();
        EndTimer(client);
    }
    
A method needs to be created in *StatusCheckerController.cs* which contains the code to be run every time the timer iterates.

    public static void Function(object o)
    {   
        AutoResetEvent autoEvent = (AutoResetEvent)o;
        
        Enter code here...

        autoEvent.Set();
    }
    
## Commands
### Creating an instance of a timer
To create an instance of a timer you need to run a http request, inside a *.http* file, like the one below. *"timeDelay"* is the delay from the timers creation before it starts (in ms). *"timeInterval"* is how often the timer ticks (in ms).
    
    GET http://localhost:5010/v1.0/invoke/timer/method/timer HTTP/1.1
    content-type: application/json

    {
        "timeDelay": "",
        "timeInterval": ""
    }

### Checking timers active
This command checks how many timers are currently running.
    
    GET http://localhost:5010/v1.0/invoke/timer/method/timerchecker HTTP/1.1
    
    
    
