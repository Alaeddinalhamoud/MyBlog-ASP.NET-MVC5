using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service
{
   public interface IDEncryptionRepository
    {
        string Encrypt(string clearText);
        string Decrypt(string cipherText);

    }
}
