using System.Text;
using FileIO.Lib;
using MainApp.Data;
using MainApp.Models;
using MainApp.Services;
using MainApp.UI;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

IPersonCodec[] codecs =
{
    new StudentCodec(),
    new TeacherCodec(),
    new AstronautCodec()
};
IPersonConsoleInput[] personInputs =
{
    new StudentConsoleInput(),
    new TeacherConsoleInput(),
    new AstronautConsoleInput()
};

PersonFileRepository repository = new PersonFileRepository(new FileManager(), codecs);
ConsoleMenu menu = new ConsoleMenu(
    new PersonService(),
    repository,
    personInputs,
    Path.Combine(AppContext.BaseDirectory, "people.txt"));
menu.Run();
