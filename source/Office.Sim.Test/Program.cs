using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Office.Sim.Core;
using Office.Sim.Core.Messaging;

namespace Office.Sim.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new ContainerSetup().BuildContainer();
            using (var game = container.Resolve<IGameEngine>())
            {
                game.Bus.Messages.Subscribe(Console.WriteLine);
                game.LoadLevel(new TestLevel());
                game.Start();
                Console.ReadLine();
            }
            Console.ReadLine();
        }

        public class ContainerSetup
        {
            private ContainerBuilder _builder;

            public IContainer BuildContainer()
            {
                _builder = new ContainerBuilder();
                _builder.RegisterType<GameEngine>().As<IGameEngine>().SingleInstance();
                _builder.RegisterType<MessageBus>().As<IMessageBus>().SingleInstance();
                return _builder.Build();
            }
        }

        public class TestLevel:ILevel
        {
            public IMap Map { get; private set; }
            public TestLevel()
            {
                Map = new TestMap(12);
            }
        }

        public class TestMap:IMap
        {
            public ITile[,] Tiles { get; private set; }
            public TestMap(int size)
            {
                Tiles = new ITile[12,12];
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        Tiles[i,j] = new TestTile(0);
                    }
                }
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                for (var i = 0; i < Tiles.GetLength(0); i++)
                {
                    for (var j = 0; j < Tiles.GetLength(1); j++)
                    {
                        sb.Append(Tiles[i, j].Height);
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }

        public class TestTile:ITile
        {
            public int Height { get; private set; }
            public TestTile(int height)
            {
                Height = height;
            }
        }
    }
}
