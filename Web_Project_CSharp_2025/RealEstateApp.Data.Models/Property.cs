using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RealEstateApp.Data.Models
{
    public class Property
    {
        public Property()
        {
            this.Id = Guid.NewGuid();
            this.PropertyTags = new HashSet<PropertyTag>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Size { get; set; }

     
        public int? YardSize { get; set; }

  
        public byte? Floor { get; set; }

      
        public byte? TotalFloors { get; set; }

        public DateTime DateAdded { get; set; }

        public int? Year { get; set; }

        public int? Price { get; set; }

        [Required]
        public Guid DistrictId { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District? District { get; set; }

        [Required]
        public Guid PropertyTypeId { get; set; }

        [ForeignKey(nameof(PropertyTypeId))]
        public virtual PropertyType? PropertyType { get; set; }

        [Required]
        public Guid BuildingTypeId { get; set; }

        [ForeignKey(nameof(BuildingTypeId))]
        public virtual BuildingType? BuildingType { get; set; }

        [InverseProperty(nameof(PropertyTag.Property))]
        public virtual ICollection<PropertyTag>? PropertyTags { get; set; }
    }
}
