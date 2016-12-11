using System;
using System.IO;

namespace Launcher_new2
{
    class Cvars
    {
        private string doomEngine;
        private string doomIwads;
        private string doomPwads;
        private string cvars;

        public string Port
        {
            get { return doomEngine; }
            set { doomEngine = value; }

        }

        public string Iwads
        {
            get { return doomIwads; }
            set { doomIwads = value; }

        }
        public string Files
        {
            get { return doomPwads; }
            set { doomPwads = value; }
        }
        public string Commands
        {
            get { return cvars; }
            set { cvars = value; }
        }

        public Cvars()
        {
            Port = "";
            Iwads = "";
            Files = "";
            Commands = "";
        }

        public Cvars(string doomEngine, string doomIwads, string doomPwads, string cvars)
        {
            Port = doomEngine;
            Iwads = doomIwads;
            Files = doomPwads;
            Commands = cvars;
        }

        public override string ToString()
        {
            string output = String.Empty;

            output += String.Format("{0} -{3} -file {1} -file {2} ", Port, Iwads, Files, Commands);

            return output;
        }
    }
}