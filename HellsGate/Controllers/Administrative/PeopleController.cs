using Microsoft.AspNetCore.Mvc;

namespace HellsGate.Controllers.Administrative
{
    public class PeopleController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult ShowGrid() => View();

        public IActionResult LoadData() => View();

        public IActionResult Edit() => View();
    }
}