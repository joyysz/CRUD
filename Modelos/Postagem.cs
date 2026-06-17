using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRUD.Modelos;

public class Postagem : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Conteudo { get; set; }

    public int Curtidas
    {
        get => _curtidas;
        set
        {
            _curtidas = value;
            NotificarPropriedadeAlterada();
        }
    }
    
    public DateTime Postado_em { get; set; }
    public Usuario Usuario { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private bool _foiCurtido;
    private int _curtidas;


    public bool FoiCurtido
    {
        get => _foiCurtido;
        set
        {
            if (_foiCurtido != value)
            {
                _foiCurtido = value;
                NotificarPropriedadeAlterada();
            }
        }
    }

    private void NotificarPropriedadeAlterada ([CallerMemberName] string nomePropriedade = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
    }
}