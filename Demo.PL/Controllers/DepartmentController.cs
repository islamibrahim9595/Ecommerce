using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
       
        private readonly IUnitOfWork unitofwork;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork unitofwork, IMapper mapper)
        {
            this.unitofwork = unitofwork;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var mappedDepartment = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(await unitofwork.DepartmentRepositary.GetAll());
                return View(mappedDepartment);

            }
            else
            {
                var mappedDepartment = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(unitofwork.DepartmentRepositary.SearchDepartmentByName(SearchValue));
                return View(mappedDepartment);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                #region AutoMapper

                var mappedDepartmentAutoMapper = mapper.Map<DepartmentViewModel, Department>(departmentVM);

                #endregion

                await unitofwork.DepartmentRepositary.Add(mappedDepartmentAutoMapper);
                TempData["Message"] = "Department is Created Successfuly";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(departmentVM);
            }
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var Department = mapper.Map<Department, DepartmentViewModel>(await unitofwork.DepartmentRepositary.Get(id));
            if (Department == null)
                return NotFound();
            return View(ViewName, Department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //    return NotFound();
            //var Department = departmentRepositary.Get(id);
            //if (Department == null)
            //    return NotFound();
            //return View(Department);
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDep = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    await unitofwork.DepartmentRepositary.Update(mappedDep);
                    TempData["Message"] = "Department updated";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(departmentVM);
                }
            }
            return View(departmentVM);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            try
            {
                var mappedDepartment = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await unitofwork.DepartmentRepositary.Delete(mappedDepartment);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(departmentVM);
            }

        }

    }
}
