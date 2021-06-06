using lab4.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab4.Controllers
{
    public class ControlsController : Controller
    {
        
        [HttpGet]
        public IActionResult TextBox() 
        { return View(); }
        [HttpPost]
        public IActionResult TextBox(TextInputModel model) 
        { return View(model); }

        [HttpGet]
        public IActionResult TextArea()
        { return View(); }
        [HttpPost]
        public IActionResult TextArea(TextInputModel model) 
        { return View(model); }

        [HttpGet]
        public IActionResult CheckBox() 
        { return View(); }
        [HttpPost]
        public IActionResult CheckBox(BoolInputModel model) 
        { return View(model); }

        [HttpGet]
        public IActionResult Radio()
        { return View(); }
        [HttpPost]
        public IActionResult Radio(OneSelectModel model)
        { return View(model); }

        [HttpGet]
        public IActionResult DropDownList()
        { return View(); }
        [HttpPost]
        public IActionResult DropDownList(OneSelectModel model)
        { return View(model); }
        [HttpGet]
        public IActionResult ListBox()
        { return View(); }
        [HttpPost]
        public IActionResult ListBox(OneSelectModel model)
        { return View(model); }
    }
}
