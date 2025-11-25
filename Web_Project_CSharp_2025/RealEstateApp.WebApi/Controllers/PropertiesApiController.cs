using RealEstateApp.Web.ViewModels.Property;
using RealEstateApp.Data.DataServices.Contracts;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesApiController : ControllerBase
    {

        private readonly IValidationService validationService;
        private readonly IPropertyService propertyService;

        public PropertiesApiController(IValidationService valService, IPropertyService propService)
        {
            this.validationService = valService;
            this.propertyService = propService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("details")]
        public async Task<ActionResult<PropertyViewModel>> GetPropertyDetails(string? propId)
        {
            bool isValidGuid = this.validationService.IsValidGuid(propId, out Guid validId);

            if (isValidGuid == false)
            {
                return this.BadRequest();
            }

            PropertyViewModel? property = await this.propertyService.GetPropertyDetailsByIdAsync(validId);

            if (property == null)
            {
                return this.BadRequest(); 
            }

            return this.Ok(property);

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("properties")]
        public async Task<ActionResult<IEnumerable<PropertyViewModel>>> GetAllProperties()
        {
            IEnumerable<PropertyViewModel>? properties = await this.propertyService.IndexGetAllPropertiesAsync();

            if (properties == null)
            {
                return this.BadRequest();
            }

            return this.Ok(properties);

        }
    }
}
