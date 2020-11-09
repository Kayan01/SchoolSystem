using System.ComponentModel.DataAnnotations;

namespace Auth.Core.ViewModels.SchoolClass
{
    public class ClassVM
    {
        public long Id { get; internal set; }
        public string  Name { get; set; }
        public long SectionId { get; set; }
        public long ClassGroupId { get; set; }

        public static implicit operator ClassVM(Models.SchoolClass model)
        {
            return model == null ? null : new ClassVM
            {
                Id = model.Id,
                Name = model.Name + model.ClassArm,
                SectionId = model.SchoolSectionId,
            };
        }
    }

    public class AddClassVM
    {
        public long Id { get; internal set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public long SectionId { get; set; }
        [Required]
        public long ClassArmId { get; set; }
    }


}
