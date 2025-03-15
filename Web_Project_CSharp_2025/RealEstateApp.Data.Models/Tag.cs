using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Id = Guid.NewGuid();   
            this.TagProperties = new HashSet<PropertyTag>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int? Importance { get; set; }

        [InverseProperty(nameof(PropertyTag.Tag))]
        public virtual ICollection<PropertyTag>? TagProperties { get; set; }
    }
}
