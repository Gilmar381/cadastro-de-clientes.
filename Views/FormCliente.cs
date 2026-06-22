using Cadastro_de_cliente.Controllers;
using Cadastro_de_cliente.Models;
using System;
using System.Windows.Forms;
namespace Cadastro_de_cliente.Views
{
    public class FormCliente : Form
    {
        // Instância do Controller que vai cuidar das regras de negócio
        private readonly ControllerCliente _controller = new ControllerCliente();

        // Guarda o Id do cliente selecionado no grid (para editar/excluir)
        // -1 significa que nenhum cliente está selecionado (modo "novo cadastro")
        private int _idSelecionado = -1;

        // ===== Controles da tela =====
        private Label lblNome;
        private Label lblEmail;
        private Label lblTelefone;
        private Label lblEndereco;

        private TextBox txtNome;
        private TextBox txtEmail;
        private MaskedTextBox txtTelefone;
        private TextBox txtEndereco;

        private Button btnCadastrar;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnLimpar;

        private DataGridView dgvClientes;

        public FormCliente()
        {
            InitializeComponent();
            CarregarGrid();
        }

        // Monta a interface "na mão" (sem precisar do Designer)
        private void InitializeComponent()
        {
            this.Text = "Cadastro de Clientes";
            this.Width = 650;
            this.Height = 520;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ---------- Label + TextBox Nome ----------
            lblNome = new Label { Text = "Nome:", Left = 20, Top = 20, Width = 80 };
            txtNome = new TextBox { Left = 110, Top = 17, Width = 480 };

            // ---------- Label + TextBox Email ----------
            lblEmail = new Label { Text = "Email:", Left = 20, Top = 55, Width = 80 };
            txtEmail = new TextBox { Left = 110, Top = 52, Width = 480 };

            // ---------- Label + MaskedTextBox Telefone ----------
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

            // ---------- Label + TextBox Endereco ----------
            lblEndereco = new Label { Text = "Endereço:", Left = 20, Top = 125, Width = 80 };
            txtEndereco = new TextBox { Left = 110, Top = 122, Width = 480 };

            // ---------- Botões ----------
            btnCadastrar = new Button { Text = "Cadastrar", Left = 110, Top = 165, Width = 110, Height = 30 };
            btnCadastrar.Click += BtnCadastrar_Click;

            btnEditar = new Button { Text = "Salvar Edição", Left = 230, Top = 165, Width = 110, Height = 30 };
            btnEditar.Click += BtnEditar_Click;

            btnExcluir = new Button { Text = "Excluir", Left = 350, Top = 165, Width = 110, Height = 30 };
            btnExcluir.Click += BtnExcluir_Click;

            btnLimpar = new Button { Text = "Limpar", Left = 470, Top = 165, Width = 110, Height = 30 };
            btnLimpar.Click += BtnLimpar_Click;

            // ---------- Grid de clientes ----------
            dgvClientes = new DataGridView
            {
                Left = 20,
                Top = 210,
                Width = 590,
                Height = 250,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoGenerateColumns = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            // Colunas criadas manualmente, com larguras fixas (somam 590, sem precisar de "Fill")
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

            // Adiciona todos os controles ao formulário
            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblTelefone);
            this.Controls.Add(txtTelefone);
            this.Controls.Add(lblEndereco);
            this.Controls.Add(txtEndereco);
            this.Controls.Add(btnCadastrar);
            this.Controls.Add(btnEditar);
            this.Controls.Add(btnExcluir);
            this.Controls.Add(btnLimpar);
            this.Controls.Add(dgvClientes);
        }

        // ===================== EVENTOS =====================

        // Botão Cadastrar: cria um novo cliente e adiciona na lista
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

        // Botão Salvar Edição: atualiza o cliente selecionado no grid
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

        // Botão Excluir: remove o cliente selecionado no grid
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

        // Botão Limpar: limpa os campos e desfaz a seleção
        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        // Clique em uma linha do grid: carrega os dados do cliente nos campos
        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // clique no cabeçalho, ignora

            DataGridViewRow linha = dgvClientes.Rows[e.RowIndex];

            _idSelecionado = Convert.ToInt32(linha.Cells["Id"].Value);
            txtNome.Text = linha.Cells["Nome"].Value?.ToString();
            txtEmail.Text = linha.Cells["Email"].Value?.ToString();
            txtTelefone.Text = linha.Cells["Telefone"].Value?.ToString();
            txtEndereco.Text = linha.Cells["Endereco"].Value?.ToString();
        }

        // ===================== MÉTODOS AUXILIARES =====================

        // Recarrega o grid com a lista atualizada de clientes
        // (sem usar DataSource: limpa as linhas e adiciona uma a uma)
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

        // Limpa os campos do formulário e desfaz a seleção do grid
        private void LimparCampos()
        {
            txtNome.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
            txtEndereco.Clear();
            _idSelecionado = -1;
            dgvClientes.ClearSelection();
        }
    }
}