using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trainer.BLL.DTO;
using Trainer.BLL.Infrastructure;
using Trainer.BLL.Interfaces;
using Trainer.DAL.Util.Constant;
using Trainer.Models;
using Trainer.Util;

namespace Trainer.Controllers
{
    public class PatientController : Controller
    {
        private readonly IContextService _contextService;
        private readonly IMapper _mapper;
        private readonly PatientValidator _validator;
        public PatientController(IContextService serv, PatientValidator validator, IMapper mapper)
        {
            _contextService = serv ?? throw new ArgumentNullException($"{nameof(serv)} is null.");
            _validator = validator ?? throw new ArgumentNullException($"{nameof(validator)} is null.");
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mapper)} is null.");
        }

        [HttpGet]
        [Authorize(Roles = "admin,doctor")]
        public async Task<IActionResult> GetModels(SortState sortOrder = SortState.FirstNameSort)
        {
            ViewData["FirstNameSort"] = sortOrder == SortState.FirstNameSort ? SortState.FirstNameSortDesc : SortState.FirstNameSort;
            ViewData["LastNameSort"] = sortOrder == SortState.LastNameSort ? SortState.LastNameSortDesc : SortState.LastNameSort;
            ViewData["MiddleNameSort"] = sortOrder == SortState.MiddleNameSort ? SortState.MiddleNameSortDesc : SortState.MiddleNameSort;
            ViewData["AgeSort"] = sortOrder == SortState.AgeSort ? SortState.AgeSortDesc : SortState.AgeSort;
            ViewData["SexSort"] = sortOrder == SortState.SexSort ? SortState.SexSortDesc : SortState.SexSort;
            IEnumerable<PatientDTO> peopleDtos = await _contextService.GetPatients(sortOrder);
            var peoples = _mapper.Map<List<PatientViewModel>>(peopleDtos);
            return View(peoples);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,doctor")]
        public async Task<IActionResult> GetModel(Guid id)
        {
            try
            {
                PatientDTO peopleDTO = await _contextService.GetPatient(id);
                var peopleView = _mapper.Map<PatientViewModel>(peopleDTO);
                return View(peopleView);
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AddModel()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddModel(PatientViewModel model)
        {
            try
            {
                _validator.ValidateAndThrow(model);
                var patientDto = _mapper.Map<PatientDTO>(model);
                await _contextService.Create(patientDto);
                return RedirectToAction("GetModels");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateModel(Guid id)
        {
            PatientDTO patientDTO = await _contextService.GetPatient(id);
            var patientView = _mapper.Map<PatientViewModel>(patientDTO);
            ViewBag.Patient = patientView;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateModel(PatientViewModel model)
        {
            try
            {
                _validator.ValidateAndThrow(model);
                var patientDto = _mapper.Map<PatientDTO>(model);
                await _contextService.Update(patientDto);
                return RedirectToAction("GetModels");
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
 
        [Authorize(Roles = "admin")]
        public async Task<RedirectToActionResult> DeleteModelAsync(Guid[] selectedPatient)
        {
            foreach (var patient in selectedPatient)
            {
                await _contextService.DeletePatient(patient);
            }
            return RedirectToAction("GetModels");
        }
    }
}
