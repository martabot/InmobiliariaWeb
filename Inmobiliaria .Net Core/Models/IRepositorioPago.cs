using System.Collections.Generic;

namespace Inmobiliaria_.Net_Core.Models {
    public interface IRepositorioPago : IRepositorio<Pago> {
        IList<Pago> ObtenerPorAlquiler(int alquiler);
        int maxPago(int alquiler);
    }
}
