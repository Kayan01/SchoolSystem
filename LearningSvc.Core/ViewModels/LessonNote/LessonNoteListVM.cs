using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.LessonNote
{
    public class LessonNoteListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileURL { get; set; }

        public static implicit operator LessonNoteListVM(Models.Files.LessonNote model)
        {
            return model == null ? null : new LessonNoteListVM
            {
                Id = model.Id,
                Name = model.File?.Name,
                ClassName = model.SchoolClass?.Name,
                CreationDate = model.CreationTime,
                FileURL = model.File?.Path,
                SubjectName = model.Subject?.Name,
                TeacherName = $"{model.Teacher?.FirstName} {model.Teacher?.LastName}",
            };
        }

    }
}
