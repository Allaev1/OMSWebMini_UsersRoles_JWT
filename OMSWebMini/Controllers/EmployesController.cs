using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSWebMini.Data;
using OMSWebMini.Model;

namespace OMSWebMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployesController : ControllerBase
    {
        NorthwindContext northwindContext;

        public EmployesController(NorthwindContext northwindContext)
        {
            this.northwindContext = northwindContext;
        }

        //GET api/Employes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll(bool showReports = false, bool showPhoto = false)
        {
            var employes = await northwindContext.Employees.ToListAsync();

            if (showPhoto & showReports)
                return employes;
            else if (!showPhoto & !showReports)
                await Task.Run(() =>
                {
                    employes.ForEach(a =>
                    {
                        a.Photo = null;
                        a.ReportsToNavigation = null;
                        a.InverseReportsToNavigation = null;
                    });
                });
            else if (!showReports)
                await Task.Run(() => { employes.ForEach(a => a.ReportsToNavigation = null); });
            else
                await Task.Run(() => { employes.ForEach(a => a.Photo = null); });

            return employes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await northwindContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }
    }
}
