using System;

namespace Trainer.Models
{
    public class ResultsViewModel
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

        public Guid ExaminationId
        {
            get;
            set;
        }

        public int AverageHeartRate
        {
            get;
            set;
        }

        public int AverageDia
        {
            get;
            set;
        }

        public int AverageSis
        {
            get;
            set;
        }

        public int AverageOxigen
        {
            get;
            set;
        }

        public double AverageTemperature
        {
            get;
            set;
        }

        public PatientViewModel Patient
        {
            get;
            set;
        }

        public ExaminationViewModel Examination
        {
            get;
            set;
        }
    }
}
