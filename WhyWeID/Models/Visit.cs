//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WhyWeID.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Visit
    {
        public int ID { get; set; }
        public string SchoolId { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Reason For Visit")]
        public int ReasonForVisit { get; set; }
        [Display(Name = "Area to Visit")]
        public int AreaToVisit { get; set; }
       [Range(2015, 3030, ErrorMessage = "Please enter valid school year")]
        public int SchoolYear { get; set; }
        [Display(Name ="Date of Visit")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM0-dd}")]
        public Nullable<System.DateTime> VisitDate { get; set; }
        [Display(Name = "Time In")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        public Nullable<System.TimeSpan> TimeIn { get; set; }
        [Display(Name = "Time out")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        public Nullable<System.TimeSpan> TimeOut { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            [Display(Name = "Full Name")]
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        [Display(Name = "Full Name")]
        public string FNFFullName
        {
            [Display(Name = "Full Name")]
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public virtual Area Area { get; set; }
        public virtual Reason Reason { get; set; }
    }
}
