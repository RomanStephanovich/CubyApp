using Xunit;
using Moq;
using CUBY.Services.Implementations;
using CUBY.Services.Interfaces;


namespace CUBY.Test
{
    public class CncServiceTests
    {
        [Fact]
        public void GetInfo_ShouldReturnSuccessMessage_WhenResultIsZero()
        {
            var mock = new Mock<ISyntecCncClient>();

            mock.Setup(x => x.READ_information(
                out It.Ref<short>.IsAny,
                out It.Ref<string>.IsAny,
                out It.Ref<short>.IsAny,
                out It.Ref<string>.IsAny,
                out It.Ref<string>.IsAny,
                out It.Ref<string[]>.IsAny))
            .Callback(new ReadInfoCallback((out short axes, out string cncType, out short maxAxes, out string series, out string version, out string[] axisNames) =>
            {
                axes = 3;
                cncType = "CNC-9000";
                maxAxes = 6;
                series = "S-9";
                version = "1.2.3";
                axisNames = new[] { "X", "Y", "Z" };
            }))
            .Returns(0);

            var service = new CncService(mock.Object);

            var result = service.GetInfo();

            Assert.Contains("Machine Type: CNC-9000", result);
            Assert.Contains("Axes: 3", result);
        }

        [Fact]
        public void GetStatus_ShouldReturnError_WhenResultIsNegative()
        {
            var mock = new Mock<ISyntecCncClient>();

            mock.Setup(x => x.READ_status(
                out It.Ref<string>.IsAny,
                out It.Ref<string>.IsAny,
                out It.Ref<int>.IsAny,
                out It.Ref<string>.IsAny,
                out It.Ref<string>.IsAny,
                out It.Ref<string>.IsAny,
                out It.Ref<string>.IsAny))
            .Returns(-16);

            var service = new CncService(mock.Object);

            var result = service.GetStatus();

            Assert.Contains("Failed to read status", result);
        }

        private delegate void ReadInfoCallback(
            out short axes,
            out string cncType,
            out short maxAxes,
            out string series,
            out string version,
            out string[] axisNames);
    }
}