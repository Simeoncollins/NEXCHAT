using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness.Classes
{
    public class GuidCombiner
    {
        public static Guid CombineGuids(Guid guid1, Guid guid2)
        {
            var ordered = new[] { guid1, guid2 }.OrderBy(g => g).ToArray();
            var bytes = ordered[0].ToByteArray().Concat(ordered[1].ToByteArray()).ToArray();

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(bytes);

            // Truncate the 32-byte SHA256 hash to 16 bytes for a GUID
            return new Guid(hash.Take(16).ToArray());
        }
    }
}
