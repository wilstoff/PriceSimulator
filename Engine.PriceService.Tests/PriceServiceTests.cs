using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data.Objects;
using NUnit.Framework;

namespace Engine.PriceService.Tests
{
    [TestFixture]
    public class PriceServiceTests
    {
        public List<PriceSnapshot> PriceSnapshots;

        public List<Instrument> Instruments;
        
        [SetUp]
        public void Setup()
        {
            PriceSnapshots = new List<PriceSnapshot>();
            Instruments = Instrument.GetInstruments().ToList();
        }

        [Test]
        public void TestPriceUpdatesMultipleTimes()
        {
            var engineUnderTest = new SimulationPriceService();
            engineUnderTest.Subscribe("Test1", Instruments[0].Id, x => PriceSnapshots.Add(x));

            engineUnderTest.Start();
            Thread.Sleep(1099); // Todo: Use TPL or timer for better timing.
            engineUnderTest.Stop();
            engineUnderTest.Unsubscribe("Test1", Instruments[0].Id);
            Assert.That(PriceSnapshots, Has.Count.EqualTo(10).Within(2)); // depending on hardware it may fire less
            Assert.That(PriceSnapshots, Has.All.Property("InstrumentId").EqualTo(Instruments[0].Id));
        }

        [Test]
        public void TestSubscribeToMultipleInstruments()
        {
            var engineUnderTest = new SimulationPriceService();
            engineUnderTest.Subscribe("Test1", Instruments[0].Id, x => PriceSnapshots.Add(x));
            engineUnderTest.Subscribe("Test1", Instruments[1].Id, x => PriceSnapshots.Add(x));

            engineUnderTest.Start();
            Thread.Sleep(1099); // Todo: Use TPL or timer for better timing.
            engineUnderTest.Stop();
            engineUnderTest.Unsubscribe("Test1", Instruments[0].Id);
            engineUnderTest.Unsubscribe("Test1", Instruments[1].Id);
            Assert.That(PriceSnapshots, Has.Count.EqualTo(20).Within(2));// depending on hardware it may fire less
            Assert.That(PriceSnapshots, Has.Some.Property("InstrumentId").EqualTo(Instruments[0].Id));
            Assert.That(PriceSnapshots, Has.Some.Property("InstrumentId").EqualTo(Instruments[1].Id));
        }

        [Test]
        public void TestUnsubscribeDoesNotSendMoreUpdates()
        {
            var engineUnderTest = new SimulationPriceService();
            engineUnderTest.Subscribe("Test1", Instruments[0].Id, x => PriceSnapshots.Add(x));

            engineUnderTest.Start();
            Thread.Sleep(599); // Todo: Use TPL or timer for better timing.
            engineUnderTest.Stop();
            engineUnderTest.Unsubscribe("Test1", Instruments[0].Id);
            Assert.That(PriceSnapshots, Has.Count.EqualTo(5).Within(2)); // depending on hardware it may fire less
            Assert.That(PriceSnapshots, Has.All.Property("InstrumentId").EqualTo(Instruments[0].Id));

            var savedCount = PriceSnapshots.Count;
            engineUnderTest.Start();
            Thread.Sleep(599);
            engineUnderTest.Stop();
            Assert.That(PriceSnapshots, Has.Count.EqualTo(savedCount));
        }
    }
}
