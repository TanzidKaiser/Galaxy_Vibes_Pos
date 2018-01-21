using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GalaxyVibesPos.Models;
using GalaxyVibesPos.Models.ViewBag;
namespace GalaxyVibesPos.Controllers
{
    public class ItemSattingController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        //-----For Main Category Index Edit

        // index
        public ActionResult MainCategoryIndex()
        {
            return View(db.CategoryMain.ToList());
        }

        // Details
        public ActionResult MainCategoryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryMain categorymain = db.CategoryMain.Find(id);
            if (categorymain == null)
            {
                return HttpNotFound();
            }
            return View(categorymain);
        }


        // Edit
        public ActionResult MainCategoryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryMain categorymain = db.CategoryMain.Find(id);
            if (categorymain == null)
            {
                return HttpNotFound();
            }
            return View(categorymain);
        }

        // ----------For Category Index Edit-----------------------



        // Index
        public ActionResult CategoryIndex()
        {
            return View(db.Category.ToList());
        }

        // Edit
        public ActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // Details
        public ActionResult CategoryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //----------- For Sub Category index Edit -----------------------

        // Index
        public ActionResult SubCategoryIndex()
        {
            return View(db.CategorySub.ToList());
        }

        // Edit
        public ActionResult SubCategoryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategorySub subCategory = db.CategorySub.Find(id);

            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);

        }

        // Details
        public ActionResult SubCategoryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategorySub subCategory = db.CategorySub.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // ---------------- For Product Edit -----------

        //index
        public ActionResult ProductIndex()
        {
            return View(db.productDetails.ToList());
        }

        //Edit
        public ActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetails productDetails = db.productDetails.Find(id);

            if (productDetails == null)
            {
                return HttpNotFound();
            }
            return View(productDetails);
        }

        // Details
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetails productDetails = db.productDetails.Find(id);
            if (productDetails == null)
            {
                return HttpNotFound();
            }
            return View(productDetails);
        }

        //For Item Setting

        public ActionResult ItemSettings()
        {

            var mainCategories = db.CategoryMain.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Main Category", Value = "0" });
            foreach (var m in mainCategories)
            {
                list.Add(new SelectListItem { Text = m.MaincategoryName, Value = Convert.ToString(m.MainCategoryID) });

            }

            ViewBag.MainCategoryList = list;

            ViewBag.ProductUnit = GetUnit();
            ViewBag.Warehouse = GetWarehouse();
            return View();


        }
        //Get All Unit Name
        public dynamic GetUnit()
        {
            var units = db.ProductUnit.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Unit", Value = "0" });
            foreach (var m in units)
            {
                list.Add(new SelectListItem { Text = m.UnitSize, Value = Convert.ToString(m.UnitID) });

            }
            return list;
        }
        //Get All Warehouse Name
        public dynamic GetWarehouse()
        {
            var warehouses = db.Warehouse.ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select Warehouse", Value = "0" });
            foreach (var m in warehouses)
            {
                list.Add(new SelectListItem { Text = m.WarehouseName, Value = Convert.ToString(m.WarehouseID) });

            }
            return list;
        }

        //--------- Save Main Category --------- !

        public JsonResult SaveMainCategoryInDatabase(CategoryMain model)
        {


            string Msg = null;

            CategoryMain mainCategory = new CategoryMain();

            var mainCategoryName = db.CategoryMain.Where(c => c.MaincategoryName == model.MaincategoryName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (mainCategoryName == null)
                {
                    if (model.MainCategoryID > 0)
                    {
                        mainCategory = db.CategoryMain.SingleOrDefault(x => x.MainCategoryID == model.MainCategoryID);
                        mainCategory.MaincategoryName = model.MaincategoryName;
                        db.SaveChanges();
                        Msg = "Item Update Successfully";
                    }
                    else
                    {
                        mainCategory.MaincategoryName = model.MaincategoryName;
                        db.CategoryMain.Add(mainCategory);
                        db.SaveChanges();
                        Msg = "Main Category Save Successfully";

                    }
                }
                else
                {
                    Msg = "Item Name already Exists";
                }
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        // ---------   Save Category   --------- !

        public JsonResult SaveCategoryInDatabase(Category model)
        {


            string Msg = null;

            Category category = new Category();

            var CategoryName = db.Category.Where(c => c.CategoryName == model.CategoryName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (CategoryName == null)
                {
                    if (model.CategoryID > 0)
                    {
                        category = db.Category.SingleOrDefault(x => x.CategoryID == model.CategoryID);
                        category.CategoryName = model.CategoryName;
                        db.SaveChanges();
                        Msg = "Category Update Successfully";
                    }
                    else
                    {
                        category.CategoryName = model.CategoryName;
                        category.MainCategoryID = model.MainCategoryID;
                        db.Category.Add(category);
                        db.SaveChanges();
                        Msg = "Main Category Save Successfully";

                    }
                }
                else
                {
                    Msg = "Item Name already Exists";
                }
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }


        // ---------   Save Sub Category   --------- !

        public JsonResult SaveSubCategoryInDatabase(CategorySub model)
        {


            string Msg = null;

            CategorySub subCategory = new CategorySub();

            var SubCategoryName = db.CategorySub.Where(c => c.SubCategoryName == model.SubCategoryName && c.CategoryID == model.CategoryID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (SubCategoryName == null)
                {
                    if (model.SubCategoryID > 0)
                    {
                        subCategory = db.CategorySub.SingleOrDefault(x => x.SubCategoryID == model.SubCategoryID);
                        subCategory.SubCategoryName = model.SubCategoryName;
                        db.SaveChanges();
                        Msg = "Sub Category Update Successfully";
                    }
                    else
                    {
                        subCategory.SubCategoryName = model.SubCategoryName;
                        subCategory.MainCategoryID = model.MainCategoryID;
                        subCategory.CategoryID = model.CategoryID;
                        db.CategorySub.Add(subCategory);
                        db.SaveChanges();
                        Msg = "Sub Category Save Successfully";

                    }
                }
                else
                {
                    Msg = "Item Name already Exists";
                }
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }


        // ---------   Save Add Product    --------- !

        public JsonResult SaveAddProductInDatabase(ProductDetails model)
        {


            string Msg = null;


            ProductDetails Product = new ProductDetails();

            var productName = db.productDetails.Where(c => c.ProductName == model.ProductName && c.SubCategoryID == model.SubCategoryID).FirstOrDefault();



            if (ModelState.IsValid)
            {
                if (productName == null)
                {
                    if (model.ProductDetailsID > 0)
                    {
                        Product = db.productDetails.SingleOrDefault(x => x.ProductDetailsID == model.ProductDetailsID);
                        Product.ProductName = model.ProductName;
                        db.SaveChanges();
                    }
                    else
                    {
                        Product.Code = getProductCode();
                        Product.ProductName = model.ProductName;
                        Product.MainCategoryID = model.MainCategoryID;
                        Product.SubCategoryID = model.SubCategoryID;
                        Product.CategoryID = model.CategoryID;
                        db.productDetails.Add(Product);
                        db.SaveChanges();
                        Msg = "Product Add Successfully";

                    }
                }
                else
                {
                    Msg = "Product Already Exists";
                }

            }
            return Json(Msg, JsonRequestBehavior.AllowGet);


        }
        // Get Code For ProductDetails tbl 

        public string getProductCode()
        {
            string code = null;
            var maxID = db.productDetails.OrderByDescending(x => x.ProductDetailsID).FirstOrDefault();

            if(maxID != null)
            {
                
                code = "P00" + Convert.ToString(maxID.ProductDetailsID + 1); 
            }
            else
            {

                code = "P001";

            }
          
            return code;

        }

        // Save Product Details

        public JsonResult SaveProductDetailsInDatabase(ProductDetails model)
        {
            ProductDetails productDetail = new ProductDetails();
            string Msg = null;

            var product = db.productDetails.Where(c => c.SubCategoryID == model.SubCategoryID && c.CategoryID == model.CategoryID && c.ProductDetailsID == model.ProductDetailsID).First();



            if (product != null)
            {
                productDetail = db.productDetails.SingleOrDefault(x => x.ProductDetailsID == product.ProductDetailsID);
                productDetail.MainCategoryID = model.MainCategoryID;
                productDetail.CategoryID = model.CategoryID;
                productDetail.SubCategoryID = model.SubCategoryID;

                productDetail.Stoke = 0;

                productDetail.PurchasePrice = model.PurchasePrice;
                productDetail.SalePrice = model.SalePrice;

                productDetail.Description = model.Description;
                productDetail.UnitID = model.UnitID;
                productDetail.MinimumProductQuantity = model.MinimumProductQuantity;
                productDetail.WarehouseID = model.WarehouseID;
                productDetail.RackID = model.RackID;
                productDetail.CellID = model.CellID;

                db.SaveChanges();
                Msg = "Product Details update Successfully";

            }


            return Json(Msg, JsonRequestBehavior.AllowGet);

        }

        // Update Product Details

        public JsonResult UpdateProductDetails(ProductDetails model)
        {
            ProductDetails productDetail = new ProductDetails();
            string Msg = null;

            if (ModelState.IsValid)
            {

                productDetail = db.productDetails.SingleOrDefault(x => x.ProductDetailsID == model.ProductDetailsID);
                //productDetail.MainCategoryID = model.MainCategoryID;
                //productDetail.CategoryID = model.CategoryID;
                //productDetail.SubCategoryID = model.SubCategoryID;
                //productDetail.Code = model.Code;
                productDetail.ProductName = model.ProductName;
                productDetail.PurchasePrice = model.PurchasePrice;
                productDetail.SalePrice = model.SalePrice;
                //productDetail.Stoke = model.Stoke;
                productDetail.Description = model.Description;
                // productDetail.UnitID = model.UnitID;
                productDetail.MinimumProductQuantity = model.MinimumProductQuantity;
                //productDetail.WarehouseID = model.WarehouseID;
                //productDetail.RackID = model.RackID;
                //productDetail.CellID = model.CellID;
                //productDetail.Status = model.Status;
                db.SaveChanges();
                Msg = "Product Details update Successfully";



            }

            return Json(Msg, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetCategoryByMainCategory(int MainCategoryID)
        {
            var categories = db.Category.Where(m => m.MainCategoryID == MainCategoryID).ToList();

            List<SelectListItem> categoryList = new List<SelectListItem>();

            categoryList.Add(new SelectListItem { Text = "--Select Category--", Value = "0" });

            foreach (var m in categories)
            {
                categoryList.Add(new SelectListItem { Text = m.CategoryName, Value = Convert.ToString(m.CategoryID) });
            }

            return Json(new SelectList(categoryList, "Value", "Text", JsonRequestBehavior.AllowGet));


        }


        //---Cascading Dropdown For Add Product--- !


        public JsonResult GetSubCategoryByCategory(int CategoryID)
        {
            //var CategoryList = this.GetCategory(MainCategoryID);
            var categoryList = db.CategorySub.Where(m => m.CategoryID == CategoryID).ToList();
            var subCategories = categoryList.Select(m => new SelectListItem()
            {
                Text = m.SubCategoryName,
                Value = m.SubCategoryID.ToString(),
            });

            return Json(subCategories, JsonRequestBehavior.AllowGet);
        }

       
        // When selct product dropdown all input field will bind selected product data 

        public JsonResult DataBindForInputField(int mainCatID)
        {
            var select = db.productDetails.Where(a => a.MainCategoryID == mainCatID).FirstOrDefault();

            ProductDetails product = new ProductDetails()
            {
                PurchasePrice = select.PurchasePrice,
                SalePrice = select.SalePrice,
                Description = select.Description,
                MinimumProductQuantity = select.MinimumProductQuantity
            };
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductBySubCategoryID(string ProductId)
        {
            if (ProductId != "0")
            {
                int id = Convert.ToInt32(ProductId);
                var products = db.productDetails.Where(a => a.SubCategoryID == id).ToList();

                List<SelectListItem> productList = new List<SelectListItem>();
                productList.Add(new SelectListItem { Text = "Select Product", Value = "0" });

                foreach (var m in products)
                {
                    productList.Add(new SelectListItem { Text = m.ProductName, Value = Convert.ToString(m.ProductDetailsID) });
                }

                return Json(new SelectList(productList, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}
