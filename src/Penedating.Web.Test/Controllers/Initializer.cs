using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Penedating.Web.App_Start;

namespace Penedating.Web.Test.Controllers
{
    [SetUpFixture]
    public class Initializer
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperInit.Start();
        }
    }
}
