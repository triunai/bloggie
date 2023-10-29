namespace CodePulse.API.Models.Domain
{
  public class BlogImage
  {
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public string FileUrl { get; set; } // can store it in API, cloud storage sllution etc
    public string Title { get; set; }
    public DateTime DateCreated { get; set; }
  }
}
