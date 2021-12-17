using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainer.BLL.DTO;
using Trainer.BLL.Infrastructure;
using Trainer.BLL.Interfaces;
using Trainer.DAL.Entities;
using Trainer.DAL.Interfaces;
using Trainer.DAL.Util.Constant;

namespace Trainer.BLL.Services
{

    public class ContextService : IContextService
    {
        private readonly ILogger<ContextService> _logger;
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public ContextService(IUnitOfWork uow, IMapper mapper, IMemoryCache memoryCache, ILogger<ContextService> logger)
        {
            _database = uow ?? throw new ArgumentNullException($"{nameof(uow)} is null.");
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mapper)} is null.");
            _cache = memoryCache ?? throw new ArgumentNullException($"{nameof(memoryCache)} is null.");
            _logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} is null.");
        }

        public async Task<IEnumerable<PatientDTO>> GetPatients(SortState sortOrder)
        {
            IEnumerable<PatientDTO> patientView = null;
            try
            {
                if (!_cache.TryGetValue("patients", out patientView))
                {
                    var patient = await _database.Patients.GetAll();
                    patientView = _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientDTO>>(patient);
                    _cache.Set("patients", patientView, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }
                switch (sortOrder)
                {
                    case SortState.FirstNameSort:
                        patientView = patientView.OrderBy(s => s.FirstName);
                        break;
                    case SortState.FirstNameSortDesc:
                        patientView = patientView.OrderByDescending(s => s.FirstName);
                        break;
                    case SortState.MiddleNameSortDesc:
                        patientView = patientView.OrderByDescending(s => s.MiddleName);
                        break;
                    case SortState.MiddleNameSort:
                        patientView = patientView.OrderBy(s => s.MiddleName);
                        break;
                    case SortState.LastNameSortDesc:
                        patientView = patientView.OrderByDescending(s => s.LastName);
                        break;
                    case SortState.LastNameSort:
                        patientView = patientView.OrderBy(s => s.LastName);
                        break;
                    case SortState.AgeSort:
                        patientView = patientView.OrderBy(s => s.Age);
                        break;
                    case SortState.AgeSortDesc:
                        patientView = patientView.OrderByDescending(s => s.Age);
                        break;
                    case SortState.SexSort:
                        patientView = patientView.OrderBy(s => s.Sex);
                        break;
                    case SortState.SexSortDesc:
                        patientView = patientView.OrderByDescending(s => s.Sex);
                        break;
                }

                _logger.LogInformation("Patients received successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

            return patientView;
        }

        public async Task<PatientDTO> GetPatient(Guid id)
        {
            PatientDTO patientDTO = null;
            try
            {
                Patient patient = await _database.Patients.Get(id);
                if (patient != null)
                {
                    patientDTO = _mapper.Map<PatientDTO>(patient);
                    _logger.LogInformation("Patient received successfully");
                }
                else
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }


            return patientDTO;
        }

        public async Task<PatientDTO> Create(PatientDTO peopleDto)
        {
            PatientDTO returnPatient = null;
            try
            {
                Patient newPatient = _mapper.Map<Patient>(peopleDto);
                returnPatient = _mapper.Map<PatientDTO>(await _database.Patients.Create(newPatient));
                _cache.Remove("patients");
                _logger.LogInformation("Patient created successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

            return returnPatient;
        }

        public async Task<PatientDTO> Update(PatientDTO peopleDto)
        {
            PatientDTO returnPatient = null;
            try
            {
                Patient patient = await _database.Patients.Get(peopleDto.Id);
                if (patient == null)
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }

                patient.Age = peopleDto.Age;
                patient.Email = peopleDto.Email;
                patient.FirstName = peopleDto.FirstName;
                patient.LastName = peopleDto.LastName;
                patient.MiddleName = peopleDto.MiddleName;
                patient.Sex = peopleDto.Sex;
                patient.About = peopleDto.About;
                patient.Hobbies = peopleDto.Hobbies;

                returnPatient = _mapper.Map<PatientDTO>(await _database.Patients.Update(patient));
                _cache.Remove("patients");
                _logger.LogInformation("Patient updated successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

            return returnPatient;
        }

        public async Task DeletePatient(Guid id)
        {
            try
            {
                var people = await _database.Patients.Get(id);
                if (people == null)
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }

                await _database.Patients.Delete(people.Id);
                _cache.Remove("patients");
                _logger.LogInformation("Patient deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }
        }

        public async Task<ExaminationDTO> GetExamination(Guid id)
        {
            ExaminationDTO examinationDTO = null;
            try
            {
                Examination examination = await _database.Examinations.Get(id);
                if (examination != null)
                {
                    examinationDTO = _mapper.Map<ExaminationDTO>(examination);
                    _logger.LogInformation("Examination received successfully");
                }
                else
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }


            return examinationDTO;
        }

        public async Task<IEnumerable<ExaminationDTO>> GetExaminations(SortState sortOrder)
        {
            IEnumerable<ExaminationDTO> examinationView = null;
            try
            {
                if (!_cache.TryGetValue("examinations", out examinationView))
                {
                    var examination = await _database.Examinations.GetAll();
                    examinationView = _mapper.Map<IEnumerable<Examination>, IEnumerable<ExaminationDTO>>(examination);
                    _cache.Set("examinations", examinationView, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }

                switch (sortOrder)
                {
                    case SortState.DateSort:
                        examinationView = examinationView.OrderBy(s => s.Date);
                        break;
                    case SortState.DateSortDesc:
                        examinationView = examinationView.OrderByDescending(s => s.Date);
                        break;
                    case SortState.TypeSort:
                        examinationView = examinationView.OrderBy(s => s.TypePhysicalActive);
                        break;
                    case SortState.TypeSortDesc:
                        examinationView = examinationView.OrderByDescending(s => s.TypePhysicalActive);
                        break;
                    case SortState.FirstNameSort:
                        examinationView = examinationView.OrderBy(s => s.Patient.FirstName);
                        break;
                    case SortState.FirstNameSortDesc:
                        examinationView = examinationView.OrderByDescending(s => s.Patient.FirstName);
                        break;
                    case SortState.MiddleNameSortDesc:
                        examinationView = examinationView.OrderByDescending(s => s.Patient.MiddleName);
                        break;
                    case SortState.MiddleNameSort:
                        examinationView = examinationView.OrderBy(s => s.Patient.MiddleName);
                        break;
                    case SortState.LastNameSortDesc:
                        examinationView = examinationView.OrderByDescending(s => s.Patient.LastName);
                        break;
                    case SortState.LastNameSort:
                        examinationView = examinationView.OrderBy(s => s.Patient.LastName);
                        break;
                }
                _logger.LogInformation("Examinations received successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }


            return examinationView;
        }

        public async Task<ExaminationDTO> Create(ExaminationDTO examinationDTO)
        {
            ExaminationDTO returnExamination = null;
            try
            {
                Examination newExamination = _mapper.Map<Examination>(examinationDTO);

                returnExamination = _mapper.Map<ExaminationDTO>(await _database.Examinations.Create(newExamination));
                _cache.Remove("examinations");
                _logger.LogInformation("Examination create successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

            return returnExamination;
        }

        public async Task<ExaminationDTO> Update(ExaminationDTO examinationDTO)
        {
            ExaminationDTO returnExamination = null;
            try
            {
                Examination examination = await _database.Examinations.Get(examinationDTO.Id);
                if (examination == null)
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }

                examination.Indicators = examinationDTO.Indicators;
                examination.TypePhysicalActive = examinationDTO.TypePhysicalActive;
                examination.Status = examinationDTO.Status;

                returnExamination = _mapper.Map<ExaminationDTO>(await _database.Examinations.Update(examination));
                _cache.Remove("examinations");
                _logger.LogInformation("Examination update successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }


            return returnExamination;
        }

        public async Task DeleteExamination(Guid id)
        {
            try
            {
                var examination = await _database.Examinations.Get(id);
                if (examination == null)
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }

                await _database.Examinations.Delete(examination.Id);
                _cache.Remove("examinations");
                _logger.LogInformation("Examination delete successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }
        }

        public async Task<ResultsDTO> GetResult(Guid id)
        {
            ResultsDTO resultsDTO = null;
            try
            {
                Results examination = await _database.Results.Get(id);
                if (examination != null)
                {
                    resultsDTO = _mapper.Map<ResultsDTO>(examination);
                    _logger.LogInformation("Result received successfully");
                }
                else
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

            return resultsDTO;
        }

        public async Task<IEnumerable<ResultsDTO>> GetResults()
        {
            IEnumerable<ResultsDTO> resultsView = null;
            try
            {
                var results = await _database.Results.GetAll();
                resultsView = _mapper.Map<IEnumerable<Results>, IEnumerable<ResultsDTO>>(results);
                _logger.LogInformation("Results received successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

            return resultsView;
        }

        public async Task<IEnumerable<ResultsDTO>> GetPatientResults(Guid id)
        {
            IEnumerable<ResultsDTO> resultsView = null;
            try
            {
                var results = await _database.GetPatientResults(id);
                resultsView = _mapper.Map<IEnumerable<Results>, IEnumerable<ResultsDTO>>(results);
                _logger.LogInformation("Patient results received successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

            return resultsView;
        }

        public async Task<ResultsDTO> Create(ResultsDTO examinationDTO)
        {
            ResultsDTO returnResults = null;
            try
            {
                Results newResults = _mapper.Map<Results>(examinationDTO);

                returnResults = _mapper.Map<ResultsDTO>(await _database.Results.Create(newResults));
                _logger.LogInformation("Results create successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }


            return returnResults;
        }

        public async Task<ResultsDTO> Update(ResultsDTO resultsDTO)
        {
            ResultsDTO returnResults = null;
            try
            {
                Results results = await _database.Results.Get(resultsDTO.Id);
                if (results == null)
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }

                results.AverageDia = resultsDTO.AverageDia;
                results.AverageHeartRate = resultsDTO.AverageHeartRate;
                results.AverageSis = resultsDTO.AverageSis;
                results.AverageOxigen = resultsDTO.AverageOxigen;
                results.AverageTemperature = resultsDTO.AverageTemperature;

                returnResults = _mapper.Map<ResultsDTO>(await _database.Results.Update(results));
                _logger.LogInformation("Results update successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }


            return returnResults;
        }

        public async Task DeleteResults(Guid id)
        {
            try
            {
                var result = await _database.Results.Get(id);
                if (result == null)
                {
                    throw new RecordNotFoundException("Record not found. Check id");
                }

                await _database.Results.Delete(result.Id);
                _logger.LogInformation("Results update successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

        }
    }
}
