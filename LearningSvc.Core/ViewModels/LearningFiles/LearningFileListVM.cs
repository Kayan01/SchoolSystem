using LearningSvc.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearningSvc.Core.ViewModels.LearningFiles
{
    public class LearningFileListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public LearningFileType FileType { get; set; }
        public DateTime CreationDate { get; set; }
        public string FileURL { get; set; }

        public static implicit operator LearningFileListVM(Models.LearningFile model)
        {
            return model == null ? null : new LearningFileListVM
            {
                Id = model.Id,
                Name = model.File?.Name,
                ClassName = model.SchoolClass?.Name,
                CreationDate = model.CreationTime,
                FileType = model.FileType,
                FileURL = model.File?.Path,
                SubjectName = model.Subject?.Name,
                TeacherName = $"{model.Teacher?.FirstName} {model.Teacher?.LastName}",
            };
        }

        public static implicit operator LearningFileListVM(Models.Assignment model)
        {
            return model == null ? null : new LearningFileListVM
            {
                Id = model.Id,
                Name = model.Attachments?.FirstOrDefault().Name,
                ClassName = model.SchoolClass?.Name,
                CreationDate = model.CreationTime,
                FileType = LearningFileType.Assignment,
                FileURL = model.Attachments?.FirstOrDefault().Path,
                SubjectName = model.Subject?.Name,
                TeacherName = $"{model.Teacher?.FirstName} {model.Teacher?.LastName}",
            };
        }
    }
}
