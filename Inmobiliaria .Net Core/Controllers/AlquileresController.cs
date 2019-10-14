using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Inmobiliaria_.Net_Core.Controllers {
    public class AlquileresController : Controller {

        private readonly IRepositorio<Alquiler> repositorio;
        public AlquileresController(IRepositorio<Alquiler> repositorio, IRepositorioInmueble inmuebles, IRepositorio<Inquilino> inquilinos) {
            this.repositorio = repositorio;
            Inmuebles = inmuebles;
            Inquilinos = inquilinos;
        }

        public IRepositorioInmueble Inmuebles { get; set; }
        public IRepositorio<Inquilino> Inquilinos { get; set; }

        [Authorize]
        public ActionResult Index() {
            if (TempData.ContainsKey("Busqueda")) {
                var list = repositorio.Buscar((string)TempData["Busqueda"]);
                ViewBag.Id = "encontraron " + list.Count + " resultados";
                return View(list);
            } else {
                var lista = repositorio.ObtenerTodos();
                if (TempData.ContainsKey("Id"))
                    ViewBag.Id = TempData["Id"];
                return View(lista);
            }
        }
        [Authorize]
        public ActionResult Create() {
            ViewBag.propis = Inmuebles.ObtenerTodos();
            ViewBag.inquis = Inquilinos.ObtenerTodos();
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alquiler alquiler) {
            try {
                repositorio.Alta(alquiler);
                Inmuebles.ActualizarDisponible(alquiler.IdInmueble, 0);
                TempData["Id"] = "creó el alquiler";
                return RedirectToAction(nameof(Index));

            } catch (Exception ex) {
                ViewBag.propis = Inmuebles.ObtenerTodos();
                ViewBag.inquis = Inquilinos.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(alquiler);
            }
        }
        [Authorize]
        public ActionResult Edit(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.trato = uno.Inmueble.Propietario.Nombre + " " + uno.Inmueble.Propietario.Apellido + " y " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " en " + uno.Inmueble.Direccion;
            return View(uno);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Alquiler alquiler) {
            try {
                alquiler.Id = id;
                repositorio.Modificacion(alquiler);
                Inmuebles.ActualizarDisponible(alquiler.IdInmueble, 1);
                TempData["Id"] = "actualizó el alquiler";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(alquiler);
            }
        }
        [Authorize]
        public ActionResult Delete(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.trato = " el locador " + uno.Inmueble.Propietario.Nombre + " " + uno.Inmueble.Propietario.Apellido + " y el locatario " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " para el inmueble con dirección en " + uno.Inmueble.Direccion;
            return View(uno);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Alquiler collection) {
            try {
                repositorio.Baja(id);
                Inmuebles.ActualizarDisponible(collection.IdInmueble, 1);
                TempData["Id"] = "eliminó el alquiler";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE")) {
                    ViewBag.Error = "No se puede eliminar el alquiler ya que tiene pagos a su nombre";
                } else {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                }
                var uno = repositorio.ObtenerPorId(id);
                ViewBag.trato = " el locador " + uno.Inmueble.Propietario.Nombre + " " + uno.Inmueble.Propietario.Apellido + " y el locatario " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " para el inmueble con dirección en " + uno.Inmueble.Direccion;
                return View(collection);
            }
        }

    }
}
