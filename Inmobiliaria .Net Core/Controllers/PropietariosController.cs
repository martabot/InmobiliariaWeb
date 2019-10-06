using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Inmobiliaria_.Net_Core.Controllers {
    public class PropietariosController : Controller {
        private readonly IRepositorio<Propietario> repositorio;
        private readonly IRepositorioPropietario propietarios;

        public PropietariosController(IRepositorio<Propietario> repositorio, IRepositorioPropietario Propietarios) {
            this.repositorio = repositorio;
            propietarios = Propietarios; 
        }

        [Authorize]
        // GET: Propietario
        public ActionResult Index() {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        // GET: Propietario/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Propietario/Create
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
                    repositorio.Alta(propietario);
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

        [HttpPost]
        public JsonResult Buscar(string s) {
            var res = repositorio.ObtenerTodos().Where(x => x.Nombre.Contains(s) || x.Apellido.Contains(s));
            return new JsonResult(res);
        }

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id) {
            Propietario p=repositorio.ObtenerPorId(id);
            return View(p);
        }

        public ActionResult PassWord(int id) {
            Propietario p = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PassWord(int id, Propietario propietario,string ClaveNueva,string claveRepetida) {
            Propietario propi = null;
            try {
                propi = repositorio.ObtenerPorId(id);
                // verificar clave antigüa
                var pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave ?? "",
                        salt: System.Text.Encoding.ASCII.GetBytes("SALADA"),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                if (propi.Clave != pass || ClaveNueva!=claveRepetida) {
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
                    propietarios.ActualizarClave(id,nueva);
                    TempData["Id"] = "actualizó la contraseña";
                    return RedirectToAction(nameof(Index));
                }
                    
            } catch (Exception ex) {
                TempData["Error"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToAction("PassWord", new { id = id });
            }
        }


        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario) {
            try {
                repositorio.Modificacion(propietario);
                TempData["Id"] = "actualizó el propietario";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(propietario);
            }
        }

        // GET: Propietario/Delete/5
        public ActionResult Delete(int id) {
            Propietario uno = repositorio.ObtenerPorId(id);
            ViewBag.datos =uno.Nombre+" "+uno.Apellido+" con D.N.I.: "+uno.Dni;
            return View();
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario collection) {
            try {
                // TODO: Add delete logic here
                repositorio.Baja(id);
                TempData["Id"] = "eliminó el propietario";
                return RedirectToAction(nameof(Index));
                } catch (Exception ex) {
                    if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE")) {
                    Propietario uno = repositorio.ObtenerPorId(id);
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