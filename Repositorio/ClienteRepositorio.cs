using Cadastro_de_cliente.Models;


namespace Cadastro_de_cliente.Repositorio

{
    public class ClienteRepositorio
    {
        public List<Cliente> _clientes = new List<Cliente>();

        public void AdicionarCliente(Cliente cliente)
        {
            _clientes.Add(cliente);
        }

        public List<Cliente> ConsultarTodos() 
        {
            return _clientes;
        }

        public void ApagarCliente()
        {
            foreach (var cliente in _clientes)
            {

            }
        }


    }
}
