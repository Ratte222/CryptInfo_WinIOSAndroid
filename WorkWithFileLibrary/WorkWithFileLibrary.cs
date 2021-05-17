using System;
using System.IO;
using System.Text;

namespace WorkWithFileLibrary
{
    public class CWorkWithFileLibrary
    {
        public static bool rewriteLineText(int rewriteLine, string path, string content, Encoding encoding)
        {
            int countBytes = encoding.GetBytes("r").Length;
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
                var buff = new byte[countBytes];
                int byteStart = rewriteLine == 1 ? 0 : -1, byteEnd = -1;

                for (int i = 0, line = 1; i < fs.Length; i += countBytes)
                {
                    fs.Read(buff, 0, countBytes);
                    if (encoding.GetString(buff) == "\n")
                    {//"\n" - перенос строки
                        if (line == rewriteLine)
                        {
                            byteEnd = i;
                            break;
                        }
                        line++;
                        if (line == rewriteLine)
                            byteStart = i + countBytes;
                    }
                    if (i == fs.Length - 1)
                        byteEnd = i;
                }
                if (byteStart == -1 || byteEnd == -1)
                    return false;
                var strByte = encoding.GetBytes(content);
                fs.Position = byteEnd;
                var tailBuff = new byte[fs.Length - byteEnd];
                fs.Read(tailBuff, 0, (int)(fs.Length - byteEnd));
                fs.Position = byteStart;
                fs.Write(strByte, 0, strByte.Length);
                fs.Write(tailBuff, 0, tailBuff.Length);
                fs.SetLength(byteStart + strByte.Length + tailBuff.Length);
                return true;
            }
            //catch { }
            finally
            {
                fs?.Close();
                fs?.Dispose();
            }
            return false;
        }

        public static bool rewriteMultiLineText(int rewriteLineStart, int countRewriteLine, string path, string content, Encoding encoding)// do not work
        {
            int countBytes = encoding.GetBytes("r").Length;
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
                var buff = new byte[countBytes];
                int byteStart = rewriteLineStart == 1 ? 0 : -1, byteEnd = -1;
                
                for (int i = 0, line = 1; i < fs.Length; i += countBytes)
                {
                    fs.Read(buff, 0, countBytes);
                    if (encoding.GetString(buff) == "\n")
                    {//"\n" - перенос строки
                        if (line == rewriteLineStart + countRewriteLine - 1)
                        {
                            byteEnd = i;
                            break;
                        }
                        line++;
                        if (line == rewriteLineStart)
                            byteStart = i + countBytes;
                    }
                    if (i == fs.Length - 1)
                        byteEnd = i;
                }
                if (byteStart == -1 || byteEnd == -1)
                    return false;
                var strByte = encoding.GetBytes(content);
                fs.Position = byteEnd;
                var tailBuff = new byte[fs.Length - byteEnd];
                fs.Read(tailBuff, 0, (int)(fs.Length - byteEnd));
                fs.Position = byteStart;
                fs.Write(strByte, 0, strByte.Length);
                fs.Write(tailBuff, 0, tailBuff.Length);
                fs.SetLength(byteStart + strByte.Length + tailBuff.Length);
                return true;
            }
            //catch { }
            finally
            {
                fs?.Flush();
                fs?.Close();
                fs?.Dispose();
            }
            return false;
        }
    }
}
