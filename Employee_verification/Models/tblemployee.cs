//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Employee_verification.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblemployee
    {
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string employee_code { get; set; }
        public string email_address { get; set; }
        public string mobile_number { get; set; }
        public string designation { get; set; }
        public string salary { get; set; }
        public string pass { get; set; }

        public string otp { get; set;}
    }
}
