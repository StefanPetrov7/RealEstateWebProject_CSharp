namespace RealEstateApp.Web.ViewModels.Property
{
    public class PropertyViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string BuildingType { get; set; } = null!;

        public string DistrictName { get; set; } = null!;

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public int Size { get; set; }

        public int? YardSize { get; set; }

        public int? Year { get; set; }

        public int? Price { get; set; }

        public DateTime DateAdded { get; set; }

    }
}

