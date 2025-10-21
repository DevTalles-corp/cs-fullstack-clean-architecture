using System;

namespace TechNotes.Domain.Notes;

public interface INoteRepository
{
  Task<List<Note>> GetAllNotesAsync();
  Task<List<Note>> GetNotesByUserAsync(string userId);
  Task<Note> CreateNoteAsync(Note note);
  Task<Note?> GetNoteByIdAsync(int id);
  Task<Note?> UpdateNoteAsync(Note note);
  Task<bool> DeleteNoteAsync(int id);
}
