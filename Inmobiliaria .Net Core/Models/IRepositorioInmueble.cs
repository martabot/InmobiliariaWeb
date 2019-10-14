namespace Inmobiliaria_.Net_Core.Models {
    public interface IRepositorioInmueble : IRepositorio<Inmueble> {
        int ActualizarDisponible(int id, int disponible);
    }
}
