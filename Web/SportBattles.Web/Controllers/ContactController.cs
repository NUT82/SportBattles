namespace SportBattles.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using SportBattles.Common;
    using SportBattles.Services.Messaging;
    using SportBattles.Web.ViewModels.Contact;

    public class ContactController : BaseController
    {
        private const string RedirectedFromContactForm = "RedirectedFromContactForm";

        private readonly IEmailSender emailSender;

        public ContactController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [Authorize]
        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(ContactFormInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.emailSender.SendEmailAsync(
                this.User.FindFirstValue(ClaimTypes.Email),
                model.Name,
                GlobalConstants.SystemEmail,
                model.Title,
                model.Content);

            this.TempData[RedirectedFromContactForm] = true;

            return this.RedirectToAction("ThankYou");
        }

        [Authorize]
        public IActionResult ThankYou()
        {
            if (this.TempData[RedirectedFromContactForm] == null)
            {
                return this.NotFound();
            }

            return this.View();
        }
    }
}
