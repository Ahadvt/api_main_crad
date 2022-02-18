using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            CategoryListDtos categoryList;

            using (HttpClient client = new HttpClient())
            {

                var respons = await client.GetAsync("https://localhost:44384/api/categories");
                var reponsStr = await respons.Content.ReadAsStringAsync();
                if (respons.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    categoryList = JsonConvert.DeserializeObject<CategoryListDtos>(reponsStr);
                    return View(categoryList);
                }
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryPostDtos postDtos)
        {
            List<CategoryPostDtos> list = new List<CategoryPostDtos>();
            string EndPoint = "https://localhost:44384/api/categories";
            using (HttpClient client = new HttpClient())
            {
                //StringContent content = new StringContent(JsonConvert.SerializeObject(postDtos), Encoding.UTF8, "application/json");
               
                byte[] bytes = null;
                if (postDtos.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        postDtos.ImageFile.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                }
                var ByteArrContent = new ByteArrayContent(bytes);
                ByteArrContent.Headers.ContentType = MediaTypeHeaderValue.Parse(postDtos.ImageFile.ContentType);
                var multiPartContent = new MultipartFormDataContent();
                multiPartContent.Add(ByteArrContent, "ImageFile", postDtos.ImageFile.FileName);
                multiPartContent.Add(new StringContent(JsonConvert.SerializeObject(postDtos.Name), Encoding.UTF8, "application/json"),"name");
                using (var Response = await client.PostAsync(EndPoint, multiPartContent))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("index", "home");
                    }

                    else
                    {
                        return BadRequest();

                    }
                }
            }
            return RedirectToAction("index","home");
            //return Ok(Response.StatusCode.ToString());
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
