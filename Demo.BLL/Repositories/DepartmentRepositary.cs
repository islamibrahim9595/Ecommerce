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
    public class DepartmentRepositary : GenericRepository<Department>, IDepartmentRepositary
    {
        private readonly MvcDbContext context;

        #region Old
        //private readonly MvcDbContext context;

        //public DepartmentRepositary(MvcDbContext _context)
        //{
        //    context = _context;
        //}

        //public int Add(Department department)
        //{
        //    context.Departments.Add(department);
        //    return context.SaveChanges();

        //}

        //public int Delete(Department department)
        //{
        //    context.Departments.Remove(department);
        //    return context.SaveChanges();
        //}

        //public Department Get(int? id)
        //    => context.Departments.FirstOrDefault(D => D.Id == id);



        //public IEnumerable<Department> GetAll()
        //    => context.Departments.ToList();


        //public int Update(Department department)
        //{
        //    context.Departments.Update(department);
        //    return context.SaveChanges();
        //} 
        #endregion
        public DepartmentRepositary(MvcDbContext context) : base(context)
        {
            this.context = context;
        }


        public IEnumerable<Department> SearchDepartmentByName(string name)
            => context.Departments.Where(D => D.Name.Contains(name));

        
    }

}
