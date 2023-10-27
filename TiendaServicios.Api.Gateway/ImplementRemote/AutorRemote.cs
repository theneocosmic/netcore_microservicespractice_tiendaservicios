using System.Runtime.CompilerServices;
using System.Text.Json;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.ImplementRemote
{
	public class AutorRemote : IAutorRemote
	{
		private readonly ILogger<AutorRemote> _logger;
		private readonly IHttpClientFactory _httpClient;

		public AutorRemote(ILogger<AutorRemote> logger, IHttpClientFactory httpClient)
		{
			_logger = logger;
			_httpClient = httpClient;
		}

		public async Task<(bool resultado, AutorModeloRemote autor, string ErrorMessage)> GetAutor(Guid AutorId)
		{
			try
			{
				var cliente = _httpClient.CreateClient("AutorService");
				var response = await cliente.GetAsync($"/Autor/{AutorId}");

				if (response.IsSuccessStatusCode)
				{
					var contenido = await response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
					var resultado = JsonSerializer.Deserialize<AutorModeloRemote>(contenido, options);
					return (true, resultado, null);
				}

				return (false, null, response.ReasonPhrase);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				return (false, null, ex.Message);
			}

		}
	}
}