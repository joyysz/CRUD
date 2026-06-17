using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class MeuPerfil : Window
{
    private readonly Usuario UsuarioAtual;

    public MeuPerfil(Usuario usuario)
    {
        InitializeComponent();
        UsuarioAtual = usuario;
        TxtNome.Text = UsuarioAtual.Nome;
        TxtEmail.Text = UsuarioAtual.Email;
        TxtUsername.Text = UsuarioAtual.Username;
    }

    private void ButtonSalvar_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtNome.Text) || string.IsNullOrWhiteSpace(TxtEmail.Text) ||
            string.IsNullOrWhiteSpace(TxtUsername.Text))
        {
            MessageBox.Show("Preencha todos os campos!");
            return;
        }

        var senhaFoiAlterada = !string.IsNullOrWhiteSpace(TxtSenha.Password);

        UsuarioAtual.Username = TxtUsername.Text;
        UsuarioAtual.Nome = TxtNome.Text;
        UsuarioAtual.Email = TxtEmail.Text;
        if (senhaFoiAlterada) UsuarioAtual.Senha = TxtSenha.Password;

        using var conexao = new MySqlConnection(App.StringConexao);
        var query = "UPDATE usuarios SET username = @username, nome = @nome, email = @email";

        if (senhaFoiAlterada) query += ", senha = @senha";

        query += " WHERE id = @id";

        using var comando = new MySqlCommand(query, conexao);

        comando.Parameters.AddWithValue("@username", UsuarioAtual.Username);
        comando.Parameters.AddWithValue("@nome", UsuarioAtual.Nome);
        comando.Parameters.AddWithValue("@email", UsuarioAtual.Email);
        comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);

        if (senhaFoiAlterada) comando.Parameters.AddWithValue("@senha", UsuarioAtual.Senha);

        try
        {
            conexao.Open();
            var linhasAfetadas = comando.ExecuteNonQuery();
            if (linhasAfetadas > 0)
                MessageBox.Show("Cadastro atualizado com sucesso!");
            else
                MessageBox.Show("Erro ao atualizar o cadastro!");
        }
        catch (Exception exception)
        {
            MessageBox.Show("Erro ao DB. ");
        }
    }

    private void BtnDeletar_OnClick(object sender, RoutedEventArgs e)
    {
        {
            var resultadoMessageBox = MessageBox.Show("Você tem certeza que deseja apagar o seu perfil?",
                "Confirmação de exclusão", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultadoMessageBox == MessageBoxResult.No) return;

            using var conexao = new MySqlConnection(App.StringConexao);
            var query = "DELETE FROM usuarios WHERE id = @id";
            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", UsuarioAtual.Id);
            try
            {
                conexao.Open();
                var linhasAfetadas = comando.ExecuteNonQuery();
                if (linhasAfetadas > 0)
                {
                    MessageBox.Show("Conta excluida com sucesso!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Nenhuma conta foi encontrada.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Erro ao excluir conta: {exception.Message} ");
            }
        }
    }
}