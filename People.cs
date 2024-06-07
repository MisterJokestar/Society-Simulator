namespace People;

public class Population
{
    public static string DisplaySocietyNames(List<Person> society)
    {
        string result = "";
        for (int i = 0; i < society.Count; i++)
        {
            result += $"\n {i + 1}.\t{society[i].Name}";
        }
        return result;
    }

    public static string DisplaySocietyPeople(List<Person> society)
    {
        string result = "";
        for (int i = 0; i < society.Count; i++)
        {
            result += $"\n {i + 1}.\t{society[i]}";
        }
        return result;
    }

    public static string DisplaySocietyRelationships(List<Person> society)
    {
        string result = "";
        for (int i = 0; i < society.Count; i++)
        {
            result += $"\n {society[i].DisplayRelationships()}";
        }
        return result;
    }

    public static string DisplaySocietyAll(List<Person> society)
    {
        string result = "";
        for (int i = 0; i < society.Count; i++)
        {
            result += $"\n {i + 1}\t{society[i]}\nRelationships:\n{society[i].DisplayRelationships()}";
        }
        return result;
    }
    public static List<Person> ParsePeople(string filepath)
    {
        if (!File.Exists(filepath))
        {
            throw new FileNotFoundException();
        }
        List<Person> people = new List<Person> { };
        int count = 0;
        List<string> relationships = new List<string> { };
        using (StreamReader sr = File.OpenText(filepath))
        {
            string? line;
            while ((line = sr.ReadLine()) != null && line != "")
            {
                string name = "";
                string age = "";
                string occupation = "";
                string likes = "";
                string dislikes = "";
                for (int i = 0; i < 5; i++)
                {
                    int index = line.IndexOf(";");
                    if (index == -1)
                        throw new FormatException("ERROR: File is not in correct format. There should be 5 fields delimited by a ';'");
                    string field = line[..index];
                    line = line.Remove(0, index + 1);
                    switch (i)
                    {
                        case 0:
                            name = field;
                            break;
                        case 1:
                            age = field;
                            break;
                        case 2:
                            occupation = field;
                            break;
                        case 3:
                            likes = field;
                            break;
                        case 4:
                            dislikes = field;
                            break;
                    }
                }
                int parsedAge;
                bool success = int.TryParse(age, out parsedAge);
                if (!success)
                    throw new FormatException("ERROR: File is not in correct format. Age must be numeric.");
                people.Add(new Person(name, parsedAge));
                if (occupation != "NA")
                    people[count].Occupation = occupation;
                if (likes != "NA")
                {
                    while (likes != "")
                    {
                        int index = likes.IndexOf(",");
                        string like;
                        if (index == -1)
                        {
                            like = likes;
                            likes = "";
                        }
                        else
                        {
                            like = likes[..index];
                            likes = likes.Remove(0, index + 1);
                        }
                        people[count].AddLike(like);
                    }
                }
                if (dislikes != "NA")
                {
                    while (dislikes != "")
                    {
                        int index = dislikes.IndexOf(",");
                        string dislike;
                        if (index == -1)
                        {
                            dislike = dislikes;
                            dislikes = "";
                        }
                        else
                        {
                            dislike = dislikes[..index];
                            dislikes = dislikes.Remove(0, index + 1);
                        }
                        people[count].AddDislike(dislike);
                    }
                }
                relationships.Add(line);
                count++;
            }
        }
        for (int i = 0; i < count; i++)
        {
            string s = relationships[i];
            int index;
            bool success;
            int relationNum;
            while (s != "")
            {
                index = s.IndexOf("-");
                success = int.TryParse(s[..index], out relationNum);
                if (!success)
                    throw new FormatException("ERROR: File is not in correct format. Relationships in incorect format <num>-<relationship>");
                s = s.Remove(0, index + 1);
                index = s.IndexOf(",");
                if (index != -1)
                {
                    people[i].AddRelationship(s[..index], people[relationNum]);
                    s = s.Remove(0, index + 1);
                }
                else
                {
                    people[i].AddRelationship(s, people[relationNum]);
                    s = "";
                }
            }
        }
        return people;
    }

    public static void WritePeople(string filepath, List<Person> people)
    {
        using (StreamWriter sw = File.CreateText(filepath))
        {
            foreach (Person person in people)
            {
                string line = $"{person.Name};{person.Age};{((person.Occupation == "") ? "NA" : person.Occupation)};";
                if (person.Likes.Count > 0)
                {
                    foreach (string like in person.Likes)
                    {
                        line += $"{like},";
                    }
                    line = $"{line[..^1]};";
                }
                else
                {
                    line += "NA;";
                }
                if (person.Dislikes.Count > 0)
                {
                    foreach (string dislike in person.Dislikes)
                    {
                        line += $"{dislike},";
                    }
                    line = $"{line[..^1]};";
                }
                else
                {
                    line += "NA;";
                }
                bool added = false;
                for (int i = 0; i < people.Count; i++)
                {
                    string? relationship = person.GetRelationship(people[i]);
                    if (relationship != null && relationship != "self")
                    {
                        line += $"{i}-{relationship},";
                        added = true;
                    }
                    if (i + 1 == people.Count && added)
                        line = line[..^1];
                }
                sw.WriteLine(line);
            }
        }
    }
}

public class Person
{
    public Person(string name, int age = 0)
    {
        this.Name = name;
        this.Age = age;
        this.Occupation = "";
        this.Likes = new List<String> { };
        this.Dislikes = new List<string> { };
        relationships.Add(this, "self");
    }

    public string Name
    { get; set; }
    public int Age
    { get; set; }
    public string Occupation
    { get; set; }
    public List<string> Likes
    { get; }
    public List<string> Dislikes
    { get; }
    private readonly Dictionary<Person, string> relationships = new();

    public void AddLike(string like)
    {
        this.Likes.Add(like);
    }

    public bool RemoveLike(string like)
    {
        return this.Likes.Remove(like);
    }

    public string PrintLikes()
    {
        string result = $"{this.Name} likes";
        foreach (string like in this.Likes)
        {
            result += $", {like}";
        }
        return result;
    }

    public void AddDislike(string dislike)
    {
        this.Dislikes.Add(dislike);
    }

    public bool RemoveDislike(string dislike)
    {
        return this.Dislikes.Remove(dislike);
    }

    public string PrintDislikes()
    {
        string result = $"{this.Name} dislikes";
        foreach (string dislike in this.Dislikes)
        {
            result += $", {dislike}";
        }
        return result;
    }

    public bool AddRelationship(string relation, Person person)
    {
        bool exists = false;
        foreach (KeyValuePair<Person, string> r in relationships)
        {
            if (r.Key.Equals(person))
                exists = true;
        }
        if (!exists)
        {
            this.relationships.Add(person, relation);
        }
        return !exists;
    }

    public bool RemoveRelationship(Person person)
    {
        if (person == this)
            return false;
        bool result = relationships.Remove(person);
        return result;
    }

    public string? GetRelationship(Person person)
    {
        try
        {
            return relationships[person];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    public string DisplayRelationships()
    {
        string result = "";
        foreach (KeyValuePair<Person, string> r in relationships)
        {
            if (r.Value != "self")
            {
                result += $"{r.Key.Name} is the {r.Value} of {this.Name}.\n";
            }
        }
        if (result == "")
            result = $"{this.Name} has no relationships.\n";
        return result;
    }

    public override string ToString()
    {
        string result = $"{this.Name} is {this.Age} years old.\n";
        if (Occupation != "")
        {
            result += $"They work as a(n) {this.Occupation}.\n";
        }
        if (Likes.Count() > 0)
            result += $"{this.PrintLikes()}\n";
        if (Dislikes.Count() > 0)
            result += $"{this.PrintDislikes()}\n";
        return result;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        bool equal = obj == this;
        if (!equal && obj is Person temp)
        {
            equal = temp.Name == this.Name &&
                    temp.Age == this.Age &&
                    temp.Occupation == this.Occupation &&
                    temp.Likes.Equals(this.Likes) &&
                    temp.Dislikes.Equals(this.Dislikes);
        }
        return equal;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Age, Occupation, Likes, Dislikes);
    }
}