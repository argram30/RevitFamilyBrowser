﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitFamilyBrowser.Revit_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RevitFamilyBrowser.WPF_Classes
{

    //This class transform Revit coordinates into WPF canvas coordinates

    class RoomDimensions
    {
        public int MyProperty { get; set; }

       public void GetBoundingBox(Room newRoom, View view)
        {
            BoundingBoxXYZ box = newRoom.get_BoundingBox(view);
            Coordinates center = new Coordinates();

            center.X = (int)(box.Min.X - box.Max.X) / 2;
            center.Y = (int)(box.Min.Y - box.Max.Y) / 2;
          
            ConversionPoint roomMin = new ConversionPoint(box.Min);
            ConversionPoint roomMax = new ConversionPoint(box.Max);
        }            

        //-----Get coordinates for all walls in Room
        public List<System.Windows.Shapes.Line> GetWalls(Room room)
        {
            SpatialElementBoundaryOptions boundaryOption = new SpatialElementBoundaryOptions();
            boundaryOption.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center;

            IList<IList<BoundarySegment>> boundary = room.GetBoundarySegments(boundaryOption);

            string temp = string.Empty; //can be used to see Wall and segment numbers  
            int WallNumber = 0;
            int SegmentNumber = 0;
            List<System.Windows.Shapes.Line> wallCoord = new List<System.Windows.Shapes.Line>();           
            XYZ segmentStart = null; ///
            XYZ segmentEnd = null;

            int nLoops = boundary.Count;
            foreach (IList<BoundarySegment> walls in boundary)
            {
                WallNumber++;
                foreach (BoundarySegment segment in walls)
                {
                    System.Windows.Shapes.Line wall = new System.Windows.Shapes.Line();

                    segmentStart = segment.GetCurve().GetEndPoint(0);
                    ConversionPoint Start = new ConversionPoint(segmentStart);
                    wall.X1 = Start.X;
                    wall.Y1 = Start.Y;

                    segmentEnd = segment.GetCurve().GetEndPoint(1);
                    ConversionPoint End = new ConversionPoint(segmentEnd);
                    wall.X2 = End.X;
                    wall.Y2 = End.Y;

                    wallCoord.Add(wall);

                    SegmentNumber++;
                    temp += "WallNumber:" + WallNumber + " " + "SegmentNumber:" + SegmentNumber + " " + Start.ToString() + End.ToString() + "\n";
                }
            }
            //  TaskDialog.Show("Boundaries", temp);
            return wallCoord;
        }

        //-----Compare Longest Wall in Room with Canvas to fit room boundary into Canvas
        public int GetScale(List<System.Windows.Shapes.Line> wallCoord, int CanvasSize)
        {
            double LongestWall = 0;
            int Scale = 0;
            CanvasSize = CanvasSize - 5;
            foreach (var item in wallCoord)
            {
                Coordinates coord = new Coordinates();
                System.Windows.Shapes.Line baseLine = new System.Windows.Shapes.Line();
                baseLine = item;
                coord.GetLength(item);

                if (coord.GetLength(item) > LongestWall)
                {
                    LongestWall = coord.GetLength(item);
                }

                if ((LongestWall / CanvasSize) < 1)
                {
                    Scale = 1;
                }
                else if ((LongestWall / CanvasSize) > 1 && (LongestWall / CanvasSize) < 2)
                {
                    Scale = 2;
                }
                else if ((LongestWall / CanvasSize) > 2 && (LongestWall / CanvasSize) < 5)
                {
                    Scale = 5;
                }
                else if ((LongestWall / CanvasSize) > 5 && (LongestWall / CanvasSize) < 10)
                {
                    Scale = 10;
                }
                else if ((LongestWall / CanvasSize) > 10 && (LongestWall / CanvasSize) < 20)
                {
                    Scale = 20;
                }
                else if ((LongestWall / CanvasSize) > 20 && (LongestWall / CanvasSize) < 25)
                {
                    Scale = 25;
                }
                else if ((LongestWall / CanvasSize) > 25 && (LongestWall / CanvasSize) < 50)
                {
                    Scale = 50;
                }
                else if ((LongestWall / CanvasSize) > 50 && (LongestWall / CanvasSize) < 100)
                {
                    Scale = 100;
                }
                else if ((LongestWall / CanvasSize) > 100 && (LongestWall / CanvasSize) < 200)
                {
                    Scale = 200;
                }
                else if ((LongestWall / CanvasSize) > 200 && (LongestWall / CanvasSize) < 500)
                {
                    Scale = 500;
                }
                else if ((LongestWall / CanvasSize) > 500 && (LongestWall / CanvasSize) < 1000)
                {
                    Scale = 1000;
                }
            }
            return Scale;
        }

      
    }   
}
