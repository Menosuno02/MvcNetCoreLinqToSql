﻿using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSql.Models;
using MvcNetCoreLinqToSql.Repositories;

namespace MvcNetCoreLinqToSql.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController()
        {
            this.repo = new RepositoryEmpleados();
        }

        public IActionResult Index()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }

        public IActionResult Details(int id)
        {
            Empleado empleado = this.repo.FindEmpleado(id);
            return View(empleado);
        }

        public IActionResult BuscadorEmpleados()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }

        [HttpPost]
        public IActionResult BuscadorEmpleados(string oficio, int salario)
        {
            List<Empleado> empleados = this.repo.GetEmpleadosOficioSalario(oficio, salario);
            if (empleados == null)
            {
                ViewData["MENSAJE"] = "No se encontraron registros con oficio " + oficio + " y salario mayor a " + salario;
                return View();
            }
            return View(empleados);
        }

        public IActionResult DatosEmpleados()
        {
            ViewData["OFICIOS"] = this.repo.GetOficios();
            return View();
        }

        [HttpPost]
        public IActionResult DatosEmpleados(string oficio)
        {
            ViewData["OFICIOS"] = this.repo.GetOficios();
            ResumenEmpleados resumen = this.repo.GetEmpleadosOficio(oficio);
            return View(resumen);
        }

        public IActionResult EmpleadosDepartamento()
        {
            ViewData["DEPT"] = this.repo.GetDepartamentos();
            return View();
        }

        [HttpPost]
        public IActionResult EmpleadosDepartamento(int dept_no)
        {
            ViewData["DEPT"] = this.repo.GetDepartamentos();
            ResumenEmpleados resumen = this.repo.GetEmpleadosDept(dept_no);
            if (resumen == null)
            {
                ViewData["MENSAJE"] = "No se encontraron empleados del departamento " + dept_no;
                return View();
            }
            return View(resumen);
        }
    }
}
