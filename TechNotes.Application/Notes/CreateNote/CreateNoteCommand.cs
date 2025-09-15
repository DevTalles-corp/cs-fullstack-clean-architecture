using System;
using MediatR;
using TechNotes.Application.Abstractions.RequestHandling;
using TechNotes.Domain.Notes;

namespace TechNotes.Application.Notes.CreateNote;

public class CreateNoteCommand : ICommand<NoteResponse>
{
  public required string Title { get; set; }
  public string? Content { get; set; }
  public DateTime PublishedAt { get; set; } = DateTime.Now;
  public bool IsPublished { get; set; } = false;
}
