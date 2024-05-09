using System.ComponentModel.DataAnnotations;

namespace Mail.Hub.Domain.Models;

public class ReceiverMailOptions
{
    [Required]
    public string Server { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public bool UseSSL { get; set; }
}
