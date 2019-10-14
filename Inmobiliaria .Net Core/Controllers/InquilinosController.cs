using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Inmobiliaria_.Net_Core.Controllers {
    public class InquilinosController : Controller {
        private readonly IRepositorio<Inquilino> repositorio;

        public InquilinosController(IRepositorio<Inquilino> repositorio) {
            this.repositorio = repositorio;
        }

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
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino) {
            repositorio.Alta(inquilino);
            TempData["Id"] = "creó el inquilino";
            return RedirectToAction(nameof(Index));

        }
        [Authorize]
        public ActionResult Edit(int id) {
            var uno = repositorio.ObtenerPorId(id);
            return View(uno);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino collection) {
            try {
                collection.Id = id;
                repositorio.Modificacion(collection);
                TempData["Id"] = "atualizó el inquilino";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(collection);
            }
        }
        [Authorize]
        public ActionResult Delete(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.persona = uno.Nombre + " " + uno.Apellido;
            return View(uno);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino collection) {
            try {
                repositorio.Baja(id);
                TempData["Id"] = "eliminó el inquilino";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE")) {
                    var uno = repositorio.ObtenerPorId(id);
                    ViewBag.persona = uno.Nombre + " " + uno.Apellido;
                    ViewBag.Error = "No se puede eliminar el inquilino ya que tiene contratos a su nombre";
                } else {
                    ViewBag.Error = ex.Message;
                    ViewBag.StackTrate = ex.StackTrace;
                }
                return View(collection);
            }
        }
    }
}