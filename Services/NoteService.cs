using WebNotepad.Models;

namespace WebNotepad.Services;

public class NoteService
{
    private readonly DataBaseContext _dbContextContext;
    public NoteService(DataBaseContext dbContext)
    {
        _dbContextContext = dbContext;
    }
    public Note CreateNote(string title, string content, string userId)
    {
        var note = new Note
        {
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            Title = title,
            Content = content,
            CreatedBy = userId
        };
        _dbContextContext.Notes.Add(note);
        _dbContextContext.SaveChanges();
        return note;
    }
    public void EditNote(int id, string newTitle, string newContent, string currentUserId)
    {
        var note = _dbContextContext.Notes
            .FirstOrDefault(note => id == note.Id && note.CreatedBy == currentUserId);
        if (note == null)
        {
            throw new Exception("Note does not exist or was deleted");
        }
        note.Title = newTitle;
        note.Content = newContent;
        note.ModifiedDate = DateTime.UtcNow;
        _dbContextContext.SaveChanges();
    }
    public void DeleteNote(int id, string currentUserId)
    {
        var note = _dbContextContext.Notes
            .FirstOrDefault(note => id == note.Id && currentUserId == note.CreatedBy);
        if (note == null)
        {
            throw new Exception("Chosen note does not exist!");
        }
        _dbContextContext.Notes.Remove(note);
        _dbContextContext.SaveChanges();
    }
    public Note? GetById(int id)
    {
        var note = _dbContextContext.Notes
            .FirstOrDefault(note => id == note.Id);
        if (note == null)
        {
            throw new Exception("Selected note does not exist!");
        }
        return note;
    }
    public List<Note> GetAllByUserId(string userId)
    {
        var notes = _dbContextContext.Notes
            .Where(note => userId == note.CreatedBy)
            .ToList();
        return notes;
    }
}