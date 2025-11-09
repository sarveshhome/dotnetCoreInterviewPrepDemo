
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreInterviewPrepDemo.Model
{
    public class Patient
    {
        public int id { get; set; }

        public string patientName { get; set; }

        public List<Problem> Problems { get; set; }

        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public bool IsLatestVersion { get; set; }

        public int? PreviousVersionId { get; set; }
    }
    public class Problem
    {
        public int id { get; set; }

        public string  description { get; set; }
    }
}
