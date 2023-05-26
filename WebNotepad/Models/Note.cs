namespace WebNotepad.Models;

public class Note
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int  Id { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}