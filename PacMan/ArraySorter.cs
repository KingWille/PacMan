using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    class ArraySorter : IComparer<object>
    {
        public int Compare(object x, object y)
        {
            return new CaseInsensitiveComparer().Compare((x as Node).TotalDistance, (y as Node).TotalDistance);
        }
    }
}
