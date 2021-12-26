using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WheelSell.Models;

namespace WheelSell.Controllers.Resources
{
    public class ModelResources 
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
