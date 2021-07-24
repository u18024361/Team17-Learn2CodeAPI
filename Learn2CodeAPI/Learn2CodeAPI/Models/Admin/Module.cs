﻿using Learn2CodeAPI.Models.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Learn2CodeAPI.Models.Student;

namespace Learn2CodeAPI.Models.Admin
{
    public class Module : BaseEntity
    {
        public Module() { }
        public string ModuleCode { get; set; }

        public ICollection<StudentModule> StudentModule { get; set; }

        public int DegreeId { get; set; }

        [ForeignKey("DegreeId")]
        public Degree Degree { get; set; }

        
    }
}
