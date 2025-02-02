﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria_.Net_Core.Models {
    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario {

        public RepositorioPropietario(IConfiguration configuration) : base(configuration) {

        }

        public int Alta(Propietario p) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"INSERT INTO Propietarios (Nombre, Apellido, Dni, Telefono, Email, Clave) " +
                    $"VALUES ('{p.Nombre}', '{p.Apellido}','{p.Dni}','{p.Telefono}','{p.Email}','{p.Clave}')";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    command.CommandText = "SELECT SCOPE_IDENTITY()";
                    var id = command.ExecuteScalar();
                    p.IdPropietario = Convert.ToInt32(id);
                    connection.Close();
                }
            }
            return res;
        }
        public int Baja(int id) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"DELETE FROM Propietarios WHERE IdPropietario = {id}";
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        res = command.ExecuteNonQuery();
                        connection.Close();
                    }
            }
            return res;
        }
        public int Modificacion(Propietario p) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"UPDATE Propietarios SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@mail " +
                    $"WHERE IdPropietario = {p.IdPropietario}";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = p.Nombre;
                    command.Parameters.Add("@apellido", SqlDbType.VarChar).Value = p.Apellido;
                    command.Parameters.Add("@dni", SqlDbType.VarChar).Value = p.Dni;
                    command.Parameters.Add("@telefono", SqlDbType.VarChar).Value = p.Telefono;
                    command.Parameters.Add("@mail", SqlDbType.VarChar).Value = p.Email;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Propietario> ObtenerTodos() {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave" +
                    $" FROM Propietarios";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Propietario p = new Propietario {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Propietario ObtenerPorId(int id) {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM Propietarios" +
                    $" WHERE IdPropietario=@id";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        p = new Propietario {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public Propietario ObtenerPorEmail(string email) {
            Propietario p = null;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave FROM Propietarios" +
                    $" WHERE Email=@email";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read()) {
                        p = new Propietario {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public int ActualizarClave(int id,string ClaveNueva) {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"UPDATE Propietarios SET Clave=@clave " +
                    $"WHERE IdPropietario = {id}";
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.Parameters.Add("@clave", SqlDbType.VarChar).Value = ClaveNueva;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return res;
        }

        public IList<Propietario> Buscar(string clave) {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                string sql = $"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Clave" +
                    $" FROM Propietarios WHERE (Nombre LIKE '%' + '{clave}' + '%') OR (Apellido LIKE '%' + '{clave}' + '%') OR (Dni LIKE '%' + '{clave}' + '%') OR (Telefono LIKE '%' + '{clave}' + '%') OR (Email LIKE '%' + '{clave}' + '%')" ;
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        Propietario p = new Propietario {
                            IdPropietario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                            Clave = reader.GetString(6),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
