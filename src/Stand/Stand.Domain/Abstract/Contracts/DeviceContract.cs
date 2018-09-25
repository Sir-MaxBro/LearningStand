using System;
using System.Diagnostics.Contracts;

namespace Stand.Domain.Abstract.Contracts
{
    [ContractClassFor(typeof(Device))]
    internal abstract class DeviceContract : Device
    {
        protected DeviceContract(IProtocol protocol, ICompiler compiler)
            : base(protocol, compiler)
        {
            Contract.Requires<ArgumentNullException>(protocol != null);
            Contract.Requires<ArgumentNullException>(compiler != null);
        }
    }
}
