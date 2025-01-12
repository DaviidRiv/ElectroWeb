﻿using ElectroWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;

namespace ElectroWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subir(IFormFile xmlFile)
        {
            if (xmlFile == null || xmlFile.Length == 0)
            {
                ViewBag.Message = "Seleccione un XML valido";
                return View("Index", new List<MaterialModel>());
            }

            List<MaterialModel> materials = new List<MaterialModel>();

            using (var stream = xmlFile.OpenReadStream())
            {
                XDocument xdoc = XDocument.Load(stream);

                var conceptoNodes = xdoc.Descendants("{http://www.sat.gob.mx/cfd/4}Concepto");

                foreach (var node in conceptoNodes)
                {
                    MaterialModel material = new MaterialModel
                    {
                        Descripcion = node.Attribute("Descripcion")?.Value,
                        ValorUnitario = decimal.Parse(node.Attribute("ValorUnitario")?.Value)
                    };

                    materials.Add(material);
                }
            }

            return View("Index", materials);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
