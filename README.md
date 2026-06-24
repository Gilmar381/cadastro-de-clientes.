# 📋 Cadastro de Clientes

Sistema desktop de cadastro de clientes desenvolvido em **Windows Forms** com arquitetura **MVC**, banco de dados **SQLite** e integração com a API **ViaCEP** para preenchimento automático de endereço.

---

## 🚀 Funcionalidades

- Cadastrar, editar e excluir clientes
- Listagem de clientes em grid
- Validação de nome, e-mail e telefone
- Busca automática de endereço pelo CEP via API ViaCEP
- Persistência de dados com SQLite

---

## 🏗️ Arquitetura

O projeto segue o padrão **MVC (Model-View-Controller)**:

```
Cadastro_de_cliente/
│
├── Models/
│   └── Cliente.cs               # Entidade com validações
│
├── Views/
│   └── FormCliente.cs           # Interface gráfica (Windows Forms)
│
├── Controllers/
│   └── ControllerCliente.cs     # Regras de negócio e acesso ao banco
│
├── Services/
│   ├── ServicoViaCep.cs         # Chamada HTTP à API ViaCEP
│   └── EnderecoViaCep.cs        # Modelo de deserialização do JSON
│
└── ConexaoBanco.cs              # Gerenciamento da conexão SQLite
```

---

## 🛠️ Tecnologias utilizadas

| Tecnologia | Versão |
|---|---|
| .NET | 8.0 |
| Windows Forms | net8.0-windows |
| Microsoft.Data.Sqlite | 10.0.9 |
| API ViaCEP | — |

---

## ⚙️ Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) ou superior
- Windows (Windows Forms é exclusivo para Windows)
- Visual Studio 2022 ou VS Code com extensão C#

---

## ▶️ Como executar

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
```

2. Acesse a pasta do projeto:
```bash
cd Cadastro_de_cliente
```

3. Restaure os pacotes:
```bash
dotnet restore
```

4. Execute o projeto:
```bash
dotnet run
```

O banco de dados SQLite é criado automaticamente na primeira execução.

---

## 🗄️ Banco de dados

A tabela `Clientes` é criada automaticamente pelo método `InicializarBanco` do controller:

| Coluna | Tipo |
|---|---|
| Id | INTEGER PRIMARY KEY |
| Nome | TEXT |
| Email | TEXT |
| Telefone | TEXT |
| Endereco | TEXT |

---

## 🌐 Integração ViaCEP

Ao digitar um CEP no formulário e clicar em **"Buscar CEP"**, o sistema consulta a API pública [ViaCEP](https://viacep.com.br) e preenche automaticamente o campo de endereço com logradouro, bairro, cidade e estado.

---

## 📚 Conceitos praticados

- Arquitetura MVC
- Propriedades e construtores com validação em C#
- `List<T>`, `FirstOrDefault` e expressões lambda
- Exceções com `throw` e `try/catch`
- `MaskedTextBox` e `DataGridView` no Windows Forms
- SQL básico (`CREATE TABLE`, `INSERT`, `SELECT`, `UPDATE`, `DELETE`)
- SQLite com `Microsoft.Data.Sqlite`
- Requisições HTTP assíncronas com `HttpClient`
- Deserialização de JSON com `System.Text.Json`
- Git e GitHub

---

## .gitignore

```
*.db
bin/
obj/
.vs/
```
