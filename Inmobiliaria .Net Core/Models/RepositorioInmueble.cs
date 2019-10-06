using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria_.Net_Core.Models {
    public class RepositorioInmueble : RepositorioBase, IRepositorio<Inmueble> {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration) {

        }
        public int Alta(Inmueble p) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"INSERT INTO Inmuebles (Propietario,Direccion,Disponible,Ambientes,Precio,Categoria,Uso,Transaccion) " +
                    $"VALUES ('{p.IdPropietario}','{p.Direccion}',1,'{p.Ambientes}','{p.Precio}','{p.Categoria}','{p.Uso}','{p.Transaccion}')";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.Id = Convert.ToInt32(id);
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"DELETE FROM Inmuebles WHERE Id = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inmueble p) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"UPDATE Inmuebles SET Direccion=@direccion, Disponible=@disponible, Ambientes =@ambientes, Categoria=@categoria, Uso=@uso, Transaccion=@transaccion, Precio=@precio " +
                    $"WHERE Id = {p.Id}";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@direccion", SqlDbType.VarChar).Value = p.Direccion;
                    command.Parameters.Add("@disponible", SqlDbType.Int).Value = p.Disponible;
                    command.Parameters.Add("@ambientes", SqlDbType.Int).Value = p.Ambientes;
                    command.Parameters.Add("@categoria", SqlDbType.VarChar).Value = p.Categoria;
                    command.Parameters.Add("@uso", SqlDbType.VarChar).Value = p.Uso;
                    command.Parameters.Add("@transaccion", SqlDbType.VarChar).Value = p.Transaccion;
                    command.Parameters.Add("@precio", SqlDbType.Decimal).Value = p.Precio;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble ObtenerPorId(int id) {
            Inmueble p = null;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT Direccion, Propietario, Ambientes, Precio, Disponible, Categoria, Uso, Transaccion, p.Nombre, p.Apellido FROM Inmuebles i JOIN Propietarios p ON(p.IdPropietario=i.Propietario)" +
                    $" WHERE Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        p = new Inmueble {
                            Id = id,
                            Direccion = reader.GetString(0),
                            Propietario = new Propietario {
                                IdPropietario = reader.GetInt32(1),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9)
                            },
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            Disponible = reader.GetInt32(4),
                            Categoria = reader.GetString(5),
                            Uso = reader.GetString(6),
                            Transaccion = reader.GetString(7),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Inmueble> ObtenerTodos() {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT i.Id, Direccion, Ambientes,Precio,Categoria,Uso,Transaccion, i.Propietario, p.Nombre, p.Apellido, Disponible " +
                    $" FROM Inmuebles i JOIN Propietarios p ON (p.IdPropietario=i.Propietario)";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Inmueble i = new Inmueble {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            Categoria = reader.GetString(4),
                            Uso = reader.GetString(5),
                            Transaccion = reader.GetString(6),
                            Disponible = reader.GetInt32(10),
                            Propietario = new Propietario {
                                IdPropietario = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9)
                            }
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
