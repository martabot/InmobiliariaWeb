﻿using System.Collections.Generic;

namespace Inmobiliaria_.Net_Core.Models {
    public interface IRepositorio<T> {
        int Alta(T p);
        int Baja(int id);
        int Modificacion(T p);

        IList<T> ObtenerTodos();
        T ObtenerPorId(int id);
        IList<T> Buscar(string clave);
    }
}
