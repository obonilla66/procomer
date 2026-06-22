
using System.Net.Http.Json;
using InvestorScheduling.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvestorScheduling.Web.Controllers;

public class InversoresController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public InversoresController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("CatalogApi");
        var inversores = await client.GetFromJsonAsync<List<InversorResponse>>("/api/inversores") ?? [];
        return View(inversores);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CrearInversorRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CrearInversorRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        if (request.CodigosIdioma.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Debe seleccionar al menos un idioma.");
            return View(request);
        }

        var client = _httpClientFactory.CreateClient("CatalogApi");
        var response = await client.PostAsJsonAsync("/api/inversores", request);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }
}
