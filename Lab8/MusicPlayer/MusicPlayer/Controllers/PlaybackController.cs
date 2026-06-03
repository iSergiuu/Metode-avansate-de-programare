namespace MusicPlayer.Controllers;

using MusicPlayer.Strategies;

public class PlaybackController
{
    private IPlaybackStrategy _strategy;

    public IPlaybackStrategy CurrentStrategy => _strategy;

    // Strategia este injectata prin constructor
    public PlaybackController(IPlaybackStrategy initialStrategy)
    {
        _strategy = initialStrategy;
    }

    // Permite schimbarea la runtime
    public void SetStrategy(IPlaybackStrategy newStrategy)
    {
        _strategy = newStrategy;
    }
}