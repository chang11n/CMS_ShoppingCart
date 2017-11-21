﻿using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            // Declare a list of models
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                // Init the list
                categoryVMList = db.Categories
                                    .ToArray()
                                    .OrderBy(x => x.Sorting)
                                    .Select(x => new CategoryVM(x))
                                    .ToList();
     
            }
                //Return the view with list
                return View(categoryVMList);
        }

        //POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string cateName)
        {
            // Declare id
            string id;

            using (Db db = new Db())
            {
                // Check that the category name is unique
                if (db.Categories.Any(x => x.Name == cateName))
                {
                    return "titletaken";
                }

                // Init DTO
                CategoryDTO dto = new CategoryDTO();

                // Add to DTO
                dto.Name = cateName;
                dto.Slug = cateName.Replace(" ", ".").ToLower();
                dto.Sorting = 100;

                // Save DTO
                db.Categories.Add(dto);
                db.SaveChanges();

                // Get the id 
                id = dto.Id.ToString();
            }
            // Return id
            return id;
        }


        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                // Set initial count
                int count = 1;

                // Declare PageDTO
                CategoryDTO dto;

                // Set sorting for each category
                foreach (var cateId in id)
                {
                    dto = db.Categories.Find(cateId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }

        // GET: Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                // Get the page
                CategoryDTO dto = db.Categories.Find(id);

                //Remove the page
                db.Categories.Remove(dto);

                //Save
                db.SaveChanges();

                //Redirect
                return RedirectToAction("Categories");
            }
        }

        [HttpPost]
        public string RenameCategory(string newCateName, int id)
        {
            using (Db db = new Db())
            {
                // Check category is unique
                if (db.Categories.Any(x => x.Name == newCateName))
                    return ("titletaken");

                // Get DTO
                CategoryDTO dto = db.Categories.Find(id);

                // Edit DTO
                dto.Name = newCateName;
                dto.Slug = newCateName.Replace(" ", "-").ToLower();

                // Save
                db.SaveChanges();   
            }
            // Return
            return "ok";
        }

        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            // Init model
            ProductVM model = new ProductVM();

            // Add select list of categories to model
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            // Return view with model
            return View(model);
        }

        // POST: Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }

            // Make sure product name is unique
            using (Db db = new Db())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }

            // Declare product id
            int id;

            // Init and save productDTO
            using (Db db = new Db())
            {
                ProductDTO product = new ProductDTO();

                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "-").ToLower();
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                CategoryDTO cateDTO = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.CategoryName = cateDTO.Name;

                db.Products.Add(product);
                db.SaveChanges();

                // Get the id 
                id = product.Id;
            }

            // Set TempData message
            TempData["SM"] = "You have added a product!";

            #region Upload Image
            // Create necessary directories
            // Check if a file was uploaded
            // Get file extensionSet original nad thumb image paths
            // Save original 
            // Create and save thumb
            #endregion

            // Redirect
            return View(model);
        }




    }
}