using System.Text.RegularExpressions;

using People;
using Builder;

main();

static void main()
{
    string? readresult;
    string path = FindSociety(false, true);
    List<Person> society = Population.ParsePeople(path);

    if (society.Count() == 0)
    {
        Console.WriteLine("Your society is currently empty, would you like to start building it? (Y/N)");
        readresult = Build.YNLoop();
        if (readresult == "y")
            society = Build.Society(society);
        else
            return;
    }

    MenuLoop(society, path);

    Console.Clear();
    Console.WriteLine("Thank you for using Jokestars Society tool.\nGoodbye!");
}

static string FindSociety(bool overwrite, bool doesntexistswarn)
{
    string? readresult;
    bool success = false;
    string path = "";
    Console.Clear();
    Console.WriteLine("Enter name of your society.");
    do
    {
        readresult = Console.ReadLine();
        Regex fileformat = new Regex(@"^[a-zA-Z]{3}[\-0-9a-zA-Z]*$");
        if (readresult != null)
        {
            readresult = readresult.Trim();
            if (readresult.Length > 2 && fileformat.IsMatch(readresult))
            {
                path = $".\\societies\\{readresult}.people";
                if (doesntexistswarn && !File.Exists(path))
                {
                    Console.WriteLine("Society doesn't exist yet, make new society? (Y/N)");
                    readresult = Build.YNLoop();
                    if (readresult == "n")
                        continue;
                    FileStream file = File.Create(path);
                    file.Close();
                }
                if (overwrite && File.Exists(path))
                {
                    readresult = Confirmation("This society already exists, you are about to replace it.");
                    if (readresult == "n")
                        continue;
                    FileStream file = File.Create(path);
                    file.Close();
                }
                success = true;
            }
            else
            {
                Console.WriteLine("Society must have at start with 3 alphebetic characters.\nSocietys may contain alphebetic, and numeric characters and the '-' symbol.");
            }
        }
    } while (!success);

    return path;
}

static void MenuLoop(List<Person> society, string path)
{
    int selection;
    do
    {
        Console.Clear();
        Console.WriteLine("Choose from the menu below:");
        Console.WriteLine(" 1. Display Society");
        Console.WriteLine(" 2. Add to society");
        Console.WriteLine(" 3. Edit Society");
        Console.WriteLine(" 4. Save Society");
        Console.WriteLine(" 5. Save as new Society");
        Console.WriteLine(" 6. Destroy Society");
        Console.WriteLine(" 7. Exit");
        Console.WriteLine("\nEnter the number for your selection.");
        selection = Build.NumberLoop(1, 7);
        switch (selection)
        {
            case 1:
                SubmenuDisplayLoop(society);
                break;
            case 2:
                SubmenuAddLoop(society);
                break;
            case 3:
                SubmenuEditLoop(society);
                break;
            case 4:
                Population.WritePeople(path, society);
                Console.WriteLine($"Society has been saved to {path}");
                Pause();
                break;
            case 5:
                path = FindSociety(true, false);
                Population.WritePeople(path, society);
                Console.WriteLine($"Society has been saved to {path}");
                Pause();
                break;
            case 6:
                string confirm = Confirmation("This will remove EVERYONE from this society!");
                if (confirm == "y")
                {
                    society = new();
                    Console.WriteLine("Society has been destroyed. Change has not been saved.");
                }
                Pause();
                break;
            default:
                continue;
        }
    } while (selection != 7);
}

static void SubmenuDisplayLoop(List<Person> society)
{
    int selection;
    do
    {
        Console.Clear();
        Console.WriteLine("Choose from the menu below:");
        Console.WriteLine(" 1. Display Names");
        Console.WriteLine(" 2. Display Names, Ages, Occupation, Likes and Dislikes");
        Console.WriteLine(" 3. Display Relationships");
        Console.WriteLine(" 4. Display All");
        Console.WriteLine(" 5. Exit");
        Console.WriteLine("\nEnter the number for your selection.");
        selection = Build.NumberLoop(1, 5);
        switch (selection)
        {
            case 1:
                Console.WriteLine(Population.DisplaySocietyNames(society));
                Pause();
                break;
            case 2:
                Console.WriteLine(Population.DisplaySocietyPeople(society));
                Pause();
                break;
            case 3:
                Console.WriteLine(Population.DisplaySocietyRelationships(society));
                Pause();
                break;
            case 4:
                Console.WriteLine(Population.DisplaySocietyAll(society));
                Pause();
                break;
            default:
                continue;
        }
    } while (selection != 5);

}

static void SubmenuAddLoop(List<Person> society)
{
    int selection;
    do
    {
        Console.Clear();
        Console.WriteLine("Choose from the menu below:");
        Console.WriteLine(" 1. Add People");
        Console.WriteLine(" 2. Add Relationships");
        Console.WriteLine(" 3. Exit");
        Console.WriteLine("\nEnter the number for your selection.");
        selection = Build.NumberLoop(1, 3);
        switch (selection)
        {
            case 1:
                Build.Society(society);
                break;
            case 2:
                Build.Relationships(society);
                break;
            default:
                continue;
        }
    } while (selection != 3);

}

static void SubmenuEditLoop(List<Person> society)
{
    int person_selected = Build.PersonSelection(society);
    int selection;

    if (person_selected != -1)
    {
        do
        {
            Console.Clear();
            Console.WriteLine("What do you want to change?");
            Console.WriteLine(" 1. Name");
            Console.WriteLine(" 2. Age");
            Console.WriteLine(" 3. Occupation");
            Console.WriteLine(" 4. Likes");
            Console.WriteLine(" 5. Dislikes");
            Console.WriteLine(" 6. Choose someone else to edit");
            Console.WriteLine(" 7. Exit");
            Console.WriteLine("\nEnter the number for your selection.");
            selection = Build.NumberLoop(1, 7);

            switch (selection)
            {
                case 1:
                    EditName(society, person_selected);
                    break;
                case 2:
                    EditAge(society, person_selected);
                    break;
                case 3:
                    EditOccupation(society, person_selected);
                    break;
                case 4:
                    EditLikesOrDislikes(society, person_selected, true);
                    break;
                case 5:
                    EditLikesOrDislikes(society, person_selected, false);
                    break;
                case 6:
                    person_selected = Build.PersonSelection(society);
                    continue;
                default:
                    continue;
            }
            Pause();
        } while (selection != 7);
    }
}

static void EditName(List<Person> society, int person_selected)
{
    Console.WriteLine("Enter a Name:");
    string newName = Build.GenericInputLoop();
    if (newName == "")
        Console.WriteLine("Invalid Input: No name found");
    else
    {
        society[person_selected].Name = newName;
        Console.WriteLine("Name Changed.");
    }
}

static void EditAge(List<Person> society, int person_selected)
{
    Console.WriteLine("Enter a Age:");
    int newAge = Build.NumberLoop();
    if (newAge < society[person_selected].Age)
        Console.WriteLine("Invalid Input: Age can't be younger then current age.");
    else
    {
        society[person_selected].Age = newAge;
        Console.WriteLine("Age Changed.");
    }
}

static void EditOccupation(List<Person> society, int person_selected)
{
    Console.WriteLine("Enter an Occupation: (Leave blank to set to None)");
    string newOccupation = Build.GenericInputLoop();
    society[person_selected].Occupation = newOccupation;
    Console.WriteLine("Occupation Changed.");
}

static void EditLikesOrDislikes(List<Person> society, int person_selected, bool likes_selected)
{
    string input;
    do
    {
        Console.Clear();
        if (likes_selected)
            Console.WriteLine(society[person_selected].PrintLikes());
        else
            Console.WriteLine(society[person_selected].PrintDislikes());
        Console.WriteLine("Do you want to (A)dd or (R)emove? (Leave blank for none)");
        string selection = Build.YNLoop("a", "r", true);

        if (selection != "")
        {
            Console.WriteLine($"Enter the {(likes_selected ? "like" : "dislike")} to {(selection == "a" ? "add" : "remove")}");
            input = Build.GenericInputLoop();
            if (input == "")
                Console.WriteLine($"Invalid Input: Can't {(selection == "a" ? "add" : "remove")} blank input");
            else
            {
                switch (selection)
                {
                    case "a" when likes_selected: // Adding Likes
                        society[person_selected].AddLike(input);
                        break;
                    case "a" when !likes_selected: // Adding Dislikes
                        society[person_selected].AddDislike(input);
                        break;
                    case "r" when likes_selected: // Removing Likes
                        if (!society[person_selected].RemoveLike(input))
                            Console.WriteLine("Invalid Input: Couldn't find input to remove.");
                        break;
                    case "r" when !likes_selected: // Removing Dislikes
                        if (!society[person_selected].RemoveDislike(input))
                            Console.WriteLine("Invalid Input: Couldn't find input to remove.");
                        break;
                }
            }
        }
        Console.WriteLine($"Do you want to keep editing {(likes_selected ? "likes" : "dislikes")}? (Y/N)");
        input = Build.YNLoop();
    } while (input != "n");
}

static string Confirmation(string message)
{
    Console.WriteLine($"{message}\n Are you sure you want to do this? (Y/N)");
    return Build.YNLoop();
}

static void Pause()
{
    Console.WriteLine("\n Press enter to continue.");
    Console.ReadLine();
}