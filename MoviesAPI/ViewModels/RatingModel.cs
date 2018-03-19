using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.ViewModels
{
    public class RatingModel
    {
        [Required]
        public int MovieID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [Range(1, 5)]
        public int RatingValue { get; set; }
    }
}
