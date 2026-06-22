
using System.Net.Http.Json;
using InvestorScheduling.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvestorScheduling.Web.Controllers;

public class AgendasController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AgendasController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("AgendaApi");
        var agendas = await client.GetFromJsonAsync<List<AgendaResumenResponse>>("/agendas") ?? [];
        return View(agendas);
    }

    [HttpGet]
    public async Task<IActionResult> Generate()
    {
        var model = await BuildGenerateViewModelAsync(new GenerarAgendaViewModel());
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Generate(GenerarAgendaViewModel model)
    {
        var agenda = _httpClientFactory.CreateClient("AgendaApi");

        var request = new GenerarAgendaRequest
        {
            IdInversor = model.IdInversor,
            Fecha = model.Fecha,
            DuracionReunionMinutos = model.DuracionReunionMinutos,
            CantidadReunionesMeta = model.CantidadReunionesMeta,
            IdsParticipantes = model.IdsParticipantes
        };

        var response = await agenda.PostAsJsonAsync("/agendas/generar", request);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = await response.Content.ReadAsStringAsync();
            return View(await BuildGenerateViewModelAsync(model));
        }

        var result = await response.Content.ReadFromJsonAsync<AgendaGeneradaResponse>();
        return View("Result", result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = _httpClientFactory.CreateClient("AgendaApi");
        var agenda = await client.GetFromJsonAsync<AgendaDetalleCompletaResponse>($"/agendas/{id}");

        if (agenda is null)
        {
            return NotFound();
        }

        return View(agenda);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var client = _httpClientFactory.CreateClient("AgendaApi");
        await client.DeleteAsync($"/agendas/{id}");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DownloadPdf(int id)
    {
        var client = _httpClientFactory.CreateClient("AgendaApi");
        var response = await client.GetAsync($"/agendas/{id}/pdf");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = await response.Content.ReadAsStringAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        var bytes = await response.Content.ReadAsByteArrayAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/pdf";
        return File(bytes, contentType, $"agenda-{id}.pdf");
    }

    private async Task<GenerarAgendaViewModel> BuildGenerateViewModelAsync(GenerarAgendaViewModel model)
    {
        var catalog = _httpClientFactory.CreateClient("CatalogApi");

        model.Inversores = await catalog.GetFromJsonAsync<List<InversorResponse>>("/api/inversores") ?? [];
        model.Participantes = await catalog.GetFromJsonAsync<List<ParticipanteResponse>>("/api/participantes") ?? [];

        return model;
    }
}
