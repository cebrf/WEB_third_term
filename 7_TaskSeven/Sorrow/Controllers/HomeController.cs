using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sorrow.Data;
using Sorrow.Models;

namespace Sorrow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ItemContext _context;

        public HomeController(ItemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Item);
        }
    }
}
