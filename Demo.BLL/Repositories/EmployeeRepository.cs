using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{

    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {

        #region old
        //    private readonly MvcDbContext context;

        //    public EmployeeRepository(MvcDbContext context)
        //    {
        //        this.context = context;
        //    }

        //    public int Add(Employee employee)
        //    {
        //        context.Employees.Add(employee);
        //        return context.SaveChanges();

        //    }

        //    public int Delete(Employee employee)
        //    {
        //        context.Employees.Remove(employee);
        //        return context.SaveChanges();
        //    }

        //    public Employee Get(int? id)
        //        => context.Employees.FirstOrDefault(D => D.Id == id);



        //    public IEnumerable<Employee> GetAll()
        //        => context.Employees.ToList();


        //    public int Update(Employee employee)
        //    {
        //        context.Employees.Update(employee);
        //        return context.SaveChanges();
        //    } 

        #endregion

        private readonly MvcDbContext context;
        public EmployeeRepository(MvcDbContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<Employee>> GetEmployeeByDepartmentId(int? departmentId)
        {
            return await context.Employees.Where(e => e.Department.Id.Equals(departmentId)).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchEmployeeByName(string name)
        {
            return await context.Employees.Where(e => e.Name.Contains(name)).ToListAsync();
        }
    }
}

