using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppHostTest
{

    [NUnit.Framework.TestFixture]
    public class AppHostTest
    {

        private System.AppDomain testAppDomain = null;
        [NUnit.Framework.OneTimeSetUp]
        public void Setup()
        {
            // Create the permission set to grant to all assemblies.
            System.Security.Policy.Evidence hostEvidence = new System.Security.Policy.Evidence();
            hostEvidence.AddHostEvidence(new System.Security.Policy.Zone(System.Security.SecurityZone.MyComputer));
            System.Security.PermissionSet pset = System.Security.SecurityManager.GetStandardSandbox(hostEvidence);

            // Identify the folder to use for the sandbox.
            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase = System.IO.Directory.GetCurrentDirectory();

            // Create the sandboxed application domain.
            this.testAppDomain = AppDomain.CreateDomain("Sandbox", hostEvidence, ads, pset, null);
        }

        [NUnit.Framework.Test]
        public void CoreDumpTest()
        {
            try
            {
                this.testAppDomain.ExecuteAssembly("DeadLockSample.dll");
            }
            catch (System.OutOfMemoryException)
            {
                System.AppDomain.Unload(this.testAppDomain);
                NUnit.Framework.Assert.Pass();
            }
            NUnit.Framework.Assert.Fail();
        }

    }
}
