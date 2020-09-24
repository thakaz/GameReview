using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameReview.Models
{
    public class Game
    {
        public int ID { get; set; }
        [Required]
        [DisplayName("タイトル")]
        public string Title { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [DisplayName("Thumbnail")]
        public string ImagePath { get; set; }

        public ICollection<GameGenre> GameGenres { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at {get;set;}


    }
}
