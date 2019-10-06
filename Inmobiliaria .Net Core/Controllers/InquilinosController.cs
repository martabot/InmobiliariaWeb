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
        // GET: Inquilino
        public ActionResult Index() {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        // GET: Inquilino/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino) {
            repositorio.Alta(inquilino);
            TempData["Id"] = "creó el inquilino";
            return RedirectToAction(nameof(Index));

        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id) {
            var uno = repositorio.ObtenerPorId(id);
            return View(uno);
        }

        // POST: Inquilino/Edit/5
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

        // GET: Inquilino/Delete/5
        public ActionResult Delete(int id) {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.persona = uno.Nombre + " " + uno.Apellido;
            return View(uno);
        }

        // POST: Inquilino/Delete/5
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