using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.Models
{
    public class PropertyFavorite
    {
        public Guid PropertyId { get; set; }

        [ForeignKey(nameof(PropertyId))]
        public Property Property { get; set; } = null!;

        public Guid FavoriteId { get; set; }

        [ForeignKey(nameof(FavoriteId))]
        public Favorite Favorite { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
