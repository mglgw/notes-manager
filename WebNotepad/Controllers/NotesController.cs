
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNotepad.Models;
using WebNotepad.Services;

namespace WebNotepad.Controllers;
[ApiController]

public class NotesController : Controller
{
    private readonly NotesServices _notesServices;
    private readonly UserManager<User> _userManager;
    public NotesController(NotesServices nServies, UserManager<User> userManager)
    {
        _notesServices = nServies;
        _userManager = userManager;

    }
    [ActionName("Create Note")]
    [HttpPost("api/notes")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest newNote )
    {
       // var currentUser =  await _userManager.GetUserAsync(HttpContext.User);
      //  var currentUserId = currentUser.Id;
        var tempNewNote = _notesServices.CreateNote(newNote.Title, newNote.Content /*, currentUserId*/);
        return Ok(tempNewNote);
    }
    [ActionName("Edit Note")]
    [HttpPut("api/notes")]
    public IActionResult Edit([FromBody] ModifyNote modNote)
    {
        _notesServices.EditNote(modNote.Id, modNote.Title, modNote.Content);
        return Ok();
    }
    [ActionName("Delete Note")]
    [HttpDelete("api/notes/{id}")]
    public IActionResult Delete(int id)
    {
        _notesServices.DeleteNote(id);
        return Ok();
    }
    [ActionName("Get All Notes")]
    [HttpGet("api/notes")]
    public IActionResult GetNotes()
    {
        return Ok(_notesServices.GetAllNotes());
    }
    [ActionName("Get Specific Note")]
    [HttpGet("api/notes/{id}")]
    public IActionResult GetNote(int id)
    {
        return Ok(_notesServices.GetSpecNote(id));
    }
}