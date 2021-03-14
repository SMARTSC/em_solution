namespace EM.Extend.Lock.Pioneer.Descriptors
{
    public class PasswordAlbumLockDescriptor : Lock.Descriptors.IAlbumLockDescriptor
    {
        public static System.Guid ID = System.Guid.Parse("000301F5-C60D-3F1B-AA21-FACF619DD91C");

        public System.Guid Id => ID;

        public string Name => "Protección por Contraseña";

        public string Description => "Protección de Albumes con contraseña.";
    }
}
