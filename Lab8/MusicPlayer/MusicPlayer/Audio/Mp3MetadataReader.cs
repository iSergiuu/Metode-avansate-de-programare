using NAudio.Wave;
using System;

namespace MusicPlayer.Audio
{
    public static class Mp3MetadataReader
    {
        public static TimeSpan GetDuration(string path)
        {
            using var reader = new Mp3FileReader(path);
            return reader.TotalTime;
        }
    }
}