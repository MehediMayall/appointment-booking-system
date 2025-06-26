namespace SharedKernel;

public sealed class RedisCacheErrors : ExceptionBase<RedisSettings> {

    public static Error EmptyContent(string Key) => new("Empty Content", $"Couldn't find any cache content using key {Key}. Please try again.");
}
