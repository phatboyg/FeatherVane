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
    using System.IO;
    using Visualization;


    public static class VaneVisualizationExtensions
    {
        public static void RenderGraphToFile<T>(this Vane<T> vane, FileInfo fileInfo, int width = 1920,
            int height = 1080)
        {
            var graphVisitor = new GraphVaneVisitor();
            graphVisitor.Visit(vane);

            FeatherVaneGraph graph = graphVisitor.GetGraphData();

            new FeatherVaneGraphGenerator()
                .SaveGraphToFile(graph, width, height, fileInfo.FullName);
        }
    }
}