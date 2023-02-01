using ApplicationCore.Models.db;
using ApplicationCore.ViewModels.Books;
using ApplicationCore.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApplicationCore.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TestApplicationContext _db;

        public CustomersController(TestApplicationContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var csuv = from c in _db.Customers
                      select new CusIndexViewModel
                      {
                          CusId= c.CusId,
                          CusName= c.CusName,
                          CusAddress=  c.CusAddress,
                          CusEmail=c.CusEmail
                      };

            if (csuv == null)
            {
                return NotFound();
            }
            return View(await csuv.ToListAsync());
        }

        public async Task<IActionResult> Search(string txtCus)
        {
            if (string.IsNullOrEmpty(txtCus))
            {
                var csus = from c in _db.Customers
                           select new CusIndexViewModel
                           {
                               CusId = c.CusId,
                               CusName = c.CusName,
                               CusAddress = c.CusAddress,
                               CusEmail = c.CusEmail
                           };

                if (csus == null)
                {
                    return NotFound();
                }
                return View("Index", await csus.ToListAsync());
            }
            else
            {
                var csus = from c in _db.Customers
                           where (c.CusName.Contains(txtCus))
                           select new CusIndexViewModel
                           {
                               CusId = c.CusId,
                               CusName = c.CusName,
                               CusAddress = c.CusAddress,
                               CusEmail = c.CusEmail
                           };
                if (csus == null)
                {
                    return NotFound();
                }

                return View("Index", await csus.ToListAsync());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Customer data)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(data);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Cuse = await (from c in _db.Customers
                       where (c.CusId == id)
                       select new CusEditViewModel
                       {
                           CusId = c.CusId,
                           CusName = c.CusName,
                           CusAddress =  c.CusAddress,
                           CusEmail = c.CusEmail
                       }).FirstOrDefaultAsync();
                       
            if (Cuse == null)
            {
                return NotFound();
            }

            return View(Cuse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , CusEditViewModel data)
        {
            if (id != data.CusId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var Cuse = await (from c in _db.Customers
                               where (c.CusId == id)
                               select c).FirstOrDefaultAsync();

                    Cuse.CusName = data.CusName;
                    Cuse.CusAddress = data.CusAddress;
                    Cuse.CusEmail = data.CusEmail;

                    _db.Update(Cuse);
                    await _db.SaveChangesAsync();
                   
                }
                catch (DbUpdateConcurrencyException)
                {

                    bool result = _db.Customers.Any(p => p.CusId == id);
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
            var Cusd = await (from c in _db.Customers
                       where (c.CusId == id)
                       select c).FirstOrDefaultAsync();

            _db.Remove(Cusd);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Detail(int id)
        { 
            var cust = await( from c in _db.Customers
                       where (c.CusId == id)
                       select new CusDetailViewModel
                       {
                           CusId = c.CusId,
                           CusName =  c.CusName,
                           CusAddress = c.CusAddress,
                           CusEmail = c.CusEmail
                       }).FirstOrDefaultAsync();

            if (cust == null)
            {
                return NotFound();
            }

            return View(cust);
        }

    }
}
