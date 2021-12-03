using System;
using Trainer.DAL.Util.Constant;

namespace Trainer.Models
{
    public class PatientViewModel
    {
        public Guid Id
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public int Age
        {
            get;
            set;
        }

        public Sex Sex
        {
            get;
            set;
        }
        public string About
        {
            get;
            set;
        }
        public string Hobbies
        {
            get;
            set;
        }
    }
}
