using Auth.Core.Models.UserDetails;

namespace Auth.Core.ViewModels.Staff
{
    public class NextOfKinVM
    {
        public string NextKinFirstName { get; set; }
        public string NextKinLastName { get; set; }

        public string NextKinOtherName { get; set; }

        public string NextKinRelationship { get; set; }
        public string NextKinOccupation { get; set; }
        public string NextKinPhone { get; set; }
        public string NextKinCountry { get; set; }
        public string NextKinAddress { get; set; }
        public string NextKinState { get; set; }
        public string NextKinTown { get; set; }

        public static implicit operator NextOfKinVM(NextOfKin model)
        {
            return model == null ? null : new NextOfKinVM
            {
                NextKinAddress = model.Address,
                NextKinCountry = model.Country,
                NextKinFirstName = model.FirstName,
                NextKinLastName = model.LastName,
                NextKinOccupation = model.Occupation,
                NextKinOtherName = model.OtherName,
                NextKinPhone = model.Phone,
                NextKinRelationship = model.Relationship,
                NextKinState = model.State,
                NextKinTown = model.Town

            };
        }
    }
}
