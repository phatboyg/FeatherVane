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
namespace FeatherVane
{
    using System;
    using FeatherVaneConfigurators;


    public static class LogConfigurationExtensions
    {
        public static void Log<T>(this VaneConfigurator<T> configurator,
            Action<LogConfigurator<T>> configureCallback)
        {
            var loggerConfigurator = new LogConfiguratorImpl<T>();

            configureCallback(loggerConfigurator);

            configurator.Add(loggerConfigurator);
        }


        public static void ConsoleLog<T>(this VaneConfigurator<T> configurator, Func<Payload<T>, string> formatter)
        {
            configurator.Log(x => x.SetOutput(Console.Out).SetFormat(formatter));
        }
    }
}