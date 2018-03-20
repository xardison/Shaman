using System.IO;

namespace Shaman.Common
{
    public interface IScreenCapture
    {
        Stream CaptureScreenToStream();
    }
}