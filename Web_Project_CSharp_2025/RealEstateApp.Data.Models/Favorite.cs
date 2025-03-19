using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.Models
{
    public class Favorite
    {
        public Favorite()
        {
            this.Id = Guid.NewGuid();   
            this.FavoriteProperties = new HashSet<PropertyFavorite>();
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int? Importance { get; set; }

        [InverseProperty(nameof(PropertyFavorite.Favorite))]
        public virtual ICollection<PropertyFavorite>? FavoriteProperties { get; set; }
    }
}
