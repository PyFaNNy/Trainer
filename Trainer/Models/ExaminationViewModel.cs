namespace Trainer.Models
{
    using  System;
    using Trainer.DAL.Util.Constant;

    public class ExaminationViewModel
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

        public bool Indicator1
        {
            get;
            set;
        }

        public bool Indicator2
        {
            get;
            set;
        }

        public bool Indicator3
        {
            get;
            set;
        }

        public bool Indicator4
        {
            get;
            set;
        }

        public bool Indicator5
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

        public PatientViewModel Patient
        {
            get;
            set;
        }
    }
}
