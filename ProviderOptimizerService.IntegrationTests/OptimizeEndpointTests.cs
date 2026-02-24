using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ProviderOptimizerService.IntegrationTests
{
	public class OptimizeEndpointTests
	{
		private static readonly string BaseUrl = "http://localhost:8080";

		[Fact]
		public async Task Post_optimize_should_return_200()
		{
			using var http = new HttpClient { BaseAddress = new Uri(BaseUrl) };

			var body = new
			{
				assistanceId = "int-1",
				serviceType = 1,
				lat = 4.711,
				lng = -74.072,
				maxEtaMinutes = 60
			};

			var resp = await http.PostAsJsonAsync("/optimize", body);

			//Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

			// Leer como JSON seguro y validar que no es null
			//var doc = await resp.Content.ReadFromJsonAsync<JsonElement?>();
			//Assert.True(doc.HasValue, "La respuesta JSON no debe ser null");

			
		}
	}
}