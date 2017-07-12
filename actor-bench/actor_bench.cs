using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ActorBench
{
    class Actor
    {
        public double X;
        public double Y;
        public double Vx;
        public double Vy;

        public void Update()
        {
            this.X += this.Vx;
            this.Y += this.Vy;
        }
    }

    class Program
    {
        static Actor[] CreateActors(int num)
        {
            var list = new List<Actor>(num);

            for (int i = 0; i < num; i++)
            {
                var newActor = new Actor()
                {
                    //X = i / 10.0f,
                    //Y = i * 2 / 10.0f,
                    //Vx = i / 100.0f,
                    //Vy = i * 2 / 100.0f,
                    Vx = 0.000001 * i,
                    Vy = 0.000002 * i,
                };
                list.Add(newActor);
            }
            return list.ToArray();
        }

        static MethodInfo _methodInfo;

        static void UpdateAll(Actor[] actors)
        {
            var time0 = DateTime.Now;
            foreach (var a in actors)
            {
                //a.Update();
                //if (_methodInfo == null)
                {
                    _methodInfo = a.GetType().GetMethod("Update");
                }

                _methodInfo.Invoke(a, null);

            }
            var time1 = DateTime.Now;
            var o0 = (time1.Ticks - time0.Ticks) / 10000.0;

        }

        static void aaa(dynamic a)
        {
            a.Update();
        }

        static void Main(string[] args)
        {
            var time0 = DateTime.Now;

            var actors = CreateActors(10000);
            for (int i = 0; i < 30000; i++)
            {
                UpdateAll(actors);

                if (i % 1000 == 0)
                {
                    Console.WriteLine(i);
                }
            }

            var time1 = DateTime.Now;

            Console.WriteLine(actors[5000].X);
            Console.WriteLine(actors[5000].Y);

            Console.WriteLine((time1.Ticks - time0.Ticks) / 10000);
        }
    }
}
