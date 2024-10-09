using BRSS.ManageStudent.Domain.Entity.Base;

namespace BRSS.ManageStudent.Domain.Entity
{
    public class Class: IEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string? ClassName { get; set; }
        
        public ICollection<Student> Students { get; set; } = new List<Student>();
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