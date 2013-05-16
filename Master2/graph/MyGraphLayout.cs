using GraphSharp.Controls;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Master2.graph
{
  
        public class MyGraph : GraphSharp.SoftMutableBidirectionalGraph<MyVertexBase, MyEdgeBase>
        {
            /* nothing to do here... */
        }


        public class MyGraphLayout : GraphSharp.Controls.GraphLayout<MyVertexBase, MyEdgeBase, MyGraph>
        {
            /* nothing to do here... */
        }

        

        public class MyEdgeBase : Edge<MyVertexBase>
        {
            /* nothing to do here... */

            public MyEdgeBase(MyVertexBase fromvertex, MyVertexBase tovertex)
                : base(fromvertex, tovertex)
            {

            }

        }

        

    
}
