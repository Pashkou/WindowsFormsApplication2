using Company.Master2.xmlmodel;
using GraphSharp.Controls;
using Microsoft.master2.contextmodel;
using Microsoft.master2.model;
using QuickGraph;
using Relations.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Relations
{
    /* public class MyGraphLayout : GraphSharp.Controls.GraphLayout<MyVertexBase, TaggedEdge<object, object>, MyGraph>
     {
         public MyGraphLayout()
         { }
     
     }*/

    public class MyGraph : GraphSharp.SoftMutableBidirectionalGraph<MyVertexBase, MyEdgeBase>
    {
        /* nothing to do here... */
    }

    public class GraphLayout : GraphLayout<MyVertexBase, MyEdgeBase, MyGraph>//<object, IEdge<object>, IBidirectionalGraph<object, IEdge<object>>>
    {
        public GraphLayout()
        {

            var g = new MyGraph();
            var vertices = new object[] { "S", "A", "M", "P", "L", "E" };
          
            OverlapRemovalAlgorithmType = "FSA";
            LayoutAlgorithmType = "FreeFS";
           // LayoutAlgorithmType = "CompoundFDP";
            
            IsAnimationEnabled = false;
            CreationTransition = null;
            ShowAllStates = false;
            MoveAnimation = null;
            AsyncCompute = false;
            AnimationDisablerEdgeCount = 0;
            AnimationDisablerVertexCount = 0;
            HighlightAlgorithmType = "";
            AnimationLength = new TimeSpan(0);
            OverlapRemovalConstraint = AlgorithmConstraints.Must;
            
            Graph = g;

            
        }

    }

    public class MyVertexBaseMiddle : MyVertexBase
    {

        public override  int getNumberOfBloks()
        {
            return 8;
        }
        public override int getSizeOfVertex()
        {
            return 140;
        }
    }

    public class MyVertexBaseSmall : MyVertexBase
    {

        public override int getNumberOfBloks()
        {
            return 4;
        }
        public override int getSizeOfVertex()
        {
            return 110;
        }
    }

    public class MyVertexBaseSmallFirst : MyVertexBaseSmall { 
    
    }

    public class MyVertexBaseSmallSecond : MyVertexBaseSmall
    {

    }

    public class MyVertexBaseSmallThird : MyVertexBaseSmall
    {

    }

    public class MyVertexBaseVerySmall : MyVertexBase
    {
        public MyVertexBaseVerySmall(){
            string classPath = "Resources/class.jpg";
            BitmapImage classBitmap = new BitmapImage(new Uri(classPath, UriKind.Relative));
            Image classImage = new Image();
            classImage.Source = classBitmap;
            Image1 = classImage;
        }
        
        
        public double Relevance = 0;
        public override int getNumberOfBloks()
        {
            return 2;
        }
        public override int getSizeOfVertex() {
            return 30;
        }
    }

    public class MyVertexBaseAbsolute : MyVertexBase
    {
        public MyVertexBaseAbsolute()
        {
            string classPath = "Resources/class.jpg";
            BitmapImage classBitmap = new BitmapImage(new Uri(classPath, UriKind.Relative));
            Image classImage = new Image();
            classImage.Source = classBitmap;
            Image1 = classImage;
        }


        public double Relevance = 0;
        public override int getNumberOfBloks()
        {
            return 2;
        }
        public override int getSizeOfVertex()
        {
            return 30;
        }
    }


    public abstract class MyVertexBase : DependencyObject
    {
        public ArrayList cSharpModel = null;
        public int displayElementInThePast = 0;
        public Boolean IsThirdColumn = false;
        public Boolean toAggregate = false;
        public abstract int getNumberOfBloks();
        public abstract int getSizeOfVertex();
        private bool isCoordinateCalculated = false;
        private int numberInThePast = 0;
        private int sortedNumberInThePast = 0;

        public int SortedNumberInThePast
        {
            get { return sortedNumberInThePast; }
            set { sortedNumberInThePast = value; }
        }
        public int NumberInThePast
        {
            get { return numberInThePast; }
            set { numberInThePast = value; }
        }


        public bool IsCoordinateCalculated
        {
            get { return isCoordinateCalculated; }
            set { isCoordinateCalculated = value; }
        }

        /* 1 */
        public static readonly DependencyProperty NameProperty1 =
            DependencyProperty.Register("Name1", typeof(string), typeof(MyVertexBase));

        public string Name1
        {
            get { return (string)GetValue(NameProperty1); }
            set { SetValue(NameProperty1, value); }
        }


        public static readonly DependencyProperty ImageProperty1 =
           DependencyProperty.Register("Image1", typeof(Image), typeof(MyVertexBase));

        public Image Image1
        {
            get { return (Image)GetValue(ImageProperty1); }
            set { SetValue(ImageProperty1, value); }
        }
        /* 2  */
        public static readonly DependencyProperty NameProperty2 =
           DependencyProperty.Register("Name2", typeof(string), typeof(MyVertexBase));

        public string Name2
        {
            get { return (string)GetValue(NameProperty2); }
            set { SetValue(NameProperty2, value); }
        }

        /* 2 Bold  */
        public static readonly DependencyProperty NameProperty2Bold =
           DependencyProperty.Register("Name2Bold", typeof(string), typeof(MyVertexBase));

        public string Name2Bold
        {
            get { return (string)GetValue(NameProperty2Bold); }
            set { SetValue(NameProperty2Bold, value); }
        }


        public static readonly DependencyProperty ImageProperty2 =
           DependencyProperty.Register("Image2", typeof(Image), typeof(MyVertexBase));

        public Image Image2
        {
            get { return (Image)GetValue(ImageProperty2); }
            set { SetValue(ImageProperty2, value); }
        }
        /* 3 */

        public static readonly DependencyProperty NameProperty3 =
           DependencyProperty.Register("Name3", typeof(string), typeof(MyVertexBase));

        public string Name3
        {
            get { return (string)GetValue(NameProperty3); }
            set { SetValue(NameProperty3, value); }
        }

        /* Bold */
        public static readonly DependencyProperty NameProperty3Bold =
          DependencyProperty.Register("Name3Bold", typeof(string), typeof(MyVertexBase));

        public string Name3Bold
        {
            get { return (string)GetValue(NameProperty3Bold); }
            set { SetValue(NameProperty3Bold, value); }
        }


        public static readonly DependencyProperty ImageProperty3 =
           DependencyProperty.Register("Image3", typeof(Image), typeof(MyVertexBase));

        public Image Image3
        {
            get { return (Image)GetValue(ImageProperty3); }
            set { SetValue(ImageProperty3, value); }
        }

        /* 4 */

        public static readonly DependencyProperty NameProperty4 =
           DependencyProperty.Register("Name4", typeof(string), typeof(MyVertexBase));

        public string Name4
        {
            get { return (string)GetValue(NameProperty4); }
            set { SetValue(NameProperty4, value); }
        }
        /* Bold */
        public static readonly DependencyProperty NameProperty4Bold =
          DependencyProperty.Register("Name4Bold", typeof(string), typeof(MyVertexBase));

        public string Name4Bold
        {
            get { return (string)GetValue(NameProperty4Bold); }
            set { SetValue(NameProperty4Bold, value); }
        }

        public static readonly DependencyProperty ImageProperty4 =
           DependencyProperty.Register("Image4", typeof(Image), typeof(MyVertexBase));

        public Image Image4
        {
            get { return (Image)GetValue(ImageProperty4); }
            set { SetValue(ImageProperty4, value); }
        }

        /* 5 */

        public static readonly DependencyProperty NameProperty5 =
           DependencyProperty.Register("Name5", typeof(string), typeof(MyVertexBase));

        public string Name5
        {
            get { return (string)GetValue(NameProperty5); }
            set { SetValue(NameProperty5, value); }
        }


        public static readonly DependencyProperty ImageProperty5 =
           DependencyProperty.Register("Image5", typeof(Image), typeof(MyVertexBase));

        public Image Image5
        {
            get { return (Image)GetValue(ImageProperty5); }
            set { SetValue(ImageProperty5, value); }
        }

        /* 6 */

        public static readonly DependencyProperty NameProperty6 =
           DependencyProperty.Register("Name6", typeof(string), typeof(MyVertexBase));

        public string Name6
        {
            get { return (string)GetValue(NameProperty6); }
            set { SetValue(NameProperty6, value); }
        }


        public static readonly DependencyProperty ImageProperty6 =
           DependencyProperty.Register("Image6", typeof(Image), typeof(MyVertexBase));

        public Image Image6
        {
            get { return (Image)GetValue(ImageProperty6); }
            set { SetValue(ImageProperty6, value); }
        }

        /* 7 */

        public static readonly DependencyProperty NameProperty7 =
           DependencyProperty.Register("Name7", typeof(string), typeof(MyVertexBase));

        public string Name7
        {
            get { return (string)GetValue(NameProperty7); }
            set { SetValue(NameProperty7, value); }
        }


        public static readonly DependencyProperty ImageProperty7 =
           DependencyProperty.Register("Image7", typeof(Image), typeof(MyVertexBase));

        public Image Image7
        {
            get { return (Image)GetValue(ImageProperty7); }
            set { SetValue(ImageProperty7, value); }
        }

        /* 8 */

        public static readonly DependencyProperty NameProperty8 =
           DependencyProperty.Register("Name8", typeof(string), typeof(MyVertexBase));

        public string Name8
        {
            get { return (string)GetValue(NameProperty8); }
            set { SetValue(NameProperty8, value); }
        }


        public static readonly DependencyProperty ImageProperty8 =
           DependencyProperty.Register("Image8", typeof(Image), typeof(MyVertexBase));

        public Image Image8
        {
            get { return (Image)GetValue(ImageProperty8); }
            set { SetValue(ImageProperty8, value); }
        }

        ArrayList cSharpClasses = new ArrayList();
        ArrayList edges = new ArrayList();


        public ArrayList Edges
        {
            get { return edges; }
            set { edges = value; }
        }
        public ArrayList CSharpClasses
        {
            get { return cSharpClasses; }
            set { cSharpClasses = value; }
        }
        private int commonThreshold = 3;

        public int CommonThreshold
        {
            get { return commonThreshold; }
            set { commonThreshold = value; }
        }

        public void prepareToDisplay()
        {

            if (cSharpClasses.ToArray().Length > 1)
            {
                 
                int i = 0;
                foreach (CSharpClass cSharpClass in cSharpClasses)
                {
                    string classPath = "Resources/class.jpg";
                    BitmapImage classBitmap = new BitmapImage(new Uri(classPath, UriKind.Relative));
                    Image classImage = new Image();
                    classImage.Source = classBitmap;

                    switch (i)
                    {
                        case 0:
                            {
                                Image1 = classImage;
                                Name1 = cSharpClass.Name;
                            } break;
                        case 1:
                            {
                                Image2 = classImage;
                                Name2 = cSharpClass.Name;
                            } break;
                        case 2:
                            {
                                if (getNumberOfBloks() > 2)
                                {
                                    Image3 = classImage;
                                    Name3 = cSharpClass.Name;
                                }
                            } break;
                        case 3: {
                            Name4 = "Relevance: " + cSharpClass.countRelevance().ToString();
                        }break;
                    }
                    i++;
                }
                
            }
            else
            {

                    string classPath = "Resources/class.jpg";
                    BitmapImage classBitmap = new BitmapImage(new Uri(classPath, UriKind.Relative));
                    Image classImage = new Image();
                    classImage.Source = classBitmap;

                    Image1 = classImage;
                    foreach (CSharpClass cSharpClass in cSharpClasses)
                    {
                        Name1 = cSharpClass.Name;
                        if (cSharpClass.Methods.ToArray().Length > 0)
                        {

                            ArrayList methods = cSharpClass.Methods;
                            methods.Sort();
                            int i = 0;
                            foreach (CSharpMethod cSharpMethod in methods)
                            {
                                string methodPath = "Resources/method.jpg";
                                BitmapImage methodBitmap = new BitmapImage(new Uri(methodPath, UriKind.Relative));
                                Image methodImage = new Image();
                                methodImage.Source = methodBitmap;

                                switch (i)
                                {
                                    case 0:
                                        {
                                            Image2 = methodImage;
                                            Name2 = cSharpMethod.Name;
                                        } break;
                                    case 1:
                                        {
                                            if (getNumberOfBloks() > 2)
                                            {
                                                Image3 = methodImage;
                                                Name3 = cSharpMethod.Name;
                                            }
                                        } break;
                                    case 2:
                                        {
                                            if (getNumberOfBloks() > 2)
                                            {
                                                Image4 = methodImage;
                                                Name4 = cSharpMethod.Name;
                                            }
                                        } break;
                                }
                                i++;
                                if (i > 2)
                                {
                                    break;
                                }
                            }
                        }
                        if (cSharpClass.Fields.ToArray().Length > 0)
                        {
                            if (getNumberOfBloks() > 7)
                            {
                                int i = 0;
                                ArrayList fields = cSharpClass.Fields;
                                fields.Sort();
                                foreach (CSharpField cSharpField in fields)
                                {
                                    string fieldPath = "Resources/field.jpg";
                                    BitmapImage fieldBitmap = new BitmapImage(new Uri(fieldPath, UriKind.Relative));
                                    Image fieldImage = new Image();
                                    fieldImage.Source = fieldBitmap;
                                    switch (i)
                                    {
                                        case 0:
                                            {
                                                Image5 = fieldImage;
                                                Name5 = cSharpField.Name ;
                                            } break;
                                        case 1:
                                            {
                                                Image6 = fieldImage;
                                                Name6 = cSharpField.Name;
                                            } break;
                                        case 2:
                                            {
                                                Image7 = fieldImage;
                                                Name7 = cSharpField.Name;
                                            } break;
                                    }
                                    i++;
                                    if (i > 2)
                                    {
                                        break;
                                    }

                                }
                            }
                       //     Name8 = "Relevance: " + cSharpClass.countRelevance().ToString();
                        }

                       

                    }
                }
          

        }

        public override string ToString()
        {


            string result = "";
            if ((cSharpClasses != null) && (cSharpClasses.ToArray().Length != 0))
            {
                cSharpClasses.Sort();
                foreach (CSharpClass cSharpClass in cSharpClasses)
                {
                    result += " Name: " + cSharpClass.Name + "\r\n";
                    result += "  Relevance: " + String.Format("{0:0.0}", cSharpClass.countRelevance()) + "\r\n";
                }
            }
            else {
                result += Name1 + Name2;  
            }
            result += "\r\n";
            result += "THRESHOLD: " + commonThreshold;
            return result;
        }


    }

    public class MyEdgeBase : Edge<MyVertexBase>
    {

        private string tag;

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public MyEdgeBase(MyVertexBase fromvertex, MyVertexBase tovertex, string weight)
            : base(fromvertex, tovertex)
        {

            tag = weight;

        }



    }



    public class EdgeLabelControl : ContentControl
    {
        public EdgeLabelControl()
        {
          
            LayoutUpdated += EdgeLabelControl_LayoutUpdated;
        }

        private EdgeControl GetEdgeControl(DependencyObject parent)
        {
            while (parent != null)
                if (parent is EdgeControl)
                    return (EdgeControl)parent;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            return null;
        }

        private static double GetAngleBetweenPoints(Point point1, Point point2)
        {
            return Math.Atan2(point1.Y - point2.Y, point2.X - point1.X);
        }

        private static double GetDistanceBetweenPoints(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        private static double GetLabelDistance(double edgeLength)
        {
            return edgeLength / 2;  // set the label halfway the length of the edge
        }

        private void EdgeLabelControl_LayoutUpdated(object sender, EventArgs e)
        {
            if (!IsLoaded)
                return;
            var edgeControl = GetEdgeControl(VisualParent);
            if (edgeControl == null)
                return;

            var source = edgeControl.Source;
            var p1 = new Point(GraphCanvas.GetX(source), GraphCanvas.GetY(source));
            var target = edgeControl.Target;
            var p2 = new Point(GraphCanvas.GetX(target), GraphCanvas.GetY(target));

            double edgeLength;
            var routePoints = edgeControl.RoutePoints;
            if (routePoints == null)
                // the edge is a single segment (p1,p2)
                edgeLength = GetLabelDistance(GetDistanceBetweenPoints(p1, p2));
            else
            {
                // the edge has one or more segments
                // compute the total length of all the segments
                edgeLength = 0;
                for (int i = 0; i <= routePoints.Length; ++i)
                    if (i == 0)
                        edgeLength += GetDistanceBetweenPoints(p1, routePoints[0]);
                    else if (i == routePoints.Length)
                        edgeLength += GetDistanceBetweenPoints(routePoints[routePoints.Length - 1], p2);
                    else
                        edgeLength += GetDistanceBetweenPoints(routePoints[i - 1], routePoints[i]);
                // find the line segment where the half distance is located
                edgeLength = GetLabelDistance(edgeLength);
                Point newp1 = p1;
                Point newp2 = p2;
                for (int i = 0; i <= routePoints.Length; ++i)
                {
                    double lengthOfSegment;
                    if (i == 0)
                        lengthOfSegment = GetDistanceBetweenPoints(newp1 = p1, newp2 = routePoints[0]);
                    else if (i == routePoints.Length)
                        lengthOfSegment = GetDistanceBetweenPoints(newp1 = routePoints[routePoints.Length - 1], newp2 = p2);
                    else
                        lengthOfSegment = GetDistanceBetweenPoints(newp1 = routePoints[i - 1], newp2 = routePoints[i]);
                    if (lengthOfSegment >= edgeLength)
                        break;
                    edgeLength -= lengthOfSegment;
                }
                // redefine our edge points
                p1 = newp1;
                p2 = newp2;
            }
            // align the point so that it  passes through the center of the label content
            var p = p1;
            var desiredSize = DesiredSize;
            p.Offset(-desiredSize.Width / 2, -desiredSize.Height / 2);

            // move it "edgLength" on the segment
            var angleBetweenPoints = GetAngleBetweenPoints(p1, p2);
            //p.Offset(edgeLength * Math.Cos(angleBetweenPoints), -edgeLength * Math.Sin(angleBetweenPoints));
            float x = 12.5f, y = 12.5f;
            double sin = Math.Sin(angleBetweenPoints);
            double cos = Math.Cos(angleBetweenPoints);
            double sign = sin * cos / Math.Abs(sin * cos);
            p.Offset(x * sin * sign + edgeLength * cos, y * cos * sign - edgeLength * sin);
            Arrange(new Rect(p, desiredSize));
        }

    }


}
