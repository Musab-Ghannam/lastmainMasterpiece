//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mainMasterpiesce.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class transactionsdoctor
    {
        public int transDoctorId { get; set; }
        public Nullable<int> doctorId { get; set; }
        public string transactiontype { get; set; }
        public Nullable<double> amount { get; set; }
        public string paymentmethod { get; set; }
        public Nullable<System.DateTime> transactionDate { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> Tansactiontime { get; set; }
        public string DOctorName { get; set; }
    
        public virtual doctor doctor { get; set; }
    }
}
