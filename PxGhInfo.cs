using System;
using System.Drawing;
using Grasshopper.Kernel;


namespace AIM.Simulation
{
    public class PxGhInfo : GH_AssemblyInfo
    {
        public override string Name => "AIM.Simulation";
        //public override Bitmap Icon => Properties.Resources.logo_24x24;
        public override string Description => "Simulation Component for the Assembly Information Modeling Framework";
        public override Guid Id => new Guid("7ce8061e-1d39-4806-9b51-3fa2a925caf3");
        public override string AuthorName => "Ayoub Lharchi";
        public override string AuthorContact => "alha@kadk.dk";
    }
}
