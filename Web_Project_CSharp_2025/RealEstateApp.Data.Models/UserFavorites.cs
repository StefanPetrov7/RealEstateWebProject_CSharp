using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.Models
{
    public class UserFavorites
    {
        public Guid ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser User { get; set; } = null!;

        public Guid FavoriteId { get; set; }

        [ForeignKey(nameof(FavoriteId))]
        public Favorite Favorites { get; set; } = null!;
    }
}
