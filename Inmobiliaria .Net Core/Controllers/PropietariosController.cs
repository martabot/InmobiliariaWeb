using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inmobiliaria_.Net_Core.Controllers {
    public class PropietariosController : Controller {
        private readonly IRepositorioPropietario propietarios;

        public PropietariosController(IRepositorioPropietario Propietarios) {
            propietarios = Propietarios;
        }

        [Authorize]
        public ActionResult Index() {
            if (TempData.ContainsKey("Busqueda")) {
                var list = propietarios.Buscar((string)TempData["Busqueda"]);
                ViewBag.Id = "encontraron " + list.Count() + " resultados";
                return View(list);
            } else {
                var lista = propietarios.ObtenerTodos();
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                return View(lista);
            }
        }
        [Authorize]
        public ActionResult Create() {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario) {
            try {
                TempData["Nombre"] = propietario.Nombre;
                if (ModelState.IsValid) {
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    propietarios.Alta(propietario);
                    TempData["Id"] = "creó el propietario";
                    return RedirectToAction(nameof(Index));
                } else
                    return View();
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }
        [Authorize]
        public ActionResult Edit(int id) {
            Propietario p = propietarios.ObtenerPorId(id);
            return View(p);
        }
        [Authorize]
        public ActionResult PassWord(int id) {
            Propietario p = propietarios.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PassWord(int id, Propietario propietario, string ClaveNueva, string claveRepetida) {
            Propietario propi = null;
            try {
                propi = propietarios.ObtenerPorId(id);
                // verificar clave antigüa
                var pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave ?? "",
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                if (propi.Clave != pass || ClaveNueva != claveRepetida) {
                    ViewBag.Mensaje = "Datos inválidos";
                    //se rederige porque no hay vista de cambio de pass, está compartida con Edit
                    return RedirectToAction("PassWord", new { id = id });
                } else {
                    var nueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: ClaveNueva ?? "",
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    propietarios.ActualizarClave(id, nueva);
                    TempData["Id"] = "actualizó la contraseña";
                    return RedirectToAction(nameof(Index));
                }

            } catch (Exception ex) {
                TempData["Error"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToAction("PassWord", new { id = id });
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario) {
            try {
                propietarios.Modificacion(propietario);
                TempData["Id"] = "actualizó el propietario";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(propietario);
            }
        }
        [Authorize]
        public ActionResult Delete(int id) {
            Propietario uno = propietarios.ObtenerPorId(id);
            ViewBag.datos = uno.Nombre + " " + uno.Apellido + " con D.N.I.: " + uno.Dni;
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario collection) {
            try {
                // TODO: Add delete logic here
                propietarios.Baja(id);
                TempData["Id"] = "eliminó el propietario";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE")) {
                    Propietario uno = propietarios.ObtenerPorId(id);
                    ViewBag.datos = uno.Nombre + " " + uno.Apellido + " con D.N.I.: " + uno.Dni;
                    ViewBag.Error = "No se puede eliminar el propietario ya que tiene inmuebles a su nombre";
                } else {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                }
                return View(collection);
            }
        }
    }
}