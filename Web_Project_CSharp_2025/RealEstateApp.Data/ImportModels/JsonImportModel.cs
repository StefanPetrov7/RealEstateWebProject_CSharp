

namespace RealEstateApp.Data.ImportModels
{
    public class JsonImportModel
    {
        // Describing the Json file which are going to import into our model, Attributes can be used if the prop names are not matching!

        public string Url { get; set; } = null!;

        public int Size { get; set; }

        public int YardSize { get; set; }

        public byte? Floor { get; set; }

        public byte? TotalFloor { get; set; }

        public string District { get; set; } = null!;

        public int Year { get; set; }

        public string Type { get; set; } = null!;

        public string BuildingType { get; set; } = null!;

        public int Price { get; set; }

    }
}
