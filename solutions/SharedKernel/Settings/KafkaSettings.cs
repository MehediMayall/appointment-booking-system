namespace SharedKernel;

public sealed class KafkaSettings
{
    public string BootstrapServers { get; init; }

    public string GroupId { get; init; }
}