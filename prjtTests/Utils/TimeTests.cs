using Microsoft.VisualStudio.TestTools.UnitTesting;
using prjt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjt.Utils.Tests
{
    [TestClass()]
    public class TimeTests
    {
        [TestMethod()]
        public void TimeTest()
        {
            Time t1 = new Time("1:05:10");

            Assert.AreEqual(3910, t1.TotalSeconds);
            Assert.AreEqual(1, t1.Hours);
            Assert.AreEqual(5, t1.Minutes);
            Assert.AreEqual(10, t1.Seconds);
            Assert.AreEqual("01:05", t1.HoursAndMinutes);
            Assert.AreEqual("01:05:10", t1.Text);
            Assert.AreEqual(false, t1.IsNegative);

            // -----


            Time t2 = new Time("-1:05:10");

            Assert.AreEqual(-3910, t2.TotalSeconds);
            Assert.AreEqual(-1, t2.Hours);
            Assert.AreEqual(-5, t2.Minutes);
            Assert.AreEqual(-10, t2.Seconds);
            Assert.AreEqual("-01:05", t2.HoursAndMinutes);
            Assert.AreEqual("-01:05:10", t2.Text);
            Assert.AreEqual(true, t2.IsNegative);
        }


        [TestMethod()]
        public void GetNegativeTest()
        {
            Time t1 = new Time("1:00");
            Assert.AreEqual(new Time("-1:00"), t1.GetNegative());

            Time t2 = new Time();
            Assert.AreEqual(new Time("00:00"), t2.GetNegative());
        }


        [TestMethod()]
        public void Seconds2TimeTest()
        {
            Assert.AreEqual("01:00:00", Time.Seconds2Time(3600));
            Assert.AreEqual("-01:00:00", Time.Seconds2Time(-3600));
            Assert.AreEqual("00:00:00", Time.Seconds2Time(0));
        }


        [TestMethod()]
        public void Time2SecondsTest()
        {
            Assert.AreEqual(3600, Time.Time2Seconds("01:00:00"));
            Assert.AreEqual(3600, Time.Time2Seconds("01:00"));
            Assert.AreEqual(3600, Time.Time2Seconds("1:00"));
            Assert.AreEqual(0, Time.Time2Seconds("00:00"));
            Assert.AreEqual(-3600, Time.Time2Seconds("-1:00"));
        }


        [TestMethod()]
        public void AreEqualTest()
        {
            Time t1 = new Time("01:00:00");
            Time t2 = new Time("01:00:00");

            Assert.IsTrue(t1 == t2);
            Assert.IsTrue(t1 == 3600);
            Assert.IsTrue(3600 == t1);


            Time t3 = new Time("-01:00:00");
            Time t4 = new Time("-01:00:00");

            Assert.IsTrue(t3 == t4);
            Assert.IsTrue(t3 == -3600);
            Assert.IsTrue(-3600 == t3);
        }


        [TestMethod()]
        public void AreNotEqualTest()
        {
            Time t1 = new Time("01:00:00");
            Time t2 = new Time("02:00:00");

            Assert.IsTrue(t1 != t2);
            Assert.IsTrue(t1 != 3000);
            Assert.IsTrue(3000 != t1);


            Time t3 = new Time("-01:00:00");
            Time t4 = new Time("-02:00:00");

            Assert.IsTrue(t3 != t4);
            Assert.IsTrue(t3 != -3000);
            Assert.IsTrue(-3000 != t3);
        }


        [TestMethod()]
        public void SumTest()
        {
            Time t1 = new Time("01:00:00");
            Time t2 = new Time("02:00:00");
            Time t3 = new Time("-05:00:00");

            Assert.AreEqual(new Time("03:00"), t1 + t2);
            Assert.AreEqual(new Time("02:00"), t1 + 3600);
            Assert.AreEqual(new Time("02:00"), 3600 + t1);

            Assert.AreEqual(new Time("-04:00"), t3 + t1);
            Assert.AreEqual(new Time("-06:00"), t3 + (-3600));
            Assert.AreEqual(new Time("-06:00"), (-3600) + t3);
        }


        [TestMethod()]
        public void SubTest()
        {
            Time t1 = new Time("02:00:00");
            Time t2 = new Time("01:00:00");

            Assert.AreEqual(new Time("01:00"), t1 - t2);
            Assert.AreEqual(new Time("01:00"), t1 - 3600);
            Assert.AreEqual(new Time("-01:00"), 3600 - t1);
        }


        [TestMethod()]
        public void HigherThanTest()
        {
            Assert.IsTrue(new Time("01:00") > new Time("00:00"));
            Assert.IsTrue(new Time("01:00") > 0);
            Assert.IsTrue(0 < new Time("01:00"));
        }


        [TestMethod()]
        public void HigherOrEqualThanTest()
        {
            Assert.IsTrue(0 <= new Time("00:00"));
        }


        [TestMethod()]
        public void LowerThanTest()
        {
            Assert.IsTrue(new Time("00:00") < new Time("01:00"));
            Assert.IsTrue(new Time("00:00") < 3600);
            Assert.IsTrue(3600 > new Time("00:00"));
        }


        [TestMethod()]
        public void LowerOrEqualThanTest()
        {
            Assert.IsTrue(0 >= new Time("00:00"));
        }
    }
}