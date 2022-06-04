using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Examen3.Data;
using Examen3.Models;

namespace Examen3.Controllers
{
    public class ComidasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnviroment;

        public ComidasController(ApplicationDbContext context,IWebHostEnvironment hostEnviroment)
        {
            _context = context;
            _hostEnviroment = hostEnviroment;
        }

        // GET: Comidas
        public async Task<IActionResult> Index()
        {
              return _context.Comidas != null ? 
                          View(await _context.Comidas.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Comidas'  is null.");
        }

        // GET: Comidas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comidas == null)
            {
                return NotFound();
            }

            var comida = await _context.Comidas
                .FirstOrDefaultAsync(m => m.id == id);
            if (comida == null)
            {
                return NotFound();
            }

            return View(comida);
        }

        // GET: Comidas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comidas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,precio,urlImagen")] Comida comida)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if(archivos.Count()>0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\comidas\");
                    var extencion = Path.GetExtension(archivos[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(subidas,nombreArchivo+extencion), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }
                    comida.urlImagen = @"imagenes\comidas\" + nombreArchivo + extencion;
                }
                _context.Add(comida);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comida);
        }

        // GET: Comidas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comidas == null)
            {
                return NotFound();
            }

            var comida = await _context.Comidas.FindAsync(id);
            if (comida == null)
            {
                return NotFound();
            }
            return View(comida);
        }

        // POST: Comidas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,precio,urlImagen")] Comida comida)
        {
            if (id != comida.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string rutaPrincipal = _hostEnviroment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;
                    if(archivos.Count()>0)
                    {
                        Comida? comidaBD = await _context.Comidas.FindAsync(id);
                        if (comidaBD!=null && comidaBD.urlImagen!=null)
                        {
                            var rutaImagenActual = Path.Combine(rutaPrincipal, comidaBD.urlImagen);
                            if (System.IO.File.Exists(rutaImagenActual))
                            {
                                System.IO.File.Delete(rutaImagenActual);
                            }
                            
                        }
                        _context.Entry(comidaBD).State = EntityState.Detached;
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\comidas\");
                        var extencion = Path.GetExtension(archivos[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(subidas,nombreArchivo+extencion), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStream);
                        }
                        comida.urlImagen = @"imagenes\comidas\" + nombreArchivo + extencion;
                        _context.Entry(comida).State = EntityState.Modified;
                    }
                    _context.Update(comida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComidaExists(comida.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comida);
        }

        // GET: Comidas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comidas == null)
            {
                return NotFound();
            }

            var comida = await _context.Comidas
                .FirstOrDefaultAsync(m => m.id == id);
            if (comida == null)
            {
                return NotFound();
            }

            return View(comida);
        }

        // POST: Comidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comidas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comidas'  is null.");
            }
            var comida = await _context.Comidas.FindAsync(id);
            if (comida != null)
            {
                _context.Comidas.Remove(comida);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComidaExists(int id)
        {
          return (_context.Comidas?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
