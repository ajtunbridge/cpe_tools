namespace CPE.Domain.Security.Cryptography
{
    public interface IHashProvider
    {
        byte[] ComputeHashToBytes(byte[] bytes);
        string ComputeHashToString(byte[] bytes);
    }
}