using System;

namespace MusicPlayer.Models;

public record Track(Guid Id, string Title, string Artist, string Album, TimeSpan Duration, string FilePath);