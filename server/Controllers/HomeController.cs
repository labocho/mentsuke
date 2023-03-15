using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers;

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

    public IActionResult Impose(IFormFile file)
    {
        var src = Path.GetTempFileName();
        var srcStream = new FileStream(src, FileMode.Create, FileAccess.ReadWrite);
        file.CopyTo(srcStream);
        srcStream.Close();

        var dest = Path.GetTempFileName();
        mentsuke.Imposer.Impose(src, dest);

        var destStream = new FileStream(dest, FileMode.Open, FileAccess.Read);
        return File(destStream, "application/pdf", file.FileName);
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

