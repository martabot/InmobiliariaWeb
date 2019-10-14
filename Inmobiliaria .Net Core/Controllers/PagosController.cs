using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Inmobiliaria_.Net_Core.Controllers {
    public class PagosController : Controller {

        private readonly IRepositorio<Pago> repositorio;
        private readonly IRepositorioPago pagos;
        public PagosController(IRepositorio<Pago> repositorio, IRepositorioPago pagos, IRepositorio<Alquiler> alquiler) {
            this.repositorio = repositorio;
            Alquileres = alquiler;
            this.pagos = pagos;
        }

        public IRepositorio<Alquiler> Alquileres { get; set; }

        [Authorize]
        public ActionResult Index(int id) {
            var lista = pagos.ObtenerPorAlquiler(id);
            ViewBag.alquiler = Alquileres.ObtenerPorId(id);
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }
        [Authorize]
        public ActionResult Create(int id) {
            Alquiler uno = Alquileres.ObtenerPorId(id);
            ViewBag.ultimo = pagos.maxPago(id);
            ViewBag.alquiler = uno;
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Pago pago) {
            try {
                repositorio.Alta(pago);
                TempData["Id"] = "efectuó el pago";
                return RedirectToAction(nameof(Index), new { id = id });

            } catch (Exception ex) {
                ViewBag.alquileres = Alquileres.ObtenerPorId(id);
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }
        [Authorize]
        public ActionResult Edit(int id) {
            //var uno = repositorio.ObtenerPorId(id);
            //ViewBag.trato = uno.Inmueble.Propietario.Nombre + " " + uno.Inmueble.Propietario.Apellido + " y " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " en " + uno.Inmueble.Direccion;
            return null;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pago) {
            try {
                return RedirectToAction(nameof(Index), new { id = id });
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }
        [Authorize]
        public ActionResult Delete(int id) {
            var uno = repositorio.ObtenerPorId(id);
            var dos = Alquileres.ObtenerPorId(uno.Alquiler.Id);
            ViewBag.alquiler = dos;
            ViewBag.trato = uno.NroPago + " para el alquiler " + uno.Alquiler.Id + " en el inmueble ubicado en " + dos.Inmueble.Direccion + " por $" + uno.Importe + "- para la fecha " + uno.Fecha;
            return View(uno);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago collection) {
            try {
                repositorio.Baja(id);
                TempData["Id"] = "eliminó el pago";
                return RedirectToAction(nameof(Index), new { id = collection.IdAlquiler });
            } catch (Exception ex) {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE")) {
                    ViewBag.Error = "No se puede eliminar el pago ya que tiene contratos a su nombre";
                } else {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                }
                return View(collection);
            }
        }

    }
}