using Cadastro_de_cliente.Controllers;
using Cadastro_de_cliente.Models;
using Cadastro_de_cliente.Services;

namespace Cadastro_de_cliente.Views
{
    public class FormCliente : Form
    {
        private readonly ControllerCliente _controller = new ControllerCliente();
        private int _idSelecionado = -1;

        // ===== Controles da tela =====
        private Label lblNome;
        private Label lblEmail;
        private Label lblTelefone;
        private Label lblCep;
        private Label lblEndereco;

        private TextBox txtNome;
        private TextBox txtEmail;
        private MaskedTextBox txtTelefone;
        private MaskedTextBox txtCep;
        private TextBox txtEndereco;

        private Button btnCadastrar;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnLimpar;
        private Button btnBuscarCep;

        private DataGridView dgvClientes;

        public FormCliente()
        {
            InitializeComponent();
            CarregarGrid();
        }

        private void InitializeComponent()
        {
            this.Text = "Cadastro de Clientes";
            this.Width = 650;
            this.Height = 560;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ---------- Nome ----------
            lblNome = new Label { Text = "Nome:", Left = 20, Top = 20, Width = 80 };
            txtNome = new TextBox { Left = 110, Top = 17, Width = 480 };

            // ---------- Email ----------
            lblEmail = new Label { Text = "Email:", Left = 20, Top = 55, Width = 80 };
            txtEmail = new TextBox { Left = 110, Top = 52, Width = 480 };

            // ---------- Telefone ----------
            lblTelefone = new Label { Text = "Telefone:", Left = 20, Top = 90, Width = 80 };
            txtTelefone = new MaskedTextBox
            {
                Left = 110,
                Top = 87,
                Width = 200,
                Mask = "(00) 00000-0000",
                SkipLiterals = true,
                TextMaskFormat = MaskFormat.ExcludePromptAndLiterals
            };

            // ---------- CEP + Botão Buscar ----------
            lblCep = new Label { Text = "CEP:", Left = 20, Top = 125, Width = 80 };
            txtCep = new MaskedTextBox
            {
                Left = 110,
                Top = 122,
                Width = 100,
                Mask = "00000-000",
                TextMaskFormat = MaskFormat.ExcludePromptAndLiterals
            };
            btnBuscarCep = new Button { Text = "Buscar CEP", Left = 220, Top = 121, Width = 90, Height = 23 };
            btnBuscarCep.Click += BtnBuscarCep_Click;

            // ---------- Endereço ----------
            lblEndereco = new Label { Text = "Endereço:", Left = 20, Top = 160, Width = 80 };
            txtEndereco = new TextBox { Left = 110, Top = 157, Width = 480 };

            // ---------- Botões ----------
            btnCadastrar = new Button { Text = "Cadastrar", Left = 110, Top = 200, Width = 110, Height = 30 };
            btnCadastrar.Click += BtnCadastrar_Click;
            btnEditar = new Button { Text = "Salvar Edição", Left = 230, Top = 200, Width = 110, Height = 30 };
            btnEditar.Click += BtnEditar_Click;
            btnExcluir = new Button { Text = "Excluir", Left = 350, Top = 200, Width = 110, Height = 30 };
            btnExcluir.Click += BtnExcluir_Click;
            btnLimpar = new Button { Text = "Limpar", Left = 470, Top = 200, Width = 110, Height = 30 };
            btnLimpar.Click += BtnLimpar_Click;

            // ---------- Grid ----------
            dgvClientes = new DataGridView
            {
                Left = 20,
                Top = 245,
                Width = 590,
                Height = 265,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            dgvClientes.Columns.Add("Id", "Id");
            dgvClientes.Columns["Id"].Width = 50;
            dgvClientes.Columns.Add("Nome", "Nome");
            dgvClientes.Columns["Nome"].Width = 160;
            dgvClientes.Columns.Add("Email", "Email");
            dgvClientes.Columns["Email"].Width = 160;
            dgvClientes.Columns.Add("Telefone", "Telefone");
            dgvClientes.Columns["Telefone"].Width = 110;
            dgvClientes.Columns.Add("Endereco", "Endereço");
            dgvClientes.Columns["Endereco"].Width = 110;
            dgvClientes.CellClick += DgvClientes_CellClick;

            // ---------- Adiciona ao form ----------
            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblTelefone);
            this.Controls.Add(txtTelefone);
            this.Controls.Add(lblCep);
            this.Controls.Add(txtCep);
            this.Controls.Add(btnBuscarCep);
            this.Controls.Add(lblEndereco);
            this.Controls.Add(txtEndereco);
            this.Controls.Add(btnCadastrar);
            this.Controls.Add(btnEditar);
            this.Controls.Add(btnExcluir);
            this.Controls.Add(btnLimpar);
            this.Controls.Add(dgvClientes);
        }
        private async void BtnBuscarCep_Click(object sender, EventArgs e)
        {
            string cep = txtCep.Text.Replace("-", "").Trim();

            if (cep.Length != 8)
            {
                MessageBox.Show("CEP inválido. Digite os 8 dígitos.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServicoViaCep servico = new ServicoViaCep();
            EnderecoViaCep? endereco = await servico.BuscarAsync(cep);

            if (endereco == null)
            {
                MessageBox.Show("CEP não encontrado.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtEndereco.Text = $"{endereco.Logradouro}, {endereco.Bairro}, {endereco.Localidade} - {endereco.Uf}";
        }
        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = new Cliente(
                    txtNome.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtTelefone.Text.Trim(),
                    txtEndereco.Text.Trim()
                );

                _controller.Adicionar(cliente);

                MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparCampos();
                CarregarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro de validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (_idSelecionado == -1)
            {
                MessageBox.Show("Selecione um cliente no grid para editar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cliente clienteAtualizado = new Cliente(
                    txtNome.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtTelefone.Text.Trim(),
                    txtEndereco.Text.Trim()
                );
                clienteAtualizado.Id = _idSelecionado;

                bool sucesso = _controller.Atualizar(clienteAtualizado);

                if (sucesso)
                {
                    MessageBox.Show("Cliente atualizado com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LimparCampos();
                    CarregarGrid();
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro de validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (_idSelecionado == -1)
            {
                MessageBox.Show("Selecione um cliente no grid para excluir.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resposta = MessageBox.Show(
                "Tem certeza que deseja excluir este cliente?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resposta == DialogResult.Yes)
            {
                bool sucesso = _controller.Remover(_idSelecionado);

                if (sucesso)
                {
                    LimparCampos();
                    CarregarGrid();
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow linha = dgvClientes.Rows[e.RowIndex];

            _idSelecionado = Convert.ToInt32(linha.Cells["Id"].Value);
            txtNome.Text = linha.Cells["Nome"].Value?.ToString();
            txtEmail.Text = linha.Cells["Email"].Value?.ToString();
            txtTelefone.Text = linha.Cells["Telefone"].Value?.ToString();
            txtEndereco.Text = linha.Cells["Endereco"].Value?.ToString();
        }
        private void CarregarGrid()
        {
            dgvClientes.Rows.Clear();

            foreach (Cliente cliente in _controller.Listar())
            {
                dgvClientes.Rows.Add(
                    cliente.Id,
                    cliente.Nome,
                    cliente.Email,
                    cliente.Telefone,
                    cliente.Endereco
                );
            }
        }
        private void LimparCampos()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
            txtCep.Clear();
            txtEndereco.Clear();
            _idSelecionado = -1;
            dgvClientes.ClearSelection();
        }
    }
}