using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cadastro_de_cliente.Services
{
    internal class ServicoViaCep
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<EnderecoViaCep?> BuscarAsync(string cep)
        {
            try
            {
                string url = $"https://viacep.com.br/ws/{cep}/json/";

                string json = await _httpClient.GetStringAsync(url);

                EnderecoViaCep? resultado = JsonSerializer.Deserialize<EnderecoViaCep>(json);

                if (resultado == null || resultado.Erro == "true")
                    return null;

                return resultado;
            }
            catch
            {
                return null;
            }
        }
    }
    
}
