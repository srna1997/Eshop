using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace EnterWell.Controllers
{
    public interface IPDVService
    {
        decimal PDV();
        
    }
    //[Export(typeof(IPDVService))]
    [Export(typeof(IPDVService)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PDVService : IPDVService
    {
        public decimal PDV()
        {
            return Convert.ToDecimal(1.25);
        }
    }
}