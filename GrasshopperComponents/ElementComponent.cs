using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using AIM.Simulation.Kernel;
using Rhino.Geometry;
using PhysX;
using Plane = Rhino.Geometry.Plane;


namespace AIM.Simulation.GrasshopperComponents
{
    public class ElementComponent : GH_Component
    {

        public override GH_Exposure Exposure { get { return GH_Exposure.primary; } }
        public override Guid ComponentGuid => new Guid("2cb14aa4-3a82-434c-b1cc-84998e925842");
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.iconEle ; } }


        public ElementComponent() : base("Element", "Element", "Create an element for the rigid body simulation. Must be composed ONLY for convex meshes.", "AIM", "Stability Simulation") { }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "Meshes", "Meshes used for the computation in physics simulation", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Dynamic", "Dynamic", "Dynamic or Static, True: Dynamic, False: Static", GH_ParamAccess.item, true);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "Elements", "Elements - (Only Dynamic Elements)", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> iMeshes = new List<Mesh>();
            bool iDynamic = true;

            DA.GetDataList(0, iMeshes);
            DA.GetData(1, ref iDynamic);

            if (iDynamic)
            {
                PxGhRigidDynamic rigidObject = new PxGhRigidDynamiCompoundConvexMesh(iMeshes, Plane.WorldXY, PxGhManager.DefaultMaterial, (float)1.0, Vector3d.Zero, Vector3d.Zero);

                rigidObject.Actor.LinearDamping = (float)1.0;
                rigidObject.Actor.AngularDamping = (float)1.0;
                DA.SetData(0, rigidObject);
            }
            else
                DA.SetData(0, new PxGhRigidStaticCompoundConvexMesh(Plane.WorldXY, iMeshes, PxGhManager.DefaultMaterial));
        }
    }
}