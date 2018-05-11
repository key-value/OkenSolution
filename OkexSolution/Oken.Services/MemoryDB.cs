using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oken.Services
{
    public class MemoryDB
    {
        public static ConcurrentDictionary<string, Currencie> dictionary = new ConcurrentDictionary<string, Currencie>();
    }
}
