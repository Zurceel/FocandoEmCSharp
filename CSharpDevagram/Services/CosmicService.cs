using CSharpDevagram.Dto;
using System.Net.Http.Headers;

namespace CSharpDevagram.Services
{
	public class CosmicService
	{
		public string EnviarImagem(ImagemDto imagemDto)
		{
			Stream imagem; 
			
			imagem = imagemDto.Imagem.OpenReadStream();

			var client = new HttpClient();

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "3oIhGDaF7JZaupojZ6Q5IIVxY7k0Qc0jNYCjl7SAo6punTAldN");

			var request = new HttpRequestMessage(HttpMethod.Post, "file");
			var conteudo = new MultipartFormDataContent
			{
				{new StreamContent(imagem), "media", imagemDto.Nome }
			};

			request.Content = conteudo;
			var retornoreq = client.PostAsync("https://upload.cosmicjs.com/v2/buckets/083a0520-4fd2-11ed-a80c-9d7ab94085b2/media", request.Content).Result;

			var urlretorno = retornoreq.Content.ReadFromJsonAsync<CosmicRespostaDto>();

			return urlretorno.Result.media.url;
		}
	}
}
