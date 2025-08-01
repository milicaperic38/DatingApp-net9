using System.Runtime.CompilerServices;

namespace API;

//treba nam neki medjusloj koji od svega kreira json format exceptiona, i obavestava usera
public class ApiException(int statusCode, string message, string? details)
{
    public int StatusCodee { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public string? Details { get; set; } = details; //upitnik i gore i ovde znaci da je optional
}