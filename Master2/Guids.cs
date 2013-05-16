// Guids.cs
// MUST match guids.h
using System;

namespace Microsoft.Master2
{
    static class GuidList
    {
        public const string guidMaster2PkgString = "cbe9f821-ba83-4fe4-b87b-cf52543e9962";
        public const string guidMaster2CmdSetString = "41fe2b33-3385-446a-b7b4-5d2e3a741177";
        public const string guidToolWindowPersistanceString = "7bcb8666-322c-462c-9f6b-4afc5f344bf1";

        public static readonly Guid guidMaster2CmdSet = new Guid(guidMaster2CmdSetString);
    };
}