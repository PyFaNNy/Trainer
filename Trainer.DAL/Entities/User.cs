using Microsoft.AspNetCore.Identity;

namespace Trainer.DAL.Entities
{
    public class User : IdentityUser
    {
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

        public string Password 
        {
            get;
            set;
        }

        public bool RememberMe 
        { 
            get;
            set;
        }

    }
}
