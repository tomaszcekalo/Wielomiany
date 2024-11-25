using System;
using System.IO;
using System.Numerics;


// funkcje

static List<List<Double>> openFile(string path)
{
    List<List<Double>> wielomiany = new List<List<Double>>();
    try
    {
        using StreamReader reader = new StreamReader(path);

        while (reader.EndOfStream == false)
        {
            string text = reader.ReadLine();
            string[] spl = text.Split(" ");

            List<Double> row = new List<Double>();

            foreach (string s in spl)
            {
                row.Add(Double.Parse(s));
            }
            wielomiany.Add(row);
        }
        Console.WriteLine("-- załadowano wielomiany pomyślnie");
        return wielomiany;
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
        return null;
    }
}

static void writePolynomial(List<List<Double>> wielomiany)
{
    Console.WriteLine("Wielomiany:");
    foreach (List<Double> row in wielomiany)
    {
        foreach (Double d in row)
        {
            Console.Write(" " + d);
        }
        Console.WriteLine();
    }
}

static void writePopulation(List<List<Double>> populacja)
{
    foreach (List<Double> item in populacja)
    {
        foreach (Double val in item)
        {
            Console.Write(val.ToString("0.00") + " ");
        }
        Console.WriteLine();
    }
}

static void writeDopasowanie(List<Double> dopasowanie)
{
    foreach (Double item in dopasowanie)
    {
        Console.Write(item.ToString("0.00") + " ");
    }
    Console.WriteLine();
}

static List<List<Double>> createZeroPopulation(int S, int N)
{
    Random rnd = new Random();
    List<List<Double>> populacjaZero = new List<List<Double>>();
    int i = 0, j = 0;
    while (i < N)
    {
        List<Double> row = new List<Double>();
        j = 0;
        while (j < S)
        {
            row.Add(rnd.NextDouble() * 2 - 1);
            j++;
        }
        populacjaZero.Add(row);
        i++;
    }
    return populacjaZero;
}

/*
static List<List<Double>> generatePopulation(List<Double> staraPopulacja, List<List<Double>> wielomiany)
{
    Random rnd = new Random();
    List<List<Double>> populacjaNowa = new List<List<Double>>();
    return populacjaNowa;
}*/

static List<Double> dopasowanie(List<List<Double>> populacja, List<List<Double>> wielomiany)
{
    List<Double> dopasowanie = new List<Double>();

    int len = wielomiany.Count();
    int i, j;

    foreach (List<Double> osobnik in populacja)
    {
        Double wartosc = new Double();
        
        wartosc = 0;
        i = 0;
        while(i < len)// pojedynczy wielomian
        {
            Double suma = new Double();
            suma = 0;
            j = 0;    
            while (j+1 < len) // wyliczanie wielomianu
            {
                suma = (suma + wielomiany[i][j]) * osobnik[i];
                j++;
            }
            suma += wielomiany[i][j];
            wartosc += suma;
            i++;
        }
        dopasowanie.Add(wartosc);
    }
    return dopasowanie;
}

// START
int max_k = 10; // number of iterations
int S = 3; //number of polynomials
int k = 0; // algorithm iteration
int i = 0; // iterator
int N = 20; // number of individuals
List<List<List<Double>>> populacje = new List<List<List<Double>>>(); // k N S
List<List<Double>> wielomiany = openFile("C:\\Users\\smate\\Documents\\TestFile.txt");
writePolynomial(wielomiany);
List<List<Double>> populacjaZero = createZeroPopulation(S, N);
populacje.Add(populacjaZero);
Console.WriteLine("Populacja 0:");
writePopulation(populacje[0]);
Console.WriteLine();
List<List<Double>> dopasowania = new List<List<double>>();
dopasowania.Add(dopasowanie(populacje[k], wielomiany));
writeDopasowanie(dopasowania[k]);
k = 1;

// end of initialization

//krok 3

// krok 4 //dopasowania.Add(dopasowanie(populacje[k], wielomiany));

// krok 5

// krok 6 //k=k+1

// krok 7 (k<max_k) kontynuuj petle od 3 kroku


//populacje.Add(generatePopulation(populacje[k-1], wielomiany));




return;