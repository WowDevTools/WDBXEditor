using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBXEditor.ConsoleHandler
{
    class ConsoleHelpAttribute : Attribute
    {
        public string Information { get; set; }
        public string Arguments { get; set; }
        public string Example { get; set; }

        public ConsoleHelpAttribute(string information, string arguments, string example)
        {
            this.Information = information;
            this.Arguments = arguments;
            this.Example = example;
        }

        public void Print()
        {
            Console.WriteLine("   Description: " + Information);

            if (!string.IsNullOrEmpty(Arguments))
                Console.WriteLine("   Arguments: " + Arguments);

            if (!string.IsNullOrEmpty(Example))
                Console.WriteLine("   Example: " + Example);
        }

    }
}
