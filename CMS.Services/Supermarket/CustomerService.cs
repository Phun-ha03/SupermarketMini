using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Customers;
using CMS.Models.Supermarket.Customers;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket
{
    public class CustomerService : ICustomerService
    {
        private readonly AICMSDBContext _context;

        public CustomerService(AICMSDBContext context
        )
        {
            _context = context;
        }

        public async Task<ApiResult<CustomerViewModel>> GetById(int id)
        {
            try
            {
                var function = await _context.Customers.FindAsync(id);

                if (function == null)
                {
                    return new ApiErrorResult<CustomerViewModel>(ConstantHelper.DataNotfound);
                }

                return new ApiSuccessResult<CustomerViewModel>(new CustomerViewModel(function));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<CustomerViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Customers.AsNoTracking().AsQueryable();

                var data = await query.Select(x => new CustomerViewModel(x))
                    .ToListAsync();

                data = data == null ? new List<CustomerViewModel>() : data;

                return new ApiSuccessResult<List<CustomerViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<CustomerViewModel>> Create(CustomerCreateRequest request)
        {
            try
            {
                var is_exists = (await _context.Customers.Where(m => m.Name.Equals(request.Name))
                    .ToListAsync()).Any();
                if (is_exists)
                {
                    return new ApiErrorResult<CustomerViewModel>("Loại khách hàng đã tồn tại");
                }

                var newObject = new Customer()
                {
                    CustomerID = request.CustomerID,
                    Name = request.Name,
                    CreateAt = request.CreateAt,
                    Address = request.Address,
                    Phone = request.Phone,
                    LoyalPoints = request.LoyalPoints,
                    Email = request.Email,

                };

                _context.Customers.Add(newObject);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<CustomerViewModel>(new CustomerViewModel(newObject));
                }
                return new ApiErrorResult<CustomerViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<CustomerViewModel>> Update(CustomerEditRequest request)
        {
            try
            {
                var is_exists = await _context.Customers
                    .Where(m => m.Name.Equals(request.Name)
                        && m.CustomerID != request.CustomerID)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<CustomerViewModel>("Tên khách hàng đã tồn tại");
                }

                var editObject = await _context.Customers.FindAsync(request.CustomerID);

                if (editObject == null)
                {
                    return new ApiErrorResult<CustomerViewModel>(ConstantHelper.UpdateNotfound);
                }

                editObject.Name = request.Name;
                editObject.Phone = request.Phone;
                editObject.Email = request.Email;
                editObject.Address = request.Address;
                //editObject.CreateAt = request.CreateAt;
                editObject.LoyalPoints = request.LoyalPoints;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<CustomerViewModel>(new CustomerViewModel(editObject));
                }
                else
                {
                    return new ApiErrorResult<CustomerViewModel>(ConstantHelper.UpdateError);
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
                var delObject = await _context.Customers.FindAsync(id);

                if (delObject == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                _context.Customers.Remove(delObject);

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

        public async Task<ApiResult<PagedResult<CustomerViewModel>>> GetListPaging(GetCustomerPagingRequest request)
        {
            try
            {
                var query = _context.Customers.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();

                    query = query.Where(x => x.Name.ToLower().Contains(keyword)
                                || x.Phone.ToLower().Contains(keyword));
                }

                int totalRow = await query.CountAsync();

                var data = await query.OrderByDescending(p => p.CustomerID)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new CustomerViewModel(x))
                    .ToListAsync();

                var pageResult = new PagedResult<CustomerViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<CustomerViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<CustomerViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<CustomerViewModel>> GetCustomerByPhone(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    return new ApiErrorResult<CustomerViewModel>("Số điện thoại không hợp lệ.");
                }

                var customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Phone == phone);

                if (customer == null)
                {
                    return new ApiErrorResult<CustomerViewModel>("Không tìm thấy khách hàng.");
                }

                return new ApiSuccessResult<CustomerViewModel>(new CustomerViewModel(customer));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), nameof(GetCustomerByPhone));
                throw;
            }
        }

    }
}
