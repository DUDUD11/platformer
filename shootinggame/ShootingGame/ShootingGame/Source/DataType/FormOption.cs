using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    public class FormOption
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public FormOption(string name,object value) {
            Name = name;
            Value = value;
        }

    }
}
