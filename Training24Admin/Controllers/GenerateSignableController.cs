using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;

namespace Training24Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateSignableController : ControllerBase
    {
        private IConverter _converter;

        public GenerateSignableController(IConverter converter)
        {
            _converter = converter;
        }

        //[HttpGet("PdfCreatorController")]
        //public IActionResult PdfCreatorController()
        //{
        //    try
        //    {
        //        var htmlToPdf = new IronPdf.HtmlToPdf();
        //        var html = TemplateGenerator.GetHTMLString();
        //        var pdf = htmlToPdf.RenderHtmlAsPdf(html);
        //        pdf.SaveAs(Path.Combine(Directory.GetCurrentDirectory(), "Receipt.Pdf"));
        //        return Ok("Successfully created PDF document.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

    }
}