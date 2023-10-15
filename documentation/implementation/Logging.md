# Logging

Unfortunately, logging is for the most developer a boring topic.
Even though it helps us to understand our application better and to find bugs faster.
A good centralized logging system empowers the team with insight and knowledge about the application.
For this reason, you should invest from the beginning in logging and decide what you want to log and how you want to log it.

## Logging with Serilog

Serilog is the de facto standard for logging in C#.
It has a lot of different addons and can be overwhelming at the beginning, until you found what you need.

I am using serilog configured in the settings.json file.
This is for me the most clear way of configuring serilog.

To use the settings.json file, you need to add the following nuget package:

```xml
<PackageReference Include="Serilog.Settings.Configuration" Version="3.1.0" />
```
unfortunately, it's sometimes hard to find the correct settings, for different addons.
Therefore have a look at the Appsettings from this project: [appsettings.json](../../src/Zimmj.Bootstrap/appsettings.json).

Pay attention that the write to settings are added to the different environments.
To enable different formatting for the different environments.
On local, we probably prefer a plain text format, while on the others, we want to have a json format.

## Centralized Logging system

Even if centralized a big topic, most of the teams and companies are not using it to it fullest potential.
A good logging strategy need time and investment, some of the basics I got from [this article](https://www.techtarget.com/searchcloudcomputing/tip/5-centralized-logging-best-practices-for-cloud-admins).
Some of the things, which I will talk about, I am learned over time, and found them to be useful to know.

### Log Format for centralized logging

As centralized logging goes hand in hand with analyzing the logs, it is important to change the log to an json format.
This enables to analyze the logs and map the values to the corresponding fields in the log system.
In the end it's not important, how the fields are called.
It's important that the fields are always called the same.
To allow the mapping to be accurate.

To change the log format to json, you can use the following settings for serilog:

```json
{
  "Name": "Console",
  "Args": {
    "formatter": {
      "type": "Serilog.Templates.ExpressionTemplate, Serilog.Expressions",
      "template": "{ {Timestamp: @t, Level: @l, Message: @m, Exception: @x, Properties: if IsDefined(@p[?]) then @p else undefined()} }\n"
    }
  }
}
```

This is the settings for serilog to log into the console with a change format.
It uses Expression to change the format into the custom json format.

An important field are the Properties.
As they allow us, to enhance the logs with different information as the correlation id.
This is made with following setting:

```json
{
  "Name": "WithCorrelationId",
  "Args": {
    "headerName": "traceparent"
  }
}
```

Don't forget to add the needed classes into the using part of serilog settings:

```json
{
  "Using": [
    "Serilog.Enrichers.ClientInfo",
    "Serilog.Sinks.Console"
  ]
}
```

### Log Level

Sometimes it feels like, that developer forgot, that there are different log levels.
I have seen a lot of code, where the developer just used the log level information.
Instead of changing the log level to the correct level of the purpose of the log.

For the environment of local and development, it is ok to use the log level debug or even verbose.
This are the environment with least traffic and security concerns.
We want to see what happened, when a bug was found.

Therefore I set the following level in the different environments:

```json
{
  "MinimumLevel": {
    "Default": "Verbose"
  }
}
```

With this, we can add debug logs for local and development environment.
And they will not be logged in the other environments.

Then the logs we write, while developing are helpful even after the development.
So please consider to use debug and verbose logs, when you are developing.

### Log Correlation

One of the most important thing, when you are using a microservice architecture, is to have a correlation id.
This id is used to track the request through the different services.
One of the most used correlation id is the [W3C Trace Context](https://www.w3.org/TR/trace-context/).
This should be added to any log logged for a request.
That's why I am using the enricher for this, which is logging the "traceparent" header, if it exist.

### Visualization and Metrics

One of the most forgotten part of logging is the visualization and metrics.
As we usually only log messages and information for the developer and don't think about the visualization of metrics.
One metric I like to log, is the time it took to process the request.

With serilog you can do the following, to get a nice json formatted log for the time it took to process the request:

```csharp
_logger.LogInformation("{@Event}", eventLog);

public class HttpEvent
{
    public string Path { get; init; }
    public string Method { get; init; }
    public int StatusCode { get; init; }
    public long Duration { get; init; }
}
```

I am using a simple class, with all the information I want for the event.
Then I am using the string pattern to log the event.
With the @ sign, I am telling serilog to serialize the object into json.
So the correct json is added to the message part, as well to the properties part.
Then we can use this json properties to build visualization and metrics.
This helps us to monitor our application and be able to react to troubles much earlier the the customer will feel it.