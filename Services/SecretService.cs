namespace SLocker.Services;

public class SecretService
{
    public string? Secret { get; set; }

    public bool HasSecret() => Secret is not null;
}