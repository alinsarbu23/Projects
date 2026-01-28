using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Respawn;
using Respawn.Graph;

public static class DatabaseReset
{
    private static Respawner? _respawner;
    private static string? _connectionString;

    public static async Task InitializeAsync(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("InsuranceDb");

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" },

            TablesToIgnore = new Table[]
            {
                new Table("dbo", "__EFMigrationsHistory"),
                new Table("dbo", "AspNetRoles"),
                new Table("dbo", "Countries"),
                new Table("dbo", "Counties"),
                new Table("dbo", "Cities"),
                new Table("dbo", "BuildingTypes")
            }
        });
    }

    public static async Task ResetAsync()
    {
        await using var conn = new SqlConnection(_connectionString!);
        await conn.OpenAsync();

        await _respawner!.ResetAsync(conn);
    }
}
