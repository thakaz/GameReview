using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameReview.Models
{

    public enum eGrade
    {
        A,B,C,D,E,
    }
        


    public class Review
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int ReviewerID { get; set; }

        [DisplayName("評価")]
        public eGrade? Grade { get; set; }
        [DisplayName("一言コメント")]
        public string Summary { get; set; }
        [DisplayName("感想")]
        public string Comment { get; set; }

        public Game Game { get; set; }
        public Reviewer Reviewer { get; set; }

        [DisplayName("特徴")]
        public string ProsPoints { get; set; }
        [DisplayName("不満点")]
        public string ConsPoints { get; set; }


        [DisplayName("作成日")]
        public DateTime? created_at { get; set; }

        [DisplayName("最終更新日")]
        public DateTime? updated_at { get; set; }
    }
}
