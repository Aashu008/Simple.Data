using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a3becbf2-2e23-4399-a6d0-267e0e278ca6")]

[assembly: InternalsVisibleTo("Simple.Data.AdapterApi")]
[assembly: InternalsVisibleTo("Simple.Data.TestHelper")]
[assembly: InternalsVisibleTo("Simple.Data.UnitTest")]
[assembly: InternalsVisibleTo("Simple.Data.IntegrationTest")]
[assembly: InternalsVisibleTo("Simple.Data.Mocking")]
[assembly: InternalsVisibleTo("Simple.Data.Mocking.Test")]

[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: InternalsVisibleTo("Simple.Data.InMemory")]
[assembly: InternalsVisibleTo("Simple.Data.Ado")]