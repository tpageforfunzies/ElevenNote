using System.Collections.Generic;
using System.Threading.Tasks;
using ElevenNote.Models;

namespace ElevenNoteMobileApp.Contracts
{
    /// <summary>
    /// Defines a CRUD and Authentication service for Notes.
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Attempts to log the user in.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> Login(string username, string password);

        /// <summary>
        /// Gets a list of all the user's notes.
        /// </summary>
        /// <returns></returns>
        Task<List<NoteListItem>> GetAll();

        /// <summary>
        /// Gets note details by ID.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        Task<NoteDetail> GetById(int noteId);

        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        Task<bool> AddNew(NoteCreate note);

        /// <summary>
        /// Updates the passed note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        Task<bool> Update(NoteEdit note);

        /// <summary>
        /// Deletes the passed note by ID.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        Task<bool> Delete(int noteId);

    }
}
