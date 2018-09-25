using Stand.Domain.Abstract;
using Stand.Domain.Infractructure.Compilers;
using Stand.Domain.Infractructure.Devices;
using Stand.Domain.Infractructure.Protocols;
using Unity;
using Unity.Resolution;

namespace Stand.UI.Infrastructure
{
    internal class IoC
    {
        private static IoC _dependencyContainer;
        private IUnityContainer _unityContainer;

        private IoC()
        {
            _unityContainer = new UnityContainer();
            this.AddBindings();
        }

        private static IoC GetInstance()
        {
            if (_dependencyContainer == null)
            {
                _dependencyContainer = new IoC();
            }
            return _dependencyContainer;
        }

        private void AddBindings()
        {
            // add compilers
            _unityContainer.RegisterType<ICompiler, SwitchCompiler>("switch");

            // add protocols
            _unityContainer.RegisterType<IProtocol, TelnetProtocol>("telnet");

            // add devices
            _unityContainer.RegisterType<Device, SwitchDevice>("switch");
        }

        public static IProtocol GetProtocol(string name)
        {
            return GetInstance()._unityContainer.Resolve<IProtocol>(name.ToLower());
        }

        public static ICompiler GetCompiler(string name)
        {
            return GetInstance()._unityContainer.Resolve<ICompiler>(name.ToLower());
        }

        public static Device GetDevice(string name, IProtocol protocol)
        {
            var compiler = GetCompiler(name);
            ResolverOverride[] overrides = new ResolverOverride[] {
                 new ParameterOverride("protocol", protocol),
                 new ParameterOverride("compiler", compiler)
            };
            return GetInstance()._unityContainer.Resolve<Device>(name.ToLower(), overrides);
        }
    }
}
