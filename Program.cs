string omina_directory = $@"G:\Storage\Archives\Pixiv Archives";
string hitomi_directory = $@"G:\Storage\Macrojack VR Build Programs\hitomi_downloader_GUI\hitomi_downloaded_pixiv";

if (!Directory.Exists(omina_directory))
{
    Console.WriteLine("Omina directory does not exist. Please check your input and try again.\n");
    return;
}

if (!Directory.Exists(hitomi_directory))
{
    Console.WriteLine("Hitomi directory does not exist. Please check your input and try again.\n");
    return;
}

Console.WriteLine("Counting Omina subfolders...\n");

int omina_subfolder_count = Directory.GetDirectories(omina_directory).Length;

Console.WriteLine($"Omina subfolder directory count is {omina_subfolder_count}. Continue? (Y/N)\n");

bool validation = false;

while (validation == false)
{
    var user_confirmation = Console.ReadLine();

    if (user_confirmation != null)
    {
        user_confirmation = user_confirmation.ToLower();

        if (user_confirmation == "n" || user_confirmation == "no")
        {
            return;
        }
        else if (user_confirmation == "y" || user_confirmation == "yes")
        {
            validation = true;
        }
    }
}

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
string[] artist_subdirectories_paths = Directory.GetDirectories(omina_directory);
string[] artist_subdirectories_names = Directory.GetDirectories(omina_directory).Select(Path.GetFileName).ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

string artist_folder_name = "";
string artwork_id = "";
string artwork_name = "";
//string image_number = "";

for (int i = 21; i < 22; i++) // i < omina_subfolder_count
{
    artist_folder_name = artist_subdirectories_names[i];

    Console.WriteLine($"({i + 1} / {omina_subfolder_count}) Current folder: {artist_folder_name}\n");

    string new_hitomi_artist_directory = $@"{hitomi_directory}\{artist_folder_name}";

    if (!Directory.Exists(new_hitomi_artist_directory))
    {
        Directory.CreateDirectory(new_hitomi_artist_directory);
    }

    #pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
    string current_artist_subdirectory_path = artist_subdirectories_paths[i];
    string[] artwork_of_artist_folder_paths = Directory.GetDirectories(current_artist_subdirectory_path);
    string[] artwork_of_artist_folder_names = Directory.GetDirectories(current_artist_subdirectory_path).Select(Path.GetFileName).ToArray();
    #pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

    for (int j = 0; j < artwork_of_artist_folder_names.Length; j++)
    {
        char[] delimiterChars = { '_' };
        List<string> omina_artwork_folder_title = artwork_of_artist_folder_names[j].Split(delimiterChars).ToList();

        artwork_id = omina_artwork_folder_title[0];
        omina_artwork_folder_title.RemoveAt(0);

        artwork_name = String_List_To_String(omina_artwork_folder_title);

        string[] artwork_image_paths = Directory.GetFiles(artwork_of_artist_folder_paths[j]);

        for (int pic_number = 0; pic_number < artwork_image_paths.Length; pic_number++)
        {
            string image_extention = Path.GetExtension($@"{artwork_of_artist_folder_paths[j]}\{artwork_image_paths[pic_number]}");
            string new_filename = $@"{artwork_id}_p{pic_number} {artwork_name}{image_extention}";

            Console.WriteLine($"Transferring {new_filename}...");

            System.IO.File.Copy($@"{artwork_image_paths[pic_number]}", $@"{new_hitomi_artist_directory}\{new_filename}");
        }
    }

    // GIF Transfer
    #pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
    string[] gif_files = Directory.GetFiles($@"{current_artist_subdirectory_path}", "*.gif").Select(Path.GetFileName).ToArray();
    #pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

    for (int iterated_gif = 0; iterated_gif < gif_files.Length; iterated_gif++)
    {
        char[] delimiterChars = { '_' };
        List<string> gif_title = gif_files[iterated_gif].Split(delimiterChars).ToList();

        artwork_id = gif_title[0];
        gif_title.RemoveAt(0);

        artwork_name = String_List_To_String(gif_title);

        string new_filename = $@"{artwork_id}_p0 {artwork_name}";

        Console.WriteLine($"Transferring {new_filename}...");

        System.IO.File.Copy($@"{current_artist_subdirectory_path}\{gif_files[iterated_gif]}", $@"{new_hitomi_artist_directory}\{new_filename}");
    }
}

Console.WriteLine($"Transfer complete!\n");

string String_List_To_String(List<string> input_list)
{
    // Create an empty string variable.
    string output_string = "";

    // Iterate through each index of the list and add it to the string variable.
    for (int i = 0; i < input_list.Count; i++)
    {
        if (i == input_list.Count - 1)
        {
            output_string += input_list[i];
        }
        else
        {
            output_string += input_list[i] + " ";
        }
    }

    // Return the string variable.
    return output_string;
}