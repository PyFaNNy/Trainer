using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trainer.BLL.DTO;
using Trainer.BLL.Infrastructure;
using Trainer.BLL.Interfaces;
using Trainer.Chart;
using Trainer.DAL.Util.Constant;
using Trainer.Models;
using Trainer.Util;

namespace Trainer.Controllers
{
    [Authorize(Roles = "doctor")]
    public class ExaminationController : Controller
    {
        private readonly IContextService _contextService;
        private readonly IMapper _mapper;
        private readonly ExaminationValidator _validator;
        private readonly IHubContext<ChartHub> _chartHub;
        public ExaminationController(IContextService serv, ExaminationValidator validator, IMapper mapper, IHubContext<ChartHub> chartHub)
        {
            _contextService = serv ?? throw new ArgumentNullException($"{nameof(serv)} is null.");
            _validator = validator ?? throw new ArgumentNullException($"{nameof(validator)} is null.");
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mapper)} is null.");
            _chartHub = chartHub ?? throw new ArgumentNullException($"{nameof(chartHub)} is null.");
        }

        [HttpGet]
        public async Task<IActionResult> GetModels(SortState sortOrder = SortState.FirstNameSort)
        {
            ViewData["FirstNameSort"] = sortOrder == SortState.FirstNameSort ? SortState.FirstNameSortDesc : SortState.FirstNameSort;
            ViewData["LastNameSort"] = sortOrder == SortState.LastNameSort ? SortState.LastNameSortDesc : SortState.LastNameSort;
            ViewData["MiddleNameSort"] = sortOrder == SortState.MiddleNameSort ? SortState.MiddleNameSortDesc : SortState.MiddleNameSort;

            IEnumerable<ExaminationDTO> examinationDtos = await _contextService.GetExaminations(sortOrder);
            var examinations = _mapper.Map<List<ExaminationViewModel>>(examinationDtos);
            return View(examinations);
        }

        [HttpGet]
        public async Task<IActionResult> GetModel(Guid id)
        {
            try
            {
                ExaminationDTO examinationDTO = await _contextService.GetExamination(id);
                var examinationView = _mapper.Map<ExaminationViewModel>(examinationDTO);
                ViewBag.Id = examinationView.Id;
                InvCountIndicators(examinationView);
                return View(examinationView);
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddModel(Guid id)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(ExaminationViewModel model, Guid patientid)
        {
            try
            {
                model.Date = DateTime.UtcNow;
                CountIndicators(model);
                _validator.ValidateAndThrow(model);
                var examinationDto = _mapper.Map<ExaminationDTO>(model);
                await _contextService.Create(examinationDto);
                return RedirectToAction("GetModels");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateModel(Guid id)
        {
            ExaminationDTO examinationDTO = await _contextService.GetExamination(id);
            var examinationView = _mapper.Map<ExaminationViewModel>(examinationDTO);
            InvCountIndicators(examinationView);
            ViewBag.Examination = examinationView;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateModel(ExaminationViewModel model, Guid patientid)
        {
            try
            {
                model.Date = DateTime.UtcNow;
                CountIndicators(model);
                _validator.ValidateAndThrow(model);
                var examinationDto = _mapper.Map<ExaminationDTO>(model);
                await _contextService.Update(examinationDto);
                return RedirectToAction("GetModels");
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        public async Task<RedirectToActionResult> DeleteModel(Guid[] selectedExamination)
        {
            foreach (var patient in selectedExamination)
            {
                await _contextService.DeleteExamination(patient);
            }
            return RedirectToAction("GetModels");
        }

        private void CountIndicators(ExaminationViewModel model)
        {
            model.Indicators = 0;
            if (model.Indicator1)
            {
                model.Indicators += 1;
            }
            if (model.Indicator2)
            {
                model.Indicators += 2;
            }
            if (model.Indicator3)
            {
                model.Indicators += 4;
            }
            if (model.Indicator4)
            {
                model.Indicators += 8;
            }
            if (model.Indicator5)
            {
                model.Indicators += 16;
            }
        }
        private void InvCountIndicators(ExaminationViewModel model)
        {
            var temp = model.Indicators;
            if (temp - 16 >= 0)
            {
                temp -= 16;
                model.Indicator5 = true;
            }
            if (temp - 8 >= 0)
            {
                temp -= 8;
                model.Indicator4 = true;
            }
            if (temp - 4 >= 0)
            {
                temp -= 4;
                model.Indicator3 = true;
            }
            if (temp - 2 >= 0)
            {
                temp -= 2;
                model.Indicator2 = true;
            }
            if (temp - 1 >= 0)
            {
                temp -= 1;
                model.Indicator1 = true;
            }
        }
    }
}
