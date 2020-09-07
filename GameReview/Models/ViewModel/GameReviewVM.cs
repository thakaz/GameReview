using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace GameReview.Models.ViewModel
{
    public class GameReviewVM
    {
        public Game Game { get; set; }
        public Reviewer Reviewer { get; set; }
        public Review Review { get; set; }

        public IFormFile ImageFile { get; set; }


    }
}
