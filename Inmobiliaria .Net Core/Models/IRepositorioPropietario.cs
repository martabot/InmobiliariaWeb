namespace Inmobiliaria_.Net_Core.Models {
    public interface IRepositorioPropietario : IRepositorio<Propietario> {
        Propietario ObtenerPorEmail(string email);

        int ActualizarClave(int id, string clave);
    }
}
