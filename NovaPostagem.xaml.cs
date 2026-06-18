using System.Windows;
using CRUD.Modelos;

namespace CRUD;

public partial class NovaPostagem : Window
{
    private Usuario _usuario;
    public NovaPostagem(Usuario usuario)
    {
        InitializeComponent();
        _usuario = usuario;
    }
}