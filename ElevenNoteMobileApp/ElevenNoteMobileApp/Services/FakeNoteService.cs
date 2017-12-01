using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevenNote.Models;
using ElevenNoteMobileApp.Contracts;
using ElevenNoteMobileApp.Services;

namespace ElevenNoteMobileApp.Services
{
    internal class FakeNoteService : INoteService
    {
        private List<NoteDetail> Notes { get; set; } = new List<NoteDetail>();

        public async Task<bool> Login(string username, string password)
        {
            return true;
        }

        public async Task<List<NoteListItem>> GetAll()
        {
            return Notes.Select(note => new NoteListItem
            {
                NoteId = note.NoteId,
                Content = note.Content,
                CreatedUtc = note.CreatedUtc,
                IsStarred = false,
                Title = note.Title
            }).OrderBy(o => o.CreatedUtc).ToList();
        }

        public async Task<NoteDetail> GetById(int noteId)
        {
            return Notes.FirstOrDefault(note => note.NoteId == noteId);
        }

        public async Task<bool> AddNew(NoteCreate note)
        {
            if (note.Title == string.Empty) return false;

            Notes.Add(new NoteDetail
            {
                NoteId = Notes.Any() ? Notes.Max(n => n.NoteId) + 1 : 1,
                Content = note.Content,
                Title = note.Title,
                CreatedUtc = DateTimeOffset.UtcNow
            });

            return true;
        }

        public async Task<bool> Update(NoteEdit note)
        {
            var noteToUpdate = await GetById(note.NoteId);

            if (noteToUpdate == null) return false;

            noteToUpdate.Title = note.Title;
            noteToUpdate.Content = note.Content;
            noteToUpdate.ModifiedUtc = DateTimeOffset.UtcNow;

            return true;
        }

        public async Task<bool> Delete(int noteId)
        {
            var noteToUpdate = await GetById(noteId);

            if (noteToUpdate == null) return false;

            Notes.Remove(noteToUpdate);
            return true;
        }
    }
}
