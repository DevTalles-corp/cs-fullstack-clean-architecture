using System;
using MediatR;
using TechNotes.Domain.Notes;

namespace TechNotes.Application.Notes.CreateNote;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Note>
{
  private readonly INoteRepository _noteRepository;

  public CreateNoteCommandHandler(INoteRepository noteRepository)
  {
    _noteRepository = noteRepository;
  }
  public Task<Note> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
  {
    var newNote = new Note
    {
      Title = request.Title,
      Content = request.Content,
      PublishedAt = request.PublishedAt,
      IsPublished = request.IsPublished
    };
    return _noteRepository.CreateNoteAsync(newNote);
  }
}
