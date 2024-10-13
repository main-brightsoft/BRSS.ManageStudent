using System.Text.Json.Serialization;
using BRSS.ManageStudent.Domain.Entity.Base;

namespace BRSS.ManageStudent.Domain.Entity
{
    public class Student: IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public DateTime? DayOfBirth { get; set; } = null;

        public string? PhoneNumber { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;
        [JsonIgnore]
        public List<Class> Classes { get; set; } = new List<Class>();
        
        public Guid GetId()
        {
            return Id;
        }

        public void SetId(Guid id)
        {
            Id = id;
        }
    }
}