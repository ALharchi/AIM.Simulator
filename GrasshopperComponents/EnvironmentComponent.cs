using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using PhysX;
using AIM.Simulation.Kernel;

namespace AIM.Simulation.GrasshopperComponents
{
    public class EnvironmentComponent : GH_Component
    {
        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }
        public override Guid ComponentGuid { get { return new Guid("8dc19e98-1f8b-49bd-bf14-fa3fa52aef08"); } }
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.iconEnv; } }


        public EnvironmentComponent() : base("Environment", "Environment", "Assembly Environment", "AIM", "Stability Simulation") {}

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "Meshes", "Environment Meshes as a list", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Environment", "Environment", "Environment for Assembly Simulation", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> iMeshes = new List<Mesh>();
            DA.GetDataList(0, iMeshes);
            DA.SetData(0, new AIM.Simulation.Kernel.PxGhRigidStaticCompoundConvexMesh(Plane.WorldXY, iMeshes, PxGhManager.DefaultMaterial));
        }

    }
}