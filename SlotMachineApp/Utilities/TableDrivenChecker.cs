using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachineApp.Utilities
{
    public static class TableDrivenChecker
    {
        public static bool IsJackpot(List<string> items)
        {
            if (items == null || items.Count == 0) return false;
            string first = items[0];
            return items.All(item => item == first);
        }
    }
}
