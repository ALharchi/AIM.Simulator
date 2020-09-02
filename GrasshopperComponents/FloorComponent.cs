using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AIM.Simulation.GrasshopperComponents
{
    public class FloorComponent : GH_Component
    {

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }
        public override Guid ComponentGuid { get { return new Guid("f72076d7-a5e7-4bba-936b-3307df50a2b7"); } }
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.iconFloor; } }


        public FloorComponent() : base("Floor", "Floor", "Simulation Floor", "AIM", "Stability Simulation")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "Plane", "Plane", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Extent", "Extent", "Floor Extent", GH_ParamAccess.item, 1000);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Floor", "Floor", "Simulation Floor", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Plane plane = new Plane();
            Double extent = 1000;

            DA.GetData(0, ref plane);
            DA.GetData(1, ref extent);

            BoundingBox bnd = new BoundingBox(-extent, -extent, plane.OriginZ - 20, extent, extent, plane.OriginZ);

            Mesh floorMesh = Mesh.CreateFromBox(bnd, 1, 1, 1);

            List<Mesh> iMeshes = new List<Mesh>();
            iMeshes.Add(floorMesh);

            DA.SetData(0, new AIM.Simulation.Kernel.PxGhRigidStaticCompoundConvexMesh(Plane.WorldXY, iMeshes, PxGhManager.DefaultMaterial));

            this.Hidden = true;
        }
        
    }
}