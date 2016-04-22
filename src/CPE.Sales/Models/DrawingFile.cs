using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CPE.Sales.Models
{
    public sealed class DrawingFile : IComparable<DrawingFile>, IEquatable<DrawingFile>
    {
        public string FileName { get; set; }

        public string FullPath { get; set; }

        public DateTime CreatedAt { get; set; }

        public BitmapSource Icon { get; set; }

        public int CompareTo(DrawingFile other)
        {
            return CreatedAt.CompareTo(other.CreatedAt);
        }

        public bool Equals(DrawingFile other)
        {
            return other != null && FullPath.Equals(other.FullPath);
        }
    }
}
