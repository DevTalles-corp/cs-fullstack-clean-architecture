using System;
using TechNotes.Domain.Notes;

namespace TechNotes.Application.Notes;

public class NoteService : INoteService
{
  public List<Note> GetAllNotes()
  {
    return new List<Note>
    {
      new(){
        Id=1,
        Title="Primera nota",
        Content="Contenido de nuestra primera nota",
        IsPublished=true,
        PublishedAt=DateTime.UtcNow,
        CreatedAt =DateTime.UtcNow
      },
      new(){
        Id=2,
        Title="Segunda nota",
        Content="Contenido de nuestra segunda nota",
        IsPublished=false,
        PublishedAt=null,
        CreatedAt =DateTime.UtcNow
      },
    };
  }
}
