using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Inmobiliaria_.Net_Core.Controllers {
    public class InmueblesController : Controller {
        private readonly IRepositorio<Inmueble> repositorio;
        public InmueblesController(IRepositorio<Inmueble> repositorio, IRepositorio<Propietario> propietarios) {
            this.repositorio = repositorio;
            Propietarios = propietarios;
        }

        public IRepositorio<Propietario> Propietarios { get; set; }

        [Authorize]
        // GET:
        public ActionResult Index() {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        public ActionResult Create() {
            ViewBag.propis = Propietarios.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble) {
            try {
                repositorio.Alta(inmueble);
                TempData["Id"] = "creó el inmueble";
                return RedirectToAction(nameof(Index));

            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.propis = Propietarios.ObtenerTodos();
                return View(inmueble);
            }
        }
        public ActionResult Edit(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.nombre = uno.Propietario.Nombre + " " + uno.Propietario.Apellido;
            return View(uno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble) {
            try {
                inmueble.Id = id;
                repositorio.Modificacion(inmueble);
                TempData["Id"] = "actualizó el inmueble";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inmueble);
            }
        }

        public ActionResult Delete(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.lugar = uno.Propietario.Nombre + " " + uno.Propietario.Apellido+" en "+uno.Direccion;
            return View(uno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble collection) {
            try {
                repositorio.Baja(id);
                TempData["Id"] = "eliminó el inmueble";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE")) {
                    var uno = repositorio.ObtenerPorId(id);
                    ViewBag.lugar = uno.Propietario.Nombre + " " + uno.Propietario.Apellido + " en " + uno.Direccion;
                    ViewBag.Error = "No se puede eliminar el inmueble ya que tiene contratos a su nombre";
                } else {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                }
                return View(collection);
            }
        }

    }
}
