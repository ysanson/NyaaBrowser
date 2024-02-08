using System;

namespace NyaapiDotnet.Si.Models
{
    public class SiTorrent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Hash { get; set; }
        public DateTime Date { get; set; }
        public string Filesize { get; set; }
        public string Description { get; set; }
        public string SubCategory { get; set; }
        public string Category { get; set; }
        public string Magnet { get; set; }
        public string Torrent { get; set; }
        public int Leechers { get; set; }
        public int Seeders { get; set; }
        public int Completed { get; set; }
    }
}