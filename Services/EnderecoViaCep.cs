
using System.Text.Json.Serialization;

namespace Cadastro_de_cliente.Services
{
    public class EnderecoViaCep
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }                                                       

        [JsonPropertyName("Bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string Localidade { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("erro")]
        public string Erro { get; set; }
    }
}
