namespace SportBattles.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class SportController : AdministrationController
    {
        public IActionResult Add()
        {
            return this.View();
        }
    }
}
