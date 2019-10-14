using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria_.Net_Core.Models {
    public class RepositorioInquilino : RepositorioBase, IRepositorio<Inquilino> {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration) {

        }

        public int Alta(Inquilino p) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"INSERT INTO Inquilinos (Nombre, Apellido, Dni, Telefono, Mail,DireccionTrabajo,DniGarante,TelGarante) " +
                    $"VALUES ('{p.Nombre}', '{p.Apellido}','{p.Dni}','{p.Telefono}','{p.Mail}','{p.DireccionTrabajo}','{p.DniGarante}','{p.TelGarante}')";
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
                string sql = $"DELETE FROM Inquilinos WHERE Id = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int Modificacion(Inquilino p) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"UPDATE Inquilinos SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Mail=@mail,DireccionTrabajo=@dirTrab, DniGarante=@dniGarante, TelGarante=@telGarante  " +
                    $"WHERE Id = {p.Id}";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = p.Nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = p.Apellido;
                    command.Parameters.Add("@dni", SqlDbType.VarChar).Value = p.Dni;
                    command.Parameters.Add("@telefono", SqlDbType.VarChar).Value = p.Telefono;
                    command.Parameters.Add("@mail", SqlDbType.VarChar).Value = p.Mail;
                    command.Parameters.Add("@dirTrab", SqlDbType.VarChar).Value = p.DireccionTrabajo;
                    command.Parameters.Add("@dniGarante", SqlDbType.VarChar).Value = p.DniGarante;
                    command.Parameters.Add("@telGarante", SqlDbType.VarChar).Value = p.TelGarante;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
                return res;
            }
        }

        public IList<Inquilino> ObtenerTodos() {
            IList<Inquilino> res = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Mail, DireccionTrabajo,DniGarante,TelGarante" +
                    $" FROM Inquilinos";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Inquilino i = new Inquilino {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Mail = reader.GetString(5),
                            DireccionTrabajo = reader.GetString(6),
                            DniGarante = reader.GetString(7),
                            TelGarante = reader.GetString(8)
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Inquilino ObtenerPorId(int id) {
            Inquilino p = null;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Mail, DireccionTrabajo, DniGarante,TelGarante FROM Inquilinos" +
                    $" WHERE Id=@id";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        p = new Inquilino {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Mail = reader.GetString(5),
                            DireccionTrabajo = reader.GetString(6),
                            DniGarante = reader.GetString(7),
                            TelGarante = reader.GetString(8)
                        };
                        return p;
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Inquilino> Buscar(string clave) {
            IList<Inquilino> res = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Mail, DireccionTrabajo,DniGarante,TelGarante" +
                    $" FROM Inquilinos WHERE (Nombre LIKE '%' + '{clave}' + '%') OR (Apellido LIKE '%' + '{clave}' + '%') OR (Dni LIKE '%' + '{clave}' + '%') OR (Telefono LIKE '%' + '{clave}' + '%') OR (Mail LIKE '%' + '{clave}' + '%') OR (DniGarante LIKE '%' + '{clave}' + '%') OR (TelGarante LIKE '%' + '{clave}' + '%') OR (DireccionTrabajo LIKE '%' + '{clave}' + '%')";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Inquilino i = new Inquilino {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Mail = reader.GetString(5),
                            DireccionTrabajo = reader.GetString(6),
                            DniGarante = reader.GetString(7),
                            TelGarante = reader.GetString(8)
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
