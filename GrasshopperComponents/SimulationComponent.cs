using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using AIM.Simulation.Kernel;
using Rhino.Display;
using Rhino.Geometry;


namespace AIM.Simulation.GrasshopperComponents
{
    public class SimulationComponent : GH_Component
    {
        private GhPxSystem system;
        private readonly Stopwatch stopwatch = new Stopwatch();
        private List<GH_Mesh> staticGhMeshes;

        public override GH_Exposure Exposure { get { return GH_Exposure.tertiary; } }
        public override Guid ComponentGuid => new Guid("{582fe019-242f-4cf2-980d-9f27a0c462b5}");
        protected override System.Drawing.Bitmap Icon { get { return Properties.Resources.iconSolver; } }
        
        public SimulationComponent() : base( "Simulate Assembly", "Simulate Assembly", "Simulate an Assembly Stability", "AIM", "Stability Simulation") { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Elements", "Elements", "Elements to Simulate", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Environment", "Environment", "Assembly Environment (Optional)", GH_ParamAccess.item);
            pManager.AddGenericParameter("Floor", "Floor", "Simulation Floor. (Optional, but recommanded.)", GH_ParamAccess.item);
            pManager.AddVectorParameter("Gravity", "Gravity", "Gravity", GH_ParamAccess.item, new Vector3d(0.0, 0.0, -9.8));
            pManager.AddBooleanParameter("Reset", "Reset", "Reset", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Run", "Run", "Run", GH_ParamAccess.item, false);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Elements", "Elements", "Assembly Elements", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vector3d iGravity = Vector3d.Unset;
            double iTimestep = 0.01;
            int iSteps = 10;
            bool iReset = false;
            bool iRun = false;

            DA.GetData(3, ref iGravity);
            DA.GetData(4, ref iReset);
            DA.GetData(5, ref iRun);

            if (iReset || system == null)
            {
                system = new GhPxSystem();

                List<PxGhRigidBody> iRigidBodies = new List<PxGhRigidBody>();
                foreach (var ghGoo in Params.Input[0].VolatileData.AllData(true))
                    iRigidBodies.Add((PxGhRigidBody)(((GH_ObjectWrapper)ghGoo).Value));

                foreach (var ghGoo in Params.Input[1].VolatileData.AllData(true))
                    iRigidBodies.Add((PxGhRigidBody)(((GH_ObjectWrapper)ghGoo).Value));

                foreach (var ghGoo in Params.Input[2].VolatileData.AllData(true))
                    iRigidBodies.Add((PxGhRigidBody)(((GH_ObjectWrapper)ghGoo).Value));


                foreach (PxGhRigidBody o in iRigidBodies)
                    switch (o)
                    {
                        case PxGhRigidDynamic rigidDynamic:
                            {
                                system.AddRigidDynamic(rigidDynamic);
                                rigidDynamic.Reset();
                                break;
                            }
                        case PxGhRigidStatic rigidStatic:
                            system.AddRigidStatic(rigidStatic);
                            break;
                    }

                staticGhMeshes = system.GetRigidStaticDisplayedGhMeshes();
            }


            if (iRun)
            {
                ExpireSolution(true);
                stopwatch.Restart();
                system.Gravity = iGravity;
                system.Iterate((float)iTimestep, iSteps);
                stopwatch.Stop();
            }

            DA.SetDataList(0, system.GetRigidDynamicDisplayedGhMeshes());
        }

    }
}