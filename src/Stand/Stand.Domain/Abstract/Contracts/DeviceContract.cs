using System;
using System.Diagnostics.Contracts;

namespace Stand.Domain.Abstract.Contracts
{
    [ContractClassFor(typeof(Device))]
    internal abstract class DeviceContract : Device
    {
        protected DeviceContract(ICompiler compiler)
            : base(compiler)
        {
            Contract.Requires<ArgumentNullException>(compiler != null);
        }
    }
}
