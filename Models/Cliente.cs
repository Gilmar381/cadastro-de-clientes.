

namespace Cadastro_de_cliente.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
       
       
        public Cliente()
        {
            Nome = string.Empty;
            Email = string.Empty; 
            Telefone = string.Empty;
            Endereco = string.Empty;
        }

       
        public Cliente(string nome, string email, string telefone, string endereco)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("Nome é obrigatório!");
                if (nome.Length < 3)
                {
                    throw new Exception("O nome deve ter mais que três caracteres");
                }

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email é obrigatório!");
                if (!email.Contains("@") || !email.Contains("."))
                {
                throw new Exception("Email invalido");
                }

            if (string.IsNullOrWhiteSpace(telefone))
                throw new Exception("O numero de telefone é obrigatório!");
                if (telefone.Length < 10 || telefone.Length > 11)
                {
                throw new Exception("Telefone invalido");
                }

                Nome = nome;
                Email = email;
                Telefone = telefone;
                Endereco = endereco;
        }
    }
}
