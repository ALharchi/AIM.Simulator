using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;


namespace AIM.Simulation.Kernel
{
    public abstract class PxGhRigidBody
    {
        internal List<Mesh> DisplayMeshes = new List<Mesh>();
        public abstract void GetDisplayGhMeshes(List<GH_Mesh> ghMeshes);
        public abstract void GetDisplayMeshes(List<Mesh> meshes);
    }
}
