using NUnit.Framework;
using Prost.Runtime;
using System;

namespace Prost.Tests.Runtime
{
    [TestFixture]
    class ThreadPoolTest
    {
        [Test]
        public void PoolFullException()
        {
            ThreadPool pool = new ThreadPool(2);

            pool.Run((obj) => { System.Threading.Thread.Sleep(200); });
            pool.Run((obj) => { System.Threading.Thread.Sleep(100); });
            try
            {
                pool.Run((obj) => { System.Threading.Thread.Sleep(200); });
            }
            catch (ThreadPoolFullException)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test, MaxTime(1000)]
        public void StopAndWait()
        {
            ThreadPool pool = new ThreadPool(2);
            bool flag = false;

            pool.Run((obj) => {
                System.Threading.Thread.Sleep(300);
                flag = true;
            });

            // Si el proceso se para antes de que el subhilo comience a ejecutar
            // la tarea, el test puede fallar.
            System.Threading.Thread.Sleep(100);

            pool.Stop(true);

            Assert.AreEqual(pool.AvailableThreads, 2);
            Assert.IsTrue(flag);
        }

        [Test, MaxTime(1000)]
        public void StopAndDontWait()
        {
            ThreadPool pool = new ThreadPool(2);
            bool flag = false;

            pool.Run((obj) => {
                System.Threading.Thread.Sleep(300);
                flag = true;
            });

            pool.Stop(false);

            Assert.AreEqual(pool.AvailableThreads, 2);

            System.Threading.Thread.Sleep(500);

            Assert.IsFalse(flag);
        }
    }
}
