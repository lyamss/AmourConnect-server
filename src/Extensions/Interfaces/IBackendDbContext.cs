namespace Infrastructure.Interfaces;

public interface IBackendDbContext
{
    void Migrate();
}