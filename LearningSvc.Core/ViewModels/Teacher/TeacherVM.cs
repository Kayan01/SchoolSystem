using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSvc.Core.ViewModels.Teacher
{
    public class TeacherVM
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public static implicit operator TeacherVM(Models.Teacher model)
        {
            return model == null ? null : new TeacherVM
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone
            };
        }
    }
}
