using MainApp.Models;

namespace MainApp.Data;

public interface IPersonCodec
{
    string TypeName { get; }
    bool CanSerialize(Person person);
    string Serialize(Person person);
    Person Deserialize(string json);
}
