using System.Net;

namespace Platform.Client.Http;

public interface IHttpClientObserver
{
    void OnRequestStart(string operationName);
    
    void OnRequestSuccess(string operationName, HttpStatusCode httpResponseStatusCode);
    
    void OnRequestError(string operationName, HttpRequestException httpRequestException);
    
    void OnUnexpectedError(string operationName, Exception exception);
    
    void OnRequestEnd(string operationName, TimeSpan swElapsed);
}