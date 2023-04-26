using EntityFrameWork.Areas.Admin.View_Models;
using EntityFrameWork.Helpers;
using EntityFrameWork.Models;
using EntityFrameWork.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameWork.Areas.Admin.Controllers;

    [Area("Admin")]
public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int page=1,int take=2)
        {

            List<Product> products = await _productService.GetPaginatedDatas(page,take);

            List<ProductListVM> mappedDatas = GetMappedDatas(products);

            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);

            return View(paginatedDatas);
        }


    private List<ProductListVM> GetMappedDatas(List<Product>products)
    {
        List<ProductListVM> mappedDatas = new();

        foreach (var product in products)
        {

            ProductListVM productVm = new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Count = product.Count,
                Description = product.Description,
                CategoryName = product.Category.Name,
                MainImage = product.Images.Where(m => m.IsMain).FirstOrDefault()?.Image

            };

            mappedDatas.Add(productVm);
        }

        return mappedDatas;
    }


    private async Task<int> GetPageCountAsync(int take)
    {
        var product =await _productService.GetCountAsync();

        return (int)Math.Ceiling((decimal)product / take);
    }

    }

