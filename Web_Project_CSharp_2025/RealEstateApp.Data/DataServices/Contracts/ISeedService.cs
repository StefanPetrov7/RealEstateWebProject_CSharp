using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface ISeedService
    {
        Task RunSeed();

        Task ImportProperties(string fileLocation);
    }
}
