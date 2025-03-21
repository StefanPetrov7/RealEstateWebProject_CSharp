﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.Models
{
    public class District
    {
        public District()
        {
            this.Id = Guid.NewGuid();   
            this.Properties = new HashSet<Property>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Property.District))]
        public virtual ICollection<Property>? Properties { get; set; }
    }
}
