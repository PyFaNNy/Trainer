using AutoMapper;
using Trainer.BLL.DTO;
using Trainer.DAL.Entities;
using Trainer.Models;

namespace Trainer.Util
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<PatientViewModel, PatientDTO>();
            CreateMap<PatientDTO, PatientViewModel>();
            CreateMap<Patient, PatientDTO>();
            CreateMap<PatientDTO, Patient>();

            CreateMap<ExaminationViewModel, ExaminationDTO>();
            CreateMap<ExaminationDTO, ExaminationViewModel>();
            CreateMap<Examination, ExaminationDTO>();
            CreateMap<ExaminationDTO, Examination>();
        }
    }
}
