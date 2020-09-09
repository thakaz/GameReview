using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GameReview.Models
{
    public class Game
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Thumbnail")]
        public string ImagePath { get; set; }

        public ICollection<GameGenre> GameGenres { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
