using People;

namespace Builder;

class Build
{

    public static List<Person> Society()
    {
        return Society(new List<Person> { });
    }

    public static List<Person> Society(List<Person> society)
    {
        string? readresult;
        if (society.Count > 0)
        {
            Console.Clear();
            Console.WriteLine($"There are already {society.Count} people in our society");
            Console.WriteLine("Would you like to list them? (Y/N)");
            readresult = YNLoop();
            if (readresult == "y")
            {
                Population.DisplaySocietyNames(society);
            }
        }
        do
        {
            string name = "";
            Console.WriteLine("Enter a name");
            while (name == "")
            {
                readresult = Console.ReadLine();
                if (readresult != null && readresult != "")
                    name = readresult;
                else
                    Console.WriteLine("Name is required. Please enter a name");
            }

            Console.WriteLine("Enter an age");
            int age = NumberLoop(0, int.MaxValue);

            Person person = new Person(name, age);
            Console.WriteLine("Enter their occupation (leave blank for none)");
            do
            {
                readresult = Console.ReadLine();
                if (readresult != null)
                    person.Occupation = readresult;
            } while (readresult == null);
            Console.WriteLine("Would you like to add some of their interests? (Y/N)");
            readresult = YNLoop();
            if (readresult == "y")
            {
                do
                {
                    Console.WriteLine("Enter an interest (Leave blank to stop adding interests)");
                    readresult = Console.ReadLine();
                    if (readresult != null && readresult != "")
                        person.AddLike(readresult);
                } while (readresult != "");
            }
            Console.WriteLine("Would you like to add some of their dislikes? (Y/N)");
            readresult = YNLoop();
            if (readresult == "y")
            {
                do
                {
                    Console.WriteLine("Enter a dislike (Leave blank to stop adding interests)");
                    readresult = Console.ReadLine();
                    if (readresult != null && readresult != "")
                        person.AddDislike(readresult);
                } while (readresult != "");
            }
            society.Add(person);
            Console.WriteLine("Would you like to add another person? (Y/N)");
            readresult = YNLoop();
        } while (readresult != "n");
        return society;
    }

    public static void Relationships(List<Person> society)
    {
        string? readresult;
        int selection;
        int relationNum;
        do
        {
            Console.Clear();
            Console.WriteLine("Available people and their relationships");
            for (int i = 0; i < society.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {society[i].Name}");
                Console.WriteLine(society[i].DisplayRelationships());
            }
            Console.WriteLine("Enter the number of the person you would like to add relationships to");
            selection = NumberLoop(1, society.Count) - 1;
            do
            {
                Console.Clear();
                for (int i = 0; i < society.Count; i++)
                {
                    if (i != selection)
                        Console.WriteLine($"{i + 1}: {society[i].Name}");
                }
                Console.WriteLine($"Who does {society[selection].Name} have a relationship with? (Choose their number)");
                do
                {
                    relationNum = NumberLoop(1, society.Count) - 1;
                    if (relationNum == selection)
                        Console.WriteLine("Can't form a relationship with self.");
                } while (selection == relationNum);
                Console.WriteLine("What is their relation? (Leave blank to cancel)");
                do
                {
                    readresult = Console.ReadLine();
                    if (readresult != null && readresult != "")
                    {
                        society[selection].AddRelationship(readresult, society[relationNum]);
                    }
                } while (readresult == null);
                Console.WriteLine("Would you like to add another relationship? (Y/N)");
                readresult = YNLoop();
            } while (readresult != "n");
            Console.WriteLine("Would you like to add relationships to someone else? (Y/N)");
            readresult = YNLoop();
        } while (readresult != "n");
    }

    public static int PersonSelection(List<Person> society)
    {
        int person_selected;
        Console.Clear();
        Console.WriteLine("Available People in the Society: ");
        for (int i = 0; i < society.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {society[i].Name}");
        }
        Console.WriteLine($"{society.Count + 1}: NO ONE");
        Console.WriteLine("Enter the number of the person you want to select.");
        person_selected = NumberLoop(1, society.Count + 1) - 1;
        if (person_selected == society.Count)
            person_selected = -1;
        return person_selected;
    }

    public static string GenericInputLoop()
    {
        string? readresult;
        do
        {
            readresult = Console.ReadLine();
        } while (readresult == null);
        return readresult;
    }

    public static string YNLoop(string y = "y", string n = "n", bool escape_allowed = false)
    {
        string? readresult;
        do
        {
            readresult = Console.ReadLine();
            if (readresult != null)
                readresult = readresult.ToLower();
            if (readresult != y && readresult != n && !(escape_allowed && readresult == ""))
                Console.WriteLine($"Invalid Selection, Try again, ({y.ToUpper()}/{n.ToUpper()}{(escape_allowed ? "/leave blank to escape" : "")}):");
        } while (readresult != y && readresult != n && !(escape_allowed && readresult == ""));
        return readresult;
    }

    public static int NumberLoop(int low = 0, int high = int.MaxValue)
    {
        string? readresult;
        bool success;
        int selection = low;
        do
        {
            readresult = Console.ReadLine();
            success = false;
            if (readresult != null)
            {
                readresult = readresult.Trim().ToLower();
                if (int.TryParse(readresult, out selection))
                    success = selection >= low && selection <= high;
            }
            if (!success)
            {
                Console.WriteLine("Invalid Selection, Try again");
            }
        } while (!success || readresult == null);
        return selection;
    }
}