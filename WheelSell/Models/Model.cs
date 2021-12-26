using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WheelSell.Models
{
    public class Model
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Name { get; set; }

        public Make Make { get; set; }

        [ForeignKey("Make")]
        public int MakeID { get; set; }

    }
}
