using System.Collections.Generic;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class CredentialType //: EntityBase, ICredentialType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Position { get; set; }

        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
