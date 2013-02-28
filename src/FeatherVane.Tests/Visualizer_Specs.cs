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
namespace FeatherVane.Tests
{
    using Feathers;
    using NUnit.Framework;
    using Vanes;
    using Visualization;
    using Visualizer;


    [TestFixture, Explicit]
    public class Displaying_a_graph_with_the_visualizer
    {
        [Test]
        public void Should_render_properly()
        {
            Vane<A> vane = VaneFactory.Success(new TransactionFeather<A>(),
                new ExecuteFeather<A>(x => { }),
                new CompensateFeather<A>(x => false));

            var graphVisitor = new GraphVaneVisitor();
            graphVisitor.Visit(vane);

            FeatherVaneGraph graph = graphVisitor.GetGraphData();

            new FeatherVaneGraphGenerator().SaveGraphToFile(graph, 1920, 1080, "superGraph.png");
        }


        class A
        {
        }
    }
}