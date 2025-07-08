using ClosedXML.Excel;
using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.ProductUnits;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket
{
    public class ProductService : IProductService
    {
        private readonly AICMSDBContext _context;
       
        public ProductService(AICMSDBContext context
        )
        {
            _context = context;
           
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int id)
        {
            try
            {
                var function = await _context.Products.FindAsync(id);

                if (function == null)
                {
                    return new ApiErrorResult<ProductViewModel>(ConstantHelper.DataNotfound);
                }

                return new ApiSuccessResult<ProductViewModel>(new ProductViewModel(function));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<ProductViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Products.AsNoTracking().AsQueryable();

                var data = await query.Select(x => new ProductViewModel(x))
                    .ToListAsync();

                data = data == null ? new List<ProductViewModel>() : data;

                return new ApiSuccessResult<List<ProductViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<bool>> Create(ProductCreateRequest model)
        {
            try
            {
                var isExists = await _context.Products
               .AnyAsync(x => x.Name == model.Name || x.Barcode == model.Barcode ||x.ProductCode==model.ProductCode);
                if (isExists)
                {
                    return new ApiErrorResult<bool>("Tên,mã sản phẩm hoặc mã vạch đã tồn tại");
                }
                string imagePath = null;
                if (model.ImageUpload != null && model.ImageUpload.Length > 0)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageUpload.FileName);
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUpload.CopyToAsync(stream);
                    }

                    imagePath = "/images/products/" + uniqueFileName;
                }

                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    ProductCode = model.ProductCode,
                    Price = model.Price,
                    Barcode = model.Barcode,
                    StockQuantity = model.StockQuantity,
                    ExpirationDate = model.ExpirationDate,
                    CategoryID = model.CategoryID,
                    SupplierID = model.SupplierID,
                    Image = imagePath
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần
                return new ApiErrorResult<bool>("Lỗi khi tạo sản phẩm: " + ex.Message);
            }
        }
        public async Task<ApiResult<bool>> Update(ProductEditRequest request)
        {
            try
            {
                var product = await _context.Products.FindAsync(request.ProductID);

                if (product == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy sản phẩm để cập nhật");
                }

                // Xử lý ảnh mới nếu có
                if (request.ImageUpload != null && request.ImageUpload.Length > 0)
                {
                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageUpload.FileName);
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.ImageUpload.CopyToAsync(stream);
                    }

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.Image.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    product.Image = "/images/products/" + uniqueFileName;
                }

                // Cập nhật các trường khác
                product.Name = request.Name;
                product.Description = request.Description;
                product.ProductCode = request.ProductCode;
                product.Price = request.Price;
                product.Barcode = request.Barcode;
                product.StockQuantity = request.StockQuantity;
                product.ExpirationDate = request.ExpirationDate;
                product.CategoryID = request.CategoryID;
                product.SupplierID = request.SupplierID;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        //public async Task<ApiResult<ProductViewModel>> Update(ProductEditRequest request)
        //{
        //    try
        //    {
        //        var is_exists = await _context.Products
        //            .Where(m => m.Name.Equals(request.Name)
        //                && m.ProductID != request.ProductID)
        //            .AnyAsync();
        //        if (is_exists)
        //        {
        //            return new ApiErrorResult<ProductViewModel>("Tên loại nguyên liệu đã tồn tại");
        //        }

        //        var editObject = await _context.Products.FindAsync(request.ProductID);

        //        if (editObject == null)
        //        {
        //            return new ApiErrorResult<ProductViewModel>(ConstantHelper.UpdateNotfound);
        //        }
        //        if (request.ImageUpload != null && request.ImageUpload.Length > 0)
        //        {
        //            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
        //            if (!Directory.Exists(uploadFolder))
        //            {
        //                Directory.CreateDirectory(uploadFolder);
        //            }

        //            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.ImageUpload.FileName);
        //            string filePath = Path.Combine(uploadFolder, uniqueFileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await request.ImageUpload.CopyToAsync(stream);
        //            }

        //            // Xóa ảnh cũ nếu cần (optionally)
        //            if (!string.IsNullOrEmpty(editObject.Image))
        //            {
        //                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", editObject.Image.TrimStart('/'));
        //                if (System.IO.File.Exists(oldImagePath))
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }

        //            editObject.Image = "/images/products/" + uniqueFileName;
        //        }
        //        editObject.Name = request.Name;
        //        editObject.Description = request.Description;
        //        editObject.ProductCode = request.ProductCode;
        //        editObject.Price = request.Price;
        //        editObject.StockQuantity = request.StockQuantity;
        //        editObject.Barcode = request.Barcode;
        //        editObject.CategoryID = request.CategoryID;
        //        editObject.SupplierID = request.SupplierID;
        //        editObject.ExpirationDate = request.ExpirationDate;



        //        var result = await _context.SaveChangesAsync();

        //        if (result > 0)
        //        {
        //            return new ApiSuccessResult<ProductViewModel>(new ProductViewModel(editObject));
        //        }
        //        else
        //        {
        //            return new ApiErrorResult<ProductViewModel>(ConstantHelper.UpdateError);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
        //        throw;
        //    }
        //}

        //public async Task<ApiResult<int>> Delete(int id)
        //{
        //    try
        //    {
        //        var delObject = await _context.Products.FindAsync(id);

        //        if (delObject == null)
        //        {
        //            return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
        //        }

        //        _context.Products.Remove(delObject);

        //        var result = await _context.SaveChangesAsync();

        //        if (result > 0)
        //        {
        //            return new ApiSuccessResult<int>(1);
        //        }
        //        else
        //        {
        //            return new ApiErrorResult<int>(ConstantHelper.UpdateError);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
        //        throw;
        //    }
        //}

        public async Task<ApiResult<int>> Delete(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return new ApiErrorResult<int>("Không tìm thấy sản phẩm.");

                // 1. Xóa ReturnOrder
                var returnOrders = await _context.ReturnOrders
                    .Where(ro => ro.ProductID == id)
                    .ToListAsync();
                _context.ReturnOrders.RemoveRange(returnOrders);

                // 2. Xóa OrderDetails
                var orderDetails = await _context.OrderDetails
                    .Where(od => od.ProductID == id)
                    .ToListAsync();
                _context.OrderDetails.RemoveRange(orderDetails);

                // 3. Xóa StockImportDetails
                var importDetails = await _context.StockImportDetails
                    .Where(sid => sid.ProductID == id)
                    .ToListAsync();
                _context.StockImportDetails.RemoveRange(importDetails);

                // 4. Xóa ProductUnits
                var productUnits = await _context.ProductUnits
                    .Where(pu => pu.ProductID == id)
                    .ToListAsync();
                _context.ProductUnits.RemoveRange(productUnits);

                // 5. Cuối cùng xóa Product
                _context.Products.Remove(product);

                await _context.SaveChangesAsync();

                return new ApiSuccessResult<int>(1);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), nameof(Delete));
                return new ApiErrorResult<int>("Lỗi khi xóa sản phẩm.");
            }
        }



        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetListPaging(GetProductPagingRequest request)
        {
            try
            {
                var query = from p in _context.Products.AsNoTracking()
                            join c in _context.Categories on p.CategoryID equals c.CategoryID
                            join s in _context.Suppliers on p.SupplierID equals s.SupplierID
                            select new ProductViewModel
                            {
                                ProductID = p.ProductID,
                                Name = p.Name,
                                Barcode = p.Barcode,
                                Description = p.Description,
                                ProductCode=p.ProductCode,
                                Price = p.Price,
                                StockQuantity = p.StockQuantity,
                                ExpirationDate= p.ExpirationDate,
                                CategoryID = p.CategoryID,
                                CategoryName = c.CategoryName, // Lấy tên danh mục 
                                SupplierID = p.SupplierID,
                                SupplierName = s.SupplierName,// Lấy tên nhà cung cấp 
                                Image=p.Image,
                            };

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(keyword)
                                          || x.Barcode.ToLower().Contains(keyword));
                }

                if (request.CategoryID.HasValue && request.CategoryID.Value > 0)
                {
                    query = query.Where(x => x.CategoryID == request.CategoryID.Value);
                }

                if (request.SupplierID.HasValue && request.SupplierID.Value > 0)
                {
                    query = query.Where(x => x.SupplierID == request.SupplierID.Value);
                }
                // Lọc theo khoảng giá (Price range)
                if (!string.IsNullOrEmpty(request.priceFilter))
                {
                    var priceRange = request.priceFilter.Split('-');
                    if (priceRange.Length == 2)
                    {
                        if (decimal.TryParse(priceRange[0], out var minPrice) && decimal.TryParse(priceRange[1], out var maxPrice))
                        {
                            query = query.Where(x => x.Price >= minPrice && x.Price <= maxPrice);
                        }
                    }
                }
                int totalRow = await query.CountAsync();

                var data = await query.OrderByDescending(p => p.ProductID)
                      .Skip((request.PageIndex - 1) * request.PageSize)
                      .Take(request.PageSize)
                      .ToListAsync();
                var pageResult = new PagedResult<ProductViewModel>
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data ?? new List<ProductViewModel>()
                };

                return new ApiSuccessResult<PagedResult<ProductViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        
        public async Task<byte[]> ExportCsv()
        {
            try
            {
                var products = await _context.Products
                    .Include(x => x.Category)
                    .Include(x => x.Supplier)
                    .AsNoTracking()
                    .ToListAsync();

                var csv = new StringBuilder();
                csv.AppendLine("Name,Decription,Price,StockQuantity,Barcode,Category,Supplier");

                foreach (var p in products)
                {
                    var line = 
                               $"{EscapeCsv(p.Name)}," +
                               $"{EscapeCsv(p.Description)}," +
                               $"{p.Price}," +
                               $"{p.StockQuantity}," +
                               $"{p.Barcode}," +
                               $"{EscapeCsv(p.Category?.CategoryName)}," +
                               $"{EscapeCsv(p.Supplier?.SupplierName)}";

                    csv.AppendLine(line);
                }

                return Encoding.UTF8.GetBytes(csv.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(",") || value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }
        
        public async Task<ApiResult<List<ProductViewModel>>> GetProductsByKeyword(string keyword)
        {
            try
            {
                     var query = _context.Products
                    .AsNoTracking() 
                    .Include(p => p.Category) 
                    .Include(p => p.Supplier) 
                    .AsQueryable(); 

                // Kiểm tra nếu từ khóa không rỗng hoặc trắng
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string keywordLower = keyword.ToLower(); 
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(keywordLower) || 
                        p.Barcode.ToLower().Contains(keywordLower) // Kiểm tra mã vạch sản phẩm
                    );
                }

                // Lấy danh sách sản phẩm theo từ khóa
                var data = await query
                    .Select(p => new ProductViewModel
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Image=p.Image,
                        
                    }).Take(5)
                    .ToListAsync();

                // Nếu không có sản phẩm nào, trả về danh sách rỗng
                data ??= new List<ProductViewModel>();

                return new ApiSuccessResult<List<ProductViewModel>>(data);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi vào log nếu có lỗi xảy ra
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<List<ProductUnitViewModel>> GetUnitsByProductID(int productID)
        {
            var units = await _context.ProductUnits
                .Where(x => x.ProductID == productID)
                .Select(x => new ProductUnitViewModel(x))
                .ToListAsync();

            return units;
        }

        public async Task<byte[]> ExportProductsToExcelAsync()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .AsNoTracking()
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Products");

                // Header
                worksheet.Cell(1, 1).Value = "Tên sản phẩm";
                worksheet.Cell(1, 2).Value = "Mô tả";
                worksheet.Cell(1, 3).Value = "Giá";
                worksheet.Cell(1, 4).Value = "Tồn kho";
                worksheet.Cell(1, 5).Value = "Barcode";
                worksheet.Cell(1, 6).Value = "Danh mục";
                worksheet.Cell(1, 7).Value = "Nhà cung cấp";

                // Nội dung
                int row = 2;
                foreach (var p in products)
                {
                    worksheet.Cell(row, 1).Value = p.Name;
                    worksheet.Cell(row, 2).Value = p.Description;
                    worksheet.Cell(row, 3).Value = p.Price;
                    worksheet.Cell(row, 4).Value = p.StockQuantity;
                    worksheet.Cell(row, 5).Value = p.Barcode;
                    worksheet.Cell(row, 6).Value = p.Category?.CategoryName;
                    worksheet.Cell(row, 7).Value = p.Supplier?.SupplierName;
                    row++;
                }

                // Auto-fit
                worksheet.Columns().AdjustToContents();

                // Xuất file ra byte[]
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

    }
}
    


