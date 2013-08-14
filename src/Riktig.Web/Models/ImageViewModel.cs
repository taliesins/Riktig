namespace Riktig.Web.Models
{
    using System.ComponentModel.DataAnnotations;


    public class ImageViewModel
    {
        [Required]
        public string SourceAddress { get; set; }
    }
}