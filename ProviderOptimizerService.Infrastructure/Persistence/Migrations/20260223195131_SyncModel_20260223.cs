using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProviderOptimizerService.Infrastructure.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class SyncModel_20260223 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// 🔒 Idempotente: en PostgreSQL no falla si la columna ya no existe
			// OJO: "Capabilities" fue creada con comillas => sensible a mayúsculas
			migrationBuilder.Sql(@"ALTER TABLE ""public"".""providers"" DROP COLUMN IF EXISTS ""Capabilities"";");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// Si necesitas revertir, re‑crea la columna (nullable para evitar conflictos)
			// Nota: era un array de enteros (integer[]) en PostgreSQL
			migrationBuilder.Sql(@"ALTER TABLE ""public"".""providers"" ADD COLUMN IF NOT EXISTS ""Capabilities"" integer[] NULL;");
		}
	}
}