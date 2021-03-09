using Microsoft.VisualStudio.TestTools.UnitTesting;
using T2BootstrapServers.Tokenization;
using T2BootstrapServers.Tokenization.Model;

namespace T2BootstrapServer.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GenerateToken()
        {

            IAuthService service = new JwtService("aGs4andKSWZUaFSALEMVlN2dzSVhPWTJicHpoMzljUndXR1I0Zm5tS2NXb29CWUZs");
            JWTContainerModel model = new JWTContainerModel();
            model.ExpireDays = 360 * 5;
            model.Issuer = "https://www.t2.sa";
           var token = service.GenerateToken(model);
            var d = "";

        }
    }
}
