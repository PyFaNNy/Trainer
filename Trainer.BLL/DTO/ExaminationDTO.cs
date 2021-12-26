using CsvHelper.Configuration.Attributes;
using System;
using Trainer.DAL.Util.Constant;

namespace Trainer.BLL.DTO
{
    public class ExaminationDTO
    {
        public Guid Id
        {
            get;
            set;
        }

        public Guid PatientId
        {
            get;
            set;
        }

        public TypePhysicalActive TypePhysicalActive
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public int Indicators
        {
            get;
            set;
        }

        public Status Status
        {
            get;
            set;
        }

        public PatientDTO Patient
        {
            get;
            set;
        }
    }
}
