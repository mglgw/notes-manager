using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNotepad.Models;
using WebNotepad.Services;

namespace WebNotepad.Controllers;

[ApiController]
[Route("api/notes")]
public class NoteController : Controller
{
    private readonly NoteService _noteService;
    private readonly UserManager<User> _userManager;
    public NoteController(NoteService noteService, UserManager<User> userManager)
    {
        _noteService = noteService;
        _userManager = userManager;
    }

    [ActionName("Create Note")]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest newNoteRequest)
    {
        var newNote = _noteService.CreateNote(newNoteRequest.Title, newNoteRequest.Content, await GetCurrentUser());
        return Ok(newNote);
    }

    [ActionName("Edit Note")]
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] ModifyNote modifyNoteRequest)
    {
        _noteService.EditNote(modifyNoteRequest.Id, modifyNoteRequest.Title, modifyNoteRequest.Content,
            await GetCurrentUser());
        return Ok();
    }

    [ActionName("Delete Note")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _noteService.DeleteNote(id, await GetCurrentUser());
        return Ok();
    }

    [ActionName("Get All Notes")]
    [HttpGet]
    public async Task<IActionResult> GetNotes()
    {
        return Ok(_noteService.GetAllByUserId(await GetCurrentUser()));
    }

    [ActionName("Get Specific Note")]
    [HttpGet("{id}")]
    public IActionResult GetNote(int id)
    {
        return Ok(_noteService.GetById(id));
    }

    private async Task<string> GetCurrentUser()
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        return currentUser.Id;
    }
}