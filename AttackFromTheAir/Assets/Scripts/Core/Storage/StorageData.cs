using System.Collections.Generic;

namespace Core.Storage
{
    public interface IStoragable
    {
        void Save();

        void Load();

        event System.Action OnLoadCompleteEvent;
    }

    public interface IInitable
    {
        void Init();
    }

    public interface IStoragable<Tdata>
    {
        void Save(Tdata data);

        void Load(Tdata data);
    }

    public interface IStoragableDictionary : IStoragable<Dictionary<string, object>>, IInitable
    {

    }
}
