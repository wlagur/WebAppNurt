//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<int> IdType { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> IdUser { get; set; }
    
        public virtual TypeOfTransaction TypeOfTransaction { get; set; }
        public virtual User User { get; set; }
    }
}
