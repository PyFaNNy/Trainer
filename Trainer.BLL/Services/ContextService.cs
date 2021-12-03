using AutoMapper;
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
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public ContextService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow ?? throw new ArgumentNullException($"{nameof(uow)} is null.");
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mapper)} is null.");
        }

        public async Task<IEnumerable<PatientDTO>> GetPatients(SortState sortOrder)
        {
            var people = await _database.Patients.GetAll();
            var peopleView = _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientDTO>>(people);

            switch (sortOrder)
            {
                case SortState.FirstNameSort:
                    peopleView = peopleView.OrderBy(s => s.FirstName);
                    break;
                case SortState.FirstNameSortDesc:
                    peopleView = peopleView.OrderByDescending(s => s.FirstName);
                    break;
                case SortState.MiddleNameSortDesc:
                    peopleView = peopleView.OrderByDescending(s => s.MiddleName);
                    break;
                case SortState.MiddleNameSort:
                    peopleView = peopleView.OrderBy(s => s.MiddleName);
                    break;
                case SortState.LastNameSortDesc:
                    peopleView = peopleView.OrderByDescending(s => s.LastName);
                    break;
                case SortState.LastNameSort:
                    peopleView = peopleView.OrderBy(s => s.LastName);
                    break;
                case SortState.AgeSort:
                    peopleView = peopleView.OrderBy(s => s.Age);
                    break;
                case SortState.AgeSortDesc:
                    peopleView = peopleView.OrderByDescending(s => s.Age);
                    break;
                case SortState.SexSort:
                    peopleView = peopleView.OrderBy(s => s.Sex);
                    break;
                case SortState.SexSortDesc:
                    peopleView = peopleView.OrderByDescending(s => s.Sex);
                    break;
            }

            return peopleView;
        }

        public async Task<PatientDTO> GetPatient(Guid id)
        {
            PatientDTO peopleDTO = null;
            Patient people = await _database.Patients.Get(id);
            if (people != null)
            {
                peopleDTO = _mapper.Map<PatientDTO>(people);
            }
            else
            {
                throw new RecordNotFoundException("Record not found. Check id");
            }

            return peopleDTO;
        }

        public async Task<PatientDTO> Create(PatientDTO peopleDto)
        {
            Patient newPeople = _mapper.Map<Patient>(peopleDto);
            var returnPeople = _mapper.Map<PatientDTO>(await _database.Patients.Create(newPeople));

            return returnPeople;
        }

        public async Task<PatientDTO> Update(PatientDTO peopleDto)
        {
            Patient people = await _database.Patients.Get(peopleDto.Id);
            if (people == null)
            {
                throw new RecordNotFoundException("Record not found. Check id");
            }

            people.Age = peopleDto.Age;
            people.FirstName = peopleDto.FirstName;
            people.LastName = peopleDto.LastName;
            people.MiddleName = peopleDto.MiddleName;
            people.Sex = peopleDto.Sex;
            people.About = peopleDto.About;
            people.Hobbies = peopleDto.Hobbies;

            var returnPeople = _mapper.Map<PatientDTO>(await _database.Patients.Update(people));

            return returnPeople;
        }

        public async Task DeletePatient(Guid id)
        {
            var people = await _database.Patients.Get(id);
            if (people == null)
            {
                throw new RecordNotFoundException("Record not found. Check id");
            }

            await _database.Patients.Delete(people.Id);
        }

        public async Task<ExaminationDTO> GetExamination(Guid id)
        {
            ExaminationDTO examinationDTO = null;
            Examination examination = await _database.Examinations.Get(id);
            if (examination != null)
            {
                examinationDTO = _mapper.Map<ExaminationDTO>(examination);
            }
            else
            {
                throw new RecordNotFoundException("Record not found. Check id");
            }

            return examinationDTO;
        }

        public async Task<IEnumerable<ExaminationDTO>> GetExaminations(SortState sortOrder)
        {
            var examination = await _database.Examinations.GetAll();
            var examinationView = _mapper.Map<IEnumerable<Examination>, IEnumerable<ExaminationDTO>>(examination);

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

            return examinationView;
        }

        public async Task<ExaminationDTO> Create(ExaminationDTO examinationDTO)
        {
            Examination newExamination = _mapper.Map<Examination>(examinationDTO);

            var returnExamination = _mapper.Map<ExaminationDTO>(await _database.Examinations.Create(newExamination));

            return returnExamination;
        }

        public async Task<ExaminationDTO> Update(ExaminationDTO examinationDTO)
        {
            Examination examination = await _database.Examinations.Get(examinationDTO.Id);
            if (examination == null)
            {
                throw new RecordNotFoundException("Record not found. Check id");
            }

            examination.Indicators = examinationDTO.Indicators;
            examination.TypePhysicalActive = examinationDTO.TypePhysicalActive;
            examination.Status = examinationDTO.Status;

            var returnExamination = _mapper.Map<ExaminationDTO>(await _database.Examinations.Update(examination));

            return returnExamination;
        }

        public async Task DeleteExamination(Guid id)
        {
            var examination = await _database.Examinations.Get(id);
            if (examination == null)
            {
                throw new RecordNotFoundException("Record not found. Check id");
            }

            await _database.Examinations.Delete(examination.Id);
        }
    }
}
