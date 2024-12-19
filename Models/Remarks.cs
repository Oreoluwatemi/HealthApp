using System;
namespace OreoHealth.Models
{
    public class Remarks
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string description { get; set; }
        public Patient Patient { get; set; }
    }
}
