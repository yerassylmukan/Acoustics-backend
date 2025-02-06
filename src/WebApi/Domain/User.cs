namespace WebApi.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public string PictuireUrl { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string Role { get; set; }
}