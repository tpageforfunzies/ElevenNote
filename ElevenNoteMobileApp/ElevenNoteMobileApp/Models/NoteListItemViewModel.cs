namespace ElevenNoteMobileApp.Models
{
    /// <summary>
    /// Represents a note as displayed in a list.
    /// </summary>
    internal class NoteListItemViewModel
    {
        /// <summary>
        /// The note's ID on the server.
        /// </summary>
        public int NoteId { get; set; }

        /// <summary>
        /// The note's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The icon to use when displaying the note.
        /// </summary>
        public string StarImage { get; set; }
    }
}
