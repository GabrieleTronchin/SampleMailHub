using System.ComponentModel.DataAnnotations;

namespace Mail.Hub.Domain.Models
{

    public class SenderMailOptions
    {
        [Required]
        public string Server { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string SenderMail { get; set; }

        [Required]
        public string SenderPassword { get; set; }

        [Required]
        public string SenderUsername { get; set; }
    }
}
