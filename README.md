## Description

Penggunaan untuk data access layer agar dinamis 

## Main steps

### 1.Installation package - Wonderkid.DAL

In this tutorial, we will only demonstrate the use of DAL. We need to install the following packages in the prepared project with the following reference commands：

```powershell
Install-Package Wonderkid.DAL
```

### 2.Register Service

```Startup.cs
using WonderKid.DAL;
using WonderKid.DAL.Interface;

public void ConfigureServices(IServiceCollection services){
  services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
  ...
 }
```

### 3.Implement Interface IEntity 

implement IEntity on your model database

```Order.cs
using WonderKid.DAL.Interface;

namespace WonderKid.Data.Model
{
    public partial class Order : IEntity
    {
```
you can implement BaseGuidEntity,BaseIntEntity,BaseStringEntity 
if your database have structure Id,CreatedDate,ModifiedDate,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedAt

```
public class BaseEntity : IBaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    [Column("CreatedBy")]
    [Display(Name = "Creator")]
    public string CreatedBy { get; set; }

    [Column("ModifiedBy")]
    [Display(Name = "Modifier")]
    public string ModifiedBy { get; set; }
    public string CreatedByWithUserNameOnly { get { if (this.CreatedBy != null) { if (this.CreatedBy.Contains("|")) { return this.CreatedBy.Split("|")[0]; } else { return this.CreatedBy; } } else { return "N/A"; } } }
    public string CreatedAtFormated { get { return this.CreatedDate.ToString("dd MMM yyyy HH:mm:ss"); } }
    public string ModifiedByWithUserNameOnly { get { if (this.ModifiedBy != null) { if (this.ModifiedBy.Contains("|")) { return this.ModifiedBy.Split("|")[0]; } else { return this.ModifiedBy; } } else { return "N/A"; } } }
    public string ModifiedAtFormated { get { return this.ModifiedDate.HasValue ? this.ModifiedDate.Value.ToString("dd MMM yyyy HH:mm:ss") : "N/A"; } }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
    public string DeletedByWithUserNameOnly { get { if (this.DeletedBy != null) { if (this.DeletedBy.Contains("|")) { return this.DeletedBy.Split("|")[0]; } else { return this.CreatedBy; } } else { return "N/A"; } } }
    public string DeletedAtFormated { get { return this.DeletedAt.HasValue ? this.DeletedAt.Value.ToString("dd MMM yyyy HH:mm:ss") : "N/A"; } }
}
public class BaseGuidEntity:BaseEntity
{
    public BaseGuidEntity()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
}
public class BaseIntEntity : BaseEntity
{
    [Column(Order = 0)]
    public int Id { get; set; }
}
public class BaseStringEntity : BaseEntity
{
    public string Id { get; set; }
}
```

### 3. Implement in controller/services ADD/UPDATE/DELETE/COMMIT

add unitofwork in constructor

- Constructor 
  ```
  private readonly IUnitOfWork<NorthwindContext> context;
  public CustomerController(IUnitOfWork<NorthwindContext> context)
  {
      this.context = context;
  }
  ```
  
- Insert 1 row only and save 
  ```
      var data = new Customer()
        {
            CustomerId = "CST01",
            CompanyName = "TESTING"
        };
      var result = await context.AddSave<Customer>(data);
  ```
  
- Insert multiple row only and save 
  ```
      var data = new List<Customer>();
      data.Add(new Customer()
        {
            CustomerId = "CST01",
            CompanyName = "TESTING"
        });
      data.Add(new Customer()
        {
            CustomerId = "CST02",
            CompanyName = "TESTING"
        });
      var result = await context.AddSave<Customer>(data);
  ```
  
- update 1 row only and save 
  ```
     var data = await context.Entity<Customer>().Where(d=>d.CustomerId==customer_id).FirstOrDefaultAsync();
      if (data != null)
      {
          data.ContactName = "testing";
          var result = await context.UpdateSave<Customer>(data);
      }
  ```
  ** updatesave also support multiple data with List<T>  
  
  - delete 1 row only and save 
  ```
     var data = await context.Entity<Customer>().Where(d=>d.CustomerId==customer_id).FirstOrDefaultAsync();
      if (data != null)
      {
          var result = await context.DeleteSave<Customer>(data);
      }
  ```
 
- commit 
  you can update, delete and insert at the same transaction and if one failed then the transaction become rollback
  ```
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
  ```
  as you can see if add/update/delete is void type and no return except commit.
  
### 4. QUERY Command
you can use query command for complex or if u familiar with sql query
- List 
  ```
    var result = await context.ListQuery<Customer>("select * from Customers");
    return StatusCode(200, result);
  ```
- Single 
  ```
    var result = await context.SingleQuery<Customer>("select * from Customers");
    return StatusCode(200, result);
  ```
- ExecuteQuery 
  ```
    var result = await context.ExecuteQuerySave("delete from Customers where CustomerID = "+customer_id);
    return StatusCode(200, result);
  ```  
## Finally
 
This concludes the entire tutorial on using Data Access Layer. ** Related libraries will be updated all the time, and there may be slight differences in functional experience with this tutorial, please refer to the relevant specific code, version logs, and examples. **
