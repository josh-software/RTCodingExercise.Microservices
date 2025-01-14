﻿using System.Diagnostics;
using DTOs;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(HomeService homeService, ILogger<HomeController> logger)
        {
            _homeService = homeService;
            _logger = logger;
        }

        [ResponseCache(Duration = 5)]
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, string sortBy = "Id", string sortDirection = "Asc")
        {
            var pageSize = 20;
            var plates = await _homeService.GetPlatesAsync(pageNumber, pageSize, sortBy, sortDirection);

            // Prepare ViewModel
            var viewModel = new HomeViewModel<PlateDto>
            {
                Items = plates.Items,
                SortBy = sortBy,
                SortDirection = sortDirection,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)plates.Total / plates.Limit)
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddPlate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlate(PlateDto plateDto)
        {
            if (ModelState.IsValid)
            {
                await _homeService.UpsertPlateAsync(plateDto);
                return RedirectToAction("");
            }

            return View(plateDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}