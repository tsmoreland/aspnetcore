namespace CarvedRock.Application.Contracts;

public interface IHostEnvironmentFacade
{
    bool IsDevelopment { get; }
}