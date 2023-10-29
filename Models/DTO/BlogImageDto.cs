namespace CodePulse.API.Models.DTO
{
  public class BlogImageDto
  {
    //public Guid Id { get; set; } // MAYBE REMOVE THIS LATER?!
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public string FileUrl { get; set; } // can store it in API, cloud storage sllution etc
    public string Title { get; set; }
    public DateTime DateCreated { get; set; }
  }
}
