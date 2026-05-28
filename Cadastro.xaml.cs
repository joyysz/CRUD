using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace CRUD;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class Cadastro : Window
{
    public string stringConexao = Environment.GetEnvironmentVariable("MYSQL_STRING"); 
        
    public Cadastro()
    {
        InitializeComponent();
    }

    private void BtnCadastrar_OnClick (object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text) ||
            string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtSenha.Password))

        {
            MessageBox.Show("Todos os campos são obrigatórios.", "Erro!");
            return; 
        }

        using (var conexao = new MySqlConnection(stringConexao))
        {
            var query = "INSERT INTO usuarios(nome, username, email, senha) VALUES(@nome, @username, @email, @senha)";

            using (var comando = new MySqlCommand(query, conexao))
            {
                
                comando.Parameters.AddWithValue("@nome", txtNome.Text);
                comando.Parameters.AddWithValue("@username", txtUsername.Text);
                comando.Parameters.AddWithValue("@email", txtEmail.Text);
                comando.Parameters.AddWithValue("@senha", txtSenha.Password);

                try
                { 
                    conexao.Open();

                    var linhasAfetadas = comando.ExecuteNonQuery();
                    if (linhasAfetadas > 0) ;
                    {
                        MessageBox.Show("Cadastro efetuado com sucesso!");
                        return;
                    }
                }
                catch (Exception exception)
                {
                    if (exception is MySqlException erroSql)
                    {
                        if (erroSql.Number == 1062)
                        {
                            MessageBox.Show("O email ou username já foram utilizados");
                            return;
                        }
                    }
                        
                    Console.WriteLine(exception);
                }
            }
        }
    }
}