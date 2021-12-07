using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

    public class ExaminationController : Controller
    {
        private readonly IContextService _contextService;
        private readonly IMapper _mapper;
        private readonly ExaminationValidator _validator;
        private readonly IHubContext<ChartHub> _chartHub;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ExaminationController(IContextService serv, ExaminationValidator validator, IMapper mapper, IHubContext<ChartHub> chartHub, IHostingEnvironment hostingEnvironment)
        {
            _contextService = serv ?? throw new ArgumentNullException($"{nameof(serv)} is null.");
            _validator = validator ?? throw new ArgumentNullException($"{nameof(validator)} is null.");
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mapper)} is null.");
            _chartHub = chartHub ?? throw new ArgumentNullException($"{nameof(chartHub)} is null.");
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException($"{nameof(hostingEnvironment)} is null.");
        }

        [HttpGet]
        [Authorize(Roles = "admin,doctor")]
        public async Task<IActionResult> GetModels(SortState sortOrder = SortState.FirstNameSort)
        {
            ViewData["DateSort"] = sortOrder == SortState.DateSort ? SortState.DateSortDesc : SortState.DateSort;
            ViewData["TypeSort"] = sortOrder == SortState.TypeSort ? SortState.TypeSortDesc : SortState.TypeSort;
            ViewData["FirstNameSort"] = sortOrder == SortState.FirstNameSort ? SortState.FirstNameSortDesc : SortState.FirstNameSort;
            ViewData["LastNameSort"] = sortOrder == SortState.LastNameSort ? SortState.LastNameSortDesc : SortState.LastNameSort;
            ViewData["MiddleNameSort"] = sortOrder == SortState.MiddleNameSort ? SortState.MiddleNameSortDesc : SortState.MiddleNameSort;

            IEnumerable<ExaminationDTO> examinationDtos = await _contextService.GetExaminations(sortOrder);
            var examinations = _mapper.Map<List<ExaminationViewModel>>(examinationDtos);
            return View(examinations);
        }

        [HttpGet]
        [Authorize(Roles = "admin,doctor")]
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
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> AddModel(Guid id)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> AddModel(ExaminationViewModel model, Guid patientid)
        {
            try
            {
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
        [Authorize(Roles = "doctor")]
        public async Task<IActionResult> UpdateModel(Guid id)
        {
            ExaminationDTO examinationDTO = await _contextService.GetExamination(id);
            var examinationView = _mapper.Map<ExaminationViewModel>(examinationDTO);
            InvCountIndicators(examinationView);
            ViewBag.Examination = examinationView;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "doctor")]
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

        [Authorize(Roles = "doctor")]
        public async Task<RedirectToActionResult> DeleteModel(Guid[] selectedExamination)
        {
            foreach (var patient in selectedExamination)
            {
                await _contextService.DeleteExamination(patient);
            }
            return RedirectToAction("GetModels");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ExportToCSV()
        {
            try
            {
                var builder = new StringBuilder();
                builder.Append("Id;Type Physical Active;Last Name;First Name;Middle Name;Age;Tonometr;Termometr;Heart Rate;Oximetr;Date\n");
                IEnumerable<ExaminationDTO> patientsDTO = await _contextService.GetExaminations(SortState.FirstNameSort);
                var examinations = _mapper.Map<List<ExaminationViewModel>>(patientsDTO);
                foreach (var examination in examinations)
                {
                    InvCountIndicators(examination);
                    builder.AppendLine($"{examination.Id};{examination.TypePhysicalActive};{examination.Patient.LastName};" +
                        $"{examination.Patient.FirstName};{examination.Patient.MiddleName};{examination.Patient.Age};" +
                        $"{examination.Indicator1};{examination.Indicator2};{examination.Indicator3};{examination.Indicator4};{examination.Date}\n");
                }
                return File(Encoding.Unicode.GetBytes(builder.ToString()), "text/csv", fileDownloadName: "Examinations.csv");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize(Roles = "doctor")]
        public IActionResult ExportToPDF()
        {
            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings settings = new WebKitConverterSettings();
            //Set WebKit path
            settings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "QtBinariesWindows");
            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("https://www.google.com");
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output.pdf");
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
