using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElevenNoteMobileApp.Models
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
