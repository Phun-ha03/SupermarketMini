using ClosedXML.Excel;
using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Suppliers;
using CMS.Models.Supermarket.Suppliers;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket

{
    public class SupplierService : ISupplierService
    {
        private readonly AICMSDBContext _context;

        public SupplierService(AICMSDBContext context
        )
        {
            _context = context;
        }

        //public async Task<ApiResult<SupplierViewModel>> GetById(int id)
        //{
        //    try
        //    {
        //        var function = await _context.Suppliers.FindAsync(id);

        //        if (function == null)
        //        {
        //            return new ApiErrorResult<SupplierViewModel>(ConstantHelper.DataNotfound);
        //        }

        //        return new ApiSuccessResult<SupplierViewModel>(new SupplierViewModel(function));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
        //        throw;
        //    }
        //}
        public async Task<ApiResult<SupplierViewModel>> GetById(int id)
        {
            try
            {
                LogHelper.writeLog($"Đang tìm Supplier với ID: {id}", "GetById");

                var supplier = await _context.Suppliers.FindAsync(id);

                if (supplier == null)
                {
                    LogHelper.writeLog($"Không tìm thấy nhà cung cấp với ID: {id}", "GetById");
                    return new ApiErrorResult<SupplierViewModel>(ConstantHelper.DataNotfound);
                }

                return new ApiSuccessResult<SupplierViewModel>(new SupplierViewModel(supplier));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<SupplierViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Suppliers.AsNoTracking().AsQueryable();

                var data = await query.Select(x => new SupplierViewModel(x))
                    .ToListAsync();

                data = data == null ? new List<SupplierViewModel>() : data;

                return new ApiSuccessResult<List<SupplierViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<SupplierViewModel>> Create(SupplierCreateRequest request)
        {
            try
            {
                var is_exists = (await _context.Suppliers.Where(m => m.SupplierName.Equals(request.SupplierName))
                    .ToListAsync()).Any();
                if (is_exists)
                {
                    return new ApiErrorResult<SupplierViewModel>("Tên nhãn hàng đã tồn tại");
                }

                var newObject = new Supplier()
                {
                    SupplierID = request.SupplierID,
                    SupplierName = request.SupplierName,
                    ContactPerson = request.ContactPerson,
                    Address = request.Address,
                    Phone = request.Phone,
                    Email = request.Email,
                    CreateAt= DateTime.UtcNow,

                };

                _context.Suppliers.Add(newObject);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<SupplierViewModel>(new SupplierViewModel(newObject));
                }
                return new ApiErrorResult<SupplierViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<SupplierViewModel>> Update(SupplierEditRequest request)
        {
            try
            {
                var is_exists = await _context.Suppliers
                    .Where(m => m.SupplierName.Equals(request.SupplierName)
                        && m.SupplierID != request.SupplierID)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<SupplierViewModel>("Tên loại nguyên liệu đã tồn tại");
                }

                var editObject = await _context.Suppliers.FindAsync(request.SupplierID);

                if (editObject == null)
                {
                    return new ApiErrorResult<SupplierViewModel>(ConstantHelper.UpdateNotfound);
                }

                editObject.SupplierName = request.SupplierName;
                editObject.SupplierID = request.SupplierID;
                editObject.ContactPerson = request.ContactPerson;
                editObject.Address = request.Address;
                editObject.Phone = request.Phone;
                editObject.Email = request.Email;
                editObject.CreateAt = request.CreateAt;


                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<SupplierViewModel>(new SupplierViewModel(editObject));
                }
                else
                {
                    return new ApiErrorResult<SupplierViewModel>(ConstantHelper.UpdateError);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<int>> Delete(int id)
        {
            try
            {
                var delObject = await _context.Suppliers.FindAsync(id);

                if (delObject == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                _context.Suppliers.Remove(delObject);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<int>(1);
                }
                else
                {
                    return new ApiErrorResult<int>(ConstantHelper.UpdateError);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<PagedResult<SupplierViewModel>>> GetListPaging(GetSupplierPagingRequest request)
        {
            try
            {
                var query = _context.Suppliers.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();

                    query = query.Where(x => x.SupplierName.ToLower().Contains(keyword)
                                || x.ContactPerson.ToLower().Contains(keyword));
                }

                int totalRow = await query.CountAsync();

                var data = await query.OrderByDescending(p => p.SupplierID)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new SupplierViewModel(x))
                    .ToListAsync();

                var pageResult = new PagedResult<SupplierViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<SupplierViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<SupplierViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<List<SelectListItem>> GetSelectListAsync()
        {
            return await _context.Suppliers
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierID.ToString(),
                    Text = s.SupplierName
                }).ToListAsync();
        }
        public async Task<byte[]> ExportSuppliersToExcelAsync()
        {
            try
            {
                var suppliers = await _context.Suppliers
                    .AsNoTracking()
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Suppliers");

                // Header
                worksheet.Cell(1, 1).Value = "Tên nhà cung cấp";
                worksheet.Cell(1, 2).Value = "Người liên hệ";
                worksheet.Cell(1, 3).Value = "Số điện thoại";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "Địa chỉ";
                worksheet.Cell(1, 6).Value = "Ngày tạo";

                // Nội dung
                int row = 2;
                foreach (var s in suppliers)
                {
                    worksheet.Cell(row, 1).Value = s.SupplierName;
                    worksheet.Cell(row, 2).Value = s.ContactPerson;
                    worksheet.Cell(row, 3).Value = s.Phone;
                    worksheet.Cell(row, 4).Value = s.Email;
                    worksheet.Cell(row, 5).Value = s.Address;
                    worksheet.Cell(row, 6).Value = s.CreateAt.ToString("dd/MM/yyyy");
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
