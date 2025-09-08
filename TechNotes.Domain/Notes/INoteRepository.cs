using System;

namespace TechNotes.Domain.Notes;

public interface INoteRepository
{
  Task<List<Note>> GetAllNotesAsync();
  Task<Note> CreateNoteAsync(Note note);
  Task<Note?> GetNoteByIdAsync(int id);
}
