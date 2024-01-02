using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintana_AppApuntes.Models;

internal class Note
{
    public string Filename_Quintana { get; set; }
    public string Text_Quintana { get; set; }
    public DateTime Date_Quintana { get; set; }

    public Note()
    {
        Filename_Quintana = $"{Path.GetRandomFileName()}.notes.txt";
        Date_Quintana = DateTime.Now;
        Text_Quintana = "";
    }

    public void Save() =>
        File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename_Quintana), Text_Quintana);

    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename_Quintana));

    public static Note Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                Filename_Quintana = Path.GetFileName(filename),
                Text_Quintana = File.ReadAllText(filename),
                Date_Quintana = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<Note> LoadAll()
    {
        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.notes.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.notes.txt")

                // Each file name is used to load a note
                .Select(filename => Note.Load(Path.GetFileName(filename)))

                // With the final collection of notes, order them by date
                .OrderByDescending(note => note.Date_Quintana);
    }
}
