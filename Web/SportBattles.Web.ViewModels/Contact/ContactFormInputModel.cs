namespace SportBattles.Web.ViewModels.Contact
{
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Web.Infrastructure;

    public class ContactFormInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your name.")]
        [Display(Name = "Your name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter subject ot message.")]
        [StringLength(100, ErrorMessage = "Subject must be not less than {2} and not longer than {1} characters.", MinimumLength = 5)]
        [Display(Name = "Subject of message")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the content of the message.")]
        [StringLength(10000, ErrorMessage = "The message must be at least {2} characters.", MinimumLength = 20)]
        [Display(Name = "Content of the message")]
        public string Content { get; set; }

        ////[GoogleReCaptchaValidation]
        ////public string RecaptchaValue { get; set; }
    }
}
