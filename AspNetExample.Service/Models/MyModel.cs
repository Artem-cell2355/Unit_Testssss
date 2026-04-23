namespace AspNetExample.Service.Models;

public record MyModel(string Data, Guid Id)
{
    public string Description { get; init; }
}