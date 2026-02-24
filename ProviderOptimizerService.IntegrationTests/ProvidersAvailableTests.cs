using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProviderOptimizerService.IntegrationTests
{
	public class ProvidersAvailableTests
	{
		private static readonly string BaseUrl = "http://localhost:8080";

		[Fact]
		public async Task Get_available_should_return_items()
		{
			using var http = new HttpClient { BaseAddress = new Uri(BaseUrl) };

			var resp = await http.GetAsync(
				"/providers/available?serviceType=1&lat=4.711&lng=-74.072&maxDistanceKm=30");

			Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

			var items = await resp.Content.ReadFromJsonAsync<object[]>();
			Assert.NotNull(items);
			Assert.True(items!.Length > 0);
		}
	}
}