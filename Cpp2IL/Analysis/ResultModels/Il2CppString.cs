﻿namespace Cpp2IL.Analysis.ResultModels
{
    public class Il2CppString
    {
        public string ContainedString;
        public ulong Address;

        public Il2CppString(string containedString, ulong addr)
        {
            ContainedString = containedString;
            Address = addr;
        }
    }
}