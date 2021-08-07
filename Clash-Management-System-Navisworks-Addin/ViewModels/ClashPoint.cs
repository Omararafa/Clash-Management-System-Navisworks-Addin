using Autodesk.Navisworks.Api;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class ClashPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public ClashPoint(Point3D point)
        {
            this.X = (float) point.X;
            this.Y = (float) point.Y;
            this.Z = (float) point.Z;
        }
    }
}
