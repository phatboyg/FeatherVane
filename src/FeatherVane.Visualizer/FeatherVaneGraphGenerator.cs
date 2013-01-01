// Copyright 2012-2012 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
// except in compliance with the License. You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
// ANY KIND, either express or implied. See the License for the specific language governing
// permissions and limitations under the License.
namespace FeatherVane.Visualizer
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using Microsoft.Glee.Drawing;
    using Microsoft.Glee.GraphViewerGdi;
    using QuickGraph;
    using QuickGraph.Glee;
    using SourceVanes;
    using Vanes;
    using Visualization;


    public class FeatherVaneGraphGenerator
    {
        public Graph CreateGraph(FeatherVaneGraph data)
        {
            var graph = new AdjacencyGraph<Vertex, Edge<Vertex>>();

            graph.AddVertexRange(data.Vertices);
            graph.AddEdgeRange(data.Edges.Select(x => new Edge<Vertex>(x.From, x.To)));

            GleeGraphPopulator<Vertex, Edge<Vertex>> glee = graph.CreateGleePopulator();

            glee.NodeAdded += NodeStyler;
            glee.EdgeAdded += EdgeStyler;
            glee.Compute();

            Graph gleeGraph = glee.GleeGraph;

            return gleeGraph;
        }

        public void SaveGraphToFile(FeatherVaneGraph data, int width, int height, string filename)
        {
            Graph gleeGraph = CreateGraph(data);

            var renderer = new GraphRenderer(gleeGraph);
            renderer.CalculateLayout();

            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            renderer.Render(bitmap);

            bitmap.Save(filename, ImageFormat.Png);
        }

        void NodeStyler(object sender, GleeVertexEventArgs<Vertex> args)
        {
            args.Node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.White;
            args.Node.Attr.Fontsize = 8;
            args.Node.Attr.FontName = "Arial";
            args.Node.Attr.Padding = 1.2;

            if (args.Vertex.VertexType.GetGenericTypeDefinition() == typeof(Success<>))
            {
                args.Node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.White;
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Green;
                args.Node.Attr.Shape = Shape.Box;

                args.Node.Attr.Label = "Success";
            }
            else if (args.Vertex.VertexType.GetGenericTypeDefinition() == typeof(Unhandled<>))
            {
                args.Node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.White;
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.DarkRed;
                args.Node.Attr.Shape = Shape.Box;

                args.Node.Attr.Label = "Unhandled";
            }
            else if (args.Vertex.VertexType.GetGenericTypeDefinition() == typeof(Factory<>)
                     || args.Vertex.VertexType.GetGenericTypeDefinition() == typeof(Instance<>))
            {
                args.Node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.White;
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Navy;
                args.Node.Attr.Shape = Shape.House;
                args.Node.Attr.Label = args.Vertex.Title;
                args.Node.Attr.AddStyle(Style.Bold);
            }
            else
            {
                args.Node.Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Black;
                args.Node.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Cyan;
                args.Node.Attr.Shape = Shape.Box;

                args.Node.Attr.Label = args.Vertex.Title;
            }
        }

        static void EdgeStyler(object sender, GleeEdgeEventArgs<Vertex, Edge<Vertex>> e)
        {
            e.GEdge.EdgeAttr.FontName = "Tahoma";
            e.GEdge.EdgeAttr.Fontsize = 6;

            if (e.Edge.Source.VertexType.GetGenericTypeDefinition() == typeof(Factory<>))
                e.GEdge.EdgeAttr.AddStyle(Style.Dashed);
            if (e.Edge.Source.VertexType.GetGenericTypeDefinition() == typeof(Instance<>))
                e.GEdge.EdgeAttr.AddStyle(Style.Dashed);
        }
    }
}