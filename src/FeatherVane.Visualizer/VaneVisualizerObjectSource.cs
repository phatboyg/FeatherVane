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
    using System;
    using System.IO;
    using Microsoft.VisualStudio.DebuggerVisualizers;
    using Visualization;


    public class VaneVisualizerObjectSource :
        VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            if (target == null)
                return;

            Type vaneType = target.GetType();
            Type genericType = GetGenericType(vaneType);
            if (genericType == null)
                return;

            object graph = GetType()
                .GetMethod("CreateGraph")
                .MakeGenericMethod(genericType)
                .Invoke(this, new[] {target});

            base.GetData(graph, outgoingData);
        }

        Type GetGenericType(Type vaneType)
        {
            while (vaneType != null && vaneType != typeof(object))
            {
                if (vaneType.IsGenericType && vaneType.GetGenericTypeDefinition() == typeof(Vane<>))
                {
                    Type genericType = vaneType.GetGenericArguments()[0];
                    return genericType;
                }

                vaneType = vaneType.BaseType;
            }

            return null;
        }

        FeatherVaneGraph CreateGraph<T>(Vane<T> vane)
        {
            var visitor = new GraphVaneVisitor();
            visitor.Visit(vane);

            return visitor.GetGraphData();
        }
    }
}