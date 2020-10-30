using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Media
{
    public class MediaListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileURL { get; set; }

        public static implicit operator MediaListVM(Models.Files.Media model)
        {
            return model == null ? null : new MediaListVM
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
