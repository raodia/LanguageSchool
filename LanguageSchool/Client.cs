namespace LanguageSchool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.ClientService = new HashSet<ClientService>();
        }

        public string LastSignUp
        {
            get
            {
 
                var a = "нет";
                var last = ClientService.Where(p => p.ClientID == this.ID).ToList();
                last = last.OrderByDescending(p => p.StartTime).ToList();
                //if (last.Count > 0)
                //{

                //var result = last[0].StartTime;
                //for (var i = 0; i < last.Count; i++)
                //{
                //    if (result > last[i].StartTime) 
                //            result = last[i].StartTime;
                        
                //}
                //    a = result.ToShortDateString();
                //}

                if (last.Count > 0)
                {
                    a = last[0].StartTime.ToShortDateString();
                }
                // iiioioaf


                return a;
            }
        }

        public System.DateTime LastSignUpDate
        {
            get
            {
                DateTime a = DateTime.MinValue;
                var last = LastSignUp == "нет" ? a : DateTime.Parse(LastSignUp);
                

                return last;
            }
        }

        public string GenderFull
        {
            get
            {
                return Gender.Name;
            }
        }

        public string RegDateShort
        {
            get
            {
                if (RegistrationDate != null)
                {
                    return RegistrationDate.ToShortDateString();
                }
                else
                {
                    return "нет";
                }
            }
        }

        public int SignUpCount
        {
            get
            {
                return ClientService.Count(p => p.ClientID == this.ID);
            }
        }
    
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string GenderCode { get; set; }
        public string Phone { get; set; }
        public string PhotoPath { get; set; }
        public System.DateTime Birthday { get; set; }

        public string BirthdayCool
        {
            get
            {
                return Birthday.ToShortDateString();
            }
        }

        public string Email { get; set; }
        public System.DateTime RegistrationDate { get; set; }
    
        public virtual Gender Gender { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientService> ClientService { get; set; }
    }
}
