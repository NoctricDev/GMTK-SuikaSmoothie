namespace CSharpTools.HTTP.Architecture
{
    public class HttpResponse
    {
        public long StatusCode { get; }
        public bool IsStatusCodeSuccess => StatusCode is >= 200 and < 300;
        public string? ReasonPhrase { get; }
        public byte[]? Bytes { get; }
        public string? Content { get; }
    
        public HttpResponse(byte[]? data, long? statusCode = null, string? reasonPhrase = null)
        {
            Bytes = data;

            if (statusCode.HasValue)
                StatusCode = statusCode.Value;
            
            ReasonPhrase = reasonPhrase;
        }

        public HttpResponse(string? content, long? statusCode = null, string? reasonPhrase = null)
        {
            Content = content;
            
            if (statusCode.HasValue)
                StatusCode = statusCode.Value;
            
            ReasonPhrase = reasonPhrase;
        }

        public HttpResponse(byte[]? data, string? content, long? statusCode = null, string? reasonPhrase = null)
        {
            Bytes = data;
            Content = content;

            if (statusCode.HasValue)
                StatusCode = statusCode.Value;
            
            ReasonPhrase = reasonPhrase;
        }
    }
}