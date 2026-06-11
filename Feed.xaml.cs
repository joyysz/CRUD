using System.Windows;
using CRUD.Modelos;
using MySql.Data.MySqlClient;

namespace CRUD;

public partial class Feed : Window
{
    public Feed(Usuario usuario)
    {
        InitializeComponent();
        CarregarPosts_Quandoiniciar();
    }

    private void CarregarPosts_Quandoiniciar()
    {
        List<Postagem> listaPostagens = [];

        const string query =
            "SELECT p.id, p.conteudo,p.curtidas,p.postado_em,u.nome,u.username FROM postagens p INNER JOIN usuarios u ON p.usuario_id = u.id ORDER BY p.postado_em DESC";
        
        using var conexao = new MySqlConnection(App.StringConexao);

        using var comando = new MySqlCommand(query, conexao);

        try
        {
            conexao.Open();

            using var leitor = comando.ExecuteReader();

            if (!leitor.HasRows)
            {
                MessageBox.Show("Nenhuma postagem foi encontrada.");
                return;
            }

            while (leitor.Read())
            {
                var postagem = new Postagem()
                {
                    Id = leitor.GetInt32("id"),
                    Conteudo = leitor.GetString("conteudo"),
                    Curtidas = leitor.GetInt32("curtidas"),
                    Postado_em = leitor.GetDateTime("postado_em"),
                    Usuario = new Usuario
                    {
                        Nome = leitor.GetString("nome"),
                        Username = leitor.GetString("username")
                    }
                };
                
                listaPostagens.Add(postagem);
            }
            
            ItemsControlFeed.ItemsSource = listaPostagens;
        }
        catch (MySqlException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}