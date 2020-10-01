using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameReview.Models.ViewModel
{
    public class IndexVM
    {

        public enum SortCondEnum
        {

            [Display(Name = "デフォルト")]
            not_sort,
            [Display(Name = "タイトル")]
            title,
            [Display(Name = "評価")]
            grade,
            [Display(Name = "リリース日")]
            release_date
        }

        public IEnumerable<Game> Game { get; set; }

        public SortCondEnum? SortCond { get; set; }

    }
}
