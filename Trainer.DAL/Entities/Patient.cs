using System;
using System.Collections.Generic;
using Trainer.DAL.Util.Constant;

namespace Trainer.DAL.Entities
{
    public class Patient
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

        public IList<Examination> Examinations
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
