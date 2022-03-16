using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using MYSQLStoreAPI.Models;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace MYSQLStoreAPI.Controllers
{   
    // [ApiExplorerSettings(IgnoreApi = true)] //ซ่อน path
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController:ControllerBase
    {
         private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context){
            _context = context;
        }

        // Read Categories
    [HttpGet]
    [Route("all")]
    public ActionResult<Category> GetAll()
    {
        var allCategory = _context.category.ToList();
        return Ok(allCategory);
    }

    [HttpGet]
    [Route("{id}")]

    public ActionResult GetById(int id) {
        
        
        var category_ById = _context.category.Where(c => c.Category_id == id);
        
        if(category_ById == null)
            return NotFound();

        return Ok(category_ById);
    }

    [HttpGet]
    [Route("generatePDF")]
    public ActionResult GeneratePDF(){
        var document = new PdfDocument();
       
        string htmlstring ="<!DOCTYPE html>";
        htmlstring = "<table style='width:800px; border:solid;border-width:1px;'><tr>";
        htmlstring += "<th style = 'width:10%;text-align:left;'> No. </th>";
        htmlstring += "<th style = 'width:30%;text-align:left;'> Name </th>";
        htmlstring += "<th style = 'width:30%;text-align:left;'> Status</th>";
        htmlstring += "<br>asd</br></tr>";
       
    
        var allCategory = _context.category.ToList();

       
        foreach(var c in allCategory){
            
            htmlstring += "<tr><td style = 'width:10%;text-align:left;'>" + c.Category_name +"</td>";
            htmlstring += "<td style = 'width:30%;text-align:left;'>" + c.Category_name +"</td>";
            htmlstring += "<td style = 'width:30%;text-align:left;'>" + c.Category_status  +"</td></tr>";
            
        }
       
          htmlstring += "</tr></table>";
          
        PdfGenerator.AddPdfPages(document,htmlstring,PdfSharpCore.PageSize.A4);
        
        
        Byte[] res = null;
        using (MemoryStream ms = new MemoryStream()){
            document.Save(ms);
            res = ms.ToArray();
        
        }
        return File(res, "application/pdf");
    }
    


    [HttpPost]
    public ActionResult Create(Category category){

        _context.category.Add(category);
        _context.SaveChanges();

        return Ok(category);
        
    }

    [HttpPut]
    public ActionResult Update(Category category){

        if(category == null)
            return NotFound();

        _context.Update(category);
        _context.SaveChanges();

        return Ok(category);
        
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public ActionResult Delete(int id){

        var category_delete = _context.category.Where(c=>c.Category_id == id).FirstOrDefault();

        if(category_delete == null)
            return NotFound();

        _context.Remove(category_delete);
        _context.SaveChanges();

        return NoContent();
        
    }

    
    }
}