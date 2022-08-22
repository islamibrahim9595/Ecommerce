using Demo.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepositary DepartmentRepositary { get; set; }

        public UnitOfWork(IEmployeeRepository employeeRepository, IDepartmentRepositary departmentRepositary)
        {
            EmployeeRepository = employeeRepository;
            DepartmentRepositary = departmentRepositary;
        }
    }
}
