using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElevenNoteMobileApp.Models
{
    public class NoteListItem
    {
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }

        public override string ToString() => $"[{NoteId}] {Title}";
    }
}
