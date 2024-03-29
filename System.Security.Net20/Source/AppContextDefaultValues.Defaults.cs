﻿// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
using System;

namespace System
{
    internal static partial class AppContextDefaultValues
    {
        // This is a partial method. Platforms can provide an implementation of it that will set override values
        // from whatever mechanism is available on that platform. If no implementation is provided, the compiler is going to remove the calls
        // to it from the code
        // We are going to have an implementation of this method for the Desktop platform that will read the overrides from app.config, registry and
        // the shim database. Additional implementation can be provided for other platforms.
        //static partial void PopulateOverrideValuesPartial();

        static  void PopulateDefaultValuesPartial(string platformIdentifier, string profile, int version)
        {
            // When defining a new switch  you should add it to the last known version.
            // For instance, if you are adding a switch in .NET 4.6 (the release after 4.5.2) you should defined your switch
            // like this:
            //    if (version <= 40502) ...
            // This ensures that all previous versions of that platform (up-to 4.5.2) will get the old behavior by default
            // NOTE: When adding a default value for a switch please make sure that the default value is added to ALL of the existing platforms!
            // NOTE: When adding a new if statement for the version please ensure that ALL previous switches are enabled (ie. don't use else if)
            switch (platformIdentifier)
            {
                case ".NETCore":
                case ".NETFramework":
                    {
                        if (version <= 40700)
                        {
                            LocalAppContext.DefineSwitchDefault(LocalAppContextSwitches.SwitchXmlUseInsecureHashAlgorithms, true);
                            LocalAppContext.DefineSwitchDefault(LocalAppContextSwitches.SwitchCmsUseInsecureHashAlgorithms, true);
                        }

                        if (version <= 40702)
                        {
                            LocalAppContext.DefineSwitchDefault(LocalAppContextSwitches.SwitchEnvelopedCmsUseLegacyDefaultAlgorithm, true);
                        }

                        break;
                    }
            }

            // At this point we should read the overrides if any are defined
            PopulateOverrideValuesPartial();
        }
    }
}
