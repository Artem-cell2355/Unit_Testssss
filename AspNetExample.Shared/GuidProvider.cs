namespace AspNetExample.Service;

public class GuidProvider : IGuidProvider
{
    public Guid GetGuid() => Guid.CreateVersion7();
}