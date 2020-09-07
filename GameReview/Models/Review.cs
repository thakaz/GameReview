using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReview.Models
{

    public enum Grade
    {
        A,B,C,D,E,
    }
        


    public class Review
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int ReviewerID { get; set; }


        public Grade? Grade { get; set; }
        public string Summary { get; set; }

        public Game Game { get; set; }
        public Reviewer Reviewer { get; set; }

    }
}
