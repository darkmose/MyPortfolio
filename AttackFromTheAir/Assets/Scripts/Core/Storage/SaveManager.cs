using Core.PlayerModule;
using Core.Events;

public class SaveManager
{
    private IPlayer _player;

    public SaveManager(IPlayer player)
    {
        _player = player;
        EventAggregator.Subscribe<StorageEvent>(OnStorageEvent);
    }

    private void OnStorageEvent(object sender, StorageEvent data)
    {
        _player.Save();
    }
}
