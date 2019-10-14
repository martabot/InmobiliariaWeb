using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria_.Net_Core.Models {
    public class RepositorioAlquiler : RepositorioBase, IRepositorio<Alquiler> {
        public RepositorioAlquiler(IConfiguration configuration) : base(configuration) {

        }

        public int Alta(Alquiler a) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {

                string sql = $"INSERT INTO Contratos (Inquilino,Inmueble,FechaInicio,MontoTotal) " +
                    $"VALUES ('{a.IdInquilino}','{a.IdInmueble}','{a.FechaInicio}','{a.MontoTotal}')";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    a.Id = Convert.ToInt32(id);
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"DELETE FROM Contratos WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Alquiler> Buscar(string clave) {
            IList<Alquiler> res = new List<Alquiler>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT c.Id, p.Nombre, p.Apellido, i.Nombre, i.Apellido, d.Direccion, c.FechaInicio,c.FechaFin, c.MontoTotal,c.Multa, c.Inmueble, d.Propietario, i.Id " +
                    $" FROM Contratos c, Propietarios p, Inquilinos i, Inmuebles d WHERE c.Inmueble=d.Id AND d.Propietario=p.IdPropietario AND c.Inquilino=i.Id AND ((p.Nombre LIKE '%' + '{clave}' + '%') OR (i.Nombre LIKE '%' + '{clave}' + '%') OR (p.Apellido LIKE '%' + '{clave}' + '%') OR (i.Apellido LIKE '%' + '{clave}' + '%') OR (d.Direccion LIKE '%' + '{clave}' + '%') OR (p.Email LIKE '%' + '{clave}' + '%') OR (p.Dni LIKE '%' + '{clave}' + '%') OR (p.Telefono LIKE '%' + '{clave}' + '%') OR (i.Mail LIKE '%' + '{clave}' + '%') OR (i.Dni LIKE '%' + '{clave}' + '%') OR (i.Telefono LIKE '%' + '{clave}' + '%') OR (i.DniGarante LIKE '%' + '{clave}' + '%') OR (i.TelGarante LIKE '%' + '{clave}' + '%') OR (i.DireccionTrabajo LIKE '%' + '{clave}' + '%') OR (d.Categoria LIKE '%' + '{clave}' + '%') OR (d.Uso LIKE '%' + '{clave}' + '%') OR (d.Transaccion LIKE '%' + '{clave}' + '%') OR (FechaInicio LIKE '%' + '{clave}' + '%'))";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Alquiler a = new Alquiler {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetString(6),
                            MontoTotal = reader.GetDecimal(8),
                            Inmueble = new Inmueble {
                                Propietario = new Propietario {
                                    IdPropietario = reader.GetInt32(11),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2)
                                },
                                Id = reader.GetInt32(10),
                                Direccion = reader.GetString(5)
                            },
                            Inquilino = new Inquilino {
                                Id = reader.GetInt32(12),
                                Nombre = reader.GetString(3),
                                Apellido = reader.GetString(4)
                            },
                            Multa = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                            FechaFin = reader.IsDBNull(7) ? "" : reader.GetString(7),
                        };
                        res.Add(a);

                    };
                    connection.Close();
                }
                return res;
            }
        }

        public int Modificacion(Alquiler a) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"UPDATE Contratos SET FechaFin={a.FechaFin} , Multa={a.Multa} " +
                    $"WHERE Id ={a.Id}";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Alquiler ObtenerPorId(int id) {
            Alquiler a = null;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT c.Id, p.Nombre, p.Apellido, i.Nombre, i.Apellido, d.Direccion, c.FechaInicio,c.FechaFin, c.MontoTotal,c.Multa, c.Inmueble, d.Propietario, i.Id, d.Precio " +
                    $" FROM Contratos c, Propietarios p, Inquilinos i, Inmuebles d WHERE c.Inmueble=d.Id AND d.Propietario=p.IdPropietario AND c.Inquilino=i.Id AND c.Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        a = new Alquiler {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetString(6),
                            MontoTotal = reader.GetDecimal(8),
                            Inmueble = new Inmueble {
                                Propietario = new Propietario {
                                    IdPropietario = reader.GetInt32(11),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2)
                                },
                                Id = reader.GetInt32(10),
                                Direccion = reader.GetString(5),
                                Precio = reader.GetDecimal(13)
                            },
                            Inquilino = new Inquilino {
                                Id = reader.GetInt32(12),
                                Nombre = reader.GetString(3),
                                Apellido = reader.GetString(4)
                            },
                            Multa = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                            FechaFin = reader.IsDBNull(7) ? "" : reader.GetString(7),
                        };
                    }
                    connection.Close();
                }
            }
            return a;
        }



        public IList<Alquiler> ObtenerTodos() {
            IList<Alquiler> res = new List<Alquiler>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT c.Id, p.Nombre, p.Apellido, i.Nombre, i.Apellido, d.Direccion, c.FechaInicio,c.FechaFin, c.MontoTotal,c.Multa, c.Inmueble, d.Propietario, i.Id " +
                    $" FROM Contratos c, Propietarios p, Inquilinos i, Inmuebles d WHERE c.Inmueble=d.Id AND d.Propietario=p.IdPropietario AND c.Inquilino=i.Id";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Alquiler a = new Alquiler {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetString(6),
                            MontoTotal = reader.GetDecimal(8),
                            Inmueble = new Inmueble {
                                Propietario = new Propietario {
                                    IdPropietario = reader.GetInt32(11),
                                    Nombre = reader.GetString(1),
                                    Apellido = reader.GetString(2)
                                },
                                Id = reader.GetInt32(10),
                                Direccion = reader.GetString(5)
                            },
                            Inquilino = new Inquilino {
                                Id = reader.GetInt32(12),
                                Nombre = reader.GetString(3),
                                Apellido = reader.GetString(4)
                            },
                            Multa = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9),
                            FechaFin = reader.IsDBNull(7) ? "" : reader.GetString(7),
                        };
                        res.Add(a);

                    };
                    connection.Close();
                }
                return res;
            }

        }
    }
}
