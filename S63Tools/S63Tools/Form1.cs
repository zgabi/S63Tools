using System.Buffers.Binary;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace S63Tools
{
    public partial class Form1 : Form
    {
        private readonly int _zipHeader = 0x04034b50; // 'P', 'K', 3, 4
        private byte[]? _hardwareId;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            var hwId = S63Tools.HackUserPermit(textBoxUserPermit.Text, out var mId, out var keyBytes);
            _hardwareId = hwId;
            labelMId.Text = $"x{mId:X4} ({(char)(mId >> 8)}{(char)(mId & 0xff)})";
            labelMKey.Text = Encoding.ASCII.GetString(keyBytes ?? Array.Empty<byte>());
            labelHwId.Text = Encoding.ASCII.GetString(hwId ?? Array.Empty<byte>());
        }

        private void buttonCalculateFromPermit_Click(object sender, EventArgs e)
        {
            if (openFileDialogPermit.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var hwId = S63Tools.HackCellPermit(openFileDialogPermit.FileName);
            _hardwareId = hwId;
            labelHwId.Text = Encoding.ASCII.GetString(hwId ?? Array.Empty<byte>());
        }

        private void buttonDecryptCells_Click(object sender, EventArgs e)
        {
            var hwId = _hardwareId;
            if (hwId == null)
            {
                MessageBox.Show("First decrypt the HW_ID.");
                return;
            }

            var permits = new Dictionary<string, (byte[], byte[])>();
            if (openFileDialogPermit.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            S63Tools.LoadPermit(openFileDialogPermit.FileName, permits, new[] { hwId });

            foreach (var file in Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.*", SearchOption.AllDirectories))
            {
                var ext = Path.GetExtension(file).TrimStart('.');
                if (!int.TryParse(ext, out _))
                {
                    continue;
                }

                byte[]? zipData = null;
                var data = File.ReadAllBytes(file);
                string fn = Path.GetFileNameWithoutExtension(file);
                if (permits.TryGetValue(fn, out var cellKeys))
                {
                    var blow = new BlowFish(cellKeys.Item1);
                    var decFile = blow.Decrypt(data, CipherMode.ECB);

                    int header = BinaryPrimitives.ReadInt32LittleEndian(decFile);
                    if (header != _zipHeader)
                    {
                        blow = new BlowFish(cellKeys.Item2);
                        decFile = blow.Decrypt(data, CipherMode.ECB);
                        header = BinaryPrimitives.ReadInt32LittleEndian(decFile);
                    }

                    if (header == _zipHeader)
                    {
                        zipData = decFile;
                    }
                }

                if (zipData == null)
                {
                    continue;
                }

                var zip = new ZipArchive(new MemoryStream(zipData));
                var entry = zip.Entries[0];

                data = new byte[entry.Length];
                int read = 0;
                var stream = entry.Open();
                while (read != data.Length)
                {
                    read += stream.Read(data, read, data.Length - read);
                }

                File.WriteAllBytes(Path.Combine(folderBrowserDialog1.SelectedPath, "..", Path.GetFileName(file)), data);
            }
        }
    }
}