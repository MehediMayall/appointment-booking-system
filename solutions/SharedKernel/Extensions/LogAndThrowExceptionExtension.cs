namespace SharedKernel;

public static class LogAndThrowException
{
    public static void This(string Message) => LogAndThrowIt(Message);
    

    public static void IfNull<T>(T? Value, string Message) where T : class{
        if (Value != null) return;
        LogAndThrowIt(Message);
    }

    public static void IfNullOrEmpty(string? Value, string Message){
        if (!string.IsNullOrEmpty(Value)) return;
        LogAndThrowIt(Message);
    }

    private static void LogAndThrowIt(string Message){
        Log.Fatal(Message);
        throw new ArgumentNullException(Message);
    }
}