namespace Brainshare.Infrastructure.Platform.Output
{
    public interface IInputReader
    {
        string ReadLine();
        void ReadKey();
    }
}