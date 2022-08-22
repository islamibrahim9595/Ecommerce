using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var mappedEmpAuto_Mapper = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(await unitOfWork.EmployeeRepository.GetAll());
                //ViewData["Message"] = "Hello View Data";
                return View(mappedEmpAuto_Mapper);
            }
            else
            {
                var mappedEmpAuto_Mapper = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(await unitOfWork.EmployeeRepository.SearchEmployeeByName(SearchValue));
                return View(mappedEmpAuto_Mapper);

            }
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Message = "Hello View Bag";
            ViewBag.Departments = await unitOfWork.DepartmentRepositary.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                #region Manual Mapping

                //var mappedEmp = new Employee()
                //{
                //    Id = employeeVM.Id,
                //    Name = employeeVM.Name,
                //    Age = employeeVM.Age,
                //    Address = employeeVM.Address,
                //    Salary = employeeVM.Salary,
                //    IsActive = employeeVM.IsActive,
                //    Email = employeeVM.Email,
                //    PhoneNumber = employeeVM.PhoneNumber,
                //    HireDate = employeeVM.HireDate,
                //    DepartmentId = employeeVM.DepartmentId
                //};

                #endregion

                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                #region AutoMapper

                var mappedEmpAutoMapper = mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                #endregion

                await unitOfWork.EmployeeRepository.Add(mappedEmpAutoMapper);
                TempData["Message"] = "Employee is Created Successfuly";

                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            //var Employee = employeeRepository.Get(id);
            var Employee = mapper.Map<Employee, EmployeeViewModel>(await unitOfWork.EmployeeRepository.Get(id));
            if (Employee == null)
                return NotFound();
            return View(ViewName, Employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments = unitOfWork.DepartmentRepositary.GetAll();

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditAsync([FromRoute] int? id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmpAutoMapper = mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    await unitOfWork.EmployeeRepository.Update(mappedEmpAutoMapper);
                    TempData["Message"] = "Employee Updated";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(employeeVM);
                }
            }

            return View(employeeVM);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            try
            {
                await unitOfWork.EmployeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(employee);
            }
        }

    }
}
