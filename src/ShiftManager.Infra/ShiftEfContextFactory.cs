using Microsoft.EntityFrameworkCore.Design;

namespace ShiftManager.Infra;

public class ShiftEfContextFactory : IDesignTimeDbContextFactory<ShiftContext>
{
    public ShiftContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShiftContext>();
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ShiftDb;Integrated Security=True");

        return new ShiftContext(optionsBuilder.Options);
    }
}