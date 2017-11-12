using GalaxyVibesPos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models.ViewBag;
namespace GalaxyVibesPos.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ItemSettings()
        //{
        //    //ViewBag.MainCategoryList = db.CategoryMain.ToList();
        //    //ViewBag.CategoryList = db.Category.ToList();
        //    //ViewBag.SubcategoryList = db.CategorySub.ToList();

        //    //List<CategoryMain> MainCategoryList = db.CategoryMain.ToList();
        //    //List<Category> CategoryList = db.Category.ToList();
        //    //ViewBag.MainCategoryList = new SelectList(MainCategoryList, "MainCategoryID", "MaincategoryName");
        //    //ViewBag.CategoryList = new SelectList(CategoryList, "CategoryID", "CategoryName");
        //    //ViewBag.MainCategoryList = MainCategoryList;

        //    var mainCategories = db.CategoryMain.ToList();
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    list.Add(new SelectListItem { Text = "Select Main Category", Value = "0" });
        //    foreach (var m in mainCategories)
        //    {
        //        list.Add(new SelectListItem { Text = m.MaincategoryName, Value = Convert.ToString(m.MainCategoryID) });

        //    }
        //    ViewBag.MainCategoryList = list;
        //    ViewBag.ProductList = db.ProductAdd.ToList();
        //    ViewBag.ProductUnit = GetUnit();

        //    return View();


        //}


        ////--------- Save Main Category --------- !

        //public JsonResult SaveMainCategoryInDatabase(CategoryMain model)
        //{


        //    string Msg = null;

        //    CategoryMain mainCategory = new CategoryMain();

        //    var mainCategoryName = db.CategoryMain.Where(c => c.MaincategoryName == model.MaincategoryName).FirstOrDefault();
        //    if (ModelState.IsValid)
        //    {
        //        if (mainCategoryName == null)
        //        {
        //            if (model.MainCategoryID > 0)
        //            {
        //                mainCategory = db.CategoryMain.SingleOrDefault(x => x.MainCategoryID == model.MainCategoryID);
        //                mainCategory.MaincategoryName = model.MaincategoryName;
        //                db.SaveChanges();
        //                Msg = "Item Update Successfully";
        //            }
        //            else
        //            {
        //                mainCategory.MaincategoryName = model.MaincategoryName;
        //                db.CategoryMain.Add(mainCategory);
        //                db.SaveChanges();
        //                Msg = "Main Category Save Successfully";

        //            }
        //        }
        //        else
        //        {
        //            Msg = "Item Name already Exists";
        //        }
        //    }
        //    return Json(Msg, JsonRequestBehavior.AllowGet);
        //}

        //// ---------   Save Category   --------- !

        //public JsonResult SaveCategoryInDatabase(Category model)
        //{


        //    string Msg = null;

        //    Category category = new Category();

        //    var CategoryName = db.Category.Where(c => c.CategoryName == model.CategoryName).FirstOrDefault();
        //    if (ModelState.IsValid)
        //    {
        //        if (CategoryName == null)
        //        {
        //            if (model.CategoryID > 0)
        //            {
        //                category = db.Category.SingleOrDefault(x => x.CategoryID == model.CategoryID);
        //                category.CategoryName = model.CategoryName;
        //                db.SaveChanges();

        //            }
        //            else
        //            {
        //                category.CategoryName = model.CategoryName;
        //                category.MainCategoryID = model.MainCategoryID;
        //                db.Category.Add(category);
        //                db.SaveChanges();
        //                Msg = "Main Category Save Successfully";

        //            }
        //        }
        //        else
        //        {
        //            Msg = "Item Name already Exists";
        //        }
        //    }
        //    return Json(Msg, JsonRequestBehavior.AllowGet);
        //}


        //// ---------   Save Sub Category   --------- !

        //public JsonResult SaveSubCategoryInDatabase(CategorySub model)
        //{


        //    string Msg = null;

        //    CategorySub subCategory = new CategorySub();

        //    var SubCategoryName = db.CategorySub.Where(c => c.SubCategoryName == model.SubCategoryName).FirstOrDefault();
        //    if (ModelState.IsValid)
        //    {
        //        if (SubCategoryName == null)
        //        {
        //            if (model.SubCategoryID > 0)
        //            {
        //                subCategory = db.CategorySub.SingleOrDefault(x => x.SubCategoryID == model.SubCategoryID);
        //                subCategory.SubCategoryName = model.SubCategoryName;
        //                db.SaveChanges();

        //            }
        //            else
        //            {
        //                subCategory.SubCategoryName = model.SubCategoryName;
        //                subCategory.MainCategoryID = model.MainCategoryID;
        //                subCategory.CategoryID = model.CategoryID;
        //                db.CategorySub.Add(subCategory);
        //                db.SaveChanges();
        //                Msg = "Sub Category Save Successfully";

        //            }
        //        }
        //        else
        //        {
        //            Msg = "Item Name already Exists";
        //        }
        //    }
        //    return Json(Msg, JsonRequestBehavior.AllowGet);
        //}


        //// ---------   Save Add Product    --------- !

        //public JsonResult SaveAddProductInDatabase(ProductAdd model)
        //{


        //    string Msg = null;

            
        //    ProductAdd addProduct = new ProductAdd();
        //    var addProductname = db.ProductAdd.Where(c => c.ProductName == model.ProductName).FirstOrDefault();
        //    if(ModelState.IsValid)
        //    {
        //        if(addProductname == null)
        //        {
        //            if(model.ProductID>0)
        //            {
        //                addProduct = db.ProductAdd.SingleOrDefault(x => x.ProductID == model.ProductID);
        //                addProduct.ProductName = model.ProductName;
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                addProduct.ProductName = model.ProductName;
        //                addProduct.MainCategoryID = model.MainCategoryID;
        //                addProduct.SubCategoryID = model.SubCategoryID;
        //                addProduct.CategoryID = model.CategoryID;
        //                db.ProductAdd.Add(addProduct);
        //                db.SaveChanges();
        //                Msg = "Product Add Successfully";

        //            }
        //        }
        //        else
        //        {
        //            Msg = "Product Already Exists";
        //        }
                
        //    }
        //    return Json(Msg, JsonRequestBehavior.AllowGet);

            
        //}




        //// --Get Category in Category DropDown When Select a Main Category From Main Category Dropdown-- !

        ////public JsonResult GetCategoryByMainCategory(int MaincategoryID)
        ////{
        ////    var categories = db.Category.Where(a => a.MainCategoryID == MaincategoryID).ToList();
        ////    List<SelectListItem> list = new List<SelectListItem>();
        ////    list.Add(new SelectListItem { Text = "--Select Country--", Value = "0" });
        ////    foreach(Category category in categories)
        ////    {
        ////        list.Add(new SelectListItem {Text = category.CategoryName, Value = Convert.ToString(category.CategoryID) });
        ////    }
        ////    return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        ////    //return Json(categories, JsonRequestBehavior.AllowGet);
        ////    //return Json(categories.Select(c => new
        ////    //    {
        ////    //        ID = c.CategoryID,
        ////    //        Name = c.CategoryName

        ////    //    }), JsonRequestBehavior.AllowGet);
        ////}


        ////private IList<Category> GetCategory(int MainCategoryID)
        ////{
        ////    return db.Category.Where(m => m.MainCategoryID == MainCategoryID).ToList();
        ////}


        ////--- 

        ////---Cascading Dropdown For Categories--- !

        ////[AcceptVerbs(HttpVerbs.Get)]
        ////public JsonResult GetCategoryByMainCategory(int MainCategoryID)
        ////{
        ////    //var CategoryList = this.GetCategory(MainCategoryID);
        ////    var CategoryList = db.Category.Where(m => m.MainCategoryID == MainCategoryID).ToList();
        ////    var Categories = CategoryList.Select(m => new SelectListItem()
        ////    {
        ////        Text = m.CategoryName,
        ////        Value = m.CategoryID.ToString(),
        ////    });

        ////    return Json(Categories, JsonRequestBehavior.AllowGet);
        ////}

        //public JsonResult GetCategoryByMainCategory(int MainCategoryID)
        //{
        //    var categories = db.Category.Where(m => m.MainCategoryID == MainCategoryID).ToList();

        //    List<SelectListItem> categoryList = new List<SelectListItem>();

        //    categoryList.Add(new SelectListItem { Text = "--Select Category--", Value = "0" });

        //    foreach (var m in categories)
        //    {
        //        categoryList.Add(new SelectListItem { Text = m.CategoryName, Value = Convert.ToString(m.CategoryID) });
        //    }

        //    return Json(new SelectList(categoryList, "Value", "Text", JsonRequestBehavior.AllowGet));


        //}


        ////---Cascading Dropdown For Add Product--- !


        //public JsonResult GetSubCategoryByCategory(int CategoryID)
        //{
        //    //var CategoryList = this.GetCategory(MainCategoryID);
        //    var categoryList = db.CategorySub.Where(m => m.CategoryID == CategoryID).ToList();
        //    var subCategories = categoryList.Select(m => new SelectListItem()
        //    {
        //        Text = m.SubCategoryName,
        //        Value = m.SubCategoryID.ToString(),
        //    });

        //    return Json(subCategories, JsonRequestBehavior.AllowGet);
        //}

        //---Cascading Dropdown For Product Details--- !


        //public JsonResult GetProductBySubCategory(int ProductID)
        //{

        //    var productList = db.ProductAdd.Where(m => m.ProductID == ProductID).ToList();
        //    var products = productList.Select(m => new SelectListItem()
        //        {
        //            Text = m.ProductName,
        //            Value = m.ProductID.ToString(),
        //        });
        //    return Json(products, JsonRequestBehavior.AllowGet);

        //}

        //private List<ProductUnit> GetUnit()
        //{
        //    List<ProductUnit> productUnit = new List<ProductUnit>
        //    {
        //        new ProductUnit{UnitID = 1, UnitSize = "PCS"},
        //        new ProductUnit{UnitID = 2, UnitSize = "Carton(12)"},
        //        new ProductUnit{UnitID = 3, UnitSize = "Carton(24)"},
        //        new ProductUnit{UnitID = 4, UnitSize = "Carton(36)"},
        //        new ProductUnit{UnitID = 5, UnitSize = "Carton(48)"},
        //        new ProductUnit{UnitID = 6, UnitSize = "Carton(601)"},
        //        new ProductUnit{UnitID = 7, UnitSize = "Carton(72)"}
                
        //    };
        //    return productUnit;
        //}

        //public JsonResult GetProductBySubCategory(int ProductID)
        //{
        //    var products = db.ProductAdd.Where(m => m.ProductID == ProductID).ToList();

        //    List<SelectListItem> productList = new List<SelectListItem>();

        //    productList.Add(new SelectListItem { Text = "--Select Category--", Value = "0" });

        //    foreach (var m in products)
        //    {
        //        productList.Add(new SelectListItem { Text = m.ProductName, Value = Convert.ToString(m.ProductID) });
        //    }

        //    return Json(new SelectList(products, "Value", "Text", JsonRequestBehavior.AllowGet));


        //}

        //public JsonResult GetSubCategoryByCategory(int CategoryID)
        //{
        //    var subCategories = db.CategorySub.Where(m => m.CategoryID == CategoryID).ToList();

        //    List<SelectListItem> subCategoryList = new List<SelectListItem>();

        //    subCategoryList.Add(new SelectListItem { Text = "--Select Sub Category--", Value = "0" });

        //    foreach (var m in subCategories)
        //    {
        //        subCategoryList.Add(new SelectListItem { Text = m.SubCategoryName, Value = Convert.ToString(m.SubCategoryID) });
        //    }

        //    return Json(new SelectList(subCategoryList, "Value", "Text", JsonRequestBehavior.AllowGet));


        //}
                           //---Angular Js----

        //public ActionResult GetCity(int id)
        //{
        //    GaneshaEntities db = new GaneshaEntities();
        //    var data = db.Cities.Where(m => m.StateId == id);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

    }
}