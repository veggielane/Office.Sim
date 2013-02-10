using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Office.Sim.Core;
using Office.Sim.Core.GameObjects;
using Office.Sim.Core.Graphics;
using Office.Sim.Core.Graphics.OpenTK;
using Office.Sim.Core.Mapping;
using Office.Sim.Core.Messaging;
using Office.Sim.Core.Timing;

namespace Office.Sim.Test
{
    class Program
    {
        public class ContainerSetup
        {
            private ContainerBuilder _builder;
            public IContainer BuildContainer()
            {
                _builder = new ContainerBuilder();

                _builder.RegisterType<TestGameEngine>().As<IGameEngine>().SingleInstance();
                _builder.RegisterType<Timer>().As<ITimer>().SingleInstance();



                _builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
                _builder.RegisterType<OpenTKGraphicsEngine>().As<IGraphicsEngine>().SingleInstance();
                _builder.RegisterType<TestLevel>().As<ILevel>().SingleInstance();

                _builder.RegisterType<TestGameObjectFactory>().As<IGameObjectFactory>().SingleInstance();

                


              //  _builder.RegisterType<AutofacObjectCreator>().As<IObjectCreator>();


                return _builder.Build();
            }
        }

        static void Main(string[] args)
        {
            var container = new ContainerSetup().BuildContainer();
            using (var game = container.Resolve<IGameEngine>())
            {
                game.Bus.Messages.Subscribe(Console.WriteLine);
                game.Start();
                Console.ReadLine();
            }
            Console.ReadLine();
        }

        public class TestGameObjectFactory:BaseGameObjectFactory
        {
            
        }

        /*
        public class AutofacObjectCreator:IObjectCreator
        {
            private readonly IContainer _container;

            public AutofacObjectCreator(IContainer container)
            {
                _container = container;
            }

            public T Create<T>()
            {
                return _container.Resolve<T>();
            }
        }*/
    }
}
