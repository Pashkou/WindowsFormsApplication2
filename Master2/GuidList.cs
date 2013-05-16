// Guids.cs
// MUST match guids.h
using System;

namespace Company.Master2
{
    static class GuidList
    {
        public const string guidMaster2PkgString = "aa8dafdc-1e08-4240-b0d7-a64e82789cc0";
        public const string guidMaster2CmdSetString = "13557b70-1b14-4ef4-b856-af8e7c7d9fa7";
        public const string guidToolWindowPersistanceString = "1a28b48f-db1a-4eb1-ab90-0118db97df9e";

        public static readonly Guid guidMaster2CmdSet = new Guid(guidMaster2CmdSetString);
    };
}