using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface ITagService
    {
        Task AddTag(string name, int? importance = null);   

        Task TagAllProperties();
         
    }
}
