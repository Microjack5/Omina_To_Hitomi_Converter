string omina_main_directory = $@"G:\Storage\Archives\Pixiv Archives";
string hitomi_main_directory = $@"G:\Storage\Macrojack VR Build Programs\hitomi_downloader_GUI\hitomi_downloaded_pixiv";

if (!Directory.Exists(omina_main_directory))
{
    Console.WriteLine("Omina directory does not exist. Please check your input and try again.\n");
    return;
}

if (!Directory.Exists(hitomi_main_directory))
{
    Console.WriteLine("Hitomi directory does not exist. Please check your input and try again.\n");
    return;
}

Console.WriteLine("Counting Omina subfolders...\n");

int omina_subfolder_count = Directory.GetDirectories(omina_main_directory).Length;

Console.WriteLine($"Omina subfolder directory count is {omina_subfolder_count}. Continue? (Y/N)\n");

bool input_validation = false;

while (input_validation == false)
{
    var user_input = Console.ReadLine();

    if (user_input != null)
    {
        user_input = user_input.ToLower();

        if (user_input == "n" || user_input == "no")
        {
            return;
        }
        else if (user_input == "y" || user_input == "yes")
        {
            input_validation = true;
        }
    }
}

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
string[] omina_subfolder_paths = Directory.GetDirectories(omina_main_directory);
string[] omina_subfolder_names = Directory.GetDirectories(omina_main_directory).Select(Path.GetFileName).ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

string artist_name = "";
string post_id = "";
string post_title = "";

for (int i = 0; i < omina_subfolder_count; i++)
{
    artist_name = omina_subfolder_names[i];

    Console.WriteLine($"({i + 1} / {omina_subfolder_count}) Current folder: {artist_name}\n");

    string new_hitomi_artist_directory = $@"{hitomi_main_directory}\{artist_name}";

    if (!Directory.Exists(new_hitomi_artist_directory))
    {
        Directory.CreateDirectory(new_hitomi_artist_directory);

        #pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        string current_omina_subfolder_path = omina_subfolder_paths[i];
        string[] post_paths = Directory.GetDirectories(current_omina_subfolder_path);
        string[] post_titles = Directory.GetDirectories(current_omina_subfolder_path).Select(Path.GetFileName).ToArray();
        #pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

        for (int j = 0; j < post_titles.Length; j++)
        {
            char[] delimiterChars = { '_' };
            List<string> current_post_title = post_titles[j].Split(delimiterChars).ToList();

            post_id = current_post_title[0];
            current_post_title.RemoveAt(0);

            post_title = String_List_To_String(current_post_title);

            string[] post_image_paths = Directory.GetFiles(post_paths[j]);

            for (int image_number = 0; image_number < post_image_paths.Length; image_number++)
            {
                string image_extention = Path.GetExtension($@"{post_paths[j]}\{post_image_paths[image_number]}");
                string new_filename = $@"{post_id}_p{image_number} {post_title}{image_extention}";

                Console.WriteLine($"Transferring {new_filename}...");

                System.IO.File.Copy($@"{post_image_paths[image_number]}", $@"{new_hitomi_artist_directory}\{new_filename}");
            }
        }

        // GIF Transfer
        #pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        string[] gif_files = Directory.GetFiles($@"{current_omina_subfolder_path}", "*.gif").Select(Path.GetFileName).ToArray();
        #pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

        for (int iterated_gif = 0; iterated_gif < gif_files.Length; iterated_gif++)
        {
            char[] delimiterChars = { '_' };
            List<string> gif_title = gif_files[iterated_gif].Split(delimiterChars).ToList();

            post_id = gif_title[0];
            gif_title.RemoveAt(0);

            post_title = String_List_To_String(gif_title);

            string new_filename = $@"{post_id}_p0 {post_title}";

            Console.WriteLine($"Transferring {new_filename}...");

            try
            {
                System.IO.File.Copy($@"{current_omina_subfolder_path}\{gif_files[iterated_gif]}", $@"{new_hitomi_artist_directory}\{new_filename}");
            }
            catch (IOException)
            {
                Console.WriteLine("File already exists");
            }
        }

        Console.WriteLine("\n");
    }
}

Console.WriteLine($"Transfer complete!\n");

string String_List_To_String(List<string> input_list)
{
    string output_string = "";

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

    return output_string;
}