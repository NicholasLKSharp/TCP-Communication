﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static ClientServer.EncodingClasses;

namespace ClientServer
{
    public class Command
    {
        public string operation;
        public Func<Tuple<NetworkData, string>, NetworkData> action;

        public Command()
        {

        }
    }
}
