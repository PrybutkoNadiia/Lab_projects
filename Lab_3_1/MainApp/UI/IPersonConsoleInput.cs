using MainApp.Models;

namespace MainApp.UI;

public interface IPersonConsoleInput
{
    string MenuName { get; }
    Person ReadFromConsole();
}
