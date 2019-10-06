using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Inmobiliaria_.Net_Core.Controllers {
    public class AlquileresController : Controller {

        private readonly IRepositorio<Alquiler> repositorio;
        public AlquileresController(IRepositorio<Alquiler> repositorio, IRepositorio<Inmueble> inmuebles, IRepositorio<Inquilino> inquilinos) {
            this.repositorio = repositorio;
            Inmuebles = inmuebles;
            Inquilinos = inquilinos;
        }

        public IRepositorio<Inmueble> Inmuebles { get; set; }
        public IRepositorio<Inquilino> Inquilinos { get; set; }

        [Authorize]
        // GET:
        public ActionResult Index() {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        public ActionResult Create() {
            ViewBag.propis = Inmuebles.ObtenerTodos();
            ViewBag.inquis = Inquilinos.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alquiler alquiler) {
            try {
                repositorio.Alta(alquiler);
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
        public ActionResult Edit(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.trato = uno.Inmueble.Propietario.Nombre + " " + uno.Inmueble.Propietario.Apellido + " y " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " en " + uno.Inmueble.Direccion;
            return View(uno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Alquiler alquiler) {
            try {
                alquiler.Id = id;
                repositorio.Modificacion(alquiler);
                TempData["Id"] = "actualizó el alquiler";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(alquiler);
            }
        }

        public ActionResult Delete(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.trato = " el locador " + uno.Inmueble.Propietario.Nombre + " " + uno.Inmueble.Propietario.Apellido + " y el locatario " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " para el inmueble con dirección en " + uno.Inmueble.Direccion;
            return View(uno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Alquiler collection) {
            try {
                repositorio.Baja(id);
                TempData["Id"] = "eliminó el alquiler";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE")) {
                    ViewBag.trato = " el locador " + collection.Inmueble.Propietario.Nombre + " " + collection.Inmueble.Propietario.Apellido + " y el locatario " + collection.Inquilino.Nombre + " " + collection.Inquilino.Apellido + " para el inmueble con dirección en " + collection.Inmueble.Direccion;
                    ViewBag.Error = "No se puede eliminar el alquiler ya que tiene pagos a su nombre";
                } else {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                }
                return View(collection);
            }
        }

    }
}
