using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class Cadastro : Window
{
    public Cadastro()
    {
        InitializeComponent();
        txtNome.Focus();
    }

    private void BtnCadastrar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text) ||
            string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtSenha.Password))

        {
            MessageBox.Show("Todos os campos são obrigatórios.", "Erro!");
            return;
        }

        using var conexao = new MySqlConnection(App.StringConexao);
        const string query =
            "INSERT INTO usuarios(nome, username, email, senha) VALUES(@nome, @username, @email, @senha); SELECT LAST_INSERT_ID()";

        using var comando = new MySqlCommand(query, conexao);
        comando.Parameters.AddWithValue("@nome", txtNome.Text);
        comando.Parameters.AddWithValue("@username", txtUsername.Text);
        comando.Parameters.AddWithValue("@email", txtEmail.Text);
        comando.Parameters.AddWithValue("@senha", txtSenha.Password);

        try
        {
            conexao.Open();

            var IdGerado = comando.ExecuteScalar();
            if (IdGerado is null) throw new Exception ("Cadastro não foi realizado!");
            new Feed(new Usuario
            {
                Nome = txtNome.Text,
                Email = txtEmail.Text,
                Username = txtUsername.Text,
                Id = Convert.ToInt32(IdGerado)
                
            }).Show();
            Close();
        }
        catch (Exception exception)
        {
            if (exception is MySqlException { Number: 1062 })
            {
                MessageBox.Show("O email ou username já foram utilizados");
                return;
            }

            MessageBox.Show(exception.Message);
        }
        finally
        {
            conexao.Close();
        }
    }
}