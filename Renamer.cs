using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoRenamer
{
    abstract class Renamer
    {
        public delegate void Log(string msg);

        protected readonly string dir;
        protected readonly Log log;
        protected readonly bool recursive;

        public Renamer(string dir, Log log, bool recursive)
        {
            this.dir = dir;
            this.log = log;
            this.recursive = recursive;
        }

        public abstract void Rename();
    }
}
