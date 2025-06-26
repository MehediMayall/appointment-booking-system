namespace SharedKernel;


public record Error(string Code, string Message)
{
    public static Error None = new(string.Empty, string.Empty);
    public static Error NullValue = new("Error.NullValue", "Null value was provided");
    public static Error New(string ErrorMessage, bool LogThisError = false) {
        if (LogThisError) { Log.Warning(ErrorMessage); }        
        return new("Error.NullValue", ErrorMessage);
    }
 
    public static Error New(string Code, string ErrorMessage, bool LogThisError = false) {
        if (LogThisError) { Log.Warning(ErrorMessage); }
        return new(Code, ErrorMessage);
    }

    public static Error New(int Code, string ErrorMessage) => 
        new(Code.ToString(), ErrorMessage);

}