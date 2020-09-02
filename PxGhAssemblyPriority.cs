using Grasshopper.Kernel;


namespace AIM.Simulation
{
    public class PxGhAssemblyPriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            PxGhManager.Initialize();
            return GH_LoadingInstruction.Proceed;
        }
    }
}