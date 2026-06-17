namespace CRUD.Modelos;

public class Usuario
{
    public string Email;
    public int Id;
    public string Senha;
    public string Nome { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}