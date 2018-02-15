using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMC_NET.Tests.TestEntities
{
    public class LaptopEntity : EntityBase
    {
        public string Model { get; set; }

        public int RAM { get; set; }

        public int SSD { get; set; }

        public int HDD { get; set; }

    }
}
