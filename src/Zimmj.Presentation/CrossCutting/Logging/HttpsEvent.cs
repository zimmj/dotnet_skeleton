namespace Zimmj.Presentation.CrossCutting.Logging;

public class HttpEvent
{
    public string Path { get; init; }
    public string Method { get; init; }
    public int StatusCode { get; init; }
    public long Duration { get; init; }
}

public class HttpEventBuilder
{
    private readonly string _path;
    private readonly string _method;
    private int _statusCode;
    private long _duration;
    
    public HttpEventBuilder(HttpRequest request)
    {
        _path = request.Path;
        _method = request.Method;
    }
    
    public HttpEvent Ok(long milliseconds)
    {
        _statusCode = StatusCodes.Status200OK;
        _duration = milliseconds;
        return this.Build();
    }
    
    public HttpEvent Created(long milliseconds)
    {
        _statusCode = StatusCodes.Status201Created;
        _duration = milliseconds;
        return this.Build();
    }
    
    public HttpEvent Accepted(long milliseconds)
    {
        _statusCode = StatusCodes.Status202Accepted;
        _duration = milliseconds;
        return this.Build();
    }
    
    public HttpEvent NotFound(long milliseconds)
    {
        _statusCode = StatusCodes.Status404NotFound;
        _duration = milliseconds;
        return this.Build();
    }
    
    public HttpEvent BadRequest(long milliseconds)
    {
        _statusCode = StatusCodes.Status400BadRequest;
        _duration = milliseconds;
        return this.Build();
    }
    
    public HttpEvent InternalServerError(long milliseconds)
    {
        _statusCode = StatusCodes.Status500InternalServerError;
        _duration = milliseconds;
        return this.Build();
    }

    public HttpEventBuilder WithStatusCode(int statusCode)
    {
        _statusCode = statusCode;
        return this;
    }
    
    public HttpEventBuilder WithDuration(long duration)
    {
        _duration = duration;
        return this;
    }
    
    public HttpEvent Build()
    {
        return new HttpEvent
        {
            Path = _path,
            Method = _method,
            StatusCode = _statusCode,
            Duration = _duration
        };
    }
}