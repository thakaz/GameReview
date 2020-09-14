﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameReview.Models
{
    public class ConsPoint
    {
        [Key]
        public int ID { get; set; }
        public string Cons { get; set; }
        public Review Review { get; set; }
    }
}
