using Stand.Domain.Abstract;
using Stand.Domain.Infractructure.Compilers;
using Stand.Domain.Infractructure.Devices;
using Stand.Domain.Infractructure.Protocols;
using Unity;
using Unity.Resolution;

namespace Stand.IoC.DependencyInjection
{
    public class IoCContainer
    {
        private static IoCContainer _dependencyContainer;
        private IUnityContainer _unityContainer;

        private IoCContainer()
        {
            _unityContainer = new UnityContainer();
            this.AddBindings();
        }

        private static IoCContainer GetInstance()
        {
            if (_dependencyContainer == null)
            {
                _dependencyContainer = new IoCContainer();
            }
            return _dependencyContainer;
        }

        private void AddBindings()
        {
            // add compilers
            _unityContainer.RegisterType<ICompiler, SwitchCompiler>(IoCKeys.SwitchCompilerKey);

            // add protocols
            _unityContainer.RegisterType<IProtocol, TelnetProtocol>(IoCKeys.TelnetProtocolKey);
            _unityContainer.RegisterType<IProtocol, SshProtocol>(IoCKeys.SshProtocolKey);

            // add devices
            _unityContainer.RegisterType<Device, SwitchDevice>(IoCKeys.SwitchDeviceKey);
        }

        public static IProtocol GetProtocol(string name)
        {
            return GetInstance()._unityContainer.Resolve<IProtocol>(name);
        }

        public static ICompiler GetCompiler(string name)
        {
            return GetInstance()._unityContainer.Resolve<ICompiler>(name);
        }

        public static Device GetDevice(string name)
        {
            var compiler = GetCompiler(name);
            ResolverOverride[] overrides = new ResolverOverride[] {
                 new ParameterOverride("compiler", compiler)
            };
            return GetInstance()._unityContainer.Resolve<Device>(name, overrides);
        }
    }
}
