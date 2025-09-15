using System;
using MediatR;
using TechNotes.Application.Abstractions.RequestHandling;
using TechNotes.Domain.Notes;

namespace TechNotes.Application.Notes.GetNotes;

public class GetNotesQuery : IQuery<List<NoteResponse>>
{

}
