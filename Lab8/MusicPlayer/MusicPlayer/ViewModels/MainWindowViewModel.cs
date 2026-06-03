namespace MusicPlayer.ViewModels;

using System.ComponentModel;
using System.Windows.Input;
using MusicPlayer.Audio;
using MusicPlayer.Models;
using MusicPlayer.Commands;
using MusicPlayer.Controllers;
using MusicPlayer.Strategies;
using MusicPlayer.Observers;
using System;
using System.Collections.Generic;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public AudioPlayer Player { get; }
    public Playlist Playlist { get; }
    public CommandHistory History { get; }
    public PlaybackController Controller { get; }
    public StatisticsTracker StatsTracker { get; }

    // Proprietati pentru interfata
    public IEnumerable<IPlayerCommand> HistoryList => History.GetHistory();
    public StatisticsSnapshot Stats => StatsTracker.Snapshot;

    // Comenzi WPF pentru butoane
    public ICommand PlayCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand UndoCommand { get; }
    public ICommand RedoCommand { get; }
    public ICommand ChangeToShuffleCommand { get; }

    public MainWindowViewModel()
    {
        // 1. Initializăm Modelele
        Playlist = new Playlist();
        Player = new AudioPlayer();
        History = new CommandHistory();

        // 2. Initializam Controller-ul cu o strategie default
        Controller = new PlaybackController(new SequentialStrategy());

        // 3. Initializăm Observatorii
        var logger = new PlaybackLogger(Player);
        StatsTracker = new StatisticsTracker(Player);
        var autoNext = new AutoNextHandler(Player, Playlist, () => Controller.CurrentStrategy);

        // Observer 1: ViewModel-ul se abonează la Player pentru a actualiza UI-ul
        Player.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(e.PropertyName ?? string.Empty);
            if (e.PropertyName == nameof(AudioPlayer.CurrentTrack))
                OnPropertyChanged(nameof(Stats)); // Actualizăm statisticile la schimbarea piesei
        };

        // 4. Definim acțiunile butoanelor din UI
        PlayCommand = new RelayCommand(() =>
        {
            var cmd = new Commands.PlayCommand(Player);
            History.Execute(cmd);
        }, () => Player.CurrentTrack != null && Player.State != PlayerState.Playing);

        PauseCommand = new RelayCommand(() =>
        {
            var cmd = new Commands.PauseCommand(Player);
            History.Execute(cmd);
        }, () => Player.State == PlayerState.Playing);

        UndoCommand = new RelayCommand(() =>
        {
            History.Undo();
            OnPropertyChanged(nameof(HistoryList));
        }, () => History.CanUndo);

        RedoCommand = new RelayCommand(() =>
        {
            History.Redo();
            OnPropertyChanged(nameof(HistoryList));
        }, () => History.CanRedo);

        // Exemplu comanda schimbare strategie la Shuffle
        ChangeToShuffleCommand = new RelayCommand(() =>
        {
            // Aici în mod normal ar trebui un ChangeStrategyCommand cu Undo. Pentru brevitate, setam direct:
            Controller.SetStrategy(new ShuffleStrategy());
            Controller.CurrentStrategy.Reset(Playlist);
        });
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}