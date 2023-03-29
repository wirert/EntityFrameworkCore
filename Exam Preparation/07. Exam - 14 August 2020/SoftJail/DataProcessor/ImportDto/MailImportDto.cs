namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class MailImportDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        public string Sender { get; set; }

        [Required]
        [MaxLength(60)]
        [RegularExpression(@"^[\w\s]+ str.$")]
        public string Address { get; set; }
    }
}
