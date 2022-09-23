namespace AuthorizationApp.DialogService
{
    internal interface IFileService
    {
        byte[] Open(string fileName);
        void Save(string fileName, byte[] photoBinary);
    }
}