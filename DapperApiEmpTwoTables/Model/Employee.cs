namespace DapperApiEmpTwoTables.Model
{
    public class Employee
    {
        public int id { get; set; }
        public string? ename { get; set; }
        public double salary { get; set; }
        public List<EmployeeDetails> employeeDetailsList { get; set; }
    }
}
