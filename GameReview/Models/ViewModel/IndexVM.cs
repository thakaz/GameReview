using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReview.Models.ViewModel
{
    public class IndexVM
    {
        public IEnumerable<Game> Game { get; set; }
        public string SortCol { get; set; }
        public string AscDesc { get; set; }

    }
}
