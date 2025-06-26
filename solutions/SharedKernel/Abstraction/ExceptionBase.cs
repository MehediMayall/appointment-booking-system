namespace SharedKernel;

public class ExceptionBase<T> where T : class {
    public static Error NotFound() => new("Not Found", $"Couldn't find data from {typeof(T)}. Please try again.");
    public static Error NotFound(Guid ID) => new("Not Found", $"Couldn't find {typeof(T)} for id {ID}. Please try again.");
    public static Error NotFound(object ID) => new("Not Found", $"Couldn't find {typeof(T)} using {nameof(ID)} {ID}. Please try again.");
    public static Error EmptyContent(string Key) => new("Empty Content", $"Couldn't find data using {Key}. Please try again.");
    public static Error EmptyArgument(string ArgumentName) => new("Empty Argument", $"Empty argument provided. {ArgumentName}. Please try again.");

}