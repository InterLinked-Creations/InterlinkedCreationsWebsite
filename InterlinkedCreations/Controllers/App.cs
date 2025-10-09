using Microsoft.AspNetCore.Mvc;

namespace InterlinkedCreations.Controllers
{
    public class App : Controller
    {
        // Intro page - Show the Interlinked Creations intro animation and a start button
        public IActionResult Index()
        {
            return View();
        }

        // Home menu -  Show the main menu with the menu layout. This includes buttons to navigate to different sections of the app.
        public IActionResult Home()
        {
            return View();
        }
    }
}
