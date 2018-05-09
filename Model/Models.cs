using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toramp.Model
{
    class Serial
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int YearStart { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }
    }


    class Ganre
    {
        public string Name { get; set; }
    }

    class Chanale
    {
        public string Name { get; set; }
    }
}
