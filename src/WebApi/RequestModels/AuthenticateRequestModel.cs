using System.ComponentModel.DataAnnotations;

namespace WebApi.RequestModels;

public class AuthenticateRequestModel
{
    [Required] public string Username { get; set; }
    [Required] public int Code { get; set; }
}