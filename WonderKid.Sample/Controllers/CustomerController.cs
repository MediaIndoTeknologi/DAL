using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WonderKid.DAL.Interface;
using WonderKid.Data;
using WonderKid.Data.Model;

namespace WonderKid.Sample.Controllers
{
    [ApiController]
    [Route("Customer")]
    public class CustomerController : Controller
    {
        #region Fields and Contructor
        private readonly IUnitOfWork<NorthwindContext> context;
        public CustomerController(IUnitOfWork<NorthwindContext> context)
        {
            this.context = context;
        }
        #endregion

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await context.Entity<Customer>().ToListAsync();
                return StatusCode(200, result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("add_save")]
        [HttpPost]
        public async Task<IActionResult> Addsave([FromForm] Customer data)
        {
            try
            {
                var result = await context.AddSave<Customer>(data);
                return StatusCode(200, result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("add_range_save")]
        [HttpPost]
        public async Task<IActionResult> AddRangesave([FromBody]List<Customer> data)
        {
            try
            {

                var result = await context.AddSave<Customer>(data);
                return StatusCode(200, result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("update_save")]
        [HttpPatch]
        public async Task<IActionResult> Updatesave([FromForm]string customer_id)
        {
            try
            {

                var data = await context.Entity<Customer>().Where(d=>d.CustomerId==customer_id).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.ContactName = "testing";
                    var result = await context.UpdateSave<Customer>(data);
                    return StatusCode(200, result);
                }
                else
                    return StatusCode(404, "CustomerID not found!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("delete_save")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSave(string customer_id)
        {
            try
            {
                var result = await context.DeleteSave<Customer>(context.Entity<Customer>().Where(d => d.CustomerId == customer_id).FirstOrDefault());
                return StatusCode(200, result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("custom")]
        [HttpPost]
        public async Task<IActionResult> Custom([FromForm] int update_product_id, [FromForm] string product_name, [FromForm] int remove_order_detail_id)
        {
            try
            {
                var add_customer = new Customer()
                {
                    CustomerId = "CST01",
                    CompanyName = "TESTING"
                };
                var update_product =  await context.Entity<Product>().Where(d => d.ProductId == update_product_id).FirstOrDefaultAsync();
                var delete_order = await context.Entity<OrderDetail>().Where(d => d.OrderId == remove_order_detail_id).ToListAsync();
                if(update_product != null && delete_order != null)
                {
                    //add
                    context.Add<Customer>(add_customer);

                    //update
                    update_product.ProductName = product_name;
                    context.Update<Product>(update_product);

                    //delete list
                    context.Delete<OrderDetail>(delete_order);

                    //commit
                    var result = await context.Commit();
                    return StatusCode(200, result);

                }
                else
                    return StatusCode(404, "update_product_id  or remove_order_detail_id not found!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
