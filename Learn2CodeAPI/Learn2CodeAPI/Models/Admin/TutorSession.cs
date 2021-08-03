﻿using Learn2CodeAPI.Models.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Learn2CodeAPI.Models.Admin
{
    public class TutorSession : BaseEntity
    {
        public int SessionTypeId { get; set; }

        [ForeignKey("SessionTypeId")]
        public SessionType SessionType { get; set; }

        public ICollection<TutorSessionModule> TutorSessionModule { get; set; }
        public ICollection<SubscriptionTutorSession> SubscriptionTutorSession { get; set; }
    }
}