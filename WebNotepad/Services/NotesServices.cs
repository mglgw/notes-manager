using Intro;
using Microsoft.IdentityFramework;
using WebNotepad.Models;

namespace WebNotepad.Services;

public class NotesServices
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
    public NotesServices(DataBaseContext DataBase, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
    {
        _dataBaseContext = DataBase;
        _userManager = userManager;
    }
    public Note CreateNote(string title, string content,string userId)
    {
        var tempnote = new Note();
        tempnote.CreatedDate = DateTime.UtcNow;
        tempnote.ModifiedDate = tempnote.CreatedDate;
        tempnote.Title = title;
        tempnote.Content = content;
        tempnote.CreatedBy = userId;
        _dataBaseContext.Notes.Add(tempnote);
        _dataBaseContext.SaveChanges();
        return tempnote;
    }
    public void EditNote(int tempid, string newtitle, string newcontent, string currentUserId)
    {
        var tempnote = _dataBaseContext.Notes
            .Where(note => tempid == note.Id && note.CreatedBy == currentUserId)
            .FirstOrDefault();
        if (tempnote == null)
        {
            throw new Exception("Note does not exist or was deleted");
        }
        tempnote.Title = newtitle;
        tempnote.Content = newcontent;
        tempnote.ModifiedDate = DateTime.UtcNow;
        _dataBaseContext.SaveChanges();
    }
    public void DeleteNote(int tempid, string currentUserId)
    {
        var tempnote = _dataBaseContext.Notes
            .Where(note => tempid ==note.Id && currentUserId == note.CreatedBy)
            .FirstOrDefault();
        if (tempnote == null)
        {
            throw new Exception("Chosen note does not exist!");
        }
        _dataBaseContext.Notes.Remove(tempnote);
        _dataBaseContext.SaveChanges();
    }
    public Note? GetSpecNote(int tempid)
    {
        var tempnote = _dataBaseContext.Notes
            .Where(note => tempid == note.Id)
            .FirstOrDefault();
        if (tempnote == null)
        {
            throw new Exception("Selected note does not exist!");
        }
        return tempnote;
    }
    public List<Note> GetAllNotes( string userId )
    {
        var tempNotes = _dataBaseContext.Notes
                .Where(note => userId == note.CreatedBy)
                .ToList();
        return tempNotes;
    }
}