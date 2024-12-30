using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTechLabs.DAL.Entities
{
    public class DishGroup
    {
        public DishGroup()
        {
            Dishes = new List<Dish>();
        }

        public int DishGroupId { get; set; }

        public string GroupName { get; set; } = null!;

        /// <summary>
        /// Навигационное свойство 1-ко-многим
        /// </summary>
        public List<Dish> Dishes { get; set; }
    }

}
