using Dapper;
using DapperApiEmpTwoTables.Context;
using DapperApiEmpTwoTables.Model;
using DapperApiEmpTwoTables.Repository.Interface;

namespace DapperApiEmpTwoTables.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            List<Employee> emplist = new List<Employee>();
            var query = "SELECT * FROM Employee";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.QueryAsync<Employee>(query);
                //return employee.ToList();
                emplist = employee.ToList();
                foreach (var emp in emplist)
            {
                var res = await connection.QueryAsync<EmployeeDetails>(@"select * from Employee_details_table");
                    emp.employeeDetailsList = res.ToList();
            }

            return emplist;
            }
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
           Employee emp = new Employee();
            var query = "select * from Employee where Id=@id";
           
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Employee>(query, new {id=id});
                emp = result.FirstOrDefault();
                if(emp!=null)
                {
                    var employees=await connection.QueryAsync<EmployeeDetails>("select * from Employee_details_table where Id=@id", new {id=id});
                    emp.employeeDetailsList=employees.ToList();
                }
                return emp;
            }
        }

        public async Task<int> CreateEmployee(Employee emp)
        {
            //double ret = 0;
            var query = @"Insert into Employee(ename,salary) values (@ename,@salary);SELECT CAST(SCOPE_IDENTITY() as int)";
          //  var query1 = @"Insert into Employee_details_table(eid,id,designation,joindate) values (@designation,@joindate);SELECT CAST(SCOPE_IDENTITY() as int)";
            List<EmployeeDetails> emplist = new List<EmployeeDetails>();
            emplist = emp.employeeDetailsList.ToList();


            using (var connection = _context.CreateConnection())
            {

                {
                    int result = await connection.ExecuteAsync(query, emp);
                    foreach(var details in emp.employeeDetailsList)
                    {
                        int re = await connection.ExecuteAsync(@"Insert into Employee_details_table(eid,designation,joindate) values (@eid,@designation,@joindate)", details);
                        //ret = await InsertUpdateOrder(emplist, result);
                        //emp.totalOrderAmount = ret;

                    }
                    return Convert.ToInt32(result);
                }
                

            }
           
        }

        /*public async Task<double> InsertUpdateOrder(List<EmployeeDetails> emplist, int result)
        {
            int res = 0;
            double grandtotal = 0;
            if (result != 0)
            {
                using (var connection = _context.CreateConnection())
                {
                    foreach (EmployeeDetails e in emplist)
                    {
                        e.eid = result;
                       // var pquery = "select productprice from MProduct_Table where productid = @pid";
                        //var resprice = await connection.QuerySingleAsync<int>(pquery, new { pid = od.productId });
                       // e.totalAmount = resprice * od.quantity;
                        var qry = @"Insert into Employee_details_table(designation,joindate) values (@designation,@joindate) ";

                        var result1 = await connection.ExecuteAsync(qry, e);

                        res = res + result1;
                       // grandtotal = grandtotal + e.totalAmount;
                    }
                }
            }
            return res;
        }*/

        public async Task<int> UpdateEmployee(Employee emp)
        {
           // double ret;
            var query = @"Update Employee set ename=@ename,salary=@salary where id=@id ";
            using (var connection = _context.CreateConnection())
            {

                var result = await connection.ExecuteAsync(query, emp);
                foreach (var details in emp.employeeDetailsList)
                {
                    var re = await connection.ExecuteAsync(@"Update Employee_details_table set designation=@designation,joindate=@joindate where id=@id ", details);
                }
               // ret = await InsertUpdateOrder(emp.employeeDetailsList, emp.id);
                // emp.totalOrderAmount = ret;

                return result;

            }
        }


        public async Task<int> DeleteEmployee(int id)
        {
            var query = @"Delete from Employee where id=@Id
                          Delete from Employee_details_table where eid=@eid";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result;
            }
        }

       
    }
}
