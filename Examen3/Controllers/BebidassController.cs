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
    public class BebidassController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnviroment;

        public BebidassController(ApplicationDbContext context,IWebHostEnvironment hostEnviroment)
        {
            _context = context;
            _hostEnviroment = hostEnviroment;
        }

        // GET: Bebidass
        public async Task<IActionResult> Index()
        {
              return _context.Bebidas != null ? 
                          View(await _context.Bebidas.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Bebidas'  is null.");
        }

        // GET: Bebidass/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bebidas == null)
            {
                return NotFound();
            }

            var bebida = await _context.Bebidas
                .FirstOrDefaultAsync(m => m.id == id);
            if (bebida == null)
            {
                return NotFound();
            }

            return View(bebida);
        }

        // GET: Bebidass/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bebidass/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,tmaño,precio,urlImagen")] Bebida bebida)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if(archivos.Count()>0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\bebidas\");
                    var extencion = Path.GetExtension(archivos[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(subidas,nombreArchivo+extencion), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }
                    bebida.urlImagen = @"imagenes\bebidas\" + nombreArchivo + extencion;
                }
                _context.Add(bebida);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bebida);
        }

        // GET: Bebidass/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bebidas == null)
            {
                return NotFound();
            }

            var bebida = await _context.Bebidas.FindAsync(id);
            if (bebida == null)
            {
                return NotFound();
            }
            return View(bebida);
        }

        // POST: Bebidass/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,tmaño,precio,urlImagen")] Bebida bebida)
        {
            if (id != bebida.id)
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
                        Bebida? bebidaBD = await _context.Bebidas.FindAsync(id);
                        if (bebidaBD!=null && bebidaBD.urlImagen!=null)
                        {
                            var rutaImagenActual = Path.Combine(rutaPrincipal, bebidaBD.urlImagen);
                            if (System.IO.File.Exists(rutaImagenActual))
                            {
                                System.IO.File.Delete(rutaImagenActual);
                            }
                            
                        }
                        _context.Entry(bebidaBD).State = EntityState.Detached;
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\bebidas\");
                        var extencion = Path.GetExtension(archivos[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(subidas,nombreArchivo+extencion), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStream);
                        }
                        bebida.urlImagen = @"imagenes\bebidas\" + nombreArchivo + extencion;
                        _context.Entry(bebida).State = EntityState.Modified;
                    }
                    _context.Update(bebida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BebidaExists(bebida.id))
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
            return View(bebida);
        }

        // GET: Bebidass/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bebidas == null)
            {
                return NotFound();
            }

            var bebida = await _context.Bebidas
                .FirstOrDefaultAsync(m => m.id == id);
            if (bebida == null)
            {
                return NotFound();
            }

            return View(bebida);
        }

        // POST: Bebidass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bebidas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bebidas'  is null.");
            }
            var bebida = await _context.Bebidas.FindAsync(id);
            if (bebida != null)
            {
                _context.Bebidas.Remove(bebida);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BebidaExists(int id)
        {
          return (_context.Bebidas?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
