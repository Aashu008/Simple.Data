using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2c80e128-ac0b-4656-b059-3661bff8095e")]

[assembly: InternalsVisibleTo("Simple.Data.Mocking")]
[assembly: InternalsVisibleTo("Simple.Data.TestHelper")]
[assembly: InternalsVisibleTo("Simple.Data.IntegrationTest")]
[assembly: InternalsVisibleTo("Simple.Data.Ado.Test")]

[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
[assembly: AllowPartiallyTrustedCallers]
