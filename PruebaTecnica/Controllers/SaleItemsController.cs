using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models;

namespace PruebaTecnica.Controllers
{
    public class SaleItemsController : Controller
    {
        private readonly PruebaTecnicaContext _context;

        public SaleItemsController(PruebaTecnicaContext context)
        {
            _context = context;
        }

        // GET: SaleItems
        public async Task<IActionResult> Index(string buscar)
        {
            var saleItems = from venta in _context.SaleItem select venta;

            if (!String.IsNullOrEmpty(buscar))
            {
                int id;
                if (int.TryParse(buscar, out id))
                {
                    saleItems = saleItems.Where(s => s.Id == id);
                }
            }

            return View(await saleItems.ToListAsync());
        }


        // GET: SaleItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SaleItem == null)
            {
                return NotFound();
            }

            var saleItem = await _context.SaleItem
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleItem == null)
            {
                return NotFound();
            }

            return View(saleItem);
        }

        // GET: SaleItems/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

        // POST: SaleItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Quantity,Subtotal")] SaleItem saleItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saleItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", saleItem.ProductId);
            return View(saleItem);
        }

        // GET: SaleItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SaleItem == null)
            {
                return NotFound();
            }

            var saleItem = await _context.SaleItem.FindAsync(id);
            if (saleItem == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", saleItem.ProductId);
            return View(saleItem);
        }

        // POST: SaleItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Quantity,Subtotal")] SaleItem saleItem)
        {
            if (id != saleItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleItemExists(saleItem.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", saleItem.ProductId);
            return View(saleItem);
        }

        // GET: SaleItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaleItem == null)
            {
                return NotFound();
            }

            var saleItem = await _context.SaleItem
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleItem == null)
            {
                return NotFound();
            }

            return View(saleItem);
        }

        // POST: SaleItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SaleItem == null)
            {
                return Problem("Entity set 'PruebaTecnicaContext.SaleItem'  is null.");
            }
            var saleItem = await _context.SaleItem.FindAsync(id);
            if (saleItem != null)
            {
                _context.SaleItem.Remove(saleItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleItemExists(int id)
        {
          return (_context.SaleItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
