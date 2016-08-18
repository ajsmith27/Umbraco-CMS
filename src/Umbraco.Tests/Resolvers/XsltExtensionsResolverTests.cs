using System.Linq;
using LightInject;
using Moq;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Core.Macros;
using Umbraco.Core.ObjectResolution;
using Umbraco.Core.Profiling;
using Umbraco.Web;
using Umbraco.Web.Macros;
using umbraco;
using Umbraco.Web._Legacy.Actions;

namespace Umbraco.Tests.Resolvers
{
    [TestFixture]
    public class XsltExtensionsResolverTests : ResolverBaseTest
    {
        // NOTE
        // ManyResolverTests ensure that we'll get our actions back and ActionsResolver works,
        // so all we're testing here is that plugin manager _does_ find our actions
        // which should be ensured by PlugingManagerTests anyway, so this is useless?
        // maybe not as it seems to handle the "instance" thing... so we test that we respect the singleton?
        [Test]
        public void Find_All_Extensions()
        {
            var container = new ServiceContainer();
            var builder = new XsltExtensionCollectionBuilder(container);
            builder.AddExtensionObjectProducer(() => PluginManager.ResolveXsltExtensions());
            var extensions = builder.CreateCollection();

            Assert.AreEqual(3, extensions.Count());

            Assert.IsTrue(extensions.Select(x => x.ExtensionObject.GetType()).Contains(typeof (XsltEx1)));
            Assert.IsTrue(extensions.Select(x => x.ExtensionObject.GetType()).Contains(typeof(XsltEx2)));
            Assert.AreEqual("test1", extensions.Single(x => x.ExtensionObject.GetType() == typeof(XsltEx1)).Namespace);
            Assert.AreEqual("test2", extensions.Single(x => x.ExtensionObject.GetType() == typeof(XsltEx2)).Namespace);
        }

        #region Classes for tests

        [Umbraco.Core.Macros.XsltExtension("test1")]
        public class XsltEx1
        {
            
        }

        //test with legacy one
        [umbraco.XsltExtension("test2")]
        public class XsltEx2
        {
        }

        #endregion
    }
}