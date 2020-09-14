using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameReview.Models
{
    public class ProsPoint
    {
        [Key]
        public int ID { get; set; }
        public string Pros { get; set; }
        public Review Review { get; set; }

    }
}
