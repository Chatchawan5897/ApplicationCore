using ApplicationCore.Models.db;
using ApplicationCore.ViewModels.Books;
using ApplicationCore.ViewModels.Customer;
using ApplicationCore.ViewModels.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System;
using System.Dynamic;
using static System.Reflection.Metadata.BlobBuilder;

namespace ApplicationCore.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly TestApplicationContext _db;

        public TransactionsController(TestApplicationContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var trni = from t in _db.Transactions
                       from b in _db.Books
                       from c in _db.Customers
                       where (t.Isbn == b.Isbn )
                       && (t.CusId == c.CusId)
                       select new TranIndexViewModel
                       {
                           Isbn=  t.Isbn,
                           CusId =  t.CusId,
                           Quatity = t.Quatity,
                           TotalPrice =t.TotalPrice,
                           TiTle =   b.TiTle,
                           CusName =    c.CusName
                       };

            if (trni == null)
            {
                return NotFound();
            }
            return View(await trni.ToListAsync());
        }

        public async Task<IActionResult> Detail(int id)
        {
            var Trand =await  (from t in _db.Transactions
                               from b in _db.Books
                               from c in _db.Customers
                        where (t.Isbn == id)
                        && (t.CusId == c.CusId)
                        && (t.Isbn == b.Isbn)
                        select new TranDetailViewModel
                        {
                            Isbn =  t.Isbn,
                            CusId = t.CusId,
                            Quatity = t.Quatity,
                            TotalPrice = t.TotalPrice,
                            TiTle =   b.TiTle,
                            CusName =  c.CusName

                        }).FirstOrDefaultAsync();

            if (Trand == null)
            {
                return NotFound();
            }

            return View(Trand);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Trane = await (from t in _db.Transactions
                               from b in _db.Books
                               from c in _db.Customers
                               where (t.Isbn == id)
                               && (t.CusId == c.CusId)
                               && (t.Isbn == b.Isbn)
                               select new  TranEditViewModel
                               {
                                   Isbn =  t.Isbn,
                                   CusId =     t.CusId,
                                   Quatity =   t.Quatity,
                                   TotalPrice =    t.TotalPrice,
                                   TiTle =     b.TiTle,
                                   Description =  b.Description,
                                   Price =  b.Price,
                                   CusName =  c.CusName,
                                   CusAddress =  c.CusAddress,
                                   CusEmail =   c.CusEmail
                                  
                               }).FirstOrDefaultAsync();

            if (Trane == null)
            {
                return NotFound();
            }

            ViewData["CustomerLists"] = new SelectList(_db.Customers, "CusId", "CusName");
            return View(Trane);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , Transaction data 
            , int txtcusid , int txtprice , int txtorder 
            )
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var sum = txtprice * txtorder;

                    data.Isbn = id;
                    data.CusId = txtcusid;
                    data.Quatity = txtorder;
                    data.TotalPrice = sum;

                    _db.Transactions.Update(data);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {


                    bool result = _db.Transactions.Any(p => p.Isbn == id);
                    if (result == false)
                    {
                        return NotFound();
                    };
                }
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }

    }
}
