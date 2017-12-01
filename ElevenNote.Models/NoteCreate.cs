using System.ComponentModel.DataAnnotations;

namespace ElevenNote.Models
{
    public class NoteCreate
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public override string ToString() => Title;

    }
}
