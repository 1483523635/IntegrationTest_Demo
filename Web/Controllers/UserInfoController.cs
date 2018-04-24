using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly UserInfoContext _context;

        public UserInfoController(UserInfoContext context)
        {
            _context = context;
        }

        // GET: UserInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserInfoDto.ToListAsync());
        }

        // GET: UserInfo/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfoDto = await _context.UserInfoDto
                .SingleOrDefaultAsync(m => m.Id == id);
            if (userInfoDto == null)
            {
                return NotFound();
            }

            return View(userInfoDto);
        }

        // GET: UserInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreateTime")] UserInfoDto userInfoDto)
        {
            if (ModelState.IsValid)
            {
                userInfoDto.Id = Guid.NewGuid();
                _context.Add(userInfoDto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInfoDto);
        }

        // GET: UserInfo/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfoDto = await _context.UserInfoDto.SingleOrDefaultAsync(m => m.Id == id);
            if (userInfoDto == null)
            {
                return NotFound();
            }
            return View(userInfoDto);
        }

        // POST: UserInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,CreateTime")] UserInfoDto userInfoDto)
        {
            if (id != userInfoDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfoDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoDtoExists(userInfoDto.Id))
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
            return View(userInfoDto);
        }

        // GET: UserInfo/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfoDto = await _context.UserInfoDto
                .SingleOrDefaultAsync(m => m.Id == id);
            if (userInfoDto == null)
            {
                return NotFound();
            }

            return View(userInfoDto);
        }

        // POST: UserInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userInfoDto = await _context.UserInfoDto.SingleOrDefaultAsync(m => m.Id == id);
            _context.UserInfoDto.Remove(userInfoDto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoDtoExists(Guid id)
        {
            return _context.UserInfoDto.Any(e => e.Id == id);
        }
    }
}
