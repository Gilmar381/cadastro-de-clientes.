using System;
using System.Windows.Forms;
using Cadastro_de_cliente.Views;

namespace Cadastro_de_cliente
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configurações padrão geradas pelo Visual Studio
            ApplicationConfiguration.Initialize();

            // Inicia a aplicação abrindo a FormCliente
            Application.Run(new FormCliente());
        }
    }
}