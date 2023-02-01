using ApplicationCore.Models.db;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.ViewModels.Books;
using ApplicationCore.ViewModels.Transaction;
using System;

namespace ApplicationCore.Controllers
{
    public class BooksController : Controller
    {
        private readonly TestApplicationContext _db;

        public BooksController(TestApplicationContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var bookv = from b in _db.Books
                        select new BookIndexViewmodel
                        {
                           Isbn= b.Isbn,
                            TiTle=b.TiTle,
                            Description=  b.Description,
                            Price= b.Price,
                        };

            if (bookv == null)
            {
                return NotFound();
            }

            return View(await bookv.ToListAsync());
        }

        public async Task<IActionResult> Search(int txtisbn)
        {
            if (string.IsNullOrEmpty(txtisbn.ToString()))
            {
                var bookv = from b in _db.Books
                            select new BookIndexViewmodel
                            {
                                Isbn = b.Isbn,
                                TiTle = b.TiTle,
                                Description = b.Description,
                                Price = b.Price,
                            };

                if (bookv == null)
                {
                    return NotFound();
                }

                return View("Index",await bookv.ToListAsync());
            }
           else
            {

                var bookv = from b in _db.Books
                            where (b.Isbn== txtisbn)
                            select new BookIndexViewmodel
                            {
                                Isbn = b.Isbn,
                                TiTle = b.TiTle,
                                Description = b.Description,
                                Price = b.Price,
                            };

                if (bookv == null)
                {
                    return NotFound();
                }

                return View("Index", await bookv.ToListAsync());
            }
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book data)
        {
            if (ModelState.IsValid)
            {

                _db.Books.Add(data);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var booke = await (from b in _db.Books
                               where (b.Isbn == id)
                               select new BookEditViewModel
                               {
                                   Isbn = b.Isbn,
                                   TiTle = b.TiTle,
                                   Description = b.Description,
                                   Price = b.Price
                               }).FirstOrDefaultAsync();

            if (booke == null)
            {
                return NotFound();
            }

            return View(booke);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , BookEditViewModel data)
        {
            if (id != data.Isbn)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var booke = await (from b in _db.Books
                                where (b.Isbn == id)
                                select b).FirstOrDefaultAsync();

                    booke.TiTle = data.TiTle;
                    booke.Description = data.Description;
                    booke.Price = data.Price;

                    _db.Update(booke);
                   await  _db.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {

                    bool result = _db.Books.Any(p => p.Isbn == id);
                    if (result == false)
                    {
                        return NotFound();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            var bookd = await(from b in _db.Books
                        where (b.Isbn == id)
                        select b).FirstOrDefaultAsync();

            _db.Remove(bookd);
           await   _db.SaveChangesAsync();

           return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            var bookd = await (from b in _db.Books
                               from c in _db.Customers
                               where (b.Isbn == id)
                               select new BookDetailViewModel
                               {
                                   Isbn=b.Isbn,
                                   TiTle=b.TiTle,
                                   Description=b.Description,
                                   Price= b.Price,
                                   CusId=c.CusId,
                                   CusName=c.CusName,
                                   CusAddress=c.CusAddress,
                                   CusEmail=c.CusEmail
                               }).FirstOrDefaultAsync();

            if (bookd == null)
            {
                return NotFound();
            }

            return View(bookd);
        }

        public async Task<IActionResult> Order(int id)
        {
            var Booko = await (from t in _db.Transactions
                               from b in _db.Books
                               from c in _db.Customers
                               where (b.Isbn == id)
                               select new BookOrderViewModel
                               {
                                  Isbn =  b.Isbn,
                                   TiTle =  b.TiTle,
                                   Description =  b.Description,
                                   Price =     b.Price,
                                   CusId = c.CusId,
                                   Quatity =  t.Quatity,
                                   TotalPrice =   t.TotalPrice


                               }).FirstOrDefaultAsync();

            if (Booko == null)
            {
                return NotFound();
            }

            ViewData["CustomerLists"] = new SelectList(_db.Customers, "CusId", "CusName");
            return View(Booko);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(Transaction data,
            int cusid, int txtorder , int txtprice, int id
            )
        {
            var sum = txtprice * txtorder;

            data.Isbn = id ;
            data.CusId = cusid;
            data.Quatity = txtorder;
            data.TotalPrice = sum;

            _db.Transactions.Add(data);
           await  _db.SaveChangesAsync();

            return RedirectToAction("index","Transactions");
        }

    }
}
