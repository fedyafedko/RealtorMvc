using Microsoft.AspNetCore.Mvc;
using RealtorMVC.BLL.Interfaces;
using RealtorMVC.BLL.Service.Auth;
using RealtorMVC.Extentions;
using RealtorMVC.Models.Apartment;

namespace RealtorMVC.Controllers
{
    public class ApartmentController : Controller
    {
        public readonly IApartmentService _apartmentService;

        public ApartmentController(IApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        [HttpGet]
        public IActionResult CreateApartment()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateApartment()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddApartmentImages()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromBody] CreateApartmentModel model)
        {
            var userId = HttpContext.GetUserId();
            var result = await _apartmentService.AddApartmentAsync(userId, model);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _apartmentService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllForRealtor()
        {
            var userId = HttpContext.GetUserId();
            var result = await _apartmentService.GetAllForRealtor(userId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await _apartmentService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(UploadApartmentImageRequest request)
        {
            var userId = HttpContext.GetUserId();
            var result = await _apartmentService.UploadImagesAsync(userId, request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var result = await _apartmentService.DeleteApartmentAsync(id);
            return Ok(result);
        }

        [HttpDelete]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteApartmentImageRequest request)
        {
            var userId = HttpContext.GetUserId();
            var result = await _apartmentService.DeleteImagesAsync(userId, request);
            return result ? Ok() : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateApartmentModel model)
        {
            var result = await _apartmentService.UpdateApartmentAsync(model.Id, model);
            return Ok(result);
        }
    }
}
